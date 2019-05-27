<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/SolarPMS.Master" AutoEventWireup="true" CodeBehind="VillageMaster.aspx.cs" Inherits="SolarPMS.Admin.VillageMaster" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="../Scripts/Page/Common.js"></script>

    <script>


        function onRequestStart(sender, args) {
            if (args.get_eventTarget().indexOf("btnExcel") >= 0)
                args.set_enableAjax(false);
        }

    </script>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <ClientEvents OnRequestStart="onRequestStart" />
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="gridVillages">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="gridVillages" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="lblErrorMessage"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnAddNew">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="gridVillages" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="LoadingPanel1" runat="server" Skin="Default">
    </telerik:RadAjaxLoadingPanel>

    <%--        <div class="col-xs-12 margin-0">
        <div>
        <telerik:RadAsyncUpload runat="server" ID="btnImport" HideFileInput="true" OnClientFileUploaded="callAjaxRequest"
            OnFileUploaded="btnImport_FileUploaded" CssClass="pull-right btn-right " TargetFolder="~/Upload/Village/" AllowedFileExtensions=".xlsx,.xls"
            MaxFileInputsCount="1">
            <Localization Select="Import" />
        </telerik:RadAsyncUpload>
          
        </div>
         
        </div>--%>
    <style type="text/css">
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
            <h5 class="margin-0 lineheight-30 breath-ctrl">Home / Administration / <span style="font-weight: normal !important">Village Master</span></h5>
        </div>

        <div class="col-xs-6">
            <div class="col-xs-12 padding-0">
                <div class="list-btn">
                    <ul>
                        <li>
                            <telerik:RadButton RenderMode="Lightweight" ID="btnAddNew" runat="server" Text="Add Village" OnClick="btnAddNew_Click">
                                <Icon PrimaryIconCssClass="rbAdd"></Icon>
                            </telerik:RadButton>
                        </li>

                        <li style="min-width: 84px;">
                            <telerik:RadAsyncUpload runat="server" ID="btnImport" HideFileInput="true" OnClientFileUploaded="callAjaxRequest"
                                OnFileUploaded="btnImport_FileUploaded" TargetFolder="~/Upload/Village/"
                                MaxFileInputsCount="1">

                                <Localization Select="" />
                            </telerik:RadAsyncUpload>
                        </li>

                        <li>
                            <telerik:RadButton RenderMode="Lightweight" CssClass="pull-right" ID="btnExcel" runat="server" Text="Export" OnClick="btnExcel_Click">
                                <Icon PrimaryIconCssClass="rbDownload"></Icon>
                            </telerik:RadButton>
                        </li>

                    </ul>
                </div>
            </div>
        </div>

    </div>

    <div class="clearfix"></div>

    <div class="col-xs-12 ">
        <%--<div class="col-sm-10 col-xs-12">
            
        </div>
        <div class="col-sm-2 col-xs-12 padding-0">
            <div class="col-sm-5 col-xs-12">--%>
        <%--<telerik:RadAsyncUpload runat="server" ID="btnImport" HideFileInput="false" OnClientFileUploaded="callAjaxRequest"
                    OnFileUploaded="btnImport_FileUploaded" TargetFolder="~/Upload/Village/" AllowedFileExtensions=".xlsx,.xls"
                    MaxFileInputsCount="1">
                    <Localization Select="Import" />
                </telerik:RadAsyncUpload>--%>


        <%-- </div>

            <div class="col-sm-7 col-xs-12">
                
            </div>

        </div>--%>
    </div>

    <div class="padding-lr-10">

        <div class="text-center alertbackground">

            <asp:Label ID="lblErrorMessage" CssClass="alert alert-warning" runat="server" Visible="false"></asp:Label>
        </div>
        <div id="grid">
            <telerik:GridTextBoxColumnEditor runat="server" ID="TextboxEditor">
                <TextBoxStyle Width="30%" />
            </telerik:GridTextBoxColumnEditor>
            <telerik:RadGrid RenderMode="Lightweight" ID="gridVillages" runat="server" OnNeedDataSource="LocationGridNeedDataSource"
                AutoGenerateColumns="false" MasterTableView-CommandItemSettings-AddNewRecordText="Add Village" AllowMultiRowEdit="false"
                OnExcelMLWorkBookCreated="gridVillages_ExcelMLWorkBookCreated" ExportSettings-ExportOnlyData="true" OnCancelCommand="gridVillages_CancelCommand"
                AllowPaging="true" AllowSorting="true" AllowFilteringByColumn="true" PageSize="<%# Convert.ToInt32(SolarPMS.Models.Constants.CONST_GRID_PAGE_SIZE) %>"
                OnItemCommand="gridVillages_ItemCommand" OnInsertCommand="gridVillages_InsertCommand" OnUpdateCommand="gridVillages_UpdateCommand" OnItemDataBound="gridVillages_ItemDataBound">
                <GroupingSettings CaseSensitive="false" />
                <ExportSettings IgnorePaging="true" FileName="VillageMaster"></ExportSettings>
                <MasterTableView DataKeyNames="VillageId" EditMode="InPlace" CommandItemDisplay="None" EnableNoRecordsTemplate="true">
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
                        <telerik:GridBoundColumn DataField="VillageName" MaxLength="30" HeaderText="Village" ColumnEditorID="TextboxEditor" DataType="System.String">
                            <ColumnValidationSettings EnableRequiredFieldValidation="true" ModelErrorMessage-ForeColor="Red">
                                <RequiredFieldValidator ErrorMessage="*Required" Display="Static" ForeColor="Red"></RequiredFieldValidator>
                            </ColumnValidationSettings>
                        </telerik:GridBoundColumn>
                        <%--<telerik:GridCheckBoxColumn DataField="Status" HeaderText="Enabled" UniqueName="chkStatus" DefaultInsertValue="true" DataType="System.Boolean" ShowFilterIcon="false" AllowFiltering="false">
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
                            <HeaderStyle Width="100px" />
                        </telerik:GridEditCommandColumn>

                    </Columns>
                    <PagerStyle PageSizes="5,10,15" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                        PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />
                </MasterTableView>
            </telerik:RadGrid>
        </div>
        <telerik:RadNotification RenderMode="Lightweight" Position="Center" runat="server" ID="RadNotification1" VisibleOnPageLoad="false"
            Width="450px" AutoCloseDelay="0" TitleIcon="none" ContentIcon="none">
        </telerik:RadNotification>
    </div>
</asp:Content>
