<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="SuzlonBPP.Master" CodeBehind="BankValidator.aspx.cs" Inherits="SuzlonBPP.BankValidator" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        .RadDropDownList_Default .rddlFocused{
            width:150px !important;
        }
        .RadDropDownList_Default .rddlInner {
            width:150px !important;
        }

        .linkButtonColor {
            color: #337ab7 !important;
        }
    </style>
    <div class="row margin-0">
        <div class="col-xs-11 heading-big">
            <h5 class="margin-0 lineheight-42 breath-ctrl">Home / <span>Vendor Bank Details(Validator)</span></h5>
        </div>
        <div class="col-xs-1" style="padding-top:5px">
            <asp:Button class="btn btn-grey" runat="server" CssClass="pull-right btn btn-grey button button-style" OnClick="linkToAdd_Click" Text="Add"></asp:Button>
        </div>
    </div>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" OnAjaxRequest="RadAjaxManager1_AjaxRequest">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadAjaxManager1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdComment" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="radMessage" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
             <telerik:AjaxSetting AjaxControlID="grdValidator">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdValidator" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="radMessage" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
     <telerik:RadAjaxLoadingPanel ID="LoadingPanel1" runat="server" Skin="Default"></telerik:RadAjaxLoadingPanel>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript">
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

            function ShowComments(id, rowIndex) {
                var ajaxManager = $find("<%= RadAjaxManager1.ClientID %>");
                ajaxManager.ajaxRequest("Comment#" + id + "");
                $('#divComments').modal();
            }

            function Validate(drpStatus, custValidatorStatus, drpSubVertical, custValidatorSubVertical, comment, custValidatorComment) {
                var isValid = true;
                var statusValue = $($("#" + drpStatus)[0]).val().trim();
                if (statusValue == "" || statusValue=="Select Status") {
                    $($("#" + custValidatorStatus)[0]).removeClass("hidden");
                    $($("#" + custValidatorStatus)[0]).addClass("visible");
                    isValid = false;
                }
                else {
                    $($("#" + custValidatorStatus)[0]).addClass("hidden");
                    $($("#" + custValidatorStatus)[0]).removeClass("visible");
                }

                if (statusValue == "Approved" && ($($("#" + drpSubVertical)[0]).val().trim() == "" || $($("#" + drpSubVertical)[0]).val().trim() == "Select Sub Vertical")) {
                    $($("#" + custValidatorSubVertical)[0]).removeClass("hidden");
                    $($("#" + custValidatorSubVertical)[0]).addClass("visible");
                    isValid = false;
                }
                else {
                    $($("#" + custValidatorSubVertical)[0]).addClass("hidden");
                    $($("#" + custValidatorSubVertical)[0]).removeClass("visible");
                }

                if ($($("#" + comment)[0]).val().trim() == "") {
                    $($("#" + custValidatorComment)[0]).removeClass("hidden");
                    $($("#" + custValidatorComment)[0]).addClass("visible");
                    isValid = false;
                }
                else {
                    $($("#" + custValidatorComment)[0]).addClass("hidden");
                    $($("#" + custValidatorComment)[0]).removeClass("visible");
                }
                return isValid;
            }
        </script>
    </telerik:RadCodeBlock>
    <style>
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

        .MyImageButton{
            border:none !important;
        }
        .MyImageButton a{
            border:none !important;
        }
        
    </style>
    <script src="Scripts/bootstrap.min.js"></script>
    <div class="modal fade" id="divComments" tabindex="-1" role="dialog" aria-labelledby="modalLabel" aria-hidden="true">
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
                                        <td align="center" class="txt-white">No records to display.
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
    <div class="modal fade" id="divAttachment" tabindex="-1" role="dialog" aria-labelledby="modalLabel" aria-hidden="true">
        <div class="modal-dialog1">
            <div class="modal-content">
                <div class="modal-header modal-header-resize">
                    <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">×</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title text-primary" id="lineModalLabel">Attchments</h4>
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
    <div class="row margin-0 overflow-h padding-lr-10">
        <div id="grid " class="col-md-12 padding-0" style="overflow: hidden; overflow-x: scroll;">
            <telerik:RadGrid RenderMode="Lightweight" ID="grdValidator" runat="server" AutoGenerateColumns="false" ClientSettings-EnablePostBackOnRowClick="false" OnItemDataBound="grdValidator_ItemDataBound"
                AllowPaging="true" AllowSorting="true" AllowFilteringByColumn="true" OnNeedDataSource="grdValidator_NeedDataSource"
                OnPreRender="grdValidator_PreRender" PageSize="<%# Convert.ToInt32(SuzlonBPP.Models.Constants.CONST_GRID_PAGE_SIZE) %>"
                AllowMultiRowEdit="true" OnUpdateCommand="grdValidator_UpdateCommand" OnItemCommand="grdValidator_ItemCommand">
                <GroupingSettings CaseSensitive="false" />
                <ValidationSettings CommandsToValidate="Update" />
                <MasterTableView CommandItemDisplay="None" DataKeyNames="BankDetailId" EnableViewState="true" AutoGenerateColumns="false"
                    EnableNoRecordsTemplate="true" EditMode="InPlace" AllowFilteringByColumn="false">
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
                        <%--<telerik:GridBoundColumn DataField="VendorName" HeaderText="Vendor Name"  DataType="System.String" ReadOnly="true" />--%>
                         <telerik:GridTemplateColumn UniqueName="VendorName" HeaderText="Vendor Name" HeaderStyle-Width="20%" ColumnGroupName="">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="vendorName" CommandName="Redirect" runat="server" CssClass="linkButtonColor"></asp:LinkButton>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn DataField="VendorCode" UniqueName="VendorCode" HeaderText="Vendor Code"  DataType="System.String" ReadOnly="true" />
                        <telerik:GridBoundColumn DataField="CompanyCode" UniqueName="CompanyCode" HeaderText="Company Code"  DataType="System.String" ReadOnly="true" />
                        <telerik:GridBoundColumn DataField="BankName" HeaderText="Bank Name"  DataType="System.String" ReadOnly="true" />
                        <telerik:GridBoundColumn DataField="BranchName" HeaderText="Branch"  DataType="System.String" ReadOnly="true" />
                        <telerik:GridBoundColumn DataField="IFSCCode" HeaderText="IFSC Code"  DataType="System.String" ReadOnly="true" />
                        <telerik:GridBoundColumn DataField="AccountNumber" HeaderText="Account No." DataType="System.String" ReadOnly="true" />
                        <telerik:GridBoundColumn DataField="AccountType" HeaderText="Account Type"  DataType="System.String" ReadOnly="true"/>
                        <telerik:GridBoundColumn DataFormatString="{0:dd/MM/yyyy hh:mm tt}"  DataField="Modifidate" HeaderText="Assigned Date" ReadOnly="true"/>
                        <telerik:GridTemplateColumn UniqueName="TemplateAttachment" HeaderText="Attachment" >
                            <EditItemTemplate>
                                <asp:HyperLink ID="viewAttachment" runat="server" Text="View" CssClass="btn btn-grey gridHyperlinks"></asp:HyperLink>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn UniqueName="TemplateComment" HeaderText="View Comments" >
                            <EditItemTemplate>
                                <asp:HyperLink ID="viewComment" runat="server" Text="View" CssClass="btn btn-grey gridHyperlinks"></asp:HyperLink>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Sub-Vertical" UniqueName="SubVertical" >
                            <EditItemTemplate>
                                <telerik:RadDropDownList DataTextField="Name" DataValueField="Id" RenderMode="Lightweight"  ID="drpSubVertical" runat="server" DefaultMessage="Select Sub Vertical" DropDownHeight="110px">
                                </telerik:RadDropDownList>
                                <asp:CustomValidator CssClass="hidden" ForeColor="Red" ID="custValidatorSubVertical" runat="server" ErrorMessage="* Required"
                                    ControlToValidate="drpSubVertical" />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Status" UniqueName="Status" >
                            <EditItemTemplate>
                                <telerik:RadDropDownList DataTextField="Name" DataValueField="Id" RenderMode="Lightweight"  ID="drpStatus" runat="server" DefaultMessage="Select Assigned To" DropDownHeight="110px">
                                </telerik:RadDropDownList>
                                <asp:CustomValidator CssClass="hidden" ForeColor="Red" ID="custValidatorStatus" runat="server" ErrorMessage="* Required"
                                    ControlToValidate="drpStatus" />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn UniqueName="TemplateComment" HeaderText="Comment">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtComment" runat="server"></asp:TextBox>
                                <asp:CustomValidator ForeColor="Red" ID="custValidatorComment" runat="server" ErrorMessage="* Required"
                                    ControlToValidate="txtComment" CssClass="hidden" />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="EditCommandColumn" HeaderText="Action" >
                            <ItemStyle CssClass="MyImageButton" VerticalAlign="Middle" HorizontalAlign="Center"></ItemStyle>
                        </telerik:GridEditCommandColumn>
                    </Columns>
                    <PagerStyle PageSizes="5,10,15" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                        PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />
                </MasterTableView>
            </telerik:RadGrid>
        </div>
    </div>
    <telerik:RadNotification RenderMode="Lightweight" Position="Center" runat="server" ID="radMessage" VisibleOnPageLoad="false"
        Width="350px" AutoCloseDelay="0" TitleIcon="none" ContentIcon="none">
    </telerik:RadNotification>
</asp:Content>