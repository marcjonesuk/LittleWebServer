namespace LittleHttpServer
{
    public class TextResponse : Response
    {
        public override string ContentType => "text/plain; charset=utf-8";   

        public TextResponse(string body)
        {
            Body = body;
        }

        public TextResponse(int statusCode, string body)
        {
            StatusCode = statusCode;
            Body = body;
        }
    }
}
