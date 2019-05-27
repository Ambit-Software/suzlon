<%@ Page Title="Profile Management Application Role" Language="C#" MasterPageFile="SuzlonBPP.Master" AutoEventWireup="true" CodeBehind="ProfileMaster.aspx.cs" Inherits="SuzlonBPP.Admin.ProfileMaster" %>

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
    <style type="text/css">
       .RemoveEdit
       {
           display:none !important;
       }
   </style>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="grdProfile">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdProfile" LoadingPanelID="LoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="radMessage" LoadingPanelID="LoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="LoadingPanel1" runat="server" Skin="Default" />

    <div class="row margin-0">
        <div class="col-xs-6 heading-big">
            <h5 class="margin-0 lineheight-42 breath-ctrl">Home / <span>Profile</span></h5>
        </div>
        <div class="col-sm-6 col-xs-6 list-btn padding-t-6">
            <ul class="list-btn ">
                <li>
                    <telerik:RadButton RenderMode="Lightweight" ID="btnAddNew" runat="server" Text="Add Profile" OnClick="btnAddNew_Click">
                        <Icon PrimaryIconCssClass="rbAdd"></Icon>
                    </telerik:RadButton>
                </li>
            </ul>
        </div>
    </div>

    


    <div class="clearfix"></div>
    <div class="padding-lr-10">
        <div id="grid ">
            <telerik:GridTextBoxColumnEditor runat="server" ID="TextboxEditor">
                <TextBoxStyle Width="100%" />
            </telerik:GridTextBoxColumnEditor>
            <telerik:RadGrid RenderMode="Lightweight" ID="grdProfile" runat="server" AutoGenerateColumns="false" MasterTableView-CommandItemSettings-AddNewRecordText="Add Profile"
                AllowPaging="true" AllowSorting="true" AllowFilteringByColumn="true" PageSize="<%# Convert.ToInt32(SuzlonBPP.Models.Constants.CONST_GRID_PAGE_SIZE) %>" OnInsertCommand="grdProfile_InsertCommand" OnNeedDataSource="grdProfile_NeedDataSource" OnUpdateCommand="grdProfile_UpdateCommand"
                OnItemDataBound="grdProfile_ItemDataBound" AllowMultiRowEdit="false" OnItemCreated="grdProfile_ItemCreated" GroupingSettings-CaseSensitive="false" OnItemCommand="grdProfile_ItemCommand">
                <MasterTableView EnableNoRecordsTemplate="true" CommandItemSettings-ShowRefreshButton="false" ClientDataKeyNames="profileId" EditMode="InPlace" CommandItemDisplay="None" AllowFilteringByColumn="true">
                    <NoRecordsTemplate>
                        <table width="100%" border="0" cellpadding="20" cellspacing="20">
                            <tr>
                                <td align="center" class="txt-white">No records to display.
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <Columns>
                        <telerik:GridBoundColumn DataField="profileName" MaxLength="30" HeaderText="Profile Name" UniqueName="Profile" ColumnEditorID="TextboxEditor" HeaderStyle-Width="15%">
                            <ColumnValidationSettings EnableRequiredFieldValidation="true" ModelErrorMessage-ForeColor="Red">
                                <RequiredFieldValidator ErrorMessage="*Required" Display="Static" ForeColor="Red"></RequiredFieldValidator>
                            </ColumnValidationSettings>
                        </telerik:GridBoundColumn>

                        <telerik:GridTemplateColumn DataField="Description" HeaderText="Description" UniqueName="Description" SortExpression="description" HeaderStyle-Width="20%">
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
                      <%--  <telerik:GridCheckBoxColumn DataField="Status" HeaderText="Status" UniqueName="chkStatus" DefaultInsertValue="true"
                            HeaderStyle-Width="5%" DataType="System.Boolean" ShowFilterIcon="false" AllowFiltering="false"/>--%>
                         <telerik:GridTemplateColumn DataField="Status" HeaderText="Status" UniqueName="Status"  SortExpression="Status">
                            <ItemTemplate>
                                <asp:Label ID="lblStatus" Text='<%# Bind( "status") %>' runat="server" Width="70"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:CheckBox ID="chkStatus" runat="server" Checked='<%# Bind( "status") %>' />
                               
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>  

                       
                         <telerik:GridTemplateColumn HeaderText="Menu Authorization" UniqueName="MenuAuthorization" DataField="MenuAuthorization" HeaderStyle-Width="55%" >
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container.DataItem,"menuNames")%>
                            </ItemTemplate>
                            <EditItemTemplate>

                                <asp:Label ID="lblMenuAccess" runat="server" Visible="False" Text='<%# Eval("menuIds") %>'></asp:Label>
                                <telerik:RadComboBox RenderMode="Lightweight" ID="cmbMenuAccess" runat="server" CheckBoxes="true" EnableCheckAllItemsCheckBox="true"
                                    Width="200" Label="" EmptyMessage="None Selected" MarkFirstMatch="true">
                                </telerik:RadComboBox>
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="cmbMenuAccess"
                                    Display="Static" ErrorMessage="*Required" ForeColor="Red" CssClass="validator" />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        
                        
                         <telerik:GridEditCommandColumn HeaderText="Edit" HeaderStyle-Width="5%" UniqueName="EditCommandColumn">
                            <HeaderStyle Width="100px" />
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
