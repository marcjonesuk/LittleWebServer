using LittleHttpServer;
using System;

namespace TestServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var http = new HttpServer("http://localhost:50000/api");


            //http.Get("/test/{Id}", () =>
            //{
            //    return Response.Html("test");
            //});

            //http.Get("/test/{Id}.jpg", (req) => new FileResponse($"/{req.Params}.jpg"));




            //http.Get<string>("/health/{test}", req => Console.WriteLine(req.Params["test"]));

            Console.ReadLine();
        }
    }
}
