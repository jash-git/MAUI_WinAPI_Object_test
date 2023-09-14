using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//---
//using Newtonsoft.Json;//使用外部函式庫-Newtonsoft.Json.dll (VS2010- .net4X )
//.net6 內建支援json
//https://docs.microsoft.com/zh-tw/dotnet/standard/serialization/system-text-json-how-to?pivots=dotnet-6-0
using System.Text.Json;
using System.IO.Ports;
//---.net6 內建支援json

namespace VPOS
{
    public class JsonClassConvert
    {
        private static bool m_blnlogfile = true;
        //---
        //oauth API
        public static String oauthInput2String(oauthInput inputbuf)
        {
            String StrResult = "";
            StrResult = JsonSerializer.Serialize(inputbuf);
            return StrResult;
        }
        public static oauthResult oauthResult2Class(String inputbuf)
        {
            oauthResult oauthResult = new oauthResult();
            if (inputbuf.Length > 0)
            {
                try
                {
                    oauthResult = JsonSerializer.Deserialize<oauthResult>(inputbuf);
                }
                catch
                {
                    oauthResult = null;
                }
            }
            else
            {
                oauthResult = null;
            }

            if ((m_blnlogfile) && (oauthResult == null))
            {
                String StrMsg = String.Format("oauthResult2Class:{0}", inputbuf);
                LogFile.Write("SystemError ; " + StrMsg);
            }
            return oauthResult;
        }
        //---oauth API

        //---
        //orders_new API
        public static String ordersnew2String(orders_new inputbuf)
        {
            String StrResult = "";
            
            try
            {
                StrResult = JsonSerializer.Serialize(inputbuf);
            }
            catch
            {
                StrResult = "";
            }

            StrResult = StrResult.Replace("product_sale_countCS0102", "product_sale_count");
            StrResult = StrResult.Replace("null", "\"\"");
            StrResult = StrResult.Replace("\"discount_info\":\"\",", "");
            StrResult = StrResult.Replace("\"print_flag\":\"\"", "\"print_flag\":\"N\"");
            StrResult = StrResult.Replace("\"condiments\":[],", "");
            StrResult = StrResult.Replace("\"packages\":[],", "");
            
            return StrResult;
        }

        public static orders_new ordersnew2Class(String inputbuf)
        {
            orders_new orders_newResult = new orders_new();
            if (inputbuf.Length > 0)
            {
                try
                {
                    orders_newResult = JsonSerializer.Deserialize<orders_new>(inputbuf);
                }
                catch
                {
                    orders_newResult = null;
                }
            }
            else
            {
                orders_newResult = null;
            }

            if ((m_blnlogfile) && (orders_newResult == null))
            {
                String StrMsg = String.Format("ordersnew2Class:{0}", inputbuf);
                LogFile.Write("SystemError ; " + StrMsg);
            }
            return orders_newResult;
        }

        public static orders_newResult ordersnewResult2Class(String inputbuf)
        {
            orders_newResult oauthResult=new orders_newResult();
            if (inputbuf.Length > 0)
            {
                try
                {
                    oauthResult = JsonSerializer.Deserialize<orders_newResult>(inputbuf);
                }
                catch
                {
                    oauthResult = null;
                }
            }
            else
            {
                oauthResult = null;
            }

            if ((m_blnlogfile) && (oauthResult == null))
            {
                String StrMsg = String.Format("ordersnewResult2Class:{0}", inputbuf);
                LogFile.Write("SystemError ; " + StrMsg);
            }

            return oauthResult;
        }
        //---orders_new API

        //---
        //orders_cancel API
        public static String orderscancelInput2String(orders_cancel inputbuf)
        {
            String StrResult = "";

            StrResult = JsonSerializer.Serialize(inputbuf);
            StrResult = StrResult.Replace("null", "\"\"");

            return StrResult;
        }
        //---orders_cancel

        //---
        //DiscountInfo
        public static DiscountInfo DiscountInfo2Class(String inputbuf)
        {
            
            DiscountInfo DiscountInfoResult = new DiscountInfo();

            if(inputbuf.Length>0)
            {
                try
                {
                    DiscountInfoResult = JsonSerializer.Deserialize<DiscountInfo>(inputbuf);
                }
                catch
                {
                    DiscountInfoResult = null;
                }
            }
            else
            {
                DiscountInfoResult = null;
            }
            if ((m_blnlogfile) && (DiscountInfoResult == null))
            {
                String StrMsg = String.Format("DiscountInfo2Class:{0}", inputbuf);
                LogFile.Write("SystemError ; " + StrMsg);
            }
            return DiscountInfoResult;
        }

        public static String DiscountInfo2String(DiscountInfo inputbuf)
        {
            String StrResult = "";

            StrResult = JsonSerializer.Serialize(inputbuf);
            StrResult = StrResult.Replace("null", "\"\"");

            return StrResult;
        }
        //---DiscountInfo

        //---
        //VLCS
        public static VLCS VLCS2Class(String inputbuf)
        {
            VLCS VLCSResult = new VLCS();

            if (inputbuf.Length > 0)
            {
                try
                {
                    VLCSResult = JsonSerializer.Deserialize<VLCS>(inputbuf);
                }
                catch
                {
                    VLCSResult = null;
                }
            }
            else
            {
                VLCSResult = null;
            }

            if ((m_blnlogfile) && (VLCSResult == null))
            {
                String StrMsg = String.Format("VLCS2Class:{0}", inputbuf);
                LogFile.Write("SystemError ; " + StrMsg);
            }

            return VLCSResult;
        }
        public static String VLCS2String(VLCS inputbuf)
        {
            String StrResult = "";

            StrResult = JsonSerializer.Serialize(inputbuf);
            StrResult = StrResult.Replace("null", "\"\"");

            return StrResult;
        }
        //---VLCS
        //---
        //get_terminal_data
        public static get_terminal_data get_terminal_data2Class(String inputbuf)
        {
            get_terminal_data get_terminal_dataResult = new get_terminal_data();

            if (inputbuf.Length > 0)
            {
                try
                {
                    get_terminal_dataResult = JsonSerializer.Deserialize<get_terminal_data>(inputbuf);
                }
                catch
                {
                    get_terminal_dataResult = null;
                }
            }
            else
            {
                get_terminal_dataResult = null;
            }

            if ((m_blnlogfile) && (get_terminal_dataResult == null))
            {
                String StrMsg = String.Format("get_terminal_data2Class:{0}", inputbuf);
                LogFile.Write("SystemError ; " + StrMsg);
            }

            return get_terminal_dataResult;
        }
        //---get_terminal_data

        //---
        //terminal_register
        public static terminal_register terminal_register2Class(String inputbuf)
        {
            terminal_register terminal_registerResult = new terminal_register();

            if (inputbuf.Length > 0)
            {
                try
                {
                    terminal_registerResult = JsonSerializer.Deserialize<terminal_register>(inputbuf);
                }
                catch
                {
                    terminal_registerResult = null;
                }
            }
            else
            {
                terminal_registerResult = null;
            }

            if((m_blnlogfile)&&(terminal_registerResult == null))
            {
                String StrMsg = String.Format("terminal_register2Class:{0}", inputbuf);
                LogFile.Write("SystemError ; " + StrMsg);
            }

            return terminal_registerResult;
        }
        //---terminal_register

        //---
        //get_company
        public static get_company get_company2Class(String inputbuf)
        {
            get_company get_companyResult = new get_company();

            if (inputbuf.Length > 0)
            {
                try
                {
                    get_companyResult = JsonSerializer.Deserialize<get_company>(inputbuf);
                }
                catch
                {
                    get_companyResult = null;
                }
            }
            else
            {
                get_companyResult = null;
            }

            if ((m_blnlogfile) && (get_companyResult == null))
            {
                String StrMsg = String.Format("get_company2Class:{0}", inputbuf);
                LogFile.Write("SystemError ; " + StrMsg);
            }

            return get_companyResult;
        }
        //---get_company

        //---
        //get_terminal_env_params
        public static get_terminal_env_params get_terminal_env_params2Class(String inputbuf)
        {
            get_terminal_env_params get_terminal_env_paramsResult = new get_terminal_env_params();

            if (inputbuf.Length > 0)
            {
                try
                {
                    get_terminal_env_paramsResult = JsonSerializer.Deserialize<get_terminal_env_params>(inputbuf);
                }
                catch
                {
                    get_terminal_env_paramsResult = null;
                }
            }
            else
            {
                get_terminal_env_paramsResult = null;
            }

            if ((m_blnlogfile) && (get_terminal_env_paramsResult == null))
            {
                String StrMsg = String.Format("get_terminal_env_params2Class:{0}", inputbuf);
                LogFile.Write("SystemError ; " + StrMsg);
            }

            return get_terminal_env_paramsResult;
        }
        //---get_terminal_env_params

        //---
        //get_terminal_func_main
        public static get_terminal_func_main get_terminal_func_main2Class(String inputbuf)
        {
            get_terminal_func_main get_terminal_func_mainResult = new get_terminal_func_main();

            if (inputbuf.Length > 0)
            {
                try
                {
                    get_terminal_func_mainResult = JsonSerializer.Deserialize<get_terminal_func_main>(inputbuf);
                }
                catch
                {
                    get_terminal_func_mainResult = null;
                }
            }
            else
            {
                get_terminal_func_mainResult = null;
            }

            if ((m_blnlogfile) && (get_terminal_func_mainResult == null))
            {
                String StrMsg = String.Format("get_terminal_func_main2Class:{0}", inputbuf);
                LogFile.Write("SystemError ; " + StrMsg);
            }

            return get_terminal_func_mainResult;
        }
        //---get_terminal_func_main

        //---
        //get_terminal_roles
        public static get_terminal_roles get_terminal_roles2Class(String inputbuf)
        {
            get_terminal_roles get_terminal_rolesResult = new get_terminal_roles();

            if (inputbuf.Length > 0)
            {
                try
                {
                    get_terminal_rolesResult = JsonSerializer.Deserialize<get_terminal_roles>(inputbuf);
                }
                catch
                {
                    get_terminal_rolesResult = null;
                }
            }
            else
            {
                get_terminal_rolesResult = null;
            }

            if ((m_blnlogfile) && (get_terminal_rolesResult == null))
            {
                String StrMsg = String.Format("get_terminal_roles2Class:{0}", inputbuf);
                LogFile.Write("SystemError ; " + StrMsg);
            }

            return get_terminal_rolesResult;
        }
        //---get_terminal_roles

        //---
        //get_order_type_data
        public static get_order_type_data get_order_type_data2Class(String inputbuf)
        {
            get_order_type_data get_order_type_dataResult = new get_order_type_data();

            if (inputbuf.Length > 0)
            {
                try
                {
                    get_order_type_dataResult = JsonSerializer.Deserialize<get_order_type_data>(inputbuf);
                }
                catch
                {
                    get_order_type_dataResult = null;
                }
            }
            else
            {
                get_order_type_dataResult = null;
            }

            if ((m_blnlogfile) && (get_order_type_dataResult == null))
            {
                String StrMsg = String.Format("get_order_type_data2Class:{0}", inputbuf);
                LogFile.Write("SystemError ; " + StrMsg);
            }

            return get_order_type_dataResult;
        }
        //---get_order_type_data

        //---
        //get_payment_module_params
        public static get_payment_module_params get_payment_module_params2Class(String inputbuf)
        {
            get_payment_module_params get_payment_module_paramsResult = new get_payment_module_params();

            if (inputbuf.Length > 0)
            {
                try
                {
                    get_payment_module_paramsResult = JsonSerializer.Deserialize<get_payment_module_params>(inputbuf);
                }
                catch
                {
                    get_payment_module_paramsResult = null;
                }
            }
            else
            {
                get_payment_module_paramsResult = null;
            }

            if ((m_blnlogfile) && (get_payment_module_paramsResult == null))
            {
                String StrMsg = String.Format("get_payment_module_params2Class:{0}", inputbuf);
                LogFile.Write("SystemError ; " + StrMsg);
            }

            return get_payment_module_paramsResult;
        }
        //---get_payment_module_params

        //---
        //get_company_payment_type
        public static get_company_payment_type get_company_payment_type2Class(String inputbuf)
        {
            get_company_payment_type get_company_payment_typeResult = new get_company_payment_type();

            if (inputbuf.Length > 0)
            {
                try
                {
                    get_company_payment_typeResult = JsonSerializer.Deserialize<get_company_payment_type>(inputbuf);
                }
                catch
                {
                    get_company_payment_typeResult = null;
                }
            }
            else
            {
                get_company_payment_typeResult = null;
            }

            if ((m_blnlogfile) && (get_company_payment_typeResult == null))
            {
                String StrMsg = String.Format("get_company_payment_type2Class:{0}", inputbuf);
                LogFile.Write("SystemError ; " + StrMsg);
            }

            return get_company_payment_typeResult;
        }
        //---get_company_payment_type

        //---
        //get_invoice_platform
        public static get_invoice_platform get_invoice_platform2Class(String inputbuf)
        {
            get_invoice_platform get_invoice_platformResult = new get_invoice_platform();

            if (inputbuf.Length > 0)
            {
                try
                {
                    get_invoice_platformResult = JsonSerializer.Deserialize<get_invoice_platform>(inputbuf);
                }
                catch
                {
                    get_invoice_platformResult = null;
                }
            }
            else
            {
                get_invoice_platformResult = null;
            }

            if ((m_blnlogfile) && (get_invoice_platformResult == null))
            {
                String StrMsg = String.Format("get_invoice_platform2Class:{0}", inputbuf);
                LogFile.Write("SystemError ; " + StrMsg);
            }

            return get_invoice_platformResult;
        }
        //---get_invoice_platform

        //---
        //get_company_invoice_params
        public static get_company_invoice_params get_company_invoice_params2Class(String inputbuf)
        {
            get_company_invoice_params get_company_invoice_paramsResult = new get_company_invoice_params();

            if (inputbuf.Length > 0)
            {
                try
                {
                    get_company_invoice_paramsResult = JsonSerializer.Deserialize<get_company_invoice_params>(inputbuf);
                }
                catch
                {
                    get_company_invoice_paramsResult = null;
                }
            }
            else
            {
                get_company_invoice_paramsResult = null;
            }

            if ((m_blnlogfile) && (get_company_invoice_paramsResult == null))
            {
                String StrMsg = String.Format("get_company_invoice_params2Class:{0}", inputbuf);
                LogFile.Write("SystemError ; " + StrMsg);
            }

            return get_company_invoice_paramsResult;
        }
        //---get_company_invoice_params

        //---
        //get_company_customized_params
        public static get_company_customized_params get_company_customized_params2Class(String inputbuf)
        {
            get_company_customized_params get_company_customized_paramsResult = new get_company_customized_params();

            if (inputbuf.Length > 0)
            {
                try
                {
                    get_company_customized_paramsResult = JsonSerializer.Deserialize<get_company_customized_params>(inputbuf);
                }
                catch
                {
                    get_company_customized_paramsResult = null;
                }
            }
            else
            {
                get_company_customized_paramsResult = null;
            }

            if ((m_blnlogfile) && (get_company_customized_paramsResult == null))
            {
                String StrMsg = String.Format("get_company_customized_params2Class:{0}", inputbuf);
                LogFile.Write("SystemError ; " + StrMsg);
            }

            return get_company_customized_paramsResult;
        }
        //---get_company_customized_params

        //---
        //get_takeaways_platform
        public static get_takeaways_platform get_takeaways_platform2Class(String inputbuf)
        {
            get_takeaways_platform get_takeaways_platformResult = new get_takeaways_platform();

            if (inputbuf.Length > 0)
            {
                try
                {
                    get_takeaways_platformResult = JsonSerializer.Deserialize<get_takeaways_platform>(inputbuf);
                }
                catch
                {
                    get_takeaways_platformResult = null;
                }
            }
            else
            {
                get_takeaways_platformResult = null;
            }

            if ((m_blnlogfile) && (get_takeaways_platformResult == null))
            {
                String StrMsg = String.Format("get_takeaways_platform2Class:{0}", inputbuf);
                LogFile.Write("SystemError ; " + StrMsg);
            }

            return get_takeaways_platformResult;
        }
        //---get_takeaways_platform

        //---
        //get_takeaways_params
        public static get_takeaways_params get_takeaways_params2Class(String inputbuf)
        {
            get_takeaways_params get_takeaways_paramsResult = new get_takeaways_params();

            if (inputbuf.Length > 0)
            {
                try
                {
                    get_takeaways_paramsResult = JsonSerializer.Deserialize<get_takeaways_params>(inputbuf);
                }
                catch
                {
                    get_takeaways_paramsResult = null;
                }
            }
            else
            {
                get_takeaways_paramsResult = null;
            }

            if ((m_blnlogfile) && (get_takeaways_paramsResult == null))
            {
                String StrMsg = String.Format("get_takeaways_params2Class:{0}", inputbuf);
                LogFile.Write("SystemError ; " + StrMsg);
            }

            return get_takeaways_paramsResult;
        }
        //---get_takeaways_params

        //---
        //get_member_platform_params
        public static get_member_platform_params get_member_platform_params2Class(String inputbuf)
        {
            get_member_platform_params get_member_platform_paramsResult = new get_member_platform_params();

            if (inputbuf.Length > 0)
            {
                try
                {
                    get_member_platform_paramsResult = JsonSerializer.Deserialize<get_member_platform_params>(inputbuf);
                }
                catch
                {
                    get_member_platform_paramsResult = null;
                }
            }
            else
            {
                get_member_platform_paramsResult = null;
            }

            if ((m_blnlogfile) && (get_member_platform_paramsResult == null))
            {
                String StrMsg = String.Format("get_member_platform_params2Class:{0}", inputbuf);
                LogFile.Write("SystemError ; " + StrMsg);
            }

            return get_member_platform_paramsResult;
        }
        //---get_member_platform_params

        //---
        //get_user_data
        public static get_user_data get_user_data2Class(String inputbuf)
        {
            get_user_data get_user_dataResult = new get_user_data();

            if (inputbuf.Length > 0)
            {
                try
                {
                    get_user_dataResult = JsonSerializer.Deserialize<get_user_data>(inputbuf);
                }
                catch
                {
                    get_user_dataResult = null;
                }
            }
            else
            {
                get_user_dataResult = null;
            }

            if ((m_blnlogfile) && (get_user_dataResult == null))
            {
                String StrMsg = String.Format("get_user_data2Class:{0}", inputbuf);
                LogFile.Write("SystemError ; " + StrMsg);
            }

            return get_user_dataResult;
        }
        //---get_user_data

        //---
        //get_class_data
        public static get_class_data get_class_data2Class(String inputbuf)
        {
            get_class_data get_class_dataResult = new get_class_data();

            if (inputbuf.Length > 0)
            {
                try
                {
                    get_class_dataResult = JsonSerializer.Deserialize<get_class_data>(inputbuf);
                }
                catch
                {
                    get_class_dataResult = null;
                }
            }
            else
            {
                get_class_dataResult = null;
            }

            if ((m_blnlogfile) && (get_class_dataResult == null))
            {
                String StrMsg = String.Format("get_class_data2Class:{0}", inputbuf);
                LogFile.Write("SystemError ; " + StrMsg);
            }

            return get_class_dataResult;
        }
        //---get_class_data

        //---
        //get_tax_data
        public static get_tax_data get_tax_data2Class(String inputbuf)
        {
            get_tax_data get_tax_dataResult = new get_tax_data();

            if (inputbuf.Length > 0)
            {
                try
                {
                    get_tax_dataResult = JsonSerializer.Deserialize<get_tax_data>(inputbuf);
                }
                catch
                {
                    get_tax_dataResult = null;
                }
            }
            else
            {
                get_tax_dataResult = null;
            }

            if ((m_blnlogfile) && (get_tax_dataResult == null))
            {
                String StrMsg = String.Format("get_tax_data2Class:{0}", inputbuf);
                LogFile.Write("SystemError ; " + StrMsg);
            }

            return get_tax_dataResult;
        }
        //---get_tax_data

        //---
        //get_product_unit
        public static get_product_unit get_product_unit2Class(String inputbuf)
        {
            get_product_unit get_product_unitResult = new get_product_unit();

            if (inputbuf.Length > 0)
            {
                try
                {
                    get_product_unitResult = JsonSerializer.Deserialize<get_product_unit>(inputbuf);
                }
                catch
                {
                    get_product_unitResult = null;
                }
            }
            else
            {
                get_product_unitResult = null;
            }

            if ((m_blnlogfile) && (get_product_unitResult == null))
            {
                String StrMsg = String.Format("get_product_unit2Class:{0}", inputbuf);
                LogFile.Write("SystemError ; " + StrMsg);
            }

            return get_product_unitResult;
        }
        //---get_product_unit

        //---
        //get_price_typet
        public static get_price_type get_price_type2Class(String inputbuf)
        {
            get_price_type get_price_typeResult = new get_price_type();

            if (inputbuf.Length > 0)
            {
                try
                {
                    get_price_typeResult = JsonSerializer.Deserialize<get_price_type>(inputbuf);
                }
                catch
                {
                    get_price_typeResult = null;
                }
            }
            else
            {
                get_price_typeResult = null;
            }

            if ((m_blnlogfile) && (get_price_typeResult == null))
            {
                String StrMsg = String.Format("get_price_type2Class:{0}", inputbuf);
                LogFile.Write("SystemError ; " + StrMsg);
            }

            return get_price_typeResult;
        }
        //---get_price_typet

        //---
        //get_condiment_group
        public static get_condiment_group get_condiment_group2Class(String inputbuf)
        {
            get_condiment_group get_condiment_groupResult = new get_condiment_group();

            if (inputbuf.Length > 0)
            {
                try
                {
                    get_condiment_groupResult = JsonSerializer.Deserialize<get_condiment_group>(inputbuf);
                }
                catch
                {
                    get_condiment_groupResult = null;
                }
            }
            else
            {
                get_condiment_groupResult = null;
            }

            if ((m_blnlogfile) && (get_condiment_groupResult == null))
            {
                String StrMsg = String.Format("get_condiment_group2Class:{0}", inputbuf);
                LogFile.Write("SystemError.log ; " + StrMsg);
            }

            return get_condiment_groupResult;
        }
        //---get_condiment_group

        //---
        //get_condiment_data
        public static get_condiment_data get_condiment_data2Class(String inputbuf)
        {
            get_condiment_data get_condiment_dataResult = new get_condiment_data();

            if (inputbuf.Length > 0)
            {
                try
                {
                    get_condiment_dataResult = JsonSerializer.Deserialize<get_condiment_data>(inputbuf);
                }
                catch
                {
                    get_condiment_dataResult = null;
                }
            }
            else
            {
                get_condiment_dataResult = null;
            }

            if ((m_blnlogfile) && (get_condiment_dataResult == null))
            {
                String StrMsg = String.Format("get_condiment_data2Class:{0}", inputbuf);
                LogFile.Write("SystemError ; " + StrMsg);
            }

            return get_condiment_dataResult;
        }
        //---get_condiment_data

        //---
        //get_products_data
        public static get_products_data get_products_data2Class(String inputbuf)
        {
            get_products_data get_products_dataResult = new get_products_data();

            if (inputbuf.Length > 0)
            {
                try
                {
                    get_products_dataResult = JsonSerializer.Deserialize<get_products_data>(inputbuf);
                }
                catch
                {
                    get_products_dataResult = null;
                }
            }
            else
            {
                get_products_dataResult = null;
            }

            if ((m_blnlogfile) && (get_products_dataResult == null))
            {
                String StrMsg = String.Format("get_products_data2Class:{0}", inputbuf);
                LogFile.Write("SystemError ; " + StrMsg);
            }

            return get_products_dataResult;
        }
        //---get_products_data

        //---
        //get_products_category
        public static get_products_category get_products_category2Class(String inputbuf)
        {
            get_products_category get_products_categoryResult = new get_products_category();

            if (inputbuf.Length > 0)
            {
                try
                {
                    get_products_categoryResult = JsonSerializer.Deserialize<get_products_category>(inputbuf);
                }
                catch
                {
                    get_products_categoryResult = null;
                }
            }
            else
            {
                get_products_categoryResult = null;
            }

            if ((m_blnlogfile) && (get_products_categoryResult == null))
            {
                String StrMsg = String.Format("get_products_category2Class:{0}", inputbuf);
                LogFile.Write("SystemError ; " + StrMsg);
            }

            return get_products_categoryResult;
        }
        //---get_products_category

        //---
        //get_product_spec_data
        public static get_product_spec_data get_product_spec_data2Class(String inputbuf)
        {
            get_product_spec_data get_product_spec_dataResult = new get_product_spec_data();

            if (inputbuf.Length > 0)
            {
                try
                {
                    get_product_spec_dataResult = JsonSerializer.Deserialize<get_product_spec_data>(inputbuf);
                }
                catch
                {
                    get_product_spec_dataResult = null;
                }
            }
            else
            {
                get_product_spec_dataResult = null;
            }

            if ((m_blnlogfile) && (get_product_spec_dataResult == null))
            {
                String StrMsg = String.Format("get_product_spec_data2Class:{0}", inputbuf);
                LogFile.Write("SystemError ; " + StrMsg);
            }

            return get_product_spec_dataResult;
        }
        //---get_product_spec_data

        //---
        //get_set_attribute_data
        public static get_set_attribute_data get_set_attribute_data2Class(String inputbuf)
        {
            get_set_attribute_data get_set_attribute_dataResult = new get_set_attribute_data();

            if (inputbuf.Length > 0)
            {
                try
                {
                    get_set_attribute_dataResult = JsonSerializer.Deserialize<get_set_attribute_data>(inputbuf);
                }
                catch
                {
                    get_set_attribute_dataResult = null;
                }
            }
            else
            {
                get_set_attribute_dataResult = null;
            }

            if ((m_blnlogfile) && (get_set_attribute_dataResult == null))
            {
                String StrMsg = String.Format("get_set_attribute_data2Class:{0}", inputbuf);
                LogFile.Write("SystemError.log ; " + StrMsg);
            }

            return get_set_attribute_dataResult;
        }
        //---get_set_attribute_data

        //---
        //get_promotion_data
        public static get_promotion_data get_promotion_data2Class(String inputbuf)
        {
            get_promotion_data get_promotion_dataResult = new get_promotion_data();

            if (inputbuf.Length > 0)
            {
                try
                {
                    get_promotion_dataResult = JsonSerializer.Deserialize<get_promotion_data>(inputbuf);
                }
                catch
                {
                    get_promotion_dataResult = null;
                }
            }
            else
            {
                get_promotion_dataResult = null;
            }

            if ((m_blnlogfile) && (get_promotion_dataResult == null))
            {
                String StrMsg = String.Format("get_promotion_data2Class:{0}", inputbuf);
                LogFile.Write("SystemError ; " + StrMsg);
            }

            return get_promotion_dataResult;
        }
        //---get_promotion_data

        //---
        //get_store_table_data
        public static get_store_table_data get_store_table_data2Class(String inputbuf)
        {
            get_store_table_data get_store_table_dataResult = new get_store_table_data();

            if (inputbuf.Length > 0)
            {
                try
                {
                    get_store_table_dataResult = JsonSerializer.Deserialize<get_store_table_data>(inputbuf);
                }
                catch
                {
                    get_store_table_dataResult = null;
                }
            }
            else
            {
                get_store_table_dataResult = null;
            }

            if ((m_blnlogfile) && (get_store_table_dataResult == null))
            {
                String StrMsg = String.Format("get_store_table_data2Class:{0}", inputbuf);
                LogFile.Write("SystemError ; " + StrMsg);
            }

            return get_store_table_dataResult;
        }
        //---get_store_table_data

        //---
        //get_formula_data
        public static get_formula_data get_formula_data2Class(String inputbuf)
        {
            get_formula_data get_formula_dataResult = new get_formula_data();

            if (inputbuf.Length > 0)
            {
                try
                {
                    get_formula_dataResult = JsonSerializer.Deserialize<get_formula_data>(inputbuf);
                }
                catch
                {
                    get_formula_dataResult = null;
                }
            }
            else
            {
                get_formula_dataResult = null;
            }

            if ((m_blnlogfile) && (get_formula_dataResult == null))
            {
                String StrMsg = String.Format("get_formula_data2Class:{0}", inputbuf);
                LogFile.Write("SystemError ; " + StrMsg);
            }

            return get_formula_dataResult;
        }
        //---get_formula_data

        //---
        //get_printer_data
        public static get_printer_data get_printer_data2Class(String inputbuf)
        {
            get_printer_data get_printer_dataResult = new get_printer_data();

            if (inputbuf.Length > 0)
            {
                try
                {
                    get_printer_dataResult = JsonSerializer.Deserialize<get_printer_data>(inputbuf);
                }
                catch
                {
                    get_printer_dataResult = null;
                }
            }
            else
            {
                get_printer_dataResult = null;
            }

            if ((m_blnlogfile) && (get_printer_dataResult == null))
            {
                String StrMsg = String.Format("get_printer_data2Class:{0}", inputbuf);
                LogFile.Write("SystemError ; " + StrMsg);
            }

            return get_printer_dataResult;
        }
        //---get_printer_data

        //---
        //get_printer_group_data
        public static get_printer_group_data get_printer_group_data2Class(String inputbuf)
        {
            get_printer_group_data get_printer_group_dataResult = new get_printer_group_data();

            if (inputbuf.Length > 0)
            {
                try
                {
                    get_printer_group_dataResult = JsonSerializer.Deserialize<get_printer_group_data>(inputbuf);
                }
                catch
                {
                    get_printer_group_dataResult = null;
                }
            }
            else
            {
                get_printer_group_dataResult = null;
            }

            if ((m_blnlogfile) && (get_printer_group_dataResult == null))
            {
                String StrMsg = String.Format("get_printer_group_data2Class:{0}", inputbuf);
                LogFile.Write("SystemError ; " + StrMsg);
            }

            return get_printer_group_dataResult;
        }
        //---get_printer_group_data

        //---
        //get_packaging_type
        public static get_packaging_type get_packaging_type2Class(String inputbuf)
        {
            get_packaging_type get_packaging_typeResult = new get_packaging_type();

            if (inputbuf.Length > 0)
            {
                try
                {
                    get_packaging_typeResult = JsonSerializer.Deserialize<get_packaging_type>(inputbuf);
                }
                catch
                {
                    get_packaging_typeResult = null;
                }
            }
            else
            {
                get_packaging_typeResult = null;
            }

            if ((m_blnlogfile) && (get_packaging_typeResult == null))
            {
                String StrMsg = String.Format("get_packaging_type2Class:{0}", inputbuf);
                LogFile.Write("SystemError ; " + StrMsg);
            }

            return get_packaging_typeResult;
        }
        //---get_packaging_type

        //---
        //get_packaging_data
        public static get_packaging_data get_packaging_data2Class(String inputbuf)
        {
            get_packaging_data get_packaging_dataResult = new get_packaging_data();

            if (inputbuf.Length > 0)
            {
                try
                {
                    get_packaging_dataResult = JsonSerializer.Deserialize<get_packaging_data>(inputbuf);
                }
                catch
                {
                    get_packaging_dataResult = null;
                }
            }
            else
            {
                get_packaging_dataResult = null;
            }

            if ((m_blnlogfile) && (get_packaging_dataResult == null))
            {
                String StrMsg = String.Format("get_packaging_data2Class:{0}", inputbuf);
                LogFile.Write("SystemError ; " + StrMsg);
            }

            return get_packaging_dataResult;
        }
        //---get_packaging_data

        //---
        //get_discount_param
        public static get_discount_param get_discount_param2Class(String inputbuf)
        {
            get_discount_param get_discount_paramResult = new get_discount_param();

            if (inputbuf.Length > 0)
            {
                try
                {
                    get_discount_paramResult = JsonSerializer.Deserialize<get_discount_param>(inputbuf);
                }
                catch
                {
                    get_discount_paramResult = null;
                }
            }
            else
            {
                get_discount_paramResult = null;
            }

            if ((m_blnlogfile) && (get_discount_paramResult == null))
            {
                String StrMsg = String.Format("get_discount_param2Class:{0}", inputbuf);
                LogFile.Write("SystemError ; " + StrMsg);
            }

            return get_discount_paramResult;
        }
        //---get_discount_param

        //---
        //get_discount_hotkey
        public static get_discount_hotkey get_discount_hotkey2Class(String inputbuf)
        {
            get_discount_hotkey get_discount_hotkeyResult = new get_discount_hotkey();

            if (inputbuf.Length > 0)
            {
                try
                {
                    get_discount_hotkeyResult = JsonSerializer.Deserialize<get_discount_hotkey>(inputbuf);
                }
                catch
                {
                    get_discount_hotkeyResult = null;
                }
            }
            else
            {
                get_discount_hotkeyResult = null;
            }

            if ((m_blnlogfile) && (get_discount_hotkeyResult == null))
            {
                String StrMsg = String.Format("get_discount_hotkey2Class:{0}", inputbuf);
                LogFile.Write("SystemError ; " + StrMsg);
            }

            return get_discount_hotkeyResult;
        }
        //---get_discount_hotkey

        //---
        //get_account_data
        public static get_account_data get_account_data2Class(String inputbuf)
        {
            get_account_data get_account_dataResult = new get_account_data();

            if (inputbuf.Length > 0)
            {
                try
                {
                    get_account_dataResult = JsonSerializer.Deserialize<get_account_data>(inputbuf);
                }
                catch
                {
                    get_account_dataResult = null;
                }
            }
            else
            {
                get_account_dataResult = null;
            }

            if ((m_blnlogfile) && (get_account_dataResult == null))
            {
                String StrMsg = String.Format("get_account_data2Class:{0}", inputbuf);
                LogFile.Write("SystemError ; " + StrMsg);
            }

            return get_account_dataResult;
        }
        //---get_account_data

        //---
        //get_basic_params_param_value
        public static get_basic_params_param_value get_basic_params_param_value2Class(String inputbuf)
        {
            get_basic_params_param_value get_basic_params_param_valueResult = new get_basic_params_param_value();

            if (inputbuf.Length > 0)
            {
                try
                {
                    get_basic_params_param_valueResult = JsonSerializer.Deserialize<get_basic_params_param_value>(inputbuf);
                }
                catch
                {
                    get_basic_params_param_valueResult = null;
                }
            }
            else
            {
                get_basic_params_param_valueResult = null;
            }

            if ((m_blnlogfile) && (get_basic_params_param_valueResult == null))
            {
                String StrMsg = String.Format("get_basic_params_param_value2Class:{0}", inputbuf);
                LogFile.Write("SystemError ; " + StrMsg);
            }

            return get_basic_params_param_valueResult;
        }
        //---get_basic_params_param_value
        //---
        //get_basic_params_CALL_NUM_PARAM
        public static get_basic_params_CALL_NUM_PARAM get_basic_params_CALL_NUM_PARAM2Class(String inputbuf)
        {
            get_basic_params_CALL_NUM_PARAM get_basic_params_CALL_NUM_PARAMResult = new get_basic_params_CALL_NUM_PARAM();

            if (inputbuf.Length > 0)
            {
                try
                {
                    get_basic_params_CALL_NUM_PARAMResult = JsonSerializer.Deserialize<get_basic_params_CALL_NUM_PARAM>(inputbuf);
                }
                catch
                {
                    get_basic_params_CALL_NUM_PARAMResult = null;
                }
            }
            else
            {
                get_basic_params_CALL_NUM_PARAMResult = null;
            }

            if ((m_blnlogfile) && (get_basic_params_CALL_NUM_PARAMResult == null))
            {
                String StrMsg = String.Format("get_basic_params_CALL_NUM_PARAM2Class:{0}", inputbuf);
                LogFile.Write("SystemError ; " + StrMsg);
            }

            return get_basic_params_CALL_NUM_PARAMResult;
        }
        //---get_basic_params_CALL_NUM_PARAM

        //---
        //pos_serial_param
        public static String pos_serial_param2String(pos_serial_param inputbuf)
        {
            String StrResult = "";
            StrResult = JsonSerializer.Serialize(inputbuf);
            return StrResult;
        }

        public static pos_serial_param pos_serial_param2Class(String inputbuf)
        {
            pos_serial_param pos_serial_paramResult = new pos_serial_param();

            if (inputbuf.Length > 0)
            {
                try
                {
                    pos_serial_paramResult = JsonSerializer.Deserialize<pos_serial_param>(inputbuf);
                }
                catch
                {
                    pos_serial_paramResult = null;
                }
            }
            else
            {
                pos_serial_paramResult = null;
            }

            if ((m_blnlogfile) && (pos_serial_paramResult == null))
            {
                String StrMsg = String.Format("pos_serial_param2Class:{0}", inputbuf);
                LogFile.Write("SystemError ; " + StrMsg);
            }

            return pos_serial_paramResult;
        }
        //---pos_serial_param

        //---
        //vteam_kds_api_info
        public static String vteam_kds_api_info2String(vteam_kds_api_info inputbuf)
        {
            String StrResult = "";
            StrResult = JsonSerializer.Serialize(inputbuf);
            return StrResult;
        }

        public static vteam_kds_api_info vteam_kds_api_info2Class(String inputbuf)
        {
            vteam_kds_api_info vteam_kds_api_infoResult = new vteam_kds_api_info();

            if (inputbuf.Length > 0)
            {
                try
                {
                    vteam_kds_api_infoResult = JsonSerializer.Deserialize<vteam_kds_api_info>(inputbuf);
                }
                catch
                {
                    vteam_kds_api_infoResult = null;
                }
            }
            else
            {
                vteam_kds_api_infoResult = null;
            }

            if ((m_blnlogfile) && (vteam_kds_api_infoResult == null))
            {
                String StrMsg = String.Format("vteam_kds_api_info2Class:{0}", inputbuf);
                LogFile.Write("SystemError ; " + StrMsg);
            }

            return vteam_kds_api_infoResult;
        }
        //---vteam_kds_api_info

        //---
        //order_type_data_params
        public static order_type_data_params order_type_data_params2Class(String inputbuf)
        {
            order_type_data_params order_type_data_paramsResult = new order_type_data_params();

            if (inputbuf.Length > 0)
            {
                try
                {
                    order_type_data_paramsResult = JsonSerializer.Deserialize<order_type_data_params>(inputbuf);
                }
                catch
                {
                    order_type_data_paramsResult = null;
                }
            }
            else
            {
                order_type_data_paramsResult = null;
            }

            if ((m_blnlogfile) && (order_type_data_paramsResult == null))
            {
                String StrMsg = String.Format("order_type_data_params2Class:{0}", inputbuf);
                LogFile.Write("SystemError ; " + StrMsg);
            }

            return order_type_data_paramsResult;
        }
        //---order_type_data_params

        //---
        //
        public static String CHaccount_data2String(get_CHaccount_data inputbuf)
        {
            String StrResult = "";
            StrResult = JsonSerializer.Serialize(inputbuf);
            StrResult= StrResult.Replace("{\"data\":", "");
            StrResult = StrResult.Substring(0, StrResult.Length - 1);
            return StrResult;
        }
        public static String CHpayment_data2String(get_CHpayment_data inputbuf)
        {
            String StrResult = "";
            StrResult = JsonSerializer.Serialize(inputbuf);
            StrResult = StrResult.Replace("{\"data\":", "");
            StrResult = StrResult.Substring(0, StrResult.Length - 1);
            return StrResult;
        }
        //---

        //---
        //FMDAattribute
        public static FMDAattribute FMDAattribute2Class(String inputbuf)
        {
            FMDAattribute FMDAattributeResult = new FMDAattribute();

            if (inputbuf.Length > 0)
            {
                try
                {
                    FMDAattributeResult = JsonSerializer.Deserialize<FMDAattribute>(inputbuf);
                }
                catch
                {
                    FMDAattributeResult = null;
                }
            }
            else
            {
                FMDAattributeResult = null;
            }

            if ((m_blnlogfile) && (FMDAattributeResult == null))
            {
                String StrMsg = String.Format("FMDAattribute2Class:{0}", inputbuf);
                LogFile.Write("SystemError ; " + StrMsg);
            }

            return FMDAattributeResult;
        }
        //---FMDAattribute

        //---
        //promotion_data_rule
        public static promotion_data_rule promotion_data_rule2Class(String inputbuf)
        {
            promotion_data_rule promotion_data_ruleResult = new promotion_data_rule();

            if (inputbuf.Length > 0)
            {
                try
                {
                    promotion_data_ruleResult = JsonSerializer.Deserialize<promotion_data_rule>(inputbuf);
                }
                catch
                {
                    promotion_data_ruleResult = null;
                }
            }
            else
            {
                promotion_data_ruleResult = null;
            }

            if ((m_blnlogfile) && (promotion_data_ruleResult == null))
            {
                String StrMsg = String.Format("promotion_data_rule2Class:{0}", inputbuf);
                LogFile.Write("SystemError ; " + StrMsg);
            }

            return promotion_data_ruleResult;
        }
        //---promotion_data_rule

        //---
        //ODPromotionValue
        public static ODPromotionValue ODPromotionValue2Class(String inputbuf)
        {
            ODPromotionValue ODPromotionValueResult = new ODPromotionValue();

            if (inputbuf.Length > 0)
            {
                inputbuf = inputbuf.Insert(0, @"{""Array"":");
                inputbuf += "}";
                try
                {
                    ODPromotionValueResult = JsonSerializer.Deserialize<ODPromotionValue>(inputbuf);
                }
                catch
                {
                    ODPromotionValueResult = null;
                }
            }
            else
            {
                ODPromotionValueResult = null;
            }

            if ((m_blnlogfile) && (ODPromotionValueResult == null))
            {
                String StrMsg = String.Format("ODPromotionValue2Class:{0}", inputbuf);
                LogFile.Write("SystemError ; " + StrMsg);
            }

            return ODPromotionValueResult;
        }
        public static String ODPromotionValue2String(ODPromotionValue inputbuf)
        {
            String StrResult = "";
            StrResult = JsonSerializer.Serialize(inputbuf);
            StrResult = StrResult.Replace(@"{""Array"":", "");
            StrResult = StrResult.Substring(0, StrResult.Length - 1);
            return StrResult;
        }
        //---ODPromotionValue

        //---
        //getorderno
        public static getorderno getorderno2Class(String inputbuf)
        {
            getorderno getordernoResult = new getorderno();

            if (inputbuf.Length > 0)
            {
                try
                {
                    getordernoResult = JsonSerializer.Deserialize<getorderno>(inputbuf);
                }
                catch
                {
                    getordernoResult = null;
                }
            }
            else
            {
                getordernoResult = null;
            }

            if ((m_blnlogfile) && (getordernoResult == null))
            {
                String StrMsg = String.Format("getorderno2Class:{0}", inputbuf);
                LogFile.Write("SystemError ; " + StrMsg);
            }

            return getordernoResult;
        }

        public static getdailyno getdailyno2Class(String inputbuf)
        {
            getdailyno getdailynoResult = new getdailyno();

            if (inputbuf.Length > 0)
            {
                try
                {
                    getdailynoResult = JsonSerializer.Deserialize<getdailyno>(inputbuf);
                }
                catch
                {
                    getdailynoResult = null;
                }
            }
            else
            {
                getdailynoResult = null;
            }

            if ((m_blnlogfile) && (getdailynoResult == null))
            {
                String StrMsg = String.Format("getdailyno2Class:{0}", inputbuf);
                LogFile.Write("SystemError ; " + StrMsg);
            }

            return getdailynoResult;
        }

        public static getclassno getclassno2Class(String inputbuf)
        {
            getclassno getclassnoResult = new getclassno();

            if (inputbuf.Length > 0)
            {
                try
                {
                    getclassnoResult = JsonSerializer.Deserialize<getclassno>(inputbuf);
                }
                catch
                {
                    getclassnoResult = null;
                }
            }
            else
            {
                getclassnoResult = null;
            }

            if ((m_blnlogfile) && (getclassnoResult == null))
            {
                String StrMsg = String.Format("getclassno2Class:{0}", inputbuf);
                LogFile.Write("SystemError ; " + StrMsg);
            }

            return getclassnoResult;
        }
        //---getorderno

        //---
        //testconnection
        public static testconnection testconnection2Class(String inputbuf)
        {
            testconnection testconnectionResult = new testconnection();

            if (inputbuf.Length > 0)
            {
                try
                {
                    testconnectionResult = JsonSerializer.Deserialize<testconnection>(inputbuf);
                }
                catch
                {
                    testconnectionResult = null;
                }
            }
            else
            {
                testconnectionResult = null;
            }

            if ((m_blnlogfile) && (testconnectionResult == null))
            {
                String StrMsg = String.Format("testconnection2Class:{0}", inputbuf);
                LogFile.Write("SystemError ; " + StrMsg);
            }

            return testconnectionResult;
        }
        //---testconnection

        //---
        //printer_config
        public static printer_config printer_config2Class(String inputbuf)
        {
            printer_config printer_configResult = new printer_config();

            if (inputbuf.Length > 0)
            {
                try
                {
                    printer_configResult = JsonSerializer.Deserialize<printer_config>(inputbuf);
                    switch(printer_configResult.conn_type) 
                    {
                        case "RS232":
                            if((printer_configResult.serial_port!=null) &&(printer_configResult.baud_rate!=null))
                            {
                                printer_configResult.conn_data = new string("");
                                printer_configResult.conn_data = $"{printer_configResult.serial_port};{printer_configResult.baud_rate}";
                            }
                            break;
                        case "DRIVER":
                            printer_configResult.conn_type = "Dirver";
                            if(printer_configResult.driver_name!=null)
                            {
                                printer_configResult.conn_data = new string("");
                                printer_configResult.conn_data = printer_configResult.driver_name;
                            }
                            break;
                        case "TCPIP":
                            printer_configResult.conn_type = "TCP/IP";
                            if((printer_configResult.tcp_ip_address!=null) && (printer_configResult.tcp_port!=null))
                            {
                                printer_configResult.conn_data = new string("");
                                printer_configResult.conn_data = $"{printer_configResult.tcp_ip_address};{printer_configResult.tcp_port}";
                            }
                            break;
                    }
                }
                catch
                {
                    printer_configResult = null;
                }
            }
            else
            {
                printer_configResult = null;
            }

            if ((m_blnlogfile) && (printer_configResult == null))
            {
                String StrMsg = String.Format("printer_config2Class:{0}", inputbuf);
                LogFile.Write("SystemError ; " + StrMsg);
            }

            return printer_configResult;
        }

        public static String printer_config2String(printer_config inputbuf)
        {
            String StrResult = "";
            StrResult = JsonSerializer.Serialize(inputbuf);
            return StrResult;
        }
        //---printer_config

        //---
        //ProductMemo
        public static String product_memo2String(product_memo inputbuf)
        {
            String StrResult = "";

            StrResult = JsonSerializer.Serialize(inputbuf);
            return StrResult;
        }
        //---ProductMemo

        //---
        //Formula_Data
        public static Formula_Data Formula_Data2Class(String inputbuf)
        {
            Formula_Data Formula_DataResult = new Formula_Data();

            if (inputbuf.Length > 0)
            {
                try
                {
                    Formula_DataResult = JsonSerializer.Deserialize<Formula_Data>(inputbuf);
                }
                catch
                {
                    Formula_DataResult = null;
                }
            }
            else
            {
                Formula_DataResult = null;
            }

            if ((m_blnlogfile) && (Formula_DataResult == null))
            {
                String StrMsg = String.Format("Formula_Data2Class:{0}", inputbuf);
                LogFile.Write("SystemError ; " + StrMsg);
            }

            return Formula_DataResult;
        }
        //---Formula_Data

        //---
        //daily_report
        public static String daily_report2String(daily_report inputbuf)
        {
            String StrResult = "";

            StrResult = JsonSerializer.Serialize(inputbuf);
            return StrResult;
        }
        //---daily_report

        //---
        //DRPaymentInfo
        public static List<DRPaymentInfo> DRPaymentInfo2Class(String inputbuf)
        {
            List<DRPaymentInfo> DRPaymentInfoResult = new List<DRPaymentInfo>();

            if (inputbuf.Length > 0)
            {
                try
                {
                    DRPaymentInfoResult = JsonSerializer.Deserialize<List<DRPaymentInfo>>(inputbuf);
                }
                catch
                {
                    DRPaymentInfoResult = null;
                }
            }
            else
            {
                DRPaymentInfoResult = null;
            }

            if ((m_blnlogfile) && (DRPaymentInfoResult == null))
            {
                String StrMsg = String.Format("DRPaymentInfo2Class:{0}", inputbuf);
                LogFile.Write("SystemError ; " + StrMsg);
            }

            return DRPaymentInfoResult;
        }
        //---DRPaymentInfo

        //---
        //DRExpenseInfo
        public static List<DRExpenseInfo> DRExpenseInfo2Class(String inputbuf)
        {
            List<DRExpenseInfo> DRExpenseInfoResult = new List<DRExpenseInfo>();

            if (inputbuf.Length > 0)
            {
                try
                {
                    DRExpenseInfoResult = JsonSerializer.Deserialize<List<DRExpenseInfo>>(inputbuf);
                }
                catch
                {
                    DRExpenseInfoResult = null;
                }
            }
            else
            {
                DRExpenseInfoResult = null;
            }

            if ((m_blnlogfile) && (DRExpenseInfoResult == null))
            {
                String StrMsg = String.Format("DRExpenseInfo2Class:{0}", inputbuf);
                LogFile.Write("SystemError ; " + StrMsg);
            }

            return DRExpenseInfoResult;
        }
        //---DRExpenseInfo

        //---
        //DRCouponInfo
        public static String DRCouponInfo2String(List<DRCouponInfo> inputbuf)
        {
            String StrResult = "";

            StrResult = JsonSerializer.Serialize(inputbuf);
            return StrResult;
        }

        public static List<DRCouponInfo> DRCouponInfo2Class(String inputbuf)
        {
            List<DRCouponInfo> DRCouponInfoResult = new List<DRCouponInfo>();

            if (inputbuf.Length > 0)
            {
                try
                {
                    DRCouponInfoResult = JsonSerializer.Deserialize<List<DRCouponInfo>>(inputbuf);
                }
                catch
                {
                    DRCouponInfoResult = null;
                }
            }
            else
            {
                DRCouponInfoResult = null;
            }

            if ((m_blnlogfile) && (DRCouponInfoResult == null))
            {
                String StrMsg = String.Format("DRCouponInfo2Class:{0}", inputbuf);
                LogFile.Write("SystemError ; " + StrMsg);
            }

            return DRCouponInfoResult;
        }
        //---DRCouponInfo

        //---
        //DRInvSummeryInfo
        public static String DRInvSummeryInfo2String(DRInvSummeryInfo inputbuf)
        {
            String StrResult = "";

            StrResult = JsonSerializer.Serialize(inputbuf);
            return StrResult;
        }

        public static DRInvSummeryInfo DRInvSummeryInfo2Class(String inputbuf)
        {
            DRInvSummeryInfo DRInvSummeryInfoResult = new DRInvSummeryInfo();

            if (inputbuf.Length > 0)
            {
                try
                {
                    DRInvSummeryInfoResult = JsonSerializer.Deserialize<DRInvSummeryInfo>(inputbuf);
                }
                catch
                {
                    DRInvSummeryInfoResult = null;
                }
            }
            else
            {
                DRInvSummeryInfoResult = null;
            }

            if ((m_blnlogfile) && (DRInvSummeryInfoResult == null))
            {
                String StrMsg = String.Format("DRInvSummeryInfo2Class:{0}", inputbuf);
                LogFile.Write("SystemError ; " + StrMsg);
            }

            return DRInvSummeryInfoResult;
        }
        //---DRInvSummeryInfo

        //---
        //expense_API
        public static String expense_new2String(expense_new inputbuf)
        {
            String StrResult = "";

            StrResult = JsonSerializer.Serialize(inputbuf);
            return StrResult;
        }
        public static String expense_cancel2String(expense_cancel inputbuf)
        {
            String StrResult = "";

            StrResult = JsonSerializer.Serialize(inputbuf);
            return StrResult;
        }
        //---expense_API

        //---
        //DRCategorySalesStatistics
        public static String DRCategorySalesStatistics2String(List<DRCategorySalesStatistics> inputbuf)
        {
            String StrResult = "";

            StrResult = JsonSerializer.Serialize(inputbuf);
            return StrResult;
        }
        public static List<DRCategorySalesStatistics> DRCategorySalesStatistics2Class(String inputbuf)
        {
            List<DRCategorySalesStatistics> DRCategorySalesStatisticsResult = new List<DRCategorySalesStatistics>();

            if (inputbuf.Length > 0)
            {
                try
                {
                    DRCategorySalesStatisticsResult = JsonSerializer.Deserialize<List<DRCategorySalesStatistics>>(inputbuf);
                }
                catch
                {
                    DRCategorySalesStatisticsResult = null;
                }
            }
            else
            {
                DRCategorySalesStatisticsResult = null;
            }

            if ((m_blnlogfile) && (DRCategorySalesStatisticsResult == null))
            {
                String StrMsg = String.Format("DRCategorySalesStatistics2Class:{0}", inputbuf);
                LogFile.Write("SystemError ; " + StrMsg);
            }

            return DRCategorySalesStatisticsResult;
        }
        //---DRCategorySalesStatistics

        //---
        //DRpromotions_Info
        public static String DRPromotionsInfo2String(List<DRPromotions_Info> inputbuf)
        {
            String StrResult = "";

            StrResult = JsonSerializer.Serialize(inputbuf);
            return StrResult;
        }
        public static List<DRPromotions_Info> DRPromotionsInfo2Class(String inputbuf)
        {
            List<DRPromotions_Info> DRpromotions_InfoResult = new List<DRPromotions_Info>();

            if (inputbuf.Length > 0)
            {
                try
                {
                    DRpromotions_InfoResult = JsonSerializer.Deserialize<List<DRPromotions_Info>>(inputbuf);
                }
                catch
                {
                    DRpromotions_InfoResult = null;
                }
            }
            else
            {
                DRpromotions_InfoResult = null;
            }

            if ((m_blnlogfile) && (DRpromotions_InfoResult == null))
            {
                String StrMsg = String.Format("DRpromotionsInfo2Class:{0}", inputbuf);
                LogFile.Write("SystemError ; " + StrMsg);
            }

            return DRpromotions_InfoResult;
        }
        //---DRpromotions_Info

        //---
        //LinePayModuleParams
        public static LinePayModuleParams LinePayModuleParams2Class(String inputbuf)
        {
            LinePayModuleParams LinePayModuleParamsResult = new LinePayModuleParams();

            if (inputbuf.Length > 0)
            {
                try
                {
                    LinePayModuleParamsResult = JsonSerializer.Deserialize<LinePayModuleParams>(inputbuf);
                }
                catch
                {
                    LinePayModuleParamsResult = null;
                }
            }
            else
            {
                LinePayModuleParamsResult = null;
            }

            if ((m_blnlogfile) && (LinePayModuleParamsResult == null))
            {
                String StrMsg = String.Format("LinePayModuleParams2Class:{0}", inputbuf);
                LogFile.Write("SystemError ; " + StrMsg);
            }

            return LinePayModuleParamsResult;
        }
        //---LinePayModuleParams

        //---
        //LinePayRequestIn
        public static String LinePayRequestIn2String(LinePayRequestIn inputbuf)
        {
            String StrResult = "";

            StrResult = JsonSerializer.Serialize(inputbuf);
            return StrResult;
        }
        //---LinePayRequestIn

        //---
        //LinePayRequestOut
        public static LinePayRequestOut LinePayRequestOut2Class(String inputbuf)
        {
            LinePayRequestOut LinePayRequestOutResult = new LinePayRequestOut();

            if (inputbuf.Length > 0)
            {
                try
                {
                    LinePayRequestOutResult = JsonSerializer.Deserialize<LinePayRequestOut>(inputbuf);
                }
                catch
                {
                    LinePayRequestOutResult = null;
                }
            }
            else
            {
                LinePayRequestOutResult = null;
            }

            if ((m_blnlogfile) && (LinePayRequestOutResult == null))
            {
                String StrMsg = String.Format("LinePayRequestOut2Class:{0}", inputbuf);
                LogFile.Write("SystemError ; " + StrMsg);
            }

            return LinePayRequestOutResult;
        }
        //---LinePayRequestOut

        //---
        //LinePayConfirmIn
        public static String LinePayConfirmIn2String(LinePayConfirmIn inputbuf)
        {
            String StrResult = "";

            StrResult = JsonSerializer.Serialize(inputbuf);
            return StrResult;
        }
        //---LinePayConfirmIn

        //---
        //LinePayConfirmOut
        public static LinePayConfirmOut LinePayConfirmOut2Class(String inputbuf)
        {
            LinePayConfirmOut LinePayConfirmOutResult = new LinePayConfirmOut();

            if (inputbuf.Length > 0)
            {
                try
                {
                    LinePayConfirmOutResult = JsonSerializer.Deserialize<LinePayConfirmOut>(inputbuf);
                }
                catch
                {
                    LinePayConfirmOutResult = null;
                }
            }
            else
            {
                LinePayConfirmOutResult = null;
            }

            if ((m_blnlogfile) && (LinePayConfirmOutResult == null))
            {
                String StrMsg = String.Format("LinePayConfirmOut2Class:{0}", inputbuf);
                LogFile.Write("SystemError ; " + StrMsg);
            }

            return LinePayConfirmOutResult;
        }
        //---LinePayConfirmOut

        //---
        //LinePayInfoOut
        public static LinePayInfoOut LinePayInfoOut2Class(String inputbuf)
        {
            LinePayInfoOut LinePayInfoOutResult = new LinePayInfoOut();

            if (inputbuf.Length > 0)
            {
                try
                {
                    LinePayInfoOutResult = JsonSerializer.Deserialize<LinePayInfoOut>(inputbuf);
                }
                catch
                {
                    LinePayInfoOutResult = null;
                }
            }
            else
            {
                LinePayInfoOutResult = null;
            }

            if ((m_blnlogfile) && (LinePayInfoOutResult == null))
            {
                String StrMsg = String.Format("LinePayInfoOut2Class:{0}", inputbuf);
                LogFile.Write("SystemError ; " + StrMsg);
            }

            return LinePayInfoOutResult;
        }
        
        public static String LinePayInfoOut2String(LinePayInfoOut inputbuf)
        {
            String StrResult = "";

            StrResult = JsonSerializer.Serialize(inputbuf);
            return StrResult;
        }
        //---LinePayInfoOut

        //---
        //LinePayRefundIn
        public static String LinePayRefundIn2String(LinePayRefundIn inputbuf)
        {
            String StrResult = "";

            StrResult = JsonSerializer.Serialize(inputbuf);
            return StrResult;
        }
        //---LinePayRefundIn

        //---
        //LinePayRefundOut
        public static String LinePayRefundOut2String(LinePayRefundOut inputbuf)
        {
            String StrResult = "";

            StrResult = JsonSerializer.Serialize(inputbuf);
            return StrResult;
        }

        public static LinePayRefundOut LinePayRefundOut2Class(String inputbuf)
        {
            LinePayRefundOut LinePayRefundOutResult = new LinePayRefundOut();

            if (inputbuf.Length > 0)
            {
                try
                {
                    LinePayRefundOutResult = JsonSerializer.Deserialize<LinePayRefundOut>(inputbuf);
                }
                catch
                {
                    LinePayRefundOutResult = null;
                }
            }
            else
            {
                LinePayRefundOutResult = null;
            }

            if ((m_blnlogfile) && (LinePayRefundOutResult == null))
            {
                String StrMsg = String.Format("LinePayRefundOut2Class:{0}", inputbuf);
                LogFile.Write("SystemError ; " + StrMsg);
            }

            return LinePayRefundOutResult;
        }
        //---LinePayRefundOut

        //---
        //VTSTORE_params
        public static VTSTORE_params VTSTORE_params2Class(String inputbuf)
        {
            VTSTORE_params VTSTORE_paramsResult = new VTSTORE_params();

            if (inputbuf.Length > 0)
            {
                try
                {
                    VTSTORE_paramsResult = JsonSerializer.Deserialize<VTSTORE_params>(inputbuf);
                }
                catch
                {
                    VTSTORE_paramsResult = null;
                }
            }
            else
            {
                VTSTORE_paramsResult = null;
            }

            if ((m_blnlogfile) && (VTSTORE_paramsResult == null))
            {
                String StrMsg = String.Format("VTSTORE_params2Class:{0}", inputbuf);
                LogFile.Write("SystemError ; " + StrMsg);
            }

            return VTSTORE_paramsResult;
        }
        //---VTSTORE_params

        //---
        //NIDIN_POS_params
        public static NIDIN_POS_params NIDIN_POS_params2Class(String inputbuf)
        {
            NIDIN_POS_params NIDIN_POS_paramsResult = new NIDIN_POS_params();

            if (inputbuf.Length > 0)
            {
                try
                {
                    NIDIN_POS_paramsResult = JsonSerializer.Deserialize<NIDIN_POS_params>(inputbuf);
                }
                catch
                {
                    NIDIN_POS_paramsResult = null;
                }
            }
            else
            {
                NIDIN_POS_paramsResult = null;
            }

            if ((m_blnlogfile) && (NIDIN_POS_paramsResult == null))
            {
                String StrMsg = String.Format("NIDIN_POS_params2Class:{0}", inputbuf);
                LogFile.Write("SystemError ; " + StrMsg);
            }

            return NIDIN_POS_paramsResult;
        }
        //---NIDIN_POS_params

        //---
        //UBER_EATS_params
        public static UBER_EATS_params UBER_EATS_params2Class(String inputbuf)
        {
            UBER_EATS_params UBER_EATS_paramsResult = new UBER_EATS_params();

            if (inputbuf.Length > 0)
            {
                try
                {
                    UBER_EATS_paramsResult = JsonSerializer.Deserialize<UBER_EATS_params>(inputbuf);
                }
                catch
                {
                    UBER_EATS_paramsResult = null;
                }
            }
            else
            {
                UBER_EATS_paramsResult = null;
            }

            if ((m_blnlogfile) && (UBER_EATS_paramsResult == null))
            {
                String StrMsg = String.Format("UBER_EATS_params2Class:{0}", inputbuf);
                LogFile.Write("SystemError ; " + StrMsg);
            }

            return UBER_EATS_paramsResult;
        }
        //---UBER_EATS_params

        //---
        //FOODPANDA_params
        public static FOODPANDA_params FOODPANDA_params2Class(String inputbuf)
        {
            FOODPANDA_params FOODPANDA_paramsResult = new FOODPANDA_params();

            if (inputbuf.Length > 0)
            {
                try
                {
                    FOODPANDA_paramsResult = JsonSerializer.Deserialize<FOODPANDA_params>(inputbuf);
                }
                catch
                {
                    FOODPANDA_paramsResult = null;
                }
            }
            else
            {
                FOODPANDA_paramsResult = null;
            }

            if ((m_blnlogfile) && (FOODPANDA_paramsResult == null))
            {
                String StrMsg = String.Format("FOODPANDA_params2Class:{0}", inputbuf);
                LogFile.Write("SystemError ; " + StrMsg);
            }

            return FOODPANDA_paramsResult;
        }
        //---FOODPANDA_params

        //---
        //YORES_POS_params
        public static YORES_POS_params YORES_POS_params2Class(String inputbuf)
        {
            YORES_POS_params YORES_POS_paramsResult = new YORES_POS_params();

            if (inputbuf.Length > 0)
            {
                try
                {
                    YORES_POS_paramsResult = JsonSerializer.Deserialize<YORES_POS_params>(inputbuf);
                }
                catch
                {
                    YORES_POS_paramsResult = null;
                }
            }
            else
            {
                YORES_POS_paramsResult = null;
            }

            if ((m_blnlogfile) && (YORES_POS_paramsResult == null))
            {
                String StrMsg = String.Format("YORES_POS_params2Class:{0}", inputbuf);
                LogFile.Write("SystemError ; " + StrMsg);
            }

            return YORES_POS_paramsResult;
        }
        //---YORES_POS_params

        //---
        //VTSTORE_ordersnew
        public static VTSTORE_ordersnew VTSTORE_ordersnew2Class(String inputbuf)
        {
            VTSTORE_ordersnew VTSTORE_ordersnewResult = new VTSTORE_ordersnew();

            if (inputbuf.Length > 0)
            {
                try
                {
                    VTSTORE_ordersnewResult = JsonSerializer.Deserialize<VTSTORE_ordersnew>(inputbuf);
                }
                catch
                {
                    VTSTORE_ordersnewResult = null;
                }
            }
            else
            {
                VTSTORE_ordersnewResult = null;
            }

            if ((m_blnlogfile) && (VTSTORE_ordersnewResult == null))
            {
                String StrMsg = String.Format("VTSTORE_ordersnew2Class:{0}", inputbuf);
                LogFile.Write("SystemError ; " + StrMsg);
            }

            return VTSTORE_ordersnewResult;
        }
        //---VTSTORE_ordersnew

        //---
        //VTSTORE_change_state
        public static VTSTORE_change_state VTSTORE_change_state2Class(String inputbuf)
        {
            VTSTORE_change_state VTSTORE_change_stateResult = new VTSTORE_change_state();

            if (inputbuf.Length > 0)
            {
                try
                {
                    VTSTORE_change_stateResult = JsonSerializer.Deserialize<VTSTORE_change_state>(inputbuf);
                }
                catch
                {
                    VTSTORE_change_stateResult = null;
                }
            }
            else
            {
                VTSTORE_change_stateResult = null;
            }

            if ((m_blnlogfile) && (VTSTORE_change_stateResult == null))
            {
                String StrMsg = String.Format("VTSTORE_change_state2Class:{0}", inputbuf);
                LogFile.Write("SystemError ; " + StrMsg);
            }

            return VTSTORE_change_stateResult;
        }
        //---VTSTORE_change_state

        //---
        //VTSTORE_change_input
        public static String VTSTORE_change_input2String(VTSTORE_change_input inputbuf)
        {
            String StrResult = "";

            StrResult = JsonSerializer.Serialize(inputbuf);
            return StrResult;
        }
        //---VTSTORE_change_input

        //---
        //VTSTORE_ordersinfo
        public static VTSTORE_ordersinfo VTSTORE_ordersinfo2Class(String inputbuf)
        {
            VTSTORE_ordersinfo VTSTORE_ordersinfoResult = new VTSTORE_ordersinfo();

            if (inputbuf.Length > 0)
            {
                try
                {
                    VTSTORE_ordersinfoResult = JsonSerializer.Deserialize<VTSTORE_ordersinfo>(inputbuf);
                }
                catch
                {
                    VTSTORE_ordersinfoResult = null;
                }
            }
            else
            {
                VTSTORE_ordersinfoResult = null;
            }

            if ((m_blnlogfile) && (VTSTORE_ordersinfoResult == null))
            {
                String StrMsg = String.Format("VTSTORE_ordersinfo2Class:{0}", inputbuf);
                LogFile.Write("SystemError ; " + StrMsg);
            }

            return VTSTORE_ordersinfoResult;
        }
        //---VTSTORE_ordersinfo
        //---
        //VTSON_Datum
        public static String VTSON_Datum2String(VTSON_Datum inputbuf)
        {
            String StrResult = "";

            StrResult = JsonSerializer.Serialize(inputbuf);
            return StrResult;
        }

        public static VTSON_Datum VTSON_Datum2Class(String inputbuf)
        {
            VTSON_Datum VTSON_DatumResult = new VTSON_Datum();

            if (inputbuf.Length > 0)
            {
                try
                {
                    VTSON_DatumResult = JsonSerializer.Deserialize<VTSON_Datum>(inputbuf);
                }
                catch
                {
                    VTSON_DatumResult = null;
                }
            }
            else
            {
                VTSON_DatumResult = null;
            }

            if ((m_blnlogfile) && (VTSON_DatumResult == null))
            {
                String StrMsg = String.Format("VTSON_Datum2Class:{0}", inputbuf);
                LogFile.Write("SystemError ; " + StrMsg);
            }

            return VTSON_DatumResult;
        }
        //---VTSON_Datum

        //---
        //Ubereats_ordersnew
        public static Ubereats_ordersnew Ubereats_ordersnew2Class(String inputbuf)
        {
            Ubereats_ordersnew Ubereats_ordersnewResult = new Ubereats_ordersnew();

            if (inputbuf.Length > 0)
            {
                try
                {
                    Ubereats_ordersnewResult = JsonSerializer.Deserialize<Ubereats_ordersnew>(inputbuf);
                }
                catch
                {
                    Ubereats_ordersnewResult = null;
                }
            }
            else
            {
                Ubereats_ordersnewResult = null;
            }

            if ((m_blnlogfile) && (Ubereats_ordersnewResult == null))
            {
                String StrMsg = String.Format("Ubereats_ordersnew2Class:{0}", inputbuf);
                LogFile.Write("SystemError ; " + StrMsg);
            }

            return Ubereats_ordersnewResult;
        }
        //---Ubereats_ordersnew

        //---
        //UBEON_Datum
        public static String UBEON_Datum2String(UBEON_Datum inputbuf)
        {
            String StrResult = "";

            StrResult = JsonSerializer.Serialize(inputbuf);
            return StrResult;
        }

        public static UBEON_Datum UBEON_Datum2Class(String inputbuf)
        {
            UBEON_Datum UBEON_DatumResult = new UBEON_Datum();

            if (inputbuf.Length > 0)
            {
                try
                {
                    UBEON_DatumResult = JsonSerializer.Deserialize<UBEON_Datum>(inputbuf);
                }
                catch
                {
                    UBEON_DatumResult = null;
                }
            }
            else
            {
                UBEON_DatumResult = null;
            }

            if ((m_blnlogfile) && (UBEON_DatumResult == null))
            {
                String StrMsg = String.Format("UBEON_Datum2Class:{0}", inputbuf);
                LogFile.Write("SystemError ; " + StrMsg);
            }

            return UBEON_DatumResult;
        }
        //---UBEON_Datum

        //---
        //Foodpanda_ordersnew
        public static Foodpanda_ordersnew Foodpanda_ordersnew2Class(String inputbuf)
        {
            Foodpanda_ordersnew Foodpanda_ordersnewResult = new Foodpanda_ordersnew();

            if (inputbuf.Length > 0)
            {
                try
                {
                    Foodpanda_ordersnewResult = JsonSerializer.Deserialize<Foodpanda_ordersnew>(inputbuf);
                }
                catch
                {
                    Foodpanda_ordersnewResult = null;
                }
            }
            else
            {
                Foodpanda_ordersnewResult = null;
            }

            if ((m_blnlogfile) && (Foodpanda_ordersnewResult == null))
            {
                String StrMsg = String.Format("Foodpanda_ordersnew2Class:{0}", inputbuf);
                LogFile.Write("SystemError ; " + StrMsg);
            }

            return Foodpanda_ordersnewResult;
        }
        //---Foodpanda_ordersnew

        //---
        //FPAON_Datum
        public static String FPAON_Datum2String(FPAON_Datum inputbuf)
        {
            String StrResult = "";

            StrResult = JsonSerializer.Serialize(inputbuf);
            return StrResult;
        }

        public static FPAON_Datum FPAON_Datum2Class(String inputbuf)
        {
            FPAON_Datum FPAON_DatumResult = new FPAON_Datum();

            if (inputbuf.Length > 0)
            {
                try
                {
                    FPAON_DatumResult = JsonSerializer.Deserialize<FPAON_Datum>(inputbuf);
                }
                catch
                {
                    FPAON_DatumResult = null;
                }
            }
            else
            {
                FPAON_DatumResult = null;
            }

            if ((m_blnlogfile) && (FPAON_DatumResult == null))
            {
                String StrMsg = String.Format("FPAON_Datum2Class:{0}", inputbuf);
                LogFile.Write("SystemError ; " + StrMsg);
            }

            return FPAON_DatumResult;
        }
        //---FPAON_Datum

        //---
        //EASY_CARDModule
        public static EASY_CARDModule EASY_CARDModule2Class(String inputbuf)
        {
            EASY_CARDModule EASY_CARDModuleResult = new EASY_CARDModule();

            if (inputbuf.Length > 0)
            {
                try
                {
                    EASY_CARDModuleResult = JsonSerializer.Deserialize<EASY_CARDModule>(inputbuf);
                }
                catch
                {
                    EASY_CARDModuleResult = null;
                }
            }
            else
            {
                EASY_CARDModuleResult = null;
            }

            if ((m_blnlogfile) && (EASY_CARDModuleResult == null))
            {
                String StrMsg = String.Format("EASY_CARDModule2Class:{0}", inputbuf);
                LogFile.Write("SystemError ; " + StrMsg);
            }

            return EASY_CARDModuleResult;
        }
        //---EASY_CARDModule

        //---
        //EASY_CARDModuleParams
        public static EASY_CARDModuleParams EASY_CARDModuleParams2Class(String inputbuf)
        {
            EASY_CARDModuleParams EASY_CARDModuleParamsResult = new EASY_CARDModuleParams();

            if (inputbuf.Length > 0)
            {
                try
                {
                    EASY_CARDModuleParamsResult = JsonSerializer.Deserialize<EASY_CARDModuleParams>(inputbuf);
                }
                catch
                {
                    EASY_CARDModuleParamsResult = null;
                }
            }
            else
            {
                EASY_CARDModuleParamsResult = null;
            }

            if ((m_blnlogfile) && (EASY_CARDModuleParamsResult == null))
            {
                String StrMsg = String.Format("EASY_CARDModuleParams2Class:{0}", inputbuf);
                LogFile.Write("SystemError ; " + StrMsg);
            }

            return EASY_CARDModuleParamsResult;
        }
        //---EASY_CARDModuleParams

        //---
        //EasyCardAPIMsg
        public static EasyCardAPIMsg EasyCardAPIMsg2Class(String inputbuf)
        {
            EasyCardAPIMsg EasyCardAPIMsgResult = new EasyCardAPIMsg();

            if (inputbuf.Length > 0)
            {
                try
                {
                    EasyCardAPIMsgResult = JsonSerializer.Deserialize<EasyCardAPIMsg>(inputbuf);
                }
                catch
                {
                    EasyCardAPIMsgResult = null;
                }
            }
            else
            {
                EasyCardAPIMsgResult = null;
            }

            if ((m_blnlogfile) && (EasyCardAPIMsgResult == null))
            {
                String StrMsg = String.Format("EasyCardAPIMsg2Class:{0}", inputbuf);
                LogFile.Write("SystemError ; " + StrMsg);
            }

            return EasyCardAPIMsgResult;
        }
        //---EasyCardAPIMsg

        //---
        //ECAM_CardInfo
        public static ECAM_CardInfo ECAM_CardInfo2Class(String inputbuf)
        {
            ECAM_CardInfo ECAM_CardInfoResult = new ECAM_CardInfo();

            if (inputbuf.Length > 0)
            {
                try
                {
                    ECAM_CardInfoResult = JsonSerializer.Deserialize<ECAM_CardInfo>(inputbuf);
                }
                catch
                {
                    ECAM_CardInfoResult = null;
                }
            }
            else
            {
                ECAM_CardInfoResult = null;
            }

            if ((m_blnlogfile) && (ECAM_CardInfoResult == null))
            {
                String StrMsg = String.Format("ECAM_CardInfo2Class:{0}", inputbuf);
                LogFile.Write("SystemError ; " + StrMsg);
            }

            return ECAM_CardInfoResult;
        }
        //---ECAM_CardInfo

        //---
        //EasyCardBlacklist
        public static EasyCardBlacklist EasyCardBlacklist2Class(String inputbuf)
        {
            EasyCardBlacklist EasyCardBlacklistResult = new EasyCardBlacklist();

            if (inputbuf.Length > 0)
            {
                try
                {
                    EasyCardBlacklistResult = JsonSerializer.Deserialize<EasyCardBlacklist>(inputbuf);
                }
                catch
                {
                    EasyCardBlacklistResult = null;
                }
            }
            else
            {
                EasyCardBlacklistResult = null;
            }

            if ((m_blnlogfile) && (EasyCardBlacklistResult == null))
            {
                String StrMsg = String.Format("EasyCardBlacklist2Class:{0}", inputbuf);
                LogFile.Write("SystemError ; " + StrMsg);
            }

            return EasyCardBlacklistResult;
        }
        //---EasyCardBlacklist

        //---
        //ECAMGetBasicInfoMsg
        public static ECAMGetBasicInfoMsg ECAMGetBasicInfoMsg2Class(String inputbuf)
        {
            ECAMGetBasicInfoMsg ECAMGetBasicInfoMsgResult = new ECAMGetBasicInfoMsg();

            if (inputbuf.Length > 0)
            {
                try
                {
                    ECAMGetBasicInfoMsgResult = JsonSerializer.Deserialize<ECAMGetBasicInfoMsg>(inputbuf);
                }
                catch
                {
                    ECAMGetBasicInfoMsgResult = null;
                }
            }
            else
            {
                ECAMGetBasicInfoMsgResult = null;
            }

            if ((m_blnlogfile) && (ECAMGetBasicInfoMsgResult == null))
            {
                String StrMsg = String.Format("ECAMGetBasicInfoMsg2Class:{0}", inputbuf);
                LogFile.Write("SystemError ; " + StrMsg);
            }

            return ECAMGetBasicInfoMsgResult;
        }
        //---ECAMGetBasicInfoMsg

        //---
        //ECAMCheckout
        public static ECAMCheckout ECAMCheckout2Class(String inputbuf)
        {
            ECAMCheckout ECAMCheckoutResult = new ECAMCheckout();

            if (inputbuf.Length > 0)
            {
                try
                {
                    ECAMCheckoutResult = JsonSerializer.Deserialize<ECAMCheckout>(inputbuf);
                }
                catch
                {
                    ECAMCheckoutResult = null;
                }
            }
            else
            {
                ECAMCheckoutResult = null;
            }

            if ((m_blnlogfile) && (ECAMCheckoutResult == null))
            {
                String StrMsg = String.Format("ECAMCheckout2Class:{0}", inputbuf);
                LogFile.Write("SystemError ; " + StrMsg);
            }

            return ECAMCheckoutResult;
        }
        //---ECAMCheckout

        //---
        //get_printer_template
        public static get_printer_template get_printer_template2Class(String inputbuf)
        {
            get_printer_template get_printer_templateResult = new get_printer_template();

            if (inputbuf.Length > 0)
            {
                try
                {
                    get_printer_templateResult = JsonSerializer.Deserialize<get_printer_template>(inputbuf);
                }
                catch
                {
                    get_printer_templateResult = null;
                }
            }
            else
            {
                get_printer_templateResult = null;
            }

            if ((m_blnlogfile) && (get_printer_templateResult == null))
            {
                String StrMsg = String.Format("get_printer_template2Class:{0}", inputbuf);
                LogFile.Write("SystemError ; " + StrMsg);
            }

            return get_printer_templateResult;
        }
        //---get_printer_template

        //---
        //CreditCardJosn
        public static String CreditCardJosn2String(CreditCardJosn inputbuf)
        {
            String StrResult = "";
            StrResult = JsonSerializer.Serialize(inputbuf);
            return StrResult;
        }
        public static CreditCardJosn CreditCardJosn2Class(String inputbuf)
        {
            CreditCardJosn CreditCardJosnResult = new CreditCardJosn();

            if (inputbuf.Length > 0)
            {
                try
                {
                    CreditCardJosnResult = JsonSerializer.Deserialize<CreditCardJosn>(inputbuf);
                }
                catch
                {
                    CreditCardJosnResult = null;
                }
            }
            else
            {
                CreditCardJosnResult = null;
            }

            if ((m_blnlogfile) && (CreditCardJosnResult == null))
            {
                String StrMsg = String.Format("CreditCardJosn2Class:{0}", inputbuf);
                LogFile.Write("SystemError ; " + StrMsg);
            }

            return CreditCardJosnResult;
        }
        //---CreditCardJosn

        //---
        //DailyReport_CheckoutInfo
        public static String DailyReportCheckoutInfo2String(DailyReport_CheckoutInfo inputbuf)
        {
            String StrResult = "";
            StrResult = JsonSerializer.Serialize(inputbuf);
            return StrResult;
        }
        //---DailyReport_CheckoutInfo

        //---
        //inv_params
        public static String inv_params2String(inv_params inputbuf) 
        {
            String StrResult = "";
            StrResult = JsonSerializer.Serialize(inputbuf);
            return StrResult;
        }
        //---inv_params

        //---
        //Inv_Use_Info
        public static Inv_Use_Info Inv_Use_Info2Class(String inputbuf)
        {
            Inv_Use_Info Inv_Use_InfoResult = new Inv_Use_Info();

            if (inputbuf.Length > 0)
            {
                try
                {
                    Inv_Use_InfoResult = JsonSerializer.Deserialize<Inv_Use_Info>(inputbuf);
                }
                catch
                {
                    Inv_Use_InfoResult = null;
                }
            }
            else
            {
                Inv_Use_InfoResult = null;
            }

            if ((m_blnlogfile) && (Inv_Use_InfoResult == null))
            {
                String StrMsg = String.Format("Inv_Use_Info2Class:{0}", inputbuf);
                LogFile.Write("SystemError ; " + StrMsg);
            }

            return Inv_Use_InfoResult;
        }
        //---Inv_Use_Info
        //---
        //inv_result
        public static inv_result inv_result2Class(String inputbuf)
        {
            inv_result inv_resultResult = new inv_result();

            if (inputbuf.Length > 0)
            {
                try
                {
                    inv_resultResult = JsonSerializer.Deserialize<inv_result>(inputbuf);
                }
                catch
                {
                    inv_resultResult = null;
                }
            }
            else
            {
                inv_resultResult = null;
            }

            if ((m_blnlogfile) && (inv_resultResult == null))
            {
                String StrMsg = String.Format("inv_result2Class:{0}", inputbuf);
                LogFile.Write("SystemError ; " + StrMsg);
            }

            return inv_resultResult;
        }
        //---inv_result

        //---
        //inv_url
        public static inv_url inv_url2Class(String inputbuf)
        {
            inv_url inv_urlResult = new inv_url();

            if (inputbuf.Length > 0)
            {
                try
                {
                    inv_urlResult = JsonSerializer.Deserialize<inv_url>(inputbuf);
                }
                catch
                {
                    inv_urlResult = null;
                }
            }
            else
            {
                inv_urlResult = null;
            }

            if ((m_blnlogfile) && (inv_urlResult == null))
            {
                String StrMsg = String.Format("inv_url2Class:{0}", inputbuf);
                LogFile.Write("SystemError ; " + StrMsg);
            }

            return inv_urlResult;
        }
        //---inv_url

        //---
        //POSOrder2InvoiceB2COrder
        public static POSOrder2InvoiceB2COrder POSOrder2InvoiceB2COrder2Class(String inputbuf)
        {
            POSOrder2InvoiceB2COrder POSOrder2InvoiceB2COrderResult = new POSOrder2InvoiceB2COrder();

            if (inputbuf.Length > 0)
            {
                try
                {
                    POSOrder2InvoiceB2COrderResult = JsonSerializer.Deserialize<POSOrder2InvoiceB2COrder>(inputbuf);
                }
                catch
                {
                    POSOrder2InvoiceB2COrderResult = null;
                }
            }
            else
            {
                POSOrder2InvoiceB2COrderResult = null;
            }

            if ((m_blnlogfile) && (POSOrder2InvoiceB2COrderResult == null))
            {
                String StrMsg = String.Format("POSOrder2InvoiceB2COrder2Class:{0}", inputbuf);
                LogFile.Write("SystemError ; " + StrMsg);
            }

            return POSOrder2InvoiceB2COrderResult;
        }
        //---POSOrder2InvoiceB2COrder
        //---
        //VPOS_Env
        public static VPOS_Env VPOS_Env2Class(String inputbuf)
        {
            VPOS_Env VPOS_EnvResult = new VPOS_Env();

            if (inputbuf.Length > 0)
            {
                try
                {
                    VPOS_EnvResult = JsonSerializer.Deserialize<VPOS_Env>(inputbuf);
                }
                catch
                {
                    VPOS_EnvResult = null;
                }
            }
            else
            {
                VPOS_EnvResult = null;
            }

            if ((m_blnlogfile) && (VPOS_EnvResult == null))
            {
                String StrMsg = String.Format("VPOS_Env2Class:{0}", inputbuf);
                LogFile.Write("SystemError ; " + StrMsg);
            }

            return VPOS_EnvResult;
        }
        //---VPOS_Env
        //---
        //inv_params
        public static String CustomerDisplay2String(CustomerDisplay inputbuf)
        {
            String StrResult = "";
            StrResult = JsonSerializer.Serialize(inputbuf);
            return StrResult;
        }
        //---inv_params
        //---
        //GTDDescript
        public static String GTDDescript2String(GTDDescript inputbuf)
        {
            String StrResult = "";
            StrResult = JsonSerializer.Serialize(inputbuf);
            return StrResult;
        }
        //---GTDDescript
        //---
        //Vaut2Class
        public static Vaut Vaut2Class(String inputbuf)
        {
            Vaut VautResult = new Vaut();

            if (inputbuf.Length > 0)
            {
                try
                {
                    VautResult = JsonSerializer.Deserialize<Vaut>(inputbuf);
                }
                catch
                {
                    VautResult = null;
                }
            }
            else
            {
                VautResult = null;
            }

            if ((m_blnlogfile) && (VautResult == null))
            {
                String StrMsg = String.Format("Vaut2Class:{0}", inputbuf);
                LogFile.Write("SystemError ; " + StrMsg);
            }

            return VautResult;
        }
        //---Vaut2Class

        //---
        //get_qrorder_params2Class
        public static get_qrorder_params get_qrorder_params2Class(String inputbuf)
        {
            get_qrorder_params get_qrorder_paramsResult = new get_qrorder_params();

            if (inputbuf.Length > 0)
            {
                try
                {
                    get_qrorder_paramsResult = JsonSerializer.Deserialize<get_qrorder_params>(inputbuf);
                }
                catch
                {
                    get_qrorder_paramsResult = null;
                }
            }
            else
            {
                get_qrorder_paramsResult = null;
            }

            if ((m_blnlogfile) && (get_qrorder_paramsResult == null))
            {
                String StrMsg = String.Format("get_qrorder_params2Class:{0}", inputbuf);
                LogFile.Write("SystemError ; " + StrMsg);
            }

            return get_qrorder_paramsResult;
        }
        //---get_qrorder_params2Class

        //---
        //get_print_queue_data2Class
        public static get_print_queue_data get_print_queue_data2Class(String inputbuf)
        {
            get_print_queue_data get_print_queue_dataResult = new get_print_queue_data();

            if (inputbuf.Length > 0)
            {
                try
                {
                    get_print_queue_dataResult = JsonSerializer.Deserialize<get_print_queue_data>(inputbuf);
                }
                catch
                {
                    get_print_queue_dataResult = null;
                }
            }
            else
            {
                get_print_queue_dataResult = null;
            }

            if ((m_blnlogfile) && (get_print_queue_dataResult == null))
            {
                String StrMsg = String.Format("get_print_queue_data2Class:{0}", inputbuf);
                LogFile.Write("SystemError ; " + StrMsg);
            }

            return get_print_queue_dataResult;
        }
        //---get_print_queue_data2Class

        //---
        //update_print_queue_data2String
        public static String update_print_queue_data2String(List<update_print_queue_data> inputbuf)
        {
            String StrResult = "";
            StrResult = JsonSerializer.Serialize(inputbuf);
            return StrResult;
        }
        //---update_print_queue_data2String

        //---
        //get_qrorder_order_list2Class
        public static get_qrorder_order_list get_qrorder_order_list2Class(String inputbuf)
        {
            get_qrorder_order_list get_qrorder_order_listResult = new get_qrorder_order_list();

            if (inputbuf.Length > 0)
            {
                try
                {
                    get_qrorder_order_listResult = JsonSerializer.Deserialize<get_qrorder_order_list>(inputbuf);
                }
                catch
                {
                    get_qrorder_order_listResult = null;
                }
            }
            else
            {
                get_qrorder_order_listResult = null;
            }

            if ((m_blnlogfile) && (get_qrorder_order_listResult == null))
            {
                String StrMsg = String.Format("get_qrorder_order_list2Class:{0}", inputbuf);
                LogFile.Write("SystemError ; " + StrMsg);
            }

            return get_qrorder_order_listResult;
        }
        //---get_qrorder_order_list2Class

        //---
        //get_qrorder_order_data2Class
        public static get_qrorder_order_data get_qrorder_order_data2Class(String inputbuf)
        {
            get_qrorder_order_data get_qrorder_order_dataResult = new get_qrorder_order_data();

            if (inputbuf.Length > 0)
            {
                try
                {
                    get_qrorder_order_dataResult = JsonSerializer.Deserialize<get_qrorder_order_data>(inputbuf);
                }
                catch
                {
                    get_qrorder_order_dataResult = null;
                }
            }
            else
            {
                get_qrorder_order_dataResult = null;
            }

            if ((m_blnlogfile) && (get_qrorder_order_dataResult == null))
            {
                String StrMsg = String.Format("get_qrorder_order_data2Class:{0}", inputbuf);
                LogFile.Write("SystemError ; " + StrMsg);
            }

            return get_qrorder_order_dataResult;
        }
        //---get_qrorder_order_data2Class
        //---
        //update_order_data2String
        public static String update_order_data2String(update_order_data inputbuf)
        {
            String StrResult = "";
            StrResult = JsonSerializer.Serialize(inputbuf);
            return StrResult;
        }
        //---update_order_data2String
    }//JsonClassConvert
}
