namespace Nexus
{
    public readonly struct JsonGetRequest<T> 
        where T : JsonModel, new()
    {
        public Func<T, bool> Request { get; }

        internal JsonGetRequest(Func<T, bool> request)
        {
            Request = request;
        }

        public static implicit operator JsonGetRequest<T>(Func<T, bool> request)
        {
            return new JsonGetRequest<T>(request);
        }

        public static implicit operator Func<T, bool>(JsonGetRequest<T> request)
        {
            return request.Request;
        }
    }
}
