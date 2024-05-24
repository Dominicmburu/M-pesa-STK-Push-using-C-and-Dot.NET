using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;

namespace mpesa
{
    public partial class Home : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnPay_Click(object sender, EventArgs e)
        {
            string phoneNumber = txtPhoneNumber.Text;
            string amount = txtAmount.Text;
            string token = GetOAuthToken();

            if (!string.IsNullOrEmpty(token))
            {
                InitiateSTKPush(token, phoneNumber, amount);
            }
        }

        private string GetOAuthToken()
        {
            string url = "https://sandbox.safaricom.co.ke/oauth/v1/generate?grant_type=client_credentials";
            string credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes("iNPXCUm9Fvst1Xvtlz5vOYzk68nMsCorGGO68ylnGRCiNTlF:3Sv6zM74lvz4LX5vhCh9XSdzplmF4J16BwnrWGjSn7vDr9KnZrSGU70mevsPY1lM"));

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Headers.Add("Authorization", "Basic " + credentials);
            request.Method = "GET";

            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    string jsonResponse = reader.ReadToEnd();
                    dynamic result = JsonConvert.DeserializeObject(jsonResponse);
                    return result.access_token;
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                System.Diagnostics.Debug.WriteLine("Error getting OAuth token: " + ex.Message);
                return null;
            }
        }

        private void InitiateSTKPush(string token, string phoneNumber, string amount)
        {
            string url = "https://sandbox.safaricom.co.ke/mpesa/stkpush/v1/processrequest";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Headers.Add("Authorization", "Bearer " + token);
            request.Method = "POST";
            request.ContentType = "application/json";

            string businessShortCode = "174379";
            string lipaNaMpesaOnlinePasskey = "bfb279f9aa9bdbcf158e97dd71a467cd2e0c893059b10f78e6b72ada1ed2c919";
            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            string password = Convert.ToBase64String(Encoding.UTF8.GetBytes(businessShortCode + lipaNaMpesaOnlinePasskey + timestamp));

            var stkPushRequest = new
            {
                BusinessShortCode = businessShortCode,
                Password = password,
                Timestamp = timestamp,
                TransactionType = "CustomerPayBillOnline",
                Amount = amount,
                PartyA = phoneNumber,
                PartyB = businessShortCode,
                PhoneNumber = phoneNumber,
                CallBackURL = "https://us-central1-norse-journey-412116.cloudfunctions.net/callbackHandler",
                AccountReference = "Test123",
                TransactionDesc = "Payment for test"
            };

            string json = JsonConvert.SerializeObject(stkPushRequest);

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }

            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    string result = reader.ReadToEnd();
                    // Handle response from M-Pesa
                    System.Diagnostics.Debug.WriteLine("STK Push response: " + result);
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                System.Diagnostics.Debug.WriteLine("Error initiating STK Push: " + ex.Message);
            }
        }
    }
}
