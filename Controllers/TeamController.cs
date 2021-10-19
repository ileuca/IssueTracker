﻿using IssueTracker.Models;
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
        private Repository.UserRepository UserRepository = new Repository.UserRepository();
        private Repository.UserTeamRoleRepository UserTeamRoleRepository = new Repository.UserTeamRoleRepository();
        private Repository.TeamGroupRepository TeamGroupRepository = new Repository.TeamGroupRepository();

        // GET: Team
        public ActionResult Index()
        {
            List<TeamModel> userTeamRoleModels = new List<TeamModel>();
            userTeamRoleModels = TeamRepository.GetTeamsCreatedBy();
            return View("Index", userTeamRoleModels);
        }




        public ActionResult AddUser(Guid id)
        {
            TeamViewModel teamViewModel = new TeamViewModel();
            if (TeamViewRepository.GetTEamViewModelsByTeamId(id).Find(x => x.TeamId == id) != null)
            {
                teamViewModel.TeamId = TeamViewRepository.GetTEamViewModelsByTeamId(id).Find(x => x.TeamId == id).TeamId;
                return View("AddUser", teamViewModel);
            }
            else
            {
                teamViewModel.TeamId = id;
                return View("AddUser", teamViewModel);
            }
        }

        [HttpPost]
        public ActionResult AddUser(FormCollection collection)
        {
            dynamic teamGroups = new ExpandoObject();
            try
            {      //check if users exists in team and change only ream role if necesarry

                TeamViewModel teamViewModel = new TeamViewModel();
                UpdateModel(teamViewModel);
                TeamViewRepository.CreateTeamGroup(teamViewModel);
                return RedirectToAction("Details",new { id = teamViewModel.TeamId });
            }
            catch
            {
                return View();
            }


        }
        
        // GET: Team/Details/5
        public ActionResult Details(Guid id)
        {
            List<TeamViewModel> teamViewModels = TeamViewRepository.GetTEamViewModelsByTeamId(id);
            if (TeamViewRepository.GetTEamViewModelsByTeamId(id).Count == 0)
            {
                TeamViewModel teamViewModel = new TeamViewModel();
                teamViewModel.TeamId = id;
                teamViewModel.UserNameSurname = "";
                teamViewModels.Add(teamViewModel);
                return View("Details", teamViewModels);
            }
            
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
        public ActionResult DeleteTeam(Guid TeamId)
        {
            ViewBag.TeamName = TeamRepository.GetTeamsCreatedBy().Find(x => x.TeamId == TeamId).TeamName;
            return View();
        }

        [HttpPost]
        public ActionResult DeleteTeam(Guid TeamId, FormCollection collection)
        {
            try
            {
                TeamModel teamModel = new TeamModel();
                teamModel.TeamId = TeamId;
                TeamRepository.DeleteTeam(teamModel);

                return RedirectToAction("Details", new { id = teamModel.TeamId });
            }
            catch
            {
                return View();
            }
        }
        // GET: Team/Details/AddUser/Delete/5
        public ActionResult DeleteUserFromTeam(Guid TeamId, Guid UserId, Guid TeamRoleId)
        {
            ViewBag.UserName = UserRepository.GetUserById(UserId).UserName;
            ViewBag.TeamRoleName = UserTeamRoleRepository.GetTeamRoleModels().Find(x => x.userTeamRoleId == TeamRoleId).UserTeamRoleName;
            return View();
        }

        // POST: Team/Details/AddUser/Delete/5
        [HttpPost]
        public ActionResult DeleteUserFromTeam(Guid TeamId,Guid UserId,Guid TeamRoleId, FormCollection collection)
        {
            try
            {
                TeamViewModel teamViewModel = new TeamViewModel();
                teamViewModel.UserId = UserId;
                teamViewModel.TeamId = TeamId;
                teamViewModel.TeamRoleId = TeamRoleId;
                TeamGroupRepository.DeleteTeamGroup(teamViewModel);

                return RedirectToAction("Details", new { id = teamViewModel.TeamId });
            }
            catch
            {
                return View();
            }
        }
    }
}
