using IssueTracker.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IssueTracker.Models
{
    public class ProjectModel
    {
        public Guid ProjectId { get; set; }
        public Guid TeamId { get; set; }
        public string ProjectName { get; set; }
        public string ProjectDescription { get; set; }


        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Guid StatusId { get; set; }

        public List<TeamModel> teamList { get; set; }
        public ProjectModel()
        {
            TeamRepository teamRepository = new TeamRepository();
            teamList = teamRepository.GetTeamsCreatedBy();
        }

    }
}