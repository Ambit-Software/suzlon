<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ResetPassword.aspx.cs" Inherits="SolarPMS.ResetPassword" %>

<!DOCTYPE html>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Solar PMS - Reset Password</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link href="<%=SolarPMS.Models.Constants.ApplicationPath%>/Content/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
        <link href="<%=SolarPMS.Models.Constants.ApplicationPath%>/Content/css/global.css" rel="stylesheet" type="text/css" />
        <link href="<%=SolarPMS.Models.Constants.ApplicationPath%>/Content/css/suzlon.css" rel="stylesheet" type="text/css" />
        <script src="<%=SolarPMS.Models.Constants.ApplicationPath%>/Scripts/jquery.min.js"></script>
        <script src="<%=SolarPMS.Models.Constants.ApplicationPath%>/Scripts/bootstrap.min.js"></script>
    </telerik:RadCodeBlock>

    <style>
        .val-text {
            color: red;
        }
        label {
            color: #165C68;
        }
    </style>
</head>
<body>
    <div class="container-fluid padding-0 login-bg">
    <form id="form1" runat="server">
        <div class="login-wrapper">
            
            <div class="col-xs-12 padding-0 login-bg-clr border-rad">
                <div class="col-xs-12 heading-teal">
                <p><strong>Reset Password</strong></p>
            </div>
                <div id="divErrorMessage" runat="server" visible="false" class="text-center">
                    <h2>Access Denied</h2>
                </div>
                <div class="row margin-0" id="divContainer" runat="server" visible="false">
                    <div class="text-center">
                        <asp:Label ID="lblUpdateSuccess" CssClass="alert alert-success" runat="server" Font-Bold="true"></asp:Label>
                    </div>
                    <div id="divOldPassword" runat="server" class="col-xs-12" visible="false">
                        <div>
                            <div class="col-xs-12 padding-0">
                                <label class="control-label lable-txt padding-0" for="name">Old Password</label>
                            </div>
                            <!-- End of col-lg-1-->

                            <div class="col-xs-12 padding-0">
                                <asp:TextBox ID="txtOldPassword" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
                                <asp:RequiredFieldValidator ErrorMessage="*Required" ID="reqValUserName" runat="server"
                                    Display="Dynamic" ForeColor="Red" ControlToValidate="txtOldPassword"></asp:RequiredFieldValidator>                               
                            </div>
                            <!-- End of input-group-col-xs-8-->

                        </div>
                        <!-- End of form-->

                    </div>

                    <div class="col-xs-12">

                        <div class="col-xs-12 padding-0">
                            <label class="control-label lable-txt padding-0" for="name">New Password</label>
                        </div>
                        <!-- End of col-lg-1-->

                        <div class=" col-xs-12 padding-0">
                            <%--<input type="Password" class="search-query form-control" placeholder="Enter New Password" />--%>
                            <asp:TextBox ID="txtNewPassword" runat="server" CssClass="form-control" TextMode="Password" MaxLength="15"></asp:TextBox>
                            <asp:RequiredFieldValidator ErrorMessage="*Required" ID="RequiredFieldValidator1" runat="server"
                                Display="Dynamic" ForeColor="Red" ControlToValidate="txtNewPassword"></asp:RequiredFieldValidator>
                             <asp:RegularExpressionValidator ID="regPassword" ControlToValidate="txtNewPassword" runat="server" CssClass="alert alert-warning"
                                    ErrorMessage="Password criteria doesn't match." Display="Dynamic"
                                    ValidationExpression="^(?=.*[!@#$%^&*]).{6,15}$"></asp:RegularExpressionValidator>
                        </div>
                        <!-- End of input-group-col-xs-8-->
                        <!-- End of form-->
                    </div>
                    <div class="col-xs-12">
                        <div class="col-xs-12 padding-0">
                            <label class="control-label lable-txt padding-0" for="name">Confirm Password</label>
                        </div>
                        <!-- End of col-lg-1-->

                        <div class="col-xs-12 padding-0">
                            <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="form-control" TextMode="Password" MaxLength="15"></asp:TextBox>
                            <asp:RequiredFieldValidator ErrorMessage="*Required" ID="RequiredFieldValidator2" runat="server"
                                Display="Dynamic" ForeColor="Red" ControlToValidate="txtConfirmPassword"></asp:RequiredFieldValidator>
                            <asp:CompareValidator ID="cmpValPassword" runat="server" ErrorMessage="Password and confirm password should be same" style="text-align:justify;"
                                Display="Dynamic" ControlToCompare="txtNewPassword" ControlToValidate="txtConfirmPassword" CultureInvariantValues="False"></asp:CompareValidator>
                        </div>
                        <!-- End of input-group-col-xs-8-->

                        <!-- End of form-->
                    </div>
                     <div class="text-info small">
                    <div class="col-xs-12">Note: Password requires 1 special character (!@#$%^&*) and the length should be between 6-15 characters.</div>
                </div>
                    <div class="col-xs-12 text-center">
                        <div class="submit-btn text-center">
                            <asp:Button ID="btnSubmit" CausesValidation="true" runat="server" Text="Submit" CssClass="btn btn-teal button-style" OnClick="btnSubmit_Click" />
                        </div>
                         <div class="submit-btn text-center">
                            <asp:Button ID="btnLogin" CausesValidation="false" runat="server" Text="Login" CssClass="btn btn-teal button-style" PostBackUrl="~/Login.aspx" />
                        </div>
                    </div>
                </div>
            </div>
            <!-- End of Survey wrapper-->
        </div>
        <asp:HiddenField ID="hidUserName" runat="server" />
        <asp:HiddenField ID="hidOldPassword" runat="server" />
    </form>
    </div>
</body> 
</html>
    