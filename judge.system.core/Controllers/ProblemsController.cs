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

        [HttpGet("{problemId}")]
        public async Task<IActionResult> GetById(int problemId)
        {
            var res = await _problemService.GetById(problemId);
            return Ok(res);
        }

        [HttpGet]

        public async Task<IActionResult> GetAll()
        {
            var res = await _problemService.GetAll();
            return Ok(res);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetProblemDetail(int problemId)
        {
            var res = await _problemService.GetProblemDetail(problemId);
            return Ok(res);
        }
    }
}
