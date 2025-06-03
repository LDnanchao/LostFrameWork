using System;
using System.Collections.Generic;
using UnityEngine;

namespace Lost.Character
{
    public interface ICharacterAnimateController
    {
        public void SetFloat(string name, float value);
        public float GetFloat(string name);
        public void SetBool(string name, bool value);
        public bool GetBool(string name);
        public void SetInt(string name, int value);
        public int GetInt(string name);
        public void SetTrigger(string name);
        public void Trigger(string name);
    }

     public abstract class AbstractCharacterAnimateController : MonoBehaviour, ICharacterAnimateController
    {
        private Dictionary<string, bool> boolDict = new Dictionary<string, bool>();
        private Dictionary<string, float> floatDict = new Dictionary<string, float>();
        private Dictionary<string, int> intDict = new Dictionary<string, int>();

        public bool GetBool(string name)
        {
            if (boolDict.ContainsKey(name))
                return boolDict[name];
            return false;
        }

        public float GetFloat(string name)
        {
            if (floatDict.ContainsKey(name))
                return floatDict[name];
            return 0;
        }

        public int GetInt(string name)
        {
            if (intDict.ContainsKey(name))
                return intDict[name];
            return 0;
        }

        public void SetBool(string name, bool value)
        {
            boolDict[name] = value;
        }

        public void SetFloat(string name, float value)
        {
            floatDict[name] = value;
        }

        public void SetInt(string name, int value)
        {
            intDict[name] = value;
        }

        public void SetTrigger(string name)
        {
            Trigger(name);
        }

        public abstract void Trigger(string name);


        public abstract AnimateRunData PlayAnimateByName(string animateName, Action complete=null, Action interrupt=null);

        public abstract AnimateRunData PlayAnimateByTrigger(string trigger, Action complete=null, Action interrupt=null);
    }
}