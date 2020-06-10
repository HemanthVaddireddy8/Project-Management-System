using BaseDB;

using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace Dcm.Filters
{
    public class SessionCheckAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var context = filterContext.HttpContext;
            //string lang = System.Threading.Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;
            //if (string.IsNullOrEmpty(lang))
            //    lang = "tr";
            //string redirectTo = "~/" + lang + "/Account/Login";
            //string redirectToLogOut = "~/" + lang + "/Account/LogOff";

            string redirectTo = "~/Account/Login";
            
            ///Kullanıcı Login değilse İşlem Post ise

            if (BaseDB.SessionContext.Current == null || BaseDB.SessionContext.Current.ActiveUser == null)
            {
                HttpCookie ck = System.Web.HttpContext.Current.Request.Cookies["DCMGRUP23"];

                if (ck != null)
                {
                    try
                    {
                        BaseClasses.BaseLogin objLogin = new BaseClasses.BaseLogin();
                        FormsAuthenticationTicket oldTicket = FormsAuthentication.Decrypt(ck.Value);
                        objLogin.LoginFromRememberMe(oldTicket.Name);

                    }
                    catch (Exception exp)
                    {

                    }
                }
            }

            if (!context.Request.IsAjaxRequest() && SessionContext.IsSessionNull() )
            {
                filterContext.Result = new RedirectResult(redirectTo);
            }
            
            base.OnActionExecuting(filterContext);
        }
    }
}