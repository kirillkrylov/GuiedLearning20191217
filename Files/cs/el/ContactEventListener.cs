using System;
using Terrasoft.Core;
using Terrasoft.Core.Entities.AsyncOperations;
using Terrasoft.Core.Entities.AsyncOperations.Interfaces;
using Terrasoft.Core.Entities;
using Terrasoft.Core.Entities.Events;
using Terrasoft.Core.Factories;
using global::Common.Logging;

namespace GuidedLearningClio
{
    /// <summary>
    /// Listener for 'EntityName' entity events.
    /// </summary>
    /// <seealso cref="Terrasoft.Core.Entities.Events.BaseEntityEventListener" />
    [EntityEventListener(SchemaName = "Contact")]
    class EntityNameEventListener : BaseEntityEventListener
    {
        #region Enum
        #endregion

        #region Delegates
        #endregion

        #region Constants
        #endregion

        #region Fields

        #region Fields : Private
        private static readonly ILog _log = LogManager.GetLogger("GuidedLearningLogger");

        #endregion

        #region Fields : Protected
        #endregion

        #region Fields : Internal
        #endregion

        #region Fields : Protected Internal
        #endregion

        #region Fields : Public
        #endregion

        #endregion

        #region Properties

        #region Properties : Private
        #endregion

        #region Properties : Protected
        #endregion

        #region Properties : Internal
        #endregion

        #region Properties : Protected Internal
        #endregion

        #region Properties : Public
        #endregion

        #endregion

        #region Events
        #endregion

        #region Methods

        #region Methods : Private

        #endregion

        #region Methods : Public

        #region Methods : Public : OnSave
        //public override void OnSaving(object sender, EntityBeforeEventArgs e)
        //{
        //    base.OnSaving(sender, e);
        //    Entity entity = (Entity)sender;
        //    UserConnection userConnection = entity.UserConnection;
        //}
        public override void OnSaved(object sender, EntityAfterEventArgs e)
        {
            base.OnSaved(sender, e);
            Entity entity = (Entity)sender;
            //UserConnection userConnection = entity.UserConnection;

            string message = $"Changing name for - {entity.GetTypedColumnValue<string>("Name")}";
            _log.Info(message);
        }
        #endregion

        #region Methods : Public : OnInsert
        public override void OnInserting(object sender, EntityBeforeEventArgs e)
        {
            base.OnInserting(sender, e);
            //Entity entity = (Entity)sender;
            //UserConnection userConnection = entity.UserConnection;
        }
        public override void OnInserted(object sender, EntityAfterEventArgs e)
        {
            base.OnInserted(sender, e);
            //Entity entity = (Entity)sender;
            //UserConnection userConnection = entity.UserConnection;
        }
        #endregion

        #region Methods : Public : OnUpdate
        //public override void OnUpdating(object sender, EntityBeforeEventArgs e)
        //{
        //    base.OnUpdating(sender, e);
        //    Entity entity = (Entity)sender;
        //    UserConnection userConnection = entity.UserConnection;
        //}
        //public override void OnUpdated(object sender, EntityAfterEventArgs e)
        //{
        //    base.OnUpdated(sender, e);
        //    Entity entity = (Entity)sender;
        //    UserConnection userConnection = entity.UserConnection;
        //}
        #endregion

        #region Methods : Public : OnDelete
        //public override void OnDeleting(object sender, EntityBeforeEventArgs e)
        //{
        //    base.OnDeleting(sender, e);
        //    Entity entity = (Entity)sender;
        //    UserConnection userConnection = entity.UserConnection;
        //}
        //public override void OnDeleted(object sender, EntityAfterEventArgs e)
        //{
        //    base.OnDeleted(sender, e);
        //    Entity entity = (Entity)sender;
        //    UserConnection userConnection = entity.UserConnection;
        //}
        #endregion

        #endregion

        #endregion
    }
}
