using System;
using System.IO;
using System.Windows.Forms;

namespace NPMapTiles
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure(new FileInfo("log4net.config"));
                log4net.LogManager.GetLogger(typeof(Program)).Info("-----------程序启动-----------");
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                //处理未捕获的异常
                Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
                //处理UI线程异常
                Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
                //处理非UI线程异常
                AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
                Application.Run(new FrmMain());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "系统错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                writeLog(ex);
            }
            log4net.LogManager.GetLogger(typeof(Program)).Info("程序关闭");
        }
        #region 捕捉全局异常代码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        { 
            MessageBox.Show(e.Exception != null ? e.Exception.Message : "未知错误", "系统错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            writeLog(e.Exception);
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception error = e.ExceptionObject as Exception;
            writeLog(e);
            MessageBox.Show(error != null ? error.Message : "未知错误", "系统错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// 写文件
        /// </summary>
        /// <param name="str"></param>
        static void writeLog(object str)
        {
            log4net.LogManager.GetLogger(typeof(Program)).Error(str);
        }
        #endregion
    }
}
