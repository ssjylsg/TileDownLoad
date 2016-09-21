using OSGeo.OGR;
using System;
using System.Data;

namespace MapDataTools
{
    using log4net;

    public class ShpFileHelper
    {
        private static ILog log;

        static ShpFileHelper()
        {
            log = log4net.LogManager.GetLogger(typeof(ShpFileHelper));
        }
        /// <summary>
        /// 转换是否纠偏，只使用与火星坐标纠偏
        /// </summary>
        /// <param name="dataTable">数据表，如果是点表中含X,Y字段，如果是线和面，表中含有PATH字段</param>
        /// <param name="filePath">存储路径</param>
        /// <param name="geoType">数据类型，暂时支持简单的点线面</param>
        /// <param name="jiupian">是否纠偏[火星坐标转84坐标]</param>
        public static void SaveShpFile(DataTable dataTable, string filePath, wkbGeometryType geoType, ProjectConvert jiupian)
        {
            if (geoType == wkbGeometryType.wkbLineString || geoType == wkbGeometryType.wkbPolygon)
            {
                if (!dataTable.Columns.Contains("PATH"))
                {
                    return;
                }
            }
            else if (geoType == wkbGeometryType.wkbPoint)
            {
                if (!dataTable.Columns.Contains("X") || !dataTable.Columns.Contains("Y"))
                {
                    return;
                }
            }
            else
            {
                return;
            }
            // 为了支持中文路径，请添加下面这句代码
            OSGeo.GDAL.Gdal.SetConfigOption("GDAL_FILENAME_IS_UTF8", "NO");
            // 为了使属性表字段支持中文，请添加下面这句
            OSGeo.GDAL.Gdal.SetConfigOption("SHAPE_ENCODING", "");

            string strVectorFile = filePath;

            // 注册所有的驱动
            Ogr.RegisterAll();

            //创建数据，这里以创建ESRI的shp文件为例
            string strDriverName = "ESRI Shapefile";
            int count = Ogr.GetDriverCount();
            Driver oDriver = Ogr.GetDriverByName(strDriverName);
            if (oDriver == null)
            {
                log.ErrorFormat("{0}驱动不可用！\n", strVectorFile);
                Console.WriteLine("{0}驱动不可用！\n", strVectorFile);
                return;
            }

            // 创建数据源
            DataSource oDS = oDriver.CreateDataSource(strVectorFile, null);
            if (oDS == null)
            {
                log.ErrorFormat("创建矢量文件{0}失败！\n", strVectorFile);
                Console.WriteLine("创建矢量文件【%s】失败！\n", strVectorFile);
                return;
            }
            string strwkt = "GEOGCS[\"WGS 84\",DATUM[\"WGS_1984\",SPHEROID[\"WGS 84\",6378137,298.257223563,AUTHORITY[\"EPSG\",\"7030\"]],AUTHORITY[\"EPSG\",\"6326\"]],PRIMEM[\"Greenwich\",0,AUTHORITY[\"EPSG\",\"8901\"]],UNIT[\"degree\",0.0174532925199433,AUTHORITY[\"EPSG\",\"9122\"]],AUTHORITY[\"EPSG\",\"4326\"]]";
            OSGeo.OSR.SpatialReference srs = new OSGeo.OSR.SpatialReference(strwkt);
            // 创建图层，创建一个多边形图层，这里没有指定空间参考，如果需要的话，需要在这里进行指定
            Layer oLayer = oDS.CreateLayer("test", null, geoType, null);
            if (oLayer == null)
            {
                Console.WriteLine("图层创建失败！\n");
                return;
            }
            for (int i = 0; i < dataTable.Columns.Count; i++)
            {
                if (dataTable.Columns[i].ColumnName == "PATH")
                {
                    continue;
                }
                string columnName = dataTable.Columns[i].ColumnName;
                FieldDefn oFieldName = new FieldDefn(columnName, FieldType.OFTString);
                oFieldName.SetWidth(255);
                oLayer.CreateField(oFieldName, 1);
            }
            FeatureDefn oDefn = oLayer.GetLayerDefn();
        
            foreach (DataRow row in dataTable.Rows)
            { 

                if ((geoType == wkbGeometryType.wkbPolygon || geoType == wkbGeometryType.wkbLineString) && string.IsNullOrEmpty(row["PATH"].ToString()))
                {
                    continue;
                }
                // 创建Multi要素
                Feature oFeatureMulty = new Feature(oDefn);
                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    if (geoType == wkbGeometryType.wkbPolygon || geoType == wkbGeometryType.wkbLineString)
                    {
                        if (dataTable.Columns[i].ColumnName == "PATH")
                        {
                            continue;
                        }
                    }
                    string name = row[i] != null ? row[i].ToString() : "";
                    if (!string.IsNullOrEmpty(name))
                    {
                        try
                        {
                            //oFeatureMulty.SetField(dataTable.Columns[i].ColumnName,name);
                            var value = name.Trim().Replace("'", "").Replace(",", "");
                            if (value.Length > 255)
                            {
                                value = value.Substring(0, 250);
                            }
                            oFeatureMulty.SetField(i, value);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                        }
                    }
                }
                if (geoType == wkbGeometryType.wkbPolygon || geoType == wkbGeometryType.wkbLineString)
                {
                    string path = row["PATH"] != null ? row["PATH"].ToString() : "";
                    if (string.IsNullOrEmpty(path))
                    {
                        continue;
                    }
                    var geoCount = path.Split('|').Length;
                    wkbGeometryType newGeometryType = geoType;
                    OSGeo.OGR.Geometry geo1 = new OSGeo.OGR.Geometry(geoType);
                    if (geoType == wkbGeometryType.wkbPolygon)
                    {
                        if (geoCount > 1)
                        { 
                            geo1 = new OSGeo.OGR.Geometry(wkbGeometryType.wkbMultiPolygon);
                            for (int i = 0; i < geoCount; i++)
                            {
                                var temp = path.Split('|')[i];
                                var points = temp.Split(';');
                                OSGeo.OGR.Geometry innGeometry = new Geometry(wkbGeometryType.wkbLinearRing);
                                for (int j = 0; j < points.Length; j++)
                                {
                                    var c = ProjectChange(points[j], jiupian);
                                    innGeometry.AddPoint_2D(c.lon, c.lat);
                                }
                                var polygon = new Geometry(wkbGeometryType.wkbPolygon);
                                polygon.AddGeometry(innGeometry);
                                geo1.AddGeometry(polygon);
                            }
                        }
                        else
                        {
                            geo1 = new OSGeo.OGR.Geometry(wkbGeometryType.wkbPolygon);
                            for (int i = 0; i < geoCount; i++)
                            {
                                var temp = path.Split('|')[i];
                                var points = temp.Split(';');
                                OSGeo.OGR.Geometry innGeometry = new Geometry(wkbGeometryType.wkbLinearRing);
                                for (int j = 0; j < points.Length; j++)
                                {
                                    var c = ProjectChange(points[j], jiupian);
                                    innGeometry.AddPoint_2D(c.lon, c.lat);
                                }
                                geo1.AddGeometry(innGeometry);
                            }
                        }
                    }
                    else
                    {
                        if (geoCount > 1)
                        {
                            geo1 = new OSGeo.OGR.Geometry(wkbGeometryType.wkbMultiLineString);
                            for (int i = 0; i < geoCount; i++)
                            {
                                var temp = path.Split('|')[i];
                                var points = temp.Split(';');
                                OSGeo.OGR.Geometry innGeometry = new Geometry(wkbGeometryType.wkbLinearRing);
                                for (int j = 0; j < points.Length; j++)
                                {
                                    var c = ProjectChange(points[j], jiupian);
                                    innGeometry.AddPoint_2D(c.lon, c.lat);
                                }
                                geo1.AddGeometry(innGeometry);
                            }
                        }
                        else
                        {
                            for (int i = 0; i < geoCount; i++)
                            {
                                var temp = path.Split('|')[i];
                                var points = temp.Split(';');
                                geo1 = new OSGeo.OGR.Geometry(wkbGeometryType.wkbLinearRing);
                                for (int j = 0; j < points.Length; j++)
                                {
                                    var c = ProjectChange(points[j], jiupian);
                                    geo1.AddPoint_2D(c.lon, c.lat);
                                }
                            }
                        }
                    }

                   

                    
                  

                    //if (geoType == wkbGeometryType.wkbLineString)
                    //{
                    //    for (int i = 0; i < ps.Length; i++)
                    //    { 
                    //        var c = ProjectChange(ps[i], jiupian);
                    //        geo1.AddPoint_2D(c.lon, c.lat);
                    //    }
                    //}
                    //else if (geoType == wkbGeometryType.wkbPolygon)
                    //{
                    //    List<int> indexs = new List<int>();
                    //    indexs.Add(0);
                    //    for (int i = 0; i < ps.Length; i++)
                    //    {
                    //        if (ps[i].Split('|', ',').Length > 2)
                    //        {
                    //            indexs.Add(i);
                    //        }
                    //    }
                    //    indexs.Add(ps.Length - 1);
                    //    if (indexs.Count == 2)
                    //    {
                    //        OSGeo.OGR.Geometry outGeo = new OSGeo.OGR.Geometry(OSGeo.OGR.wkbGeometryType.wkbLinearRing);
                    //        for (int i = 0; i < ps.Length; i++)
                    //        {
                              
                    //            var c = ProjectChange(ps[i], jiupian);
                    //            outGeo.AddPoint_2D(c.lon, c.lat);
                    //        }
                         
                    //        var c1 = ProjectChange(ps[0], jiupian);
                    //        outGeo.AddPoint_2D(c1.lon, c1.lat);

                    //        geo1.AddGeometryDirectly(outGeo);
                    //    }
                    //    else
                    //    {
                    //        for (int j = 0; j < indexs.Count - 1; j++)
                    //        {
                    //            OSGeo.OGR.Geometry outGeo = new OSGeo.OGR.Geometry(OSGeo.OGR.wkbGeometryType.wkbLinearRing);
                    //            Coord sc = new Coord();
                    //            int first = indexs[j];
                    //            int end = indexs[j + 1];
                    //            for (int i = first; i < end + 1; i++)
                    //            {
                    //                if (i == first && first != 0)
                    //                {
                                         
                    //                    var c = ProjectChange(ps[i], jiupian);
                    //                    outGeo.AddPoint(c.lon, c.lat,0);
                    //                }
                    //                else
                    //                {
                                        
                    //                    if (jiupian != ProjectConvert.NONE)
                    //                    {
                    //                        var c = ProjectChange(ps[i], jiupian);
                    //                        outGeo.AddPoint(c.lon, c.lat,0);
                    //                    }
                    //                    else
                    //                    {
                    //                        double x = double.Parse(ps[i].Split('|', ',')[0]);
                    //                        double y = double.Parse(ps[i].Split('|', ',')[1]);
                    //                        if (i == first)
                    //                        {
                    //                            sc.lon = x;
                    //                            sc.lat = y;
                    //                        }
                    //                        outGeo.AddPoint(x, y, 0);
                    //                    }

                    //                }
                    //            }
                    //            outGeo.AddPoint(sc.lon, sc.lat, 0);
                    //            geo1.AddGeometryDirectly(outGeo);
                    //        }
                    //    }
                    //}
                    oFeatureMulty.SetGeometry(geo1);
                }
                else if (geoType == wkbGeometryType.wkbPoint)
                {
                    OSGeo.OGR.Geometry geo1 = new OSGeo.OGR.Geometry(wkbGeometryType.wkbPoint);
                    string xstring = row["X"] != null ? row["X"].ToString() : "";
                    string ystring = row["Y"] != null ? row["Y"].ToString() : "";
                    double x = double.Parse(xstring);
                    double y = double.Parse(ystring);
                    Coord c = CoordHelper.Gcj2Wgs(x, y);
                    geo1.AddPoint(c.lon, c.lat, 0);
                    oFeatureMulty.SetGeometry(geo1);
                }
                oLayer.CreateFeature(oFeatureMulty);
            }
            oLayer.Dispose();
            oDS.Dispose();
        }
        /// <summary>
        /// 坐标转换
        /// </summary>
        /// <param name="lonlat">格式[x,y]或者[x|y]</param>
        /// <param name="project"></param>
        /// <returns></returns>
        public static Coord ProjectChange(string lonlat, ProjectConvert project)
        {
            if (string.IsNullOrEmpty(lonlat))
            {
                return new Coord(0,0);
            }
            try
            {
                double x = double.Parse(lonlat.Split('|', ',')[0]);
                double y = double.Parse(lonlat.Split('|', ',')[1]);
                switch (project)
                {
                    case ProjectConvert.NONE:
                        return new Coord(x, y);
                    case ProjectConvert.GCJ_WGS:
                    case ProjectConvert.GAODE84_WGS:
                        return CoordHelper.Gcj2Wgs(x, y);
                    case ProjectConvert.BAIDU_WGS:
                        var c = CoordHelper.BdDecrypt(y, x);
                        return CoordHelper.Gcj2Wgs(c.lon, c.lat);
                    case ProjectConvert.GAODE900913_WGS:
                        var g = CoordHelper.WebMercator2lonLat(new Coord(x, y));
                        return CoordHelper.Gcj2Wgs(g.lon, g.lat);
                    default:
                        return new Coord(0, 0);
                }
            }
            catch (Exception)
            {
                Console.WriteLine(lonlat);
                return new Coord();
            }
        }

        /// <summary>
        /// 转换并纠偏，只使用与火星坐标纠偏
        /// </summary>
        /// <param name="dataTable">数据表，如果是点表中含X,Y字段，如果是线和面，表中含有PATH字段</param>
        /// <param name="filePath">存储路径</param>
        /// <param name="geoType">数据类型，暂时支持简单的点线面</param>
        //public static void saveShpFile(DataTable dataTable,string filePath,wkbGeometryType geoType)
        //{
        //    saveShpFile(dataTable, filePath, geoType, ProjectConvert.GCJ_WGS);
        //}
        /// <summary>
        /// 导出shp数据到表格
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns>DataTable</returns>
        public static DataTable GetData(string path)
        {
            try
            {
                OSGeo.OGR.Ogr.RegisterAll();
                OSGeo.OGR.Driver dr = OSGeo.OGR.Ogr.GetDriverByName("ESRI shapefile");

                if (dr == null)
                {
                    Console.WriteLine("启动驱动失败！\n");
                }

                OSGeo.OGR.DataSource ds = dr.Open(path, 0);
                int layerCount = ds.GetLayerCount();
                OSGeo.OGR.Layer layer = ds.GetLayerByIndex(0);
                DataTable dataTable = new DataTable();
                FeatureDefn fdf = layer.GetLayerDefn();
                int fieldCount = fdf.GetFieldCount();
                for (int i = 0; i < fieldCount; i++)
                {
                    dataTable.Columns.Add(fdf.GetFieldDefn(i).GetName(), Type.GetType("System.String"));
                }
                if (fdf.GetGeomType() == OSGeo.OGR.wkbGeometryType.wkbPoint)
                {
                    if (!dataTable.Columns.Contains("X"))
                    {
                        dataTable.Columns.Add("X", Type.GetType("System.String"));
                    }
                    if (!dataTable.Columns.Contains("Y"))
                    {
                        dataTable.Columns.Add("Y", Type.GetType("System.String"));
                    }
                }
                else if (fdf.GetGeomType() == OSGeo.OGR.wkbGeometryType.wkbPolygon || fdf.GetGeomType() == OSGeo.OGR.wkbGeometryType.wkbLineString)
                {
                    if (!dataTable.Columns.Contains("PATH"))
                    {
                        dataTable.Columns.Add("PATH", Type.GetType("System.String"));
                    }
                }
                OSGeo.OGR.Feature feat;
                while ((feat = layer.GetNextFeature()) != null)
                {
                    DataRow row = dataTable.NewRow();
                    OSGeo.OGR.Geometry geometry = feat.GetGeometryRef();
                    for (int i = 0; i < fieldCount; i++)
                    {
                        string value = feat.GetFieldAsString(i);
                        row[i] = value.Replace(",", "");
                    }
                    OSGeo.OGR.wkbGeometryType goetype = geometry.GetGeometryType();
                    if (goetype == OSGeo.OGR.wkbGeometryType.wkbPoint)
                    {
                        double x = geometry.GetX(0);
                        double y = geometry.GetY(0);
                        row["X"] = x.ToString();
                        row["Y"] = y.ToString();
                    }
                    else if (goetype == OSGeo.OGR.wkbGeometryType.wkbLineString)
                    {
                        string points = "";
                        for (int i = 0; i < geometry.GetPointCount(); i++)
                        {
                            double x = geometry.GetX(i);
                            double y = geometry.GetY(i);
                            points += x.ToString() + "|" + y.ToString() + ";";
                        }
                        if (points != "")
                            points.Substring(0, points.Length - 1);
                        row["PATH"] = points;
                    }
                    else if (goetype == OSGeo.OGR.wkbGeometryType.wkbPolygon)
                    {
                        int count = geometry.GetGeometryCount();
                        string points = "";
                        for (int j = 0; j < count; j++)
                        {
                            Geometry geo = geometry.GetGeometryRef(j);
                            for (int i = 0; i < geo.GetPointCount(); i++)
                            {
                                double x = geo.GetX(i);
                                double y = geo.GetY(i);
                                points += x.ToString() + "|" + y.ToString() + ";";
                            }
                            if (points != "")
                                points.Substring(0, points.Length - 1);
                            points += "|";
                        }
                        if (points != "")
                            points.Substring(0, points.Length - 1);
                        row["PATH"] = points;
                    }
                }
                layer.Dispose();
                ds.Dispose();
                dr.Dispose();
                return dataTable;
            }
            catch
            {
                return null;
            }
        }

        public static bool CovertRegion2Line(string path,string savePath)
        {
            try
            {
                OSGeo.OGR.Ogr.RegisterAll();
                OSGeo.OGR.Driver dr = OSGeo.OGR.Ogr.GetDriverByName("ESRI shapefile");
                if (dr == null)
                {
                    Console.WriteLine("启动驱动失败！\n");
                }

                OSGeo.OGR.DataSource ds = dr.Open(path, 0);
                int layerCount = ds.GetLayerCount();
                OSGeo.OGR.Layer layer = ds.GetLayerByIndex(0);
                FeatureDefn fdf = layer.GetLayerDefn();
                int fieldCount = fdf.GetFieldCount();

                // 创建数据源
                DataSource oDS = dr.CreateDataSource(savePath, null);
                if (oDS == null)
                {
                    Console.WriteLine("创建矢量文件【%s】失败！\n", savePath);
                    return false;
                }
                OSGeo.OSR.SpatialReference srs = layer.GetSpatialRef();
                // 创建图层，创建一个多边形图层，这里没有指定空间参考，如果需要的话，需要在这里进行指定
                Layer oLayer = oDS.CreateLayer("line", srs, OSGeo.OGR.wkbGeometryType.wkbMultiLineString, null);

                if (oLayer == null)
                {
                    Console.WriteLine("图层创建失败！\n");
                    return false;
                }
                for (int i = 0; i < fieldCount; i++)
                {
                    FieldDefn fieldDefn = fdf.GetFieldDefn(i);
                    oLayer.CreateField(fieldDefn, 1);
                }
                FeatureDefn oDefn = oLayer.GetLayerDefn();
                OSGeo.OGR.Feature feat;
                while ((feat = layer.GetNextFeature()) != null)
                {
                    Feature oFeatureMulty = new Feature(oDefn);
                    OSGeo.OGR.Geometry geometry = feat.GetGeometryRef();
                    for (int i = 0; i < fieldCount; i++)
                    {
                        string value = feat.GetFieldAsString(i);
                        oFeatureMulty.SetField(i, value);
                    }
                    OSGeo.OGR.wkbGeometryType goetype = geometry.GetGeometryType();
                    if (goetype == OSGeo.OGR.wkbGeometryType.wkbPolygon)
                    {
                        Geometry tempGeo = new Geometry(wkbGeometryType.wkbMultiLineString);
                        int count = geometry.GetGeometryCount();
                        for (int j = 0; j < count; j++)
                        {
                            Geometry line = new Geometry(wkbGeometryType.wkbLinearRing);
                            Geometry geo = geometry.GetGeometryRef(j);
                            for (int i = 0; i < geo.GetPointCount(); i++)
                            {
                                double x = geo.GetX(i);
                                double y = geo.GetY(i);
                                line.AddPoint(x, y, 0);
                            }
                            tempGeo.AddGeometryDirectly(line);
                        }
                        oFeatureMulty.SetGeometry(tempGeo);
                    }
                    oLayer.SetFeature(oFeatureMulty);
                }
                oLayer.Dispose();
                layer.Dispose();
                oDS.Dispose();
                ds.Dispose();
                dr.Dispose();
            }
            catch
            { }
            return false;
        }
    }
    /// <summary>
    /// 坐标转换方式
    /// </summary>
    public enum ProjectConvert
    {
        /// <summary>
        /// 不做变化处理
        /// </summary>
        NONE,
        /// <summary>
        /// 百度转WGS
        /// </summary>
        BAIDU_WGS,
        /// <summary>
        /// 高德84转WGS
        /// </summary>
        GAODE84_WGS,
        /// <summary>
        /// 火星转WGS
        /// </summary>
        GCJ_WGS,
        /// <summary>
        /// 高德平面转WGS
        /// </summary>
        GAODE900913_WGS,
    }
}
