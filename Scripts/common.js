var month = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
var weekday = ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"];
var WinId;

function reset() {
    location.reload();
}
function GetQueryStringParams(sParam) {
    var sPageURL = window.location.search.substring(1);
    var sURLVariables = sPageURL.split('&');
    for (var i = 0; i < sURLVariables.length; i++) {
        var sParameterName = sURLVariables[i].split('=');
        if (sParameterName[0] == sParam) {
            return sParameterName[1];
        }
    }
}

function goToTop() {
    $(window).scrollTop(0); 
}
function goToByScroll(obj) {
    $('html,body').animate({
        scrollTop: $(obj).offset().top-60},
        'fast');
}

function stripFirstWord(str) {
    f = str.indexOf(" ")
    return str.substr(f+1)
}

function removeEditRows() {
    $(".tdEdit").remove();
}

function toTitleCase(str) {
    return str.replace(
        /\w\S*/g,
        function (txt) {
            return txt.charAt(0).toUpperCase() + txt.substr(1).toLowerCase();
        }
    );
}

function getProcessing() {
    return "<div class='cnt'><img src='images/processing.gif' alt='Processing...'></div>";
}

function correctDate(d) {
    if (d.indexOf("/") < 1 && d.indexOf("-") < 1) {
        if (d.length == 6)
            return d.substring(0, 2) + "/" + d.substring(2, 4) + "/20" + d.substring(4);
        else if (d.length == 8)
            return d.substring(0, 2) + "/" + d.substring(2, 4) + "/" + d.substring(4);
        else
            return d;
    }
    else if (d.length < 10) {
        if (d.indexOf("/"))
            spr = "/"
        else if (d.indexOf("-"))
            spr = "-"

        s1 = d.indexOf(spr, 0)
        s2 = s1 > 0 ? d.indexOf(spr, s1 + 1) : 0

        if (s1 > 0 && s2 > 0) {
            p1 = d.substring(0, s1)
            p2 = d.substring(s1 + 1, s2)
            p3 = d.substring(s2 + 1)

            return (p1.length == 1 ? "0" + p1 : p1) + "/" + (p2.length == 1 ? "0" + p2 : p2) + "/" + (p3.length == 2 ? "20" + p3 : p3);
        }
        else
            return d;
    }
    else
        return d;
}

function is_date(txtDate) {
    var currVal = txtDate;
    if (currVal == '')
        return false;

    //Declare Regex  
    var rxDatePattern = /^(\d{1,2})(\/|-)(\d{1,2})(\/|-)(\d{4})$/;
    var dtArray = currVal.match(rxDatePattern); // is format OK?

    if (dtArray == null)
        return false;

    //Checks for mm/dd/yyyy format.
    dtMonth = dtArray[1];
    dtDay = dtArray[3];
    dtYear = dtArray[5];

    if (dtMonth < 1 || dtMonth > 12)
        return false;
    else if (dtDay < 1 || dtDay > 31)
        return false;
    else if ((dtMonth == 4 || dtMonth == 6 || dtMonth == 9 || dtMonth == 11) && dtDay == 31)
        return false;
    else if (dtMonth == 2) {
        var isleap = (dtYear % 4 == 0 && (dtYear % 100 != 0 || dtYear % 400 == 0));
        if (dtDay > 29 || (dtDay == 29 && !isleap))
            return false;
    }
    return true;
}

function parseDate(str) {
    var mdy = str.split('/');
    return new Date(mdy[2], mdy[0]-1, mdy[1]);
}

function is_future_date(str) {
    dt = parseDate(str)
    dtCurr = new Date()
    if (dt>dtCurr)
        return true;
    else
        return false;
}

function daydiff(first, second) {
    return Math.round((second-first)/(1000*60*60*24));
}

function pad(str, max) {
    str = str.toString();
    return str.length < max ? pad("0" + str, max) : str;
}

function w9v(o) {
    if (WinId && !WinId.closed) {
        //    WinId.close();
        WinId.document.body.innerHTML = "<img src='images/processing.gif' />";
        WinId.focus()
    }
    $.post('ViewerW9.aspx', { office: o }, function (result) {
        WinId = window.open('', 'newwin', 'fullscreen=1,left=0,top=0');
        WinId.document.open();
        WinId.document.write(result);
        WinId.focus()
        WinId.document.close();
    });
}

function viewer(id) {
    if (WinId && !WinId.closed) {
        //    WinId.close();
        WinId.document.body.innerHTML = "<img src='images/processing.gif' />";
        WinId.focus()
    }
    $.post('Viewer.aspx', { suspect: id,page_id: 0,qa: 0 }, function (result) {
        WinId = window.open('', 'newwin', 'fullscreen=1,left=0,top=0');
        WinId.document.open();
        WinId.document.write(result);
        WinId.focus()
        WinId.document.close();
    });
}

function viewer_cm(id) {
    if (WinId && !WinId.closed) {
        //    WinId.close();
        WinId.document.body.innerHTML = "<img src='images/processing.gif' />";
        WinId.focus()
    }
    $.post('Viewer.aspx', { suspect: id,page_id: 0,qa: 3 }, function (result) {
        WinId = window.open('', 'newwin', 'fullscreen=1'); //,left=0,top=0
        WinId.document.open();
        WinId.document.write(result);
        WinId.focus()
        WinId.document.close();
    });
}

function viewer_view(id) {
    if (WinId && !WinId.closed) {
        //    WinId.close();
        WinId.document.body.innerHTML = "<img src='images/processing.gif' />";
        WinId.focus()
    }
    $.post('Viewer.aspx', { suspect: id,page_id: 0,qa: 2 }, function (result) {
        WinId = window.open('', 'newwin', 'fullscreen=0,left=0,top=0');
        WinId.document.open();
        WinId.document.write(result);
        WinId.focus()
        WinId.document.close();
    });
}

function viewer_annotate(id,f) {
    if (WinId && !WinId.closed) {
        //    WinId.close();
        WinId.document.body.innerHTML = "<img src='images/processing.gif' />";
        WinId.focus()
    }
    $.post('ViewerAnnotate.aspx', { suspect: id,page_id: 0,qa: 0, fileId:f }, function (result) {
        WinId = window.open('', 'newwin', 'fullscreen=1,left=0,top=0');
        WinId.document.open();
        WinId.document.write(result);
        WinId.focus()
        WinId.document.close();
    });
}

function remove_attached_pdf(id,f,obj) {
    $(obj).closest("div").remove()
    $.ajax({
        type: "POST",
        url: "ACN_Measure.aspx/remove_attached_pdf",
        data: "{suspect:" + id + ",fl:" + f + "}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        cache: false,
        success: function (msg) {
            getSuspectMeasureInfo()
            return;
        }
    })
}

function viewer_qa(id) {
    if (WinId && !WinId.closed) {
        //    WinId.close();
        WinId.document.body.innerHTML = "<img src='images/processing.gif' />";
        WinId.focus()
    }
    $.post('Viewer.aspx', { suspect: id,page_id: 0,qa: 1 }, function (result) {
        WinId = window.open('', 'newwin','left=0,top=0');
        WinId.document.open();
        WinId.document.write(result);
        WinId.focus()
        WinId.document.close();
    });
}

function viewer_prospective(id) {
    if (WinId && !WinId.closed) {
        //    WinId.close();
        WinId.document.body.innerHTML = "<img src='images/processing.gif' />";
        WinId.focus()
    }
    $.post('Prospective_Viewer.aspx', { suspect: id}, function (result) {
        WinId = window.open('', 'newwin', 'fullscreen=1,left=0,top=0');
        WinId.document.open();
        WinId.document.write(result);
        WinId.focus()
        WinId.document.close();
    });
}

function viewer_page(id,page) {
    if (WinId && !WinId.closed) {
        //    WinId.close();
        WinId.document.body.innerHTML = "<img src='images/processing.gif' />";
        WinId.focus()
    }
    $.post('Viewer.aspx', { suspect: id,page_id: page,qa: 0 }, function (result) {
        WinId = window.open('', 'newwin', 'fullscreen=1,left=0,top=0');
        WinId.document.open();
        WinId.document.write(result);
        WinId.focus()
        WinId.document.close();
    });
}

function viewer_page2(id,page) {
    if (WinId && !WinId.closed) {
        //    WinId.close();
        WinId.document.body.innerHTML = "<img src='images/processing.gif' />";
        WinId.focus()
    }
    $.post('Viewer.aspx', { suspect: id,page_id: -1,page_pk: page,qa: 0 }, function (result) {
        WinId = window.open('', 'newwin', 'fullscreen=1,left=0,top=0');
        WinId.document.open();
        WinId.document.write(result);
        WinId.focus()
        WinId.document.close();
    });
}

/* email validation */
function chk_email(email, force_email) {

    if (force_email == 0 && email == "")
        return true;

    var filter = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;

    if (!filter.test(email))
        return false;
    else
        return true;
}

/* Loading Diaglog */
function clLoading() {
    $("#overlay").toggle()
    $("#dialog_loading").toggle()
}

function showDialog(css_class, shtml, title) {
    makeDrill(title, shtml, "", 0)
}
function closeDialog() {
    $("#overlay").hide()
    $("#dialog").hide()    
}

/* Error Tip */
function error_tip(obj, msg) {
    //r_error_tip(obj)
    $("#tt_error").remove();
    $(obj).focus();
    $(obj).select();
    $(obj).after("<table id='tt_error'><tr><th>&nbsp;</th><td>" + msg + "</td><th>&nbsp;</th></tr></table>");
}

function r_error_tip(obj) {
    $(obj).closest("td").find("#tt_error").remove();
}

function clean(raw_string) {
    try { 
        return raw_string.replace(/'/g, "`").replace(/\\/g, "\\\\");
    }
    catch(err) {
        try { 
            return raw_string.replace("'","`");
        }
        catch(ex) {
            return "";
        }
    }
    //replace(/\n/g, '<br />');
    
}

/* Menu Script*/
(function ($) {
    $(document).ready(function () {
        $('#cssmenu').prepend('<div id="menu-button">Menu</div>');
        $('#cssmenu #menu-button').on('click', function () {
            var menu = $(this).next('ul');
            if (menu.hasClass('open')) {
                menu.removeClass('open');
            }
            else {
                menu.addClass('open');
            }
        });
    });
})(jQuery);
/* Menu Script*/


/*Profile Common Code Starts*/
    function getProfile() {
        uf = $("#user_info").val().split("~")
        $("#Prf_Username").attr('user_pk',uf[0])
        $("#Prf_Username").html(uf[1])
        $("#Prf_Lastname").val(uf[2])
        $("#Prf_Firstname").val(uf[3])
        $("#Prf_email_address").val(uf[4])

        $("#overlay").show();
        $("#profile").show();

        if ($("#tdUserProfile").text()=="")
        {
            $("#tdUserProfile").html("<img src='../images/processing.gif' />&nbsp;Loading Profile....") 
            $.ajax({
                type: "POST",
                url: "UserManager.aspx/getProfileInfo",
                data: "{id:" + $("#Prf_Username").attr('user_pk') + "}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: true,
                cache: false,
                success: function (msg) { $("#tdUserProfile").html(msg.d); }
            })
        }
    }

    function saveProfile() {
        if ($("#Prf_Lastname").val() == "") {
            $("#Prf_Err").html("Firstname can't be empty")
            $("#Prf_Lastname").focus();
            return;
        }
        else if ($("#Prf_Firstname").val() == "") {
            $("#Prf_Err").html("Lastname can't be empty")
            $("#Prf_Firstname").focus();
            return;
        }
        else if ($("#Prf_email_address").val() == "") {
            $("#Prf_Err").html("Email address can't be empty")
            $("#Prf_email_address").focus();
            return;
        }
        else if ($("#Prf_Pwd1").val()!="" && $("#Prf_Pwd1").val() != $("#Prf_Pwd2").val()) {
            $("#Prf_Err").html("Password must be verified")
            $("#Prf_Pwd1").focus();
            return;
        }

        pwd_response = checkPasswordRules($("#Prf_Pwd1").val())
        if (pwd_response!="") {
            $("#Prf_Err").html(pwd_response)
            $("#Prf_Pwd1").focus();
            return;
        }

        $("#ctl00_lblFullname").html($("#Prf_Lastname").val() + ", " + $("#Prf_Firstname").val() + "<input type=hidden id='user_info' value='"+ $("#Prf_Username").attr('user_pk') + "~" + $("#Prf_Username").html() + "~" + $("#Prf_Lastname").val() + "~" + $("#Prf_Firstname").val() + "~" + $("#Prf_email_address").val() +"'>");
        closeProfile()

        clLoading()

        $.ajax({
            type: "POST",
            url: "UserManager.aspx/updateProfile",
            data: "{id:" + $("#Prf_Username").attr('user_pk') + ",email:'" + clean($("#Prf_email_address").val()) + "',pwd:'" + Sha1.hash($("#Prf_Pwd1").val()) + "',lastname:'" + clean($("#Prf_Lastname").val()) + "',firstname:'" + clean($("#Prf_Firstname").val()) + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: true,
            cache: false,
            success: function (msg) {
                if (msg.d=="") {
                    window.open("Login.aspx?clear=y","_self");
                }
                else
                {
                    clLoading()
                    $("#overlay").show();
                    $("#profile").show();
                    $("#Prf_Err").html(msg.d)
                    $("#Prf_Pwd1").focus();
                }
            },
        })
    }
    
    function closeProfile() {
        $("#overlay").hide();
        $("#profile").hide();
    }

    function makeDrill(sTitle,sHTML,sExtraButtons,iTop) {
        if (iTop==0)
            iTop = $(window).scrollTop()+30;
        iZIndex = 10000 + $(".drill").length;
        sOutput = "<div class='drill_overlay' style='z-index: "+ (iZIndex-1) +";'> </div><div class='drill' style='top:"+iTop+"px;z-index: "+ (iZIndex) +";'>";
        sOutput = sOutput + "<h1>"+ sTitle +"<p>"+sExtraButtons+"<img onclick='closeDrill(this)' src='imgs/close.png' /></p></h1>";
        sOutput = sOutput + "<span id=dialogHTML>" + sHTML + "</span>";
        sOutput = sOutput + "<h6>"+ $("#cr").html() +"</h6>";
        sOutput = sOutput + "</div>";

        $("#cr").before(sOutput);
    }

    function closeDrill(obj) {
        $(obj).closest("div").remove();
        $(".drill_overlay").last().remove();
    }

        var currentMousePos = { x: -1, y: -1 };
        $(document).mousemove(function (event) {
            currentMousePos.x = event.pageX;
            currentMousePos.y = event.pageY;
        });
/*Profile Common Code Ends*/

    function checkPasswordRules(password) {
			//To check a password between 8 to 15 characters which contain at least one lowercase letter, one uppercase letter, one numeric digit, and one special character
			var rules=  /^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[^a-zA-Z0-9])(?!.*\s).{8,25}$/; 	    
		    if (!password.match(rules))   
		        return "New password must be atleast 8 charactar long and must have at least one lowercase letter, one uppercase letter, one numeric digit, and one special character";

			var hasTripple = /(.)\1\1/.test(password);
			if(hasTripple)
				return "New password must not contain 3 or more consecutive repeating characters";

			return "";
    }

    function makeFullDrill(sTitle,sHTML,sExtraButtons,clearAll) {
        if (clearAll) {
            $(".full_drill").remove();
            iZIndex = 1000;
        }
        else {
            iZIndex = 1000 + $(".full_drill").length;
        }

        $("#td_page_buttons").html(sExtraButtons + " <b class=btn6 onclick='closeFullDrill(this,0)'>Close</b>")

        sOutput = "<div class='full_drill'>";
        sOutput = sOutput + "<span>"+ sTitle +"</span>";
        $("#td_page_buttons").show()
        $("#page_filters1").hide()
        $("#page_filters2").hide()
        $("#page_buttons").hide()
        window.scrollTo(0, 0);
        
        
        sOutput = sOutput + sHTML;
        //sOutput = sOutput + "<b>"+ $("#cr").html() +"</b>";
        sOutput = sOutput + "</div>";

        $("#cr").before(sOutput);
    }

    function closeFullDrill(obj,l) {
        $(".full_drill").remove();
        if (l==0) {
            $("#dashboard_main").show();
            $("#page_filters1").show()
            $("#page_filters2").show()
            $("#page_buttons").show()
            $("#td_page_buttons").hide()

        }
    }
//Session Management
        var ksto;
        var ksto_timer;
        function keep_session() {
            var dt = new Date();
            createCookie("last_click", dt.getTime());
            clearTimeout(ksto)
            clearTimeout(ksto_timer)
            $(".session_expire").hide();            
            ksto_timer = setTimeout(function () { warning_session(); }, timeout_ms);
        }
        function timer_session() {
            lc = parseInt(readCookie("last_click"))
            var ct = (new Date()).getTime();
            if (ct-lc<(timeout_ms/2)) {
                keep_session()
                return;
            }

            iSeconds = parseInt($("#se_seconds").text())
            iSeconds=iSeconds-1;
            if (iSeconds < 1)
                window.open("/Login.aspx?clear=y", "_self")
            else {
                $("#se_seconds").text(iSeconds)
                ksto = setTimeout(function () { timer_session(); }, 1000);
            }
        }
        function warning_session() {
            lc = parseInt(readCookie("last_click"))
            var ct = (new Date()).getTime();
            if (ct-lc<(timeout_ms/2)) {
                keep_session()
            }
            else {
                $("#se_seconds").text(30);
                $(".session_expire").show();
                ksto = setTimeout(function () { timer_session(); }, 1000);
            }
        }
        function verify_session() {            
            $.ajax({
                type: "POST",
                url: "Login.aspx/verify_session",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    if (msg.d=="")
                        location.reload();
                    else
                        setTimeout(function () { verify_session(); }, 600000);
                },
                error: function () {
                    location.reload();
                }
            })
        }  
/* Cookies Read Write */
function createCookie(name, value, days) {
    var expires;

    if (days) {
        var date = new Date();
        date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
        expires = "; expires=" + date.toGMTString();
    } else {
        expires = "";
    }
    document.cookie = encodeURIComponent(name) + "=" + encodeURIComponent(value) + expires + "; path=/";
}

function readCookie(name) {
    var nameEQ = encodeURIComponent(name) + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) === ' ') c = c.substring(1, c.length);
        if (c.indexOf(nameEQ) === 0) return decodeURIComponent(c.substring(nameEQ.length, c.length));
    }
    return null;
}

function eraseCookie(name) {
    createCookie(name, "", -1);
}
/* Cookies Read Write */

/* Project & Group Load from Dashboard Class */
var channel_label = "";
var project_label = "";

var chaselist_options = "";
var channel_options = "";
var project_options = "";
var state_options = "";
var status1_options = "";
var status2_options = "";

var g_group = "0"
var g_channel = "0"
var g_project = "0"
var g_state = "0"
var g_status0 = "0"
var g_status1 = "0"
var g_status2 = "0"
var hideStatus = true;
var isLeftFilters = 0;

function makeLeftFilters() {
    isLeftFilters = 1;
    makeTopFilters()
}

function makeTopFilters() {
    cCookie = getCookie("TFL")
    if (cCookie!="") {
        arCookie = cCookie.split("~")
        g_group = arCookie[0]
        g_channel = arCookie[1]
        g_project = arCookie[2]
        g_state = arCookie[3]
        g_status0 = arCookie[4]
        g_status1 = arCookie[5]
        g_status2 = arCookie[6]
    }

    initializeFilters()
}

function initializeFilters() {
    $.ajax({
        type: "POST",
        url: "Default.aspx/initializeFilters",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            chaselist_options = msg.d[0]
            channel_options = msg.d[1]
            project_options = msg.d[2]
            state_options = msg.d[3]
            status0_options = msg.d[4]
            status1_options = msg.d[5]
            status2_options = msg.d[6]

            channel_label = msg.d[7];
            project_label = msg.d[8];

            if (isLeftFilters == 0) {
                $("#page_filters1").html("<select class='db_cmb s6' id='projectGroup' onchange='gTF(1)' multiple data-placeholder='Pick Chase-Lists'></select> <select class='db_cmb s6' id='channel' onchange='gTF(2)' multiple data-placeholder='Pick " + channel_label + "'></select> <select class='db_cmb s4' id='project' onchange='gTF(3)' multiple data-placeholder='Pick " + project_label + "'></select>  <select class='db_cmb s35' id='state' onchange='gTF(4)' multiple data-placeholder='Pick State'></select>");
                $("#page_filters2").html("<select class='db_cmb s6' id='chaseStatus0' onchange='gTF(5)' multiple data-placeholder='Pick Chase Status Type'></select>  <select class='db_cmb s6' id='chaseStatus1' onchange='gTF(6)' multiple data-placeholder='Pick Chase Status'></select> <select class='db_cmb s6' id='chaseStatus2' onchange='gTF(7)' multiple data-placeholder='Pick Chase Detail'></select>");
                $("#td_apply").html("<b class=btn6 onclick='apply_filter()'>Apply Filters</b>");
            }
            else {
                $("#leftFilters").html("<select class='db_cmb s6' id='projectGroup' onchange='gTF(1)' multiple data-placeholder='Pick Chase-Lists'></select> <select class='db_cmb s6' id='channel' onchange='gTF(2)' multiple data-placeholder='Pick " + channel_label + "'></select> <select class='db_cmb s6' id='project' onchange='gTF(3)' multiple data-placeholder='Pick " + project_label + "'></select>");
                $("#leftFilters").append("<select class='db_cmb s6' id='state' onchange='gTF(4)' multiple data-placeholder='Pick State'></select>  <select class='db_cmb s6' id='chaseStatus0' onchange='gTF(5)' multiple data-placeholder='Pick Chase Status Type'></select>  <select class='db_cmb s6' id='chaseStatus1' onchange='gTF(6)' multiple data-placeholder='Pick Chase Status'></select> <select class='db_cmb s6' id='chaseStatus2' onchange='gTF(7)' multiple data-placeholder='Pick Chase Detail'></select>");
                $("#leftFilters").append("<b class='btn6 f_rt' onclick='apply_filter()'>Apply Filters</b><br class=clear>");
            }
            buildFilterOptionsAll(0)
            $(".db_cmb").chosen()
            makeTopFilterSelections()
        }
    })
}

function gTF(n) {
    g_group = "0"
    g_channel = "0"
    g_project = "0"
    g_state = "0"
    g_status0 = "0"
    g_status1 = "0"
    g_status2 = "0"
    if ($("#projectGroup").val()!=null)
        g_group = $("#projectGroup").val().toString();
    if ($("#channel").val()!=null)
        g_channel = $("#channel").val().toString();
    if ($("#project").val()!=null)
        g_project = $("#project").val().toString();
    if ($("#state").val()!=null)
        g_state = $("#state").val().toString();
    if ($("#chaseStatus0").val() != null)
        g_status0 = $("#chaseStatus0").val().toString();
    if ($("#chaseStatus1").val()!=null)
        g_status1 = $("#chaseStatus1").val().toString();
    if ($("#chaseStatus2").val()!=null)
        g_status2 = $("#chaseStatus2").val().toString();

    buildFilterOptionsAll(n);
}
function apply_filter() {
    gTF(0)
    if ($("#projectGroup").val()!=null)
        g_group = $("#projectGroup").val().toString();
    if ($("#channel").val()!=null)
        g_channel = $("#channel").val().toString();
    if ($("#project").val()!=null)
        g_project = $("#project").val().toString();
    if ($("#state").val()!=null)
        g_state = $("#state").val().toString();
    if ($("#chaseStatus0").val() != null)
        g_status0 = $("#chaseStatus0").val().toString();
    if ($("#chaseStatus1").val()!=null)
        g_status1 = $("#chaseStatus1").val().toString();
    if ($("#chaseStatus2").val()!=null)
        g_status2 = $("#chaseStatus2").val().toString();

    makeTopFilterSelections()

    lastCookie = "TFL=" + g_group + "~" + g_channel + "~" + g_project + "~" + g_state + "~" + g_status0 + "~" + g_status1 + "~" + g_status2 + ";";
    $.ajax({
        type: "POST",
        url: "Default.aspx/updateCookie",
        data: "{Cookie:'" + lastCookie + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {return;}
    })

    document.cookie = lastCookie
    topFilter();
}
function makeTopFilterSelections() {
    if (g_group!=0) {
        $("#projectGroup option").each(function() {  
            if (checkIfExistsInList(g_group.split(","),$(this).attr("value"))) 
                $(this).attr("selected","selected");
        })
        $("#projectGroup").trigger("chosen:updated");
        }

    if (g_channel!=0) {
        $("#channel option").each(function() {  
            if (checkIfExistsInList(g_channel.split(","),$(this).attr("value"))) 
                $(this).attr("selected","selected");
        })
        $("#channel").trigger("chosen:updated");
        }

    if (g_project!=0) {
        $("#project option").each(function() {
            if (checkIfExistsInList(g_project.split(","),$(this).attr("value"))) 
                $(this).attr("selected","selected");
        })
        $("#project").trigger("chosen:updated");
    }

    if (g_state!=0) {
        $("#state option").each(function() {
            if (checkIfExistsInList(g_state.split(","),$(this).attr("value"))) 
                $(this).attr("selected","selected");
        })
        $("#state").trigger("chosen:updated");
    }

    if (g_status0 != 0) {
        $("#chaseStatus0 option").each(function () {
            if (checkIfExistsInList(g_status0.split(","), $(this).attr("value")))
                $(this).attr("selected", "selected");
        })
        $("#chaseStatus0").trigger("chosen:updated");
    }

    if (g_status1!=0) {
        $("#chaseStatus1 option").each(function() {  
            if (checkIfExistsInList(g_status1.split(","),$(this).attr("value"))) 
                $(this).attr("selected","selected");
        })
        $("#chaseStatus1").trigger("chosen:updated");
        }

    if (g_status2!=0) {
        $("#chaseStatus2 option").each(function() {  
            if (checkIfExistsInList(g_status2.split(","),$(this).attr("value"))) 
                $(this).attr("selected","selected");
        })
        $("#chaseStatus2").trigger("chosen:updated");
        }
}

function buildFilterOptionsAll(n) {
    if (n<1)
        buildFilterOptions("projectGroup", chaselist_options, "", "", "", "", "")
    if (n<2)
        buildFilterOptions("channel", channel_options, g_group, "", "", "", "")
    if (n<3)
        buildFilterOptions("project", project_options, g_group, g_channel, "", "", "")
    if (n<4)
        buildFilterOptions("state", state_options, g_group, g_channel, g_project, "", "")
    if (n < 5)
        buildFilterOptions("chaseStatus0", status0_options, g_group, g_channel, g_project, "","")
    if (n < 6)
        buildFilterOptions("chaseStatus1", status1_options, g_group, g_channel, g_project, g_status0,"")
    if (n<7)
        buildFilterOptions("chaseStatus2", status2_options, g_group, g_channel, g_project, g_status0, g_status1)
}
function getParentIDs(cmb,pIDs) {
    if (pIDs!="" && pIDs=="0") {
        pIDs = "";
        $("#"+cmb+" option").each(function(){
            if (pIDs!="") 
                pIDs += ","
            pIDs += $(this).val()
        })
    }
    return pIDs;
}
function buildFilterOptions(cCmb, sOptions, pGroup, pChannel, pProject, pChase0, pChase1) {
    try {
        pGroup = getParentIDs("projectGroup",pGroup)
        pChannel = getParentIDs("channel",pChannel)
        pProject = getParentIDs("project",pProject)
        pChase0 = getParentIDs("chaseStatus0", pChase0)
        pChase1 = getParentIDs("chaseStatus1", pChase1)

        arGroup = pGroup.split(",")
        arChannel = pChannel.split(",")
        arProject = pProject.split(",")
        arChase0 = pChase0.split(",")
        arChase1 = pChase1.split(",")

        arOpt = sOptions.split("~")
        htmlOptions = "";
        lastAdded = "";
        for (o=0;o<arOpt.length;o++) {
            arVlu = arOpt[o].split("^")
            if ((pGroup=="" || checkIfExistsInList(arGroup,arVlu[2]))
                && (pChannel=="" || checkIfExistsInList(arChannel,arVlu[3]))
                && (pProject=="" || checkIfExistsInList(arProject,arVlu[4]))
                && (pChase0 == "" || checkIfExistsInList(arChase0, arVlu[5]))
                && (pChase1 == "" || checkIfExistsInList(arChase1, arVlu[6]))
            ) {
                if (lastAdded!=arVlu[0]) {
                    lastAdded=arVlu[0];
                    htmlOptions += "<option value='"+arVlu[0]+"'>"+arVlu[1]+"</option>"
                }
            }
        }
        $("#"+cCmb).html(htmlOptions);
        $("#"+cCmb).trigger("chosen:updated");
    }
    catch(err) {
        $("#page_filters1").hide();
        $("#page_filters2").hide();
        $("#td_apply").hide();
    }
}

function checkIfExistsInList(ids1, v) {
    if (typeof(v) == "undefined" || v == '0')
        return true;

    if (v.indexOf(",")>0) {
        ids2 = v.split(",")
        for(i1=0;i1<ids1.length;i1++) {
            for(i2=0;i2<ids2.length;i2++) {
                if (ids1[i1]==ids2[i2] && ids1[i1]!="0" && ids2[i2]!="0")
                    return true;
            }
        }
    }
    else {
        for(i1=0;i1<ids1.length;i1++) {
                if (ids1[i1]==v)
                    return true;
        }
    }

    return false;
}

function getCookie(cname) {
    var name = cname + "=";
    var ca = document.cookie.split(';');
    for(var i = 0; i <ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0)==' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length,c.length);
        }
    }
    return "";
}
function getProjectLists() {
    //Only Used where top filters are not present
    $.ajax({
        type: "POST",
        url: "Default.aspx/initializeFilters",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            buildFilterOptions("projectGroup", "0^All^0^0^0^0~" + msg.d[0], "", "", "", "","")
            buildFilterOptions("project", "0^All^0^0^0^0~" + msg.d[2], "", "", "", "", "")
            $("#lblProject").html(msg.d[7]);
        }
    })
}
function makeLeftFiltersDD() {
    //Call gTF(99); on main function in module to get selections in g_ variables
    //Only Used where top filters are not present
    $.ajax({
        type: "POST",
        url: "Default.aspx/initializeFilters",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            createLeftFilterDD("projectGroup", msg.d[0], "Chase List", "leftFilters",0,1)
            createLeftFilterDD("channel", msg.d[1], msg.d[6], "leftFilters",0,2)
            createLeftFilterDD("project", msg.d[2], msg.d[7], "leftFilters",0,3)
            createLeftFilterDD("state", msg.d[3], "State", "leftFilters",0,4)
            createLeftFilterDD("chaseStatus1", msg.d[4], "Chase Status", "leftFilters",0,5)
            createLeftFilterDD("chaseStatus2", msg.d[5], "Chase Detail", "leftFilters",0,6)
        }
    })
}
function createLeftFilterDD(id, sOptions, label, parent, no_label,n) {
    if (no_label==0)
        $("#" + parent).append(label + ":<br>")
    sOptions = "0^All^0^0^0^0~" + sOptions;
    arOpt = sOptions.split("~")
    htmlOptions = "";
    lastAdded = "";
    for (o=0;o<arOpt.length;o++) {
        arVlu = arOpt[o].split("^")
        if (lastAdded != arVlu[0]) {
            lastAdded = arVlu[0];
            htmlOptions += "<option value='" + arVlu[0] + "'>" + arVlu[1] + "</option>"
        }
    }
    $("#" + parent).append("<select id='" + id + "' class=s8 multiple data-placeholder='Pick " + label + "' onchange='gTF(" + n + ")'>" + htmlOptions + "</select>")
    $("#" + id).change(topFilter)
}

function makeCustomFilters() {
    //Call gTF(99); on main function in module to get selections in g_ variables
    //Only Used where top filters are not present
    $.ajax({
        type: "POST",
        url: "Default.aspx/initializeFilters",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            chaselist_options = msg.d[0]
            channel_options = msg.d[1]
            project_options = msg.d[2]
            state_options = msg.d[3]
            status1_options = msg.d[4]
            status2_options = msg.d[5]

            channel_label = msg.d[6];
            project_label = msg.d[7];

            createLeftFilterDD("projectGroup", chaselist_options, "Chase List", "leftFilters", 1,1)
            createLeftFilterDD("channel", channel_options, channel_label, "leftFilters", 1,2)
            createLeftFilterDD("project", project_options, project_label, "leftFilters", 1,3)

            $("#leftFilters select").chosen()

            getInitialize();
        }
    })
}
/* Project & Group Load from Dashboard Class */


/*Provider/Member Search*/
function showProvider(hideOtherProvider) {
    shtml = "<table class='blk_tbl'><tr><td id='searchInputs' class='frm_td s4' style='padding-top:0;'>";
    shtml += "  <b>Provider Name (Last, First):</b><br>";
    shtml += "  <input id='searchProviderName' class='s4 search' placeholder='Lastname, Firstname'><br>";
    shtml += "  <b>Provider Specialty:</b><br>";
    shtml += "  <input id='searchProviderSpecialty' class='s4 search' placeholder='Provider Specialty'><br>";
    shtml += "  <b>Provider Group:</b><br>";
    shtml += "  <input id='searchProviderGroup' class='s4 search' placeholder='Provider Group'><br>";
    shtml += "  <b>Plan Provider ID:</b><br>";
    shtml += "  <input id='searchPlanProviderID' class='s4 search' placeholder='Plan Provider ID'><br>";
    shtml += "  <b>NPI:</b><br>";
    shtml += "  <input id='searchNPI' class='s4 search' placeholder='NPI'><br>";
    shtml += "  <b>Effective Date Range:</b><br>";
    shtml += "  <input id='searchDateFrom' class='s3 dtmsk' placeholder='__/__/____' autocomplete='off'> to <input id='searchDateTo' class='s3 dtmsk' placeholder='__/__/____' autocomplete='off'><br>";
    shtml += "  <b>Provider Tax ID Number:</b><br>";
    shtml += "  <input id='searchProviderTaxID' class='s4 search' placeholder='Provider Tax ID Number'><br>";
    shtml += "  <b>Office Street Address:</b><br>";
    shtml += "  <input id='searchOfficeStreetAddress' class='s4 search' placeholder='Office Street Address'><br>";
    shtml += "  <b>Office State:</b><br>";
    shtml += "  <input id='searchProviderState' class='s4 search' placeholder='Office State'><br>";
    shtml += "  <div class='rt'><a href='javascript:resetProviderSearch();'>Reset Search</a><br><b class=btn4 onclick='serviceProvider(1,\"\",\"\")'>Search</b></div>";
    shtml += "  </td>";
    shtml += "<td id='rpt' class='cnt blk_td'>"
    shtml += "<div class=h9 id='searchResults' style='overflow:auto;'><div class='no-result'>Use search options to find providers</div></div>";
    shtml += "<div class='rt frm_td'>";
    if (hideOtherProvider!=1)
        shtml += "<span id='spnOtherProvider'><label class='check-label check-label-inline'><input type='checkbox' id='searchIsOtherProvider'> Other Provider<span class='checkmark'></span></label> <input id='searchOtherProvider' class='s5'>&nbsp;</span>";
    shtml += "<b class='btn4 s2 cnt' onclick='serviceProviderSelected()'>OK</b> <b class='btn4 s2 cnt' onclick='closeDialog()'>Cancel</b></div></td>";
    shtml += "</tr></table>";

    $("#dialog").html(shtml);
    $("#dialog").attr("class", "s95")
    $("#overlay").show()
    $("#dialog").show()

    $("#searchInputs input").keyup(function (event) {
        if (event.keyCode == 13) {
            serviceProvider(1,'','')
        }
    });

    $('#searchInputs .dtmsk').mask('00/00/0000', { placeholder: "__/__/____" }, { 'translation': { 0: { pattern: /[0-9*]/ } } });
    $('#searchInputs .dtmsk').change(function () {
        if (this.value != "" && !is_date(this.value)) {
            $(this).addClass("txt_err");
            this.focus()
        }
        else {
            $(this).removeClass("txt_err");
        }
    });

}
function showMemberSearch() {
    shtml = "<table class='blk_tbl'><tr><td id='searchInputs' class='frm_td s4' style='padding-top:0;'>";
    shtml += "  <b>Chase ID:</b><br>";
    shtml += "  <input id='searchChaseID' class='s4 search'><br>";
    shtml += "  <b>Member ID:</b><br>";
    shtml += "  <input id='searchMemberID' class='s4 search'><br>";
    shtml += "  <b>Member Name (Last, First):</b><br>";
    shtml += "  <input id='searchMemberName' class='s4 search'><br>";
    shtml += "  <b>DOB:</b><br>";
    shtml += "  <input id='searchDOB' class='s4 dtmsk search' placeholder='__/__/____' autocomplete='off'><br>";
    shtml += "  <div class='rt'><a href='javascript:resetProviderSearch();'>Reset Search</a></div>";
    shtml += "  </td>";
    shtml += "<td id='rpt' class='cnt blk_td'>"
    shtml += "<div class=h9 id='searchResults' style='overflow:auto;'><div class='no-result'>Use search options to find members</div></div>";
    shtml += "<div class='rt frm_td'>";
    shtml += "<b class='btn4 s2 cnt' onclick='closeDialog()'>Cancel</b> <b class='btn4 s2 cnt' onclick='memberSelected()'>OK</b></div></td>";
    shtml += "</tr></table>";

    $("#dialog").html(shtml);
    $("#dialog").attr("class", "s12")
    $("#overlay").show()
    $("#dialog").show()

    $("#searchInputs input").keyup(function (event) {
        if (event.keyCode == 13) {
            resultsMemberSearch(1)
        }
    });

    $('#searchInputs .dtmsk').mask('00/00/0000', { placeholder: "__/__/____" }, { 'translation': { 0: { pattern: /[0-9*]/ } } });
    $('#searchInputs .dtmsk').change(function () {
        if (this.value != "" && !is_date(this.value)) {
            $(this).addClass("txt_err");
            this.focus()
        }
        else {
            $(this).removeClass("txt_err");
        }
    });

}
function resetProviderSearch() {
    $("#spnOtherProvider").show()
    $("#searchInputs input").val("")
    $("#searchResults").html("<div class='no-result'>Use search options to find providers</div>")
}
var sp_sort = "PN";
var sp_order = "ASC";
function sp_so(s, o) { serviceProvider(1,s,o)}
function sp_pg(p) { serviceProvider(p, sp_sort, sp_order) }
function serviceProvider(page, s, o) {
    if (clean($("#searchProviderName").val()) == '' && clean($("#searchProviderSpecialty").val()) == '' && clean($("#searchProviderGroup").val()) == '' && clean($("#searchPlanProviderID").val()) == '' && clean($("#searchNPI").val()) == '' && clean($("#searchDateFrom").val()) == '' && clean($("#searchDateTo").val()) == '' && clean($("#searchProviderTaxID").val()) == '' && clean($("#searchOfficeStreetAddress").val()) == '' && clean($("#searchProviderState").val())=='') {
        $("#searchResults").html('<div class="no-result">Please select search criteria</div>')
        return;
    }
    sp_sort = s
    sp_order = o
    $("#searchResults").html(getProcessing())
    $.ajax({
        type: "POST",
        url: "ChaseManager.aspx/serviceProvider",
        data: "{providerName:'" + clean($("#searchProviderName").val()) + "', providerSpeciatly:'" + clean($("#searchProviderSpecialty").val()) + "', providerGroup:'" + clean($("#searchProviderGroup").val()) + "', planProviderId:'" + clean($("#searchPlanProviderID").val()) + "', NPI:'" + clean($("#searchNPI").val()) + "', fromDate:'" + clean($("#searchDateFrom").val()) + "', toDate:'" + clean($("#searchDateTo").val()) + "', providerTaxId:'" + clean($("#searchProviderTaxID").val()) + "', officeAddress:'" + clean($("#searchOfficeStreetAddress").val()) + "', officeState:'" + clean($("#searchProviderState").val()) + "', page:" + page + ",sSort:'" + sp_sort + "',sOrder:'" + sp_order + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json", async: true, cache: false,
        success: function (msg) {
            $("#searchResults").html(msg.d)
            $("#searchResults input:radio").click(function () { $("#spnOtherProvider").hide() })
        }
    })
}

function resultsMemberSearch(page) {
    $("#searchResults").html(getProcessing())
    $.ajax({
        type: "POST",
        url: "ChaseManager.aspx/resultsMemberSearch",
        data: "{chaseId:'" + clean($("#searchChaseID").val()) + "', memberId:'" + clean($("#searchMemberID").val()) + "', memberName:'" + clean($("#searchMemberName").val()) + "', dob:'" + clean($("#searchDOB").val()) + "', page:" + page + "}",
        contentType: "application/json; charset=utf-8",
        dataType: "json", async: true, cache: false,
        success: function (msg) {
            $("#searchResults").html(msg.d)
        }
    })
}

function createDialog(html, css) {
    shtml = "<table class='blk_tbl'><tr><td class='frm_td'>" + html + "</td></tr></table>";
    $("#dialog").html(shtml);
    $("#dialog").attr("class", css)
    $("#dialog").show()
    $("#overlay").show()
}