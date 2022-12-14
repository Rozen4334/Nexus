namespace Nexus
{
    public readonly struct JsonCreateRequest<T> 
        where T : JsonModel, new()
    {
        public Action<T> Definition { get; }

        internal JsonCreateRequest(Action<T> definition)
        {
            Definition = definition;
        }

        public static implicit operator JsonCreateRequest<T>(Action<T> definition)
        {
            return new(definition);
        }

        public static implicit operator Action<T>(JsonCreateRequest<T> definition)
        {
            return definition.Definition;
        }
    }
}
