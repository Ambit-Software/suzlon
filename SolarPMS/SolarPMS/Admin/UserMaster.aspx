<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/SolarPMS.Master" AutoEventWireup="true" CodeBehind="UserMaster.aspx.cs" Inherits="SolarPMS.Admin.UserMaster" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="GrdUser">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="GrdUser" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
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
            <h5 class="margin-0 lineheight-30 breath-ctrl">Home / Administration / <span style="font-weight: normal !important">User Details</span></h5>
        </div>

        <div class="col-xs-6 padding-0">
            <div class="col-xs-12 padding-0">


                <div class="list-btn">
                    <ul>
                        <li>
                            <telerik:RadButton RenderMode="Lightweight" ID="btnAddNew" runat="server" Text="Add User" OnClick="btnAddNew_Click">
                                <Icon PrimaryIconCssClass="rbAdd"></Icon>
                            </telerik:RadButton>
                        </li>
                    </ul>
                </div>
            </div>
        </div>
    </div>
    <div class="clearfix"></div>

    <div class="padding-lr-10" style="overflow-x:scroll; width:100%;">
        <telerik:RadGrid RenderMode="Lightweight" ID="GrdUser" runat="server" AutoGenerateColumns="false"
            AllowPaging="true" AllowSorting="true" AllowFilteringByColumn="false" PageSize="<%# Convert.ToInt32(SolarPMS.Models.Constants.CONST_GRID_PAGE_SIZE) %>" OnItemCommand="GrdUser_ItemCommand" OnNeedDataSource="GrdUser_NeedDataSource" OnEditCommand="GrdUser_EditCommand" OnItemDataBound="GrdUser_ItemDataBound">

            <MasterTableView CommandItemSettings-ShowRefreshButton="false" ClientDataKeyNames="UserDetail.userId" EditMode="InPlace" CommandItemDisplay="None" AllowFilteringByColumn="true">
                <Columns>
                    <telerik:GridBoundColumn DataField="UserDetail.Name" HeaderText="User Name" UniqueName="UserName">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="UserDetail.employeeId" HeaderText="Employee Id" UniqueName="EmployeeId">
                    </telerik:GridBoundColumn>
                    <%--<telerik:GridBoundColumn DataField="LocationName" HeaderText="Location" UniqueName="Location">
                    </telerik:GridBoundColumn>--%>
                    <telerik:GridBoundColumn DataField="UserDetail.emailId" HeaderText="Email Id" UniqueName="EmailId">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="UserDetail.mobileNo" HeaderText="Mobile No." UniqueName="MobileNo">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="UserDetail.Authentication" HeaderText="Authentication" UniqueName="Authentication">
                    </telerik:GridBoundColumn>
                    <telerik:GridCheckBoxColumn DataField="UserDetail.status" HeaderText="Enabled" Visible="false" UniqueName="chkStatus" DataType="System.Boolean" AllowFiltering="false">
                    </telerik:GridCheckBoxColumn>

                    <telerik:GridBoundColumn  DataField="UserDetail.status" HeaderText="Status" UniqueName="Status"  AllowFiltering="true" />

                    <telerik:GridEditCommandColumn HeaderText="Edit" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" Exportable="false">
                        <HeaderStyle Width="100px" />
                    </telerik:GridEditCommandColumn>

                </Columns>
                <PagerStyle PageSizes="5,10,15" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                    PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />
            </MasterTableView>
        </telerik:RadGrid>
    </div>



</asp:Content>
