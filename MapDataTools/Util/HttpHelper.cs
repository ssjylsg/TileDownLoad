using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace MapDataTools
{
    public class HttpHelper
    {
        /// <summary>  
        /// 创建GET方式的HTTP请求  
        /// </summary>  
        public static HttpWebResponse CreateGetHttpResponse(string url, int timeout, string userAgent, CookieCollection cookies)
        { 
            HttpWebRequest request = null;
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                //对服务端证书进行有效性校验（非第三方权威机构颁发的证书，如自己生成的，不进行验证，这里返回true）
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                request = WebRequest.Create(url) as HttpWebRequest;
                request.ProtocolVersion = HttpVersion.Version10;    //http版本，默认是1.1,这里设置为1.0
            }
            else
            {
                request = WebRequest.Create(url) as HttpWebRequest;
            }
            request.Method = "GET";
            var webProxy = System.Configuration.ConfigurationSettings.AppSettings["webProxy"] ?? string.Empty;
            if (!string.IsNullOrEmpty(webProxy))
            {
                request.Proxy = new WebProxy(webProxy.ToString());
            }

            //设置代理UserAgent和超时
            //request.UserAgent = userAgent;
            //request.Timeout = timeout;
            if (cookies != null)
            {
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(cookies);
            }
            request.Timeout = timeout;
            //设置连接超时时间 
            request.Headers.Set("Pragma", "no-cache");
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/36.0.1985.125 Safari/537.36";
            return request.GetResponse() as HttpWebResponse;
        }

        public static string GetRequestContent(string url, int timeout, string userAgent, CookieCollection cookies)
        {
            return GetResponseString(CreateGetHttpResponse(url, timeout, userAgent, cookies));
        }
        public static string GetRequestContent(string url)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.Encoding = System.Text.ASCIIEncoding.UTF8;
                    return client.DownloadString(url);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return string.Empty;
            }
        }
        
        /// <summary>
        /// 获取请求的数据
        /// </summary>
        public static string GetResponseString(HttpWebResponse webresponse)
        {
            using (Stream s = webresponse.GetResponseStream())
            {
                StreamReader reader = new StreamReader(s, Encoding.UTF8);
                return reader.ReadToEnd();

            }
        }
        /// <summary>
        /// 获取请求的数据
        /// </summary>
        public static string GetResponseString(HttpWebResponse webresponse,Encoding encode)
        {
            using (Stream s = webresponse.GetResponseStream())
            {
                StreamReader reader = new StreamReader(s, encode);
                return reader.ReadToEnd();

            }
        }
        /// <summary>
        /// 验证证书
        /// </summary>
        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            if (errors == SslPolicyErrors.None)
                return true;
            return false;
        }

        public static bool DownloadPicture(string picUrl, string savePath, int timeOut)
        {
            bool value = false;
            WebResponse response = null;
            Stream stream = null;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(picUrl);
                if (timeOut != -1) request.Timeout = timeOut;
                //request.UserAgent = "xxx";
                request.Method = "GET";
                request.Accept = "xxx";
                request.KeepAlive = false;
                request.UserAgent = "Opera/9.25 (Windows NT 6.0; U; en)";
                response = request.GetResponse();
                stream = response.GetResponseStream();
                if (!response.ContentType.ToLower().StartsWith("text/"))
                    value = SaveBinaryFile(response, savePath);
                request.Abort();
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                if (stream != null) stream.Close();
                if (response != null) response.Close();
            }
            return value;
        }

        private static bool SaveBinaryFile(WebResponse response, string savePath)
        {
            bool value = false;
            byte[] buffer = new byte[1024];
            Stream outStream = null;
            Stream inStream = null;
            try
            {
                if (File.Exists(savePath)) File.Delete(savePath);
                outStream = System.IO.File.Create(savePath);
                inStream = response.GetResponseStream();
                int l;
                do
                {
                    l = inStream.Read(buffer, 0, buffer.Length);
                    if (l > 0) outStream.Write(buffer, 0, l);
                }
                while (l > 0);
                value = true;
            }
            catch (Exception e)
            {
                value = false;
            }
            finally
            {
                if (outStream != null) outStream.Close();
                if (inStream != null) inStream.Close();
            }
            return value;
        }
    }
}
