using IssueTracker.Models;
using IssueTracker.Models.DBObjects;
using IssueTracker.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IssueTracker.Repository
{
    public class TeamViewRepository
    {
        private readonly Models.DBObjects.IssueTrackerModelsDataContext dbContext;
        private readonly UserRepository userRepository = new UserRepository();
        private readonly TeamRepository teamRepository = new TeamRepository();
        private readonly TeamGroupRepository teamGroupRepository = new TeamGroupRepository();
        private readonly UserTeamRoleRepository userTeamRoleRepository = new UserTeamRoleRepository();
        public TeamViewRepository()
        {
            this.dbContext = new Models.DBObjects.IssueTrackerModelsDataContext();
        }
        public TeamViewRepository(Models.DBObjects.IssueTrackerModelsDataContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public TeamViewModel MapModelsToTeamViewModel(TeamGroupsModel teamGroupsModel)
        {
            TeamViewModel teamViewModel = new TeamViewModel
            {
                TeamId = teamGroupsModel.TeamId,
                TeamName = teamRepository.GetTeamById(teamGroupsModel.TeamId).TeamName,
                TeamDescription = teamRepository.GetTeamById(teamGroupsModel.TeamId).TeamDescription,
                UserId = teamGroupsModel.UserId,
                UserDescription = userRepository.GetUserById(teamGroupsModel.UserId).UserDescription,
                UserNameSurname = userRepository.GetUserById(teamGroupsModel.UserId).UserName,
                TeamRoleId = teamGroupsModel.UserTeamRoleId,
                TeamRoleName = userTeamRoleRepository.GetTeamRoleModels().FirstOrDefault(x => x.UserTeamRoleId == teamGroupsModel.UserTeamRoleId).UserTeamRoleName
            };
            return teamViewModel;
        }
        public TeamGroup MapTeamViewModelToTeamGroup(TeamViewModel teamViewModel)
        {
            TeamGroup teamGroup = new TeamGroup
            {
                TeamId = teamViewModel.TeamId,
                UserId = teamViewModel.UserId,
                UserTeamRoleId = teamViewModel.TeamRoleId
            };
            return teamGroup;
        }
        //Create -- CreateTeamView(CreateTeam + CreateTeamGroup)
        public void CreateTeamGroup(TeamViewModel teamViewModel)
        {  
            dbContext.TeamGroups.InsertOnSubmit(MapTeamViewModelToTeamGroup(teamViewModel));
            dbContext.SubmitChanges();
        }
        //Read
        public List<UserModel> GetUsersByTeamId(Guid teamId)
        {
            List<UserModel> userList = userRepository.GetAllUsers();
            List<TeamGroupsModel> teamGroupModelList = teamGroupRepository.GetAllTeamGroups();
            List<UserModel> userListByTeam = new List<UserModel>();

            foreach (TeamGroupsModel teamGroupModel in teamGroupModelList)
            {
                foreach (UserModel userModel in userList)
                {
                    if (userModel.UserId == teamGroupModel.UserId && teamId == teamGroupModel.TeamId)
                    {
                        userListByTeam.Add(userModel);
                    }
                }
            }
            return userListByTeam;
        }
        public List<TeamViewModel> GetAllTeamViewModels()
        {
            List<TeamViewModel> teamViewModels = new List<TeamViewModel>();
            foreach(TeamGroupsModel teamGroupsModel in GetAllTeamGroups())
            {
                teamViewModels.Add(MapModelsToTeamViewModel(teamGroupsModel));
            }
            return teamViewModels;
        }
        public List<TeamViewModel> GetTeamViewModelsByTeamId(Guid id)
        {
            List<TeamViewModel> teamViewModels = new List<TeamViewModel>();
            foreach(TeamViewModel teamViewModel in GetAllTeamViewModels())
            {
                if (teamViewModel.TeamId == id)
                {
                    teamViewModels.Add(teamViewModel);
                }
            }
            return teamViewModels;
        }
        //GetAllTEamGroups
        public List<TeamGroupsModel> GetAllTeamGroups()
        {
            return teamGroupRepository.GetAllTeamGroups();
        }
        //Update
        public void UpdateTeamGroup(TeamViewModel teamViewModel)
        {
            TeamGroup existingTeamGroup = dbContext.TeamGroups
                .Where(x => x.TeamId == teamViewModel.TeamId)
                .FirstOrDefault(x => x.UserId == teamViewModel.UserId);
            existingTeamGroup.UserTeamRoleId = teamViewModel.TeamRoleId;
            dbContext.SubmitChanges();
        }
    }

}