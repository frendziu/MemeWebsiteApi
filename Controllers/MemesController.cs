using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MemeWebsiteApi.Models;
using MemeWebsiteApi.Services;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet("single/{id:length(24)}", Name = "GetMeme")]
        public ActionResult<Meme> Get(string id)
        {
            var meme = _memeService.Get(id);

            if (meme == null)
            {
                return NotFound();
            }

            return meme;
        }

        [HttpPost]
        public ActionResult<Meme> Create(Meme meme)
        {
            _memeService.Create(meme);

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
        // /api/memes/str?pagenumber=1
        [HttpGet("str")]
        public ActionResult<List<Meme>>GetInPage(int pagenumber, int limit)
        {
            List<Meme> memes = new List<Meme>();

            if (pagenumber == 0)
            {
                return BadRequest("There is no memes");
            }
            else if (pagenumber < 0 || limit <= 0)
            {
                return BadRequest("Incorrect params");
            }
            else
            {

                memes = _memeService.GetMemesPage(pagenumber, limit);


                return memes;

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

            _memeService.Remove(meme.Id);

                return NoContent();
            }
        }
}
