using System.Text.Json;

namespace Nexus
{
    public sealed class LocalCollectionBase<T>
        where T : new()
    {
        public string Path { get; }

        public JsonSerializerOptions SerializerOptions { get; }

        /// <summary>
        ///     Represents the locally stored values in question.
        /// </summary>
        public List<T> Values { get; set; }

        public LocalCollectionBase(string path, JsonSerializerOptions? options = null)
        {
            Values = new()!;

            Path = path;
            SerializerOptions = options ?? new();

            ReadAsync()
                .GetAwaiter()
                .GetResult();
        }

        public void Read()
        {
            if (!File.Exists(Path))
            {
                Create();
                return;
            }

            using var stream = File.OpenRead(Path);

            var obj = JsonSerializer.Deserialize<List<T>>(stream, SerializerOptions);

            if (obj is null)
                throw new ArgumentNullException("Returned json was received in an invalid state.", nameof(obj));

            Values = obj;
        }

        public async ValueTask ReadAsync(CancellationToken cancellationToken = default)
        {
            if (!File.Exists(Path))
            {
                await CreateAsync(cancellationToken);
                return;
            }

            using var stream = File.OpenRead(Path);

            var obj = await JsonSerializer.DeserializeAsync<List<T>>(stream, SerializerOptions, cancellationToken);

            if (obj is null)
                throw new ArgumentNullException("Returned json was received in an invalid state.", nameof(obj));

            Values = obj;
        }

        public void Create()
        {
            Directory.CreateDirectory(Path);

            Values = new();

            Save();
        }

        public async ValueTask CreateAsync(CancellationToken cancellationToken = default)
        {
            Directory.CreateDirectory(Path);

            Values = new();

            await SaveAsync(cancellationToken);
        }

        public void Save()
        {
            using var stream = File.OpenWrite(Path);

            JsonSerializer.Serialize(stream, Values, SerializerOptions);
        }

        public async ValueTask SaveAsync(CancellationToken cancellationToken = default)
        {
            using var stream = File.OpenWrite(Path);

            await JsonSerializer.SerializeAsync(stream, Values, SerializerOptions, cancellationToken);
        }
    }
}
