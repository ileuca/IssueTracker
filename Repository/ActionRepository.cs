using IssueTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IssueTracker.Repository
{
    public class ActionRepository
    {
        private readonly Models.DBObjects.IssueTrackerModelsDataContext dbContext;
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
                ActionModel actionModel = new ActionModel
                {
                    ActionId = action.ActionId,
                    IssueId = action.IssueId,
                    ActionName = action.ActionName,
                    ActionDescription = action.ActionDescription,
                    StartDate = action.StartDate,
                    EndDate = action.EndDate,
                    StatusId = action.StatusId
                };
                return actionModel;
            }
            return null;
        }
        public Models.DBObjects.Action MapModelToDbObject(ActionModel actionModel)
        {
            if (actionModel != null)
            {
                Models.DBObjects.Action action = new Models.DBObjects.Action
                {
                    ActionId = actionModel.ActionId,
                    IssueId = actionModel.IssueId,
                    ActionName = actionModel.ActionName,
                    ActionDescription = actionModel.ActionDescription,
                    StartDate = actionModel.StartDate,
                    EndDate = actionModel.EndDate,
                    StatusId = actionModel.StatusId
                };
                return action;
            }
            return null;
        }
        //Create
        public void CreateAction(ActionModel actionModel)
        {
            actionModel.ActionId = Guid.NewGuid();
            Models.DBObjects.Action action = MapModelToDbObject(actionModel);
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
            dbContext.Actions.DeleteOnSubmit(existingAction);
            dbContext.SubmitChanges();
        }
    }
}