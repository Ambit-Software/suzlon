<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="SuzlonBPP.Master" CodeBehind="VendorMaster.aspx.cs" Inherits="SuzlonBPP.VendorMaster" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="Scripts/bootstrap.min.js"></script>
    <div class="col-xs-12 heading-big">
        <h5 class="margin-0 lineheight-42 breath-ctrl">Home / <span>Vendor</span></h5>
    </div>


    <div class="clearfix"></div>
   <div class="col-xs-12 overflow-h padding-lr-10">
        <div id="grid" class="col-md-12 padding-0" style="overflow: hidden; overflow-x: scroll;">
            <telerik:RadGrid RenderMode="Lightweight" ID="grdVendor" runat="server" AutoGenerateColumns="false"
                AllowPaging="true" AllowSorting="true" AllowFilteringByColumn="true" OnNeedDataSource="grdVendor_NeedDataSource"
                PageSize="<%# Convert.ToInt32(SuzlonBPP.Models.Constants.CONST_GRID_PAGE_SIZE) %>">
                <GroupingSettings CaseSensitive="false" />
                <MasterTableView ClientDataKeyNames="VendorId" EditMode="InPlace" CommandItemDisplay="None" EnableNoRecordsTemplate="true" AllowFilteringByColumn="true">
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
                        <telerik:GridBoundColumn DataField="VendorCode" HeaderText="Vendor Code" HeaderStyle-Width="7%" DataType="System.String" />
                        <telerik:GridBoundColumn DataField="VendorName" HeaderText="Vendor Name" HeaderStyle-Width="10%" DataType="System.String" />
                        <telerik:GridBoundColumn DataField="CompanyCode" HeaderText="Company Code" HeaderStyle-Width="8%" DataType="System.String" />
                        <telerik:GridBoundColumn DataField="CompanyName" HeaderText="Company Name" HeaderStyle-Width="10%" DataType="System.String" />
                        <telerik:GridBoundColumn DataField="VendorAccountGroup" HeaderText="Vendor Group" HeaderStyle-Width="10%" DataType="System.String" />
                        <telerik:GridBoundColumn DataField="PanNo" HeaderText="Tax Number" HeaderStyle-Width="10%" DataType="System.String" />
                        <telerik:GridBoundColumn DataField="CentralPostingBlock" HeaderText="Posting Block" HeaderStyle-Width="5%" DataType="System.String" />
                        <telerik:GridBoundColumn DataField="CentImpPurchaseBlock" HeaderText="Purchase Block" HeaderStyle-Width="5%" DataType="System.String" />
                        <telerik:GridBoundColumn DataField="FunctionBlock" HeaderText="Function Block" HeaderStyle-Width="5%" DataType="System.String" />
                        <telerik:GridBoundColumn DataField="PaymentBlock" HeaderText="Payment Block" HeaderStyle-Width="5%" DataType="System.String" />
                        <telerik:GridBoundColumn DataField="CentralDeletion" HeaderText="Deletion Block" HeaderStyle-Width="5%" DataType="System.String" />
                        <telerik:GridBoundColumn DataField="Street" HeaderText="Street" HeaderStyle-Width="5%" DataType="System.String" />
                        <telerik:GridBoundColumn DataField="City" HeaderText="City" HeaderStyle-Width="7%" DataType="System.String" />
                        <telerik:GridBoundColumn DataField="Region" HeaderText="Region" HeaderStyle-Width="8%" DataType="System.String" />
                        <telerik:GridBoundColumn DataField="CountryKey" HeaderText="Country" HeaderStyle-Width="10%" DataType="System.String" />
                    </Columns>
                    <PagerStyle PageSizes="5,10,15" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                        PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />
                </MasterTableView>
            </telerik:RadGrid>
        </div>
        <telerik:RadNotification RenderMode="Lightweight" Position="Center" runat="server" ID="RadNotification1" VisibleOnPageLoad="false"
            Width="350px" AutoCloseDelay="0" TitleIcon="none" ContentIcon="none">
        </telerik:RadNotification>
    </div>
</asp:Content>
