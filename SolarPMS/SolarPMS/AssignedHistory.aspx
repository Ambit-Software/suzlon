<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AssignedHistory.aspx.cs" Inherits="SolarPMS.AssignedHistory" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
            <telerik:RadGrid RenderMode="Lightweight" ID="grdHistory" runat="server"
                                                    AutoGenerateColumns="false" AllowPaging="true" AllowSorting="true" AllowFilteringByColumn="false" PageSize="<%# Convert.ToInt32(SolarPMS.Models.Constants.CONST_GRID_PAGE_SIZE) %>" OnNeedDataSource="grdAssignToMe_NeedDataSource"  OnItemCreated="grdAssignToMe_ItemCreated">
                                                    <GroupingSettings CaseSensitive="false" />
                                                    <MasterTableView CommandItemSettings-ShowRefreshButton="false" ClientDataKeyNames="IssueId" EditMode="InPlace" CommandItemDisplay="None" AllowFilteringByColumn="true">
                                                        <CommandItemSettings ShowRefreshButton="false" ShowExportToExcelButton="false" />
                                                        <Columns>
                                                            <telerik:GridBoundColumn DataField="UserName" HeaderText="User Name" UniqueName="UserName">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="Date" HeaderText="Date" UniqueName="Date">
                                                            </telerik:GridBoundColumn>                                                                                               
                                                    

                                                        </Columns>
                                                        <PagerStyle PageSizes="5,10,15" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                                                            PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />


                                                    </MasterTableView>
                                                </telerik:RadGrid>
    </div>
    </form>
</body>
</html>
