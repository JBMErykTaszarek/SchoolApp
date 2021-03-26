using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SchoolApp.Models
{
    public class Message
    {   
        [Key]
        public int Id { get; set; }
        public string from { get; set; }
        public string to { get; set; }
        public string messageContent { get; set; }
        public bool isReaded { get; set; }
    }
}