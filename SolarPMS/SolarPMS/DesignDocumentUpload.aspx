<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/SolarPMS.Master" AutoEventWireup="true" CodeBehind="DesignDocumentUpload.aspx.cs" Inherits="SolarPMS.DesignDocumentUpload" %>

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

            function viewDocument(activityid, subactivityid, rowIndex) {
                setTimeout(function () {
                    var ajaxManager = $find("<%= RadAjaxManager.GetCurrent(this.Page).ClientID %>");// $find("<%= ajaxManagerDocumentUpload.ClientID %>");
                    ajaxManager.ajaxRequest("ViewDocument#" + activityid + "#" + subactivityid);
                    $("#divViewAttachment").modal("show");
                }, 1000);
            }

            function viewAssignedHistory(activityid, subactivityid, rowIndex) {
                setTimeout(function () {
                    var ajaxManager = $find("<%= RadAjaxManager.GetCurrent(this.Page).ClientID %>");// $find("<%= ajaxManagerDocumentUpload.ClientID %>");
                    ajaxManager.ajaxRequest("ViewAssignedHistory#" + activityid + "#" + subactivityid);
                    $("#divViewHistory").modal("show");
                }, 1000);
            }

            function viewIssues(activityid, subactivityid, rowIndex) {
                var ajaxManager = $find("<%= RadAjaxManager.GetCurrent(this.Page).ClientID %>");// $find("<%= ajaxManagerDocumentUpload.ClientID %>");
                ajaxManager.ajaxRequest("ViewIssues#" + activityid + "#" + subactivityid);
                $("#divViewIssues").modal("show");
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
                    if (screen == 'document') {
                        manager.open("UploadDEDocument.aspx", "TimesheetRadWindow");
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
                var gridActivity = $find("<%= gridActivityDetails.ClientID %>").get_masterTableView();
                gridActivity.rebind();
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
    <telerik:RadAjaxManager ID="ajaxManagerDocumentUpload" EnablePageHeadUpdate="false" runat="server" OnAjaxRequest="ajaxManagerDocumentUpload_AjaxRequest">
        <ClientEvents OnRequestStart="onRequestStart" />
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="TimesheetRadWindowManager">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="gridActivityDetails"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="TimesheetRadWindow">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="gridActivityDetails"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="gridActivityDetails">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="gridActivityDetails"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="gridDocument">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="gridDocument"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="ajaxManagerDocumentUpload">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="gridDocument" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="gridIssues" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="LoadingPanel1" runat="server">
    </telerik:RadAjaxLoadingPanel>
    <div class="col-xs-12 heading-big padding-b-10">
        <h5 class="margin-0 lineheight-30 breath-ctrl">Home / <span style="font-weight: normal !important">Design and Engineer Documents</span></h5>
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
        <div class="row margin-0">
            <div class="col-xs-12 padding-0">
                <div class="col-sm-11 col-md-11 col-lg-11 col-xs-12 padding-0">
                    <div id="divDrpSite" runat="server" class="col-sm-6 col-lg-3 col-md-3 col-xs-12 padding-0 margin-b5">
                        <div class="col-sm-3  col-xs-12">
                            <asp:Label runat="server" CssClass="control-label lable-txt" Text="Site"></asp:Label>
                        </div>
                        <div class="col-sm-8 col-xs-12">
                            <telerik:RadComboBox RenderMode="Lightweight" ID="ddlSapSite" runat="server" Height="200" AutoPostBack="true"
                                Skin="Office2010Silver" Filter="Contains" MarkFirstMatch="true" EnableLoadOnDemand="false" OnSelectedIndexChanged="ddlSapSite_SelectedIndexChanged">
                            </telerik:RadComboBox>
                        </div>
                    </div>
                    <div id="divDrpProject" runat="server" class="col-sm-6 col-lg-3 col-md-3 col-xs-12 padding-0 margin-b5">
                        <div class="col-sm-3 col-xs-12">
                            <asp:Label runat="server" CssClass="control-label lable-txt" Text="Project"></asp:Label>
                        </div>
                        <div class=" col-sm-8 col-xs-12">
                            <telerik:RadComboBox RenderMode="Lightweight" ID="ddlSapProjects" runat="server" Height="200" EmptyMessage="" OnSelectedIndexChanged="ddlSapProjects_SelectedIndexChanged"
                                AutoPostBack="true" Skin="Office2010Silver" Filter="Contains" ValidationGroup="Search" MarkFirstMatch="true" EnableLoadOnDemand="false">
                            </telerik:RadComboBox>
                        </div>
                    </div>
                    <div id="divDrpArea" runat="server" class="col-sm-6 col-lg-3 col-md-3 col-xs-12 padding-0 margin-b5">
                        <div class="col-sm-3 col-xs-12">
                            <asp:Label runat="server" CssClass="control-label lable-txt" Text="Area"></asp:Label>
                        </div>
                        <div class="col-sm-8 col-xs-12">
                            <telerik:RadComboBox RenderMode="Lightweight" ID="ddlArea" runat="server" Height="200" EmptyMessage="" OnSelectedIndexChanged="ddlArea_SelectedIndexChanged"
                                AutoPostBack="true" Skin="Office2010Silver" Filter="Contains" ValidationGroup="Search" MarkFirstMatch="true" EnableLoadOnDemand="false">
                            </telerik:RadComboBox>
                        </div>
                    </div>
                    <div id="divDrpNetwork" runat="server" class="col-sm-6 col-lg-3 col-md-3 col-xs-12 padding-0 margin-b5">
                        <div class="col-sm-3 col-xs-12">
                            <asp:Label runat="server" CssClass="control-label lable-txt" Text="Network"></asp:Label>
                        </div>
                        <div class="col-sm-8 col-xs-12">
                            <telerik:RadComboBox RenderMode="Lightweight" ID="ddlNetwork" runat="server" Height="200" EmptyMessage=""
                                Skin="Office2010Silver" Filter="Contains" ValidationGroup="Search" MarkFirstMatch="true" EnableLoadOnDemand="false">
                            </telerik:RadComboBox>
                        </div>
                    </div>
                </div>
                <div class="col-sm-1 col-sm-1 col-lg-1 col-xs-12">
                    <div id="divBtnSearch" runat="server" class=" padding-0 text-right margin-b5 pull-right">
                        <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" CssClass="btn btn-cust btn-primary " ValidationGroup="Search" />
                    </div>
                </div>
            </div>
        </div>
        <div class="row margin-0" style="margin-top: 10px !important;">
            <div class="col-xs-12 padding-lr-10">
                <telerik:RadGrid RenderMode="Lightweight" ID="gridActivityDetails" runat="server" OnNeedDataSource="gridActivityDetails_NeedDataSource"
                    OnItemCreated="gridActivityDetails_ItemCreated" AutoGenerateColumns="false" ExportSettings-ExportOnlyData="true"
                    OnItemDataBound="gridActivityDetails_ItemDataBound"
                    AllowPaging="true" AllowSorting="true" AllowFilteringByColumn="true" PageSize="<%# Convert.ToInt32(SolarPMS.Models.Constants.CONST_GRID_PAGE_SIZE) %>">
                    <GroupingSettings CaseSensitive="false" />
                    <ClientSettings>
                        <Scrolling AllowScroll="True" UseStaticHeaders="True" ScrollHeight="250px" SaveScrollPosition="true" FrozenColumnsCount="3"></Scrolling>
                    </ClientSettings>

                    <ExportSettings IgnorePaging="true" ExportOnlyData="true" HideStructureColumns="true"></ExportSettings>
                    <MasterTableView CommandItemDisplay="None" DataKeyNames="SAPSite,SAPProjectId,SAPNetwork,WBSAreaId,ActivityId,SubActivityId,ActivityPlanStartDate,
				                                                ActivityPlanFinishDate,SAPNetwork,SAPActivity,SapSubActivity,ActivityDescription,
                                                                SAPSubActivityDescription,Version,DocumentDetailsId"
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
                            <telerik:GridBoundColumn DataField="ActivityDescription" MaxLength="30" HeaderText="Activity" ColumnEditorID="TextboxEditor"
                                HeaderStyle-Width="130px" DataType="System.String">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="SAPSubActivityDescription" MaxLength="30" HeaderText="Sub Activity" ColumnEditorID="TextboxEditor"
                                HeaderStyle-Width="130px" DataType="System.String">
                            </telerik:GridBoundColumn>                            
                            <telerik:GridBoundColumn DataField="Version" HeaderText="Latest Version"
                                HeaderStyle-Width="130px" DataType="System.Int32">
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn HeaderStyle-Width="100px" FilterControlWidth="100px"
                                DataField="ReleaseToContsruction" HeaderText="Release to Contsruction" UniqueName="ReleaseToContsruction" SortExpression="ReleaseToContsruction">
                                <ItemTemplate>
                                    <asp:Label ID="lblReleaseToContsruction" Text='<%# Bind("ReleaseToContsruction") %>' runat="server"></asp:Label>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>                           
                            <telerik:GridTemplateColumn HeaderStyle-HorizontalAlign="Center" UniqueName="AddDocument" HeaderStyle-Width="90px" ShowFilterIcon="false" AllowFiltering="false" Exportable="false">
                                <HeaderTemplate>
                                    <label>Add Document</label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Button runat="server" ID="btnAAddDocument" OnClick="btnAddDocument_Click" Text="Add" CssClass="btn teal-btn btn-xs center-block" CausesValidation="false" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderStyle-HorizontalAlign="Center" UniqueName="ReviewDocument" HeaderStyle-Width="90px" ShowFilterIcon="false"
                                AllowFiltering="false" Exportable="false">
                                <HeaderTemplate>
                                    <label>Add Review</label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Button runat="server" ID="btnReviewDocument" OnClick="btnReviewDocument_Click" Text="Review"
                                        CssClass="btn teal-btn btn-xs center-block" CausesValidation="false" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn UniqueName="ViewDocument" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="90px" ShowFilterIcon="false" AllowFiltering="false" Exportable="false">
                                <HeaderTemplate>
                                    <label>Documents</label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Button runat="server" ID="btnViewDocument" Text="View" CssClass="btn btn-rb btn-xs center-block" CausesValidation="false" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                        <PagerStyle PageSizes="5,10,15" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                            PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />
                    </MasterTableView>
                </telerik:RadGrid>
            </div>
        </div>
        <div>
            <div class="modal fade" id="divViewAttachment" tabindex="-1" role="dialog" aria-labelledby="modalLabel" aria-hidden="true">
                <div class="modal-dialog1">
                    <div class="modal-content">
                        <div class="modal-header modal-header-resize">
                            <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">×</span><span class="sr-only">Close</span></button>
                            <h4 class="modal-title text-primary" id="lineModalLabel">View Attachment</h4>
                        </div>
                        <div class="modal-body">
                            <telerik:RadGrid RenderMode="Lightweight" ID="gridDocument" runat="server" OnNeedDataSource="gridDocument_NeedDataSource"
                                AutoGenerateColumns="false" AllowPaging="true" AllowSorting="true" OnItemDataBound="gridDocument_ItemDataBound"
                                OnDeleteCommand="gridDocument_DeleteCommand"
                                AllowFilteringByColumn="false"
                                PageSize="<%# Convert.ToInt32(SolarPMS.Models.Constants.CONST_GRID_PAGE_SIZE) %>">
                                <GroupingSettings CaseSensitive="false" />
                                <MasterTableView CommandItemSettings-ShowRefreshButton="false" EditMode="InPlace" DataKeyNames="Id,DocumentDetailsId,FileName"
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
                                        <telerik:GridHyperLinkColumn DataTextField="FileName" DataNavigateUrlFields="FilePath" HeaderText="File Name" UniqueName="FileName">
                                        </telerik:GridHyperLinkColumn>
                                        <telerik:GridBoundColumn DataField="Comments" HeaderText="Comments" UniqueName="Comments">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="Version" HeaderText="Version" UniqueName="Version">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="CreatedBy" HeaderText="User Name" UniqueName="CreatedBy">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="CreatedOn" HeaderText="Created On" UniqueName="CreatedOn" DataFormatString="{0:dd-MMM-yyyy}">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridButtonColumn CommandName="Delete" HeaderText="Delete" ConfirmTitle="Confirm" ConfirmDialogHeight="120" UniqueName="DeleteColumn"
                                            ConfirmDialogWidth="120" ConfirmText="Are you sure you want to delete this record?" ConfirmDialogType="Classic">
                                        </telerik:GridButtonColumn>
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
</asp:Content>
