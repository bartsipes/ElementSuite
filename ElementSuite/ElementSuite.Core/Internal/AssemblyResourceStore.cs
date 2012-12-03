using ElementSuite.Common.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace ElementSuite.Core.Internal
{
    internal class AssemblyResourceStore : IResourceStore
    {
        IsolatedStorageFile _isoFile;
        IsolatedStorageFileStream _backingFile;
        Dictionary<string, object> store;
        ILoggingService logger;
        Type evidence;
        bool disposed = false;

        internal AssemblyResourceStore(Type evidence)
        {
            this.evidence = evidence;
            logger = Initializer.Instance.WindsorContainer.Resolve<ILoggingService>();
            _isoFile = IsolatedStorageFile.GetMachineStoreForAssembly();
            _backingFile = new IsolatedStorageFileStream(evidence.Assembly.GetName().Name, FileMode.OpenOrCreate, _isoFile);
            if (_backingFile.Length > 0)
            {
                try
                {
                    var formatter = new BinaryFormatter();
                    store = (Dictionary<string, object>)formatter.Deserialize(_backingFile);
                }
                catch (Exception ex)
                {
                    logger.Log(Common.LogLevel.Error, string.Format("Error deserializing resource store for {0}. Resetting resource store.", evidence.Assembly.GetName().Name));
                    logger.Log(Common.LogLevel.Debug, string.Format("Deserialize error: {0}", ex.Message));
                    store = new Dictionary<string, object>();
                }
            }
            else
            {
                store = new Dictionary<string, object>();
            }
        }

        public int? GetInt(string key)
        {
            if (store.ContainsKey(key))
            {
                var value = store[key];
                if (value.GetType() == typeof(int))
                {
                    return (int)value;
                }
                else
                {
                    throw new ArrayTypeMismatchException(string.Format("Expected value with type of Int32. Found value with type of {0} instead.", value.GetType().Name));
                }
            }
            else
            {
                return (int?)null;
            }
        }

        public float? GetFloat(string key)
        {
            if (store.ContainsKey(key))
            {
                var value = store[key];
                if (value.GetType() == typeof(float))
                {
                    return (float)value;
                }
                else
                {
                    throw new ArrayTypeMismatchException(string.Format("Expected value with type of Single. Found value with type of {0} instead.", value.GetType().Name));
                }
            }
            else
            {
                return (float?)null;
            }
        }

        public string GetString(string key)
        {
            if (store.ContainsKey(key))
            {
                var value = store[key];
                if (value.GetType() == typeof(string))
                {
                    return (string)value;
                }
                else
                {
                    throw new ArrayTypeMismatchException(string.Format("Expected value with type of String. Found value with type of {0} instead.", value.GetType().Name));
                }
            }
            else
            {
                return null;
            }
        }

        public void SetInt(string key, int value)
        {
            if (store.ContainsKey(key))
            {
                store[key] = value;
            }
            else
            {
                store.Add(key, value);
            }
        }

        public void SetFloat(string key, float value)
        {
            if (store.ContainsKey(key))
            {
                store[key] = value;
            }
            else
            {
                store.Add(key, value);
            }
        }

        public void SetString(string key, string value)
        {
            if (store.ContainsKey(key))
            {
                store[key] = value;
            }
            else
            {
                store.Add(key, value);
            }
        }

        public void Dispose()
        {
            logger.Log(Common.LogLevel.Debug, "AssemblyResourceStore.Dispose Called");
            if (!disposed)
            {
                disposed = true;
                try
                {
                    var formatter = new BinaryFormatter();
                    formatter.Serialize(_backingFile, store);
                    _backingFile.Close();
                }
                catch (Exception ex)
                {
                    logger.Log(Common.LogLevel.Error, string.Format("There was a problem serializing the reference store for the \"{0}\" assembly.", evidence.AssemblyQualifiedName), ex);
                }
            }
        }
    }
}
