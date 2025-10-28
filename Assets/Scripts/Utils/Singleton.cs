using System;
using UnityEngine;

namespace Utils
{
    public abstract class Singleton<T> where T : class, new()
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                _instance ??= new T();
                return _instance;
            }
        }
    }
    
    public abstract class SingletonObj<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                _instance = FindFirstObjectByType<T>();
                
                if (_instance == null)
                {
                    var go = new GameObject(typeof(T).Name);
                    _instance = go.AddComponent<T>();
                }
                
                return _instance;
            }
        }

        protected virtual void Awake()
        {
            _instance = null;
            if (_instance == null)
            {
                _instance = this as T;
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }

        private void OnDisable()
        {
            _instance = null;
        }
    }
}
