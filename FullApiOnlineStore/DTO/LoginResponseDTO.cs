using TestApi.Models;

namespace TestApi.DTO
{
    public class LoginResponseDTO
    {
        public int UserID { get; set; }
        public string UserName { get; set; }

        public string UserType { get; set; }

        public int loginId { get; set; }

        public List<Cart> myOrders { get; set; }
    }
}
