using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostFramework.I10nKit
{
    /// <summary>
    /// 翻译参数
    /// </summary>
    public struct I10nParam
    {
        /// <summary>
        /// 关键字
        /// </summary>
        public string key;
        /// <summary>
        /// 值
        /// </summary>
        public object value;

        public I10nParam(string key, object value)
        {
            this.key = key;
            this.value = value;
        }
    }
}
