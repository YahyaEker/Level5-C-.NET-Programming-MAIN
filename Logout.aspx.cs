using System;
using System.Web.UI;
using EmployeeManagement.App_Code;

namespace EmployeeManagement
{
    public partial class Logout : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                return;
            }

            AuthHelper.SignOut();

            /*
             * Pass a one-request flash message through HttpContext.Items because
             * Session is cleared during sign-out.
             */
            Context.Items["LogoutMessage"] = "You have been signed out successfully.";

            /*
             * TECHNIQUE (b): Server-side transfer to Login.aspx.
             * The browser URL remains Logout.aspx while Login.aspx renders on the server,
             * demonstrating internal server-side navigation distinct from Response.Redirect.
             */
            Server.Transfer("~/Login.aspx", false);
        }
    }
}
