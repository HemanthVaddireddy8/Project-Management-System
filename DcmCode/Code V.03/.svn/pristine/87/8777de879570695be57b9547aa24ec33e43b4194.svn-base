using BaseDB;
using Dcm.Filters;
using Dcm.Models;
using Dcm.Source;
using Google.Apis.Drive.v2;
using Google.Apis.Drive.v2.Data;
using Gunluk.EntityModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using Microsoft.Reporting.WebForms;
using Dcm.EntityModels;

namespace Dcm.Controllers
{
    public class ReportController : Controller
    {
        [HttpGet]
        [AllowAnonymous]
        [SessionCheckAttribute]
        public ActionResult TaskReport( string MenuId)
        {
            TaskRepository tskDB = RepositoryManager.GetRepository<TaskRepository>();
            GenelRepository gnlDB = RepositoryManager.GetRepository<GenelRepository>();
            var totalActiveTasks = tskDB.GetTaskList(SessionContext.Current.ActiveUser.UserUid.ToString(),DateTime.MinValue,DateTime.MaxValue);

            TaskReport model = new TaskReport();
            #region Ortak Set Edilecek Değerler
            model.MenuId = MenuId;
            SessionContext.Current.ActiveUser.MenuId = MenuId;
            #endregion

            model.taskStatus = tskDB.GetTaskStatusList();
            model.task_status_id = "0";

            model.activeUsers = gnlDB.GetActiveUsers();

            model.assigned_user_id = SessionContext.Current.ActiveUser.UserUid.ToString();

            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;

            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Report/TaskReport.rdlc";

            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("TaskReportDataSet", totalActiveTasks));
            reportViewer.Height = 450;
            reportViewer.Width = 1000;
            reportViewer.AsyncRendering = false;
            
            ViewBag.ReportViewer = reportViewer;
            return View(model);
        }


        [HttpPost]
        [SessionCheckAttribute]
        public ActionResult TaskReport(TaskReport model)
        {

            TaskRepository tskDB = RepositoryManager.GetRepository<TaskRepository>();
            GenelRepository gnlDB = RepositoryManager.GetRepository<GenelRepository>();

            #region Ortak Set Edilecek Değerler
            SessionContext.Current.ActiveUser.MenuId = model.MenuId;
            ViewBag.Success = true;
            #endregion

            model.activeUsers = gnlDB.GetActiveUsers();


            DateTime startDate = DateTime.MinValue;
            DateTime endDate = DateTime.MaxValue;

            if (model.start_date != null && model.start_date.CompareTo(DateTime.MinValue) != 0 && model.start_date.ToString() != "")
                startDate = Convert.ToDateTime(model.start_date);

            if (model.end_date != null && model.end_date.CompareTo(DateTime.MinValue) != 0 && model.end_date.ToString() != "")
                endDate = Convert.ToDateTime(model.end_date);


            List<tsk_tasks_v> taskList = null;

            if (!string.IsNullOrEmpty(model.task_status_id))
            {
                if (model.task_status_id == "0")
                {


                    taskList = tskDB.GetTaskList(model.assigned_user_id, startDate, endDate);
                }
                else
                {

                    taskList = tskDB.GetTaskListByStatusId(Convert.ToInt32(model.task_status_id), startDate, endDate, model.assigned_user_id);
                }
            }
            else
            {
                taskList = tskDB.GetTaskList(model.assigned_user_id, startDate, endDate);
            }

            model.taskStatus = tskDB.GetTaskStatusList();


            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;

            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Report/TaskReport.rdlc";

            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("TaskReportDataSet", taskList));
            reportViewer.Height = 450;
            reportViewer.Width = 1000;
            reportViewer.AsyncRendering = false;

            ViewBag.ReportViewer = reportViewer;

            return View(model);
        }
    }
}