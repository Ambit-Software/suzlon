<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="SuzlonBPP.Master" CodeBehind="ApplicationConfiguration.aspx.cs" Inherits="SuzlonBPP.ApplicationConfiguration" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script src="Scripts/bootstrap.min.js"></script>
    <style>
        .riSingle {
            width: 100% !important;
        }
    </style>

    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="BtnSave">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="pnl1" LoadingPanelID="LoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="LoadingPanel1" runat="server" Skin="Default">
    </telerik:RadAjaxLoadingPanel>
    <asp:Panel ID="pnl1" runat="server">
        <div class="container-fluid padding-0">
            <div class="col-xs-12 heading-big">
                    <h5 class="margin-0 lineheight-42 breath-ctrl">Home / <span>Application Configuration</span></h5>
                </div>
           
            <div class="col-xs-12 margin-10 padding-lr-10">
                <div class="col-sm-7 col-xs-12 padding-0">
                    <div class="col-sm-4 col-xs-12 padding-0 margin-10">
                        <label class="col-xs-12 control-label lable-txt" for="name">Budget Utilisation Maximum Limit</label>
                    </div>
                    <!-- End of col-lg-1-->
                    <div class=" col-sm-5 col-xs-12 margin-10">
                        <div class="form-group">
                            <asp:TextBox ID="txtDays" runat="server" MaxLength="3" CssClass="search-query form-control" placeholder="Enter Days" />
                            <asp:RequiredFieldValidator ID="ReqFldValBudget" ErrorMessage="* Required" Display="Dynamic" ForeColor="Red" runat="server" ControlToValidate="txtDays" />
                            <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="txtDays" ID="RegularExpressionValidator4" ValidationExpression="^\d{1,3}$" runat="server" ForeColor="Red" ErrorMessage="* days should be 1 to 365."></asp:RegularExpressionValidator>
                        </div>
                    </div>
                    <!-- End of input-group-col-xs-8-->
                    <div class=" col-sm-1 col-xs-1 margin-10">
                        <label class="control-label lable-txt" for="name">Days</label>
                    </div>
                    <!-- End of input-group-col-xs-8-->
                </div>
                <!-- End of Select Module-->

                <div class="col-sm-5 col-xs-12 padding-0">
                    <div class="col-sm-4 col-xs-12 padding-0 margin-10">
                        <label class="col-xs-12 control-label lable-txt" for="name">Addendum</label>
                    </div>
                    <!-- End of col-lg-1-->
                    <div class=" col-xs-8 margin-10">
                        <div class="form-group">
                            <asp:DropDownList ID="DrpAddendum" runat="server" CssClass="form-control">
                                <asp:ListItem Value="">Select Addendum</asp:ListItem>
                                <asp:ListItem Value="Enabled">Enabled</asp:ListItem>
                                <asp:ListItem Value="Disabled">Disabled</asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ErrorMessage="* Required" Display="Dynamic" ForeColor="Red" runat="server" ControlToValidate="DrpAddendum" />
                        </div>
                    </div>
                    <!-- End of col-lg-1-->
                </div>
                <div class="col-sm-7 col-xs-12 padding-0">
                    <div class="col-sm-4 col-xs-12 padding-0 margin-10">
                        <label class="col-xs-12 control-label lable-txt" for="name">Payment Method</label>
                    </div>
                    <!-- End of col-lg-1-->
                    <div class=" col-sm-5 col-xs-12 margin-10">
                        <div class="form-group">
                            <asp:TextBox runat="server" ID="txtPaymentMethod" CssClass="form-control" />
                            <asp:RequiredFieldValidator ErrorMessage="* Required" Display="Dynamic" ForeColor="Red" runat="server" ControlToValidate="txtPaymentMethod" />
                        </div>
                    </div>
                    <!-- End of input-group-col-xs-8-->
                </div>
                  <div class="col-sm-5 col-xs-12 padding-0">
                    <div class="col-sm-4 col-xs-12 padding-0 margin-10">
                        <label class="col-xs-12 control-label lable-txt" for="name">Per Vendor Daily Payment Limit</label>
                    </div>
                    <!-- End of col-lg-1-->
                    <div class=" col-sm-8 col-xs-12 margin-10">
                        
                        <telerik:RadNumericTextBox EmptyMessage="Enter Amount" MaxLength="13" CssClass="form-control"  NumberFormat-DecimalDigits="0" MinValue="0.00001" runat="server" ID="RadDailyAmount">
                        <NumberFormat DecimalDigits="0" AllowRounding="true" />
                    </telerik:RadNumericTextBox>
                            <asp:RequiredFieldValidator ErrorMessage="* Required" Display="Dynamic" ForeColor="Red" runat="server" ControlToValidate="txtPaymentMethod" />
                        
                    </div>
                    <!-- End of input-group-col-xs-8-->
                </div>

                <!-- End of Select Module-->
            </div>
            <div class="col-xs-12 padding-0">
                <center>
                <div class="submit-btn">
                    <asp:Button ID="BtnSave" runat="server" CssClass="btn btn-primary button button-style" Text="Save" OnClick="BtnSave_Click" />
                </div>
            </center>
            </div>
        </div>
    </asp:Panel>
    <telerik:RadNotification RenderMode="Lightweight" Position="Center" runat="server" ID="radMessage" VisibleOnPageLoad="false"
        Width="450px" Animation="Fade" TitleIcon="none" ContentIcon="none" Style="z-index: 100000" EnableRoundedCorners="true" EnableShadow="true" AnimationDuration="1000">
    </telerik:RadNotification>
</asp:Content>
