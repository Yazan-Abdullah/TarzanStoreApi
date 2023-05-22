using FullApiOnlineStore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace TestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SharedController : ControllerBase
    {
        private readonly OnlineStoreContext _storeContext;
        public SharedController(OnlineStoreContext storeContext)
        {
            this._storeContext = storeContext;
        }
        [HttpGet]
        [Route("GetCategories")]
        public IActionResult GetAllGategory()
        {
            var cateigories = _storeContext.Categories.ToList();
            return Ok(cateigories); 
        }
        //Commtited By Jasser
        //[HttpGet]
        //[Route("GetItems")]
        //public IActionResult GetItems(int pageSize,int pageNumber) {
        //    var items = _storeContext.Items;
        //    int skipAmount = pageSize* pageNumber - (pageSize);
        //    return Ok(items.Skip(skipAmount).Take(pageSize));
        //}

        [HttpGet]
        [Route("Item/{id}")]
        public IActionResult GetItemById(int id)
        {
            var item = _storeContext.Items.Where(x => x.ItemId == id).SingleOrDefault();
            if(item != null)
            {
                return Ok(item);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        [Route("Item")]
        public IActionResult FillterItems(int pageSize, int pageNumber,int? categoryId, double? price, string? name, string? description)
        {
            //GEt All Item 
            var items = _storeContext.Items.ToList();
            if(categoryId != null)
            {
                items = items.Where(x=>x.CategoryId == categoryId).ToList();
            }
            if(price != null)
            {
                items = items.Where(x => x.Price >= price).ToList();
            }
            if(name != null)
            {
                items = items.Where(x => x.Name.Contains(name)).ToList();
            }
            if(description != null)
            {
                items = items.Where(x => x.Description.Contains(description)).ToList();
            }
            int skipAmount = pageSize * pageNumber - (pageSize);
            return Ok(items.Skip(skipAmount).Take(pageSize));
        }
    }
}
