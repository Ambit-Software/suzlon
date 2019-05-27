<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="SuzlonBPP.Master" CodeBehind="BankWorkFlow.aspx.cs" Inherits="WorkFlow" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">

        function getVerticalNames() {
            var combo = $find('<%= ComboSubVertical.ClientID %>');
            var items = combo.get_checkedItems();
            var verticalNames = "";
            for (count = 0; count < items.length; count++) {
                var values = items[count].get_value().split("SOLAR_BPP");
                if (verticalNames == "")
                    verticalNames = values[1];
                else
                    verticalNames = verticalNames + "," + values[1];
            }
            $('#ContentPlaceHolder1_txtVerticals').val(verticalNames);
            console.log(verticalNames);
        }

        function expandDiv(e) {
            var elem = $(e.parentElement.parentElement.parentElement.nextElementSibling);
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
            alert("Saved Successfully.");
        }

        function Failure(error) {
            alert("Error occured while saving.");
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

        function validateDateValues(fromDate, toDate, dpFromWrapper, dpToWrapper, isvalid, isNewRecord) {
            var currentDate = new Date().setHours(0, 0, 0, 0);
            if (fromDate == "" || convertDate(fromDate) == "undefined") {
                isvalid = false;
                dpFromWrapper.next().css("display", "inline");
            }
            else if (isNewRecord && currentDate > Date.parse(convertDate(fromDate))) {
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
                isvalid = validateDateValues(fromDate, toDate, dpFromWrapper, dpToWrapper, isvalid, isNewRecord);
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

                isvalid = validateDateValues(fromDate, toDate, dpFromWrapper, dpToWrapper, isvalid, isNewRecord);
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
                isvalid = validateDateValues(fromDate, toDate, dpFromWrapper, dpToWrapper, isvalid, isNewRecord);
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
                    isvalid = validateDateValues(fromDate, toDate, dpFromWrapper, dpToWrapper, isvalid, isNewRecord);
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
                    isvalid = validateDateValues(fromDate, toDate, dpFromWrapper, dpToWrapper, isvalid, isNewRecord);
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
                    isvalid = validateDateValues(fromDate, toDate, dpFromWrapper, dpToWrapper, isvalid, isNewRecord);
                }
            }
            return isvalid;
        }
    </script>
    <style type="text/css">
        .display-none {
            visibility: hidden;
            display: none;
        }

        .display-inline {
            visibility: visible;
            display: inline;
        }
    </style>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server">
        <ProgressTemplate>
            Progress...
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel runat="server" ID="updatePanel" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="container-fluid padding-0">
                <div class="row margin-0">
                    <div class="col-xs-12 background-strip txt-white">
                        <h4>Workflow Configuration</h4>
                    </div>
                </div>
                <div class="col-xs-12 margin-10">
                    <div class="col-sm-4 col-xs-12">
                        <div class="col-sm-3 col-xs-12 padding-0 margin-10">
                            <label class="col-xs-12 control-label lable-txt" for="name">Workflow</label>
                        </div>
                        <!-- End of col-lg-1-->
                        <div class=" col-sm-9 col-xs-12 margin-10">
                            <div class="form-group">
                                <asp:DropDownList ID="DrpWorkflow" DataTextField="Name" AutoPostBack="false" CausesValidation="false" DataValueField="Id" runat="server" CssClass="form-control" />
                                <asp:RequiredFieldValidator ErrorMessage="* Required" Display="Dynamic" ForeColor="Red" runat="server" ControlToValidate="DrpWorkflow" />
                            </div>
                        </div>
                        <!-- End of input-group-col-xs-8-->
                    </div>
                    <div class="col-sm-4 col-xs-12">
                        <div class="col-sm-4 col-xs-12 padding-0 margin-10">
                            <label class="col-xs-12 control-label lable-txt" for="name">Sub Vertical</label>
                        </div>
                        <!-- End of col-lg-1-->
                        <div class=" col-xs-8 margin-10">
                            <div class="form-group">
                                <telerik:RadComboBox OnClientDropDownClosing="getVerticalNames" CausesValidation="false" AutoPostBack="false" RenderMode="Lightweight" DataTextField="Name" DataValueField="Id"
                                    ID="ComboSubVertical" runat="server" CheckBoxes="true" EnableCheckAllItemsCheckBox="true"
                                    CssClass="form-control col-md-12" />
                                <asp:RequiredFieldValidator ErrorMessage="* Required" Display="Dynamic" ForeColor="Red" runat="server" ControlToValidate="ComboSubVertical" />
                            </div>
                        </div>
                        <!-- End of col-lg-1-->
                    </div>

                    <div class="col-sm-4 col-xs-12">
                        <div class="col-sm-3 col-xs-12 padding-0 margin-10">
                            <label class="col-xs-12 control-label lable-txt" for="name">Vertical</label>
                        </div>
                        <!-- End of col-lg-1-->
                        <div class=" col-xs-9 margin-10">
                            <asp:TextBox ID="txtVerticals" runat="server" CssClass="search-query form-control" disabled />
                        </div>

                        <!-- End of col-lg-1-->
                    </div>
                    <div class="row" style="float: right">
                        <div class="col-md-12">
                            <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-primary button button-style" CausesValidation="false" Text="Search" OnClientClick="javascript:return validateSearchCriteria();" OnClick="btnSearch_Click" />
                        </div>
                    </div>
                </div>
            </div>
            <asp:Repeater ID="Repeater1" EnableViewState="false" ViewStateMode="Disabled" runat="server" OnItemDataBound="Repeater1_ItemDataBound">
                <ItemTemplate>
                </ItemTemplate>
            </asp:Repeater>
            <asp:HiddenField ID="hidWorkflowType" runat="server" Value="0" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
