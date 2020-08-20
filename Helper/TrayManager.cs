﻿using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Forms;
using System.Drawing;


namespace DNS_changer.Helper
{
    public class TrayManager : Window, IDisposable, INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public TrayManager()
        {
            NotifyIcon = new NotifyIcon
            {
                Icon = Properties.Resources.systemTrayOff,
                Visible = true
            };
        }

        public void AddItemToContextStripMenu(string name, Image Image = null, EventHandler Event = null)
        {
            if (NotifyIcon.ContextMenuStrip == null)
            {
                // As this is first item, we have to create menu to hold it
                ContextMenuStrip newMenu = new ContextMenuStrip
                {
                    // Make items checked status visible
                    ShowCheckMargin = true
                };

                NotifyIcon.ContextMenuStrip = newMenu;
            }

            NotifyIcon.ContextMenuStrip.Items.Add(name, Image, Event);
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

        public ContextMenuStrip GetCurrentContextMenuStrip()
        {
            return NotifyIcon.ContextMenuStrip;
        }

        public void ActivateTray()
        {
            NotifyIcon.Icon = Properties.Resources.systemTrayOn;
        }

        public void DeactivateTray()
        {
            NotifyIcon.Icon = Properties.Resources.systemTrayOff;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        ~TrayManager()
        {
            Dispose(false);
        }

        private NotifyIcon _notifyIcon;
        public NotifyIcon NotifyIcon
        {
            get { return _notifyIcon; }
            set
            {
                _notifyIcon = value;
                NotifyPropertyChanged();
            }
        }
    }
}
