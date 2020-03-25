<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="AccessDenied.aspx.cs" Inherits="MedHealthSolutions.AccessDenied" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
MedHealth
</asp:Content>
<asp:Content ID="TitleContent" runat="server" ContentPlaceHolderID="TitleContent">
Access Denied
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="BodyContent">
    <div id='dashboad'><br />&nbsp; &nbsp; You do not have access to page<b><% 
    try { Response.Write(" '" + Request.QueryString["p"].ToString().Remove(0, 1) + "'"); }
    catch { Response.Write(""); } 
%></b>. Please contact system support at <a href='mailto:support@ramtraxs.com'>support@ramtraxs.com</a> for more details.</div>
</asp:Content>
