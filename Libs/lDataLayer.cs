
using pyfa.form.libs;
using pyfa.form.Controllers;
using Newtonsoft.Json.Linq;
using Npgsql;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace pyfa.form.libs
{
    public class lDataLayer
    {
        private lDbConn dbconn = new lDbConn();
        private lConvert lc = new lConvert();
        private BaseController bc = new BaseController();

        #region dynamic option
        public JArray checkdynamicoptionhdr(JObject json)
        {
            var jaReturn = new JArray();
            var joReturn = new JObject();
            List<dynamic> retObject = new List<dynamic>();
            var dbprv = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("pyfatrack");
            var split = "||";

            string spname = "sp_checkdynamicoptionhdrbycode";
            string p1 = "@code" + split + json.GetValue("code").ToString() + split + "s";
            retObject = bc.ExecSqlWithReturnCustomSplit(dbprv, cstrname, split, spname, p1);
            jaReturn = lc.convertDynamicToJArray(retObject);

            return jaReturn;

        }

        public JArray getdynamicoptionhdrbyid(string id)
        {
            var jaReturn = new JArray();
            var joReturn = new JObject();
            List<dynamic> retObject = new List<dynamic>();
            var dbprv = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("pyfatrack");
            var split = "||";

            string spname = "sp_getdynamicoptionhdrbyid";
            string p1 = "@id" + split + Int32.Parse(id) + split + "i";
            retObject = bc.ExecSqlWithReturnCustomSplit(dbprv, cstrname, split, spname, p1);
            jaReturn = lc.convertDynamicToJArray(retObject);

            return jaReturn;

        }

        public JArray checkdynamicoption(JObject json)
        {
            var jaReturn = new JArray();
            var joReturn = new JObject();
            List<dynamic> retObject = new List<dynamic>();
            var dbprv = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("pyfatrack");
            var split = "||";

            string spname = "sp_checkdynamicoptionbycode";
            string p1 = "@code" + split + json.GetValue("code").ToString() + split + "s";
            retObject = bc.ExecSqlWithReturnCustomSplit(dbprv, cstrname, split, spname, p1);
            jaReturn = lc.convertDynamicToJArray(retObject);

            return jaReturn;

        }
        public JArray listdynamicoption()
        {
            var jaReturn = new JArray();
            var joReturn = new JObject();
            List<dynamic> retObject = new List<dynamic>();
            var dbprv = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("pyfatrack");
            var split = "||";

            string spname = "sp_listdynamicoption";
            retObject = bc.ExecSqlWithReturnCustomSplit(dbprv, cstrname, split, spname);
            jaReturn = lc.convertDynamicToJArray(retObject);

            return jaReturn;

        }

        public JObject detaildynamicoptionhdr(JObject json)
        {
            var jaReturn = new JArray();
            var joReturn = new JObject();
            List<dynamic> retObject = new List<dynamic>();
            var dbprv = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("pyfatrack");
            var split = "||";

            string spname = "sp_detaildynamicoptionhdrbycode";
            string p1 = "@code" + split + json.GetValue("code").ToString() + split + "s";
            retObject = bc.ExecSqlWithReturnCustomSplit(dbprv, cstrname, split, spname, p1);
            jaReturn = lc.convertDynamicToJArray(retObject);

            if (jaReturn.Count > 0)
            {
                joReturn = JObject.Parse(jaReturn[0].ToString());
            }

            return joReturn;

        }

        public JArray deletedynamicoptionhdr(JObject json)
        {
            var jaReturn = new JArray();
            var joReturn = new JObject();
            List<dynamic> retObject = new List<dynamic>();
            var dbprv = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("pyfatrack");
            var split = "||";

            string spname = "sp_deletedynamicoption";
            string p1 = "@id" + split + Int32.Parse(json.GetValue("id").ToString()) + split + "i";
            string p2 = "@usr" + split + json.GetValue("user").ToString() + split + "s";
            retObject = bc.ExecSqlWithReturnCustomSplit(dbprv, cstrname, split, spname, p1, p2);
            jaReturn = lc.convertDynamicToJArray(retObject);

            return jaReturn;

        }

        public JObject detaildynamicoption(JObject json)
        {
            var jaReturn = new JArray();
            var joReturn = new JObject();
            List<dynamic> retObject = new List<dynamic>();
            var dbprv = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("pyfatrack");
            var split = "||";

            string spname = "sp_detaildynamicoptionbycode";
            string p1 = "@code" + split + json.GetValue("code").ToString() + split + "s";
            retObject = bc.ExecSqlWithReturnCustomSplit(dbprv, cstrname, split, spname, p1);
            jaReturn = lc.convertDynamicToJArray(retObject);
            if (jaReturn.Count > 0)
            {
                joReturn = JObject.Parse(jaReturn[0].ToString());
            }

            return joReturn;

        }

        public JObject detaildynamicoptionchild(JObject json)
        {
            var jaReturn = new JArray();
            var joReturn = new JObject();
            List<dynamic> retObject = new List<dynamic>();
            var dbprv = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("pyfatrack");
            var split = "||";

            string spname = "get_customer_ddl_child_byheader";
            string p1 = "@ddp_id" + split + Int32.Parse(json.GetValue("ddp_id").ToString()) + split + "i";
            retObject = bc.ExecSqlWithReturnCustomSplit(dbprv, cstrname, split, spname, p1);
            jaReturn = lc.convertDynamicToJArray(retObject);
            if (jaReturn.Count > 0)
            {
                joReturn = JObject.Parse(jaReturn[0].ToString());
            }
            return joReturn;

        }

        public JArray get_dropdown_dynamic_list(JObject json)
        {
            var jaReturn = new JArray();
            var joReturn = new JObject();
            List<dynamic> retObject = new List<dynamic>();
            var dbprv = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("pyfatrack");
            var split = "||";

            string spname = "sp_get_dropdown_dynamic_list";
            string p1 = "@code" + split + json.GetValue("code").ToString() + split + "s";
            string p2 = "@parent_code" + split + json.GetValue("parent_code").ToString() + split + "s";
            retObject = bc.ExecSqlWithReturnCustomSplit(dbprv, cstrname, split, spname, p1, p2);
            jaReturn = lc.convertDynamicToJArray(retObject);

            return jaReturn;

        }

        #endregion

        #region master field
        public JArray listmasterfield()
        {
            var jaReturn = new JArray();
            var joReturn = new JObject();
            List<dynamic> retObject = new List<dynamic>();
            var dbprv = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("pyfatrack");
            var split = "||";

            string spname = "sp_listmasterfield";
            retObject = bc.ExecSqlWithReturnCustomSplit(dbprv, cstrname, split, spname);
            jaReturn = lc.convertDynamicToJArray(retObject);

            return jaReturn;

        }
        public JObject detailmasterfield(JObject json)
        {
        
            var joReturn = new JObject();
            var jaReturn = new JArray();
            List<dynamic> retObject = new List<dynamic>();
            var dbprv = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("pyfatrack");
            var split = "||";

            string spname = "sp_detailmasterfield";
            string p1 = "@id" + split + Int32.Parse(json.GetValue("id").ToString()) + split + "i";
            retObject = bc.ExecSqlWithReturnCustomSplit(dbprv, cstrname, split, spname, p1);
            jaReturn = lc.convertDynamicToJArray(retObject);
            if (jaReturn.Count > 0)
            {
                joReturn = JObject.Parse(jaReturn[0].ToString());
            }
            return joReturn;

        }

        public JArray deletemasterfield(JObject json)
        {
            var jaReturn = new JArray();
            var joReturn = new JObject();
            List<dynamic> retObject = new List<dynamic>();
            var dbprv = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("pyfatrack");
            var split = "||";

            string spname = "sp_deletemasterfield";
            string p1 = "@id" + split + Int32.Parse(json.GetValue("id").ToString()) + split + "i";
            string p2 = "@usr" + split + json.GetValue("user").ToString() + split + "s";
            retObject = bc.ExecSqlWithReturnCustomSplit(dbprv, cstrname, split, spname, p1, p2);
            jaReturn = lc.convertDynamicToJArray(retObject);

            return jaReturn;

        }

        #endregion

        #region master form
        public JArray ListMasterform()
        {
            var jaReturn = new JArray();
            var joReturn = new JObject();
            List<dynamic> retObject = new List<dynamic>();
            var dbprv = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("pyfatrack");
            var split = "||";

            string spname = "sp_listmasterform";
            retObject = bc.ExecSqlWithReturnCustomSplit(dbprv, cstrname, split, spname);
            jaReturn = lc.convertDynamicToJArray(retObject);
            return jaReturn;

        }

        public JArray detailmasterform(JObject json)
        {
            var jaReturn = new JArray();
            var joReturn = new JObject();
            List<dynamic> retObject = new List<dynamic>();
            var dbprv = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("pyfatrack");
            var split = "||";

            string spname = "sp_detailmasterform";
            string p1 = "@code" + split + json.GetValue("code").ToString() + split + "s";
            retObject = bc.ExecSqlWithReturnCustomSplit(dbprv, cstrname, split, spname, p1);
            jaReturn = lc.convertDynamicToJArray(retObject);

            return jaReturn;

        }


        public JArray deletemasterform(JObject json)
        {
            var jaReturn = new JArray();
            var joReturn = new JObject();
            List<dynamic> retObject = new List<dynamic>();
            var dbprv = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("pyfatrack");
            var split = "||";

            string spname = "sp_deletemasterformbyid";
            string p1 = "@id" + split + Int32.Parse(json.GetValue("id").ToString()) + split + "i";
            string p2 = "@usr" + split + json.GetValue("user").ToString() + split + "s";
            retObject = bc.ExecSqlWithReturnCustomSplit(dbprv, cstrname, split, spname, p1, p2);
            jaReturn = lc.convertDynamicToJArray(retObject);

            return jaReturn;

        }

        public JArray getfieldnamebyheadername(JObject json)
        {
            var jaReturn = new JArray();
            var joReturn = new JObject();
            List<dynamic> retObject = new List<dynamic>();
            var dbprv = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("pyfatrack");
            var split = "||";

            string spname = "sp_getfieldnamebyheadername";
            string p1 = "@header_name" + split + json.GetValue("header_name").ToString() + split + "s";
            retObject = bc.ExecSqlWithReturnCustomSplit(dbprv, cstrname, split, spname, p1);
            jaReturn = lc.convertDynamicToJArray(retObject);

            return jaReturn;

        }


        public JArray genetratetemplatecode()
        {
            var jaReturn = new JArray();
            var joReturn = new JObject();
            List<dynamic> retObject = new List<dynamic>();
            var dbprv = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("pyfatrack");
            var split = "||";

            string spname = "gen_last_code_template";
            retObject = bc.ExecSqlWithReturnCustomSplit(dbprv, cstrname, split, spname);
            jaReturn = lc.convertDynamicToJArray(retObject);
            return jaReturn;

        }
        #endregion
    }
}
