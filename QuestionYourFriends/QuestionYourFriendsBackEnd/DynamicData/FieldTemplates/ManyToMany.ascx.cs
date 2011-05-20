using System;
using System.ComponentModel;
using System.Data.Objects.DataClasses;
using System.Web.DynamicData;
using System.Web.UI;

namespace QuestionYourFriendsBackEnd.DynamicData.FieldTemplates
{
    public partial class ManyToManyField : FieldTemplateUserControl
    {
        protected override void OnDataBinding(EventArgs e)
        {
            base.OnDataBinding(e);

            object entity;
            ICustomTypeDescriptor rowDescriptor = Row as ICustomTypeDescriptor;
            if (rowDescriptor != null)
            {
                // Obtient l'entité réelle du wrapper
                entity = rowDescriptor.GetPropertyOwner(null);
            }
            else
            {
                entity = Row;
            }

            // Obtient la collection et vérifie qu'elle est chargée
            RelatedEnd entityCollection = Column.EntityTypeProperty.GetValue(entity, null) as RelatedEnd;
            if (entityCollection == null)
            {
                throw new InvalidOperationException(String.Format("Le modèle ManyToMany ne prend pas en charge le type de collection de la colonne '{0}' dans la table '{1}'.", Column.Name, Table.Name));
            }
            if (!entityCollection.IsLoaded)
            {
                entityCollection.Load();
            }

            // Lie l'élément Repeater à la liste des entités enfants
            Repeater1.DataSource = entityCollection;
            Repeater1.DataBind();
        }

        public override Control DataControl
        {
            get
            {
                return Repeater1;
            }
        }

    }
}
