<%@ Page Title="" Language="C#" MasterPageFile="~/SuzlonBPP.Master" AutoEventWireup="true" CodeBehind="Treasury.aspx.cs" Inherits="SuzlonBPP.Treasury" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="Scripts/bootstrap.min.js"></script>

    <div class="container-fluid padding-0">

        <div class="row margin-0">
            <div class="col-xs-6 heading-big">
                <h5 class="margin-0 lineheight-42 breath-ctrl">Home / <span style="font-weight: normal !important">Treasury</span></h5>
            </div>

        </div>
        <!-- End of heading wrapper-->


        <div class="col-xs-12">

            <div class="col-md-12">
                <div class="panel with-nav-tabs panel-default border-0">
                    <div class="panel-heading padding-0  nocolor">
                        <ul class="nav nav-tabs border-0">
                            <li class="active"><a href="#tab1primary" data-toggle="tab">Request For Approval &nbsp;<span class="badge badge-green">12</span></a></li>
                            <li><a href="#tab2primary" data-toggle="tab">Addendum Requests &nbsp;<span class="badge badge-yellow">12</span></a></li>
                            <li><a href="#tab3primary" data-toggle="tab">Approved Requests &nbsp;<span class="badge badge-magenta">12</span></a></li>
                            <li><a href="#tab4primary" data-toggle="tab">Completed Requests &nbsp;<span class="badge badge-red">12</span></a></li>
                        </ul>
                    </div>
                    <div class="panel-body">
                        <div class="tab-content">
                            <div class="tab-pane fade in active overflow-x" id="tab1primary">
                                <telerik:RadGrid ID="TresuryRequestApproval" runat="server" AutoGenerateColumns="false" AllowPaging="true" AllowSorting="true"
                                    AllowFilteringByColumn="true" OnInsertCommand="grdVertical_InsertCommand"
                                    OnNeedDataSource="grdVertical_NeedDataSource" OnUpdateCommand="grdVertical_UpdateCommand"
                                    OnItemDataBound="grdVertical_ItemDataBound" OnItemCommand="grdVertical_ItemCommand" AllowMultiRowEdit="false" OnItemCreated="grdVertical_ItemCreated" GroupingSettings-CaseSensitive="false">

                                    <MasterTableView AutoGenerateColumns="False">
                                        <NoRecordsTemplate>
                                            <table width="100%" border="0" cellpadding="20" cellspacing="20">
                                                <tr>
                                                    <td align="center" class="txt-white">No records to display.
                                                    </td>
                                                </tr>
                                            </table>
                                        </NoRecordsTemplate>

                                        <ColumnGroups>
                                            <telerik:GridColumnGroup HeaderText="Utilisation Period" Name="UtilisationPeriod"></telerik:GridColumnGroup>
                                        </ColumnGroups>


                                        <Columns>

                                            <telerik:GridBoundColumn DataField="RequestDate" MaxLength="30" HeaderText="Date" UniqueName="Date" ColumnEditorID="TextboxEditor" HeaderStyle-Width="15%">
                                                <ColumnValidationSettings EnableRequiredFieldValidation="true" ModelErrorMessage-ForeColor="Red">
                                                    <RequiredFieldValidator ErrorMessage="* Required!" Display="Static" ForeColor="Red"></RequiredFieldValidator>
                                                </ColumnValidationSettings>
                                            </telerik:GridBoundColumn>

                                            <telerik:GridBoundColumn DataField="CompanyCode" MaxLength="30" HeaderText="Company Code" UniqueName="CompanyCode" ColumnEditorID="TextboxEditor" HeaderStyle-Width="15%">
                                                <ColumnValidationSettings EnableRequiredFieldValidation="true" ModelErrorMessage-ForeColor="Red">
                                                    <RequiredFieldValidator ErrorMessage="* Required!" Display="Static" ForeColor="Red"></RequiredFieldValidator>
                                                </ColumnValidationSettings>
                                            </telerik:GridBoundColumn>

                                            <telerik:GridBoundColumn DataField="SubVertical" MaxLength="30" HeaderText="Sub Vertical" UniqueName="SubVertical" ColumnEditorID="TextboxEditor" HeaderStyle-Width="15%">
                                                <ColumnValidationSettings EnableRequiredFieldValidation="true" ModelErrorMessage-ForeColor="Red">
                                                    <RequiredFieldValidator ErrorMessage="* Required!" Display="Static" ForeColor="Red"></RequiredFieldValidator>
                                                </ColumnValidationSettings>
                                            </telerik:GridBoundColumn>

                                            <telerik:GridBoundColumn DataField="TreasuryAllocationNumber" MaxLength="30" HeaderText="Treasury Allocation Number" UniqueName="TAN" ColumnEditorID="TextboxEditor" HeaderStyle-Width="15%">
                                                <ColumnValidationSettings EnableRequiredFieldValidation="true" ModelErrorMessage-ForeColor="Red">
                                                    <RequiredFieldValidator ErrorMessage="* Required!" Display="Static" ForeColor="Red"></RequiredFieldValidator>
                                                </ColumnValidationSettings>
                                            </telerik:GridBoundColumn>

                                            <telerik:GridBoundColumn DataField="NatureRequest" MaxLength="30" HeaderText="Nature of Request" UniqueName="NatureRequest" ColumnEditorID="TextboxEditor" HeaderStyle-Width="15%">
                                                <ColumnValidationSettings EnableRequiredFieldValidation="true" ModelErrorMessage-ForeColor="Red">
                                                    <RequiredFieldValidator ErrorMessage="* Required!" Display="Static" ForeColor="Red"></RequiredFieldValidator>
                                                </ColumnValidationSettings>
                                            </telerik:GridBoundColumn>

                                            <telerik:GridBoundColumn DataField="RequestedAmount" MaxLength="30" HeaderText="Requested Amount" UniqueName="RequestedAmount" ColumnEditorID="TextboxEditor" HeaderStyle-Width="15%">
                                                <ColumnValidationSettings EnableRequiredFieldValidation="true" ModelErrorMessage-ForeColor="Red">
                                                    <RequiredFieldValidator ErrorMessage="* Required!" Display="Static" ForeColor="Red"></RequiredFieldValidator>
                                                </ColumnValidationSettings>
                                            </telerik:GridBoundColumn>

                                            <telerik:GridBoundColumn DataField="ProposedDate" MaxLength="30" HeaderText="Proposed Date" UniqueName="ProposedDate" ColumnEditorID="TextboxEditor" HeaderStyle-Width="15%">
                                                <ColumnValidationSettings EnableRequiredFieldValidation="true" ModelErrorMessage-ForeColor="Red">
                                                    <RequiredFieldValidator ErrorMessage="* Required!" Display="Static" ForeColor="Red"></RequiredFieldValidator>
                                                </ColumnValidationSettings>
                                            </telerik:GridBoundColumn>

                                            <telerik:GridBoundColumn UniqueName="FromDate" DataField="FrmDate" ColumnGroupName="UtilisationPeriod" ColumnEditorID="TextboxEditor" HeaderStyle-Width="15%">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn UniqueName="ToDate" DataField="ToDate" ColumnGroupName="UtilisationPeriod" ColumnEditorID="TextboxEditor" HeaderStyle-Width="15%">
                                            </telerik:GridBoundColumn>
                                        </Columns>



                                    </MasterTableView>

                                </telerik:RadGrid>

                            </div>
                            <div class="tab-pane fade overflow-x" id="tab2primary">

                                <table class="table table-striped table-bordered">
                                    <thead>
                                        <tr>
                                            <th class="text-center valign">Treasury Allocation Number</th>
                                            <th class="text-center valign">Date</th>
                                            <th class="text-center valign">Addendum Amount</th>
                                            <th class="text-center valign">Addendum Approved Amount</th>
                                            <th class="text-center valign">Addenum Status</th>
                                            <th class="text-center valign">Nature of Request</th>



                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr class="text-center">
                                            <td class="valign">458795454</td>
                                            <td class="valign">23/05/2015</td>
                                            <td class="valign">60000.00</td>
                                            <td class="valign">40000.00</td>
                                            <td class="valign">Approved</td>
                                            <td class="valign">Labour Charge</td>
                                        </tr>
                                        <tr class="text-center">
                                            <td class="valign">45879784</td>
                                            <td class="valign">21/05/2015</td>
                                            <td class="valign">30000.00</td>
                                            <td class="valign">0.00</td>
                                            <td class="valign">Rejected</td>
                                            <td class="valign">Not Acceptable</td>
                                        </tr>






                                    </tbody>
                                </table>

                            </div>
                            <div class="tab-pane fade overflow-x" id="tab3primary">

                                <table class="table table-striped table-bordered">
                                    <thead>
                                        <tr>
                                            <th class="text-center">Date</th>
                                            <th class="text-center">Company Code</th>
                                            <th class="text-center">Sub Vertical</th>
                                            <th class="text-center">Treasury Allocation Number</th>
                                            <th class="text-center">Nature of Request</th>
                                            <th class="text-center">Requested Amount</th>
                                            <th class="text-center">Proposed Date</th>
                                            <th class="text-center" colspan="2">Utilisation Period</th>
                                            <th class="text-center">Status</th>


                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr class="text-center">
                                            <td class="valign">23/05/2015</td>
                                            <td class="valign">SAP001245</td>
                                            <td class="valign">PE</td>
                                            <td class="valign">458795454</td>
                                            <td class="valign">Request-1</td>
                                            <td class="valign">60000.00</td>
                                            <td class="valign">25/05/2016</td>
                                            <td class="valign">25/06/16</td>
                                            <td class="valign">25/06/16</td>
                                            <td class="valign">Approved</td>
                                        </tr>

                                        <tr class="text-center">
                                            <td class="valign">27/05/2015</td>
                                            <td class="valign">SAP001185</td>
                                            <td class="valign">OMS </td>
                                            <td class="valign">458685454</td>
                                            <td class="valign">Request-2</td>
                                            <td class="valign">10000.00</td>
                                            <td class="valign">27/05/2016</td>
                                            <td class="valign">25/06/16</td>
                                            <td class="valign">25/06/16</td>
                                            <td class="valign">Rejected</td>
                                        </tr>

                                        <tr class="text-center">
                                            <td class="valign">29/05/2015</td>
                                            <td class="valign">SAP001785</td>
                                            <td class="valign">OMS </td>
                                            <td class="valign">789456214</td>
                                            <td class="valign">Request-3</td>
                                            <td class="valign">100000.00</td>
                                            <td class="valign">29/05/2016</td>
                                            <td class="valign">25/06/16</td>
                                            <td class="valign">25/06/16</td>
                                            <td class="valign">Need Correction</td>
                                        </tr>




                                    </tbody>
                                </table>

                            </div>
                            <div class="tab-pane fade overflow-x" id="tab4primary">

                                <table class="table table-striped table-bordered">
                                    <thead>
                                        <tr>
                                            <th class="text-center">Date</th>
                                            <th class="text-center">Company Code</th>
                                            <th class="text-center">Sub Vertical</th>
                                            <th class="text-center">Treasury Allocation Number</th>
                                            <th class="text-center">Nature of Request</th>
                                            <th class="text-center">Requested Amount</th>
                                            <th class="text-center">Proposed Date</th>
                                            <th class="text-center" colspan="2">Utilisation Period</th>
                                            <th class="text-center">Status</th>


                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr class="text-center">
                                            <td class="valign">23/05/2015</td>
                                            <td class="valign">SAP001245</td>
                                            <td class="valign">PE</td>
                                            <td class="valign">458795454</td>
                                            <td class="valign">Request-1</td>
                                            <td class="valign">60000.00</td>
                                            <td class="valign">25/05/2016</td>
                                            <td class="valign">25/06/16</td>
                                            <td class="valign">25/06/16</td>
                                            <td class="valign">Approved</td>
                                        </tr>

                                        <tr class="text-center">
                                            <td class="valign">27/05/2015</td>
                                            <td class="valign">SAP001185</td>
                                            <td class="valign">OMS </td>
                                            <td class="valign">458685454</td>
                                            <td class="valign">Request-2</td>
                                            <td class="valign">10000.00</td>
                                            <td class="valign">27/05/2016</td>
                                            <td class="valign">25/06/16</td>
                                            <td class="valign">25/06/16</td>
                                            <td class="valign">Rejected</td>
                                        </tr>

                                        <tr class="text-center">
                                            <td class="valign">29/05/2015</td>
                                            <td class="valign">SAP001785</td>
                                            <td class="valign">OMS </td>
                                            <td class="valign">789456214</td>
                                            <td class="valign">Request-3</td>
                                            <td class="valign">100000.00</td>
                                            <td class="valign">29/05/2016</td>
                                            <td class="valign">25/06/16</td>
                                            <td class="valign">25/06/16</td>
                                            <td class="valign">Need Correction</td>
                                        </tr>




                                    </tbody>
                                </table>

                            </div>
                        </div>
                    </div>
                </div>



            </div>
        </div>
        <!-- End of grid-->

    </div>

</asp:Content>
