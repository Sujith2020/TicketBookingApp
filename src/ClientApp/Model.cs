
namespace ClientApp
{
    public static class Model
    {
        public class Login
        {
            public string UserName { get; set; }
            public string Password { get; set; }
        }

        public class SecurityToken
        {
            public string auth_token { get; set; }
        }


    }
}
