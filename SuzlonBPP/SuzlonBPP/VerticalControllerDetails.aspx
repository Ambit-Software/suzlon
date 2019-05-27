<%@ Page Title="" Language="C#" MasterPageFile="~/SuzlonBPP.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="VerticalControllerDetails.aspx.cs" Inherits="SuzlonBPP.VerticalControllerDetails" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="Scripts/bootstrap.min.js"></script>
    <script src="<%=Request.ApplicationPath%>/Scripts/jquery.min.js"></script>
    <style type="text/css">
        .RightAligned {
            text-align: right;
        }
        .paddingt-9{
            padding-top:9px;
        }
    </style>

    <telerik:RadCodeBlock ID="rcb" runat="server">
        <script type="text/javascript">

            <%--     function OpenBudgetUtilization() {
                debugger;
                //var oWnd = GetRadWindowManager().getWindowByName("BudgetPopup");
                //oWnd.show();
                if ($find('<%= radComboRequestType.ClientID %>').get_selectedItem().get_value() == 'Automatic' || ($find('<%= radComboRequestType.ClientID %>').get_selectedItem().get_value() == 'Manual')) {
                    var oWnd = GetRadWindowManager().getWindowByName("BudgetPopup");
                    oWnd.show();
                    return false;
                }
            }--%>

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

            <%--function OnClientClose(sender, eventArgs) {
                var ajaxManager = $find("<%= RadAjaxManager1.ClientID %>");
                //ajaxManager.ajaxRequest("ClearBudgetUtilization");
            }--%>

            function ValidateForm() {
                debugger;
                startDate = new Date($("#ctl00_ContentPlaceHolder1_rdpStart_dateInput").attr('value'));
                endDate = new Date($("#ctl00_ContentPlaceHolder1_rdpEnd_dateInput").attr('value'));
                jsonString = $("#ctl00_ContentPlaceHolder1_rdpAddendumDate_dateInput_ClientState").attr('value');
                jobject = JSON.parse(jsonString);
                adnDate = new Date(jobject.lastSetTextBoxValue);

                if (adnDate < startDate || adnDate > endDate) {
                    $("#ContentPlaceHolder1_lblRangeErr").html("Please Select Vaild Addendum Date Within Utilization Period.");
                    return false;
                } else {
                    $("#ContentPlaceHolder1_lblRangeErr").html("");
                    return;
                }
            }
            function keyPress(sender, args) {
                debugger;
                //var text = sender.get_value() + args.get_keyCharacter();
                //if (!text.match('^[0-9]+$'))
                //    args.set_cancel(true);
                var text = sender.get_value() + args.get_keyCharacter();
                var keycode = args.get_keyCode()
                if (!( (keycode >= 48 && keycode <= 57 )|| keycode == 8) || (keycode >= 96 && keycode <= 105)) {
                    args.set_cancel(true);
                   
                }
            }
        </script>
    </telerik:RadCodeBlock>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" OnAjaxRequest="RadAjaxManager1_AjaxRequest">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="lkbtnAddBudeget">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadAjaxManager1" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadAjaxManager1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="gridInverter" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadAjaxManager1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="BudgetPopup" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="rgridManualUtilisation" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="btnAddRange">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rgridManualUtilisation" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="rgridManualUtilisation">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rgridManualUtilisation" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>

        </AjaxSettings>
    </telerik:RadAjaxManager>


    <telerik:RadAjaxLoadingPanel ID="LoadingPanel1" runat="server" Skin="Default">
    </telerik:RadAjaxLoadingPanel>
    <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager" runat="server" EnableShadow="true">
        <Windows>
            <telerik:RadWindow ID="RadWindow" RenderMode="Lightweight" runat="server" CssClass="window123" VisibleStatusbar="false" 
                Width="700%" Height="550" AutoSize="false" Behaviors="Close" ShowContentDuringLoad="false"
                Modal="true" ReloadOnShow="true" />
        </Windows>
    </telerik:RadWindowManager>
    <div class="container-fluid padding-0 bg-white">
        <div class="row margin-0">
            <div class="col-xs-6 heading-big">
                <h5 class="margin-0 lineheight-42 breath-ctrl">Home / <span>Vendor Controller Details</span></h5>
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
                                <telerik:RadDatePicker ID="rdpCreatedDate" runat="server" Width="100%"></telerik:RadDatePicker>

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
                                <telerik:RadDatePicker ID="rdpPayDate" runat="server" Width="100%"></telerik:RadDatePicker>
                                <asp:RequiredFieldValidator ValidationGroup="FormLevel" runat="server" ID="RFVrdpPayDate" ControlToValidate="rdpPayDate" ErrorMessage="* Required" ForeColor="Red"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>
                    <!-- End of date wrapper-->

                    <!-- Start of Company Code wrapper-->
                    <div class="col-xs-12 padding-0">
                        <div class="col-sm-4 col-xs-12 padding-0">
                            <label class="control-label lable-txt" for="name">Company Name</label>
                        </div>
                        <div class="col-sm-8 col-xs-12 padding-0">

                            <telerik:RadComboBox ID="radcomboCmpy" RenderMode="Lightweight" runat="server" DataTextField="Name" EmptyMessage="Select Company Code" DataValueField="Id">
                            </telerik:RadComboBox>
                            <asp:RequiredFieldValidator ValidationGroup="FormLevel" runat="server" ID="RFVradcomboCmpy" ControlToValidate="radcomboCmpy" ErrorMessage="* Required" ForeColor="Red"></asp:RequiredFieldValidator>

                        </div>
                    </div>
                    <!-- End of Company Code wrapper-->

                    <!-- Start of Sub Vertical wrapper-->
                    <div class="col-xs-12 padding-0">
                        <div class="col-sm-4 col-xs-12 padding-0">
                            <label class="control-label lable-txt" for="name">Sub Vertical</label>
                        </div>
                        <div class="col-sm-8 col-xs-12 padding-0">
                            <telerik:RadComboBox ID="drpSubVertical" EmptyMessage="Select Sub Vertical" RenderMode="Lightweight" runat="server" DataTextField="Name" DataValueField="Id" AutoPostBack="true" OnSelectedIndexChanged="drpSubVertical_SelectedIndexChanged">
                            </telerik:RadComboBox>
                            <asp:RequiredFieldValidator ValidationGroup="FormLevel" runat="server" ID="RFVradComboSubVertical" ControlToValidate="drpSubVertical" ErrorMessage="* Required" ForeColor="Red"></asp:RequiredFieldValidator>

                        </div>
                    </div>
                    <!-- End of sub Vertical wrapper-->

                    <!-- Start of Vertical wrapper-->
                    <div class="col-xs-12 padding-0">
                        <div class="col-sm-4 col-xs-12 padding-0">
                            <label class="control-label lable-txt" for="name">Vertical</label>
                        </div>
                        <div class="col-sm-8 col-xs-12 padding-0">

                            <telerik:RadComboBox ID="drpVertical" EmptyMessage="Select Sub Vertical" RenderMode="Lightweight" runat="server" DataTextField="Name" DataValueField="Id" AutoPostBack="true">
                            </telerik:RadComboBox>

                        </div>
                    </div>
                    <!-- End of Vertical wrapper-->

                    <div class="col-xs-12 padding-0" runat="server" id="divAllocationNo">
                        <div class="col-sm-4 col-xs-12 padding-0">
                            <label class="control-label lable-txt" for="Requested Amount">Treasury Allocation Number</label>
                        </div>
                        <div class="col-sm-8 col-xs-12 padding-0 verticalctrl-textbox">

                            <asp:Label ID="lbTreasuryAllocNo" CssClass="control-label lable-txt text-muted " Text="00.00" runat="server"></asp:Label>

                        </div>
                    </div>

                    <!-- End of Treasury Allocation Number wrapper-->

                    <!-- Start of Request Type wrapper-->
                    <div class="col-xs-12 padding-0">
                        <div class="col-sm-4 col-xs-12 padding-0">
                            <label class="control-label lable-txt" for="name">Request Type</label>
                        </div>
                        <div class="col-sm-8 col-xs-12 padding-0">

                            <telerik:RadComboBox ID="radComboRequestType" EmptyMessage="Select Request Type" RenderMode="Lightweight" runat="server">
                                <Items>
                                    <telerik:RadComboBoxItem Text="Automatic" Value="Automatic" />
                                    <telerik:RadComboBoxItem Text="Manual" Value="Manual" />
                                </Items>
                            </telerik:RadComboBox>
                            <asp:RequiredFieldValidator ValidationGroup="FormLevel" runat="server" ID="RFVRequestType" ControlToValidate="radComboRequestType" ErrorMessage="* Required" ForeColor="Red"></asp:RequiredFieldValidator>

                        </div>
                    </div>
                    <!-- End of Nature of Request wrapper-->
                    <div class="row margin-0">
                        <div class="col-xs-6 heading-big">
                            <h5 class="margin-0 lineheight-42 breath-ctrl"></h5>
                        </div>
                        <div class="col-sm-6 col-xs-6 list-btn padding-0">
                            <ul class="list-btn ">
                                <li>
                                    <telerik:RadButton RenderMode="Lightweight" ID="btnAddNew" runat="server" Text="Add Nature of Request" AutoPostBack="true" CausesValidation="false" OnClick="btnAddNew_Click">
                                        <Icon PrimaryIconCssClass="rbAdd"></Icon>
                                    </telerik:RadButton>
                                </li>
                            </ul>
                        </div>
                    </div>
                    <div class="col-xs-12 padding-0 overflow-x">
                        <table class="table table-striped table-bordered">
                            <tbody>
                                <tr>
                                    <td>
                                        <telerik:RadGrid ID="gridNatureOfRequest" GridLines="None" AutoGenerateColumns="false" ShowHeadersWhenNoRecords="true"
                                            AllowPaging="true" AllowSorting="true" runat="server" OnItemDataBound="OnItemDataBoundHandler" OnPreRender="RadGvScope_PreRender"
                                            PageSize="<%# Convert.ToInt32(SuzlonBPP.Models.Constants.CONST_INNERGRID_PAGE_SIZE) %>"
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
                                                        SortExpression="NatureOfRequest" >
                                                        <ItemTemplate>
                                                            <telerik:RadLabel ID="lblNatureOfRequest" runat="server" Text='<%# Eval("NatureOfRequestText")%>' />
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <table Width="200px">
                                                                <tr>
                                                                    <td style="width: 100%">
                                                                        <telerik:RadComboBox MarkFirstMatch="True" runat="server" ID="radComboNatureOfRequest" AutoPostBack="false"
                                                                            Height="140px" Width="600px" DropDownWidth="200px" EmptyMessage="Select Nature of Request">
                                                                            <ItemTemplate>
                                                                                <ul>
                                                                                    <li class="col1">
                                                                                        <%# Eval("Name")%>
                                                                                    </li>
                                                                                </ul>
                                                                            </ItemTemplate>
                                                                        </telerik:RadComboBox>
                                                                        <asp:RequiredFieldValidator ID="RFVNaturOfReq" runat="server" ForeColor="Red" ErrorMessage="* Required" ControlToValidate="radComboNatureOfRequest"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                    
                                                                </tr>
                                                            </table>
                                                        </EditItemTemplate>
                                                        <HeaderStyle HorizontalAlign="left" />
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="Amount" HeaderText="Requested Amount"
                                                        SortExpression="Amount" ItemStyle-Width="300px">
                                                        <ItemTemplate>
                                                            <telerik:RadLabel ID="lblAmount" runat="server" Text='<%# Eval("Amount")%>' />
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <table>
                                                                <tr>
                                                                    <td style="padding-top: 6px;">
                                                                        <telerik:RadNumericTextBox NumberFormat-DecimalDigits="0" MinValue="0.00001" runat="server" ID="RadNumAmount" Width="150px">
                                                                            <NumberFormat DecimalDigits="2" AllowRounding="true" />
                                                                        </telerik:RadNumericTextBox>
                                                                        <asp:RequiredFieldValidator ID="RFVAmount" runat="server" ForeColor="Red" ErrorMessage="* Required" ControlToValidate="RadNumAmount"></asp:RequiredFieldValidator>
                                                                        <asp:CompareValidator ID="CmpAmount" runat="server" ControlToValidate="RadNumAmount" ErrorMessage="* Required" Display="Dynamic" ForeColor="Red" ValueToCompare="0" Operator="GreaterThan"></asp:CompareValidator>
                                                                    </td>
                                                                    
                                                                </tr>
                                                            </table>
                                                        </EditItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <HeaderStyle HorizontalAlign="left" />
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="ApprovedAmount" HeaderText="Approved Amount"
                                                        SortExpression="ApprovedAmount" ItemStyle-Width="300px"  >
                                                        <ItemTemplate>
                                                            <telerik:RadLabel ID="lblReqAmount" runat="server" Text='<%# Eval("ApprovedAmount")%>' />
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <table>
                                                                <tr>
                                                                    <td style="padding-top:6px;">
                                                                        <telerik:RadNumericTextBox NumberFormat-DecimalDigits="0" runat="server" ID="RadNumApprovedAmount" Width="150px">
                                                                            <NumberFormat DecimalDigits="2" AllowRounding="true" />
                                                                        </telerik:RadNumericTextBox>
                                                                        <asp:RequiredFieldValidator ID="RFVApprovedAmount" Enabled="false" runat="server" ForeColor="Red" ErrorMessage="* Required" ControlToValidate="RadNumApprovedAmount"></asp:RequiredFieldValidator>
                                                                        <asp:CompareValidator ID="CmpApprovedAmount" Enabled="false" runat="server" ControlToValidate="RadNumApprovedAmount" ErrorMessage="* Required" Display="Dynamic" ForeColor="Red" ValueToCompare="0" Operator="GreaterThan"></asp:CompareValidator>
                                                                    </td>
                                                                    
                                                                </tr>
                                                            </table>
                                                        </EditItemTemplate>
                                                        <ItemStyle HorizontalAlign="right" />
                                                        <HeaderStyle HorizontalAlign="left" />
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridEditCommandColumn HeaderText="Edit">
                                                        <HeaderStyle Width="80px" />
                                                    </telerik:GridEditCommandColumn>
                                                    <telerik:GridButtonColumn HeaderText="Delete" CommandName="Delete" Text="Delete" UniqueName="DeleteColumn">
                                                        <HeaderStyle Width="80px" />
                                                    </telerik:GridButtonColumn>
                                                </Columns>
                                                <PagerStyle Width="100px" ShowPagerText="false"
                                                    PageSizeLabelText=" " Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />
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
                        <div class="col-sm-8 col-xs-12 padding-0 verticalctrl-textbox">

                            <asp:Label CssClass="control-label lable-txt text-muted" ID="lblRequestedAmount" Text="00.00" runat="server"></asp:Label>

                        </div>
                    </div>
                    <!-- End of Requested Amount wrapper-->

                    <!-- Start of Amount Approved wrapper-->
                    <div class="col-xs-12 padding-0">
                        <div class="col-sm-4 col-xs-12 padding-0">
                            <label class="control-label lable-txt" for="Requested Amount">Initial Approved Amount</label>
                        </div>
                        <div class="col-sm-8 col-xs-12 padding-0 verticalctrl-textbox">

                            <asp:Label CssClass="control-label lable-txt text-muted" ID="lblInitAmount" Text="00.00" runat="server"></asp:Label>

                        </div>
                    </div>
                    <!-- End of Amount Approved wrapper-->

                    <!-- Start of Attachment wrapper-->
                    <div class="col-xs-12 padding-0">
                        <div class="col-sm-4 col-xs-12 padding-0">
                            <label class="control-label lable-txt" for="Requested Amount">Addendum Total</label>
                        </div>
                        <div class="col-sm-8 col-xs-12 padding-0 verticalctrl-textbox">
                            <asp:Label CssClass="control-label lable-txt text-muted" ID="lblAddendumAmt" Text="00.00" runat="server"></asp:Label>

                        </div>
                    </div>
                    <!-- End of Attachment wrapper-->

                    <!-- Start of Final Amount wrapper-->
                    <div class="col-xs-12 padding-0">
                        <div class="col-sm-4 col-xs-12 padding-0">
                            <label class="control-label lable-txt" for="Requested Amount">Final Amount</label>
                        </div>
                        <div class="col-sm-8 col-xs-12 padding-0 verticalctrl-textbox">
                            <asp:Label CssClass="control-label lable-txt text-muted" ID="lblFinalAmt" Text="0.00" runat="server"></asp:Label>
                        </div>
                    </div>
                    <!-- End of Attachment wrapper-->

                    <!-- Start of Allocation Period wrapper-->
                    <div class="col-xs-12 padding-0" runat="server" id="divUtilzationPeriod">
                        <div class="col-sm-4 col-xs-12 padding-0">
                            <label class="control-label lable-txt" for="Requested Amount">Utilsation Period</label>
                        </div>
                        <div class="col-sm-8 col-xs-12 padding-0">
                            <div class="col-sm-6 padding-0">
                                <label class="control-label">Start</label>

                                <table width="100%">
                                    <tr>
                                        <td>
                                            <telerik:RadDatePicker Width="100%" ID="rdpStart" runat="server"></telerik:RadDatePicker>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:RequiredFieldValidator runat="server" ID="RFVrdpStart" ValidationGroup="FormLevel" ControlToValidate="rdpStart" ErrorMessage="* Required"
                                                ForeColor="Red"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                </table>

                            </div>
                            <div class="col-sm-6">
                                <label class="control-label ">End</label>

                                <table width="100%">
                                    <tr>
                                        <td>
                                            <telerik:RadDatePicker ID="rdpEnd" runat="server" Width="100%"></telerik:RadDatePicker>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:RequiredFieldValidator runat="server" ID="RFVrdpEnd" ValidationGroup="FormLevel" ControlToValidate="rdpEnd" ErrorMessage="* Required" ForeColor="Red"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                </table>

                            </div>
                            <%--<asp:CompareValidator ID="DCVTender" runat="server" ControlToValidate="rdpStart" ValidationGroup="FormLevel" ControlToCompare="rdpEnd" Operator="LessThanEqual" Type="Date" ForeColor="Red" ErrorMessage="The Start date must less than or equal to End date." />--%>
                        </div>

                    </div>
                    <!-- End of Allocation Period wrapper-->

                    <!-- Start of Balanced Amount wrapper-->
                    <div class="col-xs-12 padding-0">
                        <div class="col-sm-4 col-xs-12 padding-0">
                            <label class="control-label lable-txt" for="Balance Amoun">Balance Amount</label>
                        </div>
                        <div class="col-sm-8 col-xs-12 padding-0">

                            <span class="control-label lable-txt text-muted col-sm-6 col-xs-12 padding-0">
                                <asp:Label ID="lblBalanceAmount" Text="10000.00" runat="server"></asp:Label>
                                &nbsp;                             
                                <a class="subheading" id="lkbtnAddBudeget" runat="server" href="javascript:void(0);" onclick="javascript:OpenBudgetUtilization();"><i>Budget Utilisation</i></a></span>


                        </div>
                    </div>
                    <!-- End of Balanced Amount wrapper-->

                    <!-- Start of Status wrapper-->
                    <div class="col-xs-12 padding-0" runat="server" id="divStatus">
                        <div class="col-sm-4 col-xs-12 padding-0">
                            <label class="control-label lable-txt" for="Requested Amount">Status</label>
                        </div>
                        <div class="col-sm-8 col-xs-12 padding-0">

                            <telerik:RadComboBox ID="rcbStatus" RenderMode="Lightweight" DataValueField="Id" DataTextField="Name" runat="server" EmptyMessage="Select Status">
                                <Items>
                                    <telerik:RadComboBoxItem Text="Approved" Value="1" />
                                    <telerik:RadComboBoxItem Text="Rejected" Value="2" />
                                    <telerik:RadComboBoxItem Text="Need Correction" Value="3" />
                                </Items>
                            </telerik:RadComboBox>
                            <asp:RequiredFieldValidator runat="server" ID="RFVStatus" ValidationGroup="FormLevel" ControlToValidate="rcbStatus" ErrorMessage="* Required" ForeColor="Red"></asp:RequiredFieldValidator>

                        </div>
                    </div>
                    <!-- End of Status wrapper-->
                </div>
                <!-- End of form Section-1-->

                <!-- Start of Form Section-2-->

                <div class="col-sm-6 col-xs-12">


                    <!-- Start of Comments/Remark wrapper-->
                    <div class="col-xs-12 padding-0 margin-10" style="margin-top: 0px !important;">
                        <div class="col-sm-4 col-xs-12 padding-0">
                            <label class="control-label lable-txt" for="Requested Amount">Comments/Remarks</label>
                        </div>
                        <div class="col-sm-8 col-xs-12 padding-0">
                            <div class="col-sm-12 col-xs-12 padding-0">
                                <div class="row  margin-0 chat-window col-xs-12 col-md-12 padding-0" id="chat_window_1">
                                    <div class="col-xs-12 col-md-12 padding-0">
                                        <div class="panel-body msg_container_base">
                                            <asp:Label ID="lblCommentDetail" CssClass="messages msg_sent" runat="server" Text=""></asp:Label>
                                        </div>
                                        <div class="panel-footer">
                                            <div class="input-group">
                                                <telerik:RadTextBox ID="txtCommentBox" TextMode="MultiLine" CssClass="form-control input-sm chat_input" Width="100%" runat="server" placeholder="Enter comments/remarks here..."></telerik:RadTextBox>
                                                <span class="input-group-btn">
                                                    <asp:LinkButton ID="btnComment" CssClass="btn btn-md btn-grey button-style" Text="Comment" runat="server" OnClick="btnComment_Click" />
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
                <div class="col-xs-6">
                    <div class="col-sm-4 col-xs-12 padding-0">
                        <label class="control-label lable-txt" for="Requested Amount">Attachment</label>
                    </div>
                    <div class="col-sm-8 col-xs-12 padding-0">
                        <div class="form-group">
                            <div class="input-group">
                               <%-- <a class="btn btn-grey" role="button" data-toggle="collapse" data-target="#add-attachment" aria-expanded="true">Add Attachment</a>--%>
                                <asp:Button runat="server" ID="btnAddAttachment" OnClick="btnAddAttachment_Click"   Text="Add/Edit Attachment" CssClass="btn btn-primary btn-xs center-block" CausesValidation="false"/>
                            </div>
                        </div>
                        <!-- /.form-group -->
                    </div>
                </div>
                <!-- End of Attachment wrapper-->

            </div>


            <!-- End of form Section-2-->

            <!-- Start of col-xs-12-->
            <div class="col-xs-12 margin-10">
                <div class="button-wrapper">
                    <ul>
                        <li>
                            <a href="#" class="btn btn-grey" id="hrefaddendum" runat="server" visible="false" data-toggle="modal" data-target="#addendum-modal">Addendum</a>
                        </li>
                        <li>
                            <asp:LinkButton ID="lbSubmit" runat="server" Text="Submit" ValidationGroup="FormLevel" CssClass="btn btn-grey" OnClick="lbSubmit_Click"></asp:LinkButton></li>
                        <li>
                            <asp:LinkButton ID="lbCancel" runat="server" Text="Cancel" CssClass="btn btn-grey" OnClick="lbCancel_Click"></asp:LinkButton></li>
                        <li>
                            <asp:LinkButton ID="lbSave" runat="server" Text="Save" ValidationGroup="FormLevel" CssClass="btn btn-grey" OnClick="lbSave_Click"></asp:LinkButton></li>
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
                                <asp:Label ID="lblAddendumAllocationNo" CssClass="control-label lable-txt text-muted" Text="00.00" runat="server"></asp:Label>
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
                                    <asp:RequiredFieldValidator runat="server" ID="RFVAddendumDate" ValidationGroup="FormAddendum" ControlToValidate="rdpAddendumDate" ErrorMessage="* Required"
                                        ForeColor="Red"></asp:RequiredFieldValidator>
                                    <br />
                                    <asp:Label ID="lblRangeErr" ForeColor="Red" runat="server"> </asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-xs-12 padding-0">
                        <div class="col-sm-6 col-xs-12 padding-0">
                            <label class="control-label lable-txt" for="Requested Amount">Addendum Amount</label>
                        </div>
                        <div class="col-sm-6 col-xs-12 padding-0">
                            <telerik:RadNumericTextBox NumberFormat-DecimalDigits="0" MinValue="0.00001" runat="server" ID="radNumAddendumAmount" Width="150px">
                                <NumberFormat DecimalDigits="2" AllowRounding="true" />
                            </telerik:RadNumericTextBox>
                            <asp:RequiredFieldValidator runat="server" ID="RFVNumAddendumAmount" ValidationGroup="FormAddendum" ControlToValidate="radNumAddendumAmount" ErrorMessage="* Required"
                                ForeColor="Red"></asp:RequiredFieldValidator>
                        </div>
                    </div>

                    <div class="col-xs-12 padding-0">
                        <div class="col-sm-6 col-xs-12 padding-0">
                            <label class="control-label lable-txt" for="Requested Amount">Addendum Approved Amount</label>
                        </div>
                        <div class="col-sm-6 col-xs-12 padding-0">
                            <telerik:RadNumericTextBox MinValue="0.00001" runat="server" ID="radNumAddendumAppAmount" Width="150px">
                                <NumberFormat DecimalDigits="2" AllowRounding="true" />
                            </telerik:RadNumericTextBox>
                            <asp:RequiredFieldValidator runat="server" ID="RFVNumAddendumAppAmount" ValidationGroup="FormAddendum" ControlToValidate="radNumAddendumAppAmount" ErrorMessage="* Required"
                                ForeColor="Red"></asp:RequiredFieldValidator>
                        </div>
                    </div>

                    <div class="col-xs-12 padding-0" runat="server" id="divAddendumStatus">
                        <div class="col-sm-6 col-xs-12 padding-0">
                            <label class="control-label lable-txt" for="Requested Amount">Addendum Status</label>
                        </div>
                        <div class="col-sm-6 col-xs-12 padding-0">
                            <telerik:RadComboBox ID="radCmbAddenumStatus" RenderMode="Lightweight" DataValueField="Id" DataTextField="Name" runat="server" EmptyMessage="Select Status">
                                <Items>
                                    <telerik:RadComboBoxItem Text="Approved" Value="1" />
                                    <telerik:RadComboBoxItem Text="Rejected" Value="2" />
                                    <telerik:RadComboBoxItem Text="Need Correction" Value="3" />
                                </Items>
                            </telerik:RadComboBox>
                            <asp:RequiredFieldValidator runat="server" ID="RFVAddenumStatus" ValidationGroup="FormAddendum" ControlToValidate="radCmbAddenumStatus" ErrorMessage="* Required"
                                ForeColor="Red"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="col-xs-12 padding-0">
                        <div class="col-sm-6 col-xs-12 padding-0">
                            <label class="control-label lable-txt" for="Requested Amount">Nature of Request</label>
                        </div>
                        <div class="col-sm-6 col-xs-12 padding-0">
                            <telerik:RadComboBox ID="radCmbAdnNatureOfRequest" RenderMode="Lightweight" DataValueField="Id" DataTextField="Name" runat="server" EmptyMessage="Select Nature of Request">
                            </telerik:RadComboBox>
                            <asp:RequiredFieldValidator runat="server" ID="RFVAdnNatureOfRequest" ValidationGroup="FormAddendum" ControlToValidate="radCmbAdnNatureOfRequest" ErrorMessage="* Required"
                                ForeColor="Red"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="col-xs-12 padding-0">
                        <div class="col-sm-6 col-xs-12 padding-0">
                            <label class="control-label lable-txt" for="Reason for addendum">Reason for Addendum</label>
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
                                <li>
                                    <asp:LinkButton ID="lnkSaveAddendum" runat="server" Text="Save" ValidationGroup="FormAddendum" OnClientClick="return ValidateForm();" CssClass="btn btn-grey" OnClick="lnkSaveAddendum_Click"></asp:LinkButton>
                                </li>
                                <li>
                                    <asp:LinkButton ID="lnkSubmitAddendum" runat="server" Text="Submit" ValidationGroup="FormAddendum" OnClientClick="return ValidateForm();" CssClass="btn btn-grey" OnClick="lnkSubmitAddendum_Click"></asp:LinkButton>
                                </li>
                            </ul>
                        </div>
                    </div>
                </div>
                <div id="addendum-history" class="collapse filter-panel" style="overflow: auto; height: 200px">
                    <div class="panel panel-default">
                        <div class="panel-body">
                            <telerik:RadGrid RenderMode="Lightweight" ID="rgridAddandumHistory" runat="server"
                                AutoGenerateColumns="false"
                                AllowPaging="false" AllowSorting="false" AllowFilteringByColumn="false">
                                <GroupingSettings CaseSensitive="false" />
                                <ClientSettings>
                                    <Selecting AllowRowSelect="false"></Selecting>
                                </ClientSettings>
                                <MasterTableView CommandItemSettings-ShowRefreshButton="false" EnableNoRecordsTemplate="true" EditMode="InPlace" CommandItemDisplay="None"
                                    AllowFilteringByColumn="false" AllowSorting="false" NoMasterRecordsText="">
                                    <CommandItemSettings ShowRefreshButton="false" ShowExportToExcelButton="false" />
                                    <NoRecordsTemplate>
                                        <table width="100%" border="0" cellpadding="20" cellspacing="20">
                                            <tr>
                                                <td align="center" class="txt-white">No records to display.
                                                </td>
                                            </tr>
                                        </table>
                                    </NoRecordsTemplate>
                                    <Columns>
                                        <telerik:GridBoundColumn HeaderText="Treasury Allocation Number" DataField="AllocationNo" UniqueName="AllocationNo">
                                            <HeaderStyle Width="120px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn HeaderText="Date" DataField="CreatedOn" UniqueName="CreatedOn" DataFormatString="{0:M/d/yyyy}">
                                            <HeaderStyle Width="60px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn HeaderText="Addendum Amount" DataField="Amount" UniqueName="Amount">
                                            <HeaderStyle Width="60px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn HeaderText="Addendum Approved Amount" DataField="ApprovedAmount" UniqueName="ApprovedAmount">
                                            <HeaderStyle Width="60px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn HeaderText="Addendum Status" DataField="AddandomWorkflowStatus" UniqueName="AddandomWorkflowStatus">
                                            <HeaderStyle Width="80px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn HeaderText="Nature of Request" DataField="NatureOfRequest" UniqueName="NatureOfRequest">
                                            <HeaderStyle Width="80px" />
                                        </telerik:GridBoundColumn>
                                    </Columns>
                                    <PagerStyle PageSizes="5,10,15" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                                        PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />
                                </MasterTableView>
                            </telerik:RadGrid>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- End of modal for addendum request-->
    <%--</div>--%>
    <telerik:RadWindowManager runat="server" ID="RadWindowManager1">
        <Windows>
            <telerik:RadWindow runat="server" ID="BudgetPopup" VisibleStatusbar="false" Width="1200px"
                Height="350px" AutoSize="false" ShowContentDuringLoad="false" Modal="true" OnClientBeforeShow="OnClientBeforeShow"
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
                            <div class="col-xs-12 padding-0">
                                <div class="button-wrapper pull-right">
                                    <ul>
                                        <li>
                                            <asp:LinkButton class="btn btn-grey" Text="Add Row" ID="btnAddRange" OnClick="btnAddRange_Click" runat="server" CausesValidation="false"></asp:LinkButton></li>
                                        <%--  <li><a href="#" onclick="CloseBudgetUtilization();" class="btn btn-grey">Cancel</a> </li>
                                        <li><a href="#" class="btn btn-grey">Save</a></li>--%>
                                    </ul>
                                </div>
                            </div>



                            <table>
                                <tbody>
                                    <tr>
                                        <td>
                                            <telerik:RadGrid ID="rgridManualUtilisation" GridLines="None" AutoGenerateColumns="false" ShowHeadersWhenNoRecords="true"
                                                AllowPaging="true" AllowSorting="true" runat="server" OnItemDataBound="rgridManualUtilisation_ItemDataBound"
                                                PageSize="5"  
                                                AllowAutomaticUpdates="true" AllowAutomaticInserts="True" OnNeedDataSource="rgridManualUtilisation_NeedDataSource"
                                                ShowStatusBar="false" OnItemCommand="rgridManualUtilisation_ItemCommand" OnInsertCommand="rgridManualUtilisation_InsertCommand" OnUpdateCommand="rgridManualUtilisation_UpdateCommand" OnDeleteCommand="rgridManualUtilisation_DeleteCommand">
                                                <MasterTableView ShowFooter="false" EditMode="InPlace" DataKeyNames="TreasuryBudgetUtilisationId" CommandItemDisplay="None" EnableNoRecordsTemplate="true">
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
                                                        <telerik:GridTemplateColumn UniqueName="PaymentDate" HeaderText="Payment Date"
                                                            SortExpression="PaymentDate" ItemStyle-Width="300px">
                                                            <ItemTemplate>
                                                                <telerik:RadLabel ID="lblPaymentDate" runat="server" Text='<%# Eval("PaymentDate" , "{0:d MMM yyyy }")%>'  />

                                                            </ItemTemplate>

                                                            <EditItemTemplate>
                                                                <table>
                                                                    <tr>
                                                                        <td>
                                                                            <telerik:RadLabel ID="lblPaymentDateVal" Visible="false" runat="server" Text='<%# Eval("PaymentDate")%>' />
                                                                            <telerik:RadDatePicker NumberFormat-DecimalDigits="0" runat="server" ID="dpkPaymentDate" Width="150px">
                                                                            </telerik:RadDatePicker>
                                                                            <asp:RequiredFieldValidator ID="RFVPaymentDate" runat="server" ForeColor="Red" ErrorMessage="* Required" ControlToValidate="dpkPaymentDate"></asp:RequiredFieldValidator>
                                                                        </td>
                                                                        
                                                                    </tr>
                                                                </table>
                                                            </EditItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridTemplateColumn UniqueName="NatureOfRequest" HeaderText="Nature of Request"
                                                            SortExpression="NatureOfReqest" ItemStyle-Width="300px">
                                                            <ItemTemplate>
                                                                <telerik:RadLabel ID="lblNatureOfRequest" runat="server" Text='<%# Eval("NatureOfReqest")%>' />

                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <table>
                                                                    <tr>
                                                                        <td style="width: 90%">
                                                                            <telerik:RadLabel ID="lblNatureOfRequestId" Visible="false" runat="server" Text='<%# Eval("NatureOfReqestId")%>' />

                                                                            <telerik:RadComboBox MarkFirstMatch="True" runat="server" ID="radComboNatureOfRequest" AutoPostBack="false"
                                                                                Height="140px" Width="600px" DropDownWidth="150px" EmptyMessage="Select Nature of Request">
                                                                            </telerik:RadComboBox>
                                                                            <asp:RequiredFieldValidator ID="RFVNaturOfReq" runat="server" ForeColor="Red" ErrorMessage="* Required" ControlToValidate="radComboNatureOfRequest"></asp:RequiredFieldValidator>
                                                                        </td>
                                                                        
                                                                    </tr>
                                                                </table>
                                                            </EditItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridTemplateColumn UniqueName="Amount" HeaderText="Amount"
                                                            SortExpression="Amount" ItemStyle-Width="250px">
                                                            <ItemTemplate>
                                                                <telerik:RadLabel ID="lblAmount" runat="server" Text='<%# Eval("Amount")%>' />
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <table>
                                                                    <tr>
                                                                        <td>
                                                                            <telerik:RadLabel ID="lblAmountVal" Visible="false" runat="server" Text='<%# Eval("Amount")%>' />
                                                                            <telerik:RadNumericTextBox NumberFormat-DecimalDigits="0" MinValue="0.00001" runat="server" ID="RadNumAmount" Width="150px">
                                                                                <NumberFormat DecimalDigits="2" AllowRounding="true" />
                                                                            </telerik:RadNumericTextBox>
                                                                            <asp:RequiredFieldValidator ID="RFVAmount" runat="server" ForeColor="Red" ErrorMessage="* Required" ControlToValidate="RadNumAmount"></asp:RequiredFieldValidator>
                                                                            <asp:CompareValidator ID="CmpAmount" runat="server" ControlToValidate="RadNumAmount" ErrorMessage="* Required" Display="Dynamic" ForeColor="Red" ValueToCompare="0" Operator="GreaterThan"></asp:CompareValidator>
                                                                        
                                                                            
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </EditItemTemplate>
                                                            <ItemStyle HorizontalAlign="Right" />
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridTemplateColumn UniqueName="AccountType" HeaderText="Account Type"
                                                            SortExpression="AccountType" ItemStyle-Width="300px">
                                                            <ItemTemplate>
                                                                <telerik:RadLabel ID="lblAccountType" runat="server" Text='<%# Eval("AccountType")%>' />
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <table>
                                                                    <tr>
                                                                        <td style="width: 90%">
                                                                            <telerik:RadLabel ID="lblAccountTypeId" Visible="false" runat="server" Text='<%# Eval("AccountType")%>' />
                                                                            <telerik:RadComboBox MarkFirstMatch="True" runat="server" ID="radComboAccountType" AutoPostBack="false"
                                                                                Height="140px" Width="600px" DropDownWidth="150px" EmptyMessage="Select Amount Type">
                                                                                <Items>
                                                                                    <telerik:RadComboBoxItem Text="Customer" Value="Customer" />
                                                                                    <telerik:RadComboBoxItem Text="GL" Value="GL" />
                                                                                    <telerik:RadComboBoxItem Text="Vendor" Value="Vendor" />
                                                                                </Items>
                                                                                <%--  <ItemTemplate>
                                                                                <ul>
                                                                                    <li class="col1">
                                                                                        <%# Eval("Name")%>
                                                                                    </li>
                                                                                </ul>
                                                                            </ItemTemplate>--%>
                                                                            </telerik:RadComboBox>
                                                                            <asp:RequiredFieldValidator ID="RFVAmountType" runat="server" ForeColor="Red" ErrorMessage="* Required" ControlToValidate="radComboAccountType"></asp:RequiredFieldValidator>
                                                                        </td>
                                                                        
                                                                    </tr>
                                                                </table>
                                                            </EditItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridTemplateColumn UniqueName="AccountCode" HeaderText="Account Code"
                                                            SortExpression="AccountCode" ItemStyle-Width="300px">
                                                            <ItemTemplate>
                                                                <telerik:RadLabel ID="lblAccountCode" runat="server" Text='<%# Eval("AccountCode")%>' />
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <table>
                                                                    <tr>
                                                                        <td>
                                                                            <telerik:RadLabel ID="lblAccountCodeVal" Visible="false" runat="server" Text='<%# Eval("AccountCode")%>' />
                                                                           
                                                                          <%--   <telerik:RadNumericTextBox NumberFormat-DecimalDigits="0" MinValue="1" MaxLength="20" runat="server" ID="RadNumAccountCode" Width="150px">
                                                                                <NumberFormat GroupSeparator="" DecimalDigits="0" AllowRounding="false" />
                                                                            </telerik:RadNumericTextBox>--%>
                                                                             <telerik:RadTextBox ID="RadNumAccountCode" runat="server" MaxLength="20">
                                                                            <ClientEvents OnKeyPress="keyPress" />
                                                                            </telerik:RadTextBox >
                                                                            <asp:RequiredFieldValidator ID="RFVAmountCode" runat="server" ForeColor="Red" ErrorMessage="* Required" ControlToValidate="RadNumAccountCode"></asp:RequiredFieldValidator>
                                                                            <asp:CompareValidator ID="CmpAmountCode" runat="server" ControlToValidate="RadNumAccountCode" ErrorMessage="* Required" Display="Dynamic" ForeColor="Red" ValueToCompare="0" Operator="GreaterThan"></asp:CompareValidator>
                                                                        </td>
                                                                        <td>
                                                                            
                                                                        </td>
                                                                        <td>
                                                                            
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </EditItemTemplate>
                                                            <ItemStyle HorizontalAlign="Right" />
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridTemplateColumn UniqueName="DocumentCode" HeaderText="Document Code"
                                                            SortExpression="Amount" ItemStyle-Width="300px">
                                                            <ItemTemplate>
                                                                <telerik:RadLabel ID="lblDocumentCode" runat="server" Text='<%# Eval("DocumentNo")%>' />
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <table>
                                                                    <tr>
                                                                        <td>
                                                                            <telerik:RadLabel ID="lblDocumentCodeVal" Visible="false" runat="server" Text='<%# Eval("DocumentNo")%>' />
                                                                           <%-- <telerik:RadNumericTextBox DisplayText='<%#Eval("DocumentNo")%>' NumberFormat-DecimalDigits="0" MinValue="1" MaxLength="20" runat="server" ID="RadNumDocumentCode" Width="150px">
                                                                                <NumberFormat GroupSeparator="" DecimalDigits="0" AllowRounding="false" />
                                                                            </telerik:RadNumericTextBox>--%>
                                                                            <telerik:RadTextBox ID="RadNumDocumentCode" runat="server" MaxLength="20">
                                                                            <ClientEvents OnKeyPress="keyPress" />
                                                                            </telerik:RadTextBox >
                                                                            <asp:RequiredFieldValidator ID="RFVDocumentCode" runat="server" ForeColor="Red" ErrorMessage="* Required" ControlToValidate="RadNumDocumentCode"></asp:RequiredFieldValidator>
                                                                            <asp:CompareValidator ID="CmpDocumentCode" runat="server" ControlToValidate="RadNumAmount" ErrorMessage="* Required" Display="Dynamic" ForeColor="Red" ValueToCompare="0" Operator="GreaterThan"></asp:CompareValidator>
                                                                        </td>
                                                                        
                                                                    </tr>
                                                                </table>
                                                            </EditItemTemplate>
                                                            <ItemStyle HorizontalAlign="Right" />
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridEditCommandColumn HeaderText="Edit" UniqueName="EditCommandColumn" >
                                                            <HeaderStyle Width="100px" />
                                                        </telerik:GridEditCommandColumn>
                                                        <telerik:GridButtonColumn HeaderText="Delete" CommandName="Delete" Text="Delete" UniqueName="DeleteColumn">
                                                            <HeaderStyle Width="80px" />
                                                        </telerik:GridButtonColumn>
                                                    </Columns>
                                                    <%--<PagerStyle  ShowPagerText="false" PageSizeLabelText=" " Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />--%>
                                                     <PagerStyle PageSizes="5,10,15" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                                PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Left" VerticalAlign="Middle" />

                                                </MasterTableView>
                                            </telerik:RadGrid>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>


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

