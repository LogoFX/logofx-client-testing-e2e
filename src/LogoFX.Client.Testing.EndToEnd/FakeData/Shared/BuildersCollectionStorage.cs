using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Solid.Practices.Composition;

namespace LogoFX.Client.Testing.EndToEnd.FakeData.Shared
{
    /// <summary>
    /// Enables storing and loading builders collection.
    /// </summary>
    public interface IBuildersCollectionStorage
    {
        /// <summary>
        /// Stores the specified builders collection at the specified resource identifier.
        /// </summary>
        /// <param name="buildersCollection">The builders collection.</param>
        /// <param name="id">The resource identifier.</param>
        void Store(BuildersCollection buildersCollection, string id);

        /// <summary>
        /// Loads the builders collection using the specified resource identifier.
        /// </summary>
        /// <param name="id">The resource identifier.</param>
        /// <returns></returns>
        BuildersCollection Load(string id);
    }    

    /// <summary>
    /// Enables storing and loading builders collection using JSON format.
    /// </summary>
    /// <seealso cref="IBuildersCollectionStorage" />
    public class JsonBuildersCollectionStorage : IBuildersCollectionStorage
    {
        /// <summary>
        /// Stores the specified builders collection.
        /// </summary>
        /// <param name="buildersCollection">The builders collection.</param>
        /// <param name="id">The identifier.</param>
        public void Store(BuildersCollection buildersCollection, string id)
        {
            var serializerSettings = CreateSerializerSettings();
            var str = JsonConvert.SerializeObject(buildersCollection, serializerSettings);
            PlatformProvider.Current.WriteText(id, str);
        }

        /// <summary>
        /// Loads the builders collection using the specified resource identifier.
        /// </summary>
        /// <param name="id">The resource identifier.</param>
        /// <returns></returns>
        public BuildersCollection Load(string id)
        {                        
            var str = PlatformProvider.Current.ReadText(id);
            var serializerSettings = CreateSerializerSettings();
            return JsonConvert.DeserializeObject<BuildersCollection>(str, serializerSettings);
        }

        private static JsonSerializerSettings CreateSerializerSettings()
        {
            return new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
                ContractResolver = new FieldsContractResolver()
            };
        }
    }

    internal sealed class FieldsContractResolver : DefaultContractResolver
    {
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            var jsonProperties = type.GetRuntimeFields().Select(f => CreateProperty(f, memberSerialization)).ToList();

            foreach (var p in jsonProperties)
            {
                p.Writable = true;
                p.Readable = true;
            }

            return jsonProperties;
        }
    }
}
