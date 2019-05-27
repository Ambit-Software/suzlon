<%@ Page Title="Project Management Application To Do List" Language="C#" MasterPageFile="~/MasterPages/SolarPMS.Master" EnableEventValidation="false"
    AutoEventWireup="true" CodeBehind="ToDoList.aspx.cs" Inherits="SolarPMS.ToDoList" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <telerik:RadScriptBlock ID="radSript1" runat="server">
        <script type="text/javascript">
            $(document).ready(function () {
                $('[id^=detail-]').hide();
                $('.toggle').click(function () {
                    $input = $(this);
                    $target = $('#' + $input.attr('data-toggle'));
                    $target.slideToggle();
                });
            });

            function openAttachment(activityid, subactivityid, rowIndex) {
                setTimeout(function () {
                    var ajaxManager = $find("<%= RadAjaxManager.GetCurrent(this.Page).ClientID %>");// $find("<%= ToDoListRadAjaxManager.ClientID %>");
                    ajaxManager.ajaxRequest("TimesheetAttachment#" + activityid + "#" + subactivityid);
                    $("#divViewAttachment").modal("show");
                }, 1000);
            }

            function openAttachmentForActivity(activityid, subactivityid, rowIndex) {
                var ajaxManager = $find("<%= RadAjaxManager.GetCurrent(this.Page).ClientID %>");// $find("<%= ToDoListRadAjaxManager.ClientID %>");
                ajaxManager.ajaxRequest("TimesheetAttachmentForActivity#" + activityid + "#" + subactivityid);
                $("#divViewAttachment").modal("show");
            }

            function viewTimesheets(activityid, subactivityid, rowIndex) {
                var ajaxManager = $find("<%= RadAjaxManager.GetCurrent(this.Page).ClientID %>");// $find("<%= ToDoListRadAjaxManager.ClientID %>");
                ajaxManager.ajaxRequest("ViewTimesheet#" + activityid + "#" + subactivityid);
                $("#divViewTimesheet").modal("show");
            }

            function viewAssignedHistory(activityid, subactivityid, rowIndex) {
                setTimeout(function () {
                    var ajaxManager = $find("<%= RadAjaxManager.GetCurrent(this.Page).ClientID %>");// $find("<%= ToDoListRadAjaxManager.ClientID %>");
                    ajaxManager.ajaxRequest("ViewAssignedHistory#" + activityid + "#" + subactivityid);
                    $("#divViewHistory").modal("show");
                }, 1000);
            }

            function viewIssues(activityid, subactivityid, rowIndex) {
                var ajaxManager = $find("<%= RadAjaxManager.GetCurrent(this.Page).ClientID %>");// $find("<%= ToDoListRadAjaxManager.ClientID %>");
                ajaxManager.ajaxRequest("ViewIssues#" + activityid + "#" + subactivityid);
                $("#divViewIssues").modal("show");
            }

            function viewInverterSCBTable(activityid, subactivityid, rowIndex) {
                var ajaxManager = $find("<%= RadAjaxManager.GetCurrent(this.Page).ClientID %>");// $find("<%= ToDoListRadAjaxManager.ClientID %>");
                ajaxManager.ajaxRequest("ViewInverterSCBTable#" + activityid + "#" + subactivityid);
                $("#divViewInvSCBTable").modal("show");
            }

            function viewSurveys(activityid, subactivityid, rowIndex) {
                var ajaxManager = $find("<%= RadAjaxManager.GetCurrent(this.Page).ClientID %>");// $find("<%= ToDoListRadAjaxManager.ClientID %>");
                ajaxManager.ajaxRequest("ViewSurvey#" + activityid + "#" + subactivityid);
                $("#divViewSurveys").modal("show");
            }

            function viewComments(activityid, subactivityid, rowIndex) {
                var ajaxManager = $find("<%= RadAjaxManager.GetCurrent(this.Page).ClientID %>");// $find("<%= ToDoListRadAjaxManager.ClientID %>");
                ajaxManager.ajaxRequest("ViewComments#" + activityid + "#" + subactivityid);
                $("#divViewComments").modal("show");
            }

            function viewBlocks(activityid, subactivityid, rowIndex) {
                var ajaxManager = $find("<%= RadAjaxManager.GetCurrent(this.Page).ClientID %>");// $find("<%= ToDoListRadAjaxManager.ClientID %>");
                ajaxManager.ajaxRequest("ViewBlocks#" + activityid + "#" + subactivityid);
                $("#divViewBlocks").modal("show");
            }

            function onRequestStart(sender, args) {
                if (args.get_eventTarget().indexOf("btnExport") >= 0)
                    args.set_enableAjax(false);
            }

            (function (global, undefined) {
                var widthTextBox, heightTextBox, leftTextBox, topTextBox;

                function widthTextBox_load(sender, args) {
                    widthTextBox = sender;
                }
                function heightTextBox_load(sender, args) {
                    heightTextBox = sender;
                }
                function leftTextBox_load(sender, args) {
                    leftTextBox = sender;
                }
                function topTextBox_load(sender, args) {
                    topTextBox = sender;
                }

                function openRadWin(screen) {
                    var manager = $find("<%= TimesheetRadWindowManager.ClientID %>");
                    if (screen == 'timesheet') {
                        manager.open("TimeSheetmanagement.aspx", "TimesheetRadWindow");
                        return false;
                    }
                    else
                        if (screen == 'issue') {
                            manager.open("IssueManagementDetail.aspx", "TimesheetRadWindow");
                            return false;
                        }
                }

                function openIssueDetailsRadWin() {
                    var manager = $find("<%= TimesheetRadWindowManager.ClientID %>");
                    manager.open("IssueManagementDetail.aspx", "TimesheetRadWindow");
                    return false;
                }
                global.openRadWin = openRadWin;
                //global.openPopUp = openPopUp;
                global.widthTextBox_load = widthTextBox_load;
                global.heightTextBox_load = heightTextBox_load;
                global.leftTextBox_load = leftTextBox_load;
                global.topTextBox_load = topTextBox_load;
            })(window);

            function RebindMyRecordsData() {
                $("#divViewTimesheet").modal("hide");
                $("#divViewIssues").modal("hide");

                var gridActivity = $find("<%= gridActivityDetails.ClientID %>").get_masterTableView();
                gridActivity.rebind();

                var pendingTimesheet = $find("<%= gridPendingTimesheet.ClientID %>").get_masterTableView();
                pendingTimesheet.rebind();

                var gridRejectedTimesheet = $find("<%= gridRejectedTimesheet.ClientID %>").get_masterTableView();
                gridRejectedTimesheet.rebind();
            }

            var gridToCheckAll = '<%= gridPendingTimesheet.ClientID %>';
            function checkAllChildGridCheckbox(sender) {
                var checked = $(sender)[0].checked;
                var parentCheckbox = $(sender).parents("div").siblings("div:first").find("table input[type='checkbox']");
                var allCheckboxLength = $(sender).closest("table").find("input[type='checkbox']").length;
                var checkedCheckboxLength = $(sender).closest("table").find("input[type='checkbox']:checked").length;
                if (allCheckboxLength == checkedCheckboxLength)
                    parentCheckbox[0].checked = true;
                else
                    parentCheckbox[0].checked = false;
            }


            function checkAllRows(sender) {
                var checked = sender.checked;
                var container = document.getElementById(gridToCheckAll);
                var checkboxes = container.getElementsByTagName('input');
                for (var i = 0, l = checkboxes.length; i < l; i++) {
                    if (checkboxes[i] != sender && !checkboxes[i].disabled)
                        checkboxes[i].checked = checked;
                }
            }
        </script>
        <style>
            .RadGrid_Default .rgFilterBox {
                width: 56% !important;
            }

            .MyGridClass .rgDataDiv {
                height: auto !important;
            }

            .RadGrid_Default {
                background-color: #FFF !important;
            }

            .table-scroll {
                overflow-x: hidden !important;
            }

            .panel-body {
                padding: 0px !important;
            }
        </style>
    </telerik:RadScriptBlock>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadAjaxManager ID="ToDoListRadAjaxManager" EnablePageHeadUpdate="false" runat="server" OnAjaxRequest="ToDoListRadAjaxManager_AjaxRequest">
        <ClientEvents OnRequestStart="onRequestStart" />
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="btnExportMyRecord">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="gridActivityDetails"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnExportPendingRecord">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="gridPendingTimesheet"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnExportApprovedTimesheet">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="gridApprovedTimesheet"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnExportRejectedTimesheet">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="gridRejectedTimesheet"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="TimesheetRadWindowManager">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="tabToDoList"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="RadMultiPage1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="TimesheetRadWindow">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="tabToDoList"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="RadMultiPage1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="gridActivityDetails">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="gridActivityDetails"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="gridPendingTimesheet">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="gridPendingTimesheet"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="gridApprovedTimesheet"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="gridRejectedTimesheet"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnApprove">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="tabToDoList"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="RadMultiPage1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="radNotificationMessage"></telerik:AjaxUpdatedControl>

                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnReject">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="tabToDoList"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="RadMultiPage1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="radNotificationMessage"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="gridApprovedTimesheet">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="gridApprovedTimesheet"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="gridRejectedTimesheet">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="gridRejectedTimesheet"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="tabToDoList"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="tabToDoList">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="tabToDoList"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="RadMultiPage1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="btnExportMyRecord"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="btnExportPendingRecord"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="btnExportApprovedTimesheet"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="btnExportRejectedTimesheet"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="ToDoListRadAjaxManager">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="gridViewComments" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="gridTimesheetAttachment" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="gridTimesheetView" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="gridAssignedHistory" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="gridIssues" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="gridSurveys" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="gridInvSCBTable" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="gridInvSCBTableQM" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="gridInvSCBTablePM" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="gridBlocks" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="gridBlocksPM" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="gridBlocksQM" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="gridSurveysQM" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="gridSurveysPM" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="hBlockApprovedByQM" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="hBlockApprovedByPM" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="hSurveyQM" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="hSurveyPM" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="hTableQM" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="hTablePM" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="LoadingPanel1" runat="server">
    </telerik:RadAjaxLoadingPanel>
    <div class="col-xs-12 heading-big padding-b-10">
        <h5 class="margin-0 lineheight-30 breath-ctrl">Home / <span style="font-weight: normal !important">To Do Task</span></h5>
    </div>
    <telerik:RadWindowManager RenderMode="Lightweight" ID="TimesheetRadWindowManager" runat="server" EnableShadow="true">
        <Windows>
            <telerik:RadWindow ID="TimesheetRadWindow" RenderMode="Lightweight" runat="server" CssClass="window123" VisibleStatusbar="false" OnClientClose="RebindMyRecordsData"
                Width="98%" Height="550" AutoSize="false" Behaviors="Close" ShowContentDuringLoad="false"
                Modal="true" ReloadOnShow="true" />
        </Windows>
    </telerik:RadWindowManager>
    <telerik:RadNotification RenderMode="Lightweight" ID="radNotificationMessage" runat="server" VisibleOnPageLoad="true" Position="Center"
        Width="330" Height="80" Animation="Fade" EnableRoundedCorners="true" EnableShadow="true" Visible="false" AnimationDuration="500"
        ContentIcon="none" TitleIcon="none"
        Title="Success" Text="Task allocated/removed successfully."
        Style="z-index: 100000">
    </telerik:RadNotification>
    <div class="container-fluid padding-0">
        <div class="row margin-0 hide">
            <div class="col-xs-12 padding-0">
                <div class="col-sm-11 col-xs-12 padding-0">
                    <div class="col-sm-3 col-xs-12">
                        <div class="col-sm-4 col-xs-12 padding-0 margin-10">
                            <label class="col-lg-12 padding-0 control-label lable-txt" for="name">Site</label>
                        </div>
                        <!-- End of col-lg-1-->
                        <div class="col-sm-8 col-xs-12 padding-0 margin-10">
                            <telerik:RadDropDownList RenderMode="Lightweight" ID="ddlSapSite" runat="server" AutoPostBack="true"
                                DropDownWidth="200px" OnSelectedIndexChanged="ddlSapSite_SelectedIndexChanged">
                            </telerik:RadDropDownList>
                        </div>
                        <!-- End of input-group-col-xs-8-->
                    </div>
                    <div class="col-sm-3 col-xs-12 margin-10">
                        <div class="col-sm-4 col-xs-12 padding-0">
                            <label class="col-lg-12 padding-0 control-label lable-txt" for="name">Project</label>
                        </div>
                        <!-- End of col-lg-1-->
                        <div class=" col-sm-8 col-xs-12 padding-0 ">
                            <telerik:RadDropDownList RenderMode="Lightweight" ID="ddlSapProjects" CausesValidation="false" AutoPostBack="true"
                                runat="server" ValidationGroup="Search">
                            </telerik:RadDropDownList>
                        </div>
                        <!-- End of input-group-col-xs-8-->
                    </div>
                    <div class="col-sm-3 col-xs-12 margin-10">
                        <div class="col-sm-4 col-xs-12 padding-0 ">
                            <label class="col-xs-12 padding-0 control-label lable-txt" for="name">Area</label>
                        </div>
                        <!-- End of col-lg-1-->
                        <div class=" col-sm-8 col-xs-12 padding-0">
                            <telerik:RadDropDownList RenderMode="Lightweight" ID="ddlArea" AutoPostBack="true" CausesValidation="false"
                                DataTextField="Name" DataValueField="Id"
                                runat="server" OnSelectedIndexChanged="ddlArea_SelectedIndexChanged" ValidationGroup="Search">
                            </telerik:RadDropDownList>
                        </div>
                        <!-- End of input-group-col-xs-8-->
                    </div>
                    <div class="col-sm-3 col-xs-12 margin-10">
                        <div class="col-sm-4 col-xs-12 padding-0 ">
                            <label class="col-xs-12 padding-0 control-label lable-txt" for="name">Network</label>
                        </div>
                        <!-- End of col-lg-1-->
                        <div class=" col-sm-8 col-xs-12 padding-0">
                            <telerik:RadDropDownList RenderMode="Lightweight" ID="ddlNetwork" runat="server" DefaultMessage="<%# SolarPMS.Models.Constants.CONST_SELECT_TEXT %>"
                                DataTextField="Name" DataValueField="Id" ValidationGroup="Search">
                            </telerik:RadDropDownList>
                        </div>
                        <!-- End of input-group-col-xs-8-->
                    </div>
                </div>
                <!--End of Column 10-->
                <div class="col-sm-1 col-xs-12">
                    <div class="col-xs-12 margin-10 padding-0">
                        <div class="input-group-btn">
                            <button class="btn btn-primary pull-right hidden-xs" type="submit"><i class="glyphicon glyphicon-search"></i></button>
                        </div>
                    </div>
                </div>
            </div>
            <!--End of Column 12-->
        </div>

        <!-- End of row-->
        <div class="row margin-0">
            <div class="col-xs-12 padding-lr-10">
                <ul class="col-xs-1 pull-right nav" style="padding: 0px 0px 7px 5px; border-bottom: 1px solid rgb(201, 201, 201);">
                    <li>
                        <telerik:RadButton RenderMode="Lightweight" ID="btnExportMyRecord" CausesValidation="false" CssClass="pull-right btn button-style" runat="server" Text="Export" Style="padding: 6px 17px !important;" OnClick="btnExportMyRecord_Click">
                            <Icon PrimaryIconCssClass="rbDownload"></Icon>
                        </telerik:RadButton>
                        <telerik:RadButton RenderMode="Lightweight" ID="btnExportPendingRecord" CausesValidation="false" CssClass="pull-right" runat="server" Text="Export" Style="padding: 6px 21px !important;" OnClick="btnExportPendingRecord_Click" Visible="false">
                            <Icon PrimaryIconCssClass="rbDownload"></Icon>
                        </telerik:RadButton>
                        <telerik:RadButton RenderMode="Lightweight" ID="btnExportApprovedTimesheet" CausesValidation="false" CssClass="pull-right" runat="server" Text="Export" Style="padding: 6px 21px !important;" OnClick="btnExportApprovedTimesheet_Click" Visible="false">
                            <Icon PrimaryIconCssClass="rbDownload"></Icon>
                        </telerik:RadButton>
                        <telerik:RadButton RenderMode="Lightweight" ID="btnExportRejectedTimesheet" CausesValidation="false" CssClass="pull-right" runat="server" Text="Export" Style="padding: 6px 21px !important;" OnClick="btnExportRejectedTimesheet_Click" Visible="false">
                            <Icon PrimaryIconCssClass="rbDownload"></Icon>
                        </telerik:RadButton>
                    </li>
                </ul>
                <div>
                    <telerik:RadTabStrip RenderMode="Lightweight" runat="server" ID="tabToDoList" CausesValidation="false" MultiPageID="RadMultiPage1"
                        OnTabClick="tabToDoList_TabClick" SelectedIndex="0" Skin="Silk">
                        <Tabs>
                            <telerik:RadTab Text="My Records" Width="200px">
                                <TabTemplate>My Records&nbsp;&nbsp;<asp:Label ID="lblMyRecordCount" runat="server" Text="" CssClass="badge badge-info"></asp:Label></TabTemplate>
                            </telerik:RadTab>
                            <telerik:RadTab Text="Pending for Approval" Width="220px">
                                <TabTemplate>Pending for Approval&nbsp;&nbsp;<asp:Label ID="lblPendingRecordCount" runat="server" Text="" CssClass="badge badge-important"></asp:Label></TabTemplate>
                            </telerik:RadTab>
                            <telerik:RadTab Text="Approved Records" Width="200px">
                                <TabTemplate>Approved Records&nbsp;&nbsp;<asp:Label ID="lblApprovedRecordCount" runat="server" Text="" CssClass="badge badge-warning"></asp:Label></TabTemplate>
                            </telerik:RadTab>
                            <telerik:RadTab Text="Rejected Records" Width="200px">
                                <TabTemplate>Rejected Records&nbsp;&nbsp;<asp:Label ID="lblRejectedRecordCount" runat="server" Text="" CssClass="badge badge-purple"></asp:Label></TabTemplate>
                            </telerik:RadTab>
                            <telerik:RadTab Text="Invalid Offline Timesheet" Width="250px">
                                <TabTemplate>Invalid Offline Timesheet&nbsp;&nbsp;<asp:Label ID="lblOfflineRecordCount" runat="server" Text="" CssClass="badge badge-inverse"></asp:Label></TabTemplate>
                            </telerik:RadTab>
                        </Tabs>
                    </telerik:RadTabStrip>
                    <telerik:RadMultiPage runat="server" ID="RadMultiPage1" SelectedIndex="0">

                        <telerik:RadPageView runat="server" ID="RadPageView1">
                            <div class="col-xs-12 padding-0">
                                <div class="col-xs-12 padding-0 margin-10">
                                </div>
                                <div class="table-scroll">
                                    <div>
                                        <telerik:RadGrid RenderMode="Lightweight" ID="gridActivityDetails" runat="server" OnNeedDataSource="gridActivityDetails_NeedDataSource"
                                            OnItemDataBound="gridActivityDetails_ItemDataBound" OnItemCreated="gridActivityDetails_ItemCreated" OnDataBound="gridActivityDetails_DataBound"
                                            AutoGenerateColumns="false" MasterTableView-CommandItemSettings-AddNewRecordText="Add Location" ExportSettings-ExportOnlyData="true"
                                            AllowPaging="true" AllowSorting="true" AllowFilteringByColumn="true" PageSize="<%# Convert.ToInt32(SolarPMS.Models.Constants.CONST_GRID_PAGE_SIZE) %>">
                                            <GroupingSettings CaseSensitive="false" />
                                            <ClientSettings>
                                                <Scrolling AllowScroll="True" UseStaticHeaders="True" ScrollHeight="250px" SaveScrollPosition="true" FrozenColumnsCount="3"></Scrolling>
                                            </ClientSettings>

                                            <ExportSettings IgnorePaging="true" ExportOnlyData="true" HideStructureColumns="true"></ExportSettings>
                                            <MasterTableView CommandItemDisplay="None" DataKeyNames="ActivityId,TimeSheetId,StatusId,SubActivityId,ActivityDescription,SAPSubActivityDescription,MobileFunction,
                                                                           BlockTable,SAPProjectId,WBSAreaId,SAPNetwork,SAPActivity,SapSubActivity,SAPSite,ActivityPlanStartDate,
                                                                           ActivityPlanFinishDate,ActivityPlanQtyUoM,ActivityQty,ActualQty"
                                                EditMode="InPlace" EnableNoRecordsTemplate="true" TableLayout="Fixed">
                                                <NoRecordsTemplate>
                                                    <table width="100%" border="0" cellpadding="20" cellspacing="20">
                                                        <tr>
                                                            <td align="center">No records to display.
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </NoRecordsTemplate>
                                                <CommandItemSettings ShowRefreshButton="false" ShowExportToExcelButton="false" />
                                                <Columns>
                                                    <telerik:GridBoundColumn DataField="NetworkDescription" MaxLength="30" HeaderText="Network" ColumnEditorID="TextboxEditor"
                                                        HeaderStyle-Width="130px" HeaderStyle-Height="30px" DataType="System.String">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="ActivityDescription" MaxLength="30" HeaderText="Activity" ColumnEditorID="TextboxEditor"
                                                        HeaderStyle-Width="130px" DataType="System.String">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="SAPSubActivityDescription" MaxLength="30" HeaderText="Sub Activity" ColumnEditorID="TextboxEditor"
                                                        HeaderStyle-Width="130px" DataType="System.String">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="CRR" MaxLength="30" HeaderText="CRR" ColumnEditorID="TextboxEditor"
                                                        HeaderStyle-Width="100px" DataType="System.String" Visible="false">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="RRR" MaxLength="30" HeaderText="RRR" ColumnEditorID="TextboxEditor"
                                                        HeaderStyle-Width="100px" DataType="System.String" Visible="false">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="ActivityQty" MaxLength="30" HeaderText="Qty." ColumnEditorID="TextboxEditor"
                                                        HeaderStyle-Width="70px" DataType="System.String">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="ActivityPlanQtyUoM" MaxLength="30" HeaderText="UOM" ColumnEditorID="TextboxEditor"
                                                        HeaderStyle-Width="70px" DataType="System.String">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="ActivityPlanStartDate" MaxLength="30" HeaderText="Est. Start Date" ColumnEditorID="TextboxEditor"
                                                        FilterControlWidth="80px" HeaderStyle-Width="125px" DataType="System.DateTime" DataFormatString="{0:dd-MMM-yyyy}">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="ActivityPlanFinishDate" MaxLength="30" HeaderText="Est. End Date" ColumnEditorID="TextboxEditor" FilterControlWidth="80px"
                                                        HeaderStyle-Width="125px" DataType="System.DateTime" DataFormatString="{0:dd-MMM-yyyy}">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="EstDuration" MaxLength="30" HeaderText="Estimated Duration" ColumnEditorID="TextboxEditor"
                                                        HeaderStyle-Width="125px" DataType="System.Int32">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="Actualstart" MaxLength="30" HeaderText="Actual Start Date" ColumnEditorID="TextboxEditor" FilterControlWidth="80px"
                                                        HeaderStyle-Width="125px" DataType="System.DateTime" DataFormatString="{0:dd-MMM-yyyy}">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="ActualEnd" MaxLength="30" HeaderText="Actual End Date" ColumnEditorID="TextboxEditor" FilterControlWidth="80px"
                                                        HeaderStyle-Width="125px" DataType="System.DateTime" DataFormatString="{0:dd-MMM-yyyy}">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="ActualDuration" MaxLength="30" HeaderText="Actual Duration" ColumnEditorID="TextboxEditor"
                                                        HeaderStyle-Width="70px" DataType="System.String">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="ActualQty" MaxLength="30" HeaderText="Quantity Completed" ColumnEditorID="TextboxEditor"
                                                        HeaderStyle-Width="100px" DataType="System.String">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="QuantityUnderApproval" MaxLength="30" HeaderText="Quantity under approval" ColumnEditorID="TextboxEditor"
                                                        HeaderStyle-Width="100px" DataType="System.String">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="QuantityPending" MaxLength="30" HeaderText="Quantity pending" ColumnEditorID="TextboxEditor"
                                                        HeaderStyle-Width="100px" DataType="System.String">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="Status" MaxLength="30" HeaderText="Status" ColumnEditorID="TextboxEditor"
                                                        HeaderStyle-Width="70px" DataType="System.String">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridTemplateColumn UniqueName="AddTimesheet" HeaderStyle-Width="90px" ShowFilterIcon="false" AllowFiltering="false" Exportable="false">
                                                        <HeaderTemplate>
                                                            <label>Add Timesheet</label>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Button runat="server" ID="btnAddTimesheet" OnClick="btnAddTimesheet_Click" Text="Add" CssClass="btn teal-btn btn-xs center-block" CausesValidation="false" />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="ViewAttachment" HeaderStyle-Width="90px" ShowFilterIcon="false" AllowFiltering="false" Exportable="false">
                                                        <HeaderTemplate>
                                                            <label>Attachment</label>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Button runat="server" ID="btnViewAttachment" Text="View" CssClass="btn btn-rb btn-xs center-block" CausesValidation="false" />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="RaisesIssues" HeaderStyle-Width="70px" ShowFilterIcon="false" AllowFiltering="false" Exportable="false">
                                                        <HeaderTemplate>
                                                            <label>Raise Issue</label>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Button runat="server" ID="btnRaiseIssues" OnClick="btnRaiseIssues_Click" Text="Add" CssClass="btn blue-btn btn-xs center-block" CausesValidation="false" />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="AssignedHistory" HeaderStyle-Width="70px" ShowFilterIcon="false" AllowFiltering="false" Exportable="false">
                                                        <HeaderTemplate>
                                                            <label>Assigned History</label>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Button runat="server" ID="btnAssignedHistory" Text="View" CssClass="btn orange-btn btn-xs center-block" CausesValidation="false" />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="ViewTimesheet" HeaderStyle-Width="80px" ShowFilterIcon="false" AllowFiltering="false" Exportable="false">
                                                        <HeaderTemplate>
                                                            <label>Timesheet</label>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Button runat="server" ID="btnViewTimesheets" Text="View" CssClass="btn purple-btn btn-xs center-block" CausesValidation="false" />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="ViewIssues" HeaderStyle-Width="70px" ShowFilterIcon="false" AllowFiltering="false" Exportable="false">
                                                        <HeaderTemplate>
                                                            <label>Issue</label>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Button runat="server" ID="btnViewIssues" Text="View" CssClass="btn green-btn btn-xs center-block" CausesValidation="false" />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>

                                                    <telerik:GridBoundColumn DataField="SAPSite" MaxLength="30" HeaderText="Site" ColumnEditorID="TextboxEditor"
                                                        HeaderStyle-Width="170px" DataType="System.String">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="ProjectDescription" MaxLength="30" HeaderText="Project" ColumnEditorID="TextboxEditor"
                                                        HeaderStyle-Width="170px" DataType="System.String">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn HeaderStyle-Width="170px" DataField="WBSArea" MaxLength="30" HeaderText="WBS Area" ColumnEditorID="TextboxEditor"
                                                        DataType="System.String">
                                                    </telerik:GridBoundColumn>

                                                </Columns>
                                                <PagerStyle PageSizes="5,10,15" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                                                    PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />
                                            </MasterTableView>
                                        </telerik:RadGrid>
                                    </div>
                                </div>
                            </div>
                        </telerik:RadPageView>
                        <telerik:RadPageView runat="server" ID="RadPageView2">
                            <div class="col-lg-12 padding-0 margin-10">
                            </div>
                            <div class="table-scroll">
                                <telerik:RadGrid RenderMode="Lightweight" ID="gridPendingTimesheet" runat="server" OnNeedDataSource="gridPendingTimesheet_NeedDataSource"
                                    AutoGenerateColumns="false" MasterTableView-CommandItemSettings-AddNewRecordText="Add Location" ExportSettings-ExportOnlyData="true"
                                    AllowPaging="true" AllowSorting="true" AllowFilteringByColumn="true" OnItemCreated="gridPendingTimesheet_ItemCreated" OnItemDataBound="gridPendingTimesheet_ItemDataBound"
                                    PageSize="<%# Convert.ToInt32(SolarPMS.Models.Constants.CONST_GRID_PAGE_SIZE) %>">
                                    <GroupingSettings CaseSensitive="false" />
                                    <ClientSettings>
                                        <Scrolling AllowScroll="True" UseStaticHeaders="True" ScrollHeight="250px" SaveScrollPosition="true" FrozenColumnsCount="4"></Scrolling>
                                    </ClientSettings>
                                    <ExportSettings IgnorePaging="true" FileName="LocationMaster"></ExportSettings>
                                    <MasterTableView DataKeyNames="ActivityId,SubActivityId,TimeSheetId,SAPActivity,SAPNetwork" EditMode="InPlace" CommandItemDisplay="None" EnableNoRecordsTemplate="true" TableLayout="Fixed">
                                        <NoRecordsTemplate>
                                            <table width="100%" border="0" cellpadding="20" cellspacing="20">
                                                <tr>
                                                    <td align="center">No records to display.
                                                    </td>
                                                </tr>
                                            </table>
                                        </NoRecordsTemplate>
                                        <CommandItemSettings ShowRefreshButton="false" ShowExportToExcelButton="false" />
                                        <Columns>
                                            <telerik:GridTemplateColumn UniqueName="CheckboxColumn" DataField="IsSelected" HeaderStyle-Width="50px" ItemStyle-Width="50px" AllowFiltering="false" ShowSortIcon="false" AllowSorting="false">
                                                <HeaderTemplate>
                                                    <asp:CheckBox ID="chkAll" runat="server" onclick="checkAllRows(this);" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkSelect" onclick="checkAllChildGridCheckbox(this);" runat="server" />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridBoundColumn DataField="NetworkDescription" MaxLength="30" HeaderText="Network" ColumnEditorID="TextboxEditor"
                                                HeaderStyle-Width="120px" DataType="System.String">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="ActivityDescription" MaxLength="30" HeaderText="Activity" ColumnEditorID="TextboxEditor"
                                                HeaderStyle-Width="120px" DataType="System.String">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="SAPSubActivityDescription" MaxLength="30" HeaderText="Sub Activity" ColumnEditorID="TextboxEditor"
                                                HeaderStyle-Width="120px" DataType="System.String">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="ActivityQty" MaxLength="30" HeaderText="Qty." ColumnEditorID="TextboxEditor"
                                                HeaderStyle-Width="100px" DataType="System.String">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="ActivityPlanQtyUoM" MaxLength="30" HeaderText="UOM" ColumnEditorID="TextboxEditor"
                                                HeaderStyle-Width="100px" DataType="System.String">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="ActivityPlanStartDate" MaxLength="30" HeaderText="Est. Start Date" ColumnEditorID="TextboxEditor"
                                                FilterControlWidth="80px" HeaderStyle-Width="125px" DataType="System.DateTime" DataFormatString="{0:dd-MMM-yyyy}">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="ActivityPlanFinishDate" MaxLength="30" HeaderText="Est. End Date" ColumnEditorID="TextboxEditor"
                                                FilterControlWidth="80px" HeaderStyle-Width="125px" DataType="System.DateTime" DataFormatString="{0:dd-MMM-yyyy}">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="ActualDate" MaxLength="30" HeaderText="Actual Date" ColumnEditorID="TextboxEditor"
                                                FilterControlWidth="80px" HeaderStyle-Width="125px" DataType="System.DateTime" DataFormatString="{0:dd-MMM-yyyy}">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="ActualQuantity" MaxLength="30" HeaderText="Actual Qty" ColumnEditorID="TextboxEditor"
                                                HeaderStyle-Width="100px" DataType="System.Decimal">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="WorkflowStatus" MaxLength="30" HeaderText="Status" ColumnEditorID="TextboxEditor"
                                                HeaderStyle-Width="100px" DataType="System.String">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="UserName" MaxLength="30" HeaderText="User" ColumnEditorID="TextboxEditor"
                                                HeaderStyle-Width="100px" DataType="System.String">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridTemplateColumn UniqueName="CommentColumn" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="175px" ItemStyle-Width="50px" ShowFilterIcon="false" AllowFiltering="false" Exportable="false">
                                                <HeaderTemplate>
                                                    <label>Comments</label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <telerik:RadTextBox ID="txtTimesheetComments" runat="server"></telerik:RadTextBox>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn UniqueName="ViewTimesheetPartialApprove" HeaderStyle-Width="115px" ShowFilterIcon="false" AllowFiltering="false" Exportable="false">
                                                <HeaderTemplate>
                                                    <label>Timesheet</label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Button runat="server" ID="btnPartialApprove" Text="Partial Approve" OnClick="btnPartialApprove_Click" CssClass="btn purple-btn btn-xs center-block" CausesValidation="false" />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderStyle-Width="80px" ItemStyle-Width="80px" ShowFilterIcon="false" AllowFiltering="false" Exportable="false">
                                                <HeaderTemplate>
                                                    <label>Survey</label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Button runat="server" ID="btnViewSurvey" Text="View" CssClass="btn blue-btn btn-xs center-block" CausesValidation="false" />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderStyle-Width="80px" ItemStyle-Width="80px" ShowFilterIcon="false" AllowFiltering="false" Exportable="false">
                                                <HeaderTemplate>
                                                    <label>Block</label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Button runat="server" ID="btnViewBlock" Text="View" CssClass="btn orange-btn btn-xs center-block" CausesValidation="false" />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderStyle-Width="80px" ItemStyle-Width="80px" ShowFilterIcon="false" AllowFiltering="false" Exportable="false">
                                                <HeaderTemplate>
                                                    <label>Inverter / SCB / Table / Number / Location</label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Button runat="server" ID="btnViewInvSCBTable" Text="View" CssClass="btn purple-btn btn-xs center-block" CausesValidation="false" />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderStyle-Width="90px" ItemStyle-Width="90px" ShowFilterIcon="false" AllowFiltering="false" Exportable="false">
                                                <HeaderTemplate>
                                                    <label>Attachment</label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Button runat="server" ID="btnViewAttachment" Text="View" CssClass="btn green-btn btn-xs center-block" CausesValidation="false" />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderStyle-Width="90px" ItemStyle-Width="90px" ShowFilterIcon="false" AllowFiltering="false" Exportable="false">
                                                <HeaderTemplate>
                                                    <label>Comments</label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Button runat="server" ID="btnViewComments" Text="View" CssClass="btn btn-rb btn-xs center-block" CausesValidation="false" />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>

                                            <telerik:GridBoundColumn DataField="SAPSite" MaxLength="30" HeaderText="Site" ColumnEditorID="TextboxEditor"
                                                HeaderStyle-Width="100px" DataType="System.String">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="ProjectDescription" MaxLength="30" HeaderText="Project" ColumnEditorID="TextboxEditor"
                                                HeaderStyle-Width="100px" DataType="System.String">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="WBSArea" MaxLength="30" HeaderText="WBS Area" ColumnEditorID="TextboxEditor"
                                                HeaderStyle-Width="100px" DataType="System.String">
                                            </telerik:GridBoundColumn>

                                        </Columns>
                                        <PagerStyle PageSizes="5,10,15" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                                            PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />
                                    </MasterTableView>
                                </telerik:RadGrid>
                                <div class="col-lg-12 padding-0 margin-10" id="divActionButtons" runat="server">
                                    <center>
                                            <asp:Button runat="server" ID="btnApprove" CausesValidation="false" Text="Approve" CssClass="btn btn-success button" OnClick="btnApprove_Click" />
                                            <asp:Button runat="server" ID="btnReject" Text="Reject" CausesValidation="false" CssClass="btn btn-danger button" OnClick="btnReject_Click"/>
                                        </center>
                                </div>
                            </div>
                        </telerik:RadPageView>
                        <telerik:RadPageView runat="server" ID="RadPageView3">
                            <div class="col-lg-12 padding-0 margin-10">
                            </div>
                            <div class="table-scroll">
                                <telerik:RadGrid RenderMode="Lightweight" ID="gridApprovedTimesheet" runat="server" OnNeedDataSource="gridApprovedTimesheet_NeedDataSource"
                                    AutoGenerateColumns="false" MasterTableView-CommandItemSettings-AddNewRecordText="Add Location" ExportSettings-ExportOnlyData="true"
                                    AllowPaging="true" AllowSorting="true" AllowFilteringByColumn="true" OnItemCreated="gridApprovedTimesheet_ItemCreated"
                                    PageSize="<%# Convert.ToInt32(SolarPMS.Models.Constants.CONST_GRID_PAGE_SIZE) %>">
                                    <GroupingSettings CaseSensitive="false" />
                                    <ClientSettings>
                                        <Scrolling AllowScroll="True" UseStaticHeaders="True" ScrollHeight="250px" SaveScrollPosition="true" FrozenColumnsCount="3"></Scrolling>
                                    </ClientSettings>
                                    <ExportSettings IgnorePaging="true" FileName="LocationMaster"></ExportSettings>
                                    <MasterTableView DataKeyNames="ActivityId,SubActivityId,TimeSheetId" EditMode="InPlace" CommandItemDisplay="None" EnableNoRecordsTemplate="true" TableLayout="Fixed">
                                        <NoRecordsTemplate>
                                            <table width="100%" border="0" cellpadding="20" cellspacing="20">
                                                <tr>
                                                    <td align="center">No records to display.
                                                    </td>
                                                </tr>
                                            </table>
                                        </NoRecordsTemplate>
                                        <CommandItemSettings ShowRefreshButton="false" ShowExportToExcelButton="false" />
                                        <Columns>

                                            <telerik:GridBoundColumn DataField="NetworkDescription" MaxLength="30" HeaderText="Network" ColumnEditorID="TextboxEditor"
                                                HeaderStyle-Width="120px" DataType="System.String">
                                            </telerik:GridBoundColumn>

                                            <telerik:GridBoundColumn DataField="ActivityDescription" MaxLength="30" HeaderText="Activity" ColumnEditorID="TextboxEditor"
                                                HeaderStyle-Width="120px" DataType="System.String">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="SAPSubActivityDescription" MaxLength="30" HeaderText="Sub Activity" ColumnEditorID="TextboxEditor"
                                                HeaderStyle-Width="120px" DataType="System.String">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="ActivityQty" MaxLength="30" HeaderText="Qty." ColumnEditorID="TextboxEditor"
                                                HeaderStyle-Width="100px" DataType="System.String">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="ActivityPlanQtyUoM" MaxLength="30" HeaderText="UOM" ColumnEditorID="TextboxEditor"
                                                HeaderStyle-Width="100px" DataType="System.String">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="ActivityPlanStartDate" MaxLength="30" HeaderText="Est. Start Date" ColumnEditorID="TextboxEditor"
                                                FilterControlWidth="80px" HeaderStyle-Width="125px" DataType="System.String" DataFormatString="{0:dd-MMM-yyyy}">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="ActivityPlanFinishDate" MaxLength="30" HeaderText="Est. End Date" ColumnEditorID="TextboxEditor"
                                                FilterControlWidth="80px" HeaderStyle-Width="125px" DataType="System.DateTime" DataFormatString="{0:dd-MMM-yyyy}">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="ActualDate" MaxLength="30" HeaderText="Actual Date" ColumnEditorID="TextboxEditor"
                                                FilterControlWidth="80px" HeaderStyle-Width="125px" DataType="System.DateTime" DataFormatString="{0:dd-MMM-yyyy}">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="ActualQuantity" MaxLength="30" HeaderText="Actual Qty" ColumnEditorID="TextboxEditor"
                                                HeaderStyle-Width="100px" DataType="System.String">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="ApprovedDate" MaxLength="30" HeaderText="Approved Date" ColumnEditorID="TextboxEditor"
                                                FilterControlWidth="80px" HeaderStyle-Width="125px" DataType="System.DateTime" DataFormatString="{0:dd-MMM-yyyy}">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="WorkflowStatus" MaxLength="30" HeaderText="Status" ColumnEditorID="TextboxEditor"
                                                HeaderStyle-Width="100px" DataType="System.String">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="UserName" MaxLength="30" HeaderText="User" ColumnEditorID="TextboxEditor"
                                                HeaderStyle-Width="100px" DataType="System.String">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridTemplateColumn HeaderStyle-Width="100px" ItemStyle-Width="100px" ShowFilterIcon="false" AllowFiltering="false" Exportable="false">
                                                <HeaderTemplate>
                                                    <label>Survey</label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Button runat="server" ID="btnViewSurvey" Text="View" CssClass="btn btn-rb btn-xs center-block" CausesValidation="false" />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderStyle-Width="80px" ItemStyle-Width="80px" ShowFilterIcon="false" AllowFiltering="false" Exportable="false">
                                                <HeaderTemplate>
                                                    <label>Block</label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Button runat="server" ID="btnViewBlock" Text="View" CssClass="btn blue-btn btn-xs center-block" CausesValidation="false" />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderStyle-Width="80px" ItemStyle-Width="80px" ShowFilterIcon="false" AllowFiltering="false" Exportable="false">
                                                <HeaderTemplate>
                                                    <label>Inverter / SCB / Table / Number / Location</label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Button runat="server" ID="btnViewInvSCBTable" Text="View" CssClass="btn orange-btn btn-xs center-block" CausesValidation="false" />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderStyle-Width="100px" ItemStyle-Width="100px" ShowFilterIcon="false" AllowFiltering="false" Exportable="false">
                                                <HeaderTemplate>
                                                    <label>Attachment</label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Button runat="server" ID="btnViewAttachment" Text="View" CssClass="btn purple-btn btn-xs center-block" CausesValidation="false" />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderStyle-Width="90px" ItemStyle-Width="90px" ShowFilterIcon="false" AllowFiltering="false" Exportable="false">
                                                <HeaderTemplate>
                                                    <label>Comments</label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Button runat="server" ID="btnViewComments" Text="View" CssClass="btn green-btn btn-xs center-block" CausesValidation="false" />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>

                                            <telerik:GridBoundColumn DataField="SAPSite" MaxLength="30" HeaderText="Site" ColumnEditorID="TextboxEditor"
                                                HeaderStyle-Width="100px" DataType="System.String">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="ProjectDescription" MaxLength="30" HeaderText="Project" ColumnEditorID="TextboxEditor"
                                                HeaderStyle-Width="100px" DataType="System.String">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="WBSArea" MaxLength="30" HeaderText="WBS Area" ColumnEditorID="TextboxEditor"
                                                HeaderStyle-Width="100px" DataType="System.String">
                                            </telerik:GridBoundColumn>
                                        </Columns>
                                        <PagerStyle PageSizes="5,10,15" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                                            PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />
                                    </MasterTableView>
                                </telerik:RadGrid>
                            </div>
                        </telerik:RadPageView>
                        <telerik:RadPageView runat="server" ID="RadPageView4">
                            <div class="col-lg-12 padding-0 margin-10">
                            </div>
                            <div class="table-scroll">
                                <telerik:RadGrid RenderMode="Lightweight" ID="gridRejectedTimesheet" runat="server" OnNeedDataSource="gridRejectedTimesheet_NeedDataSource"
                                    AutoGenerateColumns="false" MasterTableView-CommandItemSettings-AddNewRecordText="Add Location" ExportSettings-ExportOnlyData="true"
                                    AllowPaging="true" AllowSorting="true" AllowFilteringByColumn="true" OnItemCreated="gridRejectedTimesheet_ItemCreated"
                                    PageSize="<%# Convert.ToInt32(SolarPMS.Models.Constants.CONST_GRID_PAGE_SIZE) %>">
                                    <GroupingSettings CaseSensitive="false" />
                                    <ClientSettings>
                                        <Scrolling AllowScroll="True" UseStaticHeaders="True" ScrollHeight="250px" SaveScrollPosition="true" FrozenColumnsCount="3"></Scrolling>
                                    </ClientSettings>
                                    <ExportSettings IgnorePaging="true" FileName="LocationMaster"></ExportSettings>
                                    <MasterTableView DataKeyNames="ActivityId,SubActivityId,TimeSheetId" EditMode="InPlace" CommandItemDisplay="None" EnableNoRecordsTemplate="true">
                                        <NoRecordsTemplate>
                                            <table width="100%" border="0" cellpadding="20" cellspacing="20">
                                                <tr>
                                                    <td align="center">No records to display.
                                                    </td>
                                                </tr>
                                            </table>
                                        </NoRecordsTemplate>
                                        <CommandItemSettings ShowRefreshButton="false" ShowExportToExcelButton="false" />
                                        <Columns>
                                            <telerik:GridBoundColumn DataField="NetworkDescription" MaxLength="30" HeaderText="Network" ColumnEditorID="TextboxEditor"
                                                HeaderStyle-Width="100px" DataType="System.String">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="ActivityDescription" MaxLength="30" HeaderText="Activity" ColumnEditorID="TextboxEditor"
                                                HeaderStyle-Width="100px" DataType="System.String">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="SAPSubActivityDescription" MaxLength="30" HeaderText="Sub Activity" ColumnEditorID="TextboxEditor"
                                                HeaderStyle-Width="100px" DataType="System.String">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="ActivityQty" MaxLength="30" HeaderText="Qty." ColumnEditorID="TextboxEditor"
                                                HeaderStyle-Width="100px" DataType="System.String">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="ActivityPlanQtyUoM" MaxLength="30" HeaderText="UOM" ColumnEditorID="TextboxEditor"
                                                HeaderStyle-Width="100px" DataType="System.String">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="ActivityPlanStartDate" MaxLength="30" HeaderText="Est. Start Date" ColumnEditorID="TextboxEditor"
                                                FilterControlWidth="80px" HeaderStyle-Width="125px" DataType="System.DateTime" DataFormatString="{0:dd-MMM-yyyy}">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="ActivityPlanFinishDate" MaxLength="30" HeaderText="Est. End Date" ColumnEditorID="TextboxEditor"
                                                FilterControlWidth="80px" HeaderStyle-Width="125px" DataType="System.DateTime" DataFormatString="{0:dd-MMM-yyyy}">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="ActualDate" MaxLength="30" HeaderText="Actual Date" ColumnEditorID="TextboxEditor"
                                                FilterControlWidth="80px" HeaderStyle-Width="125px" DataType="System.DateTime" DataFormatString="{0:dd-MMM-yyyy}">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="ActualQuantity" MaxLength="30" HeaderText="Actual Qty" ColumnEditorID="TextboxEditor"
                                                HeaderStyle-Width="100px" DataType="System.String">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="RejectedDate" MaxLength="30" HeaderText="Rejected Date" ColumnEditorID="TextboxEditor"
                                                FilterControlWidth="80px" HeaderStyle-Width="125px" DataType="System.DateTime" DataFormatString="{0:dd-MMM-yyyy}">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="WorkflowStatus" MaxLength="30" HeaderText="Status" ColumnEditorID="TextboxEditor"
                                                HeaderStyle-Width="100px" DataType="System.String">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="UserName" MaxLength="30" HeaderText="User" ColumnEditorID="TextboxEditor"
                                                HeaderStyle-Width="100px" DataType="System.String">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridTemplateColumn HeaderStyle-Width="80px" ItemStyle-Width="80px" ShowFilterIcon="false" AllowFiltering="false" Exportable="false">
                                                <HeaderTemplate>
                                                    <label>Survey</label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Button runat="server" ID="btnViewSurvey" Text="View" CssClass="btn btn-rb btn-xs center-block" CausesValidation="false" />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderStyle-Width="80px" ItemStyle-Width="80px" ShowFilterIcon="false" AllowFiltering="false" Exportable="false">
                                                <HeaderTemplate>
                                                    <label>Block</label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Button runat="server" ID="btnViewBlock" Text="View" CssClass="btn btn-primary btn-xs center-block" CausesValidation="false" />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderStyle-Width="80px" ItemStyle-Width="80px" ShowFilterIcon="false" AllowFiltering="false" Exportable="false">
                                                <HeaderTemplate>
                                                    <label>Inverter / SCB / Table / Number / Location</label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Button runat="server" ID="btnViewInvSCBTable" Text="View" CssClass="btn blue-btn btn-xs center-block" CausesValidation="false" />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderStyle-Width="100px" ItemStyle-Width="100px" ShowFilterIcon="false" AllowFiltering="false" Exportable="false">
                                                <HeaderTemplate>
                                                    <label>Attachment</label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Button runat="server" ID="btnViewAttachment" Text="View" CssClass="btn orange-btn btn-xs center-block" CausesValidation="false" />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderStyle-Width="90px" ItemStyle-Width="90px" ShowFilterIcon="false" AllowFiltering="false" Exportable="false">
                                                <HeaderTemplate>
                                                    <label>Comments</label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Button runat="server" ID="btnViewComments" Text="View" CssClass="btn green-btn btn-xs center-block" CausesValidation="false" />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>

                                            <telerik:GridBoundColumn DataField="SAPSite" MaxLength="30" HeaderText="Site" ColumnEditorID="TextboxEditor"
                                                HeaderStyle-Width="100px" DataType="System.String">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="ProjectDescription" MaxLength="30" HeaderText="Project" ColumnEditorID="TextboxEditor"
                                                HeaderStyle-Width="100px" DataType="System.String">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="WBSArea" MaxLength="30" HeaderText="WBS Area" ColumnEditorID="TextboxEditor"
                                                HeaderStyle-Width="100px" DataType="System.String">
                                            </telerik:GridBoundColumn>

                                        </Columns>
                                        <PagerStyle PageSizes="5,10,15" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                                            PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />
                                    </MasterTableView>
                                </telerik:RadGrid>
                            </div>
                        </telerik:RadPageView>
                        <telerik:RadPageView runat="server" ID="RadPageView5">
                            <div class="col-lg-12 padding-0 margin-10">
                            </div>
                            <div class="table-scroll">
                                <telerik:RadGrid RenderMode="Lightweight" ID="gridOfflineTimesheet" runat="server" OnNeedDataSource="gridOfflineTimesheet_NeedDataSource"
                                    AutoGenerateColumns="false" ExportSettings-ExportOnlyData="true"
                                    AllowPaging="true" AllowSorting="true" AllowFilteringByColumn="true"
                                    PageSize="<%# Convert.ToInt32(SolarPMS.Models.Constants.CONST_GRID_PAGE_SIZE) %>">
                                    <GroupingSettings CaseSensitive="false" />
                                    <ClientSettings>
                                        <Scrolling AllowScroll="True" UseStaticHeaders="True" ScrollHeight="250px" SaveScrollPosition="true" FrozenColumnsCount="3"></Scrolling>
                                    </ClientSettings>
                                    <ExportSettings IgnorePaging="true" FileName="OfflineTimesheetEntry"></ExportSettings>
                                    <MasterTableView EditMode="InPlace" CommandItemDisplay="None" EnableNoRecordsTemplate="true">
                                        <NoRecordsTemplate>
                                            <table width="100%" border="0" cellpadding="20" cellspacing="20">
                                                <tr>
                                                    <td align="center">No records to display.
                                                    </td>
                                                </tr>
                                            </table>
                                        </NoRecordsTemplate>
                                        <CommandItemSettings ShowRefreshButton="false" ShowExportToExcelButton="false" />
                                        <Columns>
                                            <telerik:GridBoundColumn DataField="SAPSite" MaxLength="30" HeaderText="Site" ColumnEditorID="TextboxEditor"
                                                HeaderStyle-Width="170px" DataType="System.String">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="ProjectDescription" MaxLength="30" HeaderText="Project" ColumnEditorID="TextboxEditor"
                                                HeaderStyle-Width="170px" DataType="System.String">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn HeaderStyle-Width="100px" DataField="WBSArea" MaxLength="30" HeaderText="WBS Area" ColumnEditorID="TextboxEditor"
                                                DataType="System.String">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="NetworkDescription" MaxLength="30" HeaderText="Network" ColumnEditorID="TextboxEditor"
                                                HeaderStyle-Width="170px" DataType="System.String">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="ActivityDescription" MaxLength="30" HeaderText="Activity" ColumnEditorID="TextboxEditor"
                                                HeaderStyle-Width="100px" DataType="System.String">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="SAPSubActivityDescription" MaxLength="30" HeaderText="Sub Activity" ColumnEditorID="TextboxEditor"
                                                HeaderStyle-Width="100px" DataType="System.String">
                                            </telerik:GridBoundColumn>

                                            <telerik:GridBoundColumn DataField="ActualDate" MaxLength="30" HeaderText="Actual Date" ColumnEditorID="TextboxEditor"
                                                FilterControlWidth="80px" HeaderStyle-Width="125px" DataType="System.DateTime" DataFormatString="{0:dd-MMM-yyyy}">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="ActualQuantity" MaxLength="30" HeaderText="Actual Qty" ColumnEditorID="TextboxEditor"
                                                HeaderStyle-Width="100px" DataType="System.String">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Comments" MaxLength="30" HeaderText="Comments" ColumnEditorID="TextboxEditor"
                                                HeaderStyle-Width="100px" DataType="System.String">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Reason" MaxLength="30" HeaderText="Reason" ColumnEditorID="TextboxEditor"
                                                HeaderStyle-Width="200px" DataType="System.String">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Name" MaxLength="30" HeaderText="User" ColumnEditorID="TextboxEditor"
                                                HeaderStyle-Width="100px" DataType="System.String">
                                            </telerik:GridBoundColumn>
                                        </Columns>
                                        <PagerStyle PageSizes="5,10,15" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                                            PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />
                                    </MasterTableView>
                                </telerik:RadGrid>
                            </div>
                        </telerik:RadPageView>
                    </telerik:RadMultiPage>
                </div>
                <div class="panel with-nav-tabs panel-default border-0">
                    <div class="panel-body">
                        <div class="tab-content">
                            <div class="tab-pane active" id="tab1primary">

                                <!-- End of survey table wrapper-->
                            </div>
                            <div class="tab-pane fade" id="tab2primary">
                            </div>
                            <div class="tab-pane fade" id="tab3primary">
                            </div>
                            <div class="tab-pane fade" id="tab4primary">

                                <!--End of Scroll-->
                            </div>

                            <!--  Modal for deleted Task- view History Start Here-->
                            <div class="modal fade" id="divViewHistory" tabindex="-1" role="dialog" aria-labelledby="modalLabel" aria-hidden="true">
                                <div class="modal-dialog1">
                                    <div class="modal-content">
                                        <div class="modal-header modal-header-resize">
                                            <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">×</span><span class="sr-only">Close</span></button>
                                            <h4 class="modal-title text-primary" id="lineModalLabel">Assigned History</h4>
                                        </div>
                                        <div class="modal-body">
                                            <telerik:RadGrid RenderMode="Lightweight" ID="gridAssignedHistory" runat="server"
                                                AutoGenerateColumns="false" AllowPaging="true" AllowSorting="true" AllowFilteringByColumn="false" PageSize="<%# Convert.ToInt32(SolarPMS.Models.Constants.CONST_GRID_PAGE_SIZE) %>">
                                                <GroupingSettings CaseSensitive="false" />
                                                <MasterTableView CommandItemSettings-ShowRefreshButton="false" ClientDataKeyNames="" EditMode="InPlace"
                                                    CommandItemDisplay="None" AllowFilteringByColumn="false">
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
                                                        <telerik:GridBoundColumn DataField="Date" HeaderText="Actual Date" DataFormatString="{0:dd-MMM-yyyy}">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="Name" HeaderText="User Name">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridTemplateColumn HeaderText="Allocated" DataField="IsAssigned" UniqueName="IsAssigned">
                                                            <ItemTemplate>
                                                                <%# Eval("IsAssigned").ToString() %>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                    </Columns>
                                                    <PagerStyle PageSizes="5,10,15" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                                                        PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />
                                                </MasterTableView>
                                            </telerik:RadGrid>
                                        </div>
                                        <!--End of Scroll-->
                                    </div>
                                </div>
                            </div>
                            <!-- Row 1 Modal for deleted Task- view History End Here-->
                            <!--  Modal for deleted Task- view Timesheet Start Here-->

                            <div class="modal fade" id="divViewTimesheet" tabindex="-1" role="dialog" aria-labelledby="modalLabel" aria-hidden="true">
                                <div class="modal-dialog1">
                                    <div class="modal-content">
                                        <div class="modal-header modal-header-resize">
                                            <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">×</span><span class="sr-only">Close</span></button>
                                            <h4 class="modal-title text-primary" id="lineModalLabel">View Timesheet</h4>
                                        </div>
                                        <div class="modal-body">
                                            <telerik:RadGrid RenderMode="Lightweight" ID="gridTimesheetView" runat="server" OnNeedDataSource="gridTimesheetView_NeedDataSource"
                                                AutoGenerateColumns="false" AllowPaging="true" AllowSorting="true" AllowFilteringByColumn="false" OnDataBound="gridTimesheetView_DataBound"
                                                PageSize="<%# Convert.ToInt32(SolarPMS.Models.Constants.CONST_GRID_PAGE_SIZE) %>">
                                                <GroupingSettings CaseSensitive="false" />
                                                <MasterTableView CommandItemSettings-ShowRefreshButton="false" ClientDataKeyNames="" EditMode="InPlace"
                                                    DataKeyNames="TimeSheetId,ActivityId,SubActivityId,AllowEdit,UserId" CommandItemDisplay="None" AllowFilteringByColumn="false">
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
                                                        <telerik:GridBoundColumn DataField="ActualDate" HeaderText="Actual Date" DataFormatString="{0:dd-MMM-yyyy}">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="ActualQuantity" HeaderText="Quantity">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="Name" HeaderText="User">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="Status" HeaderText="Status">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridTemplateColumn UniqueName="EditTimesheet" HeaderStyle-Width="50px" ItemStyle-Width="50px" ShowFilterIcon="false" AllowFiltering="false" Exportable="false">
                                                            <HeaderTemplate>
                                                                <label>View</label>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Button runat="server" ID="btnEditTimesheet" Text="Edit" OnClick="btnEditTimesheet_Click" CssClass="btn btn-rb btn-xs center-block" CausesValidation="false" />
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                    </Columns>
                                                    <PagerStyle PageSizes="5,10,15" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                                                        PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />
                                                </MasterTableView>
                                            </telerik:RadGrid>
                                        </div>

                                    </div>
                                </div>
                            </div>

                            <!-- Row  Modal for deleted Task- view Timesheet End Here-->
                            <!--  Modal for deleted Task- view Issue Start Here-->

                            <div class="modal fade" id="divViewIssues" tabindex="-1" role="dialog" aria-labelledby="modalLabel" aria-hidden="true">
                                <div class="modal-dialog1">
                                    <div class="modal-content">
                                        <div class="modal-header modal-header-resize">
                                            <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">×</span><span class="sr-only">Close</span></button>
                                            <h4 class="modal-title text-primary" id="lineModalLabel">View Issue</h4>
                                        </div>
                                        <div class="modal-body">
                                            <telerik:RadGrid RenderMode="Lightweight" ID="gridIssues" runat="server"
                                                AutoGenerateColumns="false" AllowPaging="true" AllowSorting="true"
                                                AllowFilteringByColumn="false" OnDataBound="gridIssues_DataBound" OnNeedDataSource="gridIssues_NeedDataSource"
                                                PageSize="<%# Convert.ToInt32(SolarPMS.Models.Constants.CONST_GRID_PAGE_SIZE) %>">
                                                <GroupingSettings CaseSensitive="false" />
                                                <MasterTableView CommandItemSettings-ShowRefreshButton="false" EditMode="InPlace" DataKeyNames="ActivityId,SubActivityId,IssueId,IssueStatus"
                                                    CommandItemDisplay="None" AllowFilteringByColumn="false">
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
                                                        <telerik:GridBoundColumn DataField="IssueDate" HeaderText="Issue Date" DataFormatString="{0:dd-MMM-yyyy}">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="IssueId" HeaderText="Issue ID">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="CategoryName" HeaderText="Category">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="Name" HeaderText="Assigned To">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="ExpectedClosureDate" HeaderText="Expected Closure Date" DataFormatString="{0:dd-MMM-yyyy}">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="ActualClosingDate" HeaderText="Actual Close Date" DataFormatString="{0:dd-MMM-yyyy}">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="IssueStatus" HeaderText="Status">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridTemplateColumn UniqueName="EditIssue" HeaderStyle-Width="50px" ItemStyle-Width="50px" ShowFilterIcon="false" AllowFiltering="false" Exportable="false">
                                                            <HeaderTemplate>
                                                                <label>View</label>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Button runat="server" ID="btnEditIssue" Text="Edit" OnClick="btnRaiseIssues_Click" CssClass="btn btn-primary btn-xs center-block" CausesValidation="false" />
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>

                                                    </Columns>
                                                    <PagerStyle PageSizes="5,10,15" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                                                        PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />
                                                </MasterTableView>
                                            </telerik:RadGrid>
                                        </div>
                                    </div>
                                </div>
                                <!--End of modal-dialog1-->
                            </div>
                            <!-- Row  Modal for deleted Task- view Timesheet End Here-->


                            <!-- Row  Modal for survey Start Here-->

                            <div class="modal fade" id="divViewSurveys" tabindex="-1" role="dialog" aria-labelledby="modalLabel" aria-hidden="true">
                                <div class="modal-dialog1">
                                    <div class="modal-content">
                                        <div class="modal-header modal-header-resize">
                                            <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">×</span><span class="sr-only">Close</span></button>
                                            <h4 class="modal-title text-primary" id="lineModalLabel">View Survey</h4>
                                        </div>
                                        <div class="modal-body">

                                            <div class="row margin-0">
                                                <div class="col-xs-12">
                                                    <telerik:RadGrid RenderMode="Lightweight" ID="gridSurveys" runat="server" OnNeedDataSource="gridSurveys_NeedDataSource"
                                                        AutoGenerateColumns="false" AllowPaging="true" AllowSorting="true" AllowFilteringByColumn="false"
                                                        PageSize="<%# Convert.ToInt32(SolarPMS.Models.Constants.CONST_GRID_PAGE_SIZE) %>">
                                                        <GroupingSettings CaseSensitive="false" />
                                                        <MasterTableView CommandItemSettings-ShowRefreshButton="false" ClientDataKeyNames="" EditMode="InPlace"
                                                            CommandItemDisplay="None" AllowFilteringByColumn="false">
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
                                                                <telerik:GridBoundColumn DataField="VillageName" HeaderText="Village">
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn DataField="SurveyNo" HeaderText="Survey No.">
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn DataField="PraposedDivision" HeaderText="Proposed No. of Division">
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn DataField="PraposedArea" HeaderText="Proposed Area">
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn DataField="ActualNoOfDivision" HeaderText="Actual No. of Division">
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn DataField="ActualArea" HeaderText="Actual Area">
                                                                </telerik:GridBoundColumn>
                                                            </Columns>
                                                            <PagerStyle PageSizes="5,10,15" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                                                                PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />
                                                        </MasterTableView>
                                                    </telerik:RadGrid>

                                                    <div runat="server" id="div1">
                                                        <h5 runat="server" id="hSurveyQM">Partially Approved by QM</h5>
                                                        <telerik:RadGrid RenderMode="Lightweight" ID="gridSurveysQM" runat="server" OnNeedDataSource="gridSurveys_NeedDataSource"
                                                            AutoGenerateColumns="false" AllowPaging="true" AllowSorting="true" AllowFilteringByColumn="false"
                                                            PageSize="<%# Convert.ToInt32(SolarPMS.Models.Constants.CONST_GRID_PAGE_SIZE) %>">
                                                            <GroupingSettings CaseSensitive="false" />
                                                            <MasterTableView CommandItemSettings-ShowRefreshButton="false" ClientDataKeyNames="" EditMode="InPlace"
                                                                CommandItemDisplay="None" AllowFilteringByColumn="false">
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
                                                                    <telerik:GridBoundColumn DataField="VillageName" HeaderText="Village">
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridBoundColumn DataField="SurveyNo" HeaderText="Survey No.">
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridBoundColumn DataField="PraposedDivision" HeaderText="Proposed No. of Division">
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridBoundColumn DataField="PraposedArea" HeaderText="Proposed Area">
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridBoundColumn DataField="ActualNoOfDivision" HeaderText="Actual No. of Division">
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridBoundColumn DataField="ActualArea" HeaderText="Actual Area">
                                                                    </telerik:GridBoundColumn>
                                                                </Columns>
                                                                <PagerStyle PageSizes="5,10,15" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                                                                    PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />
                                                            </MasterTableView>
                                                        </telerik:RadGrid>
                                                    </div>
                                                    <div runat="server" id="div2">
                                                        <h5 runat="server" id="hSurveyPM">Partially Approved by PM</h5>
                                                        <telerik:RadGrid RenderMode="Lightweight" ID="gridSurveysPM" runat="server" OnNeedDataSource="gridSurveys_NeedDataSource"
                                                            AutoGenerateColumns="false" AllowPaging="true" AllowSorting="true" AllowFilteringByColumn="false"
                                                            PageSize="<%# Convert.ToInt32(SolarPMS.Models.Constants.CONST_GRID_PAGE_SIZE) %>">
                                                            <GroupingSettings CaseSensitive="false" />
                                                            <MasterTableView CommandItemSettings-ShowRefreshButton="false" ClientDataKeyNames="" EditMode="InPlace"
                                                                CommandItemDisplay="None" AllowFilteringByColumn="false">
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
                                                                    <telerik:GridBoundColumn DataField="VillageName" HeaderText="Village">
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridBoundColumn DataField="SurveyNo" HeaderText="Survey No.">
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridBoundColumn DataField="PraposedDivision" HeaderText="Proposed No. of Division">
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridBoundColumn DataField="PraposedArea" HeaderText="Proposed Area">
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridBoundColumn DataField="ActualNoOfDivision" HeaderText="Actual No. of Division">
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridBoundColumn DataField="ActualArea" HeaderText="Actual Area">
                                                                    </telerik:GridBoundColumn>
                                                                </Columns>
                                                                <PagerStyle PageSizes="5,10,15" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                                                                    PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />
                                                            </MasterTableView>
                                                        </telerik:RadGrid>
                                                    </div>
                                                </div>
                                                <!--End of scroll-->
                                            </div>
                                            <!-- End of row-->
                                        </div>
                                        <!--End of Scroll-->
                                    </div>
                                </div>
                            </div>

                            <div class="modal fade" id="divViewComments" tabindex="-1" role="dialog" aria-labelledby="modalLabel" aria-hidden="true">
                                <div class="modal-dialog1">
                                    <div class="modal-content">
                                        <div class="modal-header modal-header-resize">
                                            <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">×</span><span class="sr-only">Close</span></button>
                                            <h4 class="modal-title text-primary" id="lineModalLabel">View Comments</h4>
                                        </div>
                                        <div class="modal-body">

                                            <div class="row margin-0">
                                                <div class="col-xs-12">
                                                    <telerik:RadGrid RenderMode="Lightweight" ID="gridViewComments" runat="server" OnNeedDataSource="gridViewComments_NeedDataSource"
                                                        AutoGenerateColumns="false" AllowPaging="true" AllowSorting="true" AllowFilteringByColumn="false"
                                                        PageSize="<%# Convert.ToInt32(SolarPMS.Models.Constants.CONST_GRID_PAGE_SIZE) %>">
                                                        <GroupingSettings CaseSensitive="false" />
                                                        <MasterTableView CommandItemSettings-ShowRefreshButton="false" EditMode="InPlace"
                                                            CommandItemDisplay="None" AllowFilteringByColumn="false">
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
                                                                <telerik:GridBoundColumn DataField="Name" HeaderText="User Name">
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn DataField="Comment" HeaderText="Comment">
                                                                </telerik:GridBoundColumn>
                                                            </Columns>
                                                            <PagerStyle PageSizes="5,10,15" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                                                                PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />
                                                        </MasterTableView>
                                                    </telerik:RadGrid>

                                                </div>
                                                <!--End of scroll-->
                                            </div>
                                            <!-- End of row-->
                                        </div>
                                        <!--End of Scroll-->
                                    </div>
                                </div>
                            </div>
                            <!-- Row  Modal for survey End Here-->

                            <!-- Row  Modal for Block Start Here-->

                            <div class="modal fade" id="divViewBlocks" tabindex="-1" role="dialog" aria-labelledby="modalLabel" aria-hidden="true">
                                <div class="modal-dialog1">
                                    <div class="modal-content">
                                        <div class="modal-header modal-header-resize">
                                            <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">×</span><span class="sr-only">Close</span></button>
                                            <h4 class="modal-title text-primary" id="lineModalLabel">View Block</h4>
                                        </div>
                                        <div class="modal-body">
                                            <telerik:RadGrid RenderMode="Lightweight" ID="gridBlocks" runat="server" OnNeedDataSource="gridBlocks_NeedDataSource"
                                                AutoGenerateColumns="false" AllowPaging="true" AllowSorting="true" AllowFilteringByColumn="false"
                                                PageSize="<%# Convert.ToInt32(SolarPMS.Models.Constants.CONST_GRID_PAGE_SIZE) %>">
                                                <GroupingSettings CaseSensitive="false" />
                                                <MasterTableView CommandItemSettings-ShowRefreshButton="false" DataKeyNames="TimesheetBlockDetailId,TimesheetId" EditMode="InPlace"
                                                    CommandItemDisplay="None" AllowFilteringByColumn="false">
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
                                                        <telerik:GridBoundColumn DataField="BlockNo" HeaderText="Block No.">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="Quantity" HeaderText="Proposed Quantity">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="ActualQuantity" HeaderText="Actual Quantity">
                                                        </telerik:GridBoundColumn>
                                                    </Columns>
                                                    <PagerStyle PageSizes="5,10,15" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                                                        PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />
                                                </MasterTableView>
                                            </telerik:RadGrid>

                                            <div runat="server" id="divBlocksQM">
                                                <h5 runat="server" id="hBlockApprovedByQM">Partially Approved by QM</h5>
                                                <telerik:RadGrid RenderMode="Lightweight" ID="gridBlocksQM" runat="server" OnNeedDataSource="gridBlocks_NeedDataSource"
                                                    AutoGenerateColumns="false" AllowPaging="true" AllowSorting="true" AllowFilteringByColumn="false"
                                                    PageSize="<%# Convert.ToInt32(SolarPMS.Models.Constants.CONST_GRID_PAGE_SIZE) %>">
                                                    <GroupingSettings CaseSensitive="false" />
                                                    <MasterTableView CommandItemSettings-ShowRefreshButton="false" DataKeyNames="TimesheetBlockDetailId,TimesheetId" EditMode="InPlace"
                                                        CommandItemDisplay="None" AllowFilteringByColumn="false">
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
                                                            <telerik:GridBoundColumn DataField="BlockNo" HeaderText="Block No.">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="Quantity" HeaderText="Proposed Quantity">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="ActualQuantity" HeaderText="Actual Quantity">
                                                            </telerik:GridBoundColumn>
                                                        </Columns>
                                                        <PagerStyle PageSizes="5,10,15" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                                                            PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />
                                                    </MasterTableView>
                                                </telerik:RadGrid>
                                            </div>
                                            <div runat="server" id="divBlocksPM">
                                                <h5 runat="server" id="hBlockApprovedByPM">Partially Approved by PM</h5>
                                                <telerik:RadGrid RenderMode="Lightweight" ID="gridBlocksPM" runat="server" OnNeedDataSource="gridBlocks_NeedDataSource"
                                                    AutoGenerateColumns="false" AllowPaging="true" AllowSorting="true" AllowFilteringByColumn="false"
                                                    PageSize="<%# Convert.ToInt32(SolarPMS.Models.Constants.CONST_GRID_PAGE_SIZE) %>">
                                                    <GroupingSettings CaseSensitive="false" />
                                                    <MasterTableView CommandItemSettings-ShowRefreshButton="false" DataKeyNames="TimesheetBlockDetailId,TimesheetId" EditMode="InPlace"
                                                        CommandItemDisplay="None" AllowFilteringByColumn="false">
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
                                                            <telerik:GridBoundColumn DataField="BlockNo" HeaderText="Block No.">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="Quantity" HeaderText="Proposed Quantity">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="ActualQuantity" HeaderText="Actual Quantity">
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
                            <!-- Row  Modal for Block End Here-->

                            <!-- Row  Modal for Invertor/SCB/Table  Start Here-->
                            <div class="modal fade" id="divViewInvSCBTable" tabindex="-1" role="dialog" aria-labelledby="modalLabel" aria-hidden="true">
                                <div class="modal-dialog1">
                                    <div class="modal-content">
                                        <div class="modal-header modal-header-resize">
                                            <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">×</span><span class="sr-only">Close</span></button>
                                            <h4 class="modal-title text-primary" id="lineModalLabel">View Invertor/SCB/Table/Number/Location Details</h4>
                                        </div>
                                        <div class="modal-body">
                                            <telerik:RadGrid RenderMode="Lightweight" ID="gridInvSCBTable" runat="server" OnNeedDataSource="gridInvSCBTable_NeedDataSource"
                                                AutoGenerateColumns="false" AllowPaging="true" AllowSorting="true" AllowFilteringByColumn="false" PageSize="<%# Convert.ToInt32(SolarPMS.Models.Constants.CONST_GRID_PAGE_SIZE) %>">
                                                <GroupingSettings CaseSensitive="false" />
                                                <MasterTableView CommandItemSettings-ShowRefreshButton="false" ClientDataKeyNames="" EditMode="InPlace"
                                                    CommandItemDisplay="None" AllowFilteringByColumn="false">
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
                                                        <telerik:GridBoundColumn DataField="FromRange" HeaderText="From Range">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="ToRange" HeaderText="To Range">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="ProposedQuantity" HeaderText="Proposed Quantity">
                                                        </telerik:GridBoundColumn>
                                                    </Columns>
                                                    <PagerStyle PageSizes="5,10,15" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                                                        PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />
                                                </MasterTableView>
                                            </telerik:RadGrid>

                                            <div runat="server" id="divInvScbQM">
                                                <h5 runat="server" id="hTableQM">Partially Approved by QM</h5>

                                                <telerik:RadGrid RenderMode="Lightweight" ID="gridInvSCBTableQM" runat="server"
                                                    AutoGenerateColumns="false" AllowPaging="true" AllowSorting="true" AllowFilteringByColumn="false"
                                                    PageSize="<%# Convert.ToInt32(SolarPMS.Models.Constants.CONST_GRID_PAGE_SIZE) %>">
                                                    <GroupingSettings CaseSensitive="false" />
                                                    <MasterTableView CommandItemSettings-ShowRefreshButton="false" ClientDataKeyNames="" EditMode="InPlace"
                                                        CommandItemDisplay="None" AllowFilteringByColumn="false">
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
                                                            <telerik:GridBoundColumn DataField="FromRange" HeaderText="From Range">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="ToRange" HeaderText="To Range">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="ProposedQuantity" HeaderText="Proposed Quantity">
                                                            </telerik:GridBoundColumn>
                                                        </Columns>
                                                        <PagerStyle PageSizes="5,10,15" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                                                            PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />
                                                    </MasterTableView>
                                                </telerik:RadGrid>
                                            </div>
                                            <div runat="server" id="divInvScbPM">
                                                <h5 runat="server" id="hTablePM">Partially Approved by PM</h5>
                                                <telerik:RadGrid RenderMode="Lightweight" ID="gridInvSCBTablePM" runat="server"
                                                    AutoGenerateColumns="false" AllowPaging="true" AllowSorting="true" AllowFilteringByColumn="false" PageSize="<%# Convert.ToInt32(SolarPMS.Models.Constants.CONST_GRID_PAGE_SIZE) %>">
                                                    <GroupingSettings CaseSensitive="false" />
                                                    <MasterTableView CommandItemSettings-ShowRefreshButton="false" ClientDataKeyNames="" EditMode="InPlace"
                                                        CommandItemDisplay="None" AllowFilteringByColumn="false">
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
                                                            <telerik:GridBoundColumn DataField="FromRange" HeaderText="From Range">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="ToRange" HeaderText="To Range">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="ProposedQuantity" HeaderText="Proposed Quantity">
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
                            <!-- Row  Modal for Invertor/SCB/Table End Here-->

                            <!-- Row  Modal for View Attachment  Start Here-->
                            <div class="modal fade" id="divViewAttachment" tabindex="-1" role="dialog" aria-labelledby="modalLabel" aria-hidden="true">
                                <div class="modal-dialog1">
                                    <div class="modal-content">
                                        <div class="modal-header modal-header-resize">
                                            <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">×</span><span class="sr-only">Close</span></button>
                                            <h4 class="modal-title text-primary" id="lineModalLabel">View Attachment</h4>
                                        </div>
                                        <div class="modal-body">
                                            <telerik:RadGrid RenderMode="Lightweight" ID="gridTimesheetAttachment" runat="server" OnItemDataBound="gridTimesheetAttachment_ItemDataBound"
                                                OnNeedDataSource="gridTimesheetAttachment_NeedDataSource" AutoGenerateColumns="false" AllowPaging="true" AllowSorting="true" AllowFilteringByColumn="false"
                                                PageSize="<%# Convert.ToInt32(SolarPMS.Models.Constants.CONST_GRID_PAGE_SIZE) %>">
                                                <GroupingSettings CaseSensitive="false" />
                                                <MasterTableView CommandItemSettings-ShowRefreshButton="false" ClientDataKeyNames="" EditMode="InPlace"
                                                    CommandItemDisplay="None" AllowFilteringByColumn="false">
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
                                                        <telerik:GridHyperLinkColumn DataTextField="FilePath" DataNavigateUrlFields="FilePath" HeaderText="File Name" UniqueName="FileName">
                                                        </telerik:GridHyperLinkColumn>
                                                        <telerik:GridBoundColumn DataField="CreatedOn" HeaderText="Date" UniqueName="Date" DataFormatString="{0:dd-MMM-yyyy}">
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
                            <!-- Row  Modal for View Attachment End Here-->
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- End of container-fluid-->
</asp:Content>
