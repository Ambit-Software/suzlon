<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/SolarPMS.Master" AutoEventWireup="true" CodeBehind="SurveyMaster.aspx.cs" Inherits="SolarPMS.Admin.SurveyMaster" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script>


        function onRequestStart(sender, args) {
            if (args.get_eventTarget().indexOf("btnExcel") >= 0)
                args.set_enableAjax(false);
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="../Scripts/Page/SurveyMaster.js"></script>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <ClientEvents OnRequestStart="onRequestStart" />
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="gridSurvey">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="gridSurvey" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <%--  <telerik:AjaxUpdatedControl ControlID="lblErrorMessage"></telerik:AjaxUpdatedControl>--%>
                    <telerik:AjaxUpdatedControl ControlID="RadNotification1"></telerik:AjaxUpdatedControl>

                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnAddNew">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="gridSurvey" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
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
    min-height: 27px !important;
    color: #FFF;
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
        .RadGrid .rgInput, .RadGrid .rgEditRow > td > [type="text"], .RadGrid .rgEditForm td > [type="text"], .RadGrid .rgBatchContainer > [type="text"], .RadGrid .rgFilterBox, .RadGrid .rgFilterApply, .RadGrid .rgFilterCancel {
    
    width: 65% !important;
}
    </style>

    <div class="col-xs-12 padding-0">
        <div class="col-xs-6 heading-big">
            <h5 class="margin-0 lineheight-30 breath-ctrl">Home / Administration / <span style="font-weight: normal !important">Survey Master</span></h5>
        </div>
        <div class="col-xs-6 padding-0">

            <div class="col-xs-12">

                <div class="list-btn">
                    <ul>
                        <li>
                            <telerik:RadButton RenderMode="Lightweight" ID="btnAddNew" runat="server" Text="Add Survey" OnClick="btnAddNew_Click">
                                <Icon PrimaryIconCssClass="rbAdd"></Icon>
                            </telerik:RadButton>
                        </li>
                        <li style="min-width: 84px;">
                            <telerik:RadAsyncUpload runat="server" ID="BtnImport1" HideFileInput="true" OnClientFileUploaded="callAjaxRequest"
                                OnFileUploaded="BtnImport1_FileUploaded" TargetFolder="~/Upload/Survey/"
                                MaxFileInputsCount="1">

                                <Localization Select="" />
                            </telerik:RadAsyncUpload>
                        </li>
                        <li>
                            <telerik:RadButton RenderMode="Lightweight" ID="btnExcel" runat="server" Text="Export" OnClick="btnExcel_Click">
                                <Icon PrimaryIconCssClass="rbDownload"></Icon>
                            </telerik:RadButton>
                        </li>

                    </ul>
                </div>
                <div class="col-sm-3 col-xs-12 pull-right padding-0">
                    <%-- <telerik:RadButton RenderMode="Lightweight" ID="btnAddNew" CssClass="pull-right" runat="server" Text="Add Survey" OnClick="btnAddNew_Click">
                <Icon PrimaryIconCssClass="rbAdd"></Icon>
            </telerik:RadButton>--%>
                </div>
                <div class="col-sm-4 col-xs-12 padding-0 pull-right padding-0">
                    <div class="col-sm-5 col-xs-12">
                        <%--<telerik:RadAsyncUpload runat="server" ID="BtnImport1" HideFileInput="true" OnClientFileUploaded="callAjaxRequest"
                    OnFileUploaded="BtnImport1_FileUploaded" TargetFolder="~/Upload/Survey/" 
                    MaxFileInputsCount="1">

                    <Localization Select="Import" />
                </telerik:RadAsyncUpload>--%>
                    </div>

                    <div class="col-sm-7 col-xs-12 pull-right ">
                        <%--<telerik:RadButton RenderMode="Lightweight" ID="btnExcel" CssClass="pull-right" runat="server" Text="Export" OnClick="btnExcel_Click">
                    <Icon PrimaryIconCssClass="rbDownload"></Icon>
                </telerik:RadButton>--%>
                    </div>

                </div>

            </div>
        </div>
    </div>
    <div class="clearfix"></div>



    <div class="padding-lr-10">

        <div class="text-center">
            <asp:Label ID="lblErrorMessage" runat="server" CssClass="alert alert-warning" Visible="false"></asp:Label>
        </div>
        <div id="grid overflow-x">


            <telerik:RadGrid RenderMode="Lightweight" ID="gridSurvey" runat="server" OnNeedDataSource="SurveyGridNeedDataSource"
                AutoGenerateColumns="false" MasterTableView-CommandItemSettings-AddNewRecordText="Add Survey" ExportSettings-ExportOnlyData="true"
                AllowPaging="true" AllowSorting="true" AllowFilteringByColumn="false" PageSize="<%# Convert.ToInt32(SolarPMS.Models.Constants.CONST_GRID_PAGE_SIZE) %>"
                OnInsertCommand="gridSurvey_InsertCommand" OnUpdateCommand="gridSurvey_UpdateCommand" OnItemDataBound="gridSurvey_ItemDataBound" OnCancelCommand="gridSurvey_CancelCommand" OnItemCommand="gridSurvey_ItemCommand">
                <GroupingSettings CaseSensitive="false" />
                <ExportSettings IgnorePaging="true" FileName="SurveyMasterMaster"></ExportSettings>
                <MasterTableView CommandItemSettings-ShowRefreshButton="false" ClientDataKeyNames="surveyId" EditMode="InPlace" CommandItemDisplay="None" AllowFilteringByColumn="true" TableLayout="Fixed">
                    
                    <CommandItemSettings ShowRefreshButton="false" ShowExportToExcelButton="false" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Site" HeaderStyle-Width="250px" UniqueName="Site" DataField="Site">
                            <ItemTemplate>
                                <%# Eval("Site") %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Label ID="lblSite" runat="server" Visible="False" Text='<%# Eval("Site") %>'> </asp:Label>
                                <telerik:RadDropDownList AutoPostBack="true"  CausesValidation="false" OnSelectedIndexChanged="drpSite_SelectedIndexChanged" RenderMode="Lightweight" ID="drpSite" runat="server" DefaultMessage="Select Site" DropDownHeight="110px">
                                </telerik:RadDropDownList>
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="drpSite"
                                    Display="Static" ErrorMessage="* Required!" ForeColor="Red" CssClass="validator" />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Project" HeaderStyle-Width="250px" UniqueName="Project" DataField="SAPProjectId">
                            <ItemTemplate>
                                <%# Eval("ProjectDescription") %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Label ID="lblProject" runat="server" Visible="False" Text='<%# Eval("SAPProjectId") %>'></asp:Label>
                                <telerik:RadDropDownList RenderMode="Lightweight"  ID="drpProject" runat="server"
                                    DefaultMessage="Select Project" DropDownHeight="110px">
                                </telerik:RadDropDownList>
                                <asp:RequiredFieldValidator runat="server" ID="ReqProjValidator" ControlToValidate="drpProject"
                                    Display="Static" ErrorMessage="* Required!" ForeColor="Red" CssClass="validator" />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Village" HeaderStyle-Width="250px" UniqueName="Village" DataField="villageName">
                            <ItemTemplate>
                                <%# Eval("villageName") %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Label ID="lblVillage" runat="server" Visible="False" Text='<%# Eval("villageId") %>'></asp:Label>
                                <telerik:RadDropDownList RenderMode="Lightweight"  ID="drpVillage" runat="server" DefaultMessage="Select Village" DropDownHeight="110px">
                                </telerik:RadDropDownList>
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator3" ControlToValidate="drpVillage"
                                    Display="Static" ErrorMessage="* Required!" ForeColor="Red" CssClass="validator" />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Area" UniqueName="Area"  DataField="Area" HeaderStyle-Width="120px" HeaderStyle-HorizontalAlign="Left">
                         <ItemStyle HorizontalAlign="Right" />
                             <ItemTemplate>
                                <%# Eval("Area") %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <%--    <telerik:RadNumericTextBox MinValue="0" MaxValue="999999999"  MaxLength="9" ID="txtArea" Text='<%#Bind("Area") %>' runat="server">
                                    <NumberFormat GroupSeparator="" DecimalDigits="2" KeepNotRoundedValue="false" />
                                    <ClientEvents OnKeyPress="KeyPress" />
                                </telerik:RadNumericTextBox>--%>
                                <%--        <telerik:RadNumericTextBox ID="txtArea" runat="server" Text='<%#Bind("Area") %>' MinValue="0"
                            MaxValue="100" Width="50px">
                            <NumberFormat AllowRounding="true" DecimalDigits="2" />
                                <ClientEvents OnKeyPress="NewKeyPress" />
                        </telerik:RadNumericTextBox>--%>
                                <telerik:RadNumericTextBox ID="txtArea" Text='<%#Bind("Area") %>' runat="server" MaxLength="12">
                                    <NumberFormat DecimalDigits="2" DecimalSeparator="." AllowRounding="true" />
                                </telerik:RadNumericTextBox>



                                <%--  <telerik:RadTextBox ID="txtPropsedTotal" TextMode="SingleLine" Text='<%#Bind("propsedTotal") %>' runat="server">
                                    <ClientEvents OnKeyPress="numberOnly" />
                                </telerik:RadTextBox>--%>
                                <asp:RequiredFieldValidator ID="ReqArea" runat="server" ErrorMessage="* Required!" ForeColor="Red" ControlToValidate="txtArea"></asp:RequiredFieldValidator>

                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Survey No." HeaderStyle-Width="130px"  UniqueName="SurveyNo" DataField="surveyNo">
                            <ItemTemplate>
                                <%# Eval("surveyNo") %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtSurveyNo" MaxLength="50" Text='<%#Bind("surveyNo") %>' runat="server">
                                </telerik:RadTextBox>
                                <asp:RequiredFieldValidator ID="ReqSurveyNo" runat="server" ErrorMessage="* Required!" ForeColor="Red" ControlToValidate="txtSurveyNo"></asp:RequiredFieldValidator>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="No. of Division" HeaderStyle-Width="100px" UniqueName="NoOfDivision" HeaderStyle-HorizontalAlign="Left" DataField="noOfDivision">
                               <ItemStyle HorizontalAlign="Right" />
                              <ItemTemplate>
                                <%# Eval("noOfDivision") %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <%-- <telerik:RadTextBox ID="txtNoOfDivision" MaxLength="9"  Width="100px" TextMode="SingleLine" Text='<%#Bind("noOfDivision") %>' runat="server">
                                    <ClientEvents OnKeyPress="numberOnly" />
                                </telerik:RadTextBox>--%>
                                <telerik:RadNumericTextBox MinValue="0" MaxValue="999999999" MaxLength="9" ID="txtNoOfDivision" Text='<%#Bind("noOfDivision") %>' runat="server">
                              
                                     <NumberFormat GroupSeparator="" DecimalDigits="0" KeepNotRoundedValue="false" />
                                    <ClientEvents OnKeyPress="KeyPress" />
                                </telerik:RadNumericTextBox>

                                <asp:RequiredFieldValidator ID="ReqNoOdDiv" runat="server" ErrorMessage="* Required!" ForeColor="Red" ControlToValidate="txtNoOfDivision"></asp:RequiredFieldValidator>

                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%--  <telerik:GridCheckBoxColumn DataField="Status" HeaderText="Enabled" UniqueName="chkStatus" DefaultInsertValue="true" DataType="System.Boolean" ShowFilterIcon="false" AllowFiltering="false">
                        </telerik:GridCheckBoxColumn>--%>

                        <telerik:GridEditCommandColumn HeaderText="Edit" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" Exportable="false">
                            <HeaderStyle Width="150px" />
                        </telerik:GridEditCommandColumn>
                    </Columns>
                    <PagerStyle PageSizes="5,10,15" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                        PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />
                </MasterTableView>
            </telerik:RadGrid>
        </div>
        <telerik:RadNotification RenderMode="Lightweight" Position="Center" runat="server" ID="RadNotification1" VisibleOnPageLoad="false"
            Width="450px" TitleIcon="none" ContentIcon="none" Style="z-index: 100000" EnableRoundedCorners="true" EnableShadow="true" AnimationDuration="1000">
        </telerik:RadNotification>
    </div>
</asp:Content>
