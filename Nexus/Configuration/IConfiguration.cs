using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Nexus
{
    public interface IConfiguration
    {
        private static readonly Lazy<JsonSerializerOptions> _defaultOptions = new(new JsonSerializerOptions()
        {
            WriteIndented = true,
            ReadCommentHandling = JsonCommentHandling.Skip,
            AllowTrailingCommas = true
        });

        /// <summary>
        ///     The path where this file should be saved.
        /// </summary>
        public string FilePath { get; }

        public static T Load<T>(JsonSerializerOptions? options = null)
            where T : IConfiguration, new()
        {
            options ??= _defaultOptions.Value;

            var obj = new T();

            if (!File.Exists(obj.FilePath))
                return Create<T>(options);

            using var stream = File.OpenRead(obj.FilePath);

            var result = JsonSerializer.Deserialize<T>(stream, options);

            result ??= obj;

            return result;
        }

        public static async Task<T> LoadAsync<T>(JsonSerializerOptions? options = null, CancellationToken cancellationToken = default)
            where T : IConfiguration, new()
        {
            options ??= _defaultOptions.Value;

            var obj = new T();

            if (!File.Exists(obj.FilePath))
                return await CreateAsync<T>(options, cancellationToken);

            using var stream = File.OpenRead(obj.FilePath);

            var result = await JsonSerializer.DeserializeAsync<T>(stream, options, cancellationToken);

            result ??= obj;

            return result;
        }

        public static T Create<T>(JsonSerializerOptions? options = null)
            where T : IConfiguration, new()
        {
            options ??= _defaultOptions.Value;

            var obj = new T();

            Directory.CreateDirectory(obj.FilePath);

            using var stream = File.CreateText(obj.FilePath);

            JsonSerializer.Serialize<T>(stream.BaseStream, obj, options);

            return obj;
        }

        public static async Task<T> CreateAsync<T>(JsonSerializerOptions? options = null, CancellationToken cancellationToken = default)
            where T : IConfiguration, new()
        {
            options ??= _defaultOptions.Value;

            var obj = new T();

            Directory.CreateDirectory(obj.FilePath);

            await SaveAsync(obj, options, cancellationToken);

            return obj;
        }

        public static bool Save<T>(T value, JsonSerializerOptions? options = null)
            where T : IConfiguration, new()
        {
            options ??= _defaultOptions.Value;

            using var stream = File.CreateText(value.FilePath);

            try
            {
                JsonSerializer.Serialize<T>(stream.BaseStream, value, options);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static async Task<bool> SaveAsync<T>(T value, JsonSerializerOptions? options = null, CancellationToken cancellationToken = default)
            where T : IConfiguration, new()
        {
            options ??= _defaultOptions.Value;

            using var stream = File.CreateText(value.FilePath);
            try
            {
                await JsonSerializer.SerializeAsync<T>(stream.BaseStream, value, options, cancellationToken);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
