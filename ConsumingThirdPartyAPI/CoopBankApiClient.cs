
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;
using System.Text;
using System.Web.Script.Serialization;

namespace ConsumingThirdPartyAPI
{
    public class CoopBankApiClient
    {
        public static string DataTableToJSON(DataTable table)
        {
            string _json = string.Empty;
            try
            {
                _json = JsonConvert.SerializeObject(table);
            }
            catch (Exception ex)
            {
                _json = "invalid";
            }

            return _json;
        }

        public static string GetCoopURL()
        {
            String URL = string.Empty;

            //URL = "https://developer.co-opbank.co.ke:8243/";
            URL = "https://openapi-sandbox.co-opbank.co.ke/";
            return URL;
        }

        public static string GenerateCooptoken(string URL, string consumerKey, string consumerSecret)
        {
            string AccessToken = string.Empty;
            string encodedStr = Convert.ToBase64String(Encoding.UTF8.GetBytes(consumerKey + ":" + consumerSecret));
            RestClient client = new RestClient(URL);
            RestRequest request = new RestRequest("token", Method.Post);
            //request.AddHeader("Content-Type", "application/json");
            request.AddParameter("grant_type", "client_credentials");
            request.AddHeader("Authorization", "Basic " + encodedStr);
            var responseOUT = client.Execute(request).Content;
            var Dserializer = new JavaScriptSerializer();
            var DreceivedData = Dserializer.Deserialize<dynamic>(responseOUT);
            try { AccessToken = DreceivedData["access_token"].ToString(); } catch (Exception ex) { }
            return AccessToken;
        }

        public static string AccountBalance(string MessageReference, string AccountNumber, string URL, string endpoint, string TokenURl, string Consumerkey, string ConsumerSecret)
        {

            RestClient client = new RestClient(URL);
            RestRequest request = new RestRequest(endpoint, Method.Post);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", "Bearer " + GenerateCooptoken(TokenURl, Consumerkey, ConsumerSecret));
            var body = new
            {
                MessageReference = MessageReference,
                AccountNumber = AccountNumber

            };
            request.AddJsonBody(body);
            var responseOUT = client.Execute(request).Content;

            return responseOUT.ToString();

            //return "{\"MessageReference\": \"40ca18c6765086089a1\",\"MessageDateTime\": \"2022-06-23T04:42:43.778Z\",\"MessageCode\": \"0\",\"MessageDescription\": \"Success\",\"AccountNumber\": \"36001873000\",\"AccountName\": \"Your Account Name\",\"Currency\": \"KES\",\"ProductName\": \"Savings Account\",\"ClearedBalance\": 2195.5,\"BookedBalance\": 0,\"BlockedBalance\": 1760,\"AvailableBalance\": 0,\"ArrearsAmount\": 0,\"BranchName\": \"UKULIMA BRANCH\",\"BranchSortCode\": \"00011011\",\"AverageBalance\": 75.83,\"UnclearedBalance\": -2195.5,\"ODLimit\": 0,\"CreditLimit\": 1000}";
        }

        public static string AccountFullStatement(string MessageReference, string AccountNumber, string StartDate, string EndDate, string URL, string endpoint, string TokenURl, string Consumerkey, string ConsumerSecret)
        {
            RestClient client = new RestClient(URL);
            RestRequest request = new RestRequest(endpoint, Method.Post);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", "Bearer " + GenerateCooptoken(TokenURl, Consumerkey, ConsumerSecret));
            var body = new
            {
                MessageReference = MessageReference,
                AccountNumber = AccountNumber,
                StartDate = StartDate,
                EndDate = EndDate

            };
            request.AddJsonBody(body);
            var responseOUT = client.Execute(request).Content;
            return responseOUT.ToString();
            //return "{\"MessageReference\":\"40ca18c6765086089a1\",\"MessageDateTime\":\"2021-03-22 08:42:41\",\"MessageCode\":\"0\",\"MessageDescription\":\"Success\",\"AccountNumber\":\"36001873000\",\"AccountName\":\"JOE K. DOE\",\"Transactions\":[{\"TransactionID\":\"f166dbca-d\",\"TransactionDate\":\"2021-05-09 14:43:38\",\"ValueDate\":\"2021-05-09 14:43:38\",\"Narration\":\"CASH DEPOSIT\",\"TransactionType\":\"C\",\"ServicePoint\":\"BRANCH\",\"TransactionReference\":\"21703a85-7\",\"CreditAmount\":\"67929.04\",\"DebitAmount\":\"0.00\",\"RunningClearedBalance\":\"82839.91\",\"RunningBookBalance\":\"82839.91\",\"DebitLimit\":\"0.00\",\"LimitExpiryDate\":\"2021-03-13 00:00:00\"},{\"TransactionID\":\"b86cf550-f\",\"TransactionDate\":\"2021-05-07 08:19:31\",\"ValueDate\":\"2021-05-07 08:19:31\",\"Narration\":\"Electricity payment\",\"TransactionType\":\"D\",\"ServicePoint\":\"POS-P0917\",\"TransactionReference\":\"e58f781a-4\",\"CreditAmount\":\"0.00\",\"DebitAmount\":\"19175.04\",\"RunningClearedBalance\":\"14910.87\",\"RunningBookBalance\":\"14910.87\",\"DebitLimit\":\"0.00\",\"LimitExpiryDate\":\"2021-03-12 00:00:00\"},{\"TransactionID\":\"84dd3d1f-4\",\"TransactionDate\":\"2021-03-12 15:04:28\",\"ValueDate\":\"2021-03-12 15:04:28\",\"Narration\":\"Payment for goods\",\"TransactionType\":\"D\",\"ServicePoint\":\"POS-P0001\",\"TransactionReference\":\"80a5d71e-0\",\"CreditAmount\":\"0.00\",\"DebitAmount\":\"72313.18\",\"RunningClearedBalance\":\"10526.72\",\"RunningBookBalance\":\"10526.72\",\"DebitLimit\":\"0.00\",\"LimitExpiryDate\":\"2021-04-21 00:00:00\"},{\"TransactionID\":\"aa57011a-f\",\"TransactionDate\":\"2021-01-19 01:38:29\",\"ValueDate\":\"2021-01-19 01:38:29\",\"Narration\":\"C2B PUSH\",\"TransactionType\":\"C\",\"ServicePoint\":\"MOBILE\",\"TransactionReference\":\"663a3311-4\",\"CreditAmount\":\"7894.87\",\"DebitAmount\":\"0.00\",\"RunningClearedBalance\":\"34085.91\",\"RunningBookBalance\":\"34085.91\",\"DebitLimit\":\"0.00\",\"LimitExpiryDate\":\"2021-04-28 00:00:00\"}]}";
        }

        public static string AccountMiniStatement(string MessageReference, string AccountNumber, string URL, string endpoint, string TokenURl, string Consumerkey, string ConsumerSecret)
        {

            RestClient client = new RestClient(URL);
            RestRequest request = new RestRequest(endpoint, Method.Post);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", "Bearer " + GenerateCooptoken(TokenURl, Consumerkey, ConsumerSecret));
            var body = new
            {
                MessageReference = MessageReference,
                AccountNumber = AccountNumber
            };
            request.AddJsonBody(body);
            var responseOUT = client.Execute(request).Content;
            return responseOUT.ToString();
            //return "{\"MessageReference\":\"40ca18c6765086089a1\",\"MessageDateTime\":\"2021-03-22 08:42:41\",\"MessageCode\":\"0\",\"MessageDescription\":\"Success\",\"AccountNumber\":\"36001873000\",\"AccountName\":\"JOE K. DOE\",\"Transactions\":[{\"TransactionID\":\"f166dbca-d\",\"TransactionDate\":\"2021-05-09 14:43:38\",\"ValueDate\":\"2021-05-09 14:43:38\",\"Narration\":\"CASH DEPOSIT\",\"TransactionType\":\"C\",\"ServicePoint\":\"BRANCH\",\"TransactionReference\":\"21703a85-7\",\"CreditAmount\":\"67929.04\",\"DebitAmount\":\"0.00\",\"RunningClearedBalance\":\"82839.91\",\"RunningBookBalance\":\"82839.91\",\"DebitLimit\":\"0.00\",\"LimitExpiryDate\":\"2021-03-13 00:00:00\"},{\"TransactionID\":\"b86cf550-f\",\"TransactionDate\":\"2021-05-07 08:19:31\",\"ValueDate\":\"2021-05-07 08:19:31\",\"Narration\":\"Electricity payment\",\"TransactionType\":\"D\",\"ServicePoint\":\"POS-P0917\",\"TransactionReference\":\"e58f781a-4\",\"CreditAmount\":\"0.00\",\"DebitAmount\":\"19175.04\",\"RunningClearedBalance\":\"14910.87\",\"RunningBookBalance\":\"14910.87\",\"DebitLimit\":\"0.00\",\"LimitExpiryDate\":\"2021-03-12 00:00:00\"},{\"TransactionID\":\"84dd3d1f-4\",\"TransactionDate\":\"2021-03-12 15:04:28\",\"ValueDate\":\"2021-03-12 15:04:28\",\"Narration\":\"Payment for goods\",\"TransactionType\":\"D\",\"ServicePoint\":\"POS-P0001\",\"TransactionReference\":\"80a5d71e-0\",\"CreditAmount\":\"0.00\",\"DebitAmount\":\"72313.18\",\"RunningClearedBalance\":\"10526.72\",\"RunningBookBalance\":\"10526.72\",\"DebitLimit\":\"0.00\",\"LimitExpiryDate\":\"2021-04-21 00:00:00\"},{\"TransactionID\":\"aa57011a-f\",\"TransactionDate\":\"2021-01-19 01:38:29\",\"ValueDate\":\"2021-01-19 01:38:29\",\"Narration\":\"C2B PUSH\",\"TransactionType\":\"C\",\"ServicePoint\":\"MOBILE\",\"TransactionReference\":\"663a3311-4\",\"CreditAmount\":\"7894.87\",\"DebitAmount\":\"0.00\",\"RunningClearedBalance\":\"34085.91\",\"RunningBookBalance\":\"34085.91\",\"DebitLimit\":\"0.00\",\"LimitExpiryDate\":\"2021-04-28 00:00:00\"}]}";
        }

        public static string AccountTransactions(string MessageReference, string AccountNumber, string NoOfTransactions, string URL, string endpoint, string TokenURl, string Consumerkey, string ConsumerSecret)
        {
            RestClient client = new RestClient(URL);
            RestRequest request = new RestRequest(endpoint, Method.Post);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", "Bearer " + GenerateCooptoken(TokenURl, Consumerkey, ConsumerSecret));
            var body = new
            {
                MessageReference = MessageReference,
                AccountNumber = AccountNumber,
                NoOfTransactions = NoOfTransactions
            };
            request.AddJsonBody(body);
            var responseOUT = client.Execute(request).Content;
            return responseOUT.ToString();

            //return "{\"MessageReference\": \"40ca18c6765086089a1\",\"MessageDateTime\": \"2017 - 12 - 04T09: 27:00\",\"MessageCode\": \"0\",\"MessageDescription\": \"Success\",\"AccountNumber\":\"36001873000\",\"AccountName\": \"Your Account Name\",\"NoOfTransactions\": \"1\",\"TotalCredits\": \"200.0\",\"TotalDebits\": \"0.0\",\"Transactions\": [{\"TransactionID\":\"116bdbebcca41aXF\",\"TransactionDate\": \"2019 - 04 - 29T10: 05:41.178 + 03:00\",\"ValueDate\": \"2019 - 04 - 29T10: 05:40.751 + 03:00\",\"Narration\": \"Electricity Payment\",\"TransactionType\": \"C\",\"ServicePoint\": \"ATM\",\"TransactionReference\": \"911909902484902484\",\"CreditAmount\": \"200.0\",\"DebitAmount\": \"0.0\",\"RunningClearedBalance\": \"1215.7\",\"RunningBookBalance\": \"1215.7\",\"DebitLimit\": \"0.0\",\"LimitExpiryDate\": \"2019 - 04 - 29T10: 05:41.178 + 03:00\"}]}";
        }

        public static string AccountValidation(string MessageReference, string AccountNumber, string URL, string endpoint, string TokenURl, string Consumerkey, string ConsumerSecret)
        {

            RestClient client = new RestClient(URL);
            RestRequest request = new RestRequest(endpoint, Method.Post);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", "Bearer " + GenerateCooptoken(TokenURl, Consumerkey, ConsumerSecret));
            var body = new
            {
                MessageReference = MessageReference,
                AccountNumber = AccountNumber
            };
            request.AddJsonBody(body);
            var responseOUT = client.Execute(request).Content;
            return responseOUT.ToString();
            //return "{\"MessageReference\":\"40ca18c6765086089a1\",\"MessageDateTime\":\"2021-03-22 08:42:41\",\"MessageCode\":\"0\",\"MessageDescription\":\"Success\",\"AccountNumber\":\"36001873000\",\"AccountName\":\"JOE K. DOE\",\"Transactions\":[{\"TransactionID\":\"f166dbca-d\",\"TransactionDate\":\"2021-05-09 14:43:38\",\"ValueDate\":\"2021-05-09 14:43:38\",\"Narration\":\"CASH DEPOSIT\",\"TransactionType\":\"C\",\"ServicePoint\":\"BRANCH\",\"TransactionReference\":\"21703a85-7\",\"CreditAmount\":\"67929.04\",\"DebitAmount\":\"0.00\",\"RunningClearedBalance\":\"82839.91\",\"RunningBookBalance\":\"82839.91\",\"DebitLimit\":\"0.00\",\"LimitExpiryDate\":\"2021-03-13 00:00:00\"},{\"TransactionID\":\"b86cf550-f\",\"TransactionDate\":\"2021-05-07 08:19:31\",\"ValueDate\":\"2021-05-07 08:19:31\",\"Narration\":\"Electricity payment\",\"TransactionType\":\"D\",\"ServicePoint\":\"POS-P0917\",\"TransactionReference\":\"e58f781a-4\",\"CreditAmount\":\"0.00\",\"DebitAmount\":\"19175.04\",\"RunningClearedBalance\":\"14910.87\",\"RunningBookBalance\":\"14910.87\",\"DebitLimit\":\"0.00\",\"LimitExpiryDate\":\"2021-03-12 00:00:00\"},{\"TransactionID\":\"84dd3d1f-4\",\"TransactionDate\":\"2021-03-12 15:04:28\",\"ValueDate\":\"2021-03-12 15:04:28\",\"Narration\":\"Payment for goods\",\"TransactionType\":\"D\",\"ServicePoint\":\"POS-P0001\",\"TransactionReference\":\"80a5d71e-0\",\"CreditAmount\":\"0.00\",\"DebitAmount\":\"72313.18\",\"RunningClearedBalance\":\"10526.72\",\"RunningBookBalance\":\"10526.72\",\"DebitLimit\":\"0.00\",\"LimitExpiryDate\":\"2021-04-21 00:00:00\"},{\"TransactionID\":\"aa57011a-f\",\"TransactionDate\":\"2021-01-19 01:38:29\",\"ValueDate\":\"2021-01-19 01:38:29\",\"Narration\":\"C2B PUSH\",\"TransactionType\":\"C\",\"ServicePoint\":\"MOBILE\",\"TransactionReference\":\"663a3311-4\",\"CreditAmount\":\"7894.87\",\"DebitAmount\":\"0.00\",\"RunningClearedBalance\":\"34085.91\",\"RunningBookBalance\":\"34085.91\",\"DebitLimit\":\"0.00\",\"LimitExpiryDate\":\"2021-04-28 00:00:00\"}]}";
        }

        public static string ExchangeRate(string MessageReference, string FromCurrencyCode, string ToCurrencyCode, string URL, string endpoint, string TokenURl, string Consumerkey, string ConsumerSecret)
        {

            RestClient client = new RestClient(URL);
            RestRequest request = new RestRequest(endpoint, Method.Post);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", "Bearer " + GenerateCooptoken(TokenURl, Consumerkey, ConsumerSecret));
            var body = new
            {
                MessageReference = MessageReference,
                FromCurrencyCode = FromCurrencyCode,
                ToCurrencyCode = ToCurrencyCode
            };
            request.AddJsonBody(body);
            var responseOUT = client.Execute(request).Content;
            return responseOUT.ToString();
            //return "{\"MessageReference\": \"40ca18c6765086089a1\",\"MessageDateTime\": \"2017-12-04T09:27:00\",\"MessageCode\": \"0\",\"MessageDescription\": \"Success\",\"FromCurrencyCode\": \"KES\",\"ToCurrencyCode\": \"USD\",\"RateType\": \"SPOT\",\"Rate\": \"103.5\",\"Tolerance\": \"15.0\",\"MultiplyDivide\": \"D\"}";
        }

        public static string IFTAccountToAccount(string _SourceAccountNumber, string _Amount, string _SourceTransactionCurrency, string _SourceNarration, string _ReferenceNumber, string _DestinationsAccountNumber, string _DestinationsTransactionCurrency, string _DestinationsNarration, string URL, string Endpoint, string MessageReference, string CallBackUrl, string TokenURl, string Consumerkey, string ConsumerSecret)
        {

            RestClient client = new RestClient(URL);
            RestRequest request = new RestRequest(Endpoint, Method.Post);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", "Bearer " + GenerateCooptoken(TokenURl, Consumerkey, ConsumerSecret));
            var body = new
            {
                MessageReference = MessageReference,
                CallBackUrl = CallBackUrl,
                Source = new
                {
                    AccountNumber = _SourceAccountNumber,
                    Amount = _Amount,
                    TransactionCurrency = _SourceTransactionCurrency,
                    Narration = _SourceNarration
                },
                Destinations = new List<dynamic> {
                    new
                    {
                        ReferenceNumber = _ReferenceNumber,
                        AccountNumber = _DestinationsAccountNumber,
                        Amount = _Amount,
                        TransactionCurrency = _DestinationsTransactionCurrency,
                        Narration = _DestinationsNarration
                    }
                },
            };
            request.AddJsonBody(body);
            var responseOUT = client.Execute(request).Content;
            return responseOUT.ToString();
            //return "{\"MessageReference\": \"EEBAF245-0016\",\"MessageDateTime\": \"2017 - 12 - 04T09: 27:00\",\"MessageCode\": \"0\",\"MessageDescription\": \"REQUEST ACCEPTED FOR PROCESSING\"}";
        }

        public static string PesaLinkSendToAccount(string _SourceAccountNumber, string _Amount, string _SourceTransactionCurrency, string _SourceNarration, string _ReferenceNumber, string _DestinationsAccountNumber, string _BankCode, string _DestinationsTransactionCurrency, string _DestinationsNarration, string MessageReference, string CallBackUrl, string URL, string endpoint, string TokenURl, string Consumerkey, string ConsumerSecret)
        {
            RestClient client = new RestClient(URL);
            RestRequest request = new RestRequest(endpoint, Method.Post);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", "Bearer " + GenerateCooptoken(TokenURl, Consumerkey, ConsumerSecret));
            var body = new
            {
                MessageReference = MessageReference,
                CallBackUrl = CallBackUrl,
                Source = new
                {
                    AccountNumber = _SourceAccountNumber,
                    Amount = _Amount,
                    TransactionCurrency = _SourceTransactionCurrency,
                    Narration = _SourceNarration
                },
                Destinations = new List<dynamic> {
                    new
                    {
                        ReferenceNumber = _ReferenceNumber,
                        BankCode = _BankCode,
                        AccountNumber = _DestinationsAccountNumber,
                        Amount = _Amount,
                        TransactionCurrency = _DestinationsTransactionCurrency,
                        Narration = _DestinationsNarration
                    }
                },
            };
            request.AddJsonBody(body);
            var responseOUT = client.Execute(request).Content;
            return responseOUT.ToString();
            //return "{\"MessageReference\": \"40ca18c6765086089a1\",\"MessageDateTime\": \"2017 - 12 - 04T09: 27:00\",\"MessageCode\": \"0\",\"MessageDescription\": \"REQUEST ACCEPTED FOR PROCESSING\"}";
        }

        public static string PesaLinkSendToPhone(string _SourceAccountNumber, string _Amount, string _SourceTransactionCurrency, string _SourceNarration, string _ReferenceNumber, string _PhoneNumber, string _DestinationsTransactionCurrency, string _DestinationsNarration, string MessageReference, string CallBackUrl, string URL, string endpoint, string TokenURl, string Consumerkey, string ConsumerSecret)
        {

            RestClient client = new RestClient(URL);
            RestRequest request = new RestRequest(endpoint, Method.Post);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", "Bearer " + GenerateCooptoken(TokenURl, Consumerkey, ConsumerSecret));
            var body = new
            {
                MessageReference = MessageReference,
                CallBackUrl = CallBackUrl,
                Source = new
                {
                    AccountNumber = _SourceAccountNumber,
                    Amount = _Amount,
                    TransactionCurrency = _SourceTransactionCurrency,
                    Narration = _SourceNarration
                },
                Destinations = new List<dynamic> {
                    new
                    {
                        ReferenceNumber = _ReferenceNumber,
                        PhoneNumber = _PhoneNumber,
                        Amount = _Amount,
                        TransactionCurrency = _DestinationsTransactionCurrency,
                        Narration = _DestinationsNarration
                    }
                },
            };
            request.AddJsonBody(body);
            var responseOUT = client.Execute(request).Content;
            return responseOUT.ToString();
            //return "{\"MessageReference\": \"40ca18c6765086089a1\",\"MessageDateTime\": \"2017 - 12 - 04T09: 27:00\",\"MessageCode\": \"0\",\"MessageDescription\": \"REQUEST ACCEPTED FOR PROCESSING\"}";
        }

        public static string TransactionStatus(string MessageReference, string TokenURl, string Consumerkey, string ConsumerSecret)
        {
            string URL = GetCoopURL();
            RestClient client = new RestClient(URL);
            RestRequest request = new RestRequest("Enquiry/TransactionStatus/2.0.0/", Method.Post);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", "Bearer " + GenerateCooptoken(TokenURl, Consumerkey, ConsumerSecret));
            var body = new
            {
                MessageReference = MessageReference,
            };
            request.AddJsonBody(body);
            var responseOUT = client.Execute(request).Content;
            return responseOUT.ToString();
            //return "{\"MessageReference\": \"40ca18c6765086089a1\",\"MessageDateTime\": \"2017 - 12 - 04T09: 27:00\",\"MessageCode\": \"0\",\"MessageDescription\": \"REQUEST ACCEPTED FOR PROCESSING\"}";
        }

   
        public static DateTime GetExtractTimeStamp()
        {
            string TimeStamp = Convert.ToString(DateTime.Now.ToString("yyyyMMddHHmmss"));
            string FormatString = "yyyyMMddHHmmss";
            DateTime dt = DateTime.ParseExact(TimeStamp, FormatString, null);
            return dt;
        }

        public static string BusinessSearch(string AccessToken, string BusinessKey)
        {
            string respone = string.Empty;
            string URL = "https://cloud.unistrat.co.ke/";
            RestClient client = new RestClient(URL);
            RestRequest request = new RestRequest("finnettapidemo/business/verification2", Method.Post);
            request.AddHeader("Accept", "application/json");
            var body = new
            {
                AccessToken = AccessToken,
                BusinessKey = BusinessKey,
                TerminalID = "api",
                TerminalType = "API",
                TimeStamp = GetExtractTimeStamp()
            };
            request.AddJsonBody(body);
            var responseOUT = client.Execute(request).Content;
            var serializer = new JavaScriptSerializer();
            var receivedData = serializer.Deserialize<dynamic>(responseOUT);
            respone = receivedData;
            return respone;
        }

        public static string ClientSearch(string SearchParameter, string AccessToken, string BusinessKey)
        {
            string respone = string.Empty;
            string URL = "https://cloud.unistrat.co.ke/";
            RestClient client = new RestClient(URL);
            RestRequest request = new RestRequest("finnettapidemo/client/search", Method.Post);
            request.AddHeader("Accept", "application/json");
            var body = new
            {
                AccessKey = AccessToken,
                AccessToken = AccessToken,
                BusinessKey = BusinessKey,
                SearchCode = "ANY",
                SearchParameter = SearchParameter,
                TerminalID = "api",
                TerminalType = "api",
                TimeStamp = GetExtractTimeStamp()

            };
            request.AddJsonBody(body);
            var responseOUT = client.Execute(request).Content;
            var serializer = new JavaScriptSerializer();
            var receivedData = serializer.Deserialize<dynamic>(responseOUT);
            respone = receivedData;
            return respone;
        }

    }
}