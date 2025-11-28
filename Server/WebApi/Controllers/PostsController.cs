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
        catch (KeyNotFoundException)
        {
            return NotFound($"Post with ID {postId} not found.");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet("users/{userId:int}")]
    public async Task<ActionResult<List<PostDTO>>> GetPostsByUserId
    (
        [FromRoute] int userId
    )
    {
        try
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
        catch (KeyNotFoundException)
        {
            return NotFound($"Posts for User with ID {userId} not found.");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet]
    public async Task<ActionResult<List<PostDTO>>> GetAllPosts()
    {
        try
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
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet("title")]
    public async Task<ActionResult<List<PostDTO>>> GetPostsByTitle
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
            Post post = new(postToUpdate.Title, postToUpdate.Body, postToUpdate.UserId)
            {
                Id = postToUpdate.PostId
            };
            await postRepository.UpdateAsync(post);
            PostDTO postDTO = new()
            {
                PostId = post.Id,
                Title  = post.Title,
                Body = post.Body,
                UserId = post.UserId
            };
            return Ok(postDTO);
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
        [FromBody] CreatePostDTO postToAdd
    )
    {
        try
        {
            Post post = new(postToAdd.Title, postToAdd.Body, postToAdd.UserId);
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