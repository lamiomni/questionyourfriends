﻿using System.Collections.Generic;

namespace QuestionYourFriendsDataAccess.BusinessManagement
{
    public class Transac
    {
        public static bool CreateTransac(QuestionYourFriendsDataAccess.Transac transac)
        {
            return DataAccess.Transac.CreateTransac(transac);
        }

        public static bool DeleteTransac(long id)
        {
            return DataAccess.Transac.DeleteTransac(id);
        }

        public static bool UpdateTransac(QuestionYourFriendsDataAccess.Transac transac)
        {
            return DataAccess.Transac.UpdateTransac(transac);
        }

        public static QuestionYourFriendsDataAccess.Transac GetTransac(long id)
        {
            return DataAccess.Transac.GetTransac(id);
        }

        public static List<QuestionYourFriendsDataAccess.Transac> GetListTransac()
        {
            return DataAccess.Transac.GetListTransac();
        }
    }
}