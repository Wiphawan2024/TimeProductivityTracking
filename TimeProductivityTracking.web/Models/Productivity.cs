using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeProductivityTracking.web.Models
{
    public class Productivity
    {

        public int Id { get; set; }
        [DisplayName("Date")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        public string? Monthly { get; set; }
        [DisplayName("SEC Name")]
        public string? SECName { get; set; }
        [DisplayName("County")]

        public Counties County { get; set; }

        [DisplayName("Planned This Month")]
        public Decimal PlannedDays { get; set; }


        [DisplayName("Task_P")]
        public Tasks Task_P { get; set; }
        [DisplayName("Mentor")]
        public string? CountryMentor_P { get; set; }

        public Decimal AchevedDays { get; set; }
        public Tasks Tasks_A { get; set; }
        public string? CountryMentor_A { get; set; }
        [DisplayName("User Email")]
        public string? UserEmail { get; set; }
        [ForeignKey("Contractor")]
        public int? ContractorId { get; set; }
        public UserInfo? Contractor { get; set; }
        public string? statusApproval { get; set; }
    }

    public enum Counties { Longford, Westmeath, Offaly , Laois }
    public enum Tasks
    { [Description("New SEC Registered")] NewSECRegistered = 0,
      [Description("EMP application submitted")] EMPApplicationSubmitted =1,
      [Description("EMP Completed")] EMPCompleted=2,
      [Description("Project - Indiviudual Solar PV started")] ProjectIndiviudualSolarPVstarted=3,
      [Description("Project - Indiviudual Solar PV completed")] ProjectIndividualSolarPVcompleted=4,
      [Description("Project - Community Retrofit started")]ProjectCommunityRetrofitstarted=5,
      [Description("Project - Community Retrofit completed")]ProjectCommunityRetrofitcompleted=6,
      [Description("Project - Community Solar 'Meitheal' started")]ProjectCommunitySolarMeithealstarted=7,
      [Description("Project - Community Solar 'Meitheal' completed")]ProjectCommunitySolarMeithealcompleted=8,
      [Description("Event - Online Webinar")]EventOnlineWebinar=9,
      [Description("Event - In-person Workshop")]EventInpersonWorkshop=10,
      [Description("None - No completions this month")]NoneNoCompletionsThisMonth=11,
      [Description("None - No planned actvities")]NoneNoPlannedActivities=12


    }


}

