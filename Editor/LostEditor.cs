using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using System.IO;
using System.Linq;

public class LostEditor : Editor
{
    [MenuItem("Lost/Install All Dependencies", false, 1)]
    static void InstallAllDependencies()
    {

    }
    [MenuItem("Lost/Open Framework Manager", false, 0)]
    static void OpenFrameworkManager()
    {

    }
    //创建标准非热更项目
    [MenuItem("Lost/Create Standard Project", false, 2)]
    static void CreateStandardDirectory()
    {

    }

    //创建标准热更项目
    [MenuItem("Lost/Create Hotfix Project", false, 3)]
    static void CreateHotfixDirectory()
    {

    }
    public IEnumerator InstallPackages(params string[] packageNames)
    {
        // 检查是否已安装
        var packages = UnityEditor.PackageManager.Client.List();

        List<string> waitInstallPackages = new List<string>();
        yield return new WaitUntil(() => packages.IsCompleted);

        List<UnityEditor.PackageManager.PackageInfo> installedPackages = packages.Result.ToList();
        foreach (var packageName in packageNames)
        {
            if(installedPackages.Exists(p => p.name.Equals(packageName)))
            {
                var pkg = installedPackages.Find(p => p.name.Equals(packageName));
                Debug.Log($"{packageName} 已安装，版本：{pkg.version}");
            }
            else
            {
                waitInstallPackages.Add(packageName);
            }
        }

        if(waitInstallPackages.Count > 0)
        {
            foreach (var packageName in waitInstallPackages)
            {
                // 发起安装请求
                AddRequest addRequest = UnityEditor.PackageManager.Client.Add(packageName);
                yield return new WaitWhile(() => addRequest.IsCompleted);

                if (addRequest.Status == StatusCode.Success)
                {
                    Debug.Log($"包 {packageName} 安装成功！");
                }
                else
                {
                    Debug.LogError($"安装失败：{addRequest.Error.message}");
                }
            }
        }
    }


    public static void ImportPackage(string dirName,string requiredPackage)
    {
        string packagePath = Path.Combine(Application.dataPath, dirName, requiredPackage);

        if (!File.Exists(packagePath))
        {
            Debug.LogWarning($"{requiredPackage} 未找到，尝试从服务器下载...");
            // 此处可添加下载逻辑（需自行实现）
        }
        else
        {
            AssetDatabase.ImportPackage(packagePath, false);
        }
    }



    public static IEnumerator CheckPackages(params string[] targetPackageNames)
    {

        // 获取所有已安装的包
        var packages = UnityEditor.PackageManager.Client.List();
        yield return new WaitUntil(() => packages.IsCompleted);
        List<UnityEditor.PackageManager.PackageInfo> installedPackages = packages.Result.ToList();

        foreach (var targetPackageName in targetPackageNames)
        {
            if(installedPackages.Exists(p => p.name.Equals(targetPackageName)))
            {
                var pkg = installedPackages.Find(p => p.name.Equals(targetPackageName));
                Debug.Log($"包 {targetPackageName} 已安装，版本：{pkg.version}");
            }
            else
            {
                Debug.LogWarning($"包 {targetPackageName} 未安装！");
            }
        } 
    }
}
