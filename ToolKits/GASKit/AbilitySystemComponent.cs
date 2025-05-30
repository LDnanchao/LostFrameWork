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
        private List<string> abilityTags = new List<string>();
        public List<string> currentAbilityNames = new List<string>();

        private Dictionary<string, List<GamePlayCueBase>> gamePlayCues = new Dictionary<string, List<GamePlayCueBase>>();
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
            AddAbilityTags(handle,true);
            handle.ability.OnExecute();
        }

        private bool CheckCanApply(AbilityHandle handle)
        {
            if (!ownAbilities.Contains(handle))
            {
                Debug.LogError("Ability not owned by this component");
                return false;
            }
            var blockTags = handle.ability.GetBlockTags();
            var activeTags = GetAbilityTags();
            foreach (var blockTag in blockTags)
            {
                if (activeTags.Contains(blockTag))
                {
                    Debug.LogError("Ability blocked by another ability" + "BlockTag: " + blockTag);
                    return false;
                }

            }
            return true;
        }
        public bool HasTags(params string[] tags)
        {
            var activeTags = GetAbilityTags();
            foreach (var tag in tags)
            {
                if (activeTags.Contains(tag))
                {
                    return true;
                }

            }
            return false;
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
            AddAbilityTags(handle,true);
            effect.OnExecute();
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
            target.AddAbilityTags(handle,true);
            effect.OnExecute();
            return handle;
        }

        public List<string> GetAbilityTags()
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
                if (handle.ability.IsExecute())
                {
                    handle.ability.FinishAbility();
                }

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
            if (handle.effect.isExecuting())
            {
                handle.effect.FinishEffect();
            }
            handle.effect.OnExit();
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

                if (handle.ability.IsExecute())
                {
                    handle.ability.OnUpdate();
                }
                else
                {
                    unExecutedAbilities.Add(handle);
                }
            }
            List<AbilityEffectHandle> unExecutedEffects = new List<AbilityEffectHandle>();
            foreach (AbilityEffectHandle handle in activeEffects)
            {
                if (!handle.effect.isExecuting())
                {
                    unExecutedEffects.Add(handle);
                }
                else
                {
                    handle.effect.OnUpdate();
                }
            }
            foreach (AbilityHandle handle in unExecutedAbilities)
            {
                activeAbilities.Remove(handle);
                handle.ability.OnExit();
                // RemoveAbility(handle);
            }
            foreach (AbilityEffectHandle handle in unExecutedEffects)
            {
                RemoveEffect(handle);
            }
            UpdateAbilityTags();
        }
        /// <summary>
        /// 刷新所有能力的标签
        /// </summary>
        private void UpdateAbilityTags()
        {
            var beforeTags = abilityTags.ToList();
            abilityTags.Clear();
            foreach (AbilityHandle handle in activeAbilities)
            {
                AddAbilityTags(handle);
            }
            foreach (AbilityEffectHandle handle in activeEffects)
            {
                AddAbilityTags(handle);
            }
            foreach (var tag in abilityTags)
            {
                OnRefreshTag(tag);
            }
            foreach (var tag in beforeTags)
            {
                if (!abilityTags.Contains(tag))
                {
                    OnRemoveTag(tag);
                }
            }
        }

        private void OnRemoveTag(string tag)
        {
            if (gamePlayCues.ContainsKey(tag))
            {
                var cues = gamePlayCues[tag];
                foreach (var cue in cues)
                {
                    cue.OnExit();
                }
                cues.Clear();
            }
        }

        private void OnRefreshTag(string tag)
        {
            if (gamePlayCues.ContainsKey(tag))
            {
                var cues = gamePlayCues[tag];
                foreach (var cue in cues)
                {
                    cue.OnUpdate();
                }
            }
        }

        private void OnNewTag(string tag)
        {
            if (!gamePlayCues.ContainsKey(tag))
            {
                gamePlayCues.Add(tag, new List<GamePlayCueBase>());
            }
            var cues = gamePlayCues[tag];
            cues.AddRange(GamePlayCueManager.Instance.GetGamePlayCues(tag));
            foreach (var cue in cues)
            {
                cue.owner = this;
                cue.OnExecute();
            }
        }

        private void AddAbilityTags(AbilityEffectHandle handle, bool trigger = false)
        {
            var effect = handle.effect;
            foreach (string tag in effect.GetAbilityTags())
            {
                if (!abilityTags.Contains(tag))
                {
                    abilityTags.Add(tag);
                    if (trigger) OnNewTag(tag);
                }
            }
        }

        private void AddAbilityTags(AbilityHandle handle, bool trigger = false)
        {
            var ability = handle.ability;
            foreach (string tag in ability.GetAbilityTags())
            {
                if (!abilityTags.Contains(tag))
                {
                    abilityTags.Add(tag);
                    if (trigger) OnNewTag(tag);
                }
            }
        }

        public void StopAbility()
        {
            foreach (AbilityHandle handle in activeAbilities)
            {
                handle.ability.FinishAbility();
                handle.ability.OnExit();
            }
            activeAbilities.Clear();
        }

        public List<AbilityEffectHandle> GetEffects<T>() where T : AbilityEffectBase
        {
            List<AbilityEffectHandle> result = new List<AbilityEffectHandle>();
            foreach (AbilityEffectHandle handle in activeEffects)
            {
                if (handle.effect is T)
                {
                    result.Add(handle);
                }
            }
            return result;
        }

        public List<AbilityHandle> GetAbilities<T>() where T : AbilityBase
        {
            List<AbilityHandle> result = new List<AbilityHandle>();
            foreach (AbilityHandle handle in activeAbilities)
            {
                if (handle.ability is T)
                {
                    result.Add(handle);
                }
            }
            return result;
        }
    }
}


