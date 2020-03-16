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

            [HttpGet("{id:length(24)}", Name = "GetMeme")]
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

            [HttpPut("{id:length(24)}")]
            public IActionResult Update(string id, Meme memeIn)
            {
                var meme = _memeService.Get(id);

                if (meme == null)
                {
                    return NotFound();
                }

            _memeService.Update(id, memeIn);

                return NoContent();
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
