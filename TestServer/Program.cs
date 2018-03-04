using LittleHttpServer;
using System;
using System.Threading.Tasks;

namespace TestServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var http = new HttpServer("http://localhost:50000/api");

            //http.Get("/")

            http.Get("/test/{Id}/{X}", (request) =>
            {
                var auth = request.Authorization;

                if (auth == null || auth.Password != "xxx")
                    return new UnauthorizedResponse();

                return new HtmlResponse("test")
                {
                    CacheControl = "max-age=360",
                    ETag = Guid.NewGuid().ToString()
                };
            });

            var r = new Response();



            //var x = new UnauthorizedResponse();


            //.WithBody("test")
            //.WithContentType("application/json")
            //.WithCookie("key", "value")
            //.WithHeader("", "");



            //http.Get("/test/{Id}.jpg", (req) => new FileResponse($"/{req.Params}.jpg"));




            //http.Get<string>("/health/{test}", req => Console.WriteLine(req.Params["test"]));

            Console.ReadLine();
        }
    }
}
