using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using pyfa.form.libs;
using System.Text;
using Npgsql;
using System.Data;
using System.Dynamic;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net.Http;
using System.Data.SqlClient;
using System.Globalization;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace pyfa.form.Controllers
{
    public class BaseController : Controller
    {
        private lDbConn dbconn = new lDbConn();
        public string execExtAPIGetWithToken(string api, string path, string credential)
        {
            var WebAPIURL = dbconn.domainGetApi2(api);
            string requestStr = WebAPIURL + path;

            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", credential);
            HttpResponseMessage response = client.GetAsync(requestStr).Result;
            string result = response.Content.ReadAsStringAsync().Result;
            return result;
        }

        public List<dynamic> getDataToObject(string spname, params string[] list)
        {
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("pyfahome");

            var conn = dbconn.constringList(provider, cstrname);
            StringBuilder sb = new StringBuilder();
            SqlConnection nconn = new SqlConnection(conn);
            var retObject = new List<dynamic>();

            try
            {
                nconn.Open();
                SqlCommand cmd = new SqlCommand(spname, nconn);
                cmd.CommandType = CommandType.StoredProcedure;

                if (list != null && list.Count() > 0)
                {
                    foreach (var item in list)
                    {
                        var pars = item.Split(',');

                        if (pars.Count() > 2)
                        {
                            if (pars[2] == "i")
                            {
                                cmd.Parameters.AddWithValue(pars[0], Convert.ToInt32(pars[1]));
                            }
                            else if (pars[2] == "s")
                            {
                                cmd.Parameters.AddWithValue(pars[0], Convert.ToString(pars[1]));
                            }
                            else if (pars[2] == "d")
                            {
                                cmd.Parameters.AddWithValue(pars[0], Convert.ToDecimal(pars[1]));
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue(pars[0], pars[1]);
                            }
                        }
                        else if (pars.Count() > 1)
                        {
                            cmd.Parameters.AddWithValue(pars[0], pars[1]);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue(pars[0], pars[0]);
                        }
                    }
                }

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr == null || dr.FieldCount == 0)
                {
                    return retObject;
                }

                retObject = GetDataObjSqlsvr(dr);

                nconn.Close();
                return retObject;
            }
            catch (Exception ex)
            {
                dynamic DyObj = new ExpandoObject() ;
                DyObj.success = false;
                DyObj.message = ex.Message;
                retObject.Add(DyObj);

                return retObject;
            }
        }
        public List<dynamic> getDynamicDataToObject(string spname,string parameter)
        {
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("pyfadashboard");

            var conn = dbconn.constringList(provider, cstrname);
            StringBuilder sb = new StringBuilder();
            SqlConnection nconn = new SqlConnection(conn);
            SqlCommand cmd = new SqlCommand(spname, nconn);
            cmd.CommandType = CommandType.StoredProcedure;
            var retObject = new List<dynamic>();

            try
            {
                nconn.Open();                
                var data = parameter.Split('|');
                if (data.Count() > 1)
                {
                    for (int i = 0; i < data.Count() - 1; i++)
                    {
                        var pars = data[i].Split(',');
                        if (pars[2] == "i")
                        {
                            cmd.Parameters.AddWithValue(pars[0], Convert.ToInt32(pars[1]));
                        }
                        else if (pars[2] == "s")
                        {
                            cmd.Parameters.AddWithValue(pars[0], Convert.ToString(pars[1]));
                        }
                        else if (pars[2] == "d")
                        {
                            cmd.Parameters.AddWithValue(pars[0], Convert.ToDecimal(pars[1]));
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue(pars[0], pars[1]);
                        }
                    }
                }

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr == null || dr.FieldCount == 0)
                {
                    return retObject;
                }

                retObject = GetDataObjSqlsvr(dr);
                nconn.Close();
                return retObject;
            }
            catch (Exception ex)
            {
                dynamic DyObj = new ExpandoObject() ;
                DyObj.success = false;
                DyObj.message = ex.Message;
                retObject.Add(DyObj);
                return retObject;
            }
            
        }

        public List<dynamic> getDynamicDataToObjectv2(string spname, string parameter)
        {
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("pyfadashboard");

            var conn = dbconn.constringList(provider, cstrname);
            StringBuilder sb = new StringBuilder();
            SqlConnection nconn = new SqlConnection(conn);
            SqlCommand cmd = new SqlCommand(spname, nconn);
            cmd.CommandType = CommandType.StoredProcedure;
            var retObject = new List<dynamic>();

            try
            {
                nconn.Open();
                var data = parameter.Split('^');
                if (data.Count() > 1)
                {
                    for (int i = 0; i < data.Count() - 1; i++)
                    {
                        var pars = data[i].Split(',');
                        if (pars[2] == "i")
                        {
                            cmd.Parameters.AddWithValue(pars[0], Convert.ToInt32(pars[1]));
                        }
                        else if (pars[2] == "s")
                        {
                            cmd.Parameters.AddWithValue(pars[0], Convert.ToString(pars[1]));
                        }
                        else if (pars[2] == "d")
                        {
                            cmd.Parameters.AddWithValue(pars[0], Convert.ToDecimal(pars[1]));
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue(pars[0], pars[1]);
                        }
                    }
                }

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr == null || dr.FieldCount == 0)
                {
                    return retObject;
                }

                retObject = GetDataObjSqlsvr(dr);
                nconn.Close();
                return retObject;
            }
            catch (Exception ex)
            {
                dynamic DyObj = new ExpandoObject();
                DyObj.success = false;
                DyObj.message = ex.Message;
                retObject.Add(DyObj);
                return retObject;
            }

        }

        
        protected List<dynamic> GetDataObj(SqlDataReader dr)
        {
            var retObject = new List<dynamic>();
            while (dr.Read())
            {
                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                for (int i = 0; i < dr.FieldCount; i++)
                {
                    dataRow.Add(
                           dr.GetName(i),
                           dr.IsDBNull(i) ? null : dr[i] // use null instead of {}
                   );
                }
                retObject.Add((ExpandoObject)dataRow);
            }

            return retObject;
        }

        public List<dynamic> ExecSqlWithReturnCustomSplit(string dbprv, string strname, string cstsplit, string spname, params string[] list)
        {
            var retObject = new List<dynamic>();
            StringBuilder sb = new StringBuilder();
            var conn = dbconn.constringList(dbprv, strname);

            SqlConnection nconn = new SqlConnection(conn);
            nconn.Open();
            SqlCommand cmd = new SqlCommand(spname, nconn);
            cmd.CommandType = CommandType.StoredProcedure;

            if (list != null && list.Count() > 0)
            {
                foreach (var item in list)
                {
                    var pars = item.Split(cstsplit);

                    if (pars.Count() > 2)
                    {
                        if (pars[2] == "i")
                        {
                            cmd.Parameters.AddWithValue(pars[0], Convert.ToInt32(pars[1]));
                        }
                        else if (pars[2] == "s")
                        {
                            cmd.Parameters.AddWithValue(pars[0], Convert.ToString(pars[1]));
                        }
                        else if (pars[2] == "d")
                        {
                            cmd.Parameters.AddWithValue(pars[0], Convert.ToDecimal(pars[1]));
                        }
                        else if (pars[2] == "dt")
                        {
                            cmd.Parameters.AddWithValue(pars[0], DateTime.ParseExact(pars[1], "yyyy-MM-dd", CultureInfo.InvariantCulture));
                        }
                        else if (pars[2] == "b")
                        {
                            cmd.Parameters.AddWithValue(pars[0], Convert.ToBoolean(pars[1]));
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue(pars[0], pars[1]);
                        }
                    }
                    else if (pars.Count() > 1)
                    {
                        cmd.Parameters.AddWithValue(pars[0], pars[1]);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue(pars[0], pars[0]);
                    }
                }
            }

            SqlDataReader dr = cmd.ExecuteReader();

            if (dr == null || dr.FieldCount == 0)
            {
                return retObject;
            }

            retObject = GetDataObjSqlsvr(dr);
            nconn.Close();


            return retObject;
        }

        public string execSqlWithExecptionModule(string dbprv, string strname, string cstsplit, string spname, params string[] list)
        {

            var conn = dbconn.constringList(dbprv, strname);
            string message = "";
            SqlConnection nconn = new SqlConnection(conn);
            nconn.Open();
            SqlCommand cmd = new SqlCommand(spname, nconn);
            cmd.CommandType = CommandType.StoredProcedure;
            if (list != null && list.Count() > 0)
            {
                foreach (var item in list)
                {
                    var pars = item.Split(cstsplit);

                    if (pars.Count() > 2)
                    {
                        if (pars[2] == "i")
                        {
                            cmd.Parameters.AddWithValue(pars[0], Convert.ToInt32(pars[1]));
                        }
                        else if (pars[2] == "bi")
                        {
                            cmd.Parameters.AddWithValue(pars[0], Convert.ToInt64(pars[1]));
                        }
                        else if (pars[2] == "s")
                        {
                            cmd.Parameters.AddWithValue(pars[0], Convert.ToString(pars[1]));
                        }
                        else if (pars[2] == "d")
                        {
                            cmd.Parameters.AddWithValue(pars[0], Convert.ToDecimal(pars[1]));
                        }
                        else if (pars[2] == "b")
                        {
                            cmd.Parameters.AddWithValue(pars[0], Convert.ToBoolean(pars[1]));
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue(pars[0], pars[1]);
                        }
                    }
                    else if (pars.Count() > 1)
                    {
                        cmd.Parameters.AddWithValue(pars[0], pars[1]);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue(pars[0], pars[0]);
                    }
                }
            }
            try
            {
                cmd.ExecuteNonQuery();
                message = "success";
            }
            catch (NpgsqlException e)
            {
                message = e.Message;
            }
            finally
            {
                if (nconn.State.Equals(ConnectionState.Open))
                {
                    nconn.Close();
                }
                SqlConnection.ClearPool(nconn);
            }
            return message;
        }

        public List<dynamic> execSqlWithReturnDataModule(string dbprv, string strname, string cstsplit, string spname, params string[] list)
        {
            var conn = dbconn.constringList(dbprv, strname);
            StringBuilder sb = new StringBuilder();
            SqlConnection nconn = new SqlConnection(conn);
            var retObject = new List<dynamic>();

            nconn.Open();
            SqlCommand cmd = new SqlCommand(spname, nconn);
            cmd.CommandType = CommandType.StoredProcedure;

            if (list != null && list.Count() > 0)
            {
                foreach (var item in list)
                {
                    var pars = item.Split(cstsplit);

                    if (pars.Count() > 2)
                    {
                        if (pars[2] == "bi")
                        {
                            cmd.Parameters.AddWithValue(pars[0], Convert.ToInt64(pars[1]));
                        }
                        else if (pars[2] == "i")
                        {
                            cmd.Parameters.AddWithValue(pars[0], Convert.ToInt32(pars[1]));
                        }
                        else if (pars[2] == "s")
                        {
                            cmd.Parameters.AddWithValue(pars[0], Convert.ToString(pars[1]));
                        }
                        else if (pars[2] == "d")
                        {
                            cmd.Parameters.AddWithValue(pars[0], Convert.ToDecimal(pars[1]));
                        }
                        else if (pars[2] == "dt")
                        {
                            cmd.Parameters.AddWithValue(pars[0], DateTime.ParseExact(pars[1], "yyyy-MM-dd", CultureInfo.InvariantCulture));
                        }
                        else if (pars[2] == "b")
                        {
                            cmd.Parameters.AddWithValue(pars[0], Convert.ToBoolean(pars[1]));
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue(pars[0], pars[1]);
                        }
                    }
                    else if (pars.Count() > 1)
                    {
                        cmd.Parameters.AddWithValue(pars[0], pars[1]);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue(pars[0], pars[0]);
                    }
                }
            }

            SqlDataReader dr = cmd.ExecuteReader();
            

            if (dr == null || dr.FieldCount == 0)
            {
                nconn.Close();
                SqlConnection.ClearPool(nconn);
                return retObject;
            }

            retObject = GetDataObj(dr);

            nconn.Close();
            SqlConnection.ClearPool(nconn);
            return retObject;
        }
        public List<dynamic> GetDataObjSqlsvr(SqlDataReader dr)
        {
            var retObject = new List<dynamic>();
            while (dr.Read())
            {
                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                for (int i = 0; i < dr.FieldCount; i++)
                {
                    dataRow.Add(
                           dr.GetName(i),
                           dr.IsDBNull(i) ? null : dr[i] // use null instead of {}
                   );
                }
                retObject.Add((ExpandoObject)dataRow);
            }

            return retObject;
        }
        public void execSqlWithSplitSemicolon(string spname, params string[] list)
        {
            var conn = dbconn.conStringLog();
            string message = "";
            NpgsqlConnection nconn = new NpgsqlConnection(conn);
            nconn.Open();
            NpgsqlCommand cmd = new NpgsqlCommand(spname, nconn);
            cmd.CommandType = CommandType.StoredProcedure;
            if (list != null && list.Count() > 0)
            {
                foreach (var item in list)
                {
                    var pars = item.Split(';');

                    if (pars.Count() > 2)
                    {
                        if (pars[2] == "i")
                        {
                            cmd.Parameters.AddWithValue(pars[0], Convert.ToInt32(pars[1]));
                        }
                        else if (pars[2] == "s")
                        {
                            cmd.Parameters.AddWithValue(pars[0], Convert.ToString(pars[1]));
                        }
                        else if (pars[2] == "d")
                        {
                            cmd.Parameters.AddWithValue(pars[0], Convert.ToDecimal(pars[1]));
                        }
                        else if (pars[2] == "b")
                        {
                            cmd.Parameters.AddWithValue(pars[0], Convert.ToBoolean(pars[1]));
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue(pars[0], pars[1]);
                        }
                    }
                    else if (pars.Count() > 1)
                    {
                        cmd.Parameters.AddWithValue(pars[0], pars[1]);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue(pars[0], pars[0]);
                    }
                }
            }
            try
            {
                cmd.ExecuteNonQuery();
                message = "success";
            }
            catch (NpgsqlException e)
            {
                message = e.Message;
            }
            finally
            {
                if (nconn.State.Equals(ConnectionState.Open))
                {
                    nconn.Close();
                }
                NpgsqlConnection.ClearPool(nconn);
            }
            //return message;
        }

        public List<dynamic> getDataToObjectCustomSplit(string split, string spname, params string[] list)
        {
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("idccore");

            //var conn = dbconn.conString();
            var conn = dbconn.constringList(provider, cstrname);

            StringBuilder sb = new StringBuilder();
            SqlConnection nconn = new SqlConnection(conn);
            var retObject = new List<dynamic>();

            nconn.Open();
            //NpgsqlTransaction tran = nconn.BeginTransaction();
            SqlCommand cmd = new SqlCommand(spname, nconn);
            cmd.CommandType = CommandType.StoredProcedure;

            if (list != null && list.Count() > 0)
            {
                foreach (var item in list)
                {
                    var pars = item.Split(split);

                    if (pars.Count() > 2)
                    {
                        if (pars[2] == "i")
                        {
                            cmd.Parameters.AddWithValue(pars[0], Convert.ToInt32(pars[1]));
                        }
                        else if (pars[2] == "s")
                        {
                            cmd.Parameters.AddWithValue(pars[0], Convert.ToString(pars[1]));
                        }
                        else if (pars[2] == "d")
                        {
                            cmd.Parameters.AddWithValue(pars[0], Convert.ToDecimal(pars[1]));
                        }
                        else if (pars[2] == "b")
                        {
                            cmd.Parameters.AddWithValue(pars[0], Convert.ToBoolean(pars[1]));
                        }
                        else if (pars[2] == "bg")
                        {
                            cmd.Parameters.AddWithValue(pars[0], Convert.ToInt64(pars[1]));
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue(pars[0], pars[1]);
                        }
                    }
                    else if (pars.Count() > 1)
                    {
                        cmd.Parameters.AddWithValue(pars[0], pars[1]);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue(pars[0], pars[0]);
                    }
                }
            }
            try
            {
             SqlDataReader dr = cmd.ExecuteReader();
                if (dr == null || dr.FieldCount == 0)
                {
                    nconn.Close();
                    return retObject;
                }
                retObject = GetDataObjSqlsvr(dr);
            }
            catch (Exception ex)
            {
                nconn.Close();
                retObject = new List<dynamic>();
                dynamic row = new ExpandoObject();
                row.status = "Invalid";
                row.message = "Invalid (" + ex.Message + ").";
                retObject.Add((ExpandoObject)row);
            }

            nconn.Close();
            return retObject;
        }

        public string CheckValueData(JObject json, string prop)
        {
            var strReturn = "";
            if (json.Property(prop) != null)
            {
                strReturn = json.GetValue(prop).ToString();
            }
            return strReturn;
        }
    }
}
