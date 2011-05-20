using System;
using System.Collections.Generic;
using System.Linq;

namespace QuestionYourFriendsDataAccess.DataAccess
{
    public class Question
    {
        public static bool CreateQuestion(int id_owner, int id_receiver, string text, int anom_price, int private_price, DateTime date_pub)
        {
            try
            {
                var model = new QuestionYourFriendsEntities();
                QuestionYourFriendsDataAccess.Question question = model.Questions.CreateObject();
                question.id_owner = id_owner;
                question.id_receiver = id_receiver;
                question.text = text;
                question.answer = null;
                question.anom_price = anom_price;
                question.private_price = private_price;
                question.undesirable = false;
                question.date_pub = date_pub;
                question.date_answer = null;
                question.deleted = false;
                model.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool CreateQuestion(QuestionYourFriendsDataAccess.Question question)
        {
            try
            {

                var model = new QuestionYourFriendsEntities();
                model.Questions.AddObject(question);
                model.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool DeleteQuestion(long id)
        {
            try
            {
                var model = new QuestionYourFriendsEntities();
                model.DeleteObject(model.Questions.Where(x => x.id == id).FirstOrDefault());
                model.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool UpdateQuestion(QuestionYourFriendsDataAccess.Question question)
        {
            try
            {
                var model = new QuestionYourFriendsEntities();
                QuestionYourFriendsDataAccess.Question questionFound = model.Questions.Where(x => x.id == question.id).FirstOrDefault();
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

        public static QuestionYourFriendsDataAccess.Question GetQuestion(long id)
        {
            try
            {
                var model = new QuestionYourFriendsEntities();
                return model.Questions.Where(x => x.id == id).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static List<QuestionYourFriendsDataAccess.Question> GetListQuestion()
        {
            try
            {
                var model = new QuestionYourFriendsEntities();
                return model.Questions.ToList();
            }
            catch (Exception)
            {
                return new List<QuestionYourFriendsDataAccess.Question>();
            }
        }
    }
}
