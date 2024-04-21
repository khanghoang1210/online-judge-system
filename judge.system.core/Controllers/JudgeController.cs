using judge.system.core.Service;
using Microsoft.AspNetCore.Mvc;

namespace judge.system.core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JudgeController : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult> Judge(string code)
        {
            var res = JudgeService.JudgeC(code);

            return Ok(res);
        }

    }
}
