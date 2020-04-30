using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MemeWebsiteApi.Models;
using MemeWebsiteApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace MemeWebsiteApi.Controllers
{

    [Route("api/comments")]
        [ApiController]
        public class CommentsController : ControllerBase
        {
            private readonly CommentService _commentService;
        private readonly UserService _userService;

        public CommentsController(CommentService commentService, UserService userService)
            {
            _commentService = commentService;
            _userService = userService;
        }

        protected string GetUserId()
        {
            return this.User.Claims.First(i => i.Type == "UserId").Value;
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
            [Authorize]
            [HttpPost]
            public ActionResult<Comment> Create(Comment comment)
            {
            string Id = GetUserId();
            string nickname = _userService.GetUserNameById(Id);

            _commentService.Create(comment, nickname);

                return CreatedAtRoute("GetComment", new { id = comment.Id.ToString() }, comment);
            }

        [HttpGet("getbyid")]
        public ActionResult <List<Comment>> GetMemeComments(string id)
        {
            List<Comment> memeComments = new List <Comment>();

            memeComments = _commentService.GetMemeComments(id);

            return memeComments;

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
