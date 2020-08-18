using System;
using System.Globalization;

namespace DNS_changer.Helper
{
    public class LanguageHelper
    {
        // Logging
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public string SavedValue(string valueName)
        {
            try
            {
                if (Properties.Settings.Default.Language == "lt-LT")
                {
                    TranslationSource.Instance.CurrentCulture = new CultureInfo("lt-LT");
                    return Properties.Resources.ResourceManager.GetString(valueName, TranslationSource.Instance.CurrentCulture);
                }
                else if (Properties.Settings.Default.Language == "en-US")
                {
                    TranslationSource.Instance.CurrentCulture = new CultureInfo("en-US");
                    return Properties.Resources.ResourceManager.GetString(valueName, TranslationSource.Instance.CurrentCulture);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Failed to get saved language value");
            }
            return "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="language"></param>
        public void SetLanguage(string language)
        {
            try
            {
                // Update UI
                TranslationSource.Instance.CurrentCulture = new CultureInfo(language);
                // Update Code
                Properties.Settings.Default.Language = language;
                Properties.Settings.Default.Save();
            }
            catch(Exception ex)
            {
                logger.Error(ex, "Failed to set new language");
            }
        }
    }
}
