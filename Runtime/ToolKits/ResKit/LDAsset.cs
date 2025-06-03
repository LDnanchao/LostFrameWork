using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

public class LDAsset
{

}

public interface IAssetManager
{
    /// <summary>
    /// 基于一套协议进行加载,提供四种协议，所有的加载必须按照这种加载
    /// boundle://boundname/assetname
    /// path://asset/aaa/sss/assetname
    /// local://asset/aaa/sss/assetname
    /// resource://asset/aaa/sss/assetname
    /// 图集补充协议，在后面再加上&altasSprite=spriteName
    /// 即boundle://boundname/assetname&altasSprite=spriteName,如果有其他的也需要去处理
    /// 并增加altasSprite解析器，以及配套解析器规则，以支持更多资源解析方式，如文本解析，jsonCovert=Unity或jsonCovert=Json.Net
    /// 提供可扩展的解析方案
    /// 默认使用的是YooAsset,不再使用Qframework的框架
    /// </summary>
    /// <param name="assetpath"></param>
    public void LoadAsset<T>(string url, Action<bool, T> callback);
    public T LoadAssetSync<T>(string url);
    public UniTask<T> LoadAssetAsync<T>(string url);
    /// <summary>
    /// 所有的场景都是单独的boundle,这里只需要传入场景名即可
    /// </summary>
    /// <param name="sceneName"></param>
    public void LoadSceneSync(string sceneName, LoadSceneMode mode = LoadSceneMode.Single);
    public UniTask LoadSceneAsync(string sceneName, LoadSceneMode mode = LoadSceneMode.Single);
    public void LoadScene(string sceneName, LoadSceneMode mode = LoadSceneMode.Single, Action<bool> callback = null);


    public void LoadSprite(string altName, string spriteName);
}