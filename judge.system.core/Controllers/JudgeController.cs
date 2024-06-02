using judge.system.core.DTOs.Requests.Judge;
using judge.system.core.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace judge.system.core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JudgeController : ControllerBase
    {

        private readonly IJudgeService _judgeService;

        public JudgeController(IJudgeService judgeService)
        {
            _judgeService = judgeService;
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> Submit(SubmitCodeReq req)
        {
            var res = await _judgeService.Submit(req);
            return Ok(res);
        }
        [HttpGet("GetInputOut/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var res = await _judgeService.GetInOut(id);
            return Ok(res);
        }
    }
}
