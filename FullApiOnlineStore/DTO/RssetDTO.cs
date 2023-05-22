namespace TestApi.DTO
{
    public class ResetDTO
    {
        public string UserName { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfermPassword { get; set; }
    }
}
