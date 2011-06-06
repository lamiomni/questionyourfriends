using System.Collections.Generic;
using System.Linq;
using Facebook.Web;

namespace QuestionYourFriends.Models
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
            var res = new Dictionary<long, object>();

            foreach (dynamic friend in friends.data)
                res.Add(long.Parse(friend.id), friend);
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

        /// <summary>
        /// Conversion from Facebook IDs to our IDs
        /// </summary>
        /// <returns></returns>
        public static dynamic GetUidFromFid(long[] fids)
        {
            var friends = User.GetUsersFromFids(fids);

            return friends.ToDictionary(friend => friend.fid, friend => friend.id);
        }


        public static string GetFriendName(long fid)
        {
            //return "";
            var fb = new FacebookWebClient();
            dynamic result = fb.Get("/" + fid);
            return (result.last_name + " " + result.first_name);
        }

        public static void publish(long wall_id, string message, string picture = null, string link = null, string name=null, string caption=null, string description=null, string source=null)
        {
            var fb = new FacebookWebClient();
            var dic = new Dictionary<string,string>();
            dic.Add("message",message);
            if (picture != null)
             dic.Add("picture",picture);
            if (link != null)
            dic.Add("link",link);
            if (name != null)
            dic.Add("name",name);
            if (caption != null)
            dic.Add("caption",caption);
            if (description != null)
            dic.Add("description",description);
            if (source != null)
            dic.Add("source",source);
            
            fb.Post("/" + wall_id + "/feed",dic);
        }
    }
}