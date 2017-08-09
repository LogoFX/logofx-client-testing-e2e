using LogoFX.Client.Testing.Contracts;
using LogoFX.Client.Testing.EndToEnd.FakeData.Shared;
using Solid.Patterns.Builder;

namespace LogoFX.Client.Testing.EndToEnd.FakeData
{   
    /// <summary>
    /// Represents builder registration service for End-To-End tests.
    /// </summary>
    /// <seealso cref="IBuilderRegistrationService" />
    public class BuilderRegistrationService : IBuilderRegistrationService
    {
        /// <summary>
        /// Registers the builder into ioc container.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="builder">The builder.</param>
        public void RegisterBuilder<TService>(IBuilder<TService> builder) where TService : class
        {
            BuildersCollectionContext.AddBuilder(builder);
        }
    }
}
