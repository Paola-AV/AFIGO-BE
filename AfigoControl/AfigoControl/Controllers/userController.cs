using AfigoControl.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AfigoControl.Controllers

{
    public class userController : ControllerBase
    {

        private readonly UserDbContext _userDbContext;

        public userController(UserDbContext userDbContext) {

            _userDbContext = userDbContext;
        }

        [HttpGet]
        [Route("GetUser")]

        public async Task<IEnumerable<User>> GetUsers() 
        {
            return await _userDbContext.Users.ToListAsync();

        }


        [HttpPost]
        [Route("AddUser")]

        public async Task<User> AddUser(User objUser)
        {
            _userDbContext.Users.Add(objUser);
            await _userDbContext.SaveChangesAsync();
            return objUser;
        }


        [HttpPatch]
        [Route("UpdateUser/{id}")]

        public async Task<User> UpdateUser(User objUser)
        {
            await _userDbContext.SaveChangesAsync();
            return objUser;
        }

        [HttpDelete]
        [Route("DeleteUser/{id}")]

        public bool DeleteUser(int id)
        { 
            bool a = false;
            var user = _userDbContext.Users.Find(id);

            if(user != null)
            { 
                a=true;
                _userDbContext.Entry(user).State = EntityState.Modified;
                _userDbContext.SaveChanges();
            }
            else{ 

                a=false;    
            
            }

            return a;   
        }
       


    }
}
