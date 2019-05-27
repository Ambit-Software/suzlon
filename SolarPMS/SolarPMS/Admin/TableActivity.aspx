<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/SolarPMS.Master" AutoEventWireup="true" CodeBehind="TableActivity.aspx.cs" Inherits="SolarPMS.Admin.TableActivity" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="../Scripts/Page/UserMaster.js"></script>
    <style type="text/css">
        .RadUpload {
            width: 0px !important;
            float:left;
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
            float:left;
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
            width: 1px;
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
            width: 100%;
        }

        .RadNotification .rnContentWrapper {
            overflow: hidden !important;
            max-height: 500px;
        }

        .RadNotification, .RadNotification * {
            max-height: 500px;
            overflow: hidden;
        }
        .list-btn {
            float: left;
        }

            .list-btn ul {
                float: left;
                margin: 0px 0px 0px 0px;
                list-style-type: none;
                display: inline-block;
                padding:0px;
            }

                .list-btn ul li {
                    float: left;
                    margin-left: 10px;
                }
                .list-btn ul li:first-child {
                    float: left;
                    margin-left: 0px;
                }
    </style>
    <div class="row margin-0">
        <div class="col-xs-12 heading-big">
            <h5 class="margin-0 lineheight-30 breath-ctrl">Home / Administration / <span style="font-weight: normal !important">Table Activity</span></h5>
        </div>

    </div>
    <div class="row margin-0">
        <div class="col-sm-12 margin-10">
            <div class="col-sm-1 padding-0">
                <label class="control-label lable-txt" for="name">Block</label>
            </div>

            <div class="col-sm-10 padding-0">
                <div class="list-btn">
                    <ul>
                        <li>
                            <telerik:RadButton RenderMode="Lightweight" ID="rdbAddBlock" runat="server" Text="Add" OnClick="rdbAddBlock_Click">
                                <Icon PrimaryIconCssClass="rbAdd"></Icon>
                            </telerik:RadButton>
                        </li>
                        <li>
                            <telerik:RadButton RenderMode="Lightweight" ID="btnDownloadBlockTemplate" runat="server" Text="Template" OnClick="btnDownloadBlockTemplate_Click">
                                <Icon PrimaryIconCssClass="rbDownload"></Icon>
                            </telerik:RadButton>
                        </li>
                        <li style="width: 84px;">
                            <%--<telerik:RadButton RenderMode="Lightweight" ID="rdbUploadBlock" runat="server" Text="Upload">
                    <Icon PrimaryIconCssClass="rbUpload"></Icon>
                    </telerik:RadButton>--%>

                            <telerik:RadAsyncUpload runat="server" ID="btnImportBlock" HideFileInput="true" TargetFolder="~/Upload/TableActivity/Block/"
                                OnFileUploaded="btnImportBlock_FileUploaded" OnClientFileUploaded="callAjaxRequest" MaxFileInputsCount="1">
                                <Localization Select="" />
                            </telerik:RadAsyncUpload>
                        </li>
                        <li style="min-width: 84px;">

                            <telerik:RadButton RenderMode="Lightweight" ID="rdbDownLoadBlock" runat="server" Text="Download" OnClick="rdbDownLoadBlock_Click">
                                <Icon PrimaryIconCssClass="rbDownload"></Icon>
                            </telerik:RadButton>
                        </li>

                    </ul>
                </div>

            </div>
        </div>

        <div class="col-md-12 margin-10">
            <div class="col-sm-1 padding-0">
                <label class="control-label lable-txt" for="name">Inverter</label>
            </div>
            <div class="col-sm-10 padding-0">
                <div class="list-btn">
                    <ul>
                         <li>
                <telerik:RadButton RenderMode="Lightweight" ID="rdbAddInvertor" runat="server" Text="Add" OnClick="rdbAddInvertor_Click">
                    <Icon PrimaryIconCssClass="rbAdd"></Icon>
                </telerik:RadButton>
                            </li>
                        <li>
                <telerik:RadButton RenderMode="Lightweight" ID="btnDownloadInverterTemplate" runat="server" Text="Template" OnClick="btnDownloadInverterTemplate_Click">
                    <Icon PrimaryIconCssClass="rbDownload"></Icon>
                </telerik:RadButton>
                            </li>
                        <li style="width: 84px;">
                <telerik:RadAsyncUpload runat="server" ID="btnImportInvertor" HideFileInput="true" TargetFolder="~/Upload/TableActivity/Invertor/"
                    OnFileUploaded="btnImportInvertor_FileUploaded" OnClientFileUploaded="callAjaxRequest" MaxFileInputsCount="1">
                    <Localization Select="" />
                </telerik:RadAsyncUpload>
                            </li>
                        <li>
                <telerik:RadButton RenderMode="Lightweight" ID="rdbDownloadInvertor" runat="server" Text="Download" OnClick="rdbDownloadInvertor_Click">
                    <Icon PrimaryIconCssClass="rbDownload"></Icon>
                </telerik:RadButton>
                            </li>
                       
                <%--    <button class="btn btn-primary button" style="margin-right:10px !important;"> <span class=" glyphicon glyphicon-export text-default"></span> Export Excel</button>
                    <button class="btn btn-primary button" style="margin-right:10px !important;"> <span class=" glyphicon glyphicon-import text-default"></span> Import Excel</button>
                    <a href="add_invertor.html">
                    <button class="btn btn-primary button" style="margin-right:10px !important;">Add</button>
                    </a>--%>
                        </ul>
            </div>
        </div>
            </div>

        <div class="col-md-12 margin-10">
            <div class="col-sm-1 padding-0">
                <label class="control-label lable-txt" for="name">SCB</label>
            </div>

            <div class="col-sm-10 padding-0">
                <div class="list-btn">
                    <ul>
                        <li>
                            <telerik:RadButton RenderMode="Lightweight" ID="rdbAddSCB" runat="server" Text="Add" OnClick="rdbAddSCB_Click">
                                <Icon PrimaryIconCssClass="rbAdd"></Icon>
                            </telerik:RadButton>
                        </li>
                        <li>
                            <telerik:RadButton RenderMode="Lightweight" ID="btnDownloadSCBTemplate" runat="server" Text="Template" OnClick="btnDownloadSCBTemplate_Click">
                                <Icon PrimaryIconCssClass="rbDownload"></Icon>
                            </telerik:RadButton>
                        </li>

                        <li style="width: 84px;">
                            <telerik:RadAsyncUpload runat="server" ID="btnImportSCB" HideFileInput="true" TargetFolder="~/Upload/TableActivity/SCB/"
                                OnFileUploaded="btnImportSCB_FileUploaded" OnClientFileUploaded="callAjaxRequest" MaxFileInputsCount="1">
                                <Localization Select="" />
                            </telerik:RadAsyncUpload>
                        </li>
                        <li>
                            <telerik:RadButton RenderMode="Lightweight" ID="rdbDownloadSCB" runat="server" Text="Download" OnClick="rdbDownloadSCB_Click">
                                <Icon PrimaryIconCssClass="rbDownload"></Icon>
                            </telerik:RadButton>
                        </li>
                        
                        <%--  <button class="btn btn-primary button" style="margin-right:10px !important;"> <span class=" glyphicon glyphicon-export text-default"></span> Export Excel</button>
                    <button class="btn btn-primary button" style="margin-right:10px !important;"> <span class=" glyphicon glyphicon-import text-default"></span> Import Excel</button>
                        <a href="add_scb.html">
                        <button class="btn btn-primary button" style="margin-right:10px !important;">Add</button>
                        </a>--%>
                    </ul>
                </div>
        </div>
            </div>

        <div class="col-md-12 margin-10">
            <div class="col-sm-1 padding-0">
                <label class="control-label lable-txt" for="name">Table</label>
            </div>

            <div class="col-sm-10 padding-0">
                <div class="list-btn">
                    <ul>
                        
                        <li>
                            <telerik:RadButton RenderMode="Lightweight" ID="rdbAddTable" runat="server" Text="Add" OnClick="rdbAddTable_Click">
                                <Icon PrimaryIconCssClass="rbAdd"></Icon>
                            </telerik:RadButton>
                        </li>
                        <li>
                            <telerik:RadButton RenderMode="Lightweight" ID="btnDownloadTableTemplate" runat="server" Text="Template" OnClick="btnDownloadTableTemplate_Click">
                                <Icon PrimaryIconCssClass="rbDownload"></Icon>
                            </telerik:RadButton>
                        </li>
                        <li style="width: 84px;">
                            <telerik:RadAsyncUpload runat="server" ID="btnImportTable" HideFileInput="true" TargetFolder="~/Upload/TableActivity/Table/"
                                OnFileUploaded="btnImportTable_FileUploaded" OnClientFileUploaded="callAjaxRequest" MaxFileInputsCount="1">
                                <Localization Select="" />
                            </telerik:RadAsyncUpload>
                        </li>
                        <li>
                            <telerik:RadButton RenderMode="Lightweight" ID="rdbDownloadTable" runat="server" Text="Download" OnClick="rdbDownloadTable_Click">
                                <Icon PrimaryIconCssClass="rbDownload"></Icon>
                            </telerik:RadButton>
                        </li>
                        <%--<button class="btn btn-primary button" style="margin-right:10px !important;"> <span class=" glyphicon glyphicon-export text-default"></span> Export Excel</button>
                    <button class="btn btn-primary button" style="margin-right:10px !important;"> <span class=" glyphicon glyphicon-import text-default"></span> Import Excel</button>
                        <a href="add_table.html">
                        <button class="btn btn-primary button" style="margin-right:10px !important;">Add</button>
                        </a>--%>
                    </ul>
                </div>
        </div>
           
    
    </div>
</div>
    <telerik:RadGrid ID="radGrid" runat="server" ExportSettings-ExportOnlyData="false" OnInfrastructureExporting="radGrid_InfrastructureExporting" CssClass="hide" >
        <ClientSettings></ClientSettings>
        <ExportSettings IgnorePaging="true" Excel-Format="Biff" FileName="Template" ></ExportSettings>
        <MasterTableView>
        </MasterTableView>
    </telerik:RadGrid>
    <telerik:RadNotification RenderMode="Lightweight" Position="Center" runat="server" ID="RadNotification1" VisibleOnPageLoad="false"
        Width="450px" AutoCloseDelay="0" TitleIcon="none" ContentIcon="none">
    </telerik:RadNotification>
</asp:Content>
