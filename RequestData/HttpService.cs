using System;
using System.Configuration;
using System.IO;
using System.Net;

namespace RequestData
{
    public static class HttpService
    {
        private static readonly object lockDet = new object();

        public static string MakeSecretRequest(string secretName)
        {
            try
            {
                HttpWebRequest request;
                lock (lockDet)
                {
                    var secret = new RequestSecretModel
                    {
                        SecretName = secretName
                    };
                    var jsonData = secret.ToJson();

                    request = (HttpWebRequest)WebRequest.Create(ConfigurationManager.AppSettings["internalUrl"]);
                    request.Method = "POST";
                    request.ContentType = "application/json";
                    request.Headers.Add("Custom-Claims", ConfigurationManager.AppSettings["internalClaims"]);

                    using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                    {
                        streamWriter.Write(jsonData);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }
                }

                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        throw new WebException($"HTTP status code - {response.StatusCode}");

                    using (var responseStream = response.GetResponseStream())
                    {
                        using (var reader = new StreamReader(responseStream ?? throw new InvalidOperationException()))
                        {
                            return reader.ReadToEnd();
                        }
                    }
                }
            }
            catch (WebException webEx)
            {
                if (webEx.Response == null) throw;
                using (var errorResponse = (HttpWebResponse)webEx.Response)
                {
                    using (var reader = new StreamReader(errorResponse.GetResponseStream() ?? throw new InvalidOperationException()))
                    {
                        var errorText = reader.ReadToEnd();
                        throw new Exception(errorText);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error - {ex.Message}");
            }
        }
    }
}
