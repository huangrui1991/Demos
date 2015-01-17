using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESRI.ArcGIS.Controls;


namespace AE_with_Ribbon
{
    class MapModel 
    {
        private AxMapControl _MapControl;
        private AxTOCControl _TOCControl;

        public MapModel()
        {
            _TOCControl = new AxTOCControl();
            _MapControl = new AxMapControl();
        }

        public AxMapControl MapControl
        {
            get { return _MapControl; }
            set { _MapControl = value; }
        }
        public AxTOCControl TOCControl
        {
            get { return _TOCControl; }
            set { _TOCControl = value; }
        }

    }
}
