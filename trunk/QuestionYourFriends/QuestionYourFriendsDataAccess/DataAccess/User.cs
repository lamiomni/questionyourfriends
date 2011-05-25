using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace QuestionYourFriendsDataAccess.DataAccess
{
    public static class User
    {
        public static bool Create(QuestionYourFriendsEntities qyfEntities, long fid)
        {
            try
            {
                QuestionYourFriendsDataAccess.User user = qyfEntities.Users.CreateObject();
                user.fid = fid;
                user.activated = true;
                user.credit_amount = 0;
                qyfEntities.Users.AddObject(user);
                qyfEntities.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return false;
            }
        }

        public static bool Create(QuestionYourFriendsEntities qyfEntities, QuestionYourFriendsDataAccess.User user)
        {
            try
            {
                qyfEntities.Users.AddObject(user);
                qyfEntities.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return false;
            }
        }

        public static bool Delete(QuestionYourFriendsEntities qyfEntities, long fid)
        {
            try
            {
                qyfEntities.DeleteObject(qyfEntities.Users.Where(x => x.fid == fid).FirstOrDefault());
                qyfEntities.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return false;
            }
        }

        public static bool Delete(QuestionYourFriendsEntities qyfEntities, QuestionYourFriendsDataAccess.User u)
        {
            try
            {
                qyfEntities.DeleteObject(u);
                qyfEntities.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return false;
            }
        }

        public static bool Update(QuestionYourFriendsEntities qyfEntities, QuestionYourFriendsDataAccess.User user)
        {
            try
            {
                QuestionYourFriendsDataAccess.User userFound =
                    qyfEntities.Users.Where(x => x.id == user.id).FirstOrDefault();
                if (userFound != null)
                {
                    userFound.fid = user.fid;
                    userFound.credit_amount = user.credit_amount;
                    userFound.activated = user.activated;

                    qyfEntities.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return false;
            }
        }

        public static QuestionYourFriendsDataAccess.User Get(QuestionYourFriendsEntities qyfEntities, long fid)
        {
            try
            {
                return qyfEntities.Users.Where(x => x.fid == fid).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return null;
            }
        }

        public static List<QuestionYourFriendsDataAccess.User> GetList(QuestionYourFriendsEntities qyfEntities)
        {
            try
            {
                return qyfEntities.Users.ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return new List<QuestionYourFriendsDataAccess.User>();
            }
        }
    }
}