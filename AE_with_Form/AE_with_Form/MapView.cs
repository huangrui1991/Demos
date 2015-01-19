using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.SystemUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AE_with_Form
{
    public partial class MapView : Form
    {
        public MapView()
        {
            InitializeComponent();
        }

        #region menber
        IToolbarMenu m_ToolbarMenu;
        #endregion

        #region Function

        private void LicenseCheck()
        {
            if (axLicenseControl1.Status != esriLicenseStatus.esriLicenseCheckedOut)
            {
                string failure = "License initialization failed for the following reason:" +
                    Environment.NewLine + axLicenseControl1.Summary + Environment.NewLine +
                    Environment.NewLine + "The status" +
                    "of the requested license(s) is as follows:";

                //get the status of the requested licenses.
                string[] availability = axLicenseControl1.get_LicenseAvailability
                    (esriLicenseStatusOptions.esriLicenseStatusRequested).Split
                    (Environment.NewLine.ToCharArray());
                for (int i = 0; i <= availability.Length - 1; i++)
                {
                    failure = failure + Environment.NewLine + availability[i];
                }

                //Report failure to user.
                System.Windows.Forms.MessageBox.Show(failure, "LicenseControl",
                    MessageBoxButtons.OK);

                //Programmatically shut down the application...
            }
        }
        private void ToolBarControlAddCommands()
        {
            axToolbarControl1.AddItem("esriControls.ControlsOpenDocCommand", -1, -1, 
                false, 0, esriCommandStyles.esriCommandStyleIconOnly);
            axToolbarControl1.AddItem("esriControls.ControlsAddDataCommand", -1, -1,
                false, 0, esriCommandStyles.esriCommandStyleIconOnly);
            //Add page layout navigation commands.
            axToolbarControl1.AddItem("esriControls.ControlsPageZoomInTool", -1, -1,
                true, 0, esriCommandStyles.esriCommandStyleIconOnly);
            axToolbarControl1.AddItem("esriControls.ControlsPageZoomOutTool", -1, -1,
                false, 0, esriCommandStyles.esriCommandStyleIconOnly);
            axToolbarControl1.AddItem("esriControls.ControlsPagePanTool", -1, -1, false,
                0, esriCommandStyles.esriCommandStyleIconOnly);
            axToolbarControl1.AddItem("esriControls.ControlsPageZoomWholePageCommand", -1,
                -1, false, 0, esriCommandStyles.esriCommandStyleIconOnly);
            //Add map navigation commands.
            axToolbarControl1.AddItem("esriControls.ControlsMapZoomInTool", -1, -1, true,
                0, esriCommandStyles.esriCommandStyleIconOnly);
            axToolbarControl1.AddItem("esriControls.ControlsMapZoomOutTool", -1, -1,
                false, 0, esriCommandStyles.esriCommandStyleIconOnly);
            axToolbarControl1.AddItem("esriControls.ControlsMapPanTool", -1, -1, false,
                0, esriCommandStyles.esriCommandStyleIconOnly);
            axToolbarControl1.AddItem("esriControls.ControlsMapFullExtentCommand", -1, -
                1, false, 0, esriCommandStyles.esriCommandStyleIconOnly);
            axToolbarControl1.AddItem("esriControls.ControlsMapZoomToLastExtentBackCommand",
                -1, -1, false, 0, esriCommandStyles.esriCommandStyleIconOnly);
            axToolbarControl1.AddItem(
                "esriControls.ControlsMapZoomToLastExtentForwardCommand", -1, -1, false,
                0, esriCommandStyles.esriCommandStyleIconOnly);
            //Add map inquiry commands.
            axToolbarControl1.AddItem("esriControls.ControlsMapIdentifyTool", -1, -1,
                true, 0, esriCommandStyles.esriCommandStyleIconOnly);
            axToolbarControl1.AddItem("esriControls.ControlsMapFindCommand", -1, -1,
                false, 0, esriCommandStyles.esriCommandStyleIconOnly);
            axToolbarControl1.AddItem("esriControls.ControlsMapMeasureTool", -1, -1,
                false, 0, esriCommandStyles.esriCommandStyleIconOnly);
        }

        private void InitToolbarMenu() 
        {
            m_ToolbarMenu = new ESRI.ArcGIS.Controls.ToolbarMenuClass();
            //Share the ToolbarControl's command pool.
            m_ToolbarMenu.CommandPool = axToolbarControl1.CommandPool;
            //Set the hook to the PageLayoutControl.
            m_ToolbarMenu.SetHook(axPageLayoutControl1);
            //Add commands to the ToolbarMenu.
            m_ToolbarMenu.AddItem("esriControls.ControlsPageZoomInFixedCommand", -1, -1,
                false, esriCommandStyles.esriCommandStyleIconAndText);
            m_ToolbarMenu.AddItem("esriControls.ControlsPageZoomOutFixedCommand", -1, -1,
                false, esriCommandStyles.esriCommandStyleIconAndText);
            m_ToolbarMenu.AddItem("esriControls.ControlsPageZoomWholePageCommand", -1, -
                1, false, esriCommandStyles.esriCommandStyleIconAndText);
            m_ToolbarMenu.AddItem("esriControls.ControlsPageZoomPageToLastExtentBackCommand",
                -1, -1, true, esriCommandStyles.esriCommandStyleIconAndText);
            m_ToolbarMenu.AddItem(
                "esriControls.ControlsPageZoomPageToLastExtentForwardCommand", -1, -1,
                false, esriCommandStyles.esriCommandStyleIconAndText);
        }

        private void InitPalette()
        {
            IToolbarPalette toolbarPalette = new ToolbarPaletteClass();
            toolbarPalette.AddItem("esriControls.ControlsNewMarkerTool", -1, -1);
            toolbarPalette.AddItem("esriControls.ControlsNewLineTool", -1, -1);
            toolbarPalette.AddItem("esriControls.ControlsNewCircleTool", -1, -1);
            toolbarPalette.AddItem("esriControls.ControlsNewEllipseTool", -1, -1);
            toolbarPalette.AddItem("esriControls.ControlsNewRectangleTool", -1, -1);
            toolbarPalette.AddItem("esriControls.ControlsNewPolygonTool", -1, -1);

            //Add the ToolbarPalette to the ToolbarControl.
            axToolbarControl1.AddItem(toolbarPalette, 0, -1, false, 0,
            esriCommandStyles.esriCommandStyleIconOnly);
        }

        private void SetBuddy()
        {
            axTOCControl1.SetBuddyControl(axPageLayoutControl1);
            axToolbarControl1.SetBuddyControl(axPageLayoutControl1);
        }
        #endregion

        #region Handles
        private void MapView_Load(object sender, EventArgs e)
        {
            LicenseCheck();
            ToolBarControlAddCommands();
            InitToolbarMenu();
            InitPalette();
            axTOCControl1.LabelEdit = esriTOCControlEdit.esriTOCControlManual;
            axMapControl1.AutoMouseWheel = true;
            axMapControl1.AutoKeyboardScrolling = true;
            axMapControl1.KeyIntercept = (int)esriKeyIntercept.esriKeyInterceptArrowKeys;
            SetBuddy();
            string filePath = @"C:\hr\experiment\gis数据\arcgis shp 武汉数据\电子地图.mxd";
            if (axPageLayoutControl1.CheckMxFile(filePath))
            {
                axPageLayoutControl1.LoadMxFile(filePath);
            }
        }

        private void axPageLayoutControl1_OnPageLayoutReplaced(object sender, ESRI.ArcGIS.Controls.IPageLayoutControlEvents_OnPageLayoutReplacedEvent e)
        {
            axMapControl1.LoadMxFile(axPageLayoutControl1.DocumentFilename, null, null);
            axMapControl1.Extent = axMapControl1.FullExtent;
        }

        private void MapView_ResizeBegin(object sender, EventArgs e)
        {
            axPageLayoutControl1.SuppressResizeDrawing(true, 0);
            axMapControl1.SuppressResizeDrawing(true, 0);
        }

        private void MapView_ResizeEnd(object sender, EventArgs e)
        {
            axPageLayoutControl1.SuppressResizeDrawing(false, 0);
            axMapControl1.SuppressResizeDrawing(false, 0);
        }

        private void axPageLayoutControl1_OnMouseDown(object sender, IPageLayoutControlEvents_OnMouseDownEvent e)
        {
            if (e.button == 2)
            {
                m_ToolbarMenu.PopupMenu(e.x, e.y, axPageLayoutControl1.hWnd);
            }
        }

        private void axTOCControl1_OnEndLabelEdit(object sender, ITOCControlEvents_OnEndLabelEditEvent e)
        {
            if (e.newLabel.Trim() == "")
                e.canEdit = false;
        }
        #endregion

        

        
    }
}
