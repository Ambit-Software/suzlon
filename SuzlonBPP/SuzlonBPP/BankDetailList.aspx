<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BankDetailList.aspx.cs" MasterPageFile="SuzlonBPP.Master" Inherits="SuzlonBPP.BankDetailList" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        /*.nav-tabs > li > div a {
            border: 1px solid transparent;
            border-radius: 4px 4px 0 0;
            line-height: 21px;
            margin-right: 2px;
            padding: 5px;
            color: #FFF;
            text-decoration: none;
        }

        .nav-tabs > li > div :hover {
            background: #FFF;
            color: #333;
        }*/

        /*.nav-tabs {
            border-bottom: none !important;
        }*/

        /*.nav-tabs > li > div > a {
                border: 1px solid transparent;
                border-radius: 4px 4px 0 0;
                line-height: 1.42857;
                margin-right: 2px;
                text-decoration: none !important;
            }*/

        .nav > li > div {
            display: inline;
        }

            /*.nav-tabs > li > div {
            border: 1px solid transparent;
            border-radius: 4px 4px 0 0;
            line-height: 1.42857;
            margin-right: 2px;
        }

            .nav-tabs > li > div > a > div {
                display: inline !important;
            }*/

            /*.nav-tabs > li > a > div {
            display: inline !important;
        }*/

            .nav > li > div > a > div {
                display: inline !important;
            }

        .nav > li > a > div {
            display: inline !important;
        }

        .nav > li > a > span > div {
            display: inline !important;
        }

        .nav > li > div {
            display: inline;
        }

        .gridHyperlinks {
            text-decoration: none !important;
            color: #fff !important;
        }

        .hidden {
            display: none !important;
            visibility: hidden !important;
        }

        .visible {
            display: block !important;
            visibility: visible !important;
        }

        .linkButtonColor {
            color: #337ab7 !important;
        }
    </style>


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


    <%--<script src="Scripts/jquery-1.10.2.js"></script>--%>
    <script src="Scripts/bootstrap.min.js"></script>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" OnAjaxRequest="RadAjaxManager1_AjaxRequest">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="btnSearch">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="pnlData" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="grdComment" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <%--<telerik:AjaxUpdatedControl ControlID="grdPending" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>--%>
                    <%--<telerik:AjaxUpdatedControl ControlID="lblMyRecordCount" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="lblPendingCount" UpdatePanelRenderMode="Inline" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>--%>
                    <telerik:AjaxUpdatedControl ControlID="radMessage" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lblMyRecordCount">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="lblMyRecordCount" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="grdPending">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdPending" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="lblPendingCount" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="radMessage" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="grdMyRecords">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdComment" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="radMessage" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="grdCBDocPending">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdCBDocPending" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="lblDocPendingCount" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="radMessage" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
                     <telerik:AjaxSetting AjaxControlID="grdInitiatorPendingDoc">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdInitiatorPendingDoc" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="lblDocSendPendingCount" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="radMessage" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>


            <telerik:AjaxSetting AjaxControlID="DrpCompanyCode">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="txtVendorName" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="txtPanNo" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="radMessage" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadAjaxManager1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdComment" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="radMessage" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="DrpVendorCode">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="DrpCompanyCode" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="radMessage" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="txtVendorName" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="txtPanNo" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
  <%--  <telerik:RadAjaxLoadingPanel ID="LoadingPanel1" runat="server" Skin="Default"></telerik:RadAjaxLoadingPanel>--%>

        <telerik:RadAjaxLoadingPanel ID="LoadingPanel1" BackgroundPosition="Center" runat="server" Height="100%" Width="75px" Transparency="50">
        <div class="dialog-background">
            <div class="dialog-loading-wrapper">
                <span class="dialog-loading-icon"></span>
            </div>
        </div>
    </telerik:RadAjaxLoadingPanel>




    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript">
            $(document).ready(function () {

                var a = document.getElementById("<%=lblMyRecordCount.ClientID%>");
                if (a != null)
                    $(a.parentElement).css("display", "inline");
                var a = document.getElementById("<%=lblPendingCount.ClientID%>");
                if (a != null)
                    $(a.parentElement).css("display", "inline");

                var a = document.getElementById("<%=hidTabActive.ClientID%>").value;
                if (a == 'PendingApproval') {
                    var tabPending = document.getElementById("<%=tabPending.ClientID%>");
                    tabPending.click();
                }
                else if (a == 'CBDocPending') {
                    var tabCBdocPending = document.getElementById("<%=tabCBdocPending.ClientID%>");
                        tabCBdocPending.click();
                    }
            });
            function ShowAttachment(path, attachment1, attachment2) {
                if (attachment1 != undefined && attachment1 != "") {
                    $("#attachment1").text(attachment1);
                    $("#attachment1").attr("href", path + attachment1);
                }
                else {
                    $("#attachment1").text("");
                    $("#attachment1").attr("href", "#");
                }
                if (attachment2 != undefined && attachment2 != "") {
                    $("#attachment2").attr("href", path + attachment2);
                    $("#attachment2").text(attachment2);
                }
                else {
                    $("#attachment2").attr("href", "#");
                    $("#attachment2").text("");;
                }
                $('#divAttachment').modal();
            }

            function Validate(rdbApproved, rdbReject, rdbNeedCorrection, custValidatorAssignedTo, CustValidatorComment, drpAssignedTo, comment) {
                var isValid = true;
                if ($("#" + rdbNeedCorrection).is(':checked') && ($($("#" + drpAssignedTo)[0]).val().trim() == "" || $($("#" + drpAssignedTo)[0]).val().trim() == "Select Assigned To")) {
                    $($("#" + custValidatorAssignedTo)[0]).removeClass("hidden");
                    $($("#" + custValidatorAssignedTo)[0]).addClass("visible");
                    isValid = false;
                }
                else {
                    $($("#" + custValidatorAssignedTo)[0]).addClass("hidden");
                    $($("#" + custValidatorAssignedTo)[0]).removeClass("visible");

                }

                if ($($("#" + comment)[0]).val().trim() == "") {
                    $($("#" + CustValidatorComment)[0]).removeClass("hidden");
                    $($("#" + CustValidatorComment)[0]).addClass("visible");
                    isValid = false;
                }
                else {
                    $($("#" + CustValidatorComment)[0]).addClass("hidden");
                    $($("#" + CustValidatorComment)[0]).removeClass("visible");
                }
                return isValid;
            }
            function ValidateCBDoc(ReceivedDate, chkReceived, custValidatorchk, CustValidatorDate) {
                var isValid = true;
                if ($("#" + chkReceived).is(':checked')) {
                    $($("#" + custValidatorchk)[0]).removeClass("visible");
                    $($("#" + custValidatorchk)[0]).addClass("hidden");
                }
                else {
                    $($("#" + custValidatorchk)[0]).removeClass("hidden");
                    $($("#" + custValidatorchk)[0]).addClass("visible");
                    isValid = false;
                }

                var receivedDate = $($("#" + ReceivedDate)[0]).val().trim();
                if (receivedDate == "") {
                    $($("#" + CustValidatorDate)[0]).removeClass("hidden");
                    $($("#" + CustValidatorDate)[0]).addClass("visible");
                    isValid = false;
                }
                else {
                    $($("#" + CustValidatorDate)[0]).removeClass("visible");
                    $($("#" + CustValidatorDate)[0]).addClass("hidden");
                }

                return isValid;
            }


            function ShowComments(id) {
                var ajaxManager = $find("<%= RadAjaxManager1.ClientID %>");
                ajaxManager.ajaxRequest("Comment#" + id + "");
                $('#squarespaceCommentModal').modal();
            }
        </script>
    </telerik:RadCodeBlock>
    <asp:HiddenField ID="hidTabActive" runat="Server" />
    <div class="modal fade" id="divAttachment" tabindex="-1" role="dialog" aria-labelledby="modalLabel" aria-hidden="true">
        <div class="modal-dialog1">
            <div class="modal-content">
                <div class="modal-header modal-header-resize">
                    <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">×</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title text-primary" id="lineModalLabel">Attachments</h4>
                </div>
                <div class="modal-body row">
                    <div class="col-md-12">
                        <table class="table table-striped table-bordered">
                            <thead>
                                <tr>
                                    <th class="text-center valign">Document Type</th>
                                    <th class="text-center valign">Attachment</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr class="text-center">
                                    <td class="valign">Cancelled Cheque
                                    </td>
                                    <td class="valign">
                                        <div class="input-group">
                                            <a href="#" target="_blank" id="attachment1" class="col-md-12"></a>
                                        </div>
                                    </td>
                                </tr>
                                <tr class="text-center">
                                    <td class="valign">Account Certificate
                                    </td>
                                    <td class="valign">
                                        <div class="input-group">
                                            <a href="#" target="_blank" id="attachment2" class="col-md-12"></a>
                                        </div>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
                <!--End of Scroll-->
            </div>
        </div>
    </div>


    <div class="modal fade" id="squarespaceCommentModal" tabindex="-1" role="dialog" aria-labelledby="modalLabel" aria-hidden="true">
        <div class="modal-dialog1">
            <div class="modal-content">
                <div class="modal-header modal-header-resize">
                    <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">×</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title text-primary" id="lineModalCommentLabel">Comments</h4>
                </div>
                <div class="modal-body">
                    <telerik:RadGrid RenderMode="Lightweight" ID="grdComment" runat="server" OnNeedDataSource="grdComment_NeedDataSource"
                        AutoGenerateColumns="false" AllowPaging="true" AllowSorting="true" AllowFilteringByColumn="false" PageSize="10">
                        <GroupingSettings CaseSensitive="false" />
                        <MasterTableView CommandItemSettings-ShowRefreshButton="false" ClientDataKeyNames="" EditMode="InPlace" CommandItemDisplay="None" AllowFilteringByColumn="false">
                            <NoRecordsTemplate>
                                <table width="100%" border="0" cellpadding="20" cellspacing="20">
                                    <tr>
                                        <td align="center">No records to display.
                                        </td>
                                    </tr>
                                </table>
                            </NoRecordsTemplate>
                            <CommandItemSettings ShowRefreshButton="false" ShowExportToExcelButton="false" />
                            <Columns>
                                <telerik:GridBoundColumn DataField="CommentBy" HeaderText="Comment By" UniqueName="CommentBy" HeaderStyle-Width="30%">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Comment" HeaderText="Comment" UniqueName="Comment" HeaderStyle-Width="70%">
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
    <div class="modal fade" id="divComments" tabindex="-1" role="dialog" aria-labelledby="modalLabel" aria-hidden="true">
        <div class="modal-dialog1">
            <div class="modal-content">
                <div class="modal-header modal-header-resize">
                    <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">×</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title text-primary" id="hComment">Comments</h4>
                </div>
                <div class="modal-body">
                    <div id="Comment"></div>
                </div>
                <!--End of Scroll-->
            </div>
        </div>
    </div>
    <div class="container-fluid padding-0">
        <div class="row margin-0">
            <div class="col-xs-6 heading-big padding-lr-10">
                <h5 class="margin-0 lineheight-42 breath-ctrl">Home / <span>Bank Detail List</span></h5>
            </div>
            <div class="col-sm-6 col-xs-6 padding-t-6">
            </div>
        </div>
        <!-- End of heading wrapper-->


                       <div class="row margin-10 margin-b-0">
                         <div class="col-xs-12 padding-lr-10">
                         <div class="col-sm-1 col-md-1 col-xs-12 padding-0">
                            <label class="control-label lable-txt" for="name">Vendor Code</label>
                        </div>
                         <div class="col-sm-11 col-md-11 col-xs-12 padding-0">
                            <div class="form-group">
                                <%-- <asp:DropDownList ID="DrpVendorCode" DataTextField="VendorCode" AutoPostBack="true" CausesValidation="false" VendorCode="VendorId" runat="server" CssClass="form-control" OnSelectedIndexChanged="DrpVendorCode_SelectedIndexChanged" />--%>
                                <telerik:RadComboBox CausesValidation="false" DataTextField="VendorName" DataValueField="VendorCode" RenderMode="Lightweight" ID="DrpVendorCode" 
                                    runat="server" Height="200" Width="305"  AutoPostBack="true" Filter="Contains" MarkFirstMatch="true"
                                     EnableVirtualScrolling="true"  DataSourceID="EntityDataSource1"
                                    ItemsPerRequest="10" EnableAutomaticLoadOnDemand="true" OnSelectedIndexChanged="DrpVendorCode_SelectedIndexChanged" >
                                </telerik:RadComboBox>

                                <asp:EntityDataSource ID="EntityDataSource1" runat="server" ConnectionString="name=SuzlonBPPEntities"
                                    DefaultContainerName="SuzlonBPPEntities" Select="it.[VendorCode], it.[VendorName]"
                                    AutoPage="true" CommandText="GetVendorCode(@UserID)">
                                 <CommandParameters>
                                    <asp:ControlParameter Name="UserId"  PropertyName="Value"
                                        ControlID="hidSerachUserId" Type="Int32"/>
                                </CommandParameters>
                                </asp:EntityDataSource>

                            </div>
                        </div>
                    </div>
                 </div>

                   
        <!-- New telerik popopu-->

     <%--   <telerik:RadWindow RenderMode="Lightweight" ID="modalPopup" runat="server" Width="360px" Height="365px" Modal="true" OffsetElementID="main" OnClientShow="setCustomPosition" Style="z-index: 100001;">
        <ContentTemplate>
            <!-- Add your Grid from here-->

                <telerik:RadGrid ID="TresuryRequestApproval" runat="server">

                    <mastertableview autogeneratecolumns="False">
                                        <NoRecordsTemplate>
                                            <table width="100%" border="0" cellpadding="20" cellspacing="20">
                                                <tr>
                                                    <td align="center" class="txt-white">No records to display.
                                                    </td>
                                                </tr>
                                            </table>
                                        </NoRecordsTemplate>

                                        <ColumnGroups>
                                            <telerik:GridColumnGroup HeaderText="Utilisation Period" Name="UtilisationPeriod"></telerik:GridColumnGroup>
                                        </ColumnGroups>


                                        <Columns>

                                            <telerik:GridBoundColumn DataField="RequestDate" MaxLength="30" HeaderText="Vendor Name" UniqueName="Date" ColumnEditorID="VendorName" HeaderStyle-Width="15%">
                                                <ColumnValidationSettings EnableRequiredFieldValidation="true" ModelErrorMessage-ForeColor="Red">
                                                    <RequiredFieldValidator ErrorMessage="* Required!" Display="Static" ForeColor="Red"></RequiredFieldValidator>
                                                </ColumnValidationSettings>
                                            </telerik:GridBoundColumn>

                                            <telerik:GridBoundColumn DataField="CompanyCode" MaxLength="30" HeaderText="Vendor Code" UniqueName="CompanyCode" ColumnEditorID="VendorCode" HeaderStyle-Width="15%">
                                                <ColumnValidationSettings EnableRequiredFieldValidation="true" ModelErrorMessage-ForeColor="Red">
                                                    <RequiredFieldValidator ErrorMessage="* Required!" Display="Static" ForeColor="Red"></RequiredFieldValidator>
                                                </ColumnValidationSettings>
                                            </telerik:GridBoundColumn>

                                            <telerik:GridBoundColumn DataField="SubVertical" MaxLength="30" HeaderText="Vendor City" UniqueName="SubVertical" ColumnEditorID="VendorCity" HeaderStyle-Width="15%">
                                                <ColumnValidationSettings EnableRequiredFieldValidation="true" ModelErrorMessage-ForeColor="Red">
                                                    <RequiredFieldValidator ErrorMessage="* Required!" Display="Static" ForeColor="Red"></RequiredFieldValidator>
                                                </ColumnValidationSettings>
                                            </telerik:GridBoundColumn>

                                            
                                        </Columns>



                                    </mastertableview>

                </telerik:RadGrid>

            <!-- End of grid-->
        </ContentTemplate>
    </telerik:RadWindow>--%>

    <telerik:RadCodeBlock runat="server" ID="rdbScripts">
        <script type="text/javascript">
            $(document).ready(function () {

                $("#RadSearchBox1").click(function () {
                    $('#modalPopup').modal('show');
                });

            });
            
           <%-- $modalWindowDemo.modalWindowID = "<%=modalPopup.ClientID %>";--%>
        </script>
    </telerik:RadCodeBlock>

        <!-- end of popup-->
        <div class=" col-xs-12 padding-lr-10">
            <div class="col-xs-12 padding-0" style="background-color: #FFF;">
                <div class="col-sm-12 col-lg-11 padding-0" style="background-color: #FFF;">
                   
                     <div class="col-sm-4 col-md-4 col-xs-12 padding-0">
                         <div class="col-sm-3 col-md-3 col-xs-12 padding-0">
                            <label class="control-label lable-txt" for="name">Company Code</label>
                        </div>
                          <div class="col-sm-9 col-md-9 col-xs-12 padding-0">
                            <%-- <asp:DropDownList ID="DrpCompanyCode" DataTextField="Name" AutoPostBack="true" CausesValidation="false" DataValueField="Id" runat="server" CssClass="form-control" OnSelectedIndexChanged="DrpCompanyCode_SelectedIndexChanged" />--%>
                            <telerik:RadComboBox CausesValidation="false" RenderMode="Lightweight" ID="DrpCompanyCode" runat="server" Height="200" Width="305" AutoPostBack="true"
                            EmptyMessage="Select Company Code"  Filter="Contains"    MarkFirstMatch="true" EnableLoadOnDemand="false" OnSelectedIndexChanged="DrpCompanyCode_SelectedIndexChanged">
                            </telerik:RadComboBox>
                        </div>
                    </div>

                   <div class="col-sm-4 col-md-4 col-xs-12 ">
                         <div class="col-sm-3 col-md-3 col-xs-12 padding-0">
                            <label class="control-label lable-txt" for="name">Vendor Name</label>
                        </div>
                      <div class="col-sm-9 col-md-9 col-xs-12 padding-0">
                            <div class="form-group">
                                <asp:TextBox ID="txtVendorName" runat="server" class="form-control" ReadOnly />
                            </div>
                        </div>
                    </div>

                     <div class="col-sm-4 col-md-4 col-xs-12">
                         <div class="col-sm-4 col-md-4 col-xs-12 padding-0">
                            <label class="control-label lable-txt" for="name">Vendor PAN No.</label>
                        </div>
                        <div class="col-sm-8 col-md-8 col-xs-12 padding-0">
                            <div class="form-group">
                                <asp:TextBox ID="txtPanNo" runat="server" class="form-control" ReadOnly />
                            </div>
                        </div>

                    </div>

                </div>
                <div class="col-sm-12 col-lg-1 col-xs-12 padding-0">
                    <div style="padding-top: 3px;">
                        <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" CssClass="btn btn-primary pull-right" ValidationGroup="Search" />
                    </div>
                </div>
            </div>

        </div>
        <div class="col-xs-12 padding-lr-10" style="padding-top: 5px !important;">
            <div class="panel with-nav-tabs panel-default border-0">
                <asp:Panel ID="pnlData" runat="server">
                    <div class="panel-heading padding-0 nocolor">
                        <div class="row margin-0">
                            <div class="col-md-12" style="padding-left: 0px; padding-right: 5px">
                                <ul class="col-xs-11 nav nav-tabs border-0 ">
                                    <li class="active"><a href="#tab1default" data-toggle="tab" runat="server" id="tabMyRecord"><span runat="server" id="lblMyRecord">Initiated by Me&nbsp;</span><span class="badge badge-important" style="display: inline" id="lblMyRecordCount" runat="server">0</span></a></li>
                                    <li><a href="#tab2default" data-toggle="tab" runat="server" id="tabPending"><span runat="server" id="lblPending">Pending for Approval&nbsp;</span><span class="badge badge-purple " style="display: inline" runat="server" id="lblPendingCount">0</span></a></li>
                                    <li><a href="#tab3default" data-toggle="tab" runat="server" id="tabCBdocPending"><span runat="server" id="Span1">Pending for CB Document Received&nbsp;</span><span class="badge badge-purple " style="display: inline" id="lblDocPendingCount" runat="server">0</span></a></li>
                                     <li><a href="#tab4default" data-toggle="tab" runat="server" id="tabIniitatorDocPending"><span runat="server" id="Span2">Pending for Document&nbsp;</span><span class="badge badge-purple " style="display: inline" id="lblDocSendPendingCount" runat="server">0</span></a></li>
                                </ul>
                                <ul class="col-xs-1 pull-right nav" style="padding: 5px 0px 0px 5px;">
                                    <li>
                                        <asp:Button ID="linkToAdd" class="btn btn-grey" runat="server" CssClass="pull-right btn btn-grey button button-style" OnClick="linkToAdd_Click" Text="Add"></asp:Button></li>
                                </ul>
                            </div>
                        </div>
                    </div>
                    <div class="panel-body overflow-x">
                        <div class="tab-content">
                            <div class="tab-pane fade in active" id="tab1default">
                                <div class="container-fluid">
                                    <telerik:RadGrid RenderMode="Lightweight" ID="grdMyRecords" runat="server" AutoGenerateColumns="false" ClientSettings-EnablePostBackOnRowClick="false"
                                        AllowPaging="true" AllowSorting="true" AllowFilteringByColumn="true" OnNeedDataSource="grdMyRecords_NeedDataSource" OnItemCommand="grdMyRecords_ItemCommand"
                                        OnItemCreated="grdMyRecords_ItemCreated" PageSize="<%# Convert.ToInt32(SuzlonBPP.Models.Constants.CONST_GRID_PAGE_SIZE) %>" OnItemDataBound="grdMyRecords_ItemDataBound">
                                        <GroupingSettings CaseSensitive="false" />
                                        <MasterTableView CommandItemDisplay="None" ClientDataKeyNames="BankDetailId" EnableNoRecordsTemplate="true" AllowFilteringByColumn="false" TableLayout="Fixed">
                                            <HeaderStyle Width="150px" />
                                            <NoRecordsTemplate>
                                                <table width="100%" border="0" cellpadding="20" cellspacing="20">
                                                    <tr>
                                                        <td align="center">No records to display.
                                                        </td>
                                                    </tr>
                                                </table>
                                            </NoRecordsTemplate>
                                            <CommandItemSettings ShowRefreshButton="false" />
                                            <Columns>
                                                <%--<telerik:GridBoundColumn DataField="VendorName" HeaderText="Vendor Name" HeaderStyle-Width="20%" DataType="System.String" />--%>
                                                <telerik:GridTemplateColumn UniqueName="VendorName" HeaderText="Vendor Name"  ColumnGroupName="">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="vendorName" CommandName="Redirect" runat="server" CssClass="linkButtonColor"></asp:LinkButton>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridBoundColumn DataField="CompanyCode" HeaderText="Company Code"  DataType="System.String" />
                                                <telerik:GridBoundColumn DataField="CompanyName" HeaderText="Company Name" DataType="System.String" />

                                                <telerik:GridBoundColumn DataField="BankName" HeaderText="Bank Name"  DataType="System.String" />
                                                <telerik:GridBoundColumn DataField="AccountNumber" HeaderText="Account No."  DataType="System.String" />
                                                <telerik:GridBoundColumn DataField="AccountType" HeaderText="Account Type"  DataType="System.String" />
                                                <telerik:GridBoundColumn DataField="IFSCCode" HeaderText="IFSC Code"  DataType="System.String" />
                                                <telerik:GridBoundColumn DataField="BranchName" HeaderText="Branch"  DataType="System.String" />
                                                <telerik:GridBoundColumn DataField="City" HeaderText="City"  DataType="System.String" Visible="false"/>                                                
                                                <telerik:GridDateTimeColumn   DataFormatString="{0:dd/MM/yyyy hh:mm tt}"  DataField="Modifidate" HeaderText="Assigned Date" EnableTimeIndependentFiltering="true"/>
                                                <telerik:GridTemplateColumn UniqueName="TemplateAttachment" HeaderText="Attachment" >
                                                    <ItemTemplate>
                                                        <asp:HyperLink ID="viewAttachment" runat="server" Text="View" CssClass="btn btn-grey gridHyperlinks"></asp:HyperLink>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn UniqueName="TemplateComment" HeaderText="Comments" >
                                                    <ItemTemplate>
                                                        <asp:HyperLink ID="viewComment" runat="server" Text="View" CssClass="btn btn-grey gridHyperlinks"></asp:HyperLink>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridBoundColumn DataField="WorkFlowStatus" HeaderText="Status"  DataType="System.String" />
                                            </Columns>
                                            <PagerStyle PageSizes="5,10,15" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                                                PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />
                                        </MasterTableView>
                                    </telerik:RadGrid>
                                </div>
                                <!-- End of table container fluid-->
                            </div>
                            <div class="tab-pane fade" id="tab2default">
                                <div class="container-fluid">
                                    <telerik:RadGrid RenderMode="Lightweight" ID="grdPending" runat="server" ClientSettings-EnablePostBackOnRowClick="false" AutoGenerateColumns="false" OnItemDataBound="grdPending_ItemDataBound"
                                        AllowPaging="true" AllowSorting="true" AllowFilteringByColumn="true" OnNeedDataSource="grdPending_NeedDataSource"
                                        OnPreRender="grdPending_PreRender" PageSize="<%# Convert.ToInt32(SuzlonBPP.Models.Constants.CONST_GRID_PAGE_SIZE) %>"
                                        AllowMultiRowEdit="true" OnUpdateCommand="grdPending_UpdateCommand" OnItemCommand="grdPending_ItemCommand">
                                        <GroupingSettings CaseSensitive="false" />
                                        <ValidationSettings CommandsToValidate="Update" />
                                        <MasterTableView CommandItemDisplay="None" DataKeyNames="BankDetailId" EnableViewState="true" AutoGenerateColumns="false"
                                            EnableNoRecordsTemplate="true" EditMode="InPlace" AllowFilteringByColumn="false" TableLayout="Fixed">
                                            <HeaderStyle Width="150px" />
                                            <NoRecordsTemplate>
                                                <table width="100%" border="0" cellpadding="20" cellspacing="20">
                                                    <tr>
                                                        <td align="center">No records to display.
                                                        </td>
                                                    </tr>
                                                </table>
                                            </NoRecordsTemplate>
                                            <CommandItemSettings ShowRefreshButton="false" />
                                            <Columns>
                                                <telerik:GridTemplateColumn UniqueName="VendorName" HeaderText="Vendor Name" ColumnGroupName="">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="vendorName" CommandName="Redirect" runat="server" CssClass="linkButtonColor"></asp:LinkButton>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridBoundColumn DataField="VendorCode" UniqueName="VendorCode" HeaderText="Vendor Code" DataType="System.String" ReadOnly="true" Visible="false" />
                                                <telerik:GridBoundColumn DataField="CompanyCode" UniqueName="CompanyCode" HeaderText="Company Code" DataType="System.String" ReadOnly="true" />                                               
                                                <telerik:GridBoundColumn DataField="CompanyName" HeaderText="Company Name" DataType="System.String"  ReadOnly="true" />

                                                <telerik:GridBoundColumn DataField="BankName" HeaderText="Bank Name"  DataType="System.String" ReadOnly="true" />
                                                <telerik:GridBoundColumn DataField="AccountNumber" HeaderText="Account No."  DataType="System.String" ReadOnly="true" />
                                                <telerik:GridBoundColumn DataField="AccountTypeDesc" HeaderText="Account Type"  DataType="System.String" ReadOnly="true"/>
                                                <telerik:GridBoundColumn DataField="IFSCCode" HeaderText="IFSC Code"  DataType="System.String" ReadOnly="true" />
                                                <telerik:GridBoundColumn DataField="BranchName" HeaderText="Branch"  DataType="System.String" ReadOnly="true" />
                                                <telerik:GridBoundColumn  DataFormatString="{0:dd/MM/yyyy hh:mm tt}"  DataField="ModifiedOn" HeaderText="Assigned Date" ReadOnly="true"/>
                                                <telerik:GridTemplateColumn UniqueName="TemplateAttachment" HeaderText="Attachment" >
                                                    <EditItemTemplate>
                                                        <asp:HyperLink ID="viewAttachment" runat="server" Text="View" CssClass="btn btn-grey gridHyperlinks"></asp:HyperLink>
                                                    </EditItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn UniqueName="TemplateApproval" HeaderText="Approval" HeaderStyle-Width="300px" HeaderStyle-HorizontalAlign="Center">
                                                    <EditItemTemplate>
                                                        <table>
                                                            <tr>
                                                                <td class="text-center">
                                                                    <asp:RadioButton ID="rdbApproved" runat="server" Text="Approved" GroupName="grpStatus" Width="84px" /></td>
                                                                <td class="text-center">
                                                                    <asp:RadioButton ID="rdbReject" runat="server" Text="Reject" GroupName="grpStatus" Width="72px" /></td>
                                                                <td class="text-center">
                                                                    <asp:RadioButton ID="rdbNeedCorrection" runat="server" Text="Need Correction" GroupName="grpStatus" Width="130px" /></td>
                                                            </tr>

                                                        </table>
                                                    </EditItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Assigned To" UniqueName="AssignedTo">
                                                    <EditItemTemplate>
                                                        <telerik:RadDropDownList RenderMode="Lightweight" Width="150px" ID="drpAssignedTo" runat="server" DefaultMessage="Select Assigned To" DropDownHeight="110px">
                                                        </telerik:RadDropDownList>
                                                        <asp:CustomValidator CssClass="hidden" ForeColor="Red" ID="custValidatorAssignedTo" runat="server" ErrorMessage="* Required"
                                                            ControlToValidate="drpAssignedTo" />
                                                    </EditItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn UniqueName="TemplateComment" HeaderText="Comments" >
                                                    <EditItemTemplate>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:TextBox ID="txtComment" runat="server"></asp:TextBox>
                                                                </td>
                                                                <td style="padding-left: 10px;">
                                                                    <asp:HyperLink ID="viewComment" runat="server" Text="View" CssClass="btn btn-grey gridHyperlinks"></asp:HyperLink>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:CustomValidator ForeColor="Red" ID="CustValidatorComment" runat="server" ErrorMessage="* Required"
                                                                        ControlToValidate="txtComment" CssClass="hidden" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </EditItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="EditCommandColumn" HeaderText="Action">
                                                    <ItemStyle CssClass="MyImageButton" VerticalAlign="Middle" HorizontalAlign="Center"></ItemStyle>
                                                </telerik:GridEditCommandColumn>
                                            </Columns>
                                            <PagerStyle PageSizes="5,10,15" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                                                PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />
                                        </MasterTableView>
                                    </telerik:RadGrid>
                                </div>
                                <!-- End of table container fluid-->
                            </div>
                            <div class="tab-pane fade" id="tab3default">
                                <div class="container-fluid">
                                    <telerik:RadGrid RenderMode="Lightweight" ID="grdCBDocPending" runat="server" ClientSettings-EnablePostBackOnRowClick="false" AutoGenerateColumns="false"
                                        AllowPaging="true" AllowSorting="true" AllowFilteringByColumn="true" OnNeedDataSource="grdCBDocPending_NeedDataSource" OnPreRender="grdCBDocPending_PreRender"
                                        OnUpdateCommand="grdCBDocPending_UpdateCommand" OnItemCommand="grdCBDocPending_ItemCommand" OnItemDataBound="grdCBDocPending_ItemDataBound" PageSize="<%# Convert.ToInt32(SuzlonBPP.Models.Constants.CONST_GRID_PAGE_SIZE) %>"
                                        AllowMultiRowEdit="true">
                                        <GroupingSettings CaseSensitive="false" />
                                        <ValidationSettings CommandsToValidate="Update" />
                                        <MasterTableView CommandItemDisplay="None" DataKeyNames="BankDetailId,BankDetailHistoryId" EnableViewState="true" AutoGenerateColumns="false"
                                            EnableNoRecordsTemplate="true" EditMode="InPlace" AllowFilteringByColumn="false" TableLayout="Fixed">
                                            <HeaderStyle Width="150px" />
                                            <NoRecordsTemplate>
                                                <table width="100%" border="0" cellpadding="20" cellspacing="20">
                                                    <tr>
                                                        <td align="center">No records to display.
                                                        </td>
                                                    </tr>
                                                </table>
                                            </NoRecordsTemplate>
                                            <CommandItemSettings ShowRefreshButton="false" />
                                            <Columns>
                                                <%-- <telerik:GridBoundColumn DataField="BankDetailHistoryId" HeaderText="BankDetailHistoryId" UniqueName="BankDetailHistoryId" Visible="false"/>--%>
                                                <%--<telerik:GridBoundColumn DataField="vendorname" HeaderText="Vendor Name" HeaderStyle-Width="15%" DataType="System.String" ReadOnly="true" />--%>
                                                <telerik:GridTemplateColumn UniqueName="VendorName" HeaderText="Vendor Name" >
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="vendorName" CommandName="Redirect" runat="server" CssClass="linkButtonColor"></asp:LinkButton>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridBoundColumn DataField="vendorcode" UniqueName="VendorCode" HeaderText="Vendor Code" DataType="System.String" ReadOnly="true" />
                                                <telerik:GridBoundColumn DataField="companycode" UniqueName="CompanyCode" HeaderText="Company Code" DataType="System.String" ReadOnly="true" />
                                                <telerik:GridBoundColumn DataField="BankName" HeaderText="Bank Name" DataType="System.String" ReadOnly="true" />
                                                <telerik:GridBoundColumn DataField="AccountNumber" HeaderText="Account No."  DataType="System.String" ReadOnly="true" />
                                                <telerik:GridBoundColumn DataField="AccountTypeDesc" HeaderText="Account Type"  DataType="System.String" ReadOnly="true"/>
                                                <telerik:GridBoundColumn DataField="IFSCCode" HeaderText="IFSC Code"  DataType="System.String" ReadOnly="true" />
                                                <telerik:GridBoundColumn DataField="BranchName" HeaderText="Branch"  DataType="System.String" ReadOnly="true" />
                                                <telerik:GridBoundColumn DataFormatString="{0:dd/MM/yyyy hh:mm tt}"  DataField="Modifidate" HeaderText="Assigned Date" ReadOnly="true"/>
                                                <telerik:GridTemplateColumn UniqueName="DocumentReceived" HeaderText="Document Received"  HeaderStyle-HorizontalAlign="Center">
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <EditItemTemplate>
                                                        <asp:CheckBox ID="chkDocReceived" runat="server" Text="" />
                                                        <%--  <asp:CustomValidator ForeColor="Red" ID="CustValidatorchk" runat="server" ErrorMessage="* Required"
                                                    ControlToValidate="chkDocReceived" CssClass="hidden" />    --%>
                                                        <asp:Label runat="server" ID="CustValidatorchk" Text="*Required" ForeColor="Red" CssClass="hidden"></asp:Label>

                                                    </EditItemTemplate>
                                                </telerik:GridTemplateColumn>

                                                <telerik:GridTemplateColumn UniqueName="ReceiveDate" HeaderText="Receive Date" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <EditItemTemplate>
                                                        <telerik:RadDatePicker RenderMode="Lightweight" ID="ReceiveDate" EnableTyping="false" runat="server" DateInput-DateFormat="MM/dd/yyyy">
                                                        </telerik:RadDatePicker>
                                                        <asp:CustomValidator ForeColor="Red" ID="CustValidatorRecDoc" runat="server" ErrorMessage="* Required"
                                                            ControlToValidate="ReceiveDate" CssClass="hidden" />
                                                    </EditItemTemplate>
                                                </telerik:GridTemplateColumn>


                                                <telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="EditCommandColumn" HeaderText="Action" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemStyle CssClass="MyImageButton" VerticalAlign="Middle" HorizontalAlign="Center"></ItemStyle>
                                                </telerik:GridEditCommandColumn>
                                            </Columns>
                                            <PagerStyle PageSizes="5,10,15" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                                                PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />
                                        </MasterTableView>
                                    </telerik:RadGrid>
                                </div>
                                <!-- End of table container fluid-->
                            </div>

                                <div class="tab-pane fade" id="tab4default">
                                <div class="container-fluid">
                                    <telerik:RadGrid RenderMode="Lightweight" ID="grdInitiatorPendingDoc" runat="server" ClientSettings-EnablePostBackOnRowClick="false" AutoGenerateColumns="false"
                                        AllowPaging="true" AllowSorting="true" AllowFilteringByColumn="true" OnNeedDataSource="grdInitiatorPendingDoc_NeedDataSource" OnPreRender="grdInitiatorPendingDoc_PreRender"
                                        OnUpdateCommand="grdInitiatorPendingDoc_UpdateCommand" OnItemCommand="grdInitiatorPendingDoc_ItemCommand" OnItemDataBound="grdInitiatorPendingDoc_ItemDataBound" PageSize="<%# Convert.ToInt32(SuzlonBPP.Models.Constants.CONST_GRID_PAGE_SIZE) %>"
                                        AllowMultiRowEdit="true">
                                        <GroupingSettings CaseSensitive="false" />
                                        <ValidationSettings CommandsToValidate="Update" />
                                        <MasterTableView CommandItemDisplay="None" DataKeyNames="BankDetailId,BankDetailHistoryId" EnableViewState="true" AutoGenerateColumns="false"
                                            EnableNoRecordsTemplate="true" EditMode="InPlace" AllowFilteringByColumn="false" TableLayout="Fixed">
                                            <HeaderStyle Width="150px" />
                                            <NoRecordsTemplate>
                                                <table width="100%" border="0" cellpadding="20" cellspacing="20">
                                                    <tr>
                                                        <td align="center">No records to display.
                                                        </td>
                                                    </tr>
                                                </table>
                                            </NoRecordsTemplate>
                                            <CommandItemSettings ShowRefreshButton="false" />
                                            <Columns>
                                                <%-- <telerik:GridBoundColumn DataField="BankDetailHistoryId" HeaderText="BankDetailHistoryId" UniqueName="BankDetailHistoryId" Visible="false"/>--%>
                                                <%--<telerik:GridBoundColumn DataField="vendorname" HeaderText="Vendor Name" HeaderStyle-Width="15%" DataType="System.String" ReadOnly="true" />--%>
                                                <telerik:GridTemplateColumn UniqueName="VendorName" HeaderText="Vendor Name">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="vendorName" CommandName="Redirect" runat="server" CssClass="linkButtonColor"></asp:LinkButton>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridBoundColumn DataField="vendorcode" UniqueName="VendorCode" HeaderText="Vendor Code" DataType="System.String" ReadOnly="true" />
                                                <telerik:GridBoundColumn DataField="companycode" UniqueName="CompanyCode" HeaderText="Company Code" DataType="System.String" ReadOnly="true" />
                                                <telerik:GridBoundColumn DataField="BankName" HeaderText="Bank Name"  DataType="System.String" ReadOnly="true" />
                                                <telerik:GridBoundColumn DataField="AccountNumber" HeaderText="Account No."  DataType="System.String" ReadOnly="true" />
                                                <telerik:GridBoundColumn DataField="IFSCCode" HeaderText="IFSC Code"  DataType="System.String" ReadOnly="true" />
                                                <telerik:GridBoundColumn DataField="BranchName" HeaderText="Branch"  DataType="System.String" ReadOnly="true" />
                                                <%-- <telerik:GridCheckBoxColumn HeaderText="Document Received" UniqueName="chkDocReceived"></telerik:GridCheckBoxColumn>   --%>

                                                <telerik:GridTemplateColumn UniqueName="DocumentSend" HeaderText="Document Send"  HeaderStyle-HorizontalAlign="Center">
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <EditItemTemplate>
                                                        <asp:CheckBox ID="chkDocsend" runat="server" Text="" />                                            
                                                        <asp:Label runat="server" ID="CustValidatorchkInitiator" Text="*Required" ForeColor="Red" CssClass="hidden"></asp:Label>

                                                    </EditItemTemplate>
                                                </telerik:GridTemplateColumn>

                                                <telerik:GridTemplateColumn UniqueName="ReceiveDate" HeaderText="Send Date" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <EditItemTemplate>
                                                        <telerik:RadDatePicker RenderMode="Lightweight" ID="SendDate" EnableTyping="false" runat="server" DateInput-DateFormat="MM/dd/yyyy">
                                                        </telerik:RadDatePicker>
                                                        <asp:CustomValidator ForeColor="Red" ID="CustValidatorSentDoc" runat="server" ErrorMessage="* Required"
                                                            ControlToValidate="SendDate" CssClass="hidden" />
                                                    </EditItemTemplate>
                                                </telerik:GridTemplateColumn>


                                                <telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="EditCommandColumn" HeaderText="Action" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemStyle CssClass="MyImageButton" VerticalAlign="Middle" HorizontalAlign="Center"></ItemStyle>
                                                </telerik:GridEditCommandColumn>
                                            </Columns>
                                            <PagerStyle PageSizes="5,10,15" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                                                PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />
                                        </MasterTableView>
                                    </telerik:RadGrid>
                                </div>
                                <!-- End of table container fluid-->
                            </div>

                        </div>
                    </div>
                </asp:Panel>
            </div>
        </div>
        <asp:HiddenField runat="server" ID="hidSerachUserId" />
        <!-- End of Bank Details Grid-->
    </div>
    <telerik:RadNotification RenderMode="Lightweight" Position="Center" runat="server" ID="radMessage" VisibleOnPageLoad="false"
        Width="350px" AutoCloseDelay="0" TitleIcon="none" ContentIcon="none">
    </telerik:RadNotification>
</asp:Content>
