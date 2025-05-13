using System.Collections.Generic;
using UnityEngine;


namespace Lost.Ability
{
    public interface IAbilitySystemComponent
    {
        public List<string> GetAbilityTags();
        /// <summary>
        /// 赋予技能，返回技能句柄，通过句柄可以释放技能
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public AbilityHandle GiveAbility<T>() where T : AbilityBase, new();
        /// <summary>
        /// 赋予技能，返回技能句柄，通过句柄可以释放技能
        /// </summary>
        /// <param name="ability"></param>
        /// <returns></returns>
        public AbilityHandle GiveAbility(AbilityBase ability);
        /// <summary>
        /// 释放技能
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void ApplyAbility<T>() where T : AbilityBase, new();
        /// <summary>
        /// 释放技能
        /// </summary>
        /// <param name="handle"></param>
        public void ApplyAbility(AbilityHandle handle);
        /// <summary>
        /// 移除技能
        /// </summary>
        /// <param name="handle"></param>
        public void RemoveAbility(AbilityHandle handle);
        /// <summary>
        /// 移除技能
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void RemoveAbility<T>() where T : AbilityBase, new();
        /// <summary>
        /// 赋予效果，返回效果句柄，通过句柄可以移除效果
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public AbilityEffectHandle ApplyEffectToOwner<T>() where T : AbilityEffectBase, new();
        /// <summary>
        /// 赋予效果，返回效果句柄，通过句柄可以移除效果
        /// </summary>
        /// <param name="effect"></param>
        /// <returns></returns>
        public AbilityEffectHandle ApplyEffectToOwner(AbilityEffectBase effect);
        public AbilityEffectHandle ApplyEffectToTarget<T>(AbilitySystemComponent target) where T : AbilityEffectBase, new();
        public AbilityEffectHandle ApplyEffectToTarget(AbilityEffectBase effect, AbilitySystemComponent target);
        /// <summary>
        /// 移除效果
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void RemoveEffect<T>() where T : AbilityEffectBase, new();
        /// <summary>
        /// 移除效果
        /// </summary>
        /// <param name="handle"></param>
        public void RemoveEffect(AbilityEffectHandle handle);
        public bool HasTags(params string[] tags);
        public List<AbilityEffectHandle> GetEffects<T>() where T : AbilityEffectBase;
        public List<AbilityHandle> GetAbilities<T>() where T : AbilityBase;

    }

}
