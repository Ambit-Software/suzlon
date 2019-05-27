<%@ Page Title="" Language="C#" MasterPageFile="~/SuzlonBPP.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="VerticalControllerDetails.aspx.cs" Inherits="SuzlonBPP.VerticalControllerDetails" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="Scripts/bootstrap.min.js"></script>
    <style type="text/css">
        .RightAligned {
            text-align: right;
        }
    </style>

    <telerik:RadCodeBlock ID="rcb" runat="server">
        <script type="text/javascript">

            function OpenBudgetUtilization() {
                if ($find('<%= radComboRequestType.ClientID %>').get_selectedItem().get_value() == 'Automatic' || ($find('<%= radComboRequestType.ClientID %>').get_selectedItem().get_value() == 'Manual')) {
                    var oWnd = GetRadWindowManager().getWindowByName("BudgetPopup");
                    oWnd.show();
                }
            }

            function OnClientBeforeShow(sender, eventArgs) {
                if ($find('<%= radComboRequestType.ClientID %>').get_selectedItem().get_value() == 'Automatic') {
                    $get("<%=AutomaticBudget.ClientID %>").style.display = 'block';
                    $get("<%=ManualBudget.ClientID %>").style.display = 'none';
                    sender.set_height(200);

                }

                if ($find('<%= radComboRequestType.ClientID %>').get_selectedItem().get_value() == 'Manual') {
                    $get("<%=AutomaticBudget.ClientID %>").style.display = 'none';
                    $get("<%=ManualBudget.ClientID %>").style.display = 'block';
                    sender.set_height(385);
                }

            }

            function CloseBudgetUtilization() {
                var window = $find('<%=BudgetPopup.ClientID %>');
                window.close();
            }

            function OnClientClose(sender, eventArgs) {
                var ajaxManager = $find("<%= RadAjaxManager1.ClientID %>");
                ajaxManager.ajaxRequest("ClearBudgetUtilization");
            }

            function OnAddRecord() {

            }
        </script>
    </telerik:RadCodeBlock>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" OnAjaxRequest="RadAjaxManager1_AjaxRequest">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="btnAddRange">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="ManualBudget" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadAjaxManager1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="gridInverter" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="LoadingPanel1" runat="server" Skin="Default">
    </telerik:RadAjaxLoadingPanel>
    <div class="container-fluid padding-0 bg-white">
        <div class="row margin-0">
            <div class="col-xs-6 heading-big">
                <h5 class="margin-0 lineheight-42 breath-ctrl">Home / <span style="font-weight: normal !important">Vendor Controller Details</span></h5>
            </div>
            <div class="col-sm-6 col-xs-6 ">
            </div>
        </div>
        <!-- End of search Criteria-->
        <div class="col-xs-12 padding-0">
            <div class="col-xs-12 margin-10">
                <!-- Start of form Section-1-->
                <div class="col-sm-6 col-xs-12">
                    <!-- Start of date wrapper-->
                    <div class="col-xs-12 padding-0">
                        <div class="col-sm-4 col-xs-12 padding-0">
                            <label class="control-label lable-txt" for="name">Creation Date</label>
                        </div>
                        <div class="col-sm-8 col-xs-12 padding-0">
                            <div class="form-group">
                                <telerik:RadDatePicker ID="rdpCreatedDate" runat="server"></telerik:RadDatePicker>

                            </div>
                        </div>
                    </div>
                    <!-- End of date wrapper-->

                    <!-- Start of date wrapper-->
                    <div class="col-xs-12 padding-0">
                        <div class="col-sm-4 col-xs-12 padding-0">
                            <label class="control-label lable-txt" for="name">Proposed Payment Date</label><small><i>On/Before</i></small>
                        </div>
                        <div class="col-sm-8 col-xs-12 padding-0">
                            <div class="form-group">
                                <telerik:RadDatePicker ID="rdpPayDate" runat="server"></telerik:RadDatePicker>
                                <asp:RequiredFieldValidator runat="server" ID="RFVrdpPayDate" Enabled="false" ControlToValidate="rdpPayDate" ErrorMessage="* Required" ForeColor="Red"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>
                    <!-- End of date wrapper-->

                    <!-- Start of Company Code wrapper-->
                    <div class="col-xs-12 padding-0">
                        <div class="col-sm-4 col-xs-12 padding-0">
                            <label class="control-label lable-txt" for="name">Company Code</label>
                        </div>
                        <div class="col-sm-6 col-xs-12 padding-0">
                            <div class="form-group">
                                <telerik:RadComboBox ID="radcomboCmpy" RenderMode="Lightweight" runat="server" DataTextField="Name" DataValueField="Id">
                                </telerik:RadComboBox>
                                <asp:RequiredFieldValidator runat="server" ID="RFVradcomboCmpy" Enabled="false" ControlToValidate="radcomboCmpy" ErrorMessage="* Required" ForeColor="Red"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>
                    <!-- End of Company Code wrapper-->

                    <!-- Start of Sub Vertical wrapper-->
                    <div class="col-xs-12 padding-0">
                        <div class="col-sm-4 col-xs-12 padding-0">
                            <label class="control-label lable-txt" for="name">Sub Vertical</label>
                        </div>
                        <div class="col-sm-6 col-xs-12 padding-0">
                            <div class="form-group">
                                <telerik:RadComboBox ID="drpSubVertical" RenderMode="Lightweight" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpSubVertical_SelectedIndexChanged">
                                </telerik:RadComboBox>
                                <asp:RequiredFieldValidator runat="server" ID="RFVradComboSubVertical" Enabled="false" ControlToValidate="drpSubVertical" ErrorMessage="* Required" ForeColor="Red"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>
                    <!-- End of sub Vertical wrapper-->

                    <!-- Start of Vertical wrapper-->
                    <div class="col-xs-12 padding-0">
                        <div class="col-sm-4 col-xs-12 padding-0">
                            <label class="control-label lable-txt" for="name">Vertical</label>
                        </div>
                        <div class="col-sm-8 col-xs-12 padding-0">
                            <telerik:RadTextBox ID="tbVertical" Width="315px" CssClass="form-control form-group" runat="server"></telerik:RadTextBox>
                        </div>
                    </div>
                    <!-- End of Vertical wrapper-->

                    <!-- Start of Treasury Allocation Number wrapper-->
                    <div class="col-xs-12 padding-0">
                        <div class="col-sm-4 col-xs-12 padding-0">
                            <label class="control-label lable-txt" for="Treasury Allocation Number">Treasury Allocation Number</label>
                        </div>
                        <div class="col-sm-8 col-xs-12 padding-0">
                            <div class="form-group">
                                <telerik:RadTextBox ID="tbTreasuryAllocNo" Width="315px" CssClass="form-control" runat="server"></telerik:RadTextBox>
                            </div>
                        </div>
                    </div>
                    <!-- End of Treasury Allocation Number wrapper-->

                    <!-- Start of Request Type wrapper-->
                    <div class="col-xs-12 padding-0">
                        <div class="col-sm-4 col-xs-12 padding-0">
                            <label class="control-label lable-txt" for="name">Request Type</label>
                        </div>
                        <div class="col-sm-6 col-xs-12 padding-0">
                            <div class="form-group">
                                <telerik:RadComboBox ID="radComboRequestType" RenderMode="Lightweight" runat="server">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="Select Request Type" Value="Select Request Type" />
                                        <telerik:RadComboBoxItem Text="Automatic" Value="Automatic" />
                                        <telerik:RadComboBoxItem Text="Manual" Value="Manual" />
                                    </Items>
                                </telerik:RadComboBox>
                            </div>
                        </div>
                    </div>
                    <!-- End of sub Vertical wrapper-->

                    <!-- Start of Nature of Request wrapper-->
                    <%--    <div class="col-xs-12 padding-0">
                        <div class="col-sm-4 col-xs-12 padding-0">
                            <label class="control-label lable-txt" for="Nature of Request">Nature of Request</label>
                        </div>
                        <div class="col-sm-8 col-xs-12 padding-0">
                            <div class="form-group">
                                <span class="control-label lable-txt text-muted col-sm-6 col-xs-12 padding-0">
                                    <asp:Label ID="lbBalance" Text="10000.00" runat="server"></asp:Label>&nbsp;
                                <a class="subheading" data-toggle="collapse" data-target="#add-request" aria-expanded="true"><i>ADD</i></a></span>
                            </div>
                        </div>
                        <!-- Collapse pannel for add Request btn-->

                        <div id="add-request" class="collapse filter-panel">
                            <div class="panel panel-default">
                                <div class="panel-body ">

                                    <table class="table table-striped table-bordered">
                                        <thead>
                                            <tr>
                                                <th class="text-center valign">Nature Of Request</th>
                                                <th class="text-center valign">Requested Amount</th>
                                                <th class="text-center valign">Approved Amount</th>


                                            </tr>
                                        </thead>
                                        <tbody id="tbNatureOfRequest">
                                            <tr class="text-center" >
                                                <td class="valign">
                                                    <div class="form-group" >
                                                        <telerik:RadComboBox ID="rcbNatureOfRequest" RenderMode="Lightweight" runat="server">
                                                            <Items>
                                                                <telerik:RadComboBoxItem Text="Select Nature Of Request" Value="Select Nature Of Request" />
                                                                <telerik:RadComboBoxItem Text="Approved" Value="Approved" />
                                                                <telerik:RadComboBoxItem Text="Reject" Value="Reject" />
                                                                <telerik:RadComboBoxItem Text="Reject" Value="Reject" />
                                                            </Items>
                                                        </telerik:RadComboBox>
                                                    </div>
                                                </td>
                                                <td class="valign">
                                                    <telerik:RadNumericTextBox ID="tbRequestedAmt" CssClass="form-control" MinValue="0" runat="server"></telerik:RadNumericTextBox>
                                                </td>
                                                <td class="valign">
                                                    <telerik:RadNumericTextBox ID="tbApprovedAmt" CssClass="form-control" MinValue="0" runat="server"></telerik:RadNumericTextBox>
                                                </td>

                                            </tr>
                                        </tbody>
                                    </table>
                                    <div class="col-xs-12 padding-0">
                                        <div class="button-wrapper pull-left">
                                            <ul>
                                                <li>
                                                    <asp:LinkButton ID="LinkButton1" runat="server" Text="Add Row" CssClass="btn btn-grey" OnClientClick="return OnAddRecord();" ></asp:LinkButton></li>
                                               <%-- <li>
                                                    <asp:LinkButton ID="LinkButton2" runat="server" Text="Cancel" CssClass="btn btn-grey"> </asp:LinkButton></li>
                                                <li>
                                                    <asp:LinkButton ID="LinkButton3" runat="server" Text="Save" CssClass="btn btn-grey"></asp:LinkButton></li>
                                            </ul>

                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <!-- End of collapse panel for add Request btn-->
                    </div>--%>
                    <!-- End of Nature of Request wrapper-->
                    <div class="row margin-0">
                        <div class="col-xs-6 heading-big">
                            <h5 class="margin-0 lineheight-42 breath-ctrl"></h5>
                        </div>
                        <div class="col-sm-6 col-xs-6 list-btn padding-t-6">
                            <ul class="list-btn ">
                                <li>
                                    <telerik:RadButton RenderMode="Lightweight" ID="btnAddNew" runat="server" Text="Add Nature of Request" AutoPostBack="true" CausesValidation="false" OnClick="btnAddNew_Click">
                                        <Icon PrimaryIconCssClass="rbAdd"></Icon>
                                    </telerik:RadButton>
                                </li>
                            </ul>
                        </div>
                    </div>
                    <div class="col-xs-12 padding-0">
                        <table class="table table-striped table-bordered">
                            <tbody>
                                <tr>
                                    <td>
                                        <telerik:RadGrid ID="gridNatureOfRequest" GridLines="None" AutoGenerateColumns="false" ShowHeadersWhenNoRecords="true"
                                            AllowPaging="true" AllowSorting="true" runat="server" OnItemDataBound="OnItemDataBoundHandler" OnPreRender="RadGvScope_PreRender" PageSize="<%# Convert.ToInt32(SuzlonBPP.Models.Constants.CONST_GRID_PAGE_SIZE) %>"
                                            AllowAutomaticUpdates="true" AllowAutomaticInserts="True" OnUpdateCommand="gridNatureOfRequest_UpdateCommand" OnNeedDataSource="gridNatureOfRequest_NeedDataSource"
                                            ShowStatusBar="true" OnItemCommand="gridNatureOfRequest_ItemCommand" OnInsertCommand="gridNatureOfRequest_InsertCommand">
                                            <MasterTableView ShowFooter="false" EditMode="InPlace" CommandItemDisplay="None" EnableNoRecordsTemplate="true">
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
                                                    <telerik:GridTemplateColumn UniqueName="NatureOfRequest" HeaderText="Nature of Request"
                                                        SortExpression="NatureOfRequest" ItemStyle-Width="600px">
                                                        <ItemTemplate>
                                                            <telerik:RadLabel ID="lblNatureOfRequest" runat="server" Text='<%# Eval("NatureOfRequest")%>' />
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <table>
                                                                <tr>
                                                                    <td style="width: 90%">
                                                                        <telerik:RadComboBox MarkFirstMatch="True" runat="server" ID="radComboNatureOfRequest" AutoPostBack="false"
                                                                            Height="140px" Width="600px" DropDownWidth="150px" EmptyMessage="Select Nature of Request">
                                                                            <ItemTemplate>
                                                                                <ul>
                                                                                    <li class="col1">
                                                                                        <%# Eval("NatureOfRequest")%>
                                                                                    </li>
                                                                                </ul>
                                                                            </ItemTemplate>
                                                                        </telerik:RadComboBox>
                                                                    </td>
                                                                    <td style="width: 10%">
                                                                        <asp:RequiredFieldValidator ID="RFVNaturOfReq" runat="server" ForeColor="Red" ErrorMessage="*" ControlToValidate="radComboNatureOfRequest"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </EditItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="Amount" HeaderText="Rquested Amount"
                                                        SortExpression="Amount" ItemStyle-Width="300px">
                                                        <ItemTemplate>
                                                            <telerik:RadLabel ID="lblAmount" runat="server" Text='<%# Eval("Amount")%>' />
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <telerik:RadNumericTextBox NumberFormat-DecimalDigits="0" MinValue="0.00001" runat="server" ID="RadNumAmount" Width="150px">
                                                                            <NumberFormat DecimalDigits="2" AllowRounding="true" />
                                                                        </telerik:RadNumericTextBox>
                                                                    </td>
                                                                    <td>
                                                                        <asp:RequiredFieldValidator ID="RFVAmount" runat="server" ForeColor="Red" ErrorMessage="*" ControlToValidate="RadNumAmount"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                    <td>
                                                                        <asp:CompareValidator ID="CmpAmount" runat="server" ControlToValidate="RadNumAmount" ErrorMessage="*" Display="Dynamic" ForeColor="Red" ValueToCompare="0" Operator="GreaterThan"></asp:CompareValidator>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </EditItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="ApprovedAmount" HeaderText="Approved Amount"
                                                        SortExpression="ApprovedAmount" ItemStyle-Width="300px">
                                                        <ItemTemplate>
                                                            <telerik:RadLabel ID="lblReqAmount" runat="server" Text='<%# Eval("ApprovedAmount")%>' />
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <telerik:RadNumericTextBox NumberFormat-DecimalDigits="0" runat="server" ID="RadNumApprovedAmount" Width="150px">
                                                                <NumberFormat DecimalDigits="2" AllowRounding="true" />
                                                            </telerik:RadNumericTextBox>
                                                        </EditItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridEditCommandColumn HeaderText="Edit">
                                                        <HeaderStyle Width="80px" />
                                                    </telerik:GridEditCommandColumn>
                                                </Columns>
                                                <PagerStyle PageSizes="5,10,15" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                                                    PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />
                                            </MasterTableView>
                                        </telerik:RadGrid>

                                    </td>
                                </tr>
                            </tbody>

                        </table>
                    </div>

                    <!-- Start of Requested Amount wrapper-->
                    <div class="col-xs-12 padding-0">
                        <div class="col-sm-4 col-xs-12 padding-0">
                            <label class="control-label lable-txt" for="Requested Amount">Requested Amount</label>
                        </div>
                        <div class="col-sm-8 col-xs-12 padding-0">
                            <div class="form-group">
                                <asp:Label CssClass="control-label lable-txt text-muted" ID="lblRequestedAmount" Text="00.00" runat="server"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <!-- End of Requested Amount wrapper-->

                    <!-- Start of Amount Approved wrapper-->
                    <div class="col-xs-12 padding-0">
                        <div class="col-sm-4 col-xs-12 padding-0">
                            <label class="control-label lable-txt" for="Requested Amount">Initial Approved Amount</label>
                        </div>
                        <div class="col-sm-8 col-xs-12 padding-0">
                            <div class="form-group">
                                <asp:Label CssClass="control-label lable-txt text-muted" ID="lblInitAmount" Text="00.00" runat="server"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <!-- End of Amount Approved wrapper-->

                    <!-- Start of Attachment wrapper-->
                    <div class="col-xs-12 padding-0">
                        <div class="col-sm-4 col-xs-12 padding-0">
                            <label class="control-label lable-txt" for="Requested Amount">Addendum Total</label>
                        </div>
                        <div class="col-sm-8 col-xs-12 padding-0">
                            <asp:Label CssClass="control-label lable-txt text-muted" ID="lblApprovedAmt" Text="00.00" runat="server"></asp:Label>

                        </div>
                    </div>
                    <!-- End of Attachment wrapper-->

                    <!-- Start of Final Amount wrapper-->
                    <div class="col-xs-12 padding-0">
                        <div class="col-sm-4 col-xs-12 padding-0">
                            <label class="control-label lable-txt" for="Requested Amount">Final Amount</label>
                        </div>
                        <div class="col-sm-8 col-xs-12 padding-0">
                            <asp:Label CssClass="control-label lable-txt text-muted" ID="lblFinalAmt" Text="10000.00" runat="server"></asp:Label>
                        </div>
                    </div>
                    <!-- End of Attachment wrapper-->

                    <!-- Start of Allocation Period wrapper-->
                    <div class="col-xs-12 padding-0">
                        <div class="col-sm-4 col-xs-12 padding-0">
                            <label class="control-label lable-txt" for="Requested Amount">Utilsation Period</label>
                        </div>
                        <div class="col-sm-8 col-xs-12 padding-0">
                            <div class="col-sm-5 padding-0">
                                <label class="control-label">Start</label>
                                <div class="form-group">
                                    <telerik:RadDatePicker Width="150px" ID="rdpStart" runat="server" min></telerik:RadDatePicker>
                                     <asp:RequiredFieldValidator runat="server" ID="RFVrdpStart" ControlToValidate="rdpStart" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator> 
                                </div>
                            </div>
                            <div class="col-sm-5">
                                <label class="control-label ">End</label>
                                <div class="form-group">
                                    <telerik:RadDatePicker Width="145px" ID="rdpEnd" runat="server"></telerik:RadDatePicker>
                                    <asp:RequiredFieldValidator runat="server" ID="RFVrdpEnd" ControlToValidate="rdpEnd"  ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                                    <asp:CompareValidator ID="DCVTender" runat="server" ControlToValidate="rdpStart" ControlToCompare="rdpEnd" Operator="GreaterThan" Type="Date" ErrorMessage="The second date must be after the first one."/>
                                </div>
                            </div>

                        </div>
                    </div>
                    <!-- End of Allocation Period wrapper-->

                    <!-- Start of Balanced Amount wrapper-->
                    <div class="col-xs-12 padding-0">
                        <div class="col-sm-4 col-xs-12 padding-0">
                            <label class="control-label lable-txt" for="Balance Amoun">Balance Amount</label>
                        </div>
                        <div class="col-sm-8 col-xs-12 padding-0">
                            <div class="form-group">
                                <span class="control-label lable-txt text-muted col-sm-6 col-xs-12 padding-0">
                                    <asp:Label ID="lblBalanceAmount" Text="10000.00" runat="server"></asp:Label>
                                    &nbsp;                             
                                <a class="subheading" id="lkbtnAddBudeget" onclick="OpenBudgetUtilization();"><i>Budget Utilisation</i></a></span>
                            </div>
                        </div>
                    </div>
                    <!-- End of Balanced Amount wrapper-->

                    <!-- Start of Status wrapper-->
                    <div class="col-xs-12 padding-0">
                        <div class="col-sm-4 col-xs-12 padding-0">
                            <label class="control-label lable-txt" for="Requested Amount">Status</label>
                        </div>
                        <div class="col-sm-6 col-xs-12 padding-0">
                            <div class="form-group">
                                <telerik:RadComboBox ID="rcbStatus" RenderMode="Lightweight" runat="server">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="Select Status" Value="Select Status" />
                                        <telerik:RadComboBoxItem Text="Approved" Value="Approved" />
                                        <telerik:RadComboBoxItem Text="Reject" Value="Reject" />
                                        <telerik:RadComboBoxItem Text="Need Correction" Value="Need Correction" />
                                    </Items>
                                </telerik:RadComboBox>
                            </div>
                        </div>
                    </div>
                    <!-- End of Status wrapper-->



                </div>
                <!-- End of form Section-1-->

                <!-- Start of Form Section-2-->

                <div class="col-sm-6 col-xs-12">


                    <!-- Start of Comments/Remark wrapper-->
                    <div class="col-xs-12 padding-0">
                        <div class="col-sm-4 col-xs-12 padding-0">
                            <label class="control-label lable-txt" for="Requested Amount">Comments/Remarks</label>
                        </div>
                        <div class="col-sm-8 col-xs-12 padding-0">
                            <div class="col-sm-12 col-xs-12 padding-0">
                                <div class="row chat-window col-xs-12 col-md-12 padding-0" id="chat_window_1">
                                    <div class="col-xs-12 col-md-12">
                                        <div class="panel panel-default">

                                            <div class="panel-body msg_container_base">
                                                <div class="row msg_container base_sent">
                                                    <div class="col-md-10 col-xs-10">
                                                        <div class="messages msg_sent">
                                                            <p>
                                                                Hi Abheejeet,
                                                            <br>
                                                                Lorem Ipsum is simply dummy text of the printing and typesetting industry.
                                                            </p>
                                                            <time datetime="2009-11-13T20:00">21-05-2015 | 22:10</time>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-2 col-xs-2 avatar">
                                                        <img src="http://www.bitrebels.com/wp-content/uploads/2011/02/Original-Facebook-Geek-Profile-Avatar-1.jpg" class=" img-responsive ">
                                                    </div>
                                                </div>
                                                <div class="row msg_container base_receive">
                                                    <div class="col-md-2 col-xs-2 avatar">
                                                        <img src="http://www.bitrebels.com/wp-content/uploads/2011/02/Original-Facebook-Geek-Profile-Avatar-1.jpg" class=" img-responsive ">
                                                    </div>
                                                    <div class="col-md-10 col-xs-10">
                                                        <div class="messages msg_receive">
                                                            <p>
                                                                Hi Giridhar
                                                            <br>
                                                                Lorem Ipsum is simply dummy text of the printing and typesetting industry.
                                                            </p>
                                                            <time datetime="2009-11-13T20:00">22-05-2015 | 22:10</time>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row msg_container base_receive">
                                                    <div class="col-md-2 col-xs-2 avatar">
                                                        <img src="http://www.bitrebels.com/wp-content/uploads/2011/02/Original-Facebook-Geek-Profile-Avatar-1.jpg" class=" img-responsive ">
                                                    </div>
                                                    <div class="col-xs-10 col-md-10">
                                                        <div class="messages msg_receive">
                                                            <p>
                                                                Hi Shital Kumar
                                                            <br>
                                                                Lorem Ipsum is simply dummy text of the printing and typesetting industry.
                                                            </p>
                                                            <time datetime="2009-11-13T20:00">Timothy • 51 min</time>
                                                        </div>
                                                    </div>
                                                </div>

                                            </div>
                                            <div class="panel-footer">
                                                <div class="input-group">
                                                    <telerik:RadTextBox ID="tbComment" TextMode="MultiLine" CssClass="form-control input-sm chat_input" Width="270px" runat="server"></telerik:RadTextBox>
                                                    <span class="input-group-btn">
                                                        <asp:LinkButton ID="btnComment" CssClass="btn btn-sm btn-grey button-style" Text="Comment" runat="server" />
                                                    </span>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>
                    <!-- End of Comments/Remark wrapper-->



                    <!-- Start of Attachment wrapper-->
                    <div class="col-xs-12 padding-0">
                        <div class="col-sm-4 col-xs-12 padding-0">
                            <label class="control-label lable-txt" for="Requested Amount">Attachment</label>
                        </div>
                        <div class="col-sm-8 col-xs-12 padding-0">
                            <div class="form-group">
                                <div class="input-group">
                                    <a class="btn btn-grey" role="button" data-toggle="collapse" data-target="#add-attachment" aria-expanded="true">Add Attachment</a>
                                </div>

                                <!-- Collapse pannel for add attachment btn-->

                                <div id="add-attachment" class="filter-panel collapse in" aria-expanded="true">
                                    <div class="panel panel-default">
                                        <div class="panel-body ">
                                            <div class="col-xs-12 margin-10"><small><i>Attach cancelled cheque, Account Certificate</i></small></div>
                                            <table class="table table-striped table-bordered">
                                                <thead>
                                                    <tr>
                                                        <th class="text-center valign">Document Type</th>

                                                        <th class="text-center valign">Attachment</th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    <tr class="text-center">
                                                        <td class="valign">
                                                            <telerik:RadTextBox ID="tbDocType" Width="100px" class="form-control input-sm chat_input" EmptyMessage="Enter Document Type" runat="server"></telerik:RadTextBox>
                                                        </td>

                                                        <td class="valign">
                                                            <div class="input-group">
                                                                <label class="input-group-btn">
                                                                    <span class="btn btn-grey ">Browse…
                                                                        <input type="file" style="display: none;" multiple="">
                                                                    </span>
                                                                </label>
                                                                <input type="text" class="form-control" readonly="">
                                                            </div>

                                                        </td>
                                                    </tr>

                                                </tbody>
                                            </table>
                                            <div class="col-xs-12 padding-0">
                                                <div class="button-wrapper pull-left">
                                                    <ul>
                                                        <li>
                                                            <asp:LinkButton ID="lbAddRow" class="btn btn-grey" Text="Add Row" runat="server" /></li>
                                                        <%--<li>
                                                            <asp:LinkButton ID="lbDocCancel" class="btn btn-grey" Text="Cancel" runat="server" /></li>
                                                        <li>
                                                            <asp:LinkButton ID="lbDocSave" class="btn btn-grey" Text="Save" runat="server" /></li>--%>
                                                    </ul>

                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <!-- End of collapse panel for add attachment btn-->
                            </div>
                            <!-- /.form-group -->



                        </div>
                    </div>
                    <!-- End of Attachment wrapper-->

                </div>


                <!-- End of form Section-2-->

                <!-- Start of col-xs-12-->
                <div class="col-xs-12 margin-10">
                    <div class="button-wrapper pull-left">
                        <ul>
                            <li>
                                <a href="#" class="btn btn-grey" data-toggle="modal" data-target="#addendum-modal">Addendum</a></li>
                            <li>
                                <asp:LinkButton ID="lbSubmit" runat="server" Text="Submit" CssClass="btn btn-grey"></asp:LinkButton></li>
                            <li>
                                <asp:LinkButton ID="lbCancel" runat="server" Text="Cancel" CssClass="btn btn-grey"></asp:LinkButton></li>
                            <li>
                                <asp:LinkButton ID="lbSave" runat="server" Text="Save" CssClass="btn btn-grey"></asp:LinkButton></li>
                        </ul>

                    </div>
                </div>
                <!-- End of col-xs-12-->


            </div>
            <!-- End of grid-->
        </div>
        <!-- Start of modal for addendum request-->

        <!-- End of modal for addendum request-->

        <!-- Start of modal for addendum request-->
        <div class="modal fade" id="addendum-modal" tabindex="-1" role="dialog" aria-labelledby="modalLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">×</span><span class="sr-only">Close</span></button>
                        <h4 class="modal-title subheading margin-0" id="lineModalLabel">Add Addendum Request</h4>
                    </div>
                    <div class="modal-body" style="overflow: hidden">

                        <!-- content goes here -->

                        <div class="col-xs-12 padding-0">
                            <div class="col-sm-6 col-xs-12 padding-0">
                                <label class="control-label lable-txt" for="Treasury Allocation Number">Treasury Allocation Number</label>
                            </div>
                            <div class="col-sm-6 col-xs-12 padding-0">
                                <div class="form-group">
                                    <telerik:RadTextBox CssClass="form-control" ID="tbTreasureAllocNo" runat="server" EmptyMessage="TAN457896"></telerik:RadTextBox>
                                </div>
                            </div>
                        </div>
                        <!-- End of Treasury Allocation Number wrapper-->
                        <div class="col-xs-12 padding-0">
                            <div class="col-sm-6 col-xs-12 padding-0">
                                <label class="control-label lable-txt" for="Requested Amount">Date</label>
                            </div>
                            <div class="col-sm-6 col-xs-12 padding-0">
                                <div class="form-group">
                                    <div class="input-group">
                                        <telerik:RadDatePicker ID="rdpAddendumDate" runat="server"></telerik:RadDatePicker>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-xs-12 padding-0">
                            <div class="col-sm-6 col-xs-12 padding-0">
                                <label class="control-label lable-txt" for="Requested Amount">Addendum Amount</label>
                            </div>
                            <div class="col-sm-6 col-xs-12 padding-0">
                                <telerik:RadTextBox ID="tbAddendumAmt" CssClass="form-control" runat="server" EmptyMessage="Enter Addendum Amount"></telerik:RadTextBox>
                            </div>
                        </div>

                        <div class="col-xs-12 padding-0">
                            <div class="col-sm-6 col-xs-12 padding-0">
                                <label class="control-label lable-txt" for="Requested Amount">Addendum Approved Amount</label>
                            </div>
                            <div class="col-sm-6 col-xs-12 padding-0">
                                <telerik:RadTextBox ID="tbAddendumAppAmt" CssClass="form-control" runat="server"></telerik:RadTextBox>
                            </div>
                        </div>

                        <div class="col-xs-12 padding-0">
                            <div class="col-sm-6 col-xs-12 padding-0">
                                <label class="control-label lable-txt" for="Requested Amount">Addenum Status</label>
                            </div>
                            <div class="col-sm-6 col-xs-12 padding-0">
                                <telerik:RadTextBox ID="tbAddenumStatus" CssClass="form-control" runat="server"></telerik:RadTextBox>
                            </div>
                        </div>

                        <div class="col-xs-12 padding-0">
                            <div class="col-sm-6 col-xs-12 padding-0">
                                <label class="control-label lable-txt" for="Reason for addendum">Reason for addendum</label>
                            </div>
                            <div class="col-sm-6 col-xs-12 padding-0">
                                <telerik:RadTextBox ID="tbaddendumReason" CssClass="form-control" runat="server"></telerik:RadTextBox>
                            </div>
                        </div>

                    </div>
                    <div class="modal-footer">
                        <div class="col-xs-12">
                            <div class="button-wrapper pull-left">
                                <ul>
                                    <li>
                                        <a href="#" class="btn btn-grey collapsed" role="button" data-toggle="collapse" data-target="#addendum-history" aria-expanded="false">View History</a>
                                    </li>
                                </ul>
                            </div>
                        </div>
                        <div id="addendum-history" class="collapse filter-panel">
                            <div class="panel panel-default">
                                <div class="panel-body">
                                    <table class="table table-striped table-bordered">
                                        <thead>
                                            <tr>
                                                <th class="text-center valign">Treasury Allocation Number</th>
                                                <th class="text-center valign">Date</th>
                                                <th class="text-center valign">Addendum Amount</th>
                                                <th class="text-center valign">Addendum Approved Amount</th>
                                                <th class="text-center valign">Addenum Status</th>
                                                <th class="text-center valign">Nature Of Request</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <tr class="text-center">
                                                <td class="valign">458795454</td>
                                                <td class="valign">23/05/2015</td>
                                                <td class="valign">60000.00</td>
                                                <td class="valign">40000.00</td>
                                                <td class="valign">Approved</td>
                                                <td class="valign">Labour Charge</td>
                                            </tr>
                                            <tr class="text-center">
                                                <td class="valign">45879784</td>
                                                <td class="valign">21/05/2015</td>
                                                <td class="valign">30000.00</td>
                                                <td class="valign">0.00</td>
                                                <td class="valign">Rejected</td>
                                                <td class="valign">Not Acceptable</td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <!-- End of modal for addendum request-->
    </div>
    <telerik:RadWindowManager runat="server" ID="RadWindowManager1">
        <Windows>
            <telerik:RadWindow runat="server" ID="BudgetPopup" VisibleStatusbar="false" Width="1200px" OnClientClose="OnClientClose"
                Height="385px" AutoSize="false" ShowContentDuringLoad="false" Modal="true" OnClientBeforeShow="OnClientBeforeShow"
                ReloadOnShow="true" Behaviors="Close,Move,Resize" Title="Budget Utilisation">
                <ContentTemplate>
                    <div id="AutomaticBudget" runat="server">
                        <!-- content goes here -->
                        <table class="table table-striped table-bordered">
                            <thead>
                                <tr>
                                    <th class="text-center">Vendor Code</th>
                                    <th class="text-center">Vendor Name</th>
                                    <th class="text-center">Nature of Request</th>
                                    <th class="text-center">Utilised Amount</th>


                                </tr>
                            </thead>
                            <tbody>
                                <tr class="text-center">
                                    <td class="valign">SAP0012345</td>
                                    <td class="valign">Ambit Software</td>
                                    <td class="valign">Electrical</td>
                                    <td class="valign">800000.00</td>
                                </tr>
                                <tr class="text-center">
                                    <td class="valign">SAP0012345</td>
                                    <td class="valign">TATA</td>
                                    <td class="valign">Mechanical</td>
                                    <td class="valign">50000.00</td>
                                </tr>
                                <tr class="text-center">
                                    <td class="valign">SAP0012345</td>
                                    <td class="valign">BSNL</td>
                                    <td class="valign">Electrical</td>
                                    <td class="valign">170000.00</td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <div id="ManualBudget" runat="server">
                        <div>
                            <!-- content goes here -->
                            <table>
                                <tbody>
                                    <tr>
                                        <td>
                                            <telerik:RadGrid RenderMode="Lightweight" ID="gridInverter" runat="server"
                                                AutoGenerateColumns="false" OnDeleteCommand="gridInverter_DeleteCommand"
                                                AllowPaging="true" AllowSorting="false" AllowFilteringByColumn="false" PageSize="5">
                                                <GroupingSettings CaseSensitive="false" />
                                                <ClientSettings>
                                                    <Selecting AllowRowSelect="true"></Selecting>
                                                </ClientSettings>
                                                <MasterTableView CommandItemSettings-ShowRefreshButton="false" EnableNoRecordsTemplate="true" EditMode="InPlace" CommandItemDisplay="None"
                                                    AllowFilteringByColumn="false" AllowSorting="false" NoMasterRecordsText="" DataKeyNames="DocumentNumber">
                                                    <CommandItemSettings ShowRefreshButton="false" ShowExportToExcelButton="false" />
                                                    <Columns>
                                                        <telerik:GridTemplateColumn HeaderText="Nature Of Request" UniqueName="NatureOfRequest" DataField="NatureOfRequest">
                                                            <ItemTemplate>
                                                                <select required="required" class="form-control">
                                                                    <option value="profile1">Select Nature Of Request</option>
                                                                    <option value="Approved">Wire</option>
                                                                    <option value="Reject">Electrical Board </option>
                                                                    <option value="Reject">Screw</option>
                                                                </select>
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <select required="required" class="form-control">
                                                                    <option value="profile1">Select Nature Of Request</option>
                                                                    <option value="Approved">Wire</option>
                                                                    <option value="Reject">Electrical Board </option>
                                                                    <option value="Reject">Screw</option>
                                                                </select>
                                                            </EditItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridTemplateColumn HeaderText="Amount" UniqueName="Amount" DataField="Amount">
                                                            <ItemTemplate>
                                                                <telerik:RadTextBox ID="tbAmtToAdd" runat="server"></telerik:RadTextBox>
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <telerik:RadTextBox ID="tbAmtToAdd1" runat="server"></telerik:RadTextBox>
                                                            </EditItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridTemplateColumn HeaderText="Account Type" UniqueName="AccountType" DataField="AccountType">
                                                            <ItemTemplate>
                                                                <select required="required" class="form-control">
                                                                    <option value="profile1">Select Account Type</option>
                                                                    <option value="Approved">Customer</option>
                                                                    <option value="Reject">GL</option>
                                                                    <option value="Reject">Vendor</option>
                                                                </select>
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <select required="required" class="form-control">
                                                                    <option value="profile1">Select Account Type</option>
                                                                    <option value="Approved">Customer</option>
                                                                    <option value="Reject">GL</option>
                                                                    <option value="Reject">Vendor</option>
                                                                </select>
                                                            </EditItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridTemplateColumn HeaderText="Account Code" UniqueName="AccountCode" DataField="AccountCode">
                                                            <ItemTemplate>
                                                                <telerik:RadTextBox ID="tbAmtToCode" runat="server"></telerik:RadTextBox>
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <telerik:RadTextBox ID="tbAmtToCode1" runat="server"></telerik:RadTextBox>
                                                            </EditItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridTemplateColumn HeaderText="Document Number" UniqueName="DocumentNumber" DataField="DocumentNumber">
                                                            <ItemTemplate>
                                                                <telerik:RadTextBox ID="EnterDocumentNumber" runat="server"></telerik:RadTextBox>
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <telerik:RadTextBox ID="EnterDocumentNumber1" runat="server"></telerik:RadTextBox>
                                                            </EditItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridButtonColumn CommandName="Delete" Text="Delete" UniqueName="DeleteColumn" />
                                                    </Columns>
                                                    <PagerStyle PageSizes="5,10,15" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                                                        PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />
                                                </MasterTableView>
                                            </telerik:RadGrid>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                            <div class="col-xs-12 padding-0">
                                <div class="button-wrapper pull-left">
                                    <ul>
                                        <li>
                                            <asp:LinkButton class="btn btn-grey" Text="Add Row" ID="btnAddRange" OnClick="btnAddRange_Click" runat="server" CausesValidation="false"></asp:LinkButton></li>
                                        <li><a href="#" onclick="CloseBudgetUtilization();" class="btn btn-grey">Cancel</a> </li>
                                        <li><a href="#" class="btn btn-grey">Save</a></li>
                                    </ul>
                                </div>
                            </div>

                        </div>

                    </div>
                </ContentTemplate>
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>
    <telerik:RadNotification RenderMode="Lightweight" Position="Center" runat="server" ID="radMessage" VisibleOnPageLoad="false"
        Width="350px" TitleIcon="none" ContentIcon="none" EnableShadow="true" AnimationDuration="1000">
    </telerik:RadNotification>
</asp:Content>

