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
       
        [Route("api/users")]
        [ApiController]
        public class UsersController : ControllerBase
        {
            private readonly UserService _userService;

            public UsersController(UserService userService)
            {
            _userService = userService;
            }

            [HttpGet()]
            public ActionResult<List<User>> Get() =>
                _userService.Get();

            [HttpGet("{id:length(24)}", Name = "GetUser")]
            public ActionResult<User> Get(string id)
            {
                var user = _userService.Get(id);

                if (user == null)
                {
                    return NotFound();
                }

                return user;
            }

        [HttpPost("register")]
        public ActionResult<User> Create(User user)
        {
            
             _userService.Create(user);
             return CreatedAtRoute("GetUser", new { id = user.Id.ToString() }, user);
        }

        [AllowAnonymous]
        [HttpPost("auth")]
        public IActionResult Authenticate([FromBody]AuthenticateModel model)
        {
            var user = _userService.Authenticate(model.Username, model.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(user);
        }


        [HttpPut("{id:length(24)}")]
            public IActionResult Update(string id, User userIn)
            {
                var user = _userService.Get(id);

                if (user == null)
                {
                    return NotFound();
                }

            _userService.Update(id, userIn);

                return NoContent();
            }

            [HttpDelete("{id:length(24)}")]
            public IActionResult Delete(string id)
            {
                var user = _userService.Get(id);

                if (user == null)
                {
                    return NotFound();
                }

            _userService.Remove(user.Id);

                return NoContent();
            }

        
        }
}
