using pyfa.form.Controllers;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using pyfa.form.libs;
using System.Data.SqlClient;
using System.Data;
using Microsoft.VisualBasic;

namespace pyfa.form.libs
{
 
    public class lPgsqlMapping
    {
        private BaseController bc = new BaseController();
        private TokenController tc = new TokenController();
        private lDataLayer ld = new lDataLayer();
        private lConvert lc = new lConvert();
        private lPgsql pgsql = new lPgsql();
        private lDbConn dbconn = new lDbConn();

       
        internal List<dynamic> executeStringQuery(string qryStr)
        {
            var jaReturn = new JArray();
            List<dynamic> retObject = new List<dynamic>();
            retObject = pgsql.execStringQuerywithresult(qryStr);
            return retObject;
        }
        

    }
}
