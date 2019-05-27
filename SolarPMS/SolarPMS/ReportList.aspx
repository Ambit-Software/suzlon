<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/SolarPMS.Master" AutoEventWireup="true" CodeBehind="ReportList.aspx.cs" Inherits="SolarPMS.ReportList" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadAjaxManager runat="server" EnablePageHeadUpdate="false" ID="radAjaxMgrReports">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="radlistboxReports">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="ddlSapSite" />
                    <telerik:AjaxUpdatedControl ControlID="divDrpArea" />
                    <telerik:AjaxUpdatedControl ControlID="ddlSapProjects" />
                    <telerik:AjaxUpdatedControl ControlID="ddlArea" />
                    <telerik:AjaxUpdatedControl ControlID="ddlNetwork" />
                    <telerik:AjaxUpdatedControl ControlID="ddlType" />
                    <telerik:AjaxUpdatedControl ControlID="btnSearch" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="ddlSapSite">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="ddlSapProjects" />
                    <telerik:AjaxUpdatedControl ControlID="ddlNetwork" />
                    <telerik:AjaxUpdatedControl ControlID="ddlArea" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="ddlSapProjects">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="ddlArea" />
                    <telerik:AjaxUpdatedControl ControlID="ddlNetwork" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="ddlArea">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="ddlNetwork" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <style>
        .RadComboBox{
    width:100% !important;
}
        .margin-b5{
        margin-bottom: 8px;
        }
        .RadComboBoxDropDown_Office2010Silver {
            width: 203px !important;
        }

        #ctl00_ContentPlaceHolder1_RPTViewer_ReportViewer > div {
            height:450px !important;
        }
    </style>
    <div class="col-xs-12 padding-0">
        <div class="col-xs-12 heading-big">
            <h5 class="margin-0 lineheight-30 breath-ctrl">Home / <span style="font-weight: normal !important">Reports / </span><span id="spanReporName" runat="server"></span></h5>
        </div>
    </div>
    <div class="col-sm-12 padding-tb-10">
            <div class="col-sm-12 col-lg-10 col-xs-12 padding-0">

                <div id="divDrpSite" runat="server" class="col-sm-6 col-lg-5 col-md-4 col-xs-12 padding-0 margin-b5">
                    <div class="col-sm-4  col-xs-12">
                        <asp:Label runat="server" CssClass="control-label lable-txt" Text="Site"></asp:Label>

                    </div>
                    <div class="col-sm-8 col-xs-12">
                        <telerik:RadComboBox RenderMode="Lightweight" ID="ddlSapSite" runat="server" Height="200"  AutoPostBack="true"
                            Skin="Office2010Silver" Filter="Contains" MarkFirstMatch="true" EnableLoadOnDemand="false" OnSelectedIndexChanged="ddlSapSite_SelectedIndexChanged">
                        </telerik:RadComboBox>
                    </div>
                </div>

                <div id="divDrpProject" runat="server" class="col-sm-6 col-lg-5 col-md-4 col-xs-12 padding-0 margin-b5">
                    <div class="col-sm-4 col-xs-12">
                        <asp:Label runat="server" CssClass="control-label lable-txt" Text="Project"></asp:Label>

                    </div>
                    <div class=" col-sm-8 col-xs-12">
                        <telerik:RadComboBox RenderMode="Lightweight" ID="ddlSapProjects" runat="server" Height="200"  EmptyMessage="" OnSelectedIndexChanged="ddlSapProjects_SelectedIndexChanged"
                            AutoPostBack="true" Skin="Office2010Silver" Filter="Contains" ValidationGroup="Search" MarkFirstMatch="true" EnableLoadOnDemand="false">
                        </telerik:RadComboBox>
                    </div>
                </div>

                <div id="divDrpArea" runat="server" class="col-sm-6 col-lg-5 col-md-4 col-xs-12 padding-0 margin-b5">
                    <div class="col-sm-4 col-xs-12">
                        <asp:Label runat="server" CssClass="control-label lable-txt" Text="Area"></asp:Label>
                    </div>
                    <div class="col-sm-8 col-xs-12">
                        <telerik:RadComboBox RenderMode="Lightweight" ID="ddlArea" runat="server" Height="200"  EmptyMessage="" OnSelectedIndexChanged="ddlArea_SelectedIndexChanged"
                            AutoPostBack="true" Skin="Office2010Silver" Filter="Contains" ValidationGroup="Search" MarkFirstMatch="true" EnableLoadOnDemand="false">
                        </telerik:RadComboBox>
                    </div>

                </div>
                <div id="divDrpNetwork" runat="server" class="col-sm-6 col-lg-5 col-md-4 col-xs-12 padding-0 margin-b5">
                    <div class="col-sm-4 col-xs-12">
                        <asp:Label runat="server" CssClass="control-label lable-txt" Text="Network"></asp:Label>
                    </div>
                    <div class="col-sm-8 col-xs-12">
                        <telerik:RadComboBox RenderMode="Lightweight" ID="ddlNetwork" runat="server" Height="200"  EmptyMessage=""
                            Skin="Office2010Silver" Filter="Contains" CheckBoxes="true" ValidationGroup="Search" MarkFirstMatch="true" EnableLoadOnDemand="false">
                        </telerik:RadComboBox>
                    </div>
                </div>
                <div id="divDrpType" runat="server" class="col-sm-6 col-lg-5 col-md-4 col-xs-12 padding-0 margin-b5">
                    <div class="col-sm-4 col-xs-12">
                        <asp:Label runat="server" CssClass="control-label lable-txt" Text="Type"></asp:Label>
                    </div>
                    <div class="col-sm-8 col-xs-12">
                        <telerik:RadComboBox RenderMode="Lightweight" ID="ddlType" runat="server" Height="200" EmptyMessage=""
                            Skin="Office2010Silver" Filter="Contains" ValidationGroup="Search" MarkFirstMatch="true" EnableLoadOnDemand="false">
                        </telerik:RadComboBox>
                    </div>
                </div>
            </div>
        <div class="col-sm-12 col-lg-2 col-xs-12">
            <div id="divBtnSearch" runat="server" class=" padding-0 text-right margin-b5 pull-right">
                <asp:Button ID="btnSearch" runat="server" Text="View Report" OnClick="btnSearch_Click" CssClass="btn btn-cust btn-primary " ValidationGroup="Search" />
            </div>
        </div>
        <div class="col-xs-12" > 
            <div id="ReportViewerDIV" style="margin-top: 10px;">
                <rsweb:ReportViewer ID="RPTViewer" runat="server" Style="width: 100%;" BorderWidth="1" BorderColor="#8ba0bc">
                </rsweb:ReportViewer>
            </div>
        </div>
    </div>
</asp:Content>
