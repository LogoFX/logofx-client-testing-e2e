using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Attest.Fake.Builders;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace LogoFX.Client.Testing.EndToEnd.FakeData.Shared
{
    internal sealed class FieldsContractResolver : DefaultContractResolver
    {
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            var fileds = type.GetRuntimeFields().Select(f => CreateProperty(f, memberSerialization));
            var propList = fileds.ToList();


            foreach (var p in propList)
            {
                p.Writable = true;
                p.Readable = true;
            }

            return propList;
        }
    }

    /// <summary>
    /// Allows to manage builders collection, including serialization/deserialization.
    /// </summary>
    public static class BuildersCollectionContext
    {
        //TODO: The file name should be scenario-specific in case of parallel End-To-End tests
        //which run in the same directory - highly unlikely and thus has low priority.
        private const string SerializedBuildersPath = "SerializedBuildersCollection.Data";

        private static readonly BuildersCollection _buildersCollection = new BuildersCollection();

        /// <summary>
        /// Gets the builders of the specified service type.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <returns></returns>
        public static IEnumerable<FakeBuilderBase<TService>> GetBuilders<TService>() where TService : class
        {
            return _buildersCollection.GetBuilders<TService>();
        }

        /// <summary>
        /// Adds the builder of the specified service type.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="builder">The builder.</param>
        public static void AddBuilder<TService>(FakeBuilderBase<TService> builder) where TService : class
        {
            _buildersCollection.AddBuilder(builder);
        }

        /// <summary>
        /// Serializes the builders.
        /// </summary>
        public static void SerializeBuilders()
        {
            var jss = new JsonSerializerSettings();
            jss.TypeNameHandling = TypeNameHandling.All;
            var dcr = new FieldsContractResolver();
            jss.ContractResolver = dcr;

            var str = JsonConvert.SerializeObject(_buildersCollection, jss);
            var fileStream = new FileStream(SerializedBuildersPath, FileMode.Create);
            var textWritter = new StreamWriter(fileStream);
            textWritter.Write(str);
            textWritter.Close();
        }

        /// <summary>
        /// Deserializes the builders.
        /// </summary>
        public static void DeserializeBuilders()
        {
            var fileStream = new FileStream(SerializedBuildersPath, FileMode.Open);
            var textReader = new StreamReader(fileStream);
            var str = textReader.ReadToEnd();

            var jss = new JsonSerializerSettings();
            jss.TypeNameHandling = TypeNameHandling.All;
            var dcr = new FieldsContractResolver();
            jss.ContractResolver = dcr;
            var data = JsonConvert.DeserializeObject<BuildersCollection>(str, jss);
            _buildersCollection.ResetBuilders(data.GetAllBuilders());
            textReader.Close();
        }
    }

    /// <summary>
    /// Represents builders collection.
    /// </summary>
    [Serializable]    
    public class BuildersCollection
    {
        private readonly List<object> _allBuilders = new List<object>();

        internal IEnumerable<FakeBuilderBase<TService>> GetBuilders<TService>() where TService : class
        {
            return _allBuilders.OfType<FakeBuilderBase<TService>>();
        }

        internal IEnumerable<object> GetAllBuilders()
        {
            return _allBuilders;
        }

        internal void AddBuilder<TService>(FakeBuilderBase<TService> builder) where TService : class
        {
            _allBuilders.Add(builder);
        }

        internal void ResetBuilders(IEnumerable<object> builders)
        {
            _allBuilders.Clear();
            _allBuilders.AddRange(builders);
        }
    }
}
