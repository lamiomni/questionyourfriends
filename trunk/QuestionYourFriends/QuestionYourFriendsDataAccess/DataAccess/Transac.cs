using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuestionYourFriendsDataAccess.DataAccess
{
    class Transac
    {
        public static bool CreateTransac(QuestionYourFriendsDataAccess.Transac transac)
        {
            try
            {

                QuestionYourFriendsDataAccess.QuestionYourFriendsEntities model = new QuestionYourFriendsDataAccess.QuestionYourFriendsEntities();
                model.AddToTransacs(transac);
                model.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool DeleteTransac(long id)
        {
            try
            {
                QuestionYourFriendsDataAccess.QuestionYourFriendsEntities model = new QuestionYourFriendsDataAccess.QuestionYourFriendsEntities();
                model.DeleteObject(model.Transacs.Where(x => x.id == id).FirstOrDefault());
                model.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool UpdateTransac(QuestionYourFriendsDataAccess.Transac transac)
        {
            try
            {
                QuestionYourFriendsDataAccess.QuestionYourFriendsEntities model = new QuestionYourFriendsDataAccess.QuestionYourFriendsEntities();
                QuestionYourFriendsDataAccess.Transac transacFound = model.Transacs.Where(x => x.id == transac.id).FirstOrDefault();
                if (transacFound != null)
                {
                    transacFound.fid = transac.fid;
                    transacFound.amount = transac.amount;
                    transacFound.status = transac.status;
                    transacFound.userId = transac.userId;
             
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

        public static QuestionYourFriendsDataAccess.Transac GetTransac(long id)
        {
            try
            {
                QuestionYourFriendsDataAccess.QuestionYourFriendsEntities model = new QuestionYourFriendsDataAccess.QuestionYourFriendsEntities();
                return model.Transacs.Where(x => x.id == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<QuestionYourFriendsDataAccess.Transac> GetListTransac()
        {
            try
            {
                QuestionYourFriendsDataAccess.QuestionYourFriendsEntities model = new QuestionYourFriendsDataAccess.QuestionYourFriendsEntities();
                return model.Transacs.ToList();
            }
            catch (Exception ex)
            {
                return new List<QuestionYourFriendsDataAccess.Transac>();
            }
        }
    }
}
