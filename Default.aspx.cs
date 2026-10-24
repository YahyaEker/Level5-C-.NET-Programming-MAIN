using System;
using System.Globalization;
using System.Web.UI;
using EmployeeManagement.App_Code;

namespace EmployeeManagement
{
    public partial class Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            /*
             * TECHNIQUE (c): Cross-page posting.
             * Login.aspx posts credentials directly to this page via PostBackUrl.
             */
            if (PreviousPage != null && PreviousPage.IsCrossPagePostBack)
            {
                Login loginPage = PreviousPage as Login;
                if (loginPage != null)
                {
                    AdminUser admin = DatabaseHelper.AuthenticateAdmin(
                        loginPage.LoginUsername,
                        loginPage.LoginPassword);

                    if (admin == null)
                    {
                        Session["LoginError"] = "Invalid username or password.";
                        // TECHNIQUE (a): Client-side browser redirect
                        Response.Redirect("~/Login.aspx", false);
                        Context.ApplicationInstance.CompleteRequest();
                        return;
                    }

                    AuthHelper.SignIn(admin);
                }
            }

            if (!AuthHelper.RequireAuthentication(this))
            {
                return;
            }

            if (!IsPostBack)
            {
                AdminUser current = AuthHelper.CurrentAdmin;
                litWelcome.Text = string.Format(
                    "<p class=\"welcome-line\">Welcome, <strong>{0}</strong> ({1}).</p>",
                    current != null ? current.Username : "Admin",
                    current != null ? current.DepartmentName : "General");

                string idQuery = Request.QueryString["id"];
                if (!string.IsNullOrWhiteSpace(idQuery))
                {
                    txtSearchId.Text = idQuery.Trim();
                    LoadEmployeeBySearchId();
                }
            }

            pnlLogoutOnly.Visible = !pnlEmployee.Visible;
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadEmployeeBySearchId();
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                int employeeId;
                if (!int.TryParse(txtEmployeeId.Text.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out employeeId))
                {
                    ShowStatus("Search for a valid employee before updating.", false);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtFullName.Text) ||
                    string.IsNullOrWhiteSpace(txtDepartment.Text) ||
                    string.IsNullOrWhiteSpace(txtPosition.Text) ||
                    string.IsNullOrWhiteSpace(txtEmail.Text))
                {
                    ShowStatus("Full name, department, position, and email are required.", false);
                    return;
                }

                decimal salary;
                if (!decimal.TryParse(txtSalary.Text.Trim(), NumberStyles.Number, CultureInfo.InvariantCulture, out salary) &&
                    !decimal.TryParse(txtSalary.Text.Trim(), NumberStyles.Number, CultureInfo.CurrentCulture, out salary))
                {
                    ShowStatus("Salary must be a valid number.", false);
                    return;
                }

                EmployeeRecord employee = new EmployeeRecord
                {
                    EmployeeId = employeeId,
                    FullName = txtFullName.Text.Trim(),
                    Department = txtDepartment.Text.Trim(),
                    Position = txtPosition.Text.Trim(),
                    Salary = salary,
                    Email = txtEmail.Text.Trim()
                };

                // ADO.NET parameterized UPDATE (DatabaseHelper / SQLiteCommand)
                bool updated = DatabaseHelper.UpdateEmployee(employee);
                if (updated)
                {
                    ShowStatus("Employee information was updated successfully.", true);
                }
                else
                {
                    ShowStatus("No matching employee record was updated.", false);
                }
            }
            catch (Exception ex)
            {
                ShowStatus("Unable to update employee: " + ex.Message, false);
            }

            pnlLogoutOnly.Visible = !pnlEmployee.Visible;
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            AuthHelper.SignOut();
            // TECHNIQUE (a): Client-side browser redirect
            Response.Redirect("~/Login.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }

        private void LoadEmployeeBySearchId()
        {
            try
            {
                int employeeId;
                if (!int.TryParse(txtSearchId.Text.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out employeeId))
                {
                    pnlEmployee.Visible = false;
                    pnlLogoutOnly.Visible = true;
                    ShowStatus("Enter a numeric Employee ID.", false);
                    return;
                }

                // ADO.NET parameterized SELECT (DatabaseHelper / SQLiteCommand)
                EmployeeRecord employee = DatabaseHelper.GetEmployeeById(employeeId);
                if (employee == null)
                {
                    pnlEmployee.Visible = false;
                    pnlLogoutOnly.Visible = true;
                    ShowStatus("No employee was found with ID " + employeeId + ".", false);
                    return;
                }

                BindEmployee(employee);
                ShowStatus("Employee " + employee.EmployeeId + " loaded. Edit the fields and click Update to save.", true);
            }
            catch (Exception ex)
            {
                pnlEmployee.Visible = false;
                pnlLogoutOnly.Visible = true;
                ShowStatus("Unable to load employee: " + ex.Message, false);
            }
        }

        private void BindEmployee(EmployeeRecord employee)
        {
            txtEmployeeId.Text = employee.EmployeeId.ToString(CultureInfo.InvariantCulture);
            txtFullName.Text = employee.FullName;
            txtDepartment.Text = employee.Department;
            txtPosition.Text = employee.Position;
            txtSalary.Text = employee.Salary.ToString("0.00", CultureInfo.InvariantCulture);
            txtEmail.Text = employee.Email;
            pnlEmployee.Visible = true;
            pnlLogoutOnly.Visible = false;
        }

        private void ShowStatus(string message, bool success)
        {
            lblStatus.Visible = true;
            lblStatus.Text = message;
            lblStatus.CssClass = success ? "alert alert-success" : "alert alert-error";
        }
    }
}
