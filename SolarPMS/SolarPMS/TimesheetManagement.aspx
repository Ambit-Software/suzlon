<%@ Page Title="Timesheet Management" Language="C#" MasterPageFile="~/MasterPages/SolarPMS.Master" AutoEventWireup="true"
    EnableEventValidation="false" CodeBehind="TimesheetManagement.aspx.cs" Inherits="SolarPMS.TimesheetManagement" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="Content/css/bootstrap.min.css" rel="stylesheet" />
    <link href="Content/css/global.css" rel="stylesheet" />
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.12.2/jquery.min.js"></script>
    <script src="Scripts/Page/Common.js"></script>
    <telerik:RadScriptBlock ID="radSript1" runat="server">
        <script type="text/javascript">
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
                        $find("<%= TimesheetManagementRadAjaxManager.ClientID %>").ajaxRequest("ProceedToSaveYes#" + btntext);
                    }
                }
                Telerik.Web.UI.RadWindowUtils.Localization =
                {
                    "OK": "Yes",
                    "Cancel": "No"
                };
                radconfirm("Estimated Quantity is greater than Actual Quantity. Do you still want to go ahead?", confirmCallBack, "150px", "150px", null, "Confirmation");
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
                var notification = $find("<%= radNotificationMessage.ClientID %>");
                var message = title;
                notification.set_title(title);
                notification.set_text(text);
                notification.show();
            }

            function calculateQuantity(obj, manager) {
                var txtActualQty = ''; //"<%=this.txtActualQty.ClientID%>";
                var txtNumber = '';
                if (manager == 'manager') {
                    txtActualQty = "<%=this.txtApprovedQuantity.ClientID%>";
                    txtNumber = "<%=this.txtManagerNumber.ClientID%>";
                }
                else {
                    txtActualQty = "<%=this.txtActualQty.ClientID%>";
                    txtNumber = "<%=this.txtNumber.ClientID%>";
                }
                    
                if ($("#" + txtNumber).val() != "Numbers" && $("#" + txtNumber).val() != "") {
                    var numbers = $("#" + txtNumber).val().replace(/^,|,$/g, '').split(',');
                    $("#" + txtActualQty).val(numbers.length)
                }
                else {
                    $("#" + txtActualQty).val('')
                }
            }

            function bindEvents() {
                var txtNumber = "<%=this.txtNumber.ClientID%>";
                $("#" + txtNumber).on('input', function () {
                    var value = $(this).val().replace(/[^0-9.,]*/g, '');
                    value = value.replace(/\.{2,}/g, '.');
                    value = value.replace(/\.,/g, ',');
                    value = value.replace(/\,\./g, ',');
                    value = value.replace(/\,{2,}/g, ',');
                    value = value.replace(/\.[0-9]+\./g, '.');
                    $(this).val(value);
                });
            }
        </script>
    </telerik:RadScriptBlock>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadAjaxManager ID="TimesheetManagementRadAjaxManager" runat="server" EnablePageHeadUpdate="false" OnAjaxRequest="TimesheetManagementRadAjaxManager_AjaxRequest">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="gridTimesheetAttachment">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="gridTimesheetAttachment" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="TimesheetManagementRadAjaxManager">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="radNotificationMessage" UpdatePanelRenderMode="Inline" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="gridSurvey">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="gridSurvey" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="txtActualQty" LoadingPanelID="LoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnNewRow">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="gridSurvey" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="gridBlockDetails">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="gridBlockDetails" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="radNotificationMessage" UpdatePanelCssClass="LoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="txtActualQty" LoadingPanelID="LoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnBlock">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="gridBlockDetails" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="gridInverter">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="gridInverter" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="txtQty" LoadingPanelID="LoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="radNotificationMessage" UpdatePanelCssClass="LoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="hidManualEntryQuantity" LoadingPanelID="LoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="hidRangeQuantity" LoadingPanelID="LoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="txtActualQty" LoadingPanelID="LoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnAddRange">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="gridInverter" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnCalculatePraposedQuantity">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="txtQty" LoadingPanelID="LoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="hidManualEntryQuantity" LoadingPanelID="LoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="hidRangeQuantity" LoadingPanelID="LoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="txtManualEntry" LoadingPanelID="LoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="txtActualQty" LoadingPanelID="LoadingPanel1" />
                    
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnSave">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="formDiv" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>                    
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnPartialApprove">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="divMangerNumberSection" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="txtManagerActualQuantity" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="txtManagerNumber" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="txtApprovedQuantity" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
             <telerik:AjaxSetting AjaxControlID="txtManagerNumber">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="txtManagerNumber" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnAddManagerRange">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="gridManagerInvertor" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="radNotificationMessage" UpdatePanelCssClass="LoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnAddManagerBlock">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="gridManagerBlockDetails" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="gridManagerBlockDetails">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="txtApprovedQuantity" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="gridManagerBlockDetails" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="radNotificationMessage" UpdatePanelCssClass="LoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnAddManagerSurvey">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="gridManagerSurvayDetails" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="gridManagerSurvayDetails">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="txtApprovedQuantity" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="gridManagerSurvayDetails" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="radNotificationMessage" UpdatePanelCssClass="LoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="gridManagerInvertor">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="txtApprovedQuantity" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="gridManagerInvertor" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="radNotificationMessage" UpdatePanelCssClass="LoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="txtManagerQuantity" LoadingPanelID="LoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="hidManualEntryQuantity" LoadingPanelID="LoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="hidRangeQuantity" LoadingPanelID="LoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnSubmit">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="formDiv" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnManagerManualRange">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="txtApprovedQuantity" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="hidManualEntryQuantity" LoadingPanelID="LoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="hidRangeQuantity" LoadingPanelID="LoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="txtManualEntry" LoadingPanelID="LoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="txtManagerManualEntry" LoadingPanelID="LoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="txtManagerQuantity" LoadingPanelID="LoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="LoadingPanel1" runat="server" Skin="Default">
    </telerik:RadAjaxLoadingPanel>
    <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        <Windows>
            <telerik:RadWindow RenderMode="Lightweight" ID="RadWindow1" runat="server" ShowContentDuringLoad="false" Width="400px" OnClientPageLoad="bindEvents()"
                Height="400px" Title="Telerik RadWindow" Behaviors="Default">
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>
    <div class="container-fluid padding-0">
        <div style="padding-left:10px; padding-right:10px; padding-top:10px;">
        <div class="panel panel-default">
            <div class="panel-heading">
                <a data-toggle="collapse" data-parent="" aria-expanded="true" href="#divTimesheetDetails" aria-controls="" class="">Timesheet Details</a> 
            </div>
            <div id="divTimesheetDetails" class="panel-collapse collapse in" role="" aria-labelledby="" aria-expanded="true">
                <div class="row margin-0">
                    <div class="block-wrapper" id="formDiv" runat="server">
                        <telerik:RadNotification RenderMode="Lightweight" ID="radNotificationMessage" runat="server" Position="Center"
                            Width="400" Height="100" Animation="Fade" EnableRoundedCorners="true" EnableShadow="true" AnimationDuration="500"
                            ContentIcon="none" TitleIcon="none" Title="Success" Text="" Style="z-index: 100000">
                        </telerik:RadNotification>
                        <div class=" col-xs-12 padding-0">
                            <div class=" col-xs-6 padding-0">
                                <div class="col-sm-12 padding-0">
                                    <label class="col-xs-12 text-primary" for="name" id="lblActivityName" runat="server" style="font-family:Verdana;font-size:12px;"></label>
                                </div>
                                <div class="col-sm-6 padding-0">
                                    <label class="col-xs-12 text-primary" for="name" id="lblSubActivityName" runat="server" style="font-family:Verdana;font-size:12px;"></label>
                                </div>

                            </div>
                            <div class="col-sm-6 padding-0">
                                <div class="col-sm-4"></div>
                            </div>
                            <div class="col-md-12 padding-0">
                               <%-- <div class="panel panel-default">
                                    <div class="panel-heading" role="tab" id="">
                                        <a role="" data-toggle="collapse" data-parent="" href="#divCommonDetails" aria-expanded="true" aria-controls="" class="">Common Details <span class="glyphicon glyphicon-plus pull-right" aria-hidden="true"></span>
                                        </a>
                                    </div>--%>
                               <%--     <div id="divCommonDetails" class="panel-body collapse in">--%>
                                        <div class="col-md-6">
                                            <!--Start of first column 6-->
                                            <div class="row margin-0">
                                                <div class="col-md-12 padding-0">
                                                    <div class="col-md-4 col-xs-12 padding-0">
                                                        <label class="col-xs-12 control-label lable-txt" for="name">Est. Start Date</label>
                                                    </div>
                                                    <!-- End of col-md-4 col-xs-12-->

                                                    <div class="row margin-0">
                                                        <div class="col-md-8  col-xs-12 padding-0">
                                                            <div class="form-group">
                                                                <telerik:RadDatePicker DateInput-DateFormat="dd-MMM-yyyy" Enabled="false" RenderMode="Lightweight" ID="RadDatePickerEstStart" Width="100%" runat="server" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <!-- End of col-xs-12-->
                                                </div>

                                            </div>
                                            <!-- End of row-->

                                            <div class="col-md-12 padding-0">
                                                <div class="col-md-4 col-xs-12">
                                                    <label class="col-xs-12 padding-0 control-label lable-txt" for="name">Actual Date</label>
                                                </div>
                                                <!-- End of col-md-4 col-xs-12-->
                                                <div class="col-md-8 col-xs-12 padding-0 ">
                                                    <div class="form-group">
                                                        <telerik:RadDatePicker DateInput-DateFormat="dd-MMM-yyyy" DateInput-ReadOnly="true" RenderMode="Lightweight" ID="RadDatePickerActualDate" Width="100%" runat="server" />
                                                    </div>
                                                </div>

                                            </div>
                                            <!-- End of col-md-12-->

                                            <div class="col-md-12 padding-0 margin-10" id="divAttachment" runat="server">
                                                <div class="col-md-4 col-xs-12">
                                                    <label class="control-label lable-txt" for="name">Attachment</label>
                                                </div>
                                                <!-- End of col-md-4 col-xs-12-->

                                                <div class="col-md-8 col-xs-12 padding-0 ">
                                                    <style>
                                                        .RadAsyncUpload span.ruFileWrap {
                                                            padding-left: 0;
                                                            width: 100% !important;
                                                        }

                                                        .RadUpload_Default .ruSelectWrap .ruButton {
                                                            background-color: #E6E6E6 !important;
                                                            background-image: none !important;
                                                            line-height: 18px !important;
                                                            height: 30px !important;
                                                        }

                                                        .RadUpload .ruFakeInput {
                                                            min-width: 100px !important;
                                                            border: 1px solid #c3c3c3;
                                                            padding: 4px 7px !important;
                                                            border-radius: 4px;
                                                            height: 50px !important;
                                                            font-size: 14px !important;
                                                            line-height: 1.42857143 !important;
                                                        }

                                                        .TSGrid-Wrapper {
                                                            width: 100%;
                                                            overflow-x: scroll;
                                                            max-height: 200px !important;
                                                            overflow-y: scroll;
                                                            margin-top: 10px;
                                                        }

                                                        .RadUpload .ruFileWrap {
                                                            height: auto !important;
                                                        }
                                                    </style>
                                                    <telerik:RadAsyncUpload RenderMode="Lightweight" runat="server" ID="radUploadAttachment" HideFileInput="true"
                                                        MultipleFileSelection="Automatic" TargetFolder="~/Upload/Attachment/Timesheet"
                                                        OnFileUploaded="radUploadAttachment_FileUploaded" OnClientFileUploaded="callAjaxRequest"
                                                        MaxFileSize="10485760" UploadedFilesRendering="BelowFileInput" ToolTip="Image must be smaller than 10MB">
                                                    </telerik:RadAsyncUpload>

                                                    <%--<label id="lblUploadError" style="color: red; font-weight: 100">Please select at least a single file!</label>--%>

                                                    <div class="TSGrid-Wrapper">
                                                        <telerik:RadGrid RenderMode="Lightweight" ID="gridTimesheetAttachment" Width="100%" runat="server"
                                                            OnItemCommand="gridTimesheetAttachment_ItemCommand" OnItemDataBound="gridTimesheetAttachment_ItemDataBound"
                                                            AutoGenerateColumns="false" AllowPaging="true" AllowSorting="true" AllowFilteringByColumn="false"
                                                            PageSize="<%# Convert.ToInt32(SolarPMS.Models.Constants.CONST_GRID_PAGE_SIZE) %>">
                                                            <GroupingSettings CaseSensitive="false" />
                                                            <MasterTableView Width="100%" CommandItemSettings-ShowRefreshButton="false" DataKeyNames="FileName,AttachmentId" EditMode="InPlace" CommandItemDisplay="None" AllowFilteringByColumn="false">
                                                                <CommandItemSettings ShowRefreshButton="false" ShowExportToExcelButton="false" />
                                                                <Columns>
                                                                    <telerik:GridHyperLinkColumn DataTextField="FileName" DataNavigateUrlFields="FileName" HeaderText="File Name" UniqueName="FileName">
                                                                    </telerik:GridHyperLinkColumn>
                                                                    <telerik:GridButtonColumn CommandName="Remove" Text="Remove" HeaderButtonType="LinkButton" UniqueName="Remove" HeaderText="Remove"></telerik:GridButtonColumn>
                                                                </Columns>
                                                                <PagerStyle PageSizes="5,10,15" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                                                                    PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />
                                                            </MasterTableView>
                                                        </telerik:RadGrid>
                                                    </div>
                                                </div>
                                                <!-- End of col-xs-12-->

                                            </div>
                                            <!-- End of col-lg-6-->

                                            <div id="divEstQty" runat="server" class="col-md-12 padding-0">
                                                <div class="col-md-4 col-xs-12 padding-0">
                                                    <label class="col-xs-12 control-label lable-txt" for="name">Est. Quantity</label>
                                                </div>
                                                <!-- End of col-md-4 col-xs-12-->
                                                <div class="col-md-8 col-xs-12 padding-0">
                                                    <div class="form-group">
                                                        <input id="txtEstQty" runat="server" type="text" class="search-query form-control" disabled="disabled" placeholder="Est. Quantity" />
                                                    </div>
                                                </div>
                                                <!-- End of col-xs-12-->
                                            </div>
                                            <!-- End of row-->
                                            <div id="divNumberControls" runat="server" class="col-md-12 padding-0" visible="false">
                                                <div class="col-md-4 col-xs-12 padding-0">
                                                    <label class="col-xs-12 control-label lable-txt" for="name">Number</label>
                                                </div>
                                                <!-- End of col-md-4 col-xs-12-->
                                                <div class="col-md-8 col-xs-12 padding-0">
                                                    <div class="form-group">
                                                        <telerik:RadTextBox ID="txtNumber" MaxLength="100" runat="server" InputType="Text" Width="100%" onchange="calculateQuantity(this,'')"
                                                            ClientEvents-OnBlur="calculateQuantity(this,'')" ValidationGroup="formValidation"
                                                            CssClass="search-query form-control" EmptyMessage="Numbers">
                                                        </telerik:RadTextBox>
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" ValidationExpression="^[0-9]{1,9}( *, *[0-9]{1,9})*$" runat="server"
                                                            ValidationGroup="formValidation" ForeColor="Red" ControlToValidate="txtNumber"
                                                            ErrorMessage="Please enter comma separated numbers."></asp:RegularExpressionValidator>
                                                    </div>
                                                </div>
                                            </div>
                                            <div id="divActualQty" runat="server" class="col-md-12 padding-0">
                                                <div class="col-md-4 col-xs-12 padding-0">
                                                    <label class="col-xs-12 control-label lable-txt" for="name">Actual Quantity</label>
                                                </div>
                                                <!-- End of col-md-4 col-xs-12-->
                                                <div class="col-md-8 col-xs-12 padding-0">
                                                    <div class="form-group">
                                                        <%--<input id="txtActualQty" runat="server" type="number" class="search-query form-control" placeholder="100 Acrs" />--%>

                                                        <telerik:RadTextBox ID="txtActualQty" MaxLength="10" runat="server" InputType="Number" Width="100%" CssClass="search-query form-control"
                                                            EmptyMessage="Quantity">
                                                        </telerik:RadTextBox>
                                                        <asp:RequiredFieldValidator runat="server" ID="ReqtxtActualQty" ControlToValidate="txtActualQty"
                                                            ErrorMessage="Please enter a Actual Quantity!" ForeColor="Red" ValidationGroup="formValidation"></asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                                <!-- End of col-xs-12-->
                                            </div>
                                            <!-- End of row-->
                                        </div>
                                        <!-- End of first column 6-->
                                        <!-- start of col-md-6 col-xs-12-->
                                        <div class="col-md-6 col-xs-12 padding-0">
                                            <div class="col-md-12">
                                                <div class="col-md-4 col-xs-12">
                                                    <label class=" col-xs-12 control-label lable-txt" for="name">Est. End Date</label>
                                                </div>
                                                <!-- End of col-md-4 col-xs-12-->
                                                <div class="row margin-0">
                                                    <div class="col-md-8 col-xs-12 padding-0">
                                                        <div class="form-group">
                                                            <telerik:RadDatePicker DateInput-DateFormat="dd-MMM-yyyy" Enabled="false" RenderMode="Lightweight" ID="RadDatePickerEstEnd" Width="100%" runat="server" />
                                                        </div>
                                                    </div>
                                                </div>
                                                <!-- End of col-xs-12-->

                                            </div>
                                            <!-- End of column-->

                                            <div class="col-md-12">
                                                <div class="col-md-4 col-xs-12">
                                                    <label class="col-xs-12 control-label lable-txt" for="name">Status </label>
                                                </div>
                                                <!-- End of col-md-4 col-xs-12-->

                                                <div class="col-md-8 col-xs-12 padding-0 ">
                                                    <telerik:RadDropDownList RenderMode="Lightweight" ID="ddlStatus" runat="server" AutoPostBack="false"
                                                        DataTextField="Name" DataValueField="Id">
                                                    </telerik:RadDropDownList>
                                                </div>

                                            </div>
                                            <!-- End of col-md-12-->

                                            <div class="col-md-12">
                                                <div class="col-md-4 col-xs-12">
                                                    <label class="col-xs-12 control-label lable-txt" for="name">Comments</label>
                                                </div>
                                                <!-- End of col-md-4 col-xs-12-->
                                                <div class="col-md-8 col-xs-12 padding-0 ">
                                                    <form class="form-horizontal">
                                                        <asp:TextBox TextMode="MultiLine" runat="server" MaxLength="50" Rows="5" ID="txtComment" Style="width: 100%"></asp:TextBox>
                                                        <asp:RegularExpressionValidator runat="server" ID="valInput" ForeColor="Red" Height="20px"
                                                            ControlToValidate="txtComment"
                                                            ValidationGroup="formValidation"
                                                            ValidationExpression="^[\s\S]{0,200}$"
                                                            ErrorMessage="Maximum 200 characters allowed."
                                                            Display="Static"></asp:RegularExpressionValidator>
                                                    </form>
                                                </div>
                                                <!-- End of col-xs-12-->
                                            </div>
                                            <!-- End of row-->

                                            <div class="col-md-12" id="divEstUOM" runat="server">
                                                <div class="col-md-4 col-xs-12">
                                                    <label class="col-xs-12 control-label lable-txt" for="name">U.O.M.</label>
                                                </div>
                                                <!-- End of col-md-4 col-xs-12-->
                                                <div class="row margin-0">
                                                    <div class="col-md-8 col-xs-12 padding-0">
                                                        <div class="form-group">
                                                            <input runat="server" class="form-control" id="txtUOM" type="text" disabled placeholder="Acrs" />
                                                        </div>

                                                    </div>
                                                </div>
                                            </div>
                                            <!-- End of row-->

                                        </div>
                                        <!-- End of col-md-6 col-xs-12-->
                                    <%--</div>
                                </div>--%>
                            </div>
                        </div>
                    </div>
                    <div class="row margin-0" id="SurveyTab" runat="server" visible="false">
                        <div class="col-xs-12">
                            <div class="panel panel-default">
                                <div class="panel-heading" role="tab" id="">
                                        <a role="" data-toggle="collapse" data-parent="" href="#collapseOne" aria-expanded="true" aria-controls="" class="">Survey Details <span class="glyphicon glyphicon-plus pull-right" aria-hidden="true"></span>
                                        </a>
                                </div>
                                <div id="collapseOne" class="panel-collapse collapse in" role="" aria-labelledby="" aria-expanded="true">
                                    <div class="panel-body" style="overflow-x: scroll;">
                                        <table class="table table-striped table-bordered">
                                            <tfoot>
                                                <div class="col-lg-1  col-xs-3 padding-0">
                                                    <asp:Button class="btn btn-primary margin-10 btn-sm" Text="Add Survey" ID="btnNewRow"
                                                        runat="server" OnClick="btnNewRow_Click" CausesValidation="false"></asp:Button>
                                                </div>
                                            </tfoot>
                                            <tbody>
                                                <tr>
                                                    <td>
                                                        <telerik:RadGrid RenderMode="Lightweight" ID="gridSurvey" runat="server" OnNeedDataSource="gridSurvey_NeedDataSource"
                                                            AutoGenerateColumns="false" ValidationSettings-ValidationGroup="surveyValidation"
                                                            AllowPaging="true" AllowSorting="false" AllowFilteringByColumn="false" PageSize="5"
                                                            OnInsertCommand="gridSurvey_InsertCommand" OnUpdateCommand="gridSurvey_UpdateCommand"
                                                            OnItemDataBound="gridSurvey_ItemDataBound" OnItemCommand="gridSurvey_ItemCommand" OnDeleteCommand="gridSurvey_DeleteCommand">
                                                            <GroupingSettings CaseSensitive="false" />
                                                            <MasterTableView CommandItemSettings-ShowRefreshButton="false" EditMode="InPlace" NoMasterRecordsText=""
                                                                CommandItemDisplay="None" AllowFilteringByColumn="false" AllowSorting="false" DataKeyNames="SurveyId">
                                                                <CommandItemSettings ShowRefreshButton="false" ShowExportToExcelButton="false" />
                                                                <Columns>
                                                                    <telerik:GridTemplateColumn HeaderText="Site" UniqueName="Site" DataField="Site" Visible="false">
                                                                        <ItemTemplate>
                                                                            <%# Eval("Site") %>
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <asp:Label ID="lblSite" runat="server" Visible="False" Text='<%# Eval("Site") %>'> </asp:Label>
                                                                            <telerik:RadDropDownList AutoPostBack="true" Width="150px" CausesValidation="false" OnSelectedIndexChanged="drpSite_SelectedIndexChanged" RenderMode="Lightweight" ID="drpSite" runat="server" DefaultMessage="Select Site" DropDownHeight="110px">
                                                                            </telerik:RadDropDownList>
                                                                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="drpSite"
                                                                                Display="Static" ErrorMessage="* Required!" ForeColor="Red" CssClass="validator" ValidationGroup="surveyValidation" />
                                                                        </EditItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn HeaderText="Project" UniqueName="Project" DataField="SAPProjectId" Visible="false">
                                                                        <ItemTemplate>
                                                                            <%# Eval("ProjectDescription") %>
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <asp:Label ID="lblProject" runat="server" Visible="False" Text='<%# Eval("SAPProjectId") %>'></asp:Label>
                                                                            <telerik:RadDropDownList RenderMode="Lightweight" Width="150px" ID="drpProject" runat="server"
                                                                                DefaultMessage="Select Project" DropDownHeight="110px">
                                                                            </telerik:RadDropDownList>
                                                                            <asp:RequiredFieldValidator runat="server" ID="ReqProjValidator" ControlToValidate="drpProject"
                                                                                Display="Static" ErrorMessage="* Required!" ForeColor="Red" CssClass="validator" ValidationGroup="surveyValidation" />
                                                                        </EditItemTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridTemplateColumn HeaderText="Village" UniqueName="Village" DataField="villageName">
                                                                        <ItemTemplate>
                                                                            <%# Eval("villageName") %>
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <asp:Label ID="lblVillage" runat="server" Visible="False" Text='<%# Eval("villageId") %>'></asp:Label>
                                                                            <telerik:RadDropDownList RenderMode="Lightweight" Width="150px" ID="drpVillage" runat="server" AutoPostBack="true"
                                                                                OnSelectedIndexChanged="drpVillage_SelectedIndexChanged" DefaultMessage="Select Village" DropDownHeight="110px">
                                                                            </telerik:RadDropDownList>
                                                                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator3" ControlToValidate="drpVillage"
                                                                                Display="Static" ErrorMessage="* Required!" ForeColor="Red" CssClass="validator" ValidationGroup="surveyValidation" />
                                                                        </EditItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn HeaderText="Survey No." UniqueName="SurveyNo" DataField="SurveyNo">
                                                                        <ItemTemplate>
                                                                            <%# Eval("surveyNo") %>
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <asp:Label ID="lblSurveyNo" runat="server" Visible="False" Text='<%# Eval("surveyNo") %>'></asp:Label>
                                                                            <telerik:RadDropDownList RenderMode="Lightweight" Width="150px" ID="drpSurvayNo" runat="server" OnSelectedIndexChanged="drpSurvayNo_SelectedIndexChanged"
                                                                                AutoPostBack="true" DefaultMessage="Select Survey Number" DropDownHeight="110px">
                                                                            </telerik:RadDropDownList>
                                                                            <asp:RequiredFieldValidator runat="server" ID="reqSurveyNumber" ControlToValidate="drpSurvayNo"
                                                                                Display="Static" ErrorMessage="* Required!" ForeColor="Red" CssClass="validator" ValidationGroup="surveyValidation" />
                                                                        </EditItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn HeaderText="Proposed Area" UniqueName="PraposedArea" DataField="PraposedArea">
                                                                        <ItemTemplate>
                                                                            <%# Eval("PraposedArea") %>
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <telerik:RadNumericTextBox MinValue="0" DataType="decimal" ReadOnly="true" MaxValue="999999999" MaxLength="9" ID="txtPraposedArea" Text='<%#Bind("PraposedArea") %>'
                                                                                Width="100px" runat="server">
                                                                                <NumberFormat GroupSeparator="" DecimalDigits="2" KeepNotRoundedValue="false" />
                                                                            </telerik:RadNumericTextBox>
                                                                            <asp:RequiredFieldValidator runat="server" ErrorMessage="* Required!" ForeColor="Red"
                                                                                ControlToValidate="txtPraposedArea" ValidationGroup="surveyValidation"></asp:RequiredFieldValidator>
                                                                        </EditItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn HeaderText="Proposed No. of Division" UniqueName="PraposedNoOfDivision" DataField="PraposedNoOfDivision">
                                                                        <ItemTemplate>
                                                                            <%# Eval("PraposedNoOfDivision") %>
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <telerik:RadNumericTextBox MinValue="0" MaxValue="999999999" MaxLength="9" ID="txtPraposedNoOfDivision" Text='<%#Bind("PraposedNoOfDivision") %>'
                                                                                Width="100px" ReadOnly="true" runat="server">
                                                                                <NumberFormat GroupSeparator="" DecimalDigits="0" KeepNotRoundedValue="false" />
                                                                            </telerik:RadNumericTextBox>
                                                                            <asp:RequiredFieldValidator ID="ReqPraposedNoOdDiv" runat="server" ErrorMessage="* Required!" ForeColor="Red"
                                                                                ControlToValidate="txtPraposedNoOfDivision" ValidationGroup="surveyValidation"></asp:RequiredFieldValidator>
                                                                        </EditItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn HeaderText="Area" UniqueName="Area" DataField="Area">
                                                                        <ItemTemplate>
                                                                            <%# Eval("Area") %>
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <telerik:RadNumericTextBox ID="txtArea" Text='<%#Bind("Area") %>' runat="server" MaxLength="12" Width="100px">
                                                                                <NumberFormat DecimalDigits="2" DecimalSeparator="." AllowRounding="true" />
                                                                            </telerik:RadNumericTextBox>
                                                                            <asp:RequiredFieldValidator ID="ReqArea" runat="server" ErrorMessage="* Required!" ForeColor="Red"
                                                                                ControlToValidate="txtArea" ValidationGroup="surveyValidation"></asp:RequiredFieldValidator>
                                                                        </EditItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn HeaderText="No. of Division" UniqueName="NoOfDivision" DataField="noOfDivision">
                                                                        <ItemTemplate>
                                                                            <%# Eval("noOfDivision") %>
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <telerik:RadNumericTextBox MinValue="0" MaxValue="999999999" MaxLength="9" ID="txtNoOfDivision" Width="100px"
                                                                                Text='<%#Bind("noOfDivision") %>' runat="server">
                                                                                <NumberFormat GroupSeparator="" DecimalDigits="0" KeepNotRoundedValue="false" />
                                                                                <%--<ClientEvents OnKeyPress="KeyPress" />--%>
                                                                            </telerik:RadNumericTextBox>
                                                                            <asp:RequiredFieldValidator ID="ReqNoOdDiv" runat="server" ErrorMessage="* Required!" ForeColor="Red"
                                                                                ControlToValidate="txtNoOfDivision" ValidationGroup="surveyValidation"></asp:RequiredFieldValidator>
                                                                        </EditItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn HeaderText="Completed Area" UniqueName="NoOfDivision" DataField="CompletedArea">
                                                                        <ItemTemplate>
                                                                            <%# Eval("CompletedArea") %>
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <telerik:RadNumericTextBox MinValue="0" MaxValue="999999999" MaxLength="9" ID="txtCompletedArea" ReadOnly="true"
                                                                                Width="100px" Text='<%#Bind("CompletedArea") %>' runat="server">
                                                                                <NumberFormat GroupSeparator="" DecimalDigits="0" KeepNotRoundedValue="false" />
                                                                            </telerik:RadNumericTextBox>
                                                                        </EditItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn HeaderText="Remaining Area" UniqueName="NoOfDivision" DataField="RemainingArea">
                                                                        <ItemTemplate>
                                                                            <%# Eval("RemainingArea") %>
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <telerik:RadNumericTextBox MinValue="0" MaxValue="999999999" MaxLength="9" ID="txtRemainingArea" Width="100px"
                                                                                ReadOnly="true" Text='<%#Bind("RemainingArea") %>' runat="server">
                                                                                <NumberFormat GroupSeparator="" DecimalDigits="0" KeepNotRoundedValue="false" />
                                                                            </telerik:RadNumericTextBox>
                                                                        </EditItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn HeaderText="Completed No. of Division" UniqueName="NoOfDivision" DataField="CompletedNoOfDivision">
                                                                        <ItemTemplate>
                                                                            <%# Eval("CompletedNoOfDivision") %>
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <telerik:RadNumericTextBox MinValue="0" MaxValue="999999999" MaxLength="9" ID="txtCompletedNoOfDivision"
                                                                                ReadOnly="true" Width="100px" Text='<%#Bind("CompletedNoOfDivision") %>' runat="server">
                                                                                <NumberFormat GroupSeparator="" DecimalDigits="0" KeepNotRoundedValue="false" />
                                                                            </telerik:RadNumericTextBox>
                                                                        </EditItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn HeaderText="Remaining No. of Division" UniqueName="RemainingNoOfDivision" DataField="RemainingNoOfDivision">
                                                                        <ItemTemplate>
                                                                            <%# Eval("RemainingNoOfDivision") %>
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <telerik:RadNumericTextBox MinValue="0" MaxValue="999999999" MaxLength="9" ID="txtRemainingNoOfDivision"
                                                                                ReadOnly="true" Width="100px" Text='<%#Bind("RemainingNoOfDivision") %>' runat="server">
                                                                                <NumberFormat GroupSeparator="" DecimalDigits="0" KeepNotRoundedValue="false" />
                                                                            </telerik:RadNumericTextBox>
                                                                        </EditItemTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridEditCommandColumn UniqueName="EditSurvey" HeaderText="Edit" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                        Exportable="false">
                                                                        <HeaderStyle Width="150px" />
                                                                    </telerik:GridEditCommandColumn>
                                                                    <telerik:GridButtonColumn CommandName="Delete" Text="Delete" UniqueName="DeleteColumn" />
                                                                </Columns>
                                                                <PagerStyle PageSizes="5,10,15" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                                                                    PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />
                                                            </MasterTableView>
                                                        </telerik:RadGrid>
                                                    </td>
                                                </tr>
                                            </tbody>

                                        </table>
                                    </div>
                                    <!--End of scroll-->
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row margin-0" id="BlockTab" runat="server" visible="false">
                    <div class="col-xs-12">
                        <div class="panel panel-default">
                            <div class="panel-heading" role="tab" id="">
                                <a role="" data-toggle="collapse" data-parent="" href="#collapsetwo" aria-expanded="true" aria-controls="" class="">Block Details <span class="glyphicon glyphicon-plus pull-right" aria-hidden="true"></span>
                                </a>
                            </div>
                            <div id="collapsetwo" class="panel-collapse collapse in" role="" aria-labelledby="" aria-expanded="true">
                                <div class="panel-body" style="overflow-x: scroll;">
                                    <table class="table table-striped table-bordered">
                                        <tfoot>
                                            <div class="col-lg-1  col-xs-3 padding-0">
                                                <asp:Button class="btn btn-primary margin-10 btn-sm" Text="Add Block" ID="btnBlock"
                                                    runat="server" OnClick="btnBlock_Click" CausesValidation="false"></asp:Button>
                                            </div>
                                        </tfoot>
                                        <tbody>
                                            <tr>
                                                <td>
                                                    <telerik:GridDropDownListColumnEditor runat="server" ID="DropdownEditor">
                                                        <DropDownStyle Width="100px" />
                                                    </telerik:GridDropDownListColumnEditor>
                                                    <telerik:RadGrid RenderMode="Lightweight" ID="gridBlockDetails" runat="server" OnNeedDataSource="gridBlockDetails_NeedDataSource"
                                                        AutoGenerateColumns="false" ValidationSettings-ValidationGroup="blockValidation"
                                                        AllowPaging="true" AllowSorting="false" AllowFilteringByColumn="false" PageSize="5"
                                                        OnInsertCommand="gridBlockDetails_InsertCommand" OnUpdateCommand="gridBlockDetails_UpdateCommand"
                                                        OnItemDataBound="gridBlockDetails_ItemDataBound" OnItemCommand="gridBlockDetails_ItemCommand" OnDeleteCommand="gridBlockDetails_DeleteCommand">
                                                        <GroupingSettings CaseSensitive="false" />
                                                        <MasterTableView CommandItemSettings-ShowRefreshButton="false" EditMode="InPlace" CommandItemDisplay="None"
                                                            AllowFilteringByColumn="false" AllowSorting="false" NoMasterRecordsText="" DataKeyNames="BlockId">
                                                            <CommandItemSettings ShowRefreshButton="false" ShowExportToExcelButton="false" />
                                                            <Columns>
                                                                <telerik:GridTemplateColumn HeaderText="Block No." UniqueName="BlockNo" DataField="BlockNo">
                                                                    <ItemTemplate>
                                                                        <%# Eval("BlockNo") %>
                                                                        <asp:Label ID="lblBlockno" runat="server" Visible="False" Text='<%# Eval("BlockNo") %>'> </asp:Label>
                                                                    </ItemTemplate>
                                                                    <EditItemTemplate>
                                                                        <telerik:RadDropDownList ID="ddlBlockNo" AutoPostBack="true" CausesValidation="false" OnSelectedIndexChanged="ddlBlockNo_SelectedIndexChanged"
                                                                            RenderMode="Lightweight" runat="server" DefaultMessage="Select Block">
                                                                        </telerik:RadDropDownList>
                                                                        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="ddlBlockNo"
                                                                            Display="Static" ErrorMessage="* Required!" ForeColor="Red" CssClass="validator" ValidationGroup="blockValidation" />
                                                                    </EditItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridTemplateColumn HeaderText="Proposed Quantity" UniqueName="ProposedQuantity" DataField="ProposedQuantity">
                                                                    <ItemTemplate>
                                                                        <%# Eval("ProposedQuantity") %>
                                                                    </ItemTemplate>
                                                                    <EditItemTemplate>
                                                                        <telerik:RadNumericTextBox ID="txtProposedQuantity" Text='<%#Bind("ProposedQuantity") %>' runat="server" MaxLength="9" ReadOnly="true">
                                                                            <NumberFormat DecimalDigits="2" />
                                                                        </telerik:RadNumericTextBox>
                                                                        <asp:RequiredFieldValidator ID="ReqProposedQuantitya" runat="server" ErrorMessage="* Required!" ForeColor="Red"
                                                                            Display="Static" ControlToValidate="txtProposedQuantity" ValidationGroup="blockValidation">
                                                                        </asp:RequiredFieldValidator>
                                                                    </EditItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridTemplateColumn HeaderText="Actual Quantity" UniqueName="ActualQuantity" DataField="ActualQuantity">
                                                                    <ItemTemplate>
                                                                        <%# Eval("ActualQuantity") %>
                                                                    </ItemTemplate>
                                                                    <EditItemTemplate>
                                                                        <telerik:RadNumericTextBox ID="txtActualQuantity" Text='<%#Bind("ActualQuantity") %>' runat="server" MaxLength="9">
                                                                        </telerik:RadNumericTextBox>
                                                                        <asp:RequiredFieldValidator ID="ReqActualQuantity" runat="server" ErrorMessage="* Required!" ForeColor="Red"
                                                                            Display="Static" ControlToValidate="txtActualQuantity" ValidationGroup="blockValidation">
                                                                        </asp:RequiredFieldValidator>
                                                                    </EditItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridTemplateColumn HeaderText="Completed Quantity" UniqueName="CompletedQuantity" DataField="CompletedQuantity">
                                                                    <ItemTemplate>
                                                                        <%# Eval("CompletedQuantity") %>
                                                                    </ItemTemplate>
                                                                    <EditItemTemplate>
                                                                        <telerik:RadNumericTextBox ID="txtCompletedQuantity" Text='<%#Bind("CompletedQuantity") %>' runat="server" MaxLength="9" ReadOnly="true">
                                                                        </telerik:RadNumericTextBox>
                                                                    </EditItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridTemplateColumn HeaderText="Remaining Quantity" UniqueName="RemainingQuantity" DataField="RemainingQuantity">
                                                                    <ItemTemplate>
                                                                        <%# Eval("RemainingQuantity") %>
                                                                    </ItemTemplate>
                                                                    <EditItemTemplate>
                                                                        <telerik:RadNumericTextBox ID="txtRemainingQuantity" Text='<%#Bind("RemainingQuantity") %>' runat="server"
                                                                            MaxLength="9" ReadOnly="true">
                                                                        </telerik:RadNumericTextBox>
                                                                    </EditItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridEditCommandColumn HeaderText="Edit" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                    UniqueName="EditBlock" Exportable="false">
                                                                    <HeaderStyle Width="150px" />
                                                                </telerik:GridEditCommandColumn>
                                                                <telerik:GridButtonColumn CommandName="Delete" Text="Delete" UniqueName="DeleteColumn" />
                                                            </Columns>
                                                            <PagerStyle PageSizes="5,10,15" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                                                                PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />
                                                        </MasterTableView>
                                                    </telerik:RadGrid>
                                                </td>
                                            </tr>
                                        </tbody>

                                    </table>

                                </div>
                                <!--End of scroll-->

                            </div>
                        </div>
                    </div>

                </div>
                <div class="row margin-0" id="GlobalTab" runat="server" visible="false" style="margin-top:-30px !important; padding-top:0px;">
                    <div class="col-xs-12 padding-lr-10 margin-10">
                        <div class="panel panel-default">
                            <div class="panel-heading" role="tab" id="">
                                    <a id="lblGlobalTab" runat="server" role="" data-toggle="collapse" data-parent="" href="#collapsethree" aria-expanded="true" aria-controls="" class="">
                                        <span class="caret pull-right" style="margin-top: 8px;"></span>
                                    </a>
                            </div>
                            <div id="collapsethree" class="panel-collapse collapse in" role="" aria-labelledby="" aria-expanded="true">
                                <div class="panel-body" style="overflow-x: scroll;">
                                    <div class="row margin-0">
                                        <div class="col-md-12 padding-0">
                                            <table class="table table-striped table-bordered">
                                                <tfoot>
                                                    <div class="col-lg-1  col-xs-3 padding-0">
                                                        <asp:Button class="btn btn-primary margin-10 btn-sm" Text="Add Range" ID="btnAddRange"
                                                            runat="server" OnClick="btnAddRange_Click" CausesValidation="false"></asp:Button>
                                                    </div>
                                                </tfoot>
                                                <tbody>
                                                    <tr>
                                                        <td>
                                                            <telerik:RadGrid RenderMode="Lightweight" ID="gridInverter" runat="server" OnNeedDataSource="gridInverter_NeedDataSource"
                                                                AutoGenerateColumns="false" ValidationSettings-ValidationGroup="inverterValidation"
                                                                AllowPaging="true" AllowSorting="false" AllowFilteringByColumn="false" PageSize="5"
                                                                OnInsertCommand="gridInverter_InsertCommand" OnUpdateCommand="gridInverter_UpdateCommand"
                                                                OnItemCommand="gridInverter_ItemCommand" OnDeleteCommand="gridInverter_DeleteCommand">
                                                                <GroupingSettings CaseSensitive="false" />
                                                                <MasterTableView CommandItemSettings-ShowRefreshButton="false" EditMode="InPlace" CommandItemDisplay="None"
                                                                    AllowFilteringByColumn="false" AllowSorting="false" NoMasterRecordsText="" DataKeyNames="TimesheetActivityId">
                                                                    <CommandItemSettings ShowRefreshButton="false" ShowExportToExcelButton="false" />
                                                                    <Columns>

                                                                        <telerik:GridTemplateColumn HeaderText="From Range" UniqueName="FromRange" DataField="FromRange">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblFromRange" runat="server" Text='<%# Eval("FromRange") %>'> </asp:Label>
                                                                            </ItemTemplate>
                                                                            <EditItemTemplate>
                                                                                <telerik:RadTextBox ID="txtFromRange" Text='<%#Bind("FromRange") %>' runat="server" InputType="Number">
                                                                                </telerik:RadTextBox>
                                                                                <asp:RequiredFieldValidator ID="ReqFromRange" runat="server" ErrorMessage="* Required!" ForeColor="Red"
                                                                                    ControlToValidate="txtFromRange" ValidationGroup="inverterValidation">
                                                                                </asp:RequiredFieldValidator>
                                                                            </EditItemTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                        <telerik:GridTemplateColumn HeaderText="To Range" UniqueName="ToRange" DataField="ToRange">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblToRange" runat="server" Text='<%# Eval("ToRange") %>'> </asp:Label>
                                                                            </ItemTemplate>
                                                                            <EditItemTemplate>
                                                                                <telerik:RadTextBox ID="txtToRange" Text='<%#Bind("ToRange") %>' runat="server" InputType="Number">
                                                                                </telerik:RadTextBox>
                                                                                <asp:RequiredFieldValidator ID="ReqToRange" runat="server" ErrorMessage="* Required!" ForeColor="Red"
                                                                                    ControlToValidate="txtToRange" ValidationGroup="inverterValidation">
                                                                                </asp:RequiredFieldValidator>
                                                                                <asp:CompareValidator ID="cmpValidateRange" Operator="GreaterThanEqual" ControlToValidate="txtToRange" ControlToCompare="txtFromRange"
                                                                                    ValidationGroup="inverterValidation" ErrorMessage="To range must be greater than From range." Type="Integer"
                                                                                    ForeColor="Red" runat="server"></asp:CompareValidator>
                                                                            </EditItemTemplate>
                                                                        </telerik:GridTemplateColumn>

                                                                        <telerik:GridEditCommandColumn HeaderText="Edit" HeaderStyle-HorizontalAlign="Center"
                                                                            UniqueName="EditTable" ItemStyle-HorizontalAlign="Center" Exportable="false">
                                                                            <HeaderStyle Width="150px" />
                                                                        </telerik:GridEditCommandColumn>
                                                                        <telerik:GridButtonColumn CommandName="Delete" Text="Delete" UniqueName="DeleteColumn" />
                                                                    </Columns>
                                                                    <PagerStyle PageSizes="5,10,15" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                                                                        PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />
                                                                </MasterTableView>
                                                            </telerik:RadGrid>
                                                        </td>
                                                    </tr>
                                                </tbody>

                                            </table>

                                        </div>
                                        <!--End of column 12-->

                                    </div>
                                    <!--End of row-->
                                    <div class="col-md-12">
                                    <div class="col-md-6 padding-0">
                                        <div class="col-md-3 col-xs-12 padding-0">
                                            <label class="col-xs-12 control-label lable-txt" for="name">Manual Entry</label>
                                        </div>
                                        <div class="col-md-9 col-xs-12 padding-0">
                                            <asp:TextBox runat="server" ID="txtManualEntry" CssClass="search-query form-control" ValidationGroup="ManualEntry"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ValidationExpression="^[0-9]{1,9}( *, *[0-9]{1,9})*$" runat="server"
                                                ValidationGroup="ManualEntry" ForeColor="Red" ControlToValidate="txtManualEntry" ErrorMessage="Please enter comma separated numbers."></asp:RegularExpressionValidator>
                                        </div>
                                    </div>
                                    <div class="col-md-6 col-xs-12 padding-0">
                                        <div style="margin-left: 5px; margin-top: -8px;">
                                            <asp:Button class="btn btn-primary margin-10 btn-sm" Text="Calculate Quantity" ID="btnCalculatePraposedQuantity" runat="server"
                                                OnClick="btnCalculatePraposedQuantity_Click" CausesValidation="true" ValidationGroup="ManualEntry"></asp:Button>
                                            <asp:HiddenField runat="server" ID="hidRangeQuantity" Value="0" />
                                            <asp:HiddenField runat="server" ID="hidManualEntryQuantity" Value="0" />
                                        </div>
                                    </div>
                                    <!-- End of row-->
                                    </div>
                                    <div class="col-md-12">
                                    <div class="col-md-6 padding-0">
                                        <div class="col-md-3 col-xs-12 padding-0">
                                            <label class="col-xs-12 control-label lable-txt" for="name">Quantity</label>
                                        </div>
                                        <!-- End of col-md-4 col-xs-12-->
                                        <div class="col-md-9 col-xs-12 padding-0">
                                            <%--<input type="text" class="search-query form-control" placeholder="Enter Quantity" id="txtQty" runat="server" />--%>
                                            <asp:TextBox runat="server" ID="txtQty" CssClass="search-query form-control" ReadOnly="true"></asp:TextBox>
                                        </div>
                                        <!-- End of col-xs-12-->
                                    </div>
                                        </div>
                                    <!-- End of row-->
                                </div>
                                <!--End of scroll-->

                            </div>
                        </div>
                    </div>
                </div>
                <div class="row margin-0" runat="server" id="divButtons">
                    <div class="col-sm-12 padding-0">
                        <div class="button-wrapper margin-10 text-center">
                            <div class="submit-btn">
                                <asp:Button ID="btnSubmit" runat="server" Text="Submit" class="btn btn-primary button button-style btn-sm"
                                    OnClick="btnSubmit_Click" ValidationGroup="formValidation"
                                    Style="margin-right: 10px; margin-bottom: 5px;"></asp:Button>
                            </div>
                            <div class="submit-btn">
                                <asp:Button ID="btnSave" runat="server" Text="Save" class="btn btn-primary button button-style btn-sm"
                                    Style="margin-right: 10px; margin-bottom: 5px;" OnClick="btnSave_Click" ValidationGroup="formValidation"></asp:Button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
            </div>
        <div id="divMangerSection" runat="server" visible="false" class="row margin-0" style="padding-left:10px; padding-right:10px;">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <a data-toggle="collapse" aria-expanded="true" href="#divManagerSectionBody"> Manager Approval Section</a> 
                </div>
                <div class="panel-body collapse in" id="divManagerSectionBody">
                    <div class="row" id="divMangerSurveySection" runat="server" visible="false">
                        <div class="col-xs-12">
                            <div class="panel panel-default">
                                <div class="panel-heading" role="tab" id="">
                                    <a role="" data-toggle="collapse" data-parent="" href="#divMangerSurveyDetails" aria-expanded="true" aria-controls="" class="">Survey Details <span class="glyphicon glyphicon-plus pull-right" aria-hidden="true"></span>
                                    </a>
                                </div>
                                <div id="divMangerSurveyDetails" class="panel-collapse collapse in" role="" aria-labelledby="" aria-expanded="true">
                                    <div class="panel-body" style="overflow-x: scroll;">
                                        <table class="table table-striped table-bordered">
                                            <tfoot>
                                                <div class="col-lg-1  col-xs-3 padding-0">
                                                    <asp:Button class="btn btn-primary margin-10 btn-sm" Text="Add Survey" ID="btnAddManagerSurvey"
                                                        runat="server" OnClick="btnAddManagerSurvey_Click" CausesValidation="false"></asp:Button>
                                                </div>
                                            </tfoot>
                                            <tbody>
                                                <tr>
                                                    <td>
                                                        <telerik:RadGrid RenderMode="Lightweight" ID="gridManagerSurvayDetails" runat="server" OnNeedDataSource="gridManagerSurvayDetails_NeedDataSource"
                                                            AutoGenerateColumns="false" ValidationSettings-ValidationGroup="surveyValidation"
                                                            AllowPaging="true" AllowSorting="false" AllowFilteringByColumn="false" PageSize="5"
                                                            OnInsertCommand="gridSurvey_InsertCommand" OnUpdateCommand="gridSurvey_UpdateCommand"
                                                            OnItemDataBound="gridSurvey_ItemDataBound" OnItemCommand="gridSurvey_ItemCommand" OnDeleteCommand="gridManagerSurvayDetails_DeleteCommand">
                                                            <GroupingSettings CaseSensitive="false" />
                                                            <MasterTableView CommandItemSettings-ShowRefreshButton="false" EditMode="InPlace" NoMasterRecordsText=""
                                                                CommandItemDisplay="None" AllowFilteringByColumn="false" AllowSorting="false" DataKeyNames="SurveyId">
                                                                <CommandItemSettings ShowRefreshButton="false" ShowExportToExcelButton="false" />
                                                                <Columns>
                                                                    <telerik:GridTemplateColumn HeaderText="Site" UniqueName="Site" DataField="Site" Visible="false">
                                                                        <ItemTemplate>
                                                                            <%# Eval("Site") %>
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <asp:Label ID="lblSite" runat="server" Visible="False" Text='<%# Eval("Site") %>'> </asp:Label>
                                                                            <telerik:RadDropDownList AutoPostBack="true" Width="150px" CausesValidation="false" OnSelectedIndexChanged="drpSite_SelectedIndexChanged" RenderMode="Lightweight" ID="drpSite" runat="server" DefaultMessage="Select Site" DropDownHeight="110px">
                                                                            </telerik:RadDropDownList>
                                                                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="drpSite"
                                                                                Display="Static" ErrorMessage="* Required!" ForeColor="Red" CssClass="validator" ValidationGroup="surveyValidation" />
                                                                        </EditItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn HeaderText="Project" UniqueName="Project" DataField="SAPProjectId" Visible="false">
                                                                        <ItemTemplate>
                                                                            <%# Eval("ProjectDescription") %>
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <asp:Label ID="lblProject" runat="server" Visible="False" Text='<%# Eval("SAPProjectId") %>'></asp:Label>
                                                                            <telerik:RadDropDownList RenderMode="Lightweight" Width="150px" ID="drpProject" runat="server"
                                                                                DefaultMessage="Select Project" DropDownHeight="110px">
                                                                            </telerik:RadDropDownList>
                                                                            <asp:RequiredFieldValidator runat="server" ID="ReqProjValidator" ControlToValidate="drpProject"
                                                                                Display="Static" ErrorMessage="* Required!" ForeColor="Red" CssClass="validator" ValidationGroup="surveyValidation" />
                                                                        </EditItemTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridTemplateColumn HeaderText="Village" UniqueName="Village" DataField="villageName">
                                                                        <ItemTemplate>
                                                                            <%# Eval("villageName") %>
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <asp:Label ID="lblVillage" runat="server" Visible="False" Text='<%# Eval("villageId") %>'></asp:Label>
                                                                            <telerik:RadDropDownList RenderMode="Lightweight" Width="150px" ID="drpVillage" runat="server" AutoPostBack="true"
                                                                                OnSelectedIndexChanged="drpVillage_SelectedIndexChanged" DefaultMessage="Select Village" DropDownHeight="110px">
                                                                            </telerik:RadDropDownList>
                                                                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator3" ControlToValidate="drpVillage"
                                                                                Display="Static" ErrorMessage="* Required!" ForeColor="Red" CssClass="validator" ValidationGroup="surveyValidation" />
                                                                        </EditItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn HeaderText="Survey No." UniqueName="SurveyNo" DataField="SurveyNo">
                                                                        <ItemTemplate>
                                                                            <%# Eval("surveyNo") %>
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <asp:Label ID="lblSurveyNo" runat="server" Visible="False" Text='<%# Eval("surveyNo") %>'></asp:Label>
                                                                            <telerik:RadDropDownList RenderMode="Lightweight" Width="150px" ID="drpSurvayNo" runat="server" OnSelectedIndexChanged="drpSurvayNo_SelectedIndexChanged"
                                                                                AutoPostBack="true" DefaultMessage="Select Survey Number" DropDownHeight="110px">
                                                                            </telerik:RadDropDownList>
                                                                            <asp:RequiredFieldValidator runat="server" ID="reqSurveyNumber" ControlToValidate="drpSurvayNo"
                                                                                Display="Static" ErrorMessage="* Required!" ForeColor="Red" CssClass="validator" ValidationGroup="surveyValidation" />
                                                                        </EditItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn HeaderText="Proposed Area" UniqueName="PraposedArea" DataField="PraposedArea">
                                                                        <ItemTemplate>
                                                                            <%# Eval("PraposedArea") %>
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <telerik:RadNumericTextBox MinValue="0" ReadOnly="true" MaxValue="999999999" MaxLength="9" ID="txtPraposedArea" Text='<%#Bind("PraposedArea") %>'
                                                                                Width="100px" runat="server">
                                                                                <NumberFormat GroupSeparator="" DecimalDigits="0" KeepNotRoundedValue="false" />
                                                                            </telerik:RadNumericTextBox>
                                                                            <asp:RequiredFieldValidator runat="server" ErrorMessage="* Required!" ForeColor="Red"
                                                                                ControlToValidate="txtPraposedArea" ValidationGroup="surveyValidation"></asp:RequiredFieldValidator>
                                                                        </EditItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn HeaderText="Proposed No. of Division" UniqueName="PraposedNoOfDivision" DataField="PraposedNoOfDivision">
                                                                        <ItemTemplate>
                                                                            <%# Eval("PraposedNoOfDivision") %>
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <telerik:RadNumericTextBox MinValue="0" MaxValue="999999999" MaxLength="9" ID="txtPraposedNoOfDivision" Text='<%#Bind("PraposedNoOfDivision") %>'
                                                                                Width="100px" ReadOnly="true" runat="server">
                                                                                <NumberFormat GroupSeparator="" DecimalDigits="0" KeepNotRoundedValue="false" />
                                                                            </telerik:RadNumericTextBox>
                                                                            <asp:RequiredFieldValidator ID="ReqPraposedNoOdDiv" runat="server" ErrorMessage="* Required!" ForeColor="Red"
                                                                                ControlToValidate="txtPraposedNoOfDivision" ValidationGroup="surveyValidation"></asp:RequiredFieldValidator>
                                                                        </EditItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn HeaderText="Area" UniqueName="Area" DataField="Area">
                                                                        <ItemTemplate>
                                                                            <%# Eval("Area") %>
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <telerik:RadNumericTextBox ID="txtArea" Text='<%#Bind("Area") %>' runat="server" MaxLength="12" Width="100px">
                                                                                <NumberFormat DecimalDigits="2" DecimalSeparator="." AllowRounding="true" />
                                                                            </telerik:RadNumericTextBox>
                                                                            <asp:RequiredFieldValidator ID="ReqArea" runat="server" ErrorMessage="* Required!" ForeColor="Red"
                                                                                ControlToValidate="txtArea" ValidationGroup="surveyValidation"></asp:RequiredFieldValidator>
                                                                        </EditItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn HeaderText="No. of Division" UniqueName="NoOfDivision" DataField="noOfDivision">
                                                                        <ItemTemplate>
                                                                            <%# Eval("noOfDivision") %>
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <telerik:RadNumericTextBox MinValue="0" MaxValue="999999999" MaxLength="9" ID="txtNoOfDivision" Width="100px"
                                                                                Text='<%#Bind("noOfDivision") %>' runat="server">
                                                                                <NumberFormat GroupSeparator="" DecimalDigits="0" KeepNotRoundedValue="false" />
                                                                                <%--<ClientEvents OnKeyPress="KeyPress" />--%>
                                                                            </telerik:RadNumericTextBox>
                                                                            <asp:RequiredFieldValidator ID="ReqNoOdDiv" runat="server" ErrorMessage="* Required!" ForeColor="Red"
                                                                                ControlToValidate="txtNoOfDivision" ValidationGroup="surveyValidation"></asp:RequiredFieldValidator>
                                                                        </EditItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn HeaderText="Completed Area" UniqueName="NoOfDivision" DataField="CompletedArea">
                                                                        <ItemTemplate>
                                                                            <%# Eval("CompletedArea") %>
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <telerik:RadNumericTextBox MinValue="0" MaxValue="999999999" MaxLength="9" ID="txtCompletedArea" ReadOnly="true"
                                                                                Width="100px" Text='<%#Bind("CompletedArea") %>' runat="server">
                                                                                <NumberFormat GroupSeparator="" DecimalDigits="0" KeepNotRoundedValue="false" />
                                                                            </telerik:RadNumericTextBox>
                                                                        </EditItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn HeaderText="Remaining Area" UniqueName="NoOfDivision" DataField="RemainingArea">
                                                                        <ItemTemplate>
                                                                            <%# Eval("RemainingArea") %>
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <telerik:RadNumericTextBox MinValue="0" MaxValue="999999999" MaxLength="9" ID="txtRemainingArea" Width="100px"
                                                                                ReadOnly="true" Text='<%#Bind("RemainingArea") %>' runat="server">
                                                                                <NumberFormat GroupSeparator="" DecimalDigits="0" KeepNotRoundedValue="false" />
                                                                            </telerik:RadNumericTextBox>
                                                                        </EditItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn HeaderText="Completed No. of Division" UniqueName="NoOfDivision" DataField="CompletedNoOfDivision">
                                                                        <ItemTemplate>
                                                                            <%# Eval("CompletedNoOfDivision") %>
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <telerik:RadNumericTextBox MinValue="0" MaxValue="999999999" MaxLength="9" ID="txtCompletedNoOfDivision"
                                                                                ReadOnly="true" Width="100px" Text='<%#Bind("CompletedNoOfDivision") %>' runat="server">
                                                                                <NumberFormat GroupSeparator="" DecimalDigits="0" KeepNotRoundedValue="false" />
                                                                            </telerik:RadNumericTextBox>
                                                                        </EditItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn HeaderText="Remaining No. of Division" UniqueName="RemainingNoOfDivision" DataField="RemainingNoOfDivision">
                                                                        <ItemTemplate>
                                                                            <%# Eval("RemainingNoOfDivision") %>
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <telerik:RadNumericTextBox MinValue="0" MaxValue="999999999" MaxLength="9" ID="txtRemainingNoOfDivision"
                                                                                ReadOnly="true" Width="100px" Text='<%#Bind("RemainingNoOfDivision") %>' runat="server">
                                                                                <NumberFormat GroupSeparator="" DecimalDigits="0" KeepNotRoundedValue="false" />
                                                                            </telerik:RadNumericTextBox>
                                                                        </EditItemTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridEditCommandColumn UniqueName="EditSurvey" HeaderText="Edit" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                        Exportable="false">
                                                                        <HeaderStyle Width="150px" />
                                                                    </telerik:GridEditCommandColumn>
                                                                    <telerik:GridButtonColumn CommandName="Delete" Text="Delete" UniqueName="DeleteColumn" />
                                                                </Columns>
                                                                <PagerStyle PageSizes="5,10,15" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                                                                    PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />
                                                            </MasterTableView>
                                                        </telerik:RadGrid>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>
                                    <!--End of scroll-->
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row margin-0" id="divMangerBlockSection" runat="server" visible="false">
                        <div class="col-xs-12">
                            <div class="panel panel-default">
                                <div class="panel-heading" role="tab" id="">
                                        <a role="" data-toggle="collapse" data-parent="" href="#divMangerBlockDetails" aria-expanded="true" aria-controls="" class="">Block Details <span class="glyphicon glyphicon-plus pull-right" aria-hidden="true"></span>
                                        </a>
                                </div>
                                <div id="divMangerBlockDetails" class="panel-collapse collapse in" role="" aria-labelledby="" aria-expanded="true">
                                    <div class="panel-body" style="overflow-x: scroll;">
                                        <table class="table table-striped table-bordered">
                                            <tfoot>
                                                <div class="col-lg-1  col-xs-3 padding-0">
                                                    <asp:Button class="btn btn-primary margin-10 btn-sm" Text="Add Block" ID="btnAddManagerBlock"
                                                        runat="server" OnClick="btnAddManagerBlock_Click" CausesValidation="false"></asp:Button>
                                                </div>
                                            </tfoot>
                                            <tbody>
                                                <tr>
                                                    <td>
                                                        <telerik:GridDropDownListColumnEditor runat="server" ID="GridDropDownListColumnEditor1">
                                                            <DropDownStyle Width="100px" />
                                                        </telerik:GridDropDownListColumnEditor>
                                                        <telerik:RadGrid RenderMode="Lightweight" ID="gridManagerBlockDetails" runat="server" OnNeedDataSource="gridManagerBlockDetails_NeedDataSource"
                                                            AutoGenerateColumns="false" ValidationSettings-ValidationGroup="blockValidation"
                                                            AllowPaging="true" AllowSorting="false" AllowFilteringByColumn="false" PageSize="5"
                                                            OnInsertCommand="gridBlockDetails_InsertCommand" OnUpdateCommand="gridBlockDetails_UpdateCommand"
                                                            OnItemDataBound="gridBlockDetails_ItemDataBound" OnItemCommand="gridBlockDetails_ItemCommand" OnDeleteCommand="gridBlockDetails_DeleteCommand">
                                                            <GroupingSettings CaseSensitive="false" />
                                                            <MasterTableView CommandItemSettings-ShowRefreshButton="false" EditMode="InPlace" CommandItemDisplay="None"
                                                                AllowFilteringByColumn="false" AllowSorting="false" NoMasterRecordsText="" DataKeyNames="BlockId">
                                                                <CommandItemSettings ShowRefreshButton="false" ShowExportToExcelButton="false" />
                                                                <Columns>
                                                                    <telerik:GridTemplateColumn HeaderText="Block No." UniqueName="BlockNo" DataField="BlockNo">
                                                                        <ItemTemplate>
                                                                            <%# Eval("BlockNo") %>
                                                                            <asp:Label ID="lblBlockno" runat="server" Visible="False" Text='<%# Eval("BlockNo") %>'> </asp:Label>
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <telerik:RadDropDownList ID="ddlBlockNo" AutoPostBack="true" CausesValidation="false" OnSelectedIndexChanged="ddlBlockNo_SelectedIndexChanged"
                                                                                RenderMode="Lightweight" runat="server" DefaultMessage="Select Block">
                                                                            </telerik:RadDropDownList>
                                                                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="ddlBlockNo"
                                                                                Display="Static" ErrorMessage="* Required!" ForeColor="Red" CssClass="validator" ValidationGroup="blockValidation" />
                                                                        </EditItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn HeaderText="Proposed Quantity" UniqueName="ProposedQuantity" DataField="ProposedQuantity">
                                                                        <ItemTemplate>
                                                                            <%# Eval("ProposedQuantity") %>
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <telerik:RadNumericTextBox ID="txtProposedQuantity" Text='<%#Bind("ProposedQuantity") %>' runat="server" MaxLength="9" ReadOnly="true">
                                                                                <NumberFormat DecimalDigits="2" />
                                                                            </telerik:RadNumericTextBox>
                                                                            <asp:RequiredFieldValidator ID="ReqProposedQuantitya" runat="server" ErrorMessage="* Required!" ForeColor="Red"
                                                                                Display="Static" ControlToValidate="txtProposedQuantity" ValidationGroup="blockValidation">
                                                                            </asp:RequiredFieldValidator>
                                                                        </EditItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn HeaderText="Actual Quantity" UniqueName="ActualQuantity" DataField="ActualQuantity">
                                                                        <ItemTemplate>
                                                                            <%# Eval("ActualQuantity") %>
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <telerik:RadNumericTextBox ID="txtActualQuantity" Text='<%#Bind("ActualQuantity") %>' runat="server" MaxLength="9">
                                                                            </telerik:RadNumericTextBox>
                                                                            <asp:RequiredFieldValidator ID="ReqActualQuantity" runat="server" ErrorMessage="* Required!" ForeColor="Red"
                                                                                Display="Static" ControlToValidate="txtActualQuantity" ValidationGroup="blockValidation">
                                                                            </asp:RequiredFieldValidator>
                                                                        </EditItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn HeaderText="Completed Quantity" UniqueName="CompletedQuantity" DataField="CompletedQuantity">
                                                                        <ItemTemplate>
                                                                            <%# Eval("CompletedQuantity") %>
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <telerik:RadNumericTextBox ID="txtCompletedQuantity" Text='<%#Bind("CompletedQuantity") %>' runat="server" MaxLength="9" ReadOnly="true">
                                                                                <NumberFormat DecimalDigits="2" />
                                                                            </telerik:RadNumericTextBox>
                                                                        </EditItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn HeaderText="Remaining Quantity" UniqueName="RemainingQuantity" DataField="RemainingQuantity">
                                                                        <ItemTemplate>
                                                                            <%# Eval("RemainingQuantity") %>
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <telerik:RadNumericTextBox ID="txtRemainingQuantity" Text='<%#Bind("RemainingQuantity") %>' runat="server"
                                                                                MaxLength="9" ReadOnly="true">
                                                                            </telerik:RadNumericTextBox>
                                                                        </EditItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridEditCommandColumn HeaderText="Edit" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                        UniqueName="EditBlock" Exportable="false">
                                                                        <HeaderStyle Width="150px" />
                                                                    </telerik:GridEditCommandColumn>
                                                                    <telerik:GridButtonColumn CommandName="Delete" Text="Delete" UniqueName="DeleteColumn" />
                                                                </Columns>
                                                                <PagerStyle PageSizes="5,10,15" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                                                                    PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />
                                                            </MasterTableView>
                                                        </telerik:RadGrid>
                                                    </td>
                                                </tr>
                                            </tbody>

                                        </table>

                                    </div>
                                    <!--End of scroll-->

                                </div>
                            </div>
                        </div>

                    </div>
                    <div class="row margin-0" id="divManagerActivitySection" runat="server" visible="false">
                        <div class="col-xs-12 margin-0 padding-0">
                            <div class="panel panel-default">
                                <div class="panel-heading" role="tab" id="">
                                    <a id="lnkTableSCBInvertor" runat="server" data-toggle="collapse" href="#divManagerActivityDetails"
                                        aria-expanded="true" aria-controls="" class="">
                                        <span class="caret pull-right" style="margin-top: 8px;"></span>
                                    </a>
                                </div>
                                <div id="divManagerActivityDetails" class="panel-collapse collapse in" role="" aria-labelledby="" aria-expanded="true">
                                    <div class="panel-body" style="overflow-x: scroll;">
                                        <div class="row margin-0">
                                            <div class="col-md-12 padding-0">
                                                <table class="table table-striped table-bordered">
                                                    <tfoot>
                                                        <div class="col-lg-1  col-xs-3 padding-0">
                                                            <asp:Button class="btn btn-primary margin-10 btn-sm" Text="Add Range" ID="btnAddManagerRange"
                                                                runat="server" OnClick="btnAddManagerRange_Click" CausesValidation="false"></asp:Button>
                                                        </div>
                                                    </tfoot>
                                                    <tbody>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadGrid RenderMode="Lightweight" ID="gridManagerInvertor" runat="server" OnNeedDataSource="gridManagerInvertor_NeedDataSource"
                                                                    AutoGenerateColumns="false" ValidationSettings-ValidationGroup="inverterValidation"
                                                                    AllowPaging="true" AllowSorting="false" AllowFilteringByColumn="false" PageSize="5"
                                                                    OnInsertCommand="gridInverter_InsertCommand" OnUpdateCommand="gridInverter_UpdateCommand"
                                                                    OnItemCommand="gridInverter_ItemCommand" OnDeleteCommand="gridInverter_DeleteCommand">
                                                                    <GroupingSettings CaseSensitive="false" />
                                                                    <MasterTableView CommandItemSettings-ShowRefreshButton="false" EditMode="InPlace" CommandItemDisplay="None"
                                                                        AllowFilteringByColumn="false" AllowSorting="false" NoMasterRecordsText="" DataKeyNames="TimesheetActivityId">
                                                                        <CommandItemSettings ShowRefreshButton="false" ShowExportToExcelButton="false" />
                                                                        <Columns>
                                                                            <telerik:GridTemplateColumn HeaderText="From Range" UniqueName="FromRange" DataField="FromRange">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblFromRange" runat="server" Text='<%# Eval("FromRange") %>'> </asp:Label>
                                                                                </ItemTemplate>
                                                                                <EditItemTemplate>
                                                                                    <telerik:RadTextBox ID="txtFromRange" Text='<%#Bind("FromRange") %>' runat="server" InputType="Number">
                                                                                    </telerik:RadTextBox>
                                                                                    <asp:RequiredFieldValidator ID="ReqFromRange" runat="server" ErrorMessage="* Required!" ForeColor="Red"
                                                                                        ControlToValidate="txtFromRange" ValidationGroup="inverterValidation">
                                                                                    </asp:RequiredFieldValidator>
                                                                                </EditItemTemplate>
                                                                            </telerik:GridTemplateColumn>
                                                                            <telerik:GridTemplateColumn HeaderText="To Range" UniqueName="ToRange" DataField="ToRange">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblToRange" runat="server" Text='<%# Eval("ToRange") %>'> </asp:Label>
                                                                                </ItemTemplate>
                                                                                <EditItemTemplate>
                                                                                    <telerik:RadTextBox ID="txtToRange" Text='<%#Bind("ToRange") %>' runat="server" InputType="Number">
                                                                                    </telerik:RadTextBox>
                                                                                    <asp:RequiredFieldValidator ID="ReqToRange" runat="server" ErrorMessage="* Required!" ForeColor="Red"
                                                                                        ControlToValidate="txtToRange" ValidationGroup="inverterValidation">
                                                                                    </asp:RequiredFieldValidator>
                                                                                    <asp:CompareValidator ID="cmpValidateRange" Operator="GreaterThanEqual" ControlToValidate="txtToRange" ControlToCompare="txtFromRange"
                                                                                        ValidationGroup="inverterValidation" ErrorMessage="To range must be greater than From range." Type="Integer"
                                                                                        ForeColor="Red" runat="server"></asp:CompareValidator>
                                                                                </EditItemTemplate>
                                                                            </telerik:GridTemplateColumn>

                                                                            <telerik:GridEditCommandColumn HeaderText="Edit" HeaderStyle-HorizontalAlign="Center"
                                                                                UniqueName="EditTable" ItemStyle-HorizontalAlign="Center" Exportable="false">
                                                                                <HeaderStyle Width="150px" />
                                                                            </telerik:GridEditCommandColumn>
                                                                            <telerik:GridButtonColumn CommandName="Delete" Text="Delete" UniqueName="DeleteColumn" />
                                                                        </Columns>
                                                                        <PagerStyle PageSizes="5,10,15" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                                                                            PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />
                                                                    </MasterTableView>
                                                                </telerik:RadGrid>
                                                            </td>
                                                        </tr>
                                                    </tbody>

                                                </table>

                                            </div>
                                            <!--End of column 12-->

                                        </div>
                                        <!--End of row-->
                                        <div class="col-md-12">
                                        <div class="col-md-6 padding-0">
                                            <div class="col-md-3 col-xs-12 padding-0">
                                                <label class="col-xs-12 control-label lable-txt" for="name">Manual Entry</label>
                                            </div>
                                            <!-- End of col-md-4 col-xs-12-->
                                            <div class="col-md-9 col-xs-12 padding-0">
                                                <%--<telerik:RadTextBox InputType="Number" runat="server" ID="txtManualEntry" CssClass="search-query form-control" ValidationGroup="ManualEntry"></telerik:RadTextBox>--%>
                                                <asp:TextBox runat="server" ID="txtManagerManualEntry" CssClass="search-query form-control" ValidationGroup="ManualEntry"></asp:TextBox>
                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator3" ValidationExpression="^[0-9]{1,9}( *, *[0-9]{1,9})*$" runat="server"
                                                    ValidationGroup="ManualEntry" ForeColor="Red" ControlToValidate="txtManualEntry" ErrorMessage="Please enter comma separated numbers."></asp:RegularExpressionValidator>
                                            </div>
                                            <!-- End of col-xs-12-->
                                        </div>
                                        <div class="col-md-6 col-xs-12 padding-0">
                                            <div style="margin-left: 5px; margin-top: -8px;">
                                                <asp:Button class="btn btn-primary margin-10 btn-sm" Text="Calculate Quantity" ID="btnManagerManualRange" runat="server"
                                                    OnClick="btnManagerManualRange_Click" CausesValidation="true"></asp:Button>
                                                <asp:HiddenField runat="server" ID="HiddenField1" Value="0" />
                                                <asp:HiddenField runat="server" ID="HiddenField2" Value="0" />
                                            </div>
                                        </div>
                                        <!-- End of row-->
                                            </div>
                                        <div class="col-md-12">
                                        <div class="col-md-6 padding-0">
                                            <div class="col-md-3 col-xs-12 padding-0">
                                                <label class="col-xs-12 control-label lable-txt" for="name">Quantity</label>
                                            </div>
                                            <!-- End of col-md-4 col-xs-12-->
                                            <div class="col-md-9 col-xs-12 padding-0">
                                                <asp:TextBox runat="server" ID="txtManagerQuantity" CssClass="search-query form-control" ReadOnly="true"></asp:TextBox>
                                            </div>
                                            <!-- End of col-xs-12-->
                                        </div>
                                        <div class="col-md-6 padding-0">
                                            </div>
                                            </div>
                                        <!-- End of row-->
                                    </div>
                                    <!--End of scroll-->

                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row margin-0" id="divMangerNumberSection" runat="server" visible="false">
                        <div class="col-md-6">
                            <div id="divManagerNumbers" runat="server" class="col-md-12">
                                <div class="col-md-4 col-xs-12 padding-0">
                                    <label class="col-xs-12 control-label lable-txt" for="name">Number</label>
                                </div>
                                <!-- End of col-md-4 col-xs-12-->
                                <div class="col-md-8 col-xs-12 padding-0">
                                    <div class="form-group">                                      
                                        <asp:TextBox ID="txtManagerNumber" runat="server" CssClass="search-query form-control" ValidationGroup="formValidation"
                                            onchange="calculateQuantity(this,'manager')" ClientEvents-OnBlur="calculateQuantity(this,'manager')"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator4" ValidationExpression="^[0-9]{1,9}( *, *[0-9]{1,9})*$" runat="server"
                                            ValidationGroup="formValidation" ForeColor="Red" ControlToValidate="txtManagerNumber" ErrorMessage="Please enter comma separated numbers."></asp:RegularExpressionValidator>
                                    </div>
                                </div>
                            </div>
                            <div id="divManagerActualQunatity" runat="server" class="col-md-12" visible="false">
                                <div class="col-md-4 col-xs-12 padding-0">
                                    <label class="col-xs-12 control-label lable-txt" for="name">Actual Quantity</label>
                                </div>
                                <!-- End of col-md-4 col-xs-12-->
                                <div class="col-md-8 col-xs-12 padding-0">
                                    <div class="form-group">
                                        <telerik:RadTextBox ID="txtManagerActualQuantity" MaxLength="10" runat="server" Enabled="false" InputType="Number" Width="100%" CssClass="search-query form-control"
                                            EmptyMessage="Actual Quantity">
                                        </telerik:RadTextBox>
                                        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ControlToValidate="txtManagerActualQuantity"
                                            ErrorMessage="Please enter a Actual Quantity!" ForeColor="Red" ValidationGroup="formValidation"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <!-- End of col-xs-12-->
                            </div>
                        </div>
                    </div>
                    <div class="row" id="div1" runat="server">
                        <div class="col-md-12 col-xs-12">
                             <div class="col-md-6 col-xs-12  padding-0">
                                <div class="col-md-4 col-xs-12  padding-0">
                                    <label class="col-xs-12 control-label lable-txt" for="name">Approved Quantity</label>
                                </div>
                                <div class="col-md-8 col-xs-12 padding-0 ">
                                    <asp:TextBox TextMode="Number" runat="server" MaxLength="50" Enabled="false"
                                        ID="txtApprovedQuantity" Style="width: 100%" CssClass="search-query form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator4" ControlToValidate="txtApprovedQuantity"
                                        ErrorMessage="Please enter Approved Quantity!" ForeColor="Red" ValidationGroup="formValidation"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="col-md-6 col-xs-12">
                                <div class="col-md-3 col-xs-12">
                                    <label class="col-xs-12 control-label lable-txt" for="name">Comments</label>
                                </div>
                                <div class="col-md-9 col-xs-12 padding-0 ">
                                    <asp:TextBox TextMode="MultiLine" runat="server" MaxLength="50" Rows="1" ID="txtManagerComments" Style="width: 100%"></asp:TextBox>
                                    <asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator5" ForeColor="Red"
                                        ControlToValidate="txtManagerComments"
                                        ValidationGroup="formValidation"
                                        ValidationExpression="^[\s\S]{0,200}$"
                                        ErrorMessage="Maximum 200 characters allowed."
                                        Display="Static"></asp:RegularExpressionValidator>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row margin-0">
                        <div class="text-center">
                            <asp:Button ID="btnPartialApprove" runat="server" Text="Partial Approve" class="btn btn-primary button button-style btn-sm"
                                ValidationGroup="formValidation" OnClick="btnPartialApprove_Click" Style="margin-right: 10px; margin-bottom: 5px; width: 150px;"></asp:Button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
