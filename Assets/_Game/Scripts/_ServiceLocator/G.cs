using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Game.ServiceLocator
{
    public enum ServiceLifetime
    {
        Singleton,
        Transient
    }
    
    public static class G 
    {
        private static List<IService> _services = new List<IService>();
        private static Dictionary<Type, IService> _servicesByType = new Dictionary<Type, IService>();
        private static Dictionary<IService, ServiceLifetime> _lifeTimeService = new Dictionary<IService, ServiceLifetime>();

        private static Dictionary<Type, IService> _wasIntedService = new Dictionary<Type, IService>();

        public static void InitializeServices()
        {
            foreach (var service in _services)
            {
                if (_wasIntedService.ContainsKey(service.GetType()))
                {
                    continue;
                }
                
                if (_lifeTimeService[service] == ServiceLifetime.Singleton)
                {
                    _wasIntedService.Add(service.GetType(), service);
                }
                
                service.Initialize();
            }
        }

        public static void InstantiateAndRegisterService<T>(ServiceLifetime lifetime, string prefabName = "") where T: Component, IService
        {
            if (_servicesByType.ContainsKey(typeof(T)))
            {
                Debug.LogWarning($"Service {typeof(T)} already exists!");
                return;
            }
            
            var prefabAsset = Resources.Load<T>(prefabName);
            T prefab;
            
            if (prefabAsset == null)
            {
                prefab = new GameObject(typeof(T).ToString()).AddComponent<T>();
            }
            else
            {
                prefab = Object.Instantiate(prefabAsset);
            }
            
            if (lifetime == ServiceLifetime.Singleton)
            {
                Object.DontDestroyOnLoad(prefab.gameObject);
            }
            
            Register(prefab, lifetime);
        }
        
        public static T Get<T>()
        {
            if (_servicesByType.TryGetValue(typeof(T), out IService service))
            {
                return (T)service;
            }
            
            Debug.LogWarning($"Service {typeof(T)} not found!");
            return default;
        }
        
        public static void Register<T>(T service, ServiceLifetime lifetime) where T : IService
        {
            Type type = typeof(T);
            
            if (_servicesByType.ContainsKey(type))
            {
                Debug.LogWarning($"Service {type} already registered!");
                return;
            }
            
            _lifeTimeService.Add(service, lifetime);
            _services.Add(service);
            _servicesByType.Add(type, service);
        }

        public static void Unregister<T>() where T : IService
        {
            Type type = typeof(T);
            
            if (!_servicesByType.TryGetValue(type, out IService service))
            {
                Debug.LogWarning($"Service {type} not registered!");
                return;
            }
            
            if (service is IDisposable disposable)
            {
                disposable.Dispose();
            }
            
            _services.Remove(service);
            _servicesByType.Remove(type);
            _lifeTimeService.Remove(service);
        }
        
        public static void UnregisterAllDestroyed()
        {
            for (int i = _services.Count - 1; i >= 0; i--)
            {
                IService service = _services[i];
                
                if (service == null || service.Equals(null))
                {
                    _services.RemoveAt(i);
                    _servicesByType.Remove(service.GetType());
                    _lifeTimeService.Remove(service);
                    continue;
                }

                if (_lifeTimeService[service] == ServiceLifetime.Transient)
                {
                    if (service is MonoBehaviour monoBehaviour)
                    {
                        Object.Destroy(monoBehaviour.gameObject);
                    }
                    else if (service is IDisposable disposable)
                    {
                        disposable.Dispose();
                    }
                    
                    _services.RemoveAt(i);
                    _servicesByType.Remove(service.GetType());
                    _lifeTimeService.Remove(service);
                }
            }
        }
    }
}