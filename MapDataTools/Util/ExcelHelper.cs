using System;
using System.Data;
using System.Data.OleDb;
using System.Runtime.InteropServices;
using System.Windows.Forms;
namespace MapDataTools
{
    public class ExcelHelper
    {
        //然后写API引用部分的代码，放入 class 内部
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern int GetWindowThreadProcessId(IntPtr hwnd, out int ID);
        //把数据表的内容导出到Excel文件中
        public static void OutDataToExcel(System.Data.DataTable srcDataTable, string excelFilePath)
        {
            Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
            object missing = System.Reflection.Missing.Value;

            //导出到execl 
            try
            {
                if (xlApp == null)
                {
                    MessageBox.Show("无法创建Excel对象，可能您的电脑未安装Excel!");
                    return;
                }

                Microsoft.Office.Interop.Excel.Workbooks xlBooks = xlApp.Workbooks;
                Microsoft.Office.Interop.Excel.Workbook xlBook = xlBooks.Add(Microsoft.Office.Interop.Excel.XlWBATemplate.xlWBATWorksheet);
                Microsoft.Office.Interop.Excel.Worksheet xlSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlBook.Worksheets[1];

                //让后台执行设置为不可见，为true的话会看到打开一个Excel，然后数据在往里写
                xlApp.Visible = false;

                object[,] objData = new object[srcDataTable.Rows.Count + 1, srcDataTable.Columns.Count];
                //首先将数据写入到一个二维数组中
                for (int i = 0; i < srcDataTable.Columns.Count; i++)
                {
                    objData[0, i] = srcDataTable.Columns[i].ColumnName;
                }
                if (srcDataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < srcDataTable.Rows.Count; i++)
                    {
                        if (srcDataTable.Rows[i] == null)
                        {
                            continue;
                        }
                        for (int j = 0; j < srcDataTable.Columns.Count; j++)
                        {
                            objData[i + 1, j] = srcDataTable.Rows[i][j];
                        }
                    }
                }

                string startCol = "A";
                int iCnt = (srcDataTable.Columns.Count / 26);
                string endColSignal = (iCnt == 0 ? "" : ((char)('A' + (iCnt - 1))).ToString());
                string endCol = endColSignal + ((char)('A' + srcDataTable.Columns.Count - iCnt * 26 - 1)).ToString();
                Microsoft.Office.Interop.Excel.Range range = xlSheet.get_Range(startCol + "1", endCol + (srcDataTable.Rows.Count - iCnt * 26 + 1).ToString());

                range.Value = objData; //给Exccel中的Range整体赋值
                range.EntireColumn.AutoFit(); //设定Excel列宽度自适应
                xlSheet.get_Range(startCol + "1", endCol + "1").Font.Bold = 1;//Excel文件列名 字体设定为Bold

                //设置禁止弹出保存和覆盖的询问提示框
                xlApp.DisplayAlerts = false;
                xlApp.AlertBeforeOverwriting = false;

                if (xlSheet != null)
                {
                    xlSheet.SaveAs(excelFilePath, missing, missing, missing, missing, missing, missing, missing, missing, missing);
                    xlApp.Quit();
                    KillSpecialExcel(xlApp);
                }
            }
            catch
            {
                xlApp.Quit();
                KillSpecialExcel(xlApp);
            }
        }

        public static void KillSpecialExcel(Microsoft.Office.Interop.Excel.Application Excel)//杀死线程
        {
            try
            {
                IntPtr intptr = new IntPtr(Excel.Hwnd);
                int lpdwProcessId;
                GetWindowThreadProcessId(intptr, out lpdwProcessId);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static DataSet ToDatasetFormEx(string path)
        {
            string strCon = " Provider = Microsoft.Jet.OLEDB.4.0 ; Data Source = " + path + ";Extended Properties=Excel 8.0";
            OleDbConnection myConn = new OleDbConnection(strCon);
            string strCom = " SELECT * FROM [Sheet1$] ";
            myConn.Open();
            OleDbDataAdapter myCommand = new OleDbDataAdapter(strCom, myConn);
            DataSet myDataSet = new DataSet();
            myCommand.Fill(myDataSet, "[Sheet1$]");
            myConn.Close();
            return myDataSet;
        }
    }
}
