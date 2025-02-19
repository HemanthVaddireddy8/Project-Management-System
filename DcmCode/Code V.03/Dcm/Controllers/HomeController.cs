﻿using BaseDB;
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
    public class HomeController : Controller
    {
        [SessionCheckAttribute]
        public ActionResult Index()
        {
            Index model=new Models.Index();
            SessionContext.Current.ActiveUser.MenuId = "0";

            DataSet totalActiveTask = BaseDB.DBManager.AppConnection.GetDataSet("select count(*) kayit_sayisi from tsk_tasks_v where is_active_bit=@is_active_bit ", new ArrayList { "is_active_bit" }, new ArrayList { 1 });
            DataSet totalActiveFinishedTask = BaseDB.DBManager.AppConnection.GetDataSet("select count(*) kayit_sayisi  from tsk_tasks_v where is_active_bit=@is_active_bit and task_status_id=@task_status_id ", new ArrayList { "is_active_bit", "task_status_id" }, new ArrayList { 1, 6 });
            DataSet totalActiveClosedTask = BaseDB.DBManager.AppConnection.GetDataSet("select count(*) kayit_sayisi from tsk_tasks_v where is_active_bit=@is_active_bit and task_status_id=@task_status_id ", new ArrayList { "is_active_bit", "task_status_id" }, new ArrayList { 1, 7 });
            DataSet totalActiveProcessedTask = BaseDB.DBManager.AppConnection.GetDataSet("select count(*) kayit_sayisi from tsk_tasks_v where is_active_bit=@is_active_bit  and task_status_id in (2,3,4,8)  ", new ArrayList { "is_active_bit"}, new ArrayList { 1 });

            model.totalActiveTask = (totalActiveTask!=null && totalActiveTask.Tables[0].Rows.Count>0) ? totalActiveTask.Tables[0].Rows[0]["kayit_sayisi"].ToString():"0"; 
            model.totalActiveFinishedTask = (totalActiveFinishedTask!=null && totalActiveFinishedTask.Tables[0].Rows.Count>0) ? totalActiveFinishedTask.Tables[0].Rows[0]["kayit_sayisi"].ToString():"0"; 
            model.totalActiveClosedTask = (totalActiveClosedTask!=null && totalActiveClosedTask.Tables[0].Rows.Count>0) ? totalActiveClosedTask.Tables[0].Rows[0]["kayit_sayisi"].ToString():"0";
            model.totalActiveProcessedTask = (totalActiveProcessedTask != null && totalActiveProcessedTask.Tables[0].Rows.Count > 0) ? totalActiveProcessedTask.Tables[0].Rows[0]["kayit_sayisi"].ToString() : "0";

            DataSet totalActiveTaskMy = BaseDB.DBManager.AppConnection.GetDataSet("select count(*) kayit_sayisi from tsk_tasks_v where is_active_bit=@is_active_bit and assigned_user_id=@assigned_user_id", new ArrayList { "is_active_bit", "assigned_user_id" }, new ArrayList { 1, SessionContext.Current.ActiveUser.UserUid });
            DataSet totalActiveFinishedTaskMy = BaseDB.DBManager.AppConnection.GetDataSet("select count(*) kayit_sayisi  from tsk_tasks_v where is_active_bit=@is_active_bit and task_status_id=@task_status_id  and assigned_user_id=@assigned_user_id", new ArrayList { "is_active_bit", "task_status_id", "assigned_user_id" }, new ArrayList { 1, 6, SessionContext.Current.ActiveUser.UserUid });
            DataSet totalActiveClosedTaskMy = BaseDB.DBManager.AppConnection.GetDataSet("select count(*) kayit_sayisi from tsk_tasks_v where is_active_bit=@is_active_bit and task_status_id=@task_status_id and assigned_user_id=@assigned_user_id", new ArrayList { "is_active_bit", "task_status_id", "assigned_user_id" }, new ArrayList { 1, 7, SessionContext.Current.ActiveUser.UserUid });
            DataSet totalActiveProcessedTaskMy = BaseDB.DBManager.AppConnection.GetDataSet("select count(*) kayit_sayisi from tsk_tasks_v where is_active_bit=@is_active_bit  and task_status_id in (2,3,4,8) and  assigned_user_id=@assigned_user_id", new ArrayList { "is_active_bit", "assigned_user_id" }, new ArrayList { 1, SessionContext.Current.ActiveUser.UserUid });


            model.totalActiveTaskMy = (totalActiveTaskMy != null && totalActiveTaskMy.Tables[0].Rows.Count > 0) ? totalActiveTaskMy.Tables[0].Rows[0]["kayit_sayisi"].ToString() : "0";
            model.totalActiveFinishedTaskMy = (totalActiveFinishedTaskMy != null && totalActiveFinishedTaskMy.Tables[0].Rows.Count > 0) ? totalActiveFinishedTaskMy.Tables[0].Rows[0]["kayit_sayisi"].ToString() : "0";
            model.totalActiveClosedTaskMy = (totalActiveClosedTaskMy != null && totalActiveClosedTaskMy.Tables[0].Rows.Count > 0) ? totalActiveClosedTaskMy.Tables[0].Rows[0]["kayit_sayisi"].ToString() : "0";
            model.totalActiveProcessedTaskMy = (totalActiveProcessedTaskMy != null && totalActiveProcessedTaskMy.Tables[0].Rows.Count > 0) ? totalActiveProcessedTaskMy.Tables[0].Rows[0]["kayit_sayisi"].ToString() : "0";

            #region Görev Bitiş Tarihi Geçmiş Olanların Bildirimleri Oluşturuluyor

            try
            {
                TaskRepository tskDB = RepositoryManager.GetRepository<TaskRepository>();
                GenelRepository gnlDB = RepositoryManager.GetRepository<GenelRepository>();
                DataSet dsExpireTask = tskDB.SendTaskExpireNotification();

                foreach (DataRow row in dsExpireTask.Tables[0].Rows)
                {

                    gnlDB.AddNotification(Guid.Parse(row["task_id"].ToString()), Guid.Parse(row["task_user_id"].ToString()), (int)Enums.NotificationType.Expire, (int)Enums.NotificationModuleType.Task, Resources.GlobalResource.task_expire_end_date_title, Resources.GlobalResource.task_expire_end_date_body);
                    gnlDB.Kaydet();


                    gnlDB.AddNotification(Guid.Parse(row["task_id"].ToString()), Guid.Parse(row["assigned_user_id"].ToString()), (int)Enums.NotificationType.Expire, (int)Enums.NotificationModuleType.Task, Resources.GlobalResource.task_expire_end_date_title, Resources.GlobalResource.task_expire_end_date_body);
                    gnlDB.Kaydet();

                    tsk_tasks task = new tsk_tasks();
                    task = tskDB.GetTask(Guid.Parse(row["task_id"].ToString()));


                    tskDB.TaskMailSend(Guid.Parse(row["task_user_id"].ToString()), task.order_id.ToString(), task.task_name, task.task_id, "Task End Date Expired", "", "", "99");
                    tskDB.TaskMailSend(Guid.Parse(row["assigned_user_id"].ToString()), task.order_id.ToString(), task.task_name, task.task_id, "Task End Date Expired", "", "", "99");
                   
                }
            }
            catch(Exception exp)
            {
            
            }


            #endregion



            return View(model);
        }

        [SessionCheckAttribute]
        public ActionResult Calendar()
        {

            SessionContext.Current.ActiveUser.MenuId = "-1";
            CalendarList model = new CalendarList();
            //model.CalendarHiddenData = GetCalendarDataShow();
            return View(model);
        }

        public PartialViewResult MenuPartial()
        {
            var model = GetData();
            return PartialView("_MenuPartial", model);
        }

        public PartialViewResult MenuPartialMini()
        {
            var model = GetDataMini();
            return PartialView("_MenuPartialMini", model);
        }

        private Tree GetData()
        {
            var tree = new Tree();

            using (var db = new Dcm.EntityModels.GenelEntities())
            {
                // Add each element as a tree node
                tree.Nodes = db.gnl_menu
                    .Where(t => t.show_in_menu == 1 || t.show_in_menu == null)
                    .Select(t => new TreeNode { MenuId = t.menu_id, ParentMenuId = t.parent_menu_id.Value, Name = t.name, MenuOrder = t.menu_order.Value, PrimaryKey = t.primary_key, TableName = t.table_name, ObjectName = t.object_name, OnlyDetailPage = t.only_detail_page, DetailPageUrl=t.detail_page_url, IconClass =t.icon_class})
                    .ToDictionary(t => t.MenuId);

                // Create a new root node
                tree.RootNode = new TreeNode { MenuId = 0, Name = "Root" };

                // Build the tree, setting parent and children references for all elements
                tree.BuildTree();
            }

            return tree;
        }

        private TreeMini GetDataMini()
        {
            var tree = new TreeMini();

            using (var db = new Dcm.EntityModels.GenelEntities())
            {
                // Add each element as a tree node
                tree.Nodes = db.gnl_menu
                    .Where(t => t.show_in_menu == 1 || t.show_in_menu == null)
                    .Select(t => new TreeNodeMini { MenuId = t.menu_id, ParentMenuId = t.parent_menu_id.Value, Name = t.name, MenuOrder = t.menu_order.Value, PrimaryKey = t.primary_key, TableName = t.table_name, ObjectName = t.object_name, OnlyDetailPage = t.only_detail_page, DetailPageUrl = t.detail_page_url, IconClass = t.icon_class })
                    .ToDictionary(t => t.MenuId);

                // Create a new root node
                tree.RootNode = new TreeNodeMini { MenuId = 0, Name = "Root" };

                // Build the tree, setting parent and children references for all elements
                tree.BuildTreeMini();
            }

            return tree;
        }

        public ActionResult SetCookie()
        {
            bool result = false;
            if (BaseDB.SessionContext.Current == null || BaseDB.SessionContext.Current.ActiveUser == null)
            {
                HttpCookie ck = System.Web.HttpContext.Current.Request.Cookies["DCMGRUP23"];

                if (ck != null)
                {
                    try
                    {
                        BaseClasses.BaseLogin objLogin = new BaseClasses.BaseLogin();
                        FormsAuthenticationTicket oldTicket = FormsAuthentication.Decrypt(ck.Value);
                        result = objLogin.LoginFromRememberMe(oldTicket.Name);

                    }
                    catch (Exception exp)
                    {

                    }
                }

            }

            return Content(result.ToString(), "text/html"); ;


        }

        public JsonResult GetNotificationCount(string userId)
        {
            GenelRepository gnlDB = RepositoryManager.GetRepository<GenelRepository>();
            int count = 0;
            if (GlobalHelper.IsGuid(userId))
            {
                var list = gnlDB.GetActiveNotificationByUser(Guid.Parse(userId));

                if (list != null && list.Count > 0)
                {
                    count = list.Count;
                }
            }
         
            var modelNotification = new {
                count = count
            };

            return Json(modelNotification, JsonRequestBehavior.AllowGet);
        }


        [SessionCheckAttribute]
        public PartialViewResult NotificationList(string userId)
        {
            GenelRepository gnlDB = RepositoryManager.GetRepository<GenelRepository>();
            if (GlobalHelper.IsGuid(userId))
            {
                var list = gnlDB.GetActiveNotificationByUser(Guid.Parse(userId));
                Session["ListNotification"] = list;

            }

            return PartialView("NotificationList");
        }

        public JsonResult GetAllTaskStatusChartData()
        {
            TaskRepository tskDB = RepositoryManager.GetRepository<TaskRepository>();
            Index model = new Index();

            DataSet dsChart1 = tskDB.GetAllTaskChartData();
            model.allTaskStatusChartSourceData = new List<AllTaskStatusChartSourceData>();

            int counter = 0;
            foreach (DataRow dr in dsChart1.Tables[0].Rows)
            {
                AllTaskStatusChartSourceData item = new AllTaskStatusChartSourceData();
                item.label = dr["task_status_name"].ToString();
                item.value = Convert.ToInt32(dr["statu_count"].ToString());
                model.allTaskStatusChartSourceData.Add(item);
                counter++;
            }

            return Json(model.allTaskStatusChartSourceData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMyAssignedTaskStatusChartData()
        {
            TaskRepository tskDB = RepositoryManager.GetRepository<TaskRepository>();
            Index model = new Index();

            DataSet dsChart1 = tskDB.GetMyAssignedTaskChartData();

            model.myAssignedTaskStatusChartSourceData = new List<MyAssignedTaskStatusChartSourceData>();
            
            int counter = 0;
            foreach (DataRow dr in dsChart1.Tables[0].Rows)
            {
                MyAssignedTaskStatusChartSourceData item = new MyAssignedTaskStatusChartSourceData();
                item.label = dr["task_status_name"].ToString();
                item.value = Convert.ToInt32(dr["statu_count"].ToString());
                model.myAssignedTaskStatusChartSourceData.Add(item);
                counter++;
            }

            return Json(model.myAssignedTaskStatusChartSourceData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMyTaskStatusChartData()
        {
            TaskRepository tskDB = RepositoryManager.GetRepository<TaskRepository>();
            Index model = new Index();

            DataSet dsChart1 = tskDB.GetMyTaskChartData();

            model.myTaskStatusChartSourceData = new List<MyTaskStatusChartSourceData>();

            int counter = 0;
            foreach (DataRow dr in dsChart1.Tables[0].Rows)
            {
                MyTaskStatusChartSourceData item = new MyTaskStatusChartSourceData();
                item.label = dr["task_status_name"].ToString();
                item.value = Convert.ToInt32(dr["statu_count"].ToString());
                model.myTaskStatusChartSourceData.Add(item);
                counter++;
            }

            return Json(model.myTaskStatusChartSourceData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllCompletedTaskStatusChartData()
        {
            TaskRepository tskDB = RepositoryManager.GetRepository<TaskRepository>();
            Index model = new Index();

            DataSet dsChart1 = tskDB.GetAllCompletedTaskByMonthChartData();
            model.allCompletedTaskByMonthChartSourceData = new List<AllCompletedTaskByMonthChartSourceData>();

            int counter = 0;
            foreach (DataRow dr in dsChart1.Tables[0].Rows)
            {
                AllCompletedTaskByMonthChartSourceData item = new AllCompletedTaskByMonthChartSourceData();
                item.label = dr["month_str"].ToString();
                item.value = Convert.ToInt32(dr["statu_count"].ToString());
                model.allCompletedTaskByMonthChartSourceData.Add(item);
                counter++;
            }

            return Json(model.allCompletedTaskByMonthChartSourceData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllClosedTaskStatusChartData()
        {
            TaskRepository tskDB = RepositoryManager.GetRepository<TaskRepository>();
            Index model = new Index();

            DataSet dsChart1 = tskDB.GetAllClosedTaskByMonthChartData();
            model.allClosedTaskByMonthChartSourceData = new List<AllClosedTaskByMonthChartSourceData>();

            int counter = 0;
            foreach (DataRow dr in dsChart1.Tables[0].Rows)
            {
                AllClosedTaskByMonthChartSourceData item = new AllClosedTaskByMonthChartSourceData();
                item.label = dr["month_str"].ToString();
                item.value = Convert.ToInt32(dr["statu_count"].ToString());
                model.allClosedTaskByMonthChartSourceData.Add(item);
                counter++;
            }

            return Json(model.allClosedTaskByMonthChartSourceData, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetAllCompletedTaskStatusChartOnTimeData()
        {
            TaskRepository tskDB = RepositoryManager.GetRepository<TaskRepository>();
            Index model = new Index();

            DataSet dsChart1 = tskDB.GetAllCompletedTaskByMonthChartOnTimeData();
            model.allCompletedTaskByMonthChartOnTimeSourceData = new List<AllCompletedTaskByMonthChartOnTimeSourceData>();

            int counter = 0;
            foreach (DataRow dr in dsChart1.Tables[0].Rows)
            {
                AllCompletedTaskByMonthChartOnTimeSourceData item = new AllCompletedTaskByMonthChartOnTimeSourceData();
                item.label = dr["month_str"].ToString();
                item.value = Convert.ToInt32(dr["statu_count"].ToString());
                model.allCompletedTaskByMonthChartOnTimeSourceData.Add(item);
                counter++;
            }

            return Json(model.allCompletedTaskByMonthChartOnTimeSourceData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCalendar()
        {
            GenelRepository gnlDB = RepositoryManager.GetRepository<GenelRepository>();
            CalendarList model = new CalendarList();

            DataSet dsCalendar = new DataSet();

            if (Dcm.Source.GlobalHelper.IsAuthorized("110", "menu_right", BaseDB.SessionContext.Current.ActiveUser.UserUid.ToString()))
            {
                dsCalendar = gnlDB.GetAllCalendar();
            }
            else
            {
                dsCalendar = gnlDB.GetCalendar();
            }

            model.calendarListItems = new List<CalendarItem>();


            int counter = 0;
            foreach (DataRow dr in dsCalendar.Tables[0].Rows)
            {
                CalendarItem item = new CalendarItem();
                string[] startArr = null;
                string[] endArr = null;
                if ((dr["start_date"] != null && dr["start_date"].ToString() != ""))
                {
                    startArr = dr["start_date"].ToString().Split('.');
                }

                if ((dr["end_date"] != null && dr["end_date"].ToString() != ""))
                {
                    endArr = dr["end_date"].ToString().Split('.');
                }

                item.title = dr["assigned_user_name"].ToString() + " - " + dr["task_status_name"].ToString() + " - " + dr["task_name"].ToString();
                item.start = (dr["start_date"] != null && dr["start_date"].ToString() != "") ? startArr[2] + "-" + startArr[1] + "-" + startArr[0] : "";
                item.end = (dr["end_date"] != null && dr["end_date"].ToString() != "") ? endArr[2] + "-" + endArr[1] + "-" + endArr[0] : "";

                if (dr["task_status_id"].ToString() == "2")
                    item.backgroundColor = "#ff1800";
                else if (dr["task_status_id"].ToString() == "3" || dr["task_status_id"].ToString() == "4")
                    item.backgroundColor = "#ff9600";
                else if (dr["task_status_id"].ToString() == "5")
                    item.backgroundColor = "#195794";
                else if (dr["task_status_id"].ToString() == "6")
                    item.backgroundColor = "#50a100";
                else if (dr["task_status_id"].ToString() == "7")
                    item.backgroundColor = "#5aa5cc";

                item.url = Url.Content("~/Task/Tasks?RecordId=" + dr["task_id"].ToString() + "&MenuId=" + @Dcm.Source.GlobalHelper.Encrypt("149"));

                model.calendarListItems.Add(item);
                counter++;
            }

            return Json(model.calendarListItems, JsonRequestBehavior.AllowGet);
        }
    }
}