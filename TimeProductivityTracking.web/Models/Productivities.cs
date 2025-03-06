using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeProductivityTracking.web.Models
{
    public class Productivities
    {
       
        public int Id{ get; set; }
        [DisplayName("Monthly")]
        [DataType(DataType.Date)]

        public DateTime? Monthly { get; set; } 
        [DisplayName("SEC Name")]
        public string SECName { get; set; }
        [DisplayName("County")]
        public Counties? County { get; set; }
        [DisplayName("Planned Days")]
        public int?  PlannedDays{  get; set; }
        [DisplayName("Task_P")]
        public Tasks?  Task_P { get; set; }
        [DisplayName("Mentor")]
        public string? CounryMentor_P { get; set; }

        public int? AchevedDays { get; set; }
        public Tasks? Tasks_A {  get; set; }
        public string? CounryMentor_A { get; set; }

      //  [ForeignKey("Contractor")]
        public int ContractorId_FK { get; set; }
       // [ForeignKey("SECContract")]


    }
   
    public enum Counties { Longford,Roscommon,Sligo,Donegal,Leitrim,Mayo,Meath,Wexford,Wicklow }
    public enum Tasks
    { [Description("New SEC Registered")] NewSECRegistered = 0,
      [Description("Energy Master Plan(EMP)")] EMP =1,
      [Description("EMP Completed")] EMPCompleted=2,
      [Description("Project - Community Retrofit started")] ProjectCommunityRetrofitStarted=3
    }


}

