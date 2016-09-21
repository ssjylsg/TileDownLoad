using System;
using System.Text;
using System.Management;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.IO;
namespace MapDataTools
{
    public static class LisenceManager
    {
        public static bool IsKeyUsing()
        {
            HardwareInfo hardInfo = new HardwareInfo();
            string message = hardInfo.GetCpuID() + hardInfo.GetMacAddress();
            string okMessage = Encrypt(message);
            string key = Read(System.Windows.Forms.Application.StartupPath + "\\Lisence.ini");
            return key.Equals(okMessage, StringComparison.CurrentCultureIgnoreCase);
        }
        public static string Read(string path)
        {
            if (!File.Exists(path))
            {
                return "";
            }
            using (var sr = new StreamReader(path, Encoding.Default))
            {
                return  sr.ReadLine();
            }
        }
        ///   <summary>
        ///   给一个字符串进行MD5加密
        ///   </summary>
        ///   <param   name="strText">待加密字符串</param>
        ///   <returns>加密后的字符串</returns>
        public static string MD5Encrypt(string strText)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(System.Text.Encoding.Default.GetBytes(strText));
            return System.Text.Encoding.ASCII.GetString(result);
        }
        public static string Encrypt(string toEncrypt)
        {
            byte[] keyArray = UTF8Encoding.UTF8.GetBytes("netposagis0123456789012345678901");
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);
            RijndaelManaged rDel = new RijndaelManaged();//using System.Security.Cryptography;    
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;//using System.Security.Cryptography;    
            rDel.Padding = PaddingMode.PKCS7;//using System.Security.Cryptography;    
            ICryptoTransform cTransform = rDel.CreateEncryptor();//using System.Security.Cryptography;    
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
    }
    /// <summary> 
    /// HardwareInfo 的摘要说明。 
    /// </summary> 
    public class HardwareInfo
    {
        private log4net.ILog log;

        public HardwareInfo()
        {
            log = log4net.LogManager.GetLogger(this.GetType());
        }
        //取机器名  
        public string GetHostName()
        {
            return System.Net.Dns.GetHostName();
        }
        //取CPU编号 
        public String GetCpuID()
        {
            try
            {
                ManagementClass mc = new ManagementClass("Win32_Processor");
                ManagementObjectCollection moc = mc.GetInstances();

                String strCpuID = null;
                foreach (ManagementObject mo in moc)
                {
                    strCpuID += mo.Properties["ProcessorId"].Value.ToString();
                }
                return strCpuID;
            }
            catch(Exception ex)
            {
                log.Error(ex);
                return "";
            }

        }//end method 

        //取第一块硬盘编号 
        public String GetHardDiskID()
        {
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMedia");
                String strHardDiskID = null;
                foreach (ManagementObject mo in searcher.Get())
                {
                    strHardDiskID = mo["SerialNumber"].ToString().Trim();
                    break;
                }
                return strHardDiskID;
            }
            catch
            {
                return "";
            }
        }//end  

        public enum NCBCONST
        {
            NCBNAMSZ = 16,      /* absolute length of a net name         */
            MAX_LANA = 254,      /* lana's in range 0 to MAX_LANA inclusive   */
            NCBENUM = 0x37,      /* NCB ENUMERATE LANA NUMBERS            */
            NRC_GOODRET = 0x00,      /* good return                              */
            NCBRESET = 0x32,      /* NCB RESET                        */
            NCBASTAT = 0x33,      /* NCB ADAPTER STATUS                  */
            NUM_NAMEBUF = 30,      /* Number of NAME's BUFFER               */
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ADAPTER_STATUS
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
            public byte[] adapter_address;
            public byte rev_major;
            public byte reserved0;
            public byte adapter_type;
            public byte rev_minor;
            public ushort duration;
            public ushort frmr_recv;
            public ushort frmr_xmit;
            public ushort iframe_recv_err;
            public ushort xmit_aborts;
            public uint xmit_success;
            public uint recv_success;
            public ushort iframe_xmit_err;
            public ushort recv_buff_unavail;
            public ushort t1_timeouts;
            public ushort ti_timeouts;
            public uint reserved1;
            public ushort free_ncbs;
            public ushort max_cfg_ncbs;
            public ushort max_ncbs;
            public ushort xmit_buf_unavail;
            public ushort max_dgram_size;
            public ushort pending_sess;
            public ushort max_cfg_sess;
            public ushort max_sess;
            public ushort max_sess_pkt_size;
            public ushort name_count;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct NAME_BUFFER
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)NCBCONST.NCBNAMSZ)]
            public byte[] name;
            public byte name_num;
            public byte name_flags;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct NCB
        {
            public byte ncb_command;
            public byte ncb_retcode;
            public byte ncb_lsn;
            public byte ncb_num;
            public IntPtr ncb_buffer;
            public ushort ncb_length;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)NCBCONST.NCBNAMSZ)]
            public byte[] ncb_callname;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)NCBCONST.NCBNAMSZ)]
            public byte[] ncb_name;
            public byte ncb_rto;
            public byte ncb_sto;
            public IntPtr ncb_post;
            public byte ncb_lana_num;
            public byte ncb_cmd_cplt;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public byte[] ncb_reserve;
            public IntPtr ncb_event;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct LANA_ENUM
        {
            public byte length;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)NCBCONST.MAX_LANA)]
            public byte[] lana;
        }

        [StructLayout(LayoutKind.Auto)]
        public struct ASTAT
        {
            public ADAPTER_STATUS adapt;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)NCBCONST.NUM_NAMEBUF)]
            public NAME_BUFFER[] NameBuff;
        }
        public class Win32API
        {
            [DllImport("NETAPI32.DLL")]
            public static extern char Netbios(ref NCB ncb);
        }

        public string GetMacAddress()
        {
            string addr = "";
            try
            {
                int cb;
                ASTAT adapter;
                NCB Ncb = new NCB();
                char uRetCode;
                LANA_ENUM lenum;

                Ncb.ncb_command = (byte)NCBCONST.NCBENUM;
                cb = Marshal.SizeOf(typeof(LANA_ENUM));
                Ncb.ncb_buffer = Marshal.AllocHGlobal(cb);
                Ncb.ncb_length = (ushort)cb;
                uRetCode = Win32API.Netbios(ref Ncb);
                lenum = (LANA_ENUM)Marshal.PtrToStructure(Ncb.ncb_buffer, typeof(LANA_ENUM));
                Marshal.FreeHGlobal(Ncb.ncb_buffer);
                if (uRetCode != (short)NCBCONST.NRC_GOODRET) return "";

                for (int i = 0; i < lenum.length; i++)
                {
                    Ncb.ncb_command = (byte)NCBCONST.NCBRESET;
                    Ncb.ncb_lana_num = lenum.lana[i];
                    uRetCode = Win32API.Netbios(ref Ncb);
                    if (uRetCode != (short)NCBCONST.NRC_GOODRET) return "";

                    Ncb.ncb_command = (byte)NCBCONST.NCBASTAT;
                    Ncb.ncb_lana_num = lenum.lana[i];
                    Ncb.ncb_callname[0] = (byte)'*';
                    cb = Marshal.SizeOf(typeof(ADAPTER_STATUS))
                         + Marshal.SizeOf(typeof(NAME_BUFFER)) * (int)NCBCONST.NUM_NAMEBUF;
                    Ncb.ncb_buffer = Marshal.AllocHGlobal(cb);
                    Ncb.ncb_length = (ushort)cb;
                    uRetCode = Win32API.Netbios(ref Ncb);
                    adapter.adapt = (ADAPTER_STATUS)Marshal.PtrToStructure(Ncb.ncb_buffer, typeof(ADAPTER_STATUS));
                    Marshal.FreeHGlobal(Ncb.ncb_buffer);

                    if (uRetCode == (short)NCBCONST.NRC_GOODRET)
                    {
                        if (i > 0) addr += ":";
                        addr = string.Format(
                            "{0,2:X}{1,2:X}{2,2:X}{3,2:X}{4,2:X}{5,2:X}",
                            adapter.adapt.adapter_address[0],
                            adapter.adapt.adapter_address[1],
                            adapter.adapt.adapter_address[2],
                            adapter.adapt.adapter_address[3],
                            adapter.adapt.adapter_address[4],
                            adapter.adapt.adapter_address[5]);
                    }
                }
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }
            return addr.Replace(' ', '0');
        }
    } 
}
