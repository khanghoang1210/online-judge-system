using judge.system.core.Service.Interface;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
        public async Task<IActionResult> GetById(int problemId)
        {
            var res = await _problemService.GetById(problemId);
            return Ok(res);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            var res = await _problemService.GetAll();
            return Ok(res);
        }
    }
}
