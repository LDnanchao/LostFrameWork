using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogKit 
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Init()
    {
        GameObject go = new GameObject("LogKit");
        go.AddComponent<LogToFile>();
    }

}

