﻿using System;
using System.Collections.Generic;
using System.Runtime.Caching;
using R.Scheduler.Contracts.DataContracts;
using R.Scheduler.Interfaces;

namespace R.Scheduler.Persistance
{
    /// <summary>
    /// InMemory implementation of IPluginStore
    /// </summary>
    public class InMemoryPluginStore : IPluginStore
    {
        private static readonly ObjectCache Cache = MemoryCache.Default;
        private CacheItemPolicy _policy; 

        /// <summary>
        /// Get registered plugin
        /// </summary>
        /// <param name="pluginName"></param>
        /// <returns></returns>
        public Plugin GetRegisteredPlugin(string pluginName)
        {
            Plugin retval = null;

            if (Cache.Contains(pluginName))
            {
                retval = Cache[pluginName] as Plugin; 
            }

            return retval;
        }

        /// <summary>
        /// Get all registered plugin
        /// </summary>
        /// <returns></returns>
        public IList<Plugin> GetRegisteredPlugins()
        {
            IList<Plugin> retval = new List<Plugin>();

            if (Cache.GetCount() > 0)
            {
                foreach (KeyValuePair<string, object> caheItem in Cache)
                {
                    retval.Add(new Plugin
                    {
                        Id = ((Plugin)caheItem.Value).Id,
                        Name = caheItem.Key,
                        AssemblyPath = ((Plugin)caheItem.Value).AssemblyPath,
                        Status = ((Plugin)caheItem.Value).Status
                    });
                }
            }

            return retval;
        }

        /// <summary>
        /// Register new plugin, or update existing one.
        /// </summary>
        /// <param name="plugin"></param>
        public void RegisterPlugin(Plugin plugin)
        {
            _policy = new CacheItemPolicy {AbsoluteExpiration = DateTimeOffset.Now.AddHours(10.00)};

            if (!Cache.Contains(plugin.Name))
            {
                plugin.Id = Guid.NewGuid();
            }

            Cache.Set(plugin.Name, plugin, _policy); 
        }

        public PluginDetails GetRegisteredPluginDetails(string pluginName)
        {
            throw new NotImplementedException();
        }
    }
}