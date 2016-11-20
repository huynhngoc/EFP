using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using ApiAiSDK;
using ApiAiSDK.Model;
using Newtonsoft.Json;

namespace ShopManager.Api.Ai.Custom
{    
    public class AIConfigurationCustom : AIConfiguration
    {
        private const string SERVICE_PROD_URL = "https://api.api.ai/v1/";
        private const string SERVICE_DEV_URL = "https://dev.api.ai/api/";

        private const string CURRENT_PROTOCOL_VERSION = "20150910";

        public new string ClientAccessToken { get; private set; }

        public string EndPoint { get; set; }

        public AIConfigurationCustom(string clientAccessToken, SupportedLanguage language) : base(clientAccessToken, language)
        {
            this.ClientAccessToken = clientAccessToken;
            this.Language = language;

            DevMode = false;
            DebugLog = false;
            VoiceActivityDetectionEnabled = true;

            ProtocolVersion = CURRENT_PROTOCOL_VERSION;
            EndPoint = "query";
        }

        public AIConfigurationCustom(string clientAccessToken, SupportedLanguage language, string endPoint) : base(clientAccessToken, language)
        {
            this.ClientAccessToken = clientAccessToken;
            this.Language = language;

            DevMode = false;
            DebugLog = false;
            VoiceActivityDetectionEnabled = true;

            ProtocolVersion = CURRENT_PROTOCOL_VERSION;
            EndPoint = endPoint;
        }
        

        public new string RequestUrl
        {
            get
            {
                var baseUrl = DevMode ? SERVICE_DEV_URL : SERVICE_PROD_URL;
                return string.Format("{0}{1}?v={2}", baseUrl, EndPoint, ProtocolVersion);
            }
        }
    }

    public class AIDataServiceCustom : AIDataService
    {
        private readonly AIConfigurationCustom configCustom;

        public AIDataServiceCustom(AIConfigurationCustom config): base(config)
        {
            this.configCustom = config;
        }

        private string AddUrl(string url, string more)
        {
            return url.Split('?')[0] + "/" + more + "?" + url.Split('?')[1];
        }
     
        public AiIntent RequestIntentGet(string intentId)
        {
            //request.Language = config.Language.code;
            //request.Timezone = TimeZone.CurrentTimeZone.StandardName;

            try
            {
                var httpRequest = (HttpWebRequest)WebRequest.Create(AddUrl(configCustom.RequestUrl,intentId));
                httpRequest.Method = "GET";
                httpRequest.ContentType = "application/json; charset=utf-8";
                httpRequest.Accept = "application/json";

                httpRequest.Headers.Add("Authorization", "Bearer " + configCustom.ClientAccessToken);

                //var jsonSettings = new JsonSerializerSettings
                //{
                //    NullValueHandling = NullValueHandling.Ignore
                //};

                //var jsonRequest = JsonConvert.SerializeObject(request, Formatting.None, jsonSettings);

                //if (config.DebugLog)
                //{
                //    Debug.WriteLine("Request: " + jsonRequest);
                //}

                //using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
                //{
                //    streamWriter.Write(jsonRequest);
                //    streamWriter.Close();
                //}

                var httpResponse = httpRequest.GetResponse() as HttpWebResponse;
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();

                    if (configCustom.DebugLog)
                    {
                        Debug.WriteLine("Response: " + result);
                    }

                    var aiResponse = JsonConvert.DeserializeObject<AiIntent>(result);

                    CheckForErrorsIntent(aiResponse);

                    return aiResponse;
                }

            }
            catch (Exception e)
            {
                throw new AIServiceException(e);
            }
        }

        public List<AiIntentResponse>  RequestIntentGetAll()
        {
            //request.Language = config.Language.code;
            //request.Timezone = TimeZone.CurrentTimeZone.StandardName;

            try
            {
                var httpRequest = (HttpWebRequest)WebRequest.Create(configCustom.RequestUrl);
                httpRequest.Method = "GET";
                httpRequest.ContentType = "application/json; charset=utf-8";
                httpRequest.Accept = "application/json";

                httpRequest.Headers.Add("Authorization", "Bearer " + configCustom.ClientAccessToken);

                //var jsonSettings = new JsonSerializerSettings
                //{
                //    NullValueHandling = NullValueHandling.Ignore
                //};

                //var jsonRequest = JsonConvert.SerializeObject(request, Formatting.None, jsonSettings);

                //if (config.DebugLog)
                //{
                //    Debug.WriteLine("Request: " + jsonRequest);
                //}

                //using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
                //{
                //    streamWriter.Write(jsonRequest);
                //    streamWriter.Close();
                //}

                var httpResponse = httpRequest.GetResponse() as HttpWebResponse;
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();

                    if (configCustom.DebugLog)
                    {
                        Debug.WriteLine("Response: " + result);
                    }

                    var aiResponse = JsonConvert.DeserializeObject<List<AiIntentResponse>>(result);

                    foreach (var res in aiResponse)
                    {
                        CheckForErrorsIntentObject(res);
                    }
                    return aiResponse;
                }

            }
            catch (Exception e)
            {
                throw new AIServiceException(e);
            }
        }
        public AIResponse RequestIntentPost(AiIntent request)
        {
            //request.Language = config.Language.code;
            //request.Timezone = TimeZone.CurrentTimeZone.StandardName;

            try
            {
                var httpRequest = (HttpWebRequest)WebRequest.Create(configCustom.RequestUrl);
                httpRequest.Method = "POST";
                httpRequest.ContentType = "application/json; charset=utf-8";
                httpRequest.Accept = "application/json";

                httpRequest.Headers.Add("Authorization", "Bearer " + configCustom.ClientAccessToken);

                var jsonSettings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                };

                var jsonRequest = JsonConvert.SerializeObject(request, Formatting.None, jsonSettings);

                if (configCustom.DebugLog)
                {
                    Debug.WriteLine("Request: " + jsonRequest);
                }

                using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
                {
                    streamWriter.Write(jsonRequest);
                    streamWriter.Close();
                }

                var httpResponse = httpRequest.GetResponse() as HttpWebResponse;
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();

                    if (configCustom.DebugLog)
                    {
                        Debug.WriteLine("Response: " + result);
                    }

                    var aiResponse = JsonConvert.DeserializeObject<AIResponse>(result);

                    CheckForErrors(aiResponse);

                    return aiResponse;
                }

            }
            catch (Exception e)
            {
                throw new AIServiceException(e);
            }
        }

        public AIResponse RequestIntentPut(AiIntent request, string intentId)
        {
            //request.Language = config.Language.code;
            //request.Timezone = TimeZone.CurrentTimeZone.StandardName;

            try
            {
                var httpRequest = (HttpWebRequest)WebRequest.Create(AddUrl(configCustom.RequestUrl,intentId));
                httpRequest.Method = "PUT";
                httpRequest.ContentType = "application/json; charset=utf-8";
                httpRequest.Accept = "application/json";

                httpRequest.Headers.Add("Authorization", "Bearer " + configCustom.ClientAccessToken);

                var jsonSettings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                };

                var jsonRequest = JsonConvert.SerializeObject(request, Formatting.None, jsonSettings);

                if (configCustom.DebugLog)
                {
                    Debug.WriteLine("Request: " + jsonRequest);
                }

                using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
                {
                    streamWriter.Write(jsonRequest);
                    streamWriter.Close();
                }

                var httpResponse = httpRequest.GetResponse() as HttpWebResponse;
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();

                    if (configCustom.DebugLog)
                    {
                        Debug.WriteLine("Response: " + result);
                    }

                    var aiResponse = JsonConvert.DeserializeObject<AIResponse>(result);

                    CheckForErrors(aiResponse);

                    return aiResponse;
                }

            }
            catch (Exception e)
            {
                throw new AIServiceException(e);
            }
        }

        public AIResponse RequestIntentDelete(string intentId)
        {
            //request.Language = config.Language.code;
            //request.Timezone = TimeZone.CurrentTimeZone.StandardName;

            try
            {
                var httpRequest = (HttpWebRequest)WebRequest.Create(AddUrl(configCustom.RequestUrl, intentId));
                httpRequest.Method = "DELETE";
                httpRequest.ContentType = "application/json; charset=utf-8";
                httpRequest.Accept = "application/json";

                httpRequest.Headers.Add("Authorization", "Bearer " + configCustom.ClientAccessToken);

                var jsonSettings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                };

                //var jsonRequest = JsonConvert.SerializeObject(request, Formatting.None, jsonSettings);

                //if (config.DebugLog)
                //{
                //    Debug.WriteLine("Request: " + jsonRequest);
                //}

                //using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
                //{
                //    streamWriter.Write(jsonRequest);
                //    streamWriter.Close();
                //}

                var httpResponse = httpRequest.GetResponse() as HttpWebResponse;
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();

                    if (configCustom.DebugLog)
                    {
                        Debug.WriteLine("Response: " + result);
                    }

                    var aiResponse = JsonConvert.DeserializeObject<AIResponse>(result);

                    CheckForErrors(aiResponse);

                    return aiResponse;
                }

            }
            catch (Exception e)
            {
                throw new AIServiceException(e);
            }
        }

        static void CheckForErrors(AIResponse aiResponse)
        {
            if (aiResponse == null)
            {
                throw new AIServiceException("API.AI response parsed as null. Check debug log for details.");
            }            
        }

        static void CheckForErrorsIntent(AiIntent aiResponse)
        {
            if (aiResponse == null)
            {
                throw new AIServiceException("API.AI response parsed as null. Check debug log for details.");
            }
            
        }

        static void CheckForErrorsIntentObject(AiIntentResponse aiResponse)
        {
            if (aiResponse == null)
            {
                throw new AIServiceException("API.AI response parsed as null. Check debug log for details.");
            }

        }
    }

    [JsonObject]
    public class AiIntent
    {
        public AiIntent()
        {

        }
        public AiIntent(string name)
        {
            Auto = true;
            Contexts = new List<AIContext>();
            Templates = new List<string>();
            Responses = new List<AiIntentResponseModel>();
            Responses.Add(new AiIntentResponseModel());
            //Responses.Add(JsonConvert.DeserializeObject("{\"resetContexts\": false,\"affectedContexts\": [],\"parameters\": [],\"messages\": [{\"type\": 0,\"speech\": []}]}"));
            Priority = 500000;
            WebhookUsed = false;
            FallbackIntent = false;
        }

        public AiIntent(string name, List<string> templates)
        {
            Name = name;
            Auto = true;
            Contexts = new List<AIContext>();
            if (templates==null) {
                Templates = new List<string>();
            } else
            {
                Templates = templates;
            }
            Responses = new List<AiIntentResponseModel>();
            Responses.Add(new AiIntentResponseModel());
            //Responses.Add(JsonConvert.DeserializeObject("{\"resetContexts\": false,\"affectedContexts\": [],\"parameters\": [],\"messages\": [{\"type\": 0,\"speech\": []}]}"));
            Priority = 500000;
            WebhookUsed = false;
            FallbackIntent = false;
        }
        
        public void SetMoreTemplates(string template)
        {
            if (Templates == null)            
            {
                Templates = new List<string>();
                
            }
            Templates.Add(template);
        }

        public void SetMoreTemplates(List<string> templates)
        {
            if (Templates == null)
            {
                Templates = new List<string>();

            }
            Templates.AddRange(templates);
        }

        public void RemoveTemplate(string template)
        {
            string removed = Templates.Find(q => q == template);
            Templates.Remove(removed);
        }

        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("auto")]
        public bool Auto { get; set; }

        [JsonProperty("contexts")]
        public List<ApiAiSDK.Model.AIContext> Contexts { get; set; }

        [JsonProperty("templates")]
        public List<string> Templates { get; set; }

        //[JsonProperty("userSays")]
        //public List<object> UserSays { get; set; }

        [JsonProperty("responses")]
        public List<AiIntentResponseModel> Responses { get; set; }

        [JsonProperty("priority")]
        public long Priority { get; set; }

        [JsonProperty("webhookUsed")]
        public bool WebhookUsed { get; set; }

        [JsonProperty("cortanaCommand")]
        public object CortanaCommand { get; set; }

        [JsonProperty("fallbackIntent")]
        public bool FallbackIntent { get; set; }

    }

    [JsonObject]
    public class AiIntentResponse
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("contextIn")]
        public List<ApiAiSDK.Model.AIContext> ContextIn { get; set; }
        [JsonProperty("contextOut")]
        public List<ApiAiSDK.Model.AIContext> ContextOut { get; set; }
        [JsonProperty("fallbackIntent")]
        public bool FallbackIntent { get; set; }

    }
    [JsonObject]
    public class AiIntentResponseModel
    {
        [JsonProperty("resetContexts")]
        public bool ResetContexts { get; set; }
        [JsonProperty("affectedContexts")]
        public List<AIContext> AffectedContexts { get; set; }
        [JsonProperty("messages")]
        public List<object> Messages { get; set; }
        [JsonProperty("parameters")]
        public List<object> Parameters { get; set; }

        public AiIntentResponseModel()
        {
            ResetContexts = false;
            AffectedContexts = new List<AIContext>();
            Parameters = new List<object>();
            Messages = new List<object>();
        }
    }
}