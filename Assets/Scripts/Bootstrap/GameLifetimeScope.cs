using System.Collections.Generic;
using GUI.Forms;
using StateMachine;
using StateMachine.States;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Bootstrap
{
    /// <summary>
    /// Регистрирукем зависимости : ui билдер, стейты, точку входа в приложение
    /// </summary>
    public class GameLifetimeScope : LifetimeScope
    {
        [SerializeField] private UIFormLoader _formLoader;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(_formLoader).As<IUIFormLoader>();

            builder.Register<SplashState>(Lifetime.Singleton);
            builder.Register<LoadState>(Lifetime.Singleton);
            builder.Register<MenuState>(Lifetime.Singleton);

            builder.Register<IStatesController<AppState>>(resolver =>
            {
                var map = new Dictionary<AppState, IState>
                {
                    [AppState.Splash] = resolver.Resolve<SplashState>(),
                    [AppState.Load]   = resolver.Resolve<LoadState>(),
                    [AppState.Menu]   = resolver.Resolve<MenuState>(),
                };
                return new StatesController<AppState>(map);
            }, Lifetime.Singleton);

            builder.RegisterEntryPoint<AppEntryPoint>();
        }
    }
}
