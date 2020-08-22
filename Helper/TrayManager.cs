using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Forms;
using System.Drawing;


namespace DNS_changer.Helper
{
    public class TrayManager : Window, IDisposable, INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        // Logging
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

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

        /// <summary>
        /// Add item to system tray menu
        /// </summary>
        /// <param name="name">Name of the item</param>
        /// <param name="Image">Image of the item</param>
        /// <param name="Event">Event of the item</param>
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

        protected virtual void Dispose(bool disposing)
        {

            _notifyIcon.Visible = false;

        }

        /// <summary>
        /// Changes NotifyIcon image to appear active
        /// </summary>
        public void ActivateTray()
        {
            try
            {
                NotifyIcon.Icon = Properties.Resources.systemTrayOn;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Failed to activate system tray");
            }
        }


        /// <summary>
        /// Changes NotifyIcon image to appear not active
        /// </summary>
        public void DeactivateTray()
        {
            try
            {
                NotifyIcon.Icon = Properties.Resources.systemTrayOff;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Failed to deactivate system tray");
            }
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
