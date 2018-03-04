namespace LittleHttpServer
{
    public class HtmlResponse : Response
    {
        public override string ContentType => "text/html; charset=utf-8";

        public HtmlResponse()
        {
            StatusCode = 200;
        }

        public HtmlResponse(int statusCode, string body)
        {
            StatusCode = statusCode;
            Body = body;
        }

        public HtmlResponse(string body)
        {
            Body = body;
        }
    }
}
