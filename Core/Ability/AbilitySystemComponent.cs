using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Lost.Ability
{
    public class AbilitySystemComponent : MonoBehaviour, IAbilitySystemComponent
    {
        private List<AbilityHandle> ownAbilities = new List<AbilityHandle>();

        private List<AbilityHandle> activeAbilities = new List<AbilityHandle>();
        private List<AbilityEffectHandle> activeEffects = new List<AbilityEffectHandle>();
        [SerializeField]
        private List<AbilityTag> abilityTags = new List<AbilityTag>();
        public List<string> currentAbilityNames = new List<string>();
        public void ApplyAbility<T>() where T : AbilityBase, new()
        {
            AbilityBase ability = new T();
            AbilityHandle handle = GiveAbility(ability);
            ApplyAbility(handle);
        }

        public void ApplyAbility(AbilityHandle handle)
        {
            if (!CheckCanApply(handle))
            {
                return;
            }
            activeAbilities.Add(handle);
            AddAbilityTags(handle);
            handle.ability.OnExecute();
        }

        private bool CheckCanApply(AbilityHandle handle)
        {
            if (!ownAbilities.Contains(handle))
            {
                Debug.Log("Ability not owned by this component");
                return false;
            }
            var blockTags = handle.ability.GetBlockTags();
            var activeTags = GetAbilityTags();
            foreach (var blockTag in blockTags)
            {
                if (activeTags.Contains(blockTag))
                {
                    Debug.Log("Ability blocked by another ability" + "BlockTag: " + blockTag);
                    return false;
                }

            }
            return true;
        }

        public AbilityEffectHandle ApplyEffectToOwner<T>() where T : AbilityEffectBase, new()
        {
            AbilityEffectBase effect = new T();
            return ApplyEffectToOwner(effect);
        }

        public AbilityEffectHandle ApplyEffectToOwner(AbilityEffectBase effect)
        {
            AbilityEffectHandle handle = effect.Init(this, this);
            activeEffects.Add(handle);
            AddAbilityTags(handle);
            effect.OnApply();
            return handle;
        }

        public AbilityEffectHandle ApplyEffectToTarget<T>(AbilitySystemComponent target) where T : AbilityEffectBase, new()
        {
            AbilityEffectBase effect = new T();
            return ApplyEffectToTarget(effect, target);
        }

        public AbilityEffectHandle ApplyEffectToTarget(AbilityEffectBase effect, AbilitySystemComponent target)
        {
            AbilityEffectHandle handle = effect.Init(this, target);
            target.activeEffects.Add(handle);
            target.AddAbilityTags(handle);
            effect.OnApply();
            return handle;
        }

        public List<AbilityTag> GetAbilityTags()
        {
            return abilityTags.ToList();
        }

        public AbilityHandle GiveAbility<T>() where T : AbilityBase, new()
        {
            AbilityBase ability = new T();
            return GiveAbility(ability);
        }

        public AbilityHandle GiveAbility(AbilityBase ability)
        {
            AbilityHandle handle = ability.Init(this);
            ownAbilities.Add(handle);
            currentAbilityNames.Add(handle.ability.GetName());
            return handle;
        }

        public void RemoveAbility(AbilityHandle handle)
        {
            if (!ownAbilities.Contains(handle)) return;
            ownAbilities.Remove(handle);
            currentAbilityNames.Remove(handle.ability.GetName());
            if (activeAbilities.Contains(handle))
            {
                activeAbilities.Remove(handle);
                handle.ability.FinishAbility();
                handle.ability.OnExit();
            }
        }

        public void RemoveAbility<T>() where T : AbilityBase, new()
        {
            AbilityHandle targetHandle = new AbilityHandle();
            foreach (AbilityHandle handle in ownAbilities)
            {
                if (typeof(T) == handle.ability.GetType())
                {
                    targetHandle = handle;
                    break;
                }
            }
            RemoveAbility(targetHandle);
        }
        public void RemoveEffect(AbilityEffectHandle handle)
        {
            if (!activeEffects.Contains(handle)) return;
            activeEffects.Remove(handle);
            handle.effect.InterruptEffect();
        }
        public void RemoveEffect<T>() where T : AbilityEffectBase, new()
        {
            AbilityEffectHandle targetHandle = new AbilityEffectHandle();
            foreach (AbilityEffectHandle handle in activeEffects)
            {
                if (typeof(T) == handle.effect.GetType())
                {
                    targetHandle = handle;
                    break;
                }
            }
            RemoveEffect(targetHandle);
        }


        void Update()
        {
            List<AbilityHandle> unExecutedAbilities = new List<AbilityHandle>();
            foreach (AbilityHandle handle in activeAbilities)
            {
                handle.ability.OnUpdate();
                if (!handle.ability.IsExecute())
                {
                    unExecutedAbilities.Add(handle);
                }
            }
            foreach (AbilityEffectHandle handle in activeEffects)
            {
                handle.effect.OnUpdate();
            }
            foreach (AbilityHandle handle in unExecutedAbilities)
            {
                activeAbilities.Remove(handle);
                handle.ability.OnExit();
            }
            UpdateAbilityTags();
        }
        /// <summary>
        /// 刷新所有能力的标签
        /// </summary>
        private void UpdateAbilityTags(){
            abilityTags.Clear();
            foreach (AbilityHandle handle in activeAbilities)
            {
                AddAbilityTags(handle);
            }
             foreach (AbilityEffectHandle handle in activeEffects)
            {
                AddAbilityTags(handle);
            }
        }
        private void AddAbilityTags(AbilityEffectHandle handle)
        {
            var effect = handle.effect;
            foreach (AbilityTag tag in effect.GetAbilityTags())
            {
                if (!abilityTags.Contains(tag)) abilityTags.Add(tag);
            }
        }

        private void AddAbilityTags(AbilityHandle handle)
        {
            var ability = handle.ability;
            foreach (AbilityTag tag in ability.GetAbilityTags())
            {
                if (!abilityTags.Contains(tag)) abilityTags.Add(tag);
            }
        }

        public void StopAbility()
        {
            foreach (AbilityHandle handle in activeAbilities){
                handle.ability.FinishAbility();
                handle.ability.OnExit();
            }
            activeAbilities.Clear();
        }
    }

}
