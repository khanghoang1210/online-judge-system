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
        public AccountService(Context context)
        {
            _context = context;
        }
        public async Task<APIResponse<string>> Create(CreateAccountReq req)
        {
            try
            {
                var newAcc = new Account
                {
                    UserName = req.UserName,
                    Password = req.Password,
                    Email = req.Email,
                    FullName = req.FullName,
                };
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
