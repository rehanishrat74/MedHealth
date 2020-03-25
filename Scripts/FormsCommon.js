var PK = 0;
var FormName = ""
var ContactNoteList = "";
var isArc = 0
function InitializeLists() {
    if (typeof (GetQueryStringParams("arc")) == 'undefined') {
        isArc = 0
    }
    else {
        isArc = 1
        $("#IsOnlyFollowup").closest("label").hide()
    }

    $("#gridHeader .m_hnd").addClass("not_edit_able")
    $("#patient_info input").change(checkRequiredFields)
    $(".date").datepick({ onSelect: function () { $(this).trigger("change"); } });
    $("#page_button_title").html(FormName)
    $.ajax({
        type: "POST",
        url: "FormCHF.aspx/InitializeLists",
        data: "{FormName:'" + FormName + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        cache: false,
        success: function (msg) {
            showInitializeLists(msg.d)
        },
        error: function () {
            alert("Unable to reach central server")
        }
    })
    $.ajax({
        type: "POST",
        url: "FormCHF.aspx/CommonInitializeLists",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        cache: false,
        success: function (msg) {
            ContactNoteList = msg.d;
        },
        error: function () {
            alert("Unable to reach central server")
        }
    })
    $("#patient_info tr:first input").change(savePatient);
    $("#PCP_ID, #PCP_Lastname, #PCP_Firstname").change(savePCP);
    buildList(1, 'HN', 'ASC')
    $("#IsOnlyFollowup").click(getbuildList)
    $("#searchValue").keyup(function (event) {
        if (this.value.trim().length > 2 && event.keyCode == 13) {
            getbuildList()
        }
    });
}
function getbuildList() { buildList(1, 'HN', 'ASC'); }
function reset() {
    $("#searchValue").val("")
    $("#IsOnlyFollowup").prop("checked") == false
    getbuildList()
}
function NewForm(d) {
    no_action_bar = true
    shTopBar()
    $("#rpt_tbl").hide()
    clearForm()
    $("#entry_form").show()
    $("#btnContactLog").hide()
    PK = d;
    if (d != 0) {
        clLoading()
        $("#btnContactLog").show()
        $.ajax({
            type: "POST",
            url: "FormCHF.aspx/getForm",
            data: "{PK:" + d + ",FormName:'"+FormName+"'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: true,
            cache: false,
            success: function (msg) {
                ar = msg.d[0].split("~")
                $("#Patient_ID").attr("d", ar[0])
                $("#Patient_ID").val(ar[1])
                $("#Lastname").val(ar[2])
                $("#Firstname").val(ar[3])
                $("#DOB").val(ar[4])
                $("#PhoneNumber").val(ar[5])
                $("#AlternatePhoneNumber").val(ar[6])
                $("#Email").val(ar[7])

                $("#PCP_ID").attr("d", ar[8])
                $("#PCP_ID").val(ar[9])
                $("#PCP_Lastname").val(ar[10])
                $("#PCP_Firstname").val(ar[11])

                $("#DischargeDate").val(ar[12])
                $("#Insurance").val(ar[13])

                for (r = 1; r < msg.d.length; r++) {
                    FollowupPK = "0"
                    if (msg.d[r] != null) {
                        ar = msg.d[r].split("~")
                        for (i = 0; i < ar.length; i++) {
                            a = ar[i].split("|")
                            if (a[0] == "Followup_PK") {
                                FollowupPK = a[1]
                            }
                            else if (a[0].substring(0, 2).toLowerCase() == "is") {
                                d = a[0].substring(2)
                                td = $("#row" + d).find(".fu" + FollowupPK)
                                if (a[1].toLowerCase() == "true")
                                    td.attr("b", "1")
                                else if (a[1].toLowerCase() == "false")
                                    td.attr("b", "0")
                                else
                                    td.attr("b", "")
                            }
                            else {
                                if (a[0].indexOf("_PK") > 1)
                                    d = a[0].replace("_PK", "")
                                else
                                    d = a[0]

                                td = $("#row" + d).find(".fu" + FollowupPK)
                                td.attr("v", a[1])
                            }
                        }
                        enableControls($("#H" + FollowupPK))
                    }
                }

                checkRequiredFields()
                clLoading()
            },
            error: function () {
                alert("Unable to reach central server")
            }
        })
    }
    else
        checkRequiredFields();
}
function checkRequiredFields() {
    col_list = "";
    if ($("#Patient_ID").val() == "")
        col_list += ", Patient ID"
    if ($("#Lastname").val() == "")
        col_list += ", Patient Lastname"
    if ($("#Firstname").val() == "")
        col_list += ", Patient Firstname"
    if ($("#PhoneNumber").val() == "")
        col_list += ", Phone Number"
    if ($("#PCP_ID").val() == "")
        col_list += ", PCP ID"
    if ($("#PCP_Lastname").val() == "")
        col_list += ", PCP Lastname"
    if ($("#PCP_Firstname").val() == "")
        col_list += ", PCP Firstname"
    if ($("#DischargeDate").val() == "")
        col_list += ", D/C Date"

    if (col_list != "") {
        col_list = col_list.substring(2)
        li = col_list.lastIndexOf(",")
        if (li < 0)
            col_list = col_list + " is "
        else
            col_list = col_list.substring(0, li) + " and " + col_list.substring(li + 1) + " are "
        $("#td_err").html(col_list + "required entries to enable follow-up entry");
    }
    else {
        last_enabled = ""
        fu_enabled = ""
        var date1 = new Date($("#DischargeDate").val());
        var date2 = new Date();
        var timeDiff = date2.getTime() - date1.getTime();
        var diffDays = 0
        if (timeDiff > 0)
            diffDays = Math.floor(timeDiff / (1000 * 3600 * 24));

        if (diffDays > 0) {
            last_enabled = "#H1"
            $("#H1").removeClass("not_edit_able")
            $("#H1").click(function () { enableControls(this) })
            fu_enabled += "24 Hrs"
        }
        else {
            $("#H1").addClass("not_edit_able")
            $("#H1").unbind("click")
        }
        if (diffDays > 1) {
            last_enabled = "#H2"
            $("#H2").removeClass("not_edit_able")
            $("#H2").click(function () { enableControls(this) })
            fu_enabled += ", 48 Hrs"
        }
        else {
            $("#H2").addClass("not_edit_able")
            $("#H2").unbind("click")
        }
        if (diffDays > 2) {
            last_enabled = "#H3"
            $("#H3").removeClass("not_edit_able")
            $("#H3").click(function () { enableControls(this) })
            fu_enabled += ", 72 Hrs"
        }
        else {
            $("#H3").addClass("not_edit_able")
            $("#H3").unbind("click")
        }
        if (diffDays > 4) {
            last_enabled = "#H4"
            $("#H4").removeClass("not_edit_able")
            $("#H4").click(function () { enableControls(this) })
            fu_enabled += ", 5 Days"
        }
        else {
            $("#H4").addClass("not_edit_able")
            $("#H4").unbind("click")
        }
        if (diffDays > 6) {
            last_enabled = "#H5"
            $("#H5").removeClass("not_edit_able")
            $("#H5").click(function () { enableControls(this) })
            fu_enabled += ", 7 Days"
        }
        else {
            $("#H5").addClass("not_edit_able")
            $("#H5").unbind("click")
        }
        if (diffDays > 13) {
            last_enabled = "#H6"
            $("#H6").removeClass("not_edit_able")
            $("#H6").click(function () { enableControls(this) })
            fu_enabled += ", 14 Days"
        }
        else {
            $("#H6").addClass("not_edit_able")
            $("#H6").unbind("click")
        }
        if (diffDays > 20) {
            last_enabled = "#H7"
            $("#H7").removeClass("not_edit_able")
            $("#H7").click(function () { enableControls(this) })
            fu_enabled += ", 21 Days"
        }
        else {
            $("#H7").addClass("not_edit_able")
            $("#H7").unbind("click")
        }
        if (diffDays > 27) {
            last_enabled = "#H8"
            $("#H8").removeClass("not_edit_able")
            $("#H8").click(function () { enableControls(this) })
            fu_enabled += ", 28 Days"
        }
        else {
            $("#H8").addClass("not_edit_able")
            $("#H8").unbind("click")
        }

        if (diffDays == 0) {
            $("#td_err").html("No follow-up allowed with-in 24 hours of discharge");
            clearControls()
        }
        else {
            li = fu_enabled.lastIndexOf(",")
            if (li < 0)
                fu_enabled = fu_enabled + " is "
            else
                fu_enabled = fu_enabled.substring(0, li) + " and " + fu_enabled.substring(li + 1) + " are "

            $("#td_err").html("Follow-up for " + fu_enabled + "enabled");
            enableControls($(last_enabled))
        }
    }
}
function clearControls() {
    edited = $(".edited")
    if (edited.length == 0)
        return;

    edited.removeClass("s7")
    edited.removeClass("edited")
    edited.addClass("s2")

    d = edited.attr("d")
    control_tds = $("#controls td")
    target_tds = $(".fu" + d)
    for (i = 0; i < control_tds.length; i++) {
        t = $(target_tds[i]).closest("tr").attr("t")
        $(target_tds[i]).addClass("s2")
        $(target_tds[i]).removeClass("s7")
        if (t == "d") {
            $(target_tds[i]).attr("v", $(target_tds[i]).find("select").val())
            $(target_tds[i]).html($(target_tds[i]).find("select option:selected").text())
        }
        else if (t == "t") {
            $(target_tds[i]).attr("v", $(target_tds[i]).find("input").val())
            $(target_tds[i]).html($(target_tds[i]).find("input").val())
        }
        else if (t == "r") {
            $(target_tds[i]).attr("b", $(target_tds[i]).find("input:checked").attr("class"))
            $(target_tds[i]).html($(target_tds[i]).find("input:checked").closest("label").text())
        }
        else if (t == "rd") {
            $(target_tds[i]).attr("b", $(target_tds[i]).find("input:checked").attr("class"))
            $(target_tds[i]).attr("v", $(target_tds[i]).find("select").val())
            YesNo = $(target_tds[i]).find("input:checked").closest("label").text()
            Txt = $(target_tds[i]).find("select option:selected").text()
            if (YesNo != "" && Txt != "")
                Txt = YesNo + " - " + Txt
            else
                Txt = YesNo + Txt
            $(target_tds[i]).html(Txt)
        }
        else if (t == "rt") {
            $(target_tds[i]).attr("b", $(target_tds[i]).find("input:checked").attr("class"))
            $(target_tds[i]).attr("v", $(target_tds[i]).find("input:text").val())
            YesNo = $(target_tds[i]).find("input:checked").closest("label").text()
            Txt = $(target_tds[i]).find("input:text").val()
            if (YesNo != "" && Txt != "")
                Txt = YesNo + " - " + Txt
            else
                Txt = YesNo + Txt
            $(target_tds[i]).html(Txt)
        }
    }
}
function checkIfValuesPresent(obj) {
    if ($(obj).find("option").length == 0)
        addNewValue(obj)
}
function addNewValue(obj) {
    lbl = $(obj).closest("tr").find("th").text()
    obj = $(obj).closest("tr").attr("d")
    html = "<b>New entry for '" + lbl + "':<br>";
    if (obj != "Provider")
        html += "<input type=text class=s8 placeholder='" + lbl + "'>"
    else
        html += "<input type=text class=s33 placeholder='Lastname'>, <input type=text class=s33 placeholder='Firstname'>"
    createDialog(html + "<div class=rt><b class=btn4 onclick=\"SaveValue('" + obj + "')\">Save</b> <b class=btn4 onclick='closeDialog()'>Cancel</b></div>", "s9")
}
function SaveValue(obj) {
    inp = $("#dialog").find("input")
    if (obj == "Provider")
        vlu = inp[0].value + "~" + inp[1].value + "~" + FormName
    else
        vlu = inp[0].value

    $.ajax({
        type: "POST",
        url: "FormCHF.aspx/SaveValue",
        data: "{vlu:'" + vlu + "',obj:'" + obj + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        context: obj,
        cache: false,
        success: function (msg) {
            $("." + this).html(msg.d)
            $("." + this).trigger("chosen:updated");
            clLoading();
        },
        error: function () {
            alert("Unable to reach central server")
        }
    })
    closeDialog();
    clLoading();
}
function savePatient() {
    if ($(this).attr("id") == "Patient_ID") {
        $("#Patient_ID").attr("d", "0")
        $("#Lastname").val("")
        $("#Firstname").val("")
        $("#DOB").val("")
        $("#PhoneNumber").val("")
        $("#AlternatePhoneNumber").val("")
        $("#Email").val("")
        checkRequiredFields()
        clLoading()
    }
    $.ajax({
        type: "POST",
        url: "FormCHF.aspx/savePatient",
        data: "{PK:'" + $("#Patient_ID").attr("d") + "',Patient_ID:'" + $("#Patient_ID").val() + "',Lastname:'" + $("#Lastname").val() + "',Firstname:'" + $("#Firstname").val() + "',DOB:'" + $("#DOB").val() + "',PhoneNumber:'" + $("#PhoneNumber").val() + "',AlternatePhoneNumber:'" + $("#AlternatePhoneNumber").val() + "',Email:'" + $("#Email").val() + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        context: this,
        cache: false,
        success: function (msg) {
            if (msg.d != "") {
                ar = msg.d.split("~")
                $("#Patient_ID").attr("d", ar[0])
                $("#Patient_ID").val(ar[1])
                $("#Lastname").val(ar[2])
                $("#Firstname").val(ar[3])
                $("#DOB").val(ar[4])
                $("#PhoneNumber").val(ar[5])
                $("#AlternatePhoneNumber").val(ar[6])
                $("#Email").val(ar[7])
                checkRequiredFields()
                clLoading()
            }
        },
        error: function () {
            alert("Unable to reach central server")
        }
    })
}
function savePCP() {
    if ($(this).attr("id") == "PCP_ID") {
        $("#PCP_ID").attr("d", "0")
        $("#PCP_Lastname").val("")
        $("#PCP_Firstname").val("")
        checkRequiredFields()
        clLoading()
    }
    $.ajax({
        type: "POST",
        url: "FormCHF.aspx/savePCP",
        data: "{PK:'" + $("#PCP_ID").attr("d") + "',PCP_ID:'" + $("#PCP_ID").val() + "',Lastname:'" + $("#PCP_Lastname").val() + "',Firstname:'" + $("#PCP_Firstname").val() + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        context: this,
        cache: false,
        success: function (msg) {
            if (msg.d != "") {
                ar = msg.d.split("~")
                $("#PCP_ID").attr("d", ar[0])
                $("#PCP_ID").val(ar[1])
                $("#PCP_Lastname").val(ar[2])
                $("#PCP_Firstname").val(ar[3])
                checkRequiredFields()
                clLoading()
            }
        },
        error: function () {
            alert("Unable to reach central server")
        }
    })
}
function enableControls(edited) {
    edited = $(edited)
    if (edited.hasClass("edited"))
        return;

    clearControls()
    edited.removeClass("s2")
    edited.addClass("edited")
    edited.addClass("s7")
    d = edited.attr("d")
    control_tds = $("#controls td")
    target_tds = $(".fu" + d)
    for (i = 0; i < control_tds.length; i++) {
        $(target_tds[i]).removeClass("s2")
        $(target_tds[i]).addClass("s7")
        $(target_tds[i]).html($(control_tds[i]).html())

        t = $(target_tds[i]).closest("tr").attr("t")
        if (t == "d") {
            $(target_tds[i]).find("select").val($(target_tds[i]).attr("v"))
        }
        else if (t == "t") {
            $(target_tds[i]).find("input").val($(target_tds[i]).attr("v"))
        }
        else if (t == "r") {
            if ($(target_tds[i]).attr("b") == "1" || $(target_tds[i]).attr("b") == "0")
                $(target_tds[i]).find("." + $(target_tds[i]).attr("b"))[0].checked = true;
        }
        else if (t == "rd") {
            if ($(target_tds[i]).attr("b") == "1" || $(target_tds[i]).attr("b") == "0")
                $(target_tds[i]).find("." + $(target_tds[i]).attr("b"))[0].checked = true;
            $(target_tds[i]).find("select").val($(target_tds[i]).attr("v"))
        }
        else if (t == "rt") {
            if ($(target_tds[i]).attr("b") == "1" || $(target_tds[i]).attr("b") == "0")
                $(target_tds[i]).find("." + $(target_tds[i]).attr("b"))[0].checked = true;
            $(target_tds[i]).find("input:text").val($(target_tds[i]).attr("v"))
        }
    }

    //$(".fu" + d + " select").html("<option value=1>Green</option><option value=1>Red</option><option value=1>Blue</option>")
    $(".fu" + d + " select").chosen({
        no_results_text: "<a onclick='addNewValue(this)'>Click here to add new</a><br>No results found for"
    })
    $(".fu" + d + " select").on('chosen:showing_dropdown', function () { checkIfValuesPresent(this); })
}
function getAttrVal(a, t) {
    if (a == null || a == "")
        return "null"
    else if (t == "t")
        return "\\'" + a + "\\'"
    return a
}
function buildList(page, sort, order) {
    $("#rpt").html(getProcessing());

    if (sort == "") sort = "UN"
    if (order == "") order = "ASC"
    Search = $("#searchValue").val()
    OnlyFollowup = ($("#IsOnlyFollowup").prop("checked") == true ? 1 : 0)
    $.ajax({
        type: "POST",
        url: "FormCHF.aspx/buildList",
        data: "{page:" + page + ",sSort:'" + sort + "',sOrder:'" + order + "',FormName:'" + FormName + "',isArc:" + isArc + ",OnlyFollowup:" + OnlyFollowup + ",Search:'" + Search + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        cache: false,
        success: function (msg) {
            $("#rpt").html(msg.d);
            $("#rpt td").addClass("m_hnd")
            $("#rpt td").click(function () { NewForm($(this).closest("tr").attr("d")); })
        },
        error: function () {
            $("#rpt").html("<table class='rpt_tbl'><tr><th>ERROR</th></tr><tr><td>Unable to reach central server</td></tr></table>")
        }
    })
}
function clearForm() {
    $("#Insurance").val(0)
    $("#patient_info input").val("")
    $("#gridDetail td").html("")
    $("#gridDetail td").attr("v", "")
    $("#gridDetail td").attr("b", "")
    $("#gridDetail td").removeClass("s7")
    $("#gridDetail td").addClass("s2")
    $("#gridHeader .m_hnd").attr("class", "s2 m_hnd")
}
function topSave() {
    clearControls()

    Patient_PK = $("#Patient_ID").attr("d")
    PCP_PK = $("#PCP_ID").attr("d")
    DischargeDate = $("#DischargeDate").val()
    Insurance = $("#Insurance").val()
    FUs = $("#gridHeader .m_hnd")
    FU_Data = "";
    for (i = 0; i < FUs.length; i++) {
        d = $(FUs[i]).attr("d")
        if (FU_Data != "")
            FU_Data += "~"
        FU_Data += d + "^"
        if ($(FUs[i]).hasClass("not_edit_able"))
            FU_Data += "0"
        else {
            tds = $(".fu" + d)
            if ($(tds[0]).attr("v") == null || $(tds[0]).attr("v") == "")
                FU_Data += "0"
            else {
                FU_Data += "1"
                for (t = 0; t < tds.length; t++) {
                    td = $(tds[t])
                    tr = td.closest("tr")
                    if (tr.attr("t") == "d") {
                        FU_Data += "^" + tr.attr("d") + "_PK|" + getAttrVal(td.attr("v"), tr.attr("t"))
                    }
                    else if (tr.attr("t") == "r") { //|| tr.attr("t") == "rd" || tr.attr("t") == "rt"
                        FU_Data += "^Is" + tr.attr("d") + "|" + getAttrVal(td.attr("b"), tr.attr("t"))
                    }
                    else if (tr.attr("t") == "t") {
                        FU_Data += "^" + tr.attr("d") + "|" + getAttrVal(td.attr("v"), tr.attr("t"))
                    }
                    else if (tr.attr("t") == "rd") {
                        FU_Data += "^Is" + tr.attr("d") + "|" + getAttrVal(td.attr("b"), tr.attr("t"))
                        FU_Data += "^" + tr.attr("d") + "_PK|" + getAttrVal(td.attr("v"), tr.attr("t"))
                    }
                    else if (tr.attr("t") == "rt") {
                        FU_Data += "^Is" + tr.attr("d") + "|" + getAttrVal(td.attr("b"), tr.attr("t"))
                        FU_Data += "^" + tr.attr("d") + "|" + getAttrVal(td.attr("v"), tr.attr("t"))
                    }
                }
            }
        }
    }
    clLoading()
    $.ajax({
        type: "POST",
        url: "FormCHF.aspx/saveForm",
        data: "{PK:" + PK + ",Patient_PK:" + $("#Patient_ID").attr("d") + ",PCP_PK:" + $("#PCP_ID").attr("d") + ",DischargeDate:'" + $("#DischargeDate").val() + "',Insurance:'" + $("#Insurance").val() + "',FU_Data:'" + FU_Data + "',FormName:'" + FormName + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        context: this,
        cache: false,
        success: function (msg) {
            topCancel()
            clLoading()
            buildList(1, 'HN', 'ASC')
        },
        error: function () {
            alert("Unable to reach central server")
        }
    })
}
function contactLog() {
    clLoading()
    $.ajax({
        type: "POST",
        url: "FormCHF.aspx/getContactLog",
        data: "{PK:" + PK + "}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        cache: false,
        success: function (msg) {
            clLoading()
            html = "<table class=s100><tr><td class='s5' id='ContactNoteOptions'>" + ContactNoteList + "<b>Additional Details</b><br><input maxlength=1000 id='additionalDetail' class=s5><div class=rt><b class=btn4 onclick='addContactNote()'>Add Contact Note</b> <b class=btn4 onclick='closeDialog()'>Close</b></div></td><td style='vertical-align:top'><h4>Contact Log</h4>" + msg.d + "</td></tr></table>";
            createDialog(html, "s12")            
        },
        error: function () {
            alert("Unable to reach central server")
        }
    })
}
function addContactNote() {
    if ($("#ContactNoteOptions input:checked").length == 0)
        return;


    $.ajax({
        type: "POST",
        url: "FormCHF.aspx/saveContactLog",
        data: "{PK:" + PK + ",NotePK:" + $("#ContactNoteOptions input:checked").val() + ",additionalDetail:'" + $("#additionalDetail").val() + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        cache: false,
        success: function (msg) {
            closeDialog()
        },
        error: function () {
            alert("Unable to reach central server")
        }
    })
}