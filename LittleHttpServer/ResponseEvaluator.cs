namespace LittleHttpServer
{
    public class ResponseEvaluator
    {
        public Response Evaluate(object i)
        {
            if (i == null)
                return new FileResponse(string.Empty);

            if (i is Response r)
                return r;

            if (i is string s)
                return new FileResponse(s);

            return new JsonResponse(i);
        }
    }
}
