using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;

namespace BaseDB
{
    [Serializable]
    public class SessionInfo
    {
        public Guid UserUid { get; set; }
        public string UserName { get; set; }
        public string UserNameAndSurname { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string UserEmail { get; set; }
        public string PublicUserName { get; set; }
        public string MenuId { get; set; }
        public string FromUpdateButton { get; set; }
        public string FromDeleteButton { get; set; }

        public string SelectedRoleId { get; set; }

        public TimeZoneInfo TimeZoneInfoUser { get; set; }

    }
    [Serializable]
    public class SessionContext
    {
        private SessionContext()
        {
            //hide constructor
            ActiveUser = new SessionInfo();
        }

        public SessionInfo ActiveUser { get; private set; }

        public static SessionContext Current
        {
            get
            {
                return (SessionContext)System.Web.HttpContext.Current.Session["SessionContext"];
            }
        }

        public static bool IsSessionNull()
        {
            return (System.Web.HttpContext.Current.Session["SessionContext"] == null) ? true : false;
        }

        

        public static SessionContext StartSession()
        {
            System.Web.HttpContext.Current.Session["SessionContext"] = null;

            SessionContext session = new SessionContext();
            System.Web.HttpContext.Current.Session["SessionContext"] = session;
            return session;
        }

    }
}
