namespace TestApi.DTO
{
    public class UserListInformation
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }


        public UserListInformation(string name, string email, string phone)
        {
            Name = name;
            Email = email;
            Phone = phone;
        }
    }
}
