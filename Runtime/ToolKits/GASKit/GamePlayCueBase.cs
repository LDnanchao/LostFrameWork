
using System;
using Lost.Ability;


public abstract class GamePlayCueBase
{
    public AbilitySystemComponent owner;
    public void OnExecute()
    {
        Execute();
    }

    protected abstract void Execute();

    public void OnExit()
    {
        Exit();
    }

    protected abstract void Exit();

    public void OnUpdate()
    {
        Update();
    }

    protected abstract void Update();
}


[AttributeUsage(AttributeTargets.Class)]
public class GamePlayCueTagsAttribute : Attribute
{
    public string abilityTag;
    public GamePlayCueTagsAttribute( string tag)
    {
        abilityTag = tag;
    }
}