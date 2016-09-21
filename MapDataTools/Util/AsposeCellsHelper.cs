namespace MapDataTools
{
    using System;
    using System.Data;
    using System.IO;
    using System.Linq;

    using Aspose.Cells;

    /// <summary>
    /// 第三方组件Aspose导出excel 2007文件,不依赖于微软的office　组件
    /// </summary>
    public class AsposeCellsHelper
    {
        /// <summary>
        /// 获取Worksheets[0]列数
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static int GetCellsColumnsCount(string fileName)
        {
            Workbook workbook = new Workbook();
            workbook.Open(fileName);
            Cells cells = workbook.Worksheets[0].Cells;
            return cells.MaxColumn + 1;
        }
        /// <summary>
        /// 获取Worksheets[0]行数
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static int GetCellsRowsCount(string fileName)
        {
            Workbook workbook = new Workbook();
            workbook.Open(fileName);
            Cells cells = workbook.Worksheets[0].Cells;
            return cells.MaxDataRow + 1;
        }

        /// <summary>
        /// 读取Excel文件到DataTable
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static DataTable ExportToDataTable(string fileName)
        {
            return ExportToDataTable(fileName, true);
        }

        public static string[] GetFileColumns(string fileName)
        {
            Workbook workbook = new Workbook();
            workbook.Open(fileName);
            Cells cells = workbook.Worksheets[0].Cells;
            //这里取的是列和行的索引值，所以实际值需要+1
            int columnsCount = cells.MaxColumn + 1;
            int rowCount = cells.MaxDataRow + 1;
            string[] columns = new string[columnsCount];
            if (rowCount > 0 && columnsCount > 0)
            {
                for (int k = 0; k < columnsCount; k++)
                {
                    columns[k] = (cells[0, k].StringValue.Trim()).ToUpper();
                }
            }
            return columns;
        }

        public static bool IsContainsCloumnName(string file,string column)
        {
            return GetFileColumns(file).Contains(column);
        }
        /// <summary>
        /// 读取数据到DataTable
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="isHaveColumnName"></param>
        /// <returns></returns>
        public static DataTable ExportToDataTable(string fileName,bool isHaveColumnName)
        {
            DataTable dtb = new DataTable();
            Workbook workbook = new Workbook();
            workbook.Open(fileName);
            Cells cells = workbook.Worksheets[0].Cells;
            //这里取的是列和行的索引值，所以实际值需要+1
            int columnsCount = cells.MaxColumn + 1;
            int rowCount = cells.MaxDataRow + 1;

            if (rowCount > 0 && columnsCount > 0)
            {
                for (int k = 0; k < columnsCount; k++)
                {
                    if (isHaveColumnName)
                    {
                        dtb.Columns.Add(cells[0,k].StringValue.Trim().ToUpper());
                    }
                    else
                        dtb.Columns.Add("Column" + k);
                }

                int index = 0;
                if (isHaveColumnName)
                    index = 1;
                for (int i = index; i < rowCount; i++)
                {
                    DataRow dtr = dtb.NewRow();
                    for (int j = 0; j < columnsCount; j++)
                    {
                        dtr[j] = cells[i, j].StringValue.Trim();//读取单元格数据添加到行
                    }
                    dtb.Rows.Add(dtr);
                }
            }
            return dtb;
        }
      
        /// <summary>
        ///DataTable导出到Excel文件
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="fileName"></param>
        public static void ExportToExcel(DataTable dt, string fileName, bool isTextWrapped = false)
        {
            Aspose.Cells.Worksheet sheet;
            Aspose.Cells.Workbook book = new Aspose.Cells.Workbook();
            sheet = book.Worksheets[0];
            sheet.IsGridlinesVisible = true;
            //AddBody(dt);

            if (dt.Rows.Count == 0)
            {
                for (int c = 0; c < dt.Columns.Count; c++)
                {
                    sheet.Cells[0, c].PutValue(dt.Columns[c].ColumnName);//列标题
                }
            }
            else
            {
                for (int r = 0; r < dt.Rows.Count; r++)
                {
                    for (int c = 0; c < dt.Columns.Count; c++)
                    {
                        sheet.Cells[0, c].PutValue(dt.Columns[c].ColumnName);//列标题
                        sheet.Cells[r + 1, c].PutValue(dt.Rows[r][c].ToString());
                        if (isTextWrapped && sheet.Cells[r + 1, c].Style != null)
                        {
                            sheet.Cells[r + 1, c].Style.IsTextWrapped = true;
                        }
                    }
                }
            }

            sheet.AutoFitColumns();
            sheet.AutoFitRows();
            book.Save(fileName, Aspose.Cells.FileFormatType.Excel2007Xlsx);

        }

        /// <summary>
        ///DataTable导出到Excel文件 
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="fileName"></param>
        /// <param name="linkTable">超链接信息</param>
        /// <param name="columnWidth">列宽</param>
        /// <param name="isTextWrapped">是否自动换行</param>
        public static void ExportToExcel(DataTable dt, string fileName,DataTable linkTable,int? columnWidth, bool isTextWrapped = false)
        {
            Aspose.Cells.Worksheet sheet;
            Aspose.Cells.Workbook book = new Aspose.Cells.Workbook();
            sheet = book.Worksheets[0];
            sheet.IsGridlinesVisible = true;
            //AddBody(dt);

            if (dt.Rows.Count == 0)
            {
                for (int c = 0; c < dt.Columns.Count; c++)
                {
                    sheet.Cells[0, c].PutValue(dt.Columns[c].ColumnName);//列标题
                }
            }
            else
            {
                for (int r = 0; r < dt.Rows.Count; r++)
                {
                    for (int c = 0; c < dt.Columns.Count; c++)
                    {
                        string columnName = dt.Columns[c].ColumnName;
                        sheet.Cells[0, c].PutValue(columnName);//列标题
                        sheet.Cells[r + 1, c].PutValue(dt.Rows[r][c].ToString());
                        if (isTextWrapped && sheet.Cells[r + 1, c].Style != null)
                        {
                            sheet.Cells[r + 1, c].Style.IsTextWrapped = true;
                        }
                        if (linkTable != null && linkTable.Columns.Contains(columnName) && linkTable.Rows.Count >= r)
                        {
                            sheet.Hyperlinks.Add(r + 1, c, 1, 1, linkTable.Rows[r][columnName].ToString());
                        }
                    }
                }
            }

            if (columnWidth != null && columnWidth > 0)
            {
                sheet.Cells.StandardWidth = Convert.ToInt32(columnWidth);
            }
            else
            {
                sheet.AutoFitColumns();
            }
            sheet.AutoFitRows();
            book.Save(fileName, Aspose.Cells.FileFormatType.Excel2007Xlsx);

        }

        /// <summary>
        ///DataSet导出到Excel文件
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="fileName"></param>
        public static void ExportToExcel(DataSet ds, string fileName)
        {
            Aspose.Cells.Workbook book = new Aspose.Cells.Workbook();
            book.Worksheets.Clear();
            int tbCount = ds.Tables.Count;
            if (tbCount > 0)
            {
                for (int i = 0; i < tbCount; i++)
                {
                    string sheetName = ds.Tables[i].TableName;
                    book.Worksheets.Add(sheetName);
                    Aspose.Cells.Worksheet sheet;
                    sheet = book.Worksheets[i];
                    sheet.Name = sheetName;
                    sheet.IsGridlinesVisible = true;

                    //AddBody(ds.Tables[i]);
                    for (int r = 0; r < ds.Tables[i].Rows.Count; r++)
                    {
                        for (int c = 0; c < ds.Tables[i].Columns.Count; c++)
                        {
                            sheet.Cells[0, c].PutValue(ds.Tables[i].Columns[c].ColumnName);//列标题
                            sheet.Cells[r + 1, c].PutValue(ds.Tables[i].Rows[r][c].ToString());
                        }
                    }

                    sheet.AutoFitColumns();
                    sheet.AutoFitRows();
                }
             
            }
         
            book.Save(fileName, Aspose.Cells.FileFormatType.Excel2007Xlsx);

        }
        public static byte[] DownloadExcelByStream(string fileName)
        {
             if (fileName == null || fileName.Trim() == "")
                return null;
            FileInfo fil = new FileInfo(fileName);
            if (fil.Exists)
            {
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                byte[] FileByteArray = new byte[Int32.Parse(fs.Length.ToString())];
                fs.Read(FileByteArray, 0, Int32.Parse(fs.Length.ToString()));
                fs.Flush();
                fs.Close();
                return FileByteArray;
            }
            return null;
        }

        public static void ExportToExcelTest(string fileName)
        {
            Aspose.Cells.Worksheet sheet;
            Aspose.Cells.Workbook book = new Aspose.Cells.Workbook();

            sheet = book.Worksheets[0];

            for (int r = 0; r < 100000; r++)
            {
                for (int c = 0; c < 50; c++)
                {
                    sheet.Cells[r + 1, c].PutValue(Convert.ToString(r + c));
                }
            }

            //sheet.AutoFitColumns();
            sheet.AutoFitRows();

            //book.Save(fileName);
            book.Save(fileName, Aspose.Cells.FileFormatType.Excel2007Xlsx);

        }
    } 
}
