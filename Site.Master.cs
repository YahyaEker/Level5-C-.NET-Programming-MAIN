using System.Web.UI;
using EmployeeManagement.App_Code;

namespace EmployeeManagement
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, System.EventArgs e)
        {
        }

        protected void Page_PreRender(object sender, System.EventArgs e)
        {
            ApplyDepartmentTheme();
            ConfigureNavigation();
        }

        /// <summary>
        /// Reads the authenticated admin's department from Session and applies
        /// the matching CSS theme class and department-specific stylesheet.
        /// </summary>
        private void ApplyDepartmentTheme()
        {
            siteStylesheet.Href = ResolveUrl("~/Content/Site.css");

            string themeClass = "theme-default";
            themeStylesheet.Visible = false;

            if (AuthHelper.IsAuthenticated && AuthHelper.CurrentAdmin != null)
            {
                AdminUser admin = AuthHelper.CurrentAdmin;
                themeClass = admin.ThemeCssClass;
                if (admin.DepartmentId == 2)
                {
                    themeClass = "theme-finance theme-fin";
                }
                themeStylesheet.Href = ResolveUrl(admin.ThemeStylesheet);
                themeStylesheet.Visible = true;
            }

            bodyElement.Attributes["class"] = themeClass;
        }

        private void ConfigureNavigation()
        {
            bool isAuthenticated = AuthHelper.IsAuthenticated;

            navDashboardItem.Visible = isAuthenticated;
            navEmployeeListItem.Visible = isAuthenticated;
            navLogoutItem.Visible = isAuthenticated;
            navLoginItem.Visible = !isAuthenticated;

            pnlUserBadge.Visible = isAuthenticated;

            if (isAuthenticated)
            {
                AdminUser admin = AuthHelper.CurrentAdmin;
                litAdminName.Text = admin.Username;
                litDepartment.Text = admin.DepartmentName;
            }
        }
    }
}
