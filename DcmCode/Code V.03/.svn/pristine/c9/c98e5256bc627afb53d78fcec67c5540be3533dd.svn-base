using Dcm.EntityModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Dcm.Models
{
    public class Task
    {
        [Required]
        public string RecordId { get; set; }
        public string MenuId { get; set; }

        public string task_name { get; set; }

        public string task_description { get; set; }

        public string task_type_id { get; set; }
        public string task_status_id { get; set; }
        public string task_priority_id { get; set; }

        public string related_project_id { get; set; }
        public string task_user_id { get; set; }

        public string assigned_user_id { get; set; }
        
        public string explanations { get; set; }

        public string new_explanation { get; set; }
        
        public bool is_active { get; set; }

        public bool is_task_sent { get; set; }

        public DateTime start_date { get; set; }

        public DateTime end_date { get; set; }

        public string FromDeleteButton { get; set; }

        public string FromCreateTaskButton { get; set; }

        public string FromAddExplanationButton { get; set; }

        public int OrderId { get; set; }

        public List<crm_projects> activeProjects { get; set; }

        public List<ref_tsk_task_priority> taskPriority { get; set; }

        public List<ref_tsk_task_status> taskStatus { get; set; }

        public List<gnl_users> activeUsers { get; set; }

        public List<tsk_task_history_v> taskHistoryList { get; set; }



    }
}