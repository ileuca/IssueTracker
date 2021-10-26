using IssueTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IssueTracker.Repository
{
    public class ActionRepository
    {
        private Models.DBObjects.IssueTrackerModelsDataContext dbContext;
        private IssueRepository issueRepository = new IssueRepository();

        public ActionRepository()
        {
            this.dbContext = new Models.DBObjects.IssueTrackerModelsDataContext();
        }
        public ActionRepository(Models.DBObjects.IssueTrackerModelsDataContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public ActionModel MapDbObjectToModel(Models.DBObjects.Action action)
        {
            if (action != null)
            {
                ActionModel actionModel = new ActionModel();
                actionModel.ActionId = action.ActionId;
                actionModel.IssueId = action.IssueId;
                actionModel.ActionName = action.ActionName;
                actionModel.ActionDescription = action.ActionDescription;
                actionModel.StartDate = action.StartDate;
                actionModel.EndDate = action.EndDate;
                actionModel.StatusId = action.StatusId;

                return actionModel;
            }
            return null;
        }
        public Models.DBObjects.Action MapModelToDbObject(ActionModel actionModel)
        {
            if (actionModel != null)
            {
                Models.DBObjects.Action action = new Models.DBObjects.Action();
                action.ActionId = actionModel.ActionId;
                action.IssueId = actionModel.IssueId;
                action.ActionName = actionModel.ActionName;
                action.ActionDescription = actionModel.ActionDescription;
                action.StartDate = actionModel.StartDate;
                action.EndDate = actionModel.EndDate;
                action.StatusId = actionModel.StatusId;

                return action;
            }
            return null;
        }

        //Create
        public void CreateAction(ActionModel actionModel)
        {
            Models.DBObjects.Action action = new Models.DBObjects.Action();
            actionModel.ActionId = Guid.NewGuid();
            action = MapModelToDbObject(actionModel);
            dbContext.Actions.InsertOnSubmit(action);
            dbContext.SubmitChanges();
        }
        //Read
        public List<ActionModel> GetActionsByIssueId(Guid IssueId)
        {
            List<ActionModel> actionsByIssueId = new List<ActionModel>();
            foreach(var action in dbContext.Actions.Where(a => a.IssueId == IssueId))
            {
                actionsByIssueId.Add(MapDbObjectToModel(action));
            }
            return actionsByIssueId;
        }
        public ActionModel GetActionById(Guid ActionId)
        {
            return MapDbObjectToModel(dbContext.Actions.FirstOrDefault(a => a.ActionId == ActionId));
        }
        public IssueModel GetIssueByAction(ActionModel actionModel)
        {
            IssueModel issueModel = new IssueModel();
            issueModel = issueRepository.MapDbObjectToModel(dbContext.Issues.FirstOrDefault(i => i.IssueId == actionModel.IssueId));
            return issueModel;
        }
        //Update
        public void UpdateAction(ActionModel actionModel)
        {
            var existingAction = dbContext.Actions.FirstOrDefault(a => a.ActionId == actionModel.ActionId);
            existingAction.ActionName = actionModel.ActionName;
            existingAction.ActionDescription = actionModel.ActionDescription;
            existingAction.StartDate = actionModel.StartDate;
            existingAction.EndDate = actionModel.EndDate;
            dbContext.SubmitChanges();
        }
        //Delete
        public void DeleteAction(ActionModel actionModel)
        {
            var existingAction = dbContext.Actions.FirstOrDefault(a => a.ActionId == actionModel.ActionId);
            dbContext.Actions.DeleteOnSubmit(MapModelToDbObject(actionModel));
            dbContext.SubmitChanges();
        }


    }
}