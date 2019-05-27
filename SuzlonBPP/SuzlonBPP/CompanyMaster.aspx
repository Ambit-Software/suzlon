<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CompanyMaster.aspx.cs" MasterPageFile="SuzlonBPP.Master" Inherits="SuzlonBPP.CompanyMaster" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="Scripts/bootstrap.min.js"></script>
   <div class="col-xs-12 heading-big">
                    <h5 class="margin-0 lineheight-42 breath-ctrl">Home / <span style="font-weight: normal !important">Company Master</span></h5>
                </div>
     

    <div class="clearfix"></div>
    <div class="col-xs-12 overflow-h padding-lr-10">
        <div id="grid">
            <telerik:RadGrid RenderMode="Lightweight" ID="grdCompany" runat="server" AutoGenerateColumns="false"
                AllowPaging="true" AllowSorting="true" AllowFilteringByColumn="true" OnNeedDataSource="grdCompany_NeedDataSource"
                PageSize="<%# Convert.ToInt32(SuzlonBPP.Models.Constants.CONST_GRID_PAGE_SIZE) %>">
                <GroupingSettings CaseSensitive="false" />
                <MasterTableView ClientDataKeyNames="CompanyId" EditMode="InPlace" CommandItemDisplay="None" EnableNoRecordsTemplate="true" AllowFilteringByColumn="true">
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
                        <telerik:GridBoundColumn DataField="CompanyCode" HeaderText="Company Code" HeaderStyle-Width="25%" DataType="System.String" />
                        <telerik:GridBoundColumn DataField="Name" HeaderText="Company Name" HeaderStyle-Width="25%" DataType="System.String" />
                        <telerik:GridBoundColumn DataField="Description" HeaderText="Description" HeaderStyle-Width="50%" DataType="System.String" />
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
