using I2.Loc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace LostFramework.I10nKit
{
    internal class I10nManager:Singleton<I10nManager>
    {
        private GlobalParamManager _paramsManager;
        private I10nLanguageManager _langManager;
        private I10nManager() { }
        public override void OnSingletonInit()
        {
            base.OnSingletonInit();
            _paramsManager = new GlobalParamManager();
            LocalizationManager.ParamManagers.Add(_paramsManager);
            LocalizationManager.LocalizeAll(true);
            _langManager = new I10nLanguageManager();
            _langManager.Init();
            //重写参数规则
            //LocalizationManager.CustomApplyLocalizationParams = ApplyLocalizationParams;
            //当前只需要重写数据加载流程
        }
        public override void Dispose()
        {
            base.Dispose();
            _langManager.Dispose();
        }
        public void SetLanguageSourceByAssetName(string assetName)
        {
            _langManager.LoadLanguage(assetName);
        }
        public string GetTermTranslation(string term, I10nParam[] i10nParams)
        {
            //设置参数
            string translate = LocalizationManager.GetTranslation(term);
            Dictionary<string, object> dict = I10nParamsToDictionary(i10nParams);
            LocalizationManager.ApplyLocalizationParams(ref translate, p => GetLocalizationParam(p, dict));
            Debug.Log($"{term}/{translate}");
            return translate;
        }
        /// <summary>
        /// 应用参数
        /// </summary>
        /// <param name="translate"></param>
        /// <param name="i10nParams"></param>
        public void ApplyLocalizationParams(ref string translate, I10nParam[] i10nParams)
        {
            Dictionary<string, object> dict = I10nParamsToDictionary(i10nParams);
            LocalizationManager.ApplyLocalizationParams(ref translate, p => GetLocalizationParam(p, dict));
        }
        private Dictionary<string, object> I10nParamsToDictionary(I10nParam[] i10nParams)
        {
            Dictionary<string, object> map = new Dictionary<string, object>();
            foreach (var item in i10nParams)
            {
                map.Add(item.key, item.value);
            }
            return map;
        }

        public object GetLocalizationParam(string param,Dictionary<string,object> i10nParams)
        {
            string result = null;
            if (i10nParams.ContainsKey(param))
            {
                result =  i10nParams[param].ToString();
                return result;
            }

            if(result == null)
            {
                result = GetGlobalParameterValue(param);
                if(result!=null) 
                    return result;
            }
           
            return null;
        }
        public void SetGlobalParameterValue(string param, object value)
        {
            if (value != null)
            {
                _paramsManager.SetParameterValue(param, value.ToString());
            }

        }
        public string GetGlobalParameterValue(string param)
        {
            string result = null;
            for (int i = 0, imax = LocalizationManager.ParamManagers.Count; i < imax; ++i)
            {
                result = LocalizationManager.ParamManagers[i].GetParameterValue(param);
                if (result != null)
                    return result;
            }
            return null;
        }
       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="translation"></param>
        /// <param name="getParam"></param>
        /// <param name="allowLocalizedParameters"></param>
        /// <returns>返回true,则应用解析，返回false则不应用解析</returns>
        public bool ApplyLocalizationParams(ref string translation, LocalizationManager._GetParam getParam, bool allowLocalizedParameters)
        {
            throw new NotImplementedException();
        }

        internal void SetLanguage(string languageCode)
        {
            LocalizationManager.CurrentLanguage = languageCode;
        }
    }
}
