using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geometry;
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

namespace MapControlRotate
{
    public partial class Form1 : Form
    {
        private bool isRotate = false;
        public Form1()
        {
            InitializeComponent();
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
        private void Form1_Load(object sender, EventArgs e)
        {
            #region CheckLicense
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

            #endregion
            ToolBarControlAddCommands();
            #region initIcon
            IMapControl3 mapControl = (IMapControl3)axMapControl1.Object;
            Icon icon = new System.Drawing.Icon("..//..//Resources//Rotate.ico");
            mapControl.MouseIcon = (stdole.IPictureDisp)
                ESRI.ArcGIS.ADF.COMSupport.OLE.GetIPictureDispFromIcon(icon);
            #endregion
            #region setbuddy
            axToolbarControl1.SetBuddyControl(axMapControl1);
            axTOCControl1.SetBuddyControl(axMapControl1);
            #endregion
        }

        private void axMapControl1_OnMouseMove(object sender, IMapControlEvents2_OnMouseMoveEvent e)
        {
            if (!isRotate)
                return;
            IPoint curPoint = new PointClass();
            curPoint.PutCoords(e.mapX, e.mapY);

            axMapControl1.ActiveView.ScreenDisplay.RotateMoveTo(curPoint);
            axMapControl1.ActiveView.ScreenDisplay.RotateTimer();
        }
        private void axMapControl1_OnMouseDown(object sender, IMapControlEvents2_OnMouseDownEvent e)
        {
            if(e.button == 1)
            {
                isRotate = false;
                axMapControl1.Extent = axMapControl1.TrackRectangle();
            }
            else if (e.button == 2)
            {
                isRotate = true;
                IPoint curPoint = new PointClass();
                IPoint centerPoint = new PointClass();
                curPoint.PutCoords(e.mapX, e.mapY);
                centerPoint.X = axMapControl1.Extent.XMin + (axMapControl1.Extent.Width / 2);
                centerPoint.Y = axMapControl1.Extent.YMin + (axMapControl1.Extent.Height / 2);

                axMapControl1.ActiveView.ScreenDisplay.RotateStart(curPoint, centerPoint);
                axMapControl1.MousePointer = esriControlsMousePointer.esriPointerCustom;

            }
        }

        private void axMapControl1_OnMouseUp(object sender, IMapControlEvents2_OnMouseUpEvent e)
        {
            if (!isRotate)
                return;
            isRotate = false;

            double rotateAngle = axMapControl1.ActiveView.ScreenDisplay.RotateStop();
            axMapControl1.Rotation = rotateAngle;
            axMapControl1.Refresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGeography, Type.Missing, Type.Missing);

        }

        private void axMapControl1_OnAfterScreenDraw(object sender, IMapControlEvents2_OnAfterScreenDrawEvent e)
        {
            if(!isRotate)
            {
                axMapControl1.MousePointer = esriControlsMousePointer.esriPointerDefault;
            }
        }

        private void axMapControl1_OnBeforeScreenDraw(object sender, IMapControlEvents2_OnBeforeScreenDrawEvent e)
        {
            if(!isRotate)
            {
                axMapControl1.MousePointer = esriControlsMousePointer.esriPointerHourglass;
            }
        }

        
    }
}   
