using System.Web;
using System.Web.UI;

namespace EmployeeManagement.App_Code
{
    /// <summary>
    /// Session-based authentication state for admin users.
    /// </summary>
    public static class AuthHelper
    {
        private const string SessionKey = "CurrentAdmin";

        public static bool IsAuthenticated
        {
            get
            {
                return HttpContext.Current != null && HttpContext.Current.Session[SessionKey] != null;
            }
        }

        // C# 5 uyumlu klasik get yapısı ve manuel null kontrolü
        public static AdminUser CurrentAdmin
        {
            get
            {
                if (HttpContext.Current != null && HttpContext.Current.Session != null)
                {
                    return HttpContext.Current.Session[SessionKey] as AdminUser;
                }
                return null;
            }
        }

        public static void SignIn(AdminUser admin)
        {
            HttpContext.Current.Session[SessionKey] = admin;
        }

        public static void SignOut()
        {
            HttpContext.Current.Session.Remove(SessionKey);
            HttpContext.Current.Session.Abandon();
        }

        /// <summary>
        /// Guards secure pages and redirects unauthenticated visitors to Login.aspx.
        /// </summary>
        public static bool RequireAuthentication(Page page)
        {
            if (IsAuthenticated)
            {
                return true;
            }

            /*
             * TECHNIQUE (a): Client-side browser redirect for unauthorized access.
             */
            page.Response.Redirect("~/Login.aspx", false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
            return false;
        }
    }
}