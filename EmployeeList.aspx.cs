using System;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using EmployeeManagement.App_Code;

namespace EmployeeManagement
{
    public partial class EmployeeListPage : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!AuthHelper.RequireAuthentication(this))
            {
                return;
            }

            if (!IsPostBack)
            {
                BindEmployeeGrid();
                ShowStatus("Showing " + gvEmployees.Rows.Count + " employee record(s). Click <strong>Edit</strong> to load a record.", "alert alert-info");
            }
        }

        protected void gvEmployees_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "SelectEmployee")
            {
                return;
            }

            int employeeId;
            if (!int.TryParse(Convert.ToString(e.CommandArgument), NumberStyles.Integer, CultureInfo.InvariantCulture, out employeeId))
            {
                ShowStatus("Invalid employee ID.", "alert alert-error");
                return;
            }

            LoadEmployeeIntoForm(employeeId);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                return;
            }

            try
            {
                int employeeId;
                if (!int.TryParse(txtEmployeeId.Text.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out employeeId))
                {
                    ShowStatus("Select an employee before saving.", "alert alert-error");
                    return;
                }

                decimal salary;
                if (!decimal.TryParse(txtSalary.Text.Trim(), NumberStyles.Number, CultureInfo.InvariantCulture, out salary) &&
                    !decimal.TryParse(txtSalary.Text.Trim(), NumberStyles.Number, CultureInfo.CurrentCulture, out salary))
                {
                    ShowStatus("Salary must be a valid number.", "alert alert-error");
                    return;
                }

                EmployeeRecord existing = DatabaseHelper.GetEmployeeById(employeeId);
                if (existing == null)
                {
                    ShowStatus("No matching employee record was found.", "alert alert-error");
                    return;
                }

                EmployeeRecord employee = new EmployeeRecord
                {
                    EmployeeId = employeeId,
                    FullName = txtFullName.Text.Trim(),
                    Department = txtDepartment.Text.Trim(),
                    Position = txtPosition.Text.Trim(),
                    Salary = salary,
                    Email = string.IsNullOrWhiteSpace(hfEmail.Value) ? existing.Email : hfEmail.Value
                };

                bool updated = DatabaseHelper.UpdateEmployee(employee);
                if (!updated)
                {
                    ShowStatus("No matching employee record was updated.", "alert alert-error");
                    return;
                }

                BindEmployeeGrid();
                LoadEmployeeIntoForm(employeeId);
                ShowStatus("Employee " + employeeId + " was updated successfully.", "alert alert-success");
            }
            catch (Exception ex)
            {
                ShowStatus("Unable to update employee: " + ex.Message, "alert alert-error");
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearEditor();
            ShowStatus("Edit cancelled. Showing " + gvEmployees.Rows.Count + " employee record(s).", "alert alert-info");
        }

        protected void gvEmployees_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow)
            {
                return;
            }

            e.Row.Attributes["data-employee-id"] = DataBinder.Eval(e.Row.DataItem, "EmployeeId").ToString();
        }

        private void LoadEmployeeIntoForm(int employeeId)
        {
            EmployeeRecord employee = DatabaseHelper.GetEmployeeById(employeeId);
            if (employee == null)
            {
                ClearEditor();
                ShowStatus("No employee was found with ID " + employeeId + ".", "alert alert-error");
                return;
            }

            txtEmployeeId.Text = employee.EmployeeId.ToString(CultureInfo.InvariantCulture);
            txtFullName.Text = employee.FullName;
            txtDepartment.Text = employee.Department;
            txtPosition.Text = employee.Position;
            txtSalary.Text = employee.Salary.ToString("0.00", CultureInfo.InvariantCulture);
            hfEmail.Value = employee.Email;
            pnlEditor.Visible = true;
            ShowStatus("Loaded employee " + employee.EmployeeId + ". Edit the fields and click Save / Update.", "alert alert-info");
        }

        private void BindEmployeeGrid()
        {
            gvEmployees.DataSource = DatabaseHelper.GetAllEmployees();
            gvEmployees.DataBind();
        }

        private void ClearEditor()
        {
            txtEmployeeId.Text = string.Empty;
            txtFullName.Text = string.Empty;
            txtDepartment.Text = string.Empty;
            txtPosition.Text = string.Empty;
            txtSalary.Text = string.Empty;
            hfEmail.Value = string.Empty;
            pnlEditor.Visible = false;
        }

        private void ShowStatus(string message, string cssClass)
        {
            litStatus.Text = message;
            pnlStatus.CssClass = cssClass;
            pnlStatus.Visible = true;
            upEmployees.Update();
        }
    }
}
