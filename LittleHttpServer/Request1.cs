using System.Collections.Generic;
using System.Collections.Specialized;

namespace LittleHttpServer
{
    public class Request
    {
        public string Body { get; }
        public dynamic Params { get; }
        public dynamic QueryString { get; }
        public NameValueCollection Headers { get; }

        public Request()
        {
        }
        public Request(string body, dynamic p, dynamic qs, NameValueCollection headers)
        {
            Body = body;
            Params = p;
            QueryString = qs;
            Headers = headers;
        }
    }
}
