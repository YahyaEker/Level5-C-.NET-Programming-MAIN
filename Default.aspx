<%@ Page Title="Dashboard" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="EmployeeManagement.Default" %>
<%@ PreviousPageType VirtualPath="~/Login.aspx" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="ContentMain" ContentPlaceHolderID="MainContent" runat="server">
    <div class="dashboard-container">
        <asp:UpdatePanel ID="upEmployee" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="page-card">
                    <h1 class="page-title">Dashboard</h1>
                    <asp:Literal ID="litWelcome" runat="server" />
                    <p class="page-subtitle">
                        Search an employee by ID to view and update their record on this page.
                    </p>

                    <section class="search-panel" aria-labelledby="searchHeading">
                        <h3 id="searchHeading">Search employee</h3>
                        <div class="search-row">
                            <div class="form-group search-id-group">
                                <asp:Label ID="lblSearchId" runat="server" AssociatedControlID="txtSearchId" Text="Employee ID" />
                                <asp:TextBox ID="txtSearchId" runat="server" CssClass="form-control" MaxLength="10" />
                            </div>
                            <asp:Button ID="btnSearch" runat="server"
                                Text="Search"
                                CssClass="btn btn-primary"
                                OnClick="btnSearch_Click" />
                        </div>
                    </section>

                    <asp:Label ID="lblStatus" runat="server" CssClass="alert" Visible="false" />

                    <asp:Panel ID="pnlEmployee" runat="server" Visible="false" CssClass="employee-editor">
                        <h3>Employee information</h3>
                        <div class="form-grid">
                            <div class="form-group">
                                <asp:Label ID="lblEmployeeId" runat="server" AssociatedControlID="txtEmployeeId" Text="Employee ID" />
                                <asp:TextBox ID="txtEmployeeId" runat="server" CssClass="form-control" ReadOnly="true" />
                            </div>
                            <div class="form-group">
                                <asp:Label ID="lblFullName" runat="server" AssociatedControlID="txtFullName" Text="Full Name" />
                                <asp:TextBox ID="txtFullName" runat="server" CssClass="form-control" />
                            </div>
                            <div class="form-group">
                                <asp:Label ID="lblDepartment" runat="server" AssociatedControlID="txtDepartment" Text="Department" />
                                <asp:TextBox ID="txtDepartment" runat="server" CssClass="form-control" />
                            </div>
                            <div class="form-group">
                                <asp:Label ID="lblPosition" runat="server" AssociatedControlID="txtPosition" Text="Position" />
                                <asp:TextBox ID="txtPosition" runat="server" CssClass="form-control" />
                            </div>
                            <div class="form-group">
                                <asp:Label ID="lblSalary" runat="server" AssociatedControlID="txtSalary" Text="Salary" />
                                <asp:TextBox ID="txtSalary" runat="server" CssClass="form-control" />
                            </div>
                            <div class="form-group">
                                <asp:Label ID="lblEmail" runat="server" AssociatedControlID="txtEmail" Text="Email" />
                                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" />
                            </div>
                        </div>

                        <div class="btn-group">
                            <asp:Button ID="btnUpdate" runat="server"
                                Text="Update"
                                CssClass="btn btn-success"
                                OnClick="btnUpdate_Click" />
                            <asp:Button ID="btnLogout" runat="server"
                                Text="Sign Out"
                                CssClass="btn btn-secondary"
                                OnClick="btnLogout_Click"
                                CausesValidation="false" />
                        </div>
                    </asp:Panel>

                    <asp:Panel ID="pnlLogoutOnly" runat="server" CssClass="btn-group">
                        <asp:Button ID="btnLogoutAlt" runat="server"
                            Text="Sign Out"
                            CssClass="btn btn-secondary"
                            OnClick="btnLogout_Click"
                            CausesValidation="false" />
                    </asp:Panel>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
