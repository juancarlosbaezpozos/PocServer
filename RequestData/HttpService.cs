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
                    RequestSecretModel secret = new RequestSecretModel
                    {
                        SecretName = secretName
                    };
                    var jsonData = secret.ToJson();

                    request = (HttpWebRequest)WebRequest.Create(ConfigurationManager.AppSettings["internalUrl"].ToString());
                    request.Method = "POST";
                    request.ContentType = "application/json";
                    request.Headers.Add("Custom-Claims", ConfigurationManager.AppSettings["internalClaims"].ToString());

                    using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                    {
                        streamWriter.Write(jsonData);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }
                }

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        using (Stream responseStream = response.GetResponseStream())
                        {
                            using (StreamReader reader = new StreamReader(responseStream))
                            {
                                return reader.ReadToEnd();
                            }
                        }
                    }
                    else
                    {
                        throw new WebException($"HTTP status code - {response.StatusCode}");
                    }
                }
            }
            catch (WebException webEx)
            {
                if (webEx.Response != null)
                {
                    using (var errorResponse = (HttpWebResponse)webEx.Response)
                    {
                        using (var reader = new StreamReader(errorResponse.GetResponseStream()))
                        {
                            string errorText = reader.ReadToEnd();
                            throw new Exception(errorText);
                        }
                    }
                }
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error - {ex.Message}");
            }
        }
    }
}
