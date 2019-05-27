<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/SolarPMS.Master" AutoEventWireup="true" CodeBehind="MaterialMaster.aspx.cs" Inherits="SolarPMS.MaterialMaster" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
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
    </style>
    <div class="col-xs-12 padding-0 padding-b-2">
        <div class="col-xs-6 heading-big">
            <h5 class="margin-0 lineheight-30 breath-ctrl">Home / Administration / <span style="font-weight: normal !important">Material Master</span></h5>
        </div>
        <div class="col-xs-6 padding-0">
            <div class="col-xs-12 ">
                <div class="list-btn">
                    <ul>
                        <li>
                            <telerik:RadButton RenderMode="Lightweight" ID="btnAddNew" runat="server" Text="Add Material Code" OnClick="btnAddNew_Click">
                                <Icon PrimaryIconCssClass="rbAdd"></Icon>
                            </telerik:RadButton>
                        </li>
                    </ul>
                </div>
            </div>
        </div>
    </div>
    <div class="clearfix"></div>
    <div class="padding-lr-10">
        <div class="text-center">
            <telerik:RadNotification RenderMode="Lightweight" ID="radNotificationMessage" runat="server" Position="Center"
                Width="400" Height="100" Animation="Fade" EnableRoundedCorners="true" EnableShadow="true" AnimationDuration="500"
                ContentIcon="none" TitleIcon="none" Title="Success" Text="" Style="z-index: 100000">
            </telerik:RadNotification>
        </div>
        <div id="grid overflow-x">
            <telerik:RadGrid RenderMode="Lightweight" ID="gridMaterialMaster" runat="server" OnNeedDataSource="gridMaterialMaster_NeedDataSource"
                AutoGenerateColumns="false" MasterTableView-CommandItemSettings-AddNewRecordText="Add Details" ExportSettings-ExportOnlyData="true"
                AllowPaging="true" AllowSorting="true" AllowFilteringByColumn="false" PageSize="<%# Convert.ToInt32(SolarPMS.Models.Constants.CONST_GRID_PAGE_SIZE) %>"
                OnInsertCommand="gridMaterialMaster_InsertCommand" OnUpdateCommand="gridMaterialMaster_UpdateCommand" OnItemDataBound="gridMaterialMaster_ItemDataBound"
                OnCancelCommand="gridMaterialMaster_CancelCommand" OnItemCommand="gridMaterialMaster_ItemCommand">
                <GroupingSettings CaseSensitive="false" />
                <MasterTableView CommandItemSettings-ShowRefreshButton="false" ClientDataKeyNames="Id" EditMode="InPlace" CommandItemDisplay="None"
                    AllowFilteringByColumn="true" TableLayout="Fixed">
                    <CommandItemSettings ShowRefreshButton="false" ShowExportToExcelButton="false" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Site" HeaderStyle-Width="200px" UniqueName="Site" DataField="Site">
                            <ItemTemplate>
                                <%# Eval("Site") %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Label ID="lblSite" runat="server" Visible="False" Text='<%# Eval("Site") %>'> </asp:Label>
                                <telerik:RadDropDownList AutoPostBack="true" CausesValidation="false" OnSelectedIndexChanged="drpSite_SelectedIndexChanged" RenderMode="Lightweight"
                                    ID="drpSite" runat="server" DefaultMessage="Select Site" DropDownHeight="110px">
                                </telerik:RadDropDownList>
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="drpSite"
                                    Display="Dynamic" ErrorMessage="* Required!" ForeColor="Red" CssClass="validator" />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Project" HeaderStyle-Width="200px" UniqueName="Project" DataField="ProjectId">
                            <ItemTemplate>
                                <%# Eval("ProjectDescription") %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Label ID="lblProject" runat="server" Visible="False" Text='<%# Eval("ProjectId") %>'></asp:Label>
                                <telerik:RadDropDownList RenderMode="Lightweight" ID="drpProject" runat="server"
                                    DefaultMessage="Select Project" DropDownHeight="110px">
                                </telerik:RadDropDownList>
                                <asp:RequiredFieldValidator runat="server" ID="ReqProjValidator" ControlToValidate="drpProject"
                                    Display="Dynamic" ErrorMessage="* Required!" ForeColor="Red" CssClass="validator" />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Material Code" HeaderStyle-Width="200px" UniqueName="MaterialCode" DataField="MaterialCode">
                            <ItemTemplate>
                                <%# Eval("MaterialCode") %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtMaterialCode" Width="270px" MaxLength="100" Text='<%#Bind("MaterialCode") %>' runat="server">
                                </telerik:RadTextBox>
                                <asp:RequiredFieldValidator Display="Dynamic" ID="ReqMaterialCode" runat="server" ErrorMessage="* Required!" ForeColor="Red" ControlToValidate="txtMaterialCode"></asp:RequiredFieldValidator>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="IsActive" HeaderText="Status" UniqueName="Status" SortExpression="Status"  HeaderStyle-Width="100px">
                            <ItemTemplate>
                                <asp:Label ID="lblStatus" Text='<%# Bind("IsActive") %>' runat="server"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:CheckBox ID="chkStatus" runat="server" Checked='<%# Bind( "IsActive") %>' />
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
    </div>
</asp:Content>
