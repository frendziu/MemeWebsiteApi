﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MemeWebsiteApi.Models;
using MemeWebsiteApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Web;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace MemeWebsiteApi.Controllers
{

    [Route("api/memes")]
    [ApiController]
    public class MemesController : ControllerBase
    {
    
        private readonly MemeService _memeService;

        public MemesController(MemeService memeService)
        {
            _memeService = memeService;
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

            Byte[] b = System.IO.File.ReadAllBytes("Images/UploadedImages/" + id + ".jpg");   // You can use your own method over here.         
            return File(b, "image/jpeg");
        }

        [HttpPost]
       public ActionResult<Meme> Upload(Meme meme)
        {
            _memeService.Upload(meme);

            return CreatedAtRoute("GetMeme", new { id = meme.Id.ToString() }, meme);
        }




        [HttpPut("rateplus/{id:length(24)}")]
        public IActionResult RPlus(string id)
        {
            var meme = _memeService.Get(id);

            if (meme == null)
            {
                return NotFound();
            }

            _memeService.RatingPlus(id, meme);

            return NoContent();
        }

        [HttpPut("rateminus/{id:length(24)}")]
        public IActionResult RMinus(string id)
        {
            var meme = _memeService.Get(id);

            if (meme == null)
            {
                return NotFound();
            }

            _memeService.RatingMinus(id, meme);

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
