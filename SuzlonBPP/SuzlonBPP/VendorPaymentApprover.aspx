<%@ Page Title="" Language="C#" MasterPageFile="~/SuzlonBPP.Master" AutoEventWireup="true" CodeBehind="VendorPaymentApprover.aspx.cs" Inherits="SuzlonBPP.VendorPaymentApprover" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="Scripts/bootstrap.min.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>

    </style>
    <div class="row margin-0">
            <div class="col-xs-12 heading-big">
                <h5 class="margin-0 lineheight-42 breath-ctrl">Home / <span>Advance:Approver</span></h5>
            </div>
            
        </div>
    
    <div class="row margin-0">

        <div class="col-md-4 margin-10">
            <div class="col-sm-12">
                <div class="col-sm-2 col-xs-12 padding-0">
                    <label class="control-label lable-txt" for="name">Vertical</label>
                </div>
                <div class="col-sm-10 col-xs-12 padding-0">
                    <div class="form-group">
                        <telerik:RadComboBox ID="cmbSubVertical" RenderMode="Lightweight" runat="server">
                            <Items>
                                <telerik:RadComboBoxItem Value="Select Vertical" Text="Select Vertical" />
                                <telerik:RadComboBoxItem Value="Vertical One" Text="Vertical One" />
                                <telerik:RadComboBoxItem Value="Vertical Two" Text="Vertical Two" />
                                <telerik:RadComboBoxItem Value="Vertical Three" Text="Vertical Three" />
                            </Items>
                        </telerik:RadComboBox>
                    </div>
                </div>


            </div>

        </div>

    </div>
    <!--End of heading wrapper-->

    <div class="col-md-12 margin-10">

        <div class="col-md-6">

            <div class="col-sm-12 col-xs-12 padding-0">
                <asp:RadioButton ID="rbtnBill" GroupName="rblInitiator" runat="server" />Against Bill
                 <asp:RadioButton ID="rbtnAdvance" GroupName="rblInitiator" runat="server" />Advance

            </div>
        </div>
        <!-- End of Company name-row-->


    </div>
    <div class="row margin-0">

        <div class="col-md-4">
            <div class="col-sm-12">
                <div class="col-sm-4 col-xs-12 subheading padding-0">
                    <label class="control-label lable-txt" for="name">Vertical Budget:</label>
                </div>
                <div class="col-sm-8 col-xs-12 padding-0">
                    <div class="form-group">
                        <label class="control-label lable-txt" for="name">
                            <a class="budget" data-toggle="modal" data-target="#budget-utilization-modal">20,0000</a>
                        </label>
                    </div>
                </div>


            </div>

        </div>

    </div>
    <!-- End of Company name-row-->





    <div class="row margin-10">

        <div class="col-xs-12">
            <div class="panel with-nav-tabs panel-default border-0">
                <telerik:RadTabStrip RenderMode="Lightweight" runat="server" ID="RadTabStrip1" MultiPageID="RadMultiPage1" SelectedIndex="0" Skin="Silk">
                    <Tabs>
                        <telerik:RadTab Text="My Request  For Approval"></telerik:RadTab>
                        <telerik:RadTab Text="Approved Request"></telerik:RadTab>
                        <telerik:RadTab Text="Need Correction"></telerik:RadTab>
                    </Tabs>
                </telerik:RadTabStrip>
                <telerik:RadMultiPage runat="server" ID="RadMultiPage1" SelectedIndex="0" CssClass="outerMultiPage">
                    <telerik:RadPageView runat="server" ID="RadPageView1">
                        <telerik:RadGrid RenderMode="Lightweight" ID="gvPendingApproval" runat="server" AutoGenerateColumns="false"
                            AllowPaging="true" AllowSorting="false" AllowFilteringByColumn="false" PageSize="5" OnItemDataBound="gvPendingApproval_ItemDataBound">
                            <GroupingSettings CaseSensitive="false" />
                            <ClientSettings>
                                <Selecting AllowRowSelect="true"></Selecting>
                            </ClientSettings>
                            <MasterTableView CommandItemSettings-ShowRefreshButton="false" EditMode="InPlace" ShowHeader="false" CommandItemDisplay="None"
                                AllowFilteringByColumn="false" AllowSorting="false" NoMasterRecordsText="">
                                <CommandItemSettings ShowRefreshButton="false" ShowExportToExcelButton="false" />
                                <NoRecordsTemplate>
                                    <table width="100%" border="0" cellpadding="20" cellspacing="20">
                                        <tr>
                                            <td align="center" class="txt-white">No records to display.
                                            </td>
                                        </tr>
                                    </table>
                                </NoRecordsTemplate>
                                <NestedViewTemplate>
                                    <telerik:RadGrid RenderMode="Lightweight" ID="gvPendingApproval1" runat="server" AutoGenerateColumns="false" 
                                        AllowPaging="true" AllowSorting="false" AllowFilteringByColumn="false" PageSize="5" OnItemDataBound="gvPendingApproval1_ItemDataBound">
                                        <GroupingSettings CaseSensitive="false" />
                                        <ClientSettings>
                                            <Selecting AllowRowSelect="true"></Selecting>
                                        </ClientSettings>
                                        <MasterTableView CommandItemSettings-ShowRefreshButton="false" EditMode="InPlace" ShowHeader="false" CommandItemDisplay="None"
                                            AllowFilteringByColumn="false" AllowSorting="false" NoMasterRecordsText="">
                                            <CommandItemSettings ShowRefreshButton="false" ShowExportToExcelButton="false" />
                                            <NoRecordsTemplate>
                                                <table width="100%" border="0" cellpadding="20" cellspacing="20">
                                                    <tr>
                                                        <td align="center" class="txt-white">No records to display.
                                                        </td>
                                                    </tr>
                                                </table>
                                            </NoRecordsTemplate>
                                            <NestedViewTemplate>
                                                <div class="table-scroll">
                                                    <telerik:RadGrid RenderMode="Lightweight" ID="gvPendingApproval2" runat="server" AutoGenerateColumns="false"
                                                        AllowPaging="true" AllowSorting="false" AllowFilteringByColumn="false" PageSize="5">
                                                        <GroupingSettings CaseSensitive="false" />
                                                        <ClientSettings>
                                                            <Selecting AllowRowSelect="true"></Selecting>
                                                        </ClientSettings>
                                                        <MasterTableView CommandItemSettings-ShowRefreshButton="false" EditMode="InPlace" CommandItemDisplay="None"
                                                            AllowFilteringByColumn="false" AllowSorting="false" NoMasterRecordsText="">
                                                            <CommandItemSettings ShowRefreshButton="false" ShowExportToExcelButton="false" />
                                                            <NoRecordsTemplate>
                                                                <table width="100%" border="0" cellpadding="20" cellspacing="20">
                                                                    <tr>
                                                                        <td align="center" class="txt-white">No records to display.
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </NoRecordsTemplate>
                                                            <Columns>
                                                                <telerik:GridClientSelectColumn HeaderText="Select" UniqueName="RequestSelect">
                                                                </telerik:GridClientSelectColumn>
                                                                <telerik:GridTemplateColumn HeaderText="Sub Vertical" UniqueName="SubVertical">
                                                                    <ItemTemplate>
                                                                        <telerik:RadComboBox ID="cmbSubVertical" CssClass="form-control" runat="server">
                                                                            <Items>
                                                                                <telerik:RadComboBoxItem Value="OMS" Text="OMS" />
                                                                                <telerik:RadComboBoxItem Value="OMS" Text="OMS" />
                                                                                <telerik:RadComboBoxItem Value="OMS" Text="OMS" />
                                                                            </Items>
                                                                        </telerik:RadComboBox>
                                                                    </ItemTemplate>
                                                                    <EditItemTemplate>
                                                                        <telerik:RadComboBox ID="cmbSubVertical" CssClass="form-control" runat="server">
                                                                            <Items>
                                                                                <telerik:RadComboBoxItem Value="OMS" Text="OMS" />
                                                                                <telerik:RadComboBoxItem Value="OMS" Text="OMS" />
                                                                                <telerik:RadComboBoxItem Value="OMS" Text="OMS" />
                                                                            </Items>
                                                                        </telerik:RadComboBox>
                                                                    </EditItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridBoundColumn HeaderText="Assigned By" UniqueName="AssignedBy" DataField="AssignedBy">
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridTemplateColumn HeaderText="Comments" UniqueName="Comments">
                                                                    <ItemTemplate>
                                                                        <asp:HyperLink ID="viewComments" runat="server" Text="View" CssClass="btn btn-grey gridHyperlinks"></asp:HyperLink>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridBoundColumn HeaderText="Company Code" UniqueName="CompanyCode" DataField="CompanyCode">
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn HeaderText="Vendor Code" UniqueName="VendorCode" DataField="VendorCode">
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn HeaderText="Vendor Name" UniqueName="VendorName" DataField="VendorName">
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridTemplateColumn HeaderText="Nature Of Request" UniqueName="NatureOfRequest">
                                                                    <ItemTemplate>
                                                                        <telerik:RadComboBox ID="cmbNatureofRequest" CssClass="form-control" runat="server">
                                                                            <Items>
                                                                                <telerik:RadComboBoxItem Value="Select Nature" Text="Select Nature" />
                                                                                <telerik:RadComboBoxItem Value="Select Nature" Text="Select Nature" />
                                                                                <telerik:RadComboBoxItem Value="Select Nature" Text="Select Nature" />
                                                                            </Items>
                                                                        </telerik:RadComboBox>
                                                                    </ItemTemplate>
                                                                    <EditItemTemplate>
                                                                        <telerik:RadComboBox ID="cmbNatureofRequest" CssClass="form-control" runat="server">
                                                                            <Items>
                                                                                <telerik:RadComboBoxItem Value="Select Nature" Text="Select Nature" />
                                                                                <telerik:RadComboBoxItem Value="Select Nature" Text="Select Nature" />
                                                                                <telerik:RadComboBoxItem Value="Select Nature" Text="Select Nature" />
                                                                            </Items>
                                                                        </telerik:RadComboBox>
                                                                    </EditItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridBoundColumn HeaderText="DRP Number" UniqueName="DRPNumber" DataField="DRPNumber">
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn HeaderText="Reference" UniqueName="Reference" DataField="Reference">
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn HeaderText="Fiscal Year" UniqueName="FiscalYear" DataField="FiscalYear">
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridDateTimeColumn HeaderText="Posting Date" UniqueName="PostingDate" DataField="PostDate">
                                                                </telerik:GridDateTimeColumn>
                                                                <telerik:GridBoundColumn HeaderText="Amount in INR" UniqueName="Amount" DataField="Amount">
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridTemplateColumn HeaderText="Amount Proposed" UniqueName="AmountProposed">
                                                                    <ItemTemplate>
                                                                        <telerik:RadTextBox ReadOnly="true" ID="tbAmountProposed" Width="125px" class="form-control" runat="server"></telerik:RadTextBox>
                                                                    </ItemTemplate>
                                                                    <EditItemTemplate>
                                                                        <telerik:RadTextBox ReadOnly="true" ID="tbAmountProposed" Width="125px" class="form-control" runat="server"></telerik:RadTextBox>
                                                                    </EditItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridBoundColumn HeaderText="Currency" UniqueName="Currency" DataField="Amount">
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridDateTimeColumn HeaderText="Expected Clearing Date" UniqueName="ExpectedClearingdate" DataField="Netduedate">
                                                                </telerik:GridDateTimeColumn>
                                                                <telerik:GridBoundColumn HeaderText="Business Area" UniqueName="BusinessArea" DataField="BusinessArea">
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn HeaderText="Profit Centre" UniqueName="ProfitCentre" DataField="ProfitCentre">
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn HeaderText="Purchasing Document" UniqueName="PurchasingDocument" DataField="PurchasingDocument">
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn HeaderText="PO Line Item Text" UniqueName="POLineItem" DataField="POLineItem">
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn HeaderText="Special GL" UniqueName="SpecialGL" DataField="SpecialGL">
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridTemplateColumn HeaderText="Withholding Tax Code" UniqueName="WithholdingTaxCode">
                                                                    <ItemTemplate>
                                                                        <telerik:RadTextBox ID="tbTaxCode" ReadOnly="true" runat="server"></telerik:RadTextBox>
                                                                    </ItemTemplate>
                                                                    <EditItemTemplate>
                                                                        <telerik:RadTextBox ID="tbTaxCode" ReadOnly="true" runat="server"></telerik:RadTextBox>
                                                                    </EditItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridBoundColumn HeaderText="Un-Settled Open Advance (INR)" UniqueName="UnSettledOpenAdvance" DataField="UnSettledOpenAdvance">
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridTemplateColumn HeaderText="Justification for  Advance Payment" UniqueName="AdvancePaymentJustification" DataField="AdvancePaymentJustification">
                                                                    <ItemTemplate>
                                                                        <telerik:RadTextBox ID="tbJustification" ReadOnly="true" runat="server"></telerik:RadTextBox>
                                                                    </ItemTemplate>
                                                                    <EditItemTemplate>
                                                                        <telerik:RadTextBox ID="tbJustification" ReadOnly="true" runat="server"></telerik:RadTextBox>
                                                                    </EditItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridBoundColumn HeaderText="Vertical" UniqueName="Vertical" DataField="Vertical">
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridTemplateColumn HeaderText="Attachments" UniqueName="Attachments">
                                                                    <ItemTemplate>
                                                                        <asp:HyperLink ID="viewAttachment" runat="server" Text="Browse" CssClass="btn btn-grey gridHyperlinks"></asp:HyperLink>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridDateTimeColumn HeaderText="Submission Date" UniqueName="SubmissionDate" DataField="PostDate">
                                                                </telerik:GridDateTimeColumn>
                                                            </Columns>
                                                            <PagerStyle PageSizes="5,10,15" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                                                                PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />
                                                        </MasterTableView>
                                                    </telerik:RadGrid>
                                                    <div class="col-lg-12 padding-0 margin-10">

                                                        <asp:LinkButton CssClass="btn btn-grey" runat="server" ID="LinkButton3">Save</asp:LinkButton>
                                                        <asp:LinkButton CssClass="btn btn-grey" runat="server" ID="LinkButton4">Submit</asp:LinkButton>
                                                        <a href="home_screen.html" class="btn btn-grey">Cancel</a>

                                                    </div>
                                                </div>
                                            </NestedViewTemplate>



                                            <Columns>
                                                <telerik:GridClientSelectColumn HeaderText="Select" UniqueName="Select">
                                                </telerik:GridClientSelectColumn>
                                                <telerik:GridBoundColumn HeaderText="Total Amount" UniqueName="TotalAmount" DataField="TotalAmount">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn HeaderText="Proposed Total" UniqueName="ProposedTotal" DataField="ProposedTotal">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn HeaderText="Approved Total" UniqueName="ApprovedTotal" DataField="ApprovedTotal">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn HeaderText="Today's Paid Total" UniqueName="TodayTotal" DataField="TodayTotal">
                                                </telerik:GridBoundColumn>
                                            </Columns>
                                            <PagerStyle PageSizes="5,10,15" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                                                PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />
                                        </MasterTableView>
                                    </telerik:RadGrid>
                                </NestedViewTemplate>
                                <Columns>
                                    <telerik:GridClientSelectColumn HeaderText="Select" UniqueName="Select">
                                    </telerik:GridClientSelectColumn>
                                    <telerik:GridBoundColumn HeaderText="Total Amount" UniqueName="TotalAmount" DataField="TotalAmount">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn HeaderText="Proposed Total" UniqueName="ProposedTotal" DataField="ProposedTotal">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn HeaderText="Approved Total" UniqueName="ApprovedTotal" DataField="ApprovedTotal">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn HeaderText="Today's Paid Total" UniqueName="TodayTotal" DataField="TodayTotal">
                                    </telerik:GridBoundColumn>
                                </Columns>
                                <PagerStyle PageSizes="5,10,15" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                                    PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />
                            </MasterTableView>
                        </telerik:RadGrid>
                    </telerik:RadPageView>
                    <telerik:RadPageView runat="server" ID="RadPageView2">
                             <telerik:RadGrid RenderMode="Lightweight" ID="gvApproved" runat="server" AutoGenerateColumns="false" OnItemCommand="gvApproved_ItemCommand" OnDetailTableDataBind="gvApproved_DetailTableDataBind"
                                    AllowPaging="true" AllowSorting="false" AllowFilteringByColumn="false" PageSize="5">
                                    <GroupingSettings CaseSensitive="false" />
                                    <ClientSettings>
                                        <Selecting AllowRowSelect="true"></Selecting>
                                    </ClientSettings>
                                    <MasterTableView CommandItemSettings-ShowRefreshButton="false" EditMode="InPlace" ShowHeader="false" CommandItemDisplay="None"
                                        AllowFilteringByColumn="false" AllowSorting="false" NoMasterRecordsText="">
                                        <CommandItemSettings ShowRefreshButton="false" ShowExportToExcelButton="false" />
                                        <NoRecordsTemplate>
                                            <table width="100%" border="0" cellpadding="20" cellspacing="20">
                                                <tr>
                                                    <td align="center" class="txt-white">No records to display.
                                                    </td>
                                                </tr>
                                            </table>
                                        </NoRecordsTemplate>
                                        <DetailTables>
                                            <telerik:GridTableView ShowHeader="false" Name="gvApproved1" CssClass="subactivity" DataKeyNames="i" Width="100%" HierarchyLoadMode="Conditional">
                                                <Columns>
                                                    <telerik:GridClientSelectColumn HeaderText="Select" UniqueName="Select">
                                                    </telerik:GridClientSelectColumn>
                                                    <telerik:GridBoundColumn HeaderText="Total Amount" UniqueName="TotalAmount" DataField="TotalAmount">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn HeaderText="Proposed Total" UniqueName="ProposedTotal" DataField="ProposedTotal">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn HeaderText="Approved Total" UniqueName="ApprovedTotal" DataField="ApprovedTotal">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn HeaderText="Today's Paid Total" UniqueName="TodayTotal" DataField="TodayTotal">
                                                    </telerik:GridBoundColumn>
                                                </Columns>
                                                <DetailTables>
                                                    <telerik:GridTableView ShowHeader="false" Name="gvApproved2" CssClass="subactivity" DataKeyNames="i" Width="100%" HierarchyLoadMode="Conditional">

                                                        <Columns>
                                                            <telerik:GridClientSelectColumn HeaderText="Select" UniqueName="RequestSelect">
                                                            </telerik:GridClientSelectColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Sub Vertical" UniqueName="SubVertical">
                                                                <ItemTemplate>
                                                                    <telerik:RadComboBox ID="RadComboBox1" CssClass="form-control" runat="server">
                                                                        <Items>
                                                                            <telerik:RadComboBoxItem Value="OMS" Text="OMS" />
                                                                            <telerik:RadComboBoxItem Value="OMS" Text="OMS" />
                                                                            <telerik:RadComboBoxItem Value="OMS" Text="OMS" />
                                                                        </Items>
                                                                    </telerik:RadComboBox>
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                    <telerik:RadComboBox ID="RadComboBox2" CssClass="form-control" runat="server">
                                                                        <Items>
                                                                            <telerik:RadComboBoxItem Value="OMS" Text="OMS" />
                                                                            <telerik:RadComboBoxItem Value="OMS" Text="OMS" />
                                                                            <telerik:RadComboBoxItem Value="OMS" Text="OMS" />
                                                                        </Items>
                                                                    </telerik:RadComboBox>
                                                                </EditItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridBoundColumn HeaderText="Assigned By" UniqueName="AssignedBy" DataField="AssignedBy">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Comments" UniqueName="Comments">
                                                                <ItemTemplate>
                                                                    <asp:HyperLink ID="viewComments" runat="server" Text="View" CssClass="btn btn-grey gridHyperlinks"></asp:HyperLink>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridBoundColumn HeaderText="Company Code" UniqueName="CompanyCode" DataField="CompanyCode">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn HeaderText="Vendor Code" UniqueName="VendorCode" DataField="VendorCode">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn HeaderText="Vendor Name" UniqueName="VendorName" DataField="VendorName">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Nature Of Request" UniqueName="NatureOfRequest">
                                                                <ItemTemplate>
                                                                    <telerik:RadComboBox ID="cmbNatureofRequest" CssClass="form-control" runat="server">
                                                                        <Items>
                                                                            <telerik:RadComboBoxItem Value="Select Nature" Text="Select Nature" />
                                                                            <telerik:RadComboBoxItem Value="Select Nature" Text="Select Nature" />
                                                                            <telerik:RadComboBoxItem Value="Select Nature" Text="Select Nature" />
                                                                        </Items>
                                                                    </telerik:RadComboBox>
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                    <telerik:RadComboBox ID="RadComboBox3" CssClass="form-control" runat="server">
                                                                        <Items>
                                                                            <telerik:RadComboBoxItem Value="Select Nature" Text="Select Nature" />
                                                                            <telerik:RadComboBoxItem Value="Select Nature" Text="Select Nature" />
                                                                            <telerik:RadComboBoxItem Value="Select Nature" Text="Select Nature" />
                                                                        </Items>
                                                                    </telerik:RadComboBox>
                                                                </EditItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridBoundColumn HeaderText="DRP Number" UniqueName="DRPNumber" DataField="DRPNumber">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn HeaderText="Reference" UniqueName="Reference" DataField="Reference">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn HeaderText="Fiscal Year" UniqueName="FiscalYear" DataField="FiscalYear">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridDateTimeColumn HeaderText="Posting Date" UniqueName="PostingDate" DataField="PostDate">
                                                            </telerik:GridDateTimeColumn>
                                                            <telerik:GridBoundColumn HeaderText="Amount in INR" UniqueName="Amount" DataField="Amount">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Amount Proposed" UniqueName="AmountProposed">
                                                                <ItemTemplate>
                                                                    <telerik:RadTextBox ReadOnly="true" ID="tbAmountProposed" Width="125px" class="form-control" runat="server"></telerik:RadTextBox>
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                    <telerik:RadTextBox ReadOnly="true" ID="RadTextBox1" Width="125px" class="form-control" runat="server"></telerik:RadTextBox>
                                                                </EditItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridBoundColumn HeaderText="Currency" UniqueName="Currency" DataField="Amount">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridDateTimeColumn HeaderText="Expected Clearing Date" UniqueName="ExpectedClearingdate" DataField="Netduedate">
                                                            </telerik:GridDateTimeColumn>
                                                            <telerik:GridBoundColumn HeaderText="Business Area" UniqueName="BusinessArea" DataField="BusinessArea">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn HeaderText="Profit Centre" UniqueName="ProfitCentre" DataField="ProfitCentre">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn HeaderText="Purchasing Document" UniqueName="PurchasingDocument" DataField="PurchasingDocument">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn HeaderText="PO Line Item Text" UniqueName="POLineItem" DataField="POLineItem">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn HeaderText="Special GL" UniqueName="SpecialGL" DataField="SpecialGL">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Withholding Tax Code" UniqueName="WithholdingTaxCode">
                                                                <ItemTemplate>
                                                                    <telerik:RadTextBox ID="tbTaxCode" ReadOnly="true" runat="server"></telerik:RadTextBox>
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                    <telerik:RadTextBox ID="RadTextBox2" ReadOnly="true" runat="server"></telerik:RadTextBox>
                                                                </EditItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridBoundColumn HeaderText="Un-Settled Open Advance (INR)" UniqueName="UnSettledOpenAdvance" DataField="UnSettledOpenAdvance">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Justification for  Advance Payment" UniqueName="AdvancePaymentJustification" DataField="AdvancePaymentJustification">
                                                                <ItemTemplate>
                                                                    <telerik:RadTextBox ID="tbJustification" ReadOnly="true" runat="server"></telerik:RadTextBox>
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                    <telerik:RadTextBox ID="RadTextBox3" ReadOnly="true" runat="server"></telerik:RadTextBox>
                                                                </EditItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridBoundColumn HeaderText="Vertical" UniqueName="Vertical" DataField="Vertical">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Attachments" UniqueName="Attachments">
                                                                <ItemTemplate>
                                                                    <asp:HyperLink ID="viewAttachment" runat="server" Text="Browse" CssClass="btn btn-grey gridHyperlinks"></asp:HyperLink>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridDateTimeColumn HeaderText="Submission Date" UniqueName="SubmissionDate" DataField="PostDate">
                                                            </telerik:GridDateTimeColumn>
                                                        </Columns>
                                                        <PagerStyle PageSizes="5,10,15" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                                                            PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />

                                                    </telerik:GridTableView>
                                                </DetailTables>
                                            </telerik:GridTableView>
                                        </DetailTables>
                                        <Columns>
                                            <telerik:GridClientSelectColumn HeaderText="Select" UniqueName="Select">
                                            </telerik:GridClientSelectColumn>
                                            <telerik:GridBoundColumn HeaderText="Total Amount" UniqueName="TotalAmount" DataField="TotalAmount">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn HeaderText="Proposed Total" UniqueName="ProposedTotal" DataField="ProposedTotal">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn HeaderText="Approved Total" UniqueName="ApprovedTotal" DataField="ApprovedTotal">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn HeaderText="Today's Paid Total" UniqueName="TodayTotal" DataField="TodayTotal">
                                            </telerik:GridBoundColumn>
                                        </Columns>
                                        <PagerStyle PageSizes="5,10,15" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                                            PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />
                                    </MasterTableView>
                                </telerik:RadGrid>
                    </telerik:RadPageView>
                    <telerik:RadPageView runat="server" ID="RadPageView3">
                                             <telerik:RadGrid RenderMode="Lightweight" ID="gvCorrection" runat="server" AutoGenerateColumns="false"
                                    AllowPaging="true" AllowSorting="false" AllowFilteringByColumn="false" PageSize="5">
                                    <GroupingSettings CaseSensitive="false" />
                                    <ClientSettings>
                                        <Selecting AllowRowSelect="true"></Selecting>
                                    </ClientSettings>
                                    <MasterTableView CommandItemSettings-ShowRefreshButton="false" EditMode="InPlace" ShowHeader="false" CommandItemDisplay="None"
                                        AllowFilteringByColumn="false" AllowSorting="false" NoMasterRecordsText="">
                                        <CommandItemSettings ShowRefreshButton="false" ShowExportToExcelButton="false" />
                                        <NoRecordsTemplate>
                                            <table width="100%" border="0" cellpadding="20" cellspacing="20">
                                                <tr>
                                                    <td align="center" class="txt-white">No records to display.
                                                    </td>
                                                </tr>
                                            </table>
                                        </NoRecordsTemplate>
                                        <DetailTables>
                                            <telerik:GridTableView ShowHeader="false" Name="gvApproved2" CssClass="subactivity" DataKeyNames="i" Width="100%"  HierarchyLoadMode="ServerBind">

                                                <Columns>

                                                    <telerik:GridClientSelectColumn HeaderText="Select" UniqueName="RequestSelect">
                                                    </telerik:GridClientSelectColumn>
                                                    <telerik:GridTemplateColumn HeaderText="Sub Vertical" UniqueName="SubVertical">
                                                        <ItemTemplate>
                                                            <telerik:RadComboBox ID="RadComboBox1" CssClass="form-control" runat="server">
                                                                <Items>
                                                                    <telerik:RadComboBoxItem Value="OMS" Text="OMS" />
                                                                    <telerik:RadComboBoxItem Value="OMS" Text="OMS" />
                                                                    <telerik:RadComboBoxItem Value="OMS" Text="OMS" />
                                                                </Items>
                                                            </telerik:RadComboBox>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <telerik:RadComboBox ID="RadComboBox2" CssClass="form-control" runat="server">
                                                                <Items>
                                                                    <telerik:RadComboBoxItem Value="OMS" Text="OMS" />
                                                                    <telerik:RadComboBoxItem Value="OMS" Text="OMS" />
                                                                    <telerik:RadComboBoxItem Value="OMS" Text="OMS" />
                                                                </Items>
                                                            </telerik:RadComboBox>
                                                        </EditItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridBoundColumn HeaderText="Assigned By" UniqueName="AssignedBy" DataField="AssignedBy">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridTemplateColumn HeaderText="Comments" UniqueName="Comments">
                                                        <ItemTemplate>
                                                            <asp:HyperLink ID="viewComments" runat="server" Text="View" CssClass="btn btn-grey gridHyperlinks"></asp:HyperLink>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridBoundColumn HeaderText="Company Code" UniqueName="CompanyCode" DataField="CompanyCode">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn HeaderText="Vendor Code" UniqueName="VendorCode" DataField="VendorCode">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn HeaderText="Vendor Name" UniqueName="VendorName" DataField="VendorName">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridTemplateColumn HeaderText="Nature Of Request" UniqueName="NatureOfRequest">
                                                        <ItemTemplate>
                                                            <telerik:RadComboBox ID="cmbNatureofRequest" CssClass="form-control" runat="server">
                                                                <Items>
                                                                    <telerik:RadComboBoxItem Value="Select Nature" Text="Select Nature" />
                                                                    <telerik:RadComboBoxItem Value="Select Nature" Text="Select Nature" />
                                                                    <telerik:RadComboBoxItem Value="Select Nature" Text="Select Nature" />
                                                                </Items>
                                                            </telerik:RadComboBox>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <telerik:RadComboBox ID="RadComboBox3" CssClass="form-control" runat="server">
                                                                <Items>
                                                                    <telerik:RadComboBoxItem Value="Select Nature" Text="Select Nature" />
                                                                    <telerik:RadComboBoxItem Value="Select Nature" Text="Select Nature" />
                                                                    <telerik:RadComboBoxItem Value="Select Nature" Text="Select Nature" />
                                                                </Items>
                                                            </telerik:RadComboBox>
                                                        </EditItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridBoundColumn HeaderText="DRP Number" UniqueName="DRPNumber" DataField="DRPNumber">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn HeaderText="Reference" UniqueName="Reference" DataField="Reference">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn HeaderText="Fiscal Year" UniqueName="FiscalYear" DataField="FiscalYear">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridDateTimeColumn HeaderText="Posting Date" UniqueName="PostingDate" DataField="PostDate">
                                                    </telerik:GridDateTimeColumn>
                                                    <telerik:GridBoundColumn HeaderText="Amount in INR" UniqueName="Amount" DataField="Amount">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridTemplateColumn HeaderText="Amount Proposed" UniqueName="AmountProposed">
                                                        <ItemTemplate>
                                                            <telerik:RadTextBox ReadOnly="true" ID="tbAmountProposed" Width="125px" class="form-control" runat="server"></telerik:RadTextBox>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <telerik:RadTextBox ReadOnly="true" ID="RadTextBox1" Width="125px" class="form-control" runat="server"></telerik:RadTextBox>
                                                        </EditItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridBoundColumn HeaderText="Currency" UniqueName="Currency" DataField="Amount">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridDateTimeColumn HeaderText="Expected Clearing Date" UniqueName="ExpectedClearingdate" DataField="Netduedate">
                                                    </telerik:GridDateTimeColumn>
                                                    <telerik:GridBoundColumn HeaderText="Business Area" UniqueName="BusinessArea" DataField="BusinessArea">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn HeaderText="Profit Centre" UniqueName="ProfitCentre" DataField="ProfitCentre">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn HeaderText="Purchasing Document" UniqueName="PurchasingDocument" DataField="PurchasingDocument">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn HeaderText="PO Line Item Text" UniqueName="POLineItem" DataField="POLineItem">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn HeaderText="Special GL" UniqueName="SpecialGL" DataField="SpecialGL">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridTemplateColumn HeaderText="Withholding Tax Code" UniqueName="WithholdingTaxCode">
                                                        <ItemTemplate>
                                                            <telerik:RadTextBox ID="tbTaxCode" ReadOnly="true" runat="server"></telerik:RadTextBox>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <telerik:RadTextBox ID="RadTextBox2" ReadOnly="true" runat="server"></telerik:RadTextBox>
                                                        </EditItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridBoundColumn HeaderText="Un-Settled Open Advance (INR)" UniqueName="UnSettledOpenAdvance" DataField="UnSettledOpenAdvance">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridTemplateColumn HeaderText="Justification for  Advance Payment" UniqueName="AdvancePaymentJustification" DataField="AdvancePaymentJustification">
                                                        <ItemTemplate>
                                                            <telerik:RadTextBox ID="tbJustification" ReadOnly="true" runat="server"></telerik:RadTextBox>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <telerik:RadTextBox ID="RadTextBox3" ReadOnly="true" runat="server"></telerik:RadTextBox>
                                                        </EditItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridBoundColumn HeaderText="Vertical" UniqueName="Vertical" DataField="Vertical">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridTemplateColumn HeaderText="Attachments" UniqueName="Attachments">
                                                        <ItemTemplate>
                                                            <asp:HyperLink ID="viewAttachment" runat="server" Text="Browse" CssClass="btn btn-grey gridHyperlinks"></asp:HyperLink>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridDateTimeColumn HeaderText="Submission Date" UniqueName="SubmissionDate" DataField="PostDate">
                                                    </telerik:GridDateTimeColumn>
                                                </Columns>
                                                <PagerStyle PageSizes="5,10,15" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                                                    PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />
                                                <DetailTables>
                                                    <telerik:GridTableView ShowHeader="false" Name="gvApproved1" CssClass="subactivity" DataKeyNames="i" Width="100%" HierarchyLoadMode="Conditional">
                                                        <Columns>
                                                            <telerik:GridClientSelectColumn HeaderText="Select" UniqueName="Select">
                                                            </telerik:GridClientSelectColumn>
                                                            <telerik:GridBoundColumn HeaderText="Total Amount" UniqueName="TotalAmount" DataField="TotalAmount">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn HeaderText="Proposed Total" UniqueName="ProposedTotal" DataField="ProposedTotal">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn HeaderText="Approved Total" UniqueName="ApprovedTotal" DataField="ApprovedTotal">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn HeaderText="Today's Paid Total" UniqueName="TodayTotal" DataField="TodayTotal">
                                                            </telerik:GridBoundColumn>
                                                        </Columns>
                                                        <PagerStyle PageSizes="5,10,15" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                                                            PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />

                                                    </telerik:GridTableView>
                                                </DetailTables>
                                            </telerik:GridTableView>
                                        </DetailTables>
                                        <Columns>
                                            <telerik:GridClientSelectColumn HeaderText="Select" UniqueName="Select">
                                            </telerik:GridClientSelectColumn>
                                            <telerik:GridBoundColumn HeaderText="Total Amount" UniqueName="TotalAmount" DataField="TotalAmount">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn HeaderText="Proposed Total" UniqueName="ProposedTotal" DataField="ProposedTotal">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn HeaderText="Approved Total" UniqueName="ApprovedTotal" DataField="ApprovedTotal">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn HeaderText="Today's Paid Total" UniqueName="TodayTotal" DataField="TodayTotal">
                                            </telerik:GridBoundColumn>

                                        </Columns>
                                        <PagerStyle PageSizes="5,10,15" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                                            PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />
                                    </MasterTableView>
                                </telerik:RadGrid>
                    </telerik:RadPageView>
                </telerik:RadMultiPage>
                <div class="panel-heading">
                    <ul class="nav nav-tabs">
                        <li class="active"><a href="#tab1primary" data-toggle="tab">My Request  For Approval</a></li>
                        <li><a href="#tab2primary" data-toggle="tab">Approved Request</a></li>
                        <li><a href="#tab3primary" data-toggle="tab">Need Correction</a></li>

                    </ul>
                </div>
                <div class="panel-body grid-wrapper">
                    <div class="tab-content">
                        <div class="tab-pane fade in active" id="tab1primary">
                            <div class="col-xs-12 padding-0">
                                <div class="button-wrapper margin-0">
                                    <ul>
                                        <li>
                                            <asp:LinkButton CssClass="btn btn-grey button pull-right" runat="server" ID="LinkButton5"><span class=" glyphicon glyphicon-export text-default"></span>  Export Excel</asp:LinkButton></li>
                                    </ul>
                                </div>

                                <div class="table-scroll">
                                </div>
                                <!--End of Scroll-->



                            </div>
                            <!-- End of survey table wrapper-->
                        </div>




                        <div class="tab-pane fade" id="tab2primary">
                            <div class="button-wrapper margin-0">
                                <ul>
                                    <li>
                                        <asp:LinkButton CssClass="btn btn-grey button pull-right" runat="server" ID="LinkButton6"><span class=" glyphicon glyphicon-export text-default"></span>  Export Excel</asp:LinkButton></li>
                                </ul>
                            </div>

                            <div class="table-scroll">
                           
                            </div>
                            <!--End of Scroll-->

                        </div>
                        <div class="tab-pane fade" id="tab3primary">
                            <div class="button-wrapper margin-0">
                                <ul>
                                    <li>
                                        <asp:LinkButton CssClass="btn btn-grey button pull-right" runat="server" ID="LinkButton7"><span class=" glyphicon glyphicon-export text-default"></span>  Export Excel</asp:LinkButton></li>
                                </ul>
                            </div>

                            <div class="table-scroll">
           
                            </div>

                        </div>





                    </div>
                </div>
            </div>

        </div>

    </div>

    <!-- Start of modal for addendum request-->
    <div class="modal fade" id="budget-utilization-modal" tabindex="-1" role="dialog" aria-labelledby="modalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">×</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title subheading" id="lineModalLabel">Request Already Under Process</h4>
                </div>
                <div class="modal-body" style="overflow: hidden">

                    <!-- content goes here -->
                    <table class="table table-striped table-bordered">
                        <thead>
                            <tr>
                                <th class="text-center">Document Number</th>
                                <th class="text-center">Submitted On</th>
                                <th class="text-center">Approval Status</th>

                            </tr>
                        </thead>
                        <tbody>
                            <tr class="text-center">
                                <td class="valign">12345</td>
                                <td class="valign">20/04/2016</td>
                                <td class="valign">Controller Pending-Amol Ferange</td>

                            </tr>
                            <tr class="text-center">
                                <td class="valign">23456</td>
                                <td class="valign">22/04/2016</td>
                                <td class="valign">Controller Pending-Amol Ferange</td>

                            </tr>
                            <tr class="text-center">
                                <td class="valign">34567</td>
                                <td class="valign">24/04/2016</td>
                                <td class="valign">Controller Pending-Amol Ferange</td>

                            </tr>
                        </tbody>
                    </table>

                </div>


            </div>
        </div>
    </div>
</asp:Content>
