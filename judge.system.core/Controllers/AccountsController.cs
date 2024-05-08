using judge.system.core.DTOs.Requests.Account;
using judge.system.core.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace judge.system.core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        [HttpPost("[action]")]
        public async Task<ActionResult> Create(CreateAccountReq req)
        {
            var res = await _accountService.Create(req);
            return Ok(res);
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> Login(LoginReq req)
        {
            var res = await _accountService.Login(req);
            return Ok(res);
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAll()
        {
            var res = await _accountService.ReadAll();
            return Ok(res);
        }

        [HttpPut("[action]")]
        public async Task<ActionResult> Update(UpdateAccountReq req, [FromQuery] string userName)
        {
            var res = await _accountService.Update(req, userName);
            return Ok(res);
        }
    }
}
