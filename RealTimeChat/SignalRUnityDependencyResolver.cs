using System;
using Microsoft.AspNet.SignalR;
using Microsoft.Practices.Unity;
using System.Collections.Generic;

namespace RealTimeChat
{
    internal class SignalRUnityDependencyResolver : DefaultDependencyResolver
    {
        private IUnityContainer _container;

        public SignalRUnityDependencyResolver(IUnityContainer container)
        {
            _container = container;
        }

        /// <summary>
        /// Checks if Unity has a registered service first, if not it 
        /// defaults to signalr
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        public override object GetService(Type serviceType)
        {
            if (_container.IsRegistered(serviceType))
            {
                return _container.Resolve(serviceType);
            }
            return base.GetService(serviceType);
        }

        public override IEnumerable<object> GetServices(Type serviceType)
        {
            if (_container.IsRegistered(serviceType))
            {
                return _container.ResolveAll(serviceType);
            }

            return base.GetServices(serviceType);
        }
    }
}