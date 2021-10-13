using IssueTracker.Models;
using IssueTracker.Models.DBObjects;
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
        public UserModel TeamUser { get; set; }
        public UserTeamRoleModel TeamRole { get; set; }
    }

}