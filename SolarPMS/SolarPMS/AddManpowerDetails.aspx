<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/SolarPMS.Master" AutoEventWireup="true" CodeBehind="AddManpowerDetails.aspx.cs" Inherits="SolarPMS.AddManpowerDetails" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="col-xs-12 heading-big">
        <h5 class="margin-0 lineheight-30 breath-ctrl">Home / <span style="font-weight: normal !important">Add Man Power Details</span></h5>
    </div>
    <div class="col-xs-12 padding-0">
        <strong>
            <asp:Label ID="lblIssueMode" runat="server" class="col-xs-12 control-label lable-txt padding-bottom user-details-color" Text="Add details"></asp:Label></strong>
    </div>
    <telerik:RadAjaxManager ID="TimesheetManagementRadAjaxManager" runat="server" EnablePageHeadUpdate="false">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="divControls">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="divControls" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="gridManPowerDetails" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnAddNew">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="gridManPowerDetails" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="radNotificationMessage"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="gridManPowerDetails">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="gridManPowerDetails" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="radNotificationMessage"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="ddlSapSite">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="ddlSapProjects" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="ddlArea" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="ddlNetwork" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="gridManPowerDetails" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="ddlSapProjects">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="ddlArea" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="ddlNetwork" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="gridManPowerDetails" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="ddlArea">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="ddlNetwork" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadNotification RenderMode="Lightweight" ID="radNotificationMessage" runat="server" Position="Center"
        Width="400" Height="100" Animation="Fade" EnableRoundedCorners="true" EnableShadow="true" AnimationDuration="500"
        ContentIcon="none" TitleIcon="none" Title="Success" Text="" Style="z-index: 100000">
    </telerik:RadNotification>
    <div class="row" runat="server" id="divControls">
        <div class="col-md-6 col-xs-12">
            <div class="col-xs-12 padding-0 padding-b-2">
                <div class="row margin-0">
                    <div class="col-md-4  col-xs-12 padding-0">
                        <label class="col-xs-12 control-label lable-txt" for="name">Site</label>
                    </div>
                    <div class="col-md-8 col-xs-12">
                        <telerik:RadComboBox RenderMode="Lightweight" ID="ddlSapSite" runat="server" AutoPostBack="true" CausesValidation="false" Height="200" Width="305" ValidationGroup="Save"
                            Skin="Office2010Silver" Filter="Contains" MarkFirstMatch="true" EnableLoadOnDemand="false" OnSelectedIndexChanged="ddlSapSite_SelectedIndexChanged">
                        </telerik:RadComboBox>
                        <asp:RequiredFieldValidator runat="server" ID="RFV1" ControlToValidate="ddlSapSite" ErrorMessage="* Required" ForeColor="Red" ValidationGroup="Save"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-md-6 col-xs-12">
            <div class="col-xs-12 padding-0 padding-b-2">
                <div class="row margin-0">
                    <div class="col-md-4  col-xs-12 padding-0">
                        <label class="col-xs-12 control-label lable-txt" for="name">Project</label>
                    </div>
                    <div class="row margin-0">
                        <div class="col-md-8 col-xs-12">
                            <telerik:RadComboBox RenderMode="Lightweight" ID="ddlSapProjects" CausesValidation="false" AutoPostBack="true" Height="200" Width="305"
                                Skin="Office2010Silver" Filter="Contains" MarkFirstMatch="true" EnableLoadOnDemand="false" OnSelectedIndexChanged="ddlSapProjects_SelectedIndexChanged" runat="server" ValidationGroup="Save">
                            </telerik:RadComboBox>
                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="ddlSapProjects" ErrorMessage="* Required" ForeColor="Red" ValidationGroup="Save"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-md-6 col-xs-12">
            <div class="col-xs-12 padding-0 padding-b-2">
                <div class="row margin-0">
                    <div class="col-md-4  col-xs-12 padding-0">
                        <label class="col-xs-12 control-label lable-txt" for="name">Area</label>
                    </div>
                    <div class="row margin-0">
                        <div class="col-md-8 col-xs-12">
                            <telerik:RadComboBox RenderMode="Lightweight" ID="ddlArea" AutoPostBack="true" CausesValidation="false" Height="200" Width="305"
                                Skin="Office2010Silver" Filter="Contains" MarkFirstMatch="true" EnableLoadOnDemand="false" DataTextField="Name" DataValueField="Id"
                                runat="server" OnSelectedIndexChanged="ddlArea_SelectedIndexChanged" ValidationGroup="Save">
                            </telerik:RadComboBox>
                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ControlToValidate="ddlArea" ErrorMessage="* Required" ForeColor="Red" ValidationGroup="Save"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-md-6 col-xs-12">
            <div class="col-xs-12 padding-0 padding-b-2">
                <div class="row margin-0">
                    <div class="col-md-4  col-xs-12 padding-0">
                        <label class="col-xs-12 control-label lable-txt" for="name">Network</label>
                    </div>
                    <div class="row margin-0">
                        <div class="col-md-8 col-xs-12">
                            <telerik:RadComboBox RenderMode="Lightweight" ID="ddlNetwork" runat="server" DefaultMessage="<%# SolarPMS.Models.Constants.CONST_SELECT_TEXT %>" AutoPostBack="true"
                                Skin="Office2010Silver" Filter="Contains" MarkFirstMatch="true" EnableLoadOnDemand="false" OnSelectedIndexChanged="ddlNetwork_SelectedIndexChanged"
                                Height="200" Width="305" CausesValidation="false" DataTextField="Name" DataValueField="Id" ValidationGroup="Save">
                            </telerik:RadComboBox>
                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator3" ControlToValidate="ddlNetwork" ErrorMessage="* Required" ForeColor="Red" ValidationGroup="Save"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-md-6 col-xs-12">
            <div class="col-xs-12 padding-0 padding-b-2">
                <div class="row margin-0">
                    <div class="col-md-4  col-xs-12 padding-0">
                        <label class="col-xs-12 control-label lable-txt" for="name">Date</label>
                    </div>
                    <div class="row margin-0">
                        <div class="col-md-8 col-xs-12">
                            <telerik:RadDatePicker AutoPostBack="true" OnSelectedDateChanged="dtDetailsDate_SelectedDateChanged" RenderMode="Lightweight" ID="dtDetailsDate"
                                DateInput-DateFormat="dd-MMM-yyyy" runat="server" Width="305" ValidationGroup="Save">
                            </telerik:RadDatePicker>
                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator4" ControlToValidate="dtDetailsDate" ErrorMessage="* Required"
                                ForeColor="Red" ValidationGroup="Save"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="grid overflow-x">
        <div class="col-md-12">
            <div>
                <div class="padding-10">
                    <telerik:RadButton RenderMode="Lightweight" ID="btnAddNew" runat="server" Text="Add Details" OnClick="btnAddNew_Click">
                        <Icon PrimaryIconCssClass="rbAdd"></Icon>
                    </telerik:RadButton>
                </div>
                <div class="table-scroll">
                    <telerik:RadGrid RenderMode="Lightweight" ID="gridManPowerDetails" runat="server" OnNeedDataSource="gridManPowerDetails_NeedDataSource"
                        AutoGenerateColumns="false" MasterTableView-CommandItemSettings-AddNewRecordText="Add Contractor" ExportSettings-ExportOnlyData="true"
                        AllowPaging="true" AllowSorting="true" AllowFilteringByColumn="true" PageSize="<%# Convert.ToInt32(SolarPMS.Models.Constants.CONST_GRID_PAGE_SIZE) %>"
                        OnInsertCommand="gridManPowerDetails_InsertCommand" OnUpdateCommand="gridManPowerDetails_UpdateCommand" OnDeleteCommand="gridManPowerDetails_DeleteCommand"
                        OnItemDataBound="gridManPowerDetails_ItemDataBound">
                        <GroupingSettings CaseSensitive="false" />
                        <ExportSettings IgnorePaging="true" FileName="ManPower"></ExportSettings>
                        <MasterTableView CommandItemSettings-ShowRefreshButton="false" DataKeyNames="Id,ContractorId,CanDelete" TableLayout="Auto"
                            CommandItemDisplay="None" EnableNoRecordsTemplate="true" EditMode="InPlace">
                            <NoRecordsTemplate>
                                <table width="100%" border="0" cellpadding="20" cellspacing="20">
                                    <tr>
                                        <td align="center">No records to display.
                                        </td>
                                    </tr>
                                </table>
                            </NoRecordsTemplate>
                            <CommandItemSettings ShowRefreshButton="false" ShowExportToExcelButton="false" />
                            <Columns>
                                <telerik:GridTemplateColumn HeaderText="Contractor" HeaderStyle-Width="180px" UniqueName="Contractor" DataField="ContractorName">
                                    <ItemTemplate>
                                        <%# Eval("ContractorName") %>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="lblContractorId" runat="server" Visible="False" Text='<%# Eval("ContractorId") %>'> </asp:Label>
                                        <telerik:RadDropDownList CausesValidation="false"
                                            RenderMode="Lightweight" ID="ddlContractor" runat="server"
                                            DefaultMessage="Select Contractor" DropDownHeight="110px">
                                        </telerik:RadDropDownList>
                                        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator8" ControlToValidate="ddlContractor"
                                            Display="Dynamic" ErrorMessage="* Required!" ForeColor="Red" CssClass="validator" />
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderText="Shift" FilterControlWidth="50px" HeaderStyle-Width="100px" UniqueName="Shift" DataField="Shift">
                                    <ItemTemplate>
                                        <%# Eval("Shift") %>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="lblShift" runat="server" Visible="False" Text='<%# Eval("Shift") %>'> </asp:Label>
                                        <telerik:RadDropDownList AutoPostBack="false" CausesValidation="false"
                                            RenderMode="Lightweight" ID="drpShift" runat="server" DefaultMessage="Select Shift" DropDownHeight="110px">
                                        </telerik:RadDropDownList>
                                        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator9" ControlToValidate="drpShift"
                                            Display="Dynamic" ErrorMessage="* Required!" ForeColor="Red" CssClass="validator" />
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderText="Unskilled" FilterControlWidth="70px" HeaderStyle-Width="120px" UniqueName="UnskilledLabourCount" DataField="UnskilledLabourCount">
                                    <ItemTemplate>
                                        <%# Eval("UnskilledLabourCount") %>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="lblLaborCount" runat="server" Visible="False" Text='<%# Eval("UnskilledLabourCount") %>'> </asp:Label>
                                        <telerik:RadTextBox CausesValidation="false" RenderMode="Lightweight" ID="txtLaborCount" runat="server" InputType="Number">
                                        </telerik:RadTextBox>
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderText="Mechanical" FilterControlWidth="70px" HeaderStyle-Width="120px" UniqueName="MechanicalLabourCount" DataField="MechanicalLabourCount">
                                    <ItemTemplate>
                                        <%# Eval("MechanicalLabourCount") %>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="lblMechanicalLabourCount" runat="server" Visible="False" Text='<%# Eval("MechanicalLabourCount") %>'> </asp:Label>
                                        <telerik:RadTextBox CausesValidation="false" RenderMode="Lightweight" ID="txtMechanicalLabourCount" runat="server" InputType="Number">
                                        </telerik:RadTextBox>
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderText="Civil" FilterControlWidth="70px" HeaderStyle-Width="120px" UniqueName="CivilLabourCount" DataField="CivilLabourCount">
                                    <ItemTemplate>
                                        <%# Eval("CivilLabourCount") %>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="lblCivilLabourCount" runat="server" Visible="False" Text='<%# Eval("CivilLabourCount") %>'> </asp:Label>
                                        <telerik:RadTextBox CausesValidation="false" RenderMode="Lightweight" ID="txtCivilLabourCount" runat="server" InputType="Number">
                                        </telerik:RadTextBox>
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderText="Electrical" FilterControlWidth="70px" HeaderStyle-Width="120px"
                                    UniqueName="ElectricalLabourCount" DataField="ElectricalLabourCount">
                                    <ItemTemplate>
                                        <%# Eval("ElectricalLabourCount") %>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="lblElectricalLabourCount" runat="server" Visible="False" Text='<%# Eval("ElectricalLabourCount") %>'> </asp:Label>
                                        <telerik:RadTextBox CausesValidation="false" RenderMode="Lightweight" ID="txtElectricalLabourCount" runat="server" InputType="Number">
                                        </telerik:RadTextBox>
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Block Numbers" HeaderStyle-Width="250px" UniqueName="BlockNumbers" DataField="BlockNumbers">
                                    <ItemTemplate>
                                        <%# Eval("BlockNumbers") %>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="lblBlockNumbers" runat="server" Visible="False" Text='<%# Eval("BlockNumbers") %>'> </asp:Label>
                                        <telerik:RadTextBox CausesValidation="false" Width="250px" RenderMode="Lightweight" ID="txtBlockNumbers" runat="server" InputType="Text" MaxLength="100">
                                        </telerik:RadTextBox>
                                     <%--   <asp:RequiredFieldValidator runat="server" ID="reqBlockNumbers" ControlToValidate="txtBlockNumbers"
                                            Display="Dynamic" ErrorMessage="* Required!" ForeColor="Red" CssClass="validator" />--%>
                                        <asp:RegularExpressionValidator runat="server" ControlToValidate="txtBlockNumbers" ValidationExpression="^[0-9]{1,9}( *, *[0-9]{1,9})*$" CssClass="validator"
                                            ErrorMessage="Please enter comma separated block numbers." Display="Dynamic" ForeColor="Red">
                                        </asp:RegularExpressionValidator>
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Comments" HeaderStyle-Width="250px" UniqueName="Comments" DataField="Comments">
                                    <ItemTemplate>
                                        <%# Eval("Comments") %>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="lblComments" runat="server" Visible="False" Text='<%# Eval("Comments") %>'> </asp:Label>
                                        <telerik:RadTextBox CausesValidation="false" TextMode="MultiLine" Width="250px" RenderMode="Lightweight" ID="txtComments" runat="server" InputType="Text" MaxLength="1000">
                                        </telerik:RadTextBox>
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridBoundColumn DataField="UserName" HeaderStyle-Width="180px" HeaderText="Username" UniqueName="UserName"></telerik:GridBoundColumn>
                                <telerik:GridEditCommandColumn HeaderText="Edit" ItemStyle-Width="150px" UniqueName="EditColumn" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" Exportable="false">
                                    <HeaderStyle Width="150px" />
                                </telerik:GridEditCommandColumn>

                                <telerik:GridButtonColumn CommandName="Delete" HeaderText="Delete" ConfirmTitle="Confirm" ConfirmDialogHeight="120" UniqueName="DeleteColumn"
                                    ConfirmDialogWidth="120" ConfirmText="Are you sure you want to delete this record?" ConfirmDialogType="Classic">
                                </telerik:GridButtonColumn>
                            </Columns>
                            <PagerStyle PageSizes="5,10,15" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                                PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />
                        </MasterTableView>
                    </telerik:RadGrid>
                </div>

            </div>
        </div>
    </div>
    <div id="divButtons" runat="server" class="col-lg-12 text-center padding-10">
        <asp:Button ID="btnCancel" runat="server" CausesValidation="false" Text="Cancel" OnClick="btnCancel_Click" CssClass="btn btn-primary button" />
    </div>
</asp:Content>
