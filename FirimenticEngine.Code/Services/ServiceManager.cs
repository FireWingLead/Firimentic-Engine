using System;
using System.Collections.Generic;

namespace FirimenticEngine.Services
{
    public class ServiceManager
    {
        private static volatile ServiceManager sharedInstance;
        private static object initKey = new object();

        static ServiceManager() { }


        private Dictionary<Type, IService> services = new Dictionary<Type, IService>();


        private ServiceManager() {

        }


        /// <summary>
        /// Gets the application's ServiceManager.
        /// </summary>
        public static ServiceManager SharedInstance { get { Load(); return sharedInstance; } }

        /// <summary>
        /// Creates and loads the SharedInstance if it has not already been loaded. This method is thread safe.
        /// </summary>
        private static void Load() {
            if (sharedInstance == null) {
                lock (initKey) {
                    if (sharedInstance == null)
                        sharedInstance = new ServiceManager();
                }
            }
        }


        /// <summary>
        /// Gets the instance of ServiceType that was registered by the client.
        /// </summary>
        /// <typeparam name="ServiceType">The type of the IService interface needed by the application. Must inherit IService.</typeparam>
        /// <returns></returns>
        public static ServiceType GetService<ServiceType>() where ServiceType : IService {
            Load();
            return (ServiceType)sharedInstance.services[typeof(ServiceType)];
        }

        /// <summary>
        /// Registers an instance of a service for the appplication to use. All services in the FirimenticEngine.Services
        /// namespace must be implemented and registered by the client.
        /// </summary>
        /// <param name="serviceInterfaceType">The System.Type of the IService interface being implemented by the client service.</param>
        /// <param name="serviceInstance">The instance of the service that the application should use.</param>
        public static void RegisterSingleton(Type serviceInterfaceType, Func<IService> serviceInstance) {
            Load();
            if (!sharedInstance.services.ContainsKey(serviceInterfaceType))
                sharedInstance.services.Add(serviceInterfaceType, serviceInstance());
        }
    }
}
