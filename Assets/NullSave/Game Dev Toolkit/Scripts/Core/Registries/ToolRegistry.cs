using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace NullSave.GDTK
{
    /// <summary>
    /// Static registry for storing and retrieving components
    /// </summary>
    public class ToolRegistry
    {

        #region Fields

        private static List<ToolRegistryEntry> entries;
        private static Scene lastScene;

        #endregion

        #region Public Methods

        /// <summary>
        /// Clear all registered components
        /// </summary>
        public static void Clear()
        {
            if (entries == null)
            {
                entries = new List<ToolRegistryEntry>();
            }
            else
            {
                entries.Clear();
            }
        }

        /// <summary>
        /// Get registered component
        /// </summary>
        /// <typeparam name="T">Type of component to return</typeparam>
        /// <returns></returns>
        public static T GetComponent<T>()
        {
            EnsureSceneAndList();

            foreach (ToolRegistryEntry entry in entries)
            {
                if (entry.Object is T t)
                {
                    return t;
                }
            }

            return default;
        }

        /// <summary>
        /// Get registered component with key
        /// </summary>
        /// <typeparam name="T">Type of component to return</typeparam>
        /// <returns></returns>
        public static T GetComponent<T>(string key)
        {
            if (string.IsNullOrEmpty(key)) return GetComponent<T>();

            EnsureSceneAndList();

            foreach (ToolRegistryEntry entry in entries)
            {
                if (entry.Object is T t && entry.Key == key)
                {
                    return t;
                }
            }

            return default;
        }

        /// <summary>
        /// Get list of registered components
        /// </summary>
        /// <typeparam name="T">Type of components to return</typeparam>
        /// <returns></returns>
        public static List<T> GetComponents<T>()
        {
            EnsureSceneAndList();

            List<T> results = new List<T>();
            foreach (ToolRegistryEntry entry in entries)
            {
                if (entry.Object is T t)
                {
                    results.Add(t);
                }
            }

            return results;
        }

        /// <summary>
        /// Add a component to registry
        /// </summary>
        /// <param name="component"></param>
        public static void RegisterComponent(object component, bool persistBetweenScenes = false)
        {
            RegisterComponent(component, null, persistBetweenScenes);
        }

        /// <summary>
        /// Add a component to registry
        /// </summary>
        /// <param name="component"></param>
        public static void RegisterComponent(object component, string key, bool persistBetweenScenes = false)
        {
            EnsureSceneAndList();

            foreach (ToolRegistryEntry entry in entries)
            {
                if (entry.Object == component && entry.Key == key) return;
            }

            entries.Add(new ToolRegistryEntry { Object = component, Key = key, Persist = persistBetweenScenes });
        }

        /// <summary>
        /// Remove a component from registry
        /// </summary>
        /// <param name="component"></param>
        public static void RemoveComponent(object component)
        {
            EnsureSceneAndList();

            foreach (ToolRegistryEntry entry in entries)
            {
                if (entry.Object == component)
                {
                    entries.Remove(entry);
                    return;
                }
            }
        }

        /// <summary>
        /// Remove all components using the specified key
        /// </summary>
        public static void RemoveComponentsByKey(string key)
        {
            List<ToolRegistryEntry> removeList = new List<ToolRegistryEntry>();
            foreach(ToolRegistryEntry entry in entries)
            {
                if(entry.Key == key)
                {
                    removeList.Add(entry);
                }
            }

            foreach(ToolRegistryEntry entry in removeList)
            {
                entries.Remove(entry);
            }
        }

        #endregion

        #region Private Methods

        private static void EnsureSceneAndList()
        {
            if (entries == null) entries = new List<ToolRegistryEntry>();

            Scene activeScene = SceneManager.GetActiveScene();
            if (lastScene != activeScene)
            {
                lastScene = activeScene;

                List<ToolRegistryEntry> persistedEntries = new List<ToolRegistryEntry>();
                foreach(ToolRegistryEntry entry in entries)
                {
                    if(entry.Persist)
                    {
                        persistedEntries.Add(entry);
                    }
                }

                entries.Clear();
                entries.AddRange(persistedEntries);
            }
        }

        #endregion

    }
}