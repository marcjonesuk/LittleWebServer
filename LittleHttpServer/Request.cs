using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace LittleHttpServer
{
    public class Request<T>
    {
        public T Body { get; }
        public dynamic Params { get; }
        public dynamic QueryString { get; }

        public IDictionary<string, string> Headers
        {
            get
            {
                if (headersDictionary == null)
                {
                    headersDictionary = new Dictionary<string, string>();
                    foreach (string key in headers)
                    {
                        headersDictionary[key] = headers[key];
                    }
                }
                return headersDictionary;
            }
        }

        public Authorization Authorization
        {
            get
            {
                try
                {
                    var headerValue = TryGetHeaderValue("Authorization");
                    if (headerValue != null)
                    {
                        var t = headerValue.Split(' ');
                        byte[] data = Convert.FromBase64String(t[1]);
                        var userPass = Encoding.UTF8.GetString(data).Split(':');
                        return new Authorization(t[0], userPass[0], userPass[1]);
                    }
                    return null;
                }
                catch
                {
                    return null;
                }
            }
        }

        private string TryGetHeaderValue(string name)
        {
            string value;
            Headers.TryGetValue(name, out value);

            if (value != null)
                return value;

            name = name.ToLower();
            foreach (var kvp in Headers)
            {
                if (kvp.Key.ToLower() == name)
                {
                    return Headers[kvp.Key];
                }
            }
            return null;
        }

        private NameValueCollection headers;
        private Dictionary<string, string> headersDictionary = null;

        public Request(T body, dynamic p, dynamic queryString, NameValueCollection requestHeaders)
        {
            Body = body;
            Params = p;
            QueryString = queryString;
            headers = requestHeaders;
        }
    }
}
