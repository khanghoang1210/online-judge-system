using judge.system.core.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace judge.system.core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private IPostService _postService;

        public PostsController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var res = await _postService.GetAll();
            return Ok(res);
        }

        [HttpGet("{postId}")]
        public async Task<IActionResult> GetById(int Id)
        {
            var res = await _postService.GetById(Id);
            return Ok(res);
        }
    }
}
