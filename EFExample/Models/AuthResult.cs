namespace EFExample.Models
{
    public class AuthResult
    {
        public string Token { get; set; }
        public bool Result { get; set; }
        public List<String> error { get; set; }
    }
}
