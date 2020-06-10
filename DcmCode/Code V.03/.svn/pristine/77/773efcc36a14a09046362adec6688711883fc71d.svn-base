using Dcm.Models;
using Dcm.Source;
using Gunluk.EntityModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dcm.EntityModels;
using BaseDB;
using Dcm.Filters;
using System.Web.Script.Serialization;
using BaseClasses;

namespace Dcm.Controllers
{
    public class TaskController : Controller
    {

        #region Görev İşlemleri
        [HttpGet]
        [AllowAnonymous]
        [SessionCheckAttribute]
        public ActionResult Tasks(string RecordId, string MenuId, string FromNotification, string NotificationId)
        {
            CrmRepository crmDB = RepositoryManager.GetRepository<CrmRepository>();
            TaskRepository tskDB = RepositoryManager.GetRepository<TaskRepository>();
            GenelRepository gnlDB = RepositoryManager.GetRepository<GenelRepository>();
            Task model = new Task();

            #region Ortak Set Edilecek Değerler
            MenuId = GlobalHelper.Decrypt(MenuId);
            model.RecordId = RecordId;
            model.MenuId = MenuId;
            SessionContext.Current.ActiveUser.MenuId = MenuId;
            #endregion

            Guid recordId = Guid.Empty;

            model.activeProjects = crmDB.GetProjectList();
            model.activeUsers = gnlDB.GetActiveUsers();
            model.taskStatus = tskDB.GetTaskStatusList();
            model.taskPriority = tskDB.GetTaskPriorityList();

            if (GlobalHelper.IsGuid(model.RecordId))
            {
                try
                {
                    recordId = Guid.Parse(model.RecordId);
                    model = tskDB.BindTask(model, recordId);


                    ViewBag.ResultMessage = Resources.GlobalResource.transaction_success;

                    if (FromNotification == "1" && GlobalHelper.IsGuid(NotificationId))
                    {
                        gnlDB.UpdateNotificationShown(Guid.Parse(NotificationId));
                    }
                }
                catch (Exception exp)
                {
                    ViewBag.Success = false;
                    ModelState.AddModelError("Error", exp.Message);
                }
            }
            else
            {
                if (recordId == Guid.Empty)
                {
                    model.task_user_id = SessionContext.Current.ActiveUser.UserUid.ToString();
                    model.assigned_user_id = SessionContext.Current.ActiveUser.UserUid.ToString();
                }
            }

           

            return View(model);
        }

        [HttpPost]
        [SessionCheckAttribute]
        public ActionResult Tasks(Task model)
        {
            Guid recordId = Guid.Empty;
            TaskRepository tskDB = RepositoryManager.GetRepository<TaskRepository>();
            CrmRepository crmDB = RepositoryManager.GetRepository<CrmRepository>();
            GenelRepository gnlDB = RepositoryManager.GetRepository<GenelRepository>();

            #region Ortak Set Edilecek Değerler
            SessionContext.Current.ActiveUser.MenuId = model.MenuId;
            ViewBag.Success = true;
            #endregion

            model.activeProjects = crmDB.GetProjectList();
            model.activeUsers = gnlDB.GetActiveUsers();
            model.taskStatus = tskDB.GetTaskStatusList();
            model.taskPriority = tskDB.GetTaskPriorityList();

            ModelState.Remove("is_active");
            ModelState.Remove("is_task_sent");

            if (model.FromDeleteButton == "1")
            {
                if (GlobalHelper.IsGuid(model.RecordId))
                {
                    tskDB.DeleteTask(model, Guid.Parse(model.RecordId));
                    return RedirectToAction("ListPage", "General", new { MenuId = Dcm.Source.GlobalHelper.Encrypt(model.MenuId) });
                }
            }
            else if (model.FromCreateTaskButton == "1")
            { 
                if (GlobalHelper.IsGuid(model.RecordId))
                {
                    tskDB.UpdateTaskStatus(model, Guid.Parse(model.RecordId),(int)Enums.TaskStatus.Gonderildi,true);
                    model = tskDB.BindTask(model, Guid.Parse(model.RecordId));

                    ViewBag.ResultMessage = Resources.GlobalResource.transaction_success;
                }
            }
            else if (model.FromAddExplanationButton == "1")
            {
                if (GlobalHelper.IsGuid(model.RecordId))
                {
                    tskDB.UpdateExplanation(model, Guid.Parse(model.RecordId));
                    model = tskDB.BindTask(model, Guid.Parse(model.RecordId));

                    ViewBag.ResultMessage = Resources.GlobalResource.transaction_success;
                }
            }
            else
            {
                if (ModelState.IsValid)
                {
                    if (GlobalHelper.IsGuid(model.RecordId))
                    {
                        recordId = Guid.Parse(model.RecordId);
                        try
                        {
                            if (model.task_user_id != BaseDB.SessionContext.Current.ActiveUser.UserUid.ToString() && model.assigned_user_id == BaseDB.SessionContext.Current.ActiveUser.UserUid.ToString())
                            {
                                tskDB.UpdateTaskStatus(model, Guid.Parse(model.RecordId), Convert.ToInt32(model.task_status_id),model.is_active);
                                model = tskDB.BindTask(model, Guid.Parse(model.RecordId));
                            }
                            else
                            {
                                model = tskDB.UpdateTask(model, recordId);
                                model = tskDB.BindTask(model, Guid.Parse(model.RecordId));
                            }

                            ViewBag.ResultMessage = Resources.GlobalResource.transaction_success;
                        }
                        catch (Exception exp)
                        {
                            ViewBag.Success = false;
                            ModelState.AddModelError("Error", exp.Message);
                        }
                    }
                    else
                    {
                        try
                        {
                            tsk_tasks task = new tsk_tasks();
                            tskDB.AddTask(task, model);
                            model.RecordId = task.task_id.ToString();

                            ViewBag.ResultMessage = Resources.GlobalResource.transaction_success;
                        }
                        catch (Exception exp)
                        {
                            ViewBag.Success = false;
                            ModelState.AddModelError("Error", exp.Message);
                        }
                    }
                }
                else
                {
                    ViewBag.Success = false;
                }
            }

            return View(model);
        }


        [SessionCheckAttribute]
        public PartialViewResult TaskHistory(string task_id)
        {
            TaskRepository tskDB = RepositoryManager.GetRepository<TaskRepository>();
            gnl_roles_related_users role_related_users = new gnl_roles_related_users();

            Task model = new Task();
            Guid task_uid = Guid.Empty;
            Guid user_uid = Guid.Empty;

            if (Guid.TryParse(task_id, out task_uid))
            {
                if (task_uid != Guid.Empty)
                {
                    List<tsk_task_history_v> list = tskDB.GetTaskHistoryListByTaskId(task_uid);
                    model.taskHistoryList = list;   
                }
            }

            return PartialView("TaskHistory", model);
        }
        #endregion
     

    }
}