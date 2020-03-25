<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="ApplicationSettings.aspx.cs" Inherits="MedHealthSolutions.ApplicationSettings" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
App Settings
</asp:Content>
<asp:Content ID="TitleContent" runat="server" ContentPlaceHolderID="TitleContent">
Application Settings
</asp:Content>
<asp:Content ID="DownloadIcons" runat="server" ContentPlaceHolderID="DownloadIcons"></asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="BodyContent" >
    <script type="text/javascript" language="javascript">
    $(document).ready(function () {
        initialize()
    });
    function initialize() {
        $("#rpt").html(getProcessing());

        $.ajax({
            type: "POST",
            url: "ApplicationSettings.aspx/getInitialize",
            contentType: "application/json; charset=utf-8", dataType: "json", cache: false,
            success: function (msg) { $("#rpt").html(msg.d[1]); $("#portal_db").html(msg.d[0]); }
        })
    }
     </script>
<table id='rpt_tbl' class='blk_tbl'>
    <tr>
        <td class='frm_td s4'>
        This page shows current application settings for the portal. <br /><br />
        In case of any change in application settings, user must logout and log back in to let settings take effect. <br /><br />
        These settings are stored in the portal database '<b id=portal_db>...</b>'
        </td>
        <td id='rpt' class='cnt blk_td'></td>
    </tr>
</table>
</asp:Content>

