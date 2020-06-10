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
    public class UsersController : Controller
    {

        #region Kullanıcı İşlemleri
        [HttpGet]
        [AllowAnonymous]
        [SessionCheckAttribute]
        public ActionResult User(string RecordId,string MenuId )
        {
            GenelRepository gnlDB = RepositoryManager.GetRepository<GenelRepository>();
            User model = new User();

            #region Ortak Set Edilecek Değerler  
            MenuId = GlobalHelper.Decrypt(MenuId);
            model.RecordId = RecordId;
            model.MenuId = MenuId;
            SessionContext.Current.ActiveUser.MenuId = MenuId;
            #endregion

            Guid recordId = Guid.Empty;

            model.activeGroups = gnlDB.GetActiveGroups();

            if (model.activeGroups != null && model.activeGroups.Count > 0)
            {
                model.SelectedGroupId = model.activeGroups[0].group_id.ToString();
            }

            if (GlobalHelper.IsGuid(model.RecordId))
            {
                try
                {
                    recordId = Guid.Parse(model.RecordId);
                    model = gnlDB.BindUser(model, recordId);
                    ViewBag.ResultMessage = Resources.GlobalResource.transaction_success;
                }
                catch (Exception exp)
                {
                    ViewBag.Success = false;
                    ModelState.AddModelError("Error", exp.Message);
                }
            }

            if (!string.IsNullOrEmpty(model.ManagerId) && GlobalHelper.IsGuid(model.ManagerId))
            {
                gnl_users userManager = gnlDB.GetUser(Guid.Parse(model.ManagerId));

                if (userManager != null)
                    model.ManagerName = userManager.name + " " + userManager.surname;
            }

            return View(model);
        }

        [HttpPost]
        [SessionCheckAttribute]
        public ActionResult User(User model)
        {
            Guid recordId = Guid.Empty;
            GenelRepository gnlDB = RepositoryManager.GetRepository<GenelRepository>();

            #region Ortak Set Edilecek Değerler
            SessionContext.Current.ActiveUser.MenuId = model.MenuId;
            ViewBag.Success = true;
            #endregion

            model.activeGroups = gnlDB.GetActiveGroups();

            ModelState.Remove("start_date");
            ModelState.Remove("end_date");
            ModelState.Remove("is_active");

            if (model.FromDeleteButton == "1")
            {
                if (GlobalHelper.IsGuid(model.RecordId))
                {
                    gnlDB.DeleteUser(model, Guid.Parse(model.RecordId));
                    return RedirectToAction("ListPage", "General", new { MenuId = Dcm.Source.GlobalHelper.Encrypt(model.MenuId) });
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
                            if (!string.IsNullOrEmpty(model.password))
                                model.password = GlobalHelper.EncriptText(model.password.Trim());

                            gnl_users userByEmail = new gnl_users();
                            userByEmail = gnlDB.GetUserByEmail(model.email.Trim());

                            if (userByEmail != null && userByEmail.user_id != recordId)
                            {
                                ViewBag.Success = false;
                                ModelState.AddModelError("Error", Resources.GlobalResource.exists_user_email);
                            }
                            else
                            {
                                model = gnlDB.UpdateUser(model, recordId);

                                if (!string.IsNullOrEmpty(model.password))
                                    ViewBag.ResultMessage = Resources.GlobalResource.transaction_success_with_password;
                                else
                                    ViewBag.ResultMessage = Resources.GlobalResource.transaction_success;
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
                        try
                        {
                            gnl_users user = new gnl_users();

                            if (!string.IsNullOrEmpty(model.password))
                                model.password = GlobalHelper.EncriptText(model.password.Trim());

                            gnl_users userByEmail = new gnl_users();
                            userByEmail = gnlDB.GetUserByEmail(model.email.Trim());

                            if (userByEmail != null)
                            {
                                ViewBag.Success = false;
                                ModelState.AddModelError("Error", Resources.GlobalResource.exists_user_email);
                            }
                            else
                            {
                                gnlDB.AddUser(user, model);
                                model.RecordId = user.user_id.ToString();

                                if (!string.IsNullOrEmpty(model.password))
                                    ViewBag.ResultMessage = Resources.GlobalResource.transaction_success_with_password;
                                else
                                    ViewBag.ResultMessage = Resources.GlobalResource.transaction_success;
                            }
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

                if (string.IsNullOrEmpty(model.ManagerName))
                    model.ManagerId = null;

                if (!string.IsNullOrEmpty(model.ManagerId) && GlobalHelper.IsGuid(model.ManagerId))
                {
                    gnl_users userManager = gnlDB.GetUser(Guid.Parse(model.ManagerId));

                    if (userManager != null)
                        model.ManagerName = userManager.name + " " + userManager.surname;
                }
            }

            return View(model);
        }

        public JsonResult UserList(string type, string query)
        {
            GenelRepository gnlDB = RepositoryManager.GetRepository<GenelRepository>();
            string list = string.Empty;
            List<UserListModel> userListModel = new List<UserListModel>();

            if (type == "ALL")
            {
                List<gnl_users> userList = gnlDB.GetUserByNameSurname(query.ToUpperInvariant());
               
                foreach (var item in userList)
                {
                    UserListModel model = new UserListModel();
                    model.value = item.name + " " + item.surname;
                    UserListCategoryModel data = new UserListCategoryModel();
                    data.category = "";
                    data.id = item.user_id.ToString();
                    model.data = data;
                    userListModel.Add(model);
                }
            }

            JavaScriptSerializer js = new JavaScriptSerializer();
            string userListModelStr = js.Serialize(userListModel);
            string resultArray = "{\"suggestions\":" + userListModelStr + " }";

            list = js.Serialize(resultArray);

            return Json(list, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Rol İşlemleri
        [HttpGet]
        [AllowAnonymous]
        [SessionCheckAttribute]
        public ActionResult Role(string RecordId, string MenuId)
        {
            GenelRepository gnlDB = RepositoryManager.GetRepository<GenelRepository>();
            Role model = new Role();

            #region Ortak Set Edilecek Değerler
            MenuId = GlobalHelper.Decrypt(MenuId);
            model.RecordId = RecordId;
            model.MenuId = MenuId;
            SessionContext.Current.ActiveUser.MenuId = MenuId;
            #endregion

            Guid recordId = Guid.Empty;

            if (GlobalHelper.IsGuid(model.RecordId))
            {
                try
                {
                    recordId = Guid.Parse(model.RecordId);
                    model = gnlDB.BindRole(model, recordId);
                    ViewBag.ResultMessage = Resources.GlobalResource.transaction_success;
                }
                catch (Exception exp)
                {
                    ViewBag.Success = false;
                    ModelState.AddModelError("Error", exp.Message);
                }
            }

            Guid role_uid = Guid.Empty;
            if (Guid.TryParse(model.RecordId, out role_uid))
            {
                List<gnl_role_related_users_v> listRoleRelatedUsers = gnlDB.GetRoleRelatedUsers(role_uid);
                model.roleRelatedUsers = listRoleRelatedUsers;
            }

            return View(model);
        }

        [HttpPost]
        [SessionCheckAttribute]
        public ActionResult Role(Role model)
        {
            Guid recordId = Guid.Empty;
            GenelRepository gnlDB = RepositoryManager.GetRepository<GenelRepository>();

            #region Ortak Set Edilecek Değerler
            SessionContext.Current.ActiveUser.MenuId = model.MenuId;
            ViewBag.Success = true;
            #endregion

            ModelState.Remove("is_active");

            if (model.FromDeleteButton == "1")
            {
                if (GlobalHelper.IsGuid(model.RecordId))
                {
                    gnlDB.DeleteRole(model, Guid.Parse(model.RecordId));
                    return RedirectToAction("ListPage", "General", new { MenuId = Dcm.Source.GlobalHelper.Encrypt(model.MenuId) });
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
                            model = gnlDB.UpdateRole(model, recordId);
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
                            gnl_roles role = new gnl_roles();
                            gnlDB.AddRole(role, model);
                            model.RecordId = role.role_id.ToString();
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

                Guid role_uid = Guid.Empty;
                if (Guid.TryParse(model.RecordId, out role_uid))
                {
                    List<gnl_role_related_users_v> listRoleRelatedUsers = gnlDB.GetRoleRelatedUsers(role_uid);
                    model.roleRelatedUsers = listRoleRelatedUsers;
                }
            }

            return View(model);
        }


        [SessionCheckAttribute]
        public PartialViewResult RoleRelatedUsers(string role_id,string user_id,string input)
        {
            GenelRepository gnlDB = RepositoryManager.GetRepository<GenelRepository>();
            gnl_roles_related_users role_related_users = new gnl_roles_related_users();

            Role model=new  Role();
            Guid role_uid =Guid.Empty;
            Guid user_uid =Guid.Empty;

            if (Guid.TryParse(role_id, out role_uid) && Guid.TryParse(user_id, out user_uid))
            {
                if (role_uid != Guid.Empty && user_uid != Guid.Empty)
                {
                    List<gnl_role_related_users_v> list = gnlDB.GetRoleRelatedUsers(role_uid);

                    if (list.Where(x => x.user_id == user_uid).Count() <= 0)
                    {
                        model.role_id = role_uid;
                        model.user_id = user_uid;
                        gnlDB.AddRoleRelatedUsers(role_related_users, model);
                    }
                    else
                    {
                        if (input != "")
                            model.errorMessage = Resources.GlobalResource.role_related_users_defined;
                    }
                }
                else
                {
                    if (input != "")
                        model.errorMessage = Resources.GlobalResource.role_related_role_not_defined;
                }
            }

            List<gnl_role_related_users_v> listRoleRelatedUsers = gnlDB.GetRoleRelatedUsers(role_uid);

            model.roleRelatedUsers = listRoleRelatedUsers;

            return PartialView("RoleRelatedUsers",model);
        }

        [SessionCheckAttribute]
        public PartialViewResult RoleRelatedUsersDelete(string role_id, string id)
        {
            GenelRepository gnlDB = RepositoryManager.GetRepository<GenelRepository>();
            gnl_roles_related_users role_related_users = new gnl_roles_related_users();

            Role model = new Role();
            Guid role_uid = Guid.Empty;
            Guid user_uid = Guid.Empty;

            if (Guid.TryParse(role_id, out role_uid) && BaseFunctions.getInstance().IsNumeric(id))
            {
                var obj = gnlDB.GetRoleRelatedUsersById(Convert.ToInt32(id));
                if (obj!=null && obj.Count>0)
                    gnlDB.DeleteRoleRelatedUsers(obj[0]);
            }

            List<gnl_role_related_users_v> listRoleRelatedUsers = gnlDB.GetRoleRelatedUsers(role_uid);

            model.roleRelatedUsers = listRoleRelatedUsers;

            return PartialView("RoleRelatedUsers", model);
        }
        #endregion

        #region Rol Yetkilendirme İşlemleri
        [HttpGet]
        [AllowAnonymous]
        [SessionCheckAttribute]
        public ActionResult RoleAuthorization(string MenuId)
        {
            GenelRepository gnlDB = RepositoryManager.GetRepository<GenelRepository>();
            RoleAuthorization model = new RoleAuthorization();

            #region Ortak Set Edilecek Değerler
            model.MenuId = MenuId;
            SessionContext.Current.ActiveUser.MenuId = MenuId;
            #endregion

            model.activeRoles = gnlDB.GetActiveRoles();

            if (model.activeRoles != null && model.activeRoles.Count > 0)
            {
                model.SelectedRoleId = model.activeRoles[0].role_id.ToString();
                SessionContext.Current.ActiveUser.SelectedRoleId = model.SelectedRoleId;
            }

            return View(model);
        }

        [HttpPost]
        [SessionCheckAttribute]
        public ActionResult RoleAuthorization(RoleAuthorization model, FormCollection formCollection)
        {

            GenelRepository gnlDB = RepositoryManager.GetRepository<GenelRepository>();
            model.activeRoles = gnlDB.GetActiveRoles();
            ViewBag.Success = true;

            SessionContext.Current.ActiveUser.FromUpdateButton = model.FromUpdateButton;
            SessionContext.Current.ActiveUser.SelectedRoleId = model.SelectedRoleId;

            if (SessionContext.Current.ActiveUser.FromUpdateButton == "1")
            {
                try
                {
                    gnlDB.DeleteRoleRights(model.SelectedRoleId);

                    DataSet dsMenu = BaseDB.DBManager.AppConnection.GetDataSet("select * from gnl_menu");

                    foreach (DataRow row in dsMenu.Tables[0].Rows)
                    {

                        bool menuRight = false;
                        bool deleteRight = false;
                        bool updateRight = false;
                        bool reportRight = false;
                        bool newRecordRight = false;

                        foreach (var key in formCollection.AllKeys)
                        {
                            string[] arr = key.Split('_');
                            if (arr.Length > 1)
                            {
                                if (arr[1] != "" && BaseFunctions.getInstance().IsNumeric(arr[1]) && arr[1] == row["menu_id"].ToString())
                                {
                                    if (arr[0] == "Show")
                                        menuRight = true;

                                    if (arr[0] == "Update")
                                        updateRight = true;

                                    if (arr[0] == "Delete")
                                        deleteRight = true;

                                    if (arr[0] == "Report")
                                        reportRight = true;

                                    if (arr[0] == "NewRecord")
                                        newRecordRight = true;
                                }
                            }

                        }
                        gnl_role_rights rights = new gnl_role_rights();
                        gnlDB.AddRoleRights(rights, Guid.Parse(model.SelectedRoleId), Convert.ToInt32(row["menu_id"].ToString()), menuRight, updateRight, deleteRight, reportRight, newRecordRight);
                    }

                    ViewBag.ResultMessage = Resources.GlobalResource.transaction_success;
                }
                catch (Exception exp)
                {
                    ViewBag.Success = false;
                    ModelState.AddModelError("Error", exp.Message);
                }
            }
            return View(model);
        }
        #endregion

        #region Group İşlemleri
        [HttpGet]
        [AllowAnonymous]
        [SessionCheckAttribute]
        public ActionResult Group(string RecordId, string MenuId)
        {
            GenelRepository gnlDB = RepositoryManager.GetRepository<GenelRepository>();
            Group model = new Group();

            #region Ortak Set Edilecek Değerler
            MenuId = GlobalHelper.Decrypt(MenuId);
            model.RecordId = RecordId;
            model.MenuId = MenuId;
            SessionContext.Current.ActiveUser.MenuId = MenuId;
            #endregion


            Guid recordId = Guid.Empty;

            if (GlobalHelper.IsGuid(model.RecordId))
            {
                try
                {
                    recordId = Guid.Parse(model.RecordId);
                    model = gnlDB.BindGroup(model, recordId);
                    ViewBag.ResultMessage = Resources.GlobalResource.transaction_success;
                }
                catch (Exception exp)
                {
                    ViewBag.Success = false;
                    ModelState.AddModelError("Error", exp.Message);
                }
            }

          
            if (!string.IsNullOrEmpty(model.ManagerId) && GlobalHelper.IsGuid(model.ManagerId))
            {
                gnl_users userManager = gnlDB.GetUser(Guid.Parse(model.ManagerId));

                if (userManager != null)
                    model.ManagerName = userManager.name + " " + userManager.surname;
            }

            return View(model);
        }

        [HttpPost]
        [SessionCheckAttribute]
        public ActionResult Group(Group model)
        {
            Guid recordId = Guid.Empty;
            GenelRepository gnlDB = RepositoryManager.GetRepository<GenelRepository>();

            #region Ortak Set Edilecek Değerler
            SessionContext.Current.ActiveUser.MenuId = model.MenuId;
            ViewBag.Success = true;
            #endregion

            ModelState.Remove("is_active");

            if (model.FromDeleteButton == "1")
            {
                if (GlobalHelper.IsGuid(model.RecordId))
                {
                    gnlDB.DeleteGroup(model, Guid.Parse(model.RecordId));
                    return RedirectToAction("ListPage", "General", new { MenuId = Dcm.Source.GlobalHelper.Encrypt(model.MenuId) });
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
                            model = gnlDB.UpdateGroup(model, recordId);

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
                            gnl_user_groups group = new gnl_user_groups();
                            gnlDB.AddGroup(group, model);
                            model.RecordId = group.group_id.ToString();
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

                if (string.IsNullOrEmpty(model.ManagerName))
                    model.ManagerId = null;

                if (!string.IsNullOrEmpty(model.ManagerId) && GlobalHelper.IsGuid(model.ManagerId))
                {
                    gnl_users userManager = gnlDB.GetUser(Guid.Parse(model.ManagerId));

                    if (userManager != null)
                        model.ManagerName = userManager.name + " " + userManager.surname;
                }

            }

            return View(model);
        }
        #endregion

        #region Kullanıcı Profili İşlemleri
        [HttpGet]
        [AllowAnonymous]
        [SessionCheckAttribute]
        public ActionResult UserProfile(string RecordId, string MenuId)
        {
            GenelRepository gnlDB = RepositoryManager.GetRepository<GenelRepository>();
            User model = new User();

            #region Ortak Set Edilecek Değerler
            MenuId = GlobalHelper.Decrypt(MenuId);
            model.RecordId = RecordId;
            model.MenuId = MenuId;
            SessionContext.Current.ActiveUser.MenuId = MenuId;
            #endregion

            Guid recordId = Guid.Empty;

            if (GlobalHelper.IsGuid(model.RecordId))
            {
                try
                {
                    recordId = Guid.Parse(model.RecordId);
                    model = gnlDB.BindUser(model, recordId);
                    ViewBag.ResultMessage = Resources.GlobalResource.transaction_success;
                }
                catch (Exception exp)
                {
                    ViewBag.Success = false;
                    ModelState.AddModelError("Error", exp.Message);
                }
            }

          

            return View(model);
        }

        [HttpPost]
        [SessionCheckAttribute]
        public ActionResult UserProfile(User model)
        {
            Guid recordId = Guid.Empty;
            GenelRepository gnlDB = RepositoryManager.GetRepository<GenelRepository>();

            #region Ortak Set Edilecek Değerler
            SessionContext.Current.ActiveUser.MenuId = model.MenuId;
            ViewBag.Success = true;
            #endregion


            
            ModelState.Remove("name");
            ModelState.Remove("surname");
            ModelState.Remove("email");
            ModelState.Remove("mobile_phone");
            ModelState.Remove("address");

            


            if (ModelState.IsValid)
            {
                if (GlobalHelper.IsGuid(model.RecordId))
                {
                    recordId = Guid.Parse(model.RecordId);
                    try
                    {
                        string password = gnlDB.GetUserPassword(recordId);
                        if (string.IsNullOrEmpty(model.oldPassword))
                        {
                            ViewBag.Success = false;
                            ModelState.AddModelError("Error", Resources.GlobalResource.old_password_not_null);
                        }
                        else if (string.IsNullOrEmpty(model.newPassword))
                        {
                            ViewBag.Success = false;
                            ModelState.AddModelError("Error", Resources.GlobalResource.new_password_not_null);
                        }
                        else if (string.IsNullOrEmpty(model.newPasswordRepeat))
                        {
                            ViewBag.Success = false;
                            ModelState.AddModelError("Error", Resources.GlobalResource.new_password_repeat_not_null);
                        }
                        else if (password!=GlobalHelper.EncriptText(model.oldPassword))
                        {
                            ViewBag.Success = false;
                            ModelState.AddModelError("Error", Resources.GlobalResource.wrong_old_password);
                        }
                        else if (model.newPassword != model.newPasswordRepeat)
                        {
                            ViewBag.Success = false;
                            ModelState.AddModelError("Error", Resources.GlobalResource.wrong_repeat_password);
                        }
                        else
                        {
                          model.password = GlobalHelper.EncriptText(model.newPassword.Trim());
                          model = gnlDB.UpdateUserPassword(model, recordId);

                          if (!string.IsNullOrEmpty(model.password))
                              ViewBag.ResultMessage = Resources.GlobalResource.transaction_success_with_password;
                          else
                              ViewBag.ResultMessage = Resources.GlobalResource.transaction_success;
                        }

                        model = gnlDB.BindUser(model, recordId);
                    }
                    catch (Exception exp)
                    {
                        ViewBag.Success = false;
                        ModelState.AddModelError("Error", exp.Message);
                    }
                }
                
            }
            return View(model);
        }
        #endregion

    }
}