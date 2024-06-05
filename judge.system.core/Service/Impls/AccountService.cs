using AutoMapper;
using EmailService;
using judge.system.core.Database;
using judge.system.core.DTOs.Requests.Account;
using judge.system.core.DTOs.Responses;
using judge.system.core.DTOs.Responses.Account;
using judge.system.core.Helper;
using judge.system.core.Helper.Converter;
using judge.system.core.Models;
using judge.system.core.Service.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace judge.system.core.Service.Impls
{
    public class AccountService : IAccountService
    {
        private readonly Context _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IEmailSender _emailSender;

        public AccountService(Context context, IMapper mapper, IConfiguration configuration, IEmailSender emailSender)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
            _emailSender = emailSender;
        }
        public async Task<APIResponse<string>> Create(CreateAccountReq req)
        {
            try
            {
                var users = ReadAll();
                foreach (var user in users.Result.Data)
                {
                    if (user.UserName == req.UserName)
                    {
                        return new APIResponse<string>
                        {
                            StatusCode = 200,
                            Message = "User name already exists"
                        };
                    }
                }
                var passwordHash = BCrypt.Net.BCrypt.HashPassword(req.Password);
                var newAcc = _mapper.Map<Account>(req);
                newAcc.Password = passwordHash;
                _context.Accounts.Add(newAcc);

                await _context.SaveChangesAsync();
                return new APIResponse<string>
                {
                    StatusCode = 200,
                    Message = "Success"
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<string>
                {
                    StatusCode = 500,
                    Message = ex.Message,
                };
            }
        }

        public async Task<APIResponse<LoginRes>> Login(LoginReq req)
        {
            try
            {
                var user = _context.Accounts.FirstOrDefault(x => x.UserName == req.UserName);
                if (user is null)
                {
                    return new APIResponse<LoginRes>
                    {
                        StatusCode = 200,
                        Message = $"User {req.UserName} does not exists",
                    };
                }
                bool isValidPassword = BCrypt.Net.BCrypt.Verify(req.Password, user.Password);
                var token = await TokenManager.GenerateToken(user, _configuration);

                if (!isValidPassword)
                {
                    return new APIResponse<LoginRes>
                    {
                        StatusCode = 401,
                        Message = $"Password is wrong",
                    };
                }
                return new APIResponse<LoginRes>
                {
                    StatusCode = 200,
                    Message = $"Success",
                    Data = token
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<LoginRes>
                {
                    StatusCode = 500,
                    Message = ex.Message,
                };
            }
        }

        public async Task<APIResponse<List<ReadAccountsRes>>> ReadAll()
        {
            try
            {
                var accounts = _context.Accounts.ToList();
                List<ReadAccountsRes> res = new List<ReadAccountsRes>();

                foreach (var account in accounts)
                {
                    ReadAccountsRes item = new ReadAccountsRes();
                    item.UserName = account.UserName;
                    item.FullName = account.FullName;
                    item.Email = account.Email;
                    res.Add(item);
                }

                return new APIResponse<List<ReadAccountsRes>>
                {
                    StatusCode = 200,
                    Message = "Success",
                    Data = res
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<List<ReadAccountsRes>> { StatusCode = 500, Message = ex.Message };
            }
        }

        public async Task<APIResponse<string>> Update(UpdateAccountReq req, string userName)
        {
            try
            {
                var acc = _context.Accounts.FirstOrDefault(x => x.UserName == userName);

                if (acc == null)
                {
                    return new APIResponse<string>
                    {
                        StatusCode = 400,
                        Message = $"User {userName} not found"
                    };
                }
                acc.FullName = req.FullName;
                _context.Accounts.Update(acc);
                await _context.SaveChangesAsync();
                return new APIResponse<string>
                {
                    StatusCode = 200,
                    Message = "Success",
                    Data = acc.UserName
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<string>
                {
                    StatusCode = 500,
                    Message = ex.Message,

                };
            }
        }

        public async Task<APIResponse<LoginRes>> RefreshToken(TokenReq tokenRequest)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var secretKey = _configuration.GetValue<string>("AppSettings:SecretKey");
            var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);
            var param = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes),
                ClockSkew = TimeSpan.Zero,
                ValidateLifetime = false // do not check token expired
            };
            try
            {
                // check format token
                var tokenValid = jwtTokenHandler.ValidateToken(tokenRequest.AccessToken, param, out var validatedToken);
                // check algorithm encode token
                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512, StringComparison.InvariantCultureIgnoreCase);

                    if (!result)
                    {
                        return new APIResponse<LoginRes>
                        {
                            StatusCode = 400,
                            Message = "Invalid token"
                        };
                    }
                }

                //check token expired?
                var utcExpireDate = long.Parse(tokenValid.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);
                var expireDate = Converter.ConvertToDateTime(utcExpireDate);
                if (expireDate > DateTime.UtcNow)
                {
                    return new APIResponse<LoginRes>
                    {
                        StatusCode = 400,
                        Message = "Access token has not expired yet."
                    };
                }

                var storedToken = await _context.RefreshTokens.FirstOrDefaultAsync(x => x.Token == tokenRequest.RefreshToken);

                if (storedToken is null)
                {
                    return new APIResponse<LoginRes>
                    {
                        StatusCode = 400,
                        Message = "Refresh token does not exist."
                    };
                }
                if (storedToken.IsUsed)
                {
                    return new APIResponse<LoginRes>
                    {
                        StatusCode = 400,
                        Message = "Refresh token has been used."
                    };

                }
                if (storedToken.IsRevoked)
                {

                    return new APIResponse<LoginRes>
                    {
                        StatusCode = 400,
                        Message = "Refresh token has been revoked."
                    };
                }
                // check access token id match jwt id in DB
                var jti = tokenValid.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
                if (storedToken.JwtId != jti)
                {
                    return new APIResponse<LoginRes>
                    {
                        StatusCode = 400,
                        Message = "Token does not match."
                    };
                }




                // Update token  is used
                storedToken.IsRevoked = true;
                storedToken.IsUsed = true;
                _context.RefreshTokens.Update(storedToken);
                await _context.SaveChangesAsync();

                var currentUser = await _context.Accounts.FirstOrDefaultAsync(x => x.Id == storedToken.UserId);
                var token = await TokenManager.GenerateToken(currentUser, _configuration);
                //Response.Cookies.Append("access", token.AccessToken, new CookieOptions //Save the access token in the browser cookies, Key is "access"
                //{
                //    HttpOnly = true,
                //    SameSite = SameSiteMode.None,
                //    Secure = true
                //});
                //Response.Cookies.Append("refresh", token.RefreshToken, new CookieOptions //Save the refresh token in the browser cookies, Key is "refresh"
                //{
                //    HttpOnly = true,
                //    SameSite = SameSiteMode.None,
                //    Secure = true
                //});

                return new APIResponse<LoginRes>
                {
                    StatusCode = 200,
                    Message = "Refresh token successful.",
                    Data = token
                };

            }
            catch (Exception ex)
            {
                return new APIResponse<LoginRes>
                {
                    StatusCode = 500,
                    Message = ex.Message

                };
            }
        }

        public async Task<APIResponse<string>> ResetPassword(ResetPasswordReq resetPassword)
        {
            try
            {

                var handler = new JwtSecurityTokenHandler();
                var jwtSecurityToken = handler.ReadToken(resetPassword.Token);
                var jsonToken = jwtSecurityToken as JwtSecurityToken;
                string userInToken = jsonToken.Claims.FirstOrDefault(x => x.Type == "nameid")?.Value;
                if (userInToken == null)
                {
                    return new APIResponse<string>
                    {
                        StatusCode = 400,
                        Message = "Access token invalid."
                    };
                }
                var passwordHash = BCrypt.Net.BCrypt.HashPassword(resetPassword.Password);
                var updatedPasswordUser = await _context.Accounts.FirstOrDefaultAsync(x => x.UserName == userInToken);
                updatedPasswordUser.Password = passwordHash;
                _context.Accounts.Update(updatedPasswordUser);
                await _context.SaveChangesAsync();

                return new APIResponse<string>
                {
                    StatusCode = 200,
                    Data = userInToken
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<string>
                {
                    StatusCode = 500,
                    Message = ex.Message,
                };
            }
        }

        public async Task<APIResponse<string>> ForgotPassword(string email)
        {
            try
            {
                var user = await _context.Accounts.FirstOrDefaultAsync(x => x.Email == email);

                if (user == null)
                {

                    return new APIResponse<string>
                    {
                        StatusCode = 404,
                        Message = "Email not found."
                    };
                }
                var token = await TokenManager.GenerateToken(user, _configuration);
                var message = new Message
                (

                    new string[] { email },
                    "Password Confirmation",
                   $@"<style>
    p {{
        font-family: SVN-Poppins;

    }}

    td {{
        font-family: SVN-Poppins;

    }}

    .detail-text {{
        color: #bbb;
        font-family: SVN-Poppins;
        font-size: 12px;
        font-weight: 400;
        font-style: normal;
        letter-spacing: normal;
        line-height: 20px;
        text-transform: none;
        text-align: center;
        padding: 0;
        margin: 0
    }}

    .topBorder {{
        background-color: #0091ff;
        font-size: 1px;
        line-height: 3px
    }}



    .text-button {{
        color: #fff;
        font-family: SVN-Poppins;
        font-size: 13px;
        font-weight: 600;
        font-style: normal;
        letter-spacing: 1px;
        line-height: 20px;
        text-transform: uppercase;
        text-decoration: none;
        display: block
    }}

    .normal-text {{
        color: #000;
        font-family: SVN-Poppins;
        font-size: 28px;
        font-weight: 500;
        font-style: normal;
        letter-spacing: normal;
        line-height: 36px;
        text-transform: none;
        text-align: center;
        padding: 0;
        margin: 0
    }}

    .img-avt {{
        width: 100%;
        max-width: 200px;
        height: auto;
        display: block;
        color: #f9f9f9;
        border-radius: 100%;
        border: 2px solid #fff;

    }}

    .tableCard {{
        background-color: #fff;
        border-color: #e5e5e5;
        border-style: solid;
        border-width: 0 1px 1px 1px;
        margin-top: 20px
    }}

    .link-text {{
        color: #bbb;
        text-decoration: underline
    }}
</style>
<table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"" style=""table-layout:fixed;background-color:#f9f9f9""
    id=""bodyTable"">
    <tbody>
        <tr>
            <td style=""padding-right:10px;padding-left:10px;"" align=""center"" valign=""top"" id=""bodyCell"">
                <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"" class=""wrapperBody""
                    style=""max-width:600px"">
                    <tbody>
                        <tr>
                            <td align=""center"" valign=""top"">
                                <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"" class=""tableCard"">
                                    <tbody>
                                        <tr>
                                            <td class=""topBorder"" height=""3"">&nbsp;</td>
                                        </tr>
                                        
                                        <tr>
                                            <td style=""padding-bottom: 20px;"" align=""center"" valign=""top""
                                                class=""imgHero""> <a href=""#"" style=""text-decoration:none""
                                                    target=""_blank""> <img alt="""" border=""0""
                                                        src=""https://i.imgur.com/RTrHqlY.png"" class=""img-avt""
                                                        width=""200px""> </a>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style=""padding-bottom: 5px; padding-left: 20px; padding-right:
                                                20px;"" align=""center"" valign=""top"" class=""mainTitle"">
                                                <h2 class=""normal-text"">Chào bạn</h2>
                                            </td>
                                        </tr>
                                        <!-- Verify Email // -->
                                        <tr>
                                            <td style=""padding-bottom: 10px;
                                                padding-left: 20px; padding-right: 20px;"" align=""center"" valign=""top""
                                                class=""subTitle"">
                                                <h4 class=""detail-text""
                                                    style=""font-size:16px;line-height:24px;padding:0;margin:0"">Xác
                                                    nhận email của bạn</h4>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style=""padding-left:20px;padding-right:20px"" align=""center"" valign=""top""
                                                class=""containtTable ui-sortable"">
                                                <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%""
                                                    class=""tableDescription"">
                                                    <tbody>
                                                        <tr>
                                                            <td style=""padding-bottom: 20px;"" align=""center""
                                                                valign=""top"" class=""description"">
                                                                <p class=""normal-text""
                                                                    style=""font-size:14px;font-weight:400;font-style:normal"">
                                                                    Để hoàn thành quá trình đặt lại mật khẩu, vui
                                                                    lòng ấn Xác nhận Email. <br> Email sẽ có hiệu
                                                                    lực trong 5 phút</p>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                                <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%""
                                                    class=""tableButton"" style="""">
                                                    <tbody>
                                                        <tr>
                                                            <td style=""padding-top:20px;padding-bottom:20px""
                                                                align=""center"" valign=""top"">
                                                                <table border=""0"" cellpadding=""0"" cellspacing=""0""
                                                                    align=""center"">
                                                                    <tbody>
                                                                        <tr>
                                                                            <td align=""center"" class=""confirm-button""
                                                                                style=""padding: 12px 35px;
                                                                                border-radius: 50px;
                                                                                background-color: #0091ff;""> <a
                                                                                    style=""text-decoration: none;""
                                                                                    href=""http://localhost:3000/password/recover?id={token.AccessToken}""
                                                                                   
                                                                                    "" target=""_blank""
                                                                                    class=""text-button"" style=""color:
                                                                                    #f9f9f9;"">Xác nhận
                                                                                    Email</a> </td>
                                                                        </tr>
                                                                    </tbody>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style=""font-size:1px;line-height:1px"" height=""20"">&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td align=""center"" valign=""middle"" style=""padding-bottom: 40px;""
                                                class=""emailRegards"">
                                                <!-- Image and Link // --> <a href=""#"" target=""_blank""
                                                    style=""text-decoration:none;""> <img mc:edit=""signature""
                                                        src=""https://i.imgur.com/HBcwSk7.png"" alt="""" width=""150""
                                                        border=""0"" style=""width:100%;
                                                        max-width:150px; height:auto; display:block;""> </a>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                                <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"" class=""space"">
                                    <tbody>
                                        <tr>
                                            <td style=""font-size:1px;line-height:1px"" height=""30"">&nbsp;</td>
                                        </tr>
                                    </tbody>
                                </table>
                            </td>
                        </tr>
                    </tbody>
                </table>
                <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"" class=""wrapperFooter""
                    style=""max-width:600px"">
                    <tbody>
                        <tr>
                            <td align=""center"" valign=""top"">
                                <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"" class=""footer"">
                                    <tbody>
                                        <tr>
                                            <td style=""padding-top:10px;padding-bottom:10px;padding-left:10px;padding-right:10px""
                                                align=""center"" valign=""top"" class=""socialLinks""> <a
                                                    href=""#facebook-link"" style=""display:inline-block"" target=""_blank""
                                                    class=""facebook""> <img alt="""" border=""0""
                                                        src=""http://email.aumfusion.com/vespro/img/social/light/facebook.png""
                                                        style=""height:auto;width:100%;max-width:40px;margin-left:2px;margin-right:2px""
                                                        width=""40""> </a> <a href=""#twitter-link""
                                                    style=""display: inline-block;"" target=""_blank"" class=""google""> <img
                                                        alt="""" border=""0""
                                                        src=""http://email.aumfusion.com/vespro/img/social/light/google.png""
                                                        style=""height:auto;width:100%;max-width:40px;margin-left:2px;margin-right:2px""
                                                        width=""40""> </a> </td>
                                        </tr>
                                        <tr>
                                            <td style=""padding: 10px 10px 5px;"" align=""center"" valign=""top""
                                                class=""brandInfo"">
                                                <p class=""detail-text"">©&nbsp;Chatable Inc. |
                                                    123 Đ.ABC | TP.HCM, VietNam .</p>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style=""padding: 0px 10px 20px;"" align=""center"" valign=""top""
                                                class=""footerLinks"">
                                                <p class=""detail-text"">
                                                    <a href=""#"" class=""link-text"" target=""_blank"">Đến
                                                        Chatable</a>&nbsp;|&nbsp; <a href=""#"" class=""link-text""
                                                        target=""_blank"">Điều khoản người dùng </a>&nbsp;|&nbsp; <a
                                                        href=""#"" class=""link-text"" target=""_blank"">Chính sách
                                                        riêng tư</a>
                                                </p>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style=""padding: 0px 10px 10px;"" align=""center"" valign=""top""
                                                class=""footerEmailInfo"">
                                                <p class=""detail-text"">Nếu bạn có thắc
                                                    mắc, vui lòng liên hệ chúng tôi tại <a href=""#"" class=""link-text""
                                                        target=""_blank"">support@mail.com.</a> </p>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style=""font-size:1px;line-height:1px"" height=""30"">&nbsp;</td>
                                        </tr>
                                    </tbody>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td style=""font-size:1px;line-height:1px"" height=""30"">&nbsp;
                            </td>
                        </tr>
                    </tbody>
                </table>
            </td>
        </tr>
    </tbody>
</table>
");

                _emailSender.SendEmail(message);

                return new APIResponse<string>
                {
                    StatusCode = 200,
                    Message = "Send request forgot password successfully.",
                    Data = token.AccessToken
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<string>
                {
                    StatusCode = 500,
                    Message = ex.Message
                };
            }

        }
    }
}
