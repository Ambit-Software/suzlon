<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="SuzlonBPP.Master" CodeBehind="IFSCCodeMaster.aspx.cs" Inherits="SuzlonBPP.IFSCCodeMaster" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="Scripts/bootstrap.min.js"></script>
    <script>
        function oncheckedChaned(obj) {
            var chkid = obj.id;
            if ($("#" + chkid).prop("checked") == false) {
                if (confirm("Are you sure you want to disable?")) {
                    $("#" + chkid).prop('checked', false);
                }
                else {
                    $("#" + chkid).prop('checked', true);
                }
            }
        }
    </script>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="grdIFSCCode">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdIFSCCode" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="radMessage" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="LoadingPanel1" runat="server" Skin="Default">
    </telerik:RadAjaxLoadingPanel>
    <div class="row margin-0">
        <div class="col-xs-6 heading-big">
            <h5 class="margin-0 lineheight-42 breath-ctrl">Home / <span>IFSC Code</span></h5>
        </div>
        <div class="col-sm-6 col-xs-6 list-btn padding-t-6">
            <ul class="list-btn ">
                <li>
                    <telerik:RadButton RenderMode="Lightweight" ID="btnAddNew" runat="server" Text="Add IFSC Code " OnClick="btnAddNew_Click">
                        <Icon PrimaryIconCssClass="rbAdd"></Icon>
                    </telerik:RadButton>
                </li>
            </ul>
        </div>
    </div>
    <div class="col-xs-12 overflow-h padding-lr-10">
        <div id="grid">
            <telerik:GridTextBoxColumnEditor runat="server" ID="TextboxEditor">
                <TextBoxStyle Width="100%" />
            </telerik:GridTextBoxColumnEditor>
            <telerik:RadGrid RenderMode="Lightweight" ID="grdIFSCCode" runat="server" AutoGenerateColumns="false" AllowPaging="true" AllowSorting="true"
                AllowFilteringByColumn="true" PageSize="<%# Convert.ToInt32(SuzlonBPP.Models.Constants.CONST_GRID_PAGE_SIZE) %>" OnInsertCommand="grdIFSCCode_InsertCommand"
                OnNeedDataSource="grdIFSCCode_NeedDataSource" OnUpdateCommand="grdIFSCCode_UpdateCommand"
                OnItemCommand="grdIFSCCode_ItemCommand" AllowMultiRowEdit="false" OnItemCreated="grdIFSCCode_ItemCreated" GroupingSettings-CaseSensitive="false">
                <MasterTableView EnableNoRecordsTemplate="true" CommandItemSettings-ShowRefreshButton="false" ClientDataKeyNames="IFSCCodeId" EditMode="InPlace" CommandItemDisplay="None" AllowFilteringByColumn="true">
                    <NoRecordsTemplate>
                        <table width="100%" border="0" cellpadding="20" cellspacing="20">
                            <tr>
                                <td align="center" class="txt-white">No records to display.
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <Columns>
                        <telerik:GridBoundColumn DataField="IFSCCode" MaxLength="15" HeaderText="IFSC Code" UniqueName="IFSCCode" ColumnEditorID="TextboxEditor">
                            <ColumnValidationSettings EnableRequiredFieldValidation="true" ModelErrorMessage-ForeColor="Red">
                                <RequiredFieldValidator ErrorMessage="*Required" Display="Static" ForeColor="Red"></RequiredFieldValidator>
                            </ColumnValidationSettings>
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="BankName" MaxLength="30" HeaderText="Bank Name" UniqueName="BankName" ColumnEditorID="TextboxEditor">
                            <ColumnValidationSettings EnableRequiredFieldValidation="true" ModelErrorMessage-ForeColor="Red">
                                <RequiredFieldValidator ErrorMessage="*Required" Display="Static" ForeColor="Red"></RequiredFieldValidator>
                            </ColumnValidationSettings>
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="BranchName" MaxLength="30" HeaderText="Branch Name" UniqueName="BranchName" ColumnEditorID="TextboxEditor">
                            <ColumnValidationSettings EnableRequiredFieldValidation="true" ModelErrorMessage-ForeColor="Red">
                                <RequiredFieldValidator ErrorMessage="*Required" Display="Static" ForeColor="Red"></RequiredFieldValidator>
                            </ColumnValidationSettings>
                        </telerik:GridBoundColumn>
                        <telerik:GridCheckBoxColumn DataField="Status" HeaderText="Status" UniqueName="chkStatus" DefaultInsertValue="true"
                            HeaderStyle-Width="100" DataType="System.Boolean" ShowFilterIcon="false" AllowFiltering="false" />
                        <telerik:GridEditCommandColumn HeaderText="Edit">
                            <HeaderStyle Width="" />
                        </telerik:GridEditCommandColumn>
                    </Columns>
                    <PagerStyle PageSizes="5,10,15" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                        PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />
                </MasterTableView>
            </telerik:RadGrid>
        </div>
        <telerik:RadNotification RenderMode="Lightweight" Position="Center" runat="server" ID="radMessage" VisibleOnPageLoad="false"
            Width="450px" Animation="Fade" TitleIcon="none" ContentIcon="none" Style="z-index: 100000" EnableRoundedCorners="true" EnableShadow="true" AnimationDuration="1000">
        </telerik:RadNotification>
    </div>
</asp:Content>
