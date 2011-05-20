using System;
using System.Web.DynamicData;
using System.Web.UI;

namespace QuestionYourFriendsBackEnd.DynamicData.FieldTemplates
{
    public partial class EmailAddressField : FieldTemplateUserControl
    {
        protected override void OnDataBinding(EventArgs e)
        {
            string url = FieldValueString;
            if (!url.StartsWith("mailto:", StringComparison.OrdinalIgnoreCase))
            {
                url = "mailto:" + url;
            }
            HyperLink1.NavigateUrl = url;
        }

        public override Control DataControl
        {
            get
            {
                return HyperLink1;
            }
        }

    }
}
