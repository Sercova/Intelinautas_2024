using System.IO;
using System.Net;
using Newtonsoft.Json;
 //Nini.Config;



namespace Comunications.AWS
{
    public class ServiceReader
    {
        public string readAWS()
        {
          //  var url = $"https://zc5a6vd0ql.execute-api.us-east-1.amazonaws.com/Prod/hello/";
            var url = $"https://mjmvump2e5.execute-api.us-east-1.amazonaws.com/develop/challenge";
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "application/json";
            request.Accept = "application/json";
            
            //request.Headers.Add("secret",readPropertie("secret"));
            request.Headers.Add("secret","465934bba207fdd8c001701c846701fdb52ad76c99b582acceda3dcaf8493af2");
            string responderBody = null;
            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream strReader = response.GetResponseStream())
                    {
                        if (strReader == null) return responderBody;
                        using (StreamReader objReader = new StreamReader(strReader))
                        {
                            responderBody =  objReader.ReadToEnd();
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                // Handle error
                return null;
            }

            return responderBody;
        }
        
       /* public string readPropertie(string propertieName)
        {
            IniConfigSource configSource = new IniConfigSource("app.properties");
            return configSource.Configs["api"].Get(propertieName);
        }*/
    }
    
  
}