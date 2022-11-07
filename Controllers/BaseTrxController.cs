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

namespace pyfa.form.Controllers
{
    public class BaseTrxController : Controller
    {
        private lDbConn dbconn = new lDbConn();
        private BaseController bc = new BaseController();
        public string InsertMasterForm(JObject json)
        {
            string strout = "";
            var retObject = new List<dynamic>();
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("pyfatrack");

            var conn = dbconn.constringList(provider, cstrname);
            SqlTransaction trans;
            SqlConnection connection = new SqlConnection(conn);
            connection.Open();
            trans = connection.BeginTransaction();

            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("insert_master_form_hdr", connection, trans);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@code", Convert.ToString(json.GetValue("temp_code").ToString()));
                cmd.Parameters.AddWithValue("@header_name", json.GetValue("header_name").ToString());
                cmd.Parameters.AddWithValue("@ttl_content", json.GetValue("ttl_contnet").ToString());
                cmd.Parameters.AddWithValue("@usr", json.GetValue("user").ToString());
                SqlDataReader dr = cmd.ExecuteReader();
                cmd.Parameters.Clear();
                retObject = bc.GetDataObjSqlsvr(dr);
                dr.Close();
                string mfh_id = Convert.ToString(retObject[0].mfh_id_header);

                JArray jaData = JArray.Parse(json["detail"].ToString());
                for (int i = 0; i < jaData.Count; i++)
                {
                    //insert form detail
                    var data = new JObject();
                    data = JObject.Parse(jaData[i].ToString());
                    cmd = new SqlCommand("insert_master_form_detail", connection, trans);
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@id", Int32.Parse(mfh_id));
                    cmd.Parameters.AddWithValue("@code", json.GetValue("temp_code").ToString());
                    cmd.Parameters.AddWithValue("@contentname", data["contentname"].ToString());
                    cmd.Parameters.AddWithValue("@contenttype",data["contenttype"].ToString());
                    cmd.Parameters.AddWithValue("@counter", data["counter"].ToString());
                    cmd.Parameters.AddWithValue("@usr", json.GetValue("user").ToString());
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
                trans.Commit();
                strout = "success";
            }
            catch (Exception ex)
            {
                trans.Rollback();
                strout = ex.Message;
            }
            connection.Close();
            SqlConnection.ClearPool(connection);
            return strout;
        }

        public string updateMasterForm(JObject json)
        {
            string strout = "";
            var retObject = new List<dynamic>();
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("pyfatrack");

            var conn = dbconn.constringList(provider, cstrname);
            SqlTransaction trans;
            SqlConnection connection = new SqlConnection(conn);
            connection.Open();
            trans = connection.BeginTransaction();

            try
            {
                SqlCommand cmd = new SqlCommand();
                JArray jaData = JArray.Parse(json["detail"].ToString());
                for (int i = 0; i < jaData.Count; i++)
                {
                    //insert form detail
                    var data = new JObject();
                    data = JObject.Parse(jaData[i].ToString());
                    cmd = new SqlCommand("update_master_form_detail", connection, trans);
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@id", Int32.Parse(data["msf_id"].ToString()));
                    cmd.Parameters.AddWithValue("@code", json.GetValue("temp_code").ToString());
                    cmd.Parameters.AddWithValue("@contentname", data["contentname"].ToString());
                    cmd.Parameters.AddWithValue("@contenttype", data["contenttype"].ToString());
                    cmd.Parameters.AddWithValue("@counter", data["counter"].ToString());
                    cmd.Parameters.AddWithValue("@usr", json.GetValue("user").ToString());
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
                trans.Commit();
                strout = "success";
            }
            catch (Exception ex)
            {
                trans.Rollback();
                strout = ex.Message;
            }
            connection.Close();
            SqlConnection.ClearPool(connection);
            return strout;
        }

        public string InsertMasterFormField(JObject json, string actiontype)
        {
            string strout = "";
            var retObject = new List<dynamic>();
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("pyfatrack");

            var conn = dbconn.constringList(provider, cstrname);
            SqlTransaction trans;
            SqlConnection connection = new SqlConnection(conn);
            connection.Open();
            trans = connection.BeginTransaction();

            try
            {
                SqlCommand cmd = new SqlCommand();
                if (actiontype == "update")
                {
                    cmd = new SqlCommand("sp_deletemasterformdetailfieldbymfhid", connection, trans);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", json.GetValue("msf_id").ToString());
                    cmd.ExecuteNonQuery();
                }

                JArray jaData = JArray.Parse(json["detail"].ToString());
                for (int i = 0; i < jaData.Count; i++)
                {
                    //insert form detail
                    var data = new JObject();
                    data = JObject.Parse(jaData[i].ToString());
                    cmd = new SqlCommand("insert_master_form_detail_field", connection, trans);
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@id", Int32.Parse(json.GetValue("msf_id").ToString()));
                    cmd.Parameters.AddWithValue("@code", json.GetValue("temp_code").ToString());
                    cmd.Parameters.AddWithValue("@fieldid", data["fieldid"].ToString());
                    cmd.Parameters.AddWithValue("@counter", data["counter"].ToString());
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
                trans.Commit();
                strout = "success";
            }
            catch (Exception ex)
            {
                trans.Rollback();
                strout = ex.Message;
            }
            connection.Close();
            SqlConnection.ClearPool(connection);
            return strout;
        }

        public string UpdateFormSubtrack(JObject json)
        {
            string strout = "";
            var retObject = new List<dynamic>();
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("pyfatrack");

            var conn = dbconn.constringList(provider, cstrname);
            SqlTransaction trans;
            SqlConnection connection = new SqlConnection(conn);
            connection.Open();
            trans = connection.BeginTransaction();

            try
            {
                JArray jaData = JArray.Parse(json["object"].ToString());


                for (int i = 0; i < jaData.Count; i++)
                {
                    //insert form detail
                    var data = new JObject();
                    data = JObject.Parse(jaData[i].ToString());
                    SqlCommand cmd = new SqlCommand("insert_form_subtrack", connection, trans);
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@code", data["code"].ToString());
                    cmd.Parameters.AddWithValue("@header_name", data["header_name"].ToString());
                    cmd.Parameters.AddWithValue("@content_type", data["content_type"].ToString());
                    cmd.Parameters.AddWithValue("@field_id", Int32.Parse(data["field_id"].ToString()));
                    cmd.Parameters.AddWithValue("@order", Int32.Parse(data["order"].ToString()));
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
                trans.Commit();
                strout = "success";
            }
            catch (Exception ex)
            {
                trans.Rollback();
                strout = ex.Message;
            }
            connection.Close();
            SqlConnection.ClearPool(connection);
            return strout;
        }

        #region dynamic option
       
        public string InsertDynamicOption(JObject json)
        {
            string strout = "";
            var retObject = new List<dynamic>();
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("pyfatrack");
            JObject jo = new JObject();
            var conn = dbconn.constringList(provider, cstrname);
            SqlTransaction trans;
            SqlConnection connection = new SqlConnection(conn);
            connection.Open();
            trans = connection.BeginTransaction();

            try
            {
                SqlCommand cmd = new SqlCommand();
                var data = new JObject();
                cmd = new SqlCommand("sp_insertdynamicoption", connection, trans);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@code", json.GetValue("code").ToString());
                cmd.Parameters.AddWithValue("@value", json.GetValue("value").ToString());
                cmd.Parameters.AddWithValue("@name", json.GetValue("name").ToString());
                cmd.Parameters.AddWithValue("@counter", Int32.Parse(json.GetValue("counter").ToString()));
                cmd.Parameters.AddWithValue("@usr", json.GetValue("user").ToString());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                trans.Commit();
                strout = "success";
            }
            catch (Exception ex)
            {
                trans.Rollback();
                strout = ex.Message;
            }
            connection.Close();
            SqlConnection.ClearPool(connection);
            return strout;
        }

        public string updateDynamicOption(JObject json)
        {
            string strout = "";
            var retObject = new List<dynamic>();
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("pyfatrack");
            JObject jo = new JObject();
            var conn = dbconn.constringList(provider, cstrname);
            SqlTransaction trans;
            SqlConnection connection = new SqlConnection(conn);
            connection.Open();
            trans = connection.BeginTransaction();

            try
            {
                SqlCommand cmd = new SqlCommand();
                var data = new JObject();
                cmd = new SqlCommand("sp_updatedynamicoption", connection, trans);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@code", json.GetValue("code").ToString());
                cmd.Parameters.AddWithValue("@value", json.GetValue("value").ToString());
                cmd.Parameters.AddWithValue("@name", json.GetValue("name").ToString());
                cmd.Parameters.AddWithValue("@counter", Int32.Parse(json.GetValue("counter").ToString()));
                cmd.Parameters.AddWithValue("@usr", json.GetValue("user").ToString());
                cmd.Parameters.AddWithValue("@id", Int32.Parse(json.GetValue("id").ToString()));
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                trans.Commit();
                strout = "success";
            }
            catch (Exception ex)
            {
                trans.Rollback();
                strout = ex.Message;
            }
            connection.Close();
            SqlConnection.ClearPool(connection);
            return strout;
        }

        public string udpateisusedParameterDdl(JObject json, string isused)
        {
            string strout = "";
            var retObject = new List<dynamic>();
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("pyfatrack");
            JObject jo = new JObject();
            var conn = dbconn.constringList(provider, cstrname);
            SqlTransaction trans;
            SqlConnection connection = new SqlConnection(conn);
            connection.Open();
            trans = connection.BeginTransaction();

            try
            {
                SqlCommand cmd = new SqlCommand();
                var data = new JObject();
                cmd = new SqlCommand("sp_insertisuseddynamicoption", connection, trans);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@id", Int32.Parse(json.GetValue("id").ToString()));
                cmd.Parameters.AddWithValue("@isused", isused);
                cmd.Parameters.AddWithValue("@usr", json.GetValue("user").ToString());
              
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                trans.Commit();
                strout = "success";
            }
            catch (Exception ex)
            {
                trans.Rollback();
                strout = ex.Message;
            }
            connection.Close();
            SqlConnection.ClearPool(connection);
            return strout;
        }

        public string insertDynamicOptionchild(JObject json)
        {
            string strout = "";
            var retObject = new List<dynamic>();
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("pyfatrack");
            JObject jo = new JObject();
            var conn = dbconn.constringList(provider, cstrname);
            SqlTransaction trans;
            SqlConnection connection = new SqlConnection(conn);
            connection.Open();
            trans = connection.BeginTransaction();

            try
            {
                string parent = bc.CheckValueData(json, "parent") != "" ? bc.CheckValueData(json, "parent") : "0";
              
                SqlCommand cmd = new SqlCommand();
                var data = new JObject();
                cmd = new SqlCommand("sp_insertdynamicoption_child", connection, trans);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@key", json.GetValue("key").ToString());
                cmd.Parameters.AddWithValue("@value", json.GetValue("value").ToString());
                cmd.Parameters.AddWithValue("@name", json.GetValue("name").ToString());
                cmd.Parameters.AddWithValue("@counter", Int32.Parse(json.GetValue("counter").ToString()));
                cmd.Parameters.AddWithValue("@parent", parent);
                cmd.Parameters.AddWithValue("@ddp_id", Int32.Parse(json.GetValue("ddp_id").ToString()));
                cmd.Parameters.AddWithValue("@usr", json.GetValue("user").ToString());
               
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                trans.Commit();
                strout = "success";
            }
            catch (Exception ex)
            {
                trans.Rollback();
                strout = ex.Message;
            }
            connection.Close();
            SqlConnection.ClearPool(connection);
            return strout;
        }

        public string UpdateDynamicOptionchild(JObject json)
        {
            string strout = "";
            var retObject = new List<dynamic>();
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("pyfatrack");
            JObject jo = new JObject();
            var conn = dbconn.constringList(provider, cstrname);
            SqlTransaction trans;
            SqlConnection connection = new SqlConnection(conn);
            connection.Open();
            trans = connection.BeginTransaction();

            try
            {
                string parent = bc.CheckValueData(json, "parent") != "" ? bc.CheckValueData(json, "parent") : "0";

                SqlCommand cmd = new SqlCommand();
                var data = new JObject();
                cmd = new SqlCommand("sp_Updatedynamicoption_child", connection, trans);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@id",Int32.Parse(json.GetValue("id").ToString()));
                cmd.Parameters.AddWithValue("@key", json.GetValue("key").ToString());
                cmd.Parameters.AddWithValue("@value", json.GetValue("value").ToString());
                cmd.Parameters.AddWithValue("@name", json.GetValue("name").ToString());
                cmd.Parameters.AddWithValue("@counter", Int32.Parse(json.GetValue("counter").ToString()));
                cmd.Parameters.AddWithValue("@parent", parent);
                cmd.Parameters.AddWithValue("@usr", json.GetValue("user").ToString());

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                trans.Commit();
                strout = "success";
            }
            catch (Exception ex)
            {
                trans.Rollback();
                strout = ex.Message;
            }
            connection.Close();
            SqlConnection.ClearPool(connection);
            return strout;
        }

        public string UpdateDynamicOptionchildisused(JObject json)
        {
            string strout = "";
            var retObject = new List<dynamic>();
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("pyfatrack");
            JObject jo = new JObject();
            var conn = dbconn.constringList(provider, cstrname);
            SqlTransaction trans;
            SqlConnection connection = new SqlConnection(conn);
            connection.Open();
            trans = connection.BeginTransaction();

            try
            {
               
                SqlCommand cmd = new SqlCommand();
                var data = new JObject();
                cmd = new SqlCommand("sp_Updatedynamicoption_child_isused", connection, trans);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@id", Int32.Parse(json.GetValue("id").ToString()));
                cmd.Parameters.AddWithValue("@isused", json.GetValue("isused").ToString());
                cmd.Parameters.AddWithValue("@usr", json.GetValue("user").ToString());

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                trans.Commit();
                strout = "success";
            }
            catch (Exception ex)
            {
                trans.Rollback();
                strout = ex.Message;
            }
            connection.Close();
            SqlConnection.ClearPool(connection);
            return strout;
        }

        #endregion

        #region master field
        public string insertmasterfield(JObject json)
        {
            string strout = "";
            var retObject = new List<dynamic>();
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("pyfatrack");
            JObject jo = new JObject();
            var conn = dbconn.constringList(provider, cstrname);
            SqlTransaction trans;
            SqlConnection connection = new SqlConnection(conn);
            connection.Open();
            trans = connection.BeginTransaction();

            try
            {
                string length = bc.CheckValueData(json, "length") != "" ? bc.CheckValueData(json, "length") : "0";

                SqlCommand cmd = new SqlCommand();
                var data = new JObject();
                cmd = new SqlCommand("sp_insertmasterfield", connection, trans);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@header_name", json.GetValue("header_name").ToString());
                cmd.Parameters.AddWithValue("@label_name", json.GetValue("label_name").ToString());
                cmd.Parameters.AddWithValue("@type_element", json.GetValue("type_element").ToString());
                cmd.Parameters.AddWithValue("@required", json.GetValue("required").ToString());
                cmd.Parameters.AddWithValue("@placeholder", json.GetValue("placeholder").ToString());
                cmd.Parameters.AddWithValue("@format", json.GetValue("format").ToString());
                cmd.Parameters.AddWithValue("@length", Int32.Parse(length));
                cmd.Parameters.AddWithValue("@source", json.GetValue("source").ToString());
                cmd.Parameters.AddWithValue("@usr", json.GetValue("user").ToString());

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                trans.Commit();
                strout = "success";
            }
            catch (Exception ex)
            {
                trans.Rollback();
                strout = ex.Message;
            }
            connection.Close();
            SqlConnection.ClearPool(connection);
            return strout;
        }

        public string updatemasterfield(JObject json)
        {
            string strout = "";
            var retObject = new List<dynamic>();
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("pyfatrack");
            JObject jo = new JObject();
            var conn = dbconn.constringList(provider, cstrname);
            SqlTransaction trans;
            SqlConnection connection = new SqlConnection(conn);
            connection.Open();
            trans = connection.BeginTransaction();

            try
            {

                string id = bc.CheckValueData(json, "id") != "" ? bc.CheckValueData(json, "id") : "0";
                string length = bc.CheckValueData(json, "length") != "" ? bc.CheckValueData(json, "length") : "0";

                SqlCommand cmd = new SqlCommand();
                var data = new JObject();
                cmd = new SqlCommand("sp_updatemasterfield", connection, trans);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@id", Int32.Parse(id));
                cmd.Parameters.AddWithValue("@header_name", json.GetValue("header_name").ToString());
                cmd.Parameters.AddWithValue("@label_name", json.GetValue("label_name").ToString());
                cmd.Parameters.AddWithValue("@type_element", json.GetValue("type_element").ToString());
                cmd.Parameters.AddWithValue("@required", json.GetValue("required").ToString());
                cmd.Parameters.AddWithValue("@placeholder", json.GetValue("placeholder").ToString());
                cmd.Parameters.AddWithValue("@format", json.GetValue("format").ToString());
                cmd.Parameters.AddWithValue("@length", Int32.Parse(length));
                cmd.Parameters.AddWithValue("@source", json.GetValue("source").ToString());
                cmd.Parameters.AddWithValue("@usr", json.GetValue("user").ToString());

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                trans.Commit();
                strout = "success";
            }
            catch (Exception ex)
            {
                trans.Rollback();
                strout = ex.Message;
            }
            connection.Close();
            SqlConnection.ClearPool(connection);
            return strout;
        }
        #endregion
    }
}
