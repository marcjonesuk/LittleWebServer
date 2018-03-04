namespace LittleHttpServer
{
    public class Authorization
    {
        public string Type { get; }
        public string Username { get; }
        public string Password { get; }

        public Authorization(string type, string username, string password)
        {
            Type = type;
            Username = username;
            Password = password;
        }
    }
}
