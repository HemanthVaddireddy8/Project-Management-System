﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Collections;
using System.Globalization;
using Dcm.EntityModels;
using Dcm.Models;
using BaseDB;
using Dcm.Source;

namespace Gunluk.EntityModels
{
    public class GenelRepository : BaseDB.BaseRepository<Dcm.EntityModels.GenelEntities>
    {

        #region Genel İşlemler
        public List<Dcm.Models.TimeZone> GetTimeZones()
        {
            DataSet ds = new DataSet();

            List<Dcm.Models.TimeZone> list = new List<Dcm.Models.TimeZone>();

            foreach (var tz in TimeZoneInfo.GetSystemTimeZones())
            {
                Dcm.Models.TimeZone tzone = new Dcm.Models.TimeZone();
                tzone.Value = tz.Id;
                tzone.Text = tz.DisplayName;
                list.Add(tzone);
            }

            return list;
        }

        public DataSet GetCalendar()
        {
            return BaseDB.DBManager.AppConnection.GetDataSet("select * from tsk_tasks_v where assigned_user_id='" + BaseDB.SessionContext.Current.ActiveUser.UserUid + "'");
        }

        public DataSet GetAllCalendar()
        {
            return BaseDB.DBManager.AppConnection.GetDataSet("select * from tsk_tasks_v ");
        }
        #endregion

        #region Kullanıcı İşlemleri
        public gnl_users GetUser(Guid user_uid)
        {
            return db.gnl_users.SingleOrDefault(d => d.user_id == user_uid);
        }

        public gnl_users GetUserByEmail(string email)
        {
            return db.gnl_users.SingleOrDefault(d => d.email == email && d.is_deleted!=true);
        }

        public List<gnl_users> GetActiveUsers()
        {
            return db.gnl_users.Where(d => d.is_active == true).ToList();
        }

        public List<gnl_users> GetUserByNameSurname(string q)
        {
            return db.gnl_users.Where(d => (d.name.ToUpper().Contains(q) || d.surname.ToUpper().Contains(q)) && d.is_deleted!=true && d.is_active!=false).ToList();
        }

        public void AddUser(gnl_users user, User model)
        {
            user.user_id = Guid.NewGuid();
            user.name = model.name;
            user.surname = model.surname;
            user.email = model.email;

            if (!string.IsNullOrEmpty(model.password))
                user.password = model.password;

            if (!string.IsNullOrEmpty(model.SelectedGroupId) && GlobalHelper.IsGuid(model.SelectedGroupId))
                user.group_id = Guid.Parse(model.SelectedGroupId);

            if (string.IsNullOrEmpty(model.ManagerName))
            {
                user.manager_id = null;
            }
            else
            {
                if (!string.IsNullOrEmpty(model.ManagerId) && GlobalHelper.IsGuid(model.ManagerId))
                    user.manager_id = Guid.Parse(model.ManagerId);
            }

            user.mobile_phone = model.mobile_phone;
            user.home_phone = model.home_phone;
            user.identity_number = model.identity_number;

            if (model.birth_date != null && model.birth_date != DateTime.MinValue && model.birth_date.ToString() != "")
                user.birth_date = Convert.ToDateTime(model.birth_date);

            if (model.end_date != null && model.end_date != DateTime.MinValue && model.end_date.ToString() != "")
                user.end_date = Convert.ToDateTime(model.end_date);

            if (model.start_date != null && model.start_date != DateTime.MinValue && model.start_date.ToString() != "")
                user.start_date = Convert.ToDateTime(model.start_date);

            user.adress = model.address;
            user.note = model.note;
            user.is_active = model.is_active;
            user.is_deleted = false;
            db.gnl_users.Add(user);
            this.Kaydet();
        }

        public User BindUser(User model, Guid recordId)
        {
            gnl_users user = new gnl_users();
            user = this.GetUser(recordId);

            model.name = user.name;
            model.surname = user.surname;
            model.email = user.email;
            model.address = user.adress;
            model.mobile_phone = user.mobile_phone;
            model.home_phone = user.home_phone;

            model.identity_number = user.identity_number;
            model.is_active = user.is_active.Value;

            if (user.group_id!=null)
                model.SelectedGroupId = user.group_id.ToString();

            if (user.manager_id != null)
                model.ManagerId = user.manager_id.ToString();

            if (user.birth_date != null)
                model.birth_date = user.birth_date.Value;
            else
                model.birth_date = DateTime.MinValue;

            if (user.end_date != null)
                model.end_date = user.end_date.Value;
            else
                model.end_date = DateTime.MinValue;

            if (user.start_date != null)
                model.start_date = user.start_date.Value;
            else
                model.start_date = DateTime.MinValue;

            model.note = user.note;



            return model;
        }

        public User UpdateUser(User model, Guid recordId)
        {
            gnl_users user = new gnl_users();
            user = this.GetUser(recordId);

            user.name = model.name;
            user.surname = model.surname;
            user.email = model.email;
            user.adress = model.address;
            user.mobile_phone = model.mobile_phone;
            user.home_phone = model.home_phone;

            if (!string.IsNullOrEmpty(model.password))
                user.password = model.password;

            if (!string.IsNullOrEmpty(model.SelectedGroupId) && GlobalHelper.IsGuid(model.SelectedGroupId))
                user.group_id = Guid.Parse(model.SelectedGroupId);


            if (string.IsNullOrEmpty(model.ManagerName))
            {
                user.manager_id = null;
            }
            else
            {
                if (!string.IsNullOrEmpty(model.ManagerId) && GlobalHelper.IsGuid(model.ManagerId))
                    user.manager_id = Guid.Parse(model.ManagerId);
            }
            
            user.identity_number = model.identity_number;
            user.is_active = model.is_active;

            if (model.birth_date != null && model.birth_date != DateTime.MinValue && model.birth_date.ToString() != "")
                user.birth_date = model.birth_date;


            if (model.end_date != null && model.end_date != DateTime.MinValue && model.end_date.ToString() != "")
                user.end_date = model.end_date;

            if (model.start_date != null && model.start_date != DateTime.MinValue && model.start_date.ToString() != "")
                user.start_date = model.start_date;

            user.note = model.note;

            user.updated_at = DateTime.UtcNow;
            user.updated_by = SessionContext.Current.ActiveUser.UserUid;

            this.Kaydet();
            return model;
        }

        public User UpdateUserPassword(User model, Guid recordId)
        {
            gnl_users user = new gnl_users();
            user = this.GetUser(recordId);

            if (!string.IsNullOrEmpty(model.password))
                user.password = model.password;

            user.updated_at = DateTime.UtcNow;
            user.updated_by = SessionContext.Current.ActiveUser.UserUid;

            this.Kaydet();
            return model;
        }

        public User DeleteUser(User model, Guid recordId)
        {
            gnl_users user = new gnl_users();
            user = this.GetUser(recordId);

            user.is_deleted = true;
            user.is_active = false;
            user.deleted_at = DateTime.UtcNow;
            user.deleted_by = SessionContext.Current.ActiveUser.UserUid;

            this.Kaydet();
            return model;
        }

        public string GetUserPassword(Guid recordId)
        {
            gnl_users user = new gnl_users();
            user = this.GetUser(recordId);

            return user.password;
        }
        #endregion

        #region Rol İşlemleri
        public gnl_roles GetRole(Guid role_uid)
        {
            return db.gnl_roles.SingleOrDefault(d => d.role_id == role_uid);
        }

        public void AddRole(gnl_roles role, Role model)
        {
            role.role_id = Guid.NewGuid();
            role.role_name = model.role_name;
            role.role_explanation = model.role_explanation;
            role.is_active = model.is_active;
            role.is_deleted = false;
            db.gnl_roles.Add(role);
            this.Kaydet();
        }

        public Role BindRole(Role model, Guid recordId)
        {
            gnl_roles role = new gnl_roles();
            role = this.GetRole(recordId);
            model.role_name = role.role_name;
            model.role_explanation = role.role_explanation;
            model.is_active = role.is_active.Value;
            return model;
        }

        public Role UpdateRole(Role model, Guid recordId)
        {
            gnl_roles role = new gnl_roles();
            role = this.GetRole(recordId);
            role.role_name = model.role_name;
            role.role_explanation = model.role_explanation;
            role.is_active = model.is_active;
            role.updated_at = DateTime.UtcNow;
            role.updated_by = SessionContext.Current.ActiveUser.UserUid;
            this.Kaydet();
            return model;
        }

        public Role DeleteRole(Role model, Guid recordId)
        {
            gnl_roles role = new gnl_roles();
            role = this.GetRole(recordId);

            role.is_deleted = true;
            role.is_active = false;
            role.deleted_at = DateTime.UtcNow;
            role.deleted_by = SessionContext.Current.ActiveUser.UserUid;

            this.Kaydet();
            return model;
        }

        public List<gnl_role_related_users_v> GetRoleRelatedUsers(Guid role_id)
        {
            return db.gnl_role_related_users_v.Where(d => d.role_id == role_id).ToList();
        }

        public List<gnl_roles_related_users> GetRoleRelatedUsersById(int id)
        {
            return db.gnl_roles_related_users.Where(d => d.id == id).ToList();
        }

        public List<gnl_roles> GetActiveRoles()
        {
            return db.gnl_roles.Where(d => d.is_active == true).OrderBy(x=>x.role_name).ToList();
        }
        public void AddRoleRelatedUsers(gnl_roles_related_users role_related_users, Role model)
        {
            role_related_users.role_id = model.role_id;
            role_related_users.user_id = model.user_id;

            db.gnl_roles_related_users.Add(role_related_users);
            this.Kaydet();
        }

        public void DeleteRoleRelatedUsers(gnl_roles_related_users obj)
        {
            db.gnl_roles_related_users.Remove(obj);
            this.Kaydet();
        }
        #endregion

        #region Rol Authorization İşlemleri
        public void DeleteRoleRights(string roleId)
        {
            if (!string.IsNullOrEmpty(roleId))
                BaseDB.DBManager.AppConnection.ExecuteSql("delete from gnl_role_rights where role_id=@role_id", new ArrayList { "role_id" }, new ArrayList { roleId.ToString() });
        }

        public void AddRoleRights(gnl_role_rights role_rights, Guid role_id,int menuId, bool menuRight, bool updateRight, bool deleteRight, bool reportRight,bool new_record_right)
        {
            role_rights.role_id = role_id;
            role_rights.menu_id = menuId;
            role_rights.menu_right = menuRight;
            role_rights.delete_right = deleteRight;
            role_rights.update_right = updateRight;
            role_rights.report_right = reportRight;
            role_rights.new_record_right = new_record_right;
            db.gnl_role_rights.Add(role_rights);
            this.Kaydet();
        }
        #endregion

        #region Grup İşlemleri
        public gnl_user_groups GetUserGroups(Guid group_uid)
        {
            return db.gnl_user_groups.SingleOrDefault(d => d.group_id == group_uid);
        }

        public void AddGroup(gnl_user_groups group, Group model)
        {
            group.group_id = Guid.NewGuid();
            group.group_name = model.group_name;
            group.group_explanation = model.group_explanation;
            group.is_active = model.is_active;

            if (string.IsNullOrEmpty(model.ManagerName))
            {
                group.manager_id = null;
            }
            else
            {
                if (!string.IsNullOrEmpty(model.ManagerId) && GlobalHelper.IsGuid(model.ManagerId))
                    group.manager_id = Guid.Parse(model.ManagerId);
            }

           
            group.is_deleted = false;
            db.gnl_user_groups.Add(group);
            this.Kaydet();
        }

        public Group BindGroup(Group model, Guid recordId)
        {
            gnl_user_groups group = new gnl_user_groups();
            group = this.GetUserGroups(recordId);
            model.group_name = group.group_name;
            model.group_explanation = group.group_explanation;
            model.is_active = group.is_active.Value;

            if (group.manager_id != null)
                model.ManagerId = group.manager_id.ToString();

            return model;
        }

        public Group UpdateGroup(Group model, Guid recordId)
        {
            gnl_user_groups group = new gnl_user_groups();
            group = this.GetUserGroups(recordId);
            group.group_name = model.group_name;
            group.group_explanation = model.group_explanation;

            if (string.IsNullOrEmpty(model.ManagerName))
            {
                group.manager_id = null;
            }
            else
            {
                if (!string.IsNullOrEmpty(model.ManagerId) && GlobalHelper.IsGuid(model.ManagerId))
                    group.manager_id = Guid.Parse(model.ManagerId);
            }


            group.is_active = model.is_active;
            group.updated_at = DateTime.UtcNow;
            group.updated_by = SessionContext.Current.ActiveUser.UserUid;
            this.Kaydet();
            return model;
        }

        public Group DeleteGroup(Group model, Guid recordId)
        {
            gnl_user_groups group = new gnl_user_groups();
            group = this.GetUserGroups(recordId);

            group.is_deleted = true;
            group.is_active = false;
            group.deleted_at = DateTime.UtcNow;
            group.deleted_by = SessionContext.Current.ActiveUser.UserUid;

            this.Kaydet();
            return model;
        }

        public List<gnl_user_groups> GetActiveGroups()
        {
            return db.gnl_user_groups.Where(d => d.is_active == true).OrderBy(x => x.group_name).ToList();
        }
        #endregion

        #region Notification İşlemleri
        public gnl_notifications GetNotification(Guid notification_id)
        {
            return db.gnl_notifications.SingleOrDefault(d => d.notification_id == notification_id);
        }

        public void AddNotification(Guid related_record_id,Guid notification_user_id,int notification_type,int notification_module_type,string title,string body)
        {
            gnl_notifications notification = new gnl_notifications();
            notification.notification_id = Guid.NewGuid();
            notification.related_record_id = related_record_id;
            notification.notification_user_id = notification_user_id;
            notification.notification_type = notification_type;
            notification.notification_module_type = notification_module_type;
            notification.notification_title = title;
            notification.notification_body = body;
            notification.notification_shown = false;
            notification.notification_created_date = DateTime.UtcNow;
            
            db.gnl_notifications.Add(notification);
            this.Kaydet();
        }

        public void UpdateNotificationShown(Guid recordId)
        {
            gnl_notifications notification = new gnl_notifications();
            notification = this.GetNotification(recordId);
            notification.notification_shown = true;
            notification.notification_shown_date = DateTime.UtcNow;
            this.Kaydet();
        }

        public List<gnl_notifications> GetActiveNotificationByUser(Guid user_id)
        {
            return db.gnl_notifications.Where(d => d.notification_shown== false && d.notification_user_id==user_id).OrderByDescending(x => x.id).ToList();
        }
        #endregion
    }
}