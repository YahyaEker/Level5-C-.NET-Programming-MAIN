<%@ Page Title="Employee List" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="EmployeeList.aspx.cs" Inherits="EmployeeManagement.EmployeeListPage" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="ContentMain" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="upEmployees" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="page-card">
                <h1 class="page-title">Employee Directory</h1>
                <p class="page-subtitle">
                    Browse all employee records. Click <strong>Edit</strong> to load a record into the form below, then save with ADO.NET.
                </p>

                <div class="quick-links">
                    <a href="Default.aspx" class="btn btn-secondary">Back to Dashboard</a>
                </div>

                <asp:Panel ID="pnlStatus" runat="server" Visible="false" CssClass="alert alert-info" role="status">
                    <asp:Literal ID="litStatus" runat="server" />
                </asp:Panel>

                <div class="table-responsive">
                    <asp:GridView ID="gvEmployees" runat="server"
                        AutoGenerateColumns="False"
                        CssClass="data-grid"
                        HeaderStyle-CssClass="data-grid-header"
                        RowStyle-CssClass="data-grid-row"
                        AlternatingRowStyle-CssClass="data-grid-row-alt"
                        GridLines="None"
                        DataKeyNames="EmployeeId"
                        OnRowCommand="gvEmployees_RowCommand"
                        OnRowDataBound="gvEmployees_RowDataBound">
                        <Columns>
                            <asp:BoundField DataField="EmployeeId" HeaderText="ID" ItemStyle-CssClass="col-id" />
                            <asp:BoundField DataField="FullName" HeaderText="Full Name" />
                            <asp:BoundField DataField="Department" HeaderText="Department" />
                            <asp:BoundField DataField="Position" HeaderText="Position" />
                            <asp:BoundField DataField="Salary" HeaderText="Salary" DataFormatString="{0:C}" HtmlEncode="True" ItemStyle-CssClass="col-salary" />
                            <asp:BoundField DataField="Email" HeaderText="Email" />
                            <asp:TemplateField HeaderText="Actions">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkEdit" runat="server"
                                        CssClass="table-action-link"
                                        CommandName="SelectEmployee"
                                        CommandArgument='<%# Eval("EmployeeId") %>'
                                        CausesValidation="false"
                                        Text="Edit" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>
                            <div class="alert alert-info">No employee records were found in the database.</div>
                        </EmptyDataTemplate>
                    </asp:GridView>
                </div>
            </div>

            <asp:Panel ID="pnlEditor" runat="server" Visible="false" CssClass="page-card employee-editor" style="margin-top: 1.25rem;">
                <h3>Edit employee</h3>
                <p class="page-subtitle">Update the fields and click Save / Update. Changes are written with a parameterized SQLite UPDATE.</p>

                <asp:HiddenField ID="hfEmail" runat="server" />

                <div class="form-grid">
                    <div class="form-group">
                        <asp:Label ID="lblEmployeeId" runat="server" AssociatedControlID="txtEmployeeId" Text="Employee ID" />
                        <asp:TextBox ID="txtEmployeeId" runat="server" CssClass="form-control" ReadOnly="true" />
                    </div>
                    <div class="form-group">
                        <asp:Label ID="lblFullName" runat="server" AssociatedControlID="txtFullName" Text="Full Name" />
                        <asp:TextBox ID="txtFullName" runat="server" CssClass="form-control" />
                        <asp:RequiredFieldValidator ID="rfvFullName" runat="server"
                            ControlToValidate="txtFullName"
                            ErrorMessage="Full name is required."
                            Display="Dynamic"
                            CssClass="alert alert-error"
                            ValidationGroup="EmployeeEdit" />
                    </div>
                    <div class="form-group">
                        <asp:Label ID="lblDepartment" runat="server" AssociatedControlID="txtDepartment" Text="Department" />
                        <asp:TextBox ID="txtDepartment" runat="server" CssClass="form-control" />
                        <asp:RequiredFieldValidator ID="rfvDepartment" runat="server"
                            ControlToValidate="txtDepartment"
                            ErrorMessage="Department is required."
                            Display="Dynamic"
                            CssClass="alert alert-error"
                            ValidationGroup="EmployeeEdit" />
                    </div>
                    <div class="form-group">
                        <asp:Label ID="lblPosition" runat="server" AssociatedControlID="txtPosition" Text="Position" />
                        <asp:TextBox ID="txtPosition" runat="server" CssClass="form-control" />
                        <asp:RequiredFieldValidator ID="rfvPosition" runat="server"
                            ControlToValidate="txtPosition"
                            ErrorMessage="Position is required."
                            Display="Dynamic"
                            CssClass="alert alert-error"
                            ValidationGroup="EmployeeEdit" />
                    </div>
                    <div class="form-group">
                        <asp:Label ID="lblSalary" runat="server" AssociatedControlID="txtSalary" Text="Salary" />
                        <asp:TextBox ID="txtSalary" runat="server" CssClass="form-control" />
                        <asp:RequiredFieldValidator ID="rfvSalary" runat="server"
                            ControlToValidate="txtSalary"
                            ErrorMessage="Salary is required."
                            Display="Dynamic"
                            CssClass="alert alert-error"
                            ValidationGroup="EmployeeEdit" />
                    </div>
                </div>

                <div class="btn-group">
                    <asp:Button ID="btnSave" runat="server"
                        Text="Save / Update"
                        CssClass="btn btn-success"
                        ValidationGroup="EmployeeEdit"
                        OnClick="btnSave_Click" />
                    <asp:Button ID="btnCancel" runat="server"
                        Text="Cancel"
                        CssClass="btn btn-secondary"
                        CausesValidation="false"
                        OnClick="btnCancel_Click" />
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
