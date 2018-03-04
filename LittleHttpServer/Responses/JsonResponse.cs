using Newtonsoft.Json;

namespace LittleHttpServer
{
    public class JsonResponse : Response
    {
        public override string ContentType => "application/json; charset=utf-8";

        public JsonResponse(object returnValue)
        {
            Body = JsonConvert.SerializeObject(returnValue);
        }

        public JsonResponse(int statusCode, object returnValue) 
        {
            StatusCode = statusCode;
            Body = JsonConvert.SerializeObject(returnValue);
        }

        public JsonResponse(string body)
        {
            StatusCode = 200;
            Body = body;
        }

        public JsonResponse(int statusCode, string body)
        { 
            StatusCode = statusCode;
            Body = body;
        }
    }
}
