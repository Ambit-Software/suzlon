<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/SolarPMS.Master" AutoEventWireup="true" CodeBehind="IssueCategoryMaster.aspx.cs" Inherits="SolarPMS.Admin.IssueCategoryMaster" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <%-- <script src="../Scripts/Page/Common.js"></script>--%>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
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

            function onRequestStart(sender, args) {
                debugger;

                if (args.get_eventTarget().indexOf("BtnImport1")) {


                }


                if (args.get_eventTarget().indexOf("btnExcel") >= 0)
                    args.set_enableAjax(false);
            }

            var currentLoadingPanel = null;
            var currentUpdatedControl = null;
            function callAjaxRequest(sender, args) {
                <%--var panel = $find("<%= LoadingPanel1.ClientID %>");
                var divElementStyle = panel.get_element().style;
                divElementStyle.position = 'absolute';
                divElementStyle.left = args.get_eventTargetElement().offsetLeft + args.get_eventTargetElement().offsetWidth + "px";
                divElementStyle.top = args.get_eventTargetElement().offsetTop + "px";
                currentUpdatedControl = "<%= BtnImport1.ClientID %>";
                panel.show(currentUpdatedControl);--%>
                currentLoadingPanel = $find("<%= LoadingPanel1.ClientID %>");
                currentUpdatedControl = "<%= gridIssueCategory.ClientID %>";
                radAjaxManager1 = $find("<%= RadAjaxManager1.ClientID %>");
                currentLoadingPanel.show(currentUpdatedControl);
                notification = $find("<%= radMesaage.ClientID %>");
                notification.set_title("Import Issue Category Master");
                radAjaxManager1.ajaxRequest();s
            }

            function hideProgress() {
                debugger;
                currentLoadingPanel.hide(currentUpdatedControl);
                return true;
            }

            function ResponseEnd() {
                //hide the loading panel and clean up the global variables
                if (currentLoadingPanel != null)
                    currentLoadingPanel.hide(currentUpdatedControl);
                currentUpdatedControl = null;
                currentLoadingPanel = null;
            }

        </script>
    </telerik:RadCodeBlock>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadAjaxManager1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="BtnImport1" />
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="gridIssueCategory">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="gridIssueCategory" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="lblErrorMessage"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="radMesaage"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnAddNew">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="gridIssueCategory" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="BtnImport1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="gridIssueCategory" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>

        </AjaxSettings>
        <ClientEvents OnResponseEnd="ResponseEnd" />
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="LoadingPanel1" runat="server" Skin="Default">
    </telerik:RadAjaxLoadingPanel>

    <style type="text/css">
        @media (min-width:768px) and (max-width:1024px) {

            .list-btn ul{
                padding:0px;
            }
        }

        .RadUpload {
            width: 0px !important;
        }

        .RadUpload_Default .ruButton {
    min-height: 27px !important;
    color: transparent !important;
    background-image: url('../Content/images/import.png') !important;
    background-position: 0px -1px, 0px 0px !important;
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
            width: 77px !important;
            height: 21px !important;
        }
        .RadUpload .ruUploadSuccess {
            background-position: 0 18%;
            display: none;
        }

        .RadUpload .ruUploadSuccess, .RadUpload .ruUploadFailure, .RadUpload .ruUploadCancelled {
            background-image: none !important;
            background-repeat: no-repeat;
            background-color: transparent;
            display: none !important;
        }

        .RadNotification .rnContentWrapper {
            position: relative;
            padding: .35714286em;
            border-top-width: 1px;
            border-top-style: solid;
            border-top-color: transparent;
            overflow: visible;
        }

        .RadNotification.rnNoContentIcon .rnContent {
            overflow-y: scroll;
            overflow-x: hidden;
            max-height: 400px;
            width:100%;
        }

        .RadNotification .rnContentWrapper {
            overflow: hidden !important;
            max-height: 500px;
        }

        .RadNotification, .RadNotification * {
            max-height: 500px;
            overflow: hidden;
        }
    </style>
    
    <div class="col-xs-12 padding-0 padding-b-2">

        <div class="col-xs-6 heading-big">
            <h5 class="margin-0 lineheight-30 breath-ctrl">Home / Administration / <span style="font-weight: normal !important">Issue Category Master</span></h5>
        </div>

        <div class="col-xs-offset-2 col-md-offset-0 col-sm-offset-0 col-md-6 col-sm-6 col-xs-4 padding-lr-10    ">

            <div class="col-xs-12 padding-0">
                
                <div class="list-btn">
                    <ul>
                        <li>
                            <telerik:RadButton RenderMode="Lightweight" ID="btnAddNew" runat="server" Text="Add Issue Category" OnClick="btnAddNew_Click">
                                <Icon PrimaryIconCssClass="rbAdd"></Icon>
                            </telerik:RadButton>
                        </li>
                        <li style="min-width: 84px;">


                            <telerik:RadAsyncUpload runat="server" ID="BtnImport1" HideFileInput="true" OnClientFileUploaded="callAjaxRequest"
                                OnFileUploaded="BtnImport1_FileUploaded" TargetFolder="~/Upload/IssueCategory/"
                                MaxFileInputsCount="1">

                                <Localization Select="" />
                            </telerik:RadAsyncUpload>

                        </li>
                        <li>
                            <telerik:RadButton RenderMode="Lightweight" ID="btnExcel" CssClass="pull-right" runat="server" Text="Export" OnClick="btnExcel_Click">
                                <Icon PrimaryIconCssClass="rbDownload"></Icon>
                            </telerik:RadButton>
                        </li>


                    </ul>
                </div>

            </div>

        </div>
    </div>
    <div class="clearfix"></div>
    <div class="padding-lr-10">
        <%-- <script src="../Scripts/Page/UserMaster.js"></script>--%>
        <%--      <div class="col-xs-12 margin-0">
        <div >
            <telerik:RadAsyncUpload runat="server" ID="BtnImport1" HideFileInput="true" OnClientFileUploaded="callAjaxRequest"
                OnFileUploaded="BtnImport1_FileUploaded" CssClass="pull-right btn-right" TargetFolder="~/Upload/IssueCategory/" AllowedFileExtensions=".xlsx,.xls"
                MaxFileInputsCount="1">
                <Localization Select="Import" />
            </telerik:RadAsyncUpload>
        </div>
    </div>--%>
        <div class="text-center">
            <asp:Label ID="lblErrorMessage" runat="server" Visible="false" CssClass="alert alert-warning"></asp:Label>
        </div>
        <div class="clearfix"></div>

        <div id="grid">
            <telerik:GridTextBoxColumnEditor runat="server" ID="TextboxEditor">
                <TextBoxStyle Width="100%" />
            </telerik:GridTextBoxColumnEditor>
            <telerik:RadGrid RenderMode="Lightweight" ID="gridIssueCategory" runat="server"
                AutoGenerateColumns="false" MasterTableView-CommandItemSettings-AddNewRecordText="Add IssueCategory" ExportSettings-ExportOnlyData="true"
                AllowPaging="true" AllowSorting="true" AllowFilteringByColumn="true" PageSize="<%# Convert.ToInt32(SolarPMS.Models.Constants.CONST_GRID_PAGE_SIZE) %>" OnItemCommand="gridIssueCategory_ItemCommand" OnNeedDataSource="gridIssueCategory_NeedDataSource" OnCancelCommand="gridIssueCategory_CancelCommand" OnInsertCommand="gridIssueCategory_InsertCommand" OnUpdateCommand="gridIssueCategory_UpdateCommand" 
                OnItemDataBound="gridIssueCategory_ItemDataBound">
                <GroupingSettings CaseSensitive="false" />
                <ExportSettings IgnorePaging="true" FileName="IssueCategoryMaster"></ExportSettings>
                <MasterTableView DataKeyNames="issueCategoryId" EditMode="InPlace" CommandItemDisplay="None" EnableNoRecordsTemplate="true">
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
                        <telerik:GridBoundColumn DataField="categoryName" MaxLength="30" HeaderText="Category" ColumnEditorID="TextboxEditor"
                            HeaderStyle-Width="20%" DataType="System.String">
                            <ColumnValidationSettings EnableRequiredFieldValidation="true" ModelErrorMessage-ForeColor="Red">
                                <RequiredFieldValidator ErrorMessage="*Required" Display="Static" ForeColor="Red"></RequiredFieldValidator>
                            </ColumnValidationSettings>
                        </telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn DataField="description" HeaderText="Description" UniqueName="Description" HeaderStyle-Width="40%"
                            SortExpression="Description">
                            <ItemTemplate>
                                <asp:Label ID="lblDescription" Text='<%# Bind( "description") %>' runat="server"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtDiscription" Width="90%" Text='<%# Bind( "Description") %>' TextMode="MultiLine" runat="server"></asp:TextBox>
                                <asp:RegularExpressionValidator runat="server" ID="valInput" ForeColor="Red" Height="20px"
                                    ControlToValidate="txtDiscription"
                                    ValidationExpression="^[\s\S]{0,150}$"
                                    ErrorMessage="<%# SolarPMS.Models.Constants.CONST_ERROR_MAX_150_CHARACTERS %>"
                                    Display="Static"></asp:RegularExpressionValidator>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>                
                       <%-- <telerik:GridCheckBoxColumn DataField="Status" Visible="false" HeaderText="Enabled" UniqueName="chkStatus" DefaultInsertValue="true" DataType="System.Boolean" ShowFilterIcon="false" AllowFiltering="false">
                        </telerik:GridCheckBoxColumn>--%>
                         <telerik:GridTemplateColumn DataField="Status" HeaderText="Status" UniqueName="Status"  SortExpression="Status">
                            <ItemTemplate>
                                <asp:Label ID="lblStatus" Text='<%# Bind( "Status") %>' runat="server"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:CheckBox ID="chkStatus" runat="server" Checked='<%# Bind( "Status") %>' />
                               
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>  


                        <telerik:GridEditCommandColumn HeaderText="Edit" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" Exportable="false">
                            <HeaderStyle Width="15%" />
                        </telerik:GridEditCommandColumn>
                    </Columns>
                    <PagerStyle PageSizes="5,10,15" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                        PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />
                </MasterTableView>
            </telerik:RadGrid>
        </div>
        <telerik:RadNotification RenderMode="Lightweight" Position="Center" runat="server" ID="radMesaage" VisibleOnPageLoad="false"
            Width="450px" Animation="Fade" TitleIcon="none" ContentIcon="none" Style="z-index: 100000" EnableRoundedCorners="true" EnableShadow="true" AnimationDuration="1000">
        </telerik:RadNotification>

    </div>
</asp:Content>
