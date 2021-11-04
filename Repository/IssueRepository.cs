using IssueTracker.Models;
using IssueTracker.Models.DBObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IssueTracker.Repository
{
    public class IssueRepository
    {
        private readonly Models.DBObjects.IssueTrackerModelsDataContext dbContext;
        private readonly ProjectRepository projectRepository = new ProjectRepository();
        public IssueRepository()
        {
            this.dbContext = new Models.DBObjects.IssueTrackerModelsDataContext();
        }
        public IssueRepository(Models.DBObjects.IssueTrackerModelsDataContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public Issue MapModelToDbObject(IssueModel issueModel)
        {
            if (issueModel != null)
            {
                Issue dbIssue = new Issue
                {
                    IssueId = issueModel.IssueId,
                    ProjectId = issueModel.ProjectId,
                    UserId = issueModel.UserId,
                    IssueName = issueModel.IssueName,
                    IssueDescription = issueModel.IssueDescription,
                    StartDate = issueModel.StartDate,
                    EndDate = issueModel.EndDate,
                    StatusId = issueModel.StatusId
                };
                return dbIssue;
            }
            return null;
        }
        public IssueModel MapDbObjectToModel(Issue dbIssue)
        {
            if (dbIssue != null)
            {
                IssueModel issueModel = new IssueModel
                {
                    IssueId = dbIssue.IssueId,
                    ProjectId = dbIssue.ProjectId,
                    UserId = dbIssue.UserId,
                    IssueName = dbIssue.IssueName,
                    IssueDescription = dbIssue.IssueDescription,
                    StartDate = dbIssue.StartDate,
                    EndDate = dbIssue.EndDate,
                    StatusId = dbIssue.StatusId
                };
                return issueModel;
            }
            return null;
        }
        //Create
        public void CreateIssue(IssueModel issueModel)
        {
            issueModel.IssueId = Guid.NewGuid();
            dbContext.Issues.InsertOnSubmit(MapModelToDbObject(issueModel));
            dbContext.SubmitChanges();
        }
        //Read
        public List<IssueModel> GetIssuesByProjectId(Guid ProjectId)
        {
            List<IssueModel> issuesByProject = new List<IssueModel>();
            foreach(var issue in dbContext.Issues.Where(x => x.ProjectId == ProjectId))
            {
                issuesByProject.Add(MapDbObjectToModel(issue));
            }
            return issuesByProject;
        }
        public IssueModel GetIssueById(Guid IssueId)
        {
            IssueModel issueModel = MapDbObjectToModel(dbContext.Issues.FirstOrDefault(i => i.IssueId == IssueId));
            return issueModel;
        }
        public Guid GetTeamIdByIssueId(Guid IssueId)
        {
            ProjectModel projectModel = projectRepository.GetProjectByProjectId(GetIssueById(IssueId).ProjectId);
            return projectModel.TeamId;
        }
        //Update
        public void UpdateIssue(IssueModel issueModel)
        {
            Issue existingIssue = dbContext.Issues.FirstOrDefault(i => i.IssueId == issueModel.IssueId);
            if(existingIssue !=null)
            {
                existingIssue.UserId = issueModel.UserId;
                existingIssue.IssueName = issueModel.IssueName;
                existingIssue.IssueDescription = issueModel.IssueDescription;
                existingIssue.StartDate = issueModel.StartDate;
                existingIssue.EndDate = issueModel.EndDate;
                existingIssue.StatusId = issueModel.StatusId;
                dbContext.SubmitChanges();
            }
        }
        //Delete
        public void DeleteIssue(IssueModel issueModel)
        {
            Issue existingIssue = dbContext.Issues.FirstOrDefault(i => i.IssueId == issueModel.IssueId);
            var actionsForIssue = dbContext.Actions.Where(a => a.IssueId == issueModel.IssueId);
            if (existingIssue != null)
            {
                dbContext.Actions.DeleteAllOnSubmit(actionsForIssue);
                dbContext.Issues.DeleteOnSubmit(existingIssue);
                dbContext.SubmitChanges();
            }
        }
    }
}