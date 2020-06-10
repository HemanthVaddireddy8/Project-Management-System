using Dcm.EntityModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Dcm.Models
{
    public class TaskReport
    {
        
        public string MenuId { get; set; }
        public List<ref_tsk_task_status> taskStatus { get; set; }

        public string task_status_id { get; set; }

        public DateTime start_date { get; set; }

        public DateTime end_date { get; set; }

        public List<gnl_users> activeUsers { get; set; }

        public string assigned_user_id { get; set; }
       
    }
}