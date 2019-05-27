<%@ Page Title="" Language="C#" MasterPageFile="~/SuzlonBPP.Master" AutoEventWireup="true" CodeBehind="TreasuryList.aspx.cs" Inherits="SuzlonBPP.TreasuryList" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script src="Scripts/bootstrap.min.js"></script>
    <style>
        /*.RadGrid_Default .rgRow a, .RadGrid_Default .rgAltRow a, .RadGrid_Default .rgFooter a, .RadGrid_Default .rgEditForm a, .RadGrid_Default .rgEditRow a, .RadGrid_Default .rgHoveredRow a {
            color: #0003ff !important;
            text-decoration: underline !important;
            }*/

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

        .nav > li > a > div {
            display: inline-block !important;
        }
    </style>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript">

            $(document).ready(function () {

                var a = document.getElementById("<%=ProfileName.ClientID%>").value;
                 if (a == 'CB') {
                     $("#lnkPendingReq").hide();
                     $("#lnkPendingAddendumReq").hide();
                     $("#lnkApproveReq").hide();
                     $("#lnkPendingReqCB").show();
                     $("#lnkPendingReqCB").click();
                  <%--  var tabPending = document.getElementById("<%=lnkPendingReqCB.ClientID%>");
                    tabPending.click();--%>
                 }
                 else {

                     $("#lnkPendingReq").show();
                     $("#lnkPendingAddendumReq").show();
                     $("#lnkApproveReq").show();
                     $("#lnkPendingReqCB").hide();


                 }

                  var a = document.getElementById("<%=hidTabActive.ClientID%>").value;
                if (a == 'PendingRequest') 
                    $("#lnkPendingReq").click();                   
                if(a=='PendingAddendumRequest') 
                    $("#lnkPendingAddendumReq").click();
                if(a=='ApprovedRequest')
                    $("#lnkApproveReq").click();
                if(a=='PendingRequestCB')
                    $("#lnkPendingReqCB").click();


             });


             function ShowComments(id) {
                 var ajaxManager = $find("<%= RadAjaxManager1.ClientID %>");
                ajaxManager.ajaxRequest("Comment#" + id + "");
                $('#squarespaceCommentModal').modal();
            }

            function openRadWin(screen, mode, entityId, canAdd, canDelete, isMultiFileUpload, showDocumentType, entityName) {
                debugger;
                var manager = $find("<%= RadWindowManager.ClientID %>");
               // if (screen == 'VC') {
                manager.open("AddAttachments.aspx?mode=" + mode + "&entityId=" + entityId + "&canAdd= " + canAdd + "&canDelete= " + canDelete + "&isMultiUpload= " + isMultiFileUpload + "&showDtype= " + showDocumentType + "&entityName=" + entityName, "RadWindow");
              
                return false;
               // }
            }
        </script>
    </telerik:RadCodeBlock>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" OnAjaxRequest="RadAjaxManager1_AjaxRequest">
        <AjaxSettings>
            <%--<telerik:AjaxSetting AjaxControlID="grdPendingRequest">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdPendingRequest" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="lblApprovePendingCount"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>--%>

            <telerik:AjaxSetting AjaxControlID="RadAjaxManager1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdComment" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="gridAttachment" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="radMessage" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="grdPendingAddendumRequest">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdPendingAddendumRequest" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="lblApprovePendingAdddendumCount"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="grdRequestToCB">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdRequestToCB" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="lblCBApproveRequestCount"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="grdApprovedRequestByMe">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdApprovedRequestByMe" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="lblApproveRequestBymeCount"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="grdPendingRequest">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdPendingRequest" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="lblApprovePendingCount"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>

        </AjaxSettings>
    </telerik:RadAjaxManager>
      <asp:HiddenField ID="hidTabActive" runat="Server" />

    <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager" runat="server" EnableShadow="true">
        <Windows>
            <telerik:RadWindow ID="RadWindow" RenderMode="Lightweight" runat="server" CssClass="window123" VisibleStatusbar="false"
                Width="700%" Height="550" AutoSize="false" Behaviors="Close" ShowContentDuringLoad="false"
                Modal="true" ReloadOnShow="true" />
        </Windows>
    </telerik:RadWindowManager>
    <telerik:RadAjaxLoadingPanel ID="LoadingPanel1" BackgroundPosition="Center" runat="server" Height="100%" Width="75px" Transparency="50">
        <div class="dialog-background">
            <div class="dialog-loading-wrapper">
                <span class="dialog-loading-icon"></span>
            </div>
        </div>
    </telerik:RadAjaxLoadingPanel>
    <asp:HiddenField ID="ProfileName" runat="Server" />
    <div class="container-fluid padding-0">

        <div class="row margin-0">
            <div class="col-xs-6 heading-big">
                <h5 class="margin-0 lineheight-42 breath-ctrl">Home / <span>Treasury Allocation</span></h5>
            </div>

        </div>
        <!-- End of heading wrapper-->


        <div class="col-xs-12">

            <div class="col-md-12 padding-0">
                <div class="panel with-nav-tabs panel-default border-0">
                    <div class="panel-heading padding-0 nocolor">
                        <ul class="nav nav-tabs border-0">
                            <li class="active"><a href="#tab1primary" data-toggle="tab" id="lnkPendingReq">Request For Approval &nbsp;<span class="badge badge-green display-inline" id="lblApprovePendingCount" runat="server" style="display: inline-block !important;">0</span></a></li>
                            <li><a href="#tab2primary" data-toggle="tab" id="lnkPendingAddendumReq">Addendum Requests &nbsp;<span class="badge badge-yellow" id="lblApprovePendingAdddendumCount" runat="server">0</span></a></li>
                            <li><a href="#tab3primary" data-toggle="tab" id="lnkApproveReq">Approved Requests &nbsp;<span class="badge badge-magenta" id="lblApproveRequestBymeCount" runat="server">0</span></a></li>
                            <li><a href="#tab4primary" data-toggle="tab" id="lnkPendingReqCB">Approved Requests &nbsp;<span class="badge badge-danger" id="lblCBApproveRequestCount" runat="server">0</span></a></li>
                        </ul>
                    </div>
                    <div class="panel-body">
                        <div class="tab-content">
                            <div class="tab-pane fade in active overflow-x" id="tab1primary">

                                <telerik:RadGrid RenderMode="Lightweight" ID="grdPendingRequest" runat="server" ClientIDMode="AutoID" OnNeedDataSource="grdPendingRequest_NeedDataSource"
                                    OnItemCreated="grdPendingRequest_ItemCreated" OnItemCommand="grdPendingRequest_ItemCommand" OnItemDataBound="grdPendingRequest_ItemDataBound"
                                    AutoGenerateColumns="false" AllowPaging="true" AllowSorting="true" AllowFilteringByColumn="false" PageSize="<%# Convert.ToInt32(SuzlonBPP.Models.Constants.CONST_GRID_PAGE_SIZE) %>">
                                    <GroupingSettings CaseSensitive="false" />
                                    <MasterTableView CommandItemSettings-ShowRefreshButton="false" ClientDataKeyNames="TreasuryDetailId" EditMode="InPlace" CommandItemDisplay="None" AllowFilteringByColumn="true" ClientIDMode="AutoID">
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
                                            <telerik:GridDateTimeColumn FilterControlWidth="300px" DataField="Date" HeaderText="Date" EnableTimeIndependentFiltering="true" DataFormatString="{0:dd/MM/yyyy}">
                                            </telerik:GridDateTimeColumn>
                                            <telerik:GridBoundColumn DataField="CompanyCode" HeaderText="Company Code" UniqueName="CompanyCode">
                                            </telerik:GridBoundColumn>
                                             <telerik:GridBoundColumn DataField="Vertical" HeaderText="Vertical" UniqueName="Vertical">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="SubVertical" HeaderText="Sub-Vertical" UniqueName="SubVertical">
                                            </telerik:GridBoundColumn>
                                            <%-- <telerik:GridTemplateColumn HeaderText="Treasury Allocation Number" UniqueName="TreasoryAllocationNo">--%>
                                            <telerik:GridTemplateColumn UniqueName="TreasoryAllocationNo" HeaderText="Treasury Allocation Number" AllowSorting="true" AllowFiltering="true" DataType="System.String" DataField="AllocationNumber" SortExpression="AllocationNumber">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="treasoryAllocationNo" CommandName="Redirect" runat="server" CssClass="linkButtonColor"></asp:LinkButton>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>

                                            <telerik:GridBoundColumn DataField="NatureOfReqest" HeaderText="Nature of Request" UniqueName="NatureOfRequest">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="RequestedAmount" HeaderText="Requested Amount" ItemStyle-HorizontalAlign="Right" UniqueName="RequestedAmount" DataFormatString="{0:#,###}">
                                            </telerik:GridBoundColumn>
                                        
                                            <telerik:GridDateTimeColumn DataField="ProposedDate" HeaderText="Proposed Payment Date" EnableTimeIndependentFiltering="true" DataFormatString="{0:dd/MM/yyyy}">
                                                <HeaderStyle Width="130px" />
                                            </telerik:GridDateTimeColumn>
                                            <telerik:GridDateTimeColumn DataField="3" HeaderText="Utilisation From Date" EnableTimeIndependentFiltering="true" Visible="false" >
                                                <HeaderStyle Width="200" />
                                            </telerik:GridDateTimeColumn>
                                            <telerik:GridDateTimeColumn DataField="3 " HeaderText="Utilisation To Date" EnableTimeIndependentFiltering="true" Visible="false" >
                                                <HeaderStyle Width="130px" />
                                            </telerik:GridDateTimeColumn>                                          


                                            <telerik:GridBoundColumn DataField="Status" HeaderText="Status" UniqueName="Status">
                                            </telerik:GridBoundColumn>
                                             <telerik:GridTemplateColumn HeaderText="Attachments" AllowFiltering="false" AllowSorting="false" UniqueName="Attachments">
                                                <ItemTemplate>
                                                    <telerik:RadButton ID="btnAttachment" runat="server" CommandName="Attachment" CommandArgument='<%# Eval("TreasuryDetailId") %>' Text="Add/View" CssClass="gridHyperlinks"></telerik:RadButton>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>

                                            <telerik:GridTemplateColumn UniqueName="TemplateComment" HeaderText="Comment" AllowFiltering="false" AllowSorting="false">
                                                <ItemTemplate>
                                                    <asp:HyperLink ID="viewComment" runat="server" Text="View" CssClass="btn btn-grey gridHyperlinks"></asp:HyperLink>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>

                                        </Columns>
                                        <PagerStyle PageSizes="5,10,15" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                                            PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />
                                    </MasterTableView>
                                </telerik:RadGrid>

                            </div>
                            <div class="tab-pane fade overflow-x" id="tab2primary">
                                <telerik:RadGrid RenderMode="Lightweight" ID="grdPendingAddendumRequest" runat="server" ClientIDMode="AutoID"
                                    OnNeedDataSource="grdPendingAddendumRequest_NeedDataSource"
                                    OnItemCreated="grdPendingAddendumRequest_ItemCreated" OnItemCommand="grdPendingAddendumRequest_ItemCommand" OnItemDataBound="grdPendingAddendumRequest_ItemDataBound"
                                    AutoGenerateColumns="false" AllowPaging="true" AllowSorting="true" AllowFilteringByColumn="false" PageSize="<%# Convert.ToInt32(SuzlonBPP.Models.Constants.CONST_GRID_PAGE_SIZE) %>">
                                    <GroupingSettings CaseSensitive="false" />
                                    <MasterTableView CommandItemSettings-ShowRefreshButton="false" ClientDataKeyNames="TreasuryDetailId" EditMode="InPlace" CommandItemDisplay="None" AllowFilteringByColumn="true" ClientIDMode="AutoID">
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

                                            <%--<telerik:GridTemplateColumn HeaderText="Treasury Allocation Number" UniqueName="TreasoryAllocationNo">--%>
                                        
                                            <telerik:GridDateTimeColumn DataField="Date" HeaderText="Date" EnableTimeIndependentFiltering="true" DataFormatString="{0:dd/MM/yyyy}">
                                                <HeaderStyle Width="200px" />
                                            </telerik:GridDateTimeColumn>
                                                <telerik:GridBoundColumn DataField="Vertical" HeaderText="Vertical" UniqueName="Vertical">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="SubVertical" HeaderText="Sub-Vertical" UniqueName="SubVertical">
                                            </telerik:GridBoundColumn>
                                                <telerik:GridTemplateColumn UniqueName="TreasoryAllocationNo" HeaderText="Treasury Allocation Number" AllowSorting="true" AllowFiltering="true" DataType="System.String" DataField="AllocationNumber" SortExpression="AllocationNumber">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="treasoryAllocationNoAddendum" CommandName="Redirect" runat="server" CssClass="linkButtonColor"></asp:LinkButton>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridBoundColumn DataField="NatureOfRequest" HeaderText="Nature of Request" UniqueName="NatureOfRequest">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Amount" HeaderText="Addendum Amount" ItemStyle-HorizontalAlign="Right" UniqueName="RequestedAmount" DataFormatString="{0:#,###}">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="ApprovedAmount" HeaderText="Addendum Approved Amount" ItemStyle-HorizontalAlign="Right" UniqueName="ApprovedAmount" DataFormatString="{0:#,###}">
                                            </telerik:GridBoundColumn>
                                  
                                            <telerik:GridBoundColumn DataField="Status" HeaderText="Addendum  Status" UniqueName="Status" AllowFiltering="false">
                                            </telerik:GridBoundColumn>
                                                      <telerik:GridTemplateColumn HeaderText="Attachments" AllowFiltering="false" AllowSorting="false" UniqueName="Attachments" Visible="false">
                                                <ItemTemplate>
                                                    <telerik:RadButton ID="btnAttachment" runat="server" CommandName="Attachment" CommandArgument='<%# Eval("TreasuryDetailId") %>' Text="Add/View" CssClass="gridHyperlinks"></telerik:RadButton>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn UniqueName="TemplateComment" HeaderText="Comment" AllowFiltering="false" AllowSorting="false" Visible="false" >
                                                <ItemTemplate>
                                                    <asp:HyperLink ID="viewCommentAddendum" runat="server" Text="View" CssClass="btn btn-grey gridHyperlinks"></asp:HyperLink>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>

                                            
                                        </Columns>
                                        <PagerStyle PageSizes="5,10,15" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                                            PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />
                                    </MasterTableView>

                                </telerik:RadGrid>


                            </div>
                            <div class="tab-pane fade overflow-x" id="tab3primary">

                                <telerik:RadGrid RenderMode="Lightweight" ID="grdApprovedRequestByMe" runat="server" ClientIDMode="AutoID" OnNeedDataSource="grdApprovedRequestByMe_NeedDataSource"
                                    OnItemCreated="grdApprovedRequestByMe_ItemCreated" OnItemCommand="grdApprovedRequestByMe_ItemCommand" OnItemDataBound="grdApprovedRequestByMe_ItemDataBound"
                                    AutoGenerateColumns="false" AllowPaging="true" AllowSorting="true" AllowFilteringByColumn="false" PageSize="<%# Convert.ToInt32(SuzlonBPP.Models.Constants.CONST_GRID_PAGE_SIZE) %>">
                                    <GroupingSettings CaseSensitive="false" />
                                    <MasterTableView CommandItemSettings-ShowRefreshButton="false" ClientDataKeyNames="TreasuryDetailId" EditMode="InPlace" CommandItemDisplay="None" AllowFilteringByColumn="true" ClientIDMode="AutoID">
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
                                            <telerik:GridDateTimeColumn DataField="Date" HeaderText="Date" EnableTimeIndependentFiltering="true" DataFormatString="{0:dd/MM/yyyy}">
                                                <HeaderStyle Width="500px" />
                                            </telerik:GridDateTimeColumn>
                                            <telerik:GridBoundColumn DataField="CompanyCode" HeaderText="Company Code" UniqueName="CompanyCode">
                                            </telerik:GridBoundColumn>
                                              <telerik:GridBoundColumn DataField="Vertical" HeaderText="Vertical" UniqueName="Vertical">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="SubVertical" HeaderText="Sub-Vertical" UniqueName="SubVertical">
                                            </telerik:GridBoundColumn>
                                            <%--  <telerik:GridTemplateColumn HeaderText="Treasury Allocation Number" UniqueName="TreasoryAllocationNo">--%>
                                            <telerik:GridTemplateColumn UniqueName="TreasoryAllocationNo" HeaderText="Treasury Allocation Number" AllowSorting="true" AllowFiltering="true" DataType="System.String" DataField="AllocationNumber" SortExpression="AllocationNumber">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="treasoryAllocationNo" CommandName="Redirect" runat="server" CssClass="linkButtonColor"></asp:LinkButton>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>

                                            <telerik:GridBoundColumn DataField="NatureOfReqest" HeaderText="Nature of Request" UniqueName="NatureOfRequest">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="RequestedAmount" HeaderText="Requested Amount" ItemStyle-HorizontalAlign="Right" UniqueName="RequestedAmount" DataFormatString="{0:#,###}">
                                            </telerik:GridBoundColumn>
                                              <telerik:GridBoundColumn DataField="InitApprovedAmount" HeaderText="Amount Approved"  ItemStyle-HorizontalAlign="Right" UniqueName="AmountApproved" DataFormatString="{0:#,###}">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridDateTimeColumn DataField="ProposedDate" HeaderText="Proposed Date" EnableTimeIndependentFiltering="true" DataFormatString="{0:dd/MM/yyyy}">
                                                <HeaderStyle Width="130px" />
                                            </telerik:GridDateTimeColumn>
                                            <telerik:GridDateTimeColumn DataField="UtilsationStartDate" HeaderText="Utilisation FromDate" EnableTimeIndependentFiltering="true" DataFormatString="{0:dd/MM/yyyy}">
                                                <HeaderStyle Width="130px" />
                                            </telerik:GridDateTimeColumn>
                                            <telerik:GridDateTimeColumn DataField="UtilsationEndDate" HeaderText="Utilisation ToDate" EnableTimeIndependentFiltering="true" DataFormatString="{0:dd/MM/yyyy}">
                                                <HeaderStyle Width="130px" />
                                            </telerik:GridDateTimeColumn>
                                            <telerik:GridBoundColumn DataField="Status" HeaderText="Status" UniqueName="Status">
                                            </telerik:GridBoundColumn>

                                            
                                             <telerik:GridTemplateColumn HeaderText="Attachments" AllowFiltering="false" AllowSorting="false" UniqueName="Attachments">
                                                <ItemTemplate>
                                                    <telerik:RadButton ID="btnAttachment" runat="server" CommandName="Attachment" CommandArgument='<%# Eval("TreasuryDetailId") %>' Text="Add/View" CssClass="gridHyperlinks"></telerik:RadButton>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>

                                            <telerik:GridTemplateColumn UniqueName="TemplateComment" HeaderText="Comment" HeaderStyle-Width="5%" AllowFiltering="false" AllowSorting="false">
                                                <ItemTemplate>
                                                    <asp:HyperLink ID="viewComment" runat="server" Text="View" CssClass="btn btn-grey gridHyperlinks"></asp:HyperLink>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                        </Columns>
                                        <PagerStyle PageSizes="5,10,15" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                                            PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />
                                    </MasterTableView>
                                </telerik:RadGrid>

                            </div>
                            <div class="tab-pane fade overflow-x" id="tab4primary">

                                <telerik:RadGrid RenderMode="Lightweight" ID="grdRequestToCB" runat="server" ClientIDMode="AutoID" OnNeedDataSource="grdRequestToCB_NeedDataSource"
                                    OnItemCreated="grdRequestToCB_ItemCreated" OnItemCommand="grdRequestToCB_ItemCommand" OnItemDataBound="grdRequestToCB_ItemDataBound"
                                    AutoGenerateColumns="false" AllowPaging="true" AllowSorting="true" AllowFilteringByColumn="false" PageSize="<%# Convert.ToInt32(SuzlonBPP.Models.Constants.CONST_GRID_PAGE_SIZE) %>">
                                    <GroupingSettings CaseSensitive="false" />
                                    <MasterTableView CommandItemSettings-ShowRefreshButton="false" ClientDataKeyNames="TreasuryDetailId" EditMode="InPlace" CommandItemDisplay="None" AllowFilteringByColumn="true" ClientIDMode="AutoID">
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
                                            <telerik:GridDateTimeColumn DataField="Date" HeaderText="Date" EnableTimeIndependentFiltering="true" DataFormatString="{0:dd/MM/yyyy}">
                                                <HeaderStyle Width="130px" />
                                            </telerik:GridDateTimeColumn>
                                            <telerik:GridBoundColumn DataField="CompanyCode" HeaderText="Company Code" UniqueName="CompanyCode">
                                            </telerik:GridBoundColumn>
                                              <telerik:GridBoundColumn DataField="Vertical" HeaderText="Vertical" UniqueName="Vertical">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="subVertical" HeaderText="Sub-Vertical" UniqueName="SubVertical">
                                            </telerik:GridBoundColumn>
                                            <%-- <telerik:GridTemplateColumn HeaderText="Treasury Allocation Number" UniqueName="TreasoryAllocationNo">--%>
                                            <telerik:GridTemplateColumn UniqueName="TreasoryAllocationNo" HeaderText="Treasury Allocation Number" AllowSorting="true" AllowFiltering="true" DataType="System.String" DataField="AllocationNumber" SortExpression="AllocationNumber">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="treasoryAllocationNo" CommandName="Redirect" runat="server" CssClass="linkButtonColor"></asp:LinkButton>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>

                                            <telerik:GridBoundColumn DataField="NatureOfReqest" HeaderText="Nature of Request" UniqueName="NatureOfRequest">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="RequestedAmount" HeaderText="Requested Amount" ItemStyle-HorizontalAlign="Right" UniqueName="RequestedAmount" DataFormatString="{0:#,###}">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="InitApprovedAmount" HeaderText="Amount Approved" ItemStyle-HorizontalAlign="Right" UniqueName="ApprovedAmount" DataFormatString="{0:#,###}">
                                            </telerik:GridBoundColumn>
                                            <%--    <telerik:GridDateTimeColumn DataField="ProposedDate" HeaderText="Proposed Date" EnableTimeIndependentFiltering="true" DataFormatString="{0:MM/dd/yyyy}">
                                        <HeaderStyle Width="130px" />
                                    </telerik:GridDateTimeColumn>
                                    <telerik:GridDateTimeColumn DataField="UtilsationStartDate" HeaderText="Utilization FromDate" EnableTimeIndependentFiltering="true" DataFormatString="{0:MM/dd/yyyy}">
                                        <HeaderStyle Width="130px" />
                                    </telerik:GridDateTimeColumn>
                                    <telerik:GridDateTimeColumn DataField="UtilsationEndDate" HeaderText="Utilization ToDate" EnableTimeIndependentFiltering="true" DataFormatString="{0:MM/dd/yyyy}">
                                        <HeaderStyle Width="130px" />
                                    </telerik:GridDateTimeColumn>--%>

                                             <telerik:GridTemplateColumn HeaderText="Attachments" AllowFiltering="false" AllowSorting="false" UniqueName="Attachments">
                                                <ItemTemplate>
                                                    <telerik:RadButton ID="btnAttachment" runat="server" CommandName="Attachment" CommandArgument='<%# Eval("TreasuryDetailId") %>' Text="Add/View" CssClass="gridHyperlinks"></telerik:RadButton>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>

                                            <telerik:GridTemplateColumn UniqueName="TemplateComment" HeaderText="Comment" HeaderStyle-Width="5%" AllowFiltering="false" AllowSorting="false" Visible="false">
                                                <ItemTemplate>
                                                    <asp:HyperLink ID="viewComment" runat="server" Text="View" CssClass="btn btn-grey gridHyperlinks"></asp:HyperLink>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>


                                            <telerik:GridBoundColumn DataField="Status" HeaderText="Status" UniqueName="Status">
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
                                    <telerik:GridBoundColumn DataField="Name" HeaderText="Comment By" UniqueName="CommentBy" HeaderStyle-Width="30%">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Comment" HeaderText="Comment" UniqueName="Comment" HeaderStyle-Width="70%">
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

        <%--        <div class="modal fade" id="squarespaceAttachmentModal" tabindex="-1" role="dialog" aria-labelledby="modalLabel" aria-hidden="true">
            <div class="modal-dialog1">
                <div class="modal-content">
                    <div class="modal-header modal-header-resize">
                        <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">×</span><span class="sr-only">Close</span></button>
                        <h4 class="modal-title text-primary" id="lineModalAttachmentLabel">Attachment</h4>
                    </div>
                    <div class="modal-body">

                        <telerik:RadGrid RenderMode="Lightweight" ID="gridAttachment" runat="server" OnItemDataBound="gridAttachment_ItemDataBound"
                            OnNeedDataSource="gridAttachment_NeedDataSource" AutoGenerateColumns="false" AllowPaging="true" AllowSorting="true" AllowFilteringByColumn="false"
                            PageSize="10">
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
                                    <telerik:GridHyperLinkColumn DataTextField="Attachment1"  DataNavigateUrlFields="FilePath" HeaderText="File Name" UniqueName="FileName">
                                        <ItemStyle CssClass="linkButtonColor"/>
                                    </telerik:GridHyperLinkColumn>
                                    <telerik:GridBoundColumn DataField="Name" HeaderText="CreatedBy" UniqueName="Date" DataFormatString="{0:dd-MMM-yyyy}">
                                    </telerik:GridBoundColumn>
                                </Columns>
                                <PagerStyle PageSizes="5,10,15" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                                    PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />
                            </MasterTableView>
                        </telerik:RadGrid>

                    </div>
                </div>
            </div>
        </div>--%>



        <!-- End of grid-->
        <telerik:RadNotification RenderMode="Lightweight" Position="Center" runat="server" ID="radMessage" VisibleOnPageLoad="false"
            Width="350px" AutoCloseDelay="0" TitleIcon="none" ContentIcon="none">
        </telerik:RadNotification>
    </div>

</asp:Content>
