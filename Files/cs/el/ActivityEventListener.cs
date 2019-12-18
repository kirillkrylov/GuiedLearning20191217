using System;
using Terrasoft.Core;
using Terrasoft.Core.Entities.AsyncOperations;
using Terrasoft.Core.Entities.AsyncOperations.Interfaces;
using Terrasoft.Core.Entities;
using Terrasoft.Core.Entities.Events;
using Terrasoft.Core.Factories;
using global::Common.Logging;
using Terrasoft.Core.DB;
using System.Data;

namespace GuidedLearningClio.Files.cs.el
{
    /// <summary>
    /// Listener for 'EntityName' entity events.
    /// </summary>
    /// <see cref="Terrasoft.Core.Entities.Events.BaseEntityEventListener" />
    [EntityEventListener(SchemaName = "Activity")]
    class ActivityEventListener : BaseEntityEventListener
    {
        private static readonly ILog _log = LogManager.GetLogger("GuidedLearningLogger");
        private static readonly Guid activityType = Guid.Parse("FBE0ACDC-CFC0-DF11-B00F-001D60E938C6"); //Task
        private static readonly Guid activityCategory = Guid.Parse("F51C4643-58E6-DF11-971B-001D60E938C6"); //ToDo
        private static readonly Guid activityStatusNotStarted = Guid.Parse("384D4B84-58E6-DF11-971B-001D60E938C6"); //No Started
        private static readonly Guid activityStatusInProgress = Guid.Parse("394D4B84-58E6-DF11-971B-001D60E938C6"); //In Progress

        /// <summary>
        /// Example using Db.Select class aka. Ad-hoc DB Queries
        /// When working with the Select class, the current user permissions are not taken into consideration
        /// </summary>
        /// <param name="activity">Activity Entity</param>
        /// <returns>Return number of records found</returns>
        /// <remarks> <see cref="https://academy.creatio.com/documents/technic-sdk/7-15/retrieving-information-database-select-class" /></remarks>
        private static int CountOverlapingActivity(Entity activity)
        {
            int result = 0;
            
            if (activity.GetTypedColumnValue<Guid>("TypeId") != activityType)
                return result;

            if (activity.GetTypedColumnValue<Guid>("ActivityCategoryId") != activityCategory)
                return result;

            if (activity.GetTypedColumnValue<Guid>("StatusId") != activityStatusNotStarted && activity.GetTypedColumnValue<Guid>("StatusId") != activityStatusInProgress)
                return result;

            Guid ownerId = activity.GetTypedColumnValue<Guid>("OwnerId");
            if (activity.GetTypedColumnValue<Guid>("OwnerId") != ownerId)
                return result;

            Guid activityId = activity.GetTypedColumnValue<Guid>("Id");

            UserConnection userConnection = activity.UserConnection;
            TimeZoneInfo userTimeZonInfo = userConnection.CurrentUser.TimeZone;

            DateTime start = TimeZoneInfo.ConvertTimeToUtc(activity.GetTypedColumnValue<DateTime>("StartDate").AddSeconds(1), userTimeZonInfo);
            DateTime due = TimeZoneInfo.ConvertTimeToUtc(activity.GetTypedColumnValue<DateTime>("DueDate").AddSeconds(-1), userTimeZonInfo);
            
            Select select = new Select(userConnection)
                .Column(Func.Count("Id"))
                .From("Activity")
                .Where("TypeId").IsEqual(Column.Parameter(activityType))
                .And("ActivityCategoryId").IsEqual(Column.Parameter(activityCategory))
                .And("Id").IsNotEqual(Column.Parameter(activityId))
                .And("OwnerId").IsEqual(Column.Parameter(ownerId))
                .And()
                    .OpenBlock("StatusId").IsEqual(Column.Parameter(activityStatusInProgress))
                    .Or("StatusId").IsEqual(Column.Parameter(activityStatusNotStarted))
                .CloseBlock() as Select;

            select.And()
                //Case 1 Starts During new Activity
                .OpenBlock("StartDate").IsBetween(Column.Parameter(start)).And(Column.Parameter(due))

                //Case 2 Ends During new Activity
                .Or("DueDate").IsBetween(Column.Parameter(start)).And(Column.Parameter(due))

                //Case3 - Starts before and ends after new Activity start
                .Or()
                    .OpenBlock("StartDate").IsLessOrEqual(Column.Parameter(start))
                    .And("DueDate").IsGreaterOrEqual(Column.Parameter(due))
                    .CloseBlock()
                .CloseBlock();

            select.BuildParametersAsValue = true;
            _log.Info(select.GetSqlText());
            
            using (DBExecutor executor = userConnection.EnsureDBConnection())
            {
                result = select.ExecuteScalar<int>(executor);
            }
            return result;
        }

        /// <summary>
        /// Example using Uses Entity Schema Query
        /// </summary>
        /// <param name="activity">Activity Entity</param>
        /// <returns>Return number of records found</returns>
        /// <remarks> <see cref="https://academy.creatio.com/documents/technic-sdk/7-15/introduction-13" /></remarks>
        private static int CountOverlapingActivityEsq(Entity activity) {
            UserConnection userConnection = activity.UserConnection;
            userConnection = activity.UserConnection;
            TimeZoneInfo userTimeZonInfo = userConnection.CurrentUser.TimeZone;

            DateTime start = TimeZoneInfo.ConvertTimeToUtc(activity.GetTypedColumnValue<DateTime>("StartDate").AddSeconds(1), userTimeZonInfo);
            DateTime due = TimeZoneInfo.ConvertTimeToUtc(activity.GetTypedColumnValue<DateTime>("DueDate").AddSeconds(-1), userTimeZonInfo);
            Guid ownerId = activity.GetTypedColumnValue<Guid>("OwnerId");

            EntitySchemaQuery esqResult = new EntitySchemaQuery(userConnection.EntitySchemaManager, "Activity");
            esqResult.AddColumn("Title");
            esqResult.AddColumn("StartDate");
            esqResult.AddColumn("DueDate");
            esqResult.AddColumn("Type");
            esqResult.AddColumn("ActivityCategory");
            esqResult.AddColumn("Owner");
            esqResult.AddColumn("Status");

            //Do not look for itself
            esqResult.Filters.LogicalOperation = Terrasoft.Common.LogicalOperationStrict.And;
            IEntitySchemaQueryFilterItem activityIdFilter =
                esqResult.CreateFilterWithParameters(FilterComparisonType.NotEqual, "Id", activity.GetTypedColumnValue<Guid>("Id"));
            esqResult.Filters.Add(activityIdFilter);

            esqResult.Filters.LogicalOperation = Terrasoft.Common.LogicalOperationStrict.And;
            IEntitySchemaQueryFilterItem activityTypeFilter = 
                esqResult.CreateFilterWithParameters(FilterComparisonType.Equal, "Type", activityType);
            esqResult.Filters.Add(activityTypeFilter);
            
            IEntitySchemaQueryFilterItem activityCategoryFilter = 
                esqResult.CreateFilterWithParameters(FilterComparisonType.Equal, "ActivityCategory", activityCategory);
            esqResult.Filters.Add(activityCategoryFilter);
            
            IEntitySchemaQueryFilterItem activityOwnerFilter = 
                esqResult.CreateFilterWithParameters(FilterComparisonType.Equal, "Owner", ownerId);
            esqResult.Filters.Add(activityOwnerFilter);
            
            //Set Statuses
            esqResult.Filters.Add(
                new EntitySchemaQueryFilterCollection(
                    esqResult,
                    Terrasoft.Common.LogicalOperationStrict.Or,
                    esqResult.CreateFilterWithParameters(FilterComparisonType.Equal, "Status", activityStatusInProgress),
                    esqResult.CreateFilterWithParameters(FilterComparisonType.Equal, "Status", activityStatusNotStarted)
                    )
                );

            //Add Dates
            var dateFilter1 = new EntitySchemaQueryFilterCollection(
                esqResult,
                Terrasoft.Common.LogicalOperationStrict.Or,
                esqResult.CreateFilterWithParameters(FilterComparisonType.Between, "StartDate", start, due),
                esqResult.CreateFilterWithParameters(FilterComparisonType.Between, "DueDate", start, due)
            );

            var dateFilter2 = new EntitySchemaQueryFilterCollection(
                esqResult,
                Terrasoft.Common.LogicalOperationStrict.And,
                esqResult.CreateFilterWithParameters(FilterComparisonType.LessOrEqual, "StartDate", start),
                esqResult.CreateFilterWithParameters(FilterComparisonType.GreaterOrEqual, "DueDate", due)
            );

            esqResult.Filters.Add(
                new EntitySchemaQueryFilterCollection(
                        esqResult,
                        Terrasoft.Common.LogicalOperationStrict.Or,
                        dateFilter1,
                        dateFilter2
                    )
               );
            
            EntityCollection activities = esqResult.GetEntityCollection(userConnection);
            int result = activities.Count;
            return result;
        }

        public override void OnInserting(object sender, EntityBeforeEventArgs e)
        {
            base.OnInserting(sender, e);
            Entity entity = (Entity)sender;
            if (CountOverlapingActivity(entity) != 0 || CountOverlapingActivityEsq(entity) != 0)
                e.IsCanceled = true;
        }
              
        public override void OnUpdating(object sender, EntityBeforeEventArgs e)
        {
            base.OnUpdating(sender, e);
            Entity entity = (Entity)sender;
            if (CountOverlapingActivity(entity) != 0 || CountOverlapingActivityEsq(entity) !=0)
                e.IsCanceled = true;
        }        
    }
}
