<%@ Page Title="" Language="C#" MasterPageFile="~/SuzlonBPP.Master" AutoEventWireup="true"
    MaintainScrollPositionOnPostback="true" CodeBehind="FnAApprover.aspx.cs" Inherits="SuzlonBPP.FnAApprover" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="Scripts/bootstrap.min.js"></script>

    <telerik:RadScriptBlock ID="radSript1" runat="server">
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

                for (var i = 0; i < selrec.length; i++) {
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


            function OnClientFileSelected(sender, args) {
                var currentFileName = args.get_fileName();
                if (currentFileName != "")
                    hideErrorMsg();
                else
                    showErrorMsg();
            }

            function hideErrorMsg() {
                $("#lblUploadError").hide();
            }

            function showErrorMsg() {
                $("#lblUploadError").show();
            }

            function saveConfirmation(btntext) {
                function confirmCallBack(arg) {
                    if (arg) {
                        $find("<%= RadAjaxManager1.ClientID %>").ajaxRequest("ProceedToSaveYes#" + btntext);
                    }
                    else {
                        eventargs.set_cancel(true);
                    }
                }
                //Telerik.Web.UI.RadWindowUtils.Localization =
                //{
                //    "OK": "Yes",
                //    "Cancel": "No"
                //};
                //radconfirm("Today's approved total limit is exceeded. Do you still want to go continue?", confirmCallBack, "500px", "150px", null, "Confirmation");

                Telerik.Web.UI.RadWindowUtils.Localization =
                              {
                                  "OK": "Send",
                                  "Cancel": "No"
                              };
                radconfirm("Today's approved total limit is exceeded. Adjust the selection of records or Send records to Auditor for approval.", confirmCallBack, "500px", "150px", null, "Confirmation");

            }

            function GetRadWindow() {
                var oWindow = null;
                if (window.radWindow)
                    oWindow = window.radWindow;
                else if (window.frameElement && window.frameElement.radWindow)
                    oWindow = window.frameElement.radWindow;
                return oWindow;
            }

            function CloseModal() {
                GetRadWindow().close();
            }

            function showNotification(title, text) {
                var notification = $find("<%= radMessage.ClientID %>");
                var message = title;
                notification.set_title(title);
                notification.set_text(text);
                notification.show();
            }

            function openTreasuryRadWin() {
             //   debugger;
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
                    $("#ContentPlaceHolder1_lbltoalAllocation").text("0");
                    $("#ContentPlaceHolder1_lbltoalAmount").text(amnt);
                    $("#ContentPlaceHolder1_lblError").text("");

                    var grid = $find("<%=grdBudgetAllocation.ClientID%>");
                    var dataItems = grid.get_masterTableView().get_dataItems();
                   
                for (i = 0; i < dataItems.length; i++) {
                    dataItems[i].findElement("numCurrentAllocation").setAttribute('value', '')
                }
              
                }, 500);
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
                  //  debugger;
                    var currentValue = JSON.parse($("#" + ClientId + "_ClientState").attr('value')).valueAsString;
                    alert(currentValue);
                }, 1000);
            }


            function ValidateForm() {
                debugger;
                if (parseFloat($("#ContentPlaceHolder1_lbltoalAmount").text()) != parseFloat($("#ContentPlaceHolder1_lbltoalAllocation").text())) {
                    //$("#ContentPlaceHolder1_lblError").text("Please Allocate The Total Amount.");
                    var $radNotify = $find("<%= radMessage.ClientID%>");
                    $radNotify.set_title("Alert");
                    $radNotify.set_text("Please Allocate The Total Amount.");
                    $radNotify.show();
                    return false;
                }
            }
            var flag = false;
            function ValidateRow(sender, args, index, curBalanceAmount) {


                debugger;
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
                ContentPlaceHolder1_lbltoalAllocation
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

            function ConfirmUser() {
                if (confirm('Are you sure you want proceed1?')) {
                    return true;
                } else {
                    return false;
                }

            }

            function OnKeyPressSerialText(sender, eventArgs) {
               
                var c = eventArgs.get_keyCode();                
                
                if ((c < 48) || (c > 40 && c < 45) || (c > 57 && c < 65) || (c > 90 && c < 97) || (c > 122))
                {
                    if (c == 8)
                        eventArgs.set_cancel(false);
                    else {
                        
                        eventArgs.set_cancel(true);
                    }
                }
                //debugger;
                //if (!(((keycode >= 48 && keycode <= 57) || keycode == 8 || keycode == 46) || (keycode >= 96 && keycode <= 105) || (keycode > 64 && keycode <= 91)))
                //    eventArgs.set_cancel(true);
                //debugger;
                //var char = eventArgs.get_keyCharacter();
                ////will allow just letters, numbers and "-" letter 
                //alert(char);
                //var exp = /[^a-zA-Z0-9-]/g;
                //if (exp.test(char)) {
                //    eventArgs.set_cancel(true);
                //}

              
               

            }

        </script>
    </telerik:RadScriptBlock>
    <style>
        .RadGrid_Frozen 
        {
            overflow-x: scroll !important;
        }
  
        [id$='Frozen'] {
            overflow:scroll !important;  
        }

           
        .linkButtonColor {
            color: #337ab7 !important;
        }

        .RadInput_Default .riTextBox, .RadInputMgr_Default {
            border-color: #cdcdcd;
            background: #fff;
            color: #333;
            width: 100%;
        }
      .RadPicker {
         
            display: inline-block !important;
            width: 159px !important;
        }




    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript">
            function ShowComments(id) {
                var ajaxManager = $find("<%= RadAjaxManager1.ClientID %>");
                ajaxManager.ajaxRequest("Comment#" + id + "");
                $('#squarespaceCommentModal').modal();
            }

            function openRadWin(screen, mode, entityId, canAdd, canDelete, isMultiFileUpload, showDocumentType, entityName) {
                debugger;
                var manager = $find("<%= RadWindowManager.ClientID %>");
                //if (screen == 'VC') {
                manager.open("AddAttachments.aspx?mode=" + mode + "&entityId=" + entityId + "&canAdd= " + canAdd + "&canDelete= " + canDelete + "&isMultiUpload= " + isMultiFileUpload + "&showDtype= " + showDocumentType + "&entityName=" + entityName, "RadWindow");
                return false;
                //}
            }
            function openRefreshTaxPopup() {
                setTimeout(function () {
                    $("#Refresh-modal").modal("show");
                }, 500);

            }

        </script>
    </telerik:RadCodeBlock>
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
            background-image: url(../Content/images/loading.gif);
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
                        <telerik:AjaxUpdatedControl ControlID="lnkVerticalBudget1" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="pnlBudgetLink" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <%-- <telerik:AjaxSetting AjaxControlID="btnRequestSave">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="gvPendingApproval" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>--%>
            <telerik:AjaxSetting AjaxControlID="RadAjaxManager1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="radNotificationMessage" UpdatePanelRenderMode="Inline" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="cmbVertical">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="lnkVerticalBudget" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                      <telerik:AjaxUpdatedControl ControlID="gvPendingApproval" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                     <telerik:AjaxUpdatedControl ControlID="gvApproved" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                      <telerik:AjaxUpdatedControl ControlID="lblINRAmt" LoadingPanelID="LoadingPanel1" UpdatePanelRenderMode="Inline"></telerik:AjaxUpdatedControl>                    
                     <telerik:AjaxUpdatedControl ControlID="lblProposedAmt" LoadingPanelID="LoadingPanel1" UpdatePanelRenderMode="Inline"></telerik:AjaxUpdatedControl>                    
                     <telerik:AjaxUpdatedControl ControlID="lnkVerticalBudget1" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="grdVerticalBudget" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                     <telerik:AjaxUpdatedControl  ControlID="gvReverse" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>    
                     <telerik:AjaxUpdatedControl  ControlID="gvReversedRequest" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>   
                                   
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lnkVerticalBudget1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdVerticalBudget" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="rbtnBill" EventName="">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rbtnBill" LoadingPanelID="LoadingPanel1" UpdatePanelRenderMode="Inline"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="rbtnAdvance" LoadingPanelID="LoadingPanel1" UpdatePanelRenderMode="Inline"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="RadMultiPage1" LoadingPanelID="LoadingPanel1" UpdatePanelRenderMode="Inline"></telerik:AjaxUpdatedControl>
                      <telerik:AjaxUpdatedControl ControlID="lblINRAmt" LoadingPanelID="LoadingPanel1" UpdatePanelRenderMode="Inline"></telerik:AjaxUpdatedControl>                    
                     <telerik:AjaxUpdatedControl ControlID="lblProposedAmt" LoadingPanelID="LoadingPanel1" UpdatePanelRenderMode="Inline"></telerik:AjaxUpdatedControl>                    
                     <%-- <telerik:AjaxUpdatedControl ControlID="gvReverse" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>--%>
                   <%--   <telerik:AjaxUpdatedControl  ControlID="gvReversedRequest" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>  --%>
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="rbtnAdvance" EventName="">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rbtnBill" LoadingPanelID="LoadingPanel1" UpdatePanelRenderMode="Inline"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="rbtnAdvance" LoadingPanelID="LoadingPanel1" UpdatePanelRenderMode="Inline"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="RadMultiPage1" LoadingPanelID="LoadingPanel1" UpdatePanelRenderMode="Inline"></telerik:AjaxUpdatedControl>                    
                      <telerik:AjaxUpdatedControl ControlID="lblINRAmt" LoadingPanelID="LoadingPanel1" UpdatePanelRenderMode="Inline"></telerik:AjaxUpdatedControl>                    
                     <telerik:AjaxUpdatedControl ControlID="lblProposedAmt" LoadingPanelID="LoadingPanel1" UpdatePanelRenderMode="Inline"></telerik:AjaxUpdatedControl>                    
                     <%-- <telerik:AjaxUpdatedControl ControlID="gvReverse" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>--%>
                   <%--   <telerik:AjaxUpdatedControl  ControlID="gvReversedRequest" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>  --%>
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="btnRequestSubmit" EventName="">
                <UpdatedControls>
                     <telerik:AjaxUpdatedControl ControlID="pnl1" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="numCurrentAllocation" LoadingPanelID="LoadingPanel1" UpdatePanelRenderMode="Inline"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="lnkVerticalBudget1" LoadingPanelID="LoadingPanel1"  UpdatePanelRenderMode="Inline"></telerik:AjaxUpdatedControl>
                   
                </UpdatedControls>
            </telerik:AjaxSetting>
             <telerik:AjaxSetting AjaxControlID="pnl1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="pnl1" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <%--<telerik:AjaxSetting AjaxControlID="btnRequestSubmit" EventName="">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdBudgetAllocation" LoadingPanelID="LoadingPanel1" UpdatePanelRenderMode="Inline"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>--%>

<%--            <telerik:AjaxSetting AjaxControlID="RadMultiPage1" EventName="">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rbtnBill" LoadingPanelID="LoadingPanel1" UpdatePanelRenderMode="Inline"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="rbtnAdvance" LoadingPanelID="LoadingPanel1" UpdatePanelRenderMode="Inline"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="RadMultiPage1" LoadingPanelID="LoadingPanel1" UpdatePanelRenderMode="Inline"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="btnRequestSave" LoadingPanelID="LoadingPanel1" UpdatePanelRenderMode="Inline"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="btnRequestSubmit" LoadingPanelID="LoadingPanel1" UpdatePanelRenderMode="Inline"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>--%>

              <telerik:AjaxSetting AjaxControlID="gvPendingApproval1" EventName="">
                <UpdatedControls>
                     <telerik:AjaxUpdatedControl ControlID="gvPendingApproval1" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>                    
                </UpdatedControls>
            </telerik:AjaxSetting>

              <telerik:AjaxSetting AjaxControlID="gvPendingApproval" EventName="">
                <UpdatedControls>
                     <telerik:AjaxUpdatedControl ControlID="gvPendingApproval" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>                    
                </UpdatedControls>
            </telerik:AjaxSetting>

                        <telerik:AjaxSetting AjaxControlID="gvApproved" EventName="">
                <UpdatedControls>
                     <telerik:AjaxUpdatedControl ControlID="gvApproved" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>                    
                </UpdatedControls>
            </telerik:AjaxSetting>

              <telerik:AjaxSetting AjaxControlID="gvApproved1" EventName="">
                <UpdatedControls>
                     <telerik:AjaxUpdatedControl ControlID="gvApproved1" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>                    
                </UpdatedControls>
            </telerik:AjaxSetting>

             <telerik:AjaxSetting AjaxControlID="RadTabStrip1" EventName="">
                <UpdatedControls>
                     <telerik:AjaxUpdatedControl ControlID="RadTabStrip1" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>   
                        <telerik:AjaxUpdatedControl ControlID="RadMultiPage1" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl> 
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="gvReverse">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="gvReverse" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                  <telerik:AjaxSetting AjaxControlID="gvReversedRequest">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="gvReversedRequest" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="btnReverse">
                <UpdatedControls>
             <telerik:AjaxUpdatedControl ControlID="gvReverse" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
            
            <%--   <telerik:AjaxSetting AjaxControlID="btnReverse">
                <UpdatedControls>
               <telerik:AjaxUpdatedControl ControlID="lblCompCode" LoadingPanelID="LoadingPanel1" ></telerik:AjaxUpdatedControl> 
                    <telerik:AjaxUpdatedControl ControlID="lblDocNo" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl> 
                     <telerik:AjaxUpdatedControl ControlID="lblFiscalYear" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl> 
                     <telerik:AjaxUpdatedControl ControlID="lblPostDate" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl> 
                     <telerik:AjaxUpdatedControl ControlID="cmbReverseReason" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl> --%>
                   <%-- <telerik:AjaxUpdatedControl ControlID="pnlReverseDetail"LoadingPanelID="LoadingPanel1" />
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
    <style>
        .RadMultiPage {
            box-sizing: border-box;
            overflow-x: scroll !important;
        }

        table {
            border-spacing: 0;
            border-collapse: collapse;
            background-color: #FFF !important;
        }

        .RadWindow .rwIcon {
            display: none !important;
        }

        .RadWindow .rwTitleWrapper .rwTitle {
            margin: 2px 0 0 -21px !important;
            font-weight: bold !important;
        }

        .agnstbillradio {
            display: table-cell;
            vertical-align: middle;
            padding-top: 5px !important;
        }

        .agnstbilllable {
            padding-top: 7px;
        }
    </style>
    <div class="row margin-0">
        <div class="col-xs-12 heading-big">
            <h5 class="margin-0 lineheight-42 breath-ctrl">Home / <span>FnA Approver</span></h5>
        </div>
        <div class="col-sm-6 col-xs-6 ">
        </div>
    </div>
    <div class="row margin-0">
        <telerik:RadNotification RenderMode="Lightweight" Position="Center" runat="server" ID="radMessage" VisibleOnPageLoad="false"
            Width="350px" TitleIcon="none" ContentIcon="none" EnableShadow="true" AnimationDuration="1000">
        </telerik:RadNotification>
        <div class="col-md-12">
            <div class="col-sm-4">
                <div class="col-sm-2 col-xs-12 padding-0">
                    <label class="control-label lable-txt" for="name">Vertical:</label>
                </div>
                <div class="col-sm-10 col-xs-12 padding-0">

                    <telerik:RadComboBox ID="cmbVertical" AutoPostBack="true" RenderMode="Lightweight" runat="server" OnSelectedIndexChanged="cmbVertical_SelectedIndexChanged">
                        <Items></Items>
                    </telerik:RadComboBox>


                </div>
                <div class="col-sm-12 padding-0">
                    <div class="text-info small">
                        Note:Items blocked for payment are not considered.
                    </div>
                </div>

            </div>
            <div class="col-sm-2 padding-0 agnstbillradio">

                <asp:RadioButton ID="rbtnBill" GroupName="rblInitiator" runat="server" AutoPostBack="true" OnCheckedChanged="rbtnBill_CheckedChanged" />Against Bill
                 <asp:RadioButton ID="rbtnAdvance" GroupName="rblInitiator" runat="server" AutoPostBack="true" OnCheckedChanged="rbtnAdvance_CheckedChanged" />Advance
                
            </div>
            <div class="col-md-2 agnstbilllable text-right">
                
                <label for="name">Vertical Budget:</label>             
               
            </div>
                <div class="col-md-2 agnstbilllable text-left" style="padding-left:0px" >
                  
                    <asp:LinkButton ID="lnkVerticalBudget1" runat="server" OnClick="lnkVerticalBudget_Click" Enabled="true" Text="0" style="display:inline"></asp:LinkButton>
                    
                </div>
        </div>
        <!--End of heading wrapper-->



        <div class="row margin-0">
        </div>
        <!-- End of Company name-row-->

        <div>
            <div class="col-xs-12">
                <div style="text-align: right">
            <%--        <asp:Label ID="lblHouseCode" runat="server" CssClass="lable-txt control-label ">Assign House Bank Code:</asp:Label>
                    <telerik:RadTextBox RenderMode="Lightweight" ID="txtHouseCode" runat="server">
                    </telerik:RadTextBox>--%>
                    <%--  <asp:Button CssClass="btn btn-grey" Text="Validate" runat="server" ID="btnValidate"></asp:Button>
                <asp:Button CssClass="btn btn-grey" Text="Apply" runat="server" ID="btnApply"></asp:Button>--%>
                    <asp:Button CssClass="btn btn-grey" Text="Export" runat="server" ID="btnExport" OnClick="btnExport_Click"></asp:Button>
                </div>
                <div>
                </div>
            </div>
            <div class="col-sm-1">
            </div>
        </div>

        <div class="row margin-10">
            <div class="col-xs-12">
                <div class="panel with-nav-tabs panel-default border-0">
                    <telerik:RadTabStrip RenderMode="Lightweight" runat="server" ID="RadTabStrip1" MultiPageID="RadMultiPage1" SelectedIndex="0" Skin="Silk" OnTabClick="RadTabStrip1_TabClick">
                        <Tabs>
                            <telerik:RadTab Text="Pending for Approval" Selected="True"></telerik:RadTab>
                            <telerik:RadTab Text="Approved Request"></telerik:RadTab>                            
                            <telerik:RadTab Text="Paid Request"></telerik:RadTab>
                            <telerik:RadTab Text="Reversed Request"></telerik:RadTab>
                            
                        </Tabs>
                    </telerik:RadTabStrip>

                      <div style="position: absolute; top: 7px; left:65%; min-width: 350px;">                                         
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



                    <telerik:RadMultiPage runat="server" ID="RadMultiPage1" SelectedIndex="0" CssClass="outerMultiPage">
                        <telerik:RadPageView runat="server" ID="RadPageView1">
                            <telerik:RadGrid RenderMode="Lightweight" ID="gvPendingApproval" runat="server" AutoGenerateColumns="false"
                                AllowPaging="false" AllowSorting="false" AllowFilteringByColumn="false" PageSize="5" OnItemCommand="gvPendingApproval_ItemCommand" OnNeedDataSource="gvPendingApproval_NeedDataSource">
                                <GroupingSettings CaseSensitive="false" />
                                <ClientSettings>
                                    <Selecting AllowRowSelect="true"></Selecting>
                                    <Scrolling AllowScroll="True" UseStaticHeaders="True" ScrollHeight="600px" SaveScrollPosition="true" FrozenColumnsCount="4"></Scrolling>
                                </ClientSettings>
                                <MasterTableView CommandItemSettings-ShowRefreshButton="false" EditMode="InPlace" ShowHeader="true" CommandItemDisplay="None"
                                    AllowFilteringByColumn="false" AllowPaging="false" AllowSorting="false" NoMasterRecordsText="" DataKeyNames="Company" TableLayout="Fixed" Font-Size="9">
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
                                            AllowPaging="false" AllowSorting="false" AllowFilteringByColumn="false" PageSize="5" OnItemCommand="gvPendingApproval1_ItemCommand">
                                            <GroupingSettings CaseSensitive="false" />
                                            <ClientSettings>
                                                <Selecting AllowRowSelect="true"></Selecting>
                                                <Scrolling AllowScroll="True" UseStaticHeaders="True" ScrollHeight="500px" SaveScrollPosition="true" FrozenColumnsCount="4"></Scrolling>
                                            </ClientSettings>
                                            <MasterTableView CommandItemSettings-ShowRefreshButton="false" EditMode="InPlace" ShowHeader="true" CommandItemDisplay="None"
                                                AllowFilteringByColumn="false" AllowPaging="false" AllowSorting="false" NoMasterRecordsText="" TableLayout="Fixed" Font-Size="9">
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
                                                        <telerik:RadGrid RenderMode="Lightweight" ID="gvPendingApproval2" runat="server" AutoGenerateColumns="false"
                                                            AllowSorting="false" AllowPaging="false" AllowFilteringByColumn="false" PageSize="5" AllowMultiRowSelection="true" OnItemDataBound="gvPendingApproval2_ItemDataBound" OnItemCommand="gvPendingApproval2_ItemCommand">
                                                            <GroupingSettings CaseSensitive="false" />
                                                            <ClientSettings>
                                                                <Selecting AllowRowSelect="true"></Selecting>
                                                                <Scrolling AllowScroll="True" UseStaticHeaders="True" ScrollHeight="300px" SaveScrollPosition="true" FrozenColumnsCount="4"></Scrolling>
                                                                 <ClientEvents OnRowSelected="RowSelectedApproverGrid" OnRowDeselected="RowSelectedApproverGrid" />
                                                            </ClientSettings>
                                                            <MasterTableView CommandItemSettings-ShowRefreshButton="false" EditMode="InPlace" CommandItemDisplay="None"
                                                                AllowFilteringByColumn="false" AllowPaging="false" AllowSorting="false" NoMasterRecordsText="" DataKeyNames="Id" TableLayout="Fixed" Font-Size="9">

                                                                <CommandItemSettings ShowRefreshButton="false" ShowExportToExcelButton="false" />
                                                                <NoRecordsTemplate>
                                                                    <table width="100%" border="0" cellpadding="20" cellspacing="20">
                                                                        <tr>
                                                                            <td align="center">No records to display.
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </NoRecordsTemplate>
                                                                <Columns>
                                                                    <telerik:GridClientSelectColumn HeaderStyle-Width="50px" HeaderText="Select" UniqueName="RequestSelect">
                                                                    </telerik:GridClientSelectColumn>

                                                                     <telerik:GridBoundColumn HeaderStyle-Width="120px" HeaderText="Document Number" UniqueName="DocumentNumber" DataField="DPRNumber">
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridBoundColumn HeaderStyle-Width="120px" HeaderText="DPR Number" UniqueName="DPRNumber" DataField="DPRNumber">
                                                                    </telerik:GridBoundColumn>
                                                                      <telerik:GridBoundColumn HeaderStyle-Width="100px" HeaderText="Reference" UniqueName="Reference" DataField="Reference">
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridBoundColumn HeaderStyle-Width="150px"   HeaderText="Gross Amount" UniqueName="GrossAmount" DataField="GrossAmount" DataFormatString="{0:###,##0.00}">
                                                                    </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn HeaderStyle-Width="150px"  HeaderText="Tax Rate" UniqueName="TaxRate" DataField="TaxRate" DataFormatString="{0:###,##0.00}">
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn HeaderStyle-Width="150px"  HeaderText="Tax Amount" UniqueName="TaxAmount" DataField="TaxAmount" DataFormatString="{0:###,##0.00}">
                                                                </telerik:GridBoundColumn>
                                                                    <telerik:GridBoundColumn HeaderStyle-Width="120px" HeaderText="Amount in INR" UniqueName="Amount" DataField="Amount" DataFormatString="{0:###,##0.00}">
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridBoundColumn HeaderStyle-Width="120px" HeaderText="Amount Proposed" UniqueName="AmountProposed" DataField="Amount" DataFormatString="{0:###,##0.00}">
                                                                    </telerik:GridBoundColumn>
                                                                     <telerik:GridTemplateColumn HeaderStyle-Width="130px" HeaderText="Approved Amount" UniqueName="ApprovedAmount" DataField="ApprovedAmount">
                                                                        <ItemTemplate>
                                                                            <telerik:RadNumericTextBox ID="tbApprovedAmount" runat="server" Width="115px" Text='<%# String.Format("{0:###,##0.00}",Eval("ApprovedAmount")) %>'></telerik:RadNumericTextBox>
                                                                            <telerik:RadLabel ID="lblApprovedAmount" runat="server" Text="Invalid Amount" Visible="false" ForeColor="Red"></telerik:RadLabel>
                                                                        </ItemTemplate>
                                                                        <ItemStyle Height="85px" />
                                                                    </telerik:GridTemplateColumn>
                                                                      <telerik:GridBoundColumn  HeaderStyle-Width="100px" HeaderText="Debit/Credit" UniqueName="DCFLag" DataField="DCFLag" >
                                                                     </telerik:GridBoundColumn>   
                                                                    <telerik:GridTemplateColumn HeaderStyle-Width="120px" HeaderText="Approval" UniqueName="Approval" DataField="Approval">
                                                                        <ItemTemplate>
                                                                            <telerik:RadDropDownList RenderMode="Lightweight" ID="cmbApproval" AutoPostBack="true" runat="server" DropDownAutoWidth="Enabled" OnSelectedIndexChanged="cmbApproval_SelectedIndexChanged">
                                                                            </telerik:RadDropDownList>
                                                                        </ItemTemplate>
                                                                        <ItemStyle Height="85px" />
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn HeaderStyle-Width="120px" HeaderText="Select Stage" UniqueName="Stage" DataField="Stage">
                                                                        <ItemTemplate>
                                                                            <telerik:RadDropDownList ID="cmbStage" RenderMode="Lightweight" runat="server" DropDownAutoWidth="Enabled">
                                                                            </telerik:RadDropDownList>
                                                                        </ItemTemplate>
                                                                        <ItemStyle Height="85px" />
                                                                    </telerik:GridTemplateColumn>
                                                                     <telerik:GridBoundColumn HeaderStyle-Width="100px" HeaderText="Document Currency" UniqueName="Currency" DataField="Currency">
                                                                    </telerik:GridBoundColumn>
                                                                         <telerik:GridDateTimeColumn HeaderStyle-Width="150px" HeaderText="Net Due Date" UniqueName="NetDueDate" DataField="NetDueDate" DataFormatString="{0:dd-MM-yyyy}">
                                                                    </telerik:GridDateTimeColumn>
                                                                           <telerik:GridBoundColumn HeaderStyle-Width="200px" HeaderText="Nature of Request" UniqueName="NatureOfRequest" DataField="NatureOfRequest">
                                                                    </telerik:GridBoundColumn>
                                                                     <telerik:GridTemplateColumn HeaderStyle-Width="200px" HeaderText="Remark" UniqueName="Remark">
                                                                        <ItemTemplate>
                                                                            <telerik:RadTextBox ID="tbRemark" TextMode="MultiLine" runat="server" MaxLength="100" Rows="1" CausesValidation="True" ValidationGroup="val" Height="70px" Width="180px" Text='<%# Eval("Remark") %>'></telerik:RadTextBox>
                                                                            <telerik:RadLabel ID="lblRemark" runat="server" Text="*Required" Visible="false" ForeColor="Red"></telerik:RadLabel>
                                                                        </ItemTemplate>
                                                                        <ItemStyle Height="85px" />
                                                                    </telerik:GridTemplateColumn>
                                                                     <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="Un-settled Open Advance (INR)" UniqueName="UnsettledOpenAdvance" DataField="UnsettledOpenAdvance">
                                                                    </telerik:GridBoundColumn>
                                                                          <telerik:GridBoundColumn HeaderStyle-Width="190px" HeaderText="Withholding Tax Code" UniqueName="WithholdingTaxCode" DataField="WithholdingTaxCode">
                                                                    </telerik:GridBoundColumn>                                                                   
                                                                    <telerik:GridBoundColumn HeaderStyle-Width="200px" HeaderText="Justification for Adv Payment" UniqueName="JustificationforAdvPayment" DataField="JustificationforAdvPayment">
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridTemplateColumn HeaderStyle-Width="150px" HeaderText="Attachments" UniqueName="ViewAttachment">
                                                                        <ItemTemplate>
                                                                            <telerik:RadButton ID="btnAttachment" runat="server" CommandName="Attachment" CommandArgument='<%# Eval("Id") %>' Text="Add/View" CssClass="gridHyperlinks"></telerik:RadButton>
                                                                        </ItemTemplate>
                                                                        <ItemStyle Height="85px" />
                                                                    </telerik:GridTemplateColumn>
                                                                     <telerik:GridDateTimeColumn HeaderStyle-Width="150px" HeaderText="Posting Date" UniqueName="PostingDate" DataField="PostingDate" DataFormatString="{0:dd-MM-yyyy}">
                                                                    </telerik:GridDateTimeColumn>   
                                                                    <telerik:GridDateTimeColumn HeaderStyle-Width="150px" HeaderText="Expected Clearing Date" UniqueName="ExpectedClearingDate" DataField="ExpectedClearingDate" DataFormatString="{0:dd-MM-yyyy}">
                                                                    </telerik:GridDateTimeColumn>
                                                                        <telerik:GridBoundColumn HeaderStyle-Width="100px" HeaderText="Fiscal Year" UniqueName="FiscalYear" DataField="FiscalYear">
                                                                    </telerik:GridBoundColumn>
                                                                       <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="Purchasing Document" UniqueName="PurchasingDocument" DataField="PurchasingDocument">
                                                                    </telerik:GridBoundColumn>
                                                                     <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="Special GL" UniqueName="SpecialGL" DataField="SpecialGL">
                                                                    </telerik:GridBoundColumn>
                                                                     <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="Profit Centre" UniqueName="ProfitCentre" DataField="ProfitCentre">
                                                                    </telerik:GridBoundColumn>
                                                                      <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="Business Area" UniqueName="BusinessArea" DataField="BusinessArea">
                                                                    </telerik:GridBoundColumn>


                                                                    <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="Sub Vertical" UniqueName="SubVertical" DataField="SubVertical">
                                                                    </telerik:GridBoundColumn>
                                                                     <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="Vertical" UniqueName="Vertical" DataField="Vertical">
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridTemplateColumn HeaderStyle-Width="150px" HeaderText="House Bank" UniqueName="HouseBank" DataField="HouseBank">
                                                                        <ItemTemplate>
                                                                            <telerik:RadTextBox ID="tbHouseBank" Width="130px" class="form-control" runat="server" MaxLength="20" Enabled="false" Text='<%# Eval("HouseBank") %>'>
                                                                            </telerik:RadTextBox>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridBoundColumn HeaderStyle-Width="120px" HeaderText="Payment Method" UniqueName="PaymentMethod" DataField="PaymentMethod">
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="Cheque Lot Number" UniqueName="ChecqueLotNumber" DataField="ChequeLotNumber">
                                                                    </telerik:GridBoundColumn>                                                                   
                                                              
                                                                    <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="TransactionStatusId" UniqueName="TransactionStatusId" DataField="TransactionStatusId" Visible="true" Display="false">
                                                                    </telerik:GridBoundColumn>
                                                                           <telerik:GridDateTimeColumn HeaderStyle-Width="150px" HeaderText="Submission Date" UniqueName="SubmissionDate" DataField="SubmissionDate" DataFormatString="{0:dd-MM-yyyy}">
                                                                    </telerik:GridDateTimeColumn>
                                                                    <telerik:GridTemplateColumn HeaderStyle-Width="120px" HeaderText="View History" UniqueName="ViewHistory">
                                                                        <ItemTemplate>
                                                                            <asp:HyperLink ID="viewHistory" runat="server" Text="View" CssClass="btn btn-grey gridHyperlinks"></asp:HyperLink>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>   
                                                             

                                                                </Columns>
                                                                <PagerStyle PageSizes="5,10,15" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                                                                    PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />
                                                            </MasterTableView>
                                                        </telerik:RadGrid>

                                                        <div class="col-lg-12 padding-0 margin-10">
                                                           <%-- <asp:Button ID="btnRequestSave1" CssClass="btn btn-grey" runat="server" OnClick="btnRequestSave_Click" Text="Save" OnClientClick="return confirm('Are you sure you want proceed?');" />
                                                            <asp:Button CssClass="btn btn-grey" runat="server" ID="btnRequestSubmit1" OnClick="btnRequestSubmit_Click" Text="Submit123" OnClientClick="return confirm('Are you sure you want proceed?');" />--%>

                                                            <asp:LinkButton CssClass="btn btn-grey" OnClientClick="return confirm('Are you sure you want proceed?');" OnClick="btnRequestSave_Click" runat="server" ID="btnRequestSave">Save</asp:LinkButton>
                                                            <asp:LinkButton CssClass="btn btn-grey" OnClientClick="return confirm('Are you sure you want proceed?');" OnClick="btnRequestSubmit_Click" runat="server" ID="btnRequestSubmit">Submit</asp:LinkButton>
                                                            <%--<a href="home_screen.html" class="btn btn-grey">Cancel</a>--%>
                                                        </div>
                                                        <div class="col-lg-12 padding-0 margin-10" style="margin-left: 10px;">
                                                            <asp:Label ID="lblHouseCode" runat="server" CssClass="lable-txt control-label ">Assign House Bank Code:</asp:Label>
                                                            <telerik:RadTextBox RenderMode="Lightweight" ID="txtHouseCode" runat="server">
                                                                <ClientEvents OnKeyPress="OnKeyPressSerialText" />
                                                            </telerik:RadTextBox>
                                                            <asp:Button CssClass="btn btn-grey" runat="server" ID="btnValidate" OnClick="btnValidate_Click" Text="Validate" />
                                                            <asp:Button CssClass="btn btn-grey" runat="server" ID="btnApply" OnClick="btnApply_Click" Text="Apply" />
                                                        </div>
                                                    </div>
                                                </NestedViewTemplate>

                                                <Columns>
                                                    <%-- <telerik:GridClientSelectColumn HeaderText="Select" UniqueName="Select">
                                                </telerik:GridClientSelectColumn>--%>
                                                    <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="Vendor" UniqueName="Vendor" DataField="Vendor">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="Total Amount" UniqueName="TotalAmount" DataField="TotalAmount" DataFormatString="{0:###,##0.00}">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="Proposed Total" UniqueName="ProposedTotal" DataField="ProposedTotal" DataFormatString="{0:###,##0.00}">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="Approved Total" UniqueName="ApprovedTotal" DataField="ApprovedTotal" DataFormatString="{0:###,##0.00}">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="Today's Paid Total" UniqueName="TodayTotal" DataField="TodayTotal" DataFormatString="{0:###,##0.00}">
                                                    </telerik:GridBoundColumn>
                                                </Columns>
                                                <PagerStyle PageSizes="5,10,15" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                                                    PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />
                                            </MasterTableView>
                                        </telerik:RadGrid>
                                        <%--<div class="col-lg-12 padding-0 margin-10" style="margin-left: 10px;">
                                            <asp:Label ID="lblHouseCode" runat="server" CssClass="lable-txt control-label ">Assign House Bank Code:</asp:Label>
                                            <telerik:RadTextBox RenderMode="Lightweight" ID="txtHouseCode" runat="server">
                                                  <ClientEvents OnKeyPress="OnKeyPressSerialText"/>
                                            </telerik:RadTextBox>                                             
                                            <asp:Button CssClass="btn btn-grey" runat="server" ID="btnValidate" OnClick="btnValidate_Click" Text="Validate" />
                                            <asp:Button CssClass="btn btn-grey" runat="server" ID="btnApply" OnClick="btnApply_Click" Text="Apply"  />
                                        </div>--%>
                                    </NestedViewTemplate>
                                    <Columns>
                                        <%--<telerik:GridClientSelectColumn HeaderText="" UniqueName="Select">
                                    </telerik:GridClientSelectColumn>--%>
                                        <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="Company" UniqueName="Company" DataField="Company">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="Total Amount" UniqueName="TotalAmount" DataField="TotalAmount" DataFormatString="{0:###,##0.00}">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="Proposed Total" UniqueName="ProposedTotal" DataField="ProposedTotal" DataFormatString="{0:###,##0.00}">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="Approved Total" UniqueName="ApprovedTotal" DataField="ApprovedTotal" DataFormatString="{0:###,##0.00}">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="Today's Paid Total" UniqueName="TodayTotal" DataField="TodayTotal" DataFormatString="{0:###,##0.00}">
                                        </telerik:GridBoundColumn>
                                    </Columns>
                                    <PagerStyle PageSizes="5,10,15" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                                        PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />
                                </MasterTableView>
                            </telerik:RadGrid>
                        </telerik:RadPageView>
                        <telerik:RadPageView runat="server" ID="RadPageView2">
                            <telerik:RadGrid RenderMode="Lightweight" ID="gvApproved" runat="server" AutoGenerateColumns="false"
                                AllowPaging="false" AllowSorting="false" AllowFilteringByColumn="false" PageSize="5" AllowMultiRowSelection="false" OnItemDataBound="gvApproved_ItemDataBound" OnItemCommand="gvApproved_ItemCommand" OnNeedDataSource="gvApproved_NeedDataSource">
                                <GroupingSettings CaseSensitive="false" />
                                <ClientSettings>
                                    <Selecting AllowRowSelect="true"></Selecting>
                                    <Scrolling AllowScroll="True" UseStaticHeaders="True" ScrollHeight="600px" SaveScrollPosition="true" FrozenColumnsCount="4"></Scrolling>
                                </ClientSettings>
                                <MasterTableView CommandItemSettings-ShowRefreshButton="false" EditMode="InPlace" ShowHeader="true" CommandItemDisplay="None"
                                    AllowFilteringByColumn="false" AllowSorting="false" AllowPaging="false" NoMasterRecordsText="" DataKeyNames="Company" TableLayout="Fixed" Font-Size="9">
                                    <CommandItemSettings ShowRefreshButton="false" ShowExportToExcelButton="false" />
                                    <NoRecordsTemplate>
                                        <table width="100%" border="0" cellpadding="20" cellspacing="20">
                                            <tr>
                                                <td align="center">No records to display.
                                                </td>
                                            </tr>
                                        </table>
                                    </NoRecordsTemplate>
                                    <NestedViewTemplate>
                                        <telerik:RadGrid RenderMode="Lightweight" ID="gvApproved1" runat="server" AutoGenerateColumns="false"
                                            AllowPaging="false" AllowSorting="false" AllowFilteringByColumn="false" PageSize="5" OnItemCommand="gvApproved1_ItemCommand">
                                            <GroupingSettings CaseSensitive="false" />
                                            <ClientSettings>
                                                <Selecting AllowRowSelect="true"></Selecting>
                                                <Scrolling AllowScroll="True" UseStaticHeaders="True" ScrollHeight="500px" SaveScrollPosition="true" FrozenColumnsCount="4"></Scrolling>
                                            </ClientSettings>
                                            <MasterTableView CommandItemSettings-ShowRefreshButton="false" EditMode="InPlace" ShowHeader="true" CommandItemDisplay="None"
                                                AllowFilteringByColumn="false" AllowPaging="false" AllowSorting="false" NoMasterRecordsText="" TableLayout="Fixed" Font-Size="9">
                                                <CommandItemSettings ShowRefreshButton="false" ShowExportToExcelButton="false" />
                                                <NoRecordsTemplate>
                                                    <table width="100%" border="0" cellpadding="20" cellspacing="20">
                                                        <tr>
                                                            <td align="center">No records to display.
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </NoRecordsTemplate>
                                                <NestedViewTemplate>
                                                    <div class="table-scroll">
                                                        <telerik:RadGrid RenderMode="Lightweight" ID="gvApproved2" runat="server" AutoGenerateColumns="false"
                                                            AllowPaging="false" AllowSorting="false" AllowFilteringByColumn="false" PageSize="5" AllowMultiRowSelection="true"
                                                            OnItemDataBound="gvApproved2_ItemDataBound" OnItemCommand="gvApproved2_ItemCommand">
                                                            <GroupingSettings CaseSensitive="false" />
                                                            <ClientSettings>
                                                                <Selecting AllowRowSelect="true"></Selecting>
                                                                <Scrolling AllowScroll="True" UseStaticHeaders="True" ScrollHeight="300px" SaveScrollPosition="true" FrozenColumnsCount="3"></Scrolling>
                                                            </ClientSettings>
                                                            <MasterTableView CommandItemSettings-ShowRefreshButton="false" EditMode="InPlace" CommandItemDisplay="None"
                                                                AllowFilteringByColumn="false" AllowPaging="false" AllowSorting="false" NoMasterRecordsText="" DataKeyNames="Id" TableLayout="Fixed" Font-Size="9">
                                                                <CommandItemSettings ShowRefreshButton="false" ShowExportToExcelButton="false" />
                                                                <NoRecordsTemplate>
                                                                    <table width="100%" border="0" cellpadding="20" cellspacing="20">
                                                                        <tr>
                                                                            <td align="center">No records to display.
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </NoRecordsTemplate>
                                                                <Columns>
                                                                          <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="DPR Number" UniqueName="DPRNumber" DataField="DPRNumber">
                                                                    </telerik:GridBoundColumn>
                                                                     <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="Document Number" UniqueName="DocumentNumber" DataField="DPRNumber">
                                                                    </telerik:GridBoundColumn>
                                                                       <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="Reference" UniqueName="Reference" DataField="Reference">
                                                                    </telerik:GridBoundColumn>
                                                                     <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="Amount in INR" UniqueName="Amount" DataField="Amount" DataFormatString="{0:###,##0.00}">
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="Amount Proposed" UniqueName="AmountProposed" DataField="Amount" DataFormatString="{0:###,##0.00}">
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="Approved Amount" UniqueName="ApprovedAmount" DataField="ApprovedAmount" DataFormatString="{0:###,##0.00}">
                                                                    </telerik:GridBoundColumn>
                                                                      <telerik:GridBoundColumn HeaderStyle-Width="170px" HeaderText="Clearing Document No" UniqueName="DocumentClearingNo" DataField="DocumentClearingNo">
                                                                </telerik:GridBoundColumn> 
                                                                    <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="Approval" UniqueName="Approval" DataField="Approval" Visible="false">
                                                                    </telerik:GridBoundColumn>
                                                                      <telerik:GridBoundColumn HeaderStyle-Width="170px" HeaderText="Document Currency" UniqueName="Currency" DataField="Currency">
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridDateTimeColumn HeaderStyle-Width="150px" HeaderText="Net Due Date" UniqueName="NetDueDate" DataField="NetDueDate">
                                                                    </telerik:GridDateTimeColumn>
                                                                      <telerik:GridBoundColumn HeaderStyle-Width="250px" HeaderText="Un-settled Open Advance (INR)" UniqueName="UnsettledOpenAdvance" DataField="UnsettledOpenAdvance">
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridBoundColumn HeaderStyle-Width="220px" HeaderText="Withholding Tax Code" UniqueName="WithholdingTaxCode" DataField="WithholdingTaxCode">
                                                                    </telerik:GridBoundColumn>
                                                                       <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="Nature of Request" UniqueName="NatureOfRequest" DataField="NatureOfRequest">
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridBoundColumn HeaderStyle-Width="250px" HeaderText="Justification for Adv Payment" UniqueName="JustificationforAdvPayment" DataField="JustificationforAdvPayment">
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="Remark" UniqueName="Remark" DataField="Remark">
                                                                    </telerik:GridBoundColumn>
                                                                     <telerik:GridTemplateColumn HeaderStyle-Width="150px" HeaderText="Attachments" UniqueName="ViewAttachment">

                                                                        <ItemTemplate>
                                                                            <telerik:RadButton ID="btnAttachment" runat="server" CommandName="Attachment" CommandArgument='<%# Eval("Id") %>' Text="Add/View" CssClass=" gridHyperlinks"></telerik:RadButton>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                       <telerik:GridDateTimeColumn HeaderStyle-Width="150px" HeaderText="Posting Date" UniqueName="PostingDate" DataField="PostingDate" DataFormatString="{0:dd-MM-yyyy}">
                                                                    </telerik:GridDateTimeColumn>
                                                                     <telerik:GridDateTimeColumn HeaderStyle-Width="220px" HeaderText="Expected Clearing Date" UniqueName="ExpectedClearingDate" DataField="ExpectedClearingDate" DataFormatString="{0:dd-MM-yyyy}">
                                                                    </telerik:GridDateTimeColumn>
                                                                       <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="Fiscal Year" UniqueName="FiscalYear" DataField="FiscalYear">
                                                                    </telerik:GridBoundColumn>
                                                                      <telerik:GridBoundColumn HeaderStyle-Width="180px" HeaderText="Purchasing Document" UniqueName="PurchasingDocument" DataField="PurchasingDocument">
                                                                    </telerik:GridBoundColumn>
                                                                      <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="Special GL" UniqueName="SpecialGL" DataField="SpecialGL">
                                                                    </telerik:GridBoundColumn>
                                                                          <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="Profit Centre" UniqueName="ProfitCentre" DataField="ProfitCentre">
                                                                    </telerik:GridBoundColumn>
                                                                        <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="Business Area" UniqueName="BusinessArea" DataField="BusinessArea">
                                                                    </telerik:GridBoundColumn>

                                                                    <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="Sub Vertical" UniqueName="SubVertical" DataField="SubVertical">
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="Vertical" UniqueName="Vertical" DataField="Vertical">
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="House Bank" UniqueName="HouseBank" DataField="HouseBank">
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="Payment Method" UniqueName="PaymentMethod" DataField="PaymentMethod">
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridBoundColumn HeaderStyle-Width="170px" HeaderText="Cheque Lot Number" UniqueName="ChecqueLotNumber" DataField="ChequeLotNumber">
                                                                    </telerik:GridBoundColumn>                                                                
                                                              
                                                                <telerik:GridDateTimeColumn HeaderStyle-Width="150px" HeaderText="Submission Date" UniqueName="SubmissionDate" DataField="SubmissionDate" DataFormatString="{0:dd-MM-yyyy}">
                                                                    </telerik:GridDateTimeColumn>                                                                  
                                                                    <%--  <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="DPR Number" UniqueName="DPRNumber" DataField="DPRNumber">
                                                                    <ItemStyle Height="85px" />
                                                                </telerik:GridBoundColumn>--%>
                                                                    
                                                                     <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="TransactionStatusId" UniqueName="TransactionStatusId" DataField="TransactionStatusId" Visible="true" Display="false">
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridTemplateColumn HeaderStyle-Width="150px" HeaderText="View History" UniqueName="ViewHistory">
                                                                        <ItemTemplate>
                                                                            <asp:HyperLink ID="viewHistory" runat="server" Text="View" CssClass="btn btn-grey gridHyperlinks"></asp:HyperLink>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <%--  <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="Stage" UniqueName="Stage" DataField="Stage">
                                                                    <ItemStyle Height="85px" />
                                                                </telerik:GridBoundColumn>--%>   
                                                              
                                                                                                                                  
                                                                </Columns>
                                                                <PagerStyle PageSizes="5,10,15" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                                                                    PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />
                                                            </MasterTableView>
                                                        </telerik:RadGrid>
                                                    </div>
                                                </NestedViewTemplate>

                                                <Columns>
                                                    <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="Vendor" UniqueName="Vendor" DataField="Vendor">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="Total Amount" UniqueName="TotalAmount" DataField="TotalAmount" DataFormatString="{0:###,##0.00}">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="Proposed Total" UniqueName="ProposedTotal" DataField="ProposedTotal" DataFormatString="{0:###,##0.00}">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="Approved Total" UniqueName="ApprovedTotal" DataField="ApprovedTotal" DataFormatString="{0:###,##0.00}">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="Today's Paid Total" UniqueName="TodayTotal" DataField="TodayTotal" DataFormatString="{0:###,##0.00}">
                                                    </telerik:GridBoundColumn>
                                                </Columns>

                                                <PagerStyle PageSizes="5,10,15" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                                                    PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />
                                            </MasterTableView>
                                        </telerik:RadGrid>
                                    </NestedViewTemplate>
                                    <Columns>
                                        <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="Company" UniqueName="Company" DataField="Company">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="Total Amount" UniqueName="TotalAmount" DataField="TotalAmount" DataFormatString="{0:###,##0.00}">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="Proposed Total" UniqueName="ProposedTotal" DataField="ProposedTotal" DataFormatString="{0:###,##0.00}">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="Approved Total" UniqueName="ApprovedTotal" DataField="ApprovedTotal" DataFormatString="{0:###,##0.00}">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="Today's Paid Total" UniqueName="TodayTotal" DataField="TodayTotal" DataFormatString="{0:###,##0.00}">
                                        </telerik:GridBoundColumn>
                                    </Columns>
                                    <PagerStyle PageSizes="5,10,15" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                                        PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />
                                </MasterTableView>
                            </telerik:RadGrid>
                        </telerik:RadPageView>

                         <telerik:RadPageView  runat="server" ID="RadPageView3">                        
                          <div class="col-sm-12">                            
                             <telerik:RadGrid RenderMode="Lightweight" ID="gvReverse" runat="server" AutoGenerateColumns="false"  ClientIDMode="AutoID" OnItemCommand="gvReverse_ItemCommand"
                                 OnNeedDataSource="gvReverse_NeedDataSource" OnItemDataBound="gvReverse_ItemDataBound" AllowPaging="true" PageSize="10" AllowSorting="true">
                                  <%--  <ClientSettings>                                   
                                    <Scrolling AllowScroll="True" UseStaticHeaders="True" ScrollHeight="300px" SaveScrollPosition="true" FrozenColumnsCount="3"></Scrolling>
                                    </ClientSettings>--%>
                                 <MasterTableView CommandItemSettings-ShowRefreshButton="false" EnableNoRecordsTemplate="true" EditMode="InPlace" CommandItemDisplay="None" ClientIDMode="AutoID"
                                     AllowFilteringByColumn="true" TableLayout="Fixed" HeaderStyle-Width="200px" AllowSorting="false" NoMasterRecordsText=""  DataKeyNames="DocumentClearingNo">
                                      <Columns>          
                                                 <telerik:GridBoundColumn HeaderText="Payment Lot No."   UniqueName="CheckLotNumber" DataField="LotNo">
                                                </telerik:GridBoundColumn>                                  
                                                <telerik:GridBoundColumn HeaderText="Company Code"   UniqueName="CompanyCode" DataField="CompanyCode">
                                                </telerik:GridBoundColumn>
                                                 <telerik:GridBoundColumn HeaderText="Document Clearing No" UniqueName="DocumentClearingNo" DataField="DocumentClearingNo">
                                                </telerik:GridBoundColumn> 
                                                 <telerik:GridDateTimeColumn HeaderText="Posting Date"   AllowFiltering="true" ItemStyle-Width="100px"  FilterControlWidth="200px"  UniqueName="PostingDate" EnableTimeIndependentFiltering="true"
                                                 DataType="System.DateTime" DataFormatString="{0:dd-MM-yyyy}" DataField="PostingDate">
                                                </telerik:GridDateTimeColumn>
                                                <telerik:GridBoundColumn HeaderText="Vendor"   UniqueName="VendorCode" DataField="VendorCode">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn HeaderText="Vendor Name"   UniqueName="VendorName" DataField="VendorName">
                                                </telerik:GridBoundColumn>                                                   
                                                   <telerik:GridBoundColumn   HeaderText="Paid Amount"  UniqueName="Amount"  DataField="ApprovedAmount" DataFormatString="{0:###,##0.00}">
                                                </telerik:GridBoundColumn>   
                                                   <telerik:GridTemplateColumn HeaderText="Reverse Reason"   HeaderStyle-Width="200px"  AllowFiltering="true"  UniqueName="ReverseReason" ItemStyle-VerticalAlign="Middle" >
                                                    <ItemStyle Width="170px" />
                                                    <ItemTemplate>                                                    
                                                        <telerik:RadComboBox EmptyMessage="-Select-" RenderMode="Lightweight"  Width="170px" ID="cmbReverseReason" runat="server">
                                                        </telerik:RadComboBox> 
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridBoundColumn HeaderText="FiscalYear"  UniqueName="FiscalYear" DataField="FiscalYear">
                                                </telerik:GridBoundColumn>  
                                             <telerik:GridTemplateColumn HeaderText="Reversal Date " AllowFiltering="false" UniqueName="ReverseDate" >
                                                 <ItemTemplate>
                                                     <telerik:RadDatePicker ID="dtpRevDate" runat="server" Width="100%" EnableTyping="false"  RenderMode="Lightweight"></telerik:RadDatePicker>
                                                 </ItemTemplate>
                                             </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn  HeaderText="Reverse" AllowFiltering="false" AllowSorting="false" UniqueName="Reverse">                                                
                                                    <ItemTemplate>
                                                        <telerik:RadButton ID="btnReverse" runat="server" CommandName="Reverse" CommandArgument='<%# Eval("DocumentClearingNo") %>' Text="Reversal" CssClass="gridHyperlinks"></telerik:RadButton>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>

                                    </Columns>

                                 </MasterTableView>

                             </telerik:RadGrid>
                        </div>
                     
                        </telerik:RadPageView>

                        <telerik:RadPageView runat="server" ID="RadPageView4">
                             <div class="col-sm-12">   
                     <%--   <telerik:RadGrid ID="gvReversedRequest" runat="server" OnNeedDataSource="gvReversedRequest_NeedDataSource" AllowPaging="true" PageSize="10"
                            AutoGenerateColumns="false">
                            <MasterTableView CommandItemSettings-ShowRefreshButton="false" EnableNoRecordsTemplate="true" EditMode="InPlace" CommandItemDisplay="None" ClientIDMode="AutoID"
                                     AllowFilteringByColumn="true" AllowSorting="false" NoMasterRecordsText=""  DataKeyNames="DocumentClearingNo">

                            <Columns>
                                      <telerik:GridBoundColumn  HeaderText="Payment Lot No." UniqueName="CheckLotNumber" DataField="LotNo">
                                          </telerik:GridBoundColumn>
                                </Columns>

                            </MasterTableView>


                        </telerik:RadGrid>--%>
                                   <telerik:RadGrid RenderMode="Lightweight" ID="gvReversedRequest" runat="server" AutoGenerateColumns="false"  ClientIDMode="AutoID" 
                                 OnNeedDataSource="gvReversedRequest_NeedDataSource"  AllowPaging="true" PageSize="10" AllowSorting="true">

                           
                                  <MasterTableView CommandItemSettings-ShowRefreshButton="false" EnableNoRecordsTemplate="true" EditMode="InPlace" CommandItemDisplay="None" ClientIDMode="AutoID"
                                    AllowFilteringByColumn="true" AllowSorting="false" NoMasterRecordsText=""  DataKeyNames="DocumentClearingNo">
                                      <Columns>

                                          <telerik:GridBoundColumn  HeaderText="Payment Lot No."   UniqueName="CheckLotNumber" DataField="LotNo">
                                          </telerik:GridBoundColumn>
                                          <telerik:GridBoundColumn HeaderText="Company Code" UniqueName="CompanyCode" DataField="CompanyCode">
                                          </telerik:GridBoundColumn>
                                          <telerik:GridBoundColumn HeaderText="Document Clearing No" UniqueName="DocumentClearingNo" DataField="DocumentClearingNo">
                                          </telerik:GridBoundColumn>
                                          <telerik:GridDateTimeColumn HeaderText="Posting Date" AllowFiltering="true" ItemStyle-Width="150px" HeaderStyle-Width="100px" FilterControlWidth="100px" UniqueName="PostingDate" EnableTimeIndependentFiltering="true"
                                              DataType="System.DateTime" DataFormatString="{0:dd-MM-yyyy}" DataField="PostingDate">
                                          </telerik:GridDateTimeColumn>
                                          <telerik:GridBoundColumn HeaderText="Vendor" UniqueName="VendorCode" DataField="VendorCode">
                                          </telerik:GridBoundColumn>
                                          <telerik:GridBoundColumn HeaderText="Vendor Name" UniqueName="VendorName" DataField="VendorName">
                                          </telerik:GridBoundColumn>
                                          <telerik:GridBoundColumn HeaderText="Paid Amount" UniqueName="Amount" DataField="ApprovedAmount" DataFormatString="{0:###,##0.00}">
                                          </telerik:GridBoundColumn>
                                           <telerik:GridBoundColumn HeaderText="Reverse Reason" UniqueName="ReverseReason" DataField="ReverseReason">
                                          </telerik:GridBoundColumn>                                         
                                          <telerik:GridBoundColumn HeaderText="FiscalYear" UniqueName="FiscalYear" DataField="FiscalYear">
                                          </telerik:GridBoundColumn>
                                         <telerik:GridDateTimeColumn HeaderText="Reversal Date" AllowFiltering="true" ItemStyle-Width="100px" HeaderStyle-Width="100px" FilterControlWidth="200px" UniqueName="ReverseDate" EnableTimeIndependentFiltering="true"
                                              DataType="System.DateTime" DataFormatString="{0:dd-MM-yyyy}" DataField="ReverseDate">
                                          </telerik:GridDateTimeColumn>
                                      </Columns>
                                 </MasterTableView>
                            </telerik:RadGrid>
                                 </div>

                        </telerik:RadPageView>

                    </telerik:RadMultiPage>
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
                            <MasterTableView CommandItemSettings-ShowRefreshButton="false" ClientDataKeyNames="" EditMode="InPlace" CommandItemDisplay="None" AllowFilteringByColumn="false">
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
                                    <telerik:GridBoundColumn DataField="UserName" HeaderText="Name" UniqueName="Name" HeaderStyle-Width="100px">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="WorkFlowStatus" HeaderText="Status" UniqueName="WorkFlowStatus" HeaderStyle-Width="100px">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="AppovedAmount" HeaderText="Approved Amount" UniqueName="AppovedAmount" HeaderStyle-Width="100px" DataFormatString="{0:###,##0.00}">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Comment" HeaderText="Comment" UniqueName="Comment" HeaderStyle-Width="250px">
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
        <div class="modal fade" id="divViewAllocation" tabindex="-1" role="dialog" aria-labelledby="modalLabel" aria-hidden="true">
            <div class="modal-dialog1">
                <div class="modal-content">
                    <div class="modal-header modal-header-resize">
                        <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">×</span><span class="sr-only">Close</span></button>
                        <h4 class="modal-title text-primary" id="lineModalLabel">Treasury Allocation</h4>
                    </div>


                    <%--     <telerik:RadAjaxPanel runat="server" ID="AllocationPanel">--%>
                    <asp:Panel ID="pnl1" runat="server">
                        <div style="height: 500px; overflow: auto;">
                            <telerik:RadGrid RenderMode="Lightweight" ID="grdBudgetAllocation" ClientIDMode="AutoID" runat="server" OnNeedDataSource="grdAttachments_NeedDataSource" OnItemDataBound="grdBudgetAllocation_ItemDataBound"
                                AutoGenerateColumns="false" EnableViewState="true" ShowFooter="true"
                                AllowPaging="false" AllowSorting="true" AllowFilteringByColumn="true">
                                <GroupingSettings CaseSensitive="false" />
                                <MasterTableView CommandItemDisplay="None" DataKeyNames="BalanceAmount,SubVerticalId,TreasuryDetailId,IsExpired" ClientDataKeyNames="BalanceAmount,SubVertical" EnableViewState="true" AllowPaging="false" AutoGenerateColumns="false"
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
                                        <telerik:GridBoundColumn DataField="ValidationDate" DataFormatString="{0:dd-MM-yyyy hh:mm tt}" HeaderText="Valid Upto" UniqueName="ValidationDate" DataType="System.String" ReadOnly="true" />
                                        <telerik:GridBoundColumn DataField="TotalAmount" HeaderText="Total Amount" UniqueName="TotalAmount" DataType="System.String" ReadOnly="true" />
                                        <telerik:GridBoundColumn DataField="Utilised" HeaderText="Utilised Amount" UniqueName="Utilised" DataType="System.String" ReadOnly="true" />
                                        <telerik:GridBoundColumn DataField="ToBeUtilised" HeaderText="To-Be Utilised Amount" FooterStyle-Width="20%" FooterStyle-HorizontalAlign="Right" FooterStyle-Font-Bold="true" UniqueName="ToBeUtilised" DataType="System.String" ReadOnly="true" />
                                        <telerik:GridTemplateColumn UniqueName="TemplateBalanceAmount" HeaderText="Balance Amount" HeaderStyle-Width="5%"  DataField="BalanceAmount">
                                            <ItemTemplate>
                                                <telerik:RadLabel runat="server" ID="lblBalanceAmount" Text='<%#Eval("BalanceAmount") %>'></telerik:RadLabel>
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
                    <%-- </telerik:RadAjaxPanel>--%>
                &nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Label runat="server" ID="Label1" Text="Total Balance:"></asp:Label>
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
                                    AllowFilteringByColumn="false" AllowSorting="false" NoMasterRecordsText="" Width="100%">
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
                                        <telerik:GridBoundColumn HeaderText="Approved Amount" DataField="ApprovedAmount" UniqueName="ApprovedAmount" DataFormatString="{0:###,##0.00}">
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
                </div>
            </div>
        </div>


</asp:Content>
