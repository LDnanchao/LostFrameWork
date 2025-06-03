
using UnityEngine;

namespace LostFramework{

    public class GlobalTaskScheduler{
        private static TaskScheduler _instance;
        public static TaskScheduler Instance{
            get{
                if(_instance == null){
                    GameObject go = new GameObject("GlobalTaskScheduler");
                    _instance = go.AddComponent<TaskScheduler>();
                    GameObject.DontDestroyOnLoad(_instance.gameObject);
                }
                return _instance;
            }
        }
    }
}