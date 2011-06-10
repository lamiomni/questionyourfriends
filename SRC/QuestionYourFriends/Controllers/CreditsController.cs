
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace QuestionYourFriends.Controllers
{
    public class CreditsController : Controller
    {
        //
        // GET: /Credits/

        public ActionResult Index()
        {


            return View();
        }

        public JsonResult CallBack()
        {
            dynamic request = Facebook.FacebookSignedRequest.Parse(Facebook.FacebookApplication.Current.AppSecret, Request.Params.Get("signed_request"));
            //List<dynamic> data = new List<dynamic>();
            //List<dynamic> content = new List<dynamic>();
            ////data.Add(content);
            //dynamic payload = request.credits;
            //string func = Request.Params.Get("method");
            //string order_id = payload.order_id;
            //if (func =="payments_status_update") {
            //    string status = payload.status;
            //     if (status == "placed") {
            //        string next_state = "settled";
            //        content.Add(next_state);
            //      }
            //}

            string fefe = Json(request);
            return this.Json(request); ;
        }

    }
}
