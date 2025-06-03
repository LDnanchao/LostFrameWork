using I2.Loc;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static I2.Loc.LocalizationManager;

namespace LostFramework.I10nKit
{
    /// <summary>
    /// 对于I2,不适用其参数工具，会在I2的基础上进行扩展
    /// </summary>
    public class I10nKit
    {
        public static string GetTermTranslation(string term, params I10nParam[] i10nParams)
        {
            return I10nManager.Instance.GetTermTranslation(term, i10nParams);
        }

        public static void SetGlobalParameterValue(string param, object value)
        {
            I10nManager.Instance.SetGlobalParameterValue(param, value);
        }
        public static string ApplyLocalizationParams( string translate, params I10nParam[] i10nParams)
        {
            string newstr = translate;
            I10nManager.Instance.ApplyLocalizationParams(ref newstr, i10nParams);
            return newstr;
        }

        public static void SetLanguage(string languageCode)
        {
            I10nManager.Instance.SetLanguage(languageCode);
        }
        /// <summary>
        /// 设置语言源，使用预制体的方式制作
        /// 参考：http://www.inter-illusion.com/assets/I2LocalizationManual/HowtouseAssetBundles.html
        /// 将场景的languageSource制作成预制体，使用资源加载的方式进行替换
        /// </summary>
        /// <param name="assetName"></param>
        public static void SetLanguageSourceByAssetName(string assetName)
        {
            I10nManager.Instance.SetLanguageSourceByAssetName(assetName);
        }
    }
}

