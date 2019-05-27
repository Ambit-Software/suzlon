<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="SuzlonBPP.Master" CodeBehind="NatureRequest.aspx.cs" Inherits="SuzlonBPP.NatureRequest" %>

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
            <telerik:AjaxSetting AjaxControlID="grdRequest">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdRequest" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="radMessage" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="LoadingPanel1" runat="server" Skin="Default">
    </telerik:RadAjaxLoadingPanel>
    <div class="row margin-0">
        <div class="col-xs-6 heading-big">
            <h5 class="margin-0 lineheight-42 breath-ctrl">Home / <span>Nature of Request</span></h5>
        </div>
        <div class="col-sm-6 col-xs-6 list-btn padding-t-6">
            <ul class="list-btn ">
                <li>
                    <telerik:RadButton RenderMode="Lightweight" ID="btnAddNew" runat="server" Text="Add Nature of Request " OnClick="btnAddNew_Click">
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
            <telerik:RadGrid RenderMode="Lightweight" ID="grdRequest" runat="server" AutoGenerateColumns="false"
                AllowPaging="true" AllowSorting="true" AllowFilteringByColumn="true" PageSize="<%# Convert.ToInt32(SuzlonBPP.Models.Constants.CONST_GRID_PAGE_SIZE) %>" OnInsertCommand="grdRequest_InsertCommand" OnNeedDataSource="grdRequest_NeedDataSource" OnUpdateCommand="grdRequest_UpdateCommand"
                OnItemDataBound="grdRequest_ItemDataBound" OnItemCommand="grdRequest_ItemCommand" AllowMultiRowEdit="false" OnItemCreated="grdRequest_ItemCreated" GroupingSettings-CaseSensitive="false">
                <MasterTableView EnableNoRecordsTemplate="true" CommandItemSettings-ShowRefreshButton="false" ClientDataKeyNames="RequestId" EditMode="InPlace" CommandItemDisplay="None" AllowFilteringByColumn="true">
                    <NoRecordsTemplate>
                        <table width="100%" border="0" cellpadding="20" cellspacing="20">
                            <tr>
                                <td align="center" class="txt-white">No records to display.
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <Columns>
                        <telerik:GridBoundColumn DataField="Name" HeaderStyle-Width="23%" MaxLength="30" HeaderText="Nature of Request" UniqueName="Request" ColumnEditorID="TextboxEditor">
                            <ColumnValidationSettings EnableRequiredFieldValidation="true" ModelErrorMessage-ForeColor="Red">
                                <RequiredFieldValidator ErrorMessage="*Required" Display="Static" ForeColor="Red"></RequiredFieldValidator>
                            </ColumnValidationSettings>
                        </telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn DataField="Description" HeaderText="Description" HeaderStyle-Width="35%" UniqueName="Description" SortExpression="description">
                            <ItemTemplate>
                                <%# Eval("description") %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtDescription" Text='<%#Bind("description") %>' runat="server" Width="90%" MaxLength="150" TextMode="MultiLine"></asp:TextBox>
                                <asp:RegularExpressionValidator runat="server" ID="valInput" ForeColor="Red"
                                    ControlToValidate="txtDescription"
                                    ValidationExpression="^[\s\S]{0,150}$"
                                    ErrorMessage="<%# SuzlonBPP.Models.Constants.CONST_ERROR_MAX_150_CHARACTERS %>"
                                    Display="Static"></asp:RegularExpressionValidator>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Type" HeaderStyle-Width="22%" UniqueName="Type" DataField="Type">
                            <ItemTemplate>
                                <%# Eval("Type") %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Label ID="lblType" runat="server" Visible="False" Text='<%# Eval("Type") %>'></asp:Label>
                                <telerik:RadDropDownList RenderMode="Lightweight" ID="drpType" runat="server" DefaultMessage="Select Type" />
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator" ControlToValidate="drpType"
                                    Display="Static" ErrorMessage="*Required" ForeColor="Red" CssClass="validator" />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridCheckBoxColumn DataField="Status" HeaderText="Status" UniqueName="chkStatus" DefaultInsertValue="true"
                            HeaderStyle-Width="50" DataType="System.Boolean" ShowFilterIcon="false" AllowFiltering="false" />
                        <telerik:GridEditCommandColumn HeaderText="Edit" HeaderStyle-Width="10%" />
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
