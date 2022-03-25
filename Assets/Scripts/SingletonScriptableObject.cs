using UnityEngine;

public abstract class SingletonScriptableObject<T> : ScriptableObject where T : ScriptableObject
{
    static T instance;
    
    public static T Instance {
        get {
            if (instance == null)
            {
                instance = Resources.Load<T>(typeof(T).ToString());
            }
            return instance;
        }
    }
}
