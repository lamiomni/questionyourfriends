
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Runtime.Serialization.Json;


namespace QuestionYourFriends.Controllers
{
 

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
                string order_details = payload.order_details;
                string[] detail_tab = order_details.Split(new char[] { ',' });
                string[] buyer_tab = detail_tab[1].Split(new char[] { ':' });
                long buyer = long.Parse(buyer_tab[1]);

                string status = payload.status;
                if (status == "placed") {
                    string next_state = "settled";
                    content.Add("status",next_state);
                    QuestionYourFriendsDataAccess.User u = Models.User.Get(buyer);
                    if (Models.Transac.Earning(u, 10000))
                        u.credit_amount += 10000;
                    Models.User.Update(u);
                }
                content.Add("order_id",order_id);
            }else if (func == "payments_get_items"){
                Dictionary<string, dynamic> item = new Dictionary<string, dynamic>();
                item.Add("title", "add credit");
                item.Add("price", 1);
              
                item.Add("description", "Add some credit to qyf");
                item.Add("product_url", "https://fbcdn-photos-a.akamaihd.net/photos-ak-snc1/v27562/215/131910193550231/app_1_131910193550231_8016.gif");
                item.Add("image_url", "https://fbcdn-photos-a.akamaihd.net/photos-ak-snc1/v27562/215/131910193550231/app_1_131910193550231_8016.gif");
                content.Add("0", item);
                string order_info = payload.order_info;
            }else {

            }
            data.Add("content", content);
            data.Add("method",func);
            JsonResult ce = Json(data);
            
            return this.Json(data);
        }

        

    }
}
