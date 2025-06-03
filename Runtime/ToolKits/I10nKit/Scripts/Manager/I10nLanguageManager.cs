using I2.Loc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LostFramework.I10nKit
{
    internal class I10nLanguageManager
    {
        ResLoader resLoader = ResLoader.Allocate();
        CustomBundlesManager resm;
        LanguageSource language;

        public void Init()
        {
            resm = new CustomBundlesManager();
            ResourceManager.pInstance.mBundleManagers.Add(resm);
            ResKit.Init();
        }

        public void LoadLanguage(string assetName)
        {
            var go = resLoader.LoadSync<GameObject>("LanguageSource");
            if(language != null)
            {
                GameObject.Destroy(language.gameObject);
                language = null;
            }
            language = GameObject.Instantiate(go.gameObject).GetComponent<LanguageSource>();
            SetDefualteLanguage(language);
            resLoader.Recycle2Cache();
        }
        public void SetDefualteLanguage(LanguageSource language)
        {
            //var go = resLoader.LoadSync<GameObject>("LanguageSource");
            //language = Instantiate(go.gameObject).GetComponent<LanguageSource>();
            LocalizationManager.Sources[0] = language.SourceData;
            //LocalizationManager.UpdateSources();
            LocalizationManager.LocalizeAll();
        }

        internal void Dispose()
        {
            resLoader.Recycle2Cache();
            resLoader.Dispose();
            resLoader = null;
            resm.Dispose();
            if(language!=null)
            {
                GameObject.Destroy(language.gameObject);
                LocalizationManager.UpdateSources();
            }
        }
    }

    public class CustomBundlesManager : IResourceManager_Bundles
    {
        private static Type ComponentType = typeof(Component);
        private static Type GameObjectType = typeof(GameObject);
        ResLoader resLoader;
        public UnityEngine.Object LoadFromBundle(string path, Type assetType)
        {
            Debug.Log(path);

            var type = assetType;
            if (ComponentType.IsAssignableFrom(type))
            {
                var resSearchKeys = ResSearchKeys.Allocate(path, null, GameObjectType);
                var retAsset = (resLoader.LoadAssetSync(resSearchKeys) as GameObject)?.GetComponent(assetType);
                resSearchKeys.Recycle2Cache();
                return retAsset;
            }
            else
            {
                var resSearchKeys = ResSearchKeys.Allocate(path, null, type);
                var retAsset = resLoader.LoadAssetSync(resSearchKeys);
                resSearchKeys.Recycle2Cache();
                return retAsset;
            }
        }

        public void Init()
        {
            resLoader = ResLoader.Allocate();
        }

        public void Dispose()
        {
            if (resLoader != null)
            {
                resLoader.Recycle2Cache();
            }
            resLoader = null;
        }
    }

}
