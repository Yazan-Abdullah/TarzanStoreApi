using FullApiOnlineStore.DTO;
using FullApiOnlineStore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestApi.DTO;


namespace TestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly OnlineStoreContext _storeContext;
        public UserController(OnlineStoreContext storeContext)
        {
            this._storeContext = storeContext;
        }
        [HttpGet]
        [Route("AddToCart/{itemId}/{qtn}/{userId}/{note}")]
        public IActionResult AddItemToCart(int itemId, int qtn, int userId, string note)
        {
            if (itemId > 0 && qtn > 0 && userId > 0)
            {
                var cart = _storeContext.Carts.Where(x => x.UserId == userId && x.IsActive == true).SingleOrDefault();
                if (cart != null)
                {
                    var item = _storeContext.Items.Where(x => x.ItemId == itemId).SingleOrDefault();
                    if (item != null)
                    {
                        var IsExistitcartItem = _storeContext.CartItems
                            .Where(y => y.ItemId == itemId && y.CartId == cart.CartId).SingleOrDefault();
                        if (IsExistitcartItem == null)
                        {
                            CartItem cartItem = new CartItem();
                            cartItem.ItemId = itemId;
                            cartItem.Note = note;
                            cartItem.CartId = cart.CartId;
                            cartItem.Qtn = qtn;
                            cartItem.NetPrice = item.Price * qtn;
                            _storeContext.Add(cartItem);
                            _storeContext.SaveChanges();
                        }
                        else
                        {
                            IsExistitcartItem.Qtn += qtn;
                            _storeContext.Update(IsExistitcartItem);
                            _storeContext.SaveChanges();
                        }

                    }

                }
                else
                {
                    var user = _storeContext.Users.Where(x => x.UserId == userId).SingleOrDefault();
                    if (user != null)
                    {
                        Cart cart1 = new Cart();
                        cart1.UserId = userId;
                        cart1.IsActive = true;
                        _storeContext.Add(cart1);
                        _storeContext.SaveChanges();
                        var cartNew = _storeContext.Carts.Where(x => x.UserId == userId && x.IsActive == true).SingleOrDefault();
                        if (cartNew != null)
                        {
                            var item = _storeContext.Items.Where(x => x.ItemId == itemId).SingleOrDefault();
                            if (item != null)
                            {
                                var IsExistitcartItem = _storeContext.CartItems
                                    .Where(y => y.ItemId == itemId && y.CartId == cartNew.CartId).SingleOrDefault();
                                if (IsExistitcartItem == null)
                                {
                                    CartItem cartItem = new CartItem();
                                    cartItem.ItemId = itemId;
                                    cartItem.Note = note;
                                    cartItem.CartId = cartNew.CartId;
                                    cartItem.Qtn = qtn;
                                    cartItem.NetPrice = item.Price * qtn;
                                    _storeContext.Add(cartItem);
                                    _storeContext.SaveChanges();
                                }
                                else
                                {
                                    IsExistitcartItem.Qtn += qtn;
                                    _storeContext.Update(IsExistitcartItem);
                                    _storeContext.SaveChanges();
                                }

                            }

                        }
                    }
                }
                return Ok("Added");
            }
            // check if the user have an active cart 

            return BadRequest("");
        }

        [HttpPut]
        [Route("RemoveFromCart/{cartItemId}")]
        public IActionResult RemoveFromCart(int cartItemId)
        {
            var cartItem = _storeContext.CartItems.Where(x => x.CartItemId == cartItemId).SingleOrDefault();
            if (cartItem != null)
            {
                var Cart = _storeContext.Carts.Where(x => x.CartId == cartItem.CartId).SingleOrDefault();
                if (Cart != null && Cart.IsActive == true)
                {
                    if (cartItem.Qtn == 1)
                    {
                        _storeContext.Remove(cartItem);
                        _storeContext.SaveChanges();
                    }
                    else
                    {
                        cartItem.Qtn -= 1;
                        cartItem.NetPrice = _storeContext.Items.Where(x => x.ItemId == cartItem.ItemId).First().Price * cartItem.Qtn;
                        _storeContext.Update(cartItem);
                        _storeContext.SaveChanges();
                    }
                    return Ok("Removed Item");
                }
            }
            else
            {
                return NotFound();
            }
            return NotFound();
        }

        [HttpDelete]
        [Route("RemoveCartItem/{cartItemId}")]
        public IActionResult RemoveCartItem(int cartItemId)
        {
            var cartItem = _storeContext.CartItems.Where(x => x.CartItemId == cartItemId).SingleOrDefault();
            if (cartItem != null)
            {
                var Cart = _storeContext.Carts.Where(x => x.CartId == cartItem.CartId).SingleOrDefault();
                if (Cart != null && Cart.IsActive == true)
                {
                    _storeContext.Remove(cartItem);
                    _storeContext.SaveChanges();
                    return Ok("Removed Item");
                }
                else
                {
                    return NotFound();
                }

            }
            return NotFound();
        }

        [HttpPost]
        [Route("CheckOutOrder")]
        public IActionResult CreateNewOrder(OrderDTO order)
        {
            var cart = _storeContext.Carts.Where(x => x.CartId == order.CartId && x.IsActive == true).SingleOrDefault();
            if(cart != null)
            {
                if(order.DelivaryDate.AddDays(-2).AddMinutes(1) > DateTime.Now)
                {
                    cart.IsActive = false;
                    _storeContext.Update(cart);
                    _storeContext.SaveChanges();

                    Order order1 = new Order();
                    order1.CartId = order.CartId;
                    order1.DeliveryDate = order.DelivaryDate;
                    order1.OrderDate = DateTime.Now;
                    order1.IsApproved = false;
                    order1.Note = order.Note;
                    order1.TotalPrice = _storeContext.CartItems.Where(x => x.CartId == order.CartId).Sum(x => x.NetPrice).Value;
                    _storeContext.Add(order1);
                    _storeContext.SaveChanges();
                    return Ok("order created");
                }
                else
                {
                    return BadRequest();
                }
            }
            return BadRequest();
        }
        [HttpGet]
        [Route("GetOrderDetils/{OrderId}")]
        public IActionResult GetOrderDetils(int OrderId)
        {
            var order = _storeContext.Orders.Where(x => x.OrderId == OrderId).SingleOrDefault();
            if(order != null)
            {
                OrderDetilesForUser orderDetiles = new OrderDetilesForUser();
                orderDetiles.OrderDate = order.OrderDate.ToString();
                orderDetiles.Note = order.Note;
                orderDetiles.TotalPrice = order.TotalPrice.ToString();
                orderDetiles.DeliveryDate = order.DeliveryDate.ToString();
                orderDetiles.IsApproved = order.IsApproved == true ? "Approved" : order.IsApproved == false ? "Rejected" : "UnCompleteOrder";
                orderDetiles.OrderStatus = _storeContext.OrderStatuses.Where(x => x.OrderStatusId == order.StatusId).First().Name;
                var cart = _storeContext.Carts.Where(x => x.CartId == order.CartId).ToList();
                var cartitem = _storeContext.CartItems.Where(x => x.CartId == order.CartId).ToList();
                var item = _storeContext.Items.ToList();
                var orderItems = from c in cart
                                 join cit in cartitem
                                 on c.CartId equals cit.CartId
                                 join it in item
                                 on cit.ItemId equals it.ItemId
                                 select new OrderCatrItemDTO
                                 {
                                     Id = it.ItemId.ToString(),
                                     Name = it.Name,
                                     Price = it.Price.ToString(),
                                     Quantity = cit.Qtn.ToString(),
                                     NetPrice = cit.NetPrice.ToString()
                                 };
                orderDetiles.MyCart = orderItems.ToList();
                return Ok(orderDetiles);
            }
            else
            {
                return NotFound();
            }

            
        }
        [HttpGet]
        [Route("CheckOrderStetus/{OrderId}")]
        public IActionResult CheckOrderStetus(int Id)
        {
            var order = _storeContext.Orders.Where(x => x.OrderId == Id).SingleOrDefault();
            if(order != null)
            {
                return Ok(_storeContext.OrderStatuses.Where(x => x.OrderStatusId == order.OrderId).First().Name);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
