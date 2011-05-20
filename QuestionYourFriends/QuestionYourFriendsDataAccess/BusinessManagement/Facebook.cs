using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Facebook;
using Facebook.Web;
using Facebook.Web.Mvc;

namespace QuestionYourFriendsDataAccess.BusinessManagement
{
    public class Facebook
    {
        /// <summary>
        /// Info User
        /// http://developers.facebook.com/docs/reference/api/
        /// </summary>
        /// <returns></returns>
        public static dynamic getUserInfo()
        {
            var fb = new FacebookWebClient();
            dynamic result = fb.Get("me");
            return result;
        }

        /// <summary>
        /// List of friend is stored in result.data
        /// Name of friend in data.name
        /// Facebook id of friend in data.id
        /// </summary>
        /// <returns>Json Array</returns>
        public static dynamic getUserFriends()
        {
            var fb = new FacebookWebClient();
            dynamic result = fb.Get("/me/friends");
            return result;
        }

    }
}
