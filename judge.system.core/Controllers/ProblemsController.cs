using judge.system.core.DTOs.Requests.Problem;
using judge.system.core.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace judge.system.core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProblemsController : ControllerBase
    {
        private IProblemService _problemService;

        public ProblemsController(IProblemService problemService)
        {
            _problemService = problemService;
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(GetAllProblemReq req)
        {
            var res = await _problemService.GetById(req);
            return Ok(res);
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var res = await _problemService.ReadAll();
            return Ok(res);
        }
    }
}
