using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources;
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

namespace SourceDemo
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

        private void Smile_Click(object sender, RoutedEventArgs e)
        {
            ResourceManager srcManager = global::Source.Resource1.ResourceManager;
            byte[] buff = srcManager.GetObject("承包地块") as byte[];
            FileStream fileStr2 = new FileStream(@"C:\hr\haha.mdb", FileMode.OpenOrCreate);
            fileStr2.Write(buff, 0, buff.Length);            
            fileStr2.Close();

        }
    }
}
