using api.DTOs.Comment;
using api.Models;
using api.Utility;

namespace api.Interfaces
{
    public interface ICommentRepository
    {
        Task<List<Comment>> GetAllAsync(CommentQueryObject queryObject);
        Task<Comment?> GetByIdAsync(int id);
        Task<Comment> CreateAsync(Comment commentModel);
        Task<Comment> UpdateAsync(int commentId, UpdateCommentRequestDto commentDto);
        Task DeleteAsync(int id);
    }
}
