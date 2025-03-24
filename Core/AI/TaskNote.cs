using System;

namespace Lost.AI
{
    //记事本,used by record some datas of task
    public interface ITaskNote
    {
        int GetInt(string key);
        void SetInt(string key, int value);
        bool GetBool(string key);
        void SetBool(string key, bool value);
        string GetString(string key);
        void SetString(string key, string value);
        float GetFloat(string key);
        void SetFloat(string key, float value);
        object GetObj(string key);
        void SetObj(string key, object value);
    }
    public class TaskNote : ITaskNote
    {
        public bool GetBool(string key)
        {
            throw new NotImplementedException();
        }

        public float GetFloat(string key)
        {
            throw new NotImplementedException();
        }

        public int GetInt(string key)
        {
            throw new NotImplementedException();
        }

        public object GetObj(string key)
        {
            throw new NotImplementedException();
        }

        public string GetString(string key)
        {
            throw new NotImplementedException();
        }

        public void SetBool(string key, bool value)
        {
            throw new NotImplementedException();
        }

        public void SetFloat(string key, float value)
        {
            throw new NotImplementedException();
        }

        public void SetInt(string key, int value)
        {
            throw new NotImplementedException();
        }

        public void SetObj(string key, object value)
        {
            throw new NotImplementedException();
        }

        public void SetString(string key, string value)
        {
            throw new NotImplementedException();
        }
    }
}