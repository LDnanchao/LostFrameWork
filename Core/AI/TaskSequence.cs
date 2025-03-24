using System.Collections;
using System.Collections.Generic;

namespace Lost.AI
{

    public enum TaskSequenceStatus
    {
        /// <summary>
        /// 等待
        /// </summary>
        Waiting,
        /// <summary>
        /// 中止
        /// </summary>
        Aborted,
        /// <summary>
        /// 闲置
        /// </summary>
        Idle,
        /// <summary>
        /// 执行中
        /// </summary>
        Running,
        /// <summary>
        /// 完成
        /// </summary>
        Finish,
    }


    //任务序列
    public abstract class TaskSequenceBase
    {

        /// <summary>
        /// 动作列表
        /// </summary>
        protected List<TaskActionBase> taskActions = new List<TaskActionBase>();
        /// <summary>
        /// 当前动作
        /// </summary>
        protected TaskActionBase currentAction = null;
        /// <summary>
        /// 并行动作
        /// </summary>
        protected List<TaskActionBase> parallelActions = new List<TaskActionBase>();
        /// <summary>
        /// 待执行动作
        /// </summary>
        protected Queue<TaskActionBase> waitAcitons = new Queue<TaskActionBase>();
        public abstract string GetSequenceName();
        /// <summary>
        /// 优先级
        /// </summary>
        public abstract int GetPriority();
        private TaskSequenceStatus status;
        /// <summary>
        /// 获得状态
        /// </summary>
        public virtual TaskSequenceStatus GetStatus()
        {
            return status;
        }
        /// <summary>
        /// 设置状态
        /// </summary>
        /// <param name="status"></param>
        public virtual void SetStatus(TaskSequenceStatus status)
        {
            this.status = status;
        }
        /// <summary>
        /// 是否支持中断
        /// </summary>
        /// <returns></returns>
        public virtual bool CanInterrupt()
        {
            return false;
        }
        /// <summary>
        /// 开始
        /// </summary>
        public virtual void Enter()
        {
            InitActions();
            waitAcitons.Clear();
            foreach (var action in taskActions)
            {
                waitAcitons.Enqueue(action);
            }
        }
        public virtual IEnumerator EnterCoroutine()
        {
            yield return null;
        }
        /// <summary>
        /// 刷新
        /// </summary>
        public virtual void Update()
        {
            //需要执行内部的任务
            //如果当前等待执行任务已经执行完毕，且没有正在执行的任务，则判定为队列结束
            if (waitAcitons.Count == 0 && currentAction == null)
            {
                Finish();
                return;
            }
            //如果当前动作有，则执行
            if (currentAction != null)
            {
                if (currentAction.Status == TaskActionStatus.Running)
                {
                    currentAction.Update();
                }
                else if (currentAction.Status == TaskActionStatus.Completed)
                {
                    currentAction.Exit();
                    currentAction = null;
                }

                if (parallelActions.Count > 0)
                {
                    List<TaskActionBase> removeActions = new List<TaskActionBase>();
                    foreach (var action in parallelActions)
                    {
                        if (action.Status == TaskActionStatus.Completed)
                        {
                            action.Exit();
                            removeActions.Add(action);
                        }
                        else if (action.Status == TaskActionStatus.Running)
                        {
                            action.Update();
                        }
                    }
                    foreach (var action in removeActions)
                    {
                        parallelActions.Remove(action);
                    }
                }
                return;
            }
            //如果当前动作没有，则取出下一个待执行的动作
            if (currentAction == null)
            {
                while (waitAcitons.Count > 0 && waitAcitons.Peek().GetTaskActionType() == TaskActionType.Parallel)
                {
                    var paralleAction = waitAcitons.Dequeue();
                    paralleAction.SetOwner(GetOwner());
                    paralleAction.Status = TaskActionStatus.Running;
                    paralleAction.Enter();
                    parallelActions.Add(paralleAction);
                }
                if (waitAcitons.Count > 0 && waitAcitons.Peek().GetTaskActionType() == TaskActionType.Solo)
                {
                    currentAction = waitAcitons.Dequeue();
                    currentAction.SetOwner(GetOwner());
                    currentAction.Status = TaskActionStatus.Running;
                    currentAction.Enter();
                }
                return;
            }
        }
        /// <summary>
        /// 结束
        /// </summary>
        public virtual void Exit()
        {

        }

        public void Finish()
        {
            SetStatus(TaskSequenceStatus.Finish);
            //所有的任务都需要退出
            if (parallelActions.Count > 0)
            {
                foreach (var action in parallelActions)
                {
                    action.Exit();
                }
            }
        }
        public abstract void InitActions();
        public void AddAction(TaskActionBase action, TaskActionType actionType = TaskActionType.Solo)
        {
            taskActions.Add(action);
        }
        public void AddAction<T>(TaskActionType actionType = TaskActionType.Solo) where T : TaskActionBase, new()
        {
            T action = new T();
            AddAction(action, actionType);
        }
        private AIController _owner;
        public void SetOwner(AIController owner)
        {
            _owner = owner;
        }
        public AIController GetOwner()
        {
            return _owner;
        }
    }
}