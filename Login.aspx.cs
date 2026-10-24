using System;
using System.Web.UI;
using EmployeeManagement.App_Code;

namespace EmployeeManagement
{
    public partial class Login : Page
    {
        public string LoginUsername
        {
            get { return txtUsername.Text.Trim(); }
        }

        public string LoginPassword
        {
            get { return txtPassword.Text; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                return;
            }

            if (AuthHelper.IsAuthenticated)
            {
                // TECHNIQUE (a): Client-side browser redirect
                Response.Redirect("~/Default.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            object logoutMessage = Context.Items["LogoutMessage"];
            object loginError = Session["LoginError"];

            if (logoutMessage != null)
            {
                pnlMessage.Visible = true;
                litMessage.Text = logoutMessage.ToString();
            }
            else if (loginError != null)
            {
                pnlMessage.Visible = true;
                litMessage.Text = loginError.ToString();
                Session.Remove("LoginError");
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtUsername.Text = string.Empty;
            txtPassword.Text = string.Empty;
            pnlMessage.Visible = false;
        }
    }
}
