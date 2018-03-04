namespace LittleHttpServer
{
    public class UnauthorizedResponse : Response
    {
        public UnauthorizedResponse()
        {
            StatusCode = 401;
        }
    }

    public class InternalServerErrorResponse : Response
    {
        public InternalServerErrorResponse()
        {
            StatusCode = 500;
        }
    }
}