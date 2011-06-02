using System.Collections.Generic;
using Facebook.Web;

namespace QuestionYourFriends.BusinessManagement
{
    /// <summary>
    /// Facebook Model
    /// </summary>
    public static class Facebook
    {
        /// <summary>
        /// Info User
        /// http://developers.facebook.com/docs/reference/api/
        /// </summary>
        /// <returns>A Json Array</returns>
        public static dynamic GetUserInfo()
        {
            var fb = new FacebookWebClient();
            dynamic result = fb.Get("me");
            return result;
        }

        /// <summary>
        /// List of friends is stored in result.data
        /// Name of friends in data.name
        /// Facebook id of friend in data.id
        /// </summary>
        /// <returns>Json Array</returns>
        public static dynamic GetUserFriends()
        {
            var fb = new FacebookWebClient();
            dynamic result = fb.Get("/me/friends");
            return result;
        }

        /// <summary>
        /// List of friends stored into a dictionary
        /// </summary>
        /// <returns></returns>
        public static dynamic GetUserFriendsDictionary()
        {
            dynamic friends = GetUserFriends();
            var res = new Dictionary<long, dynamic>();

            foreach (dynamic friend in friends.data)
            {
                res.Add(long.Parse(friend.id), friend);
            }
            return res;
        }

        /// <summary>
        /// Get friend infos
        /// </summary>
        /// <param name="fid">Facebook Id of the requested user</param>
        /// <returns>A Json Array</returns>
        public static dynamic GetFriendInfo(long fid)
        {
            var fb = new FacebookWebClient();
            dynamic result = fb.Get("/" + fid);
            return result;
        }
    }
}