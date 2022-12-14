namespace Nexus
{
    public readonly struct BsonCreateRequest<T>
        where T : BsonModel, new()
    {
        public Action<T> Definition { get; }

        internal BsonCreateRequest(Action<T> definition)
        {
            Definition = definition;
        }

        public static implicit operator BsonCreateRequest<T>(Action<T> definition)
        {
            return new(definition);
        }

        public static implicit operator Action<T>(BsonCreateRequest<T> definition)
        {
            return definition.Definition;
        }
    }
}
