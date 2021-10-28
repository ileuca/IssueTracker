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
        private readonly UserRepository UserRepository = new UserRepository();
        private readonly TeamRepository TeamRepository = new TeamRepository();
        private readonly StatusRepository StatusRepository = new StatusRepository();
        public ActionResult Index()
        {
            User currentUser = UserRepository.GetCurrentUser();
            List<TeamModel> teamList = TeamRepository.GetTeamsForCurrentUserId(currentUser.UserId);
            List<ProjectModel> projectsByUser = new List<ProjectModel>();
            foreach(var team in teamList)
            {
                List<ProjectModel> projectsByTeam = projectRepository.GetProjectsByTeamId(team.TeamId);
                foreach (var project in projectsByTeam)
                {
                    projectsByUser.Add(project);
                }
            }
            return View("Index",projectsByUser);
        }
        public ActionResult Create()
        {
            ProjectModel projectModel = new ProjectModel();
            return View(projectModel);
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
                    //de modificat sa verifice datele pentru status
                    projectModel.StatusId = StatusRepository.GetStatuses().FirstOrDefault(x => x.StatusName == "In Progress").StatusId;
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
            ProjectModel projectModel = projectRepository.GetProjectByProjectId(ProjectId);
            return View(projectModel);
        }
        [HttpPost]
        public ActionResult Edit(Guid ProjectId, FormCollection collection)
        {
            ProjectModel projectModel = new ProjectModel();
            try
            {
                UpdateModel(projectModel);
                //de modificat sa verifice datele pentru status
                projectModel.StatusId = StatusRepository.GetStatuses().FirstOrDefault(x => x.StatusName == "In Progress").StatusId;
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
            return View();
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
    }
}
