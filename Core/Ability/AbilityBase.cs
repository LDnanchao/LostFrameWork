using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Lost.Ability
{
    /// <summary>
    /// 技能基类
    /// 调用FinishAbility()结束技能
    /// 调用OnExecute()调用执行，OnUpdate()调用更新
    /// 执行技能的时候也可以释放特效
    /// </summary>
    public abstract class AbilityBase
    {

        public AbilitySystemComponent owner;
        /// <summary>
        /// 是否正在执行
        /// </summary>
        private bool isExecuting = false;
        private bool isExit = false;
        protected List<AbilityTag> abilityTags = new List<AbilityTag>();
        protected List<AbilityTag> blockTags = new List<AbilityTag>();
        private Coroutine executeCoroutine;
        /// <summary>
        /// 获取技能标签
        /// </summary>
        /// <returns></returns>
        public List<AbilityTag> GetAbilityTags()
        {
            return abilityTags.ToList();
        }
        /// <summary>
        /// 获取阻挡标签
        /// 如果技能系统中有该标签存在，技能将无法释放
        /// </summary>
        /// <returns></returns>
        public List<AbilityTag> GetBlockTags()
        {
            return blockTags.ToList();
        }
        /// <summary>
        /// 调用执行
        /// </summary>
        public void OnExecute()
        {
            isExecuting = true;
            isExit = false;
            Execute();
            executeCoroutine = owner.StartCoroutine(ExecuteCoroutine());
        }
        /// <summary>
        /// 调用更新
        /// </summary>
        public void OnUpdate()
        {
            Update();
        }
        public void OnExit()
        {
            Exit();
            if (executeCoroutine != null) owner.StopCoroutine(executeCoroutine);
            isExit = true;
        }
        /// <summary>
        /// 结束技能
        /// </summary>
        public void FinishAbility()
        {
            isExecuting = false;
        }
        /// <summary>
        /// 执行,通过effect去施加影响效果，当然也可以直接修改玩家数据
        /// </summary>
        public virtual void Execute()
        {

        }
        public virtual IEnumerator ExecuteCoroutine()
        {
            yield return null;
        }
        /// <summary>
        /// 更新
        /// </summary>
        public virtual void Update()
        {

        }

        public virtual void Exit()
        {

        }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="abilitySystemComponent"></param>
        /// <returns></returns>
        public AbilityHandle Init(AbilitySystemComponent abilitySystemComponent)
        {
            owner = abilitySystemComponent;
            AbilityHandle handle = new AbilityHandle();
            handle.ability = this;
            return handle;
        }

        /// <summary>
        /// 是否正在执行，没有执行不代表已经退出了，如果需要检查是否已经退出，请使用IsExit()
        /// </summary>
        /// <returns></returns>
        public bool IsExecute()
        {
            return isExecuting;
        }
        /// <summary>
        /// 是否已经退出
        /// </summary>
        /// <returns></returns>
        public bool IsExit()
        {
            return isExit;
        }

        public string GetName()
        {
            return GetType().FullName + "_" + GetHashCode();
        }
    }
    public class CustomAbility : AbilityBase
    {
        private Action executeAction;
        private Action updateAction;

        public override void Execute()
        {
            executeAction?.Invoke();
        }

        public override void Update()
        {
            updateAction?.Invoke();
        }
    }
    public struct AbilityHandle
    {
        public AbilityBase ability;

        public bool IsRunning()
        {
            if (ability == null)
            {
                return false;
            }
            return !ability.IsExit();
        }
    }
}

