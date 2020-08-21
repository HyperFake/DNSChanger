using System;
using System.Globalization;

namespace DNS_changer.Helper
{
    public static class LanguageHelper
    {
        // Logging
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public static string SavedValue(string valueName)
        {
            if (valueName is null) SetLanguage("en-US");

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
        /// Sets the language of the app
        /// </summary>
        /// <param name="language"></param>
        public static void SetLanguage(string language)
        {
            try
            {
                // Update UI
                TranslationSource.Instance.CurrentCulture = new CultureInfo(language);
                // Update Code
                Properties.Settings.Default.Language = language;
                Properties.Settings.Default.Save();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Failed to set new language");
            }
        }

        /// <summary>
        /// Gets current language short string
        /// </summary>
        /// <returns>Current language string</returns>
        public static string GetLanguage()
        {
            string returnString = "";
            try
            {
                returnString = Properties.Settings.Default.Language;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Failed to get current language");
            }
            return returnString;
        }
    }
}
