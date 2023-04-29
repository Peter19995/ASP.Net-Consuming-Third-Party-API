using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace ConsumingThirdPartyAPI
{
    public class GlobalOperations
    {
        public static string ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public static string GetUserName( string UserAuthToken) 
        {
            //Write your code here to return user based on the Auth Token passed
            return "TEST USER";
        }

        public static object GetAPICredentials(int code, int SearchID)
        {
            SqlConnection con = new SqlConnection(GlobalOperations.ConnectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("GetCredentials", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Code", code);
            cmd.Parameters.AddWithValue("@SearchId", SearchID);
            DataTable table = new DataTable();
            using (var da = new SqlDataAdapter(cmd))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(table);
            }
            con.Close(); con.Dispose();

            return JsonConvert.SerializeObject(table);
        }



    }
}