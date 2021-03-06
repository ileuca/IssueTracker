using IssueTracker.Models;
using System.Collections.Generic;
using IssueTracker.Models.DBObjects;

namespace IssueTracker.Repository
{
    public class UserTeamRoleRepository
    {
        private readonly IssueTrackerModelsDataContext dbContext;

        public UserTeamRoleRepository()
        {
            this.dbContext = new IssueTrackerModelsDataContext();
        }
        public UserTeamRoleRepository(IssueTrackerModelsDataContext dbContext)
        {
            this.dbContext = dbContext;
        }
        private UserTeamRoleModel MapDbObjectToModel(UserTeamRole dbUserTeamRole)
        {
            UserTeamRoleModel userTeamRoleModel = new UserTeamRoleModel();
            if (dbUserTeamRole != null)
            {
                userTeamRoleModel.UserTeamRoleId = dbUserTeamRole.UserTeamRoleId;
                userTeamRoleModel.UserTeamRoleName = dbUserTeamRole.UserTeamRoleName;

                return userTeamRoleModel;
            }
            return null;
        }
        //Read
        public List<UserTeamRoleModel> GetTeamRoleModels()
        {
            List<UserTeamRoleModel> userTeamRoleModelList = new List<UserTeamRoleModel>();
            foreach (UserTeamRole dbUserTeamRole in dbContext.UserTeamRoles)
            {
                userTeamRoleModelList.Add(MapDbObjectToModel(dbUserTeamRole));
            }
            return userTeamRoleModelList;
        }
    }
}