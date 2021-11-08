using IssueTracker.Models;
using IssueTracker.Models.DBObjects;
using IssueTracker.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace IssueTracker.Controllers
{
    public class ProjectController : Controller
    {
        private readonly ProjectRepository projectRepository = new ProjectRepository();
        private readonly IssueRepository issueRepository = new IssueRepository();
        private readonly ActionRepository actionRepository = new ActionRepository();
        private readonly UserRepository userRepository = new UserRepository();
        private readonly TeamRepository teamRepository = new TeamRepository();
        private readonly StatusRepository statusRepository = new StatusRepository();
        public ActionResult Index(string searchString)
        {
            try
            {
                User currentUser = userRepository.GetCurrentUser();
                List<TeamModel> teamList = teamRepository.GetTeamsForCurrentUserId(currentUser.UserId);
                List<ProjectModel> projectsByUser = new List<ProjectModel>();
                foreach (var team in teamList)
                {
                    List<ProjectModel> projectsByTeam = projectRepository.GetProjectsByTeamId(team.TeamId);
                    foreach (var project in projectsByTeam)
                    {
                        if (project.EndDate < DateTime.Now)
                        {
                            project.StatusId = statusRepository.GetStatuses().FirstOrDefault(x => x.StatusName == "Delayed").StatusId;
                            projectRepository.UpdateProject(project);
                        }
                        projectsByUser.Add(project);
                    }
                }
                if (!string.IsNullOrEmpty(searchString))
                {
                    projectsByUser = projectsByUser.Where(p => p.ProjectName.Contains(searchString)
                                                    || p.ProjectDescription.Contains(searchString)).ToList();
                }
                return View("Index", projectsByUser);
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }

        }
        public ActionResult Create()
        {
            try
            {
                ProjectModel projectModel = new ProjectModel();
                projectModel.TeamList = teamRepository.GetTeamsForCurrentUserId(userRepository.GetCurrentUser().UserId, "Master");
                return View(projectModel);
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }

        }
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            ProjectModel projectModel = new ProjectModel();

            try
            {
                if (ModelState.IsValid)
                { 
                    UpdateModel(projectModel);
                    if (projectModel.StartDate > DateTime.Now && projectModel.EndDate > DateTime.Now)
                    {
                        projectModel.StatusId = statusRepository.GetStatuses().FirstOrDefault(x => x.StatusName == "Not Started").StatusId;
                    }
                    else if (projectModel.StartDate <= DateTime.Now && projectModel.EndDate > DateTime.Now)
                    {
                        projectModel.StatusId = statusRepository.GetStatuses().FirstOrDefault(x => x.StatusName == "In Progress").StatusId;
                    }
                    projectRepository.CreateProject(projectModel);

                    return RedirectToAction("Index");
                }
                return View(projectModel);
            }
            catch
            {
                return View(projectModel);
            }
        }
        public ActionResult Edit(Guid ProjectId)
        {
            try
            {
                ProjectModel projectModel = projectRepository.GetProjectByProjectId(ProjectId);
                return View(projectModel);
            }
            catch 
            { 
                return RedirectToAction("Index", "Home"); 
            }
        }
        [HttpPost]
        public ActionResult Edit(Guid ProjectId, FormCollection collection)
        {
            ProjectModel projectModel = new ProjectModel();
            try
            {
                UpdateModel(projectModel);
                if (projectModel.StartDate > DateTime.Now && projectModel.EndDate > DateTime.Now)
                {
                    projectModel.StatusId = statusRepository.GetStatuses().FirstOrDefault(x => x.StatusName == "Not Started").StatusId;
                }
                else if (projectModel.StartDate <= DateTime.Now && projectModel.EndDate > DateTime.Now)
                {
                    projectModel.StatusId = statusRepository.GetStatuses().FirstOrDefault(x => x.StatusName == "In Progress").StatusId;
                }
                projectRepository.UpdateProject(projectModel);

                return RedirectToAction("Index");
            }
            catch
            {
                return View(projectModel);
            }
        }
        public ActionResult Delete(Guid ProjectId)
        {
            try
            {
                return View();
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }

        }
        [HttpPost]
        public ActionResult Delete(Guid ProjectId, FormCollection collection)
        {
            try
            {
                ProjectModel projectModel = projectRepository.GetProjectByProjectId(ProjectId);
                projectRepository.DeleteProject(projectModel);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        public ActionResult SetAsDone(Guid ProjectId)
        {
            try
            {
                ProjectModel projectModel = projectRepository.GetProjectByProjectId(ProjectId);
                List<IssueModel> issuesInThisProject = issueRepository.GetIssuesByProjectId(ProjectId);
                StatusModel status = projectModel.StatusList.Find(x => x.StatusName == "Finished");
                foreach (var issue in issuesInThisProject)
                {
                    List<ActionModel> actionsInThisIssue = actionRepository.GetActionsByIssueId(issue.IssueId);
                    foreach (var action in actionsInThisIssue)
                    {
                        action.StatusId = status.StatusId;
                        actionRepository.UpdateAction(action);
                    }
                    issue.StatusId = status.StatusId;
                    issueRepository.UpdateIssue(issue);
                }
                projectModel.StatusId = status.StatusId;
                projectRepository.UpdateProject(projectModel);
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}

