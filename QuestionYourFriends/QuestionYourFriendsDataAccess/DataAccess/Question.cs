using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace QuestionYourFriendsDataAccess.DataAccess
{
    public static class Question
    {
        public static bool CreateQuestion(QuestionYourFriendsEntities qyfEntities, int idOwner, int idReceiver,
                                          string text, int anomPrice, int privatePrice, DateTime datePub)
        {
            try
            {
                QuestionYourFriendsDataAccess.Question question = qyfEntities.Questions.CreateObject();
                question.id_owner = idOwner;
                question.id_receiver = idReceiver;
                question.text = text;
                question.answer = null;
                question.anom_price = anomPrice;
                question.private_price = privatePrice;
                question.undesirable = false;
                question.date_pub = datePub;
                question.date_answer = null;
                question.deleted = false;
                qyfEntities.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return false;
            }
        }

        public static bool CreateQuestion(QuestionYourFriendsEntities qyfEntities, QuestionYourFriendsDataAccess.Question question)
        {
            try
            {
                qyfEntities.Questions.AddObject(question);
                qyfEntities.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return false;
            }
        }

        public static bool DeleteQuestion(QuestionYourFriendsEntities qyfEntities, long id)
        {
            try
            {
                qyfEntities.DeleteObject(qyfEntities.Questions.Where(x => x.id == id).FirstOrDefault());
                qyfEntities.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return false;
            }
        }

        public static bool UpdateQuestion(QuestionYourFriendsEntities qyfEntities, QuestionYourFriendsDataAccess.Question question)
        {
            try
            {
                QuestionYourFriendsDataAccess.Question questionFound = qyfEntities.Questions.Where(x => x.id == question.id).FirstOrDefault();
                if (questionFound != null)
                {
                    questionFound.id_owner = question.id_owner;
                    questionFound.id_receiver = question.id_receiver;
                    questionFound.text = question.text;
                    questionFound.answer = question.answer;
                    questionFound.anom_price = question.anom_price;
                    questionFound.private_price = question.private_price;
                    questionFound.undesirable = question.undesirable;
                    questionFound.date_pub = question.date_pub;
                    questionFound.date_answer = question.date_answer;
                    questionFound.deleted = question.deleted;

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

        public static QuestionYourFriendsDataAccess.Question GetQuestion(QuestionYourFriendsEntities qyfEntities, long id)
        {
            try
            {
                return qyfEntities.Questions.Where(x => x.id == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return null;
            }
        }

        public static List<QuestionYourFriendsDataAccess.Question> GetListQuestion(QuestionYourFriendsEntities qyfEntities)
        {
            try
            {
                return qyfEntities.Questions.ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return new List<QuestionYourFriendsDataAccess.Question>();
            }
        }
    }
}
