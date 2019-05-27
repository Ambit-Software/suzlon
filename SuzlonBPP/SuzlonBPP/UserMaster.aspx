<%@ Page Title="" Language="C#" MasterPageFile="SuzlonBPP.Master" AutoEventWireup="true" CodeBehind="UserMaster.aspx.cs" Inherits="SuzlonBPP.Admin.UserMaster" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link href="Content/css/export_import.css" rel="stylesheet" />
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="GrdUser">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="GrdUser" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="LoadingPanel1" runat="server" Skin="Default">
    </telerik:RadAjaxLoadingPanel>
    <script src="Scripts/bootstrap.min.js"></script>
    <%--<style> 
            .RadComboBox {
                width: 90px !important;
            }
        </style>--%>
        
         <div class="row margin-0">
        <div class="col-xs-6 heading-big">
            <h5 class="margin-0 lineheight-42 breath-ctrl">Home / <span >User Details</span></h5>
        </div>
        <div class="col-sm-6 col-xs-6 list-btn padding-t-6">
            <ul class="list-btn">
            <li>
                <telerik:RadButton RenderMode="Lightweight" ID="btnAddNew" runat="server" Text="Add User" OnClick="btnAddNew_Click">
                <Icon PrimaryIconCssClass="rbAdd"></Icon>
            </telerik:RadButton>
            </li>
            <li>
                <telerik:RadButton RenderMode="Lightweight" CssClass="pull-right" ID="btnExcel" runat="server" Text="Export" OnClick="btnExcel_Click">
                <Icon PrimaryIconCssClass="rbDownload"></Icon>
            </telerik:RadButton>
            </li>

        </ul>
        </div>
    </div>

        
        
        
    
    <div class="col-xs-12 overflow-h padding-lr-10">
        <div id="grid" class="col-md-12 padding-0" style="overflow: hidden; overflow-x: scroll;">
            <telerik:RadGrid RenderMode="Lightweight"  ID="GrdUser" runat="server" AutoGenerateColumns="false" MasterTableView-CommandItemSettings-AddNewRecordText="Add User"
                AllowPaging="true" AllowSorting="true" AllowFilteringByColumn="true" GroupingSettings-CaseSensitive="false" PageSize="<%# Convert.ToInt32(SuzlonBPP.Models.Constants.CONST_GRID_PAGE_SIZE) %>"
                OnItemCommand="GrdUser_ItemCommand" OnNeedDataSource="GrdUser_NeedDataSource" OnEditCommand="GrdUser_EditCommand" ExportSettings-ExportOnlyData="true" OnItemDataBound="GrdUser_ItemDataBound">
                <ExportSettings IgnorePaging="true" FileName="UserMaster" Pdf-ContentFilter="NoFilter" ExportOnlyData="true"/>
                <MasterTableView ClientDataKeyNames="UserId" CommandItemDisplay="None" AllowFilteringByColumn="true">
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
                        <telerik:GridBoundColumn HeaderStyle-Width="13%" DataField="Name" HeaderText="User Name" UniqueName="UserName" />
                        <telerik:GridBoundColumn HeaderStyle-Width="10%" DataField="EmployeeId" HeaderText="Employee ID" UniqueName="EmployeeId" />
                        <telerik:GridBoundColumn HeaderStyle-Width="12%" DataField="CompanyNames" HeaderText="Company" UniqueName="CompanyName" />
                        <telerik:GridBoundColumn HeaderStyle-Width="10%" DataField="VerticalNames" HeaderText="Vertical" UniqueName="Vertical" />
                        <telerik:GridBoundColumn HeaderStyle-Width="10%" DataField="SubVerticalNames" HeaderText="Sub Vertical" UniqueName="SubVertical" />
                        <telerik:GridBoundColumn HeaderStyle-Width="10%" DataField="ProfileName" HeaderText="Profile" UniqueName="ProfileName" />
                        <%--<telerik:GridBoundColumn HeaderStyle-Width="10%" DataField="UserDetail.VendorCode" HeaderText="Vendor Code" UniqueName="VendorCode" />--%>
                        <telerik:GridBoundColumn HeaderStyle-Width="10%" DataField="EmailId" HeaderText="Email ID" UniqueName="EmailId" />
                        <telerik:GridBoundColumn HeaderStyle-Width="10%" DataField="MobileNo" HeaderText="Mobile No" UniqueName="MobileNo" />
                        <telerik:GridBoundColumn HeaderStyle-Width="10%" DataField="Authentication" HeaderText="Authentication" UniqueName="Authentication" />
                        <%--<telerik:GridBoundColumn HeaderStyle-Width="10%" DataField="Status" HeaderText="Status" UniqueName="Status" />--%>
                          <telerik:GridTemplateColumn DataField="Status" HeaderText="Status" UniqueName="Status"  SortExpression="Status">
                            <ItemTemplate>
                                <asp:Label ID="lblStatus" Text='<%# Bind( "Status") %>' runat="server"></asp:Label>
                            </ItemTemplate>
                     <%--       <EditItemTemplate>
                                <asp:CheckBox ID="chkStatus" runat="server" Checked='<%# Bind( "Status") %>' />                               
                            </EditItemTemplate>--%>
                        </telerik:GridTemplateColumn>  
                        <telerik:GridEditCommandColumn HeaderStyle-Width="5%" HeaderText="Edit" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" Exportable="false" />
                    </Columns>
                    <PagerStyle PageSizes="5,10,15" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                        PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />
                </MasterTableView>
            </telerik:RadGrid>
        </div>
    </div>
</asp:Content>
