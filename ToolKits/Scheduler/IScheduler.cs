
// 定义 IScheduler 接口
using System;
namespace LostFramework{
    public interface IScheduler
    {
        SchedulerHandle AddTask(Action action);
        void RemoveTask(Action action);
        void RemoveTask(SchedulerHandle schedulerHandle);
    }

    public struct SchedulerHandle{
        public string Id;
    }
}
