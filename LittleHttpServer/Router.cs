using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Dynamic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LittleHttpServer
{
    public class Router
    {
        private Dictionary<string, Dictionary<string, Func<Request, Task<Response>>>> asyncHandlers = new Dictionary<string, Dictionary<string, Func<Request, Task<Response>>>>();

        public void AddRoute(string method, string path, Func<Request, Task<Response>> handler)
        {
            method = method.ToUpper();

            var groups = Regex.Matches(path, "({.*?})");
            foreach (Match m in groups)
            {
                path = path.Replace(m.Value, $"(?<{m.Value.Replace("{", "").Replace("}", "")}>.*)");
            }

            if (!asyncHandlers.ContainsKey(method))
                asyncHandlers[method] = new Dictionary<string, Func<Request, Task<Response>>>();

            asyncHandlers[method][path] = handler;
        }

        public async Task<Response> Invoke(string method, string path, Func<string> bodyFn, NameValueCollection queryStringValues)
        {
            method = method.ToUpper();
            Match match = null;

            Func<Request, Task<Response>> handler = null;

            if (asyncHandlers.ContainsKey(method))
            {
                foreach (var k in asyncHandlers[method].Keys)
                {
                    match = Regex.Match(path, k);
                    if (match.Success)
                    {
                        handler = asyncHandlers[method][k];
                        break;
                    }
                }

                if (handler != null)
                {
                    var body = bodyFn();

                    var p = new ExpandoObject();
                    var dictionary = (IDictionary<string, object>)p;

                    for (var i = 1; i < match.Groups.Count; i++)
                    {
                        var item = match.Groups[i];
                        dictionary[(string)item.GetType().GetProperty("Name").GetValue(item)] = item.Value; //temp hack
                    }

                    ExpandoObject qs = null;
                    if (queryStringValues.HasKeys())
                    {
                        qs = new ExpandoObject();
                        var qsDictionary = (IDictionary<string, object>)qs;
                        foreach (string item in queryStringValues.Keys)
                        {
                            qsDictionary[item] = queryStringValues[item];
                        }
                    }

                    var req = new Request(body, p, qs);
                    var resp = await handler(req);

                    return resp;
                }
            }

            return null;
        }
    }
}
