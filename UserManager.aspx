<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="UserManager.aspx.cs" Inherits="MedHealthSolutions.UserManager" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
User Mngr
</asp:Content>
<asp:Content ID="TitleContent" runat="server" ContentPlaceHolderID="TitleContent">
User Management
</asp:Content>
<asp:Content ID="DownloadIcons" runat="server" ContentPlaceHolderID="DownloadIcons">
    <%--<img id='imXL' src="imgs/xl.png" height="25" title="Excel" onclick='download(1)' />--%>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="BodyContent" >
    <script type="text/javascript" language="javascript">
        var param = "";
        $(document).ready(function () {
            $("#page_filters1").hide();
            getSearchParam()
            getUsrs(1, '', '', '')
            $("#WithRole").html(makeRoleDD())
            $("#OnlyRole").html(makeRoleDD())

            getList()
        });

        function makeRoleDD() {
            roles = "<option value='IsAgent'>Agent</option>";
            roles += "<option value='IsManager'>Manager</option>";
            roles += "<option value='0'>-----------------------</option>";
            roles += "<option value='IsAdmin'>Administrator</option>";

            return roles; 
        }

        function upload_file() {
            $("#uploadedSign").html("<b>Uploading signature....</b>")
            //id:" + $("#user_id").val()

            var formData = new FormData();
            formData.append("sign_file", document.getElementById("fileSign").files[0]);

            $.ajax({
                type: "POST",
                url: "UserManager.aspx?u_id=" + $("#user_id").val(),
                data: formData,
                contentType: "application/json; charset=utf-8",
                cache: false,
                async: false,
                dataType: 'json',
                processData: false, // Don't process the files
                contentType: false, // Set content type to false as jQuery will tell the server its a query string request
                success: function (msg) {
                    $("#uploadedSign").html("<img src='/imgs/sign/sign_" + $("#user_id").val() + ".jpg' height='150px'>")
                }
                , error: function (msg) {
                    $("#uploadedSign").html("<img src='/imgs/sign/sign_" + $("#user_id").val() + ".jpg' height='150px'>")
                }
            })
        }

        function on_change() {
            $(this).removeClass("txt_err");
            if ($(this).attr("id") == "Username") {
                $("#UErr").html("<img src='../imgs/ui-anim_basic_16x16.gif' />&nbsp;verifing username");

                $.ajax({
                    type: "POST",
                    url: "UserManager.aspx/checkUser",
                    data: "{usr:'" + $(this).val() + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: true,
                    cache: false,
                    success: function (msg) {
                        $("#UErr").html(msg.d);
                    },
                    error: function () {
                        $("#UErr").html("Invalid Username")
                    }
                })
            }
        }

        function zipSearch(e) {
            if (this.value.length > 1) {

                $("#zc_sr").html("<img src='../imgs/ui-anim_basic_16x16.gif' />");
                $("#zc_sr").show();
                if (e.which == 13) {
                    $.ajax({
                        type: "POST",
                        url: "UserManager.aspx/searchAddZip",
                        data: "{zip:'" + this.value + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        async: false,
                        cache: false,
                        success: function (msg) {
                            zp = msg.d.split(",")
                            zps = "";
                            for (i = 0; i < zp.length; i++)
                                zps += " <b>" + zp[i] + "</b>";

                            
                            if ($("#zc_list b").length == 0)
                                $("#zc_list").html(zps);
                            else
                                $("#zc_list b:last").after(zps);

                            $("#zc_list b").dblclick(dblClickZip);

                            $("#zc_sr").hide();
                            $("#zc_sr").html("<img src='../imgs/ui-anim_basic_16x16.gif' />");
                        },
                        error: function () {
                            $("#zc_sr").html("Unable to reach central server")
                        }
                    })
                    return;       
                }

                $("#zc_sr").show();
                $.ajax({
                    type: "POST",
                    url: "UserManager.aspx/searchZip",
                    data: "{zip:'" + this.value + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: true,
                    cache: false,
                    success: function (msg) {
                        $("#zc_sr").html(msg.d);
                        $("#zc_list b").each(function () {
                            $("#zc_sr #" + $(this).html()).addClass("light_grey");
                        });
                        $("#zc_sr th").click(function () {
                            if (!$(this).hasClass("light_grey")) {
                                $(this).addClass("light_grey");

                                if ($("#zc_list b").length == 0)
                                    $("#zc_list").html("<b>" + $(this).attr("id") + "</b><div>Double click on the zip code to remove</div>");
                                else
                                    $("#zc_list b:last").after(" <b>" + $(this).attr("id") + "</b>");

                                $("#zc_list b").dblclick(dblClickZip);
                            }
                        });
                    },
                    error: function () {
                        $("#zc_sr").html("Unable to reach central server")
                    }
                })
            }
            else {
                $("#zc_sr").hide();
                $("#zc_sr").html("<img src='../imgs/ui-anim_basic_16x16.gif' />");
            }
        }

        function getList() {
            $.ajax({
                type: "POST",
                url: "UserManager.aspx/getList",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: true,
                cache: false,
                success: function (msg) {
                    $("#page_list").html(msg.d[0]);
                    if (msg.d[1] == "1") {
                        $("#bNew").remove()
                        $("#btnAdd").remove()
                        $("#btnRemove").remove()
                        $("#btnDup").remove()
                        $("#bChange").html("View Selected User")
                    }
                    else {
                        $("#bNew").show()
                    }
                }
            })
        }

        function getUsrs(page, alpha, sort, order) {
            $("#rpt").html(getProcessing());

            if (sort == "") sort = "UN"
            if (order == "") order = "ASC"

            $.ajax({
                type: "POST",
                url: "UserManager.aspx/buildList",
                data: "{page:" + page + ",alpha:'" + alpha + "',sSort:'" + sort + "',sOrder:'" + order + "'," + param + "}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: true,
                cache: false,
                success: function (msg) {
                    $("#rpt").html(msg.d);
                    $("#rpt td input:checkbox").click(enableChangeBtn)
                },
                error: function () {
                    $("#dc_prv_d").html("<table class='rpt_tbl'><tr><th>ERROR</th></tr><tr><td>Unable to reach central server</td></tr></table>")
                }
            })
        }

        function sortUsr(s, o) {
            getUsrs(1, '', s, o)
        }

        function prepareWorkingHours() {
            //
            var ar = ['Monday','Tuesday','Wednesday','Thursday','Friday'];
            sOurput = "<table class=frm_tbl_in><tr><th></th><th>From (Hours)</th><th></th><th>To (Hours)</th></tr>";
            sHours = ""
            sMinutes = ""
            for (i=5;i<24;i++)
            {
                sHours += "<option value='" + i + "'>" + pad(i,2) + "</option>"
            }
            for (i=0;i<60;i=i+5)
            {
                sMinutes += "<option value='" + i + "'>" + pad(i, 2) + "</option>"
            }
            for (i=0;i<ar.length;i++)
            {
                sOurput += "<tr id='d_" + (i + 1) + "'><th><label class='check-label'><input type='checkbox' id='chk' value='" + (i + 1) + "' />" + ar[i] + "<span class='checkmark'></span></label></th><td><select id='from_h'>" + sHours + "</select><select id='from_m'>" + sMinutes + "</select></td><td></td><td><select id='to_h'>" + sHours + "</select><select id='to_m'>" + sMinutes + "</select></td></tr>";
            }
            sOurput += "</table>"
            $("#tdWH").html(sOurput);
        }

        function cancelUserForm() {
            $("#UserForm").hide();
            $("#rpt_tbl").show();
        }
        function clearUserForm() {
            $("#UserForm input, #UserForm select").each(function () {
                if ($(this).is(":checkbox"))
                    this.checked = false;
                else if ($(this).is(":text") || $(this).is(":password") || $(this).is(":file"))
                    this.value = "";
                else
                    this.selectedIndex = 0;
            });

            $("#provider_id").val('')
            $("#provider_id").attr('pk', '0')

            $("#zc_list").html("User can work in any area");
            $("#user_id").val("0");
            $("#zc_sr").hide();
            $("#Username")[0].disabled = false;
            $("#Lastname")[0].disabled = false;
            $("#Firstname")[0].disabled = false;
            $("#email_address")[0].disabled = false;
            $("#btnAdd").html("Save");
        }

        function new_user() {
            clearUserForm()
            $("#UserForm").show();
            $("#rpt_tbl").hide();
            $("body").scrollTop(90);
        }

        function duplicateUserForm() {
            $("#user_id").val("0")
            $("#Username").val("")
            $("#Lastname").val("")
            $("#Firstname").val("")
            $("#Username")[0].disabled = false;
            $("#Lastname")[0].disabled = false;
            $("#Firstname")[0].disabled = false;
            $("#email_address")[0].disabled = false;
            $("#btnAdd").html("Save");
        }

        function removeUserForm() {
            if (!confirm("This action is not reversible.\n\nAre you sure, you want to remove selected users?\n(Press 'OK' for 'Yes')"))
                return;
            $.ajax({
                type: "POST",
                url: "UserManager.aspx/removeUser",
                data: "{user:'" + $("#user_id").val() + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: true,
                cache: false,
                success: function (msg) {
                    cancelUserForm()
                    $("#rpt td input:checked").each(function () {
                        $(this).closest("tr").remove()
                    })
                    $("#bChange").hide()
                },
                error: function () {
                    alert("Unable to reach central server. Please check the internet connection")
                }
            })
        }

        function save_user() {
            $("#UErr").html("")
            is_Error = false;

            if ($("#Username").val() == "") {
                $("#Username").addClass("txt_err")
                $("#Username").attr("placeholder", "Username can't be empty")
                is_Error = true;
            }

            if ($("#Pwd1").val() == "" && $("#user_id").val()=="") {
                $("#Pwd1").addClass("txt_err")
                $("#Pwd1").attr("placeholder", "Can't be empty")
                is_Error = true;
            }

            if ($("#Pwd2").val() != $("#Pwd1").val()) {
                $("#Pwd2").addClass("txt_err")
                $("#Pwd2").attr("placeholder", "Enter same password")
                is_Error = true;
            }

            if ($("#Pwd1").val() != "") {
                pwd_response = checkPasswordRules($("#Pwd1").val())
                $("#Pwd_Err").html(pwd_response)
                if (pwd_response != "") {
                    $("#Pwd2").addClass("txt_err")
                    $("#Pwd1").focus();
                    is_Error = true;
                }
            }

            if ($("#Firstname").val() == "") {
                $("#Firstname").addClass("txt_err")
                $("#Firstname").attr("placeholder", "Firstname can't be empty")
                is_Error = true;
            }

            if ($("#Lastname").val() == "") {
                $("#Lastname").addClass("txt_err")
                $("#Lastname").attr("placeholder", "Lastname can't be empty")
                is_Error = true;
            }

            if ($("#rpt td input:checked").length < 2) {
                if ($("#email_address").val() == "") {
                    $("#email_address").addClass("txt_err")
                    $("#email_address").attr("placeholder", "Email address can't be empty")
                    is_Error = true;
                }
                else if (!chk_email($("#email_address").val(), 1)) {
                    $("#EErr").html("Enter a valid email address")
                    $("#EErr").show()
                    is_Error = true;
                }
                else
                    $("#EErr").hide()
            }

            if ($("#UErr").html() != "" || is_Error) return;

            modules = "0"
            b = $("input[id^='pg_']")
            for (i = 0; i < b.length; i++) {
                if (b[i].checked)
                    modules += "," + b[i].value;
            }

            if ($("#Pwd1").val() != "")
                pwd = Sha1.hash($("#Pwd1").val())
            else
                pwd = ""

            $("#rpt_tbl").show();
            $("#UserForm").hide();

            clLoading()
           $.ajax({
                type: "POST",
                url: "UserManager.aspx/saveUser",
                data: "{id:'" + $("#user_id").val() + "',username:'" + $("#Username").val() + "',email:'" + $("#email_address").val() + "',pwd:'" + pwd + "',lastname:'" + clean($("#Lastname").val()) + "',firstname:'" + clean($("#Firstname").val()) + "',IsActive:" + ($("#IsActive")[0].checked ? 1 : 0) + ",IsAdmin:" + ($("#IsAdmin")[0].checked ? 1 : 0) + ",IsManager:" + ($("#IsManager")[0].checked ? 1 : 0) + ",IsAgent:" + ($("#IsAgent")[0].checked ? 1 : 0) + ",IsChangePasswordOnFirstLogin:" + ($("#IsChangePasswordOnFirstLogin")[0].checked ? 1 : 0) + ",pw:'" + clean($("#Pwd1").val()) + "',modules:'" + modules + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: true,
                cache: false,
                success: function (msg) {
                    if ($("#user_id").val()=="0")
                        getUsrs(1, $("#Username").val().substring(0,2), '', '')
                    $("#UErr").html("Saved successfully")
                    
                    clLoading()
                    $("#UserForm").hide();
                    $("#rpt_tbl").show();
                },
                error: function () {
                    alert("Unable to reach central server. Please check the internet connection")
                }
            })
        }
        function changeUser() {
            clLoading();
            clearUserForm()
            sUsrIDs = "0";
            $("#rpt td input:checked").each(function () {
                sUsrIDs += "," + $(this).closest("tr").attr("d")
            })
            $("#user_id").val(sUsrIDs);
            $("#uploadedSign").html("<img src='/imgs/sign/sign_" + $("#user_id").val() + ".jpg' height='150px'>")

            $.ajax({
                type: "POST",
                url: "UserManager.aspx/getUserCredentials",
                data: "{user_id:'" + sUsrIDs + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: true,
                cache: false,
                success: function (msg) {
                    ah = msg.d;
                    if (msg.d[0] != "") { //Pages
                        h = msg.d[0].split(";")
                        for (i = 0; i < h.length; i++) {
                            try {
                                $("#pg_" + h[i])[0].checked = true;
                            }
                            catch (ex) {}
                        }
                    }

                    /////////////////////////////////////////////////////
                    ar = msg.d[1].split("~")
                    $("#Username").val(ar[0]);
                    $("#Lastname").val(ar[1]);
                    $("#Firstname").val(ar[2]);
                    $("#email_address").val(ar[3]);

                    $("#Username").attr("disabled", "true")

                    $("#IsActive")[0].checked = (ar[4] == 1 ? true : false);
                    $("#IsManager")[0].checked = (ar[5] == 1 ? true : false);
                    $("#IsAgent")[0].checked = (ar[6] == 1 ? true : false);
                    $("#IsAdmin")[0].checked = (ar[7] == 1 ? true : false);
                    $("#IsChangePasswordOnFirstLogin")[0].checked = (ar[8] == 1 ? true : false);

                    /////////////////////////////////////////////////////
                    clLoading();

                    $("#UserForm").show();
                    $("#rpt_tbl").hide();
                    $("#btnAdd").html("Update");
                    $("body").scrollTop(90);

                    if ($("#btnAdd").length==0)
                        $("#UserForm select, #UserForm input").attr("disabled","disabled")
                },
                error: function () {
                    alert("Unable to reach central server, please check internet connection.")
                }
            })
        }
        function dblClickZip() {
            $("#zc_sr #" + $(this).html()).removeClass("light_grey");
            $(this).remove();
            if ($("#zc_list b").length == 0)
                $("#zc_list").html("User can work in any area");
        }
        function clearzips() {
            $("#zc_list").html("User can work in any area");
            $("#zc_search").val('')
        }

        function cAll(chk) {
            $(chk).closest("td").find("input:checkbox").each (function () {
                this.checked = chk.checked;
            });
            enableChangeBtn(chk.checked)
        }
        function enableChangeBtn() {
            if ($("#rpt td input:checked").length>0)
                $("#bChange").show()
            else
                $("#bChange").hide()
        }

        function download(t) {
            $("#param").val('download');
            $("#fDwn")[0].submit();
        }
        function getSearchParam() { 
            param = "uStatus:"
            if ($("#sActive")[0].checked)
                param += "1";
            else if ($("#sInActive")[0].checked)
                param += "0";
            else if ($("#sAll")[0].checked)
                param += "2";

            param += ",uRole:"
            if ($("#rAny")[0].checked)
                param += "0,uRoleId:'0'";
            else if ($("#rWith")[0].checked) {
                if ($("#WithRole").val() == "0") {
                    param += "0,uRoleId:'0'";
                    $("#rAny")[0].checked = true;
                }
                else
                    param += "1,uRoleId:'" + $("#WithRole").val() + "'";
            }
            else if ($("#rOnly")[0].checked) {
                if ($("#OnlyRole").val() == "0") {
                    param += "0,uRoleId:'0'";
                    $("#rAny")[0].checked = true;
                }
                else
                    param += "2,uRoleId:'" + $("#OnlyRole").val() + "'";
            }
        }
    </script>
<table id="rpt_tbl" class="main_tbl">
    <tr>
        <td class="frm_td s5">
            <b class="btn3 f_rt margin3 hide" id='bChange' onclick="changeUser()">Change Selected User(s)</b><b class="btn3 f_rt margin3 hide" onclick="new_user()"  id='bNew'>New User</b>  
            <br /><br /> 
            <h>Status:</h>
            <div>
                <label class='check-label'><input type=radio name=Status id='sActive' checked> Only Active Users<span class='radio'></span></label>
                <label class='check-label'><input type=radio name=Status id='sInActive'> Only In-Active Users<span class='radio'></span></label>
                <label class='check-label'><input type=radio name=Status id='sAll'> All Active/In-Active Users<span class='radio'></span></label>
            </div>
            <h>Role:</h>
            <div>
                <label class='check-label'><input type=radio name=role id='rAny' checked /> Users with any Role<span class='radio'></span></label>
                <label class='check-label'><input type=radio name=role id='rWith' /> Users with role:<span class='radio'></span></label>
                <div>
                    <select id=WithRole class="s4"></select> 
                </div>
                <label class='check-label'><input type=radio name=role id='rOnly' /> Users with only role:<span class='radio'></span></label>
                <div>
                    <select id=OnlyRole class="s4"></select> 
                </div>
            </div>
            <b class="btn3 f_rt" onclick="getSearchParam();getUsrs(1, '', '', '')">Show List</b> 
        </td>
        <td id="rpt" class="cnt blk_td"></td>
    </tr>
</table>

<table id="UserForm" class="blk_tbl bt_rht hide">
<tr>
<td class="frm_td s31">
    <div class=fix_btn>
    <input type="hidden" id='user_id' value="0" />
    <b class="btn3 f_rt margin3 s3 cnt" onclick="save_user()" id='btnAdd'>Save</b><br class=clear />
    <b class="btn3 f_rt margin3 s3 cnt" onclick="removeUserForm()" id='btnRemove'>Remove</b><br class=clear /> 
    <b class="btn3 f_rt margin3 s3 cnt" onclick="duplicateUserForm()" id='btnDup'>Duplicate</b><br class=clear />
    <b class="btn3 f_rt margin3 s3 cnt" onclick="cancelUserForm()" id='b1'>Close</b><br class=clear />
    </div>
</td>
<td class="blk_td">
    <table class='frm_tbl tp_rht s100i'>
    <tr><th class=s32>Username (email)</th><td class="tp_rht" colspan=3><input type='text' class=s5 id='Username' />&nbsp;&nbsp;<b class="red" id="UErr"></b></td></tr>
    <tr><th>Password</th><td colspan=3><input type='password' class=s33 id='Pwd1' />&nbsp;Re-enter&nbsp;<input type='password' class=s33 id='Pwd2' /><div class="red" id="Pwd_Err"></div></td></tr>
    <tr><th>Lastname, Firstname</th><td colspan=3><input type='text' id='Lastname' />, <input type='text' id='Firstname' /></td></tr>
    <tr><th>Email Address</th><td colspan=3><input type='text' id='email_address' class=s5 />&nbsp;&nbsp;<b class="red" id="EErr"></td></tr>
    <tr><th>Roles</th><td nowrap colspan=3>
        <div class="s33 fleft"><label class='check-label'><input type='checkbox' id='IsAgent' /> Agent<span class='checkmark'></span></label></div><div class="s33 fleft"><label class='check-label'><input type='checkbox' id='IsManager' /> Manager<span class='checkmark'></span></label></div><div class="s33 fleft red"><label class='check-label'><input type='checkbox' id='IsAdmin' /> <b>Administrator</b><span class='checkmark'></span></label></div>
    </td></tr>
    <tr id='tr_fax' class=hide><th>Fax Cover</th><td colspan=3>
    <table class="frm_tbl_in">
    <tr><th>Scheduler Name</th><td><input type='text' id='sch_name' /></td></tr>
    <tr><th>Contact Number</th><td><input type='text' id='sch_tel' /></td></tr>
    <tr><th>Fax Number</th><td><input type='text' id='sch_fax' /></td></tr>
    </table>
</td></tr>
    <tr><th>Status</th><td colspan=3>
        <div class="s3 fleft"><label class='check-label'><input type='checkbox' id='IsActive' /> Active<span class='checkmark' style="margin-top:2px"></span></label></div>
        <div class="s6 fleft"><label class='check-label'><input type='checkbox' id='IsChangePasswordOnFirstLogin' /> Force to change password on first login<span class='checkmark' style="margin-top:2px"></span></label></div>
    </td></tr>
    <tr><th>Modules</th><td colspan=3 id='page_list' style="list-style: none;"></td></tr>
    </table>
</td>
</tr>
</table>

</form><form id='fDwn' method='post' action='UserManager.aspx'><input type='hidden' name='param' id='param' />
</asp:Content>

