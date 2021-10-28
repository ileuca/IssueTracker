using IssueTracker.Models;
using IssueTracker.Models.DBObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IssueTracker.Repository
{
    public class StatusRepository
    {
        private readonly Models.DBObjects.IssueTrackerModelsDataContext dbContext;
        public StatusRepository()
        {
            this.dbContext = new Models.DBObjects.IssueTrackerModelsDataContext();
        }
        public StatusRepository(Models.DBObjects.IssueTrackerModelsDataContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public StatusModel MapDBObjectToModel(Status dbStatus)
        {
            StatusModel statusModel = new StatusModel
            {
                StatusId = dbStatus.StatusId,
                StatusName = dbStatus.StatusName
            };
            return statusModel;
        }
        //Read
        public List<StatusModel> GetStatuses()
        {
            List<StatusModel> statusList = new List<StatusModel>();
            foreach( var status in dbContext.Status)
            {
                statusList.Add(MapDBObjectToModel(status));
            }
            return statusList;
        }
    }
}