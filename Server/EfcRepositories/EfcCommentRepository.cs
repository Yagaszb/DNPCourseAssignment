using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;

namespace EfcRepositories;

public class EfcCommentRepository : ICommentRepository
{
    private readonly AppContext _appContext;
    
    public EfcCommentRepository(AppContext appContext)
    {
        _appContext = appContext;
    }
    
    public async Task<Comment> AddAsync(Comment comment)
    {
        await _appContext.Comments.AddAsync(comment);
        await _appContext.SaveChangesAsync();
        return comment;
    }

    public async Task<Comment> UpdateAsync(Comment comment)
    {
        if (!(await _appContext.Comments.AnyAsync(c => c.Id == comment.Id)))
        {
            throw new Exception("Comment with id {comment.Id} not found");
        } 
        _appContext.Comments.Update(comment); 
        await _appContext.SaveChangesAsync();//TODO
        return comment;
    }

    public async Task DeleteAsync(int id)
    {
        Comment? existing = await _appContext.Comments.SingleOrDefaultAsync(c => c.Id == id);
        if (existing == null)
        {
            throw new Exception($"Comment with id {id} not found");
        } 
        _appContext.Comments.Remove(existing); 
        await _appContext.SaveChangesAsync();
    }

    public Task<Comment> GetSingleAsync(int id)
    {
        Comment? existing =  _appContext.Comments.SingleOrDefault(c => c.Id == id);
        if (existing == null)
        {
            throw new Exception($"Comment with id {id} not found");
        }

        return Task.FromResult(existing);
    }

    public IQueryable<Comment> GetMany()
    {
        return _appContext.Comments.AsQueryable();
    }
}