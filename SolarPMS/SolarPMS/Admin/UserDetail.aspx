<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/SolarPMS.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="UserDetail.aspx.cs" Inherits="SolarPMS.Admin.UserDetail" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>

        .RadUpload_Default .ruSelectWrap .ruFakeInput{
    width: 92px !important;
}

    </style>
    <script src="../Scripts/Page/UserMaster.js"></script>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="BtnSave">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="pnl1" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <%--  <telerik:AjaxSetting >
             <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="RadAjaxPanel1" />
                <telerik:AjaxUpdatedControl ControlID="RadAjaxPanel2" />
            </UpdatedControls>
           </telerik:AjaxSetting>--%>
        </AjaxSettings>
    </telerik:RadAjaxManager>

    <telerik:RadAjaxLoadingPanel ID="LoadingPanel1" runat="server">
        <%--BackImageUrl="~/Content/images/loading.gif" style="background-position: center !important; background-repeat:no-repeat !important;">--%>
    </telerik:RadAjaxLoadingPanel>

          <div class="col-xs-6 heading-big">
                    <h5 class="margin-0 lineheight-30 breath-ctrl">Home / Administration / User Details / <span style="font-weight: normal !important">Add User Details</span></h5>
                </div>
    <div class="clearfix"></div>
    <asp:Panel ID="pnl1" runat="server">
        <div class="container-fluid padding-0">
            <div class="row margin-0">
                <div class="block-wrapper">
                    <div class="col-md-6 col-xs-12">
                        <div class="col-xs-12 padding-0">
                            <label class="col-xs-12 control-label lable-txt padding-bottom user-details-color">User Details</label>
                        </div>

                        <div class="col-md-4 col-xs-12 padding-0">
                            <label class="col-xs-12 control-label lable-txt">Photo</label>
                        </div>

                        <div class="row margin-0">


                            <div class="col-md-8 col-xs-12 padding-0">
                                <div class="col-lg-2 col-md-2 col-sm-2" style="width: 100px; height: 76px;">
                                    <asp:Image ID="ImgEmp" runat="server" ImageUrl="~/Content/images/placeholder-icon.png" style="width:100%;" />
                                </div>
                                <div class="col-lg-2 padding-0" style="margin-top: 15px;">

                                    <div id="dwndWrapper">
                                        <style type="text/css">
                                            .RadUpload .ruBrowse {
                                                min-height: 30px;
                                            }

                                            .RadUpload .ruFakeInput {
                                                min-height: 30px;
                                            }
                                        </style>
                                        <telerik:RadAsyncUpload RenderMode="Lightweight" ID="AsyncUpload1" runat="server"
                                            MaxFileSize="2097152"  TargetFolder="~/Profile/" OnClientFileUploaded="callAjaxRequest"
                                            AutoAddFileInputs="false" Localization-Select="Upload Image" OnFileUploaded="AsyncUpload1_FileUploaded" />
                                        <asp:Label ID="Label1" Text="" runat="server" Style="font-size: 10px;"></asp:Label>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row margin-0">
                            <div class="col-md-4 col-xs-12 padding-0">
                                <label class="col-xs-12 control-label lable-txt">Authentication</label>
                            </div>

                            <div class="col-md-8 col-xs-12 padding-0">

                                
                                    <div class="col-xs-12 padding-0">
                                          <telerik:RadComboBox RenderMode="Lightweight" ID="DrpAuthentication" runat="server" Height="200" Width="100%" 
                                            Skin="Office2010Silver" MarkFirstMatch="true" EnableLoadOnDemand="false">
                                              <Items>
                                                  <telerik:RadComboBoxItem Text="Select Authentication" Value="" />
                                                  <telerik:RadComboBoxItem Text="AD" Value="AD" />
                                                  <telerik:RadComboBoxItem Text="Database" Value="Database" />
                                              </Items>
                                        </telerik:RadComboBox>
                                        <asp:RequiredFieldValidator ErrorMessage="* Required" Display="Dynamic" ForeColor="Red" runat="server" ControlToValidate="DrpAuthentication"></asp:RequiredFieldValidator>
                                    </div>
                                

                            </div>
                        </div>


                        <div class="row margin-0">
                            <div class="col-md-4 col-xs-12 padding-0">
                                <label class="col-xs-12 control-label lable-txt">User Id</label>
                            </div>
                            <div class="col-md-8 col-xs-12 padding-0">
                                <%--<telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">--%>
                                <asp:TextBox ID="TxtUserId" runat="server" autocomplete="false" AutoCompleteType="Disabled" placeholder="Enter User ID" CssClass="search-query form-control"></asp:TextBox>
                                <%--  </telerik:RadAjaxPanel>--%>
                            </div>
                            <asp:RequiredFieldValidator ID="ReqFldValUserId" ErrorMessage="* Required" Display="Dynamic" ForeColor="Red" runat="server" ControlToValidate="TxtUserId"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator Display="Dynamic" ForeColor="Red" ControlToValidate="TxtUserId" ID="RegularExpressionValidator1" ValidationExpression="^[\s\S]{0,15}$" runat="server" ErrorMessage="* Maximum 15 characters allowed."></asp:RegularExpressionValidator>
                        </div>


                        <div class="row margin-0">
                            <div class="col-md-4 col-xs-12 padding-0">
                                <label class="col-xs-1 control-label lable-txt">Password</label>
                            </div>

                            <div class="col-md-8 col-xs-12 padding-0">
                              
                                <asp:TextBox ID="TxtPassword" runat="server" AutoCompleteType="None" placeholder="Enter Password" TextMode="Password" CssClass="search-query form-control"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="ReqFldValPwd" ErrorMessage="* Required" Display="Dynamic" ForeColor="Red" runat="server" ControlToValidate="TxtPassword"></asp:RequiredFieldValidator>
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
                                <label class="control-label lable-txt">User Name</label>
                            </div>
                            <div class="col-md-8 col-xs-12 padding-0">
                                <asp:TextBox ID="TxtUserName" runat="server" placeholder="Enter User Name" CssClass="search-query form-control"></asp:TextBox>
                                <asp:RequiredFieldValidator ErrorMessage="* Required" Display="Dynamic" ForeColor="Red" runat="server" ControlToValidate="TxtUserName"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator Display="Dynamic" ForeColor="Red" ControlToValidate="TxtUserName" ID="RegularExpressionValidator6" ValidationExpression="^[\s\S]{0,50}$" runat="server" ErrorMessage="* Maximum 50 characters allowed."></asp:RegularExpressionValidator>
                            </div>
                        </div>
                        <div class="row margin-0">
                            <div class="col-md-4 col-xs-12 padding-0">
                                <label class="col-xs-12 control-label lable-txt">Employee ID</label>
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
                                <asp:TextBox ID="TxtEmailId" runat="server" placeholder="Enter Email Id" CssClass="search-query form-control"></asp:TextBox>
                                <asp:RequiredFieldValidator ErrorMessage="* Required" Display="Dynamic" ForeColor="Red" runat="server" ControlToValidate="TxtEmailId"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="TxtEmailId" ID="RegularExpressionValidator5" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" runat="server" ForeColor="Red" ErrorMessage="* Enter valid email id."></asp:RegularExpressionValidator>
                                <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="TxtEmailId" ID="RegularExpressionValidator7" ValidationExpression="^[\s\S]{0,50}$" runat="server" ForeColor="Red" ErrorMessage="* Maximum 50 characters allowed."></asp:RegularExpressionValidator>
                            </div>

                        </div>


                        <div class="row margin-0" runat="server" visible="false">
                            <div class="col-md-4 col-xs-12 padding-0">
                                <label class="col-xs-12 control-label lable-txt" for="location">Location</label>
                            </div>

                            <div class="col-md-8 col-xs-12 padding-0 ">
                                    <div class="col-xs-12 padding-0">
                                        <telerik:RadComboBox RenderMode="Lightweight" ID="DrpLocation" runat="server" Height="200" Width="100%"
                                            Skin="Office2010Silver" MarkFirstMatch="true" EnableLoadOnDemand="false">
                                        </telerik:RadComboBox>
                                        <asp:RequiredFieldValidator ErrorMessage="* Required" Display="Dynamic" ForeColor="Red" runat="server" ControlToValidate="DrpLocation"></asp:RequiredFieldValidator>
                                    </div>
                            </div>
                        </div>

                        <div class="row margin-0">
                            <div class="col-md-4 col-xs-12 padding-0">
                                <label class="col-xs-12 control-label lable-txt" for="mobile">Mobile No.</label>
                            </div>

                            <div class="col-md-8 col-xs-12 padding-0">
                                <asp:TextBox ID="TxtMobileNo" runat="server" placeholder="Enter Mobile No." CssClass="search-query form-control"></asp:TextBox>
                                <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="TxtMobileNo" ID="RegularExpressionValidator4" ValidationExpression="^\d{1,15}$" runat="server" ForeColor="Red" ErrorMessage="* Maximum 15 Digits Number Only."></asp:RegularExpressionValidator>
                            </div>

                        </div>




                        <div class="row margin-0">
                            <div class="col-md-4 col-xs-12 padding-0">
                                <label class="col-xs-12 control-label lable-txt" for="status">Status</label>
                            </div>

                            <div class="col-md-8 col-xs-12 padding-0">

                                    <div class="col-xs-12 padding-0">
                                          <telerik:RadComboBox RenderMode="Lightweight" ID="DrpStatus" runat="server" Height="200" Width="100%" 
                                            Skin="Office2010Silver" MarkFirstMatch="true" EnableLoadOnDemand="false">
                                              <Items>
                                                  <telerik:RadComboBoxItem Text="Select Status" Value="" />
                                                  <telerik:RadComboBoxItem Text="Enabled" Value="Enabled" />
                                                  <telerik:RadComboBoxItem Text="Disabled" Value="Disabled" />
                                              </Items>
                                        </telerik:RadComboBox>
                                        <asp:RequiredFieldValidator ErrorMessage="* Required" Display="Dynamic" ForeColor="Red" runat="server" ControlToValidate="DrpStatus"></asp:RequiredFieldValidator>
                                    </div>
                                

                            </div>

                        </div>

                    </div>



                    <div class="col-md-6 col-xs-12">
                        <div class="col-xs-12 padding-0">
                            <label class="col-xs-12 control-label lable-txt padding-bottom user-details-color">Access Details</label>
                        </div>


                        <div class="row margin-0">
                            <div class="col-md-4 col-xs-12 padding-0">
                                <label class="col-xs-12 control-label lable-txt" for="Profile">Profile</label>
                            </div>

                            <div class="col-md-8 col-xs-12 padding-0 overflow">

                                
                                    <div class="col-xs-12">
                                        <telerik:RadComboBox RenderMode="Lightweight" ID="DrpProfile" runat="server" Height="200" Width="100%" 
                                            Skin="Office2010Silver" MarkFirstMatch="true" EnableLoadOnDemand="true">
                                        </telerik:RadComboBox>
                                        <asp:RequiredFieldValidator ErrorMessage="* Required" Display="Dynamic" ForeColor="Red" runat="server" ControlToValidate="DrpProfile"></asp:RequiredFieldValidator>
                                    </div>
                                

                            </div>

                        </div>


                        <div class="row margin-0">
                            <div class="col-md-4 col-xs-12 padding-0">
                                <label class="col-xs-12 control-label lable-txt">Reporting Manager</label>
                            </div>

                            <div class="col-md-8 col-xs-12 padding-0 overflow">

                                <div class="form-group">
                                    <div class="col-xs-12">
                                        <telerik:RadComboBox RenderMode="Lightweight" ID="DrpReportingMgr" runat="server" Height="200" Width="100%" 
                                            Skin="Office2010Silver" MarkFirstMatch="true" EnableLoadOnDemand="false">
                                        </telerik:RadComboBox>
                                        <asp:RequiredFieldValidator ErrorMessage="* Required" Display="Dynamic" ForeColor="Red" runat="server" ControlToValidate="DrpReportingMgr"></asp:RequiredFieldValidator>
                                    </div>
                                </div>

                            </div>

                        </div>
                        <div class="row margin-0">
                            <asp:Label ID="LblErrooMsg" runat="server" ForeColor="Red" Text=""></asp:Label>
                        </div>
                    </div>

                    <div class=" col-xs-12 padding-0">
                        
                            <div class="button-wrapper margin-10">
                                <center>
                <div class="submit-btn">
                   <asp:Button ID="BtnSave" runat="server" CssClass="btn btn-primary button button-style" Text="Save" OnClick="BtnSave_Click" />
                    
                </div>
                <div class="submit-btn">
                   <asp:Button ID="BtnCancel" runat="server" CausesValidation="false" CssClass="btn btn-primary button button-style" OnClientClick=" if(!CancelPopUp()) return false;"  Text="Cancel" OnClick="BtnCancel_Click" />
                   
                </div>
                </center>
                            </div>
                        
                    </div>

                <telerik:RadNotification RenderMode="Lightweight" Position="Center" runat="server" ID="radMesaage" VisibleOnPageLoad="false"
                Width="450px" Animation="Fade" TitleIcon="none" ContentIcon="none" Style="z-index: 100000" EnableRoundedCorners="true" EnableShadow="true" AnimationDuration="1000">
                </telerik:RadNotification>

                </div>



            </div>


        </div>

    </asp:Panel>
</asp:Content>
