using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fantasy.ConfigTable;
using QFramework;
using UnityEngine;

namespace Lost
{
    public class ResKitConfigTableAssetBundle : IConfigTableAssetBundle
    {
        ResLoader resLoader = null;
        public void Init()
        {
            resLoader = ResLoader.Allocate();
        }
        public string Combine(string assetBundleDirectoryPath, string dataConfig)
        {
            return assetBundleDirectoryPath + "/" + dataConfig;
        }

        public byte[] LoadConfigTable(string assetBundlePath)
        {
            var strs = assetBundlePath.Split('/');
            try
            {
                var textAsset = resLoader.LoadSync<TextAsset>(strs[0], strs[1]);
                if (textAsset != null)
                {
                    return textAsset.bytes;
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }

            return null;
        }
        public void Dispose()
        {
            if (resLoader != null)
            {
                resLoader.Recycle2Cache();
                resLoader = null;
            }
        }
    }
}