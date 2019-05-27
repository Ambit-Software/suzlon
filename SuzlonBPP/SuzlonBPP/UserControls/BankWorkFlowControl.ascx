<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BankWorkFlowControl.ascx.cs" Inherits="SuzlonBPP.UserControls.BankWorkFlowControl" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<div class="container-fluid padding-0">
    <div class="col-xs-12">
        <div class="row">
            <div class="col-sm-12 padding-lr-10">
                <div class="panel-group" id="accordion" role="tablist" aria-multiselectable="true">
                    <div class="panel panel-default">
                        <asp:Panel runat="server" class="panel-heading" role="tab" ID="headerPanel">
                            <h4 class="panel-title">
                                <a data-toggle="collapse" data-parent="#accordion" aria-expanded="true" aria-controls="collapseOne" style="color:#333 !important;">
                                    <asp:Label ID="lblName" runat="server" />
                                </a>
                                <span>
                                    <div class="caret pull-right margin-t-10"></div>
                                </span>
                            </h4>
                        </asp:Panel>
                        <div class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingOne">
                            <div class="panel-body">
                                <table class="table table-striped table-bordered">
                                    <thead>
                                        <tr>
                                            <th class="text-center">Actors</th>
                                            <th class="text-center">Vertical Controller</th>
                                            <th class="text-center">Group Controller</th>
                                            <th class="text-center">Treasury</th>
                                            <th class="text-center">Management Assurance</th>
                                            <th class="text-center">F&A SSC</th>
                                            <th class="text-center">C&B</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr>
                                            <td class="valign">Primary Actor
                                            </td>
                                            <td class="valign">
                                                <asp:DropDownList EnableViewState="false" ViewStateMode="Disabled" ID="DrpPriVerCont" DataTextField="Name" DataValueField="Id" runat="server" CssClass="form-control col-xs-12" ValidationGroup="ValidateMe" />
                                                <asp:RequiredFieldValidator ErrorMessage="* Required" Display="Dynamic" ForeColor="Red" runat="server" ControlToValidate="DrpPriVerCont" ValidationGroup="ValidateMe" />
                                            </td>
                                            <td class="valign">
                                                <asp:DropDownList ID="DrpPriGrpCont" DataTextField="Name" DataValueField="Id" runat="server" CssClass="form-control col-xs-12" ValidationGroup="ValidateMe" />
                                                <asp:RequiredFieldValidator ErrorMessage="* Required" Display="Dynamic" ForeColor="Red" runat="server" ControlToValidate="DrpPriGrpCont" ValidationGroup="ValidateMe" />
                                            </td>

                                            <td class="valign">
                                                <asp:DropDownList ID="DrpPriTreasury" DataTextField="Name" DataValueField="Id" runat="server" CssClass="form-control col-xs-12" ValidationGroup="ValidateMe" />
                                                <asp:RequiredFieldValidator ErrorMessage="* Required" Display="Dynamic" ForeColor="Red" runat="server" ControlToValidate="DrpPriTreasury" ValidationGroup="ValidateMe" />
                                            </td>

                                            <td class="valign">
                                                <asp:DropDownList ID="DrpPriMgmtAss" DataTextField="Name" DataValueField="Id" runat="server" CssClass="form-control col-xs-12" ValidationGroup="ValidateMe" />
                                                <asp:RequiredFieldValidator ErrorMessage="* Required" Display="Dynamic" ForeColor="Red" runat="server" ControlToValidate="DrpPriMgmtAss" ValidationGroup="ValidateMe" />
                                            </td>
                                            <td class="valign">
                                                <asp:DropDownList ID="DrpPriFASSC" DataTextField="Name" DataValueField="Id" runat="server" CssClass="form-control col-xs-12" ValidationGroup="ValidateMe" />
                                                <asp:RequiredFieldValidator ErrorMessage="* Required" Display="Dynamic" ForeColor="Red" runat="server" ControlToValidate="DrpPriFASSC" ValidationGroup="ValidateMe" />
                                            </td>
                                            <td class="valign">
                                                <asp:DropDownList ID="DrpPriCB" DataTextField="Name" DataValueField="Id" runat="server" CssClass="form-control col-xs-12" ValidationGroup="ValidateMe" />
                                                <asp:RequiredFieldValidator ErrorMessage="* Required" Display="Dynamic" ForeColor="Red" runat="server" ControlToValidate="DrpPriCB" ValidationGroup="ValidateMe" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="valign">Secondary Actor
                                            </td>
                                            <td class="valign">
                                                <asp:DropDownList ID="DrpSecVerCont" DataTextField="Name" DataValueField="Id" runat="server" CssClass="form-control col-xs-12" ValidationGroup="ValidateMe" />
                                                <asp:CompareValidator ID="cmpDrpVerCont" runat="server" ForeColor="Red" CssClass="display-none" ValidationGroup="ValidateMe"
                                                    ControlToValidate="DrpPriVerCont" ControlToCompare="DrpSecVerCont" Operator="NotEqual" Text="Should not equal to Primary Actor"
                                                    Type="String" />
                                            </td>
                                            <td class="valign">
                                                <asp:DropDownList ID="DrpSecGrpCont" DataTextField="Name" DataValueField="Id" runat="server" CssClass="form-control col-xs-12" ValidationGroup="ValidateMe" />
                                                <asp:CompareValidator ID="cmpDrpGrpCont" runat="server" ForeColor="Red" CssClass="display-none" ValidationGroup="ValidateMe"
                                                    ControlToValidate="DrpPriGrpCont" ControlToCompare="DrpSecGrpCont" Operator="NotEqual" Text="Should not equal to Primary Actor"
                                                    Type="String" />
                                            </td>
                                            <td class="valign">
                                                <asp:DropDownList ID="DrpSecTreasury" DataTextField="Name" DataValueField="Id" runat="server" CssClass="form-control col-xs-12" ValidationGroup="ValidateMe" />
                                                <asp:CompareValidator ID="cmpDrpTreasury" runat="server" ForeColor="Red" CssClass="display-none" ValidationGroup="ValidateMe"
                                                    ControlToValidate="DrpPriTreasury" ControlToCompare="DrpSecTreasury" Operator="NotEqual" Text="Should not equal to Primary Actor"
                                                    Type="String" />
                                            </td>
                                            <td class="valign">
                                                <asp:DropDownList ID="DrpSecMgmtAss" DataTextField="Name" DataValueField="Id" runat="server" CssClass="form-control col-xs-12" ValidationGroup="ValidateMe" />
                                                <asp:CompareValidator ID="cmpDrpMgmtAss" runat="server" ForeColor="Red" CssClass="display-none" ValidationGroup="ValidateMe"
                                                    ControlToValidate="DrpPriMgmtAss" ControlToCompare="DrpSecMgmtAss" Operator="NotEqual" Text="Should not equal to Primary Actor"
                                                    Type="String" />
                                            </td>
                                            <td class="valign">
                                                <asp:DropDownList ID="DrpSecFASSC" DataTextField="Name" DataValueField="Id" runat="server" CssClass="form-control col-xs-12" ValidationGroup="ValidateMe" />
                                                <asp:CompareValidator ID="cmpDrpFASSC" runat="server" ForeColor="Red" CssClass="display-none" ValidationGroup="ValidateMe"
                                                    ControlToValidate="DrpPriFASSC" ControlToCompare="DrpSecFASSC" Operator="NotEqual" Text="Should not equal to Primary Actor"
                                                    Type="String" />
                                            </td>
                                            <td class="valign">
                                                <asp:DropDownList ID="DrpSecCB" DataTextField="Name" DataValueField="Id" runat="server" CssClass="form-control col-xs-12" ValidationGroup="ValidateMe" />
                                                <asp:CompareValidator ID="cmpDrpCB" runat="server" ForeColor="Red" CssClass="display-none" ValidationGroup="ValidateMe"
                                                    ControlToValidate="DrpPriCB" ControlToCompare="DrpSecCB" Operator="NotEqual" Text="Should not equal to Primary Actor"
                                                    Type="String" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="valign">Secondary Actor Validity
                                            </td>
                                            <td class="valign">
                                                <telerik:RadDatePicker RenderMode="Lightweight" ID="DpFromVerCont" runat="server" ValidationGroup="ValidateMe" />
                                                <asp:RequiredFieldValidator ErrorMessage="* Required" Display="Dynamic" ForeColor="Red" runat="server" ControlToValidate="DpFromVerCont" ValidationGroup="ValidateMe" />
                                                <span style="color: Red; display: none">Should be greater than equal to Current Date</span>
                                                <div class="nav-divider">&nbsp;</div>
                                                <telerik:RadDatePicker RenderMode="Lightweight" ID="DpToVerCont" runat="server" ValidationGroup="ValidateMe" />
                                                <asp:RequiredFieldValidator ErrorMessage="* Required" Display="Dynamic" ForeColor="Red" runat="server" ControlToValidate="DpToVerCont" ValidationGroup="ValidateMe" />
                                                <asp:CompareValidator ID="cmpVerCont" ControlToCompare="DpFromVerCont" CssClass="display-none" ValidationGroup="ValidateMe"
                                                    ControlToValidate="DpToVerCont" ForeColor="Red" Type="Date" Operator="GreaterThanEqual"
                                                    ErrorMessage="* Should be greater than equal to From Date" runat="server" />
                                            </td>
                                            <td class="valign">
                                                <telerik:RadDatePicker RenderMode="Lightweight" ID="DpFromGrpCont" runat="server" ValidationGroup="ValidateMe" />
                                                <asp:RequiredFieldValidator ErrorMessage="* Required" Display="Dynamic" ForeColor="Red" ValidationGroup="ValidateMe" runat="server" ControlToValidate="DpFromGrpCont" />
                                                <span style="color: Red; display: none">Should be greater than equal to Current Date</span>
                                                <div class="nav-divider">&nbsp;</div>
                                                <telerik:RadDatePicker RenderMode="Lightweight" ID="DpToGrpCont" runat="server" ValidationGroup="ValidateMe" />
                                                <asp:RequiredFieldValidator ErrorMessage="* Required" Display="Dynamic" ForeColor="Red" ValidationGroup="ValidateMe" runat="server" ControlToValidate="DpToGrpCont" />
                                                <asp:CompareValidator ID="cmpGrpCont" ControlToCompare="DpFromGrpCont" CssClass="display-none" ValidationGroup="ValidateMe"
                                                    ControlToValidate="DpToGrpCont" ForeColor="Red" Type="Date" Operator="GreaterThanEqual"
                                                    ErrorMessage="* Should be greater than equal to From Date" runat="server" />
                                            </td>
                                            <td class="valign">
                                                <telerik:RadDatePicker RenderMode="Lightweight" ID="DpFromTreasury" runat="server" ValidationGroup="ValidateMe" />
                                                <asp:RequiredFieldValidator ErrorMessage="* Required" Display="Dynamic" ForeColor="Red" ValidationGroup="ValidateMe" runat="server" ControlToValidate="DpFromTreasury" />
                                                <span style="color: Red; display: none" class="valCurrentDate">Should be greater than equal to Current Date</span>
                                                <div class="nav-divider">&nbsp;</div>
                                                <telerik:RadDatePicker RenderMode="Lightweight" ID="DpToTreasury" runat="server" ValidationGroup="ValidateMe" />
                                                <asp:RequiredFieldValidator ErrorMessage="* Required" Display="Dynamic" ForeColor="Red" ValidationGroup="ValidateMe" runat="server" ControlToValidate="DpToTreasury" />
                                                <asp:CompareValidator ID="cmpTreasury" ControlToCompare="DpFromTreasury" CssClass="display-none" ValidationGroup="ValidateMe"
                                                    ControlToValidate="DpToTreasury" ForeColor="Red" Type="Date" Operator="GreaterThanEqual"
                                                    ErrorMessage="* Should be greater than equal to From Date" runat="server" />
                                            </td>
                                            <td class="valign">
                                                <telerik:RadDatePicker RenderMode="Lightweight" ID="DpFromMgmtAss" runat="server" ValidationGroup="ValidateMe" />
                                                <asp:RequiredFieldValidator ErrorMessage="* Required" Display="Dynamic" ForeColor="Red" ValidationGroup="ValidateMe" runat="server" ControlToValidate="DpFromMgmtAss" />
                                                <span style="color: Red; display: none">Should be greater than equal to Current Date</span>
                                                <div class="nav-divider">&nbsp;</div>
                                                <telerik:RadDatePicker RenderMode="Lightweight" ID="DpToMgmtAss" runat="server" ValidationGroup="ValidateMe" />
                                                <asp:RequiredFieldValidator ErrorMessage="* Required" Display="Dynamic" ForeColor="Red" runat="server" ValidationGroup="ValidateMe" ControlToValidate="DpToMgmtAss" />
                                                <asp:CompareValidator ID="cmpMgmtAss" ControlToCompare="DpFromMgmtAss" CssClass="display-none"
                                                    ControlToValidate="DpToMgmtAss" ForeColor="Red" Type="Date" Operator="GreaterThanEqual"
                                                    ErrorMessage="* Should be greater than equal to From Date" runat="server" />
                                            </td>
                                            <td class="valign">
                                                <telerik:RadDatePicker RenderMode="Lightweight" ID="DpFromFASCC" runat="server" ValidationGroup="ValidateMe" />
                                                <asp:RequiredFieldValidator ErrorMessage="* Required" Display="Dynamic" ForeColor="Red" runat="server" ControlToValidate="DpFromFASCC" ValidationGroup="ValidateMe" />
                                                <span style="color: Red; display: none">Should be greater than equal to Current Date</span>
                                                <div class="nav-divider">&nbsp;</div>
                                                <telerik:RadDatePicker RenderMode="Lightweight" ID="DpToFASCC" runat="server" ValidationGroup="ValidateMe" />
                                                <asp:RequiredFieldValidator ErrorMessage="* Required" Display="Dynamic" ForeColor="Red" runat="server" ControlToValidate="DpToFASCC" ValidationGroup="ValidateMe" />
                                                <asp:CompareValidator ID="cmp" ControlToCompare="DpFromFASCC" CssClass="display-none"
                                                    ControlToValidate="DpToFASCC" ForeColor="Red" Type="Date" Operator="GreaterThanEqual"
                                                    ErrorMessage="* Should be greater than equal to From Date" runat="server" />
                                            </td>
                                            <td class="valign">
                                                <telerik:RadDatePicker RenderMode="Lightweight" ID="DpFromCB" runat="server" ValidationGroup="ValidateMe" />
                                                <asp:RequiredFieldValidator ErrorMessage="* Required" Display="Dynamic" ForeColor="Red" runat="server" ControlToValidate="DpFromCB" ValidationGroup="ValidateMe" />
                                                <span style="color: Red; display: none">Should be greater than equal to Current Date</span>
                                                <div class="nav-divider">&nbsp;</div>
                                                <telerik:RadDatePicker RenderMode="Lightweight" ID="DpToCB" runat="server" ValidationGroup="ValidateMe" />
                                                <asp:RequiredFieldValidator ErrorMessage="* Required" Display="Dynamic" ForeColor="Red" runat="server" ControlToValidate="DpToCB" ValidationGroup="ValidateMe" />
                                                <asp:CompareValidator ID="cmpCB" ControlToCompare="DpFromCB" CssClass="display-none" ValidationGroup="ValidateMe"
                                                    ControlToValidate="DpToCB" ForeColor="Red" Type="Date" Operator="GreaterThanEqual"
                                                    ErrorMessage="* Should be greater than equal to From Date" runat="server" />
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                                <div class="button-wrapper margin-0">
                                    <center>
                                        <asp:Label ID="lblSave" runat="server" Text="Save" class="btn btn-primary button button-style" />
                                        <asp:HiddenField ID="hidSubVerticalId" runat="server" />
                                        <asp:HiddenField ID="hidVerticalId" runat="server" />
                                        <asp:HiddenField ID="hidWorkflowId" runat="server" />
                                        <asp:HiddenField ID="hidCreatedDate" runat="server" />
                                    </center>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
