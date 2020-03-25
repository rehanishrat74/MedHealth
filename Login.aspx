<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="MedHealthSolutions.WebForm1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
	<meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
	<title>Secure Log-In</title>
    <link rel='stylesheet' href='Styles/login.css?<%=MedHealthSolutions.Classes.portalVersion.info() %>' type='text/css' media='all' />
    <script type="text/javascript" src="Scripts/jquery-2.1.0.min.js" language="javascript"></script>
    <script type="text/javascript" src="Scripts/Sha1.js" language="javascript"></script>
    <script type="text/javascript" src="Scripts/Sha1.js" language="javascript"></script>
    <script type="text/javascript" src="Scripts/common.js?<%=MedHealthSolutions.Classes.portalVersion.info() %>" language="javascript"></script>
    <meta name='robots' content='noindex,follow' />
</head>
<body>
<script type="text/javascript" language="javascript">
    var not_login = false;
    
    $(document).ready(resetPassword);
    function change_password() {
        if ($("#pwd").val() != "" && $("#pwd").val() != $("#pwd2").val()) {
            $("#login_error").html("<b>Password Expired</b>: Password must be verified")
            $("#pwd2").focus();
            return;
        }

        pwd_response = checkPasswordRules($("#pwd").val())
        if (pwd_response != "") {
            $("#login_error").html("<b>Password Expired</b>: " + pwd_response)
            $("#pwd").focus();
            return;
        }

        $("#progress").show();
        $("table").hide();
        $("#login_error").hide();

        $.ajax({
            type: "POST",
            url: "Login.aspx/change_password",
            data: "{pwd: '" + Sha1.hash($("#pwd").val()) + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: true,
            cache: false,
            success: function (msg) {
                if (msg.d == 'success') {
                    postLogin()
                }
                else if (msg.d == 'error')
                    location.reload();
                else {
                    $("#login_error").html("<b>Password Expired</b>: " + msg.d)
                    $("#progress").hide();
                    $("table").show();
                    $("#login_error").show();
                    $(".reset").hide();
                    $(".login").hide();
                    $("#securityWarning").hide()
                }
            },
            error: function () {
                location.reload();
            }
        })
    }
    function resetPwd() {
        $(".forgot").hide();
        $(".reset").show();
        $("#login_error").hide()
    }

    function do_reset() {

        $("#progress").show();
        $("table").hide();
        $("#login_error").hide();
        $(".reset").hide();

        $.ajax({
            type: "POST",
            url: "Login.aspx/doReset",
            data: "{usr: '" + $("#usr").val() + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: true,
            cache: false,
            success: function (msg) {
                ar = msg.d.split(",")
                if (ar[0] == 'success') {
                    $("#progress").hide();
                    $("#login_error").html("<b>Password Reset Initiated</b>:<br>An email with password reset link is sent to '" + ar[1] + "' for username '" + ar[2] + "'.")
                    $("#login_error").show();
                }
                else if (ar[0] == 'inactive') {
                    $("#progress").hide();
                    $("#login_error").html("<b>Account Disabled</b>:<br>Your account associated with username '" + ar[1] + "' is disabled, please contact support@ramtraxs.com for assistance.")
                    $("#login_error").show();
                }
                else {
                    $("#progress").hide();
                    $(".reset").show();
                    $("#login_error").html("<b>Invalid Username</b>: Please type correct username")
                    $("#login_error").show();
                    $("table").show();
                    $("#securityWarning").hide()

                    $(".forgot").hide();
                }
            },
            error: function () {
                $("#progress").hide();                
                $(".reset").show();
                $("#login_error").html("<b>Denied</b>: Unable to reach central server")
                $("#login_error").show();
                $("table").show();
                $(".forgot").hide();
                $("#securityWarning").hide()
            }
        })
    }
    function resetPassword() {
        if (window.location.search.indexOf("rs=") > 0) {            

            $("#progress").show();
            $("table").hide();
            $("#login_error").hide();

            $.ajax({
                type: "POST",
                url: "Login.aspx/verifyReset",
                data: "{rs: '" + window.location.search.split("=")[1] + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: true,
                cache: false,
                success: function (msg) {
                    ar = msg.d.split(",");
                    if (ar[0] == 'expired') {
                        $("#progress").hide();
                        $("table").show();
                        $("#login_error").html("<b>Password Expired</b>: Please change your password")
                        $("#login_error").show();
                        $(".pwd").show();
                        $(".login").hide();
                        $("#usr").attr("disabled", "disabled");
                        $("#usr").val(ar[1]);
                        $("#pwd").val("");
                        $(".reset").hide();
                        not_login = true;
                        $("#securityWarning").hide()
                    }
                    else {
                        $("#progress").hide();
                        $("table").show();
                        $("#login_error").html("<b>Link Expired</b>: Password reset link is expired. Please initiate new request.")
                        $("#login_error").show();
                        $(".reset").hide();
                        $("#securityWarning").hide()
                    }
                },
                error: function () {
                    $("#progress").hide();
                    $("table").show();
                    $("#login_error").html("<b>ERROR</b>: Unable to reach central server")
                    $("#login_error").show();
                    $("#securityWarning").hide()
                }
            })
        }

    }

    function do_login() {
        if (not_login) {
            change_password()
            return;
        }
        $("#progress").show();
        $("table").hide();
        $("#login_error").hide();
        document.cookie = "TFL=";
        $.ajax({
            type: "POST",
            url: "Login.aspx/doLogin",
            data: "{usr: '" + $("#usr").val() + "', pwd: '" + Sha1.hash($("#pwd").val()) + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: true,
            cache: false,
            success: function (msg) {
                if (msg.d[0] == 'success') {
                    postLogin()
                    //$("#securityWarning").show()
                    //$(".panel-heading").html("<b>Attention</b>")

                    //$("#login_box").hide()
                    //$("#login_box").hide()
                    //$("#progress").hide();
                    //$("#login_error").hide();
                    //$(".logo").hide();
                    //$("#btnBanner").show();
                    //is_banner = true;
                    //document.cookie = msg.d[1];
                }
                else if (msg.d[0] == 'expired') {
                    $("#progress").hide();
                    $("table").show();
                    $("#login_error").html("<b>Password Expired</b>: Please change your password")
                    $("#login_error").show();
                    $(".pwd").show();
                    $(".login").hide();
                    $("#usr").attr("disabled", "disabled");
                    $("#pwd").val("");
                    $(".reset").hide();
                    $("#securityWarning").hide()
                    not_login = true;
                }
                else {
                    $("#progress").hide();
                    $("table").show();
                    $("#login_error").html("<b>ERROR</b>: " + msg.d[0])
                    $("#login_error").show();
                    $(".reset").hide();
                    $("#securityWarning").hide()
                }
            },
            error: function () {
                $("#progress").hide();
                $("table").show();
                $("#login_error").html("<b>ERROR</b>: Unable to reach central server")
                $("#login_error").show();
                $(".reset").hide();
                $("#securityWarning").hide()
            }
        })
    }

    function postLogin() {
        window.open(url_after_login, "_self");
    }

    var url_after_login = "PostLogin.aspx";
    $(document).ready(function () {
        if (GetQueryStringParams("s") == 'session') {
            $("#login_error").html("<b>Session expired</b>: Please login to the system");
            $("#login_error").show();
        } else if (GetQueryStringParams("clear") == 'y') {
            $("#login_error").html("<b>Logged out</b>: You are successfully logged out");
            $("#login_error").show();
        } else if (GetQueryStringParams("s") == 'token') {
            $("#login_error").html("<b>Invalid Token</b>:<br>Unable to validate authentication token");
            $("#login_error").show();
        }

        if (GetQueryStringParams("u") != '' && typeof GetQueryStringParams("u") != 'undefined')
            url_after_login = GetQueryStringParams("u");
        if (GetQueryStringParams("t") != '' && typeof GetQueryStringParams("t") != 'undefined')
            url_after_login = url_after_login + "?tck=" + GetQueryStringParams("t");

        if (url_after_login.toLowerCase().indexOf("default.aspx")>-1)
            url_after_login = "PostLogin.aspx";
    });

    var is_banner = false;
    $(document).keypress(function (e) {
        if (is_banner && (e.which == 13 || e.which == 32))
            postLogin();
        else if (e.which == 13) {
            do_login();
        }
    });
</script>
<div id="login_header"></div>
<div class='login_form'>
	    <div id="login_error" class='hide alert alert-danger s5 cnt'></div>
        <div class='hide alert alert-danger s5 cnt reset'>Make sure to type your correct 'Username' and hit 'Reset Password' to initiate password recovery</div>
        <div class='tcnt hide' id=progress><img src="images/processing.gif" alt='Processing...' /><br /><br /><br /></div>
        <table id="login_box" class='cnt<%=MedHealthSolutions.Classes.config.UseSSOTokenForAuthentication()?" hide":"" %>' >
            <tr><th>Username:</th><td><input type="text" id="usr" name='advance_usr' class="txt" value="" size="20" /></td></tr>
            <tr class='forgot'><th><span class='pwd hide'>New </span>Password:</th><td><input type="password" id="pwd" name='advance_pwd1' class="txt" value="" size="20" /></td></tr>
            <tr class='pwd hide'><th>Confirm Password:</th><td><input type="password" id="pwd2" name='advance_pwd2' class="txt" value="" size="20" /></td></tr>
            <tr><td colspan=2><br />
                <b class="btn5 login forgot" onclick='do_login()'>Log In</b><b class="btn5 hide pwd forgot" onclick='change_password()'>Change Password</b><b class="btn5 hide pwd reset" onclick='do_reset()'>Reset Password</b>            
            </td></tr>
            <tr><td colspan=2><br /><div class='tcnt s2 m_hnd forgot' onclick='resetPwd()'>Reset Password</div>
            </td></tr>
        </table>
        <table class='cnt hide' id="securityWarning">
            <tr><td style="text-align:justify;padding:25px;">This application belongs to MedHealth Solutions Corporation and may be accessed and used by authorized personnel only. MedHealth Solutions Corporation reserves the right to monitor use of this application to ensure security is not compromised and to respond to specific allegations of any misuse. Use of this application shall constitute consent to monitoring for such purposes. In addition, MedHealth Solutions Corporation reserves the right to consent to a valid law enforcement request to search the application for evidence of a crime stored within the system.
                <br /><br /><br />
                <b class="btn5 login forgot" onclick='postLogin()' id="btnBanner"> &nbsp; OK &nbsp; </b>
                </td></tr>
        </table>         
    <div class='panel-footer'>
    &copy; <%=DateTime.Now.Year %> MedHealth Solutions<br /><%=MedHealthSolutions.Classes.portalVersion.info() %></div>
</div>
</body>
</html>
