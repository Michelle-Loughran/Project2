using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


    namespace CMS.Web.Models
    {
       public class AboutViewModel
       {
          public string Title { get; set; }
          public string Message { get; set; }
          public DateTime Formed { get; set; } = DateTime.Now;
          public string FormedString => Formed.ToLongDateString();
          public int Days => (DateTime.Now - Formed).Days;
       }
    }
