<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/SolarPMS.Master" AutoEventWireup="true" CodeBehind="EscalationMatrics.aspx.cs" Inherits="SolarPMS.Admin.EscalationMatrics" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
         <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="BtnSave">               
                  <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="pnl1" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>               
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="LoadingPanel1" runat="server" Skin="Default">
    </telerik:RadAjaxLoadingPanel>
    
     <script src="../Scripts/Page/Common.js"></script>
        <div class="col-xs-12 heading-big">
                    <h5 class="margin-0 lineheight-30 breath-ctrl">Home / Administration / <span style="font-weight: normal !important">Escalation Matrix</span></h5>
                </div>
       <div class="escalation-matrix padding-t-20">
         <div class="text-center ">
            <asp:Label ID="lblMessage" runat="server" Visible="false" CssClass="alert alert-warning"></asp:Label>
        </div>
            <div class="item col-xs-12 col-lg-12">
                <div class="row">
                    <div class="col-md-12">
                        <div class="col-lg-9">
                            <label class=" control-label lable-txt" for="name">Activity taking Longer than Planed</label>
                        </div>
                        <div class="col-lg-3 padding-0 ">
                           <%-- <input type="text" value="0" class="form-control" placeholder="1">--%>
                            <telerik:RadNumericTextBox MinValue="0" MaxValue="9999999999" CssClass="form-control" Width="100%"  MaxLength="9" ID="txtActLonThanPlan" runat="server">
                            <NumberFormat GroupSeparator="" DecimalDigits="0" KeepNotRoundedValue="false" />
                            <ClientEvents OnKeyPress="KeyPress" /> 
                        </telerik:RadNumericTextBox>
                        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="txtActLonThanPlan"
                        Display="Static" ErrorMessage="* Required!"  ForeColor="Red" CssClass="validator" />

                        </div>
                      
                    </div>

                    <div class="col-md-12">
                        <div class="col-lg-9">
                            <label class=" control-label lable-txt" for="name">Issue Not Resolution</label>
                        </div>
                        <div class="col-lg-3 padding-0">
                            
                           <%--     <input type="text" value="0" class="form-control" placeholder="1">                           --%>
                        <telerik:RadNumericTextBox MinValue="0" MaxValue="9999999999" CssClass="form-control" Width="100%" MaxLength="9" ID="txtIssueResolution" runat="server">
                            <NumberFormat GroupSeparator="" DecimalDigits="0" KeepNotRoundedValue="false" />
                            <ClientEvents OnKeyPress="KeyPress" /> 
                        </telerik:RadNumericTextBox>       
                       <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ControlToValidate="txtIssueResolution"
                        Display="Static" ErrorMessage="* Required!" ForeColor="Red" CssClass="validator" />                    
                         
                        </div>
                     
                    </div>

                    <div class="col-md-12">
                        <div class="col-lg-9">
                            <label class=" control-label lable-txt" for="name"> Issue Not Closed</label>
                        </div>
                        <div class="col-lg-3 padding-0">
                            <%--<input type="text" value="0" class="form-control" placeholder="1">--%>
                       <telerik:RadNumericTextBox MinValue="0" MaxValue="9999999999" CssClass="form-control" Width="100%"  MaxLength="9" ID="txtIssueClosed" runat="server">
                            <NumberFormat GroupSeparator="" DecimalDigits="0" KeepNotRoundedValue="false" />
                            <ClientEvents OnKeyPress="KeyPress" /> 
                        </telerik:RadNumericTextBox>
                   <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator3" ControlToValidate="txtIssueClosed"
                        Display="Static" ErrorMessage="* Required!" ForeColor="Red" CssClass="validator" />  
                        </div>                      
                    </div>
                </div>
              
            </div>      
            <div class="item col-xs-12 col-lg-12">
                <div class="row magrin-0">
                    <div class="col-md-12">
                        <div class="col-lg-9">
                            <label class=" control-label lable-txt" for="name"> Quality Issue Not resolved</label>
                        </div>
                        <div class="col-lg-3 padding-0">
                            <%--<input type="text" value="0" class="form-control" placeholder="1">--%>
                          <telerik:RadNumericTextBox MinValue="0" MaxValue="9999999999" CssClass="form-control" Width="100%"  MaxLength="9" ID="txtQuaRejResolution" runat="server">
                            <NumberFormat GroupSeparator="" DecimalDigits="0" KeepNotRoundedValue="false" />
                            <ClientEvents OnKeyPress="KeyPress" /> 
                        </telerik:RadNumericTextBox>
                          <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator4" ControlToValidate="txtQuaRejResolution"
                        Display="Static" ErrorMessage="* Required!" ForeColor="Red" CssClass="validator" />  
                        </div>
                        
                    </div>
                    <div class="col-md-12">
                        <div class="col-lg-9">
                            <label class=" control-label lable-txt" for="name"> Quality Rejections Not Closed</label>
                        </div>
                        <div class="col-lg-3 padding-0">
                            <%--<input type="text" value="0" class="form-control" placeholder="1">--%>
                          <telerik:RadNumericTextBox MinValue="0" MaxValue="9999999999" CssClass="form-control" Width="100%"  MaxLength="9" ID="txtQulRejClosed" runat="server">
                            <NumberFormat GroupSeparator="" DecimalDigits="0" KeepNotRoundedValue="false" />
                            <ClientEvents OnKeyPress="KeyPress" /> 
                        </telerik:RadNumericTextBox>
                             <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator5" ControlToValidate="txtQulRejClosed"
                        Display="Static" ErrorMessage="* Required!" ForeColor="Red" CssClass="validator" />  
                        </div>                       
                    </div>
                </div>           

            </div>           
            <div class="col-sm-12">
                <center>
                <div class="submit-btn margin-10">
                    <%--<a href="home_screen.html" class="btn btn-primary button-style button">Save</a>--%>
                    <asp:Button ID="BtnSave" runat="server" CssClass="btn btn-primary button button-style" Text="Save" OnClick="BtnSave_Click"  />
                </div>
      <%--          <div class="submit-btn">
                    
                     <asp:Button ID="BtnCancel" runat="server" CausesValidation="false" Visible="false" CssClass="btn btn-primary button button-style" Text="Cancel" OnClick="BtnCancel_Click" />
                </div>--%>
            </center>
            </div>
           <telerik:RadNotification RenderMode="Lightweight" Position="Center" runat="server" ID="radMesaage" VisibleOnPageLoad="false"
            Width="450px" Animation="Fade" TitleIcon="none" ContentIcon="none" Style="z-index: 100000" EnableRoundedCorners="true" EnableShadow="true" AnimationDuration="1000">
        </telerik:RadNotification>
             
        </div>
</asp:Content>
