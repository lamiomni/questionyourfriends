using System.Collections.Generic;
using System.Web.Mvc;

namespace QuestionYourFriends.Controllers
{
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
        /// <returns></returns>
        public JsonResult CallBack()
        {
            Facebook.FacebookSignedRequest request = Facebook.FacebookSignedRequest.Parse(Facebook.FacebookApplication.Current.AppSecret, Request.Params.Get("signed_request"));
            dynamic requestdata = request.Data;
            
            var data = new Dictionary<string, dynamic>();
            var content = new Dictionary<string, dynamic>();

            dynamic payload = requestdata.credits;
            string func = Request.Params.Get("method");
            long order_id = payload.order_id;

            if (func == "payments_status_update")
            {
                string order_details = payload.order_details;
                string[] detail_tab = order_details.Split(new [] { ',' });
                string[] buyer_tab = detail_tab[1].Split(new [] { ':' });
                long buyer = long.Parse(buyer_tab[1]);

                string status = payload.status;
                if (status == "placed") {
                    content.Add("status", "settled");
                    QuestionYourFriendsDataAccess.User u = Models.User.Get(buyer);
                    if (Models.Transac.Earning(u, 10000))
                        u.credit_amount += 10000;
                    Models.User.Update(u);
                }
                content.Add("order_id",order_id);
            }
            else if (func == "payments_get_items")
            {
                var item = new Dictionary<string, dynamic>
                               {
                                   {"title", "Add credits"},
                                   {"price", 1},
                                   {"description", "Add 10000 credits to Question Your Friends"},
                                   {
                                       "product_url",
                                       "https://fbcdn-photos-a.akamaihd.net/photos-ak-snc1/v27562/215/131910193550231/app_1_131910193550231_8016.gif"
                                   },
                                   {
                                       "image_url",
                                       "https://fbcdn-photos-a.akamaihd.net/photos-ak-snc1/v27562/215/131910193550231/app_1_131910193550231_8016.gif"
                                   }
                               };

                content.Add("0", item);
            }
            data.Add("content", content);
            data.Add("method",func);
            return Json(data);
        }
    }
}
