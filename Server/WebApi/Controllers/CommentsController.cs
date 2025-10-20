using ApiContracts.CommentDTOs;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class CommentsController(ICommentRepository commentRepository) : ControllerBase
{
    [HttpGet("{commentId:int}")]
    public async Task<ActionResult<CommentDTO>> GetCommentById
        ([FromRoute] int commentId)
    {
        try
        {
            Comment comment = await commentRepository.GetSingleAsync(commentId);
            CommentDTO response = new()
            {
                CommentId = comment.Id,
                Body = comment.Body,
                PostId = comment.PostId,
                UserId = comment.UserId
            };
            return Ok(response);

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return NotFound();
        }
    }
    
    [HttpGet("/posts/{postId:int}/comments")]
    public async Task<ActionResult<List<CommentDTO>>> GetCommentsByPostId
        ([FromRoute] int postId)
    {
        List<CommentDTO> comments = commentRepository.GetMany()
            .Where(comment => comment.PostId == postId)
            .Select(comment => new CommentDTO
            {
                CommentId = comment.Id,
                Body = comment.Body,
                PostId = comment.PostId,
                UserId = comment.UserId
            })
            .ToList();
        
        if (comments.Count == 0 )
        {
            return NotFound();
        }
        return Ok(comments);
    }

    [HttpGet("/users/{userId:int}/comments")]
    public async Task<ActionResult<List<CommentDTO>>> GetCommentsByUserId
        ([FromRoute] int userId)
    {
        List<CommentDTO> comments = commentRepository.GetMany()
            .Where(comment => comment.UserId == userId)
            .Select(comment => new CommentDTO
            {
                CommentId = comment.Id,
                Body = comment.Body,
                PostId = comment.PostId,
                UserId = comment.UserId
            })
            .ToList();

        if (comments.Count == 0)
        {
            return NotFound();
        }
        return Ok(comments);
    }

    [HttpGet("/all-comments")]
    public async Task<ActionResult<List<CommentDTO>>> GetAllComments()
    {
        List<CommentDTO> comments = commentRepository.GetMany()
            .Select(comment => new CommentDTO
            {
                CommentId = comment.Id,
                Body = comment.Body,
                PostId = comment.PostId,
                UserId = comment.UserId
            })
            .ToList();

        if (comments.Count == 0)
        {
            return NotFound();
        }
        return Ok(comments);   
    }
    
    [HttpPut("{commentId:int}")]
    public async Task<ActionResult> UpdateComment
        ([FromRoute] int commentId, [FromBody] CommentDTO updatedComment)
    {
        if (commentId != updatedComment.CommentId)
            return BadRequest("Comment ID in route does not match Comment ID in body.");

        try
        {
            Comment existingComment = await commentRepository.GetSingleAsync(commentId);
            existingComment.Body = updatedComment.Body;
            existingComment.PostId = updatedComment.PostId;
            existingComment.UserId = updatedComment.UserId;

            await commentRepository.UpdateAsync(existingComment);
            return NoContent();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return NotFound();
        }
    }

    [HttpPost]
    public async Task<ActionResult<CommentDTO>> AddComment
        ([FromBody] CommentDTO newComment)
    {
        Comment comment = new Comment(newComment.Body, newComment.PostId, newComment.UserId);
        Comment created = await commentRepository.AddAsync(comment);
        CommentDTO response = new()
        {
            Body = created.Body,
            CommentId = created.Id,
            PostId = created.PostId,
            UserId = created.UserId
        };
        return Created($"/comments/{response.CommentId}", response);
    }

    [HttpDelete("{commentId:int}")]
    public async Task<ActionResult> DeleteComment
        ([FromRoute] int commentId)
    {
        try
        {
            await commentRepository.DeleteAsync(commentId);
            return Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return NotFound(e.Message);
        }
    }
    
    
}