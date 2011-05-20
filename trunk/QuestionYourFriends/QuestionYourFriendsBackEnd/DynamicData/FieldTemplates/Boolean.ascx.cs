using System;
using System.Web.DynamicData;
using System.Web.UI;

namespace QuestionYourFriendsBackEnd.DynamicData.FieldTemplates
{
    public partial class BooleanField : FieldTemplateUserControl
    {
        protected override void OnDataBinding(EventArgs e)
        {
            base.OnDataBinding(e);

            object val = FieldValue;
            if (val != null)
                CheckBox1.Checked = (bool)val;
        }

        public override Control DataControl
        {
            get
            {
                return CheckBox1;
            }
        }

    }
}
