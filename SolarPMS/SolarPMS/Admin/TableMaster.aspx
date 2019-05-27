<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/SolarPMS.Master" AutoEventWireup="true" CodeBehind="TableMaster.aspx.cs" Inherits="SolarPMS.Admin.TableMaster" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="../Scripts/Page/SurveyMaster.js"></script>

    <script>
        function onRequestStart(sender, args) {
            if (args.get_eventTarget().indexOf("btnExcel") >= 0)
                args.set_enableAjax(false);
        }

    </script>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="grdTable">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdTable" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <%--  <telerik:AjaxUpdatedControl ControlID="lblErrorMessage"></telerik:AjaxUpdatedControl>--%>
                    <telerik:AjaxUpdatedControl ControlID="RadNotification1"></telerik:AjaxUpdatedControl>

                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="drpSite">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="drpProject" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnAddNew">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdTable" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>

        </AjaxSettings>

    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="LoadingPanel1" runat="server" Skin="Default">
    </telerik:RadAjaxLoadingPanel>
    <%--       <div class="col-xs-12 margin-0">
        <div>
            <telerik:RadAsyncUpload runat="server" ID="BtnImport1" HideFileInput="true" OnClientFileUploaded="callAjaxRequest"
                OnFileUploaded="BtnImport1_FileUploaded" CssClass="pull-right btn-right " TargetFolder="~/Upload/Table/" AllowedFileExtensions=".xlsx,.xls"
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
            color: transparent;
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

    <div class="col-xs-12 padding-0 padding-b-1">
        <div class="col-xs-6 heading-big">
            <h5 class="margin-0 lineheight-30 breath-ctrl">Home / Administration / <span style="font-weight: normal !important">Table Master</span></h5>
        </div>
        <div class="col-xs-6 padding-0">
            <div class="col-xs-12  pull-right">


                <div class="list-btn">
                    <ul>
                        <li>
                            <telerik:RadButton RenderMode="Lightweight" ID="btnAddNew" runat="server" Text="Add Table" OnClick="btnAddNew_Click">
                                <Icon PrimaryIconCssClass="rbAdd"></Icon>
                            </telerik:RadButton>
                        </li>
                        <li style="min-width: 84px;">
                            <telerik:RadAsyncUpload runat="server" ID="BtnImport1" HideFileInput="true" OnClientFileUploaded="callAjaxRequest"
                                OnFileUploaded="BtnImport1_FileUploaded" TargetFolder="~/Upload/Table/"
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
    <%--<div class="clearfix"></div>--%>



    <%--<div class="text-center" style="padding: 10px 10px 0px 10px;">
        <asp:Label ID="lblErrorMessage" runat="server" CssClass="alert alert-warning" Visible="false" ></asp:Label>
    </div>--%>
    <div class="clearfix"></div>
    <div class="xol-xs-12 padding-lr-10">
        <div id="grid">
            <telerik:RadGrid RenderMode="Lightweight" ID="grdTable" ExportSettings-ExportOnlyData="true" runat="server" AutoGenerateColumns="false" MasterTableView-CommandItemSettings-AddNewRecordText="Add Table"
                AllowPaging="true" AllowSorting="true" AllowFilteringByColumn="false" PageSize="<%# Convert.ToInt32(SolarPMS.Models.Constants.CONST_GRID_PAGE_SIZE) %>" OnCancelCommand="grdTable_CancelCommand" OnInsertCommand="grdTable_InsertCommand" OnNeedDataSource="grdTable_NeedDataSource" OnUpdateCommand="grdTable_UpdateCommand" OnItemDataBound="grdTable_ItemDataBound" OnItemCommand="grdTable_ItemCommand">
                <GroupingSettings CaseSensitive="false" />
                <ExportSettings IgnorePaging="true" FileName="TableMaster"></ExportSettings>
                <MasterTableView ClientDataKeyNames="tableId" EditMode="InPlace" CommandItemDisplay="None" AllowFilteringByColumn="true">
                    <NoRecordsTemplate>
                        <table width="100%" border="0" cellpadding="20" cellspacing="20">
                            <tr>
                                <td align="center">No Table to display.
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <CommandItemSettings ShowRefreshButton="false" ShowExportToExcelButton="false" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Site" UniqueName="Site" DataField="site">
                            <ItemTemplate>
                                <%# Eval("site") %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Label ID="lblSite" runat="server" Visible="False" Text='<%# Eval("site") %>'> </asp:Label>
                                <telerik:RadDropDownList AutoPostBack="true" RenderMode="Lightweight" ID="drpSite"
                                    OnSelectedIndexChanged="drpSite_SelectedIndexChanged" CausesValidation="false" runat="server" DefaultMessage="Select Site"
                                    DropDownHeight="110px">
                                </telerik:RadDropDownList>
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="drpSite"
                                    Display="Static" ErrorMessage="* Required!" ForeColor="Red" CssClass="validator" />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Project" UniqueName="Project" DataField="projectId">
                            <ItemTemplate>
                                <%# Eval("projectDescription") %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Label ID="lblProject" runat="server" Visible="False" Text='<%# Eval("projectId") %>'></asp:Label>
                                <telerik:RadDropDownList RenderMode="Lightweight" ID="drpProject" runat="server"
                                    DefaultMessage="Select Project" DropDownHeight="110px">
                                </telerik:RadDropDownList>
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator3" ControlToValidate="drpProject"
                                    Display="Static" ErrorMessage="* Required!" ForeColor="Red" CssClass="validator" />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Block" UniqueName="Block" DataField="block">
                            <ItemTemplate>
                                <%# Eval("block") %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <%-- <telerik:RadNumericTextBox MinValue="0" MaxValue="999999999" ID="txtBlock" Text='<%#Bind("block") %>' runat="server">
                                <NumberFormat GroupSeparator="" DecimalDigits="0" KeepNotRoundedValue="false" />                              
                            </telerik:RadNumericTextBox>--%>

                                <telerik:RadTextBox ID="txtBlock" MaxLength="10" TextMode="SingleLine" Text='<%#Bind("Block") %>' runat="server">
                                </telerik:RadTextBox>
                                <asp:RequiredFieldValidator ID="ReqBlock" runat="server" ErrorMessage="* Required!" ForeColor="Red" ControlToValidate="txtBlock"></asp:RequiredFieldValidator>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Invertor" UniqueName="Invertor" DataField="invertor">
                            <ItemTemplate>
                                <%# Eval("invertor") %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <%-- <telerik:RadNumericTextBox MinValue="0" MaxValue="999999999" ID="txtInvertor" Text='<%#Bind("invertor") %>' runat="server">
                                <NumberFormat GroupSeparator="" DecimalDigits="0" KeepNotRoundedValue="false" />                               
                            </telerik:RadNumericTextBox>--%>

                                <telerik:RadTextBox ID="txtInvertor" TextMode="SingleLine" MaxLength="10" Text='<%#Bind("Invertor") %>' runat="server">
                                </telerik:RadTextBox>
                                <asp:RequiredFieldValidator ID="ReqInvertor" runat="server" ErrorMessage="* Required!" ForeColor="Red" ControlToValidate="txtInvertor"></asp:RequiredFieldValidator>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="SCB" UniqueName="SCB" DataField="scb">
                            <ItemTemplate>
                                <%# Eval("scb") %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <%--<telerik:RadNumericTextBox MinValue="0" MaxValue="999999999" ID="txtSCB" Text='<%#Bind("scb") %>' runat="server">
                                <NumberFormat GroupSeparator="" DecimalDigits="0" KeepNotRoundedValue="false" />                             
                            </telerik:RadNumericTextBox>--%>

                                <telerik:RadTextBox ID="txtSCB" MaxLength="10" TextMode="SingleLine" Text='<%#Bind("SCB") %>' runat="server">
                                </telerik:RadTextBox>
                                <asp:RequiredFieldValidator ID="ReqSCB" runat="server" ErrorMessage="* Required!" ForeColor="Red" ControlToValidate="txtSCB"></asp:RequiredFieldValidator>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Table" UniqueName="Table" DataField="table">
                            <ItemTemplate>
                                <%# Eval("table") %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <%-- <telerik:RadNumericTextBox MinValue="0" MaxValue="999999999" ID="txtTable" Text='<%#Bind("table") %>' runat="server">
                                <NumberFormat GroupSeparator="" DecimalDigits="0" KeepNotRoundedValue="false" />                               
                            </telerik:RadNumericTextBox>--%>
                                <telerik:RadTextBox ID="txtTable" MaxLength="10" TextMode="SingleLine" Text='<%#Bind("Table") %>' runat="server">
                                </telerik:RadTextBox>
                                <asp:RequiredFieldValidator ID="ReqTable" runat="server" ErrorMessage="* Required!" ForeColor="Red" ControlToValidate="txtTable"></asp:RequiredFieldValidator>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridEditCommandColumn HeaderText="Edit" Exportable="false">
                            <HeaderStyle Width="100px" />
                        </telerik:GridEditCommandColumn>

                    </Columns>
                    <PagerStyle PageSizes="5,10,15" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                        PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />
                </MasterTableView>
            </telerik:RadGrid>
            <%--  <telerik:RadNotification RenderMode="Lightweight" Position="Center" runat="server" ID="RadNotification1" VisibleOnPageLoad="false"
            Width="450px" AutoCloseDelay="0" TitleIcon="none" ContentIcon="none">
        </telerik:RadNotification>--%>
            <telerik:RadNotification RenderMode="Lightweight" Position="Center" runat="server" ID="RadNotification1" VisibleOnPageLoad="false"
                Width="450px" Animation="Fade" TitleIcon="none" ContentIcon="none" Style="z-index: 100000" EnableRoundedCorners="true" EnableShadow="true" AnimationDuration="1000">
            </telerik:RadNotification>

        </div>
    </div>


</asp:Content>
