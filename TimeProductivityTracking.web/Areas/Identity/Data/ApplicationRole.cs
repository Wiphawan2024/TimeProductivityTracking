using System;
using Microsoft.AspNetCore.Identity;
namespace TimeProductivityTracking.web.Areas.Identity.Data
{
    public class ApplicationRole : IdentityRole<Guid>
    {
        public string Description { get; set; }
    }
}
