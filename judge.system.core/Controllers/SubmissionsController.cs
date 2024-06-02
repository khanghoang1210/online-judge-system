using judge.system.core.DTOs.Requests.Submission;
using judge.system.core.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace judge.system.core.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class SubmissionsController : ControllerBase
    {
        private readonly ISubmissionService _submissionService;
        
        public SubmissionsController(ISubmissionService submissionService)
        {
            _submissionService = submissionService;
        }

        //[HttpGet("[action]")]
        //[Authorize]
        //public async Task<IActionResult> Get20NearestSubmissions([FromQuery] GetSubmissionReq request)
        //{
        //    var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "AccountId")?.Value);
        //    var submissions = await _submissionService.Get20Nearest(userId, request.size);
        //    return Ok(submissions);
        //}
    }
}
