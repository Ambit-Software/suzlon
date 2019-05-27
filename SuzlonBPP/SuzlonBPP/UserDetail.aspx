<%@ Page Title="" Language="C#" MasterPageFile="SuzlonBPP.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="UserDetail.aspx.cs" Inherits="SuzlonBPP.Admin.UserDetail" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="Scripts/Page/UserMaster.js"></script>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>           
            <telerik:AjaxSetting AjaxControlID="txtVendorSearch">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="txtVendorSearch"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="EntityDataSource1" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="AutoComVendorCode" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="BtnSave">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="pnl1" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="radMesaage" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="DrpProfile">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="panelVendorCode" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="divEmployee" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="DrpCompany">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="DrpCompany" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="DrpSubVertical">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="DrpSubVertical" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="VendorSelectedList">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="VendorSelectedList" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="DrpVertical">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="DrpVertical" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="DrpSubVertical" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript">
            function IsCheckAllSelectedVendor(sender, eventArgs) {
                var selectedItem = sender.get_checkedItems();
                var destinationListbox = $find('<%=VendorSelectedList.ClientID %>');
                var selectVendorList = destinationListbox.get_items();
                for (var count = 0; count < selectedItem.length; count++) {
                    CheckForVendorExistInList(selectedItem[count]._value, selectedItem[count]._text);
                }
            }

            function IsSelectedVendor(sender, eventArgs) {
                var selectedItem = eventArgs.get_item();
                CheckForVendorExistInList(selectedItem.get_value(), selectedItem.get_text());
            }

            function CheckForVendorExistInList(selectedValue, selectedText) {
                var isDuplicate = false;
                var destinationListbox = $find('<%=VendorSelectedList.ClientID %>');
                var selectVendorList = destinationListbox.get_items();
                for (var count = 0; count < selectVendorList.get_count() ; count++) {

                    if (selectedValue == destinationListbox.getItem(count).get_value())
                        isDuplicate = true;
                }
                if (!isDuplicate) {
                    var item = new Telerik.Web.UI.RadListBoxItem();
                    destinationListbox.trackChanges();
                    item.set_text(selectedText);
                    item.set_value(selectedValue);
                    destinationListbox.get_items().add(item);
                    destinationListbox.commitChanges();
                }
            }

            function ValidationVendorNames(source, args) {
                var rfvVendorcode = $("#ContentPlaceHolder1_rfvVendorcode");
                rfvVendorcode.addClass("hidden");
                rfvVendorcode.removeClass("visible");
                args.IsValid = true;
                var profile = $find('<%= DrpProfile.ClientID %>');
                if (profile._value == "9") //Vendor
                {
                    var listbox = $find('<%= VendorSelectedList.ClientID %>');
                    var count = listbox.get_items().get_count();
                    if (count <= 0) {
                        rfvVendorcode.addClass("visible");
                        rfvVendorcode.removeClass("hidden");
                        args.IsValid = false;
                    }
                }
            }
        </script>
    </telerik:RadCodeBlock>
    <telerik:RadAjaxLoadingPanel ID="LoadingPanel1" runat="server" Skin="Default">
    </telerik:RadAjaxLoadingPanel>
    <style>
        .RadListBox_Default {
            width: 100% !important;
            height: 150px !important;
        }

        .hidden {
            display: none !important;
            visibility: hidden !important;
        }

        .visible {
            display: block !important;
            visibility: visible !important;
        }
    </style>
    <script src="Scripts/bootstrap.min.js"></script>

    <asp:Panel ID="pnl1" runat="server">
        <div class="container-fluid padding-0">
            <div class="row margin-0">
                <div class="col-xs-6 heading-big">
                    <h5 class="margin-0 lineheight-42 breath-ctrl">Home / <span>Add User Details</span></h5>
                </div>
                <div class="col-sm-6 col-xs-6">
                </div>
            </div>
            <div class="row margin-0">
                <div class="block-wrapper">
                    <div class="col-md-6 col-sm-6 col-xs-12">
                        <div class="col-xs-12 padding-0">
                            <label class="col-xs-12 control-label lable-txt padding-bottom user-details-color">User Details</label>
                        </div>
                        <div class="col-md-4 col-xs-12 padding-0">
                            <label class="col-xs-12 control-label lable-txt">Photo</label>
                        </div>
                        <div class="row margin-0">
                            <div class="col-md-8 col-xs-12 padding-0">
                                <div class="col-lg-3 col-sm-3 col-md-3" style="height: 87px;">
                                    <asp:Image ID="ImgEmp" CssClass="img-responsive" runat="server" ImageUrl="~/Content/images/placeholder-icon.png" />
                                </div>
                                <div class="col-md-9 col-sm-9 col-lg-9 padding-0" style="margin-top: 15px;">
                                    <div id="dwndWrapper">
                                        <style type="text/css">
                                            .RadUpload .ruBrowse {
                                                min-height: 30px;
                                            }

                                            .RadUpload .ruFakeInput {
                                                min-height: 30px;
                                            }

                                            .RadUpload_Default .ruSelectWrap .ruFakeInput {
                                                width: 54% !important;
                                            }

                                            .RadUpload .ruFileWrap.ruStyled {
                                                width: 100% !important;
                                            }

                                            .RadUpload .ruSelectWrap {
                                                width: 100% !important;
                                            }
                                        </style>
                                        <telerik:RadAsyncUpload RenderMode="Lightweight" ID="AsyncUpload1" runat="server"
                                            MaxFileSize="2097152" TargetFolder="Profile/" OnClientFileUploaded="callAjaxRequest"
                                            AutoAddFileInputs="false" Localization-Select="Upload Image" OnFileUploaded="AsyncUpload1_FileUploaded" />
                                        <asp:Label ID="lblFileUploadMsgs" Text="" runat="server" Style="font-size: 10px;"></asp:Label>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row margin-0">
                            <div class="col-md-4 col-xs-12 padding-0">
                                <label class="col-xs-12 control-label lable-txt">Authentication</label>
                            </div>
                            <div class="col-md-8 col-xs-12 padding-0">
                                <div class="form-group">
                                    <div class="col-xs-12 padding-0">
                                        <asp:DropDownList ID="DrpAuthentication" runat="server" CssClass="form-control" AutoPostBack="false">
                                            <asp:ListItem Text="Select Authentication" Value="" />
                                            <asp:ListItem Value="AD">AD</asp:ListItem>
                                            <asp:ListItem Value="Database">Database</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ErrorMessage="*Required" Display="Dynamic" ForeColor="Red" runat="server" ControlToValidate="DrpAuthentication"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row margin-0">
                            <div class="col-md-4 col-xs-12 padding-0">
                                <label class="col-xs-12 control-label lable-txt">User ID</label>
                            </div>
                            <div class="col-md-8 col-xs-12 padding-0">
                                <asp:TextBox ID="TxtUserId" runat="server" placeholder="Enter Login ID" CssClass="search-query form-control"></asp:TextBox>
                            </div>
                            <asp:RequiredFieldValidator ID="ReqFldValUserId" ErrorMessage="*Required" Display="Dynamic" ForeColor="Red" runat="server" ControlToValidate="TxtUserId"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator Display="Dynamic" ForeColor="Red" ControlToValidate="TxtUserId" ID="RegularExpressionValidator1" ValidationExpression="^[\s\S]{0,15}$" runat="server" ErrorMessage="* Maximum 15 characters allowed."></asp:RegularExpressionValidator>
                        </div>
                        <div class="row margin-0">
                            <div class="col-md-4 col-xs-12 padding-0">
                                <label class="col-xs-1 control-label lable-txt">Password</label>
                            </div>
                            <div class="col-md-8 col-xs-12 padding-0">
                                <asp:TextBox ID="TxtPassword" runat="server" autocomplete="off" placeholder="Enter Password" TextMode="Password" CssClass="search-query form-control"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="ReqFldValPwd" ErrorMessage="*Required" Display="Dynamic" ForeColor="Red" runat="server" ControlToValidate="TxtPassword"></asp:RequiredFieldValidator>
                                <%-- <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="TxtPassword" ID="RegularExpressionValidator2" ValidationExpression="^[\s\S]{0,15}$" runat="server" ForeColor="Red" ErrorMessage="* Maximum 15 characters allowed."></asp:RegularExpressionValidator>--%>
                                <asp:RegularExpressionValidator ID="regPassword" ControlToValidate="TxtPassword" runat="server"
                                    ForeColor="Red" ErrorMessage="Password criteria doesn't match." Display="Dynamic"
                                    ValidationExpression="^(?=.*[!@#$%^&*]).{6,15}$"></asp:RegularExpressionValidator>
                                <div class="text-info small">
                                    Note: Password requires 1 special character (!@#$%^&*) and the length should be between 6-15 characters. 
                                </div>
                            </div>
                        </div>
                        <div class="row margin-0">
                            <div class="col-md-4 col-xs-12 ">
                                <label class="control-label lable-txt">Username</label>
                            </div>
                            <div class="col-md-8 col-xs-12 padding-0">
                                <asp:TextBox ID="TxtUserName" runat="server" placeholder="Enter Username" CssClass="search-query form-control"></asp:TextBox>
                                <asp:RequiredFieldValidator ErrorMessage="*Required" Display="Dynamic" ForeColor="Red" runat="server" ControlToValidate="TxtUserName"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="TxtUserName" ID="RegularExpressionValidator4" ValidationExpression="^[\s\S]{0,50}$" runat="server" ForeColor="Red" ErrorMessage="* Maximum 50 characters allowed."></asp:RegularExpressionValidator>
                            </div>
                        </div>
                        <div class="row margin-0">
                            <div class="col-md-4 col-xs-12 padding-0">
                                <label class="col-xs-12 control-label lable-txt">Emp. ID</label>
                            </div>
                            <div class="col-md-8 col-xs-12 padding-0">
                                <asp:TextBox ID="TxtEmployeeID" runat="server" placeholder="Enter Employee ID" CssClass="search-query form-control"></asp:TextBox>
                                <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="TxtEmployeeID" ID="RegularExpressionValidator3" ValidationExpression="^[\s\S]{0,15}$" runat="server" ForeColor="Red" ErrorMessage="* Maximum 15 characters allowed."></asp:RegularExpressionValidator>
                            </div>
                        </div>
                        <div class="row margin-0">
                            <div class="col-md-4 col-xs-12 padding-0">
                                <label class="col-xs-12 control-label lable-txt" for="Email Id">Email ID</label>
                            </div>
                            <div class="col-md-8 col-xs-12 padding-0">
                                <asp:TextBox ID="TxtEmailId" runat="server" placeholder="Enter Email ID" CssClass="search-query form-control"></asp:TextBox>
                                <asp:RequiredFieldValidator ErrorMessage="*Required"  Display="Dynamic" ForeColor="Red" runat="server" ControlToValidate="TxtEmailId"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="TxtEmailId" ID="RegularExpressionValidator5" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" runat="server" ForeColor="Red" ErrorMessage="*Enter valid email id."></asp:RegularExpressionValidator>
                            </div>
                        </div>
                        <div class="row margin-0">
                            <div class="col-md-4 col-xs-12 padding-0">
                                <label class="col-xs-12 control-label lable-txt" for="mobile">Mobile No.</label>
                            </div>
                            <div class="col-md-8 col-xs-12 padding-0">
                                <asp:TextBox ID="TxtMobileNo" runat="server" placeholder="Enter Mobile No." CssClass="search-query form-control"></asp:TextBox>
                                <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="TxtMobileNo" ID="RegularExpressionValidator6" ValidationExpression="^\d{1,15}$" runat="server" ForeColor="Red" ErrorMessage="* Maximum 15 Digits Number Only."></asp:RegularExpressionValidator>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6 col-sm-6 col-xs-12">
                        <div class="col-xs-12 padding-0">
                            <label class="col-xs-12 control-label lable-txt padding-bottom user-details-color">&nbsp;</label>
                        </div>

                        <div class="row margin-0">
                            <div class="col-md-4 col-xs-12 padding-0">
                                <label class="col-xs-12 control-label lable-txt" for="status">Status</label>
                            </div>
                            <div class="col-md-8 col-xs-12 padding-0">
                                <div class="form-group">
                                    <div class="col-xs-12">
                                        <asp:DropDownList ID="DrpStatus" runat="server" CssClass="form-control">
                                            <asp:ListItem Value="">Select Status</asp:ListItem>
                                            <asp:ListItem Value="Enabled">Enabled</asp:ListItem>
                                            <asp:ListItem Value="Disabled">Disabled</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ErrorMessage="*Required" Display="Dynamic" ForeColor="Red" runat="server" ControlToValidate="DrpStatus"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row margin-0">
                            <div class="col-md-4 col-xs-12 padding-0">
                                <label class="col-xs-12 control-label lable-txt" for="Profile">Profile</label>
                            </div>
                            <div class="col-md-8 col-xs-12 padding-0 overflow">
                                <div class="form-group">
                                    <div class="col-xs-12">
                                        <%--   <asp:DropDownList ID="DrpProfile" AutoPostBack="true" DataTextField="Name" DataValueField="Id" runat="server" CssClass="form-control" OnSelectedIndexChanged="DrpProfile_SelectedIndexChanged">
                                        </asp:DropDownList>--%>
                                        <telerik:RadComboBox CausesValidation="false" RenderMode="Lightweight" ID="DrpProfile" runat="server" Height="200" Width="305" AutoPostBack="true"
                                            MarkFirstMatch="true" EnableLoadOnDemand="false" OnSelectedIndexChanged="DrpProfile_SelectedIndexChanged">
                                        </telerik:RadComboBox>
                                        <asp:RequiredFieldValidator ErrorMessage="*Required" Display="Dynamic" ForeColor="Red" runat="server" ControlToValidate="DrpProfile"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                            </div>
                        </div>


                        <div class="row margin-0">
                            <asp:Panel runat="server" ID="panelVendorCode">
                                <div class="col-md-4 col-xs-12 padding-0">
                                    <label class="col-xs-12 control-label lable-txt" runat="server" id="Label2">Search Vendor Name</label>
                                </div>
                                <div class="col-md-8 col-xs-12 padding-0 overflow">
                                    <div class="form-group">
                                        <div class="col-xs-12">
                                            <asp:TextBox ID="txtVendorSearch" runat="server" CssClass="form-control" AutoPostBack="true" OnTextChanged="txtVendorSearch_TextChanged" />
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-4 col-xs-12 padding-0">
                                    <label class="col-xs-12 control-label lable-txt" runat="server" id="lblVendorCode">Choose Vendor Name</label>
                                </div>
                                <div class="col-md-8 col-xs-12 padding-0 overflow">
                                    <div class="form-group">
                                        <div class="col-xs-12">
                                            <telerik:RadComboBox CausesValidation="false" AutoPostBack="true" RenderMode="Lightweight" DataTextField="VendorName" DataValueField="VendorCode" MarkFirstMatch="true"
                                                ID="AutoComVendorCode" runat="server" CheckBoxes="true" EnableCheckAllItemsCheckBox="true"
                                                EnableVirtualScrolling="true" OnClientCheckAllChecked="IsCheckAllSelectedVendor" DataSourceID="EntityDataSource1" ItemsPerRequest="10" EnableAutomaticLoadOnDemand="true"
                                                OnClientItemChecked="IsSelectedVendor" />
                                            <asp:EntityDataSource ID="EntityDataSource1" runat="server" ConnectionString="name=SuzlonBPPEntities"
                                                DefaultContainerName="SuzlonBPPEntities" Select="DISTINCT it.[VendorCode], it.[VendorName]" EntitySetName="VendorMasters"
                                                OrderBy="it.[VendorName]" AutoPage="true" Where="@searchText is NULL OR it.VendorName LIKE '%'+@searchText+'%'">
                                                <WhereParameters>
                                                    <asp:Parameter Name="searchText" Type="String" />
                                                </WhereParameters>
                                            </asp:EntityDataSource>
                                            <asp:CustomValidator ID="rfvVendorcode" runat="server" ErrorMessage="*Required" Display="Dynamic" ForeColor="Red" ClientValidationFunction="ValidationVendorNames" CssClass="hidden"> 
                                            </asp:CustomValidator> 
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-4 col-xs-12 padding-0">
                                    <label class="col-xs-12 control-label lable-txt" runat="server" id="Label1">Selected Vendor Name</label>
                                </div>
                                <div class="col-md-8 col-xs-12 padding-0 overflow">
                                    <div class="form-group">
                                        <div class="col-xs-12">
                                            <telerik:RadListBox ID="VendorSelectedList" Height="150px" AllowDelete="true"
                                                SelectionMode="Multiple" runat="server">
                                            </telerik:RadListBox>
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>
                        </div>
                        <asp:Panel ID="divEmployee" runat="server">
                            <div class="row margin-0">
                                <div class="col-md-4 col-xs-12 padding-0">
                                    <label class="col-xs-12 control-label lable-txt" runat="server" id="lblCompany">Company Name</label>
                                </div>
                                <div class="col-md-8 col-xs-12 padding-0 overflow">
                                    <div class="form-group">
                                        <div class="col-xs-12">
                                            <telerik:RadComboBox CausesValidation="false" AutoPostBack="true" RenderMode="Lightweight" DataTextField="Name" DataValueField="Id" MarkFirstMatch="true"
                                                ID="DrpCompany" runat="server" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" OnDataBound="DrpCompany_DataBound" />
                                            <%-- <asp:DropDownList ID="DrpCompany" DataTextField="Name" DataValueField="Id" runat="server" CssClass="form-control">
                                        </asp:DropDownList>--%>
                                            <asp:RequiredFieldValidator ID="rfvCompany" ErrorMessage="*Required" Display="Dynamic" ForeColor="Red" runat="server" ControlToValidate="DrpCompany"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row margin-0">
                                <div class="col-md-4 col-xs-12 padding-0">
                                    <label class="col-xs-12 control-label lable-txt" runat="server" id="lblVertical">Vertical</label>
                                </div>
                                <div class="col-md-8 col-xs-12 padding-0 overflow">
                                    <div class="form-group">
                                        <div class="col-xs-12">
                                            <telerik:RadComboBox CausesValidation="false" AutoPostBack="true" RenderMode="Lightweight" DataTextField="Name" DataValueField="Id" MarkFirstMatch="true"
                                                ID="DrpVertical" runat="server" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" OnSelectedIndexChanged="DrpVertical_SelectedIndexChanged" OnDataBound="DrpVertical_DataBound" />
                                            <%--<asp:DropDownList ID="DrpVertical" DataTextField="Name" DataValueField="Id" runat="server" CssClass="form-control" AutoPostBack="True"  OnSelectedIndexChanged="DrpVertical_SelectedIndexChanged"/>--%>
                                            <asp:RequiredFieldValidator ID="rfvVertical" ErrorMessage="*Required" Display="Dynamic" ForeColor="Red" runat="server" ControlToValidate="DrpVertical"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row margin-0">
                                <div class="col-md-4 col-xs-12 padding-0">
                                    <label class="col-xs-12 control-label lable-txt" runat="server" id="lblSubVertical">Sub Vertical</label>
                                </div>
                                <div class="col-md-8 col-xs-12 padding-0 overflow">
                                    <div class="form-group">
                                        <div class="col-xs-12">
                                            <telerik:RadComboBox CausesValidation="false" AutoPostBack="true" RenderMode="Lightweight" DataTextField="Name" DataValueField="Id" MarkFirstMatch="true"
                                                ID="DrpSubVertical" runat="server" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" OnDataBound="DrpSubVertical_DataBound" />
                                            <%--  <asp:DropDownList ID="DrpSubVertical" DataTextField="Name" DataValueField="Id" runat="server" CssClass="form-control">
                                        </asp:DropDownList>--%>
                                            <asp:RequiredFieldValidator ID="rfvSubVertical" ErrorMessage="*Required" Display="Dynamic" ForeColor="Red" runat="server" ControlToValidate="DrpSubVertical"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>
                        <div class="row margin-0">
                            <asp:Label ID="lblErrorMsg" runat="server" ForeColor="Red" Text=""></asp:Label>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row margin-0">
                <div class="button-wrapper margin-10">
                    <center>
                        <div class="submit-btn">
                            <asp:Button ID="BtnSave" runat="server" CssClass="btn btn-primary button button-style submit-btn" Text="Save" OnClick="BtnSave_Click" />
                        </div>
                        <div class="submit-btn">
                            <asp:Button ID="BtnCancel" runat="server" CausesValidation="false" CssClass="btn btn-primary button button-style submit-btn" OnClientClick=" if(!CancelPopUp()) return false;" Text="Cancel" OnClick="BtnCancel_Click" />
                        </div>
                    </center>
                </div>
            </div>
        </div>
        <asp:HiddenField runat="server" ID="hidSerachUserId" />
    </asp:Panel>
    <telerik:RadNotification RenderMode="Lightweight" Position="Center" runat="server" ID="radMesaage" VisibleOnPageLoad="false"
        Width="450px" Animation="Fade" TitleIcon="none" ContentIcon="none" Style="z-index: 100000" EnableRoundedCorners="true" EnableShadow="true" AnimationDuration="1000">
    </telerik:RadNotification>
</asp:Content>
