using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MemeWebsiteApi.Models;
using MemeWebsiteApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Web;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace MemeWebsiteApi.Controllers
{

    [Route("api/memes")]
    [ApiController]
    public class MemesController : ControllerBase
    {

        private readonly MemeService _memeService;
        private readonly UserService _userService;
        private readonly TagService _tagService;

        protected string GetUserId()
        {
            return this.User.Claims.First(i => i.Type == "UserId").Value;
        }

        public MemesController(MemeService memeService, UserService userService, TagService tagService)
        {
            _memeService = memeService;
            _userService = userService;
            _tagService = tagService;
        }

        [HttpGet("get")]
        public ActionResult<List<Meme>> Get() =>
            _memeService.Get();

        [HttpGet("get/{id:length(24)}", Name = "GetMeme")]
        public ActionResult<Meme> Get(string id)
        {
            var meme = _memeService.Get(id);

            if (meme == null)
            {
                return NotFound();
            }

            return meme;
        }

        [HttpGet("single/{id:length(24)}")]
        public IActionResult GetSingle(string id)
        {
            var meme = _memeService.Get(id);

            if (meme == null)
            {
                return NotFound();
            }

            
            string filename = id + "." + meme.Type;

            Byte[] b = System.IO.File.ReadAllBytes("Images/UploadedImages/" + filename); // You can use your own method over here.  
                if (meme.Type == "mp4")
            return File(b, "video/mp4");
                else
                return File(b, "image/jpeg");
        }
        [Authorize]
        [HttpPost]
        public ActionResult<Meme> Upload(Meme meme)
        {
            if (meme.Type != "mp4" && meme.Type != "jpg" && meme.Type != "gif" && meme.Type != "png")
            {
                return BadRequest("Bad meme type");
            }

            bool[] isTag = new bool[_tagService.GetCount()];
            bool isTagExist = true;
            for (int i = 0; i<isTag.Length; i++)
            {
                isTag[i] = true;
            }
            for (int i = 0; i < meme.Tags.Length; i++)
            {
                isTag[i] = _tagService.CheckTag(meme.Tags[i]);
            }

            for (int i = 0; i < isTag.Length; i++)
            {
                if(isTag[i] == false)
                {
                    isTagExist = false;
                }
                
            }


            if (isTagExist == false)
            {
                return BadRequest("Invalid Tag");
            }

            string Id = GetUserId();
            string nickname = _userService.GetUserNameById(Id);
                       

            _memeService.Upload(meme, nickname);

            return CreatedAtRoute("GetMeme", new { id = meme.Id.ToString() }, meme);
        }



        [Authorize]
        [HttpPut("rateplus/{id:length(24)}")]
        public IActionResult RPlus(string id)
        {
            var meme = _memeService.Get(id);

            if (meme == null)
            {
                return NotFound();
            }
            string Id = GetUserId();
            string nickname = _userService.GetUserNameById(Id);
            bool alreadyExist = meme.Rating.Voted.Contains(nickname);
            if (alreadyExist == true)
            {
                return BadRequest("User voted");
            }
            else
            {
                _memeService.RatingPlus(id, meme, nickname);
            }
            

            return NoContent();
        }
        [Authorize]
        [HttpPut("rateminus/{id:length(24)}")]
        public IActionResult RMinus(string id)
        {
            var meme = _memeService.Get(id);

            if (meme == null)
            {
                return NotFound();
            }
            string Id = GetUserId();
            string nickname = _userService.GetUserNameById(Id);
            bool alreadyExist = meme.Rating.Voted.Contains(nickname);
            if (alreadyExist == true)
            {
                return BadRequest("User voted");
            }
            else
            {
                _memeService.RatingMinus(id, meme, nickname);
            }
            

            return NoContent();
        }

        [HttpGet("count")]
        public ActionResult<int> Count() {

            int count = _memeService.GetCount();
            return count;
        }
        // /api/memes/page?number=1&limit=1
        [HttpGet("page")]
        public ActionResult<List<Meme>>GetInPage(int number, int limit)
        {
            List<Meme> memes = new List<Meme>();

            if (number == 0)
            {
                return BadRequest("There is no memes");
            }
            else if (number < 0 || limit <= 0)
            {
                return BadRequest("Incorrect params");
            }
            else
            {

                memes = _memeService.GetMemesPage(number, limit);


                return memes;

            }
        }

        // /api/memes/tag
        [HttpGet("tags")]
        public ActionResult<List<Meme>> GetByTags([FromQuery]string[] tags, int page, int limit)
        {
            List<Meme> memes = new List<Meme>();
           
            

            if (page == 0)
            {
                return BadRequest("There is no memes");
            }
            else if (page < 0 || limit <= 0)
            {
                return BadRequest("Incorrect params");
            }
            else if (tags.Length==0)
            {
                return BadRequest("No tags");
            }
            else
            {
                memes = _memeService.GetByTags(tags, page, limit);
                if (memes.Count == 0)
                {
                    return BadRequest("Thera are no memes with that tags");
                }
                else
                {
                    return memes;
                }
               
            }
        }
               

        [HttpDelete("{id:length(24)}")]
            public IActionResult Delete(string id)
            {
                var meme = _memeService.Get(id);

                if (meme == null)
                {
                    return NotFound();
                }

            _memeService.Remove(meme.Id, meme.Type);

                return NoContent();
            }
        }
}
