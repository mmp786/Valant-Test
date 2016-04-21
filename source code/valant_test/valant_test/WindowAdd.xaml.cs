using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace valant_test
{
   

    /// <summary>
    /// Interaction logic for WindowAdd.xaml
    /// </summary>
    public partial class WindowAdd : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        // Create the OnPropertyChanged method to raise the event
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private string _label;

        public string Label
        {
            get { return _label; }
            set
            {
                if (value != _label)
                {
                    _label = value;
                    OnPropertyChanged("Label");
                }
            }
        }

        private string _type;

        public string Type
        {
            get { return _type; }
            set
            {
                if (value != _type)
                {
                    _type = value;
                    OnPropertyChanged("Type");
                }
            }
        }

        private string _quantity;

        public string Quantity
        {
            get { return _quantity; }
            set
            {
                if (value != _quantity)
                {
                    _quantity = value;
                    OnPropertyChanged("Quantity");
                }
            }
        }

        private DateTime _expiration;

        public DateTime Expiration
        {
            get { return _expiration; }
            set
            {
                if (value != _expiration)
                {
                    _expiration = value;
                    OnPropertyChanged("Expiration");
                }
            }
        }

        private MainWindow mainView;

        public WindowAdd( MainWindow main)
        {
            InitializeComponent();
            DataContext = this;
            mainView = main;
            Expiration = DateTime.Now.Date;
        }
        
        private void Window_Closed(object sender, EventArgs e)
        {
            mainView.IsEnabled = true;
            mainView.dG_Inventory.Items.Refresh();
        }

        private void btn_Confirm(object sender, RoutedEventArgs e)
        {
            if (!mainView.Inventory.Any(x => x.Label.ToLower() == Label.ToLower() || x.Type.ToLower() == Type.ToLower()))
            { 
                mainView.Inventory.Add(new InventoryItem
                {
                    Expiration = Expiration,
                    Label = Label,
                    Quantity = Convert.ToInt32(Quantity),
                    Type = Type
                });
            }
            else
            {
                InventoryItem item = mainView.Inventory.First(x => x.Label.ToLower() == Label.ToLower() && x.Type.ToLower() == Type.ToLower());
                item.Quantity += Convert.ToInt32(Quantity);
            }
            this.Close();
        }

        private void btn_Close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            char c = Convert.ToChar(e.Text);
            if (Char.IsNumber(c))
                e.Handled = false;
            else
                e.Handled = true;

            base.OnPreviewTextInput(e);
        }
    }
}
