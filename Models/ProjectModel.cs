using IssueTracker.Repository;
using IssueTracker.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IssueTracker.Models
{
    public class ProjectModel
    {
        public Guid ProjectId { get; set; }
        public Guid TeamId { get; set; }
        [Required]
        public string ProjectName { get; set; }
        [Required]
        [DataType(DataType.MultilineText)]
        public string ProjectDescription { get; set; }

        [DateCheck("StartDate", "EndDate", Compare.LessThan)]
        public DateTime? StartDate { get; set; }

        [DateCheck("EndDate","StartDate", Compare.GreaterThan)]
        public DateTime? EndDate { get; set; }
        public Guid StatusId { get; set; }
        public List<TeamModel> TeamList { get; set; }
        public List<StatusModel> StatusList { get; set; }
        public ProjectModel()
        {
            UserRepository userRepository = new UserRepository();
            TeamRepository teamRepository = new TeamRepository();
            TeamList = teamRepository.GetTeamsForCurrentUserId(userRepository.GetCurrentUser().UserId);
            StatusRepository statusRepository = new StatusRepository();
            StatusList = statusRepository.GetStatuses();
        }

    }
}