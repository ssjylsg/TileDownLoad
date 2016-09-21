using System.Threading;
using System.Windows.Forms;

namespace NPMapTiles
{
    public class CustomExceptionHandler
    {

        public CustomExceptionHandler()
        {
            Application.ThreadException += new ThreadExceptionEventHandler(this.OnThreadException);
        }
        private void OnThreadException(object sender, ThreadExceptionEventArgs args)
        {
            try
            {
                string errorMsg = "程序运行过程中发生错误,错误信息如下:\n";
                errorMsg += args.Exception.Message;
                errorMsg += "\n发生错误的程序集为:";
                errorMsg += args.Exception.Source;
                errorMsg += "\n发生错误的具体位置为:\n";
                errorMsg += args.Exception.StackTrace;
                errorMsg += "\n\n 请抓取此错误屏幕!";
                MessageBox.Show(errorMsg, "运行时错误", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch
            {
                MessageBox.Show("系统运行时发生致命错误!\n请保存好相关数据,重启系统。", "致命错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }
    }
}
