<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="SuzlonBPP.Master" CodeBehind="WorkflowConfig.aspx.cs" Inherits="SuzlonBPP.WorkflowConfig" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="btnAuditTrail">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdWorkFlowAuditTrial" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="grdWorkFlowAuditTrial">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdWorkFlowAuditTrial" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="LoadingPanel1" runat="server" Skin="Default">
    </telerik:RadAjaxLoadingPanel>

    <script src="Scripts/bootstrap.min.js"></script>
    <telerik:RadScriptBlock runat="server">
        <script type="text/javascript">

            function ShowAuditTrail() {
                $('#squarespaceModal').modal('show');
            }

            $(document).ready(function () {
                $('#loading').hide();
            });
            function getVerticalNames() {
                var combo = $find('<%= ComboSubVertical.ClientID %>');
                var items = combo.get_checkedItems();
                var verticalNames = "";
                var verticalNameArray = [];
                for (count = 0; count < items.length; count++) {
                    var values = items[count].get_value().split("SOLAR_BPP");
                    if ($.inArray(values[1], verticalNameArray) <= -1) {
                        verticalNameArray.push(values[1]);
                    }
                }
                for (count = 0; count < verticalNameArray.length; count++) {
                    if (verticalNames == "")
                        verticalNames = verticalNameArray[count];
                    else
                        verticalNames = verticalNames + ", " + verticalNameArray[count];
                }
                $('#ContentPlaceHolder1_txtVerticals').val(verticalNames);
                console.log(verticalNames);
            }

            function expandDiv(e) {
                var elem = $(e.nextElementSibling);
                if (elem.hasClass("collapse")) {
                    elem.addClass("in");
                    elem.removeClass("collapse");
                }
                else {
                    elem.addClass("collapse");
                    elem.removeClass("in");
                }
            };

            function Success(result) {
                $("#loading").css("display", "none");
                if (result != "Refresh Page") {
                    var radNotification1 = $find("<%= radMessage.ClientID %>");
                    radNotification1.set_title("Message");
                    radNotification1.set_text("Saved Successfully");
                    radNotification1.show();
                }
                else
                    window.location.reload();
            }

            function Failure(error) {
                $("#loading").css("display", "none");
                var radNotification1 = $find("<%= radMessage.ClientID %>");
                radNotification1.set_title("Message");
                radNotification1.set_text("Error occured while saving");
                radNotification1.show();
            }

            function clickLnkButton() {
                $("#ContentPlaceHolder1_lnkButton")[0].click();
                return true;
            }

            function clearDateValidation(dpFromWrapper, dpToWrapper) {
                dpFromWrapper.next().css("display", "none");
                $(dpFromWrapper.next().next()[0]).css("display", "none");
                dpToWrapper.next().css("display", "none");
                dpToWrapper.next().next().addClass("display-none").removeClass("display-inline");
            }

            function validateDateValues(fromDate, toDate, dpFromWrapper, dpToWrapper, isvalid, isNewRecord, createdDate) {
                var currentDate = new Date().setHours(0, 0, 0, 0);
                if (fromDate == "" || convertDate(fromDate) == "undefined") {
                    isvalid = false;
                    dpFromWrapper.next().css("display", "inline");

                }
                else if (isNewRecord && currentDate > Date.parse(convertDate(fromDate))) {
                    $(dpFromWrapper.next().next()).text("Should be greater than equal to Current Date");
                    isvalid = false;
                    $(dpFromWrapper.next().next()[0]).css("display", "inline");
                }
                else if (!isNewRecord && Date.parse(convertDate(createdDate)) > Date.parse(convertDate(fromDate))) {
                    $(dpFromWrapper.next().next()).text("Should be greater than equal to Created Date '" + createdDate + "'");
                    isvalid = false;
                    $(dpFromWrapper.next().next()[0]).css("display", "inline");
                }

                if (toDate == "" || convertDate(toDate) == "undefined") {
                    isvalid = false;
                    dpToWrapper.next().css("display", "inline");
                }

                if (fromDate != "" && toDate != "" && Date.parse(convertDate(fromDate)) > Date.parse(convertDate(toDate))) {
                    isvalid = false;
                    dpToWrapper.next().next().removeClass("display-none").addClass("display-inline")
                }
                return isvalid;
            }

            function validateDpValues(dpPri, dpSec, isvalid) {
                if (dpPri.val().trim() == "") {
                    isvalid = false;
                    dpPri.next().css("display", "inline");
                }
                else
                    dpPri.next().css("display", "none");

                if (dpPri.val().trim() != "" && dpSec.val().trim() != "" && dpPri.val().trim() == dpSec.val().trim()) {
                    isvalid = false;
                    dpSec.next().removeClass("display-none").addClass("display-inline");
                }
                else
                    dpSec.next().addClass("display-none").removeClass("display-inline");

                return isvalid;
            }

            function validateSearchCriteria() {
                var isValid = true;
                var drpWorkflow = $("#ContentPlaceHolder1_DrpWorkflow");
                if (drpWorkflow.val().trim() == "") {
                    isValid = false;
                    drpWorkflow.next().css("display", "inline");
                }
                else
                    drpWorkflow.next().css("display", "none");

                var comboSubVertical = $("#ctl00_ContentPlaceHolder1_ComboSubVertical");
                if (comboSubVertical.val().trim() == "") {
                    isValid = false;
                    comboSubVertical.next().css("display", "inline");
                }
                else
                    comboSubVertical.next().css("display", "none");
                return isValid;
            }

            function SaveBankDetails(e) {
                if (validateBankControls(e)) {
                    $("#loading").css("display", "block");
                    var id = e.id.replace('lblSave', '');
                    var workflowType = $("#ContentPlaceHolder1_hidWorkflowType").val();
                    var myObject = new Object();
                    myObject.WorkFlowId = $($("#" + id + "hidWorkflowId")[0]).val();
                    if (myObject.WorkFlowId == "")
                        myObject.WorkFlowId = "0";
                    myObject.SubVerticalId = $($("#" + id + "hidSubVerticalId")[0]).val();
                    myObject.VerticalId = $($("#" + id + "hidVerticalId")[0]).val();

                    myObject.PriVerContUserId = $($("#" + id + "DrpPriVerCont")[0]).val();
                    if ($($("#" + id + "DrpSecVerCont")[0]).val().trim() != "") {
                        myObject.SecVerContUserId = $($("#" + id + "DrpSecVerCont")[0]).val();
                        myObject.SecVerContFromDt = convertDate($("#ctl00_" + id + "DpFromVerCont_dateInput").val());
                        myObject.SecVerContToDt = convertDate($("#ctl00_" + id + "DpToVerCont_dateInput").val());
                    }

                    myObject.PriTreasuryUserId = $($("#" + id + "DrpPriTreasury")[0]).val();
                    if ($($("#" + id + "DrpSecTreasury")[0]).val().trim() != "") {
                        myObject.SecTreasuryUserId = $($("#" + id + "DrpSecTreasury")[0]).val();
                        myObject.SecTreasuryFromDt = convertDate($("#ctl00_" + id + "DpFromTreasury_dateInput").val());
                        myObject.SecTreasuryToDt = convertDate($("#ctl00_" + id + "DpToTreasury_dateInput").val());
                    }

                    myObject.PriCBUserId = $($("#" + id + "DrpPriCB")[0]).val();
                    if ($($("#" + id + "DrpSecCB")[0]).val().trim() != "") {
                        myObject.SecCBUserId = $($("#" + id + "DrpSecCB")[0]).val();
                        myObject.SecCBFromDt = convertDate($("#ctl00_" + id + "DpFromCB_dateInput").val());
                        myObject.SecCBToDt = convertDate($("#ctl00_" + id + "DpToCB_dateInput").val());
                    }

                    if (workflowType != "2") {

                        myObject.PriGrpContUserId = $($("#" + id + "DrpPriGrpCont")[0]).val();
                        if ($($("#" + id + "DrpSecGrpCont")[0]).val().trim() != "") {
                            myObject.SecGrpContUserId = $($("#" + id + "DrpSecGrpCont")[0]).val();
                            myObject.SecGrpContFromDt = convertDate($("#ctl00_" + id + "DpFromGrpCont_dateInput").val());
                            myObject.SecGrpContToDt = convertDate($("#ctl00_" + id + "DpToGrpCont_dateInput").val());
                        }

                        myObject.PriMgmtAssUserId = $($("#" + id + "DrpPriMgmtAss")[0]).val();
                        if ($($("#" + id + "DrpSecMgmtAss")[0]).val().trim() != "") {
                            myObject.SecMgmtAssUserId = $($("#" + id + "DrpSecMgmtAss")[0]).val();
                            myObject.SecMgmtAssFromDt = convertDate($("#ctl00_" + id + "DpFromMgmtAss_dateInput").val());
                            myObject.SecMgmtAssToDt = convertDate($("#ctl00_" + id + "DpToMgmtAss_dateInput").val());
                        }

                        myObject.PriFASSCUserId = $($("#" + id + "DrpPriFASSC")[0]).val();
                        if ($($("#" + id + "DrpSecFASSC")[0]).val().trim() != "") {
                            myObject.SecFASSCUserId = $($("#" + id + "DrpSecFASSC")[0]).val();
                            myObject.SecFASSCFromDt = convertDate($("#ctl00_" + id + "DpFromFASCC_dateInput").val());
                            myObject.SecFASSCToDt = convertDate($("#ctl00_" + id + "DpToFASCC_dateInput").val());
                        }
                    }
                    
                    var myString = JSON.stringify(myObject);
                    PageMethods.SaveBankWorkFlow(myString, workflowType, Success, Failure);
                }
            }

            function validateBankControls(e) {
                var id = e.id.replace('lblSave', '');
                var isvalid = true;
                var isNewRecord = ($($("#" + id + "hidWorkflowId")[0]).val() == "" || $($("#" + id + "hidWorkflowId")[0]).val() == "0");
                var createdDate = $($("#" + id + "hidCreatedDate")[0]).val();
                var workflowType = $("#ContentPlaceHolder1_hidWorkflowType").val();
                var dpPri = $($("#" + id + "DrpPriVerCont")[0]);
                var dpSec = $($("#" + id + "DrpSecVerCont")[0]);
                isvalid = validateDpValues(dpPri, dpSec, isvalid);
                var dpFromWrapper = $("#ctl00_" + id + "DpFromVerCont_wrapper");
                var dpToWrapper = $("#ctl00_" + id + "DpToVerCont_wrapper");
                clearDateValidation(dpFromWrapper, dpToWrapper);
                if (dpSec.val().trim() != "") {
                    var fromDate = $("#ctl00_" + id + "DpFromVerCont_dateInput").val();
                    var toDate = $("#ctl00_" + id + "DpToVerCont_dateInput").val();
                    isvalid = validateDateValues(fromDate, toDate, dpFromWrapper, dpToWrapper, isvalid, isNewRecord, createdDate);
                }

                var dpPri = $($("#" + id + "DrpPriTreasury")[0]);
                var dpSec = $($("#" + id + "DrpSecTreasury")[0]);
                isvalid = validateDpValues(dpPri, dpSec, isvalid);
                var dpFromWrapper = $("#ctl00_" + id + "DpFromTreasury_wrapper");
                var dpToWrapper = $("#ctl00_" + id + "DpToTreasury_wrapper");
                clearDateValidation(dpFromWrapper, dpToWrapper);
                if (dpSec.val().trim() != "") {
                    var fromDate = $("#ctl00_" + id + "DpFromTreasury_dateInput").val();
                    var toDate = $("#ctl00_" + id + "DpToTreasury_dateInput").val();

                    isvalid = validateDateValues(fromDate, toDate, dpFromWrapper, dpToWrapper, isvalid, isNewRecord, createdDate);
                }

                var dpPri = $($("#" + id + "DrpPriCB")[0]);
                var dpSec = $($("#" + id + "DrpSecCB")[0]);
                isvalid = validateDpValues(dpPri, dpSec, isvalid);
                var dpFromWrapper = $("#ctl00_" + id + "DpFromCB_wrapper");
                var dpToWrapper = $("#ctl00_" + id + "DpToCB_wrapper");
                clearDateValidation(dpFromWrapper, dpToWrapper);
                if (dpSec.val().trim() != "") {
                    var fromDate = $("#ctl00_" + id + "DpFromCB_dateInput").val();
                    var toDate = $("#ctl00_" + id + "DpToCB_dateInput").val();
                    isvalid = validateDateValues(fromDate, toDate, dpFromWrapper, dpToWrapper, isvalid, isNewRecord, createdDate);
                }

                if (workflowType != "2") {
                    var dpPri = $($("#" + id + "DrpPriGrpCont")[0]);
                    var dpSec = $($("#" + id + "DrpSecGrpCont")[0]);
                    isvalid = validateDpValues(dpPri, dpSec, isvalid);
                    var dpFromWrapper = $("#ctl00_" + id + "DpFromGrpCont_wrapper");
                    var dpToWrapper = $("#ctl00_" + id + "DpToGrpCont_wrapper");
                    clearDateValidation(dpFromWrapper, dpToWrapper);
                    if (dpSec.val().trim() != "") {
                        var fromDate = $("#ctl00_" + id + "DpFromGrpCont_dateInput").val();
                        var toDate = $("#ctl00_" + id + "DpToGrpCont_dateInput").val();
                        isvalid = validateDateValues(fromDate, toDate, dpFromWrapper, dpToWrapper, isvalid, isNewRecord, createdDate);
                    }

                    var dpPri = $($("#" + id + "DrpPriMgmtAss")[0]);
                    var dpSec = $($("#" + id + "DrpSecMgmtAss")[0]);
                    isvalid = validateDpValues(dpPri, dpSec, isvalid);
                    var dpFromWrapper = $("#ctl00_" + id + "DpFromMgmtAss_wrapper");
                    var dpToWrapper = $("#ctl00_" + id + "DpToMgmtAss_wrapper");
                    clearDateValidation(dpFromWrapper, dpToWrapper);
                    if (dpSec.val().trim() != "") {
                        var fromDate = $("#ctl00_" + id + "DpFromMgmtAss_dateInput").val();
                        var toDate = $("#ctl00_" + id + "DpToMgmtAss_dateInput").val();
                        isvalid = validateDateValues(fromDate, toDate, dpFromWrapper, dpToWrapper, isvalid, isNewRecord, createdDate);
                    }

                    var dpPri = $($("#" + id + "DrpPriFASSC")[0]);
                    var dpSec = $($("#" + id + "DrpSecFASSC")[0]);
                    isvalid = validateDpValues(dpPri, dpSec, isvalid);
                    var dpFromWrapper = $("#ctl00_" + id + "DpFromFASCC_wrapper");
                    var dpToWrapper = $("#ctl00_" + id + "DpToFASCC_wrapper");
                    clearDateValidation(dpFromWrapper, dpToWrapper);
                    if (dpSec.val().trim() != "") {
                        var fromDate = $("#ctl00_" + id + "DpFromFASCC_dateInput").val();
                        var toDate = $("#ctl00_" + id + "DpToFASCC_dateInput").val();
                        isvalid = validateDateValues(fromDate, toDate, dpFromWrapper, dpToWrapper, isvalid, isNewRecord, createdDate);
                    }
                }                
                return isvalid;
            }
        </script>
    </telerik:RadScriptBlock>
    <style type="text/css">
        .display-none {
            visibility: hidden;
            display: none;
        }

        .display-inline {
            visibility: visible;
            display: inline;
        }

        #loading {
            width: 100%;
            height: 100%;
            top: 0;
            left: 0;
            position: fixed;
            display: block;
            opacity: 0.7;
            z-index: 9999;
            background: url('Content/images/loading.gif') 50% 50% no-repeat rgb(249,249,249);
            text-align: center;
        }
    </style>
    <asp:UpdatePanel runat="server" ID="updatePanel" UpdateMode="Conditional">
        <ContentTemplate>
            <div style="padding: 10px;">
                <div class="container-fluid padding-0">
                    <div class="row margin-0">
                        <div class="col-xs-6 heading-big">
                            <h5 class="margin-0 lineheight-42 breath-ctrl">Home / <span>Workflow Configuration</span></h5>
                        </div>
                        <div class="col-sm-6 col-xs-6 ">
                        </div>
                    </div>
                    <div class="clearfix"></div>
                    <div class="row margin-0" style="background-color: #FFF">
                        <div class="col-xs-12 margin-t-10 padding-0">
                            <div class="col-sm-4 col-xs-12 padding-0">
                                <div class="col-sm-3 col-xs-12 padding-0">
                                    <label class="col-xs-12 control-label lable-txt" for="name">Workflow</label>
                                </div>
                                <!-- End of col-lg-1-->
                                <div class=" col-sm-9 col-xs-12 ">
                                    <div class="form-group">

                                        <asp:DropDownList ID="DrpWorkflow" DataTextField="Name" AutoPostBack="false" CausesValidation="false" DataValueField="Id" runat="server" CssClass="form-control" />
                                        <asp:RequiredFieldValidator ErrorMessage="* Required" Display="Dynamic" ForeColor="Red" runat="server" ControlToValidate="DrpWorkflow" />
                                    </div>
                                </div>
                                <!-- End of input-group-col-xs-8-->
                            </div>
                            <div class="col-sm-4 col-xs-12 padding-0 ">
                                <div class="col-sm-4 col-xs-12 padding-0 ">
                                    <label class="col-xs-12 control-label lable-txt" for="name">Sub Vertical</label>
                                </div>
                                <!-- End of col-lg-1-->
                                <div class=" col-xs-8 ">
                                    <div class="form-group">
                                        <telerik:RadComboBox OnClientDropDownClosing="getVerticalNames" CausesValidation="false" AutoPostBack="false" RenderMode="Lightweight" DataTextField="Name" DataValueField="Id"
                                            ID="ComboSubVertical" runat="server" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" />
                                        <asp:RequiredFieldValidator ErrorMessage="* Required" Display="Dynamic" ForeColor="Red" runat="server" ControlToValidate="ComboSubVertical" />
                                    </div>
                                </div>
                                <!-- End of col-lg-1-->
                            </div>

                            <div class="col-sm-3 col-xs-12 padding-0 ">
                                <div class="col-sm-3 col-xs-12 padding-0 ">
                                    <label class="col-xs-12 control-label lable-txt" for="name">Vertical</label>
                                </div>
                                <!-- End of col-lg-1-->
                                <div class=" col-xs-9 ">
                                    <asp:TextBox ID="txtVerticals" runat="server" CssClass="search-query form-control" disabled />
                                </div>

                                <!-- End of col-lg-1-->
                            </div>
                            <div class="col-sm-1 col-xs-12 padding-0 ">
                                <center>
                                     <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-primary button button-style " CausesValidation="false" Text="Search" OnClientClick="javascript:return validateSearchCriteria();" OnClick="btnSearch_Click" />
                                </center>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <asp:UpdatePanel runat="server" ID="updatePanel1">
                <ContentTemplate>
                    <!-- Start of Modals------>

                    <!-- End of Modals-------->
                    <div class="col-xs-12" style="padding-bottom: 5px;">
                        <asp:Button ID="btnAuditTrail" OnClick="btnAuditTrail_Click" runat="server" CssClass="btn btn-primary button-style margin-0 submit-btn" Text="Audit Trail" />
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnAuditTrail" />
                    <asp:AsyncPostBackTrigger ControlID="grdWorkFlowAuditTrial" />
                </Triggers>
            </asp:UpdatePanel>
            <div id="loading" style="display: none;">
            </div>
            <asp:Repeater ID="Repeater1" EnableViewState="false" ViewStateMode="Disabled" runat="server" OnItemDataBound="Repeater1_ItemDataBound">
                <ItemTemplate>
                </ItemTemplate>
            </asp:Repeater>
            <asp:HiddenField ID="hidWorkflowType" runat="server" Value="0" />

        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdateProgress runat="server">
        <ProgressTemplate>
            <div class="loading" align="center">
                Please wait...<br />
                <br />
                <img height="50" style="width: 50px; height: 50px;" width="50" src="<%=ConfigurationManager.AppSettings["WebsiteUrl"].ToString()%>/Content/images/loading.gif" alt="" />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <div class="modal fade" id="squarespaceModal" tabindex="-1" role="dialog" aria-labelledby="modalLabel" aria-hidden="true">
        <div class="modal-dialog1">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">×</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title subheading margin-0" id="lineModalLabel">Audit Trail</h4>
                </div>
                <div class="modal-body" style="overflow: hidden; overflow-x: scroll; overflow-y: scroll; max-height: 400px !important;">

                    <telerik:RadGrid RenderMode="Lightweight" ID="grdWorkFlowAuditTrial" runat="server" OnNeedDataSource="grdWorkFlowAuditTrial_NeedDataSource"
                        AutoGenerateColumns="false" AllowPaging="true" AllowSorting="true" AllowFilteringByColumn="false" PageSize="<%# Convert.ToInt32(SuzlonBPP.Models.Constants.CONST_GRID_PAGE_SIZE) %>">
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
                                <telerik:GridBoundColumn DataField="SubVertical" HeaderText="Sub Vertical" UniqueName="subVertical">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Stage" HeaderText="Stage" UniqueName="Stage">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="ModifiedDate" HeaderText="Last Modified Date" UniqueName="LastModifiedDate">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="ModifiedTime" HeaderText="Last Modified Time" UniqueName="LastModifiedDateTime">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="ModifyBy" HeaderText="ModifiedBy" UniqueName="ModifiedBy">
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
        </div>
        <telerik:RadNotification RenderMode="Lightweight" Position="Center" runat="server" ID="radMessage" VisibleOnPageLoad="false"
            Width="350px" TitleIcon="none" ContentIcon="none" EnableShadow="true" AnimationDuration="1000">
        </telerik:RadNotification>
    </div>
</asp:Content>
