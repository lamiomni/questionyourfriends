using System;
using System.ComponentModel.DataAnnotations;
using System.Web.DynamicData;

namespace QuestionsYourFriendsBackEnd
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            System.Collections.IList visibleTables = Global.DefaultModel.VisibleTables;
            if (visibleTables.Count == 0)
            {
                throw new InvalidOperationException("Il n'y a aucune table accessible. Assurez-vous qu'au moins un modèle de données est inscrit dans Global.asax et que la génération de modèles automatique est activée, ou implémentez des pages personnalisées.");
            }
            Menu1.DataSource = visibleTables;
            Menu1.DataBind();
        }

    }
}
