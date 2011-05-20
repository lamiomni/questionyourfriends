using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace QuestionYourFriendsDataAccess.DataAccess
{
    public static class Transac
    {
        public static bool CreateTransac(QuestionYourFriendsEntities qyfEntities, int fid, int amount, int userId,
                                         string type, int questionId)
        {
            try
            {
                QuestionYourFriendsDataAccess.Transac transac = qyfEntities.Transacs.CreateObject();
                transac.fid = fid;
                transac.amount = amount;
                transac.status = "Ok";
                transac.userId = userId;
                transac.type = type;
                transac.questionId = questionId;
                qyfEntities.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return false;
            }
        }

        public static bool CreateTransac(QuestionYourFriendsEntities qyfEntities, QuestionYourFriendsDataAccess.Transac transac)
        {
            try
            {
                qyfEntities.Transacs.AddObject(transac);
                qyfEntities.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return false;
            }
        }

        public static bool DeleteTransac(QuestionYourFriendsEntities qyfEntities, long id)
        {
            try
            {
                qyfEntities.DeleteObject(qyfEntities.Transacs.Where(x => x.id == id).FirstOrDefault());
                qyfEntities.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return false;
            }
        }

        public static bool UpdateTransac(QuestionYourFriendsEntities qyfEntities, QuestionYourFriendsDataAccess.Transac transac)
        {
            try
            {
                QuestionYourFriendsDataAccess.Transac transacFound = qyfEntities.Transacs.Where(x => x.id == transac.id).FirstOrDefault();
                if (transacFound != null)
                {
                    transacFound.fid = transac.fid;
                    transacFound.amount = transac.amount;
                    transacFound.status = transac.status;
                    transacFound.userId = transac.userId;
                    transacFound.type = transac.type;
                    transacFound.questionId = transac.questionId;

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

        public static QuestionYourFriendsDataAccess.Transac GetTransac(QuestionYourFriendsEntities qyfEntities, long id)
        {
            try
            {
                return qyfEntities.Transacs.Where(x => x.id == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return null;
            }
        }

        public static List<QuestionYourFriendsDataAccess.Transac> GetListTransac(QuestionYourFriendsEntities qyfEntities)
        {
            try
            {
                return qyfEntities.Transacs.ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return new List<QuestionYourFriendsDataAccess.Transac>();
            }
        }
    }
}
