﻿using System.Collections.Generic;
using System.Web.Mvc;
using Facebook;

namespace QuestionYourFriends.Controllers
{
    /// <summary>
    /// Credit management
    /// </summary>
    public class CreditsController : Controller
    {
        /// <summary>
        /// GET: /Credits/
        /// </summary>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// GET: /Credits/CallBack
        /// </summary>
        public JsonResult CallBack()
        {
            FacebookSignedRequest request = FacebookSignedRequest.Parse(FacebookApplication.Current.AppSecret, Request.Params.Get("signed_request"));
            var requestdata = (JsonObject)request.Data;
            
            var data = new Dictionary<string, dynamic>();
            var content = new Dictionary<string, dynamic>();

            var payload = (JsonObject)requestdata["credits"];
            string func = Request.Params.Get("method");
            long order_id = (long)payload["order_id"];
            
            if (func == "payments_status_update")
            {
                string order_details = payload["order_details"].ToString();
                string[] detail_tab = order_details.Split(new [] { ',' });
                string[] buyer_tab = detail_tab[1].Split(new [] { ':' });
                long buyer = long.Parse(buyer_tab[1]);

                string status = payload["status"].ToString();
                if (status == "placed") {
                    string next_state = "settled";
                    content.Add("status",next_state);
                    QuestionYourFriendsDataAccess.User u = Models.User.Get(buyer);
                    if (Models.Transac.Earning(u, 10000))
                        u.credit_amount += 10000;
                    Models.User.Update(u);
                }
                content.Add("order_id",order_id);
            }
            else if (func == "payments_get_items")
            {
                var item = new Dictionary<string, dynamic>();
                item.Add("title", "add credit");
                item.Add("price", 1);
              
                item.Add("description", "Add some credit to qyf");
                item.Add("product_url", "https://fbcdn-photos-a.akamaihd.net/photos-ak-snc1/v27562/215/131910193550231/app_1_131910193550231_8016.gif");
                item.Add("image_url", "https://fbcdn-photos-a.akamaihd.net/photos-ak-snc1/v27562/215/131910193550231/app_1_131910193550231_8016.gif");
                content.Add("0", item);
            }
            data.Add("content", content);
            data.Add("method",func);
            
            return Json(data);
        }

        

    }
}
