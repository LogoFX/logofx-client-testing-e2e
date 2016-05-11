using System;
using System.Linq;
using Attest.Fake.Builders;
using LogoFX.Client.Testing.EndToEnd.FakeData.Shared;
using Solid.Practices.IoC;
using Solid.Practices.Modularity;
using RegistrationHelper = Attest.Fake.Registration.RegistrationHelper;

namespace LogoFX.Client.Testing.EndToEnd.FakeData.Modularity
{
    /// <summary>
    /// Base module for fake providers to be used in End-To-End tests.
    /// </summary>    
    /// <seealso cref="Solid.Practices.Modularity.ICompositionModule{IIocContainerRegistrator}" />
    public abstract class ProvidersModuleBase : ICompositionModule<IIocContainerRegistrator>        
    {
        /// <summary>
        /// Registers composition module into ioc container.
        /// </summary>
        /// <param name="iocContainer">The ioc container.</param>
        public void RegisterModule(IIocContainerRegistrator iocContainer)
        {
            BuildersCollectionContext.DeserializeBuilders();
            OnRegisterProviders(iocContainer);            
        }

        /// <summary>
        /// Override this method to register application providers into the ioc container.
        /// </summary>
        /// <param name="iocContainerRegistrator">The ioc container registrator.</param>
        protected virtual void OnRegisterProviders(IIocContainerRegistrator iocContainerRegistrator)
        {

        }

        /// <summary>
        /// Registers all builders of the given provider type into the ioc container.
        /// </summary>
        /// <typeparam name="TProvider">The type of the provider.</typeparam>
        /// <param name="iocContainerRegistrator">The ioc container registrator.</param>
        /// <param name="defaultBuilderCreationFunc">The default builder creation function.</param>
        protected void RegisterAllBuilders<TProvider>(IIocContainerRegistrator iocContainerRegistrator, 
            Func<FakeBuilderBase<TProvider>> defaultBuilderCreationFunc) where TProvider : class
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