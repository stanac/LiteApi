using System.Collections.Generic;

namespace LiteApi.Contracts.Models
{
    /// <summary>
    /// Contains data that can be added to an object during runtime
    /// </summary>
    public class AdditionalData
    {
        private Dictionary<string, object> _additionalDataContainer = new Dictionary<string, object>();

        /// <summary>
        /// Sets the additional data.
        /// </summary>
        /// <typeparam name="T">Type of value</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void SetAdditionalData<T>(string key, T value) => _additionalDataContainer[key] = value;

        /// <summary>
        /// Gets additional data or default in data with provided key is not set
        /// </summary>
        /// <typeparam name="T">Type of data</typeparam>
        /// <param name="key">Key of data</param>
        /// <returns>Additional data if set, otherwise default of T</returns>
        public T GetAdditionalDataOrDefault<T>(string key)
        {
            object data;
            if (_additionalDataContainer.TryGetValue(key, out data))
            {
                return (T)data;
            }
            return default(T);
        }
    }
}
