using UnityEngine;

public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    private static bool _isLoaded = false;

    public static T Instance
    {
        get
        {
            if (_isLoaded) return _instance;
            _instance = FindObjectOfType<T>();
            _isLoaded = _instance != null;
            return _instance;
        }
    }

    protected virtual void Awake()
    {
        T[] instances = FindObjectsOfType<T>();
        foreach (T instance in instances)
        {
            if (instance != this)
            {
                Destroy(this.gameObject);
            }
        }

        DontDestroyOnLoad(this);
    }
}