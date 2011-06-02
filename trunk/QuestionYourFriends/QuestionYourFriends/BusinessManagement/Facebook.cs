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
        /// List of friend is stored in result.data
        /// Name of friend in data.name
        /// Facebook id of friend in data.id
        /// </summary>
        /// <returns>Json Array</returns>
        public static dynamic GetUserFriends()
        {
            var fb = new FacebookWebClient();
            dynamic result = fb.Get("/me/friends");
            return result;
        }
    }
}