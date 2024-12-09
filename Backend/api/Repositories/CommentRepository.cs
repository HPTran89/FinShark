using api.Data;
using api.DTOs.Comment;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDBContext _dbContext;

        public CommentRepository(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Comment> CreateAsync(Comment commentModel)
        {
            await _dbContext.Comments.AddAsync(commentModel);
            await _dbContext.SaveChangesAsync();
            return commentModel;
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var comment = await _dbContext.Comments.FirstOrDefaultAsync(c => c.Id == id);
                if (comment != null)
                {
                    _dbContext.Comments.Remove(comment);
                    _dbContext.SaveChanges();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<Comment>> GetAllAsync()
        {
            var comments = await _dbContext.Comments.Include(s => s.AppUser).ToListAsync();
            return comments;
        }

        public async Task<Comment?> GetByIdAsync(int id)
        {
            return await _dbContext.Comments.Include(a => a.AppUser).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Comment> UpdateAsync(int commentId, UpdateCommentRequestDto updateComment)
        {
            var comment = await _dbContext.Comments.FindAsync(commentId);
            if (comment == null) 
            {
                return null;
            }

            comment.Title = updateComment.Title;
            comment.Content = updateComment.Content;

            await _dbContext.SaveChangesAsync();
            return comment;
        }
    }
}
