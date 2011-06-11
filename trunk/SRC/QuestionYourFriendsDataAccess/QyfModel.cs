﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré à partir d'un modèle.
//
//     Les modifications apportées à ce fichier peuvent provoquer un comportement incorrect et seront perdues si
//     le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace QuestionYourFriendsDataAccess
{
    // Classe d'assistance qui capture la plupart du travail de suivi des modifications qui doit être effectué
    // pour les entités de suivi automatique.
    [DataContract(IsReference = true)]
    public class ObjectChangeTracker
    {
        #region  Fields
    
        private bool _isDeserializing;
        private ObjectState _objectState = ObjectState.Added;
        private bool _changeTrackingEnabled;
        private OriginalValuesDictionary _originalValues;
        private ExtendedPropertiesDictionary _extendedProperties;
        private ObjectsAddedToCollectionProperties _objectsAddedToCollections = new ObjectsAddedToCollectionProperties();
        private ObjectsRemovedFromCollectionProperties _objectsRemovedFromCollections = new ObjectsRemovedFromCollectionProperties();
    
        #endregion
    
        #region Events
    
        public event EventHandler<ObjectStateChangingEventArgs> ObjectStateChanging;
    
        #endregion
    
        protected virtual void OnObjectStateChanging(ObjectState newState)
        {
            if (ObjectStateChanging != null)
            {
                ObjectStateChanging(this, new ObjectStateChangingEventArgs(){ NewState = newState });
            }
        }
    
        [DataMember]
        public ObjectState State
        {
            get { return _objectState; }
            set
            {
                if (_isDeserializing || _changeTrackingEnabled)
                {
                    OnObjectStateChanging(value);
                    _objectState = value;
                }
            }
        }
    
        public bool ChangeTrackingEnabled
        {
            get { return _changeTrackingEnabled; }
            set { _changeTrackingEnabled = value; }
        }
    
        // Retourne les objets supprimés aux propriétés de collection qui ont été modifiées.
        [DataMember]
    	[XmlIgnore]
        public ObjectsRemovedFromCollectionProperties ObjectsRemovedFromCollectionProperties
        {
            get
            {
                if (_objectsRemovedFromCollections == null)
                {
                    _objectsRemovedFromCollections = new ObjectsRemovedFromCollectionProperties();
                }
                return _objectsRemovedFromCollections;
            }
        }
    
        // Retourne les valeurs d'origine pour les propriétés qui ont été modifiées.
        [DataMember]
    	[XmlIgnore]
        public OriginalValuesDictionary OriginalValues
        {
            get
            {
                if (_originalValues == null)
                {
                    _originalValues = new OriginalValuesDictionary();
                }
                return _originalValues;
            }
        }
    
        // Retourne les valeurs de propriétés étendues.
        // Cela inclut les valeurs de clé pour les associations indépendantes, requises pour le
        // modèle de concurrence dans Entity Framework
        [DataMember]
    	[XmlIgnore]
        public ExtendedPropertiesDictionary ExtendedProperties
        {
            get
            {
                if (_extendedProperties == null)
                {
                    _extendedProperties = new ExtendedPropertiesDictionary();
                }
                return _extendedProperties;
            }
        }
    
        // Retourne les objets ajoutés aux propriétés de collection qui ont été modifiées.
        [DataMember]
    	[XmlIgnore]
        public ObjectsAddedToCollectionProperties ObjectsAddedToCollectionProperties
        {
            get
            {
                if (_objectsAddedToCollections == null)
                {
                    _objectsAddedToCollections = new ObjectsAddedToCollectionProperties();
                }
                return _objectsAddedToCollections;
            }
        }
    
        #region MethodsForChangeTrackingOnClient
    
        [OnDeserializing]
        public void OnDeserializingMethod(StreamingContext context)
        {
            _isDeserializing = true;
        }
    
        [OnDeserialized]
        public void OnDeserializedMethod(StreamingContext context)
        {
            _isDeserializing = false;
        }
    
        // Réinitialise ObjectChangeTracker avec l'état Unchanged et
        // efface les valeurs d'origine, ainsi que l'enregistrement des modifications
        // apportées aux propriétés de collection
        public void AcceptChanges()
        {
            OnObjectStateChanging(ObjectState.Unchanged);
            OriginalValues.Clear();
            ObjectsAddedToCollectionProperties.Clear();
            ObjectsRemovedFromCollectionProperties.Clear();
            ChangeTrackingEnabled = true;
            _objectState = ObjectState.Unchanged;
        }
    
        // Capture la valeur d'origine pour une propriété en cours de changement.
        internal void RecordOriginalValue(string propertyName, object value)
        {
            if (_changeTrackingEnabled && _objectState != ObjectState.Added)
            {
                if (!OriginalValues.ContainsKey(propertyName))
                {
                    OriginalValues[propertyName] = value;
                }
            }
        }
    
        // Enregistre un ajout aux propriétés de collection sur les entités de suivi automatique.
        internal void RecordAdditionToCollectionProperties(string propertyName, object value)
        {
            if (_changeTrackingEnabled)
            {
                // Ajouter à nouveau l'entité après sa suppression, nous ne devons donc pas intervenir
                if (ObjectsRemovedFromCollectionProperties.ContainsKey(propertyName)
                    && ObjectsRemovedFromCollectionProperties[propertyName].Contains(value))
                {
                    ObjectsRemovedFromCollectionProperties[propertyName].Remove(value);
                    if (ObjectsRemovedFromCollectionProperties[propertyName].Count == 0)
                    {
                        ObjectsRemovedFromCollectionProperties.Remove(propertyName);
                    }
                    return;
                }
    
                if (!ObjectsAddedToCollectionProperties.ContainsKey(propertyName))
                {
                    ObjectsAddedToCollectionProperties[propertyName] = new ObjectList();
                    ObjectsAddedToCollectionProperties[propertyName].Add(value);
                }
                else
                {
                    ObjectsAddedToCollectionProperties[propertyName].Add(value);
                }
            }
        }
    
        // Enregistre une suppression dans les propriétés de collection sur des entités de suivi automatique.
        internal void RecordRemovalFromCollectionProperties(string propertyName, object value)
        {
            if (_changeTrackingEnabled)
            {
                // Supprimer à nouveau l'entité après son ajout, nous ne devons donc pas intervenir
                if (ObjectsAddedToCollectionProperties.ContainsKey(propertyName)
                    && ObjectsAddedToCollectionProperties[propertyName].Contains(value))
                {
                    ObjectsAddedToCollectionProperties[propertyName].Remove(value);
                    if (ObjectsAddedToCollectionProperties[propertyName].Count == 0)
                    {
                        ObjectsAddedToCollectionProperties.Remove(propertyName);
                    }
                    return;
                }
    
                if (!ObjectsRemovedFromCollectionProperties.ContainsKey(propertyName))
                {
                    ObjectsRemovedFromCollectionProperties[propertyName] = new ObjectList();
                    ObjectsRemovedFromCollectionProperties[propertyName].Add(value);
                }
                else
                {
                    if (!ObjectsRemovedFromCollectionProperties[propertyName].Contains(value))
                    {
                        ObjectsRemovedFromCollectionProperties[propertyName].Add(value);
                    }
                }
            }
        }
        #endregion
    }
    
    #region EnumForObjectState
    [Flags]
    public enum ObjectState
    {
        Unchanged = 0x1,
        Added = 0x2,
        Modified = 0x4,
        Deleted = 0x8
    }
    #endregion
    
    [CollectionDataContract (Name = "ObjectsAddedToCollectionProperties",
        ItemName = "AddedObjectsForProperty", KeyName = "CollectionPropertyName", ValueName = "AddedObjects")]
    public class ObjectsAddedToCollectionProperties : Dictionary<string, ObjectList> { }
    
    [CollectionDataContract (Name = "ObjectsRemovedFromCollectionProperties",
        ItemName = "DeletedObjectsForProperty", KeyName = "CollectionPropertyName",ValueName = "DeletedObjects")]
    public class ObjectsRemovedFromCollectionProperties : Dictionary<string, ObjectList> { }
    
    [CollectionDataContract(Name = "OriginalValuesDictionary",
        ItemName = "OriginalValues", KeyName = "Name", ValueName = "OriginalValue")]
    public class OriginalValuesDictionary : Dictionary<string, Object> { }
    
    [CollectionDataContract(Name = "ExtendedPropertiesDictionary",
        ItemName = "ExtendedProperties", KeyName = "Name", ValueName = "ExtendedProperty")]
    public class ExtendedPropertiesDictionary : Dictionary<string, Object> { }
    
    [CollectionDataContract(ItemName = "ObjectValue")]
    public class ObjectList : List<object> { }
    // L'interface est implémentée par les entités de suivi automatique générées par EF.
    // Nous disposerons d'un adaptateur qui convertit cette interface en interface attendue par EF.
    // L'adaptateur résidera côté serveur.
    public interface IObjectWithChangeTracker
    {
        // Possède toutes les informations de suivi des modifications pour le sous-graphique d'un objet spécifique.
        ObjectChangeTracker ChangeTracker { get; }
    }
    
    public class ObjectStateChangingEventArgs : EventArgs
    {
        public ObjectState NewState { get; set; }
    }
    
    public static class ObjectWithChangeTrackerExtensions
    {
        public static T MarkAsDeleted<T>(this T trackingItem) where T : IObjectWithChangeTracker
        {
            if (trackingItem == null)
            {
                throw new ArgumentNullException("trackingItem");
            }
    
            trackingItem.ChangeTracker.ChangeTrackingEnabled = true;
            trackingItem.ChangeTracker.State = ObjectState.Deleted;
            return trackingItem;
        }
    
        public static T MarkAsAdded<T>(this T trackingItem) where T : IObjectWithChangeTracker
        {
            if (trackingItem == null)
            {
                throw new ArgumentNullException("trackingItem");
            }
    
            trackingItem.ChangeTracker.ChangeTrackingEnabled = true;
            trackingItem.ChangeTracker.State = ObjectState.Added;
            return trackingItem;
        }
    
        public static T MarkAsModified<T>(this T trackingItem) where T : IObjectWithChangeTracker
        {
            if (trackingItem == null)
            {
                throw new ArgumentNullException("trackingItem");
            }
    
            trackingItem.ChangeTracker.ChangeTrackingEnabled = true;
            trackingItem.ChangeTracker.State = ObjectState.Modified;
            return trackingItem;
        }
    
        public static T MarkAsUnchanged<T>(this T trackingItem) where T : IObjectWithChangeTracker
        {
            if (trackingItem == null)
            {
                throw new ArgumentNullException("trackingItem");
            }
    
            trackingItem.ChangeTracker.ChangeTrackingEnabled = true;
            trackingItem.ChangeTracker.State = ObjectState.Unchanged;
            return trackingItem;
        }
    
        public static void StartTracking(this IObjectWithChangeTracker trackingItem)
        {
            if (trackingItem == null)
            {
                throw new ArgumentNullException("trackingItem");
            }
    
            trackingItem.ChangeTracker.ChangeTrackingEnabled = true;
        }
    
        public static void StopTracking(this IObjectWithChangeTracker trackingItem)
        {
            if (trackingItem == null)
            {
                throw new ArgumentNullException("trackingItem");
            }
    
            trackingItem.ChangeTracker.ChangeTrackingEnabled = false;
        }
    
        public static void AcceptChanges(this IObjectWithChangeTracker trackingItem)
        {
            if (trackingItem == null)
            {
                throw new ArgumentNullException("trackingItem");
            }
    
            trackingItem.ChangeTracker.AcceptChanges();
        }
    }
    
    // System.Collections.ObjectModel.ObservableCollection qui déclenche
    // des notifications de suppression d'élément individuel lors d'un effacement et évite l'ajout de doublons.
    public class TrackableCollection<T> : ObservableCollection<T>
    {
        protected override void ClearItems()
        {
            new List<T>(this).ForEach(t => Remove(t));
        }
    
        protected override void InsertItem(int index, T item)
        {
            if (!this.Contains(item))
            {
                base.InsertItem(index, item);
            }
        }
    }
    
    // Interface qui fournit un événement qui se déclenche lors de la modification de propriétés complexes.
    // Les modifications peuvent concerner le remplacement d'une propriété complexe par une nouvelle instance de type complexe ou
    // la modification d'une propriété scalaire au sein d'une instance de type complexe.
    public interface INotifyComplexPropertyChanging
    {
        event EventHandler ComplexPropertyChanging;
    }
    
    public static class EqualityComparer
    {
        // Méthode d'assistance pour déterminer si les tableaux à deux octets sont de même valeur même s'il s'agit de références d'objets différentes
        public static bool BinaryEquals(object binaryValue1, object binaryValue2)
        {
            if (Object.ReferenceEquals(binaryValue1, binaryValue2))
            {
                return true;
            }
    
            byte[] array1 = binaryValue1 as byte[];
            byte[] array2 = binaryValue2 as byte[];
    
            if (array1 != null && array2 != null)
            {
                if (array1.Length != array2.Length)
                {
                    return false;
                }
    
                for (int i = 0; i < array1.Length; i++)
                {
                    if (array1[i] != array2[i])
                    {
                        return false;
                    }
                }
    
                return true;
            }
    
            return false;
        }
    }
}