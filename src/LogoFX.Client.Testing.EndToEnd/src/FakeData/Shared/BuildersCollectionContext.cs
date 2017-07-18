﻿using System;
using System.Collections.Generic;
using System.Linq;
using Attest.Fake.Builders;

namespace LogoFX.Client.Testing.EndToEnd.FakeData.Shared
{    
    /// <summary>
    /// Allows to manage builders collection, including serialization/deserialization.
    /// </summary>
    public static class BuildersCollectionContext
    {
        //TODO: The file name should be scenario-specific in case of parallel End-To-End tests
        //which run in the same directory - highly unlikely and thus has low priority.
        private const string SerializedBuildersId = "SerializedBuildersCollection.Data";

        private static readonly BuildersCollection _buildersCollection = new BuildersCollection();        
        private static readonly IBuildersCollectionStorage _buildersCollectionStorage = new JsonBuildersCollectionStorage();

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
        /// Gets the builders of the specified service type.
        /// </summary>
        /// <param name="serviceType">The type of the service.</param>
        /// <returns></returns>
        public static IEnumerable<object> GetBuilders(Type serviceType)
        {
            return _buildersCollection.GetAllBuilders().Where(t => t.GetType() == serviceType);
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
            _buildersCollectionStorage.Store(_buildersCollection, SerializedBuildersId);
        }        

        /// <summary>
        /// Deserializes the builders.
        /// </summary>
        public static void DeserializeBuilders()
        {
            var data = _buildersCollectionStorage.Load(SerializedBuildersId);
            _buildersCollection.ResetBuilders(data.GetAllBuilders());            
        }
    }
}
