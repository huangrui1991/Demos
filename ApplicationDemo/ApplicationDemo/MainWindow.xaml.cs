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

namespace ApplicationDemo
{ 
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            Document doc = new Document();
            doc.Owner = this;
            doc.Show();
            ((App)Application.Current).Documents.Add(doc);
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            foreach(Document doc in ((App)Application.Current).Documents)
            {
                doc.SetContent("refreshed at" + System.DateTime.Now.ToLongTimeString() + ".");
                ////((App)Application.Current).Documents.Remove(doc);
            }
        }
    }
}
