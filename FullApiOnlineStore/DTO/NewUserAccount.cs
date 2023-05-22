using TestApi.Models;

namespace TestApi.DTO
{
    public class NewUserAccount
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
