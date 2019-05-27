<%@ Page Title="" Language="C#" MasterPageFile="~/SuzlonBPP.Master" AutoEventWireup="true" CodeBehind="BankDetailsReport.aspx.cs" Inherits="SuzlonBPP.BankDetailsReport" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="Scripts/bootstrap.min.js"></script>
    <div class="row margin-0">
        <div class="col-xs-12 heading-big">
            <h5 class="margin-0 lineheight-42 breath-ctrl">Home / <span>Bank Details</span></h5>
        </div>
        <div class="col-sm-6 col-xs-6 ">
        </div>
    </div>
    <div class="panel-body">
        <div class="tab-content">
            <div class="row margin-0">
                <div class=" col-xs-12 padding-lr-10">
                    <div class="col-xs-12 col-sm-10 col-md-10 col-lg-10 padding-bottom" style="background-color: #FFF;">
                        <div class="col-lg-6 col-sm-6 col-md-4 col-xs-12">
                            <div class="col-lg-4 col-sm-5 col-md-5 col-xs-12 padding-0">
                                <label class="control-label lable-txt" for="name">Company Code</label>
                            </div>
                            <div class="col-lg-8 col-sm-7 col-md-7 col-xs-12 padding-0">
                                <telerik:RadDropDownList CausesValidation="false" ID="drpCompanyCode" OnSelectedIndexChanged="drpCompanyCode_SelectedIndexChanged"
                                    runat="server" Height="200" Width="305" AutoPostBack="true" Filter="Contains" MarkFirstMatch="true">
                                </telerik:RadDropDownList>
                            </div>
                        </div>
                        <div class="col-lg-6 col-sm-6 col-md-4 col-xs-12">
                            <div class="col-lg-4 col-sm-5 col-md-5 col-xs-12 padding-0">
                                <label class="control-label lable-txt" for="name">Initiator</label>
                            </div>
                            <div class="col-lg-8 col-sm-7 col-md-7 col-xs-12 padding-0">
                                <telerik:RadDropDownList CausesValidation="false" RenderMode="Lightweight" ID="drpInitiator" DefaultMessage="All"
                                    runat="server" Height="200" Width="305" AutoPostBack="true" Filter="Contains" MarkFirstMatch="true">
                                </telerik:RadDropDownList>
                            </div>
                        </div>
                        <div class="col-lg-6 col-sm-6 col-md-4 col-xs-12">
                            <div class="col-lg-4 col-sm-5 col-md-5 col-xs-12 padding-0">
                                <label class="control-label lable-txt" for="name">Vendor Code</label>
                            </div>
                            <div class="col-lg-8 col-sm-7 col-md-7 col-xs-12 padding-0">
                                <telerik:RadDropDownList CausesValidation="false" ID="drpVendors" OnSelectedIndexChanged="drpVendors_SelectedIndexChanged" DefaultMessage="All"
                                    runat="server" Height="200" Width="305" AutoPostBack="true" Filter="Contains" MarkFirstMatch="true">
                                </telerik:RadDropDownList>
                            </div>
                        </div>

                        <div class="col-lg-6 col-sm-6 col-md-4 col-xs-12">
                            <div class="col-lg-4 col-sm-5 col-md-5 col-xs-12 padding-0">
                                <label class="control-label lable-txt" for="name">Vendor Name</label>
                            </div>
                            <div class="col-lg-8 col-sm-7 col-md-7 col-xs-12 padding-0">
                                <telerik:RadDropDownList CausesValidation="false" ID="drpVendorName" OnSelectedIndexChanged="drpVendorName_SelectedIndexChanged" DefaultMessage="All"
                                    runat="server" Height="200" Width="305" AutoPostBack="true" Filter="Contains" MarkFirstMatch="true">
                                </telerik:RadDropDownList>
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
                    <telerik:RadGrid RenderMode="Lightweight" ID="grdBankDetailsReport" runat="server" ClientIDMode="AutoID" OnNeedDataSource="grdBankDetailsReport_NeedDataSource"
                        AutoGenerateColumns="false" AllowPaging="true" AllowSorting="true" AllowFilteringByColumn="false" ExportSettings-ExportOnlyData="true"
                        PageSize="<%# Convert.ToInt32(SuzlonBPP.Models.Constants.CONST_GRID_PAGE_SIZE) %>">
                        <groupingsettings casesensitive="false" />
                        <exportsettings filename="BankDetailsReport" ignorepaging="true"></exportsettings>
                        <mastertableview commanditemsettings-showrefreshbutton="false" editmode="InPlace" commanditemdisplay="None"
                            allowfilteringbycolumn="true" clientidmode="AutoID">
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
                                <telerik:GridBoundColumn DataField="RequestId" HeaderText="Request Id" UniqueName="RequestId">
                                </telerik:GridBoundColumn>
                                <telerik:GridDateTimeColumn DataField="RequestedDate" HeaderText="Requested Date" FilterControlWidth="300px">
                                </telerik:GridDateTimeColumn>
                                <telerik:GridDateTimeColumn DataField="RequestAccepted" HeaderText="Requested Accepted Date" FilterControlWidth="300px" >
                                </telerik:GridDateTimeColumn>
                                <telerik:GridBoundColumn DataField="CompanyCode" HeaderText="Company Code" UniqueName="CompanyCode">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="VendorCode" HeaderText="Vendor Code" UniqueName="VendorCode">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="VendorName" HeaderText="Vendor Name" UniqueName="VendorName">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="VendorEmailID1" HeaderText="Vendor Email ID 1" UniqueName="VendorEmailID1">
                                </telerik:GridBoundColumn>
                                 <telerik:GridBoundColumn DataField="VendorEmailID2" HeaderText="Vendor Email ID 2" UniqueName="VendorEmailID2">
                                </telerik:GridBoundColumn>
                                 <telerik:GridBoundColumn DataField="SuzlonEmailID1" HeaderText="Suzlon Email ID 1" UniqueName="SuzlonEmailID1">
                                </telerik:GridBoundColumn>
                                 <telerik:GridBoundColumn DataField="SuzlonEmailID2" HeaderText="Suzlon Email ID 2" UniqueName="SuzlonEmailID2">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="BankName" HeaderText="Bank Name" UniqueName="BankName">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="BranchName" HeaderText="Branch">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="IFSCCode" HeaderText="IFSC code">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="AccountNumber" HeaderText="Bank Account Number">
                                </telerik:GridBoundColumn>
                                     <telerik:GridBoundColumn DataField="Status" HeaderText="Status" UniqueName="Status">
                                </telerik:GridBoundColumn>
                            </Columns>
                            <PagerStyle PageSizes="5,10,15" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                                PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />
                        </mastertableview>
                    </telerik:RadGrid>
                    <telerik:RadGrid RenderMode="Lightweight" ID="gridTemp" runat="server" ClientIDMode="AutoID" Visible="false"
                        AutoGenerateColumns="false" AllowPaging="true" AllowSorting="true" AllowFilteringByColumn="false">
                        <groupingsettings casesensitive="false" />
                    </telerik:RadGrid>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
