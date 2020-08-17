using System.Globalization;

namespace DNS_changer.Helper
{
    public class LanguageHelper
    {
        public string ReturnValue(string valueName)
        {
            if(Properties.Settings.Default.Language == "lt-LT")
            {
                TranslationSource.Instance.CurrentCulture = new CultureInfo("lt-LT");
                return Properties.Resources.ResourceManager.GetString(valueName, TranslationSource.Instance.CurrentCulture);
            }
            else if(Properties.Settings.Default.Language == "en-US")
            {
                TranslationSource.Instance.CurrentCulture = new CultureInfo("en-US");
                return Properties.Resources.ResourceManager.GetString(valueName, TranslationSource.Instance.CurrentCulture);
            }
            return "";
        }

        public void SetLanguage(string language)
        {
            // Update UI
            TranslationSource.Instance.CurrentCulture = new CultureInfo(language);
            // Update Code
            Properties.Settings.Default.Language = language;
            Properties.Settings.Default.Save();
        }
    }
}
