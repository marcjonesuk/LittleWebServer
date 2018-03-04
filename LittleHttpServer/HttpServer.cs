using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LittleHttpServer
{
    public class HttpServer
    {
        Router router = new Router();
        ResponseEvaluator responseEvaluator = new ResponseEvaluator();
        HttpListener Listener = new HttpListener();

        public void Post<TIn>(string v, Func<Request<TIn>, dynamic> p)
        {
            Add("POST", v, p);
        }

        public HttpServer(string baseUrl)
        {
            Start(baseUrl);
        }

        private void Start(string baseUrl)
        {
            if (!baseUrl.EndsWith("/"))
                baseUrl += "/";

            Listener.Prefixes.Add(baseUrl);
            Listener.Start();
            Listen();
        }

        private void Add<TIn, TOut>(string method, string path, Func<Request<TIn>, TOut> handler)
        {
            AddAsync<TIn, TOut>(method, path, p => Task.FromResult(handler(p)));
        }

        private void AddAsync<TOut>(string method, string path, Func<Task<TOut>> handler)
        {
            router.AddRoute(method, path, async (body) =>
            {
                var result = await handler();
                var response = responseEvaluator.Evaluate(result);
                return response;
            });
        }


        //private void AddAsync<TOut>(string method, string path, Func<Task> handler)
        //{
        //    router.AddRoute(method, path, async (body) =>
        //    {
        //        var result = await handler();
        //        var response = responseEvaluator.Evaluate(result);
        //        return response;
        //    });
        //}

        private void AddAsync<T, TOut>(string method, string path, Func<Request<T>, Task<TOut>> handler)
        {
            router.AddRoute(method, path, async (req) =>
            {
                var payload = JsonConvert.DeserializeObject<T>(req.Body);
                var request = new Request<T>(payload, req.Params, req.QueryString, req.Headers);
                var result = await handler(request);
                var response = responseEvaluator.Evaluate(result);
                return response;
            });
        }

        public void Get(string path, Func<Request<dynamic>, object> handler)
        {
            Add("GET", path, handler);
        }

        public void Get<T>(string path, Func<Request<T>, object> handler)
        {
            Add("GET", path, handler);
        }

        public void Get(string path, Func<dynamic> handler)
        {
            Add("GET", path, handler);
        }

        public void Get(string path, Func<Task<object>> handler)
        {
            AddAsync("GET", path, handler);
        }

        public void Get<T>(string path, Func<Request<T>, Task<object>> handler)
        {
            AddAsync("GET", path, handler);
        }

        public void Get(string path, Func<Request<object>, Task<object>> handler)
        {
            AddAsync("GET", path, handler);
        }

        private void Add<TOut>(string method, string path, Func<TOut> handler)
        {
            AddAsync(method, path, () => Task.FromResult(handler()));
        }

        private async void Listen()
        {
            while (true)
            {
                var context = await Listener.GetContextAsync();
                ThreadPool.QueueUserWorkItem((a) => ProcessRequest(context));
            }
            Listener.Close();
        }

        public void Stop()
        {

        }

        private async void ProcessRequest(HttpListenerContext context)
        {
            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response;
            try
            {
                //strip query string
                var path = request.RawUrl.Split('?')[0];
                var r = await router.Invoke(request.HttpMethod, 
                    path, 
                    () => new StreamReader(context.Request.InputStream).ReadToEnd(), 
                    request.QueryString,
                    request.Headers);

                if (r != null)
                {
                    response.StatusCode = r.StatusCode;
                    response.ContentType = r.ContentType;
                    response.Headers = r.Headers;

                    if (r.Body != null)
                    {
                        byte[] buffer = Encoding.UTF8.GetBytes(r.Body);
                        response.ContentLength64 = buffer.Length;
                        Stream output = response.OutputStream;
                        output.Write(buffer, 0, buffer.Length);
                    }
                    response.OutputStream.Close();
                }
                else
                {
                    response.StatusCode = 404;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                response.OutputStream.Close();
            }
        }
    }

    public class FileServer
    {

    }
}
