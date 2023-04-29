using ConsumingThirdPartyAPI.Models.DTO;
using Swashbuckle.Swagger.Annotations;
using Swashbuckle.Swagger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.SignalR.Hosting;
using System.Data.SqlClient;
using System.Data;
using System.Runtime.Remoting.Messaging;
using Newtonsoft.Json;

namespace ConsumingThirdPartyAPI.Controllers
{
 
    public class ThirdPartyController : ApiController
    {
        [HttpPost]
        [Route("create/credentials")]
        [SwaggerOperation(Tags = new[] { "API required Credentials" })]
        public async Task<dynamic> CreateAPICredentials(ApiCredentials information)
        {
            try
            {
                //extract Authorization token form request header 
                System.Net.Http.Headers.HttpRequestHeaders header = Request.Headers;
                string _AccessToken = header.GetValues("Authorization").FirstOrDefault();

                SqlConnection con = new SqlConnection(GlobalOperations.ConnectionString);
                con.Open();
                SqlCommand cmd = new SqlCommand("CredentialsOperations", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ApiCredentialsId", 0);
                cmd.Parameters.AddWithValue("@APIUrl", information.APIUrl);
                cmd.Parameters.AddWithValue("@ConsumerKey", information.ConsumerKey);
                cmd.Parameters.AddWithValue("@SecretKey", information.SecretKey);
                cmd.Parameters.AddWithValue("@APIkey", information.APIkey);
                cmd.Parameters.AddWithValue("@TokenURl", information.TokenURl);
                cmd.Parameters.AddWithValue("@IsDefault ", information.IsDefault);
                cmd.Parameters.AddWithValue("@IsValid", information.IsValid);
                cmd.Parameters.AddWithValue("@IsUpdate", false);
                cmd.Parameters.AddWithValue("@UserName", GlobalOperations.GetUserName(_AccessToken));
                SqlParameter responsedata = new SqlParameter("@ResponseOut", SqlDbType.NVarChar, 500)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(responsedata);
                if (con.State == ConnectionState.Closed) { con.Open(); }
                cmd.CommandTimeout = 999999999; cmd.ExecuteNonQuery();
                var Outresponse = cmd.Parameters["@responseOut"].Value.ToString();
                return Ok(Outresponse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }
            
        }



        [HttpPost]
        [Route("update/credentials")]
        [SwaggerOperation(Tags = new[] { "API required Credentials" })]
        public async Task<dynamic> UpdateCreateAPICredentials(ApiCredentials information)
        {
            try
            {
                //extract Authorization token form request header 
                System.Net.Http.Headers.HttpRequestHeaders header = Request.Headers;
                string _AccessToken = header.GetValues("Authorization").FirstOrDefault();

                SqlConnection con = new SqlConnection(GlobalOperations.ConnectionString);
                con.Open();
                SqlCommand cmd = new SqlCommand("CredentialsOperations", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ApiCredentialsId", information.ApiCredentialsId);
                cmd.Parameters.AddWithValue("@APIUrl", information.APIUrl);
                cmd.Parameters.AddWithValue("@ConsumerKey", information.ConsumerKey);
                cmd.Parameters.AddWithValue("@SecretKey", information.SecretKey);
                cmd.Parameters.AddWithValue("@APIkey", information.APIkey);
                cmd.Parameters.AddWithValue("@TokenURl", information.TokenURl);
                cmd.Parameters.AddWithValue("@IsDefault ", information.IsDefault);
                cmd.Parameters.AddWithValue("@IsValid", information.IsValid);
                cmd.Parameters.AddWithValue("@IsUpdate", true);
                cmd.Parameters.AddWithValue("@UserName", GlobalOperations.GetUserName(_AccessToken));
                SqlParameter responsedata = new SqlParameter("@ResponseOut", SqlDbType.NVarChar, 500)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(responsedata);
                if (con.State == ConnectionState.Closed) { con.Open(); }
                cmd.CommandTimeout = 999999999; cmd.ExecuteNonQuery();
                var Outresponse = cmd.Parameters["@responseOut"].Value.ToString();
                return Ok(Outresponse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }

        }



        [HttpPost]
        [Route("get/credentials")]
        [SwaggerOperation(Tags = new[] { "API required Credentials" })]
        public async Task<dynamic> GetCredentials(GetApiCredentials information)
        {
            try
            {
                //extract Authorization token form request header 
                System.Net.Http.Headers.HttpRequestHeaders header = Request.Headers;
                string _AccessToken = header.GetValues("Authorization").FirstOrDefault();

                //Get Credentials
                var credentials = GlobalOperations.GetAPICredentials(information.Code, information.SearchId);
                return Ok(credentials);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }

        }


        [HttpPost]
        [Route("get/account/balance")]
        [SwaggerOperation(Tags = new[] { "Coop Bank API Operations" })]
        public async Task<dynamic> GetAccountBalance(AccountBalance information)
        {
            try
            {
                //extract Authorization token form request header 
                System.Net.Http.Headers.HttpRequestHeaders header = Request.Headers;
                string _AccessToken = header.GetValues("Authorization").FirstOrDefault();

                //Get Credentials
                var credentials = GlobalOperations.GetAPICredentials(2,0);
                var responseValidation = JsonConvert.DeserializeObject<Dictionary<string, string>>(credentials.ToString().Replace("[",string.Empty).Replace("]",string.Empty));

                string ApiUrl = string.Empty;  try { ApiUrl = responseValidation["ApiUrl"]; } catch (Exception ex) { }
                string ConsumerKey = string.Empty; try { ConsumerKey = responseValidation["ConsumerKey"]; } catch (Exception ex) { }
                string SecretKey = string.Empty; try { SecretKey = responseValidation["SecretKey"]; } catch (Exception ex) { }
                string TokenURl = string.Empty; try { TokenURl = responseValidation["TokenURl"]; } catch (Exception ex) { }
                string APIkey = string.Empty; try { APIkey = responseValidation["APIkey"]; } catch (Exception ex) { }
                
                var ApiResponse = CoopBankApiClient.AccountBalance(information.MessageReference, information.AccountNumber, ApiUrl, "Enquiry/AccountBalance/1.0.0", TokenURl, ConsumerKey, SecretKey);
                //write code to process API responses here

                return Ok(ApiResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }

        }


    }
}
