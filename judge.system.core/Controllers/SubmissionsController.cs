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

        [HttpGet]
        public async Task<IActionResult> GetNNearest()
        {
            var response = await _submissionService.GetNNearest();
            
            if (response.StatusCode == 200)
            {
                return Ok(response);
            }
            return StatusCode(response.StatusCode, response.Message);
        }
    }
}
