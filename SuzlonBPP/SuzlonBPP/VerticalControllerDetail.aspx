<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/SuzlonBPP.Master" CodeBehind="VerticalControllerDetail.aspx.cs" Inherits="SuzlonBPP.VerticalControllerDetail" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="Scripts/Page/UserMaster.js"></script>
    <script src="Scripts/bootstrap.min.js"></script>
   <%-- <script src="<%=ConfigurationManager.AppSettings["WebsiteUrl"].ToString()%>/Scripts/jquery.min.js"></script>--%>
     <script src="Scripts/jquery.min.js"></script>
    <style type="text/css">

        .RadUpload .ruFakeInput {
            min-height: 29px !important;
            border-radius: 4px !important;
        }

        .RadUpload_Default .ruSelectWrap .ruButton {
            background-color: #00988c !important;
            background-image: none !important;
            min-height: 29px !important;
            color: #FFF !important;
            border-color: #00988C !important;
        }

        .RadUpload .ruSelectWrap {
            display: inline-block;
            float: left;
        }

        .table-scroll {
            overflow-x: scroll !important;
        }

        .RadWindow .rwIcon {
            display: none !important;
        }

        .RadWindow .rwTitleWrapper .rwTitle {
            margin: 2px 0 0 -21px !important;
            font-weight: bold !important;
        }
        .RightAligned {
            text-align: right;
        }

        .paddingt-9 {
            padding-top: 9px;
        }

        .modal-dialog {
            width: 700px !important;
        }

        .RadWindow .rwTitleRow, .RadWindow .rwTitleRow * {
            background-color: #00988C !important;
            color: #FFF !important;
        }

        .RadWindow_Default {
            /*background-color: transparent !important;*/
            background-color: white !important;
            color: #333 !important;
            border-color: transparent !important;
        }

            .RadWindow_Default .rwTopLeft, .RadWindow_Default .rwTopRight, .RadWindow_Default .rwTitlebar, .RadWindow_Default .rwTopResize, .RadWindow_Default .rwStatusbar div, .RadWindow_Default .rwStatusbar, .RadWindow_Default .rwPopupButton, .RadWindow_Default .rwPopupButton span, .RadWindow_Default.rwMinimizedWindow .rwCorner {
                background-image: none !important;
            }

            .RadWindow_Default .rwTable .rwTitlebarControls em {
                color: #FFF;
            }

        .button-wrapper li {
            padding: 0px 0px 0px 0px;
            display: inline-block;
            *display: inline;
            *zoom: 1;
        }

        .button-wrapper {
            margin: 5px auto;
            overflow: hidden;
        }

        table {
            background-color: transparent;
            width: 100% !important;
        }

        .RadGrid {
            width: 100% !important;
        }

        .lable-txt {
            line-height: 35px !important;
        }

        .border-11 {
            border: 1px solid rgb(204, 204, 204) !important;
            padding: 0px 5px !important;
            min-height: 5px !important;
            background-color: #ECECEC;
border-radius: 4px;
        }



        /*.RadGrid_Default .rgEdit {
             background-image: url('Content/images/WebResource.axd.gif') !important;
            background-repeat: no-repeat !important;
            background-position: 0px -1697px !important;
        } 
 .RadGrid_Default .rgDel{
     background-image: url('Content/images/WebResource.axd.gif') !important;
    background-repeat:no-repeat !important;   
    background-position: 0px -1747px !important;
 }*/
    </style>

    <telerik:RadCodeBlock ID="rcb" runat="server">
        <script type="text/javascript">
           
            function closemodel() {
                debugger;
                dateVar = new Date(); 
                var datepicker = $find("<%= rdpAddendumDate.ClientID %>"); 
                datepicker.set_selectedDate(dateVar); 
            };
                    
            function confirmLinkButton(button) {
                function linkButtonCallbackFn(arg) {
                    if (arg) {
                        //obtains a __doPostBack() with the correct UniqueID as rendered by the framework
                        eval(button.href);
                        //can be used in a simpler environment so that event validation is not triggered.
                        //__doPostBack(button.id, "");
                    }
                }
                radconfirm("Entered data will be lost. Do you want to continue?", linkButtonCallbackFn,370, 100, null, "Alert","");
            }

            <%--     function OpenBudgetUtilization() {
                debugger;
                //var oWnd = GetRadWindowManager().getWindowByName("BudgetPopup");
                //oWnd.show();
                if ($find('<%= radComboRequestType.ClientID %>').get_selectedItem().get_value() == 'Automatic' || ($find('<%= radComboRequestType.ClientID %>').get_selectedItem().get_value() == 'Manual')) {
                    var oWnd = GetRadWindowManager().getWindowByName("BudgetPopup");
                    oWnd.show();
                    return false;
                }
            }--%>

            function OpenBudgetUtilization() {
                debugger;
                if  ($find('<%= radComboRequestType.ClientID %>').get_selectedItem().get_value() == 'Others')
                {
                    var oWnd = $find('<%=BudgetPopup.ClientID %>');
              
                    oWnd.show();
                }
                else if ($find('<%= radComboRequestType.ClientID %>').get_selectedItem().get_value() == 'NEFT')
                {
                     var manager = $find("<%= RadWindowManager.ClientID %>");
                    manager.open("AutomaticBudgetUtilisation.aspx", "RadWindow");
                }
              }

            function OnClientBeforeShow(sender, eventArgs) {
                if ($find('<%= radComboRequestType.ClientID %>').get_selectedItem().get_value() == 'NEFT') {
                    $get("<%=AutomaticBudget.ClientID %>").style.display = 'block';
                    $get("<%=ManualBudget.ClientID %>").style.display = 'none';
                    sender.set_height(200);

                }

                if ($find('<%= radComboRequestType.ClientID %>').get_selectedItem().get_value() == 'Others') {
                    $get("<%=AutomaticBudget.ClientID %>").style.display = 'none';
                    $get("<%=ManualBudget.ClientID %>").style.display = 'block';
                    sender.set_height(385);
                }

            }

            function CloseBudgetUtilization() {
                var window = $find('<%=BudgetPopup.ClientID %>');
                window.close();
            }

            <%--function OnClientClose(sender, eventArgs) {
                var ajaxManager = $find("<%= RadAjaxManager1.ClientID %>");
                //ajaxManager.ajaxRequest("ClearBudgetUtilization");
            }--%>

            function ValidateForm() {
                debugger;

                var addendumstatus = $find("<%= radCmbAddenumStatus.ClientID %>");
                if (addendumstatus != null)
                    var valueFind = addendumstatus.get_value();

                var comment = $("#ctl00_ContentPlaceHolder1_txtAddendumComment").val()

                //startDate = new Date($("#ctl00_ContentPlaceHolder1_rdpStart_dateInput").attr('value'));
                //endDate = new Date($("#ctl00_ContentPlaceHolder1_rdpEnd_dateInput").attr('value'));
                startDate = $("#ctl00_ContentPlaceHolder1_rdpStart_dateInput").attr('value');
                endDate = $("#ctl00_ContentPlaceHolder1_rdpEnd_dateInput").attr('value');

                jsonString = $("#ctl00_ContentPlaceHolder1_rdpAddendumDate_dateInput_ClientState").attr('value');
                jobject = JSON.parse(jsonString);
                // adnDate = new Date(jobject.lastSetTextBoxValue);

                adnDate = jobject.lastSetTextBoxValue;

                ReqAmount = $("#ctl00_ContentPlaceHolder1_radNumAddendumAmount").attr('value');
               // AppAmount = JSON.parse($("#ctl00_ContentPlaceHolder1_radNumAddendumAppAmount_ClientState").attr('value')).valueAsString;
                AppAmount = JSON.parse($("#ctl00_ContentPlaceHolder1_radNumAddendumAppAmount_ClientState").attr('value')).lastSetTextBoxValue;
                if (ReqAmount == undefined || ReqAmount == null) {
                    ReqAmount = 0;
                }

                if (AppAmount == undefined || AppAmount == null) {
                    AppAmount = 0;
                }

                if (adnDate < startDate || adnDate > endDate) {
                    $("#ContentPlaceHolder1_lblRangeErr").html("Please Select Vaild Addendum Date Within Utilisation Period.");
                    return false;
                }
                //  else if (parseFloat(ReqAmount.replace(',', '')) < parseFloat(AppAmount.replace(',', ''))) {
                else if (parseFloat(ReqAmount.replace(/,/g, '')) < parseFloat(AppAmount.replace(/,/g, ''))) {
                    $("#ContentPlaceHolder1_lblRangeErr").html("Approved Amount Cannot Be Greater Than Requested Amount.");
                    return false;
                }
                else if ((valueFind == "2" || valueFind == "3") && (comment == ""||comment=="Enter Comment")) {
                    $("#ContentPlaceHolder1_lblRangeErr").html("Please Enter Comment.");
                    return false;
                }
                else {
                    $("#ContentPlaceHolder1_lblRangeErr").html("");
                    return;
                }

            }
            function keyPress(sender, args) {
                debugger;
                //var text = sender.get_value() + args.get_keyCharacter();
                //if (!text.match('^[0-9]+$'))
                //    args.set_cancel(true);
                var text = sender.get_value() + args.get_keyCharacter();
                var keycode = args.get_keyCode()
                if (!((keycode >= 48 && keycode <= 57) || keycode == 8) || (keycode >= 96 && keycode <= 105)) {
                    args.set_cancel(true);

                }
            }

            function openRadWin(screen, mode, entityId, canAdd, canDelete, isMultiFileUpload, showDocumentType, entityName) {
   
                var manager = $find("<%= RadWindowManager.ClientID %>");
                 //if (screen == 'VC') {
                 manager.open("AddAttachments.aspx?mode=" + mode + "&entityId=" + entityId + "&canAdd= " + canAdd + "&canDelete= " + canDelete + "&isMultiUpload= " + isMultiFileUpload + "&showDtype= " + showDocumentType + "&entityName=" + entityName, "RadWindow");
                 return false;
                 //  }
             }

             function EnterKeyForAmounttxt(sender, eventArgs) {

                 //  alert("Key code: " + eventArgs.get_keyCode());
                 if (eventArgs.get_keyCode() == 13) {
                     sender.set_autoPostBack(true);
                     //args.set_(true);
                 } else {
                     sender.set_autoPostBack(false);
                 }
             }
        </script>
    </telerik:RadCodeBlock>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
           /* background-image: url(../Content/images/loading.gif);*/
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
<%--    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script>
     function ResponseEnd() {
             
                if (currentLoadingPanel != null)
                    currentLoadingPanel.hide(currentUpdatedControl);
                currentUpdatedControl = null;
                currentLoadingPanel = null;
            }
              </script>
    </telerik:RadCodeBlock>--%>

    <telerik:RadAjaxManager EnablePageHeadUpdate="false" ID="RadAjaxManager1" runat="server" OnAjaxRequest="RadAjaxManager1_AjaxRequest"  >
        <AjaxSettings>
            
             <telerik:AjaxSetting AjaxControlID="lbCancel">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="lbCancel" LoadingPanelID="LoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
               <telerik:AjaxSetting AjaxControlID="lbSubmit">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="lbSubmit" LoadingPanelID="LoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="lbSave">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="lbSave" LoadingPanelID="LoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="btnAddNew">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="gridNatureOfRequest" LoadingPanelID="LoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="radComboRequestType" LoadingPanelID="LoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="radMessage" LoadingPanelID="LoadingPanel1" />
                    

                </UpdatedControls>
            </telerik:AjaxSetting>
              <telerik:AjaxSetting AjaxControlID="gridNatureOfRequest">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="gridNatureOfRequest" LoadingPanelID="LoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
             <telerik:AjaxSetting AjaxControlID="drpSubVertical">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="drpVertical" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>

              <telerik:AjaxSetting AjaxControlID="btnComment">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="lblCommentDetail" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                     <telerik:AjaxUpdatedControl ControlID="txtCommentBox" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    
                </UpdatedControls>
            </telerik:AjaxSetting>

             <telerik:AjaxSetting AjaxControlID="lkbtnAddBudeget">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadAjaxManager1" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
     <%--      <telerik:AjaxSetting AjaxControlID="RadAjaxManager1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="gridInverter" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>--%>
            <telerik:AjaxSetting AjaxControlID="RadAjaxManager1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="BudgetPopup" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>                   
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="btnAddRange">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="BudgetPopup" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="rgridManualUtilisation" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="rgridManualUtilisation">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rgridManualUtilisation" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="radMessage" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>

                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="rgridAddandumHistory">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rgridAddandumHistory" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="gvCommentApproveAddendum" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>

                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="buttonSubmit">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdAttachments" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="RadAsyncUpload" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="txtDType" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>                    
                </UpdatedControls>
            </telerik:AjaxSetting>
             <telerik:AjaxSetting AjaxControlID="grdAttachments">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdAttachments" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="lnkSaveAddendum">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="lnkSaveAddendum" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lnkSubmitAddendum">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="lnkSubmitAddendum" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>

        </AjaxSettings>
         
    </telerik:RadAjaxManager>


  <%--  <telerik:RadAjaxLoadingPanel ID="LoadingPanel1" runat="server" Skin="Default">
    </telerik:RadAjaxLoadingPanel>--%>

    <telerik:RadAjaxLoadingPanel ID="LoadingPanel1"  BackgroundPosition="Center" runat="server" Height="100%" Width="75px"
         Transparency="50" >
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
    <div class="container-fluid padding-0 bg-white">
        <div class="row margin-0">
            <div class="col-xs-6 heading-big">
                <h5 class="margin-0 lineheight-42 breath-ctrl">Home / <span>Treasury Allocation Request</span></h5>
            </div>
            <div class="col-sm-6 col-xs-6 ">
            </div>
        </div>
        <!-- End of search Criteria-->
        <div class="col-xs-12 padding-0">
            <div class="col-xs-12 margin-10">
                <!-- Start of form Section-1-->
                <div class="col-sm-6 col-xs-12">
                    <!-- Start of date wrapper-->
                    <div class="col-xs-12 padding-0">
                        <div class="col-sm-4 col-xs-12 padding-0">
                            <label class="control-label lable-txt" for="name">Creation Date</label>
                        </div>
                        <div class="col-sm-8 col-xs-12 padding-0">
                            <div class="form-group">
                                <telerik:RadDatePicker ID="rdpCreatedDate" runat="server" Width="100%" ></telerik:RadDatePicker>

                            </div>
                        </div>
                    </div>
                    <!-- End of date wrapper-->

                    <!-- Start of date wrapper-->
                    <div class="col-xs-12 padding-0">
                        <div class="col-sm-4 col-xs-12 padding-0">
                            <label class="control-label lable-txt" for="name">Proposed Payment Date</label><small><i>&nbsp;On/Before</i></small>
                        </div>
                        <div class="col-sm-8 col-xs-12 padding-0">
                                <telerik:RadDatePicker ID="rdpPayDate" runat="server" Width="100%" DateInput-EmptyMessage="Select Proposed Payment Date" ></telerik:RadDatePicker >
                                <asp:RequiredFieldValidator ValidationGroup="FormLevel" runat="server" ID="RFVrdpPayDate" ControlToValidate="rdpPayDate" ErrorMessage="* Required" ForeColor="Red"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <!-- End of date wrapper-->

                    <!-- Start of Company Code wrapper-->
                    <div class="col-xs-12 padding-0">
                        <div class="col-sm-4 col-xs-12 padding-0">
                            <label class="control-label lable-txt" for="name">Company Name</label>
                        </div>
                        <div class="col-sm-8 col-xs-12 padding-0">

                            <telerik:RadComboBox ID="radcomboCmpy" RenderMode="Lightweight" runat="server" DataTextField="Name" EmptyMessage="Select Company Code" DataValueField="Id">
                            </telerik:RadComboBox>
                            <asp:RequiredFieldValidator ValidationGroup="FormLevel" runat="server" ID="RFVradcomboCmpy" ControlToValidate="radcomboCmpy" ErrorMessage="* Required" ForeColor="Red"></asp:RequiredFieldValidator>

                        </div>
                    </div>
                    <!-- End of Company Code wrapper-->

                    <!-- Start of Sub Vertical wrapper-->
                    <div class="col-xs-12 padding-0">
                        <div class="col-sm-4 col-xs-12 padding-0">
                            <label class="control-label lable-txt" for="name">Sub Vertical</label>
                        </div>
                        <div class="col-sm-8 col-xs-12 padding-0">
                            <telerik:RadComboBox ID="drpSubVertical" EmptyMessage="Select Sub Vertical" RenderMode="Lightweight" runat="server" DataTextField="Name" DataValueField="Id" AutoPostBack="true" OnSelectedIndexChanged="drpSubVertical_SelectedIndexChanged">
                            </telerik:RadComboBox>
                            <asp:RequiredFieldValidator ValidationGroup="FormLevel" runat="server" ID="RFVradComboSubVertical" ControlToValidate="drpSubVertical" ErrorMessage="* Required" ForeColor="Red"></asp:RequiredFieldValidator>

                        </div>
                    </div>
                    <!-- End of sub Vertical wrapper-->

                    <!-- Start of Vertical wrapper-->
                    <div class="col-xs-12 padding-0">
                        <div class="col-sm-4 col-xs-12 padding-0">
                            <label class="control-label lable-txt" for="name">Vertical</label>
                        </div>
                        <div class="col-sm-8 col-xs-12 padding-0">

                            <telerik:RadComboBox ID="drpVertical" EmptyMessage="Select Vertical" RenderMode="Lightweight" runat="server" DataTextField="Name" DataValueField="Id" AutoPostBack="true">
                            </telerik:RadComboBox>

                        </div>
                    </div>
                    <!-- End of Vertical wrapper-->

                    <div class="col-xs-12 padding-0" runat="server" id="divAllocationNo" style="padding-top: 11px !important;">
                        <div class="col-sm-4 col-xs-12 padding-0">
                            <label class="control-label lable-txt" for="Requested Amount">Treasury Allocation Number</label>
                        </div>
                        <div class="col-sm-8 col-xs-12 padding-0 verticalctrl-textbox">

                            <asp:Label ID="lbTreasuryAllocNo" CssClass="control-label lable-txt text-muted " Text="00.00" runat="server" ></asp:Label>

                        </div>
                    </div>

                    <!-- End of Treasury Allocation Number wrapper-->

                    <!-- Start of Request Type wrapper-->
                    <div class="col-xs-12 padding-0" style="margin-top:5px;">
                        <div class="col-sm-4 col-xs-12 padding-0">
                            <label class="control-label lable-txt" for="name">Request Type</label>
                        </div>
                        <div class="col-sm-8 col-xs-12 padding-0">

                            <telerik:RadComboBox ID="radComboRequestType" EmptyMessage="Select Request Type" OnSelectedIndexChanged="radComboRequestType_SelectedIndexChanged" RenderMode="Lightweight" runat="server">
                                <Items>
                                    <%--<telerik:RadComboBoxItem Text="Automatic" Value="Automatic" />
                                    <telerik:RadComboBoxItem Text="Manual" Value="Manual" />--%>
                                    <telerik:RadComboBoxItem Text="NEFT" Value="NEFT" />
                                    <telerik:RadComboBoxItem Text="Others" Value="Others" />
                                </Items>
                            </telerik:RadComboBox>
                            <asp:RequiredFieldValidator ValidationGroup="FormLevel" runat="server" ID="RFVRequestType" ControlToValidate="radComboRequestType" ErrorMessage="* Required" ForeColor="Red"></asp:RequiredFieldValidator>

                        </div>
                    </div>
                    <!-- End of Nature of Request wrapper-->
                    <div class="row margin-0">
                        <div class="col-xs-6 heading-big">
                            <h5 class="margin-0 lineheight-42 breath-ctrl"></h5>
                        </div>
                        <div class="col-sm-6 col-xs-6 list-btn padding-0">
                            <ul class="list-btn ">
                                <li>
                                    <telerik:RadButton RenderMode="Lightweight" ID="btnAddNew" runat="server" Text="Add Nature of Request" AutoPostBack="true" CausesValidation="false" OnClick="btnAddNew_Click">
                                        <Icon PrimaryIconCssClass="rbAdd"></Icon>
                                    </telerik:RadButton>
                                </li>
                            </ul>
                        </div>
                    </div>
                    <div class="col-xs-12 padding-0 overflow-x">
                        <table>
                            <tbody>
                                <tr>
                                    <td>
                                        <telerik:RadGrid RenderMode="Lightweight" ID="gridNatureOfRequest" GridLines="None" AutoGenerateColumns="false" ShowHeadersWhenNoRecords="true"
                                            AllowPaging="true" AllowSorting="true" runat="server" OnItemDataBound="OnItemDataBoundHandler" OnPreRender="RadGvScope_PreRender"
                                            PageSize="<%# Convert.ToInt32(SuzlonBPP.Models.Constants.CONST_INNERGRID_PAGE_SIZE) %>"
                                            AllowAutomaticUpdates="true" AllowAutomaticInserts="True" OnUpdateCommand="gridNatureOfRequest_UpdateCommand" OnNeedDataSource="gridNatureOfRequest_NeedDataSource"
                                            ShowStatusBar="true" OnItemCommand="gridNatureOfRequest_ItemCommand" OnInsertCommand="gridNatureOfRequest_InsertCommand">
                                            <MasterTableView ShowFooter="false" EditMode="InPlace" CommandItemDisplay="None" EnableNoRecordsTemplate="true">
                                                <CommandItemSettings ShowRefreshButton="false" />
                                                <NoRecordsTemplate>
                                                    <table width="100%" border="0" cellpadding="20" cellspacing="20">
                                                        <tr>
                                                            <td align="center" class="txt-white">No records to display.
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </NoRecordsTemplate>
                                                <Columns>
                                                    <telerik:GridTemplateColumn UniqueName="NatureOfRequest" HeaderText="Nature of Request"
                                                        SortExpression="NatureOfRequest" HeaderStyle-Width="6000px">
                                                        <ItemTemplate>
                                                            <telerik:RadLabel ID="lblNatureOfRequest" runat="server" Text='<%# Eval("NatureOfRequestText")%>' />
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <table width="100%">
                                                                <tr>
                                                                    <td style="width: 300px !important;">
                                                                        <telerik:RadComboBox MarkFirstMatch="True" runat="server" ID="radComboNatureOfRequest" AutoPostBack="false"
                                                                            Height="140px" Width="700px" DropDownWidth="200px" EmptyMessage="Select Nature of Request" Filter="Contains">
                                                                            <ItemTemplate>
                                                                                <ul>
                                                                                    <li class="col1">
                                                                                        <%# Eval("Name")%>
                                                                                    </li>
                                                                                </ul>
                                                                            </ItemTemplate>
                                                                        </telerik:RadComboBox>
                                                                        <asp:RequiredFieldValidator ID="RFVNaturOfReq" runat="server" ForeColor="Red" ErrorMessage="* Required" ControlToValidate="radComboNatureOfRequest"></asp:RequiredFieldValidator>
                                                                    </td>

                                                                </tr>
                                                            </table>
                                                        </EditItemTemplate>
                                                        <HeaderStyle HorizontalAlign="left" />
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="Amount" HeaderText="Requested Amount" ItemStyle-HorizontalAlign="Right"
                                                        SortExpression="Amount" ItemStyle-Width="300px">
                                                        <ItemTemplate>
                                                            <%--<telerik:RadLabel ID="lblAmount" runat="server" Text='<%# Eval("Amount")%>' />--%>
                                                            <telerik:RadLabel ID="lblAmount" runat="server" Text='<%#String.Format("{0:#,###}", Eval("Amount")) %>' />
                                                             
                                                           
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <table>
                                                                <tr>
                                                                    <td style="padding-top: 6px;">
                                                                        <telerik:RadNumericTextBox NumberFormat-DecimalDigits="0" MinValue="0.00001" runat="server" ID="RadNumAmount" Width="150px">
                                                                            <NumberFormat DecimalDigits="2" AllowRounding="true" />
                                                                        </telerik:RadNumericTextBox>
                                                                        <asp:RequiredFieldValidator ID="RFVAmount" runat="server" ForeColor="Red" ErrorMessage="* Required" ControlToValidate="RadNumAmount"></asp:RequiredFieldValidator>
                                                                        <asp:CompareValidator ID="CmpAmount" runat="server" ControlToValidate="RadNumAmount" ErrorMessage="* Required" Display="Dynamic" ForeColor="Red" ValueToCompare="0" Operator="GreaterThan"></asp:CompareValidator>
                                                                    </td>

                                                                </tr>
                                                            </table>
                                                        </EditItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <HeaderStyle HorizontalAlign="left" Width="300px" />
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="ApprovedAmount" HeaderText="Approved Amount" ItemStyle-HorizontalAlign="Right"
                                                        SortExpression="ApprovedAmount" ItemStyle-Width="300px">
                                                        <ItemTemplate>
                                                           <%-- <telerik:RadLabel ID="lblReqAmount" runat="server" Text='<%# Eval("ApprovedAmount")%>' />--%>
                                                             <telerik:RadLabel ID="lblReqAmount" runat="server" Text= '<%#String.Format("{0:#,###}", Eval("ApprovedAmount")) %>' />

                                                           
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <table>
                                                                <tr>
                                                                    <td style="padding-top: 6px;">
                                                                        <telerik:RadNumericTextBox NumberFormat-DecimalDigits="0" runat="server" ID="RadNumApprovedAmount" Width="150px">
                                                                            <NumberFormat DecimalDigits="2" AllowRounding="true" />
                                                                        </telerik:RadNumericTextBox>
                                                                        <asp:RequiredFieldValidator ID="RFVApprovedAmount" Enabled="false" runat="server" ForeColor="Red" ErrorMessage="* Required" ControlToValidate="RadNumApprovedAmount"></asp:RequiredFieldValidator>
                                                                        <asp:CompareValidator ID="CmpApprovedAmount" Enabled="false" runat="server" ControlToValidate="RadNumApprovedAmount" ErrorMessage="* Required" Display="Dynamic" ForeColor="Red" ValueToCompare="0" Operator="GreaterThan"></asp:CompareValidator>
                                                                    </td>

                                                                </tr>
                                                            </table>
                                                        </EditItemTemplate>
                                                        <ItemStyle HorizontalAlign="right" />
                                                        <HeaderStyle HorizontalAlign="left" />
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridEditCommandColumn HeaderText="Edit">
                                                        <HeaderStyle Width="80px" />
                                                    </telerik:GridEditCommandColumn>
                                                    <telerik:GridButtonColumn HeaderText="Delete" CommandName="Delete" Text="Delete" ConfirmText="Are You Sure You Wish To Delete This Record?" UniqueName="DeleteColumn">
                                                        <HeaderStyle Width="80px" />
                                                    </telerik:GridButtonColumn>
                                                </Columns>
                                                <PagerStyle Width="100px" ShowPagerText="false"
                                                    PageSizeLabelText=" " Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />
                                            </MasterTableView>
                                        </telerik:RadGrid>

                                    </td>
                                </tr>
                            </tbody>

                        </table>
                    </div>

                    <!-- Start of Requested Amount wrapper-->
                    <div class="col-xs-12 padding-0" style="margin-top: 5px;">
                        <div class="col-sm-4 col-xs-12 padding-0">
                            <label class="control-label lable-txt" for="Requested Amount">Requested Amount</label>
                        </div>
                        <div class="col-sm-8 col-xs-12 padding-0 verticalctrl-textbox">

                            <asp:Label CssClass="control-label lable-txt text-muted" ID="lblRequestedAmount"  Text="00.00" runat="server"></asp:Label>

                        </div>
                    </div>
                    <!-- End of Requested Amount wrapper-->

                    <!-- Start of Amount Approved wrapper-->
                    <div class="col-xs-12 padding-0">
                        <div class="col-sm-4 col-xs-12 padding-0">
                            <label class="control-label lable-txt" for="Requested Amount">Initial Approved Amount</label>
                        </div>
                        <div class="col-sm-8 col-xs-12 padding-0 verticalctrl-textbox">

                            <asp:Label CssClass="control-label lable-txt text-muted" ID="lblInitAmount" Text="00.00" runat="server"></asp:Label>

                        </div>
                    </div>
                    <!-- End of Amount Approved wrapper-->

                    <!-- Start of Attachment wrapper-->
                    <div class="col-xs-12 padding-0">
                        <div class="col-sm-4 col-xs-12 padding-0">
                            <label class="control-label lable-txt" for="Requested Amount">Addendum Total</label>
                        </div>
                        <div class="col-sm-8 col-xs-12 padding-0 verticalctrl-textbox">
                            <asp:Label CssClass="control-label lable-txt text-muted" ID="lblAddendumAmt" Text="00.00" runat="server"></asp:Label>

                        </div>
                    </div>
                    <!-- End of Attachment wrapper-->

                    <!-- Start of Final Amount wrapper-->
                    <div class="col-xs-12 padding-0">
                        <div class="col-sm-4 col-xs-12 padding-0">
                            <label class="control-label lable-txt" for="Requested Amount">Final Amount</label>
                        </div>
                        <div class="col-sm-8 col-xs-12 padding-0 verticalctrl-textbox">
                            <asp:Label CssClass="control-label lable-txt text-muted" ID="lblFinalAmt" Text="0.00" runat="server"></asp:Label>
                        </div>
                    </div>
                    <!-- End of Attachment wrapper-->

                    <!-- Start of Allocation Period wrapper-->
                    <div class="col-xs-12 padding-0" runat="server" id="divUtilzationPeriod">
                        <div class="col-sm-4 col-xs-12 padding-0">
                            <label class="control-label lable-txt" for="Requested Amount">Utilisation Period</label>
                        </div>
                        <div class="col-sm-8 col-xs-12 padding-0">
                            <div class="col-sm-6 padding-0">
                                <label class="control-label">Start</label>

                                <table width="100%">
                                    <tr>
                                        <td>
                                            <telerik:RadDatePicker Width="100%" ID="rdpStart" runat="server" ></telerik:RadDatePicker>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:RequiredFieldValidator runat="server" ID="RFVrdpStart" ValidationGroup="FormLevel" ControlToValidate="rdpStart" ErrorMessage="* Required"
                                                ForeColor="Red"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                </table>

                            </div>
                            <div class="col-sm-6">
                                <label class="control-label ">End</label>

                                <table width="100%">
                                    <tr>
                                        <td>
                                            <telerik:RadDatePicker ID="rdpEnd" runat="server" Width="100%" ></telerik:RadDatePicker>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:RequiredFieldValidator runat="server" ID="RFVrdpEnd" ValidationGroup="FormLevel" ControlToValidate="rdpEnd" ErrorMessage="* Required" ForeColor="Red"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                </table>

                            </div>
                            <%--<asp:CompareValidator ID="DCVTender" runat="server" ControlToValidate="rdpStart" ValidationGroup="FormLevel" ControlToCompare="rdpEnd" Operator="LessThanEqual" Type="Date" ForeColor="Red" ErrorMessage="The Start date must less than or equal to End date." />--%>
                        </div>

                    </div>
                    <!-- End of Allocation Period wrapper-->

                    <!-- Start of Balanced Amount wrapper-->
                    <div class="col-xs-12 padding-0">
                        <div class="col-sm-4 col-xs-12 padding-0">
                            <label class="control-label lable-txt" for="Balance Amoun">Balance Amount</label>
                        </div>
                        <div class="col-sm-8 col-xs-12 padding-0">

                            <span class="control-label lable-txt text-muted col-sm-6 col-xs-12 padding-0">
                                <asp:Label ID="lblBalanceAmount" Text="0.00" runat="server"></asp:Label>
                                &nbsp;                             
                                <a class="subheading" id="lkbtnAddBudeget" runat="server" href="javascript:void(0);" onclick="javascript:OpenBudgetUtilization();"><i>Budget Utilisation</i></a></span>


                        </div>
                    </div>
                    <!-- End of Balanced Amount wrapper-->

                    <!-- Start of Status wrapper-->
                    <div class="col-xs-12 padding-0" runat="server" id="divStatus">
                        <div class="col-sm-4 col-xs-12 padding-0">
                            <label class="control-label lable-txt" for="Requested Amount">Status</label>
                        </div>
                        <div class="col-sm-8 col-xs-12 padding-0">

                            <telerik:RadComboBox ID="rcbStatus" RenderMode="Lightweight" DataValueField="Id" DataTextField="Name" runat="server" EmptyMessage="Select Status">
                                <Items>
                                    <telerik:RadComboBoxItem Text="Approved" Value="1" />
                                    <telerik:RadComboBoxItem Text="Rejected" Value="2" />
                                    <telerik:RadComboBoxItem Text="Need Correction" Value="3" />
                                </Items>
                            </telerik:RadComboBox>
                            <asp:RequiredFieldValidator runat="server" ID="RFVStatus" ValidationGroup="FormLevel" ControlToValidate="rcbStatus" ErrorMessage="* Required" ForeColor="Red"></asp:RequiredFieldValidator>

                        </div>
                    </div>
                    <!-- End of Status wrapper-->

                <div class="col-sm-12 col-xs-12 padding-0">


                    <!-- Start of Comments/Remark wrapper-->
                    <div class="col-xs-12 padding-0 margin-10" style="margin-top: 0px !important;">
                        <div class="col-sm-4 col-xs-12 padding-0">
                            <label class="control-label lable-txt" for="Requested Amount">Comments/Remarks</label>
                        </div>
                        <div class="col-sm-8 col-xs-12 padding-0">
                            <div class="col-sm-12 col-xs-12 padding-0">
                                <div class="row  margin-0 chat-window col-xs-12 col-md-12 padding-0" id="chat_window_1">
                                    <div class="col-xs-12 col-md-12 padding-0">
                                        <div class="panel-body msg_container_base">
                                            <asp:Label ID="lblCommentDetail" CssClass="messages msg_sent" runat="server" Text=""></asp:Label>
                                        </div>
                                        <div class="panel-footer">
                                            <div class="input-group">
                                                <telerik:RadTextBox ID="txtCommentBox" TextMode="MultiLine" CssClass="form-control input-sm chat_input" Width="100%" runat="server" placeholder="Enter comments/remarks here..."></telerik:RadTextBox>
                                                <span class="input-group-btn">
                                                    <asp:LinkButton ID="btnComment" CssClass="btn btn-md btn-grey button-style" Text="Comment" runat="server" OnClick="btnComment_Click" />
                                                </span>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                    </div>
                </div>

                </div>
                <!-- End of form Section-1-->

                <!-- Start of Form Section-2-->

                <!-- End of Comments/Remark wrapper-->



                <!-- Start of Attachment wrapper-->
                <div class="col-xs-6">
                    <div class="col-sm-12 col-xs-12 padding-0">
                        <label class="control-label lable-txt" for="Requested Amount">Attachment</label>
                    </div>
                    <div class="col-sm-12 col-xs-12 padding-0">
                        <div class="overflow-x">
                            <div class="input-group">
                                <div id="add-attachment" class="filter-panel collapse in" aria-expanded="true">
                                    <div class="panel panel-default">
                                        <div class="panel-body ">
                                            <table class="table table-striped table-bordered" runat="server" id="tbAttachment">
                                                <thead>
                                                    <tr>
                                                        <th class="text-center valign" runat="server" id="lblDType">Document Text</th>
                                                        <th class="text-center valign">Attachment</th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    <tr class="text-center" runat="server" id="DDDType">
                                                        <td class="valign">
                                                            <asp:TextBox runat="server" RenderMode="Lightweight" ID="txtDType"></asp:TextBox>
                                                        </td>
                                                        <td class="valign">
                                                            <telerik:RadAsyncUpload RenderMode="Lightweight" ID="RadAsyncUpload" runat="server" Width="100%"  MultipleFileSelection="Automatic"
                                                                MaxFileSize="3000000" OverwriteExistingFiles="true">
                                                                <Localization Select="Browse" />
                                                            </telerik:RadAsyncUpload>
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                            <div class="col-xs-12 ">
                                                <div class="button-wrapper pull-left">
                                                    <ul>
                                                        <li>
                                                            <asp:LinkButton ID="buttonSubmit" runat="server" OnClick="buttonSubmit_Click" CssClass="btn btn-grey">Save</asp:LinkButton>
                                                        </li>
                                                    </ul>
                                                    <ul>
                                                        <li>
                                                            <telerik:RadNotification RenderMode="Lightweight" Position="Center" runat="server" ID="RadNotification1" VisibleOnPageLoad="false"
                                                                Width="350px" AutoCloseDelay="0" TitleIcon="none" ContentIcon="none">
                                                            </telerik:RadNotification>
                                                        </li>
                                                    </ul>
                                                </div>
                                            </div>
                                            <div class="col-xs-12">
                                                <div class="">
                                                    <telerik:RadGrid RenderMode="Lightweight" ID="grdAttachments" runat="server" OnNeedDataSource="grdAttachments_NeedDataSource"
                                                        OnItemDataBound="grdAttachments_ItemDataBound" AutoGenerateColumns="false" OnItemCommand="grdAttachments_ItemCommand" PageSize="5"
                                                        AllowPaging="true" AllowSorting="true" AllowFilteringByColumn="true">
                                                        <GroupingSettings CaseSensitive="false" />
                                                        <MasterTableView CommandItemDisplay="None" DataKeyNames="FileName,FileuploadId,CreatedOn,CreatedBy" EnableViewState="true"
                                                             AllowPaging="true" AutoGenerateColumns="false"
                                                            EnableNoRecordsTemplate="true" AllowFilteringByColumn="false">
                                                             <PagerStyle AlwaysVisible="true"/> 
                                                             <NoRecordsTemplate>
                                                                <table width="100%" border="0" cellpadding="20" cellspacing="20">
                                                                    <tr>
                                                                        <td align="center">No records to display.
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </NoRecordsTemplate>
                                                            <CommandItemSettings ShowRefreshButton="false" />
                                                            <Columns>
                                                                <telerik:GridBoundColumn DataField="createdBy" HeaderText="CreatedBy" UniqueName="createdBy" DataType="System.String" ReadOnly="true" Visible="false" />
                                                                <telerik:GridBoundColumn DataField="DisplayName" HeaderText="File Name" UniqueName="DisplayName" DataType="System.String" ReadOnly="true" />
                                                                <telerik:GridBoundColumn DataField="DocumentType" HeaderText="Document Text" UniqueName="DocumentType" DataType="System.String" ReadOnly="true" />
                                                                <telerik:GridBoundColumn DataField="CreatedOn" HeaderText="Upload Date" UniqueName="DocumentType" DataType="System.String" ReadOnly="true" />
                                                                <telerik:GridBoundColumn DataField="name" HeaderText="Upload By" UniqueName="UploadBy" DataType="System.String" ReadOnly="true" />
                                                                <telerik:GridTemplateColumn UniqueName="TemplateAttachment" HeaderText="View" HeaderStyle-Width="5%">
                                                                    <ItemTemplate>
                                                                        <asp:HyperLink ID="lkbtnView" runat="server" Text="View" CssClass="btn btn-grey gridHyperlinks"></asp:HyperLink>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridTemplateColumn UniqueName="DeleteAttachment" HeaderText="Delete" HeaderStyle-Width="5%">
                                                                    <ItemTemplate>
                                                                        <asp:Button ID="btn" runat="server" CommandName="Delete" Text="Delete" CssClass="btn btn-grey gridHyperlinks"
                                                                            OnClientClick="javascript:if(!confirm('Are you sure you want to delete an Attachment?')){return false;}"></asp:Button>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                            </Columns>
                                                            <PagerStyle PageSizes="5,10,15" Width="100px" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                                                                PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />
                                                        </MasterTableView>
                                                    </telerik:RadGrid>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <!-- /.form-group -->
                    </div>
                </div>
                <!-- End of Attachment wrapper-->

            </div>


            <!-- End of form Section-2-->

            <!-- Start of col-xs-12-->
            <div class="col-xs-12 margin-10">
                <div class="button-wrapper">
                    <ul>
                        <li>
                            <asp:LinkButton ID="lbSave" runat="server" Text="Save" ValidationGroup="FormLevel" CssClass="btn btn-grey" OnClick="lbSave_Click"></asp:LinkButton>

                        </li>
                        <li>
                            <asp:LinkButton ID="lbSubmit" runat="server" Text="Submit" ValidationGroup="FormLevel" CssClass="btn btn-grey" OnClick="lbSubmit_Click"></asp:LinkButton>

                        </li>
                        <li>
                            <a href="#" class="btn btn-grey" id="hrefaddendum" runat="server" visible="false" data-toggle="modal" data-target="#addendum-modal">Addendum</a>
                        </li>

                        <li>
                           <%-- <asp:LinkButton ID="lbCancel" runat="server" CausesValidation="false" Text="Cancel" CssClass="btn btn-grey" OnClientClick="if(!CancelPopUp()) return false;" OnClick="lbCancel_Click"></asp:LinkButton>--%>
                             <asp:LinkButton ID="lbCancel" runat="server" CausesValidation="false" Text="Cancel" CssClass="btn btn-grey" OnClientClick="confirmLinkButton(this); return false;" OnClick="lbCancel_Click"></asp:LinkButton>
                           
                        </li>


                    </ul>

                </div>
            </div>
            <!-- End of col-xs-12-->


        </div>
        <!-- End of grid-->
    </div>
    <!-- Start of modal for addendum request-->

    <!-- End of modal for addendum request-->

    <!-- Start of modal for addendum request-->
    <div class="modal fade" id="addendum-modal" tabindex="-1" role="dialog" aria-labelledby="modalLabel" aria-hidden="true">
        <div class="modal-dialog">

            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" onclick="closemodel();" data-dismiss="modal" ><span aria-hidden="true">×</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title subheading margin-0" id="lineModalLabel">Addendum Request</h4>
                </div>
                <div style="max-height: 550px; overflow-y: scroll; overflow-x: hidden;">
                    <div class="modal-body" style="overflow: hidden">

                        <!-- content goes here -->

                        <div class="col-xs-12 padding-0">
                            <div class="col-sm-6 col-xs-12 padding-0">
                                <label class="control-label lable-txt" for="Treasury Allocation Number">Treasury Allocation Number</label>
                            </div>
                            <div class="col-sm-6 col-xs-12 padding-0">
                                <div class="form-group border-11">
                                    <asp:Label ID="lblAddendumAllocationNo" CssClass="control-label lable-txt text-muted" Text="00.00" runat="server"></asp:Label>
                                </div>


                            </div>
                        </div>
                        <!-- End of Treasury Allocation Number wrapper-->
                        <div class="col-xs-12 padding-0">
                            <div class="col-sm-6 col-xs-12 padding-0">
                                <label class="control-label lable-txt" for="Requested Amount">Addendum Date</label>
                            </div>
                            <div class="col-sm-6 col-xs-12 padding-0">
                                <telerik:RadDatePicker ID="rdpAddendumDate" DateInput-EmptyMessage="Select Addendum Date" runat="server" ></telerik:RadDatePicker>
                                <asp:RequiredFieldValidator runat="server" ID="RFVAddendumDate" ValidationGroup="FormAddendum" ControlToValidate="rdpAddendumDate" ErrorMessage="* Required"
                                    ForeColor="Red"></asp:RequiredFieldValidator>
                                <br />
                            </div>
                        </div>
                        <div class="col-xs-12 padding-0">
                            <div class="col-sm-6 col-xs-12 padding-0">
                                <label class="control-label lable-txt" for="Requested Amount">Addendum Amount</label>
                            </div>
                            <div class="col-sm-6 col-xs-12 padding-0">

                                <telerik:RadNumericTextBox NumberFormat-DecimalDigits="0" EmptyMessage="Enter Addendum Amount" MinValue="0.00001" Width="100%" runat="server" ID="radNumAddendumAmount">
                                    <NumberFormat DecimalDigits="2" AllowRounding="true" />
                                </telerik:RadNumericTextBox>
                                <asp:RequiredFieldValidator runat="server" ID="RFVNumAddendumAmount" ValidationGroup="FormAddendum" ControlToValidate="radNumAddendumAmount" ErrorMessage="* Required"
                                    ForeColor="Red"></asp:RequiredFieldValidator>
                            </div>
                        </div>

                        <div class="col-xs-12 padding-0">
                            <div class="col-sm-6 col-xs-12 padding-0">
                                <label class="control-label lable-txt" for="Requested Amount">Addendum Approved Amount</label>
                            </div>
                            <div class="col-sm-6 col-xs-12 padding-0">
                                <telerik:RadNumericTextBox MinValue="0.00001" runat="server" EmptyMessage="Enter Addendum Approved Amount" ID="radNumAddendumAppAmount" Width="100%">
                                    <NumberFormat DecimalDigits="2" AllowRounding="true" />
                                </telerik:RadNumericTextBox>
                                <asp:RequiredFieldValidator runat="server" ID="RFVNumAddendumAppAmount" ValidationGroup="FormAddendum" ControlToValidate="radNumAddendumAppAmount" ErrorMessage="* Required"
                                    ForeColor="Red"></asp:RequiredFieldValidator>
                            </div>
                        </div>

                        <div class="col-xs-12 padding-0" runat="server" id="divAddendumStatus">
                            <div class="col-sm-6 col-xs-12 padding-0">
                                <label class="control-label lable-txt" for="Requested Amount">Addendum Status</label>
                            </div>
                            <div class="col-sm-6 col-xs-12 padding-0">
                                <telerik:RadComboBox ID="radCmbAddenumStatus" RenderMode="Lightweight" DataValueField="Id" DataTextField="Name" runat="server" EmptyMessage="Select Addendum Status">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="Approved" Value="1" />
                                        <telerik:RadComboBoxItem Text="Rejected" Value="2" />
                                        <telerik:RadComboBoxItem Text="Need Correction" Value="3" />
                                    </Items>
                                </telerik:RadComboBox>
                                <asp:RequiredFieldValidator runat="server" ID="RFVAddenumStatus" ValidationGroup="FormAddendum" ControlToValidate="radCmbAddenumStatus" ErrorMessage="* Required"
                                    ForeColor="Red"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="col-xs-12 padding-0">
                            <div class="col-sm-6 col-xs-12 padding-0">
                                <label class="control-label lable-txt" for="Requested Amount">Nature of Request</label>
                            </div>
                            <div class="col-sm-6 col-xs-12 padding-0">
                                <telerik:RadComboBox ID="radCmbAdnNatureOfRequest" RenderMode="Lightweight" DataValueField="Id" DataTextField="Name" runat="server" EmptyMessage="Select Nature of Request">
                                </telerik:RadComboBox>
                                <asp:RequiredFieldValidator runat="server" ID="RFVAdnNatureOfRequest" ValidationGroup="FormAddendum" ControlToValidate="radCmbAdnNatureOfRequest" ErrorMessage="* Required"
                                    ForeColor="Red"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="col-xs-12 padding-0">
                            <div class="col-sm-6 col-xs-12 padding-0">
                                <label class="control-label lable-txt" for="Reason for addendum">Reason for Addendum</label>
                            </div>
                            <div class="col-sm-6 col-xs-12 padding-0">
                                <telerik:RadTextBox ID="tbaddendumReason" EmptyMessage="Enter Reason for Addendum" CssClass="form-control" Width="100%" runat="server"></telerik:RadTextBox>
                            </div>
                        </div>
                        <div class="col-xs-12 padding-0" style="padding-top: 11px !important;">
                            <div class="col-sm-6 col-xs-12 padding-0" >
                                <label class="control-label lable-txt" for="Reason for addendum">Comment</label>
                            </div>
                            <div class="col-sm-6 col-xs-12 padding-0">
                                <telerik:RadTextBox ID="txtAddendumComment" TextMode="MultiLine" EmptyMessage="Enter Comment" MaxLength="200" Width="100%" runat="server" Rows="2"></telerik:RadTextBox>
                            </div>
                        </div>
                    </div>
                    &nbsp;&nbsp;&nbsp;<asp:Label ID="lblRangeErr" ForeColor="Red" runat="server"> </asp:Label>
                    <div class="modal-footer">
                        <div class="col-xs-12">
                            <div class="button-wrapper">
                                <center>
                                <ul>
                                    <li>
                                        <a href="#" class="btn btn-grey collapsed" role="button" data-toggle="collapse" data-target="#addendum-history" aria-expanded="false">View History</a>
                                    </li>
                                    <li>
                                        <asp:LinkButton ID="lnkSaveAddendum" runat="server" Text="Save" ValidationGroup="FormAddendum" OnClientClick="return ValidateForm();" CssClass="btn btn-grey" OnClick="lnkSaveAddendum_Click"></asp:LinkButton>
                                    </li>
                                    <li>
                                        <asp:LinkButton ID="lnkSubmitAddendum" runat="server" Text="Submit" ValidationGroup="FormAddendum" OnClientClick="return ValidateForm();" CssClass="btn btn-grey" OnClick="lnkSubmitAddendum_Click"></asp:LinkButton>
                                    </li>
                                </ul>
                                    </center>
                            </div>
                        </div>


                    </div>

                    <div class="col-xs-12 margin-10">
                        <telerik:RadGrid RenderMode="Lightweight" ID="addendumComment" runat="server"
                            AutoGenerateColumns="false" Width="100%"
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
                                            <td align="center">No records to display.
                                            </td>
                                        </tr>
                                    </table>
                                </NoRecordsTemplate>
                                <Columns>
                                    <telerik:GridBoundColumn HeaderText="Date" DataField="CreatedOn" UniqueName="CreatedOn" DataFormatString="{0:dd/MM/yyyy}" >
                                        <HeaderStyle Width="60px" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn HeaderText="Comment" DataField="Comment" UniqueName="Comment">
                                        <HeaderStyle Width="120px" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn HeaderText="Comment By" DataField="UserName" UniqueName="CommentBy">
                                        <HeaderStyle Width="120px" />
                                    </telerik:GridBoundColumn>
                                </Columns>
                                <PagerStyle PageSizes="5,10,15" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                                    PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />
                            </MasterTableView>
                        </telerik:RadGrid>
                    </div>

                    <div id="addendum-history" class="collapse filter-panel">
                        <div class="panel panel-default">
                            <div class="panel-body overflow-x">
                                <telerik:RadGrid RenderMode="Lightweight" ID="rgridAddandumHistory" runat="server"
                                    AutoGenerateColumns="false" OnItemCommand="rgridAddandumHistory_ItemCommand"
                                    AllowPaging="false" AllowSorting="false" AllowFilteringByColumn="false">
                                    <GroupingSettings CaseSensitive="false" />
                                    <ClientSettings>
                                        <Selecting AllowRowSelect="true"></Selecting>
                                    </ClientSettings>
                                    <MasterTableView CommandItemSettings-ShowRefreshButton="false" EnableNoRecordsTemplate="true" EditMode="InPlace" CommandItemDisplay="None"
                                        DataKeyNames="Id" AllowFilteringByColumn="false" AllowSorting="false" NoMasterRecordsText="">
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
                                            <telerik:RadGrid RenderMode="Lightweight" ID="gvCommentApproveAddendum" runat="server" AutoGenerateColumns="false"
                                                AllowMultiRowSelection="true" AllowPaging="true" AllowSorting="false" AllowFilteringByColumn="false"
                                                Width="100%" PageSize="5">
                                                <GroupingSettings CaseSensitive="false" />
                                                <ClientSettings>
                                                    <Selecting AllowRowSelect="true"></Selecting>
                                                </ClientSettings>
                                                <MasterTableView CommandItemSettings-ShowRefreshButton="false" EditMode="InPlace" CommandItemDisplay="None"
                                                    AllowFilteringByColumn="false" DataKeyNames="UserName" AllowSorting="false" NoMasterRecordsText="">
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
                                                        <%-- <telerik:GridClientSelectColumn  HeaderText="Select" UniqueName="RequestSelect">
                                                                </telerik:GridClientSelectColumn>--%>
                                                        <telerik:GridBoundColumn HeaderText="Comment" UniqueName="Comment" DataField="Comment">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridDateTimeColumn HeaderText="Date" UniqueName="Date" DataField="CreatedOn" DataFormatString="{0:dd/MM/yyyy}">
                                                        </telerik:GridDateTimeColumn>
                                                        <telerik:GridBoundColumn HeaderText="Comment By" UniqueName="UserName" DataField="UserName">
                                                        </telerik:GridBoundColumn>

                                                    </Columns>
                                                    <PagerStyle PageSizes="5,10,15" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                                                        PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />
                                                </MasterTableView>
                                            </telerik:RadGrid>

                                        </NestedViewTemplate>
                                        <Columns>
                                            <%-- <telerik:GridClientSelectColumn HeaderText="Select" UniqueName="Select">
                                                </telerik:GridClientSelectColumn>--%>
                                            <telerik:GridBoundColumn HeaderText="Treasury Allocation Number" DataField="AllocationNo" UniqueName="AllocationNo">
                                                <HeaderStyle Width="120px" />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn HeaderText="Addendum Date" DataField="CreatedOn" UniqueName="CreatedOn" DataFormatString="{0:dd/MM/yyyy}">
                                                <HeaderStyle Width="60px" />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn HeaderText="Addendum Amount" DataField="Amount" UniqueName="Amount" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,###}" >
                                                <HeaderStyle Width="60px" />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn HeaderText="Addendum Approved Amount" DataField="ApprovedAmount" ItemStyle-HorizontalAlign="Right" UniqueName="ApprovedAmount" DataFormatString="{0:#,###}" >
                                                <HeaderStyle Width="60px" />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn HeaderText="Addendum Status" DataField="AddandomWorkflowStatus" UniqueName="AddandomWorkflowStatus">
                                                <HeaderStyle Width="80px" />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn HeaderText="Nature of Request" DataField="NatureOfRequest" UniqueName="NatureOfRequest">
                                                <HeaderStyle Width="80px" />
                                            </telerik:GridBoundColumn>
                                            <%--    <telerik:GridTemplateColumn UniqueName="ViewComment" HeaderText="View" AllowSorting="true" AllowFiltering="true" DataType="System.String">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="View" CommandName="ViewComment" runat="server" CssClass="linkButtonColor"></asp:LinkButton>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>--%>
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

        </div>
    </div>
    <!-- End of modal for addendum request-->
    <%--</div>--%>
    <telerik:RadWindowManager runat="server" ID="RadWindowManager1">
        <Windows>
        </Windows>
    </telerik:RadWindowManager>
    <telerik:RadNotification RenderMode="Lightweight" Position="Center" runat="server" ID="radMessage" VisibleOnPageLoad="false"
        Width="350px" TitleIcon="none" ContentIcon="none" EnableShadow="true" AnimationDuration="1000">
    </telerik:RadNotification>
    <telerik:RadWindow runat="server" ID="BudgetPopup" VisibleStatusbar="false" Width="1200px"
        Height="350px" AutoSize="false" ShowContentDuringLoad="false" Modal="true" OnClientBeforeShow="OnClientBeforeShow"
        ReloadOnShow="true" Behaviors="Close,Move,Resize" Title="Budget Utilisation">
        <ContentTemplate>
            <div id="AutomaticBudget" runat="server">
                <!-- content goes here -->
                <table class="table table-striped table-bordered" style="width: 100% !important;">
                    <thead>
                        <tr>
                            <th>Vendor Code</th>
                            <th>Vendor Name</th>
                            <th>Nature of Request</th>
                            <th class="text-center">Utilised Amount</th>


                        </tr>
                    </thead>
                    <tbody>
                        <tr class="text-center">
                            <td class="valign">SAP0012345</td>
                            <td class="valign">Ambit Software</td>
                            <td class="valign">Electrical</td>
                            <td class="valign">800000.00</td>
                        </tr>
                        <tr class="text-center">
                            <td class="valign">SAP0012345</td>
                            <td class="valign">TATA</td>
                            <td class="valign">Mechanical</td>
                            <td class="valign">50000.00</td>
                        </tr>
                        <tr class="text-center">
                            <td class="valign">SAP0012345</td>
                            <td class="valign">BSNL</td>
                            <td class="valign">Electrical</td>
                            <td class="valign">170000.00</td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <div id="ManualBudget" runat="server">
                <div>
                    <!-- content goes here -->
                    <div class="col-xs-12 padding-0">
                        <div class="button-wrapper pull-right">
                            <ul>
                                <li>
                                    <asp:LinkButton class="btn btn-grey" Text="Add Row" ID="btnAddRange" OnClick="btnAddRange_Click" runat="server" CausesValidation="false"></asp:LinkButton></li>
                                <%--  <li><a href="#" onclick="CloseBudgetUtilization();" class="btn btn-grey">Cancel</a> </li>
                                        <li><a href="#" class="btn btn-grey">Save</a></li>--%>
                            </ul>
                        </div>
                    </div>



                    <table>
                        <tbody>
                            <tr>
                                <td>
                                    <telerik:RadGrid ID="rgridManualUtilisation" GridLines="None" AutoGenerateColumns="false" ShowHeadersWhenNoRecords="true"
                                        AllowPaging="true" AllowSorting="true" runat="server" OnItemDataBound="rgridManualUtilisation_ItemDataBound"
                                        PageSize="5" RenderMode="Lightweight" AllowFilteringByColumn="true" GroupingSettings-CaseSensitive="false"
                                        AllowAutomaticUpdates="true" AllowAutomaticInserts="True" OnNeedDataSource="rgridManualUtilisation_NeedDataSource"
                                        ShowStatusBar="false" OnItemCommand="rgridManualUtilisation_ItemCommand" OnInsertCommand="rgridManualUtilisation_InsertCommand" OnUpdateCommand="rgridManualUtilisation_UpdateCommand" OnDeleteCommand="rgridManualUtilisation_DeleteCommand">
                                        <MasterTableView ShowFooter="false" EditMode="InPlace" DataKeyNames="TreasuryBudgetUtilisationId" EnableNoRecordsTemplate="true">
                                            <CommandItemSettings ShowRefreshButton="false" />
                                               <PagerStyle AlwaysVisible="true"/> 
                                            <NoRecordsTemplate>
                                                <table width="100%" border="0" cellpadding="20" cellspacing="20">
                                                    <tr>
                                                        <td align="center">No records to display.
                                                        </td>
                                                    </tr>
                                                </table>
                                            </NoRecordsTemplate>
                                            <Columns>
                                                <telerik:GridTemplateColumn UniqueName="PaymentDate" HeaderText="Payment Date"
                                                    SortExpression="PaymentDate" ItemStyle-Width="300px">
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblPaymentDate" runat="server" Text='<%# Eval("PaymentDate" , "{0:dd-MM-yyyy}")%>'/>
                                                       
                                                    </ItemTemplate>

                                                    <EditItemTemplate>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <telerik:RadLabel ID="lblPaymentDateVal" Visible="false" runat="server" Text='<%# Eval("PaymentDate")%>' />
                                                                    <telerik:RadDatePicker DateInput-EmptyMessage="Select Date" NumberFormat-DecimalDigits="0" runat="server" ID="dpkPaymentDate" Width="150px" >
                                                                    </telerik:RadDatePicker>
                                                                    <asp:RequiredFieldValidator ID="RFVPaymentDate" runat="server" ForeColor="Red" ErrorMessage="* Required" ControlToValidate="dpkPaymentDate"></asp:RequiredFieldValidator>
                                                                </td>

                                                            </tr>
                                                        </table>
                                                    </EditItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn UniqueName="NatureOfRequest" HeaderText="Nature of Request"
                                                    SortExpression="NatureOfReqest" ItemStyle-Width="300px">
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblNatureOfRequest" runat="server" Text='<%# Eval("NatureOfReqest")%>' />

                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <table style="text-align: left !important;">
                                                            <tr>
                                                                <td style="width: 90%">
                                                                    <telerik:RadLabel ID="lblNatureOfRequestId" Visible="false" runat="server" Text='<%# Eval("NatureOfReqestId")%>' />

                                                                    <telerik:RadComboBox MarkFirstMatch="True" runat="server" ID="radComboNatureOfRequest" AutoPostBack="false"
                                                                        Height="140px" Width="600px" DropDownWidth="150px" EmptyMessage="Select Nature of Request" Filter="Contains">
                                                                    </telerik:RadComboBox>
                                                                    <asp:RequiredFieldValidator ID="RFVNaturOfReq" runat="server" ForeColor="Red" ErrorMessage="* Required" ControlToValidate="radComboNatureOfRequest"></asp:RequiredFieldValidator>
                                                                </td>

                                                            </tr>
                                                        </table>
                                                    </EditItemTemplate>
                                                    <HeaderStyle HorizontalAlign="left" />
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn UniqueName="Amount" HeaderText="Amount" ItemStyle-HorizontalAlign="Right"
                                                    SortExpression="Amount" ItemStyle-Width="250px">
                                                    <ItemTemplate>
                                                         <telerik:RadLabel ID="lblAmount" runat="server" Text='<%#String.Format("{0:#,###}", Eval("Amount")) %>' />
                                                       <%-- <telerik:RadLabel ID="lblAmount" runat="server" Text='<%# Eval("Amount")%>' />--%>                                                       
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <telerik:RadLabel ID="lblAmountVal" Visible="false" runat="server" Text='<%# Eval("Amount")%>' />
                                                                    <telerik:RadNumericTextBox EmptyMessage="Enter Amount" MaxLength="13" ClientEvents-OnKeyPress="EnterKeyForAmounttxt" NumberFormat-DecimalDigits="0" MinValue="0.00001" runat="server" ID="RadNumAmount" Width="150px">
                                                                        <NumberFormat DecimalDigits="2" AllowRounding="true" />
                                                                    </telerik:RadNumericTextBox>
                                                                    <asp:RequiredFieldValidator ID="RFVAmount" runat="server" ForeColor="Red" ErrorMessage="* Required" ControlToValidate="RadNumAmount"></asp:RequiredFieldValidator>
                                                                    <asp:CompareValidator ID="CmpAmount" runat="server" ControlToValidate="RadNumAmount" ErrorMessage="* Required" Display="Dynamic" ForeColor="Red" ValueToCompare="0" Operator="GreaterThan"></asp:CompareValidator>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </EditItemTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />
                                                    <HeaderStyle HorizontalAlign="left" />
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn UniqueName="AccountType" HeaderText="Account Type"
                                                    SortExpression="AccountType" ItemStyle-Width="300px">
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblAccountType" runat="server" Text='<%# Eval("AccountType")%>' />
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <table>
                                                            <tr>
                                                                <td style="width: 90%">
                                                                    <telerik:RadLabel ID="lblAccountTypeId" Visible="false" runat="server" Text='<%# Eval("AccountType")%>' />
                                                                    <telerik:RadComboBox MarkFirstMatch="True" runat="server" ID="radComboAccountType" AutoPostBack="false"
                                                                        Height="140px" Width="600px" DropDownWidth="150px" EmptyMessage="Select Account Type" Filter="Contains">
                                                                        <Items>
                                                                            <telerik:RadComboBoxItem Text="Customer" Value="Customer" />
                                                                            <telerik:RadComboBoxItem Text="GL" Value="GL" />
                                                                            <telerik:RadComboBoxItem Text="Vendor" Value="Vendor" />
                                                                        </Items>
                                                                        <%--  <ItemTemplate>
                                                                                <ul>
                                                                                    <li class="col1">
                                                                                        <%# Eval("Name")%>
                                                                                    </li>
                                                                                </ul>
                                                                            </ItemTemplate>--%>
                                                                    </telerik:RadComboBox>
                                                                    <asp:RequiredFieldValidator ID="RFVAmountType" runat="server" ForeColor="Red" ErrorMessage="* Required" ControlToValidate="radComboAccountType"></asp:RequiredFieldValidator>
                                                                </td>

                                                            </tr>
                                                        </table>
                                                    </EditItemTemplate>
                                                    <HeaderStyle HorizontalAlign="left" />
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn UniqueName="AccountCode" HeaderText="Account Code"
                                                    SortExpression="AccountCode" ItemStyle-Width="300px">
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblAccountCode" runat="server" Text='<%# Eval("AccountCode")%>' />
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <telerik:RadLabel ID="lblAccountCodeVal" Visible="false" runat="server" Text='<%# Eval("AccountCode")%>' />
                                                                    <telerik:RadTextBox ID="RadNumAccountCode" runat="server" MaxLength="20" EmptyMessage="Enter Account Code">
                                                                        <ClientEvents OnKeyPress="keyPress" />
                                                                    </telerik:RadTextBox>
                                                                    <asp:RequiredFieldValidator ID="RFVAmountCode" runat="server" ForeColor="Red" ErrorMessage="* Required" ControlToValidate="RadNumAccountCode"></asp:RequiredFieldValidator>
                                                                    <asp:CompareValidator ID="CmpAmountCode" runat="server" ControlToValidate="RadNumAccountCode" ErrorMessage="* Required" Display="Dynamic" ForeColor="Red" ValueToCompare="0" Operator="GreaterThan"></asp:CompareValidator>
                                                                </td>
                                                                <td></td>
                                                                <td></td>
                                                            </tr>
                                                        </table>
                                                    </EditItemTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />
                                                    <HeaderStyle HorizontalAlign="left" />
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn UniqueName="DocumentCode" HeaderText="Document Number"
                                                    SortExpression="Amount" ItemStyle-Width="300px">
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblDocumentCode" runat="server" Text='<%# Eval("DocumentNo")%>' />
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <telerik:RadLabel ID="lblDocumentCodeVal" Visible="false" runat="server" Text='<%# Eval("DocumentNo")%>' />
                                                                    <%-- <telerik:RadNumericTextBox DisplayText='<%#Eval("DocumentNo")%>' NumberFormat-DecimalDigits="0" MinValue="1" MaxLength="20" runat="server" ID="RadNumDocumentCode" Width="150px">
                                                                                <NumberFormat GroupSeparator="" DecimalDigits="0" AllowRounding="false" />
                                                                            </telerik:RadNumericTextBox>--%>
                                                                    <telerik:RadTextBox ID="RadNumDocumentCode" EmptyMessage="Enter Document Number" runat="server" MaxLength="20">
                                                                        <ClientEvents OnKeyPress="keyPress" />
                                                                    </telerik:RadTextBox>
                                                                    <asp:RequiredFieldValidator ID="RFVDocumentCode" runat="server" ForeColor="Red" ErrorMessage="* Required" ControlToValidate="RadNumDocumentCode"></asp:RequiredFieldValidator>
                                                                    <asp:CompareValidator ID="CmpDocumentCode" runat="server" ControlToValidate="RadNumAmount" ErrorMessage="* Required" Display="Dynamic" ForeColor="Red" ValueToCompare="0" Operator="GreaterThan"></asp:CompareValidator>
                                                                </td>

                                                            </tr>
                                                        </table>
                                                    </EditItemTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />
                                                    <HeaderStyle HorizontalAlign="left" />
                                                </telerik:GridTemplateColumn>
                                                <%--        <telerik:GridEditCommandColumn HeaderText="Edit" UniqueName="EditCommandColumn" >
                                                            <HeaderStyle Width="100px" />
                                                        </telerik:GridEditCommandColumn>--%>
                                                <telerik:GridEditCommandColumn HeaderText="Edit" UniqueName="EditUtilization" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" Exportable="false" />
                                                <telerik:GridButtonColumn HeaderText="Delete" CommandName="Delete" Text="Delete" UniqueName="DeleteUtilization"
                                                    ConfirmText="Are you sure you want to delete this record?" ConfirmTitle="Alert" ConfirmDialogType="Classic" ConfirmDialogHeight="25px">
                                                    <HeaderStyle Width="80px" />
                                                </telerik:GridButtonColumn>
                                            </Columns>
                                            <%--<PagerStyle  ShowPagerText="false" PageSizeLabelText=" " Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />--%>
                                            <PagerStyle PageSizes="5,10,15" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                                                PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Left" VerticalAlign="Middle" />

                                        </MasterTableView>
                                    </telerik:RadGrid>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                 </div>

            </div>
            
        </ContentTemplate>
    </telerik:RadWindow>
            <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager2" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
</asp:Content>

