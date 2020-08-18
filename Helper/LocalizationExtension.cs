using System.Windows.Data;

namespace DNS_changer.Helper
{
    public class LocalizationExtension : Binding
    {
        public LocalizationExtension(string name) : base($"[{name}]")
        {
            Mode = BindingMode.OneWay;
            Source = TranslationSource.Instance;
        }
    }
}
