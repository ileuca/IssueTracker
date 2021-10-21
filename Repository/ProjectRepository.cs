using IssueTracker.Models;
using IssueTracker.Models.DBObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IssueTracker.Repository
{
    public class ProjectRepository
    {
        private Models.DBObjects.IssueTrackerModelsDataContext dbContext;
        private TeamRepository TeamRepository = new TeamRepository();

        public ProjectRepository()
        {
            this.dbContext = new Models.DBObjects.IssueTrackerModelsDataContext();
        }
        public ProjectRepository(Models.DBObjects.IssueTrackerModelsDataContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Project MapModelToDbObject(ProjectModel projectModel)
        {
            Project dbProject = new Project();
            dbProject.ProjectId = projectModel.ProjectId;
            dbProject.TeamId = projectModel.TeamId;
            dbProject.ProjectName = projectModel.ProjectName;
            dbProject.ProjectDescription = projectModel.ProjectDescription;
            dbProject.StartDate = projectModel.StartDate;
            dbProject.EndDate = projectModel.EndDate;
            dbProject.StatusId = projectModel.StatusId;

            return dbProject;
        }
        public ProjectModel MapDbObjectToModel(Project dbProject)
        {
            ProjectModel projectModel = new ProjectModel();
            projectModel.ProjectId = dbProject.ProjectId;
            projectModel.TeamId = dbProject.TeamId;
            projectModel.ProjectName = dbProject.ProjectName;
            projectModel.ProjectDescription = dbProject.ProjectDescription;
            projectModel.StartDate = dbProject.StartDate;
            projectModel.EndDate = dbProject.EndDate;
            projectModel.StatusId = dbProject.StatusId;

            return projectModel;
        }

        //Create
        public void CreateProject(ProjectModel projectModel)
        {
            projectModel.ProjectId = Guid.NewGuid();
            dbContext.Projects.InsertOnSubmit(MapModelToDbObject(projectModel));
            dbContext.SubmitChanges();
        }

        //Read
        public List<ProjectModel> GetProjectsByTeamId(Guid TeamId)
        {
            List<ProjectModel> teamProjects = new List<ProjectModel>();
            foreach(var project in dbContext.Projects.Where(x => x.TeamId == TeamId))
            {
                teamProjects.Add(MapDbObjectToModel(project));
            }
            return teamProjects;
        }

        //Update
        public void UpdateProject(ProjectModel projectModel)
        {
            Project existingProject = dbContext.Projects.FirstOrDefault(x => x.ProjectId == projectModel.ProjectId);
            existingProject = MapModelToDbObject(projectModel);
            dbContext.SubmitChanges();
        }


        //Delete
        public void DeleteProject(ProjectModel projectModel)
        {
            Project existingProject = dbContext.Projects.FirstOrDefault(x => x.ProjectId == projectModel.ProjectId);
            if(existingProject !=null)
            {
                dbContext.Projects.DeleteOnSubmit(existingProject);
                dbContext.SubmitChanges();
            }
        }
    }
}