using System;
using System.Collections.Generic;
using System.Linq;

namespace Toscana.Engine
{
    internal class SimpleIocContainer
    {
        private readonly Dictionary<Type, Func<object>> registrations = new Dictionary<Type, Func<object>>();

        public void Register<TService, TImpl>() where TImpl : TService
        {
            registrations[typeof (TService)] = () => GetInstance(typeof (TImpl));
        }

        public void Register<TService>(Func<TService> instanceCreator)
        {
            registrations[typeof (TService)] = () => instanceCreator();
        }

        public void RegisterSingleton<TService>(TService instance)
        {
            var type = typeof (TService);
            if (!registrations.ContainsKey(type))
            {
                registrations.Add(type, () => instance);
            }
            else
            {
                registrations[type] = () => instance;
            }
        }

        public void RegisterSingleton<TService>(Func<TService> instanceCreator)
        {
            var lazy = new Lazy<TService>(instanceCreator);
            Register(() => lazy.Value);
        }

        public object GetInstance(Type serviceType)
        {
            Func<object> creator;
            if (registrations.TryGetValue(serviceType, out creator)) return creator();
            if (!serviceType.IsAbstract) return CreateInstance(serviceType);
            throw new InvalidOperationException("No registration for " + serviceType);
        }

        public T GetInstance<T>()
        {
            Func<object> creator;
            var serviceType = typeof (T);
            if (registrations.TryGetValue(serviceType, out creator)) return (T) creator();
            if (!serviceType.IsAbstract) return (T) CreateInstance(serviceType);
            throw new InvalidOperationException("No registration for " + serviceType);
        }

        private object CreateInstance(Type implementationType)
        {
            var ctor = implementationType.GetConstructors().Single();
            var parameterTypes = ctor.GetParameters().Select(p => p.ParameterType);
            var dependencies = parameterTypes.Select(t => GetInstance(t)).ToArray();
            return Activator.CreateInstance(implementationType, dependencies);
        }

        public T GetService<T>()
        {
            return GetInstance<T>();
        }

        public void Replace<T>(T instance)
        {
            Register(() => instance);
        }
    }
}