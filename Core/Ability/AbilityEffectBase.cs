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
        protected List<AbilityTag> abilityTags = new List<AbilityTag>();

        public List<AbilityTag> GetAbilityTags()
        {
            return abilityTags.ToList();
        }
        /// <summary>
        /// 中断技能
        /// </summary>
        public abstract void InterruptEffect();
        public abstract void EndEffect();

        public void OnApply()
        {
            Apply();
        }
        public abstract void Apply();

        internal void OnUpdate()
        {
            Update();
        }
        public abstract void Update();

        internal AbilityEffectHandle Init(AbilitySystemComponent owner, AbilitySystemComponent target)
        {
            this.source = owner;
            this.target = target;
            AbilityEffectHandle handle = new AbilityEffectHandle();
            handle.effect = this;
            return handle;
        }
    }

    public class CustomAbilityEffect : AbilityEffectBase
    {
        public Action applyAction;
        public Action updateAction;


        public override void Apply()
        {
            applyAction?.Invoke();
        }

        public override void EndEffect()
        {

        }

        public override void InterruptEffect()
        {

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
