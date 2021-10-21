using IssueTracker.Models;
using IssueTracker.Models.DBObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IssueTracker.Repository
{
    public class StatusRepository
    {
        private Models.DBObjects.IssueTrackerModelsDataContext dbContext;

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
            StatusModel statusModel = new StatusModel();
            statusModel.StatusId = dbStatus.StatusId;
            statusModel.StatusName = dbStatus.StatusName;

            return statusModel;
        }

        //Read
        public StatusModel GetStatusById(Guid StatusId)
        {
            StatusModel statusModel = new StatusModel();
            statusModel = MapDBObjectToModel(dbContext.Status.FirstOrDefault(x => x.StatusId == StatusId));
            return statusModel;
        }
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