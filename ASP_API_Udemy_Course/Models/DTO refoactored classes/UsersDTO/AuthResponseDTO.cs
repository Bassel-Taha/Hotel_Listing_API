namespace ASP_API_Udemy_Course.Models.DTO_refoactored_classes.UsersDTO
{
    public class AuthResponseDTO
    {
        public string ID { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }
        public string  RefreshToken { get; set; }
    }
}
