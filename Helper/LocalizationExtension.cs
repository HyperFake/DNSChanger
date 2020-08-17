using System.Windows.Data;

namespace DNS_changer.Helper
{
    public class LocalizationExtension : Binding
    {
        public LocalizationExtension(string name) : base($"[{name}]")
        {
            this.Mode = BindingMode.OneWay;
            this.Source = TranslationSource.Instance;
        }
    }
}
