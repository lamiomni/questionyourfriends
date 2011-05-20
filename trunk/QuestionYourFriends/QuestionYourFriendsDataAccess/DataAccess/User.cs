using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuestionYourFriendsDataAccess.DataAccess
{
    public class User
    {
        public static bool CreateUser(QuestionYourFriendsDataAccess.User user)
        {
            try
            {

                QuestionYourFriendsDataAccess.QuestionYourFriendsEntities model = new QuestionYourFriendsDataAccess.QuestionYourFriendsEntities();
                model.AddToUsers(user);
                model.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool DeleteUser(long id)
        {
            try
            {
                QuestionYourFriendsDataAccess.QuestionYourFriendsEntities model = new QuestionYourFriendsDataAccess.QuestionYourFriendsEntities();
                model.DeleteObject(model.Users.Where(x => x.id == id).FirstOrDefault());
                model.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool UpdateUser(QuestionYourFriendsDataAccess.User user)
        {
            try
            {
                QuestionYourFriendsDataAccess.QuestionYourFriendsEntities model = new QuestionYourFriendsDataAccess.QuestionYourFriendsEntities();
                QuestionYourFriendsDataAccess.User userFound = model.Users.Where(x => x.id == user.id).FirstOrDefault();
                if (userFound != null)
                {
                    userFound.fid = user.fid;
                    userFound.credit_amount = user.credit_amount;
                    userFound.activated = user.activated;
             
                    model.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public static QuestionYourFriendsDataAccess.User GetUser(long id)
        {
            try
            {
                QuestionYourFriendsDataAccess.QuestionYourFriendsEntities model = new QuestionYourFriendsDataAccess.QuestionYourFriendsEntities();
                return model.Users.Where(x => x.id == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<QuestionYourFriendsDataAccess.User> GetListUser()
        {
            try
            {
                QuestionYourFriendsDataAccess.QuestionYourFriendsEntities model = new QuestionYourFriendsDataAccess.QuestionYourFriendsEntities();
                return model.Users.ToList();
            }
            catch (Exception ex)
            {
                return new List<QuestionYourFriendsDataAccess.User>();
            }
        }

    }
}
