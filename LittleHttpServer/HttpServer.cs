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

        private void AddAsync<T, TOut>(string method, string path, Func<Request<T>, Task<TOut>> handler)
        {
            router.AddRoute(method, path, async (req) =>
            {
                var payload = JsonConvert.DeserializeObject<T>(req.Body);
                var request = new Request<T>(payload, req.Params, req.QueryString);
                var result = await handler(request);
                var response = responseEvaluator.Evaluate(result);
                return response;
            });
        }

        public void Get(string path, Func<Request<dynamic>, dynamic> handler)
        {
            Add("GET", path, handler);
        }

        public void Get<T>(string path, Func<Request<T>, dynamic> handler)
        {
            Add("GET", path, handler);
        }

        public void Get<T>(string path, Action<Request<T>> handler)
        {
            Add<T, object>("GET", path, (req) => { handler(req); return null; });
        }

        public void Get(string path, Func<dynamic> handler)
        {
            Add("GET", path, handler);
        }

        public void Get<TOut>(string path, Func<Task<TOut>> handler)
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

        private async void ProcessRequest(HttpListenerContext context)
        {
            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response;
            try
            {
                //strip query string
                var path = request.RawUrl.Split('?')[0];
                var r = await router.Invoke(request.HttpMethod, path, () => new StreamReader(context.Request.InputStream).ReadToEnd(), request.QueryString);

                if (r != null)
                {
                    byte[] buffer = Encoding.UTF8.GetBytes(r.Body);
                    response.ContentLength64 = buffer.Length;
                    Stream output = response.OutputStream;
                    output.Write(buffer, 0, buffer.Length);
                    output.Close();
                }

                if (r == null)
                    response.StatusCode = 404;
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
