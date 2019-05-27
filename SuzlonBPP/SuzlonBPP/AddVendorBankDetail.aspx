<%@ Page Title="" Language="C#" MasterPageFile="~/SuzlonBPP.Master" AutoEventWireup="true" Async="true" CodeBehind="AddVendorBankDetail.aspx.cs" Inherits="SuzlonBPP.AddVendorBankDetail" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="Scripts/jquery-1.10.2.js"></script>
    <script src="Scripts/bootstrap.min.js"></script>
    <script src="Scripts/Page/UserMaster.js"></script>
    <script type="text/javascript">
        function ShowAuditTrail() {

            $('#squarespaceModal').modal('show');
        }
        //function callAjaxRequest(sender, rags) {

        //    __doPostBack();
        //}

    </script>
    <style type="text/css">
        .RadUpload .ruFakeInput {
            width: 450px;
            height: auto !important;
        }

        .RadPicker .RadInput {
            float: left;
            width: 240px;
        }

        .button-wrapper {
            margin: 0px;
        }

        .RadUpload .ruBrowse {
            min-height: 30px;
        }
        

        /*.disabled {
            background-color: #edecec !important;
        }*/
    </style>

    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script>
            function onRequestStart(sender, args) {

                if (args.get_eventTarget().indexOf("uploadCancelChq")) {

                }
                if (args.get_eventTarget().indexOf("uploadAccCertificate")) {

                }
            }

            var currentLoadingPanel = null;
            var currentUpdatedControl = null;
            function callAjaxRequest(sender, args) {

                currentLoadingPanel = $find("<%= LoadingPanel1.ClientID %>");
                currentUpdatedControl = "<%= attachment1.ClientID %>";
                radAjaxManager1 = $find("<%= RadAjaxManager1.ClientID %>");
                currentLoadingPanel.show(currentUpdatedControl);
                radAjaxManager1.ajaxRequest();
            }

            function hideProgress() {
                debugger;
                currentLoadingPanel.hide(currentUpdatedControl);
                return true;
            }

            function ResponseEnd() {
                debugger;
                if (currentLoadingPanel != null)
                    currentLoadingPanel.hide(currentUpdatedControl);
                currentUpdatedControl = null;
                currentLoadingPanel = null;
            }
        </script>
    </telerik:RadCodeBlock>

    <style type="text/css">
        .dialog-background {
            background: none repeat scroll 0 0 rgba(248, 246, 246, 0.00);
            height: 100%;
            left: 0;
            margin: 0;
            padding: 0;
            position: absolute;
            top: 0;
            width: 100%;
            z-index: 100;
        }

        .dialog-loading-wrapper {
            background-image: url(../Content/images/loading.gif);
            border: 0 none;
            height: 100px;
            left: 50%;
            margin-left: -50px;
            margin-top: -50px;
            position: fixed;
            top: 50%;
            width: 100px;
            z-index: 9999999;
            opacity: 1;
        }

        .dialog-loading-icon {
            background-image: url("content/images/loading.gif");
            background-repeat: no-repeat;
            /*background-color: #EFEFEF !important;*/
            /*border-radius: 13px;*/
            display: block;
            height: 100px;
            line-height: 100px;
            margin: 0;
            padding: 1px;
            text-align: center;
            width: 100px;
            opacity: 1;
        }
    </style>

    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" EnableAJAX="true">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadAjaxManager1">
                <UpdatedControls>
                    <%--<telerik:AjaxUpdatedControl ControlID="radMessage" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>--%>
                    <%--<telerik:AjaxUpdatedControl ControlID="uploadCancelChq" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>--%>
                    <%--<telerik:AjaxUpdatedControl ControlID="uploadAccCertificate" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>--%>
                    <telerik:AjaxUpdatedControl ControlID="txtNewIFSCCode"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="txtNewBankName"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="txtNewBranch"></telerik:AjaxUpdatedControl>
                     <telerik:AjaxUpdatedControl ControlID="txtNewAccountNo"></telerik:AjaxUpdatedControl>
                    
                    <%--<telerik:AjaxUpdatedControl ControlID="lblAttachment1"></telerik:AjaxUpdatedControl>--%>
                    <%--<telerik:AjaxUpdatedControl ControlID="lblAttachment2"></telerik:AjaxUpdatedControl>--%>
                    <telerik:AjaxUpdatedControl ControlID="attachment1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="attachment2"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="grdVendorBankDtl"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <%--  <telerik:AjaxSetting AjaxControlID="uploadCancelChq">
                <UpdatedControls>--%>
            <%--<telerik:AjaxUpdatedControl ControlID="uploadCancelChq" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>--%>
            <%-- <telerik:AjaxUpdatedControl ControlID="txtNewIFSCCode"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="txtNewBankName"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="txtNewBranch"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="lblAttachment1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="attachment1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="grdVendorBankDtl"></telerik:AjaxUpdatedControl>--%>
            <%--</UpdatedControls>
            </telerik:AjaxSetting>--%>
            <telerik:AjaxSetting AjaxControlID="attachment1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="attachment1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
             <telerik:AjaxSetting AjaxControlID="attachment2">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="attachment2"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="txtNewIFSCCode">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="txtNewBankName"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="txtNewBranch" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="grdVendorBankDtl"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="txtNewAccountNo">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdVendorBankDtl" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="txtNewBankName">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="txtNewIFSCCode" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="txtNewBranch" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="txtNewBranch">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="txtNewIFSCCode" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="txtNewBankName" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="DrpVendorCode">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="drpCompanyCode" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="txtVendorName" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="txtPanCardNo" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="drpCompanyCode">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="txtVendorName" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="txtPanCardNo" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>

                    <telerik:AjaxUpdatedControl ControlID="txtOldAccType" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="txtOldAccountNo" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="txtOldIFSCCode" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="txtOldBankName" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="txtOldBranch" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="txtOldBranch" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="txtOldCity" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="txtOldSuzlonEmail1" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="txtOldSuzlonEmail2" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="txtOldVendorEmail1" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="txtOldVendorEmail2" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>  
                    
                     <telerik:AjaxUpdatedControl ControlID="txtVendorName" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="txtPanCardNo" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>

                    <telerik:AjaxUpdatedControl ControlID="drpNewAccountType" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="txtNewAccountNo" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="txtNewIFSCCode" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="txtNewBankName" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="txtNewBranch" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>                   
                    <telerik:AjaxUpdatedControl ControlID="txtNewCity" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="txtNewSuzlonEmail1" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="txtNewSuzlonEmail2" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="txtNewVendorEmail1" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="txtNewVendorEmail2" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>  
                    <telerik:AjaxUpdatedControl ControlID="drpSubVertical" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="drpVertical" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>  
                     <telerik:AjaxUpdatedControl ControlID="txtCommentBox" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>  
                    <telerik:AjaxUpdatedControl ControlID="attachment1" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>  
                    <telerik:AjaxUpdatedControl ControlID="attachment2" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>  
                     <telerik:AjaxUpdatedControl ControlID="lblRequestType" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>                
                    
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="drpSubVertical">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="drpVertical" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="btnAddComment">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="lblCommentDetail" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="txtCommentBox" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="grdBankAuditTrial">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdBankAuditTrial" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="BtnSave">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="BtnSave" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="radMessage"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="drpStatus">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="drpAssignTo" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="lblAssignto" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="txtCommentBox" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="chkDocSent">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="dispatchDate" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="chkDocReceived">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="receiveDate" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>

        </AjaxSettings>
        <ClientEvents OnResponseEnd="ResponseEnd" />
    </telerik:RadAjaxManager>


    <%-- <table style="width: 100%; height: 100%; background-color: transparent; top: 20%; bottom: 80%; position: absolute;">
        <tr style="height: 100%">
            <td align="center" valign="middle" style="width: 100%">
                <asp:Image ID="Image2" runat="server" AlternateText="Loading..."
                    BorderWidth="0px" ImageUrl="~/Content/images/loading.gif"></asp:Image>
            </td>
        </tr>
    </table>--%>

    <telerik:RadAjaxLoadingPanel ID="LoadingPanel1" BackgroundPosition="Center" runat="server" Height="100%" Width="75px" Transparency="50">
        <div class="dialog-background">
            <div class="dialog-loading-wrapper">
                <span class="dialog-loading-icon"></span>
            </div>
        </div>
    </telerik:RadAjaxLoadingPanel>
    <asp:HiddenField ID="hdnIsExist" runat="Server" />

    <div class="row margin-0">
        <div class="col-xs-6 heading-big">
            <h5 class="margin-0 lineheight-42 breath-ctrl">Home / <span>Add New Vendor Bank Details <i>(For Suzlon Employee)</i></span></h5>
        </div>
        <div class="col-sm-6 col-xs-6 ">
        </div>
    </div>
    <div class="row margin-10 margin-b-0">
        <div class="col-xs-12 padding-lr-10">
            <div class="col-sm-1 col-md-1 col-xs-12 padding-0">
                <label class="control-label lable-txt" for="name">Vendor</label>
            </div>
            <div class="col-sm-11 col-md-11 col-xs-12 padding-0">

                <%--  <asp:DropDownList ID="DrpVendorCode" AutoPostBack="true" runat="server" CssClass="form-control" OnSelectedIndexChanged="DrpVendorCode_SelectedIndexChanged">
                </asp:DropDownList>--%>
                <telerik:RadComboBox CausesValidation="false" RenderMode="Lightweight" ID="DrpVendorCode" runat="server" Height="200" Width="305" AutoPostBack="true"
                    MarkFirstMatch="true" Filter="Contains" OnSelectedIndexChanged="DrpVendorCode_SelectedIndexChanged" DataTextField="VendorName" DataValueField="VendorCode"
                    EnableVirtualScrolling="true"  DataSourceID="EntityDataSource1" ItemsPerRequest="10" EnableAutomaticLoadOnDemand="true" >
                </telerik:RadComboBox>
                 <asp:EntityDataSource ID="EntityDataSource1" runat="server" ConnectionString="name=SuzlonBPPEntities"
                                    DefaultContainerName="SuzlonBPPEntities" Select="it.[VendorCode], it.[VendorName]"
                                    AutoPage="true" CommandText="GetVendorCode(@UserID)">
                                 <CommandParameters>
                                    <asp:ControlParameter Name="UserId"  PropertyName="Value"
                                        ControlID="hidSerachUserId" Type="Int32"/>
                                </CommandParameters>
                    </asp:EntityDataSource>
                <asp:RequiredFieldValidator SetFocusOnError="true"  runat="server" ID="RequiredFieldValidator9" ControlToValidate="DrpVendorCode" ErrorMessage="* Required" ForeColor="Red"></asp:RequiredFieldValidator>

            </div>
        </div>
    </div>

    <div class="row margin-10 margin-b-0 padding-lr-10">



        <div class="col-sm-4 col-md-4 col-xs-12 padding-0">
            <div class="col-sm-3 col-md-3 col-xs-12 padding-0">
                <label class="control-label lable-txt" for="name">Company</label>
            </div>
            <div class="col-sm-9 col-md-9 col-xs-12 padding-0">

                <%--    <asp:DropDownList ID="drpCompanyCode" runat="server" AutoPostBack="true" CssClass="form-control" OnSelectedIndexChanged="drpCompanyCode_SelectedIndexChanged">
                </asp:DropDownList>--%>
                <telerik:RadComboBox CausesValidation="false" RenderMode="Lightweight" ID="drpCompanyCode" runat="server" Height="200" Width="305" AutoPostBack="true"
                    EmptyMessage="Select Company Code" MarkFirstMatch="true" Filter="Contains" EnableLoadOnDemand="false" OnSelectedIndexChanged="drpCompanyCode_SelectedIndexChanged">
                </telerik:RadComboBox>
                <asp:RequiredFieldValidator  SetFocusOnError="true" runat="server" ID="RequiredFieldValidator10" ControlToValidate="drpCompanyCode" ErrorMessage="* Required" ForeColor="Red"></asp:RequiredFieldValidator>

            </div>
        </div>



        <div class="col-sm-4 col-md-4 col-xs-12 ">
            <div class="col-sm-3 col-md-3 col-xs-12 padding-0">
                <label class="control-label lable-txt" for="name">Vendor Name</label>
            </div>
            <div class="col-sm-9 col-md-9 col-xs-12 padding-0">

                <asp:TextBox ID="txtVendorName" Enabled="false" MaxLength="50" runat="server" CssClass="search-query form-control" placeholder="Enter Vendor Name" />
                <asp:RequiredFieldValidator SetFocusOnError="true" runat="server" ID="RequiredFieldValidator11" ControlToValidate="txtVendorName" ErrorMessage="* Required" ForeColor="Red"></asp:RequiredFieldValidator>

            </div>
        </div>



        <div class="col-sm-4 col-md-4 col-xs-12">
            <div class="col-sm-3 col-md-3 col-xs-12 padding-0">
                <label class="control-label lable-txt" for="name">Vendor PAN No.</label>
            </div>
            <div class="col-sm-9 col-md-9 col-xs-12 padding-0">

                <asp:TextBox ID="txtPanCardNo" runat="server" MaxLength="15" CssClass="search-query form-control disabled" placeholder="PAN Card No." />
                <%-- <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator12" ControlToValidate="txtPanCardNo" ErrorMessage="* Required" ForeColor="Red"></asp:RequiredFieldValidator>--%>
            </div>
        </div>
    </div>
    <div class="col-xs-12" style="padding: 10px; padding-left: 15px;">
        <div class="text-info small">
            Note:If PAN No. is not available. Please contact to SAP admin otherwise Payments cannot be done for this Vendor.
        </div>
    </div>
    <div class="col-xs-6 ">
        <div class="col-xs-12 padding-0">

            <strong>
                <asp:Label ID="lblRequestType" runat="server" class="control-label lable-txt text-success"></asp:Label></strong>
        </div>

    </div>
    <div class="col-xs-6">


        <asp:Button ID="btnAuditTrail" OnClick="btnAuditTrail_Click" runat="server" CssClass="btn btn-primary pull-right button-style margin-0 submit-btn" Text="Audit Trail" />


    </div>
    <hr />
    <div class="row margin-0 padding-lr-10">
        <div class="col-xs-12 padding-lr-10">
            <label class="control-label lable-txt" for="name">Add Attachment – Add (Any One)</label>


        </div>
        <div class="col-xs-12 margin-10"><small><i>Attach cancelled cheque, Account Certificate</i></small></div>
        <table class="table table-striped table-bordered">
            <thead>
                <tr>
                    <th class="text-center valign">Document Type</th>

                    <th class="text-center valign">Attachment</th>
                    <th class="text-center valign">View</th>
                </tr>
            </thead>
            <tbody>
                <tr class="text-center">
                    <td class="valign">Cancelled Cheque
                    </td>

                    <td class="valign">
                        <div class="input-group">

                            <telerik:RadAsyncUpload RenderMode="Lightweight" ID="uploadCancelChq" runat="server" Width="100%"
                                MaxFileSize="2097152" TemporaryFileExpiration="00:30:00" TargetFolder="Upload/Bank Detail Attachment/"
                                OnClientFileUploaded="callAjaxRequest" OnFileUploaded="uploadCancelChq_FileUploaded"
                                OverwriteExistingFiles="true" AutoAddFileInputs="false" Localization-Select="Upload Image">
                                <Localization Select="Browse" />
                            </telerik:RadAsyncUpload>
                        </div>

                    </td>
                    <td class="valign">
                        <div class="input-group">
                            <%--<a href="#" runat="server" target="_blank" id="attachment1" class="col-md-12">
                                <asp:Label ID="lblAttachment1" runat="server" Text="View"></asp:Label>
                            </a>--%>
                            <asp:HyperLink ID="attachment1" runat="server" Text="View" NavigateUrl="#" Target="_blank"></asp:HyperLink>
                        </div>
                    </td>


                </tr>
                <tr class="text-center">
                    <td class="valign">Account Certificate
                    </td>

                    <td class="valign">
                        <div class="input-group">
                            <telerik:RadAsyncUpload RenderMode="Lightweight" ID="uploadAccCertificate" runat="server" Width="100%"
                                MaxFileSize="2097152" AutoAddFileInputs="false" Localization-Select="Upload Image" TargetFolder="Upload/Bank Detail Attachment/"
                                OnClientFileUploaded="callAjaxRequest" OnFileUploaded="uploadAccCertificate_FileUploaded">
                                <Localization Select="Browse" />
                            </telerik:RadAsyncUpload>
                        </div>

                    </td>
                    <td class="valign">
                        <div class="input-group">
                            <%--<a href="#" runat="server" target="_blank" id="attachment2" class="col-md-12">
                                <asp:Label ID="lblAttachment2" runat="server" Text="View"></asp:Label>

                            </a>--%>

                             <asp:HyperLink ID="attachment2" runat="server" Text="View" NavigateUrl="#" Target="_blank"></asp:HyperLink>

                        </div>
                    </td>

                </tr>
            </tbody>
        </table>
        <div class="col-xs-12 padding-0">
            <div class="button-wrapper pull-left">
            </div>
        </div>
    </div>
    <div class="col-xs-12">
        <div class="row margin-0 ">

        
        </div>
        <!-- End of Attachment-row-->
    </div>



    <div class="col-sm-6 col-md-6 margin-10">
        <div class="row margin-0">
            <label class="control-label lable-txt subheading margin-10">New Values</label>
        </div>

        <div class="row margin-0 ">
            <div class="col-sm-3 col-md-3 col-xs-12 padding-0">
                <label class="control-label lable-txt" for="name">Account Type</label>
            </div>
            <div class="col-sm-9 col-md-9 col-xs-12 padding-0">

                <asp:DropDownList ID="drpNewAccountType" runat="server" CssClass="form-control">
                </asp:DropDownList>
                <asp:RequiredFieldValidator SetFocusOnError="true" runat="server" ID="RequiredFieldValidator3" ControlToValidate="drpNewAccountType" ErrorMessage="* Required" ForeColor="Red"></asp:RequiredFieldValidator>
            </div>
        </div>
        <!-- End of vender name-row-->

        <div class="row margin-0 ">
            <div class="col-sm-3 col-md-3 col-xs-12 padding-0">
                <label class="control-label lable-txt" for="name">Account Number</label>
            </div>
            <div class="col-sm-9 col-md-9 col-xs-12 padding-0">


                <asp:TextBox ID="txtNewAccountNo" AutoPostBack="true" OnTextChanged="txtNewAccountNo_TextChanged" MaxLength="25" runat="server" CssClass="search-query form-control" placeholder="Enter Account Number" />
                <asp:RequiredFieldValidator SetFocusOnError="true" runat="server" ID="RFV1" ControlToValidate="txtNewAccountNo" ErrorMessage="* Required" ForeColor="Red"></asp:RequiredFieldValidator>

            </div>
        </div>
        <!-- End of Account Number-row-->

        <div class="row margin-0 ">
            <div class="col-sm-3 col-md-3 col-xs-12 padding-0">
                <label class="control-label lable-txt" for="name">IFSC Code</label>
            </div>
            <div class="col-sm-9 col-md-9 col-xs-12 padding-0">
                <asp:TextBox ID="txtNewIFSCCode" MaxLength="11" AutoPostBack="true" runat="server" CssClass="search-query form-control" placeholder="Enter IFSC Code" OnTextChanged="txtNewIFSCCode_TextChanged" />
                <asp:RequiredFieldValidator SetFocusOnError="true" Display="Dynamic" runat="server" ID="RequiredFieldValidator1" ControlToValidate="txtNewIFSCCode" ErrorMessage="* Required" ForeColor="Red"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator SetFocusOnError="true" Display="Dynamic" ControlToValidate="txtNewIFSCCode" ID="RegularExpressionValidator6" ValidationExpression="^[a-zA-Z0-9]{11,}$" runat="server" ForeColor="Red" ErrorMessage="* Enter Correct IFSC Code."></asp:RegularExpressionValidator>

            </div>
        </div>
        <!-- End of IFSC Code-row-->

        <div class="row margin-0 ">
            <div class="col-sm-3 col-md-3 col-xs-12 padding-0">
                <label class="control-label lable-txt" for="name">Bank Name</label>
            </div>
            <div class="col-sm-9 col-md-9 col-xs-12 padding-0">


                <asp:TextBox ID="txtNewBankName" MaxLength="70" runat="server" CssClass="search-query form-control" placeholder="Enter Bank Name" />
                <asp:RequiredFieldValidator SetFocusOnError="true" runat="server" ID="RequiredFieldValidator2" ControlToValidate="txtNewBankName" ErrorMessage="* Required" ForeColor="Red"></asp:RequiredFieldValidator>

            </div>
        </div>
        <!-- End of Bank Name-row-->

        <div class="row margin-0 ">
            <div class="col-sm-3 col-md-3 col-xs-12 padding-0">
                <label class="control-label lable-txt" for="name">Branch</label>
            </div>
            <div class="col-sm-9 col-md-9 col-xs-12 padding-0">


                <asp:TextBox ID="txtNewBranch" MaxLength="40" runat="server" CssClass="search-query form-control" placeholder="Enter Branch Name" />
                <asp:RequiredFieldValidator SetFocusOnError="true" runat="server" ID="RequiredFieldValidator4" ControlToValidate="txtNewBranch" ErrorMessage="* Required" ForeColor="Red"></asp:RequiredFieldValidator>

            </div>
        </div>
        <!-- End of Branch-row-->

        <div class="row margin-0 ">
            <div class="col-sm-3 col-md-3 col-xs-12 padding-0">
                <label class="control-label lable-txt" for="name">City</label>
            </div>
            <div class="col-sm-9 col-md-9 col-xs-12 padding-0">


                <asp:TextBox ID="txtNewCity" MaxLength="40" runat="server" CssClass="search-query form-control" placeholder="Enter City Name" />
                <asp:RequiredFieldValidator SetFocusOnError="true" runat="server" ID="RequiredFieldValidator5" ControlToValidate="txtNewCity" ErrorMessage="* Required" ForeColor="Red"></asp:RequiredFieldValidator>

            </div>
        </div>
        <!-- End of Branch-row-->

        <div class="row margin-0 ">
            <div class="col-sm-3 col-md-3 col-xs-12 padding-0">
                <label class="control-label lable-txt" for="name">Suzlon Email ID 1</label>
            </div>
            <div class="col-sm-9 col-md-9 col-xs-12 padding-0">


                <asp:TextBox ID="txtNewSuzlonEmail1" MaxLength="50" runat="server" CssClass="search-query form-control" placeholder="Enter Suzlon Email ID" />
                <asp:RequiredFieldValidator SetFocusOnError="true" runat="server" ID="RequiredFieldValidator6" ControlToValidate="txtNewSuzlonEmail1" ErrorMessage="* Required" ForeColor="Red"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator SetFocusOnError="true" Display="Dynamic" ControlToValidate="txtNewSuzlonEmail1" ID="RegularExpressionValidator5" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" runat="server" ForeColor="Red" ErrorMessage="* Enter valid email id."></asp:RegularExpressionValidator>

            </div>
        </div>
        <!-- End of Suzlone Email ID 1-row-->

        <div class="row margin-0 ">
            <div class="col-sm-3 col-md-3 col-xs-12 padding-0">
                <label class="control-label lable-txt" for="name">Suzlon Email ID 2</label>
            </div>
            <div class="col-sm-9 col-md-9 col-xs-12 padding-0">
                <div class="form-group">

                    <asp:TextBox ID="txtNewSuzlonEmail2" MaxLength="50" runat="server" CssClass="search-query form-control" placeholder="Enter Suzlon Email ID 2" />
                    <%--<asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator7" ControlToValidate="txtNewSuzlonEmail2" ErrorMessage="* Required" ForeColor="Red"></asp:RequiredFieldValidator>					--%>
                    <asp:RegularExpressionValidator SetFocusOnError="true" Display="Dynamic" ControlToValidate="txtNewSuzlonEmail2" ID="RegularExpressionValidator1" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" runat="server" ForeColor="Red" ErrorMessage="* Enter valid email id."></asp:RegularExpressionValidator>
                </div>
            </div>
        </div>
        <!-- End of Suzlone Email ID 2-row-->

        <div class="row margin-0 ">
            <div class="col-sm-3 col-md-3 col-xs-12 padding-0">
                <label class="control-label lable-txt" for="name">Vendor Email ID 1</label>
            </div>
            <div class="col-sm-9 col-md-9 col-xs-12 padding-0">
                <div class="form-group">

                    <asp:TextBox ID="txtNewVendorEmail1" MaxLength="50" runat="server" CssClass="search-query form-control" placeholder="Vendor Email ID 1" />
                    <asp:RequiredFieldValidator SetFocusOnError="true" runat="server" ID="RequiredFieldValidator8" ControlToValidate="txtNewVendorEmail1" ErrorMessage="* Required" ForeColor="Red"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator SetFocusOnError="true" Display="Dynamic" ControlToValidate="txtNewVendorEmail1" ID="RegularExpressionValidator2" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" runat="server" ForeColor="Red" ErrorMessage="* Enter valid email id."></asp:RegularExpressionValidator>
                </div>
            </div>
        </div>
        <!-- End of Vendor Email ID 1-row-->

        <div class="row margin-0 ">
            <div class="col-sm-3 col-md-3 col-xs-12 padding-0">
                <label class="control-label lable-txt" for="name">Vendor Email ID 2</label>
            </div>
            <div class="col-sm-9 col-md-9 col-xs-12 padding-0">
                <div class="form-group">

                    <asp:TextBox ID="txtNewVendorEmail2" MaxLength="50" runat="server" CssClass="search-query form-control" placeholder="Vendor Email ID 2" />
                    <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="txtNewVendorEmail2" ID="RegularExpressionValidator3" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" runat="server" ForeColor="Red" ErrorMessage="* Enter valid email id."></asp:RegularExpressionValidator>
                </div>
            </div>
        </div>
        <!-- End of Vendor Email ID 2-row-->



      
        <!-- End of Status-row-->

        <div class="row margin-0 ">
            <div class="col-sm-3 col-md-3 col-xs-12 padding-0">
                <label class="control-label lable-txt" for="name">
                    <asp:Label ID="lblSubVertical" runat="server" Text="Sub Vertical" class="control-label lable-txt"></asp:Label>
                </label>

            </div>
            <div class="col-sm-9  col-md-9 col-xs-12 padding-0">
                <div class="form-group">
                    <%--   <asp:DropDownList ID="drpSubVertical" OnSelectedIndexChanged="drpSubVertical_SelectedIndexChanged" AutoPostBack="true" runat="server" CssClass="form-control">
                    </asp:DropDownList>--%>
                    <telerik:RadComboBox CausesValidation="false" RenderMode="Lightweight" ID="drpSubVertical" runat="server" Height="200" Width="305" AutoPostBack="true"
                        MarkFirstMatch="true" Filter="Contains" EnableLoadOnDemand="false" OnSelectedIndexChanged="drpSubVertical_SelectedIndexChanged" EmptyMessage="Select Sub Vertical">
                    </telerik:RadComboBox>


                </div>
            </div>
            <!-- /.form-group -->
        </div>

        <!-- End of Sub Vertical-row-->

        <div class="row margin-0 ">
            <div class="col-sm-3 col-md-3 col-xs-12 padding-0">
                <label class="control-label lable-txt" for="name">
                    <asp:Label ID="lblVertical" runat="server" Text="Vertical" class="control-label lable-txt"></asp:Label>
                </label>
            </div>
            <div class="col-sm-9 col-md-9 col-xs-12 padding-0">
                <div class="form-group">
                    <asp:DropDownList ID="drpVertical" runat="server" CssClass="form-control">
                    </asp:DropDownList>
                </div>
            </div>
            <!-- /.form-group -->
        </div>

        <!-- End of Vertical-row-->

          <div class="row margin-0 ">
            <div class="col-sm-3 col-md-3 col-xs-12 padding-0">
                <label class="control-label lable-txt" for="name">
                    <asp:Label runat="server" ID="lblStatus" Text="Status"></asp:Label>
                </label>
            </div>
            <div class="col-sm-9 col-md-9 col-xs-12 padding-0">
                <div class="form-group">

                    <asp:DropDownList ID="drpStatus" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="drpStatus_SelectedIndexChanged">
                    </asp:DropDownList>

                </div>
                <!-- /.form-group -->
            </div>
        </div>
        <!-- End of Status-row-->

        <div class="row margin-0 ">
            <div class="col-sm-3 col-md-3 col-xs-12 padding-0">
                <label class="control-label lable-txt" for="name">
                    <asp:Label ID="lblAssignto" runat="server">Assign To</asp:Label>
                </label>
            </div>
            <div class="col-sm-9 col-md-9 col-xs-12 padding-0">
                <div class="form-group">
                    <asp:DropDownList ID="drpAssignTo" runat="server" CssClass="form-control">
                    </asp:DropDownList>
                </div>
                <!-- /.form-group -->
            </div>
        </div>





    </div>
    <!-- End of col 1of details-->
    <div class="col-sm-6 col-md-6 margin-10">
        <div class="row margin-0">
            <label class="control-label lable-txt subheading margin-10">Old Values</label>
        </div>

        <!-- End of vender name-row-->

        <div class="row margin-0 " style="padding-bottom: 9px !important;">
            <div class="col-sm-3 col-md-3 col-xs-12 padding-0 ">
                <label class="control-label lable-txt" for="name">Account Type</label>
            </div>
            <div class="col-sm-9 col-md-9 col-xs-12 padding-0">

                <asp:TextBox ID="txtOldAccType" runat="server" Enabled="false" CssClass="search-query form-control disabled" placeholder="Account Type" disabled/>
            </div>
        </div>
        <!-- End of vender name-row-->

        <div class="row margin-0" style="padding-bottom: 9px !important;">
            <div class="col-sm-3 col-md-3 col-xs-12 padding-0">
                <label class="control-label lable-txt" for="name">Account Number</label>
            </div>
            <div class="col-sm-9 col-md-9 col-xs-12 padding-0">
                <div class="form-group">

                    <asp:TextBox ID="txtOldAccountNo" runat="server" Enabled="false" CssClass="search-query form-control disabled" placeholder="Enter Account Number" />
                </div>
            </div>
        </div>
        <!-- End of Account Number-row-->

        <div class="row margin-0 ">
            <div class="col-sm-3 col-md-3 col-xs-12 padding-0">
                <label class="control-label lable-txt" for="name">IFSC Code</label>
            </div>
            <div class="col-sm-9 col-md-9 col-xs-12 padding-0">
                <div class="form-group">

                    <asp:TextBox ID="txtOldIFSCCode" Enabled="false" runat="server" CssClass="search-query form-control disabled" placeholder="Enter IFSC Code" />
                </div>
            </div>
        </div>
        <!-- End of IFSC Code-row-->

        <div class="row margin-0 padding-b6">
            <div class="col-sm-3 col-md-3 col-xs-12 padding-0">
                <label class="control-label lable-txt" for="name">Bank Name</label>
            </div>
            <div class="col-sm-9 col-md-9 col-xs-12 padding-0">
                <div class="form-group">

                    <asp:TextBox ID="txtOldBankName" Enabled="false" runat="server" CssClass="search-query form-control disabled" placeholder="Enter Bank Name" />
                </div>
            </div>
        </div>
        <!-- End of Bank Name-row-->

        <div class="row margin-0 " style="padding-bottom: 8px !important;">
            <div class="col-sm-3 col-md-3 col-xs-12 padding-0">
                <label class="control-label lable-txt" for="name">Branch</label>
            </div>
            <div class="col-sm-9 col-md-9 col-xs-12 padding-0">
                <div class="form-group">

                    <asp:TextBox ID="txtOldBranch" runat="server" Enabled="false" CssClass="search-query form-control disabled" placeholder="Enter Branch Name" />
                </div>
            </div>
        </div>
        <!-- End of Branch-row-->

        <div class="row margin-0 padding-b6">
            <div class="col-sm-3 col-md-3 col-xs-12 padding-0">
                <label class="control-label lable-txt" for="name">City</label>
            </div>
            <div class="col-sm-9 col-md-9 col-xs-12 padding-0">
                <div class="form-group">

                    <asp:TextBox ID="txtOldCity" Enabled="false" runat="server" CssClass="search-query form-control disabled" placeholder="Enter City Name" />
                </div>
            </div>
        </div>
        <!-- End of Branch-row-->

        <div class="row margin-0" style="padding-bottom: 8px !important;">
            <div class="col-sm-3 col-md-3 col-xs-12 padding-0">
                <label class="control-label lable-txt" for="name">Suzlon Email ID 1</label>
            </div>
            <div class="col-sm-9 col-md-9 col-xs-12 padding-0">
                <div class="form-group">

                    <asp:TextBox ID="txtOldSuzlonEmail1" runat="server" Enabled="false" CssClass="search-query form-control disabled" placeholder="Enter Suzlon Email ID" />
                </div>
            </div>
        </div>
        <!-- End of Suzlone Email ID 1-row-->

        <div class="row margin-0 padding-b6">
            <div class="col-sm-3 col-md-3 col-xs-12 padding-0">
                <label class="control-label lable-txt" for="name">Suzlon Email ID 2</label>
            </div>
            <div class="col-sm-9 col-md-9 col-xs-12 padding-0">
                <div class="form-group">

                    <asp:TextBox ID="txtOldSuzlonEmail2" Enabled="false" runat="server" CssClass="search-query form-control disabled" placeholder="Enter Suzlon Email ID 2" />
                </div>
            </div>
        </div>
        <!-- End of Suzlone Email ID 2-row-->

        <div class="row margin-0 " style="padding-bottom: 16px !important;">
            <div class="col-sm-3 col-md-3 col-xs-12 padding-0">
                <label class="control-label lable-txt" for="name">Vendor Email ID 1</label>
            </div>
            <div class="col-sm-9 col-md-9 col-xs-12 padding-0">
                <div class="form-group">

                    <asp:TextBox ID="txtOldVendorEmail1" Enabled="false" runat="server" CssClass="search-query form-control disabled" placeholder="Vendor Email ID 1" />
                </div>
            </div>
        </div>
        <!-- End of Vendor Email ID 1-row-->

        <div class="row margin-0">
            <div class="col-sm-3 col-md-3 col-xs-12 padding-0">
                <label class="control-label lable-txt" for="name">Vendor Email ID 2</label>
            </div>
            <div class="col-sm-9  col-md-9 col-xs-12 padding-0">
                <div class="form-group">

                    <asp:TextBox ID="txtOldVendorEmail2" runat="server" Enabled="false" CssClass="search-query form-control disabled" placeholder="Vendor Email ID 2" />
                </div>
            </div>
        </div>
        <!-- End of Vendor Email ID 2-row-->

        <div class="row margin-0 padding-b6">
            <div class="col-sm-3 col-md-3 col-xs-12 padding-0">
                <%--  <label class="control-label lable-txt" for="name">Original Document Receipt</label>--%>
                <label class="control-label lable-txt" for="name">
                    <asp:Label ID="lbldocSent" runat="server" Visible="false">Original Document Receipt</asp:Label>
                </label>

            </div>
            <div class="col-sm-9 col-md-9 col-xs-12 padding-0">
                <div class="col-xs-12 padding-0">
                    <label class=" control-label lable-txt">
                        <asp:CheckBox ID="chkDocSent" Visible="false" runat="server" Text="Document Sent" AutoPostBack="true" OnCheckedChanged="chkDocSent_CheckedChanged" />
                    </label>
                </div>
                <div class="row margin-0">
                    <div class="col-xs-12">
                        <%-- <label class="lable-txt">Dispatch Date</label>--%>
                        <label class="control-label lable-txt" for="name">
                            <asp:Label ID="lblDispatchDate" Visible="false" runat="server">Dispatch Date</asp:Label>
                        </label>

                    </div>

                    <div class="col-xs-12">
                        <div class="input-group">

                            <telerik:RadDatePicker Visible="false" RenderMode="Lightweight" ID="dispatchDate" Width="100%" EnableTyping="false" runat="server">
                            </telerik:RadDatePicker>

                        </div>
                    </div>
                </div>
                <div class="clearfix"></div>
                <div class="row margin-0">
                    <div class="col-xs-12">
                        <label class=" control-label lable-txt">

                            <asp:CheckBox ID="chkDocReceived" Visible="false" Text="Document Received" runat="server" AutoPostBack="true" OnCheckedChanged="chkDocReceived_CheckedChanged" />
                            &nbsp;</label>
                    </div>

                    <div class="col-xs-12">
                        <div class="input-group">

                            <telerik:RadDatePicker Visible="false" RenderMode="Lightweight" ID="receiveDate" EnableTyping="false" Width="100%" runat="server">
                            </telerik:RadDatePicker>
                        </div>
                    </div>
                </div>

            </div>
        </div>
        <!-- End of DOcument Receipt ID 2-row-->

    </div>
    <!-- End of Col 2 of details-->
    <div class="row margin-0 ">
        <div class="col-sm-12 col-xs-12">
            <label class="control-label lable-txt" for="name">
                Duplicate/Similar data based on IFSC Code & Account No.
            </label>
        </div>
        <div class="col-sm-12 col-xs-12">

            <telerik:RadGrid RenderMode="Lightweight" ID="grdVendorBankDtl" runat="server" AutoGenerateColumns="false"
                AllowPaging="true" AllowSorting="true" AllowFilteringByColumn="true" OnNeedDataSource="grdVendorBankDtl_NeedDataSource"
                PageSize="<%# Convert.ToInt32(SuzlonBPP.Models.Constants.CONST_GRID_PAGE_SIZE) %>">
                <GroupingSettings CaseSensitive="false" />
                <MasterTableView ClientDataKeyNames="VendorCode" EditMode="InPlace" CommandItemDisplay="None" EnableNoRecordsTemplate="true" AllowFilteringByColumn="true">
                    <NoRecordsTemplate>
                        <table width="100%" border="0" cellpadding="20" cellspacing="20">
                            <tr>
                                <td align="center" class="txt-white">No records to display.
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <CommandItemSettings ShowRefreshButton="false" />
                    <Columns>
                        <telerik:GridBoundColumn DataField="VendorCode" HeaderText="Vendor Code" DataType="System.String" />
                        <telerik:GridBoundColumn DataField="VendorName" HeaderText="Vendor Name" DataType="System.String" />
                        <telerik:GridBoundColumn DataField="CompanyCode" HeaderText="Company Code" DataType="System.String" />
                        <telerik:GridBoundColumn DataField="CompanyName" HeaderText="Company Name" DataType="System.String" />
                        <telerik:GridBoundColumn DataField="IFSCCode" HeaderText="IFSC Code" DataType="System.String" />
                        <telerik:GridBoundColumn DataField="AccountNumber" HeaderText="Account Number" DataType="System.String" />

                    </Columns>
                    <PagerStyle PageSizes="5,10,15" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                        PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />
                </MasterTableView>
            </telerik:RadGrid>



        </div>
    </div>
    <!-- End of Action-row-->
    <!-- End of Col 2 of details-->
    <!-- End of Col 2 of details-->

    <div class="row margin-0 ">
        <div class="col-sm-12 col-xs-12">
            <label class="control-label lable-txt" for="name">
                <asp:Label ID="lblLogdtl" runat="server" Text="Log Details"></asp:Label>
            </label>
        </div>
        <div class="col-sm-12 col-xs-12">

            <telerik:RadGrid RenderMode="Lightweight" ID="grdLogDetail" runat="server" AutoGenerateColumns="false"
                AllowPaging="true" AllowSorting="true" AllowFilteringByColumn="true" OnNeedDataSource="grdLogDetail_NeedDataSource"
                PageSize="<%# Convert.ToInt32(SuzlonBPP.Models.Constants.CONST_GRID_PAGE_SIZE) %>">
                <GroupingSettings CaseSensitive="false" />
                <MasterTableView ClientDataKeyNames="Status" EditMode="InPlace" CommandItemDisplay="None" EnableNoRecordsTemplate="true" AllowFilteringByColumn="true">
                    <NoRecordsTemplate>
                        <table width="100%" border="0" cellpadding="20" cellspacing="20">
                            <tr>
                                <td align="center" class="txt-white">No records to display.
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <CommandItemSettings ShowRefreshButton="false" />
                    <Columns>
                        <telerik:GridBoundColumn DataField="Status" HeaderText="Stage" DataType="System.String" />
                        <telerik:GridBoundColumn DataField="Approver" HeaderText="Approver" DataType="System.String" />
                        <%-- <telerik:GridBoundColumn DataField="ModifiedOn" HeaderText="Approve Date" DataType="System.String" />    --%>
                        <%--   <telerik:GridDateTimeColumn DataField="ModifiedOn" HeaderText="ModifiedOn" EnableTimeIndependentFiltering="true" DataFormatString="{0:MM/dd/yyyy}">
                            <HeaderStyle Width="130px" />
                        </telerik:GridDateTimeColumn>--%>
                    </Columns>
                    <PagerStyle PageSizes="5,10,15" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                        PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />
                </MasterTableView>
            </telerik:RadGrid>



        </div>
    </div>


    <div class="row margin-0 ">
        <div class="col-sm-12 col-xs-12">
            <label class="control-label lable-txt" for="name">
                <asp:Label ID="lblWorkFlowApproveLog" runat="server" Text="Log Audit Details"></asp:Label>
            </label>
        </div>
        <div class="col-sm-12 col-xs-12">

            <telerik:RadGrid RenderMode="Lightweight" ID="grdApprovalAuditDtl" runat="server" AutoGenerateColumns="false"
                AllowPaging="true" AllowSorting="true" AllowFilteringByColumn="true" OnNeedDataSource="grdApprovalAuditDtl_NeedDataSource"
                PageSize="<%# Convert.ToInt32(SuzlonBPP.Models.Constants.CONST_GRID_PAGE_SIZE) %>">
                <GroupingSettings CaseSensitive="false" />
                <MasterTableView ClientDataKeyNames="Status" EditMode="InPlace" CommandItemDisplay="None" EnableNoRecordsTemplate="true" AllowFilteringByColumn="true">
                    <NoRecordsTemplate>
                        <table width="100%" border="0" cellpadding="20" cellspacing="20">
                            <tr>
                                <td align="center" class="txt-white">No records to display.
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <CommandItemSettings ShowRefreshButton="false" />
                    <Columns>
                        <telerik:GridBoundColumn DataField="Status" HeaderText="Stage" DataType="System.String" />
                        <telerik:GridBoundColumn DataField="UserName" HeaderText="Approver" DataType="System.String" />
                        <telerik:GridDateTimeColumn DataField="CreatedOn" HeaderText="ModifiedOn" EnableTimeIndependentFiltering="true">
                            <HeaderStyle Width="130px" />
                        </telerik:GridDateTimeColumn>
                        <telerik:GridBoundColumn DataField="Comment" HeaderText="Remark" DataType="System.String" />
                    </Columns>
                    <PagerStyle PageSizes="5,10,15" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                        PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />
                </MasterTableView>
            </telerik:RadGrid>



        </div>
    </div>
    <!-- End of Action-row-->
    <!-- End of Col 2 of details-->


    <div class="row margin-0 ">
        <div class="col-sm-12 col-xs-12">
            <label class="control-label lable-txt" for="name">Comments/Remarks</label>
        </div>
        <div class="col-sm-12 col-xs-12" style="padding-right: 0px;">
            <div class="row chat-window col-xs-12 col-md-12 padding-0" id="chat_window_1">
                <div class="col-xs-12 col-md-12" style="padding-right: 0px;">
                    <div class="panel panel-default">
                        <div class="panel-body msg_container_base">
                            <asp:Label ID="lblCommentDetail" CssClass="messages msg_sent" runat="server" Text=""></asp:Label>


                        </div>
                        <div class="panel-footer">
                            <div class="input-group">

                                <asp:TextBox runat="server" MaxLength="150" ID="txtCommentBox" Text="" class="form-control " placeholder="Enter comments/Remarks here..."></asp:TextBox>
                                <span class="input-group-btn">

                                    <asp:Button ID="btnAddComment" CausesValidation="false" runat="server" Text="Comment" class="btn btn-primary btn-sm" OnClick="btnAddComment_Click" />
                                </span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- End of Action-row-->

    <div class="col-xs-12">
        <div class="button-wrapper">

            <ul>

                <li>
                    <asp:Button ID="BtnSave" runat="server" CssClass="btn btn-primary button button-style submit-btn" Text="Submit" OnClick="BtnSave_Click" />
                </li>
                <li>
                    <asp:Button ID="BtnCancel" runat="server" CausesValidation="false" CssClass="btn btn-primary button button-style submit-btn" Text="Cancel" OnClick="BtnCancel_Click" />
                </li>

            </ul>

        </div>
    </div>
    <!-- Start of Modals------>
    <div class="modal fade" id="squarespaceModal" tabindex="-1" role="dialog" aria-labelledby="modalLabel" aria-hidden="true">
        <div class="modal-dialog1">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">×</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title subheading margin-0" id="lineModalLabel">Audit Trail</h4>
                </div>
                <div class="modal-body" style="overflow: hidden; overflow-x: scroll; overflow-y: scroll; max-height: 400px !important;">

                    <telerik:RadGrid RenderMode="Lightweight" ID="grdBankAuditTrial" runat="server"
                        AutoGenerateColumns="false" AllowPaging="true" AllowSorting="true" AllowFilteringByColumn="false" PageSize="<%# Convert.ToInt32(SuzlonBPP.Models.Constants.CONST_GRID_PAGE_SIZE) %>" OnNeedDataSource="grdBankAuditTrial_NeedDataSource">
                        <GroupingSettings CaseSensitive="false" />
                        <MasterTableView CommandItemSettings-ShowRefreshButton="false" ClientDataKeyNames="" EditMode="InPlace" CommandItemDisplay="None" AllowFilteringByColumn="false">
                            <NoRecordsTemplate>
                                <table width="100%" border="0" cellpadding="20" cellspacing="20">
                                    <tr>
                                        <td align="center" class="txt-white">No records to display.
                                        </td>
                                    </tr>
                                </table>
                            </NoRecordsTemplate>
                            <CommandItemSettings ShowRefreshButton="false" ShowExportToExcelButton="false" />
                            <Columns>
                                <telerik:GridBoundColumn DataField="FieldName" HeaderText="Field Name" UniqueName="FieldName">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="ModifiedOn" HeaderText="Last Modified Date" UniqueName="LastModifiedDate">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="ModifiedBy" HeaderText="ModifiedBy" UniqueName="ModifiedBy">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="NewValue" HeaderText="New Value" UniqueName="NewValue">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="OldValue" HeaderText="Old Value" UniqueName="OldValue">
                                </telerik:GridBoundColumn>
                            </Columns>
                            <PagerStyle PageSizes="5,10,15" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                                PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />
                        </MasterTableView>
                    </telerik:RadGrid>

                </div>

            </div>
            <asp:HiddenField runat="server" ID="hidSerachUserId" />
        </div>
        <telerik:RadNotification RenderMode="Lightweight" Position="Center" runat="server" ID="radMessage" VisibleOnPageLoad="false"
            Width="350px" TitleIcon="none" ContentIcon="none" EnableShadow="true" AnimationDuration="1000">
        </telerik:RadNotification>
    </div>

    <!-- End of Modals-------->



</asp:Content>
