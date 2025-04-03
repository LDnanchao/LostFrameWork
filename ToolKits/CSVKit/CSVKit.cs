using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using UnityEngine;

namespace Lost
{
    public class CSVKit
    {
        /// <summary>
        /// 当前底层使用的csvReader，但对于支持并不是很灵活，或许自行实现
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="asset"></param>
        /// <returns></returns>
        public static List<T> Load<T>(TextAsset asset)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                IgnoreBlankLines = true,
            };
            List<T> list = null;
            using (var stream = new MemoryStream(asset.bytes))
            using (var reader = new StreamReader(stream, Encoding.ASCII))
            using (var csv = new CsvReader(reader,config))
            {
                var records = csv.GetRecords<T>();
                list = records.ToList();
            }
            
            return list;
        }
    }
}