using System;
using System.Collections;
using System.ComponentModel;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Web.DynamicData;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QuestionYourFriendsBackEnd.DynamicData.FieldTemplates
{
    public partial class ManyToMany_EditField : FieldTemplateUserControl
    {
        public override Control DataControl
        {
            get { return CheckBoxList1; }
        }

        public void Page_Load(object sender, EventArgs e)
        {
            // Inscrit l'événement de mise à jour de DataSource
            var ds = (EntityDataSource) this.FindDataSourceControl();

            // Ce modèle de champ est utilisé pour l'édition et l'insertion
            ds.Updating += DataSource_UpdatingOrInserting;
            ds.Inserting += DataSource_UpdatingOrInserting;
        }

        private void DataSource_UpdatingOrInserting(object sender, EntityDataSourceChangingEventArgs e)
        {
            MetaTable childTable = ChildrenColumn.ChildTable;

            // Les commentaires supposent employé/territoire pour l'illustration, mais le code est générique

            // Obtient la collection de territoires pour cet employé
            var entityCollection = (RelatedEnd) Column.EntityTypeProperty.GetValue(e.Entity, null);

            // En mode Edition, vérifie qu'il est chargé (n'a pas de sens en mode Insertion)
            if (Mode == DataBoundControlMode.Edit && !entityCollection.IsLoaded)
            {
                entityCollection.Load();
            }

            // Obtient un IList (c-à-d. la liste des territoires pour l'employé actuel)
            // RÉVISION : il est conseillé d'utiliser EntityCollection directement, mais EF n'a pas de
            // type non générique pour lui. Cela sera ajouté  dans vnext
            IList entityList = ((IListSource) entityCollection).GetList();

            // Parcourt tous les territoires (pas uniquement pour cet employé)
            foreach (object childEntity in childTable.GetQuery(e.Context))
            {
                // Vérifie si l'employé a actuellement ce territoire
                bool isCurrentlyInList = entityList.Contains(childEntity);

                // Trouve la case à cocher pour ce territoire, ce qui donne le nouvel état
                string pkString = childTable.GetPrimaryKeyString(childEntity);
                ListItem listItem = CheckBoxList1.Items.FindByValue(pkString);
                if (listItem == null)
                    continue;

                // Si les états diffèrent, apporte les modifications appropriées, ajout ou suppression
                if (listItem.Selected)
                {
                    if (!isCurrentlyInList)
                        entityList.Add(childEntity);
                }
                else
                {
                    if (isCurrentlyInList)
                        entityList.Remove(childEntity);
                }
            }
        }

        protected void CheckBoxList1_DataBound(object sender, EventArgs e)
        {
            MetaTable childTable = ChildrenColumn.ChildTable;

            // Les commentaires supposent employé/territoire pour l'illustration, mais le code est générique

            IList entityList = null;
            ObjectContext objectContext = null;

            if (Mode == DataBoundControlMode.Edit)
            {
                object entity;
                var rowDescriptor = Row as ICustomTypeDescriptor;
                if (rowDescriptor != null)
                {
                    // Obtient l'entité réelle du wrapper
                    entity = rowDescriptor.GetPropertyOwner(null);
                }
                else
                {
                    entity = Row;
                }

                // Obtient la collection de territoires pour cet employé et vérifie qu'il est chargé
                var entityCollection = Column.EntityTypeProperty.GetValue(entity, null) as RelatedEnd;
                if (entityCollection == null)
                {
                    throw new InvalidOperationException(
                        String.Format(
                            "Le modèle ManyToMany ne prend pas en charge le type de collection de la colonne '{0}' dans la table '{1}'.",
                            Column.Name, Table.Name));
                }
                if (!entityCollection.IsLoaded)
                {
                    entityCollection.Load();
                }

                // Obtient un IList (c-à-d. la liste des territoires pour l'employé actuel)
                // RÉVISION : il est conseillé d'utiliser EntityCollection directement, mais EF n'a pas de
                // type non générique pour lui. Cela sera ajouté  dans vnext
                entityList = ((IListSource) entityCollection).GetList();

                // Obtient le ObjectContext actuel
                // RÉVISION : ce n'est pas vraiment la manière de faire. Rechercher une meilleure solution
                var objectQuery = (ObjectQuery) entityCollection.GetType().GetMethod(
                    "CreateSourceQuery").Invoke(entityCollection, null);
                objectContext = objectQuery.Context;
            }

            // Parcourt tous les territoires (pas uniquement pour cet employé)
            foreach (object childEntity in childTable.GetQuery(objectContext))
            {
                MetaTable actualTable = MetaTable.GetTable(childEntity.GetType());
                // Crée une case à cocher
                var listItem = new ListItem(
                    actualTable.GetDisplayString(childEntity),
                    actualTable.GetPrimaryKeyString(childEntity));

                // Sera sélectionné si l'employé actuel a ce territoire
                if (Mode == DataBoundControlMode.Edit)
                {
                    listItem.Selected = entityList.Contains(childEntity);
                }

                CheckBoxList1.Items.Add(listItem);
            }
        }
    }
}