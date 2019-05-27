<%@ Page Title="" Language="C#" MasterPageFile="~/Blank.Master" AutoEventWireup="true" CodeBehind="AutomaticBudgetUtilisation.aspx.cs" Async="true" Inherits="SuzlonBPP.AutomaticBudgetUtilisation" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script lang="javascript" type="text/javascript">

       
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        .RadUpload .ruFakeInput {
            min-height: 29px !important;
            border-radius: 4px !important;
        }

        .RadUpload_Default .ruSelectWrap .ruButton {
            background-color: #f0f0f0 !important;
            background-image: none !important;
            min-height: 29px !important;
        }

        .RadUpload .ruSelectWrap {
            display: inline-block;
            float: left;
        }

        .table-scroll {
            overflow-x: scroll !important;
        }

        .RadWindow .rwIcon {
            display: none !important;
        }

        .RadWindow .rwTitleWrapper .rwTitle {
            margin: 2px 0 0 -21px !important;
            font-weight: bold !important;
        }
    </style>

    <telerik:RadAjaxManager ID="RadAjaxManager2" runat="server">
        <AjaxSettings>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="LoadingPanel1" runat="server" Skin="Default"></telerik:RadAjaxLoadingPanel>
    <div id="add-attachment" class="filter-panel collapse in" aria-expanded="true">
        <div class="panel panel-default">
            <div class="panel-body ">
                <div class="col-xs-12 padding-0">
                    <div class="table-scroll">
                        <telerik:RadGrid ID="rgridAutomaticUtilisation" GridLines="None" AutoGenerateColumns="false" ShowHeadersWhenNoRecords="true"
                            AllowPaging="true" AllowSorting="false" runat="server"
                            PageSize="5" RenderMode="Lightweight" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false"
                            AllowAutomaticUpdates="true" AllowAutomaticInserts="True" OnNeedDataSource="rgridAutomaticUtilisation_NeedDataSource"
                            ShowStatusBar="false">
                            <MasterTableView ShowFooter="false" EditMode="InPlace" EnableNoRecordsTemplate="true">
                                <CommandItemSettings ShowRefreshButton="false" />
                                <NoRecordsTemplate>
                                    <table width="100%" border="0" cellpadding="20" cellspacing="20">
                                        <tr>
                                            <td align="center" class="txt-white">No records to display.
                                            </td>
                                        </tr>
                                    </table>
                                </NoRecordsTemplate>
                                <Columns>
                                    <telerik:GridBoundColumn HeaderText="Vendor Code" DataField="VendorCode" UniqueName="VendorCode">
                                        <HeaderStyle Width="60px" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn HeaderText="Vendor Name" DataField="VendorName" UniqueName="VendorName">
                                        <HeaderStyle Width="120px" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn HeaderText="Nature Of Request" DataField="NatureOfRequest" UniqueName="NatureOfRequest">
                                        <HeaderStyle Width="120px" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn HeaderText="Utilised Amount" DataField="Utilised" UniqueName="Utilised">
                                        <HeaderStyle Width="120px" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn HeaderText="Tobe Utilised Amount" DataField="ToBeUtilised" UniqueName="ToBeUtilised">
                                        <HeaderStyle Width="120px" />
                                    </telerik:GridBoundColumn>
                                </Columns>
                                <%--<PagerStyle  ShowPagerText="false" PageSizeLabelText=" " Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />--%>
                                <PagerStyle PageSizes="5,10,15" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                                    PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Left" VerticalAlign="Middle" />

                            </MasterTableView>
                        </telerik:RadGrid>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
