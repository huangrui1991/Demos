﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using ESRI.ArcGIS.esriSystem;

namespace AE_with_Ribbon
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            ESRI.ArcGIS.RuntimeManager.Bind(ESRI.ArcGIS.ProductCode.EngineOrDesktop);
            InitializeEngineLicense();
        }

        private void InitializeEngineLicense()
        {
            AoInitialize aoi = new AoInitializeClass();

            //more license choices could be included here
            esriLicenseProductCode productCode = esriLicenseProductCode.esriLicenseProductCodeEngine;
            if (aoi.IsProductCodeAvailable(productCode) == esriLicenseStatus.esriLicenseAvailable)
            {
                aoi.Initialize(productCode);
            }
        }
    }
}
