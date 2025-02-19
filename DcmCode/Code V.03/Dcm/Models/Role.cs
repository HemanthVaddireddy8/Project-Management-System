﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using Dcm.EntityModels;

namespace Dcm.Models
{
    public class Role
    {
        [Required]
        public string RecordId  {get;set;}
        public string MenuId { get; set; }

        [Required(ErrorMessage = "Rol name is necessary.")]
        public string role_name {get;set;}

        [Required(ErrorMessage = "Description is necessary.")]
        public string role_explanation { get; set; }

        public string UserId { get; set; }

        public bool is_active {get;set;}

        public Guid role_id { get; set; }
        public Guid user_id { get; set; }
        public List<gnl_role_related_users_v> roleRelatedUsers { get; set; }
        public string errorMessage { get; set; }

        public string FromDeleteButton { get; set; }
        
    }

    public class RoleAuthorization
    {
        public string MenuId { get; set; }

        public string menu_id { get; set; }

        public Guid role_id { get; set; }

        public bool menu_right { get; set; }
        public bool update_right { get; set; }
        public bool delete_right { get; set; }
        public bool report_right { get; set; }


        public List<gnl_roles> activeRoles { get; set; }

        public string SelectedRoleId { get; set; }

        public string FromUpdateButton { get; set; }

    }

}