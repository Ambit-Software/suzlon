<%@ Page Title="" Language="C#" MasterPageFile="~/SuzlonBPP.Master" AutoEventWireup="true" CodeBehind="PaymentDetailReport.aspx.cs" Inherits="SuzlonBPP.PaymentDetailReport" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="Scripts/bootstrap.min.js"></script>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="drpCompanyCode">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="drpVendor" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
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

    <div class="row margin-0">
        <div class="col-xs-12 heading-big">
            <h5 class="margin-0 lineheight-42 breath-ctrl">Home / <span>Payment Report</span></h5>
        </div>
        <div class="col-sm-6 col-xs-6 ">
        </div>
    </div>
    <div class="panel-body">
        <div class="tab-content">
            <div class="row margin-0">
                <div class="col-xs-12 col-md-12 padding-lr-10">
                    <div class="col-xs-11 col-sm-10 col-md-10 col-lg-11 padding-lr-10 padding-bottom">
                        <div class="col-xs-12 col-sm-12 padding-0" style="background-color: #FFF;">
                            <div class="col-sm-6 col-md-4 col-xs-12 col-lg-4">
                                <div class="col-sm-5 col-md-5 col-xs-12 padding-0">
                                    <label class="control-label lable-txt" for="name">Bill Type</label>
                                </div>
                                <div class="col-sm-7 col-md-7 col-xs-12 padding-0">
                                    <telerik:RadDropDownList CausesValidation="false" ID="drpBillType" DefaultMessage="Select"
                                        runat="server" Height="200" Width="305" Filter="Contains" MarkFirstMatch="true">
                                        <Items>
                                            <telerik:DropDownListItem Value="Against" Text="Against" />
                                            <telerik:DropDownListItem Value="Advance" Text="Advance" />
                                        </Items>
                                    </telerik:RadDropDownList>
                                </div>
                            </div>
                            <div class="col-sm-6 col-md-4 col-xs-12 col-lg-4">
                                <div class="col-sm-5 col-md-5 col-xs-12 padding-0">
                                    <label class="control-label lable-txt" for="name">Company Code</label>
                                </div>
                                <div class="col-sm-7 col-md-7 col-xs-12 padding-0">
                                    <telerik:RadDropDownList CausesValidation="false" ID="drpCompanyCode" OnSelectedIndexChanged="drpCompanyCode_SelectedIndexChanged"
                                        runat="server" Height="200" Width="305" AutoPostBack="true" Filter="Contains" MarkFirstMatch="true" DefaultMessage="All">
                                    </telerik:RadDropDownList>
                                </div>
                            </div>
                            <div class="col-sm-6 col-md-4 col-lg-4 col-xs-12">
                                <div class="col-sm-5 col-md-5 col-xs-12 padding-0">
                                    <label class="control-label lable-txt" for="name">Vendor Code</label>
                                </div>
                                <div class="col-sm-7 col-md-7 col-xs-12 padding-0">
                                    <telerik:RadDropDownList CausesValidation="false" ID="drpVendor" DefaultMessage="All"
                                        runat="server" Height="200" Width="305" AutoPostBack="true" Filter="Contains" MarkFirstMatch="true">
                                    </telerik:RadDropDownList>
                                </div>
                            </div>
                        <%--</div>
                        <div class="col-xs-12 padding-0" style="background-color: #FFF;">--%>
                            <div class="col-sm-6 col-md-4 col-xs-12 col-lg-4">
                                <div class="col-sm-5 col-md-5 col-xs-12 padding-0">
                                    <label class="control-label lable-txt" for="name">Sub Vertical</label>
                                </div>
                                <div class="col-sm-7 col-md-7 col-xs-12 padding-0">
                                    <telerik:RadDropDownList CausesValidation="false" ID="drpSubVerticals" DefaultMessage="All"
                                        runat="server" Height="200" Width="305" AutoPostBack="true" Filter="Contains" MarkFirstMatch="true">
                                    </telerik:RadDropDownList>
                                </div>
                            </div>
                             <div class="col-sm-6 col-md-4 col-xs-12 col-lg-4">
                                <div class="col-sm-5 col-md-5 col-xs-12 padding-0">
                                    <label class="control-label lable-txt" for="name">From Date</label>
                                </div>
                                <div class="col-sm-7 col-md-7 col-xs-12 padding-0">
                                    <telerik:RadDatePicker runat="server" ID="dtFromDate" DateInput-DateFormat="dd-MMM-yyyy" EnableTyping="false" RenderMode="Lightweight"></telerik:RadDatePicker>
                                </div>
                            </div>
                            <div class="col-sm-6 col-md-4 col-xs-12 col-lg-4">
                                <div class="col-sm-5 col-md-5 col-xs-12 padding-0">
                                    <label class="control-label lable-txt" for="name">To Date</label>
                                </div>
                                <div class="col-sm-7 col-md-7 col-xs-12 padding-0">
                                    <telerik:RadDatePicker RenderMode="Lightweight" ID="dtToDate" Width="100%" EnableTyping="false" runat="server" DateInput-DateFormat="dd-MMM-yyyy">
                                    </telerik:RadDatePicker>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-xs-12 col-sm-2 col-md-2 col-lg-1">
                        <div class="row col-sm-12 col-md-12 col-xs-12 padding-bottom">
                            <asp:Button CssClass="btn btn-grey" OnClick="btnExport_Click" Text="Export" runat="server" ID="btnExport"></asp:Button>
                        </div>
                        <div class="row col-sm-12 col-md-12 col-xs-12">
                            <asp:Button CssClass="btn btn-grey" OnClick="btnGenerateReport_Click" Text="Show Report" runat="server" ID="btnGenerateReport"></asp:Button>
                        </div>
                    </div>
                </div>
                <div class="col-md-12 tab-pane fade in active overflow-x" id="tab1primary">
                    <telerik:RadGrid RenderMode="Lightweight" ID="grdPaymentReportAgainst" runat="server" ClientIDMode="AutoID" OnNeedDataSource="grdPaymentReportAgainst_NeedDataSource"
                        AutoGenerateColumns="false" AllowPaging="true" AllowSorting="true" AllowFilteringByColumn="false" ExportSettings-ExportOnlyData="true" ExportSettings-IgnorePaging="true"
                        PageSize="<%# Convert.ToInt32(SuzlonBPP.Models.Constants.CONST_GRID_PAGE_SIZE) %>">
                        <GroupingSettings CaseSensitive="false" />
                        <ExportSettings FileName="AgainstBill Payment Report" IgnorePaging="true"></ExportSettings>
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
                                <telerik:GridBoundColumn DataField="Sub_Vertical" HeaderText="Sub Vertical" UniqueName="Sub_Vertical">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="CompanyCode" HeaderText="Company Code" UniqueName="CompanyCode">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="VendorCode" HeaderText="Vendor Code" UniqueName="VendorCode">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="VendorName" HeaderText="Vendor Name" UniqueName="VendorName">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="DocumentNumber" HeaderText="Document No" UniqueName="DocumentNumber">
                                </telerik:GridBoundColumn>
                                <telerik:GridDateTimeColumn DataField="Posting_date" HeaderText="Posting Date" FilterControlWidth="300px" DataFormatString="{0:dd/MM/yyyy}">
                                </telerik:GridDateTimeColumn>
                                <telerik:GridBoundColumn DataField="Reference" HeaderText="Reference" UniqueName="Reference" DataFormatString="{0:#,###}">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Nature_of_Request" HeaderText="Nature of Request" UniqueName="Nature_of_Request">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Approval_status" HeaderText="Approval status" UniqueName="Approval_status">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Remark" HeaderText="Remark" UniqueName="Remark">
                                </telerik:GridBoundColumn>
                                <telerik:GridDateTimeColumn DataField="Net_Due_Date" HeaderText="Net Due Date" FilterControlWidth="300px" DataFormatString="{0:dd/MM/yyyy}">
                                </telerik:GridDateTimeColumn>
                                <telerik:GridBoundColumn DataField="Amount_in_INR" HeaderText="Amount in INR" UniqueName="Amount_in_INR">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="AmountProposed" HeaderText="Amount Proposed" DataFormatString="{0:#,###}">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="ApprovedAmount" HeaderText="Approved Proposed" DataFormatString="{0:#,###}">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Currency" HeaderText="Currency" UniqueName="Currency">
                                </telerik:GridBoundColumn>


                                <telerik:GridBoundColumn DataField="BusinessArea" HeaderText="Business Area" UniqueName="BusinessArea">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="ProfitCentre" HeaderText="Profit Centre" UniqueName="ProfitCentre">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="HouseBank" HeaderText="HouseBank" UniqueName="HouseBank">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="PaymentMethod" HeaderText="PaymentMethod" UniqueName="PaymentMethod">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="PaymentDocumentNumber" HeaderText="Payment Document Number" UniqueName="PaymentDocumentNumber">
                                </telerik:GridBoundColumn>
                                <telerik:GridDateTimeColumn DataField="Payment_Document_Posting_Date" HeaderText="Payment Document Posting Date" FilterControlWidth="300px" DataFormatString="{0:dd/MM/yyyy}">
                                </telerik:GridDateTimeColumn>
                                <telerik:GridBoundColumn DataField="CheckLotNumber" HeaderText="CheckLotNumber" UniqueName="CheckLotNumber">
                                </telerik:GridBoundColumn>
                                <telerik:GridDateTimeColumn DataField="SubmissionDate" HeaderText="SubmissionDate" FilterControlWidth="300px" DataFormatString="{0:dd/MM/yyyy}">
                                </telerik:GridDateTimeColumn>
                                <telerik:GridBoundColumn DataField="Treasury__Allocation_No" HeaderText="Treasury Allocation No" UniqueName="Treasury__Allocation_No">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Name_of_Initiator" HeaderText="Name of Initiator" UniqueName="Name_of_Initiator">
                                </telerik:GridBoundColumn>
                                <telerik:GridDateTimeColumn DataField="Reversal_Date" HeaderText="Reversal Date" FilterControlWidth="300px" DataFormatString="{0:dd/MM/yyyy}">
                                </telerik:GridDateTimeColumn>
                                <telerik:GridBoundColumn DataField="Unique_Reference_number" HeaderText="Unique Reference number" UniqueName="Unique_Reference_number">
                                </telerik:GridBoundColumn>
                            </Columns>
                            <PagerStyle PageSizes="5,10,15" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                                PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />
                        </MasterTableView>
                    </telerik:RadGrid>
                    <telerik:RadGrid RenderMode="Lightweight" ID="grdPaymentReportAdvance" runat="server" ClientIDMode="AutoID" OnNeedDataSource="grdPaymentReportAdvance_NeedDataSource"
                        AutoGenerateColumns="false" AllowPaging="true" AllowSorting="true" AllowFilteringByColumn="false" ExportSettings-ExportOnlyData="true" PageSize="<%# Convert.ToInt32(SuzlonBPP.Models.Constants.CONST_GRID_PAGE_SIZE) %>">
                        <GroupingSettings CaseSensitive="false" />
                        <ExportSettings FileName="AdvanceBill Payment Report" IgnorePaging="true"></ExportSettings>
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
                                <telerik:GridBoundColumn DataField="Sub_Vertical" HeaderText="Sub Vertical" UniqueName="Sub_Vertical">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="CompanyCode" HeaderText="Company Code" UniqueName="CompanyCode">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="VendorCode" HeaderText="Vendor Code" UniqueName="VendorCode">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="VendorName" HeaderText="Vendor Name" UniqueName="VendorName">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="DocumentNumber" HeaderText="Document No" UniqueName="DocumentNumber">
                                </telerik:GridBoundColumn>
                                <telerik:GridDateTimeColumn DataField="Posting_date" HeaderText="Posting Date" FilterControlWidth="300px" DataFormatString="{0:dd/MM/yyyy}">
                                </telerik:GridDateTimeColumn>
                                <telerik:GridBoundColumn DataField="Nature_of_Request" HeaderText="Nature of Request" UniqueName="Nature_of_Request">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Approval_status" HeaderText="Approval status" UniqueName="Approval_status">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Remark" HeaderText="Remark" UniqueName="Remark">
                                </telerik:GridBoundColumn>
                                <telerik:GridDateTimeColumn DataField="Net_Due_Date" HeaderText="Net Due Date" FilterControlWidth="300px" DataFormatString="{0:dd/MM/yyyy}">
                                </telerik:GridDateTimeColumn>
                                <telerik:GridBoundColumn DataField="Amount_in_INR" HeaderText="Amount in INR" UniqueName="Amount_in_INR">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="AmountProposed" HeaderText="Amount Proposed" DataFormatString="{0:#,###}">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="ApprovedAmount" HeaderText="Approved Proposed" DataFormatString="{0:#,###}">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Currency" HeaderText="Currency" UniqueName="Currency">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="GrossAmount" HeaderText="Gross Amount" UniqueName="GrossAmount">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="TaxRate" HeaderText="Tax Rate" UniqueName="TaxRate">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="TaxAmount" HeaderText="Tax Amount" UniqueName="TaxAmount">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Net_Amount" HeaderText="Net Amount" UniqueName="Net_Amount">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="BusinessArea" HeaderText="Business Area" UniqueName="BusinessArea">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="ProfitCentre" HeaderText="Profit Centre" UniqueName="ProfitCentre">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="HouseBank" HeaderText="HouseBank" UniqueName="HouseBank">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="PaymentMethod" HeaderText="PaymentMethod" UniqueName="PaymentMethod">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="PaymentDocumentNumber" HeaderText="Payment Document Number" UniqueName="PaymentDocumentNumber">
                                </telerik:GridBoundColumn>
                                <telerik:GridDateTimeColumn DataField="Payment_Document_Posting_Date" HeaderText="Payment Document Posting Date" FilterControlWidth="300px" DataFormatString="{0:dd/MM/yyyy}">
                                </telerik:GridDateTimeColumn>
                                <telerik:GridBoundColumn DataField="CheckLotNumber" HeaderText="CheckLotNumber" UniqueName="CheckLotNumber">
                                </telerik:GridBoundColumn>
                                <telerik:GridDateTimeColumn DataField="SubmissionDate" HeaderText="SubmissionDate" FilterControlWidth="300px" DataFormatString="{0:dd/MM/yyyy}">
                                </telerik:GridDateTimeColumn>
                                <telerik:GridBoundColumn DataField="Treasury__Allocation_No" HeaderText="Treasury Allocation No" UniqueName="Treasury__Allocation_No">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Name_of_Initiator" HeaderText="Name of Initiator" UniqueName="Name_of_Initiator">
                                </telerik:GridBoundColumn>
                                <telerik:GridDateTimeColumn DataField="Reversal_Date" HeaderText="Reversal Date" FilterControlWidth="300px" DataFormatString="{0:dd/MM/yyyy}">
                                </telerik:GridDateTimeColumn>
                                <telerik:GridBoundColumn DataField="Unique_Reference_number" HeaderText="Unique Reference number" UniqueName="Unique_Reference_number">
                                </telerik:GridBoundColumn>
                            </Columns>
                            <PagerStyle PageSizes="5,10,15" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                                PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />
                        </MasterTableView>
                    </telerik:RadGrid>
                </div>
                <telerik:RadNotification RenderMode="Lightweight" Position="Center" runat="server" ID="radMessage" VisibleOnPageLoad="false"
                    Width="450px" AutoCloseDelay="0" TitleIcon="none" ContentIcon="none">
                </telerik:RadNotification>
            </div>
        </div>
    </div>
</asp:Content>
