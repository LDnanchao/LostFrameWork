using I2.Loc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostFramework.I10nKit
{
    internal class GlobalParamManager: ILocalizationParamsManager
    {
        private Dictionary<string, string> _params = new Dictionary<string, string>();

        string ILocalizationParamsManager.GetParameterValue(string Param)
        {
            if (_params.ContainsKey(Param))
            {
                return _params[Param]; 
            }
            return null;
        }

        public void SetParameterValue(string Param, string Value)
        {
            if (!String.IsNullOrEmpty(Param))
            {
                if(_params.ContainsKey(Param))
                {
                    _params[Param] = Value;
                }else
                {
                    _params.Add(Param, Value);
                }
            }
        }
       
    }
}
