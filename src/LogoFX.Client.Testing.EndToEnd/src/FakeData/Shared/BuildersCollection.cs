using System.Collections.Generic;
using System.Linq;
using Attest.Fake.Builders;

namespace LogoFX.Client.Testing.EndToEnd.FakeData.Shared
{
    /// <summary>
    /// Represents builders collection.
    /// </summary>    
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