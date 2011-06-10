
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Runtime.Serialization.Json;


namespace QuestionYourFriends.Controllers
{
    [Serializable]
    public class RequestData
    {
        public string algorithm;
        public RequestCredits Address;
    }

    [Serializable]
    public class RequestCredits
    {
        public long buyer;
        public long receiver;
        public long order_id;
        public string order_info;
        public int test_mode;
        public long expires;
        public long issued_at;
        public string oauth_token;
        public RequestUser user;
    }

    [Serializable]
    public class RequestUser
    {
        public string country;
        public string locale;
        public string age;
        public string user_id;
    }

    public class CreditsController : Controller
    {
        //
        // GET: /Credits/

        public ActionResult Index()
        {

            QuestionYourFriends.Models.Facebook.GetUserFriends();
            return View();
        }

        public JsonResult CallBack()
        {

            Facebook.FacebookSignedRequest request = Facebook.FacebookSignedRequest.Parse(Facebook.FacebookApplication.Current.AppSecret, Request.Params.Get("signed_request"));
            dynamic requestdata = request.Data;
          
            Dictionary<string, dynamic> data = new Dictionary<string, dynamic>();
            Dictionary<string, dynamic> content = new Dictionary<string, dynamic>();

            dynamic payload = requestdata.credits;
            string func = Request.Params.Get("method");
            long order_id = payload.order_id;
            if (func == "payments_status_update")
            {
                string status = payload.status;
                if (status == "placed") {
                    string next_state = "settled";
                    content.Add("status",next_state);
                }
                content.Add("order_id",order_id);
            }else if (func == "payments_get_items"){
                Dictionary<string, dynamic> item = new Dictionary<string, dynamic>();
                item.Add("title", "add credit");
                item.Add("price", 1);
              
                item.Add("description", "Add some credit to qyf");
                item.Add("product_url", "http://www.facebook.com/images/gifts/21.png");
                item.Add("image_url", "http://www.facebook.com/images/gifts/21.png");
                content.Add("0", item);
                string order_info = payload.order_info;
            }else {

            }
            data.Add("content", content);
            data.Add("method",func);
            JsonResult ce = Json(data);
            
            return this.Json(data);
        }

        public static Dictionary<string, object> ParseJsonToDictionary(string json)
        {
            var d = new Dictionary<string, object>();

            if (json.StartsWith("{"))
            {
                json = json.Remove(0, 1);
                if (json.EndsWith("}"))
                    json = json.Substring(0, json.Length - 1);
            }
            json.Trim();

            // Parse out Object Properties from JSON
            while (json.Length > 0)
            {
                var beginProp = json.Substring(0, json.IndexOf(':'));
                json = json.Substring(beginProp.Length);

                var indexOfComma = json.IndexOf(',');
                string endProp;
                if (indexOfComma > -1)
                {
                    endProp = json.Substring(0, indexOfComma);
                    json = json.Substring(endProp.Length);
                }
                else
                {
                    endProp = json;
                    json = string.Empty;
                }

                var curlyIndex = endProp.IndexOf('{');
                if (curlyIndex > -1)
                {
                    var curlyCount = 1;
                    while (endProp.Substring(curlyIndex + 1).IndexOf("{") > -1)
                    {
                        curlyCount++;
                        curlyIndex = endProp.Substring(curlyIndex + 1).IndexOf("{");
                    }
                    while (curlyCount > 0)
                    {
                        endProp += json.Substring(0, json.IndexOf('}') + 1);
                        json = json.Remove(0, json.IndexOf('}') + 1);
                        curlyCount--;
                    }
                }

                json = json.Trim();
                if (json.StartsWith(","))
                    json = json.Remove(0, 1);
                json.Trim();


                // Individual Property (Name/Value Pair) Is Isolated
                var s = (beginProp + endProp).Trim();


                // Now parse the name/value pair out and put into Dictionary
                var name = s.Substring(0, s.IndexOf(":")).Trim();
                var value = s.Substring(name.Length + 1).Trim();

                if (name.StartsWith("\"") && name.EndsWith("\""))
                {
                    name = name.Substring(1, name.Length - 2);
                }

                double valueNumberCheck;
                if (value.StartsWith("\"") && value.StartsWith("\""))
                {
                    // String Value
                    d.Add(name, value.Substring(1, value.Length - 2));
                }
                else if (value.StartsWith("{") && value.EndsWith("}"))
                {
                    // JSON Value
                    d.Add(name, ParseJsonToDictionary(value));
                }
                else if (double.TryParse(value, out valueNumberCheck))
                {
                    // Numeric Value
                    d.Add(name, valueNumberCheck);
                }
                else
                    d.Add(name, value);
            }
            return d;
        }

    }
}
