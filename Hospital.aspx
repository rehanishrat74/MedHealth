<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="Hospital.aspx.cs" Inherits="MedHealthSolutions.Hospital" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
Hospital
</asp:Content>
<asp:Content ID="TitleContent" runat="server" ContentPlaceHolderID="TitleContent">
Hospital Manager
</asp:Content>
<asp:Content ID="DownloadIcons" runat="server" ContentPlaceHolderID="DownloadIcons">
    <%--<img id='imXL' src="imgs/xl.png" height="25" title="Excel" onclick='download(1)' />--%>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="BodyContent" >
    <script type="text/javascript" language="javascript">
        var param = "";
        $(document).ready(function () {
            buildList(1, 'HN', 'ASC')
        });

        function buildList(page, sort, order) {
            $("#rpt").html(getProcessing());

            if (sort == "") sort = "UN"
            if (order == "") order = "ASC"

            $.ajax({
                type: "POST",
                url: "Hospital.aspx/buildList",
                data: "{page:" + page + ",sSort:'" + sort + "',sOrder:'" + order + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: true,
                cache: false,
                success: function (msg) {
                    $("#rpt").html(msg.d);
                },
                error: function () {
                    $("#rpt").html("<table class='rpt_tbl'><tr><th>ERROR</th></tr><tr><td>Unable to reach central server</td></tr></table>")
                }
            })
        }

        function newEntry(tr) {
            html = "<table class='frm_tbl tp_rht s100i'>";
            html += "<tr><th>Hospital Name</th><td colspan=3><input type='text' id='HospitalName' /></td></tr>";
            html += "<tr><th>Address</th><td colspan=3><input type='text' id='Address' class=s5 /></td></tr>";
            html += "<tr><th>Zip Code</th><td colspan=3><input type='text' id='ZipCode' class=s3 /></td></tr>";
            html += "<tr><th>Contact Number</th><td colspan=3><input type='text' id='ContactNumber' class=s4 /></td></tr>";
            html += "<tr><td></td><td><br />";
            html += "   <input type='hidden' id='entryId' value='0' />";
            html += "   <b class='btn3 s3 cnt' onclick='saveForm()' id='btnAdd'>Save</b>";
            html += "   <b class='btn3 s3 cnt' onclick='closeDialog()' id='b1'>Close</b>";
            html += "</td></tr>";
            html += "</table>";
            createDialog(html, "s10")
            if (tr != null) {
                tr = $(tr)
                $("#entryId").val(tr.attr("d"))
                $("#HospitalName").val($(tr.find("td")[0]).text())
                $("#Address").val(tr.attr("a"))
                $("#ZipCode").val(tr.attr("z"))
                $("#ContactNumber").val(tr.attr("cn"))
            }
                
        }

        function saveForm() {            
            param = "{id:'" + $("#entryId").val() + "',HospitalName:'" + $("#HospitalName").val() + "',Address:'" + $("#Address").val() + "',ZipCode:'" + clean($("#ZipCode").val()) + "',ContactNumber:'" + clean($("#ContactNumber").val()) + "'}"
            closeDialog();
            clLoading()
           $.ajax({
                type: "POST",
                url: "Hospital.aspx/saveHospital",
                data: param,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: true,
                cache: false,
                success: function (msg) {
                    clLoading();
                    buildList(1, 'HN', 'ASC')
                },
                error: function () {
                    alert("Unable to reach central server. Please check the internet connection")
                }
            })
        }
    </script>
<table id="rpt_tbl" class="main_tbl">
    <tr>
        <td class="frm_td s31"><br />
            <b class="btn3 margin3" id='bChange' onclick="newEntry(null)">Add New Hospital</b>  
        </td>
        <td id="rpt" class="cnt blk_td"></td>
    </tr>
</table>

<table id="EntryForm" class="blk_tbl bt_rht hide">
<tr>
<td class="frm_td s31">
    <div class=fix_btn>
    </div>
</td>
<td class="blk_td">

</td>
</tr>
</table>

</form><form id='fDwn' method='post' action='UserManager.aspx'><input type='hidden' name='param' id='param' />
</asp:Content>

