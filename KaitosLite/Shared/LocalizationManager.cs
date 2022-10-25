using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ViewLayer.Interfaces;

namespace ViewLayer.Shared
{
    public class LocalizationManager : ILocalizationManager
    {
        private ResourceDictionary _localization;
        public LocalizationManager()
        {
            _localization = Application.Current.Resources;
        }
        public string GetByKey(string key, string[] paras = null)
        {
            string textValue = (string)_localization[key];

            if (paras != null)
                textValue = FormatString(textValue, paras);

            return textValue;
        }
        private string FormatString(string textValue,string[] paras)
        {
            for (int i = 0; i < paras.Length; i++)
            {
                textValue = textValue.Replace("{"+i+"}", paras[i]);
            }
            
            return textValue;
        }
    }
}
