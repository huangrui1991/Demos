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
    public class FileCommands : ObservableObject
    {
        private static RelayCommand _OpenFileCommand;
        private MapModel _Model;
        
        public FileCommands()
        {
            //Init OpenFileCommand
            _Model = new MapModel();
            _OpenFileCommand = new RelayCommand(OpenFileCommand_Executed, OpenFileCommand_CanExecute);
        }

        public RelayCommand OpenFileCommand
        {
            get
            {
                return _OpenFileCommand;
            }
        }

        

        public AxMapControl MapControl
        {
            get { return _Model.MapControl; }
            set { _Model.MapControl = value; }
        }

        public string str
        {
            get { return "hehe"; }
        }

        public AxTOCControl TOCControl
        {
            get { return _Model.TOCControl; }
            set { _Model.TOCControl = value; }
        }

        public void OpenFileCommand_Executed()
        {
            string filePath = "";
               
            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
            openFileDialog.Filter = "shapeFile(*.shp)|*.shp|所有文件(*.*)|*.*";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;
            openFileDialog.ShowReadOnly = true;
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                filePath = openFileDialog.FileName;
            }
            if (filePath != "")
            {
                try
                {
                    string dialogPath = filePath.Substring(0, filePath.LastIndexOf("\\"));
                    string fileName = filePath.Substring(filePath.LastIndexOf("\\") + 1, filePath.Length - filePath.LastIndexOf("\\") - 1);
                    _Model.TOCControl.SetBuddyControl(_Model.MapControl);
                    _Model.MapControl.AddShapeFile(dialogPath, fileName);
                }
                catch(Exception e)
                {
                    MessageBox.Show(e.Message);
                }
                
            }
        }

        public bool OpenFileCommand_CanExecute()
        {
            return true; 
        }
    }
}
