using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class LostEditor : Editor
{
    [MenuItem("Lost/Install All Dependencies",false,1)]
    static void InstallAllDependencies()
    {
       
    }
    [MenuItem("Lost/Open Framework Manager",false,0)]
    static void OpenFrameworkManager()
    {

    }
    //创建标准非热更项目
    [MenuItem("Lost/Create Standard Project",false,2)]
    static void CreateStandardDirectory()
    {

    }

    //创建标准热更项目
    [MenuItem("Lost/Create Hotfix Project",false,3)]
    static void CreateHotfixDirectory()
    {

    }

}
