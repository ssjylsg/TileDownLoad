using DevComponents.DotNetBar;

namespace NPMapTiles
{
    using MapDataTools.Util;

    public partial class FrmProperty : Office2007Form
    {
        public FrmProperty(WorkInfo workInfo)
        {
            InitializeComponent();
            this.txbMaxX.Text = workInfo.maxX.ToString();
            this.txbMaxY.Text = workInfo.maxY.ToString();
            this.txbMinX.Text = workInfo.minX.ToString();
            this.txbMinY.Text = workInfo.minY.ToString();
            this.txbCenterPoint.Text = ((workInfo.minX + workInfo.maxX) / 2).ToString() + "," + ((workInfo.minY + workInfo.maxY) / 2).ToString();
            int minZoom=0;
            int maxZoom=0;
            if (workInfo.rcList.Count > 0)
            {
                minZoom = workInfo.rcList[0].zoom;
                maxZoom = workInfo.rcList[0].zoom;
            }
            foreach (RowColumns rc in workInfo.rcList)
            {
                if (minZoom > rc.zoom)
                {
                    minZoom = rc.zoom;
                }
                if (maxZoom < rc.zoom)
                {
                    maxZoom = rc.zoom;
                }
            }
            txbMaxZoom.Text = maxZoom.ToString();
            txbMinZoom.Text = minZoom.ToString();
            txbPath.Text = workInfo.filePath + "\\s";
        }
    }
}
