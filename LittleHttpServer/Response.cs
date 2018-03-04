using System;

namespace LittleHttpServer
{
    public abstract class Response
    {
        public int StatusCode { get; protected set; }
        public string Body { get; protected set; }
        public abstract string ContentType { get; }

        public Response()
        {
            StatusCode = 200;
        }

        public static FileResponse File(string v)
        {

        }

        public static JsonResponse Json(string json)
        {
            return new JsonResponse(json);
        }
    }
}
