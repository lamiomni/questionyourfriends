using System;
using System.Web.Security;
using System.Web.UI;

namespace QuestionYourFriendsBackEnd
{
    public partial class Login : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(Request.Params["logout"]))
                FormsAuthentication.SignOut();
        }
    }
}