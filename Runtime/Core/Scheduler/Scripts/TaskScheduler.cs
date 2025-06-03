using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LostFramework
{
    /// <summary>
    /// 任务调度器
    /// </summary>
    public class TaskScheduler : MonoBehaviour
    {
        private List<Action> _tasks = new List<Action>();

        private Dictionary<int, Action> _taskDict = new Dictionary<int, Action>();
        private List<int> removeList = new List<int>();
        private bool _running = false;

        /// <summary>
        /// 限制每帧执行时间
        /// </summary>
        private float maxTime = 1 / 60f;


        /// <summary>
        /// 限制每帧执行时间,有效时间必须大于0
        /// </summary>
        public float MaxTime
        {
            get => maxTime; 
            set{
                if (maxTime > 0)
                {
                    maxTime = value;
                }
            }
        }

        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public SchedulerHandle AddTask(Action action)
        {
            _tasks.Add(action);
            int id = action.GetHashCode();
            _taskDict.Add(id, action);
            _running = true;
            return new SchedulerHandle(id);
        }

        /// <summary>
        /// 移除任务
        ///</summary>
        /// <param name="action"></param>
        public void RemoveTask(Action action)
        {
            removeList.Add(action.GetHashCode());
        }

        /// <summary>   
        /// 移除任务
        /// </summary>
        /// <param name="handle"></param>
        public void RemoveTask(SchedulerHandle handle)
        {
            removeList.Add(handle.ID);
        }

        private void Update()
        {
            // 移除任务
            if (removeList.Count > 0)
            {
                foreach (int actionID in removeList)
                {
                    if (_taskDict.ContainsKey(actionID))
                    {
                        _tasks.Remove(_taskDict[actionID]);
                        _taskDict.Remove(actionID);
                    }
                }
                removeList.Clear();
            }

            // 限制每帧执行时间
            if (_running)
            {
                float startTime = Time.realtimeSinceStartup;
                while (_tasks.Count > 0 && (Time.realtimeSinceStartup - startTime) < MaxTime)
                {
                    Action task = _tasks[0];
                    _tasks.RemoveAt(0);
                    task.Invoke();
                }

                // 任务执行完毕
                if (_tasks.Count == 0)
                {
                    _running = false;
                }
            }
        }


    }
}

public struct SchedulerHandle
{
    private int _id;
    /// <summary>
    /// 任务索引
    /// </summary>
    public int ID { get { return _id; } }

    public SchedulerHandle(int id)
    {
        _id = id;
    }
}

