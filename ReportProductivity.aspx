<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="ReportProductivity.aspx.cs" Inherits="MedHealthSolutions.ReportProductivity" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
Agent Productivity Report
</asp:Content>
<asp:Content ID="TitleContent" runat="server" ContentPlaceHolderID="TitleContent">
Agent Productivity Report
</asp:Content>
<asp:Content ID="DownloadIcons" runat="server" ContentPlaceHolderID="DownloadIcons">
    <%--<img id='imXL' src="imgs/xl.png" height="25" title="Excel" onclick='download(1)' />--%>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="BodyContent" >
    <script type="text/javascript" src="Scripts/FormsCommon.js?x<%=MedHealthSolutions.Classes.portalVersion.info() %>"></script>
    <script type="text/javascript" language="javascript">
        var FormName = "AMI"
        $(document).ready(function () {
            $('.date').mask('00/00/0000', { placeholder: "__/__/____" }, { 'translation': { 0: { pattern: /[0-9*]/ } } });
            $('.date').change(function () {
                if (this.value != "" && !is_date(this.value)) {
                    $(this).addClass("txt_err");
                    this.focus()
                }
                else {
                    $(this).removeClass("txt_err");
                }
            });
            showInitializeLists()
        })

        function showInitializeLists(ar) {
            arMonth = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"]
            html = "";
            var currentDate = new Date();
            for (i = 0; i < 12; i++) {                
                html += "<option value='" + currentDate.getFullYear() + "-" + (currentDate.getMonth()+1) + "'>" + arMonth[currentDate.getMonth()] + " " + currentDate.getFullYear() + "</option>";
                currentDate.setMonth(currentDate.getMonth() - 1);
            }
            $("#DR_Month").html(html)
        }
        var params = "";
        function getParams() {
            params = "formType:'" + $("input[name='FormType']:checked").val()+"'";
            params += ",formStatus:" + $("input[name='FormStatus']:checked").val();
            params += ",dateRangeType:" + $("input[name='DateRange']:checked").val();
            if ($("input[name='DateRange']:checked").val()==1)
                params += ",dateRange:'" + $("#DR_Month").val() + "'"
            else if ($("input[name='DateRange']:checked").val() == 2)
                params += ",dateRange:'" + $("#DR_From").val() + "~" + $("#DR_To").val() + "'"
        }
        function getReport() {
            $("#rpt").html(getProcessing());
            getParams()
            $.ajax({
                type: "POST",
                url: "ReportProductivity.aspx/getReport",
                data: "{" + params + "}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: true,
                cache: false,
                success: function (msg) {
                    $("#rpt").html(msg.d);
                },
                error: function () {
                    $("#rpt").html("Unable to reach central server")
                }
            })
        }
        function exportReport() {
            getParams()
            $("#param").val(params)
            $("#fDwn")[0].submit();
        }
    </script>
<table id="rpt_tbl" class="main_tbl">
    <tr>
        <td class="frm_td s4">
            Form Type:
            <div>
                <label class="check-label"><input type="radio" name="FormType" checked value="All"> All Form<span class="radio"></span></label>
                <label class="check-label"><input type="radio" name="FormType" value="CHF"> CHF<span class="radio"></span></label>
                <label class="check-label"><input type="radio" name="FormType" value="COPD"> COPD<span class="radio"></span></label>
                <label class="check-label"><input type="radio" name="FormType" value="PNA"> PNA<span class="radio"></span></label>
                <label class="check-label"><input type="radio" name="FormType" value="AMI"> AMI<span class="radio"></span></label>
                <label class="check-label"><input type="radio" name="FormType" value="SEP"> Sepsis<span class="radio"></span></label>
            </div>
            Form Status:
            <div>
                <label class="check-label"><input type="radio" name="FormStatus" checked value="1"> Incomplete Forms<span class="radio"></span></label>
                <label class="check-label"><input type="radio" name="FormStatus" value="2"> Archived Forms<span class="radio"></span></label>
            </div>
            Date Range:
            <div>
                <label class="check-label"><input type="radio" name="DateRange" checked value="1"><select id="DR_Month"><option value="2019-01">January 2019</option></select><span class="radio" style="margin-top: 10px;"></span></label>
                <label class="check-label"><input type="radio" name="DateRange" value="2"><input id="DR_From" type="text" class="s3 date"><span class="radio" style="margin-top: 10px;"></span></label>
                &nbsp; &nbsp; To <input id="DR_To" type="text" class="s3 date">
            </div>
            <br />
            <b class="btn3 margin3" id='bView' onclick="getReport()">View Report</b>  
            <b class="btn3 margin3" id='bExport' onclick="exportReport()">Export Report</b>
        </td>
        <td id="rpt" class="cnt blk_td"><div class='no-result'>Use filters to get report</div></td>
    </tr>
</table>
</form><form id='fDwn' method='post' action='ReportProductivity.aspx'><input type='hidden' name='param' id='param' />
</asp:Content>

