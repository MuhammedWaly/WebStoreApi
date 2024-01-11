namespace WebStoreApi.Models.DTOS
{
    public class LoginResponse
    {

        public UserProfileDto profile { get; set; }
        public string Token { get; set; }
        public string? Message { get; set; }
        
    }
}
