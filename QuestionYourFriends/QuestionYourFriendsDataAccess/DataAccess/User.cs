using System;
using System.Collections.Generic;
using System.Linq;

namespace QuestionYourFriendsDataAccess.DataAccess
{
    public class User
    {
        public static bool CreateUser(QuestionYourFriendsDataAccess.User user)
        {
            try
            {
                var model = new QuestionYourFriendsEntities();
                model.Users.AddObject(user);
                model.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool DeleteUser(long id)
        {
            try
            {
                var model = new QuestionYourFriendsEntities();
                model.DeleteObject(model.Users.Where(x => x.id == id).FirstOrDefault());
                model.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool UpdateUser(QuestionYourFriendsDataAccess.User user)
        {
            try
            {
                var model = new QuestionYourFriendsEntities();
                QuestionYourFriendsDataAccess.User userFound = model.Users.Where(x => x.id == user.id).FirstOrDefault();
                if (userFound != null)
                {
                    userFound.fid = user.fid;
                    userFound.credit_amount = user.credit_amount;
                    userFound.activated = user.activated;
             
                    model.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public static QuestionYourFriendsDataAccess.User GetUser(long id)
        {
            try
            {
                var model = new QuestionYourFriendsEntities();
                return model.Users.Where(x => x.id == id).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static List<QuestionYourFriendsDataAccess.User> GetListUser()
        {
            try
            {
                var model = new QuestionYourFriendsEntities();
                return model.Users.ToList();
            }
            catch (Exception)
            {
                return new List<QuestionYourFriendsDataAccess.User>();
            }
        }

    }
}
