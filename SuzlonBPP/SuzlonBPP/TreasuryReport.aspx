<%@ Page Title="" Language="C#" MasterPageFile="~/SuzlonBPP.Master" AutoEventWireup="true" CodeBehind="TreasuryReport.aspx.cs" Inherits="SuzlonBPP.TreasuryReport" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="Scripts/bootstrap.min.js"></script>
    <div class="container-fluid padding-0">
        <div class="row margin-0">
            <div class="col-xs-12 heading-big padding-lr-10">
                <h5 class="margin-0 lineheight-42 breath-ctrl">Home / <span>Treasury Report</span></h5>
            </div>
            <div class="col-sm-6 col-xs-6 ">
            </div>
        </div>
        <div class="row margin-0  margin-b-0">
            <div class=" col-xs-12 padding-lr-10">
                <div class="col-xs-12 padding-0" style="background-color: #FFF;">
                    <div class="col-sm-12 col-lg-11 padding-0" style="background-color: #FFF;">
                        <div class="col-sm-4 col-md-4 col-xs-12 padding-0">
                            <div class="col-sm-3 col-md-3 col-xs-12 padding-0">
                                <label class="control-label lable-txt" for="name">Vertical</label>
                            </div>
                            <div class="col-sm-9 col-md-9 col-xs-12 padding-0">
                                <telerik:RadDropDownList CausesValidation="false" ID="drpVerticals" OnSelectedIndexChanged="drpVerticals_SelectedIndexChanged" DefaultMessage="Select"
                                    runat="server" Height="200" Width="305" AutoPostBack="true" Filter="Contains" MarkFirstMatch="true">
                                </telerik:RadDropDownList>
                            </div>
                        </div>
                        <div class="col-sm-4 col-md-4 col-xs-12">
                            <div class="col-sm-3 col-md-3 col-xs-12 padding-0">
                                <label class="control-label lable-txt" for="name">From Date</label>
                            </div>
                            <div class="col-sm-9 col-md-4 col-xs-12 padding-0">
                                <telerik:RadDatePicker runat="server" ID="dtFromDate" DateInput-DateFormat="dd-MMM-yyyy" EnableTyping="false" RenderMode="Lightweight"></telerik:RadDatePicker>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-xs-12 padding-0" style="background-color: #FFF;">
                    <div class="col-sm-12 col-lg-11 padding-0" style="background-color: #FFF;">
                        <div class="col-sm-4 col-md-4 col-xs-12 padding-0">
                            <div class="col-sm-3 col-md-3 col-xs-12 padding-0">
                                <label class="control-label lable-txt" for="name">Treasury No</label>
                            </div>
                            <div class="col-sm-9 col-md-9 col-xs-12 padding-0">
                                <telerik:RadDropDownList CausesValidation="false" RenderMode="Lightweight" ID="drpTreasuryNo" DefaultMessage="All"
                                    runat="server" Height="200" Width="305" AutoPostBack="true" Filter="Contains" MarkFirstMatch="true">
                                </telerik:RadDropDownList>
                            </div>
                        </div>
                        <div class="col-sm-4 col-md-4 col-xs-12">
                            <div class="col-sm-3 col-md-3 col-xs-12 ">
                                <label class="control-label lable-txt" for="name">To Date</label>
                            </div>
                            <div class="col-sm-9 col-md-4 col-xs-12 padding-0">
                                <telerik:RadDatePicker RenderMode="Lightweight" ID="dtToDate" Width="100%" EnableTyping="false" runat="server" DateInput-DateFormat="dd-MMM-yyyy">
                                </telerik:RadDatePicker>
                            </div>
                        </div>
                        <div class="col-sm-4 col-md-4 col-xs-12 padding-0">
                            <asp:Button CssClass="btn btn-grey" OnClick="btnGenerateReport_Click" Text="Show Report" runat="server" ID="btnGenerateReport"></asp:Button>
                            <asp:Button CssClass="btn btn-grey" OnClick="btnExport_Click" Text="Export" runat="server" ID="btnExport"></asp:Button>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-12 tab-pane fade in active overflow-x" id="tab1primary">
                <telerik:RadGrid RenderMode="Lightweight" ID="grdTreasuryReport" runat="server" ClientIDMode="AutoID" OnNeedDataSource="grdTreasury_NeedDataSource"
                    AutoGenerateColumns="false" AllowPaging="true" AllowSorting="true" AllowFilteringByColumn="false" ExportSettings-ExportOnlyData="true"
                    PageSize="<%# Convert.ToInt32(SuzlonBPP.Models.Constants.CONST_GRID_PAGE_SIZE) %>">
                    <GroupingSettings CaseSensitive="false" />
                    <ExportSettings FileName="TreasuryReport" IgnorePaging="true"></ExportSettings>
                    <MasterTableView CommandItemSettings-ShowRefreshButton="false" EditMode="InPlace" CommandItemDisplay="None"
                        AllowFilteringByColumn="true" ClientIDMode="AutoID">
                        <NoRecordsTemplate>
                            <table width="100%" border="0" cellpadding="20" cellspacing="20">
                                <tr>
                                    <td align="center">No records to display.
                                    </td>
                                </tr>
                            </table>
                        </NoRecordsTemplate>
                        <CommandItemSettings ShowRefreshButton="true" ShowExportToExcelButton="false" />
                        <Columns>
                            <telerik:GridBoundColumn DataField="NameOfInitiator" HeaderText="Name of Initiator" UniqueName="NameOfInitiator">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Vertical" HeaderText="Vertical" UniqueName="Vertical">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="SubVertical" HeaderText="Sub-Vertical" UniqueName="SubVertical">
                            </telerik:GridBoundColumn>
                            <telerik:GridDateTimeColumn DataField="CreatedDate" HeaderText="Created Date" FilterControlWidth="300px" DataFormatString="{0:dd/MM/yyyy}">
                            </telerik:GridDateTimeColumn>
                            <telerik:GridBoundColumn DataField="TreasuryNumber" HeaderText="Treasury Number" UniqueName="TreasuryNumber">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="RequestType" HeaderText="Request Type" UniqueName="RequestType">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="NatureOfReqest" HeaderText="Nature of Request" UniqueName="NatureOfRequest">
                            </telerik:GridBoundColumn>
                            <telerik:GridDateTimeColumn DataField="ProposedPaymentDate" HeaderText="Proposed Payment Date" EnableTimeIndependentFiltering="true" DataFormatString="{0:dd/MM/yyyy}">
                            </telerik:GridDateTimeColumn>
                            <telerik:GridBoundColumn DataField="RequestedAmount" HeaderText="Requested Amount" DataType="System.Double" ItemStyle-HorizontalAlign="Right" UniqueName="RequestedAmount" DataFormatString="{0:#,###}">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="ApprovedAmount" HeaderText="Approved Amount" DataFormatString="{0:#,###}">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="AddendumTotal" HeaderText="Addendum Total" DataFormatString="{0:#,###}">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="NETTotal" DataType="System.Double" HeaderText="Net Total" DataFormatString="{0:#,###}">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="UtilisedAmount" HeaderText="Utilized Amount" DataFormatString="{0:#,###}">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="BalanceAmount" HeaderText="Balance Amount" DataFormatString="{0:#,###}">
                            </telerik:GridBoundColumn>
                        </Columns>
                        <PagerStyle PageSizes="5,10,15" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                            PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />
                    </MasterTableView>
                </telerik:RadGrid>
                <telerik:RadGrid RenderMode="Lightweight" ID="gridTemp" runat="server" ClientIDMode="AutoID" Visible="false"
                    AutoGenerateColumns="false" AllowPaging="true" AllowSorting="true" AllowFilteringByColumn="false">
                    <GroupingSettings CaseSensitive="false" />
                </telerik:RadGrid>
            </div>
        </div>
    </div>
</asp:Content>
