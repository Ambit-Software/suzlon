<%@ Page Title="" Language="C#" MasterPageFile="~/SuzlonBPP.Master" AutoEventWireup="true"  MaintainScrollPositionOnPostback="true" CodeBehind="AgainstBillApprover.aspx.cs" Inherits="SuzlonBPP.AgainstBillApprover" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="Scripts/bootstrap.min.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="txtconformmessageValue" runat="server" />



    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript">

            function RowSelectedApproverGrid(sender, args) {
                debugger;
                var currentTempINR = 0;
                var currentTempProposed = 0;
                var CurrentTempDebit = 0;
                var CurrentTempCredit = 0;
                var CurrentProposedDebit = 0;
                var CurrentProposedCredit = 0;
                var tableView = args.get_tableView();
                var selrec = tableView._dataItems;

                var grid = tableView.get_dataItems();

                for (var i = 0; i < selrec.length; i++)
                {
                    if (selrec[i]._selected) {
                        var row = grid[i];
                        var tempInr = parseFloat($(row.get_cell('Amount')).text().split(',').join(''))
                        var dcflag = $(row.get_cell('DCFLag')).text();


                        if (tempInr) {
                            if (dcflag == 'Credit')
                                CurrentTempCredit = CurrentTempCredit + tempInr
                            else
                                CurrentTempDebit = CurrentTempDebit + tempInr
                        }

                        var tempProp = parseFloat((JSON.parse($(row.findElement('tbApprovedAmount_ClientState')).val()).valueAsString.split(',').join('')));

                        if (tempProp) {
                            if (dcflag == 'Credit')
                                CurrentProposedCredit = CurrentProposedCredit + tempInr
                            else
                                CurrentProposedDebit = CurrentProposedDebit + tempInr
                        }
                    }
                }
                currentTempINR = parseFloat(CurrentTempCredit - CurrentTempDebit).toFixed(2);
                currentTempProposed = parseFloat(CurrentProposedCredit - CurrentProposedDebit).toFixed(2);

                $("#ContentPlaceHolder1_lblINRAmt").html(currentTempINR);
                $("#ContentPlaceHolder1_lblProposedAmt").html(currentTempProposed);
            }

            function RowDeSelectedApproverGrid(sender, args) {
                var grid = $find("<%=gvPendingApproval.ClientID%>").get_masterTableView();      
            }

            function ShowComments(id) {
                var ajaxManager = $find("<%= RadAjaxManager1.ClientID %>");
    ajaxManager.ajaxRequest("Comment#" + id + "");
    $('#squarespaceCommentModal').modal();
}

        </script>
        <script lang="javascript" type="text/javascript">
            var IDs = [];
            function ConfirmMessage(TotalAmount, DailyLimit) {
                var selectedvalue = confirm("Total for today would be Rs. " + TotalAmount + " which is crossing the daily limit " + DailyLimit + ", Do you want to approve ?");
                if (selectedvalue) {
                    document.getElementById("<%=txtconformmessageValue.ClientID %>").value = "Yes";
} else {
    document.getElementById("<%=txtconformmessageValue.ClientID %>").value = "No";
}
}
function openRadWin(screen, mode, entityId, canAdd, canDelete, isMultiFileUpload, showDocumentType, entityName) {
    debugger;
    var manager = $find("<%= RadWindowManager.ClientID %>");
manager.open("AddAttachments.aspx?mode=" + mode + "&entityId=" + entityId + "&canAdd= " + canAdd + "&canDelete= " + canDelete + "&isMultiUpload= " + isMultiFileUpload + "&showDtype= " + showDocumentType + "&entityName=" + entityName, "RadWindow");
return false;

}

function openTreasuryRadWin() {
    debugger;
    var manager = $find("<%= RadWindowManager.ClientID %>");
    // if (screen == 'VC') {
manager.open("TreasuryBudgetAllocation.aspx", "RadWindow");
return false;
    // }
}

function openAllocationPopup(amnt) {
    debugger
  
    setTimeout(function () {
        $("#divViewAllocation").modal("show");
        $("#ContentPlaceHolder1_lbltoalAllocation").text('0');
        $("#ContentPlaceHolder1_lbltoalAmount").text(amnt);
        $("#ContentPlaceHolder1_lblError").text("");
    }, 500);

    var table = document.getElementById("grdBudgetAllocation");
    var rows = table.getElementsByTagName("tr")
    for (var i = 0; i < rows.length; i++) {
        totalRowCount++;
        if (rows[i].getElementsByTagName("td").length > 0) {
            rowCount++;

        }
    }

}


function openVerticalBudgerPopup() {
    setTimeout(function () {
        $("#verticalBudget-modal").modal("show");
    }, 500);

}

function ClosePopup() {
    $("#divViewAllocation").modal("hide");
}

function ValidateAllocatedRow(ClientId) {
    setTimeout(function () {
        debugger;
        var currentValue = JSON.parse($("#" + ClientId + "_ClientState").attr('value')).valueAsString;
        alert(currentValue);
    }, 1000);
}


function ValidateForm() {
    debugger;
    if (parseFloat($("#ContentPlaceHolder1_lbltoalAmount").text()) != parseFloat($("#ContentPlaceHolder1_lbltoalAllocation").text())) {
        // $("#ContentPlaceHolder1_lblError").text("Please Allocate The Total Amount.");
        var $radNotify = $find("<%= radMessage.ClientID%>");
        $radNotify.set_title("Alert");
        $radNotify.set_text("Entered amount doesn't match with required amount.");
        $radNotify.show();
        return false;
    }
    else
        return true;
}
var flag = false;
function ValidateRow(sender, args, index, curBalanceAmount) {

    if (flag == true) {
        flag = false;
        return false;
    }
    var grid = $find("<%=grdBudgetAllocation.ClientID%>");
var dataItems = grid.get_masterTableView().get_dataItems();
var dataItem = dataItems[index];
var CurrentVal = parseFloat(JSON.parse(dataItem.findElement("numCurrentAllocation_ClientState").value).valueAsString); //sender._value;//
var BalAmount = parseFloat(curBalanceAmount);// parseFloat(dataItem.findElement("lblBalanceAmount").textContent);
if (CurrentVal > BalAmount) {
    // $("#ContentPlaceHolder1_lblError").text("Please Enter Value Less Than Balance Amount (" + BalAmount + ").");
    var $radNotify = $find("<%= radMessage.ClientID%>");
    $radNotify.set_title("Alert");
    $radNotify.set_text("Please enter value less than Balance Amount (" + BalAmount + ").");
    $radNotify.show();

    flag = true
    $find(sender._clientID).set_value('');
    args.set_newValue("False");
    args.set_cancel(true);
    return false;
} else {
    $("#ContentPlaceHolder1_lblError").text("");
}
    //debugger;
    //A1 = new Array(index, sender._value);
    //IDs.push(A1);
    var currentTotalAllocation = 0;
    for (var i = 0; i < dataItems.length; i++) {
        temp = parseFloat(JSON.parse(dataItems[i].findElement("numCurrentAllocation_ClientState").value).valueAsString);
        if (temp) currentTotalAllocation = currentTotalAllocation + temp;
    }
    $("#ContentPlaceHolder1_lbltoalAllocation").text(currentTotalAllocation);
    $("#ContentPlaceHolder1_lblError").text("");
    return true;
}
        </script>
    </telerik:RadCodeBlock>
    <script>
        function GridData() {

            $('#squarespaceModal').modal('show');
        }
    </script>
    <%--    <script >
function ConfirmSubmit()
{
alert("Test");
if (confirm("Submit detail"))
    return true;
else
    return false;
}
</script> --%>
    <style>
        .RadPicker {
            width: 200px !important;
        }

        .RadTabStrip_Silk .rtsLevel1 {
            border-color: transparent;
        }

        .RadWindow .rwIcon {
            display: none !important;
        }

        .RadWindow .rwTitleWrapper .rwTitle {
            margin: 2px 0 0 -21px !important;
            font-weight: bold !important;
        }

        .RadGrid_Default {
            background-color: #FFF !important;
        }
        /** {
-webkit-box-sizing: border-box;
-moz-box-sizing: border-box;
box-sizing: border-box ;
}*/

        .RadGrid_Frozen {
            overflow-x: scroll !important;
        }

        [id$='Frozen'] {
            overflow: scroll !important;
        }
    </style>

    <telerik:RadScriptBlock ID="radSript1" runat="server">
        <script type="text/javascript">

            function saveConfirmation(btntext) {
                debugger;
                function confirmCallBack(arg) {
                    debugger;
                    if (arg) {
                        $find("<%= RadAjaxManager1.ClientID %>").ajaxRequest("ProceedToSaveYes#" + btntext);
                    }
                    else {
                        eventargs.set_cancel(true);
                    }
}
    Telerik.Web.UI.RadWindowUtils.Localization =
    {
        "OK": "Yes",
        "Cancel": "No"
    };
    radconfirm("Today's approved total limit is exceeded. Do you still want to go continue?", confirmCallBack, "500px", "150px", null, "Confirmation");
}


// function submitConfirmation() {
//     function submitCallBack(arg) {
//         debugger;
//         alert(arg);
//        //if (arg) {
//        //    return true;
//        //} else {
//        //    return false;
//        //}
//    }
//    Telerik.Web.UI.RadWindowUtils.Localization =
//    {
//        "OK": "Yes",
//        "Cancel": "No"
//    };
//    debugger;
//    radconfirm("Are you sure you want proceed?", submitCallBack, "500px", "150px", null, "Confirmation");
//}

        </script>
    </telerik:RadScriptBlock>
      <style type="text/css">
        .dialog-background {
            background: none repeat scroll 0 0 rgba(248, 246, 246, 0.00);
            height: 100%;
            left: 0;
            margin: 0;
            padding: 0;
            position: absolute;
            top: 0;
            width: 100%;
            z-index: 100;
        }

        .dialog-loading-wrapper {
            /*background-image: url(../Content/images/loading.gif);*/
           
            border: 0 none;
            height: 100px;
            left: 50%;
            margin-left: -50px;
            margin-top: -50px;
            position: fixed;
            top: 50%;
            width: 100px;
            z-index: 9999999;
            opacity: 1;
        }

        .dialog-loading-icon {
            background-image: url("content/images/loading.gif");
            background-repeat: no-repeat;
            /*background-color: #EFEFEF !important;*/
            /*border-radius: 13px;*/
            display: block;
            height: 100px;
            line-height: 100px;
            margin: 0;
            padding: 1px;
            text-align: center;
            width: 100px;
            opacity: 1;
        }
    </style>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" OnAjaxRequest="RadAjaxManager1_AjaxRequest" >
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadAjaxManager1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdComment" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="pnl1" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="gvPendingApproval" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                     <telerik:AjaxUpdatedControl ControlID="radNotificationMessage" UpdatePanelRenderMode="Inline" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="gvCorrection" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                     <telerik:AjaxUpdatedControl ControlID="lnkVerticalBudget" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>


      <%--      <telerik:AjaxSetting AjaxControlID="RadAjaxManager1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="radNotificationMessage" UpdatePanelRenderMode="Inline" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>--%>
            <telerik:AjaxSetting AjaxControlID="cmbVertical">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadMultiPage1" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                      <telerik:AjaxUpdatedControl ControlID="lblINRAmt" LoadingPanelID="LoadingPanel1" UpdatePanelRenderMode="Inline"></telerik:AjaxUpdatedControl>                    
                     <telerik:AjaxUpdatedControl ControlID="lblProposedAmt" LoadingPanelID="LoadingPanel1" UpdatePanelRenderMode="Inline"></telerik:AjaxUpdatedControl>                    

                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rbtnAgainst">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadMultiPage1" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                      <telerik:AjaxUpdatedControl ControlID="lblINRAmt" LoadingPanelID="LoadingPanel1" UpdatePanelRenderMode="Inline"></telerik:AjaxUpdatedControl>                    
                     <telerik:AjaxUpdatedControl ControlID="lblProposedAmt" LoadingPanelID="LoadingPanel1" UpdatePanelRenderMode="Inline"></telerik:AjaxUpdatedControl>                    

                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rbtnAdvance">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadMultiPage1" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                      <telerik:AjaxUpdatedControl ControlID="lblINRAmt" LoadingPanelID="LoadingPanel1" UpdatePanelRenderMode="Inline"></telerik:AjaxUpdatedControl>                    
                     <telerik:AjaxUpdatedControl ControlID="lblProposedAmt" LoadingPanelID="LoadingPanel1" UpdatePanelRenderMode="Inline"></telerik:AjaxUpdatedControl>                    

                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="gvCorrection2">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="gvCorrection2" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>

            <%--     <telerik:AjaxSetting AjaxControlID="gvCorrection2">
<UpdatedControls>
<telerik:AjaxUpdatedControl ControlID="gvCorrection1" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
</UpdatedControls>
</telerik:AjaxSetting>--%>

            <telerik:AjaxSetting AjaxControlID="gvPendingApproval2">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="gvPendingApproval2" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                
                   
                </UpdatedControls>
            </telerik:AjaxSetting>

      
            <%--  <telerik:AjaxSetting AjaxControlID="gvPendingApproval2">
<UpdatedControls>
<telerik:AjaxUpdatedControl ControlID="gvPendingApproval1" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
</UpdatedControls>
</telerik:AjaxSetting>--%>

            <telerik:AjaxSetting AjaxControlID="RadMultiPage1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadMultiPage1" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lnkSubmit">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="pnl1" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="numCurrentAllocation" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                   <%--  <telerik:AjaxUpdatedControl ControlID="gvPendingApproval" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>--%>
                    <%-- <telerik:AjaxUpdatedControl ControlID="radMessage" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>--%>
                     <telerik:AjaxUpdatedControl ControlID="lnkVerticalBudget" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lnkSubmit1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="pnl1" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="numCurrentAllocation" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                   <%--   <telerik:AjaxUpdatedControl ControlID="radMessage" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>--%>
                    <%-- <telerik:AjaxUpdatedControl ControlID="RadCodeBlock1" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>--%>
                     <telerik:AjaxUpdatedControl ControlID="lnkVerticalBudget" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="pnl1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="pnl1" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="cmbVertical">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="lnkVerticalBudget" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lnkVerticalBudget">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdVerticalBudget" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rbtnAgainst" EventName="">
                <UpdatedControls>

                    <telerik:AjaxUpdatedControl ControlID="rbtnAgainst" LoadingPanelID="LoadingPanel1" UpdatePanelRenderMode="Inline"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="rbtnAdvance" LoadingPanelID="LoadingPanel1" UpdatePanelRenderMode="Inline"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="rbtnAdvance" EventName="">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rbtnAgainst" LoadingPanelID="LoadingPanel1" UpdatePanelRenderMode="Inline"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="rbtnAdvance" LoadingPanelID="LoadingPanel1" UpdatePanelRenderMode="Inline"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
             <telerik:AjaxSetting AjaxControlID="grdVerticalBudget">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdVerticalBudget" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>

                <%--  <telerik:AjaxSetting AjaxControlID="btnSave">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="gvPendingApproval" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                   <telerik:AjaxUpdatedControl ControlID="pnl1" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                   <telerik:AjaxUpdatedControl ControlID="gvPendingApproval" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                     <telerik:AjaxUpdatedControl ControlID="gvApproved" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="gvCorrection" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="grdBudgetAllocation" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>--%>

        </AjaxSettings>

    </telerik:RadAjaxManager>
        <telerik:RadAjaxLoadingPanel ID="LoadingPanel1" BackgroundPosition="Center" runat="server" Height="100%" Width="75px" Transparency="50">
         <div class="dialog-background">
            <div class="dialog-loading-wrapper">
                <span class="dialog-loading-icon"></span>
            </div>
        </div>
    </telerik:RadAjaxLoadingPanel>


    <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager" runat="server" EnableShadow="true">
        <Windows>
            <telerik:RadWindow ID="RadWindow" RenderMode="Lightweight" runat="server" CssClass="window123" VisibleStatusbar="false"
                Width="700%" Height="550" AutoSize="false" Behaviors="Close" ShowContentDuringLoad="false"
                Modal="true" ReloadOnShow="true" />
        </Windows>
    </telerik:RadWindowManager>

    <div class="row margin-0">
        <div class="col-xs-12 heading-big">
            <h5 class="margin-0 lineheight-42 breath-ctrl">Home / <span>Against Bill: Approver</span></h5>
        </div>
        <div class="col-sm-6 col-xs-6 ">
        </div>
    </div>

    <div class="row margin-0">
        <telerik:RadWindowManager ID="RadWindowManager1" runat="server"></telerik:RadWindowManager>
        <telerik:RadNotification RenderMode="Lightweight" Position="Center" runat="server" ID="radMessage" VisibleOnPageLoad="false"
            Width="350px" TitleIcon="none" ContentIcon="none" EnableShadow="true" AnimationDuration="1000">
        </telerik:RadNotification>
        <div class="col-md-4">
            <div class="col-sm-12">
                <div class="col-sm-2 col-xs-12 padding-0">
                    <label class="control-label lable-txt" for="name">Vertical</label>
                </div>
                <div class="col-sm-10 col-xs-12 padding-0">
                    <div class="form-group">
                        <telerik:RadComboBox ID="cmbVertical" AutoPostBack="true" RenderMode="Lightweight" runat="server" OnSelectedIndexChanged="cmbVertical_SelectedIndexChanged">
                            <Items>
                                <%--<telerik:RadComboBoxItem Value="Select Vertical" Text="Select Vertical" />
            <telerik:RadComboBoxItem Value="Vertical One" Text="Vertical One" />
            <telerik:RadComboBoxItem Value="Vertical Two" Text="Vertical Two" />
            <telerik:RadComboBoxItem Value="Vertical Three" Text="Vertical Three" />--%>
                            </Items>
                        </telerik:RadComboBox>
                    </div>

                </div>
                <div class="col-xs-12 padding-0">
                    <div class="text-info small">
                        Note:Items blocked for payment are not considered.
                    </div>
                </div>

            </div>

        </div>
        <div class="col-md-2">
            <div class="col-sm-12 col-xs-12 agnstbillradio">
                <asp:RadioButton ID="rbtnAgainst" OnCheckedChanged="rbtnAgainst_CheckedChanged" AutoPostBack="true" Checked="true" GroupName="rblInitiator" runat="server" />Against Bill
                <asp:RadioButton ID="rbtnAdvance" OnCheckedChanged="rbtnAgainst_CheckedChanged" AutoPostBack="true" GroupName="rblInitiator" runat="server" />Advance
            </div>
            <!-- End of Company name-row-->
        </div>

        <div class="col-md-4">
            <div class="col-sm-12">
                <div class="col-sm-4 col-xs-12 subheading padding-0">
                    <label class="control-label lable-txt" for="name">Vertical Budget:</label>
                </div>
                <div class="col-sm-8 col-xs-12 padding-0">
                    <label class="control-label lable-txt" for="name">
                        <%--<a class="budget" id="aVerticalBudget" data-toggle="modal" runat="server"  data-target="#budget-utilization-modal">20,0000</a>--%>
                        <asp:LinkButton ID="lnkVerticalBudget" runat="server" OnClick="lnkVerticalBudget_Click" Enabled="true" Text="0"></asp:LinkButton>
                    </label>
                </div>
            </div>
        </div>
    </div>
    <!--End of heading wrapper-->


    <div class="row margin-10">
        <div>
            <div>
                <div style="text-align-last: right">
                    <asp:Button CssClass="btn btn-grey" OnClick="btnExport_Click" Text="Export" runat="server" ID="btnExport"></asp:Button>
                </div>
                <div>
                </div>
            </div>
            <div class="col-sm-1">
            </div>
        </div>
        <div class="col-xs-12 ">
            <div class="panel with-nav-tabs panel-default border-0">
                
                <telerik:RadTabStrip RenderMode="Lightweight"  runat="server" ID="rdTab" OnTabClick="rdTab_TabClick" MultiPageID="RadMultiPage1" SelectedIndex="0" Skin="Silk">
                    <Tabs>
                        <telerik:RadTab Text="Pending for Approval"></telerik:RadTab>
                        <telerik:RadTab Text="Approved Requests"></telerik:RadTab>
                        <telerik:RadTab Text="Need Correction"></telerik:RadTab>
                    </Tabs>                    
                </telerik:RadTabStrip>
                 <div style="position: absolute; top: 7px; left: 50%; min-width: 350px;">                                         
                    <div style="margin:auto!important">
                    <asp:Label runat="server" ID="lblINRAmtText" Text="INR Total: " CssClass="control-label lable-txt  pull-left"></asp:Label>
                    </div>
                    <div style="margin:auto!important">
                    <asp:Label ID="lblINRAmt" runat="server" CssClass="control-label lable-txt pull-left" Text="0"></asp:Label>                                         
                    </div>
                    <div style="margin:auto!important;">
                    <asp:Label runat="server" style="padding-left: 10px;" ID="lblProposedAmtText" CssClass="control-label lable-txt pull-left" Text="Proposed Total: "></asp:Label>                                   
                    </div>
                    <div style="margin:auto!important">
                    <asp:Label ID="lblProposedAmt" runat="server" CssClass="control-label lable-txt pull-left" Text="0"></asp:Label>
                    </div>
                   </div>
                <div class="overflow-x">
                    <telerik:RadMultiPage runat="server" ID="RadMultiPage1" SelectedIndex="0" CssClass="outerMultiPage">
                        <telerik:RadPageView runat="server" ID="RadPagePending">
                            <%--<telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" EnablePageHeadUpdate="true">--%>
                            <telerik:RadGrid RenderMode="Lightweight" ID="gvPendingApproval" runat="server" AutoGenerateColumns="false"
                                AllowPaging="true" AllowSorting="false" AllowFilteringByColumn="false" 
                                OnNeedDataSource="gvPendingApproval_NeedDataSource" OnItemCommand="gvPendingApproval_ItemCommand">

                                <GroupingSettings CaseSensitive="false" />

                                <ClientSettings>
                                    <Selecting AllowRowSelect="true"></Selecting>
                                    <Scrolling AllowScroll="True" UseStaticHeaders="True" ScrollHeight="550px" SaveScrollPosition="true" FrozenColumnsCount="4"></Scrolling>
                                </ClientSettings>
                                <MasterTableView CommandItemSettings-ShowRefreshButton="false" DataKeyNames="Vendor" EditMode="InPlace" ShowHeader="true" CommandItemDisplay="None"
                                    AllowFilteringByColumn="false" AllowSorting="false" NoMasterRecordsText="" TableLayout="Fixed" Font-Size="9">
                                    <CommandItemSettings ShowRefreshButton="false" ShowExportToExcelButton="false" />
                                    <NoRecordsTemplate>
                                        <table width="100%" border="0" cellpadding="20" cellspacing="20">
                                            <tr>
                                                <td align="center" class="txt-white">No records to display.
                                                </td>
                                            </tr>
                                        </table>
                                    </NoRecordsTemplate>
                                    <NestedViewTemplate>

                                        <telerik:RadGrid RenderMode="Lightweight" ID="gvPendingApproval1" runat="server" AutoGenerateColumns="false"
                                            AllowPaging="true" AllowSorting="false" AllowFilteringByColumn="false" PageSize="5"
                                            OnItemCommand="gvPendingApproval1_ItemCommand">
                                            <GroupingSettings CaseSensitive="false" />
                                            <ClientSettings>
                                                <Selecting AllowRowSelect="true"></Selecting>
                                                <Scrolling AllowScroll="True" UseStaticHeaders="True" ScrollHeight="470px" SaveScrollPosition="true" FrozenColumnsCount="4"></Scrolling>
                                            </ClientSettings>
                                            <MasterTableView CommandItemSettings-ShowRefreshButton="false" EditMode="InPlace" ShowHeader="true" CommandItemDisplay="None"
                                                AllowFilteringByColumn="false" DataKeyNames="Company" AllowSorting="false" NoMasterRecordsText="" TableLayout="Fixed" Font-Size="9">
                                                <CommandItemSettings ShowRefreshButton="false" ShowExportToExcelButton="false" />
                                                <NoRecordsTemplate>
                                                    <table width="100%" border="0" cellpadding="20" cellspacing="20">
                                                        <tr>
                                                            <td align="center" class="txt-white">No records to display.
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </NoRecordsTemplate>
                                                <NestedViewTemplate>
                                                    <div class="table-scroll">
                                                        <telerik:RadGrid RenderMode="Lightweight" ID="gvPendingApproval2" runat="server" AutoGenerateColumns="false" OnItemCommand="gvPendingApproval2_ItemCommand"
                                                            OnItemDataBound="gvPendingApproval2_ItemDataBound" AllowMultiRowSelection="true" AllowPaging="False" AllowSorting="false" AllowFilteringByColumn="false"
                                                            PageSize="5">
                                                            <GroupingSettings CaseSensitive="false" />
                                                            <ClientSettings>
                                                                <Selecting AllowRowSelect="true"></Selecting>
                                                                <Scrolling AllowScroll="True" UseStaticHeaders="True" ScrollHeight="300px" SaveScrollPosition="true" FrozenColumnsCount="4"></Scrolling>
                                                               <ClientEvents OnRowSelected="RowSelectedApproverGrid" OnRowDeselected="RowSelectedApproverGrid" />
                                                            </ClientSettings>
                                                            
                                                            <MasterTableView CommandItemSettings-ShowRefreshButton="false" EditMode="InPlace" CommandItemDisplay="None"
                                                                AllowFilteringByColumn="false" DataKeyNames="Id,OpenAdvance,SubVerticalId" AllowPaging="false" AllowSorting="true" NoMasterRecordsText="" TableLayout="Fixed" Font-Size="9">
                                                                <CommandItemSettings ShowRefreshButton="false" ShowExportToExcelButton="false" />
                                                                <NoRecordsTemplate>
                                                                    <table width="100%" border="0" cellpadding="20" cellspacing="20">
                                                                        <tr>
                                                                            <td align="center" class="txt-white">No records to display.
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </NoRecordsTemplate>
                                                                <Columns>
                                                                    <telerik:GridClientSelectColumn HeaderStyle-Width="80px" HeaderText="Select" UniqueName="RequestSelect">
                                                                        <ItemStyle Height="60px" />
                                                                    </telerik:GridClientSelectColumn>

                                                                            <telerik:GridBoundColumn HeaderStyle-Width="120px" HeaderText="Reference" UniqueName="Reference" DataField="Reference">
                                                                        <ItemStyle Height="60px" />
                                                                    </telerik:GridBoundColumn>
                                                                             <telerik:GridBoundColumn HeaderStyle-Width="120px" HeaderText="DPR Number" UniqueName="DPRNumber" DataField="DPRNumber">
                                                                        <ItemStyle Height="60px" />
                                                                    </telerik:GridBoundColumn>
                                                                        <telerik:GridBoundColumn HeaderStyle-Width="120px" HeaderText="Document Number" UniqueName="DocumentNumber" DataField="DPRNumber">
                                                                        <ItemStyle Height="60px" />
                                                                    </telerik:GridBoundColumn>
                                                                     <telerik:GridBoundColumn HeaderStyle-Width="150px"   HeaderText="Gross Amount" UniqueName="GrossAmount" DataField="GrossAmount" DataFormatString="{0:###,##0.00}">
                                                                    </telerik:GridBoundColumn>
                                                                      <telerik:GridBoundColumn HeaderStyle-Width="150px"  HeaderText="Tax Rate" UniqueName="TaxRate" DataField="TaxRate" DataFormatString="{0:###,##0.00}">
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridBoundColumn HeaderStyle-Width="150px"  HeaderText="Tax Amount" UniqueName="TaxAmount" DataField="TaxAmount" DataFormatString="{0:###,##0.00}">
                                                                    </telerik:GridBoundColumn>
                                                                      <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="Amount in INR" UniqueName="Amount" DataField="Amount" DataFormatString="{0:###,##0.00}">
                                                                        <ItemStyle Height="60px" />
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="Amount Proposed" UniqueName="AmountProposed" DataField="AmountProposed" DataFormatString="{0:###,##0.00}">
                                                                        <ItemStyle Height="60px" />
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridTemplateColumn HeaderStyle-Width="120px" HeaderText="Approved Amount" UniqueName="ApprovedAmount" DataField="ApprovedAmount">
                                                                        <ItemStyle Height="60px" />
                                                                        <ItemTemplate>
                                                                            <%--  <telerik:RadTextBox ID="tbApprovedAmount" runat="server"></telerik:RadTextBox>--%>
                                                                            <telerik:RadNumericTextBox ID="tbApprovedAmount" Width="90px" class="form-control" AutoPostBack="true"
                                                                                Text='<%# String.Format("{0:###,##0.00}",Eval("ApprovedAmount")) %>' OnTextChanged="tbApprovedAmount_TextChanged" runat="server">
                                                                            </telerik:RadNumericTextBox>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                     <telerik:GridBoundColumn  HeaderStyle-Width="80px" HeaderText="Debit/Credit" UniqueName="DCFLag" DataField="DCFLag" >
                                                                </telerik:GridBoundColumn>  
                                                                         <telerik:GridTemplateColumn HeaderStyle-Width="100px" HeaderText="Approval" UniqueName="Approval" DataField="Approval">
                                                                        <ItemStyle Height="60px" />
                                                                        <ItemTemplate>
                                                                            <telerik:RadDropDownList RenderMode="Lightweight" AutoPostBack="true"
                                                                                OnSelectedIndexChanged="cmbApproval_SelectedIndexChanged" ID="cmbApproval" runat="server">                                                                               
                                                                            </telerik:RadDropDownList>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridTemplateColumn HeaderStyle-Width="100px" HeaderText="Select Stage" UniqueName="Stage" DataField="Stage">
                                                                        <ItemStyle Height="60px" />
                                                                        <ItemTemplate>
                                                                            <telerik:RadDropDownList Enabled="false" RenderMode="Lightweight" ID="cmbStage" runat="server">
                                                                            </telerik:RadDropDownList>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>


                                                                    <telerik:GridBoundColumn HeaderStyle-Width="120px" HeaderText="Withholding Tax Code" UniqueName="WithholdingTaxCode" DataField="WithholdingTaxCode">
                                                                        <ItemStyle Height="60px" />
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridBoundColumn HeaderStyle-Width="120px" HeaderText="Un-settled Open Advance (INR)" UniqueName="UnsettledOpenAdvance" DataField="UnsettledOpenAdvance">
                                                                        <ItemStyle Height="60px" />
                                                                    </telerik:GridBoundColumn>
                                                                      <telerik:GridBoundColumn HeaderStyle-Width="100px" HeaderText="Document Currency" UniqueName="DocumentCurrency" DataField="Currency">
                                                                        <ItemStyle Height="60px" />
                                                                    </telerik:GridBoundColumn>
                                                                      <telerik:GridDateTimeColumn HeaderStyle-Width="150px" HeaderText="Net Due Date" UniqueName="NetDueDate" DataField="NetDuedate" DataFormatString="{0:dd-MM-yyyy}">
                                                                    </telerik:GridDateTimeColumn>
                                                                          <telerik:GridBoundColumn HeaderStyle-Width="200px" HeaderText="Nature of Request" UniqueName="NatureOfRequest" DataField="Natureofrequest">
                                                                        <ItemStyle Height="60px" />
                                                                    </telerik:GridBoundColumn>
                                                                         <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="Justification for Adv Payment" UniqueName="JustificationforAdvPayment" DataField="JustificationforAdvPayment">
                                                                        <ItemStyle Height="60px" />
                                                                    </telerik:GridBoundColumn>
                                                                     <telerik:GridTemplateColumn HeaderStyle-Width="200px" HeaderText="Remarks" UniqueName="Remark" DataField="Remark">
                                                                        <ItemStyle Height="60px" />
                                                                        <ItemTemplate>
                                                                            <telerik:RadTextBox ID="tbRemark" Width="150px" TextMode="MultiLine" Text='<%# Eval("Remark") %>' runat="server"></telerik:RadTextBox>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn HeaderStyle-Width="100px" HeaderText="Attachments" AllowFiltering="false" AllowSorting="false" UniqueName="Attachments">
                                                                        <ItemStyle Height="60px" />
                                                                        <ItemTemplate>
                                                                            <telerik:RadButton ID="btnAttachment" runat="server" CommandName="Attachment" CommandArgument='<%# Eval("Id") %>' Text="Add/View" CssClass="gridHyperlinks"></telerik:RadButton>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                         <telerik:GridDateTimeColumn HeaderStyle-Width="150px" HeaderText="Posting Date" UniqueName="PostDate" DataField="PostingDate" DataFormatString="{0:dd-MM-yyyy}">
                                                                        <ItemStyle Height="60px" />
                                                                    </telerik:GridDateTimeColumn>
                                                                     <telerik:GridDateTimeColumn HeaderStyle-Width="150px" HeaderText="Expected Clearing Date" UniqueName="ExpectedClearingDate" DataField="ExpectedClearingDate" DataFormatString="{0:dd-MM-yyyy}">
                                                                        <ItemStyle Height="60px" />
                                                                    </telerik:GridDateTimeColumn>

                                                                          <telerik:GridBoundColumn HeaderStyle-Width="100px" HeaderText="Fiscal Year" UniqueName="FiscalYear" DataField="FiscalYear">
                                                                        <ItemStyle Height="60px" />
                                                                    </telerik:GridBoundColumn>
                                                                            <telerik:GridBoundColumn HeaderStyle-Width="100px" HeaderText="Purchasing Document" UniqueName="PurchasingDocument" DataField="PurchasingDocument">
                                                                        <ItemStyle Height="60px" />
                                                                    </telerik:GridBoundColumn>
                                                                          <telerik:GridBoundColumn HeaderStyle-Width="120px" HeaderText="Special GL" UniqueName="SpecialGL" DataField="SpecialGL">
                                                                        <ItemStyle Height="60px" />
                                                                    </telerik:GridBoundColumn>
                                                                        <telerik:GridBoundColumn HeaderStyle-Width="120px" HeaderText="Profit Centre" UniqueName="ProfitCentre" DataField="ProfitCentre">
                                                                        <ItemStyle Height="60px" />
                                                                    </telerik:GridBoundColumn>
                                                                         <telerik:GridBoundColumn HeaderStyle-Width="120px" HeaderText="Business Area" UniqueName="BusinessArea" DataField="BusinessArea">
                                                                        <ItemStyle Height="60px" />
                                                                    </telerik:GridBoundColumn>
                                                                   
                                                                 
                                                                   

                                                                    <telerik:GridBoundColumn HeaderText="Sub Vertical" HeaderStyle-Width="120px" UniqueName="SubVertical" DataField="SubVertical">
                                                                        <ItemStyle Height="60px" />
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridBoundColumn HeaderStyle-Width="80px" HeaderText="Payment Method" UniqueName="PaymentMethod" DataField="PaymentMethod">
                                                                        <ItemStyle Height="60px" />
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridBoundColumn HeaderStyle-Width="80px" HeaderText="Cheque Lot Number" UniqueName="ChecqueLotNumber" DataField="ChequeLotNumber">
                                                                        <ItemStyle Height="60px" />
                                                                    </telerik:GridBoundColumn>                                                               
                                                                    <telerik:GridTemplateColumn HeaderStyle-Width="80px" UniqueName="TemplateComment" HeaderText="View History">
                                                                        <ItemStyle Height="60px" />
                                                                        <ItemTemplate>
                                                                            <asp:HyperLink ID="viewComment" runat="server" Text="View" CssClass="btn btn-grey gridHyperlinks"></asp:HyperLink>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                   
                                                                    <%--                                                               <telerik:GridTemplateColumn HeaderText="Add Attachment" UniqueName="AddAttachment">
                                                <ItemTemplate>
                                                    <asp:HyperLink ID="addAttachment" runat="server" Text="Browse" CssClass="btn btn-grey gridHyperlinks"></asp:HyperLink>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>--%>
                                                               
                                                                    <telerik:GridBoundColumn HeaderStyle-Width="100px" HeaderText="OpenAdvance" UniqueName="OpenAdvance" Visible="false" DataField="OpenAdvance">
                                                                        <ItemStyle Height="60px" />
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridDateTimeColumn HeaderStyle-Width="150px" HeaderText="Submission Date" UniqueName="SubmissionDate" DataField="SubmissionDate" DataFormatString="{0:dd-MM-yyyy}">
                                                                        <ItemStyle Height="60px" />
                                                                    </telerik:GridDateTimeColumn>
                                                                    <%--  <telerik:GridTemplateColumn UniqueName="BudgetTreasuryAllocation" HeaderText="Budget Treasury Allocation">
                                                <ItemTemplate>
                                                    <asp:HyperLink ID="viewTreasuryApplocation" runat="server" Text="View" CssClass="btn btn-grey gridHyperlinks"></asp:HyperLink>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>--%>
                                                                <telerik:GridTemplateColumn HeaderStyle-Width="100px" HeaderText="Refresh" AllowFiltering="false" AllowSorting="false" UniqueName="Refresh">
                                                                <ItemStyle Height="60px" />
                                                                <ItemTemplate>
                                                                    <telerik:RadButton ID="btnRefresh" runat="server" CommandName="Refresh" CommandArgument='<%# Eval("Id") %>' Text="Refresh" CssClass="gridHyperlinks"></telerik:RadButton>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn> 

                                                                </Columns>
                                                                <PagerStyle PageSizes="5,10,15" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                                                                    PageSizeLabelText="Records per page:" Visible="false" Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />
                                                            </MasterTableView>
                                                        </telerik:RadGrid>
                                                        <div class="col-lg-12 padding-0 margin-10">
                                                            <asp:LinkButton CssClass="btn btn-grey" OnClick="lnkSave_Click" runat="server" ID="lnkSave">Save</asp:LinkButton>
                                                            <asp:LinkButton CssClass="btn btn-grey" OnClientClick="return confirm('Are you sure you want proceed?');" OnClick="lnkSubmit_Click" runat="server" ID="lnkSubmit">Submit</asp:LinkButton>
                                                            <%--                                                        <asp:LinkButton CssClass="btn btn-grey" OnClick="lnkCancel_Click" runat="server" ID="lnkCancel">Cancel</asp:LinkButton>--%>
                                                            <%--<a href="home_screen.html" class="btn btn-grey">Cancel</a>--%>
                                                        </div>
                                                    </div>
                                                </NestedViewTemplate>



                                                <Columns>
                                                    <telerik:GridClientSelectColumn HeaderText="Select" UniqueName="Select">
                                                    </telerik:GridClientSelectColumn>
                                                    <telerik:GridBoundColumn HeaderText="Company" UniqueName="Company" DataField="Company">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn HeaderText="Total Amount" UniqueName="TotalAmount" DataField="TotalAmount" DataFormatString="{0:###,##0.00}">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn HeaderText="Proposed Total" UniqueName="ProposedTotal" DataField="ProposedTotal" DataFormatString="{0:###,##0.00}">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn HeaderText="Approved Total" UniqueName="ApprovedTotal" DataField="ApprovedTotal" DataFormatString="{0:###,##0.00}">
                                                        <%--         <ItemTemplate>
                                    <telerik:RadLabel ID="lbAmountApproved" Text='<%#Eval("ProposedTotal") %>' Width="125px" class="form-control" runat="server"></telerik:RadLabel>
                                </ItemTemplate>--%>
                                                    </telerik:GridBoundColumn>
                                                    <%-- <telerik:GridBoundColumn HeaderText="Today's Paid Total" UniqueName="TodayTotal" DataField="TodayTotal" DataFormatString="{0:###,##0.00}">
                            </telerik:GridBoundColumn>--%>
                                                </Columns>
                                                <PagerStyle PageSizes="5,10,15" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                                                    PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />
                                            </MasterTableView>
                                        </telerik:RadGrid>
                                    </NestedViewTemplate>
                                    <Columns>
                                        <telerik:GridClientSelectColumn HeaderText="Select" UniqueName="Select">
                                        </telerik:GridClientSelectColumn>
                                        <telerik:GridBoundColumn HeaderText="Vendor" UniqueName="Vendor" DataField="Vendor">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn HeaderText="Total Amount" UniqueName="TotalAmount" DataField="TotalAmount" DataFormatString="{0:###,##0.00}">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn HeaderText="Proposed Total" UniqueName="ProposedTotal" DataField="ProposedTotal" DataFormatString="{0:###,##0.00}">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn HeaderText="Approved Total" UniqueName="ApprovedTotal" DataField="ApprovedTotal" DataFormatString="{0:###,##0.00}">
                                        </telerik:GridBoundColumn>
                                        <%-- <telerik:GridBoundColumn HeaderText="Today's Paid Total" UniqueName="TodayTotal" DataField="TodayTotal" DataFormatString="{0:###,##0.00}">
                </telerik:GridBoundColumn>--%>
                                    </Columns>
                                    <PagerStyle PageSizes="5,10,15" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                                        PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />
                                </MasterTableView>
                            </telerik:RadGrid>
                            <%-- </telerik:RadAjaxPanel>--%>
                        </telerik:RadPageView>
                        <telerik:RadPageView runat="server" ID="RadPageApproved">

                            <telerik:RadGrid RenderMode="Lightweight" ID="gvApproved" runat="server" AutoGenerateColumns="false"
                                AllowPaging="False" AllowSorting="false" AllowFilteringByColumn="false" PageSize="5"
                                OnNeedDataSource="gvApproved_NeedDataSource" OnItemCommand="gvApproved_ItemCommand">

                                <GroupingSettings CaseSensitive="false" />
                                <ClientSettings>
                                    <Selecting AllowRowSelect="true"></Selecting>
                                    <Scrolling AllowScroll="True" UseStaticHeaders="True" ScrollHeight="550px" SaveScrollPosition="true" FrozenColumnsCount="4"></Scrolling>
                                </ClientSettings>
                                <MasterTableView CommandItemSettings-ShowRefreshButton="false" DataKeyNames="Vendor" EditMode="InPlace" ShowHeader="true" CommandItemDisplay="None"
                                    AllowFilteringByColumn="false" AllowSorting="false" NoMasterRecordsText="" TableLayout="Fixed" PageSize="9">
                                    <CommandItemSettings ShowRefreshButton="false" ShowExportToExcelButton="false" />
                                    <NoRecordsTemplate>
                                        <table width="100%" border="0" cellpadding="20" cellspacing="20">
                                            <tr>
                                                <td align="center" class="txt-white">No records to display.
                                                </td>
                                            </tr>
                                        </table>
                                    </NoRecordsTemplate>
                                    <NestedViewTemplate>

                                        <telerik:RadGrid RenderMode="Lightweight" ID="gvApproved1" runat="server" AutoGenerateColumns="false"
                                            AllowPaging="False" AllowSorting="false" AllowFilteringByColumn="false" PageSize="5"
                                            OnItemCommand="gvApproved1_ItemCommand">
                                            <GroupingSettings CaseSensitive="false" />
                                            <ClientSettings>
                                                <Selecting AllowRowSelect="true"></Selecting>
                                                <Scrolling AllowScroll="True" UseStaticHeaders="True" ScrollHeight="478px" SaveScrollPosition="true" FrozenColumnsCount="4"></Scrolling>
                                            </ClientSettings>
                                            <MasterTableView CommandItemSettings-ShowRefreshButton="false" EditMode="InPlace" ShowHeader="true" CommandItemDisplay="None"
                                                AllowFilteringByColumn="false" DataKeyNames="Company" AllowSorting="false" NoMasterRecordsText="" TableLayout="Fixed" PageSize="9">
                                                <CommandItemSettings ShowRefreshButton="false" ShowExportToExcelButton="false" />
                                                <NoRecordsTemplate>
                                                    <table width="100%" border="0" cellpadding="20" cellspacing="20">
                                                        <tr>
                                                            <td align="center" class="txt-white">No records to display.
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </NoRecordsTemplate>
                                                <NestedViewTemplate>
                                                    <div>
                                                        <telerik:RadGrid RenderMode="Lightweight" ID="gvApproved2" runat="server" AutoGenerateColumns="false" OnItemCommand="gvApproved2_ItemCommand"
                                                            OnItemDataBound="gvApproved2_ItemDataBound" AllowPaging="false" AllowSorting="false" AllowFilteringByColumn="false"
                                                            PageSize="5">
                                                            <GroupingSettings CaseSensitive="false" />
                                                            <ClientSettings>
                                                                <Selecting AllowRowSelect="true"></Selecting>
                                                                <Scrolling AllowScroll="True" UseStaticHeaders="True" ScrollHeight="300px" SaveScrollPosition="true" FrozenColumnsCount="3"></Scrolling>
                                                            </ClientSettings>
                                                            <MasterTableView CommandItemSettings-ShowRefreshButton="false" EditMode="InPlace" CommandItemDisplay="None"
                                                                AllowFilteringByColumn="false" AllowPaging="false" DataKeyNames="Id,OpenAdvance,SubVerticalId" AllowSorting="false" NoMasterRecordsText="" TableLayout="Fixed" PageSize="9">

                                                                <CommandItemSettings ShowRefreshButton="false" ShowExportToExcelButton="false" />
                                                                <NoRecordsTemplate>
                                                                    <table width="100%" border="0" cellpadding="20" cellspacing="20">
                                                                        <tr>
                                                                            <td align="center" class="txt-white">No records to display.
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </NoRecordsTemplate>
                                                                <Columns>
                                                                    <%-- <telerik:GridClientSelectColumn HeaderText="Select" HeaderStyle-Width="80px" UniqueName="RequestSelect">
                                                    <ItemStyle Height="85px" />
                                                </telerik:GridClientSelectColumn>--%>
                                                                      <telerik:GridBoundColumn HeaderText="Status" HeaderStyle-Width="150px" UniqueName="Status" DataField="PendingStatus">
                                                                    </telerik:GridBoundColumn>
                                                                     <telerik:GridBoundColumn HeaderText="DPR Number" HeaderStyle-Width="100px" UniqueName="DPRNumber" DataField="DPRNumber">
                                                                        <ItemStyle Height="85px" />
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridBoundColumn HeaderText="Document Number" HeaderStyle-Width="120px" UniqueName="DocumentNumber" DataField="DPRNumber">
                                                                        <ItemStyle Height="85px" />
                                                                    </telerik:GridBoundColumn>
                                                                          <telerik:GridBoundColumn HeaderText="Reference" HeaderStyle-Width="120px" UniqueName="Reference" DataField="Reference">
                                                                        <ItemStyle Height="85px" />
                                                                    </telerik:GridBoundColumn>
                                                                     <telerik:GridBoundColumn HeaderStyle-Width="150px"   HeaderText="Gross Amount" UniqueName="GrossAmount" DataField="GrossAmount" DataFormatString="{0:###,##0.00}">
                                                </telerik:GridBoundColumn>
                                                                      <telerik:GridBoundColumn HeaderStyle-Width="150px"  HeaderText="Tax Rate" UniqueName="TaxRate" DataField="TaxRate" DataFormatString="{0:###,##0.00}">
                                                                        </telerik:GridBoundColumn>
                                                                        <telerik:GridBoundColumn HeaderStyle-Width="150px"  HeaderText="Tax Amount" UniqueName="TaxAmount" DataField="TaxAmount" DataFormatString="{0:###,##0.00}">
                                                                        </telerik:GridBoundColumn>
                                                                    <telerik:GridBoundColumn HeaderText="Amount in INR" HeaderStyle-Width="120px" UniqueName="Amount" DataField="Amount" DataFormatString="{0:###,##0.00}">
                                                                        <ItemStyle Height="85px" />
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridBoundColumn HeaderText="Amount Proposed" HeaderStyle-Width="120px" UniqueName="AmountProposed" DataField="AmountProposed" DataFormatString="{0:###,##0.00}">
                                                                        <ItemStyle Height="85px" />
                                                                    </telerik:GridBoundColumn>
                                                                     <telerik:GridTemplateColumn HeaderText="Approved Amount" HeaderStyle-Width="150px" UniqueName="ApprovedAmount" DataField="ApprovedAmount">
                                                                        <ItemStyle Height="85px" />
                                                                        <ItemTemplate>
                                                                            <%--  <telerik:RadTextBox ID="tbApprovedAmount" runat="server"></telerik:RadTextBox>--%>
                                                                            <telerik:RadNumericTextBox ID="tbApprovedAmount" Width="125px" class="form-control" AutoPostBack="true"
                                                                                Text='<%#String.Format("{0:###,##0.00}",Eval("ApprovedAmount")) %>' Enabled="false" OnTextChanged="tbApprovedAmount_TextChanged" runat="server">
                                                                            </telerik:RadNumericTextBox>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                     <telerik:GridTemplateColumn HeaderText="Approval" HeaderStyle-Width="150px" UniqueName="Approval" DataField="Approval" Visible="false">
                                                                        <ItemTemplate>
                                                                            <telerik:RadDropDownList Enabled="false" RenderMode="Lightweight" ID="cmbApproval" runat="server">
                                                                                <Items>
                                                                                    <telerik:DropDownListItem Value="Approved" Text="Approved" />
                                                                                    <telerik:DropDownListItem Value="Reject" Text="Reject" />
                                                                                    <telerik:DropDownListItem Value="Need Correction" Text="Need Correction" />
                                                                                </Items>
                                                                            </telerik:RadDropDownList>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                  

                                                                    <telerik:GridTemplateColumn HeaderText="Select Stage" HeaderStyle-Width="120px" UniqueName="Stage" DataField="Stage">
                                                                        <ItemTemplate>
                                                                            <telerik:RadDropDownList Enabled="false" RenderMode="Lightweight" ID="cmbStage" runat="server">
                                                                                <Items>
                                                                                    <telerik:DropDownListItem Value="Initiator" Text="Initiator" />
                                                                                    <telerik:DropDownListItem Value="Aggregator" Text="Aggregator" />
                                                                                    <telerik:DropDownListItem Value="Controller" Text="Controller" />
                                                                                </Items>
                                                                            </telerik:RadDropDownList>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>


                                                                     <telerik:GridBoundColumn HeaderText="Document Currency" HeaderStyle-Width="100px" UniqueName="DocumentCurrency" DataField="Currency">
                                                                        <ItemStyle Height="85px" />
                                                                    </telerik:GridBoundColumn>
                                                                        <telerik:GridBoundColumn HeaderText="Un-settled Open Advance (INR)" HeaderStyle-Width="120px" UniqueName="UnsettledOpenAdvance" DataField="UnsettledOpenAdvance">
                                                                        <ItemStyle Height="85px" />
                                                                    </telerik:GridBoundColumn>
                                                                          <telerik:GridBoundColumn HeaderText="Withholding Tax Code" HeaderStyle-Width="150px" UniqueName="WithholdingTaxCode" DataField="WithholdingTaxCode">
                                                                        <ItemStyle Height="85px" />
                                                                    </telerik:GridBoundColumn>
                                                                     <telerik:GridDateTimeColumn HeaderStyle-Width="150px" HeaderText="Net Due Date" UniqueName="NetDueDate" DataField="NetDuedate" DataFormatString="{0:dd-MM-yyyy}">
                                                                    </telerik:GridDateTimeColumn>
                                                                        <telerik:GridBoundColumn HeaderText="Nature of Request" HeaderStyle-Width="150px" UniqueName="NatureOfRequest" DataField="Natureofrequest">
                                                                        <ItemStyle Height="85px" />
                                                                    </telerik:GridBoundColumn> 
                                                                    <telerik:GridBoundColumn HeaderText="Justification for Adv Payment" HeaderStyle-Width="150px" UniqueName="JustificationforAdvPayment" DataField="JustificationforAdvPayment">
                                                                        <ItemStyle Height="85px" />
                                                                    </telerik:GridBoundColumn>
                                                                           <telerik:GridTemplateColumn HeaderText="Attachments" AllowFiltering="false" HeaderStyle-Width="150px" AllowSorting="false" UniqueName="Attachments">
                                                                        <ItemStyle Height="85px" />
                                                                        <ItemTemplate>
                                                                            <telerik:RadButton ID="btnAttachment" runat="server" CommandName="Attachment" CommandArgument='<%# Eval("Id") %>' Text="Add/View" CssClass="gridHyperlinks"></telerik:RadButton>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridDateTimeColumn HeaderText="Posting Date" HeaderStyle-Width="150px" UniqueName="PostDate" DataField="PostingDate" DataFormatString="{0:dd-MM-yyyy}">
                                                                        <ItemStyle Height="85px" />
                                                                    </telerik:GridDateTimeColumn>
                                                                               <telerik:GridDateTimeColumn HeaderText="Expected Clearing Date" HeaderStyle-Width="150px" UniqueName="ExpectedClearingDate" DataField="ExpectedClearingDate" DataFormatString="{0:dd-MM-yyyy}">
                                                                        <ItemStyle Height="85px" />
                                                                    </telerik:GridDateTimeColumn>
                                                                      <telerik:GridBoundColumn HeaderText="Fiscal Year" HeaderStyle-Width="100px" UniqueName="FiscalYear" DataField="FiscalYear">
                                                                        <ItemStyle Height="85px" />
                                                                    </telerik:GridBoundColumn>
                                                                     <telerik:GridBoundColumn HeaderText="Purchasing Document" HeaderStyle-Width="100px" UniqueName="PurchasingDocument" DataField="PurchasingDocument">
                                                                        <ItemStyle Height="85px" />
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridBoundColumn HeaderText="Special GL" HeaderStyle-Width="120px" UniqueName="SpecialGL" DataField="SpecialGL">
                                                                        <ItemStyle Height="85px" />
                                                                    </telerik:GridBoundColumn>

                                                                       <telerik:GridBoundColumn HeaderText="Profit Centre" HeaderStyle-Width="120px" UniqueName="ProfitCentre" DataField="ProfitCentre">
                                                                        <ItemStyle Height="85px" />
                                                                    </telerik:GridBoundColumn>
                                                                      <telerik:GridBoundColumn HeaderText="Business Area" HeaderStyle-Width="120px" UniqueName="BusinessArea" DataField="BusinessArea">
                                                                        <ItemStyle Height="85px" />
                                                                    </telerik:GridBoundColumn>


                                                                    <telerik:GridBoundColumn HeaderText="Sub Vertical" HeaderStyle-Width="150px" UniqueName="SubVertical" DataField="SubVertical">
                                                                        <ItemStyle Height="85px" />
                                                                    </telerik:GridBoundColumn>

                                                                    <telerik:GridBoundColumn HeaderText="Payment Method" HeaderStyle-Width="80px" UniqueName="PaymentMethod" DataField="PaymentMethod">
                                                                        <ItemStyle Height="85px" />
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridBoundColumn HeaderText="Cheque Lot Number" HeaderStyle-Width="80px" UniqueName="ChecqueLotNumber" DataField="ChequeLotNumber">
                                                                        <ItemStyle Height="85px" />
                                                                    </telerik:GridBoundColumn>
                                                                 
                                                                    <%--  <telerik:GridBoundColumn HeaderText="Vertical" UniqueName="Vertical" DataField="Vertical">
                                            </telerik:GridBoundColumn>--%>
                                                                    <%-- <telerik:GridBoundColumn HeaderText="Status" UniqueName="Status" DataField="Status">
                                            </telerik:GridBoundColumn>--%>
                                                                    <%--<telerik:GridBoundColumn HeaderText="View Comments" UniqueName="ViewComments" DataField="Remark">
                                            </telerik:GridBoundColumn>--%>   
                                                                        <telerik:GridDateTimeColumn HeaderText="Submission Date" HeaderStyle-Width="150px" UniqueName="SubmissionDate" EnableTimeIndependentFiltering="true" DataField="SubmissionDate" DataFormatString="{0:dd-MM-yyyy}">
                                                                    </telerik:GridDateTimeColumn>                                                                 
                                                                    <telerik:GridTemplateColumn UniqueName="TemplateComment" HeaderStyle-Width="100px" HeaderText="View History">
                                                                        <ItemStyle Height="85px" />
                                                                        <ItemTemplate>
                                                                            <asp:HyperLink ID="viewComment" runat="server" HeaderStyle-Width="100px" Text="View" CssClass="btn btn-grey gridHyperlinks"></asp:HyperLink>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <%-- <telerik:GridTemplateColumn HeaderText="Remarks" UniqueName="Remark" DataField="Remark">
                                                <ItemTemplate>
                                                    <telerik:RadTextBox Enabled="false" ID="tbRemark" CommandArgument='<%# Eval("Remark") %>' TextMode="MultiLine" runat="server"></telerik:RadTextBox>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>--%>
                                                             

                                                                    <%--                                                               <telerik:GridTemplateColumn HeaderText="Add Attachment" UniqueName="AddAttachment">
                                                <ItemTemplate>
                                                    <asp:HyperLink ID="addAttachment" runat="server" Text="Browse" CssClass="btn btn-grey gridHyperlinks"></asp:HyperLink>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>--%>
                                                                   
                                                                    <telerik:GridBoundColumn HeaderText="OpenAdvance" HeaderStyle-Width="150px" UniqueName="OpenAdvance" Visible="false" DataField="OpenAdvance">
                                                                    </telerik:GridBoundColumn>
                                                                
                                                                    <%--   <telerik:GridTemplateColumn UniqueName="BudgetTreasuryAllocation" HeaderText="Budget Treasury Allocation">
                                                <ItemTemplate>
                                                    <asp:HyperLink ID="viewTreasuryApplocation" runat="server" Text="View" CssClass="btn btn-grey gridHyperlinks"></asp:HyperLink>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>--%>
                                                                </Columns>
                                                                <PagerStyle PageSizes="5,10,15" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                                                                    PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />
                                                            </MasterTableView>
                                                        </telerik:RadGrid>
                                                        <%-- <div class="col-lg-12 padding-0 margin-10">
                                    <asp:LinkButton CssClass="btn btn-grey" OnClick="lnkSave_Click" runat="server" ID="lnkSave">Save</asp:LinkButton>
                                    <asp:LinkButton CssClass="btn btn-grey" OnClick="lnkSubmit_Click" runat="server" ID="lnkSubmit">Submit</asp:LinkButton>
                                    <a href="home_screen.html" class="btn btn-grey">Cancel</a>
                                </div>--%>
                                                    </div>
                                                </NestedViewTemplate>

                                                <Columns>
                                                    <%--  <telerik:GridClientSelectColumn HeaderText="Select" UniqueName="Select">
                                </telerik:GridClientSelectColumn>--%>
                                                    <telerik:GridBoundColumn HeaderText="Company" UniqueName="Company" DataField="Company">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn HeaderText="Total Amount" UniqueName="TotalAmount" DataField="TotalAmount" DataFormatString="{0:###,##0.00}">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn HeaderText="Proposed Total" UniqueName="ProposedTotal" DataField="ProposedTotal" DataFormatString="{0:###,##0.00}">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn HeaderText="Approved Total" UniqueName="ApprovedTotal" DataField="ApprovedTotal" DataFormatString="{0:###,##0.00}">
                                                        <%--         <ItemTemplate>
                                    <telerik:RadLabel ID="lbAmountApproved" Text='<%#Eval("ProposedTotal") %>' Width="125px" class="form-control" runat="server"></telerik:RadLabel>
                                </ItemTemplate>--%>
                                                    </telerik:GridBoundColumn>
                                                    <%-- <telerik:GridBoundColumn HeaderText="Today's Paid Total" UniqueName="TodayTotal" DataField="TodayTotal" DataFormatString="{0:###,##0.00}">
                            </telerik:GridBoundColumn>--%>
                                                </Columns>
                                                <PagerStyle PageSizes="5,10,15" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                                                    PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />
                                            </MasterTableView>
                                        </telerik:RadGrid>
                                    </NestedViewTemplate>
                                    <Columns>
                                        <%--     <telerik:GridClientSelectColumn HeaderText="Select" UniqueName="Select">
                    </telerik:GridClientSelectColumn>--%>
                                        <telerik:GridBoundColumn HeaderText="Vendor" UniqueName="Vendor" DataField="Vendor">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn HeaderText="Total Amount" UniqueName="TotalAmount" DataField="TotalAmount" DataFormatString="{0:###,##0.00}">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn HeaderText="Proposed Total" UniqueName="ProposedTotal" DataField="ProposedTotal" DataFormatString="{0:###,##0.00}">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn HeaderText="Approved Total" UniqueName="ApprovedTotal" DataField="ApprovedTotal" DataFormatString="{0:###,##0.00}">
                                        </telerik:GridBoundColumn>
                                        <%-- <telerik:GridBoundColumn HeaderText="Today's Paid Total" UniqueName="TodayTotal" DataField="TodayTotal" DataFormatString="{0:###,##0.00}">
                </telerik:GridBoundColumn>--%>
                                    </Columns>
                                    <PagerStyle PageSizes="5,10,15" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                                        PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />
                                </MasterTableView>
                            </telerik:RadGrid>

                        </telerik:RadPageView>
                        <telerik:RadPageView runat="server" ID="RadPageCorrection">

                            <telerik:RadGrid RenderMode="Lightweight" ID="gvCorrection" runat="server" AutoGenerateColumns="false"
                                AllowPaging="false" AllowSorting="false" AllowFilteringByColumn="false" PageSize="5"
                                OnNeedDataSource="gvCorrection_NeedDataSource" OnItemCommand="gvCorrection_ItemCommand">

                                <GroupingSettings CaseSensitive="false" />
                                <ClientSettings>
                                    <Selecting AllowRowSelect="true"></Selecting>
                                    <Scrolling AllowScroll="True" UseStaticHeaders="True" ScrollHeight="400px" SaveScrollPosition="true" FrozenColumnsCount="4"></Scrolling>
                                </ClientSettings>
                                <MasterTableView CommandItemSettings-ShowRefreshButton="false" DataKeyNames="Vendor" EditMode="InPlace" ShowHeader="true" CommandItemDisplay="None"
                                    AllowFilteringByColumn="false" AllowSorting="false" AllowPaging="false" NoMasterRecordsText="" TableLayout="Fixed" PageSize="9">
                                    <CommandItemSettings ShowRefreshButton="false" ShowExportToExcelButton="false" />
                                    <NoRecordsTemplate>
                                        <table width="100%" border="0" cellpadding="20" cellspacing="20">
                                            <tr>
                                                <td align="center" class="txt-white">No records to display.
                                                </td>
                                            </tr>
                                        </table>
                                    </NoRecordsTemplate>
                                    <NestedViewTemplate>

                                        <telerik:RadGrid RenderMode="Lightweight" ID="gvCorrection1" runat="server" AutoGenerateColumns="false"
                                            AllowPaging="true" AllowSorting="false" AllowFilteringByColumn="false" PageSize="5"
                                            OnItemCommand="gvCorrection1_ItemCommand">
                                            <GroupingSettings CaseSensitive="false" />
                                            <ClientSettings>
                                                <Selecting AllowRowSelect="true"></Selecting>
                                                <Scrolling AllowScroll="True" UseStaticHeaders="True" ScrollHeight="328px" SaveScrollPosition="true" FrozenColumnsCount="4"></Scrolling>
                                                
                                            </ClientSettings>
                                            <MasterTableView CommandItemSettings-ShowRefreshButton="false" EditMode="InPlace" ShowHeader="true" CommandItemDisplay="None"
                                                AllowFilteringByColumn="false" DataKeyNames="Company" AllowPaging="false" AllowSorting="false" NoMasterRecordsText="" TableLayout="Fixed" PageSize="9">
                                                <CommandItemSettings ShowRefreshButton="false" ShowExportToExcelButton="false" />
                                                <NoRecordsTemplate>
                                                    <table width="100%" border="0" cellpadding="20" cellspacing="20">
                                                        <tr>
                                                            <td align="center" class="txt-white">No records to display.
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </NoRecordsTemplate>
                                                <NestedViewTemplate>
                                                    <div class="table-scroll">
                                                        <telerik:RadGrid RenderMode="Lightweight" ID="gvCorrection2" runat="server" AutoGenerateColumns="false" OnItemCommand="gvCorrection2_ItemCommand"
                                                            OnItemDataBound="gvCorrection2_ItemDataBound" AllowPaging="false" AllowSorting="false" AllowFilteringByColumn="false" AllowMultiRowSelection="true"
                                                            PageSize="5">
                                                            <GroupingSettings CaseSensitive="false" />
                                                            <ClientSettings>
                                                                <Selecting AllowRowSelect="true"></Selecting>
                                                                <Scrolling AllowScroll="True" UseStaticHeaders="True" ScrollHeight="155px" SaveScrollPosition="true" FrozenColumnsCount="4"></Scrolling>
                                                                 <ClientEvents OnRowSelected="RowSelectedApproverGrid" OnRowDeselected="RowSelectedApproverGrid" />

                                                            </ClientSettings>
                                                            <MasterTableView CommandItemSettings-ShowRefreshButton="false" EditMode="InPlace" CommandItemDisplay="None"
                                                                AllowFilteringByColumn="false" AllowPaging="false" DataKeyNames="Id,OpenAdvance,SubVerticalId" AllowSorting="false" NoMasterRecordsText="" TableLayout="Fixed" PageSize="9">
                                                                <CommandItemSettings ShowRefreshButton="false" ShowExportToExcelButton="false" />
                                                                <NoRecordsTemplate>
                                                                    <table width="100%" border="0" cellpadding="20" cellspacing="20">
                                                                        <tr>
                                                                            <td align="center" class="txt-white">No records to display.
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </NoRecordsTemplate>
                                                                <Columns>
                                                                    <telerik:GridClientSelectColumn HeaderStyle-Width="80px" HeaderText="Select" UniqueName="RequestSelect">
                                                                    </telerik:GridClientSelectColumn>
                                                                       <telerik:GridBoundColumn HeaderStyle-Width="100px" HeaderText="DPR Number" UniqueName="DPRNumber" DataField="DPRNumber">
                                                                    </telerik:GridBoundColumn>
                                                                        <telerik:GridBoundColumn HeaderStyle-Width="120px" HeaderText="Document Number" UniqueName="DocumentNumber" DataField="DPRNumber">
                                                                    </telerik:GridBoundColumn>
                                                                       <telerik:GridBoundColumn HeaderStyle-Width="120px" HeaderText="Reference" UniqueName="Reference" DataField="Reference">
                                                                    </telerik:GridBoundColumn>
                                                                     <telerik:GridBoundColumn HeaderStyle-Width="150px"   HeaderText="Gross Amount" UniqueName="GrossAmount" DataField="GrossAmount" DataFormatString="{0:###,##0.00}">
                                                </telerik:GridBoundColumn>
                                                                          <telerik:GridBoundColumn HeaderStyle-Width="150px"  HeaderText="Tax Rate" UniqueName="TaxRate" DataField="TaxRate" DataFormatString="{0:###,##0.00}">
                                                                        </telerik:GridBoundColumn>
                                                                        <telerik:GridBoundColumn HeaderStyle-Width="150px"  HeaderText="Tax Amount" UniqueName="TaxAmount" DataField="TaxAmount" DataFormatString="{0:###,##0.00}">
                                                                        </telerik:GridBoundColumn>

                                                                    <telerik:GridBoundColumn HeaderStyle-Width="120px" HeaderText="Amount in INR" UniqueName="Amount" DataField="Amount" DataFormatString="{0:###,##0.00}">
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridBoundColumn HeaderStyle-Width="120px" HeaderText="Amount Proposed" UniqueName="AmountProposed" DataField="AmountProposed" DataFormatString="{0:###,##0.00}">
                                                                    </telerik:GridBoundColumn>
                                                                     <telerik:GridTemplateColumn HeaderStyle-Width="150px" HeaderText="Approved Amount" UniqueName="ApprovedAmount" DataField="ApprovedAmount">
                                                                        <ItemTemplate>
                                                                            <%--  <telerik:RadTextBox ID="tbApprovedAmount" runat="server"></telerik:RadTextBox>--%>
                                                                            <telerik:RadNumericTextBox ID="tbApprovedAmount" Width="125px" class="form-control" AutoPostBack="true"
                                                                                Text='<%# String.Format("{0:###,##0.00}",Eval("ApprovedAmount")) %>' OnTextChanged="tbApprovedAmount_TextChanged" runat="server">
                                                                            </telerik:RadNumericTextBox>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>                                                                     
                                                                      <telerik:GridBoundColumn  HeaderStyle-Width="100px" HeaderText="Debit/Credit" UniqueName="DCFLag" DataField="DCFLag" >
                                                                </telerik:GridBoundColumn> 
                                                                     <telerik:GridTemplateColumn HeaderStyle-Width="120px" HeaderText="Approval" UniqueName="Approval" DataField="Approval">
                                                                        <ItemTemplate>
                                                                            <telerik:RadDropDownList RenderMode="Lightweight" ID="cmbApproval" OnSelectedIndexChanged="cmbApproval1_SelectedIndexChanged" AutoPostBack="true" runat="server">
                                                                                <Items>
                                                                                    <telerik:DropDownListItem Value="Approved" Text="Approved" />
                                                                                    <telerik:DropDownListItem Value="Reject" Text="Reject" />
                                                                                    <telerik:DropDownListItem Value="Need Correction" Text="Need Correction" />
                                                                                </Items>
                                                                            </telerik:RadDropDownList>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridTemplateColumn HeaderStyle-Width="120px" HeaderText="Select Stage" UniqueName="Stage" DataField="Stage">
                                                                        <ItemTemplate>
                                                                            <telerik:RadDropDownList Enabled="false" RenderMode="Lightweight" ID="cmbStage" runat="server">
                                                                                <Items>
                                                                                    <telerik:DropDownListItem Value="Initiator" Text="Initiator" />
                                                                                    <telerik:DropDownListItem Value="Aggregator" Text="Aggregator" />
                                                                                    <telerik:DropDownListItem Value="Controller" Text="Controller" />
                                                                                </Items>
                                                                            </telerik:RadDropDownList>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                     <telerik:GridBoundColumn HeaderStyle-Width="100px" HeaderText="Document Currency" UniqueName="DocumentCurrency" DataField="Currency">
                                                                    </telerik:GridBoundColumn>
                                                                       <telerik:GridDateTimeColumn HeaderStyle-Width="150px" HeaderText="Net Due Date" UniqueName="NetDueDate" DataField="NetDuedate" DataFormatString="{0:dd-MM-yyyy}">
                                                                    </telerik:GridDateTimeColumn>
                                                                      <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="Un-settled Open Advance (INR)" UniqueName="UnsettledOpenAdvance" DataField="UnsettledOpenAdvance">
                                                                    </telerik:GridBoundColumn>
                                                                     <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="Withholding Tax Code" UniqueName="WithholdingTaxCode" DataField="WithholdingTaxCode">
                                                                    </telerik:GridBoundColumn>
                                                                     <telerik:GridBoundColumn HeaderStyle-Width="200px" HeaderText="Nature of Request" UniqueName="NatureOfRequest" DataField="Natureofrequest">
                                                                    </telerik:GridBoundColumn> 
                                                                     <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="Justification for Adv Payment" UniqueName="JustificationforAdvPayment" DataField="JustificationforAdvPayment">
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridTemplateColumn HeaderStyle-Width="200px" HeaderText="Remarks" UniqueName="Remark" DataField="Remark">
                                                                    <ItemTemplate>
                                                                    <telerik:RadTextBox ID="tbRemark" Text='<%# Eval("Remark") %>' TextMode="MultiLine" runat="server"></telerik:RadTextBox>
                                                                    </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn HeaderStyle-Width="150px" HeaderText="Attachments" AllowFiltering="false" AllowSorting="false" UniqueName="Attachments">
                                                                    <ItemTemplate>
                                                                    <telerik:RadButton ID="btnAttachment" runat="server" CommandName="Attachment" CommandArgument='<%# Eval("Id") %>' Text="Add/View" CssClass=" gridHyperlinks"></telerik:RadButton>
                                                                    </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                     <telerik:GridDateTimeColumn HeaderStyle-Width="150px" HeaderText="Posting Date" UniqueName="PostDate" DataField="PostingDate" DataFormatString="{0:dd-MM-yyyy}">
                                                                    </telerik:GridDateTimeColumn>
                                                                     <telerik:GridDateTimeColumn HeaderStyle-Width="150px" HeaderText="Expected Clearing Date" UniqueName="ExpectedClearingDate" DataField="ExpectedClearingDate" DataFormatString="{0:dd-MM-yyyy}">
                                                                    </telerik:GridDateTimeColumn>
                                                                     <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="Fiscal Year" UniqueName="FiscalYear" DataField="FiscalYear">
                                                                    </telerik:GridBoundColumn>
                                                                       <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="Purchasing Document" UniqueName="PurchasingDocument" DataField="PurchasingDocument">
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridBoundColumn HeaderStyle-Width="120px" HeaderText="Special GL" UniqueName="SpecialGL" DataField="SpecialGL">
                                                                    </telerik:GridBoundColumn>
                                                                     <telerik:GridBoundColumn HeaderStyle-Width="120px" HeaderText="Profit Centre" UniqueName="ProfitCentre" DataField="ProfitCentre">
                                                                    </telerik:GridBoundColumn>     
                                                                     <telerik:GridBoundColumn HeaderStyle-Width="120px" HeaderText="Business Area" UniqueName="BusinessArea" DataField="BusinessArea">
                                                                    </telerik:GridBoundColumn>


                                                                    <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="Assigned By" UniqueName="AssignedBy" DataField="AssignedBy">
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="Sub Vertical" UniqueName="SubVertical" DataField="SubVertical">
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridBoundColumn HeaderStyle-Width="80px" HeaderText="Payment Method" UniqueName="PaymentMethod" DataField="PaymentMethod">
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridBoundColumn HeaderStyle-Width="80px" HeaderText="Cheque Lot Number" UniqueName="ChecqueLotNumber" DataField="ChequeLotNumber">
                                                                    </telerik:GridBoundColumn>                                                                  
                                                                  
                                                                    <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="OpenAdvance" UniqueName="OpenAdvance" Visible="false" DataField="OpenAdvance">
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridDateTimeColumn HeaderStyle-Width="150px" HeaderText="Submission Date" UniqueName="SubmissionDate" EnableTimeIndependentFiltering="true" DataField="SubmissionDate" DataFormatString="{0:dd-MM-yyyy}">
                                                                    </telerik:GridDateTimeColumn>
                                                                      <telerik:GridTemplateColumn HeaderStyle-Width="100px" UniqueName="TemplateComment" HeaderText="View History">
                                                                        <ItemTemplate>
                                                                            <asp:HyperLink ID="viewComment" runat="server" Text="View" CssClass="btn btn-grey gridHyperlinks"></asp:HyperLink>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <%--  <telerik:GridTemplateColumn UniqueName="BudgetTreasuryAllocation" HeaderText="Budget Treasury Allocation">
                                                <ItemTemplate>
                                                    <asp:HyperLink ID="viewTreasuryApplocation" runat="server" Text="View" CssClass="btn btn-grey gridHyperlinks"></asp:HyperLink>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>--%>
                                                                    

                                                                </Columns>
                                                                <PagerStyle PageSizes="5,10,15" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                                                                    PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />
                                                            </MasterTableView>
                                                        </telerik:RadGrid>
                                                        <div class="col-lg-12 padding-0 margin-10">
                                                            <asp:LinkButton CssClass="btn btn-grey" OnClick="lnkSave_Click" runat="server" ID="lnkSave1">Save</asp:LinkButton>
                                                            <asp:LinkButton CssClass="btn btn-grey" OnClientClick="return confirm('Are you sure you want proceed?');" OnClick="lnkSubmit_Click" runat="server" ID="lnkSubmit1">Submit</asp:LinkButton>
                                                            <%--                                                        <asp:LinkButton CssClass="btn btn-grey" OnClick="lnkCancel_Click" runat="server" ID="lnkCancel1">Cancel</asp:LinkButton>
                                    <a href="home_screen.html" class="btn btn-grey">Cancel</a>--%>
                                                        </div>
                                                    </div>
                                                </NestedViewTemplate>

                                                <Columns>
                                                    <telerik:GridClientSelectColumn HeaderText="Select" UniqueName="Select">
                                                    </telerik:GridClientSelectColumn>
                                                    <telerik:GridBoundColumn HeaderText="Company" UniqueName="Company" DataField="Company">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn HeaderText="Total Amount" UniqueName="TotalAmount" DataField="TotalAmount" DataFormatString="{0:###,##0.00}">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn HeaderText="Proposed Total" UniqueName="ProposedTotal" DataField="ProposedTotal" DataFormatString="{0:###,##0.00}">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn HeaderText="Approved Total" UniqueName="ApprovedTotal" DataField="ApprovedTotal" DataFormatString="{0:###,##0.00}">
                                                        <%--         <ItemTemplate>
                                    <telerik:RadLabel ID="lbAmountApproved" Text='<%#Eval("ProposedTotal") %>' Width="125px" class="form-control" runat="server"></telerik:RadLabel>
                                </ItemTemplate>--%>
                                                    </telerik:GridBoundColumn>
                                                    <%-- <telerik:GridBoundColumn HeaderText="Today's Paid Total" UniqueName="TodayTotal" DataField="TodayTotal" DataFormatString="{0:###,##0.00}">
                            </telerik:GridBoundColumn>--%>
                                                </Columns>
                                                <PagerStyle PageSizes="5,10,15" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                                                    PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />
                                            </MasterTableView>
                                        </telerik:RadGrid>
                                    </NestedViewTemplate>
                                    <Columns>
                                        <telerik:GridClientSelectColumn HeaderText="Select" UniqueName="Select">
                                        </telerik:GridClientSelectColumn>
                                        <telerik:GridBoundColumn HeaderText="Vendor" UniqueName="Vendor" DataField="Vendor">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn HeaderText="Total Amount" UniqueName="TotalAmount" DataField="TotalAmount" DataFormatString="{0:###,##0.00}">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn HeaderText="Proposed Total" UniqueName="ProposedTotal" DataField="ProposedTotal" DataFormatString="{0:###,##0.00}">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn HeaderText="Approved Total" UniqueName="ApprovedTotal" DataField="ApprovedTotal" DataFormatString="{0:###,##0.00}">
                                        </telerik:GridBoundColumn>
                                        <%--  <telerik:GridBoundColumn HeaderText="Today's Paid Total" UniqueName="TodayTotal" DataField="TodayTotal" DataFormatString="{0:###,##0.00}">
                </telerik:GridBoundColumn>--%>
                                    </Columns>
                                    <PagerStyle PageSizes="5,10,15" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                                        PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />
                                </MasterTableView>
                            </telerik:RadGrid>

                        </telerik:RadPageView>
                    </telerik:RadMultiPage>
                </div>
            </div>

        </div>

    </div>


    <div class="modal fade" id="squarespaceCommentModal" tabindex="-1" role="dialog" aria-labelledby="modalLabel" aria-hidden="true">
        <div class="modal-dialog1">
            <div class="modal-content">
                <div class="modal-header modal-header-resize">
                    <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">×</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title text-primary" id="lineModalCommentLabel">Comments</h4>
                </div>
                <div class="modal-body">
                    <telerik:RadGrid RenderMode="Lightweight" ID="grdComment" runat="server" OnNeedDataSource="grdComment_NeedDataSource"
                        AutoGenerateColumns="false" AllowPaging="true" AllowSorting="true" AllowFilteringByColumn="false" PageSize="10">
                        <GroupingSettings CaseSensitive="false" />
                        <MasterTableView CommandItemSettings-ShowRefreshButton="false" ClientDataKeyNames="" EditMode="InPlace" CommandItemDisplay="None" AllowFilteringByColumn="false" PageSize="9">
                            <NoRecordsTemplate>
                                <table width="100%" border="0" cellpadding="20" cellspacing="20">
                                    <tr>
                                        <td align="center" class="txt-white" class="txt-white">No records to display.
                                        </td>
                                    </tr>
                                </table>
                            </NoRecordsTemplate>
                            <CommandItemSettings ShowRefreshButton="false" ShowExportToExcelButton="false" />
                            <Columns>
                                <telerik:GridBoundColumn DataField="UserName" HeaderText="Name" UniqueName="Name" HeaderStyle-Width="150px">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="WorkFlowStatus" HeaderText="Status" UniqueName="WorkFlowStatus" HeaderStyle-Width="200px">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="AppovedAmount" HeaderText="Approved Amount" UniqueName="AppovedAmount" HeaderStyle-Width="100px" DataFormatString="{0:###,##0.00}">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Comment" HeaderText="Comment" UniqueName="Comment" HeaderStyle-Width="200px">
                                </telerik:GridBoundColumn>
                                <telerik:GridDateTimeColumn DataField="CreatedOn" HeaderText="CreatedOn" EnableTimeIndependentFiltering="true" DataFormatString="{0:dd-MM-yyyy}">
                                    <HeaderStyle Width="150px" />
                                </telerik:GridDateTimeColumn>
                            </Columns>
                            <PagerStyle PageSizes="5,10,15" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                                PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />
                        </MasterTableView>
                    </telerik:RadGrid>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="squarespaceModal" tabindex="-1" role="dialog" aria-labelledby="modalLabel" aria-hidden="true">
        <div class="modal-dialog1">
            <div class="modal-content">
                <div class="modal-header modal-header-resize">
                    <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">×</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title text-primary" id="lineModalCommentLabel">Comments</h4>
                </div>
                <div class="modal-body">
                    <telerik:RadGrid RenderMode="Lightweight" ID="RadGrid1" runat="server" OnNeedDataSource="grdComment_NeedDataSource"
                        AutoGenerateColumns="false" AllowPaging="true" AllowSorting="true" AllowFilteringByColumn="false" PageSize="10">
                        <GroupingSettings CaseSensitive="false" />
                        <MasterTableView CommandItemSettings-ShowRefreshButton="false" ClientDataKeyNames="" EditMode="InPlace" CommandItemDisplay="None" AllowFilteringByColumn="false" PageSize="9">
                            <NoRecordsTemplate>
                                <table width="100%" border="0" cellpadding="20" cellspacing="20">
                                    <tr>
                                        <td align="center" class="txt-white" class="txt-white">No records to display.
                                        </td>
                                    </tr>
                                </table>
                            </NoRecordsTemplate>
                            <CommandItemSettings ShowRefreshButton="false" ShowExportToExcelButton="false" />
                            <Columns>
                                <telerik:GridBoundColumn DataField="UserName" HeaderText="Name" UniqueName="Name" HeaderStyle-Width="30%">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="WorkFlowStatus" HeaderText="Status" UniqueName="WorkFlowStatus" HeaderStyle-Width="30%">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="AppovedAmount" HeaderText="Approved Amount" UniqueName="AppovedAmount" HeaderStyle-Width="70%" DataFormatString="{0:###,##0.00}">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Comment" HeaderText="Comment" UniqueName="Comment" HeaderStyle-Width="70%">
                                </telerik:GridBoundColumn>
                                <telerik:GridDateTimeColumn DataField="CreatedOn" HeaderText="CreatedOn" EnableTimeIndependentFiltering="true" DataFormatString="{0:dd-MM-yyyy}">
                                    <HeaderStyle Width="130px" />
                                </telerik:GridDateTimeColumn>
                            </Columns>
                            <PagerStyle PageSizes="5,10,15" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                                PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />
                        </MasterTableView>
                    </telerik:RadGrid>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="divViewAllocation" tabindex="-1" role="dialog" aria-labelledby="modalLabel" aria-hidden="true">
        <div class="modal-dialog1">
            <div class="modal-content">
                <div class="modal-header modal-header-resize">
                    <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">×</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title text-primary" id="lineModalLabel">Treasury Allocation</h4>
                </div>


                <%--<telerik:RadAjaxPanel runat="server" ID="AllocationPanel">--%>
                <asp:Panel ID="pnl1" runat="server">
                    <div style="height: 500px; overflow: auto;">
                        <telerik:RadGrid RenderMode="Lightweight" ID="grdBudgetAllocation" ClientIDMode="AutoID" runat="server" OnNeedDataSource="grdAttachments_NeedDataSource" OnItemDataBound="grdBudgetAllocation_ItemDataBound"
                            AutoGenerateColumns="false" EnableViewState="true" ShowFooter="True"
                            AllowPaging="false" AllowSorting="true" AllowFilteringByColumn="true">
                            <GroupingSettings CaseSensitive="false" />
                            <MasterTableView CommandItemDisplay="None" DataKeyNames="BalanceAmount,SubVerticalId,TreasuryDetailId,IsExpired" ClientDataKeyNames="BalanceAmount,SubVertical" EnableViewState="true" AllowPaging="false" AutoGenerateColumns="false" PageSize="9"
                                EnableNoRecordsTemplate="true" AllowFilteringByColumn="false" ClientIDMode="AutoID">
                                <NoRecordsTemplate>
                                    <table width="100%" border="0" cellpadding="20" cellspacing="20">
                                        <tr>
                                            <td align="center" class="txt-white">No records to display.
                                            </td>
                                        </tr>
                                    </table>
                                </NoRecordsTemplate>
                                <CommandItemSettings ShowRefreshButton="false" />
                                <Columns>
                                    <telerik:GridBoundColumn DataField="AllocationNumber" HeaderText="Budget Allocation No." UniqueName="AllocationNumber" DataType="System.String" ReadOnly="true" />
                                    <telerik:GridBoundColumn DataField="SubVertical" HeaderText="Sub Vertical" UniqueName="SubVertical" DataType="System.String" ReadOnly="true" />
                                    <telerik:GridBoundColumn DataField="ValidationDate" DataFormatString="{0:dd-MM-yyyy}" HeaderText="Valid Upto" UniqueName="ValidationDate" DataType="System.String" ReadOnly="true" />
                                    <telerik:GridBoundColumn DataField="TotalAmount" HeaderText="Total Amount" UniqueName="TotalAmount" DataType="System.String" ReadOnly="true" DataFormatString="{0:###,##0.00}" />
                                    <telerik:GridBoundColumn DataField="Utilised" HeaderText="Utilised Amount" UniqueName="Utilised" DataType="System.String" ReadOnly="true" DataFormatString="{0:###,##0.00}" />
                                    <telerik:GridBoundColumn DataField="ToBeUtilised" FooterStyle-Width="20%" FooterStyle-HorizontalAlign="Right" FooterStyle-Font-Bold="true" HeaderText="To-Be Utilised Amount" UniqueName="ToBeUtilised" DataType="System.String" ReadOnly="true" DataFormatString="{0:###,##0.00}" />
                                    <telerik:GridTemplateColumn FooterStyle-Width="20%" UniqueName="TemplateBalanceAmount" HeaderText="Balance Amount" HeaderStyle-Width="5%" SortExpression="BalanceAmount" DataField="BalanceAmount">
                                        <ItemTemplate>
                                            <telerik:RadLabel runat="server" ID="lblBalanceAmount" Text='<%# String.Format("{0:###,##0.00}",Eval("BalanceAmount")) %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="TemplateAttachment" HeaderText="Current Allocation" HeaderStyle-Width="5%">
                                        <ItemTemplate>
                                            <telerik:RadNumericTextBox runat="server" ID="numCurrentAllocation" AutoPostBack="false"
                                                ClientEvents-OnValueChanged='<%# "function (s,a){ValidateRow(s,a,"+Container.ItemIndex+","+ Eval("BalanceAmount") +");}" %>'>
                                            </telerik:RadNumericTextBox>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>
                                <%-- <PagerStyle PageSizes="5" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
            PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />--%>
                            </MasterTableView>
                        </telerik:RadGrid>
                    </div>
                </asp:Panel>
                <%--</telerik:RadAjaxPanel>--%>
&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Label runat="server" ID="Label1" Text="Total Balance :"></asp:Label>
                <asp:Label runat="server" ID="lbltoalAmount"></asp:Label>

                &nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Label runat="server" ID="Label2" Text="Current Allocation:"></asp:Label>
                <asp:Label runat="server" ID="lbltoalAllocation" Text="0"></asp:Label>

                <br>
                &nbsp;&nbsp;&nbsp;&nbsp;<asp:Label runat="server" ID="lblError"></asp:Label>
                <div class="modal-body">
                    <asp:Button runat="server" Text="Save" OnClick="btnSave_Click" ID="btnSave" OnClientClick="return ValidateForm();"></asp:Button>
                    <asp:Button runat="server" Text="Close" OnClientClick="ClosePopup();" ID="btnClose"></asp:Button>
                </div>
            </div>
        </div>
    </div>
    


    <div class="modal fade" id="verticalBudget-modal" tabindex="-1" role="dialog" aria-labelledby="modalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">×</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title subheading margin-0" id="lineModalLabel">Vertical Budget</h4>
                </div>
                 <%--<asp:Panel ID="pnl2" runat="server">--%>
                <div style="height: 500px; overflow: auto;">
                    <div class="col-xs-12 margin-10">
                        <telerik:RadGrid RenderMode="Lightweight" ID="grdVerticalBudget" runat="server"
                            AutoGenerateColumns="false" Width="100%" OnNeedDataSource="grdVerticalBudget_NeedDataSource"
                            AllowPaging="false" AllowSorting="false" AllowFilteringByColumn="false" CssClass="grid-border">
                            <GroupingSettings CaseSensitive="false" />
                            <ClientSettings>
                                <Selecting AllowRowSelect="false"></Selecting>
                            </ClientSettings>
                            <MasterTableView CommandItemSettings-ShowRefreshButton="false" EnableNoRecordsTemplate="true" EditMode="InPlace" CommandItemDisplay="None"
                                AllowFilteringByColumn="false" AllowSorting="false" NoMasterRecordsText="" Width="100%" PageSize="9">
                                <CommandItemSettings ShowRefreshButton="false" ShowExportToExcelButton="false" />
                                <NoRecordsTemplate>
                                    <table width="100%" border="0" cellpadding="20" cellspacing="20">
                                        <tr>
                                            <td align="center" class="txt-white">No records to display.
                                            </td>
                                        </tr>
                                    </table>
                                </NoRecordsTemplate>
                                <Columns>
                                    <telerik:GridBoundColumn HeaderText="Sub Vertical" DataField="SubVertical" UniqueName="SubVertical">
                                        <HeaderStyle Width="60px" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn HeaderText="Allocation Number" DataField="AllocationNumber" UniqueName="AllocationNumber">
                                        <HeaderStyle Width="120px" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn HeaderText="Approved Amount" DataField="ApprovedAmount" UniqueName="ApprovedAmount">
                                        <HeaderStyle Width="120px" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn HeaderText="Expiry Date" DataField="ExpiryDate" UniqueName="ExpiryDate" DataFormatString="{0:dd/MM/yyyy}">
                                        <HeaderStyle Width="120px" />
                                    </telerik:GridBoundColumn>
                                </Columns>
                                <PagerStyle PageSizes="5,10,15" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                                    PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />
                            </MasterTableView>
                        </telerik:RadGrid>
                    </div>
                </div>
                 <%--</asp:Panel>--%>
            </div>
        </div>
    </div>
       <div class="modal fade" id="Refresh-modal" tabindex="-1" role="dialog" aria-labelledby="modalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">×</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title subheading margin-0" id="lineModalLabel123">Vertical Budget</h4>
                </div>
               
                <div style="height: 500px; overflow: auto;">
                    <div class="col-xs-12 margin-10">

                        dfdsf
                </div>
                </div>               
            </div>
        </div>
    </div>


</asp:Content>
