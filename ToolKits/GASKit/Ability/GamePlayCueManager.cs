using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using QFramework;
using UnityEngine;

public class GamePlayCueManager : Singleton<GamePlayCueManager>
{

    private GamePlayCueManager() { }

    private Dictionary<string, List<Type>> mCues = new Dictionary<string, List<Type>>();
    public override void OnSingletonInit()
    {
        //获得当前运行的程序集
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        Debug.Log("GamePlayCueManager: assemblies count: " + assemblies.Length);
        Init(assemblies);
    }
    private void Init(params Assembly[] assemblies)
    {
        foreach (Assembly assembly in assemblies)
        {
            var types = assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(GamePlayCueBase))).ToList();
            foreach (var type in types)
            {
                if (type.HasAttribute(typeof(GamePlayCueTagsAttribute)))
                {
                    var tag = type.GetCustomAttribute<GamePlayCueTagsAttribute>().abilityTag;
                    if (!mCues.ContainsKey(tag))
                    {
                        mCues.Add(tag, new List<Type>());
                    }
                    mCues[tag].Add(type);
                    Debug.Log("GamePlayCueManager: " + type.Name + " tag: " + tag);
                }
            }
        }
    }

    public List<GamePlayCueBase> GetGamePlayCues(string tag)
    {
        List<GamePlayCueBase> cues = new List<GamePlayCueBase>();
        if (mCues.ContainsKey(tag))
        {
            var types = mCues[tag];
            foreach (var type in types)
            {
                cues.Add(Activator.CreateInstance(type) as GamePlayCueBase);
            }
        }
        return cues;
    }
}