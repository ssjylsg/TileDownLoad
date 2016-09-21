using System;

using DevComponents.DotNetBar;
using System.IO;
using System.Diagnostics;
namespace NPMapTiles
{
    public partial class FrmCaption : Office2007Form
    {
        public FrmCaption()
        {
            InitializeComponent();
            try
            {
                System.Reflection.Assembly ma = System.Reflection.Assembly.GetEntryAssembly();
                FileInfo fi = new FileInfo(ma.Location);
                FileVersionInfo mfv = FileVersionInfo.GetVersionInfo(ma.Location);
                labVersion.Text = "V" + mfv.FileVersion;
            }
            catch (Exception ex)
            {
                log4net.LogManager.GetLogger(this.GetType()).Error(ex.Message);
            }
        }
    }
}
