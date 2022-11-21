using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using CustomTokenAuthProvider;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading;
using System.Net.Http;
using pyfa.form.Controllers;
using pyfa.form.libs;

namespace pyfa.form.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("pyfaform/[controller]")]
    public class DynamicOptionController : Controller
    {
        private BaseController bc = new BaseController();
        private BaseTrxController bx = new BaseTrxController();
        private lConvert lc = new lConvert();
        private lMessage mc = new lMessage();
        private lServiceLogs lsl = new lServiceLogs();
        private lDbConn dbconn = new lDbConn();
        private lDataLayer ldl = new lDataLayer();
        private lPgsqlMapping lgsql = new lPgsqlMapping();
        private TokenController tc = new TokenController();

        #region header

        [HttpGet("header/list")]
        public JObject ListDynamicOptionhdr()
        {
            JObject jOut = new JObject();

            try
            {
                var retrunlist = ldl.listdynamicoption();
                jOut.Add("status", mc.GetMessage("api_output_ok"));
                jOut.Add("message", mc.GetMessage("process_success"));
                jOut.Add("data", retrunlist);

            }
            catch (Exception ex)
            {
                jOut = new JObject();
                jOut.Add("status", mc.GetMessage("api_output_not_ok"));
                jOut.Add("message", ex.Message);
            }

            return jOut;
        }

        [HttpPost("header/detail")]
        public JObject detailDynamicOptionhdr([FromBody] JObject json)
        {
            JObject jOut = new JObject();

            try
            {
                var retrun = ldl.detaildynamicoptionhdr(json);
                jOut.Add("status", mc.GetMessage("api_output_ok"));
                jOut.Add("message", mc.GetMessage("process_success"));
                jOut.Add("data", retrun);

            }
            catch (Exception ex)
            {
                jOut = new JObject();
                jOut.Add("status", mc.GetMessage("api_output_not_ok"));
                jOut.Add("message", ex.Message);
            }

            return jOut;
        }

        [HttpPost("header/add")]
        public IActionResult CreateDynamicOptionhdr([FromBody] JObject json)
        {
            int code = 200;
            JObject jOut = new JObject();
            var retObject1 = new List<dynamic>();
            try
            {
                List<dynamic> retObject = new List<dynamic>();
                var checkdata = ldl.checkdynamicoptionhdr(json);
                if (checkdata[0]["retrundata"].ToString() != "0")
                {
                    jOut.Add("status", mc.GetMessage("api_output_not_ok"));
                    jOut.Add("message", "Code " + json.GetValue("code").ToString() + " Already Exist.");
                }
                else
                {
                    var dbprv = dbconn.sqlprovider();
                    var cstrname = dbconn.constringName("pyfatrack");
                    var split = "||";

                    
                    string spname = "sp_insertdynamicoptionhdr";
                    string p1 = "@code" + split + bc.CheckValueData(json,"code") + split + "s";
                    string p2 = "@desc" + split + bc.CheckValueData(json, "desc") + split + "s";
                    retObject1 = bc.execSqlWithReturnDataModule(dbprv, cstrname, split, spname, p1,p2);
                    if (retObject1.Count > 0)
                    {
                        if (json.GetValue("id") != null)
                        {
                            json["id"] = retObject1[0].id;
                        }
                        else
                        {
                            json.Add("id", retObject1[0].id);
                        }
                        json["action"] = "insert";

                        insertLogdataHeader(json);
                        jOut = new JObject();
                        jOut.Add("status", mc.GetMessage("api_output_ok"));
                        jOut.Add("message", mc.GetMessage("save_success"));
                    }
                    else
                    {
                        jOut = new JObject();
                        jOut.Add("status", mc.GetMessage("api_output_not_ok"));
                        jOut.Add("message", "Create Failed");

                    }
                    
                }

            }
            catch (Exception ex)
            {
                jOut = new JObject();
                code = 500;
                jOut.Add("status", mc.GetMessage("api_output_not_ok"));
                jOut.Add("message", ex.Message);
            }

            return StatusCode(code, jOut);
        }

        [HttpPost("header/update")]
        public IActionResult UpdateDynamicOptionhdr([FromBody] JObject json)
        {
            int code = 200;
            JObject jOut = new JObject();
            var retObject1 = new List<dynamic>();
            try
            {
                List<dynamic> retObject = new List<dynamic>();
                json["action"] = "before update";
                insertLogdataHeader(json);

                var dbprv = dbconn.sqlprovider();
                var cstrname = dbconn.constringName("pyfatrack");
                var split = "||";
                string spname = "sp_updatedynamicoptionhdr";
                string p1 = "@code" + split + bc.CheckValueData(json, "code") + split + "s";
                string p2 = "@desc" + split + bc.CheckValueData(json, "desc") + split + "s";
                retObject1 = bc.execSqlWithReturnDataModule(dbprv, cstrname, split, spname, p1, p2);

                if (retObject1.Count > 0)
                {
                    if (json.GetValue("id") != null)
                    {
                        json["id"] = retObject1[0].id;
                    }
                    else
                    {
                        json.Add("id", retObject1[0].id);
                    }
                    json["action"] = "after update";
                    insertLogdataHeader(json);
                    jOut = new JObject();
                    jOut.Add("status", mc.GetMessage("api_output_ok"));
                    jOut.Add("message", mc.GetMessage("save_success"));
                }
                else
                {
                    jOut = new JObject();
                    jOut.Add("status", mc.GetMessage("api_output_not_ok"));
                    jOut.Add("message", "Update Failed");

                }


            }
            catch (Exception ex)
            {
                jOut = new JObject();
                code = 500;
                jOut.Add("status", mc.GetMessage("api_output_not_ok"));
                jOut.Add("message", ex.Message);
            }

            return StatusCode(code, jOut);
        }
        public string insertLogdataHeader(JObject json)
        {
            string strReturn = "";
            string id = bc.CheckValueData(json, "id") != "" ? bc.CheckValueData(json, "id") : "0";
        
            var jarData = ldl.getdynamicoptionhdrbyid(id);

            if (jarData.Count > 0)
            {
                // insert to log activity 
                var dbprv = dbconn.sqlprovider();
                var cstrname = dbconn.constringName("pyfatrack");
                var split = "||";

                string spname = "sp_insertdynamicoption_hdr_log";
                string p1 = "@code" + split + jarData[0]["ddh_code"].ToString() + split + "s";
                string p2 = "@desc" + split + jarData[0]["ddh_desc"].ToString() + split + "s";
                string p3 = "@isdelete" + split + jarData[0]["ddh_isdelete"].ToString() + split + "b";
                string p4 = "@action" + split + bc.CheckValueData(json, "action") + split + "s";
                string p5 = "@usr" + split + bc.CheckValueData(json, "user") + split + "s";
                string p6 = "@id" + split + id + split + "bi";
                strReturn = bc.execSqlWithExecptionModule(dbprv, cstrname, split, spname, p1,p2,p3,p4,p5,p6);
              
            }
            
            return strReturn;
        }

        [HttpPost("header/delete")]
        public JObject deletedynamicoptionhdr([FromBody] JObject json)
        {
            JObject jOut = new JObject();

            try
            {
                var retrun = ldl.deletedynamicoptionhdr(json);
                jOut.Add("status", mc.GetMessage("api_output_ok"));
                jOut.Add("message", mc.GetMessage("process_success"));

            }
            catch (Exception ex)
            {
                jOut = new JObject();
                jOut.Add("status", mc.GetMessage("api_output_not_ok"));
                jOut.Add("message", ex.Message);
            }

            return jOut;
        }
        #endregion

        #region ddl detail
        [HttpPost("detail")]
        public JObject detailDynamicOption([FromBody] JObject json)
        {
            JObject jOut = new JObject();

            try
            {
                var retrun = ldl.detaildynamicoption(json);
                jOut.Add("status", mc.GetMessage("api_output_ok"));
                jOut.Add("message", mc.GetMessage("process_success"));
                jOut.Add("data", retrun);

            }
            catch (Exception ex)
            {
                jOut = new JObject();
                jOut.Add("status", mc.GetMessage("api_output_not_ok"));
                jOut.Add("message", ex.Message);
            }

            return jOut;
        }

        [HttpPost("addParameterDdl")]
        public IActionResult CreateDynamicOption([FromBody] JObject json)
        {
            int code = 200;
            JObject jOut = new JObject();
            try
            {

                var res = bx.InsertDynamicOption(json);
                if (res == "success")
                {
                    code = 200;
                    jOut = new JObject();
                    jOut.Add("status", mc.GetMessage("api_output_ok"));
                    jOut.Add("message", mc.GetMessage("save_success"));
                }
                else
                {
                    jOut = new JObject();
                    jOut.Add("status", mc.GetMessage("api_output_not_ok"));
                    jOut.Add("message", "Create Failed");
                }
            }
            catch (Exception ex)
            {
                jOut = new JObject();
                code = 500;
                jOut.Add("status", mc.GetMessage("api_output_not_ok"));
                jOut.Add("message", ex.Message);
            }

            return StatusCode(code, jOut);
        }

        [HttpPost("udpateParameterDdl")]
        public IActionResult UpdateDynamicOption([FromBody] JObject json)
        {
            int code = 200;
            JObject jOut = new JObject();
            try
            {

                var res = bx.updateDynamicOption(json);
                if (res == "success")
                {
                    code = 200;
                    jOut = new JObject();
                    jOut.Add("status", mc.GetMessage("api_output_ok"));
                    jOut.Add("message", mc.GetMessage("save_success"));
                }
                else
                {
                    jOut = new JObject();
                    jOut.Add("status", mc.GetMessage("api_output_not_ok"));
                    jOut.Add("message", "Update Failed");
                }
            }
            catch (Exception ex)
            {
                jOut = new JObject();
                code = 500;
                jOut.Add("status", mc.GetMessage("api_output_not_ok"));
                jOut.Add("message", ex.Message);
            }

            return StatusCode(code, jOut);
        }

        [HttpPost("udpateisusedParameterDdl")]
        public IActionResult udpateisusedParameterDdl([FromBody] JObject json)
        {
            int code = 200;
            JObject jOut = new JObject();
            try
            {
                string isused = bc.CheckValueData(json, "isused") != "" ? bc.CheckValueData(json, "isused") : "0";
                var res = bx.udpateisusedParameterDdl(json, isused);
                if (res == "success")
                {
                    code = 200;
                    jOut = new JObject();
                    jOut.Add("status", mc.GetMessage("api_output_ok"));
                    jOut.Add("message", mc.GetMessage("save_success"));
                }
                else
                {
                    jOut = new JObject();
                    jOut.Add("status", mc.GetMessage("api_output_not_ok"));
                    jOut.Add("message", "Update Failed");
                }
            }
            catch (Exception ex)
            {
                jOut = new JObject();
                code = 500;
                jOut.Add("status", mc.GetMessage("api_output_not_ok"));
                jOut.Add("message", ex.Message);
            }

            return StatusCode(code, jOut);
        }

        #endregion

        #region ddl child

        [HttpPost("child/add")]
        public IActionResult insertDynamicOptionchild([FromBody] JObject json)
        {
            int code = 200;
            JObject jOut = new JObject();
            try
            {

                var res = bx.insertDynamicOptionchild(json);
                if (res == "success")
                {
                    code = 200;
                    jOut = new JObject();
                    jOut.Add("status", mc.GetMessage("api_output_ok"));
                    jOut.Add("message", mc.GetMessage("save_success"));
                }
                else
                {
                    jOut = new JObject();
                    jOut.Add("status", mc.GetMessage("api_output_not_ok"));
                    jOut.Add("message", "Update Failed");
                }
            }
            catch (Exception ex)
            {
                jOut = new JObject();
                code = 500;
                jOut.Add("status", mc.GetMessage("api_output_not_ok"));
                jOut.Add("message", ex.Message);
            }

            return StatusCode(code, jOut);
        }

        [HttpPost("child/update")]
        public IActionResult updateDynamicOptionchild([FromBody] JObject json)
        {
            int code = 200;
            JObject jOut = new JObject();
            try
            {

                var res = bx.UpdateDynamicOptionchild(json);
                if (res == "success")
                {
                    code = 200;
                    jOut = new JObject();
                    jOut.Add("status", mc.GetMessage("api_output_ok"));
                    jOut.Add("message", mc.GetMessage("save_success"));
                }
                else
                {
                    jOut = new JObject();
                    jOut.Add("status", mc.GetMessage("api_output_not_ok"));
                    jOut.Add("message", "Update Failed");
                }
            }
            catch (Exception ex)
            {
                jOut = new JObject();
                code = 500;
                jOut.Add("status", mc.GetMessage("api_output_not_ok"));
                jOut.Add("message", ex.Message);
            }

            return StatusCode(code, jOut);
        }

        [HttpPost("child/used")]
        public IActionResult updateDynamicOptionchildisused([FromBody] JObject json)
        {
            int code = 200;
            JObject jOut = new JObject();
            try
            {

                var res = bx.UpdateDynamicOptionchildisused(json);
                if (res == "success")
                {
                    code = 200;
                    jOut = new JObject();
                    jOut.Add("status", mc.GetMessage("api_output_ok"));
                    jOut.Add("message", mc.GetMessage("save_success"));
                }
                else
                {
                    jOut = new JObject();
                    jOut.Add("status", mc.GetMessage("api_output_not_ok"));
                    jOut.Add("message", "Update Failed");
                }
            }
            catch (Exception ex)
            {
                jOut = new JObject();
                code = 500;
                jOut.Add("status", mc.GetMessage("api_output_not_ok"));
                jOut.Add("message", ex.Message);
            }

            return StatusCode(code, jOut);
        }

        [HttpPost("child/detail")]
        public JObject detailDynamicOptionchild([FromBody] JObject json)
        {
            JObject jOut = new JObject();

            try
            {
                var retrun = ldl.detaildynamicoptionchild(json);
                jOut.Add("status", mc.GetMessage("api_output_ok"));
                jOut.Add("message", mc.GetMessage("process_success"));
                jOut.Add("data", retrun);

            }
            catch (Exception ex)
            {
                jOut = new JObject();
                jOut.Add("status", mc.GetMessage("api_output_not_ok"));
                jOut.Add("message", ex.Message);
            }

            return jOut;
        }

        [HttpPost("dynamic")]
        public JObject getdropdowndynamiclist([FromBody] JObject json)
        {
            JObject jOut = new JObject();

            try
            {
                var retrun = ldl.get_dropdown_dynamic_list(json);
                jOut.Add("status", mc.GetMessage("api_output_ok"));
                jOut.Add("message", mc.GetMessage("process_success"));
                jOut.Add("data", retrun);

            }
            catch (Exception ex)
            {
                jOut = new JObject();
                jOut.Add("status", mc.GetMessage("api_output_not_ok"));
                jOut.Add("message", ex.Message);
            }

            return jOut;
        }

        #endregion

    }
}
