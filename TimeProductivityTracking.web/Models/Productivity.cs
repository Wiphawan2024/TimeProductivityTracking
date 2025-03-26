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
        public DateTime? Date { get; set; }
        public string? Monthly { get; set; }
        [DisplayName("SEC Name")]
        public string SECName { get; set; }
        [DisplayName("County")]
        public Counties? County { get; set; }
        [DisplayName("Planned Days")]
        public Decimal? PlannedDays { get; set; }
        [DisplayName("Task_P")]
        public Tasks? Task_P { get; set; }
        [DisplayName("Mentor")]
        public string? CountryMentor_P { get; set; }

        public Decimal? AchevedDays { get; set; }
        public Tasks? Tasks_A { get; set; }
        public string? CountryMentor_A { get; set; }
        [DisplayName("User Email")]
        public string? UserEmail { get; set; }
        [ForeignKey("Contractor")]
        public int? ContractorId { get; set; }

        public UserInfo? Contractor { get; set; }
     
    }

    public enum Counties { Longford,Roscommon,Sligo,Donegal,Leitrim,Mayo,Meath,Wexford,Wicklow }
    public enum Tasks
    { [Description("New SEC Registered")] NewSECRegistered = 0,
      [Description("Energy Master Plan(EMP)")] EMP =1,
      [Description("EMP Completed")] EMPCompleted=2,
      [Description("Project - Community Retrofit started")] ProjectCommunityRetrofitStarted=3
    }


}

