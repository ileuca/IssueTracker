using IssueTracker.Models;
using IssueTracker.Models.DBObjects;
using IssueTracker.ViewModels;
using System.Collections.Generic;

namespace IssueTracker.Repository
{
    public class TeamGroupRepository
    {
        private readonly Models.DBObjects.IssueTrackerModelsDataContext dbContext;
        public TeamGroupRepository()
        {
            this.dbContext = new Models.DBObjects.IssueTrackerModelsDataContext();
        }
        public TeamGroupRepository(Models.DBObjects.IssueTrackerModelsDataContext dbContext)
        {
            this.dbContext = dbContext;
        }
        private TeamGroupsModel MapDbObjectToModel(TeamGroup dbteamGroup)
        {
            TeamGroupsModel teamGroupModel = new TeamGroupsModel();
            if (dbteamGroup != null)
            {
                teamGroupModel.TeamGroupId = dbteamGroup.TeamGroupID;
                teamGroupModel.TeamId = dbteamGroup.TeamId;
                teamGroupModel.UserId = dbteamGroup.UserId;
                teamGroupModel.UserTeamRoleId = dbteamGroup.UserTeamRoleId;

                return teamGroupModel;
            }
            return null;
        }
        private TeamGroup MapModelToDbObject(TeamGroupsModel teamGroupModel)
        {
            TeamGroup dbteamGroup = new TeamGroup();
            if (dbteamGroup != null)
            {
                dbteamGroup.TeamGroupID = teamGroupModel.TeamGroupId;
                dbteamGroup.TeamId = teamGroupModel.TeamId;
                dbteamGroup.UserId = teamGroupModel.UserId;
                dbteamGroup.UserTeamRoleId = teamGroupModel.UserTeamRoleId;

                return dbteamGroup;
            }
            return null;
        }
        public void CreateTeamGroup(TeamGroupsModel teamGroupsModel)
        {
            dbContext.TeamGroups.InsertOnSubmit(MapModelToDbObject(teamGroupsModel));
            dbContext.SubmitChanges();
        }

        public List<TeamGroupsModel> GetAllTeamGroups()
        {
            List<TeamGroupsModel> teamGroupModels = new List<TeamGroupsModel>();
            foreach(TeamGroup teamGroup in dbContext.TeamGroups)
            {
                teamGroupModels.Add(MapDbObjectToModel(teamGroup));
            }
            return teamGroupModels;
        }
        public void DeleteTeamGroup(TeamViewModel teamViewModel)
        {
            foreach (var item in dbContext.TeamGroups)
            {
                if (item.UserId == teamViewModel.UserId && item.TeamId == teamViewModel.TeamId && item.UserTeamRoleId == teamViewModel.TeamRoleId)
                {
                    dbContext.TeamGroups.DeleteOnSubmit(item);
                    dbContext.SubmitChanges();
                    return;
                }
            }
            return;
        }
    }
}