using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace QuestionYourFriendsDataAccess.DataAccess
{
    public static class Transac
    {
        #region CRUD methods

        public static bool Create(QuestionYourFriendsEntities qyfEntities, int amount, int userId,
                                         TransacType type, int questionId)
        {
            try
            {
                QuestionYourFriendsDataAccess.Transac transac = qyfEntities.Transacs.CreateObject();
                transac.amount = amount;
                transac.userId = userId;
                transac.SetTransacStatus(TransacStatus.Ok);
                transac.SetTransacType(type);
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

        public static bool Create(QuestionYourFriendsEntities qyfEntities, QuestionYourFriendsDataAccess.Transac transac)
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

        public static bool Delete(QuestionYourFriendsEntities qyfEntities, long id)
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

        public static bool Delete(QuestionYourFriendsEntities qyfEntities, QuestionYourFriendsDataAccess.Transac t)
        {
            try
            {
                qyfEntities.DeleteObject(t);
                qyfEntities.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return false;
            }
        }

        public static bool Update(QuestionYourFriendsEntities qyfEntities, QuestionYourFriendsDataAccess.Transac transac)
        {
            try
            {
                QuestionYourFriendsDataAccess.Transac transacFound =
                    qyfEntities.Transacs.Where(x => x.id == transac.id).FirstOrDefault();
                if (transacFound != null)
                {
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

        public static QuestionYourFriendsDataAccess.Transac Get(QuestionYourFriendsEntities qyfEntities, long id)
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

        public static List<QuestionYourFriendsDataAccess.Transac> GetList(QuestionYourFriendsEntities qyfEntities)
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

        #endregion
    }
}