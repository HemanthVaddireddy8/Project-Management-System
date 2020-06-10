using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using Dcm.EntityModels;

namespace Dcm.Models
{
    public class Group
    {
        [Required]
        public string RecordId  {get;set;}
        public string MenuId { get; set; }

        [Required(ErrorMessage = "Grup name is necessary.")]
        public string group_name {get;set;}

        [Required(ErrorMessage = "Description is necessary.")]
        public string group_explanation { get; set; }

        public bool is_active {get;set;}

        public Guid group_id { get; set; }
        public string errorMessage { get; set; }
public string FromDeleteButton { get; set; }

public string ManagerId { get; set; }

public string ManagerName { get; set; }
        
    }

}