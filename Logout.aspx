<%@ Page Title="Logout" Language="C#" AutoEventWireup="true"
    CodeBehind="Logout.aspx.cs" Inherits="EmployeeManagement.Logout" %>

<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Signing Out...</title>
    <link rel="stylesheet" href="<%= ResolveUrl("~/Content/Site.css") %>" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="site-wrapper">
            <main class="site-main">
                <div class="page-card login-container">
                    <h1 class="page-title">Signing Out</h1>
                    <p class="page-subtitle">Your session is being terminated securely...</p>
                </div>
            </main>
        </div>
    </form>
</body>
</html>
