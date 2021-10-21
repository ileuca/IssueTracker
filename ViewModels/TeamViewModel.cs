using IssueTracker.Models;
using IssueTracker.Models.DBObjects;
using IssueTracker.Repository;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IssueTracker.ViewModels
{
    public class TeamViewModel
    {
        public Guid TeamId { get; set; }
        public string TeamName { get; set; }
        public string TeamDescription { get; set; }
        public Guid UserId { get; set; }
        public string UserNameSurname { get; set; }
        public string UserDescription { get; set; }
        public Guid TeamRoleId { get; set; }
        public string TeamRoleName { get; set; }
        public List<UserModel> userList { get; set; }
        public List<UserTeamRoleModel> userTeamRoleModelList { get; set; }

        public TeamViewModel()
        {

            UserRepository userRepository = new UserRepository();
            UserTeamRoleRepository userTeamRoleRepository = new UserTeamRoleRepository();

            userList = userRepository.GetAllUsers();
            userTeamRoleModelList = userTeamRoleRepository.GetTeamRoleModels();
        }
    }

}