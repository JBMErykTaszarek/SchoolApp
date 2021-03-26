using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SchoolApp.Models
{
    public class Note
    {
        [Key]
        public int Id { get; set; }
        public string ForTask { get; set; }
        public string WchitNote { get; set; }
        public string email { get; set; }

    }
}