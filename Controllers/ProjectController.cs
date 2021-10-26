using IssueTracker.Models;
using IssueTracker.Models.DBObjects;
using IssueTracker.Repository;
using IssueTracker.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IssueTracker.Controllers
{
    public class ProjectController : Controller
    {
        private ProjectRepository projectRepository = new ProjectRepository();
        private UserRepository UserRepository = new UserRepository();
        private TeamRepository TeamRepository = new TeamRepository();
        private StatusRepository StatusRepository = new StatusRepository();
        private ActionRepository ActionRepository = new ActionRepository();

        // GET: Project
        public ActionResult Index()
        {
            User currentUser = UserRepository.GetCurrentUser();
            List<TeamModel> teamList = TeamRepository.GetTeamsForCurrentUserId(currentUser.UserId);
            List<ProjectModel> projectsByTeam = new List<ProjectModel>();
            List<ProjectModel> projectsByUser = new List<ProjectModel>();
            
            foreach(var team in teamList)
            {
                projectsByTeam = projectRepository.GetProjectsByTeamId(team.TeamId);
                foreach(var project in projectsByTeam)
                {
                    projectsByUser.Add(project);
                }
            }

            return View("Index",projectsByUser);
        }

        // GET: Project/Details/5
        public ActionResult Details(int id)
        {

            return View();
        }

        // GET: Project/Create
        public ActionResult Create()
        {
            ProjectModel projectModel = new ProjectModel();
            return View(projectModel);
        }

        // POST: Project/Create
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

        // GET: Project/Edit/5
        public ActionResult Edit(Guid ProjectId)
        {
            ProjectModel projectModel = projectRepository.GetProjectByProjectId(ProjectId);
            return View(projectModel);
        }

        // POST: Project/Edit/5
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

        // GET: Project/Delete/5
        public ActionResult Delete(Guid ProjectId)
        {
            return View();
        }

        // POST: Project/Delete/5
        [HttpPost]
        public ActionResult Delete(Guid ProjectId, FormCollection collection)
        {
            ProjectModel projectModel = new ProjectModel();

            try
            {
                projectModel = projectRepository.GetProjectByProjectId(ProjectId);
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
