using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LostFramework{
    public class TaskScheduler : MonoBehaviour, IScheduler
    {
        public SchedulerHandle AddTask(Action action)
        {
            throw new NotImplementedException();
        }

        public void RemoveTask(Action action)
        {
            throw new NotImplementedException();
        }

        public void RemoveTask(SchedulerHandle schedulerHandle)
        {
            throw new NotImplementedException();
        }
    }
}
