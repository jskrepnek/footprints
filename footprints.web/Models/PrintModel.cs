﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace footprints.web.Models
{
    [Serializable()]
    public class PrintModel
    {
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Phrase")]
        public string Phrase { get; set; }

        [DisplayFormat(DataFormatString="{0:f}")]
        public DateTime Date { get; set; }
    }
}