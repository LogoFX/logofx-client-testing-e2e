using LogoFX.Client.Testing.Contracts;
using LogoFX.Client.Testing.EndToEnd.FakeData.Shared;

namespace LogoFX.Client.Testing.EndToEnd
{
    /// <summary>
    /// Represents start application service for End-To-End tests.
    /// </summary>
    /// <seealso cref="IStartApplicationService" />
    public abstract class StartApplicationService : IStartApplicationService
    {
        /// <summary>
        /// Represents start application service for End-To-End tests which use fake data providers.
        /// </summary>
        /// <seealso cref="IStartApplicationService" />
        public class WithFakeProviders : StartApplicationService
        {
            private readonly IStartApplicationHelper _startApplicationHelper;

            /// <summary>
            /// 
            /// </summary>
            /// <param name="startApplicationHelper"></param>
            public WithFakeProviders(IStartApplicationHelper startApplicationHelper)
            {
                _startApplicationHelper = startApplicationHelper;
            }
            /// <summary>
            /// Starts the application.
            /// </summary>
            /// <param name="startupPath">The startup path.</param>
            public override void StartApplication(string startupPath)
            {
                BuildersCollectionContext.SerializeBuilders();
                _startApplicationHelper.StartApplication(startupPath);
            }
        }

        /// <summary>
        /// Represents start application service for End-To-End tests which use real data providers.
        /// </summary>
        /// <seealso cref="IStartApplicationService" />
        public class WithRealProviders : StartApplicationService
        {
            private readonly IStartApplicationHelper _startApplicationHelper;

            /// <summary>
            /// 
            /// </summary>
            /// <param name="startApplicationHelper"></param>
            public WithRealProviders(IStartApplicationHelper startApplicationHelper)
            {
                _startApplicationHelper = startApplicationHelper;
            }
            /// <summary>
            /// Starts the application.
            /// </summary>
            /// <param name="startupPath">The startup path.</param>
            public override void StartApplication(string startupPath)
            {
                _startApplicationHelper.StartApplication(startupPath);
            }
        }

        /// <summary>
        /// Starts the application.
        /// </summary>
        /// <param name="startupPath">The startup path.</param>
        public abstract void StartApplication(string startupPath);
    }
}
