using I2.Loc;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static I2.Loc.LocalizationManager;

namespace QFramework
{
    /// <summary>
    /// ����I2,��������������ߣ�����I2�Ļ����Ͻ�����չ
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
        /// ��������Դ��ʹ��Ԥ����ķ�ʽ����
        /// �ο���http://www.inter-illusion.com/assets/I2LocalizationManual/HowtouseAssetBundles.html
        /// ��������languageSource������Ԥ���壬ʹ����Դ���صķ�ʽ�����滻
        /// </summary>
        /// <param name="assetName"></param>
        public static void SetLanguageSourceByAssetName(string assetName)
        {
            I10nManager.Instance.SetLanguageSourceByAssetName(assetName);
        }
    }
}

