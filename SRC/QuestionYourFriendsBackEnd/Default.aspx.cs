using System;
using System.Collections;
using System.Web.UI;

namespace QuestionYourFriendsBackEnd
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            IList visibleTables = Global.DefaultModel.VisibleTables;
            if (visibleTables.Count == 0)
            {
                throw new InvalidOperationException(
                    "Il n'y a aucune table accessible. Assurez-vous qu'au moins un modèle de données est inscrit dans Global.asax et que la génération de modèles automatique est activée, ou implémentez des pages personnalisées.");
            }
            Menu1.DataSource = visibleTables;
            Menu1.DataBind();
        }
    }
}