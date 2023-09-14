using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Compression;//C# 透過程式壓縮資料夾為zip檔[NET4.5 版本 內建的方法]
using System.Linq;
using System.Management;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace VPOS
{
    public class FileLib
    {
        public static String GetDeviceCode()//C# 抓取 硬體序號
        {
            String StrResult = "";

            //擷取cpu序號  
            string cpuInfo = "";
            ManagementClass cimobject = new ManagementClass("Win32_Processor");
            ManagementObjectCollection moc = cimobject.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                if (mo.Properties["ProcessorId"].Value != null)
                    cpuInfo += mo.Properties["ProcessorId"].Value.ToString();
            }
            StrResult += cpuInfo;

            //擷取硬碟ID 
            string HDid = "";
            ManagementClass cimobject1 = new ManagementClass("Win32_DiskDrive");
            ManagementObjectCollection moc1 = cimobject1.GetInstances();
            foreach (ManagementObject mo in moc1)
            {
                if (mo.Properties["Model"].Value != null)
                    HDid += (string)mo.Properties["Model"].Value.ToString();
            }
            StrResult += HDid;
            
            //擷取網卡硬體地址     
            string MACAddress = "";
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc2 = mc.GetInstances();
            foreach (ManagementObject mo in moc2)
            {
                if ((bool)mo["IPEnabled"] == true && mo["MacAddress"] != null)
                    MACAddress += mo["MacAddress"].ToString();
                mo.Dispose();
            }
            StrResult += MACAddress;
            StrResult = StrResult.Replace(" ", "");
            StrResult = StrResult.Replace(":", "");

            string hash;
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(StrResult);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    builder.Append(hashBytes[i].ToString("x2")); // 将每个字节转换为两位的十六进制字符串
                }

                hash = builder.ToString();
            }

            char[] charArray = hash.ToCharArray();
            Array.Reverse(charArray);
            StrResult = hash + new string(charArray);

            return StrResult.ToUpper();
        }

        public static bool DBBackup(String StrName = "")
        {
            bool blnAns = false;

            if (StrName.Length == 0)
            {
                StrName = DateTime.Now.ToString("yyyyMMddHHmmss") + ".zip";
            }

            String StrSrcFullFilePath = String.Format("{0}\\{1}", path, "mysql\\data\\v8_workstation");
            string StrOutPath = String.Format("{0}\\{1}", path, "DBBackup\\");
            CreateDirectory(StrOutPath);
            CreateDirectory(StrOutPath + "v8_workstation\\");
            DirectoryCopy(StrSrcFullFilePath, StrOutPath + "v8_workstation\\", true);
            ZipFile.CreateFromDirectory(StrOutPath + "\\v8_workstation", StrOutPath + StrName);
            DeleteDirectory(StrOutPath + "v8_workstation\\", true);

            return blnAns;
        }

        //刪除目錄，recursive為True時，直接刪除目錄及其目錄下所有文件或子目錄;recursive為False時，需先將目錄下所有文件或子目錄刪除
        private static void DeleteDirectory(string path, bool recursive)
        {
            if (Directory.Exists(path))
            {
                if (recursive)
                {
                    Directory.Delete(path, true);
                }
            }
        }

        public static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, false);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }

        //建立目錄
        private static void CreateDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        public static string path = Directory.GetCurrentDirectory() + "\\";

        public static bool IsFileExists(String StrName)// C# 判斷檔案是否存在/檢查檔案是否存在 (C# 確認 檔案 存在) 
        {
            bool blnAns = false;

            String StrFullFileName = String.Format("{0}{1}", path, StrName);

            blnAns = System.IO.File.Exists(StrFullFileName);

            return blnAns;
        }

        public static bool IsUTF8File(String StrName)
        {
            bool blnAns = false;

            Stream reader = File.Open(StrName, FileMode.Open, FileAccess.Read);
            Encoding encoder = null;
            byte[] header = new byte[4];
            // 讀取前四個Byte
            reader.Read(header, 0, 4);
            if (header[0] == 0xFF && header[1] == 0xFE)
            {
                // UniCode File
                reader.Position = 2;
                encoder = Encoding.Unicode;
            }
            else if (header[0] == 0xEF && header[1] == 0xBB && header[2] == 0xBF)
            {
                // UTF-8 File
                reader.Position = 3;
                encoder = Encoding.UTF8;
                blnAns = true;
            }
            else
            {
                // Default Encoding File
                reader.Position = 0;
                encoder = Encoding.Default;
            }

            reader.Close();
            // .......... 接下來的程式

            return blnAns;
        }
        public static String ImageFile2Base64String(String StrDestFilePath)
        {
            String StrAns = "";
            try
            {
                //開啟檔案
                FileStream file = File.Open(StrDestFilePath, FileMode.Open, FileAccess.Read);
                //引用myReader類別
                BinaryReader read = new BinaryReader(file);
                int len = System.Convert.ToInt32(file.Length);
                //讀取位元陣列
                byte[] data = read.ReadBytes(len);
                StrAns = Convert.ToBase64String(data);
                //讀取資料
                //釋放資源
                read.Close();
                read.Dispose();
                file.Close();
                //file.Flush();

            }
            catch
            {
            }
            return StrAns;
        }
        public static void Base64String2ImageFile(String StrDestFilePath, String StrImageData)
        {
            byte[] data = Convert.FromBase64String(StrImageData);
            CreateFile(StrDestFilePath, data);
        }
        public static void CreateFile(String StrDestFilePath, byte[] byteSource)
        {
            FileLib.DeleteFile("temp.png");
            FileLib.DeleteFile(StrDestFilePath, false);
            try
            {
                //開啟建立檔案
                FileStream file = File.Open(StrDestFilePath, FileMode.Create, FileAccess.ReadWrite);
                BinaryWriter write = new BinaryWriter(file);
                write.Write(byteSource);
                write.Close();
                write.Dispose();
                write.Flush();

                file.Close();
                file.Flush();
                file.Dispose();
            }
            catch
            {
            }
        }
        public static void CopyFile(String StrSourceFilePath, String StrDestFilePath)
        {
            System.IO.File.Copy(StrSourceFilePath, StrDestFilePath, true);
        }
        public static void DeleteFile(String StrFileName, bool blnAtRoot = true)
        {
            String FilePath = "";
            if (blnAtRoot == true)
            {
                FilePath = path + "\\" + StrFileName;
            }
            else
            {
                FilePath = StrFileName;
            }
            if (System.IO.File.Exists(FilePath))
            {
                try
                {
                    System.IO.File.Delete(FilePath);
                }
                catch
                {
                    LogFile.Write($"FileLib.cs ; DeleteFile() : {FilePath}");
                }
            }
        }
        public static void WriteTxtFile(String StrFileName, String StrData)
        {
            StreamWriter sw = new StreamWriter(StrFileName);
            sw.WriteLine(StrData);// 寫入文字
            sw.Close();// 關閉串流
        }
        public static String ReadTxtFile(String StrFileName, bool blnOneLine = true)
        {
            String StrResult = "";
            String StrFullFileName = String.Format("{0}{1}", path, StrFileName);

            StreamReader sr = new StreamReader(StrFullFileName);
            while (!sr.EndOfStream)
            {
                if (StrResult.Length > 0)
                {
                    StrResult += "\n";
                }

                StrResult += sr.ReadLine();// 寫入文字

                if (blnOneLine)
                {
                    break;
                }
            }
            sr.Close();// 關閉串流
            return StrResult;
        }
        public static void TxtFile(String StrFileName, String StrData)
        {
            StreamWriter sw = new StreamWriter(StrFileName);
            sw.WriteLine(StrData);// 寫入文字
            sw.Close();// 關閉串流
        }

        public static void logFile(String StrFileName, String StrData, bool blnAutoTime = true)
        {
            FileStream fs = new FileStream(StrFileName, FileMode.Append);
            StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
            if (blnAutoTime == true)
            {
                sw.WriteLine(DateTime.Now.ToString("HH:mm:ss - ") + StrData);// 寫入文字
            }
            else
            {
                sw.WriteLine(StrData);// 寫入文字
            }

            sw.Close();// 關閉串流
        }

        //---
        //縮圖
        public static bool ImageResize(String strImageSrcPath, String strImageDesPath, int intWidth = 0, int intHeight = 0)
        {
            /*
            ImageFormat IF_Png = ImageFormat.Png; 
            bool blnAns = true;
            Image objImage = Image.FromFile(strImageSrcPath);
            if (intWidth > objImage.Width)
            {
                intWidth = objImage.Width;
            }
            if (intHeight > objImage.Height)
            {
                intHeight = objImage.Height;
            }
            if ((intWidth == 0) && (intHeight == 0))
            {
                intWidth = objImage.Width;
                intHeight = objImage.Height;
            }
            else if ((intHeight == 0) && (intWidth != 0))
            {
                intHeight = (int)(objImage.Height * intWidth / objImage.Width);
            }
            else if ((intWidth == 0) && (intHeight != 0))
            {
                intWidth = (int)(objImage.Width * intHeight / objImage.Height);
            }
            Bitmap imgOutput = new Bitmap(objImage, intWidth, intHeight);
            imgOutput.Save(strImageDesPath, IF_Png);//imgOutput.Save(strImageDesPath, objImage.RawFormat);
            objImage.Dispose();
            objImage = null;
            imgOutput.Dispose();
            imgOutput = null;
            return blnAns;
            */
            Process m_pro;
            String StrVar = String.Format("\"{0}\" \"{1}\" {2}", strImageSrcPath, strImageDesPath, intWidth);
            ProcessStartInfo startInfo = new ProcessStartInfo("CS_cmd_ImageResize.exe", StrVar);
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;

            try
            {
                m_pro = Process.Start(startInfo);
            }
            catch
            {
                return false;//找不到執行檔的防呆 at 2017/06/16
            }
            if (m_pro != null)
            {
                m_pro.WaitForExit();//下載SERVER資料
                m_pro = null;
            }

            return true;
        }
        //---縮圖
    }//FileLib

}
