using ElementSuite.Core.Internal;
using ElementSuite.UI.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace ElementSuite.UI.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            ExitCommand = new RelayCommand(_ => {
                Initializer.Instance.Cleanup();
                App.Current.Shutdown();
            });
            CloseTabCommand = new RelayCommand(param =>
            {
                TabItem ti = param as TabItem;
                if (ti != null)
                {

                }
            });
            AboutCommand = new RelayCommand(_ =>
            {
                var about = new About();
                about.Show();

            });
            Tabs = new ObservableCollection<Control>();
            Status = "Element Suite Started";
        }

        public ICommand ExitCommand { get; private set; }
        public ICommand CloseTabCommand { get; private set; }
        public ICommand AboutCommand { get; private set; }

        private string _status;
        public string Status
        {
            get
            {
                return _status;
            }
            set
            {
                _status = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Status"));
            }
        }

        private ObservableCollection<MenuItem> _earthAddins;
        public ObservableCollection<MenuItem> EarthAddins
        {
            get
            {
                if (_earthAddins == null)
                {
                    _earthAddins = new ObservableCollection<MenuItem>();
                    foreach (var item in Initializer.Instance.Addins)
                    {
                        if (item.Value.Launch != null && item.Value.AddinType == Addin.AddinType.Earth)
                        {
                            var addinLaunchMenuItem = item.Value.Launch.ToMenuItem();
                            foreach (var si in item.Value.MenuExtensions)
                            {
                                addinLaunchMenuItem.Items.Add(si.ToMenuItem());
                            }
                            _earthAddins.Add(addinLaunchMenuItem);
                        }
                    }
                    AddEmptyItem(_earthAddins);
                }
                return _earthAddins;
            }
            set
            {
                _earthAddins = value;
            }
        }

        private ObservableCollection<MenuItem> _windAddins;
        public ObservableCollection<MenuItem> WindAddins
        {
            get
            {
                if (_windAddins == null)
                {
                    _windAddins = new ObservableCollection<MenuItem>();
                    foreach (var item in Initializer.Instance.Addins)
                    {
                        if (item.Value.Launch != null && item.Value.AddinType == Addin.AddinType.Wind)
                        {
                            var addinLaunchMenuItem = item.Value.Launch.ToMenuItem();
                            foreach (var si in item.Value.MenuExtensions)
                            {
                                addinLaunchMenuItem.Items.Add(si.ToMenuItem());
                            }
                            _windAddins.Add(addinLaunchMenuItem);
                        }
                    }
                    AddEmptyItem(_windAddins);
                }
                return _windAddins;
            }
            set
            {
                _windAddins = value;
            }
        }

        private ObservableCollection<MenuItem> _fireAddins;
        public ObservableCollection<MenuItem> FireAddins
        {
            get
            {
                if (_fireAddins == null)
                {
                    _fireAddins = new ObservableCollection<MenuItem>();
                    foreach (var item in Initializer.Instance.Addins)
                    {
                        if (item.Value.Launch != null && item.Value.AddinType == Addin.AddinType.Fire)
                        {
                            var addinLaunchMenuItem = item.Value.Launch.ToMenuItem();
                            foreach (var si in item.Value.MenuExtensions)
                            {
                                addinLaunchMenuItem.Items.Add(si.ToMenuItem());
                            }
                            _fireAddins.Add(addinLaunchMenuItem);
                        }
                    }
                    AddEmptyItem(_fireAddins);
                }
                return _fireAddins;
            }
            set
            {
                _fireAddins = value;
            }
        }

        private ObservableCollection<MenuItem> _waterAddins;
        public ObservableCollection<MenuItem> WaterAddins
        {
            get
            {
                if (_waterAddins == null)
                {
                    _waterAddins = new ObservableCollection<MenuItem>();
                    foreach (var item in Initializer.Instance.Addins)
                    {
                        if (item.Value.Launch != null && item.Value.AddinType == Addin.AddinType.Water)
                        {
                            var addinLaunchMenuItem = item.Value.Launch.ToMenuItem();
                            foreach (var si in item.Value.MenuExtensions)
                            {
                                addinLaunchMenuItem.Items.Add(si.ToMenuItem());
                            }
                            _waterAddins.Add(addinLaunchMenuItem);
                        }
                    }
                    AddEmptyItem(_waterAddins);
                }
                return _waterAddins;
            }
            set
            {
                _waterAddins = value;
            }
        }

        /// <summary>
        /// Adds a placeholder if there are no items in the collection.
        /// </summary>
        /// <param name="addins"></param>
        private void AddEmptyItem(ICollection<MenuItem> addins)
        {
            if (addins.Count == 0)
            {
                var emptyMenuItem = new MenuItem();
                emptyMenuItem.Header = "No Addins";
                emptyMenuItem.IsEnabled = false;
                addins.Add(emptyMenuItem);
            }
        }

        public ObservableCollection<Control> Tabs { get; set; }

        public Tuple<int, int> GridA { get; set; }
        public Tuple<int, int> GridB { get; set; }
        public Tuple<int, int> GridC { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
