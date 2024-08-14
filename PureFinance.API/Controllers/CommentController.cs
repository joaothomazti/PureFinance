using Microsoft.AspNetCore.Mvc;
using Pure.Application.Dtos.Comment;
using Pure.Application.Interfaces;
using Pure.Application.Mappers;

namespace PureFinance.API.Controllers
{
    [Route("api/comment")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepo;
        private readonly IStockRepository _stockRepo;

        public CommentController(ICommentRepository commentRepo, IStockRepository stockRepo)
        {
            _commentRepo = commentRepo;
            _stockRepo = stockRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var comments = await _commentRepo.GetAllAsync();

            var commentDto = comments.Select(x => x.ToCommentDto());

            return Ok(commentDto);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var comment = await _commentRepo.GetByIdAsync(id);

            if(comment == null) return NotFound();

            return Ok(comment.ToCommentDto());
        }

        [HttpPost]
        [Route("{stockId}")]
        public async Task<IActionResult> Create([FromRoute]int stockId, CreateCommentRequestDto commentDto)
        {
            if(!await _stockRepo.StockExists(stockId))
            {
                return BadRequest("Stock does not exist");
            }

            var commentModel = commentDto.ToCommentFromCreateDTO(stockId);
            await _commentRepo.CreateAsync(commentModel);
            return CreatedAtAction(nameof(GetById), new {id = commentModel}, commentModel.ToCommentDto());
        }

        [HttpPut]
        [Route("{id}")]
       public async Task<IActionResult> Update([FromRoute]int id, [FromBody] UpdateCommentRequestDto updateDto)
        {
            var comment = await _commentRepo.UpdateAsync(id, updateDto.ToCommentFromUpdate());

            if (comment == null) return NotFound("Comment not Found");

            return Ok(comment.ToCommentDto());
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var deleteComment = await _commentRepo.DeleteAsync(id);

            if(deleteComment == null) return NotFound();

            return NoContent();
        }
    }
}
