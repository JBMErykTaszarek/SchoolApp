using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SchoolApp.Models
{
    public class SchoolTask
    {
        [Key]
        public int Id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string whichClas { get; set; }
        public bool IsNoted { get; set; }
        
    }
}