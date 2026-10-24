<%@ Page Title="Admin Login" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Login.aspx.cs" Inherits="EmployeeManagement.Login" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="ContentMain" ContentPlaceHolderID="MainContent" runat="server">
    <div class="login-container">
        <div class="page-card">
            <h1 class="page-title">Admin Login</h1>
            <p class="page-subtitle">Sign in with your department administrator credentials.</p>

            <asp:Panel ID="pnlMessage" runat="server" Visible="false" CssClass="alert alert-error" role="alert">
                <asp:Literal ID="litMessage" runat="server" />
            </asp:Panel>

            <div class="form-group">
                <asp:Label ID="lblUsername" runat="server" AssociatedControlID="txtUsername" Text="Username" />
                <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" autocomplete="username" />
                <asp:RequiredFieldValidator ID="rfvUsername" runat="server"
                    ControlToValidate="txtUsername"
                    ErrorMessage="Username is required."
                    Display="Dynamic"
                    CssClass="alert alert-error"
                    ValidationGroup="LoginGroup" />
            </div>

            <div class="form-group" style="margin-top: 1rem;">
                <asp:Label ID="lblPassword" runat="server" AssociatedControlID="txtPassword" Text="Password" />
                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control" autocomplete="current-password" />
                <asp:RequiredFieldValidator ID="rfvPassword" runat="server"
                    ControlToValidate="txtPassword"
                    ErrorMessage="Password is required."
                    Display="Dynamic"
                    CssClass="alert alert-error"
                    ValidationGroup="LoginGroup" />
            </div>

            <div class="btn-group">
                <!--
                    TECHNIQUE (c): Cross-page posting via PostBackUrl.
                    Credentials are posted directly to Default.aspx.
                -->
                <asp:Button ID="btnLogin" runat="server"
                    Text="Sign In"
                    CssClass="btn btn-primary"
                    ValidationGroup="LoginGroup"
                    PostBackUrl="~/Default.aspx" />

                <asp:Button ID="btnClear" runat="server"
                    Text="Clear"
                    CssClass="btn btn-secondary"
                    CausesValidation="false"
                    OnClick="btnClear_Click" />
            </div>

            <div class="login-hint">
                <strong>Demo accounts:</strong><br />
                HR &mdash; <code>hr_admin</code> / <code>Hr@2024</code><br />
                Finance &mdash; <code>fin_admin</code> / <code>Fin@2024</code><br />
                IT &mdash; <code>it_admin</code> / <code>It@2024</code>
            </div>
        </div>
    </div>
</asp:Content>