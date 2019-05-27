<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/SolarPMS.Master" AutoEventWireup="true" CodeBehind="TaskAllocationDetails.aspx.cs" Inherits="SolarPMS.TaskAllocationDetails" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--<script src="Scripts/jquery-1.10.2.js"></script>
    <script src="Scripts/bootstrap.min.js"></script>--%>
    
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" OnAjaxRequest="RadAjaxManager1_AjaxRequest">
        <ClientEvents/>
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadAjaxManager1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdAssignHistory" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="drpSite">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="drpProject" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
                  <telerik:AjaxSetting AjaxControlID="grdAllocated">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdAllocated" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
                  <telerik:AjaxSetting AjaxControlID="grdNotAllocated">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdNotAllocated" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
                  <telerik:AjaxSetting AjaxControlID="grdActivityDeleted">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdActivityDeleted" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>

    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="LoadingPanel1" runat="server" Skin="Default">
    </telerik:RadAjaxLoadingPanel>

    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script src="Scripts/Page/Common.js"></script>
        <script type="text/javascript">
            function ShowAssignHistory(siteId, project, area, network, activity, subactivity, rowIndex) {

                var ajaxManager = $find("<%= RadAjaxManager1.ClientID %>");
                ajaxManager.ajaxRequest("AssignHistory#'" + siteId + "'#'" + project + "'#'" + area + "'#'" + network + "'#'" + activity + "'#'" + subactivity + "'");

                $('#squarespaceModal').modal();
            }
            function ShowTimeSheet(id, rowIndex) {
                $('#squarespaceModal3').modal();
            }
            function ShowActivityIssue(id, rowIndex) {
                $('#squarespaceModal4').modal();
            }

            function showExportButton(tabname)
            {
                if (tabname == 'tab1') {
                    $("[id$=btnExportAssignTask]").removeClass("hide");
                    $("[id$=btnExportNotAssign]").addClass("hide");
                }
                else {
                    $("[id$=btnExportAssignTask]").addClass("hide");
                    $("[id$=btnExportNotAssign]").removeClass("hide");
                }
            }

            function onRequestStart(sender, args) {                
            }
        </script>
    </telerik:RadCodeBlock>
    <div class="row margin-0">
        <div class="col-xs-12 padding-0 padding-b-2">

                <div class="col-xs-6 heading-big">
                    <h5 class="margin-0 lineheight-30 breath-ctrl">Home / Administration / <span style="font-weight: normal !important">Task Allocation Details</span></h5>
                </div>
            </div>

        <div class="col-xs-12 padding-10">
            

            <div class="col-lg-12 padding-0">
                <div class="col-sm-6">

                    <div class="col-sm-1 padding-0">
                        <label class="control-label lable-txt" for="name">Site</label>
                    </div>
                    <!-- End of col-lg-1-->
                    <div class=" col-xs-11">
                        <%--<asp:DropDownList ID="drpSite" runat="server" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="drpSite_SelectedIndexChanged">
                        </asp:DropDownList>--%>
                        <telerik:RadComboBox runat="server" RenderMode="Lightweight" ID="drpSite" AutoPostBack="true"  CausesValidation="false"
                            Filter="Contains" MarkFirstMatch="true" Width="100%" OnSelectedIndexChanged="drpSite_SelectedIndexChanged" 
                            EmptyMessage= "Select Site" Skin="Office2010Silver" ></telerik:RadComboBox>
                        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ControlToValidate="drpSite" ErrorMessage="* Required" ForeColor="Red"></asp:RequiredFieldValidator>
                    </div>
                    <!-- End of input-group-col-xs-8-->

                </div>

                <div class="col-sm-5">

                    <div class="col-sm-3">
                        <label class="control-label lable-txt" for="name">Project</label>
                    </div>
                    <!-- End of col-lg-1-->
                    <div class=" col-xs-9">
                        <%--<asp:DropDownList ID="drpProject" runat="server" class="form-control">
                        </asp:DropDownList>--%>
                        <telerik:RadComboBox runat="server" RenderMode="Lightweight" ID="drpProject" Skin="Office2010Silver" CausesValidation="true"
                            EmptyMessage="Select Project" Filter="Contains" MarkFirstMatch="true" Width="100%" >
                        </telerik:RadComboBox>
                        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="drpProject" ErrorMessage="* Required" ForeColor="Red"></asp:RequiredFieldValidator>
                    </div>
                    <!-- End of input-group-col-xs-8-->

                </div>

                <div class="col-sm-1">
                    <div class="input-group-btn">
                        <%-- <button class="btn btn-primary" type="submit"><i class="glyphicon glyphicon-search"></i></button>--%>
                        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary" OnClick="btnSearch_Click" CausesValidation="true" />
                    </div>
                </div>


            </div>
            <!-- End of col-lg-5-->


    </div>
    <!-- End of box Wrapper-->
        <!-- Start of Task Allocation-->
        <div class="task-allocation-details" style="padding:0px !important;">

            <!-- Start of tabs-->

            <div class="col-md-12 padding-0">
                <div class="panel with-nav-tabs panel-default border-0">
                    <div class="panel-heading padding-0 nocolor">
                        <ul class="nav nav-tabs border-0 col-xs-11">
                            <li class="active"><a href="#tab1primary" onclick="showExportButton('tab1')" data-toggle="tab">Activity Allocated &nbsp;<span class="badge badge-info"></span></a></li>
                            <li><a href="#tab2primary" onclick="showExportButton('tab2')" data-toggle="tab">Activity Not Allocated  &nbsp;<span class="badge badge-important"></span></a></li>
                         <%--   <li><a href="#tab3primary" data-toggle="tab">Activity Deleted &nbsp;<span class="badge badge-warning"></span></a></li>--%>

                        </ul>
                        <ul class="col-xs-1 pull-right nav" style="padding: 5px 0px 0px 5px;">
                            <li>
                                <asp:Button ID="btnExportAssignTask" runat="server" CssClass=" btn btn-primary pull-right" Text="Export"
                                    CausesValidation="false" OnClick="btnExportAssignTask_Click" />
                                <asp:Button ID="btnExportNotAssign" runat="server" CssClass="pull-right btn btn-primary hide" Text="Export"
                                    CausesValidation="false" OnClick="btnExportNotAssign_Click" />
                            </li>
                        </ul>
                    </div>
                    <div class="panel-body padding-lr-10 ">
                        <div class="tab-content">
                            <div class="tab-pane fade in active" id="tab1primary">
                                <div class="col-lg-12 padding-0" style="margin-top:5px;">
                                    <%--<button class="btn btn-primary button pull-right"> <span class=" glyphicon glyphicon-export text-default"></span> Export Excel</button>--%>
                                   <%-- <telerik:RadButton RenderMode="Lightweight" ID="btnExportAssignTask" CssClass="pull-right" runat="server" Text="Export" OnClick="btnExportAssignTask_Click">
                                        <Icon PrimaryIconCssClass="rbDownload"></Icon>                                    </telerik:RadButton>--%>
                                          
                                    <%--<asp:Button ID="btnExportAssignTask" runat="server" CssClass="pull-right btn btn-primary" Text="Export" OnClick="btnExportAssignTask_Click" />--%>
                                </div>
                                <div class="table-scroll">

                                    <telerik:RadGrid RenderMode="Lightweight" ID="grdAllocated" runat="server" ExportSettings-ExportOnlyData="true"
                                        AutoGenerateColumns="false" AllowPaging="true" AllowSorting="true" AllowFilteringByColumn="false" PageSize="<%# Convert.ToInt32(SolarPMS.Models.Constants.CONST_GRID_PAGE_SIZE) %>" OnNeedDataSource="grdAllocated_NeedDataSource" OnItemCreated="grdAllocated_ItemCreated">
                                        <GroupingSettings CaseSensitive="false" />
                                        <MasterTableView CommandItemSettings-ShowRefreshButton="false" ClientDataKeyNames="sapSite" EditMode="InPlace" CommandItemDisplay="None" AllowFilteringByColumn="true">
                                            <NoRecordsTemplate>
                                                    <table width="100%" border="0" cellpadding="20" cellspacing="20">
                                                        <tr>
                                                            <td align="center" class="txt-white">No records to display.
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </NoRecordsTemplate>
                                            <CommandItemSettings ShowRefreshButton="false" ShowExportToExcelButton="false" />
                                            <Columns>
                                                <telerik:GridBoundColumn DataField="sapSite" HeaderText="Site" UniqueName="Site">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="projectDescription" HeaderText="Project" UniqueName="Project">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="wbsArea" HeaderText="Area" UniqueName="Area">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="sapNetWork" HeaderText="Network" UniqueName="Network">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="sapActivity" HeaderText="Activities" UniqueName="Activities">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="sapSubActivity" HeaderText="SubActivities" UniqueName="SubActivities">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="SiteFunctionalUser" HeaderText="Site Functionl User" UniqueName="SiteFunctionlUser">
                                                </telerik:GridBoundColumn>
                                                <%--<telerik:GridBoundColumn DataField="countSiteFunctionalUser" HeaderText="Count of Site Functional User" UniqueName="CountofSiteFunctionalUser">
                                                </telerik:GridBoundColumn>--%>
                                                <telerik:GridTemplateColumn HeaderText="Count of Site Functional User" DataField="CountSiteFunctionalUser" UniqueName="CountofSiteFunctionalUser">
                                                    <ItemTemplate>
                                                            <%# (Eval("CountSiteFunctionalUser").ToString() != "0" ? Eval("CountSiteFunctionalUser") : string.Empty) %>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridBoundColumn DataField="SiteFunctionalManager" HeaderText="Site Functional Manager" UniqueName="SiteFunctionalManager">
                                                </telerik:GridBoundColumn>
                                                <%--<telerik:GridBoundColumn DataField="countSiteFunctionalManager" HeaderText="Count of Site Functional Manager" UniqueName="CountofSiteFunctionalManager">
                                                </telerik:GridBoundColumn>--%>
                                                <telerik:GridTemplateColumn HeaderText="Count of Site Functional Manager" DataField="CountSiteFunctionalManager" UniqueName="CountofSiteFunctionalManager">
                                                    <ItemTemplate>
                                                        <%# (Eval("CountSiteFunctionalManager").ToString() != "0" ? Eval("CountSiteFunctionalManager") : string.Empty) %>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridBoundColumn DataField="SiteQualityManager" HeaderText="Site Quality Manager" UniqueName="SiteQualityManager">
                                                </telerik:GridBoundColumn>
                                                <%--<telerik:GridBoundColumn DataField="countSiteQualityManager" HeaderText="Count of Site Quality Manager" UniqueName="CountofSiteQualityManager">
                                                </telerik:GridBoundColumn>--%>
                                                <telerik:GridTemplateColumn HeaderText="Count of Site Quality Manager" DataField="CountSiteQualityManager" UniqueName="CountofSiteQualityManager">
                                                    <ItemTemplate>
                                                        <%# (Eval("CountSiteQualityManager").ToString() != "0" ? Eval("CountSiteQualityManager") : string.Empty) %>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridBoundColumn DataField="StateProjectHead" HeaderText="State Project Head" UniqueName="StateProjectHead">
                                                </telerik:GridBoundColumn>
                                                <%--<telerik:GridBoundColumn DataField="countStateProjectHead" HeaderText="Count of State Project Head" UniqueName="CountofStateProjectHead">
                                                </telerik:GridBoundColumn>--%>
                                                <telerik:GridTemplateColumn HeaderText="Count of State Project Head" DataField="CountStateProjectHead" UniqueName="CountofStateProjectHead">
                                                    <ItemTemplate>
                                                        <%# (Eval("CountStateProjectHead").ToString() != "0" ? Eval("CountStateProjectHead") : string.Empty) %>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridBoundColumn DataField="DesignEngineer" HeaderText="Design Engineer" UniqueName="DesignEngineer">
                                                </telerik:GridBoundColumn>
                                                <%--<telerik:GridBoundColumn DataField="CountofDesignEngineer" HeaderText="Count of Design Engineer" UniqueName="CountofDesignEngineer">
                                                </telerik:GridBoundColumn>--%>
                                                 <telerik:GridTemplateColumn HeaderText="Count of Design Engineer" DataField="CountDesignEngineer" UniqueName="CountofDesignEngineer">
                                                    <ItemTemplate>
                                                        <%# (Eval("CountDesignEngineer").ToString() != "0" ? Eval("CountDesignEngineer") : string.Empty) %>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridBoundColumn DataField="DesignManager" HeaderText="Design Manager" UniqueName="DesignManager">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridTemplateColumn HeaderText="Count of Design Manager" DataField="CountDesignManager" UniqueName="CountofDesignManager">
                                                    <ItemTemplate>
                                                        <%# (Eval("CountDesignManager").ToString() != "0" ? Eval("CountDesignManager") : string.Empty) %>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <%--<telerik:GridBoundColumn DataField="countDesignManager" HeaderText="Count of Design Manager" UniqueName="CountofDesignManager">
                                                </telerik:GridBoundColumn>--%>
                                             <%--   <telerik:GridTemplateColumn UniqueName="TemplateEditColumn" HeaderText="Assigned History">
                                                    <ItemTemplate>
                                                        <asp:HyperLink ID="ViewHistory" runat="server" Text="View"></asp:HyperLink>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>--%>

                                            </Columns>
                                            <PagerStyle PageSizes="5,10,15" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                                                PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />


                                        </MasterTableView>
                                    </telerik:RadGrid>




                                </div>
                                <!--End of scroll-->

                            </div>
                            <div class="tab-pane fade" id="tab2primary">

                                <div class="col-lg-12 padding-0"style="margin-top:5px;">
                                    <%-- <button class="btn btn-primary button pull-right"> <span class=" glyphicon glyphicon-export text-default"></span> Export Excel</button>--%>
                                 <%--   <telerik:RadButton RenderMode="Lightweight" ID="btnExportNotAssign" CssClass="pull-right" runat="server" Text="Export" OnClick="btnExportNotAssign_Click">
                                        <Icon PrimaryIconCssClass="rbDownload"></Icon>
                                    </telerik:RadButton>--%>                                     
                                </div>
                                <div class="table-scroll">
                                <telerik:RadGrid RenderMode="Lightweight" ID="grdNotAllocated" runat="server" ExportSettings-ExportOnlyData="true"
                                    AutoGenerateColumns="false" AllowPaging="true" AllowSorting="true" AllowFilteringByColumn="false" PageSize="<%# Convert.ToInt32(SolarPMS.Models.Constants.CONST_GRID_PAGE_SIZE) %>" OnNeedDataSource="grdNotAllocated_NeedDataSource">
                                    <GroupingSettings CaseSensitive="false" />
                                    <MasterTableView CommandItemSettings-ShowRefreshButton="false" ClientDataKeyNames="SAPSite" EditMode="InPlace" CommandItemDisplay="None" AllowFilteringByColumn="true">
                                        <NoRecordsTemplate>
                                                    <table width="100%" border="0" cellpadding="20" cellspacing="20">
                                                        <tr>
                                                            <td align="center" class="txt-white">No records to display.
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </NoRecordsTemplate>
                                        <CommandItemSettings ShowRefreshButton="false" ShowExportToExcelButton="false" />
                                        <Columns>
                                            <telerik:GridBoundColumn DataField="SAPSite" HeaderText="Site" UniqueName="Site">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="ProjectDescription" HeaderText="Project" UniqueName="Project">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="WBSArea" HeaderText="Area" UniqueName="Area">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="NetworkDescription" HeaderText="Network" UniqueName="Network">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="ActivityDescription" HeaderText="Activities" UniqueName="Activities">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="SAPSubActivityDescription" HeaderText="Sub Activities" UniqueName="SubActivities">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="createdOn" HeaderText="Created Date" UniqueName="CreatedDate">
                                            </telerik:GridBoundColumn>

                                            <%--  <telerik:GridTemplateColumn UniqueName="TemplateEditColumn" HeaderText="Assigned History">
                                                            <ItemTemplate>
                                                                <asp:HyperLink ID="ViewHistory" runat="server"  Text="View"></asp:HyperLink>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>   --%>
                                        </Columns>
                                        <PagerStyle PageSizes="5,10,15" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                                            PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle"  />
                                    </MasterTableView>
                                </telerik:RadGrid>
                            </div>


                            </div>
                            <div class="tab-pane fade" id="tab3primary">

                                <div class="col-lg-12 padding-0 margin-10">
                                    <%-- <button class="btn btn-primary button pull-right"> <span class=" glyphicon glyphicon-export text-default"></span> Export Excel</button>--%>
                                    <telerik:RadButton RenderMode="Lightweight" ID="btnExportDeleteActivity" CssClass="pull-right" runat="server" Text="Export" OnClick="btnExportDeleteActivity_Click">
                                        <Icon PrimaryIconCssClass="rbDownload"></Icon>
                                    </telerik:RadButton>
                                </div>
                                <div class="table-scroll">

                                    <telerik:RadGrid RenderMode="Lightweight" ID="grdActivityDeleted" runat="server"
                                        AutoGenerateColumns="false" AllowPaging="true" AllowSorting="true" AllowFilteringByColumn="false" PageSize="<%# Convert.ToInt32(SolarPMS.Models.Constants.CONST_GRID_PAGE_SIZE) %>" OnNeedDataSource="grdActivityDeleted_NeedDataSource" OnItemCreated="grdActivityDeleted_ItemCreated">
                                        <GroupingSettings CaseSensitive="false" />
                                        <MasterTableView CommandItemSettings-ShowRefreshButton="false" ClientDataKeyNames="sapSite" EditMode="InPlace" CommandItemDisplay="None" AllowFilteringByColumn="true">
                                            <CommandItemSettings ShowRefreshButton="false" ShowExportToExcelButton="false" />
                                            <Columns>
                                                <telerik:GridBoundColumn DataField="SAPSite" HeaderText="Site" UniqueName="Site">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="ProjectDescription" HeaderText="Project" UniqueName="Project">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="WBSArea" HeaderText="Area" UniqueName="Area">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="SAPNetWork" HeaderText="Network" UniqueName="Network">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="SAPActivity" HeaderText="Activities" UniqueName="Activities">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="SAPSubActivity" HeaderText="SubActivities" UniqueName="SubActivities">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="EstStartDate" HeaderText="Est. Start Date" UniqueName="EstStartDate">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="EstEndtDate" HeaderText="Est. End Date" UniqueName="EstEndDate">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="Quantity" HeaderText="Quantity" UniqueName="Quantity">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="UOM" HeaderText="UOM" UniqueName="UOM">
                                                </telerik:GridBoundColumn>

                                                <telerik:GridBoundColumn DataField="SiteFunctionalUser" HeaderText="Site Functionl User" UniqueName="SiteFunctionlUser">
                                                </telerik:GridBoundColumn>

                                                <telerik:GridBoundColumn DataField="SiteFunctionalManager" HeaderText="Site Functional Manager" UniqueName="SiteFunctionalManager">
                                                </telerik:GridBoundColumn>

                                                <telerik:GridBoundColumn DataField="SiteQualityManager" HeaderText="Site Quality Manager" UniqueName="SiteQualityManager">
                                                </telerik:GridBoundColumn>

                                                <telerik:GridBoundColumn DataField="StateProjectHead" HeaderText="State Project Head" UniqueName="StateProjectHead">
                                                </telerik:GridBoundColumn>

                                                <telerik:GridBoundColumn DataField="DesignEngineer" HeaderText="Design Engineer" UniqueName="DesignEngineer">
                                                </telerik:GridBoundColumn>

                                                <telerik:GridBoundColumn DataField="DesignManager" HeaderText="Design Manager" UniqueName="DesignManager">
                                                </telerik:GridBoundColumn>



                                                <telerik:GridTemplateColumn UniqueName="AssignedHistory" HeaderText="Assigned History">
                                                    <ItemTemplate>
                                                        <asp:HyperLink ID="ViewHistory" runat="server" Text="View"></asp:HyperLink>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn UniqueName="Timesheet" HeaderText="Timesheet">
                                                    <ItemTemplate>
                                                        <asp:HyperLink ID="ViewTimesheet" runat="server" Text="Timesheet"></asp:HyperLink>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn UniqueName="Issue" HeaderText="Issue">
                                                    <ItemTemplate>
                                                        <asp:HyperLink ID="ViewIssue" runat="server" Text="Issue"></asp:HyperLink>
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
                </div>



            </div>

            <!-- End of Tabs-->



            <!-- Row 1 Modal for Allocated Task Start Here-->

            <div class="modal fade" id="squarespaceModal" tabindex="-1" role="dialog" aria-labelledby="modalLabel" aria-hidden="true">
                <div class="modal-dialog1">
                    <div class="modal-content">
                        <div class="modal-header modal-header-resize">
                            <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">×</span><span class="sr-only">Close</span></button>
                            <h4 class="modal-title text-primary" id="lineModalLabel">Assigned History</h4>
                        </div>
                        <div class="modal-body">

                            <%--  <table class="table table-striped table-bordered">
                                    <thead>
                                        <tr>
                                            <th class="text-center">Name</th>
                                            <th class="text-center">Old Value</th>
                                            <th class="text-center">New Value</th>
                                            <th class="text-center">Modified Date (DD/MM/YYYY)</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr class="text-center">
                                            <td>Site Functional User</td>
                                            <td>Amit Kumar,Nitesh Mishra</td>
                                            <td>Amit Kumar, Pavan Dubey </td>
                                            <td>02/05/2016</td>

                                        </tr>
                                        <tr class="text-center">
                                            <td>Site Functional Manager</td>
                                            <td>Shital Mane</td>
                                            <td>Amol Firange</td>
                                            <td>02/06/2016</td>
                                        </tr>

                                    </tbody>
                                </table>--%>

                             <telerik:RadGrid RenderMode="Lightweight" ID="grdAssignHistory" runat="server"
                                    AutoGenerateColumns="false" AllowPaging="true" AllowSorting="true" AllowFilteringByColumn="false" PageSize="<%# Convert.ToInt32(SolarPMS.Models.Constants.CONST_GRID_PAGE_SIZE) %>" >
                                    <GroupingSettings CaseSensitive="false" />
                                    <MasterTableView CommandItemSettings-ShowRefreshButton="false" ClientDataKeyNames="" EditMode="InPlace" CommandItemDisplay="None" AllowFilteringByColumn="false">
                                        <CommandItemSettings ShowRefreshButton="false" ShowExportToExcelButton="false" />
                                        <Columns>
                                            <telerik:GridBoundColumn DataField="ProfileName" HeaderText="Profile Name" UniqueName="ProfileName">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="UserName" HeaderText="UserName" UniqueName="UserName">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Status" HeaderText="Status" UniqueName="Status">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="ModifyDate" HeaderText="ModifyDate" UniqueName="ModifyDate">
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

            <!-- Row 1 Modal for allocated task End Here-->




            <!--  Modal for deleted Task- view History Start Here-->

            <div class="modal fade" id="squarespaceModal2" tabindex="-1" role="dialog" aria-labelledby="modalLabel" aria-hidden="true">
                <div class="modal-dialog1">
                    <div class="modal-content">
                        <div class="modal-header modal-header-resize">
                            <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">×</span><span class="sr-only">Close</span></button>
                            <h4 class="modal-title text-primary" id="lineModalLabel1">Assigned History</h4>
                        </div>
                        <div class="modal-body">

                            <table class="table table-striped table-bordered">
                                <thead>
                                    <tr>
                                        <th class="text-center">Name</th>
                                        <th class="text-center">Old Value</th>
                                        <th class="text-center">New Value</th>
                                        <th class="text-center">Modified Date (DD/MM/YYYY)</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr class="text-center">
                                        <td>Site Functional User</td>
                                        <td>Amit Kumar,Nitesh Mishra</td>
                                        <td>Amit Kumar, Pavan Dubey </td>
                                        <td>02/05/2016</td>

                                    </tr>
                                    <tr class="text-center">
                                        <td>Site Functional Manager</td>
                                        <td>Shital Mane</td>
                                        <td>Amol Firange</td>
                                        <td>02/06/2016</td>
                                    </tr>

                                </tbody>
                            </table>


                        </div>

                    </div>
                </div>
            </div>

            <!-- Row 1 Modal for deleted Task- view History End Here-->

            <!--  Modal for deleted Task- view Timesheet Start Here-->

            <div class="modal fade" id="squarespaceModal3" tabindex="-1" role="dialog" aria-labelledby="modalLabel" aria-hidden="true">
                <div class="modal-dialog1">
                    <div class="modal-content">
                        <div class="modal-header modal-header-resize">
                            <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">×</span><span class="sr-only">Close</span></button>
                            <h4 class="modal-title text-primary" id="lineModalLabel2">View Timesheet</h4>
                        </div>
                        <div class="modal-body">

                            <table class="table table-striped table-bordered">
                                <thead>
                                    <tr>

                                        <th class="text-center">Actual Date</th>
                                        <th class="text-center">Qty</th>
                                        <th class="text-center">CRR</th>
                                        <th class="text-center">RRR</th>
                                        <th class="text-center">User</th>
                                        <th class="text-center">Status</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr class="text-center">

                                        <td><a href="#">15-04-2016</a></td>
                                        <td>10</td>
                                        <td>22.00</td>
                                        <td>20.00</td>
                                        <td>Amit Kumar</td>
                                        <td>Pushed to SAP</td>

                                    </tr>
                                    <tr class="text-center">

                                        <td><a href="#">19-05-2016</a></td>
                                        <td>10</td>
                                        <td>27.00</td>
                                        <td>27.00</td>
                                        <td>Shital Kumar</td>
                                        <td>Pending for PM Approval</td>
                                    </tr>

                                </tbody>
                            </table>


                        </div>

                    </div>
                </div>
            </div>

            <!-- Row  Modal for deleted Task- view Timesheet End Here-->


            <!--  Modal for deleted Task- view Issue Start Here-->

            <div class="modal fade" id="squarespaceModal4" tabindex="-1" role="dialog" aria-labelledby="modalLabel" aria-hidden="true">
                <div class="modal-dialog1">
                    <div class="modal-content">
                        <div class="modal-header modal-header-resize">
                            <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">×</span><span class="sr-only">Close</span></button>
                            <h4 class="modal-title text-primary" id="lineModalLabel3">View Issue</h4>
                        </div>
                        <div class="modal-body">

                            <table class="table table-striped table-bordered">
                                <thead>
                                    <tr>
                                        <th class="text-center valign">Date</th>
                                        <th class="text-center valign">Issue ID</th>
                                        <th class="text-center valign">Category</th>
                                        <th class="text-center valign">Assigned To</th>
                                        <th class="text-center valign">Expected Closure Date</th>
                                        <th class="text-center valign">Actual Close Date</th>
                                        <th class="text-center valign">Status</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr class="text-center">
                                        <td><a href="#">15-04-2016</a></td>
                                        <td>IS01005</td>
                                        <td>Electrical</td>
                                        <td>Amit Kumar</td>
                                        <td>25-05-2016</td>
                                        <td>27-05-2016</td>
                                        <td>Rework</td>

                                    </tr>
                                    <tr class="text-center">
                                        <td><a href="#">18-04-2016</a></td>
                                        <td>IS01005</td>
                                        <td>Civil</td>
                                        <td>shital Kumar</td>
                                        <td>28-06-2016</td>
                                        <td>27-06-2016</td>
                                        <td>Rework</td>
                                    </tr>

                                </tbody>
                            </table>


                        </div>

                    </div>
                </div>
            </div>

            <!-- Row  Modal for deleted Task- view Timesheet End Here-->





        </div>
        <!-- End of box row-->

            <telerik:RadNotification RenderMode="Lightweight" Position="Center" runat="server" ID="radMesaage" VisibleOnPageLoad="false"
            Width="450px" Animation="Fade" TitleIcon="none" ContentIcon="none" Style="z-index: 100000" EnableRoundedCorners="true" EnableShadow="true" AnimationDuration="1000">
        </telerik:RadNotification>
    </div>
    </div>


</asp:Content>
