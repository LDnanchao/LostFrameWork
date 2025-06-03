using I2.Loc;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace LostFramework.I10nKit.Editor
{
    public class I10nEditor : Editor
    {
        [MenuItem("Tools/I2 Localization/SetChinese", false, 16)]
        public static void SetChinese()
        {
            LocalizationManager.CurrentLanguageCode = "zh-CN";
            LocalizationEditor.CallLocalizeAll();
        }
        [MenuItem("Tools/I2 Localization/SetEnglish", false, 16)]
        public static void SetEnglish()
        {
            LocalizationManager.CurrentLanguageCode = "en";
            LocalizationEditor.CallLocalizeAll();
        }
    }

}
