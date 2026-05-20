using System.Collections.Generic;
using Dada.Foundations;
using UnityEngine;

namespace Dada.Cores
{
    public class ConfigManager : MonoSingleton<ConfigManager>
    {
        [SerializeField] private List<Cores.Config> configs = new();
        
        private Dictionary<string, Config> _configDict;

        protected override void OnAwake()
        {
            base.OnAwake();
            _configDict = new Dictionary<string, Config>();
            foreach (var config in configs)
            {
                _configDict.Add(config.configName, config);
            }
        }

        public Config GetConfig(string typeName)
        {
            return _configDict.GetValueOrDefault(typeName);
        }

        public T GetConfig<T>() 
            where T : Config
        {
            return _configDict.GetValueOrDefault(typeof(T).Name) as T;
        }
    }
}

