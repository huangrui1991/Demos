using ESRI.ArcGIS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MapControlDropAndDrag
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if(!RuntimeManager.Bind(ProductCode.EngineOrDesktop))
            {
                MessageBox.Show(
                    "Unable to bind to ArcGIS runtime. Application will be shut down.");
                return;
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
