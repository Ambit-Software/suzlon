<%@ Page Title="Upload Design Engineer Documents" Language="C#" MasterPageFile="~/MasterPages/BlankMaster.Master" AutoEventWireup="true"
    CodeBehind="UploadDEDocument.aspx.cs" Inherits="SolarPMS.UploadDEDocument" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="Scripts/Page/Common.js"></script>
    <script type="text/javascript">
        function GetRadWindow() {
            var oWindow = null;
            if (window.radWindow)
                oWindow = window.radWindow;
            else if (window.frameElement && window.frameElement.radWindow)
                oWindow = window.frameElement.radWindow;
            return oWindow;
        }

        function CloseModal() {
            GetRadWindow().close();
        }

        function onClientProgressBarUpdating(progressArea, args) {
            progressArea.updateVerticalProgressBar(args.get_progressBarElement(), args.get_progressValue());
            args.set_cancel(true);
        }

        function maxlimit(element, max) {
            var max_chars = max;
            if (element.value.length > max_chars) {
                element.value = parseInt(element.value.substr(0, max_chars));
            }
            else
                element.value = parseInt(element.value.substr(0, max_chars));
        }
    </script>
    <style>
        .RadAsyncUpload span.ruFileWrap {
            padding-left: 0;
            width: 100% !important;
        }

        .RadUpload_Default .ruSelectWrap .ruButton {
            background-color: #272727 !important;
            background-image: none !important;
            height: 34px !important;
            color: #FFF !important;
            font-weight: bold;
            border-radius: 4px !important;
            border: 0px !important;
            line-height: 24px;
        }

        .RadUpload .ruFakeInput {
            min-width: 100px !important;
            border: 1px solid #c3c3c3;
            padding: 4px 7px !important;
            border-radius: 4px;
            height: 50px !important;
            font-size: 14px !important;
            line-height: 1.42857143 !important;
        }

        .TSGrid-Wrapper {
            width: 100%;
            overflow-x: scroll;
            max-height: 200px !important;
            overflow-y: scroll;
            margin-top: 10px;
        }

        .RadUpload .ruFileWrap {
            height: auto !important;
        }

        .riSingle {
            display: block !important;
            width: 100% !important;
        }


        /** Customize the Progress area */
        .demo-container .RadUploadProgressArea {
        }


            /** Inner wrap */
            .demo-container .RadUploadProgressArea .rpaInner {
                border: 1px solid #e5e5e5;
                color: #767676;
                background: #ffffff;
                font-size: 12px;
                line-height: 1.5;
            }


            /** Progress area header */
            .demo-container .RadUploadProgressArea .rpaHeader {
                padding: 5px 10px;
                color: #ffffff;
                background: #25a0da;
                font-size: 18px;
                font-weight: 100;
            }

                .demo-container .RadUploadProgressArea .rpaHeader strong {
                    font-weight: inherit;
                }


            /** Progress area body */
            .demo-container .RadUploadProgressArea .rpaBody {
                padding: 20px;
                background: url("/Content/images/loading.gif") 80% 20px no-repeat;
            }

                .demo-container .RadUploadProgressArea .rpaBody dl {
                    margin: 0;
                    padding: 80px 0 0;
                    position: relative;
                    zoom: 1;
                }

                    .demo-container .RadUploadProgressArea .rpaBody dl:after {
                        content: "";
                        clear: both;
                        display: block;
                    }

                .demo-container .RadUploadProgressArea .rpaBody dt {
                    margin: 0 5px 10px 0;
                    padding: 0;
                    font-weight: bold;
                    float: left;
                    clear: left;
                }

                .demo-container .RadUploadProgressArea .rpaBody dd {
                    margin: 0;
                    padding: 0;
                    float: left;
                }

                .demo-container .RadUploadProgressArea .rpaBody dt.rpaStatFirst {
                    margin-bottom: 40px;
                }

                .demo-container .RadUploadProgressArea .rpaBody dd.rpaStatFirst {
                    font-size: 65px;
                    position: absolute;
                    top: 0;
                    left: 0;
                }

        .RadUpload .ruFileProgressWrap {
            margin-top: 0;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadAjaxManager ID="TimesheetManagementRadAjaxManager" runat="server" EnablePageHeadUpdate="false">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="gridDocuments">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="gridDocuments" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="radUploadDocument">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="gridDocuments" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="TimesheetManagementRadAjaxManager">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="radNotificationMessage" UpdatePanelRenderMode="Inline" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnSave">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="radNotificationMessage" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="divForm" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnAddDocument">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="radUploadDocument" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <%--<telerik:AjaxUpdatedControl ControlID="gridDocuments" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>--%>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadAsyncUpload">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="divForm" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="LoadingPanel1" runat="server" Skin="Default">
    </telerik:RadAjaxLoadingPanel>
    <telerik:RadNotification RenderMode="Lightweight" ID="radNotificationMessage" runat="server" Position="Center"
        Width="330" Height="80" Animation="Fade" EnableRoundedCorners="true" EnableShadow="true" AnimationDuration="500"
        ContentIcon="none" TitleIcon="none" Title="Success" Text="" Style="z-index: 100000">
    </telerik:RadNotification>
    <div id="divForm" runat="server">
        <div class="row margin-0">
            <div class=" col-xs-6 padding-0">
                <div class="col-sm-12 padding-0">
                    <label class="col-xs-12 control-label lable-txt text-primary" for="name" id="lblActivityName" runat="server"></label>
                </div>
                <div class="col-sm-12 padding-0">
                    <label class="col-xs-12 control-label lable-txt text-primary" for="name" id="lblSubActivityName" runat="server"></label>
                </div>
            </div>
        </div>
        <div class="row margin-0" id="divDateControls" runat="server">
            <div class="col-md-6 col-xs-12 padding-0">
                <div class="col-md-4 col-xs-12">
                    <label class="control-label lable-txt" for="name">Est. Start Date</label>
                </div>
                <div class="row margin-0">
                    <div class="col-md-7 col-xs-12 padding-0">
                        <div class="form-group">
                            <telerik:RadDatePicker DateInput-DateFormat="dd-MMM-yyyy" Enabled="false" RenderMode="Lightweight" ID="RadDatePickerEstStart" Width="100%" runat="server" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-6 col-xs-12 padding-0">
                <div class="col-md-4 col-xs-12">
                    <label class="control-label lable-txt" for="name">Est. End Date</label>
                </div>
                <div class="row margin-0">
                    <div class="col-md-7 col-xs-12 padding-0">
                        <div class="form-group">
                            <telerik:RadDatePicker DateInput-DateFormat="dd-MMM-yyyy" Enabled="false" RenderMode="Lightweight" ID="RadDatePickerEstEnd" Width="100%" runat="server" />
                        </div>
                    </div>
                </div>

            </div>

        </div>
        <div class="row margin-0" id="divAttachment" runat="server">
            <div class="col-md-6 col-xs-12 padding-0" id="divVersion" runat="server">
                <div class="col-md-4 col-xs-12">
                    <label class="control-label lable-txt" for="name">Version</label>
                </div>
                <div class="row margin-0">
                    <div class="col-md-7 col-xs-12 padding-0">
                        <div>
                            <telerik:RadTextBox ID="txtVersion" InputType="Number" MaxLength="8" runat="server" onkeydown="maxlimit(this, 8);" onkeyup="maxlimit(this, 8);"></telerik:RadTextBox>
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtVersion" ErrorMessage="*Required" ForeColor="Red"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-sm-6 padding-0">
                <div class="col-md-4 col-xs-12">
                    <label class="control-label lable-txt" for="name">Attachment</label>
                </div>
                <div class="col-md-7 col-xs-12  padding-0">
                    <div class="col-md-2 col-xs-12 padding-0">
                        <telerik:RadAsyncUpload RenderMode="Lightweight" runat="server" ID="radUploadDocument" UploadedFilesRendering="BelowFileInput" OnClientFileSelected=""
                            EnableInlineProgress="true" MultipleFileSelection="Disabled" OverwriteExistingFiles="true" HideFileInput="true" MaxFileSize="52428800"
                            ToolTip="Image must be smaller than 50MB">
                            <Localization Select="Browse" />
                        </telerik:RadAsyncUpload>
                    </div>
                    <div class="col-sm-4">
                        <asp:Button ID="btnAddDocument" runat="server" Text="Add" class="btn btn-primary"
                            Style="margin-right: 10px; margin-bottom: 5px;" OnClick="btnAddDocument_Click"></asp:Button>
                    </div>
                </div>
            </div>

        </div>
        <div class="row margin-0">
            <div class="col-md-6 col-xs-12 padding-0" style="height: 70px;">
                <div class="col-md-4 col-xs-12">
                    <label id="lblComments" runat="server" class="control-label lable-txt" for="name">Comments</label>
                </div>
                <div class="col-md-7 col-xs-12 padding-0">
                    <telerik:RadTextBox runat="server" Rows="3" TextMode="MultiLine" Height="60" ID="txtComments" MaxLength="2000"></telerik:RadTextBox>
                    <asp:RequiredFieldValidator ID="reqComments" runat="server" ControlToValidate="txtComments" ErrorMessage="*Required" ForeColor="Red"></asp:RequiredFieldValidator>
                </div>

            </div>
            <div class="col-md-6 col-xs-12 padding-0" id="divReviewDescription" runat="server" visible="false">
                <div class="col-md-4 col-xs-12">
                    <label class="control-label lable-txt" for="name">Review Description</label>
                </div>
                <div class="row margin-0">
                    <div class="col-md-7 col-xs-12 padding-0">
                        <div class="form-group">
                            <telerik:RadTextBox ID="txtReviewDescription" runat="server" TextMode="MultiLine" MaxLength="2000"></telerik:RadTextBox>
                        </div>
                    </div>
                </div>

            </div>
            <div class="col-sm-6 padding-0" id="divReleaseToConstruction" runat="server" visible="false">
                <div class="col-md-4 col-xs-12">
                    <label class="control-label lable-txt" for="name">Release to construction </label>
                </div>
                <div class="row margin-0">
                    <div class="col-md-7 col-xs-12  padding-0">
                        <telerik:RadDropDownList AutoPostBack="true" CausesValidation="false"
                            RenderMode="Lightweight" ID="drpReleaseToConstructoin" runat="server">
                            <Items>
                                <telerik:DropDownListItem Value="0" Text="No" />
                                <telerik:DropDownListItem Value="1" Text="Yes" />
                            </Items>
                        </telerik:RadDropDownList>
                    </div>
                </div>
            </div>

        </div>
        <div class="col-xs-12">
            <div class="TSGrid-Wrapper">
                <telerik:RadGrid RenderMode="Lightweight" ID="gridDocuments" Width="100%" runat="server"
                    OnItemCommand="gridDocuments_ItemCommand" OnItemDataBound="gridDocuments_ItemDataBound"
                    AutoGenerateColumns="false" AllowPaging="true" AllowSorting="true" AllowFilteringByColumn="false"
                    PageSize="<%# Convert.ToInt32(SolarPMS.Models.Constants.CONST_GRID_PAGE_SIZE) %>">
                    <GroupingSettings CaseSensitive="false" />
                    <MasterTableView Width="100%" CommandItemSettings-ShowRefreshButton="false" DataKeyNames="FileName" EditMode="InPlace" CommandItemDisplay="None" AllowFilteringByColumn="false">
                        <CommandItemSettings ShowRefreshButton="false" ShowExportToExcelButton="false" />
                        <Columns>
                            <telerik:GridHyperLinkColumn DataTextField="FileName" DataNavigateUrlFields="FileName" HeaderText="File Name" UniqueName="FileName">
                            </telerik:GridHyperLinkColumn>
                            <telerik:GridBoundColumn DataField="Comments" MaxLength="30" HeaderText="Comments" HeaderStyle-Width="130px" DataType="System.String">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Version" MaxLength="30" HeaderText="Version" HeaderStyle-Width="130px" DataType="System.String">
                            </telerik:GridBoundColumn>
                            <telerik:GridButtonColumn CommandName="Remove" Text="Remove" HeaderButtonType="LinkButton" UniqueName="Remove" HeaderText="Remove"></telerik:GridButtonColumn>
                        </Columns>
                        <PagerStyle PageSizes="5,10,15" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                            PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />
                    </MasterTableView>
                </telerik:RadGrid>
            </div>
        </div>
        <div class="row margin-0">
            <div class="col-sm-12 padding-0">
                <div class="button-wrapper margin-10 text-center">
                    <div class="submit-btn">
                        <asp:Button ID="btnSave" runat="server" Text="Save" class="btn btn-primary button button-style"
                            Style="margin-right: 10px; margin-bottom: 5px;" OnClick="btnSave_Click"></asp:Button>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
