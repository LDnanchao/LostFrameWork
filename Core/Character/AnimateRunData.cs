using System;
using UnityEngine;

namespace Lost.Character
{
    /// <summary>
    /// 动画运行时数据
    /// </summary>
    public class AnimateRunData
    {

        public Action Interrupt = delegate { };
        public Action Complete = delegate { };

        public bool IsRunning = false;
        public bool IsSuccess = false;
        public string animateName;

        // public string animateName = string.Empty;
        public AnimateRunDataCoroutine GetCoroutine()
        {
            AnimateRunDataCoroutine coroutine = new AnimateRunDataCoroutine();
            coroutine.animateRunData = this;
            return coroutine;
        }
    }

    /// <summary>
    /// 动画运行时数据协程
    /// </summary>
    public class AnimateRunDataCoroutine : CustomYieldInstruction
    {
        public AnimateRunData animateRunData;
        public override bool keepWaiting => animateRunData.IsRunning;
    }
}