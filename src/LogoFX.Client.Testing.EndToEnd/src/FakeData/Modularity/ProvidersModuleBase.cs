using System;
using System.Linq;
using LogoFX.Client.Testing.EndToEnd.FakeData.Shared;
using Solid.Patterns.Builder;
using Solid.Practices.IoC;
using Solid.Practices.Modularity;
using RegistrationHelper = Attest.Fake.Registration.RegistrationHelper;

namespace LogoFX.Client.Testing.EndToEnd.FakeData.Modularity
{
    /// <summary>
    /// Base module for fake providers to be used in End-To-End tests.
    /// </summary>    
    /// <seealso cref="Solid.Practices.Modularity.ICompositionModule{IDependencyRegistrator}" />
    public abstract class ProvidersModuleBase : ICompositionModule<IDependencyRegistrator>        
    {
        /// <summary>
        /// Registers composition module.
        /// </summary>
        /// <param name="iocContainer">The ioc container.</param>
        public void RegisterModule(IDependencyRegistrator iocContainer)
        {
            BuildersCollectionContext.DeserializeBuilders();
            OnRegisterProviders(iocContainer);            
        }

        /// <summary>
        /// Override this method to register application providers.
        /// </summary>
        /// <param name="dependencyRegistrator">The dependency registrator.</param>
        protected virtual void OnRegisterProviders(IDependencyRegistrator dependencyRegistrator)
        {

        }

        /// <summary>
        /// Registers all builders of the given provider type.
        /// </summary>
        /// <typeparam name="TProvider">The type of the provider.</typeparam>
        /// <param name="iocContainerRegistrator">The dependency registrator.</param>
        /// <param name="defaultBuilderCreationFunc">The default builder creation function.</param>
        protected void RegisterAllBuilders<TProvider>(IDependencyRegistrator iocContainerRegistrator, 
            Func<IBuilder<TProvider>> defaultBuilderCreationFunc) where TProvider : class
        {
            var builders = BuildersCollectionContext.GetBuilders<TProvider>().ToArray();
            if (builders.Length == 0)
            {
                RegistrationHelper.RegisterBuilder(iocContainerRegistrator, defaultBuilderCreationFunc());
            }
            else
            {
                foreach (var builder in builders)
                {
                    RegistrationHelper.RegisterBuilder(iocContainerRegistrator, builder);
                }
            }
        }
    }
}