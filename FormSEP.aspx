<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="FormSEP.aspx.cs" Inherits="MedHealthSolutions.FormSEP" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
Form - Sepsis
</asp:Content>
<asp:Content ID="TitleContent" runat="server" ContentPlaceHolderID="TitleContent">
Sepsis
</asp:Content>
<asp:Content ID="DownloadIcons" runat="server" ContentPlaceHolderID="DownloadIcons">
    <%--<img id='imXL' src="imgs/xl.png" height="25" title="Excel" onclick='download(1)' />--%>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="BodyContent" >
    <style >
        .btn3, .rpt_tbl th, .frm_tbl th, .default_color, #cssmenu {
            background-color: #5a99d3 !important;            
        }
        #cssmenu ul li a {
            border-bottom-color: #5a99d3 !important;
        }
    </style>
    <script type="text/javascript" src="Scripts/FormsCommon.js?x<%=MedHealthSolutions.Classes.portalVersion.info() %>"></script>
    <script type="text/javascript" language="javascript">
        var FormName = "Sepsis"
        $(document).ready(function () {
            InitializeLists()
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

        })

        function showInitializeLists(ar) {
            $("#Insurance").html(ar[0])
            $(".Provider").html(ar[1])
            $(".ProductiveCough").html(ar[2])
            $(".ProductiveCoughAmount").html(ar[3])
            $(".CalledBy").html(ar[4])
            $(".Antibiotic").html(ar[5])
            $(".SourceOfInfection").html(ar[6])
            $(".Sputum").html(ar[7])
            $(".AppetiteChange").html(ar[8])
            $(".WeightChange").html(ar[9])
            $(".UseOxygen").html(ar[10])
            $(".SOB").html(ar[11])
        }
    </script>
<table id="rpt_tbl" class="main_tbl">
    <tr>
        <td class="frm_td s31 pad10">
            <b class="btn3 margin3" id='bChange' onclick="NewForm(0)">New Sepsis Form</b>  
            <br />
            <br /> 
            <label class="check-label"><input type="checkbox" id="IsOnlyFollowup" > Only forms with required follow-up<span class="checkmark"></span></label>
            <input id="searchValue" class="s5 search" style="margin-top:5px;" placeholder="Type Patient ID or Name" title="Type Patient ID or Name and hit enter">
            <div class=rt><a onclick="javascript:reset();" id='pReset'>Reset Search</a></div>
        </td>
        <td id="rpt" class="cnt blk_td"></td>
    </tr>
</table>

<div id="entry_form" class="frm_tbl hide">
    <div style='position:fixed; background-color:#fff;z-index:999;'>
    <table id='patient_info'><tr>
        <td>Patient ID <b class="red">*</b><br /><input type='text' class=s3 d="0" id='Patient_ID' /></td>
        <td>Patient Lastname <b class="red">*</b><br /><input type='text' class=s4 id='Lastname' /></td>
        <td>Patient Firstname <b class="red">*</b><br /><input type='text' class=s4 id='Firstname' /></td>
        <td>DOB<br /><input type='text' class='s3 date' id='DOB' /></td>
        <td>Phone Number <b class="red">*</b><br /><input type='text' class=s32 id='PhoneNumber' /></td>
        <td>Alternate Phone Number<br /><input type='text' class=s32 id='AlternatePhoneNumber' /></td>
        <td>Email<br /><input type='text' class=s33 id='Email' /></td>
    </tr>
    <tr>
        <td>PCP ID <b class="red">*</b><br /><input type='text' class=s3 id='PCP_ID' /></td>
        <td>PCP Lastname <b class="red">*</b><br /><input type='text' class=s4 id='PCP_Lastname' /></td>
        <td>PCP Firstname <b class="red">*</b><br /><input type='text' class=s4 id='PCP_Firstname' /></td>
        <td>D/C Date <b class="red">*</b><br /><input type='text' class='s3 date' id='DischargeDate' /></td>
        <td>Insurance <br /><select id='Insurance' style='width:167px'></select></td>
        <td id=td_err colspan="2" class="cnt red">* required values to enable patient follow-up entry</td>
    </tr>
    </table>

    <table id="gridHeader">
        <tr><th class="s6" style="color:#007dbb;background-color:#fff;">Confirmed F/U with PCP and MD</th><th class="s2 m_hnd" d="1" id="H1">24 Hrs</th><th class="s2 m_hnd" d="2" id="H2">48 Hrs</th><th class="s2 m_hnd" d="3" id="H3">72 Hrs</th><th class="s2 m_hnd" d="4" id="H4">5 Days</th><th class="s2 m_hnd" d="5" id="H5">7 Days</th><th class="s2 m_hnd" d="6" id="H6">14 Days</th><th class="s2 m_hnd" d="7" id="H7">21 Days</th><th class="s2 m_hnd" d="8" id="H8">28 Days</th></tr>
    </table>
    </div><div style="padding-top:161px;width:1339px;">
    <table id="gridDetail" class="frm_tbl2" style="background-color:#efefef">
        <tr id="rowProvider" d="Provider" t="d"><th class="s6">MD Name</th><td class="fu1 s2">&nbsp;</td><td class="fu2 s2">&nbsp;</td><td class="fu3 s2">&nbsp;</td><td class="fu4 s2">&nbsp;</td><td class="fu5 s2">&nbsp;</td><td class="fu6 s2">&nbsp;</td><td class="fu7 s2">&nbsp;</td><td class="fu8 s2">&nbsp;</td></tr>

<tr id="rowSourceOfInfection" d="SourceOfInfection" t="d"><th>Source of Infection</th><td class="fu1 s2">&nbsp;</td><td class="fu2 s2">&nbsp;</td><td class="fu3 s2">&nbsp;</td><td class="fu4 s2">&nbsp;</td><td class="fu5 s2">&nbsp;</td><td class="fu6 s2">&nbsp;</td><td class="fu7 s2">&nbsp;</td><td class="fu8 s2">&nbsp;</td></tr>
        <tr id="rowAntibiotic" d="Antibiotic" t="d"><th>Antibiotic</th><td class="fu1 s2">&nbsp;</td><td class="fu2 s2">&nbsp;</td><td class="fu3 s2">&nbsp;</td><td class="fu4 s2">&nbsp;</td><td class="fu5 s2">&nbsp;</td><td class="fu6 s2">&nbsp;</td><td class="fu7 s2">&nbsp;</td><td class="fu8 s2">&nbsp;</td></tr>
        <tr id="rowAntibioticDate" d="AntibioticDate" t="t"><th>Date of Antibiotic Completion</th><td class="fu1 s2">&nbsp;</td><td class="fu2 s2">&nbsp;</td><td class="fu3 s2">&nbsp;</td><td class="fu4 s2">&nbsp;</td><td class="fu5 s2">&nbsp;</td><td class="fu6 s2">&nbsp;</td><td class="fu7 s2">&nbsp;</td><td class="fu8 s2">&nbsp;</td></tr>
<tr id="rowTemperatureOver1005" d="TemperatureOver1005" t="r"><th>Temperature over 100.5 degrees Fahrenheit</th><td class="fu1 s2">&nbsp;</td><td class="fu2 s2">&nbsp;</td><td class="fu3 s2">&nbsp;</td><td class="fu4 s2">&nbsp;</td><td class="fu5 s2">&nbsp;</td><td class="fu6 s2">&nbsp;</td><td class="fu7 s2">&nbsp;</td><td class="fu8 s2">&nbsp;</td></tr>

        <tr id="rowSOB" d="SOB" t="rd"><th>SOB</th><td class="fu1 s2">&nbsp;</td><td class="fu2 s2">&nbsp;</td><td class="fu3 s2">&nbsp;</td><td class="fu4 s2">&nbsp;</td><td class="fu5 s2">&nbsp;</td><td class="fu6 s2">&nbsp;</td><td class="fu7 s2">&nbsp;</td><td class="fu8 s2">&nbsp;</td></tr>
        <tr id="rowProductiveCough" d="ProductiveCough" t="rd"><th>Productive Cough</th><td class="fu1 s2">&nbsp;</td><td class="fu2 s2">&nbsp;</td><td class="fu3 s2">&nbsp;</td><td class="fu4 s2">&nbsp;</td><td class="fu5 s2">&nbsp;</td><td class="fu6 s2">&nbsp;</td><td class="fu7 s2">&nbsp;</td><td class="fu8 s2">&nbsp;</td></tr>
        <tr id="rowProductiveCoughAmount" d="ProductiveCoughAmount" t="d"><th>Productive Cough Amount</th><td class="fu1 s2">&nbsp;</td><td class="fu2 s2">&nbsp;</td><td class="fu3 s2">&nbsp;</td><td class="fu4 s2">&nbsp;</td><td class="fu5 s2">&nbsp;</td><td class="fu6 s2">&nbsp;</td><td class="fu7 s2">&nbsp;</td><td class="fu8 s2">&nbsp;</td></tr>

<tr id="rowSputum" d="Sputum" t="rd"><th>Sputum</th><td class="fu1 s2">&nbsp;</td><td class="fu2 s2">&nbsp;</td><td class="fu3 s2">&nbsp;</td><td class="fu4 s2">&nbsp;</td><td class="fu5 s2">&nbsp;</td><td class="fu6 s2">&nbsp;</td><td class="fu7 s2">&nbsp;</td><td class="fu8 s2">&nbsp;</td></tr>
<tr id="rowFatiqueDizziness" d="FatiqueDizziness" t="r"><th>Fatique/Dizziness</th><td class="fu1 s2">&nbsp;</td><td class="fu2 s2">&nbsp;</td><td class="fu3 s2">&nbsp;</td><td class="fu4 s2">&nbsp;</td><td class="fu5 s2">&nbsp;</td><td class="fu6 s2">&nbsp;</td><td class="fu7 s2">&nbsp;</td><td class="fu8 s2">&nbsp;</td></tr>
<tr id="rowAppetiteChange" d="AppetiteChange" t="rd"><th>Appetite Change</th><td class="fu1 s2">&nbsp;</td><td class="fu2 s2">&nbsp;</td><td class="fu3 s2">&nbsp;</td><td class="fu4 s2">&nbsp;</td><td class="fu5 s2">&nbsp;</td><td class="fu6 s2">&nbsp;</td><td class="fu7 s2">&nbsp;</td><td class="fu8 s2">&nbsp;</td></tr>
<tr id="rowWeightChange" d="WeightChange" t="rd"><th>Weight Change</th><td class="fu1 s2">&nbsp;</td><td class="fu2 s2">&nbsp;</td><td class="fu3 s2">&nbsp;</td><td class="fu4 s2">&nbsp;</td><td class="fu5 s2">&nbsp;</td><td class="fu6 s2">&nbsp;</td><td class="fu7 s2">&nbsp;</td><td class="fu8 s2">&nbsp;</td></tr>
<tr id="rowDepression" d="Depression" t="r"><th>Depression</th><td class="fu1 s2">&nbsp;</td><td class="fu2 s2">&nbsp;</td><td class="fu3 s2">&nbsp;</td><td class="fu4 s2">&nbsp;</td><td class="fu5 s2">&nbsp;</td><td class="fu6 s2">&nbsp;</td><td class="fu7 s2">&nbsp;</td><td class="fu8 s2">&nbsp;</td></tr>
<tr id="rowConfusion" d="Confusion" t="r"><th>Confusion</th><td class="fu1 s2">&nbsp;</td><td class="fu2 s2">&nbsp;</td><td class="fu3 s2">&nbsp;</td><td class="fu4 s2">&nbsp;</td><td class="fu5 s2">&nbsp;</td><td class="fu6 s2">&nbsp;</td><td class="fu7 s2">&nbsp;</td><td class="fu8 s2">&nbsp;</td></tr>
<tr id="rowUseOxygen" d="UseOxygen" t="rd"><th>Use Oxygen</th><td class="fu1 s2">&nbsp;</td><td class="fu2 s2">&nbsp;</td><td class="fu3 s2">&nbsp;</td><td class="fu4 s2">&nbsp;</td><td class="fu5 s2">&nbsp;</td><td class="fu6 s2">&nbsp;</td><td class="fu7 s2">&nbsp;</td><td class="fu8 s2">&nbsp;</td></tr>
<tr id="rowCPAP" d="CPAP" t="r"><th>CPAP</th><td class="fu1 s2">&nbsp;</td><td class="fu2 s2">&nbsp;</td><td class="fu3 s2">&nbsp;</td><td class="fu4 s2">&nbsp;</td><td class="fu5 s2">&nbsp;</td><td class="fu6 s2">&nbsp;</td><td class="fu7 s2">&nbsp;</td><td class="fu8 s2">&nbsp;</td></tr>

        <tr id="rowQuestionsConcerns" d="QuestionsConcerns" t="r"><th>Questions/Concerns</th><td class="fu1 s2">&nbsp;</td><td class="fu2 s2">&nbsp;</td><td class="fu3 s2">&nbsp;</td><td class="fu4 s2">&nbsp;</td><td class="fu5 s2">&nbsp;</td><td class="fu6 s2">&nbsp;</td><td class="fu7 s2">&nbsp;</td><td class="fu8 s2">&nbsp;</td></tr>
        <tr id="rowNotes" d="Notes" t="t"><th>Notes</th><td class="fu1 s2">&nbsp;</td><td class="fu2 s2">&nbsp;</td><td class="fu3 s2">&nbsp;</td><td class="fu4 s2">&nbsp;</td><td class="fu5 s2">&nbsp;</td><td class="fu6 s2">&nbsp;</td><td class="fu7 s2">&nbsp;</td><td class="fu8 s2">&nbsp;</td></tr>
        <tr id="rowCalledBy" d="CalledBy" t="d"><th>Called By</th><td class="fu1 s2">&nbsp;</td><td class="fu2 s2">&nbsp;</td><td class="fu3 s2">&nbsp;</td><td class="fu4 s2">&nbsp;</td><td class="fu5 s2">&nbsp;</td><td class="fu6 s2">&nbsp;</td><td class="fu7 s2">&nbsp;</td><td class="fu8 s2">&nbsp;</td></tr>
    </table>
    </div>
</div>

<table id="controls" class="hide">
	<tr><td>
        <select class="s7 choosen-control Provider" data-placeholder="MD Name"></select>
	</td></tr>
    <tr><td>
        <select class="s7 SourceOfInfection" data-placeholder="SourceOfInfection"></select>
	</td></tr>
    <tr><td>
        <select class="s7 Antibiotic" data-placeholder="Antibiotic"></select>
	</td></tr>
    <tr><td>
        <input value="" class="s61 date" maxlength=1000 placeholder="Date of Antibiotic Completion" />
    </td></tr>
    <tr><td>
	    <label class='check-label check-label-inline'><input type=radio name=R1 class='1'> Yes<span class='radio'></span></label>
        <label class='check-label check-label-inline'><input type=radio name=R1 class='0'> No<span class='radio'></span></label>
    </td></tr>
    <tr><td>
	    <label class='check-label check-label-inline'><input type=radio name=R2 class='1'> Yes<span class='radio'></span></label>
        <label class='check-label check-label-inline'><input type=radio name=R2 class='0'> No<span class='radio'></span></label>
        &nbsp;<select class="s4 SOB" data-placeholder="SOB"></select>
    </td></tr>
    <tr><td>
	    <label class='check-label check-label-inline'><input type=radio name=R3 class='1'> Yes<span class='radio'></span></label>
        <label class='check-label check-label-inline'><input type=radio name=R3 class='0'> No<span class='radio'></span></label>
        &nbsp;<select class="s4 ProductiveCough" data-placeholder="Productive Cough"></select>
    </td></tr>
    <tr><td>
        <select class="s7 ProductiveCoughAmount" data-placeholder="Productive Cough Amount"></select>
	</td></tr>
    <tr><td>
	    <label class='check-label check-label-inline'><input type=radio name=R4 class='1'> Yes<span class='radio'></span></label>
        <label class='check-label check-label-inline'><input type=radio name=R4 class='0'> No<span class='radio'></span></label>
        &nbsp;<select class="s4 Sputum" data-placeholder="Sputum"></select>
    </td></tr>
    <tr><td>
	    <label class='check-label check-label-inline'><input type=radio name=R5 class='1'> Yes<span class='radio'></span></label>
        <label class='check-label check-label-inline'><input type=radio name=R5 class='0'> No<span class='radio'></span></label>
    </td></tr>
    <tr><td>
	    <label class='check-label check-label-inline'><input type=radio name=R6 class='1'> Yes<span class='radio'></span></label>
        <label class='check-label check-label-inline'><input type=radio name=R6 class='0'> No<span class='radio'></span></label>
        &nbsp;<select class="s4 AppetiteChange" data-placeholder="Appetite Change"></select>
    </td></tr>
    <tr><td>
	    <label class='check-label check-label-inline'><input type=radio name=R7 class='1'> Yes<span class='radio'></span></label>
        <label class='check-label check-label-inline'><input type=radio name=R7 class='0'> No<span class='radio'></span></label>
        &nbsp;<select class="s4 WeightChange" data-placeholder="Weight Change"></select>
    </td></tr>
    <tr><td>
	    <label class='check-label check-label-inline'><input type=radio name=R8 class='1'> Yes<span class='radio'></span></label>
        <label class='check-label check-label-inline'><input type=radio name=R8 class='0'> No<span class='radio'></span></label>
    </td></tr>
    <tr><td>
	    <label class='check-label check-label-inline'><input type=radio name=R9 class='1'> Yes<span class='radio'></span></label>
        <label class='check-label check-label-inline'><input type=radio name=R9 class='0'> No<span class='radio'></span></label>
    </td></tr>
    <tr><td>
	    <label class='check-label check-label-inline'><input type=radio name=R10 class='1'> Yes<span class='radio'></span></label>
        <label class='check-label check-label-inline'><input type=radio name=R10 class='0'> No<span class='radio'></span></label>
        &nbsp;<select class="s4 UseOxygen" data-placeholder="Use Oxygen"></select>
    </td></tr>
    <tr><td>
	    <label class='check-label check-label-inline'><input type=radio name=R12 class='1'> Yes<span class='radio'></span></label>
        <label class='check-label check-label-inline'><input type=radio name=R12 class='0'> No<span class='radio'></span></label>
    </td></tr>
    <tr><td>
	    <label class='check-label check-label-inline'><input type=radio name=R11 class='1'> Yes<span class='radio'></span></label>
        <label class='check-label check-label-inline'><input type=radio name=R11 class='0'> No<span class='radio'></span></label>
    </td></tr>
    <tr><td>
        <input value="" class="s61" maxlength=1000 placeholder="Notes" />
    </td></tr>
    <tr><td>
        <select class="s7 CalledBy" data-placeholder="Called By"></select>
	</td></tr>
</table>
</form><form id='fDwn' method='post' action='UserManager.aspx'><input type='hidden' name='param' id='param' />
</asp:Content>

