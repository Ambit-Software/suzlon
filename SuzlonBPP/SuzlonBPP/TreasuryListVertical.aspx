<%@ Page Title="Cash & Bank Automation Vertical Controller" Language="C#" MasterPageFile="~/SuzlonBPP.Master" AutoEventWireup="true" CodeBehind="TreasuryListVertical.aspx.cs" Inherits="SuzlonBPP.TreasuryListVertical" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="Scripts/jquery-1.10.2.js"></script>
  <script src="Scripts/bootstrap.min.js"></script>

    <style>
        .linkButtonColor {
            color: #337ab7 !important;
        }

        .RadInput_Default .riTextBox, .RadInputMgr_Default {
            border-color: #cdcdcd;
            background: #fff;
            color: #333;
            width: 100%;
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

        .panel-default > .panel-heading2 {
            color: #333 !important;
            background-color: transparent !important;
            overflow: hidden;
            border-bottom: 1px solid #d9d9d9;
        }
        .RadPicker {
         
            display: inline-block !important;
            width: 159px !important;
        }
        .nav-tabs {
            border-bottom: none !important;
        }
    </style>

    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
            <script type="text/javascript">
                $(document).ready(function () {
                  
                    var a = document.getElementById("<%=hidTabActive.ClientID%>").value;
                    if (a == 'MyRequest')
                        $("#MyRequest").click();
                    if (a == 'MyApproveRequest')
                        $("#MyApproveRequest").click();
                    if (a == 'MyAddendumRequest')
                        $("#MyAddendumRequest").click();

                    
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
                   <telerik:AjaxSetting AjaxControlID="RadAjaxManager1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdComment" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="gridAttachment" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="radMessage" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="grdMyRequest">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdMyRequest" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="lblMyRequest"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="grdMyApprovedRequest">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdMyApprovedRequest" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="lblApprovedRequest"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="grdMyAddendum">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdMyAddendum" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="lblAddendumRequest"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>

                <telerik:AjaxSetting AjaxControlID="linkToAdd">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="linkToAdd" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>                  
                </UpdatedControls>
            </telerik:AjaxSetting>

        </AjaxSettings>
    </telerik:RadAjaxManager>

    <telerik:RadAjaxLoadingPanel ID="LoadingPanel1" BackgroundPosition="Center" runat="server" Height="100%" Width="75px" Transparency="50">
        <div class="dialog-background">
            <div class="dialog-loading-wrapper">
                <span class="dialog-loading-icon"></span>
            </div>
        </div>
    </telerik:RadAjaxLoadingPanel>

        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager" runat="server" EnableShadow="true" >
        <Windows>
            <telerik:RadWindow ID="RadWindow" RenderMode="Lightweight" runat="server" CssClass="window123" VisibleStatusbar="false"
                Width="700%" Height="550" AutoSize="false" Behaviors="Close" ShowContentDuringLoad="false" 
                                Modal="true" ReloadOnShow="true" />
        </Windows>
    </telerik:RadWindowManager>
     <asp:HiddenField ID="hidTabActive" runat="Server" />
    <div class="container-fluid padding-0">
        <div class="row margin-0">
            <div class="col-xs-6 heading-big">
                <h5 class="margin-0 lineheight-42 breath-ctrl">Home / <span>Treasury Allocation</span></h5>
            </div>
        </div>


        <div class="col-xs-12 padding-lr-10">

            <div class="col-md-12 padding-0">
                <div class="panel with-nav-tabs panel-default border-0">
                    <div class="panel-heading2 padding-0 nocolor">
                        <ul class="nav nav-tabs col-xs-11">
                            <li class="active"><a href="#tab1primary" id="MyRequest" data-toggle="tab">My Requests &nbsp;<span class="badge badge-green">
                                <asp:Label ID="lblMyRequest" runat="server" Text=""></asp:Label>
                            </span></a></li>
                            <li><a href="#tab2primary" data-toggle="tab" id="MyApproveRequest">Approved Requests &nbsp;<span class="badge badge-yellow">
                                <asp:Label ID="lblApprovedRequest" runat="server" Text=""></asp:Label>
                            </span></a></li>
                            <li><a href="#tab3primary" id="MyAddendumRequest" data-toggle="tab"> Addendum Requests&nbsp;<span class="badge badge-magenta">
                                <asp:Label ID="lblAddendumRequest" runat="server" Text=""></asp:Label>
                            </span></a></li>
                            <!--                            <li><a href="#tab4primary" data-toggle="tab">Closed Requests &nbsp;<span class="badge badge-red">12</span></a></li>-->
                        </ul>
                        <ul class="col-xs-1 pull-right nav" style="padding: 5px 0px 3px 5px;">
                            <li>
                                <asp:Button ID="linkToAdd" class="btn btn-grey" runat="server" CssClass="pull-right btn btn-grey button button-style" OnClick="linkToAdd_Click" Text="Add"></asp:Button></li>
                        </ul>
                    </div>
                    <div class="clearfix"></div>
                    <div class="panel-body overflow-x">
                        <div class="tab-content">
                            <div class="tab-pane fade in active" id="tab1primary">
                                <telerik:RadGrid RenderMode="Lightweight" ID="grdMyRequest" runat="server" ClientIDMode="AutoID" OnNeedDataSource="grdMyRequest_NeedDataSource"
                                OnItemDataBound="grdMyRequest_ItemDataBound"    AutoGenerateColumns="false" AllowPaging="true" AllowSorting="true" OnItemCreated="grdMyRequest_ItemCreated" OnItemCommand="grdMyRequest_ItemCommand" AllowFilteringByColumn="false" PageSize="<%# Convert.ToInt32(SuzlonBPP.Models.Constants.CONST_GRID_PAGE_SIZE) %>">
                                    <GroupingSettings CaseSensitive="false" />
                                    <MasterTableView CommandItemSettings-ShowRefreshButton="false" ClientDataKeyNames="TreasuryDetailId,Status" EditMode="InPlace" CommandItemDisplay="None" AllowFilteringByColumn="true" ClientIDMode="AutoID">
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
                                            <telerik:GridDateTimeColumn DataField="Date" HeaderText="Date" EnableTimeIndependentFiltering="true" DataFormatString="{0:dd/MM/yyyy}">
                                            </telerik:GridDateTimeColumn>
                                            <telerik:GridBoundColumn DataField="CompanyCode" HeaderText="Company Code" UniqueName="CompanyCode">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Vertical" HeaderText="Vertical" UniqueName="Vertical">
                                            </telerik:GridBoundColumn>
                                              <telerik:GridBoundColumn DataField="SubVertical" HeaderText="Sub-Vertical" UniqueName="SubVertical">
                                            </telerik:GridBoundColumn>
                                            <%-- <telerik:GridBoundColumn DataField="AllocationNumber" HeaderText="Treasory Allocation No." UniqueName="TreasoryAllocationNo">
                                            </telerik:GridBoundColumn>--%>
                                            <telerik:GridTemplateColumn HeaderText="Treasury Allocation Number" UniqueName="TreasoryAllocationNo" AllowSorting="true" AllowFiltering="true" DataType="System.String" DataField="AllocationNumber" SortExpression="AllocationNumber">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="treasoryAllocationNo" CommandName="Redirect" runat="server" CssClass="linkButtonColor"></asp:LinkButton>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridBoundColumn DataField="NatureOfReqest" HeaderText="Nature of Request" UniqueName="NatureOfRequest">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="RequestedAmount" ItemStyle-HorizontalAlign="Right" HeaderText="Requested Amount" UniqueName="RequestedAmount" > <%--DataFormatString="{0:###,##0.00}"--%>
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="InitApprovedAmount" ItemStyle-HorizontalAlign="Right" HeaderText="Amount Approved" UniqueName="AmountApproved" DataFormatString="{0:###,##0.00}">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Status" HeaderText="Status" UniqueName="Status">
                                            </telerik:GridBoundColumn>
                                              <telerik:GridTemplateColumn HeaderText="Attachments" AllowFiltering="false" AllowSorting="false" UniqueName="Attachments">
                                                <ItemTemplate>
                                                    <telerik:RadButton ID="btnAttachment" runat="server" CommandName="Attachment" CommandArgument='<%# Eval("TreasuryDetailId") %>' Text="Add/View" CssClass=" gridHyperlinks"></telerik:RadButton>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>

                                            <telerik:GridTemplateColumn UniqueName="TemplateComment" HeaderText="Comment" AllowFiltering="false">
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
                            <div class="tab-pane fade" id="tab2primary">

                                <telerik:RadGrid RenderMode="Lightweight" ID="grdMyApprovedRequest" runat="server" ClientIDMode="AutoID" OnNeedDataSource="grdMyApprovedRequest_NeedDataSource"
                                    AutoGenerateColumns="false" AllowPaging="true" AllowSorting="true" AllowFilteringByColumn="false" PageSize="<%# Convert.ToInt32(SuzlonBPP.Models.Constants.CONST_GRID_PAGE_SIZE) %>"
                                  OnItemDataBound="grdMyApprovedRequest_ItemDataBound"  OnItemCommand="grdMyApprovedRequest_ItemCommand" OnItemCreated="grdMyApprovedRequest_ItemCreated">
                                    <GroupingSettings CaseSensitive="false" />
                                    <MasterTableView CommandItemSettings-ShowRefreshButton="false" ClientDataKeyNames="TreasuryDetailId" EditMode="InPlace" CommandItemDisplay="None" AllowFilteringByColumn="true" ClientIDMode="AutoID">
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
                                            <telerik:GridDateTimeColumn DataField="Date" HeaderText="Date" EnableTimeIndependentFiltering="true" DataFormatString="{0:dd/MM/yyyy}">
                                                <HeaderStyle Width="130px" />
                                            </telerik:GridDateTimeColumn>
                                            <telerik:GridBoundColumn DataField="CompanyCode" HeaderText="Company Code" UniqueName="CompanyCode">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Vertical" HeaderText="Vertical" UniqueName="Vertical">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="SubVertical" HeaderText="Sub-Vertical" UniqueName="SubVertical">
                                            </telerik:GridBoundColumn>

                                            <%--<telerik:GridBoundColumn DataField="TreasoryAllocationNo" HeaderText="Treasory Allocation No." UniqueName="TreasoryAllocationNo">
                                            </telerik:GridBoundColumn>--%>
                                            <telerik:GridTemplateColumn UniqueName="TreasoryAllocationNo" HeaderText="Treasury Allocation Number" AllowSorting="true" AllowFiltering="true" DataType="System.String" DataField="AllocationNumber" SortExpression="AllocationNumber">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="treasoryAllocationNo" CommandName="Redirect" CssClass="linkButtonColor" runat="server"></asp:LinkButton>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridBoundColumn DataField="NatureOfReqest" HeaderText="Nature of Request" UniqueName="NatureOfRequest">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="RequestedAmount" ItemStyle-HorizontalAlign="Right" HeaderText="Requested Amount" UniqueName="RequestedAmount" DataFormatString="{0:###,##0.00}">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="InitApprovedAmount" ItemStyle-HorizontalAlign="Right" HeaderText="Amount Approved" UniqueName="AmountApproved" DataFormatString="{0:###,##0.00}">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Status" HeaderText="Status" UniqueName="Status">
                                            </telerik:GridBoundColumn>
                                                      <telerik:GridTemplateColumn HeaderText="Attachments" AllowFiltering="false" AllowSorting="false" UniqueName="Attachments">
                                                <ItemTemplate>
                                                    <telerik:RadButton ID="btnAttachment" runat="server" CommandName="Attachment" CommandArgument='<%# Eval("TreasuryDetailId") %>' Text="Add/View" CssClass="gridHyperlinks"></telerik:RadButton>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>

                                            <telerik:GridTemplateColumn UniqueName="TemplateComment" HeaderText="Comment" AllowFiltering="false">
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
                            <div class="tab-pane fade" id="tab3primary">

                                <telerik:RadGrid RenderMode="Lightweight" ID="grdMyAddendum" runat="server" ClientIDMode="AutoID"
                                    AutoGenerateColumns="false" AllowPaging="true" AllowSorting="true" AllowFilteringByColumn="false" PageSize="<%# Convert.ToInt32(SuzlonBPP.Models.Constants.CONST_GRID_PAGE_SIZE) %>"
                                    OnNeedDataSource="grdMyAddendum_NeedDataSource" OnItemDataBound="grdMyAddendum_ItemDataBound" OnItemCreated="grdMyAddendum_ItemCreated" OnItemCommand="grdMyAddendum_ItemCommand">
                                    <GroupingSettings CaseSensitive="false" />
                                    <MasterTableView CommandItemSettings-ShowRefreshButton="false" ClientDataKeyNames="AddandomDetailId,TreasuryDetailId" EditMode="InPlace" CommandItemDisplay="None" AllowFilteringByColumn="true" ClientIDMode="AutoID">
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
                                            <telerik:GridDateTimeColumn DataField="Date" HeaderText="Date" EnableTimeIndependentFiltering="true" DataFormatString="{0:dd/MM/yyyy}">
                                                <HeaderStyle Width="130px" />
                                            </telerik:GridDateTimeColumn>
                                            <telerik:GridBoundColumn DataField="CompanyCode" HeaderText="Company Code" UniqueName="CompanyCode">
                                            </telerik:GridBoundColumn>
                                              <telerik:GridBoundColumn DataField="Vertical" HeaderText="Vertical" UniqueName="Vertical">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="SubVertical" HeaderText="Sub-Vertical" UniqueName="SubVertical">
                                            </telerik:GridBoundColumn>
                                            <%--<telerik:GridBoundColumn DataField="TreasoryAllocationNo" HeaderText="Treasory Allocation No." UniqueName="TreasoryAllocationNo">
                                            </telerik:GridBoundColumn>--%>
                                            <%--<telerik:GridTemplateColumn UniqueName="TreasoryAllocationNo" HeaderText="Treasury Allocation Number">--%>
                                            <telerik:GridTemplateColumn UniqueName="TreasoryAllocationNo" HeaderText="Treasury Allocation Number" AllowSorting="true" AllowFiltering="true" DataType="System.String" DataField="AllocationNumber" SortExpression="AllocationNumber">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="treasoryAllocationNo" CommandName="Redirect" CssClass="linkButtonColor" runat="server"></asp:LinkButton>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridBoundColumn DataField="Name" HeaderText="Nature of Request" UniqueName="NatureOfRequest">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Amount" ItemStyle-HorizontalAlign="Right" HeaderText="Addendum Amount" UniqueName="RequestedAmount" DataFormatString="{0:###,##0.00}">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="ApprovedAmount" ItemStyle-HorizontalAlign="Right" HeaderText="Addendum Approved Amount" UniqueName="AmountApproved" DataFormatString="{0:###,##0.00}">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Status" HeaderText="Addendum Status" UniqueName="Status">
                                            </telerik:GridBoundColumn>
                                                      <telerik:GridTemplateColumn HeaderText="Attachments" AllowFiltering="false" AllowSorting="false" UniqueName="Attachments" Visible="false">
                                                <ItemTemplate>
                                                    <telerik:RadButton ID="btnAttachment" runat="server" CommandName="Attachment" CommandArgument='<%# Eval("TreasuryDetailId") %>' Text="Add/View" CssClass="gridHyperlinks"></telerik:RadButton>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>

                                            <telerik:GridTemplateColumn UniqueName="TemplateComment" HeaderText="Comment" AllowFiltering="false" Visible="false">
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
                                            <td align="center">No records to display.
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
        <!-- End of grid-->
        <telerik:RadNotification RenderMode="Lightweight" Position="Center" runat="server" ID="radMessage" VisibleOnPageLoad="false"
            Width="350px" AutoCloseDelay="0" TitleIcon="none" ContentIcon="none">
        </telerik:RadNotification>
    </div>
    <!-- End of container-fluid-->

    <!-- jQuery (necessary for Bootstrap's JavaScript plugins) -->
   <%-- <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js"></script>--%>
    <!-- Include all compiled plugins (below), or include individual files as needed -->
    <script src="js/bootstrap.min.js"></script>
     <%-- <script src="Scripts/bootstrap.min.js"></script>--%>
    

</asp:Content>
