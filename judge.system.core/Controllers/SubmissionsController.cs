using judge.system.core.DTOs.Requests.Account;
using judge.system.core.Service.Interface;
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

        [HttpGet]
        ///[Authorize]
        public async Task<IActionResult> GetAllSubmission(string userName)
        {
            var response = await _submissionService.GetAllSubmission(userName);

            return Ok(response);
        }
        [HttpPost]
        //[Authorize]
        public async Task<ActionResult> CreateSubmission(CreateSubmissionReq req)
        {
            var res = await _submissionService.CreateSubmission(req);
            return Ok(res);
        }
    }
}
