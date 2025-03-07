using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeProductivityTracking.web.Models
{
    public class Productivity
    {

        public int Id { get; set; }
        [DisplayName("Monthly")]
        [DataType(DataType.Date)]

        public DateTime? Monthly { get; set; }
        [DisplayName("SEC Name")]
        public string SECName { get; set; }
        [DisplayName("County")]
        public Counties? County { get; set; }
        [DisplayName("Planned Days")]
        public int? PlannedDays { get; set; }
        [DisplayName("Task_P")]
        public Tasks? Task_P { get; set; }
        [DisplayName("Mentor")]
        public string? CounryMentor_P { get; set; }

        public int? AchevedDays { get; set; }
        public Tasks? Tasks_A { get; set; }
        public string? CounryMentor_A { get; set; }
    //    public int? ContractorID { get; set; }
     //   public Contractor Contractor { get; set; }


    }
   // public class Contractor 
  //  {
   //     public int Id { get; set; }
   //     public List<Productivity> Productivities { get; set; }= new List<Productivity>();   
   // }
   
    public enum Counties { Longford,Roscommon,Sligo,Donegal,Leitrim,Mayo,Meath,Wexford,Wicklow }
    public enum Tasks
    { [Description("New SEC Registered")] NewSECRegistered = 0,
      [Description("Energy Master Plan(EMP)")] EMP =1,
      [Description("EMP Completed")] EMPCompleted=2,
      [Description("Project - Community Retrofit started")] ProjectCommunityRetrofitStarted=3
    }


}

