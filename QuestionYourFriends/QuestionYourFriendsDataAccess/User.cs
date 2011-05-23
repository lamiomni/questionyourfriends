//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Collections.Specialized;

namespace QuestionYourFriendsDataAccess
{
    public class User
    {
        #region Primitive Properties

        public virtual int id { get; set; }

        public virtual long fid { get; set; }

        public virtual int credit_amount { get; set; }

        public virtual bool activated { get; set; }

        #endregion

        #region Navigation Properties

        private ICollection<Question> _myQuestions;

        private ICollection<Question> _questionsToMe;

        private ICollection<Transac> _transacs;

        public virtual ICollection<Question> MyQuestions
        {
            get
            {
                if (_myQuestions == null)
                {
                    var newCollection = new FixupCollection<Question>();
                    newCollection.CollectionChanged += FixupMyQuestions;
                    _myQuestions = newCollection;
                }
                return _myQuestions;
            }
            set
            {
                if (!ReferenceEquals(_myQuestions, value))
                {
                    var previousValue = _myQuestions as FixupCollection<Question>;
                    if (previousValue != null)
                    {
                        previousValue.CollectionChanged -= FixupMyQuestions;
                    }
                    _myQuestions = value;
                    var newValue = value as FixupCollection<Question>;
                    if (newValue != null)
                    {
                        newValue.CollectionChanged += FixupMyQuestions;
                    }
                }
            }
        }

        public virtual ICollection<Question> QuestionsToMe
        {
            get
            {
                if (_questionsToMe == null)
                {
                    var newCollection = new FixupCollection<Question>();
                    newCollection.CollectionChanged += FixupQuestionsToMe;
                    _questionsToMe = newCollection;
                }
                return _questionsToMe;
            }
            set
            {
                if (!ReferenceEquals(_questionsToMe, value))
                {
                    var previousValue = _questionsToMe as FixupCollection<Question>;
                    if (previousValue != null)
                    {
                        previousValue.CollectionChanged -= FixupQuestionsToMe;
                    }
                    _questionsToMe = value;
                    var newValue = value as FixupCollection<Question>;
                    if (newValue != null)
                    {
                        newValue.CollectionChanged += FixupQuestionsToMe;
                    }
                }
            }
        }

        public virtual ICollection<Transac> Transacs
        {
            get
            {
                if (_transacs == null)
                {
                    var newCollection = new FixupCollection<Transac>();
                    newCollection.CollectionChanged += FixupTransacs;
                    _transacs = newCollection;
                }
                return _transacs;
            }
            set
            {
                if (!ReferenceEquals(_transacs, value))
                {
                    var previousValue = _transacs as FixupCollection<Transac>;
                    if (previousValue != null)
                    {
                        previousValue.CollectionChanged -= FixupTransacs;
                    }
                    _transacs = value;
                    var newValue = value as FixupCollection<Transac>;
                    if (newValue != null)
                    {
                        newValue.CollectionChanged += FixupTransacs;
                    }
                }
            }
        }

        #endregion

        #region Association Fixup

        private void FixupMyQuestions(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (Question item in e.NewItems)
                {
                    item.Owner = this;
                }
            }

            if (e.OldItems != null)
            {
                foreach (Question item in e.OldItems)
                {
                    if (ReferenceEquals(item.Owner, this))
                    {
                        item.Owner = null;
                    }
                }
            }
        }

        private void FixupQuestionsToMe(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (Question item in e.NewItems)
                {
                    item.Receiver = this;
                }
            }

            if (e.OldItems != null)
            {
                foreach (Question item in e.OldItems)
                {
                    if (ReferenceEquals(item.Receiver, this))
                    {
                        item.Receiver = null;
                    }
                }
            }
        }

        private void FixupTransacs(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (Transac item in e.NewItems)
                {
                    item.User = this;
                }
            }

            if (e.OldItems != null)
            {
                foreach (Transac item in e.OldItems)
                {
                    if (ReferenceEquals(item.User, this))
                    {
                        item.User = null;
                    }
                }
            }
        }

        #endregion
    }
}