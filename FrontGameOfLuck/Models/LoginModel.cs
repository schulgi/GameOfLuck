using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web;

namespace FrontGameOfLuck.Models
{
    public class LoginModel
    {
        public static string GetBearerToken(string user, string password, ref UserToken userToken)
        {
            try
            {
                string res = string.Empty;

                var pairs = new List<KeyValuePair<string, string>>();
                pairs.Add(new KeyValuePair<string, string>("usuario", user));
                pairs.Add(new KeyValuePair<string, string>("clave", password));
                var content = new FormUrlEncodedContent(pairs);

                using (var client = new HttpClient())
                {

                    //  var response = client.PostAsync($"{Config.ReadSettings("Auth.AuthApiPath")}/Login", content).Result;
                    res = ""; // response.Content.ReadAsStringAsync().Result;
                }

                var jsonResponse = JsonConvert.DeserializeObject<JObject>(res);

                var payload = jsonResponse.SelectToken("payload");

                if (payload == null)
                {
                    var mensaje = jsonResponse.SelectToken("message").ToObject<string>();

                    return mensaje;
                }
                else
                {

                    var jtoken = jsonResponse.SelectToken("payload").SelectToken("token");
                    var usuarioToken = jsonResponse.SelectToken("payload").SelectToken("usuario").ToObject<UserToken>();
                    usuarioToken.Token = jtoken.SelectToken("accessToken").ToObject<string>();
                    usuarioToken.RefreshToken = jtoken.SelectToken("refreshToken").ToObject<string>();
                 //   Current.Session["Token"] = usuarioToken.Token;
                   
                    userToken = usuarioToken;

                    return "Success";
                }
            }
            catch (Exception ex)
            {
                return "";
            }
        }
    }
}
