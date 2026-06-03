using System;
using VContainer;
using VContainer.Unity;

namespace Dada.Core.UI
{
    public static class UIViewInstaller
    {
        public static void RegisterUIViewManager(this IContainerBuilder builder, UIRoot uiRoot)
        {
            builder.RegisterInstance(new UIViewManager(uiRoot.RootVisualElement))
                .AsSelf()
                .AsImplementedInterfaces();
        }

        public static RegistrationBuilder RegisterUIView<TView>(this IContainerBuilder builder, Lifetime lifetime = Lifetime.Transient)
            where TView : UIView
        {
            return builder.Register<TView>(lifetime, x =>
            {
                x.AsSelf();
                x.AsImplementedInterfaces();
            });
        }

        public static RegistrationBuilder RegisterUIView<TInterface, TView>(this IContainerBuilder builder, Lifetime lifetime = Lifetime.Transient)
            where TView : class, TInterface
            where TInterface : class
        {
            return builder.Register<TInterface, TView>(lifetime);
        }

        public static void RegisterViewFactory<TView>(this IContainerBuilder builder, Lifetime lifetime = Lifetime.Transient)
            where TView : UIView
        {
            builder.Register<TView>(lifetime).AsSelf();
        }
    }
}
