using api.DTOs.Comment;
using api.Models;

namespace api.Mappers
{
    public static class CommentMappers
    {
        public static CommentDto ToCommentDto(this Comment comment)
        {
            return new CommentDto
            {
                Id = comment.Id,
                Title = comment.Title,
                Content = comment.Content,
                CreatedOn = comment.CreatedOn,
                StockId = comment.StockId,
                CreatedBy = comment.AppUser?.UserName,

            };
        }

        public static Comment ToCommentFromCreate(this CreateCommentDto comment, int stockId, AppUser appUser)
        {
            return new Comment
            {
                Title = comment.Title,
                Content = comment.Content,
                StockId = stockId,
                AppUserId = appUser.Id,
                //AppUser = appUser
            };
        }

        public static Comment ToCommentFromUpdate(this UpdateCommentRequestDto comment)
        {
            return new Comment
            {
                Title = comment.Title,
                Content = comment.Content
            };
        }
    }
}
