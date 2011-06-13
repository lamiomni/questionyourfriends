using System;
using System.Web;
using System.Web.Routing;
using System.Web.DynamicData;
using QuestionYourFriendsDataAccess;

namespace QuestionYourFriendsBackEnd
{
    public class Global : HttpApplication
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            MetaModel model = new MetaModel();

            //                    IMPORTANT : INSCRIPTION DU MODÈLE DE DONNÉES 
            // Supprimez les marques de commentaire de la ligne pour inscrire un ADO.NET Entity Data
            // pour  Dynamic Data ASP.NET. Définissez ScaffoldAllTables = true uniquement si vous êtes certain 
            // que vous voulez que toutes les tables du modèle de données prennent en charge une vue de structure (c'est-à-dire des modèles) 
            // . Pour contrôler la génération de modèles automatique des tables individuelles, créez une classe partielle pour 
            // la table et appliquez l'attribut [Scaffold(true)] à la classe partielle.
            // Remarque : vérifiez que vous remplacez "YourDataContextType" par le nom de la classe du contexte de données
            // de votre application.
            model.RegisterContext(typeof(QuestionYourFriendsEntities), new ContextConfiguration { ScaffoldAllTables = true });

            // L'instruction suivante prend en charge le mode page séparée, où les tâches Liste, Détail, Insérer et 
            // Mettre à jour sont exécutées à l'aide de pages distinctes. Pour activer ce mode, supprimez les marques de commentaire 
            // de la définition route suivante et commentez les définitions de route dans la section de mode page combinée qui suit.
            //routes.Add(new DynamicDataRoute("{table}/{action}.aspx")
            //{
            //    Constraints = new RouteValueDictionary(new { action = "List|Details|Edit|Insert" }),
            //    Model = model
            //});

            // Les instructions suivantes prennent en charge le mode page combinée, où les tâches Liste, Détail, Insérer et
            // Mettre à jour sont exécutées à l'aide de la même page. Pour activer ce mode, supprimez les marques de commentaire
            // de routes et commentez la définition de l'itinéraire dans la section du mode page séparée ci-dessus.
            routes.Add(new DynamicDataRoute("{table}/ListDetails.aspx")
            {
                Action = PageAction.List,
                ViewName = "ListDetails",
                Model = model
            });

            routes.Add(new DynamicDataRoute("{table}/ListDetails.aspx") {
                Action = PageAction.Details,
                ViewName = "ListDetails",
                Model = model
            });
        }

        void Application_Start(object sender, EventArgs e)
        {
            RegisterRoutes(RouteTable.Routes);
        }

    }
}
