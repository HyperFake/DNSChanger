using System;
using System.Globalization;

namespace DNS_changer.Helper
{
    public static class LanguageHelper
    {
        // Logging
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Returns saved value by current active language
        /// </summary>
        /// <param name="valueName"></param>
        /// <returns></returns>
        public static string SavedValue(string valueName)
        {
            if (valueName is null) SetLanguage(LanguageStrings.ShortEnglishUS);

            try
            {
                if (Properties.Settings.Default.Language == LanguageStrings.ShortLithuania)
                {
                    TranslationSource.Instance.CurrentCulture = new CultureInfo(LanguageStrings.ShortLithuania);
                    return Properties.Resources.ResourceManager.GetString(valueName, TranslationSource.Instance.CurrentCulture);
                }
                else if (Properties.Settings.Default.Language == LanguageStrings.ShortEnglishUS)
                {
                    TranslationSource.Instance.CurrentCulture = new CultureInfo(LanguageStrings.ShortEnglishUS);
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
