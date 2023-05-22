using FullApiOnlineStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestApi.DTO;

namespace TestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IndexController : ControllerBase
    {
        private readonly OnlineStoreContext _storeContext;
        public IndexController(OnlineStoreContext storeContext)
        {
            this._storeContext = storeContext;
        }
        [HttpPost]
        [Route("Register")]
        public IActionResult CreateAccount([FromBody] NewUserAccount newUser)
        {
            User user = new User();
            user.UserTypeId = 1;
            user.Name = newUser.Name;
            user.Email = newUser.Email;
            user.Phone = newUser.Phone;
            _storeContext.Add(user);
            _storeContext.SaveChanges();
            Login login = new Login();
            login.Username = newUser.Email;
            login.Password = newUser.Password;
            login.UserId = _storeContext.Users.Where(x => x.Email == newUser.Email).OrderByDescending
                (x => x.UserId).First().UserId;
            _storeContext.Add(login);
            _storeContext.SaveChanges();
            return Ok();
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login([FromBody] LoginDTO login)
        {
            var userLoginInfo = _storeContext.Logins.Where(x => x.Username == login.Email
            && x.Password == login.Password).SingleOrDefault();
            if (userLoginInfo == null)
            {
                // unauth
                return Unauthorized("Either Username or Password is incorrect ");
            }
            else
            {
                //Update Login 
                userLoginInfo.IsActive = true;
                _storeContext.Update(userLoginInfo);
                _storeContext.SaveChanges();
               
                var user = _storeContext.Users.Where(x => x.UserId == userLoginInfo.UserId).FirstOrDefault();
                LoginResponseDTO respon = new LoginResponseDTO();
                respon.UserID = user.UserId;
                respon.UserName = user.Email;
                respon.UserType = _storeContext.UserTypes.Where(x=>x.UserTypeId==user.UserTypeId).First().Name;
                respon.loginId = _storeContext.Logins.Where(x => x.UserId == user.UserId).First().LoginId;
                //respon.myOrders = _storeContext.Carts.Where(x => x.UserId == user.UserId).ToList();

                return Ok(respon);
            }
            
        }

        [HttpPut]
        [Route("ResetPassword")]
        public IActionResult UpdatePassword([FromBody] ResetDTO reset)
        {
            var check = _storeContext.Logins.FirstOrDefault(x => x.Username == reset.UserName && x.Password == reset.OldPassword);
            if (check != null)
            {
                if(reset.NewPassword == reset.ConfermPassword)
                {
                    check.Password = reset.ConfermPassword;
                    _storeContext.Update(reset);
                    _storeContext.SaveChanges();
                    return Ok(reset);
                }
            }
            return Ok("invaled Username or password");
        }
        [HttpPut]
        [Route("ForgitPassword")]
        public IActionResult FrogitPassword([FromBody] forgetPasswordDTO forget)
        {
            var check = _storeContext.Logins.FirstOrDefault(x => x.Username == forget.userName && x.Password == forget.newPassword);
            if (check != null)
            {
                if (forget.confirmPassword == forget.newPassword)
                {
                    check.Password = forget.confirmPassword;
                    _storeContext.Update(forget);
                    _storeContext.SaveChanges();
                    return Ok(forget);
                }
            }
            return Ok("invaled Username or password");
        }
        [HttpDelete]
        [Route("ReomveAccount/{Id}")]
        public IActionResult DeleteAccount(int Id) {
        var account = _storeContext.Logins.Where(y=>y.UserId == Id).SingleOrDefault();
            if(account != null)
            {
                _storeContext.Remove(account);
                _storeContext.SaveChanges();
                var user = _storeContext.Users.Where(y => y.UserId == Id).SingleOrDefault();
                if (user != null)
                {
                    _storeContext.Remove(user);
                    _storeContext.SaveChanges();
                    return Ok("User Removed Successfully");
                }

            }
            return Ok("Account Is Not Exisit");
        }

        //[HttpGet]
        //[Route("[action]")]
        //public IActionResult GetUserTypesWithStoredProcudure()
        //{
        //    using (var context = _storeContext)
        //    {
        //        var usertypes = context.UserType.FromSqlRaw<UserTypes>("EXECUTE dbo.testSQL2").ToList();
        //        return Ok(usertypes);
        //    }
        //}
    }
}
