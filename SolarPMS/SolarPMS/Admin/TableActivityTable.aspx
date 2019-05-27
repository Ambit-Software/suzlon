<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/SolarPMS.Master" AutoEventWireup="true" CodeBehind="TableActivityTable.aspx.cs" Inherits="SolarPMS.Admin.TableActivityTable" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="../Scripts/Page/Common.js"></script>

    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="drpSite">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="drpProject" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="drpArea" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="drpNetwork" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="drpActivity" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="drpSubActivity" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="ddlTableNo" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="drpProject">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="drpArea" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="drpNetwork" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="drpActivity" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="drpSubActivity" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="ddlTableNo" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="drpArea">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="drpNetwork" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="drpActivity" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="drpSubActivity" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="drpNetwork">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="drpActivity" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="drpSubActivity" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="drpActivity">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="drpSubActivity" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>

        </AjaxSettings>
    </telerik:RadAjaxManager>

    <telerik:RadAjaxLoadingPanel ID="LoadingPanel1" runat="server" Skin="Default">
    </telerik:RadAjaxLoadingPanel>
    <div class="row margin-0">
        <div class="col-xs-12 heading-big">
            <h5 class="margin-0 lineheight-30 breath-ctrl">Home / Administration / Table Activity / <span style="font-weight: normal !important">Table</span></h5>
        </div>

    </div>
    <div class="row margin-0">
        <div class="text-center">
        </div>
        <div class="survey-wrapper">
            <div class="row margin-0">
                <div class="col-xs-12">

                    <div class="col-xs-2 ">
                        <label class="col-xs-12 padding-0 control-label lable-txt" for="name">Site</label>
                    </div>
                    <div class=" col-xs-10 padding-0">
                        <telerik:RadComboBox runat="server" RenderMode="Lightweight" ID="drpSite" Skin="Office2010Silver" OnSelectedIndexChanged="drpSite_SelectedIndexChanged"
                            AutoPostBack="true" CausesValidation="false" EmptyMessage="Select Site" Filter="Contains" MarkFirstMatch="true" Width="100%">
                        </telerik:RadComboBox>
                        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="drpSite"
                            Display="Static" ErrorMessage="* Required!" ForeColor="Red" CssClass="validator" />
                    </div>
                </div>
                <div class="col-xs-12">

                    <div class="col-xs-2 ">
                        <label class="col-xs-12 padding-0 control-label lable-txt" for="name">Project</label>
                    </div>
                    <div class=" col-xs-10 padding-0">
                        <telerik:RadComboBox runat="server" RenderMode="Lightweight" ID="drpProject" Skin="Office2010Silver" OnSelectedIndexChanged="drpProject_SelectedIndexChanged"
                            CausesValidation="false" AutoPostBack="true" EmptyMessage="Select Project" Filter="Contains" MarkFirstMatch="true" Width="100%">
                        </telerik:RadComboBox>
                        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ControlToValidate="drpProject"
                            Display="Static" ErrorMessage="* Required!" ForeColor="Red" CssClass="validator" />
                    </div>
                </div>
                <div class="col-xs-12">
                    <div class="col-xs-2 ">
                        <label class="col-xs-12 padding-0 control-label lable-txt" for="name">Area</label>
                    </div>

                    <div class=" col-xs-10 padding-0">
                        <telerik:RadComboBox runat="server" RenderMode="Lightweight" ID="drpArea" Skin="Office2010Silver" OnSelectedIndexChanged="drpArea_SelectedIndexChanged"
                            EmptyMessage="Select Area" Filter="Contains" MarkFirstMatch="true" Width="100%" AutoPostBack="true" CausesValidation="false">
                        </telerik:RadComboBox>
                        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator3" ControlToValidate="drpArea"
                            Display="Static" ErrorMessage="* Required!" ForeColor="Red" CssClass="validator" />
                    </div>
                </div>
                <div class="col-xs-12">
                    <div class="col-xs-2 ">
                        <label class="col-xs-12 padding-0 control-label lable-txt" for="name">Network</label>
                    </div>
                    <div class=" col-xs-10 padding-0">
                        <telerik:RadComboBox runat="server" RenderMode="Lightweight" ID="drpNetwork" Skin="Office2010Silver" OnSelectedIndexChanged="drpNetwork_SelectedIndexChanged"
                            EmptyMessage="Select Network" CausesValidation="false" Filter="Contains" MarkFirstMatch="true" Width="100%" AutoPostBack="true">
                        </telerik:RadComboBox>
                        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator4" ControlToValidate="drpNetwork"
                            Display="Static" ErrorMessage="* Required!" ForeColor="Red" CssClass="validator" />
                    </div>
                </div>
                <div class="col-xs-12">
                    <div class="col-xs-2 ">
                        <label class="col-xs-12 padding-0 control-label lable-txt" for="name">Activity</label>
                    </div>
                    <div class=" col-xs-10 padding-0">
                        <telerik:RadComboBox runat="server" RenderMode="Lightweight" ID="drpActivity" Skin="Office2010Silver" OnSelectedIndexChanged="drpActivity_SelectedIndexChanged"
                            EmptyMessage="Select Activity" CausesValidation="false" Filter="Contains" MarkFirstMatch="true" Width="100%" AutoPostBack="true">
                        </telerik:RadComboBox>
                        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator5" ControlToValidate="drpActivity"
                            Display="Static" ErrorMessage="* Required!" ForeColor="Red" CssClass="validator" />
                    </div>
                </div>
                <div class="col-xs-12">
                    <div class="col-xs-2 ">
                        <label class="col-xs-12 padding-0 control-label lable-txt" for="name">Sub Activity</label>
                    </div>
                    <!-- End of col-lg-1-->

                    <div class=" col-xs-10 padding-0">
                        <telerik:RadComboBox runat="server" RenderMode="Lightweight" ID="drpSubActivity" Skin="Office2010Silver"
                            EmptyMessage="Select SubActivity" Filter="Contains" MarkFirstMatch="true" Width="100%">
                        </telerik:RadComboBox>
                    </div>
                </div>
                <div class="col-xs-12">
                    <div class="col-xs-2 ">
                        <label class="col-xs-12 control-label lable-txt padding-0" for="name">Table No.</label>
                    </div>
                    <div class=" col-xs-10 padding-0">
                        <telerik:RadComboBox runat="server" RenderMode="Lightweight" ID="ddlTableNo" Skin="Office2010Silver"
                            EmptyMessage="Select Table Number" Filter="Contains" MarkFirstMatch="true" Width="100%">
                        </telerik:RadComboBox>
                        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator6" ControlToValidate="ddlTableNo"
                            Display="Static" ErrorMessage="* Required!" ForeColor="Red" CssClass="validator" />
                    </div>
                </div>
                <div class="col-xs-12">
                    <div class="col-xs-2 ">
                        <label class="col-xs-12 padding-0 control-label lable-txt" for="name">Quantity</label>
                    </div>
                    <div class=" col-xs-10 padding-0">
                        <telerik:RadNumericTextBox MinValue="0" Width="100%" MaxValue="999999999" MaxLength="9" ID="txtQuantity" WatermarkText="Enter Quantity" runat="server">
                            <NumberFormat GroupSeparator="" DecimalDigits="0" KeepNotRoundedValue="false" />
                            <ClientEvents OnKeyPress="KeyPress" />
                        </telerik:RadNumericTextBox>
                        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator7" ControlToValidate="txtQuantity"
                            Display="Static" ErrorMessage="* Required!" ForeColor="Red" CssClass="validator" />
                    </div>
                </div>
            </div>
            <div class="button-wrapper margin-10">
                <center>
                <div class="submit-btn">
                    <asp:Button ID="BtnSave" runat="server" CssClass="btn btn-primary button button-style" Text="Save" OnClick="BtnSave_Click"  />
                </div>
                <div class="submit-btn">
                    <asp:Button ID="BtnCancel" runat="server" CausesValidation="false" CssClass="btn btn-primary button button-style"  Text="Cancel" OnClick="BtnCancel_Click" />
                </div>
              </center>

            </div>
        </div>
        <telerik:RadNotification RenderMode="Lightweight" Position="Center" runat="server" ID="radMesaage" VisibleOnPageLoad="false"
            Width="450px" Animation="Fade" TitleIcon="none" ContentIcon="none" Style="z-index: 100000" EnableRoundedCorners="true" EnableShadow="true" AnimationDuration="1000">
        </telerik:RadNotification>

    </div>
</asp:Content>
