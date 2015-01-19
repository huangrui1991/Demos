using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.SystemUI;

namespace MapControlDropAndDrag
{
    public partial class Form1 : Form
    {
        private esriControlsDragDropEffect _Effect;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            axMapControl1.OleDropEnabled = true;
        }

        private void axMapControl1_OnOleDrop(object sender, ESRI.ArcGIS.Controls.IMapControlEvents2_OnOleDropEvent e)
        {
            IDataObjectHelper dataObjectHelper = (IDataObjectHelper)e.dataObjectHelper;
            esriControlsDropAction action = e.dropAction;
            e.effect = (int)esriControlsDragDropEffect.esriDragDropNone;

            if (action == esriControlsDropAction.esriDropEnter)
            {
                if (dataObjectHelper.CanGetFiles() || dataObjectHelper.CanGetNames())
                {
                    _Effect = esriControlsDragDropEffect.esriDragDropCopy;
                }
            }
            else if (action == esriControlsDropAction.esriDropOver)
            {
                e.effect = (int)_Effect;
            }
            else if (action == esriControlsDropAction.esriDropped)
            {
                if (dataObjectHelper.CanGetFiles() == true)
                {
                    System.Array filePaths = System.Array.CreateInstance(typeof(string), 0, 0);
                    filePaths = (System.Array)dataObjectHelper.GetFiles();

                    for (int i = 0; i < filePaths.Length; i++)
                    {
                        if (axMapControl1.CheckMxFile(filePaths.GetValue(i).ToString()) == true)
                        {
                            try
                            {
                                axMapControl1.LoadMxFile(filePaths.GetValue(i).ToString(), Type.Missing, "");
                            }
                            catch (System.Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                                return;
                            }
                        }
                        else
                        {
                            IFileName fileName = new FileNameClass();
                            fileName.Path = filePaths.GetValue(i).ToString();
                            CreateLayer((IName)fileName);
                        }
                    }

                }
                else if (dataObjectHelper.CanGetNames() == true)
                {
                    //Get the IEnumName interface through the IDataObjectHelper.
                    IEnumName enumName = dataObjectHelper.GetNames();
                    enumName.Reset();
                    //Get the IName interface.
                    IName name = enumName.Next();
                    //Loop through the names.
                    while (name != null)
                    {
                        //Create a map layer.
                        CreateLayer(name);
                        name = enumName.Next();
                    }
                }
            }
        }

        private void CreateLayer(IName name)
        {
            axMapControl1.MousePointer = esriControlsMousePointer.esriPointerHourglass;

            ILayerFactoryHelper FatoryHelper = new LayerFactoryHelperClass();
            try
            {
                IEnumLayer Layers = FatoryHelper.CreateLayersFromName(name);
                Layers.Reset();
                ILayer layer = Layers.Next();
                while (layer != null)
                {
                    axMapControl1.AddLayer(layer, 0);
                    layer = Layers.Next();
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            axMapControl1.MousePointer = esriControlsMousePointer.esriPointerDefault;

        }
    }
}
