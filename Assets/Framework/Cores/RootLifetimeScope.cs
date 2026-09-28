using VContainer;
using VContainer.Unity;

namespace Dada.Cores
{
    public class RootLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            InjectServices(builder);
        }

        private void InjectServices(IContainerBuilder builder)
        {
            builder.Register<AudioService>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<SerializationService>(Lifetime.Singleton).AsImplementedInterfaces();
        }
    }
}
