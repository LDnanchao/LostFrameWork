using System;
using System.Collections.Generic;
using System.Linq;


namespace Lost.Ability
{
    /// <summary>
    /// 技能效果
    /// 这是一个基础的技能效果
    /// 后面会增加周期性效果，持续性效果，触发效果等，执行效果的时候也可以释放特效
    /// </summary>
    public abstract class AbilityEffectBase
    {
        /// <summary>
        /// 效果释放源
        /// </summary>
        public AbilitySystemComponent source;
        /// <summary>
        /// 效果影响者
        /// </summary>
        public AbilitySystemComponent target;
        protected List<string> abilityTags = new List<string>();
        protected List<string> blockTags = new List<string>();
        private bool _isExecuting = false;

        public List<string> GetAbilityTags()
        {
            return abilityTags.ToList();
        }

        public List<string> GetBlockTags(){
            return blockTags.ToList();
        }

        public void FinishEffect()
        {
            _isExecuting = false;
        }
        public abstract void Exit();

        public void OnExecute()
        {
            _isExecuting = true;
            Execute();
        }
        public abstract void Execute();

        internal void OnUpdate()
        {
            if (!_isExecuting) return;
            Update();
        }
        public abstract void Update();

        public AbilityEffectHandle Init(AbilitySystemComponent owner, AbilitySystemComponent target)
        {
            this.source = owner;
            this.target = target;
            AbilityEffectHandle handle = new AbilityEffectHandle();
            handle.effect = this;
            return handle;
        }

        public bool isExecuting()
        {
            return _isExecuting;
        }

        public void OnExit()
        {
            Exit();
        }
    }

    public class CustomAbilityEffect : AbilityEffectBase
    {
        public Action applyAction;
        public Action updateAction;
        public Action exitAction;

        public override void Execute()
        {
            applyAction?.Invoke();
        }

        public override void Exit()
        {
            exitAction?.Invoke();
        }

        public override void Update()
        {
            updateAction?.Invoke();
        }
    }

    public class AbilityEffectHandle
    {
        public AbilityEffectBase effect;
    }
}
