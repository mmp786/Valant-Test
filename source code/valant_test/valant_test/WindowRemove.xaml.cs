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
    public partial class WindowRemove : Window, INotifyPropertyChanged
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


        private InventoryItem SelectedItem;
        private MainWindow mainView;

        public WindowRemove(MainWindow main, InventoryItem item)
        {
            InitializeComponent();
            DataContext = this;
            mainView = main;
            SelectedItem = item;
            Label = item.Label;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            mainView.IsEnabled = true;
        }

        private void btn_Remove(object sender, RoutedEventArgs e)
        {
            int amount = Convert.ToInt32(Quantity);
            if (amount <= SelectedItem.Quantity)
            {
                SelectedItem.Quantity -= amount;

                if (SelectedItem.Quantity == 0)
                {
                    mainView.Inventory.Remove(SelectedItem);
                    OnPropertyChanged("Inventory");
                }

                this.Close();
            }
            else
            {
                MessageBox.Show("The amount entered is greater than total amount in the inventory. Please try again.");
            }
        }

        private void btn_RemoveAll(object sender, RoutedEventArgs e)
        {
            mainView.Inventory.Remove(SelectedItem);
            OnPropertyChanged("Inventory");
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

        private void txt_Quantity_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key < Key.D0 || e.Key > Key.D9)
            {
                if (e.Key < Key.NumPad0 || e.Key > Key.NumPad9)
                {
                    if (e.Key != Key.Back && e.Key != Key.Delete)
                    {
                        e.Handled = true;
                        return;
                    }
                }
            }
            e.Handled = false;
        }

        private void txt_Quantity_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            textBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
        }
    }
}
