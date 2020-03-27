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
       
        [Route("api")]
        [ApiController]
        public class UsersController : ControllerBase
        {
            private readonly UserService _userService;

            public UsersController(UserService userService)
            {
            _userService = userService;
            }

        [HttpGet("admin")]
        public ActionResult<List<User>> Get()
        {
            var currentUser = HttpContext.User;

            

            if(currentUser.HasClaim(user => user.Type == "Rank" && user.Value == "Admin"))
            {
                List<User> users = new List<User>();
                return users = _userService.Get();
            }
            else 
            {
                return BadRequest("Not permission");
            }

        }
        // /api/admin/set-rank/1321313133?rank=Member
        [HttpPut("admin/set-rank/{id:length(24)}")]
        public IActionResult SetRank(string id, string rank)
        {
            var user = _userService.Get(id);
            if (user == null)
            {
                return NotFound();
            }

            if(rank != "Guest" && rank !="Member" && rank != "Admin")
            {
                return BadRequest("Incorrect rank");
            }

            _userService.SetRank(id, rank, user);

            return NoContent();
        }

        [HttpPut("admin/ban/{id:length(24)}")]
        public IActionResult BanById(string id)
        {
            var user = _userService.Get(id);
            string rank = "Banned";
            if (user == null)
            {
                return NotFound();
            }

            if (user.Rank == rank)
            {
                return BadRequest("This user is already banned");
            }

            _userService.SetRank(id, rank, user);

            return NoContent();
        }
        // /api/admin/ban?nickname=1231231
        [HttpPut("admin/ban")]
        public IActionResult BanByNickname(string nickname)
        {
            var user = _userService.GetByNickname(nickname);
            string rank = "Banned";
            if (user == null)
            {
                return NotFound();
            }

            if (user.Rank == rank)
            {
                return BadRequest("This user is already banned");
            }

            _userService.SetRank(user.Id, rank, user);

            return NoContent();
        }
        
        [HttpPost("users/register")]
        public ActionResult<User> Create(User user)
        {
            if (_userService.CheckNickname(user.Nickname) == true)
            {
                return BadRequest(new { message = "Nickname is taken" });

            }
            else if(_userService.CheckEmail(user.Email) == true)
            {
                return BadRequest(new { message = "Email is taken" });
            }
            else
            {
                _userService.Create(user);
                return CreatedAtRoute("GetUser", new { id = user.Id.ToString() }, user);
               
            }
            
        }

        [AllowAnonymous]
        [HttpPost("users/auth")]
        public IActionResult Authenticate([FromBody]AuthenticateModel model)
        {
            var user = _userService.Authenticate(model.Username, model.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(user);
        }


        [HttpGet("users/{id:length(24)}", Name = "GetUser")]
        public ActionResult<User> Get(string id)
        {
            var user = _userService.Get(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }



        [HttpPut("users/{id:length(24)}")]
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

            [HttpDelete("users/{id:length(24)}")]
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
