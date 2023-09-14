
namespace VPOS
{
    public class CondimentBom//配方BOM表
    {
        public string condiment_code { get; set; }//配料編號
        public string condiment_name { get; set; }//配料名
        public List<MaterialList> material_list { get; set; }//材料列表
        
        public CondimentBom()
        {
            material_list =new List<MaterialList>();
        }

        public void Clone(CondimentBom CondimentBomBuf)
        {
            condiment_code = CondimentBomBuf.condiment_code;
            condiment_name = CondimentBomBuf.condiment_name;

            material_list.Clear();
            for(int i = 0; i < CondimentBomBuf.material_list.Count; i++)
            {
                MaterialList MaterialListBuf = new MaterialList();
                MaterialListBuf.Clone(CondimentBomBuf.material_list[i]);
                material_list.Add(MaterialListBuf);
            }
        }
    }

    public class DeclineList//降階
    {
        public string decline_level { get; set; }//降階等級
        public string to_product_code { get; set; }//產品編碼
        public string to_product_name { get; set; }//產品名稱

        public DeclineList()
        {
            decline_level = "";
            to_product_code = "";
            to_product_name = "";
        }
        public void Clone(DeclineList DeclineListBuf)
        {
            decline_level = DeclineListBuf.decline_level;
            to_product_code = DeclineListBuf.to_product_code;
            to_product_name = DeclineListBuf.to_product_name;
        }
    }

    public class FormulaDatum//配方資料
    {
        public string product_code { get; set; }//產品號
        public string product_name { get; set; }//產品名
        public List<MaterialList> material_list { get; set; }//材料列表
        public List<DeclineList> decline_list { get; set; }//降階資料

        public FormulaDatum()
        {
            material_list = new List<MaterialList>();
            decline_list = new List<DeclineList>();
        }

        public void Clone(FormulaDatum FormulaDatumBuf)
        {
            product_code = FormulaDatumBuf.product_code;
            product_name = FormulaDatumBuf.product_name;

            material_list.Clear();
            for(int i = 0; i < FormulaDatumBuf.material_list.Count; i++)
            {
                MaterialList MaterialListBuf=new MaterialList();
                MaterialListBuf.Clone(FormulaDatumBuf.material_list[i]);
                material_list.Add(MaterialListBuf);
            }

            decline_list.Clear();
            for (int i = 0; i < FormulaDatumBuf.decline_list.Count; i++)
            {
                DeclineList DeclineListBuf=new DeclineList();
                DeclineListBuf.Clone(FormulaDatumBuf.decline_list[i]);
                decline_list.Add(DeclineListBuf); 
            }
        }
    }

    public class FormulaList
    {
        public string condiment_code { get; set; }//配料編號
        public string condiment_name { get; set; }//配料名
        public string operator_key { get; set; }//運算元+-*/=
        public string operator_value { get; set; }//運算值
        public object to_change_code { get; set; }
        public string unit_name { get; set; }//單位:cc/杯...
        public string decline_level { get; set; }//降階旗標
    }

    public class MaterialList//材料
    {
        public string material_code { get; set; }//材料編號
        public string material_name { get; set; }//材料名
        public string material_value { get; set; }//數量
        public string material_unit { get; set; }//單位
        public string print_bill { get; set; }//帳單列印N
        public string is_display { get; set; }//列印配方
        public List<FormulaList> formula_list { get; set; }

        public MaterialList()
        {
            formula_list = new List<FormulaList>();
        }

        public void Clone(MaterialList MaterialListBuf)
        {
            material_code = MaterialListBuf.material_code;//材料編號
            material_name = MaterialListBuf.material_name;//材料名
            material_value = MaterialListBuf.material_value;//數量
            material_unit = MaterialListBuf.material_unit;//單位
            print_bill = MaterialListBuf.print_bill;//帳單列印N
            is_display = MaterialListBuf.is_display;//列印配方

            formula_list.Clear();
            for(int i=0;i< MaterialListBuf.formula_list.Count;i++)
            {
                FormulaList FormulaListB = MaterialListBuf.formula_list[i];
                formula_list.Add(FormulaListB);
            }
        }
    }

    public class Formula_Data//配方/智能食譜[DB(json)]
    {
        public List<FormulaDatum> formula_data { get; set; }
        public List<CondimentBom> condiment_bom { get; set; }

        public Formula_Data()
        {
            formula_data = new List<FormulaDatum>();
            condiment_bom = new List<CondimentBom>();
        }

        public void Clone(Formula_Data Formula_DataBuf)
        {
            formula_data.Clear();
            for (int i=0;i< Formula_DataBuf.formula_data.Count;i++)
            {
                FormulaDatum FormulaDatumBuf = new FormulaDatum();
                FormulaDatumBuf.Clone(Formula_DataBuf.formula_data[i]);
                formula_data.Add(FormulaDatumBuf);
            }

            condiment_bom.Clear();
            for (int i = 0; i < Formula_DataBuf.condiment_bom.Count; i++)
            {
                CondimentBom CondimentBomBuf = new CondimentBom();
                CondimentBomBuf.Clone(Formula_DataBuf.condiment_bom[i]);
                condiment_bom.Add(CondimentBomBuf);
            }
        }
    }
}
