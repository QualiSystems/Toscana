using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Toscana
{
    /// <summary>
    /// Common interface for esolving dependencies
    /// </summary>
    public interface IDependencyResolver
    {
        /// <summary>
        /// Returns registered type
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        object GetService(Type serviceType);

        /// <summary>
        /// Returns several registered types
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        IEnumerable<object> GetServices(Type serviceType);
        
        /// <summary>
        /// Resolves a service
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T GetService<T>();

        /// <summary>
        /// Replaces registration with a new instance
        /// </summary>
        /// <param name="instance"></param>
        /// <typeparam name="T"></typeparam>
        void Replace<T>(T instance);
    }

    /// <summary>
    /// Class that represents dependency resolver
    /// </summary>
    public class DependencyResolver
    {
        private static readonly DependencyResolver Instance = new DependencyResolver();

        private IDependencyResolver current;

        /// <summary>
        /// Cache should always be a new CacheDependencyResolver(_current).
        /// </summary>
        private CacheDependencyResolver currentCache;

        /// <summary>
        /// Default constructor of the class
        /// </summary>
        public DependencyResolver()
        {
            InnerSetResolver(new DefaultDependencyResolver());
        }

        /// <summary>
        /// Default dependency resolver or currently set 
        /// </summary>
        public static IDependencyResolver Current
        {
            get { return Instance.InnerCurrent; }
        }

        internal static IDependencyResolver CurrentCache
        {
            get { return Instance.InnerCurrentCache; }
        }

        private IDependencyResolver InnerCurrent
        {
            get { return current; }
        }

        /// <summary>
        /// Provides caching over results returned by Current.
        /// </summary>
        internal IDependencyResolver InnerCurrentCache
        {
            get { return currentCache; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resolver"></param>
        public static void SetResolver(IDependencyResolver resolver)
        {
            Instance.InnerSetResolver(resolver);
        }

        /// <summary>
        /// Sets a new dependency resolver
        /// </summary>
        /// <param name="commonServiceLocator"></param>
        public static void SetResolver(object commonServiceLocator)
        {
            Instance.InnerSetResolver(commonServiceLocator);
        }

        /// <summary>
        /// Sets a new dependency resolver
        /// </summary>
        /// <param name="getService"></param>
        /// <param name="getServices"></param>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "This is an appropriate nesting of generic types.")]
        public static void SetResolver(Func<Type, object> getService, Func<Type, IEnumerable<object>> getServices)
        {
            Instance.InnerSetResolver(getService, getServices);
        }

        private void InnerSetResolver(IDependencyResolver resolver)
        {
            if (resolver == null)
            {
                throw new ArgumentNullException(nameof(resolver));
            }

            current = resolver;
            currentCache = new CacheDependencyResolver(current);
        }

        private void InnerSetResolver(object commonServiceLocator)
        {
            if (commonServiceLocator == null)
            {
                throw new ArgumentNullException(nameof(commonServiceLocator));
            }

            Type locatorType = commonServiceLocator.GetType();
            MethodInfo getInstance = locatorType.GetMethod("GetInstance", new[] { typeof(Type) });
            MethodInfo getInstances = locatorType.GetMethod("GetAllInstances", new[] { typeof(Type) });

            if (getInstance == null ||
                getInstance.ReturnType != typeof(object) ||
                getInstances == null ||
                getInstances.ReturnType != typeof(IEnumerable<object>))
            {
                throw new ArgumentException(
                    String.Format(
                        CultureInfo.CurrentCulture,
                        @"The type {0} does not appear to implement Microsoft.Practices.ServiceLocation.IServiceLocator.",
                        locatorType.FullName),
                    nameof(commonServiceLocator));
            }

            var getService = (Func<Type, object>)Delegate.CreateDelegate(typeof(Func<Type, object>), commonServiceLocator, getInstance);
            var getServices = (Func<Type, IEnumerable<object>>)Delegate.CreateDelegate(typeof(Func<Type, IEnumerable<object>>), commonServiceLocator, getInstances);

            InnerSetResolver(new DelegateBasedDependencyResolver(getService, getServices));
        }

        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "This is an appropriate nesting of generic types.")]
        private void InnerSetResolver(Func<Type, object> getService, Func<Type, IEnumerable<object>> getServices)
        {
            if (getService == null)
            {
                throw new ArgumentNullException(nameof(getService));
            }
            if (getServices == null)
            {
                throw new ArgumentNullException(nameof(getServices));
            }

            InnerSetResolver(new DelegateBasedDependencyResolver(getService, getServices));
        }

        /// <summary>
        /// Wraps an IDependencyResolver and ensures single instance per-type.
        /// </summary>
        /// <remarks>
        /// Note it's possible for multiple threads to race and call the _resolver service multiple times.
        /// We'll pick one winner and ignore the others and still guarantee a unique instance.
        /// </remarks>
        private sealed class CacheDependencyResolver : IDependencyResolver
        {
            private readonly ConcurrentDictionary<Type, object> _cache = new ConcurrentDictionary<Type, object>();
            private readonly ConcurrentDictionary<Type, IEnumerable<object>> _cacheMultiple = new ConcurrentDictionary<Type, IEnumerable<object>>();
            private readonly Func<Type, object> _getServiceDelegate;
            private readonly Func<Type, IEnumerable<object>> _getServicesDelegate;

            private readonly IDependencyResolver _resolver;

            public CacheDependencyResolver(IDependencyResolver resolver)
            {
                _resolver = resolver;
                _getServiceDelegate = _resolver.GetService;
                _getServicesDelegate = _resolver.GetServices;
            }

            public object GetService(Type serviceType)
            {
                // Use a saved delegate to prevent per-call delegate allocation
                return _cache.GetOrAdd(serviceType, _getServiceDelegate);
            }

            public IEnumerable<object> GetServices(Type serviceType)
            {
                // Use a saved delegate to prevent per-call delegate allocation
                return _cacheMultiple.GetOrAdd(serviceType, _getServicesDelegate);
            }

            public T GetService<T>()
            {
                return (T) _cache.GetOrAdd(typeof(T), _getServiceDelegate);
            }

            public void Replace<T>(T instance)
            {
                _cache.GetOrAdd(typeof(T), t => instance);
            }
        }

        private class DefaultDependencyResolver : IDependencyResolver
        {
            [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "This method might throw exceptions whose type we cannot strongly link against; namely, ActivationException from common service locator")]
            public object GetService(Type serviceType)
            {
                // Since attempting to create an instance of an interface or an abstract type results in an exception, immediately return null
                // to improve performance and the debugging experience with first-chance exceptions enabled.
                if (serviceType.IsInterface || serviceType.IsAbstract)
                {
                    return null;
                }

                try
                {
                    return Activator.CreateInstance(serviceType);
                }
                catch
                {
                    return null;
                }
            }

            public IEnumerable<object> GetServices(Type serviceType)
            {
                return Enumerable.Empty<object>();
            }

            public T GetService<T>()
            {
                return (T) GetService(typeof(T));
            }

            public void Replace<T>(T instance)
            {
            }
        }

        private class DelegateBasedDependencyResolver : IDependencyResolver
        {
            private Func<Type, object> _getService;
            private Func<Type, IEnumerable<object>> _getServices;

            public DelegateBasedDependencyResolver(Func<Type, object> getService, Func<Type, IEnumerable<object>> getServices)
            {
                _getService = getService;
                _getServices = getServices;
            }

            [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "This method might throw exceptions whose type we cannot strongly link against; namely, ActivationException from common service locator")]
            public object GetService(Type type)
            {
                try
                {
                    return _getService.Invoke(type);
                }
                catch
                {
                    return null;
                }
            }

            public IEnumerable<object> GetServices(Type type)
            {
                return _getServices(type);
            }

            public T GetService<T>()
            {
                return (T) GetService(typeof(T));
            }

            public void Replace<T>(T instance)
            {
            }
        }
    }
}