using IssueTracker.Models;
using IssueTracker.ViewModels;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Web.Mvc;

namespace IssueTracker.Controllers
{
    public class TeamController : Controller
    {
        private Repository.TeamRepository TeamRepository = new Repository.TeamRepository();
        private Repository.TeamViewRepository TeamViewRepository = new Repository.TeamViewRepository();
        // GET: Team
        public ActionResult Index()
        {
            List<TeamModel> userTeamRoleModels = new List<TeamModel>();
            userTeamRoleModels = TeamRepository.GetTeamsCreatedBy();
            return View("Index", userTeamRoleModels);
        }

        // GET: Team/Details/5
        public ActionResult Details(Guid id)
        {
            List<TeamViewModel> teamViewModels = TeamViewRepository.GetTEamViewModelsByTeamId(id);
            return View("Details", teamViewModels);
        }

        // GET: Team/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Team/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            dynamic teamGroups = new ExpandoObject();
            try
            {
                TeamModel teamModel = new TeamModel();
                UpdateModel(teamModel);
                TeamRepository.CreateTeam(teamModel);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Team/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Team/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Team/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Team/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
