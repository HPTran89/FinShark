using api.DTOs.Comment;
using api.Extensions;
using api.Interfaces;
using api.Mappers;
using api.Models;
using api.Repositories;
using api.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/comment")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IStockRepository _stockRepository;
        private readonly UserManager<AppUser> _userManager;

        public CommentController(ICommentRepository commentRepository, IStockRepository stockRepository, UserManager<AppUser> userManager)
        {
            _commentRepository = commentRepository;
            _stockRepository = stockRepository;
            _userManager = userManager;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllAsync([FromQuery] CommentQueryObject queryObject)
        {
            var comments = await _commentRepository.GetAllAsync(queryObject);
            var result = new List<CommentDto>();
            foreach (var item in comments)
            {
                result.Add(item.ToCommentDto());
            }
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var comment = await _commentRepository.GetByIdAsync(id);
            if (comment == null) { return NotFound(); }

            return Ok(comment.ToCommentDto());
        }

        [HttpPost("{stockId}")]
        [Authorize]
        public async Task<IActionResult> Create([FromRoute] int stockId, CreateCommentDto commentDto)
        {
            if (await _stockRepository.GetStockById(stockId) == null) { return BadRequest("Stock doe not exist"); }
            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);

            var commentModel = commentDto.ToCommentFromCreate(stockId, appUser);

            await _commentRepository.CreateAsync(commentModel);

            return CreatedAtAction(nameof(GetById), new { id = commentModel.Id }, commentModel.ToCommentDto());

        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCommentRequestDto commentDto)
        {
            var comment = await _commentRepository.UpdateAsync(id, commentDto);

            if (comment == null) { return NotFound("comment not found"); };

            return Ok(comment.ToCommentDto());
        }

        [HttpDelete("{id}")]
        [Authorize(Roles ="ADMIN")]
        public async Task<IActionResult> Delete([FromBody] int id)
        {

            await _commentRepository.DeleteAsync(id);
            return Ok();
        }
    }
}
