using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MemeWebsiteApi.Models;
using MemeWebsiteApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace MemeWebsiteApi.Controllers
{

    [Route("api/comments")]
        [ApiController]
        public class CommentsController : ControllerBase
        {
            private readonly CommentService _commentService;

            public CommentsController(CommentService commentService)
            {
            _commentService = commentService;
            }

            [HttpGet("get")]
            public ActionResult<List<Comment>> Get() =>
                _commentService.Get();

            [HttpGet("{id:length(24)}", Name = "GetComment")]
            public ActionResult<Comment> Get(string id)
            {
                var comment = _commentService.Get(id);

                if (comment == null)
                {
                    return NotFound();
                }

                return comment;
            }

            [HttpPost]
            public ActionResult<Comment> Create(Comment comment)
            {
                _commentService.Create(comment);

                return CreatedAtRoute("GetComment", new { id = comment.Id.ToString() }, comment);
            }

            [HttpPut("{id:length(24)}")]
            public IActionResult Update(string id, Comment commentIn)
            {
                var comment = _commentService.Get(id);

                if (comment == null)
                {
                    return NotFound();
                }

            _commentService.Update(id, commentIn);

                return NoContent();
            }

            [HttpDelete("{id:length(24)}")]
            public IActionResult Delete(string id)
            {
                var comment = _commentService.Get(id);

                if (comment == null)
                {
                    return NotFound();
                }

            _commentService.Remove(comment.Id);

                return NoContent();
            }
        }
}
