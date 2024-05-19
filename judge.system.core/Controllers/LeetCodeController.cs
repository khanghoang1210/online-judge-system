using judge.system.core.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace judge.system.core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeetCodeController : ControllerBase
    {
        private readonly ILeetCodeService _leetCodeService;

        public LeetCodeController(ILeetCodeService leetCodeService)
        {
            _leetCodeService = leetCodeService;
        }

        /// <summary>
        /// Get problem in Leetcode and insert into DB
        /// Default 100 problems
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var result = await _leetCodeService.GetProblemListAsync();
            return Ok();
        }
    }
}
