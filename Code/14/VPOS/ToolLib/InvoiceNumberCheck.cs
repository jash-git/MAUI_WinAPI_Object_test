using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace VPOS
{
    public class InvoiceNumberCheck
    {
        public static bool VerifyEgui(String StrData)//電子發票手機條碼驗證 / 驗證手機條碼載具
        {
            /*
            如何驗證手機條碼載具	

            https://www.ecpay.com.tw/CascadeFAQ/CascadeFAQ_Qa?nID=3531

            共通性載具編碼邏輯或載具類別編號與載具編碼不符，將會影響消費者中獎權益
            綠界科技僅協助基本檢核邏輯，載具內容正確性需請商家自行驗證，
            驗證完畢後再進行建立訂單及開立票動作。

            基本檢核邏輯:第1碼為【/】; 其餘7碼則由數字【0-9】大寫英文【A-Z】
            與特殊符號【+】【-】【.】這39個字元組成的編號，總長度為8碼字元	
            */

            /*
            對應正則表達式/正規表達式
            https://cynthiachuang.github.io/Check-E-Government-Uniform-Invoice/

            ^    -> 匹配字符串的開頭
            /    -> 第一個字一定要是斜線
            []   -> 描述符合規則範圍
            A-Z  -> 大寫英文字母
            0-9  -> 純數字
            +-\. -> 四個特定符號
            {7}  -> / 之後總共7碼 
            $    -> 匹配字符串的結尾	
            */
            bool blnResult = false;

            MatchCollection matches01 = Regex.Matches(StrData, @"^/[A-Z0-9+-\.]{7}$");
            if ((matches01 != null) && (matches01.Count > 0))
            {
                blnResult = true;//Console.WriteLine(@"合法 手機載具");
            }
            else
            {
                blnResult = false;//Console.WriteLine(@"非法 手機載具");
            }
            return blnResult;
        }

        public static bool VerifyTaxID(String StrTaxID)//營利事業統一編號檢核(統編 檢查)
        {
            //---
            //資料基礎檢查
            if (StrTaxID == null)
            {
                return false;
            }
            //---資料基礎檢查

            //---
            //長度 & 純數檢查 ~ 正則表達式/正規表達式
            Regex regex = new Regex(@"^\d{8}$");
            Match match = regex.Match(StrTaxID);
            if (!match.Success)
            {
                return false;
            }
            //長度 & 純數檢查

            //---
            //實值資料驗證
            /*
            http://superlevin.ifengyuan.tw/%E7%87%9F%E5%88%A9%E4%BA%8B%E6%A5%AD%E7%B5%B1%E4%B8%80%E7%B7%A8%E8%99%9F%E9%82%8F%E8%BC%AF%E6%AA%A2%E6%9F%A5%E6%96%B9%E6%B3%95/
            最近在設計新的商業程式，怕忘記了！記錄一下營利事業統一編號的驗證公式。

            (一) 長度：共八位，，全部為數字型態。
            (二) 計算公式
            1、各數字分別乘以 1,2,1,2,1,2,4,1。
            2、當第 7 位數為 7 者，可取相加之倒數第二位取 0 及 1 來計算其和。
            3、假如其和能被 10 整除，則表示營利事業統一編號正確
            */
            int[] idNoArray = StrTaxID.ToCharArray().Select(c => Convert.ToInt32(c.ToString())).ToArray();
            int[] weight = new int[] { 1, 2, 1, 2, 1, 2, 4, 1 };

            int subSum;     //小和
            int sum = 0;    //總和
            int sumFor7 = 1;
            for (int i = 0; i < idNoArray.Length; i++)
            {
                subSum = idNoArray[i] * weight[i];
                sum += (subSum / 10)   //商數
                     + (subSum % 10);  //餘數                
            }
            if (idNoArray[6] == 7)
            {
                //若第7碼=7，則會出現兩種數值都算對，因此要特別處理。
                sumFor7 = sum + 1;
            }
            //---實值資料驗證

            return ((sum % 10 == 0) || (sumFor7 % 10 == 0));
        }

        public static bool VerifyLoveCode(String StrData)//愛心捐贈碼檢核/檢查/驗證
        {
            /*
            如何驗證愛心捐贈碼	

            https://cynthiachuang.github.io/Check-Love-Code/
            捐贈碼(愛心)
                總長度為3至7碼字元
                全部由數字【0-9】組成
                正則表達式C#:  ^\d{3,7}$
            */

            /*
            對應正則表達式/正規表達式
            https://cynthiachuang.github.io/Check-E-Government-Uniform-Invoice/

            ^     -> 匹配字符串的開頭
            \d    -> 純數
            {3,7} -> 3至7碼
            $     -> 匹配字符串的結尾	
            */
            bool blnResult = false;

            MatchCollection matches01 = Regex.Matches(StrData, @"^\d{3,7}$");
            if ((matches01 != null) && (matches01.Count > 0))
            {
                blnResult = true;//Console.WriteLine(@"合法 愛心捐贈碼");
            }
            else
            {
                blnResult = false;//Console.WriteLine(@"非法 愛心捐贈碼");
            }
            return blnResult;
        }
    }
}
