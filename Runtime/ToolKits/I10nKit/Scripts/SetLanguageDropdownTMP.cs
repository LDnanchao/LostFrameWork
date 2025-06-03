using I2.Loc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace LostFramework.I10nKit
{
    [AddComponentMenu("I2/Localization/SetLanguageDropdownTMP")]
    public class SetLanguageDropdownTMP : MonoBehaviour
    {
#if UNITY_5_2 || UNITY_5_3 || UNITY_5_4_OR_NEWER
        void OnEnable()
        {
            var dropdown = GetComponent<TMP_Dropdown>();
            if (dropdown == null)
                return;

            var currentLanguage = LocalizationManager.CurrentLanguage;
            if (LocalizationManager.Sources.Count == 0) LocalizationManager.UpdateSources();
            var languages = LocalizationManager.GetAllLanguages();

            // Fill the dropdown elements
            dropdown.ClearOptions();
            dropdown.AddOptions(languages);

            dropdown.value = languages.IndexOf(currentLanguage);
            dropdown.onValueChanged.RemoveListener(OnValueChanged);
            dropdown.onValueChanged.AddListener(OnValueChanged);
        }


        void OnValueChanged(int index)
        {
            var dropdown = GetComponent<TMP_Dropdown>();
            if (index < 0)
            {
                index = 0;
                dropdown.value = index;
            }

            LocalizationManager.CurrentLanguage = dropdown.options[index].text;
        }
#endif
    }
}
