namespace WebStoreApi.Models.DTOS
{
    public class RegisterResponse
    {
       public UserProfileDto profile {  get; set; }
       public string Token { get; set; }
    }
}
