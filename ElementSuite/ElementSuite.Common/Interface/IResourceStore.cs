using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElementSuite.Common.Interface
{
    /// <summary>
    /// Provides an assembly specific vehicle for storing basic values in key value pairs.
    /// </summary>
    public interface IResourceStore : IDisposable
    {
        /// <summary>
        /// Retrieve an integer value using the given key. Null will be returned if the value does not exist or could not be retrieved.
        /// </summary>
        /// <param name="key">The key to identify the integer value.</param>
        /// <returns></returns>
        int? GetInt(string key);

        /// <summary>
        /// Retrieve an float value using the given key. Null will be returned if the value does not exist or could not be retrieved.
        /// </summary>
        /// <param name="key">The key to identify the float value.</param>
        /// <returns></returns>
        float? GetFloat(string key);

        /// <summary>
        /// Retrieve an string value using the given key. Null will be returned if the value does not exist or could not be retrieved.
        /// </summary>
        /// <param name="key">The key to identify the string value.</param>
        /// <returns></returns>
        string GetString(string key);

        /// <summary>
        /// Sets an integer value using the given key.
        /// </summary>
        /// <param name="key">Key to identify the value.</param>
        /// <param name="value">The integer to be stored.</param>
        void SetInt(string key, int @value);

        /// <summary>
        /// Sets a float value using the given key.
        /// </summary>
        /// <param name="key">Key to identify the value.</param>
        /// <param name="value">The decimal value to be stored.</param>
        void SetFloat(string key, float @value);

        /// <summary>
        /// Sets a string value using the given key.
        /// </summary>
        /// <param name="key">Key to identify the value.</param>
        /// <param name="value">The string value to be stored.</param>
        void SetString(string key, string @value);
    }
}
