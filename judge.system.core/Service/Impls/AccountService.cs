using AutoMapper;
using judge.system.core.Database;
using judge.system.core.DTOs.Requests.Account;
using judge.system.core.DTOs.Responses;
using judge.system.core.DTOs.Responses.Account;
using judge.system.core.Models;
using judge.system.core.Service.Interface;

namespace judge.system.core.Service.Impls
{
    public class AccountService : IAccountService
    {
        private readonly Context _context;
        private readonly IMapper _mapper;
        public AccountService(Context context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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
                    Data = "Success"
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
            };
        }

        public async Task<APIResponse<List<ReadAccountsRes>>> ReadAll()
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

        public async Task<APIResponse<string>> Update(UpdateAccountReq req, string userName)
        {
            var a = 1;
            var acc = _context.Accounts.FirstOrDefault(x => x.UserName == userName);
            acc.FullName = req.FullName;
            _context.Accounts.Update(acc);
            _context.SaveChanges();
            return new APIResponse<string>
            {
                StatusCode = 200,
                Message = "Success",
                Data = acc.UserName
            };
        }
    }
}
