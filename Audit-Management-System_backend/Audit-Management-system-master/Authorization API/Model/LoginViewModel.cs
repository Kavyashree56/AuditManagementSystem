namespace AuthorizationAPI.Model
{
    public class LoginViewModel
    {
        public string Email { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
