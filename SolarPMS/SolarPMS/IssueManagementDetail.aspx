<%@ Page Title="Project Management Application Detail Issue" EnableEventValidation="false" Language="C#" MasterPageFile="~/MasterPages/SolarPMS.Master" AutoEventWireup="true" CodeBehind="IssueManagementDetail.aspx.cs" Inherits="SolarPMS.IssueManagementDetail" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="Scripts/Page/Common.js"></script>
    <style>
        .RadUpload_Default .ruButton {
            background-image: none !important;
            color: #000 !important;
            background: #E6E6E6 !important;
            border: 1px solid #d9d9d9 !important;
            min-height: 32px !important;
            min-width: 72px !important;
        }
    </style>



    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="issueDate">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="issueDate" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="expectedClosureDate" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="expectedClosureDate">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="expectedClosureDate" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <%--            <telerik:AjaxSetting AjaxControlID="grdIssueAttachment">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdIssueAttachment" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>--%>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="LoadingPanel1" runat="server" Skin="Default">
    </telerik:RadAjaxLoadingPanel>

    <div class="col-xs-12 padding-0">
        <div class="col-xs-6 heading-big">
            <h5 class="margin-0 lineheight-30 breath-ctrl">Home / Issue Management/ <span style="font-weight: normal !important">Issue Management Details</span></h5>
        </div>
    </div>

    <div class="row margin-0">
        <div class="block-wrapper">
            <div class="col-md-6 col-xs-12">
                <div class="col-xs-12 padding-0">
                    <%-- <label class="col-xs-12 control-label lable-txt padding-bottom user-details-color" for="name">Add Issue</label>--%>
                    <strong>
                        <asp:Label ID="lblIssueMode" runat="server" class="col-xs-12 control-label lable-txt padding-bottom user-details-color"></asp:Label></strong>
                </div>


                <div class="row margin-0">
                    <div class="col-md-4  col-xs-12 padding-0">
                        <label class="col-xs-12 control-label lable-txt" for="name">Issue Date</label>
                    </div>
                    <div class="row margin-0">
                        <div class="col-md-8 col-xs-12">
                            <telerik:RadDatePicker RenderMode="Lightweight" ID="issueDate" Width="100%" AutoPostBack="true" runat="server" OnSelectedDateChanged="issueDate_SelectedDateChanged">
                            </telerik:RadDatePicker>
                            <asp:RequiredFieldValidator runat="server" ID="RFV1" ControlToValidate="issueDate" ErrorMessage="* Required" ForeColor="Red"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
                <div class="row margin-0">
                    <div class="col-md-4  col-xs-12 padding-0">
                        <label class="col-xs-12 control-label lable-txt" for="name">Issue Category</label>
                    </div>
                    <div class="col-md-8 col-xs-12 padding-0 ">
                        <div class="col-xs-12">
                            <%--<asp:DropDownList runat="server" CssClass="form-control" ID="drpIssueCategory">
                                </asp:DropDownList>--%>
                            <telerik:RadComboBox RenderMode="Lightweight" ID="drpIssueCategory" runat="server" Height="200" Width="100%"
                                Skin="Office2010Silver" Filter="Contains" MarkFirstMatch="true" EnableLoadOnDemand="false">
                            </telerik:RadComboBox>
                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="drpIssueCategory" ErrorMessage="* Required" ForeColor="Red"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
                <div class="row margin-0">
                    <div class="col-md-4 col-xs-12 padding-0">
                        <label class="col-xs-12 control-label lable-txt" for="name">Location Type</label>
                    </div>
                    <div class="col-md-8 col-xs-12 padding-0 ">
                        <div class="col-xs-12">
                            <%--<asp:DropDownList runat="server" CssClass="form-control" ID="drpLocation">
                                </asp:DropDownList>--%>
                            <telerik:RadComboBox RenderMode="Lightweight" ID="drpLocation" runat="server" Height="200" Width="100%"
                                Skin="Office2010Silver" Filter="Contains" MarkFirstMatch="true" EnableLoadOnDemand="false">
                            </telerik:RadComboBox>
                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator4" ControlToValidate="drpLocation" ErrorMessage="* Required" ForeColor="Red"></asp:RequiredFieldValidator>

                        </div>


                    </div>

                </div>



                <div class="row margin-0">
                    <div class="col-md-4 col-xs-12 padding-0">
                        <label class="col-xs-12 control-label lable-txt" for="name">Description</label>
                    </div>

                    <div class="col-md-8 col-xs-12 padding-0 ">


                        <div class="col-xs-12">

                            <asp:TextBox ID="txtDecription" runat="server" TextMode="MultiLine" Width="100%" Height="100%"></asp:TextBox>
                            <asp:RegularExpressionValidator runat="server" ID="valDescription" ForeColor="Red" ControlToValidate="txtDecription"
                                ValidationExpression="^[\s\S]{0,150}$" ErrorMessage="Maximum 150 characters allowed."></asp:RegularExpressionValidator>
                        </div>


                    </div>

                </div>



                <div class="row margin-0">
                    <div class="col-md-4 col-xs-12 padding-0">
                        <label class="col-xs-12 control-label lable-txt" for="name">Expected Closure Date</label>
                    </div>

                    <div class="row margin-0">


                        <div class="col-md-8 col-xs-12">


                            <telerik:RadDatePicker RenderMode="Lightweight" ID="expectedClosureDate" AutoPostBack="true" Width="100%" runat="server" OnSelectedDateChanged="expectedClosureDate_SelectedDateChanged">
                            </telerik:RadDatePicker>
                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ControlToValidate="expectedClosureDate" ErrorMessage="* Required" ForeColor="Red"></asp:RequiredFieldValidator>


                        </div>
                    </div>


                </div>

                <div class="row margin-10">
                    <div class="col-md-4 col-xs-12 padding-0">
                        <label class="col-xs-12 control-label lable-txt" for="name">Assigned To</label>
                    </div>

                    <div class="col-md-8 col-xs-12 padding-0">


                        <div class="col-xs-12">
                            <%--<asp:DropDownList runat="server" CssClass="form-control" ID="drpAssignTo">
                                </asp:DropDownList>--%>
                            <telerik:RadComboBox RenderMode="Lightweight" ID="drpAssignTo" runat="server" Height="200" Width="100%"
                                Skin="Office2010Silver" Filter="Contains" MarkFirstMatch="true" EnableLoadOnDemand="false">
                            </telerik:RadComboBox>
                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator3" ControlToValidate="drpAssignTo" ErrorMessage="* Required" ForeColor="Red"></asp:RequiredFieldValidator>
                        </div>


                    </div>

                </div>




                <div class="row margin-0">
                    <div class="col-md-4 col-xs-12 padding-0">
                        <label class="col-xs-12 control-label lable-txt" for="name">Attachment</label>
                    </div>

                    <div class="col-md-8 col-xs-12 ">
                        <style>
                            .RadAsyncUpload span.ruFileWrap {
                                padding-left: 0;
                                width: 100% !important;
                            }

                            .RadUpload .ruFakeInput {
                                min-width: 76% !important;
                                border: 1px solid #c3c3c3;
                                padding: 4px 7px !important;
                                border-radius: 4px;
                                height: 23px !important;
                                font-size: 14px !important;
                                line-height: 1.42857143 !important;
                            }

                            .RadUpload .ruFileWrap {
                                height: auto !important;
                            }
                        </style>

                        <telerik:RadAsyncUpload runat="server" ID="BtnImport1" HideFileInput="false" OnClientFileUploaded="callAjaxRequest"
                            TargetFolder="~/Upload/Attachment/" MaxFileInputsCount="1" OnFileUploaded="BtnImport1_FileUploaded" Width="100%">
                            <Localization Select="Browse" />
                        </telerik:RadAsyncUpload>




                    </div>



                </div>

                <div class="row margin-0">
                    <div class="col-md-4 col-xs-12 padding-0">
                    </div>
                    <%--  <div class="col-md-8 col-xs-12" style="overflow-x:scroll;overflow-y:hidden">--%>
                    <div class="col-md-8 col-xs-12" style="overflow-x: scroll; overflow-y: hidden">
                        <telerik:RadGrid RenderMode="Lightweight" ID="grdIssueAttachment" Width="100%" runat="server" OnItemCommand="grdIssueAttachment_ItemCommand"
                            AutoGenerateColumns="false" AllowPaging="true" AllowSorting="true" OnItemDataBound="grdIssueAttachment_ItemDataBound" AllowFilteringByColumn="false" PageSize="<%# Convert.ToInt32(SolarPMS.Models.Constants.CONST_GRID_PAGE_SIZE) %>">
                            <GroupingSettings CaseSensitive="false" />
                            <MasterTableView Width="100%" CommandItemSettings-ShowRefreshButton="false" ClientDataKeyNames="FileName" EditMode="InPlace" CommandItemDisplay="None" AllowFilteringByColumn="false">
                                <CommandItemSettings ShowRefreshButton="false" ShowExportToExcelButton="false" />
                                <Columns>
                                    <%-- <telerik:GridBoundColumn  DataField="FileName" HeaderText="Upload Files" UniqueName="FileName">
                                           <HeaderStyle Width="60%" />
                                            </telerik:GridBoundColumn>  --%>

                                    <telerik:GridTemplateColumn HeaderText="View">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnDocument" runat="server" OnClick="btnDocument_Click" CausesValidation="false" Text='<%# Eval("FileName") %>'></asp:LinkButton>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <%--<telerik:GridButtonColumn  CommandName="download_file" Text="View"  HeaderButtonType="LinkButton" UniqueName="Download" HeaderText="View"></telerik:GridButtonColumn>                                  --%>
                                    <telerik:GridButtonColumn CommandName="Remove" Text="Remove" HeaderButtonType="LinkButton" UniqueName="Remove" HeaderText="Remove"></telerik:GridButtonColumn>





                                </Columns>
                                <PagerStyle PageSizes="5,10,15" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                                    PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />


                            </MasterTableView>
                        </telerik:RadGrid>
                    </div>
                </div>

            </div>




            <div class="col-md-6 col-xs-12">


                <div class="col-xs-12 padding-0">
                    <label class="col-xs-12 control-label lable-txt padding-bottom user-details-color" for="name">&nbsp;</label>
                </div>


                <div class="row margin-0">
                    <div class="col-md-4 col-xs-12 padding-0">
                        <label class="col-xs-12 control-label lable-txt" for="name">Resolution</label>
                    </div>

                    <div class="col-md-8 col-xs-12 padding-0 ">

                        <div class="form-group">
                            <div class="col-xs-12 padding-0">

                                <asp:TextBox ID="txtResolution" runat="server" TextMode="MultiLine" Width="100%" Height="100%"></asp:TextBox>
                                <asp:RegularExpressionValidator runat="server" ID="valResoluion" ForeColor="Red" ControlToValidate="txtResolution"
                                    ValidationExpression="^[\s\S]{0,150}$" ErrorMessage="Maximum 150 characters allowed."></asp:RegularExpressionValidator>
                            </div>
                        </div>

                    </div>
                    <!-- End of col-xs-12-->
                </div>
                <!-- End of row-->
                <div class="row margin-0">
                    <div class="col-md-4 col-xs-12 padding-0">
                        <asp:Label ID="lblLocationType" runat="server" Text="Location Number" class="col-xs-12 control-label lable-txt lable-heading"></asp:Label>
                    </div>

                    <div class="col-md-8 col-xs-12 padding-0 ">
                        <telerik:RadTextBox ID="txtLocationType" MaxLength="999" runat="server" Width="100%" Height="100%"></telerik:RadTextBox>
                    </div>
                </div>
                <div class="col-md-4 col-xs-12 padding-0">
                    <label class="col-xs-12 control-label lable-txt" for="name">Comment/Remarks</label>
                </div>
                <!-- End of col-md-4 col-xs-12-->
                <div class="col-md-8 col-xs-12 padding-0 " style="position: relative">

                    <div class="row chat-window col-xs-12 col-md-12 margin-0 padding-0" id="chat_window_1">
                        <div>
                            <div class="panel panel-default">

                                <div class="panel-body msg_container_base">

                                    <asp:Label ID="lblCommentDetail" CssClass="messages msg_sent" runat="server" Text=""></asp:Label>

                                </div>
                                <div class="panel-footer">
                                    <div class="input-group">
                                        <asp:TextBox runat="server" MaxLength="150" ID="txtCommentBox" Text="" class="form-control input-sm chat_input" placeholder="Enter comments/remarks here"></asp:TextBox>

                                        <span class="input-group-btn">

                                            <asp:Button ID="btnAddComment" CausesValidation="false" runat="server" Text="Comment" class="btn btn-primary btn-sm" OnClick="btnAddComment_Click" />
                                        </span>
                                    </div>
                                </div>
                            </div>

                        </div>

                    </div>


                </div>

                <div class="row margin-0">
                    <div class="col-md-4 col-xs-12 padding-0">

                        <asp:Label ID="lblAssignFrom" runat="server" Text="Assign From" class="col-xs-12 control-label lable-txt lable-heading"></asp:Label>
                    </div>

                    <div class="col-md-8 col-xs-12 padding-0 ">

                        <asp:TextBox ID="txtAssignFrom" runat="server" Width="100%" Height="100%"></asp:TextBox>

                    </div>

                </div>


                <div class="row margin-0">
                    <div class="col-md-4 col-xs-12 padding-0">

                        <asp:Label ID="lblStatus" runat="server" Text="Status" class="col-xs-12 control-label lable-txt lable-heading"></asp:Label>
                    </div>

                    <div class="col-md-8 col-xs-12 padding-0 ">

                        <asp:DropDownList runat="server" CssClass="form-control" ID="drpStatus">
                            <asp:ListItem Value="Open">Open</asp:ListItem>
                            <asp:ListItem Value="Ongoing">Ongoing</asp:ListItem>
                            <asp:ListItem Value="Closed">Closed</asp:ListItem>
                            <asp:ListItem Value="Resolved">Resolved</asp:ListItem>
                            <%--  <asp:ListItem Value="Draft">Draft</asp:ListItem>--%>
                        </asp:DropDownList>

                    </div>

                </div>


                <div class="row margin-0">
                    <div class="col-md-4 col-xs-12 padding-0">
                    </div>

                    <div class="col-md-8 col-xs-12 padding-0 ">
                    </div>

                </div>



            </div>


        </div>


        <div class="row margin-0">
            <center>
                  <div class="submit-btn">
                         <asp:Button ID="BtnSave" runat="server" CssClass="btn btn-primary button button-style" Text="Save" OnClick="BtnSave_Click"  />
                </div>
                 <div class="submit-btn">
                
                     <asp:Button ID="BtnSubmit" runat="server" CssClass="btn btn-primary button button-style" Text="Submit" OnClick="BtnSubmit_Click"  />
                </div>
                <div class="submit-btn">
                   <asp:Button ID="BtnCancel" runat="server" CausesValidation="false" CssClass="btn btn-primary button button-style" Text="Cancel" OnClick="BtnCancel_Click"/>                   
                </div>
                </center>

        </div>
        <telerik:RadNotification RenderMode="Lightweight" Position="Center" runat="server" ID="radMesaage" VisibleOnPageLoad="false"
            Width="450px" Animation="Fade" TitleIcon="none" ContentIcon="none" Style="z-index: 100000" EnableRoundedCorners="true" EnableShadow="true" AnimationDuration="1000">
        </telerik:RadNotification>
    </div>
    <telerik:RadScriptBlock ID="radSript1" runat="server">
        <script type="text/javascript">
            function GetRadWindow() {
                var oWindow = null;
                if (window.radWindow)
                    oWindow = window.radWindow;
                else if (window.frameElement && window.frameElement.radWindow)
                    oWindow = window.frameElement.radWindow;
                return oWindow;
            }

            function CloseModal() {
                    GetRadWindow().close();
            }

        </script>
    </telerik:RadScriptBlock>
</asp:Content>
