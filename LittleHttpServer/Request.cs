namespace LittleHttpServer
{
    public class Request<T>
    {
        public T Body { get; }
        public dynamic Params { get; }
        public dynamic QueryString { get; }

        public Request(T body, dynamic p, dynamic queryString)
        {
            Body = body;
            Params = p;
            QueryString = queryString;
        }
    }
}
