using System;
using System.Web;
using System.Web.UI;
using EmployeeManagement.App_Code;

namespace EmployeeManagement
{
    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            /*
             * Login.aspx uses RequiredFieldValidator controls. Disable unobtrusive
             * validation so jQuery ScriptResourceMapping is not required at runtime.
             */
            ValidationSettings.UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;

            /*
             * App_Start equivalent for Web Forms:
             * Ensures |DataDirectory| resolves to ~/App_Data and creates the SQLite
             * database file, tables, and sample seed data when missing.
             */
            DatabaseInitializer.Initialize();
        }
    }
}
