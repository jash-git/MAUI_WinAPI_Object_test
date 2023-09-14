using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPOS
{
    public class StringEncrypt
    {
        //---
        //for php aes128
        //資料來源: https://www.cnblogs.com/bangejingting/p/5600568.html
        //PHP 測試網頁: https://encode-decode.com/aes128-encrypt-online/
        //EX: pin: AES 128加密 範例: 假設密碼為 0501 AES加密的KEY為卡號2221629242897648, 加密後為Y5MTre3MWDwP3Zj3+YWgEw==
        //C# code
        /*
            String data01 = Web_encrypt.AesEncrypt("0501", "2221629242897648");
            String data02 = Web_encrypt.AesDecrypt(data01, "2221629242897648"); 
         */
        public static string AesEncrypt(string str, string key= "2221629242897648")
        {
            Byte[] resultArray;
            try
            {
                if (string.IsNullOrEmpty(str)) return null;
                Byte[] toEncryptArray = Encoding.UTF8.GetBytes(str);
                Byte[] ivArray = new Byte[16];
                System.Security.Cryptography.RijndaelManaged rm = new System.Security.Cryptography.RijndaelManaged
                {
                    Key = Encoding.UTF8.GetBytes(key),
                    IV = ivArray,
                    Mode = System.Security.Cryptography.CipherMode.ECB,
                    Padding = System.Security.Cryptography.PaddingMode.PKCS7,
                };

                System.Security.Cryptography.ICryptoTransform cTransform = rm.CreateEncryptor();
                resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            }
            catch(Exception e) 
            {
                resultArray = new byte[0];
            }

            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        public static string AesDecrypt(string str, string key= "2221629242897648")
        {
            Byte[] resultArray;
            try
            {
                if (string.IsNullOrEmpty(str)) return null;
                Byte[] toEncryptArray = Convert.FromBase64String(str);
                Byte[] ivArray = new Byte[16];
                System.Security.Cryptography.RijndaelManaged rm = new System.Security.Cryptography.RijndaelManaged
                {
                    Key = Encoding.UTF8.GetBytes(key),
                    IV = ivArray,
                    Mode = System.Security.Cryptography.CipherMode.ECB,
                    Padding = System.Security.Cryptography.PaddingMode.PKCS7
                };

                System.Security.Cryptography.ICryptoTransform cTransform = rm.CreateDecryptor();
                resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            }
            catch(Exception ex) 
            {
                resultArray = new byte[0];
            }

            return Encoding.UTF8.GetString(resultArray);
        }
        //---for php aes128

    }
}
