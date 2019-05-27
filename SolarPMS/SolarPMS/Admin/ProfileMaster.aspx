<%@ Page Title="Profile Management Application Role" Language="C#" MasterPageFile="~/MasterPages/SolarPMS.Master" AutoEventWireup="true" CodeBehind="ProfileMaster.aspx.cs" Inherits="SolarPMS.Admin.ProfileMaster" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>        
        function oncheckedChaned(obj) {          
            var chkid = obj.id;
            if ($("#" + chkid).prop("checked") == false)
          {
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
            <telerik:AjaxSetting AjaxControlID="grdProfile">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdProfile" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                 <%--<telerik:AjaxUpdatedControl ControlID="lblErrorMessage"></telerik:AjaxUpdatedControl>--%>
                    <telerik:AjaxUpdatedControl ControlID="radMesaage"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="LoadingPanel1" runat="server" Skin="Default">
    </telerik:RadAjaxLoadingPanel>

     <style type="text/css">
        .RadUpload {
            width: 0px !important;
        }

        .RadUpload_Default .ruButton {
            min-height: 26px !important;
            color: #FFF;
            background-image: url('../Content/images/import.png') !important;
            background-position: 0px 0px, 0px 0px !important;
            width: 84px !important;
            padding: 0px 0px 0px 13px !important;
            border-radius: 3px !important;
            border: 1px solid rgba(0, 0, 0, 0) !important;
        }

        .RadButton_Default.ruButton:hover {
            border-color: #aaa !important;
            color: #fff !important;
            background-color: #204d74 !important;
            width: 84px !important;
        }

        .RadButton_Default.ruButton:focus {
            box-shadow: inset 0 0 5px rgba(103,103,103,0.5);
            color: #fff;
            background-color: #204d74;
            border-color: #122b40;
            background-position: 6px 7px, 0px 0px !important;
        }

        .RadUpload .ruFileWrap {
            height: 32px !important;
        }

        div.RadUpload .ruFakeInput {
            visibility: hidden;
            width: 0;
            padding: 0;
        }

        div.RadUpload .ruFileInput {
            width: 1;
        }

        .list-btn {
            float: right;
        }

            .list-btn ul {
                float: left;
                margin: 0px 0px 0px 0px;
                list-style-type: none;
                display: inline-block;
            }

                .list-btn ul li {
                    float: left;
                    overflow: hidden;
                    margin-left: 10px;
                }

        .RadUpload_Default .ruStyled .ruFileInput {
            cursor: pointer !important;
            width: 0px !important;
            height: 0px !important;
        }
    </style>

    <div class="col-xs-12 padding-b-2">
        <div class="col-xs-6 heading-big padding-0">
            <h5 class="margin-0 lineheight-30 breath-ctrl">Home / Administration / <span style="font-weight: normal !important">Profile Master</span></h5>
        </div>

        <div class="col-xs-6 padding-0">
            <div class="col-xs-12 padding-0">
                

                <div class="list-btn">
                    <ul>
                        <li>
                            <telerik:RadButton RenderMode="Lightweight" ID="btnAddNew" runat="server" Text="Add Profile" OnClick="btnAddNew_Click">
                                <Icon PrimaryIconCssClass="rbAdd"></Icon>
                            </telerik:RadButton>
                        </li>
                    </ul>
                </div>
            </div>
        </div>
    </div>
    <div class="clearfix"></div>

    <div class="col-xs-12 padding-lr-10">
        <%--<div class="text-center" style="padding: 8px 10px 0px 10px;">
            <asp:Label ID="lblErrorMessage" CssClass="alert alert-warning" runat="server" Visible="false"></asp:Label>
        </div>--%>
        <div class="clearfix"></div>
        <div id="grid">
            <telerik:RadGrid RenderMode="Lightweight" ID="grdProfile" runat="server" AutoGenerateColumns="false"
                AllowPaging="true" AllowSorting="true" AllowFilteringByColumn="false" PageSize="15" OnInsertCommand="grdProfile_InsertCommand" OnNeedDataSource="grdProfile_NeedDataSource" OnUpdateCommand="grdProfile_UpdateCommand"
                OnItemDataBound="grdProfile_ItemDataBound" OnCancelCommand="grdProfile_CancelCommand" OnItemCreated="grdProfile_ItemCreated" OnItemCommand="grdProfile_ItemCommand">
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
                        <telerik:GridBoundColumn DataField="profileName" MaxLength="30" HeaderText="Profile" UniqueName="Prfile" ColumnEditorID="GridTextBoxEditor">
                            <ColumnValidationSettings EnableRequiredFieldValidation="true" ModelErrorMessage-ForeColor="Red">
                                <RequiredFieldValidator ErrorMessage="* Required!" Display="Static" ForeColor="Red"></RequiredFieldValidator>
                            </ColumnValidationSettings>
                        </telerik:GridBoundColumn>

                        <telerik:GridTemplateColumn HeaderText="Description" UniqueName="Description" SortExpression="description">
                            <ItemTemplate>
                                <%# Eval("description") %>
                            </ItemTemplate>
                            <EditItemTemplate>
                            
                                <asp:TextBox ID="txtDescription" Text='<%#Bind("description") %>' runat="server" Width="500px" MaxLength="150" TextMode="MultiLine"></asp:TextBox>
                                <asp:RegularExpressionValidator runat="server" ID="valInput" ForeColor="Red"
                                    ControlToValidate="txtDescription"
                                    ValidationExpression="^[\s\S]{0,150}$"
                                    ErrorMessage="<%# SolarPMS.Models.Constants.CONST_ERROR_MAX_150_CHARACTERS %>"
                                    Display="Static"></asp:RegularExpressionValidator>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%--<telerik:GridCheckBoxColumn DataField="status" HeaderText="Enabled" UniqueName="chkStatus" DataType="System.Boolean" AllowFiltering="false">
                        </telerik:GridCheckBoxColumn>--%>
                           <telerik:GridTemplateColumn DataField="status" HeaderText="Status" UniqueName="Status"  SortExpression="Status">
                            <ItemTemplate>
                                <asp:Label ID="lblStatus" Text='<%# Bind( "status") %>' runat="server"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:CheckBox ID="chkStatus" runat="server" Checked='<%# Bind( "status") %>' />
                               
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="DEDocumentUploadAccess" HeaderText="Design & Engineer Document Access">
                            <ItemTemplate>
                                <asp:Label ID="lblDocumentUpload" Text='<%# Bind("DEDocumentUploadAccess") %>' runat="server"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadDropDownList RenderMode="Lightweight" ID="ddlDocumentAccess" runat="server" Width="200">
                                </telerik:RadDropDownList>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Menu Authorization" UniqueName="MenuAuthorization" DataField="MenuAuthorization">
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container.DataItem,"menuNames")%>
                            </ItemTemplate>
                            <EditItemTemplate>

                                <asp:Label ID="lblMenuAccess" runat="server" Visible="False" Text='<%# Eval("menuIds") %>'></asp:Label>
                                <telerik:RadComboBox RenderMode="Lightweight" ID="cmbMenuAccess" runat="server" CheckBoxes="true" EnableCheckAllItemsCheckBox="true"
                                    Width="200" Label="" EmptyMessage="None Selected" Filter="Contains">
                                </telerik:RadComboBox>
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="cmbMenuAccess"
                                    Display="Static" ErrorMessage="* Required!" ForeColor="Red" CssClass="validator" />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridEditCommandColumn HeaderText="Edit">
                            <HeaderStyle Width="100px" />
                        </telerik:GridEditCommandColumn>
                    </Columns>
                    <PagerStyle PageSizes="5,10,15" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                        PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />
                </MasterTableView>
            </telerik:RadGrid>
            <telerik:RadNotification RenderMode="Lightweight" Position="Center" runat="server" ID="radMesaage" VisibleOnPageLoad="false"
                Width="450px" Animation="Fade" TitleIcon="none" ContentIcon="none" Style="z-index: 100000" EnableRoundedCorners="true" EnableShadow="true" AnimationDuration="1000">
            </telerik:RadNotification>


        </div>
    </div>
</asp:Content>
