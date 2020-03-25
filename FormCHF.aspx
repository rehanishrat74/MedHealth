<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="FormCHF.aspx.cs" Inherits="MedHealthSolutions.FormCHF" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
Form - CHF
</asp:Content>
<asp:Content ID="TitleContent" runat="server" ContentPlaceHolderID="TitleContent">
CHF
</asp:Content>
<asp:Content ID="DownloadIcons" runat="server" ContentPlaceHolderID="DownloadIcons">
    <%--<img id='imXL' src="imgs/xl.png" height="25" title="Excel" onclick='download(1)' />--%>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="BodyContent" >
    <script type="text/javascript" src="Scripts/FormsCommon.js?x<%=MedHealthSolutions.Classes.portalVersion.info() %>"></script>
    <style >
        .btn3, .rpt_tbl th, .frm_tbl th, .default_color, #cssmenu {
            background-color: #a3a3a3 !important;            
        }
        #cssmenu ul li a {
            border-bottom-color: #a3a3a3 !important;
        }
    </style>
    <script type="text/javascript" language="javascript">
        var FormName = "CHF"
        $(document).ready(function () {
            InitializeLists()
        })

        function showInitializeLists(ar) {
            $("#Insurance").html(ar[0])
            $(".Provider").html(ar[1])
            $(".HowYouFeel").html(ar[2])
            $(".SOB").html(ar[3])
            $(".ProductiveCough").html(ar[4])
            $(".ProductiveCoughAmount").html(ar[5])
            $(".DietaryEducation").html(ar[6])
            $(".SleepNormal").html(ar[7])
            $(".Exercise").html(ar[8])
            $(".CalledBy").html(ar[9])
            $(".ExerciseFrequency").html(ar[10])
        }
    </script>
<table id="rpt_tbl" class="main_tbl">
    <tr>
        <td class="frm_td s5 pad10">
            <b class="btn3 margin3" id='bChange' onclick="NewForm(0)">New CHF Form</b>
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
    <div style="position:fixed; background-color:#fff;z-index:999;">
    <table id="patient_info"><tr>
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
    <%--&#10003;--%>
    <table id="gridHeader">
        <tr><th class="s6" style="color:#007dbb;background-color:#fff;">Confirmed F/U with PCP and Cardiologist</th><th class="s2 m_hnd" d="1" id="H1">24 Hrs</th><th class="s2 m_hnd" d="2" id="H2">48 Hrs</th><th class="s2 m_hnd" d="3" id="H3">72 Hrs</th><th class="s2 m_hnd" d="4" id="H4">5 Days</th><th class="s2 m_hnd" d="5" id="H5">7 Days</th><th class="s2 m_hnd" d="6" id="H6">14 Days</th><th class="s2 m_hnd" d="7" id="H7">21 Days</th><th class="s2 m_hnd" d="8" id="H8">28 Days</th></tr>
    </table>
    </div><div style="padding-top:161px;width:1339px;">
    <table id="gridDetail" class="frm_tbl2" style="background-color:#efefef">
        <tr id="rowProvider" d="Provider" t="d"><th class="s6">Cardiologist Name</th><td class="fu1 s2">&nbsp;</td><td class="fu2 s2">&nbsp;</td><td class="fu3 s2">&nbsp;</td><td class="fu4 s2">&nbsp;</td><td class="fu5 s2">&nbsp;</td><td class="fu6 s2">&nbsp;</td><td class="fu7 s2">&nbsp;</td><td class="fu8 s2">&nbsp;</td></tr>
        <tr id="rowWeightGain" d="WeightGain" t="r"><th>Weight Gain</th><td class="fu1 s2">&nbsp;</td><td class="fu2 s2">&nbsp;</td><td class="fu3 s2">&nbsp;</td><td class="fu4 s2">&nbsp;</td><td class="fu5 s2">&nbsp;</td><td class="fu6 s2">&nbsp;</td><td class="fu7 s2">&nbsp;</td><td class="fu8 s2">&nbsp;</td></tr>
        <tr id="rowActualWeight" d="ActualWeight" t="t"><th>Actual Weight</th><td class="fu1 s2">&nbsp;</td><td class="fu2 s2">&nbsp;</td><td class="fu3 s2">&nbsp;</td><td class="fu4 s2">&nbsp;</td><td class="fu5 s2">&nbsp;</td><td class="fu6 s2">&nbsp;</td><td class="fu7 s2">&nbsp;</td><td class="fu8 s2">&nbsp;</td></tr>
        <tr id="rowMedComprehension" d="MedComprehension" t="t"><th>Med. Comprehension</th><td class="fu1 s2">&nbsp;</td><td class="fu2 s2">&nbsp;</td><td class="fu3 s2">&nbsp;</td><td class="fu4 s2">&nbsp;</td><td class="fu5 s2">&nbsp;</td><td class="fu6 s2">&nbsp;</td><td class="fu7 s2">&nbsp;</td><td class="fu8 s2">&nbsp;</td></tr>
        <tr id="rowMedicationsReconciled" d="MedicationsReconciled" t="r"><th>Medications reconciled with DC list</th><td class="fu1 s2">&nbsp;</td><td class="fu2 s2">&nbsp;</td><td class="fu3 s2">&nbsp;</td><td class="fu4 s2">&nbsp;</td><td class="fu5 s2">&nbsp;</td><td class="fu6 s2">&nbsp;</td><td class="fu7 s2">&nbsp;</td><td class="fu8 s2">&nbsp;</td></tr>
        <tr id="rowMedicationsRefill" d="MedicationsRefill" t="rt"><th>Medication refills</th><td class="fu1 s2">&nbsp;</td><td class="fu2 s2">&nbsp;</td><td class="fu3 s2">&nbsp;</td><td class="fu4 s2">&nbsp;</td><td class="fu5 s2">&nbsp;</td><td class="fu6 s2">&nbsp;</td><td class="fu7 s2">&nbsp;</td><td class="fu8 s2">&nbsp;</td></tr>
        <tr id="rowAllMedicationsRefill" d="AllMedicationsRefill" t="rt"><th>All Meds filled</th><td class="fu1 s2">&nbsp;</td><td class="fu2 s2">&nbsp;</td><td class="fu3 s2">&nbsp;</td><td class="fu4 s2">&nbsp;</td><td class="fu5 s2">&nbsp;</td><td class="fu6 s2">&nbsp;</td><td class="fu7 s2">&nbsp;</td><td class="fu8 s2">&nbsp;</td></tr>
        <tr id="rowHowYouFeel" d="HowYouFeel" t="d"><th>Rate how you feel today</th><td class="fu1 s2">&nbsp;</td><td class="fu2 s2">&nbsp;</td><td class="fu3 s2">&nbsp;</td><td class="fu4 s2">&nbsp;</td><td class="fu5 s2">&nbsp;</td><td class="fu6 s2">&nbsp;</td><td class="fu7 s2">&nbsp;</td><td class="fu8 s2">&nbsp;</td></tr>
        <tr id="rowChestTightness" d="ChestTightness" t="r"><th>Chest Tightness</th><td class="fu1 s2">&nbsp;</td><td class="fu2 s2">&nbsp;</td><td class="fu3 s2">&nbsp;</td><td class="fu4 s2">&nbsp;</td><td class="fu5 s2">&nbsp;</td><td class="fu6 s2">&nbsp;</td><td class="fu7 s2">&nbsp;</td><td class="fu8 s2">&nbsp;</td></tr>
        <tr id="rowSOB" d="SOB" t="rd"><th>SOB</th><td class="fu1 s2">&nbsp;</td><td class="fu2 s2">&nbsp;</td><td class="fu3 s2">&nbsp;</td><td class="fu4 s2">&nbsp;</td><td class="fu5 s2">&nbsp;</td><td class="fu6 s2">&nbsp;</td><td class="fu7 s2">&nbsp;</td><td class="fu8 s2">&nbsp;</td></tr>
        <tr id="rowProductiveCough" d="ProductiveCough" t="rd"><th>Productive Cough</th><td class="fu1 s2">&nbsp;</td><td class="fu2 s2">&nbsp;</td><td class="fu3 s2">&nbsp;</td><td class="fu4 s2">&nbsp;</td><td class="fu5 s2">&nbsp;</td><td class="fu6 s2">&nbsp;</td><td class="fu7 s2">&nbsp;</td><td class="fu8 s2">&nbsp;</td></tr>
        <tr id="rowProductiveCoughAmount" d="ProductiveCoughAmount" t="d"><th>Productive Cough Amount</th><td class="fu1 s2">&nbsp;</td><td class="fu2 s2">&nbsp;</td><td class="fu3 s2">&nbsp;</td><td class="fu4 s2">&nbsp;</td><td class="fu5 s2">&nbsp;</td><td class="fu6 s2">&nbsp;</td><td class="fu7 s2">&nbsp;</td><td class="fu8 s2">&nbsp;</td></tr>
        <tr id="rowFeet_Ankle_Abd_Swelling" d="Feet_Ankle_Abd_Swelling" t="r"><th>Feet/Ankle/Abd Swelling</th><td class="fu1 s2">&nbsp;</td><td class="fu2 s2">&nbsp;</td><td class="fu3 s2">&nbsp;</td><td class="fu4 s2">&nbsp;</td><td class="fu5 s2">&nbsp;</td><td class="fu6 s2">&nbsp;</td><td class="fu7 s2">&nbsp;</td><td class="fu8 s2">&nbsp;</td></tr>
        <tr id="rowFatiqueDizziness" d="FatiqueDizziness" t="r"><th>Fatique/dizziness</th><td class="fu1 s2">&nbsp;</td><td class="fu2 s2">&nbsp;</td><td class="fu3 s2">&nbsp;</td><td class="fu4 s2">&nbsp;</td><td class="fu5 s2">&nbsp;</td><td class="fu6 s2">&nbsp;</td><td class="fu7 s2">&nbsp;</td><td class="fu8 s2">&nbsp;</td></tr>
        <tr id="rowDietaryEducation" d="DietaryEducation" t="rd"><th>Dietary Education</th><td class="fu1 s2">&nbsp;</td><td class="fu2 s2">&nbsp;</td><td class="fu3 s2">&nbsp;</td><td class="fu4 s2">&nbsp;</td><td class="fu5 s2">&nbsp;</td><td class="fu6 s2">&nbsp;</td><td class="fu7 s2">&nbsp;</td><td class="fu8 s2">&nbsp;</td></tr>
        <tr id="rowSleepNormal" d="SleepNormal" t="rd"><th>Sleep normal</th><td class="fu1 s2">&nbsp;</td><td class="fu2 s2">&nbsp;</td><td class="fu3 s2">&nbsp;</td><td class="fu4 s2">&nbsp;</td><td class="fu5 s2">&nbsp;</td><td class="fu6 s2">&nbsp;</td><td class="fu7 s2">&nbsp;</td><td class="fu8 s2">&nbsp;</td></tr>
        <tr id="rowExercise" d="Exercise" t="rd"><th>Exercise</th><td class="fu1 s2">&nbsp;</td><td class="fu2 s2">&nbsp;</td><td class="fu3 s2">&nbsp;</td><td class="fu4 s2">&nbsp;</td><td class="fu5 s2">&nbsp;</td><td class="fu6 s2">&nbsp;</td><td class="fu7 s2">&nbsp;</td><td class="fu8 s2">&nbsp;</td></tr>
        <tr id="rowExerciseFrequency" d="ExerciseFrequency" t="d"><th>How many times/week</th><td class="fu1 s2">&nbsp;</td><td class="fu2 s2">&nbsp;</td><td class="fu3 s2">&nbsp;</td><td class="fu4 s2">&nbsp;</td><td class="fu5 s2">&nbsp;</td><td class="fu6 s2">&nbsp;</td><td class="fu7 s2">&nbsp;</td><td class="fu8 s2">&nbsp;</td></tr>
        <tr id="rowQuestionsConcerns" d="QuestionsConcerns" t="r"><th>Questions/Concerns</th><td class="fu1 s2">&nbsp;</td><td class="fu2 s2">&nbsp;</td><td class="fu3 s2">&nbsp;</td><td class="fu4 s2">&nbsp;</td><td class="fu5 s2">&nbsp;</td><td class="fu6 s2">&nbsp;</td><td class="fu7 s2">&nbsp;</td><td class="fu8 s2">&nbsp;</td></tr>
        <tr id="rowNotes" d="Notes" t="t"><th>Notes</th><td class="fu1 s2">&nbsp;</td><td class="fu2 s2">&nbsp;</td><td class="fu3 s2">&nbsp;</td><td class="fu4 s2">&nbsp;</td><td class="fu5 s2">&nbsp;</td><td class="fu6 s2">&nbsp;</td><td class="fu7 s2">&nbsp;</td><td class="fu8 s2">&nbsp;</td></tr>
        <tr id="rowCalledBy" d="CalledBy" t="d"><th>Called By</th><td class="fu1 s2">&nbsp;</td><td class="fu2 s2">&nbsp;</td><td class="fu3 s2">&nbsp;</td><td class="fu4 s2">&nbsp;</td><td class="fu5 s2">&nbsp;</td><td class="fu6 s2">&nbsp;</td><td class="fu7 s2">&nbsp;</td><td class="fu8 s2">&nbsp;</td></tr>
    </table>
    </div>
</div>

<table id="controls" class="hide">
	<tr><td>
        <select class="s7 choosen-control Provider" data-placeholder="Cardiologist Name"></select>
	</td></tr>
    <tr><td>
	    <label class='check-label check-label-inline'><input type=radio name=R1 class='1'> Yes<span class='radio'></span></label>
        <label class='check-label check-label-inline'><input type=radio name=R1 class='0'> No<span class='radio'></span></label>
    </td></tr>
    <tr><td>
        <input value="" class="s61" maxlength=1000 placeholder="Actual Weight" />
    </td></tr>
    <tr><td>
        <input value="" class="s61" maxlength=1000 placeholder="Med. Comprehension" />
    </td></tr>
    <tr><td>
	    <label class='check-label check-label-inline'><input type=radio name=R2 class='1'> Yes<span class='radio'></span></label>
        <label class='check-label check-label-inline'><input type=radio name=R2 class='0'> No<span class='radio'></span></label>
    </td></tr>
    <tr><td>
	    <label class='check-label check-label-inline'><input type=radio name=R3 class='1'> Yes<span class='radio'></span></label>
        <label class='check-label check-label-inline'><input type=radio name=R3 class='0'> No<span class='radio'></span></label>
        &nbsp;<input type="text" value="" class="s4" placeholder="Medication refills" />
    </td></tr>
    <tr><td>
	    <label class='check-label check-label-inline'><input type=radio name=R4 class='1'> Yes<span class='radio'></span></label>
        <label class='check-label check-label-inline'><input type=radio name=R4 class='0'> No<span class='radio'></span></label>
        &nbsp;<input type="text" value="" class="s4" placeholder="All Meds filled" />
    </td></tr>
    <tr><td>
        <select class="s7 HowYouFeel" data-placeholder="Rate how you feel today"></select>
	</td></tr>
    <tr><td>
	    <label class='check-label check-label-inline'><input type=radio name=R5 class='1'> Yes<span class='radio'></span></label>
        <label class='check-label check-label-inline'><input type=radio name=R5 class='0'> No<span class='radio'></span></label>
    </td></tr>
    <tr><td>
	    <label class='check-label check-label-inline'><input type=radio name=R6 class='1'> Yes<span class='radio'></span></label>
        <label class='check-label check-label-inline'><input type=radio name=R6 class='0'> No<span class='radio'></span></label>
        &nbsp;<select class="s4 SOB" data-placeholder="SOB"></select>
    </td></tr>
    <tr><td>
	    <label class='check-label check-label-inline'><input type=radio name=R7 class='1'> Yes<span class='radio'></span></label>
        <label class='check-label check-label-inline'><input type=radio name=R7 class='0'> No<span class='radio'></span></label>
        &nbsp;<select class="s4 ProductiveCough" data-placeholder="Productive Cough"></select>
    </td></tr>
    <tr><td>
        <select class="s7 ProductiveCoughAmount" data-placeholder="Productive Cough Amount"></select>
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
        &nbsp;<select class="s4 DietaryEducation" data-placeholder="Dietary Education"></select>
    </td></tr>
    <tr><td>
	    <label class='check-label check-label-inline'><input type=radio name=R11 class='1'> Yes<span class='radio'></span></label>
        <label class='check-label check-label-inline'><input type=radio name=R11 class='0'> No<span class='radio'></span></label>
        &nbsp;<select class="s4 SleepNormal" data-placeholder="Sleep normal"></select>
    </td></tr>
    <tr><td>
	    <label class='check-label check-label-inline'><input type=radio name=R12 class='1'> Yes<span class='radio'></span></label>
        <label class='check-label check-label-inline'><input type=radio name=R12 class='0'> No<span class='radio'></span></label>
        &nbsp;<select class="s4 Exercise" data-placeholder="Exercise"></select>
    </td></tr>
    <tr><td>
	    <select class="s4 ExerciseFrequency" data-placeholder="Exercise Frequency"></select>
    </td></tr>
    <tr><td>
	    <label class='check-label check-label-inline'><input type=radio name=R13 class='1'> Yes<span class='radio'></span></label>
        <label class='check-label check-label-inline'><input type=radio name=R13 class='0'> No<span class='radio'></span></label>
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

