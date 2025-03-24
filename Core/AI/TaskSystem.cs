using System;
using System.Collections.Generic;
using System.Linq;

namespace Lost.AI
{
    //任务管理系统
    public class TaskSystem
    {
        private AIBehaviorController aiBehaviorController;
        private TaskSequenceBase currentSequence;
        private List<TaskSequenceBase> waitSequences;
        private List<TaskSequenceBase> allSequences;
        private bool isInit;
        public TaskSystem(AIBehaviorController aiBehaviorController)
        {
            this.aiBehaviorController = aiBehaviorController;
            waitSequences = new List<TaskSequenceBase>();
            allSequences = new List<TaskSequenceBase>();
            allSequences.AddRange(aiBehaviorController.GetTaskSequences());
        }

        internal void Update()
        {
            //判断当前是否已经初始化了
            if (!isInit)
            {
                //如果没有则进行初始化
                Init();
            }
            else
            {
                UpdateWaitSequence();
                //如果初始化完成则开始执行任务队列
                if (currentSequence == null)
                {
                    //获得最新的任务队列
                    currentSequence = waitSequences.First();
                    waitSequences.Remove(currentSequence);
                    currentSequence.SetStatus(TaskSequenceStatus.Running);
                    currentSequence.Enter();
                    aiBehaviorController.GetAIController().StartCoroutine(currentSequence.EnterCoroutine());
                }
                else
                {
                    //如果当前任务可以中止，一旦出现高优先级的任务，则中止当前任务
                    if (currentSequence.CanInterrupt() && waitSequences.Count > 0 && waitSequences.First().GetPriority() > currentSequence.GetPriority())
                    {
                        currentSequence.SetStatus(TaskSequenceStatus.Aborted);
                        currentSequence.Exit();
                        currentSequence = null;
                    }
                    //更新当前任务
                    else if (currentSequence.GetStatus() == TaskSequenceStatus.Running)
                    {
                        currentSequence.Update();
                    }
                    else if (currentSequence.GetStatus() == TaskSequenceStatus.Finish)
                    {
                        currentSequence.Exit();
                        currentSequence.SetStatus(TaskSequenceStatus.Waiting);
                        currentSequence = null;
                    }
                    else if (currentSequence.GetStatus() == TaskSequenceStatus.Aborted)
                    {
                        currentSequence.Exit();
                        currentSequence = null;
                    }

                }
            }

        }

        private void Init()
        {
            isInit = true;
        }
        /// <summary>
        /// 刷新任务队列
        /// </summary>
        private void UpdateWaitSequence()
        {
            //执行队列需要是等待状态的队列
            //且按照优先级进行排序
            waitSequences.Clear();
            foreach (var sequence in allSequences)
            {
                if (sequence.GetStatus() == TaskSequenceStatus.Waiting || sequence.GetStatus() == TaskSequenceStatus.Aborted)
                {
                    waitSequences.Add(sequence);
                }
            }
            waitSequences.Sort((a, b) => a.GetPriority() - b.GetPriority());
        }
    }
}