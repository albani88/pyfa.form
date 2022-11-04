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
    public class MasterFormController : Controller
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


        [HttpGet("list")]
        public JObject ListMasterform()
        {
            JObject jOut = new JObject();

            try
            {
                var retrunlist = ldl.ListMasterform();
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


        [HttpPost("detail")]
        public JObject detailmasterform([FromBody] JObject json)
        {
            JObject jOut = new JObject();
            JArray jdata = new JArray();
            try
            {
                var jaData = ldl.detailmasterform(json);
                jOut.Add("status", mc.GetMessage("api_output_ok"));
                jOut.Add("message", mc.GetMessage("process_success"));
                jOut.Add("tamp_code", jaData[0]["mfh_tamp_code"].ToString());
                jOut.Add("tamp_name", jaData[0]["mfh_tamp_name"].ToString());
                jOut.Add("content_type", jaData[0]["mfh_content_type"].ToString());
                for (int i = 0; i < jaData.Count; i++)
                {

                    var jsonData = new JObject();
                    jsonData.Add("field_id", jaData[i]["mfd_field_id"].ToString());
                    jsonData.Add("counter", jaData[i]["mfd_counter"].ToString());
                    jsonData.Add("isused", jaData[i]["mfd_isused"].ToString());
                    jdata.Add(jsonData);

                }
               
                jOut.Add("data", jdata);

            }
            catch (Exception ex)
            {
                jOut = new JObject();
                jOut.Add("status", mc.GetMessage("api_output_not_ok"));
                jOut.Add("message", ex.Message);
            }

            return jOut;
        }


        [HttpPost("insert")]
        public IActionResult CreateMasterForm([FromBody] JObject json)
        {
            int code = 200;
            JObject jOut = new JObject();

            try
            {
                List<dynamic> retObject = new List<dynamic>();
                var jaData = ldl.genetratetemplatecode();
                string tmpcode = jaData[0]["tmp_code"].ToString();
                if (tmpcode != "")
                {
                    json["temp_code"] = tmpcode;
                }
                else
                {
                    json["temp_code"] = "";
                }

                var res = bx.InsertMasterForm(json, "insert");
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


        [HttpPost("update")]
        public IActionResult updateFormSubtrack([FromBody] JObject json)
        {
            int code = 200;
            JObject jOut = new JObject();

            try
            {
                List<dynamic> retObject = new List<dynamic>();

                var res = bx.InsertMasterForm(json, "update");
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


        [HttpPost("delete")]
        public JObject deletemasterform([FromBody] JObject json)
        {
            JObject jOut = new JObject();

            try
            {
                var retrun = ldl.deletemasterform(json);
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

        [HttpPost("getfield")]
        public JObject getfieldnamebyheader([FromBody] JObject json)
        {
            JObject jOut = new JObject();

            try
            {
                var retrun = ldl.getfieldnamebyheadername(json);
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
    }
}
