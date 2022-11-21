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
    public class MasterFieldController : Controller
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
        public JObject ListMasterField()
        {
            JObject jOut = new JObject();

            try
            {
                var retrunlist = ldl.listmasterfield();
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
        public JObject detailmasterfield([FromBody] JObject json)
        {
            JObject jOut = new JObject();

            try
            {
                var retrun = ldl.detailmasterfield(json);
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

        [HttpPost("insert")]
        public IActionResult insertmasterfield([FromBody] JObject json)
        {
            int code = 200;
            JObject jOut = new JObject();
            try
            {

                var res = bx.insertmasterfield(json);
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
        public IActionResult updatemasterfield([FromBody] JObject json)
        {
            int code = 200;
            JObject jOut = new JObject();
            try
            {

                var res = bx.updatemasterfield(json);
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
        public JObject deletemasterfield([FromBody] JObject json)
        {
            JObject jOut = new JObject();

            try
            {
                var retrun = ldl.deletemasterfield(json);
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

    }
}
