<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="SuzlonBPP.Master" CodeBehind="VendorBankMaster.aspx.cs" Inherits="SuzlonBPP.VendorBankMaster" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="Scripts/bootstrap.min.js"></script>
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="grdVendor">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdVendor" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="radMessage" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="LoadingPanel1" runat="server" Skin="Default">
    </telerik:RadAjaxLoadingPanel>
    <div class="row margin-0">
        <div class="col-xs-6 heading-big">
            <h5 class="margin-0 lineheight-42 breath-ctrl">Home / <span>Vendor Bank</span></h5>
        </div>
        <div class="col-sm-6 col-xs-6 ">
            
        </div>
    </div>

    
    <div class="clearfix"></div>

    <div class="col-xs-12 overflow-h padding-lr-10">
        <div id="grid" class="col-md-12 padding-0" style="overflow: hidden; overflow-x: scroll;">
            <telerik:RadGrid RenderMode="Lightweight" ID="grdVendor" runat="server" AutoGenerateColumns="false"
                AllowPaging="true" AllowSorting="true" AllowFilteringByColumn="true" OnNeedDataSource="grdVendor_NeedDataSource"
                PageSize="<%# Convert.ToInt32(SuzlonBPP.Models.Constants.CONST_GRID_PAGE_SIZE) %>">
                <GroupingSettings CaseSensitive="false" />
                <MasterTableView ClientDataKeyNames="VendorBankId" CommandItemDisplay="None" EnableNoRecordsTemplate="true" AllowFilteringByColumn="true">
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
                        <telerik:GridBoundColumn DataField="CompanyCode" HeaderText="Company Code" HeaderStyle-Width="5%" DataType="System.String" />
                        <telerik:GridBoundColumn DataField="CompanyName" HeaderText="Company Name" HeaderStyle-Width="10%" DataType="System.String" />
                        <telerik:GridBoundColumn DataField="VendorCode" HeaderText="Vendor Code" HeaderStyle-Width="5%" DataType="System.String" />
                        <telerik:GridBoundColumn DataField="VendorName" HeaderText="Vendor Name" HeaderStyle-Width="10%" DataType="System.String" />
                        <telerik:GridBoundColumn DataField="SuzlonEmailID1" HeaderText="Suzlon Email ID 1" HeaderStyle-Width="10%" DataType="System.String" />
                        <telerik:GridBoundColumn DataField="SuzlonEmailID2" HeaderText="Suzlon Email ID 2" HeaderStyle-Width="10%" DataType="System.String" />
                        <telerik:GridBoundColumn DataField="VendorEmailID1" HeaderText="Vendor Email ID 1" HeaderStyle-Width="10%" DataType="System.String" />
                        <telerik:GridBoundColumn DataField="VendorEmailID2" HeaderText="Vendor Email ID 2" HeaderStyle-Width="10%" DataType="System.String" />
                        <telerik:GridBoundColumn DataField="CreatedDate" HeaderText="Modified Date" DataFormatString="{0:dd/MMM/yyyy}" HeaderStyle-Width="10%" DataType="System.DateTime" />
                        <telerik:GridBoundColumn DataField="CreatedTime" HeaderText="Modified Time" HeaderStyle-Width="5%" DataType="System.TimeSpan" DataFormatString="{0:g}" />
                        <telerik:GridBoundColumn DataField="UserName" HeaderText="User Name" HeaderStyle-Width="10%" DataType="System.String" />
                        <telerik:GridBoundColumn DataField="AccountNumber" HeaderText="Account Number" HeaderStyle-Width="10%" DataType="System.String" />
                        <telerik:GridBoundColumn DataField="IFSCCode" HeaderText="IFSC Code" HeaderStyle-Width="10%" DataType="System.String" />
                        <telerik:GridBoundColumn DataField="AccountType" HeaderText="Account Type" HeaderStyle-Width="5%" DataType="System.String" />
                        <telerik:GridBoundColumn DataField="VendorBankName" HeaderText="Bank Name" HeaderStyle-Width="10%" DataType="System.String" />
                        <telerik:GridBoundColumn DataField="VendorBranchName" HeaderText="Branch Name" HeaderStyle-Width="10%" DataType="System.String" />
                        <telerik:GridBoundColumn DataField="Location" HeaderText="City" HeaderStyle-Width="20%" DataType="System.String" />
                    </Columns>
                    <PagerStyle PageSizes="5,10,15" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                        PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />
                </MasterTableView>
            </telerik:RadGrid>
        </div>
        <telerik:RadNotification RenderMode="Lightweight" Position="Center" runat="server" ID="radMessage" VisibleOnPageLoad="false"
            Width="350px" AutoCloseDelay="0" TitleIcon="none" ContentIcon="none">
        </telerik:RadNotification>
    </div>
</asp:Content>
