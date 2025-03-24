/***
Ai行为控制器，可以实现简单的任务队列的
但是有一个问题，即Boss复杂攻击逻辑
比如实现一个场地boss，boss的通常会指定攻击，范围攻击，追逐，再攻击等一些的复杂攻击
处理这个复杂攻击，就需要手动控制技能的顺序，按照攻击序列依次执行攻击动作，不满足的攻击动作直接抛弃，进入下一个攻击动作
反复这样就可以了。
***/


using System;
using System.Collections.Generic;
using System.Linq;

namespace Lost.AI
{
    /// <summary>
    /// AI行为控制器
    /// </summary>
    public class AIBehaviorController
    {

        /// <summary>
        /// 备忘录
        /// </summary>
        public ITaskNote note;
        /// <summary>
        /// 任务序列列表
        /// </summary>
        private List<TaskSequenceBase> taskSequences = new List<TaskSequenceBase>();

        private TaskSystem taskSystem;
        private AIController _aiController;
        private bool isInit = false;
        private bool isRun = false;
        public AIBehaviorController(AIController aiController)
        {
            _aiController = aiController;
        }


        public List<TaskSequenceBase> GetTaskSequences()
        {
            return taskSequences.ToList();
        }
        /// <summary>
        /// 添加任务序列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void AddTaskSequence<T>() where T : TaskSequenceBase, new()
        {
            T taskSequence = new T();
            taskSequences.Add(taskSequence);
            taskSequence.SetOwner(GetAIController());
        }
        public void AddTaskSequence(TaskSequenceBase taskSequence)
        {
            taskSequences.Add(taskSequence);
            taskSequence.SetOwner(GetAIController());
        }
        /// <summary>
        /// 开始
        /// </summary>
        public void Start()
        {
            Reset();
            if (!isInit)
            {
                taskSystem = new TaskSystem(this);
            }
            isRun = true;

        }
        /// <summary>
        /// 刷新
        /// </summary>
        public void Update()
        {
            if (isRun)
            {
                taskSystem.Update();
            }
        }
        /// <summary>
        /// 取消
        /// </summary>
        public void Cancel()
        {
            isRun = false;
        }
        /// <summary>
        /// 重置
        /// </summary>
        public void Reset()
        {

        }

        public AIController GetAIController()
        {
            return _aiController;
        }

        internal void SetOwener(AIController aiController)
        {
            _aiController = aiController;
        }
    }
}