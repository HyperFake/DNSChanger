using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;


namespace DNS_changer.Models
{
    class TrayManager : Window, IDisposable, INotifyPropertyChanged
    {

        private NotifyIcon _notifyIcon;

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public NotifyIcon NotifyIcon
        {
            get { return _notifyIcon; }
            set
            {
                _notifyIcon = value;
                NotifyPropertyChanged();
            }
        }

        public TrayManager()
        {
            if(NotifyIcon == null)
            {
                NotifyIcon = new NotifyIcon
                {
                    Icon = Properties.Resources.NotActiveKeksiukai,
                    Visible = true
                };
            }

        }

        public void SetToolTipText(string text)
        {
            _notifyIcon.Text = text;
        }

        protected virtual void Dispose(bool disposing)
        {
            _notifyIcon.Visible = false;
        }

        protected virtual void HideIcon()
        {
            _notifyIcon.Visible = false;
        }

        protected virtual void ShowIcon()
        {
            _notifyIcon.Visible = true;
        }

        public void Active()
        {
            if(NotifyIcon != null)
            {
                ContextMenuStrip menu = new ContextMenuStrip();
                menu.Items.Add("Active", null, null);
                _notifyIcon.ContextMenuStrip = menu;
            }
        }

        public void NotActive()
        {
            ContextMenuStrip menu = new ContextMenuStrip();
            menu.Items.Add("NotActive", null, null);
            _notifyIcon.ContextMenuStrip = menu;
        }

        public void Dispose()
        {
            Dispose(true);
        }
        ~TrayManager()
        {
            Dispose(false);
        }
    }
}
