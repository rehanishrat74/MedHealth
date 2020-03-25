<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="MedHealthSolutions._Default" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
Dashboard
</asp:Content>
<asp:Content ID="TitleContent" runat="server" ContentPlaceHolderID="TitleContent">
Dashboard
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="BodyContent">
    <script type="text/javascript" src="Scripts/d3/d3.v3.min.js" language="javascript"></script>
    <script type="text/javascript" src="Scripts/d3/liquidFillGauge.js" language="javascript"></script>
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            prepareDashboard();           
        });

        function prepareDashboard() {
            $("#dashboard_main").html("<br>" + getProcessing() + "<br>")
            $.ajax({
                type: "POST",
                url: "Default.aspx/prepareDashboard",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    $("#dashboard_main").html(msg.d)
                }
            })
        }   
     </script>
    <span id="dashboard_main">
        <div id='dashboard'></div>       
    </span>
</form><form id='fDwn' method='post' action='Default.aspx'><input type='hidden' name='param' id='param' />
</asp:Content>
