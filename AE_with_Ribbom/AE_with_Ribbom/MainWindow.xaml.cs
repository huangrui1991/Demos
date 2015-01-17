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
using ESRI.ArcGIS.Controls;

namespace AE_with_Ribbon
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Fluent.RibbonWindow
    {
        FileCommands command;

        public MainWindow()
        {
            command = new FileCommands();
            InitializeComponent();
            mapHost.Child = command.MapControl;
            tocHost.Child = command.TOCControl;
            command.TOCControl.SetBuddyControl(command.MapControl);
        }

    }
}
