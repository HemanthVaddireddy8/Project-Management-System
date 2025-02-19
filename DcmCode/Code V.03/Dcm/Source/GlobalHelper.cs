﻿using BaseDB;
using Dcm.EntityModels;
using Gunluk.EntityModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Configuration;

namespace Dcm.Source
{
    public static class GlobalHelper
    {
        const int TimedOutExceptionCode = -2147467259;
        public static bool IsMaxRequestExceededException(Exception e)
        {

            // unhandled errors = caught at global.ascx level
            // http exception = caught at page level

            Exception main;
            var unhandled = e as HttpUnhandledException;

            if (unhandled != null && unhandled.ErrorCode == TimedOutExceptionCode)
            {
                main = unhandled.InnerException;
            }
            else
            {
                main = e;
            }


            var http = main as HttpException;

            if (http != null && http.ErrorCode == TimedOutExceptionCode)
            {
                // hack: no real method of identifying if the error is max request exceeded as 
                // it is treated as a timeout exception
                if (http.StackTrace.Contains("GetEntireRawContent"))
                {
                    // MAX REQUEST HAS BEEN EXCEEDED
                    return true;
                }
            }

            return false;
        }
        public static string GetGlobalResource(string name)
        {
            string result = Resources.GlobalResource.ResourceManager.GetString(name, Thread.CurrentThread.CurrentCulture);

            if (string.IsNullOrEmpty(result))
            {
                result = name;
            }

            return result;
        }
        public static bool IsGuid(string guid)
        {
            bool isGuid = false;
            Guid tempGuid = new Guid();
            Guid.TryParse(guid, out tempGuid);

            if (tempGuid != Guid.Empty)
            {
                isGuid = true;
            }
            return isGuid;
        }
        public static bool IsGuidOrEmpty(string guid)
        {
            bool isGuid = false;
            Guid tempGuid = new Guid();
            isGuid = Guid.TryParse(guid, out tempGuid);

            return isGuid;
        }

        public static string GetCompany()
        {
            string result = "";

            if (WebConfigurationManager.AppSettings["Company"] != null)
            {
                result = WebConfigurationManager.AppSettings["Company"].ToString();
            }

            return result;
        }

        public static string GetCompanyName()
        {
            string result = "";

            if (WebConfigurationManager.AppSettings["CompanyName"] != null)
            {
                result = WebConfigurationManager.AppSettings["CompanyName"].ToString();
            }

            return result;
        }
         
        public static string DataTableToJsonObj(DataTable dt)
        {
            DataSet ds = new DataSet();
            ds.Merge(dt);
            StringBuilder JsonString = new StringBuilder();
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                JsonString.Append("[");
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    JsonString.Append("{");
                    for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                    {
                        if (j < ds.Tables[0].Columns.Count - 1)
                        {
                            JsonString.Append("\"" + ds.Tables[0].Columns[j].ColumnName.ToString() + "\":" + "\"" + ds.Tables[0].Rows[i][j].ToString() + "\",");
                        }
                        else if (j == ds.Tables[0].Columns.Count - 1)
                        {
                            JsonString.Append("\"" + ds.Tables[0].Columns[j].ColumnName.ToString() + "\":" + "\"" + ds.Tables[0].Rows[i][j].ToString() + "\"");
                        }
                    }
                    if (i == ds.Tables[0].Rows.Count - 1)
                    {
                        JsonString.Append("}");
                    }
                    else
                    {
                        JsonString.Append("},");
                    }
                }
                JsonString.Append("]");
                return JsonString.ToString();
            }
            else
            {
                return null;
            }
        }
        public static string EncriptText(string text)
        {
            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
            byte[] hashedDataBytes;
            UTF8Encoding encoder = new UTF8Encoding();
            hashedDataBytes = md5Hasher.ComputeHash(encoder.GetBytes(text));
            string encriptedPassword = Convert.ToBase64String(hashedDataBytes);
            return encriptedPassword;
        }
        public static bool IsAuthorized(string menuId, string permissionType,string userId)
        {
            bool result = false;

            if (!string.IsNullOrEmpty(menuId))
            {
                DataSet dsMenuRights = BaseDB.DBManager.AppConnection.ExecuteSP("gnl_all_user_authorization_select_sp", new ArrayList { "user_id" }, new ArrayList { userId });

                DataView dw = dsMenuRights.Tables[0].DefaultView;
                dw.RowFilter = String.Format("menu_id={0} and {1}=1", menuId, permissionType);

                GenelRepository gnlDB = RepositoryManager.GetRepository<GenelRepository>();

                result = dw.Count > 0;

                if (GlobalHelper.IsGuid(userId))
                {
                    gnl_users user = gnlDB.GetUser(Guid.Parse(userId));

                    if (user.is_administrator != null && user.is_administrator.Value == true)
                        result = true;
                }
               
            }
            return result;
        }
        public static bool IsAuthorizedByRole(string menuId, string permissionType,string roleId)
        {
            bool result=false;
            if (!string.IsNullOrEmpty(roleId))
            {
                DataSet dsMenuRights = BaseDB.DBManager.AppConnection.ExecuteSP("gnl_all_user_authorization_by_role_select_sp", new ArrayList { "role_id" }, new ArrayList { roleId });
                DataView dw = dsMenuRights.Tables[0].DefaultView;
                dw.RowFilter = String.Format("menu_id={0} and {1}=1", menuId, permissionType);
                result = dw.Count > 0;
            }

            return result;
        }
        public static bool HasAuthorizedChild(string parentMenuId)
        {
            DataSet dsMenuRights = BaseDB.DBManager.AppConnection.ExecuteSP("gnl_user_authorization_child_menu_rights_select_sp", new ArrayList { "user_id", "parent_menu_id" }, new ArrayList { SessionContext.Current.ActiveUser.UserUid, parentMenuId });

            bool result = false;
            GenelRepository gnlDB = RepositoryManager.GetRepository<GenelRepository>();

            result = dsMenuRights.Tables[0].Rows.Count > 0;

            gnl_users user = gnlDB.GetUser(SessionContext.Current.ActiveUser.UserUid);

            if (user.is_administrator!=null && user.is_administrator.Value==true)
                result = true;

            return result;
        }

        public static string Decrypt(string stringToDecrypt)
        {
            string sEncryptionKey = WebConfigurationManager.AppSettings["EncryptionKey"].ToString(); ;
            byte[] key = { };
            byte[] IV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xab, 0xcd, 0xef };

            stringToDecrypt = stringToDecrypt.Replace(' ', '+');

            byte[] inputByteArray = new byte[stringToDecrypt.Length + 1];
            try
            {

                key = System.Text.Encoding.UTF8.GetBytes(sEncryptionKey);
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                inputByteArray = Convert.FromBase64String(stringToDecrypt);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms,
                  des.CreateDecryptor(key, IV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                System.Text.Encoding encoding = System.Text.Encoding.UTF8;
                return encoding.GetString(ms.ToArray());
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public static string Encrypt(string stringToEncrypt)
        {
            if (string.IsNullOrEmpty(stringToEncrypt))
                return string.Empty;

            stringToEncrypt = stringToEncrypt.Replace(' ', '+');

            string SEncryptionKey = WebConfigurationManager.AppSettings["EncryptionKey"].ToString(); ;
            byte[] key = { };
            byte[] IV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xab, 0xcd, 0xef };

            try
            {
                key = System.Text.Encoding.UTF8.GetBytes(SEncryptionKey);
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                byte[] inputByteArray = Encoding.UTF8.GetBytes(stringToEncrypt);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms,
                  des.CreateEncryptor(key, IV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                return Convert.ToBase64String(ms.ToArray());
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }
}