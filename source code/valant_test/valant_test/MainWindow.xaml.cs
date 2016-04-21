namespace valant_test
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Navigation;
    using System.Windows.Shapes;
    using System.Windows.Threading;
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        // Create the OnPropertyChanged method to raise the event
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }        

        private InventoryItem _itemSelected;

        public InventoryItem ItemSelected
        {
            get { return _itemSelected; }
            set
            {
                if (value != _itemSelected)
                {
                    _itemSelected = value;
                    OnPropertyChanged("ItemSelected");
                }
            }
        }

        private ObservableCollection<InventoryItem> _inventory;

        public ObservableCollection<InventoryItem> Inventory
        {
            get { return _inventory; }
            set
            {
                if (value != _inventory)
                {                   
                    _inventory = value;    
                    OnPropertyChanged("Inventory");
                }
            }
        }

        private BackgroundWorker timerThread = new BackgroundWorker();
        private DispatcherTimer timerDisPatcher = new DispatcherTimer();
        private int PrevCount = 0;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            Inventory = LoadCollectionData();
            PrevCount = Inventory.Count;

            timerThread.DoWork += TimerThread_DoWork;
            timerThread.RunWorkerCompleted += TimerThread_RunWorkerCompleted;

            timerDisPatcher.Tick += new EventHandler(timerDisPatcher_Tick);
            timerDisPatcher.Interval = new TimeSpan(0, 0, 6);

            timerDisPatcher.Start();
        }

        protected void timerDisPatcher_Tick(object sender, EventArgs e)
        {
            timerThread.RunWorkerAsync();
        }

        private void TimerThread_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerDisPatcher.Start();
        }

        private void TimerThread_DoWork(object sender, DoWorkEventArgs e)
        {
            timerDisPatcher.Stop();

            List<InventoryItem> removeList = new List<InventoryItem>();
            DateTime currentTime = DateTime.Now;
            bool expireditems = false;

            if (Inventory.Count < PrevCount)
                MessageBox.Show("An item was removed for the inventory.");
            
            foreach(InventoryItem item in Inventory)
            {
                if( item.Expiration < currentTime)
                {
                    expireditems = true;
                    removeList.Add(item);
                }
            } 

            if( expireditems)
            {
                foreach (InventoryItem item in removeList)
                {
                    this.Dispatcher.BeginInvoke((Action)delegate(){ Inventory.Remove(item); } );
                }

                MessageBox.Show("One or more items have expired and been removed.");
            }

            PrevCount = Inventory.Count;
        }

        /// <summary>
        /// Inventory of Food
        /// </summary>
        /// <returns></returns>
        private ObservableCollection<InventoryItem> LoadCollectionData()
        {
            ObservableCollection<InventoryItem> Food = new ObservableCollection<InventoryItem>();
            Food.Add(new InventoryItem()
            {
                Label = "BlamCo Mac & Cheese",
                Type = "Pre-War",
                Expiration = new DateTime(3000, 12, 30),
                Quantity = 1
            });
            Food.Add(new InventoryItem()
            {
                Label = "Box of noodles",
                Type = "Pre-War",
                Expiration = new DateTime(3000, 12, 30),
                Quantity = 1
            });
            Food.Add(new InventoryItem()
            {
                Label = "Bubblegum",
                Type = "Pre-War",
                Expiration = new DateTime(2000, 12, 30),
                Quantity = 1
            });
            Food.Add(new InventoryItem()
            {
                Label = "Cram",
                Type = "Pre-War",
                Expiration = new DateTime(3000, 12, 30),
                Quantity = 1
            });
            Food.Add(new InventoryItem()
            {
                Label = "Fancy Lads Snack Cakes",
                Type = "Pre-War",
                Expiration = new DateTime(3000, 12, 30),
                Quantity = 1
            });
            Food.Add(new InventoryItem()
            {
                Label = "InstaMash",
                Type = "Pre-War",
                Expiration = new DateTime(3000, 12, 30),
                Quantity = 1
            });
            return Food;
        }

        private void btn_Add(object sender, RoutedEventArgs e)
        {
            WindowAdd win = new WindowAdd(this);
            win.Show();
            this.IsEnabled = false;
        }

        private void btn_Remove(object sender, RoutedEventArgs e)
        {
            WindowRemove win = new WindowRemove(this, ItemSelected);
            win.Show();
            this.IsEnabled = false;
        }
    }
}
