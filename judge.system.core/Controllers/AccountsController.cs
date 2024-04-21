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
        [HttpPost("Create")]
        public async Task<ActionResult> Create(CreateAccountReq req)
        {
            var res = await _accountService.Create(req);
            return Ok(res);
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var res = await _accountService.ReadAll();
            return Ok(res);
        }

        [HttpPut("Update")]
        public async Task<ActionResult> Update(UpdateAccountReq req, [FromQuery] string userName)
        {
            var res = await _accountService.Update(req, userName);
            return Ok(res);
        }
    }
}
