namespace LittleHttpServer
{
    public class ResponseEvaluator
    {
        public Response Evaluate(object i)
        {
            if (i == null)
                return new TextResponse(string.Empty);

            if (i is Response r)
                return r;

            if (i is string s)
                return new TextResponse(s);

            return new JsonResponse(i);
        }
    }
}
