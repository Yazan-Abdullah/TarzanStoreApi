using FullApiOnlineStore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TestApi.DTO;

namespace FullApiOnlineStore.Controlers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdmenController : ControllerBase
    {
        private readonly OnlineStoreContext _onlineStore;
        public AdmenController (OnlineStoreContext onlineStore)
        {
            this._onlineStore = onlineStore;
        }
        [HttpGet]
        [Route("GetUserInformaion")]
        public IActionResult GetUserInformaion()
        {
            var users = _onlineStore.Users.Where(x => x.UserTypeId == 1).ToList();
            List<UserListInformation> ListofUser = new List<UserListInformation>();
            return Ok();
        }
    }
}
