using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPOS
{
    public class Cryption
    {
        static public String Base64_encode(String StrData)
        {
            //https://www.base64encode.net/
            String StrAns;
            StrAns = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(StrData));
            return StrAns;
        }
        static public String Base64_decode(String StrData)
        {
            //https://www.base64decode.net/
            String StrAns;
            byte[] data = System.Convert.FromBase64String(StrData);
            StrAns = System.Text.ASCIIEncoding.ASCII.GetString(data);
            return StrAns;
        }

        //---
        //for php aes128
        //資料來源: https://www.cnblogs.com/bangejingting/p/5600568.html
        //PHP 測試網頁: https://encode-decode.com/aes128-encrypt-online/
        //EX: pin: AES 128加密 範例: 假設密碼為 0501 AES加密的KEY為卡號00000000FF593EEB, 加密後為Y5MTre3MWDwP3Zj3+YWgEw==
        //C# code
        /*
            String data01 = Web_encrypt.AesEncrypt("0501", "00000000FF593EEB");
            String data02 = Web_encrypt.AesDecrypt(data01, "00000000FF593EEB"); 
         */
        /*
            key:0123456789987654

            Data Source=.\vpos.db;Version=3;JounalMode=WAL;Synchronous=Normal;LockedMode=Normal;SharedCache=true;TxOptions.isolation=xiSnapshot;UpdateOptions.lockWait=true;Busytimeout=10;
            PnJr/hxqamOJC/sUCd4T0dk7zjNWeNME0IanujV7jOybNgYLGna0zIs1f21ITwz3SMfYOt6ttz+MrBBMV/hC0zkfpyVOuqtryGj8H4Z/jKI5JJaKyDN3HS1sRQ0xzk8cjHyolFTENltSoYLRGxrdJNogqCz33hNl6RmsbzdzOclF/GLh3nyHY5eD3xjJINNBFdTqZxtcPJWWVwx8/hJD1q5+IXw/hNYW1zi59t42Fu4=

            Data Source=.\vtcloud_sync.db;Version=3;JounalMode=WAL;Synchronous=Normal;LockedMode=Normal;SharedCache=true;TxOptions.isolation=xiSnapshot;UpdateOptions.lockWait=true;Busytimeout=10;
            vv7U+j7ISnOI2aLG/KwSrOEcK7bdwzXdk9fGE3URlmwF3bA23QVu9umwPaO7ZxMKdsLwwcUMdQqQQPxIENP2pW9lz1g0lRFnQ+Z17l4bl3BLA7ygZFRoXi56Q2v2Cad0VkBGhjWewFAj+SREepKj7vB3ygnkaQFJGVJHcSS1gF86rCGvBNQ9jCoFEqR325NXA/GpKL7WKj26eq33rVwFGJZ58l+lTJo/N0507Xx1GfwATOAN1t98T8C9t+qjn/yc

            Data Source=.\takeaways.db;Version=3;JounalMode=WAL;Synchronous=Normal;LockedMode=Normal;SharedCache=true;TxOptions.isolation=xiSnapshot;UpdateOptions.lockWait=true;Busytimeout=10;
            vnul9w5EOmC8lC0tqJKIO5GhRgtq7O4r444s4xn9+Qeqf0NguXUdv0mSXMFZLqXllS6KIo4u/9utY8vx7rFEIj797eEmXi1hhoEqeDxzpBBpQgOFNpJFu2S0I99QXniaEUdy2vdasnr/51sFHwxKGHxrTnRHkYh1cXTPeI+jYb9VYgf32HXVyQTb4uQoel78LBTFDcl/fEZ+H12HDlyynh7n3yZCmQVD58zjkxmq/8hUOvPH0VlLrlFZnWMgv5gD
        */
        public static string AesEncrypt(string str, string key)
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
            Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        public static string AesDecrypt(string str, string key)
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
            Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Encoding.UTF8.GetString(resultArray);
        }
        //---for php aes128
    }
}
