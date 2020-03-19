﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MemeWebsiteApi.Models;
using MemeWebsiteApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace MemeWebsiteApi.Controllers
{

        [Route("api/tags")]
        [ApiController]
        public class TagsController : ControllerBase
        {
            private readonly TagService _tagService;

            public TagsController(TagService tagService)
            {
            _tagService = tagService;
            }

            [HttpGet("get")]
            public ActionResult<List<TagModel>> Get() =>
                _tagService.Get();

            [HttpGet("{id:length(24)}", Name = "GetTag")]
            public ActionResult<TagModel> Get(string id)
            {
                var tag = _tagService.Get(id);

                if (tag == null)
                {
                    return NotFound();
                }

                return tag;
            }

            [HttpGet("get/order")]
            public ActionResult<List<TagModel>> GetByOrder() =>
            _tagService.GetInOrder();

            [HttpPost]
            public ActionResult<TagModel> Create(TagModel tag)
            {
            if (_tagService.CheckTag(tag.Name) == true)
            {
                return BadRequest(new { message = "Tag already exist" });

            }
            else 
            {
                _tagService.Create(tag);

                return CreatedAtRoute("GetTag", new { id = tag.Id.ToString() }, tag);
            }
           
            }

            [HttpPut("{id:length(24)}")]
            public IActionResult Update(string id, TagModel tagIn)
            {
                var tag = _tagService.Get(id);

                if (tag == null)
                {
                    return NotFound();
                }

            _tagService.Update(id, tagIn);

                return NoContent();
            }

            [HttpDelete("{id:length(24)}")]
            public IActionResult Delete(string id)
            {
                var tag = _tagService.Get(id);

                if (tag == null)
                {
                    return NotFound();
                }

            _tagService.Remove(tag.Id);

                return NoContent();
            }
        }
}
