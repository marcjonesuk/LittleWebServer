namespace LittleHttpServer
{
    public class Request
    {
        public string Body { get; }
        public dynamic Params { get; }
        public dynamic QueryString { get; }

        public Request()
        {
        }
        public Request(string body, dynamic p, dynamic qs)
        {
            Body = body;
            Params = p;
            QueryString = qs;
        }
    }
}
