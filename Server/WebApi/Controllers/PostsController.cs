using ApiContracts.PostDTOs;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class PostsController(IPostRepository postRepository) : ControllerBase
{
    [HttpGet("{postId:int}")]
    public async Task<ActionResult<PostDTO>> GetPostById
    (
        [FromRoute] int postId
    )
    {
        try
        {
            Post post = await postRepository.GetSingleAsync(postId);
            PostDTO response = new()
            {
                PostId = post.Id,
                Title = post.Title,
                Body = post.Body,
                UserId = post.UserId
            };
            
            return Ok(response);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return NotFound(e);
        }
    }

    [HttpGet("{userId:int}")]
    public async Task<ActionResult<List<PostDTO>>> GetPostsByUserId
    (
        [FromRoute] int userId
    )
    {
        List<PostDTO> posts = postRepository.GetMany()
            .Where(post => post.UserId == userId)
            .Select(post => new PostDTO
            {
                PostId = post.Id,
                Title = post.Title,
                Body = post.Body,
                UserId = post.UserId
            })
            .ToList();
        return Ok(posts);
    }

    [HttpGet]
    public async Task<ActionResult<List<PostDTO>>> GetAllPosts()
    {
        List<PostDTO> posts = postRepository.GetMany()
            .Select(post => new PostDTO
            {
                PostId = post.Id,
                Title = post.Title,
                Body = post.Body,
                UserId = post.UserId
            })
            .ToList();
        return Ok(posts);
    }

    [HttpGet]
    public async Task<ActionResult<List<PostDTO>>> GetPostByTitle
    (
        [FromQuery] string title
    )
    {
        title = title.Trim();
        
        if (string.IsNullOrEmpty(title))
            return BadRequest("Query 'title' is required.");
        
        List<PostDTO> response = postRepository.GetMany().Where(post => post.Title.Equals(title, StringComparison.OrdinalIgnoreCase)).Select(post => new PostDTO
        {
            PostId = post.Id,
            Title = post.Title,
            Body = post.Body,
            UserId = post.UserId
        }).ToList();
        
        if (response.Count == 0)
            return NotFound($"Post with title '{title}' not found.");
        
        return Ok(response);
    }
    
    [HttpPut("{postId:int}")]
    public async Task<ActionResult> UpdatePost
    (
        [FromRoute] int postId,
        [FromBody] PostDTO postToUpdate
    )
    {
        if (postId != postToUpdate.PostId)
            return BadRequest("Post ID in route and body do not match.");
        
        try
        {
            Post post = new()
            {
                Id = postToUpdate.PostId,
                Title = postToUpdate.Title,
                Body = postToUpdate.Body,
                UserId = postToUpdate.UserId
            };
            await postRepository.UpdateAsync(post);
            return NoContent();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return NotFound(e.Message);
        }
    }
     
    [HttpPost]
    public async Task<ActionResult<PostDTO>> AddPost
    (
        [FromBody] PostDTO postToAdd
    )
    {
        try
        {
            Post post = new()
            {
                Title = postToAdd.Title,
                Body = postToAdd.Body,
                UserId = postToAdd.UserId
            };
            Post created = await postRepository.AddAsync(post);
            PostDTO response = new()
            {
                PostId = created.Id,
                Title = created.Title,
                Body = created.Body,
                UserId = created.UserId
            };
            return Created($"/posts/{response.PostId}", response);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(404, e.Message);
        }
    }
    
    [HttpDelete("{postId:int}")]
    public async Task<ActionResult> DeletePost
    (
        [FromRoute] int postId
    )
    {
        try
        {
            await postRepository.DeleteAsync(postId);
            return NoContent();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return NotFound(e.Message);
        }
    }
}