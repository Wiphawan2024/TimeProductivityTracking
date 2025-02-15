﻿using System.ComponentModel.DataAnnotations.Schema;

namespace TimeProductivityTracking.web.Models
{
    public class SECContract
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SECContractID { get; set; }
        public string SECName { get; set; }
        public string County { get; set; }
        public string Address { get; set; }
        public string PrimaryContract { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

    }
}
