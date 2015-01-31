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
using System.Windows.Shapes;

namespace GuideDialogDemo
{
    /// <summary>
    /// Interaction logic for Dialog.xaml
    /// </summary>
    public partial class Dialog : Window
    {
        public Dialog()
        {
            InitializeComponent();
        }

        private void NextStep_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.DialogPosition = this.RestoreBounds;
            Properties.Settings.Default.Save();

            Dialog d = new Dialog();
            d.Top = Properties.Settings.Default.DialogPosition.Top;
            d.Left = Properties.Settings.Default.DialogPosition.Left;
            this.Visibility = System.Windows.Visibility.Hidden;
            d.Owner = this;
            d.ShowDialog();
        }
    }
}
