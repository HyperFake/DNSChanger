using DNS_changer.Properties;
using System.ComponentModel;
using System.Globalization;
using System.Resources;

namespace DNS_changer.Helper
{
    public class TranslationSource : INotifyPropertyChanged
    {
        private static readonly TranslationSource instance = new TranslationSource();

        public static TranslationSource Instance
        {
            get { return instance; }
        }

        private readonly ResourceManager manager = Resources.ResourceManager;
        private CultureInfo currentCulture = null;

        public string this[string key]
        {
            get { return manager.GetString(key, currentCulture); }
        }

        public CultureInfo CurrentCulture
        {
            get { return currentCulture; }
            set
            {
                if(currentCulture != value)
                {
                    currentCulture = value;
                    var @event = PropertyChanged;
                    if (@event != null)
                        @event.Invoke(this, new PropertyChangedEventArgs(string.Empty));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
