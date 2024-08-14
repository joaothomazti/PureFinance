using Microsoft.EntityFrameworkCore;
using Pure.Application.Interfaces;
using Pure.Domain.Models;
using Pure.Infrastructure.Context;

namespace Pure.Infrastructure.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDbContext _context;

        public CommentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Comment> CreateAsync(Comment commentModel)
        {
            await _context.AddAsync(commentModel);
            await _context.SaveChangesAsync();
            return commentModel;
        }

        public async Task<Comment?> DeleteAsync(int id)
        {
            var existingComment = await _context.Comments.FirstOrDefaultAsync(c => c.Id == id);

            if (existingComment == null) return null;

            _context.Comments.Remove(existingComment);
            await _context.SaveChangesAsync();
            return existingComment;
        }

        public async Task<List<Comment>> GetAllAsync()
        {
            return await _context.Comments.AsNoTracking().ToListAsync();
        }

        public async Task<Comment?> GetByIdAsync(int id)
        {
            return await _context.Comments.FindAsync(id);
        }

        public async Task<Comment?> UpdateAsync(int id, Comment commentModel)
        {
            var existingComment = await _context.Comments.FindAsync(id);

            if (existingComment == null) return null;

            existingComment.Title = commentModel.Title;
            existingComment.Content = commentModel.Content;

            await _context.SaveChangesAsync();

            return existingComment;
        }
    }
}
