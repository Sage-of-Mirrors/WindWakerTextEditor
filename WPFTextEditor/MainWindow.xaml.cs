using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Xceed.Wpf.Toolkit;

namespace WPFTextEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            BoxTypeCombo.ItemsSource = Enum.GetValues(typeof(BoxTypes)).Cast<BoxTypes>();

            BoxPositionCombo.ItemsSource = Enum.GetValues(typeof(BoxPositions)).Cast<BoxPositions>();

            ItemIDBox.ItemsSource = Enum.GetValues(typeof(ItemIDValue)).Cast<ItemIDValue>();
        }

        private void SearchBoxInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ViewModel view = (ViewModel)DataContext;

                view.Search();
            }
        }

        private void TextBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            ViewModel view = (ViewModel)DataContext;
            TextBox box = sender as TextBox;
            view.TextBoxPos = box.SelectionStart;
        }
    }
}
