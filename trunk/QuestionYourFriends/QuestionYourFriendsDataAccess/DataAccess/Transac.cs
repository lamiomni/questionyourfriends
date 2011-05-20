using System;
using System.Collections.Generic;
using System.Linq;

namespace QuestionYourFriendsDataAccess.DataAccess
{
    public class Transac
    {
        public static bool CreateTransac(QuestionYourFriendsDataAccess.Transac transac)
        {
            try
            {

                var model = new QuestionYourFriendsEntities();
                model.AddToTransacs(transac);
                model.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool DeleteTransac(long id)
        {
            try
            {
                var model = new QuestionYourFriendsEntities();
                model.DeleteObject(model.Transacs.Where(x => x.id == id).FirstOrDefault());
                model.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool UpdateTransac(QuestionYourFriendsDataAccess.Transac transac)
        {
            try
            {
                var model = new QuestionYourFriendsEntities();
                QuestionYourFriendsDataAccess.Transac transacFound = model.Transacs.Where(x => x.id == transac.id).FirstOrDefault();
                if (transacFound != null)
                {
                    transacFound.fid = transac.fid;
                    transacFound.amount = transac.amount;
                    transacFound.status = transac.status;
                    transacFound.userId = transac.userId;
                    transacFound.type = transac.type;
                    transacFound.questionId = transac.questionId;
             
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

        public static QuestionYourFriendsDataAccess.Transac GetTransac(long id)
        {
            try
            {
                var model = new QuestionYourFriendsEntities();
                return model.Transacs.Where(x => x.id == id).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static List<QuestionYourFriendsDataAccess.Transac> GetListTransac()
        {
            try
            {
                var model = new QuestionYourFriendsEntities();
                return model.Transacs.ToList();
            }
            catch (Exception)
            {
                return new List<QuestionYourFriendsDataAccess.Transac>();
            }
        }
    }
}
