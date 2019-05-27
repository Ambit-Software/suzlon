<%@ Page Title="" Language="C#" MasterPageFile="~/SuzlonBPP.Master" AutoEventWireup="true" CodeBehind="TreasuryPaymentReport.aspx.cs" Inherits="SuzlonBPP.TreasuryPaymentReport" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="Scripts/bootstrap.min.js"></script>
    <div class="row margin-0">
        <div class="col-xs-12 heading-big">
            <h5 class="margin-0 lineheight-42 breath-ctrl">Home / <span>Payment Treasury Report</span></h5>
        </div>
        <div class="col-sm-6 col-xs-6 ">
        </div>
    </div>
    <div class="panel-body">
        <div class="tab-content">
            <div class="row margin-0">
                <div class=" col-xs-12 padding-lr-10">
                    <div class="col-xs-12 col-sm-10 col-md-10 col-lg-10 padding-bottom" style="background-color: #FFF;">
                        <div class="col-lg-4 col-sm-6 col-md-4 col-xs-12">
                            <div class="col-lg-4 col-sm-5 col-md-5 col-xs-12 padding-0">
                                <label class="control-label lable-txt" for="name">Vertical</label>
                            </div>
                            <div class="col-lg-8 col-sm-7 col-md-7 col-xs-12 padding-0">
                                <telerik:RadDropDownList CausesValidation="false" ID="drpVerticals" OnSelectedIndexChanged="drpVerticals_SelectedIndexChanged" DefaultMessage="Select"
                                    runat="server" Height="200" Width="305" AutoPostBack="true" Filter="Contains" MarkFirstMatch="true">
                                </telerik:RadDropDownList>
                            </div>
                        </div>
                        <div class="col-lg-4 col-sm-6 col-md-4 col-xs-12">
                            <div class="col-lg-4 col-sm-5 col-md-5 col-xs-12 padding-0">
                                <label class="control-label lable-txt" for="name">Company Code</label>
                            </div>
                            <div class="col-md-8 col-sm-7 col-md-8 col-xs-12 padding-0">
                                <telerik:RadDropDownList CausesValidation="false" ID="drpCompanyCode" OnSelectedIndexChanged="drpCompanyCode_SelectedIndexChanged"
                                    runat="server" Height="200" Width="305" AutoPostBack="true" Filter="Contains" MarkFirstMatch="true">
                                </telerik:RadDropDownList>
                            </div>
                        </div>
                        <div class="col-lg-3 col-sm-6 col-md-4 col-xs-12">
                            <div class="col-lg-4 col-sm-5 col-md-5 col-xs-12 padding-0">
                                <label class="control-label lable-txt" for="name">From Date</label>
                            </div>
                            <div class="col-lg-8 col-sm-7 col-md-7 col-xs-12 padding-0">
                                <telerik:RadDatePicker runat="server" ID="dtFromDate" DateInput-DateFormat="dd-MMM-yyyy" EnableTyping="false" RenderMode="Lightweight"></telerik:RadDatePicker>
                            </div>
                        </div>
                        <div class="col-lg-4 col-sm-6 col-md-4 col-xs-12">
                            <div class="col-lg-4 col-sm-5 col-md-5 col-xs-12 padding-0">
                                <label class="control-label lable-txt" for="name">Payment No</label>
                            </div>
                            <div class="col-lg-8 col-sm-7 col-md-7 col-xs-12 padding-0">
                                <telerik:RadDropDownList CausesValidation="false" ID="drpPayment" OnSelectedIndexChanged="drpPayment_SelectedIndexChanged" DefaultMessage="All"
                                    runat="server" Height="200" Width="305" AutoPostBack="true" Filter="Contains" MarkFirstMatch="true">
                                </telerik:RadDropDownList>
                            </div>
                        </div>
                        <div class="col-lg-4 col-sm-6 col-md-4 col-xs-12">
                            <div class="col-lg-4 col-sm-5 col-md-5 col-xs-12 padding-0">
                                <label class="control-label lable-txt" for="name">Treasury No</label>
                            </div>
                            <div class="col-lg-8 col-sm-7 col-md-7 col-xs-12 padding-0">
                                <telerik:RadDropDownList CausesValidation="false" RenderMode="Lightweight" ID="drpTreasuryNo" DefaultMessage="All"
                                    runat="server" Height="200" Width="305" AutoPostBack="true" Filter="Contains" MarkFirstMatch="true">
                                </telerik:RadDropDownList>
                            </div>
                        </div>
                        <div class="col-lg-3 col-sm-6 col-md-4 col-xs-12">
                            <div class="col-lg-4 col-sm-5 col-md-5 col-xs-12 ">
                                <label class="control-label lable-txt" for="name">To Date</label>
                            </div>
                            <div class="col-lg-8 col-sm-7 col-md-7 col-xs-12 padding-0">
                                <telerik:RadDatePicker RenderMode="Lightweight" ID="dtToDate" Width="100%" EnableTyping="false" runat="server" DateInput-DateFormat="dd-MMM-yyyy">
                                </telerik:RadDatePicker>
                            </div>
                        </div>
                    </div>
                    <div class="col-xs-12 col-sm-2 col-md-2 col-lg-2" style="background-color: #FFF;">
                        <div class="col-lg-12 col-sm-12 col-md-12 col-xs-6 padding-bottom">
                            <asp:Button CssClass="btn btn-grey" OnClick="btnGenerateReport_Click" Text="Show Report" runat="server" ID="btnGenerateReport"></asp:Button>
                        </div>
                        <div class="col-lg-12 col-sm-12 col-md-12 col-xs-6">
                            <asp:Button CssClass="btn btn-grey" OnClick="btnExport_Click" Text="Export" runat="server" ID="btnExport"></asp:Button>
                        </div>
                    </div>
                </div>
                <div class="col-md-12 tab-pane fade in active overflow-x" id="tab1primary">
                    <telerik:RadGrid RenderMode="Lightweight" ID="grdTreasuryReport" runat="server" ClientIDMode="AutoID" OnNeedDataSource="grdTreasuryReport_NeedDataSource"
                        AutoGenerateColumns="false" AllowPaging="true" AllowSorting="true" AllowFilteringByColumn="false" ExportSettings-ExportOnlyData="true"
                        PageSize="<%# Convert.ToInt32(SuzlonBPP.Models.Constants.CONST_GRID_PAGE_SIZE) %>">
                        <GroupingSettings CaseSensitive="false" />
                        <ExportSettings FileName="PaymentTreasuryReport" IgnorePaging="true"></ExportSettings>
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
                                <telerik:GridBoundColumn DataField="Vertical" HeaderText="Vertical" UniqueName="Vertical">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="CompanyCode" HeaderText="Company Code" UniqueName="CompanyCode">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="PaymentLotNo" HeaderText="Payment Lot No." UniqueName="PaymentLotNo">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="PaymentDoc" HeaderText="Payment Doc" UniqueName="PaymentDoc">
                                </telerik:GridBoundColumn>
                                <telerik:GridDateTimeColumn DataField="PaymentDate" HeaderText="Payment Date" FilterControlWidth="300px" DataFormatString="{0:dd/MM/yyyy}">
                                </telerik:GridDateTimeColumn>
                                <telerik:GridBoundColumn DataField="PaymentAmount" HeaderText="Payment Amount" DataType="System.Double" ItemStyle-HorizontalAlign="Right" UniqueName="RequestedAmount" DataFormatString="{0:#,###}">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="BusinessArea" HeaderText="Business Area" UniqueName="BusinessArea">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="TreasuryNo" HeaderText="Treasury Number" UniqueName="TreasuryNumber">
                                </telerik:GridBoundColumn>
                                <telerik:GridDateTimeColumn DataField="CreationDate" HeaderText="Creation Date" FilterControlWidth="300px" DataFormatString="{0:dd/MM/yyyy}">
                                </telerik:GridDateTimeColumn>
                                <telerik:GridBoundColumn DataField="RequestType" HeaderText="Request Type">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="NatureOfReqest" HeaderText="Nature of Reqest">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="ApprovedAmount" HeaderText="Approved Amount" DataFormatString="{0:#,###}">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="AddendumTotal" HeaderText="Addendum Total" DataFormatString="{0:#,###}">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="NETTotal" DataType="System.Double" HeaderText="Net Total" DataFormatString="{0:#,###}">
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
    </div>
</asp:Content>
