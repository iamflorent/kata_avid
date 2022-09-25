namespace WebApi.ApiModels
{
    public class AuthenticateResponse
    {
        public string Token { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
    }
}
