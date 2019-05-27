﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ForgotPassword.aspx.cs" Inherits="SolarPMS.ForgotPassword" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Solar PMS - Forgot Password</title>
    <link href="Content/css/bootstrap.min.css" rel="stylesheet" />
    <link href="Content/css/global.css" rel="stylesheet" />
    <style>
        .val-text {
            color: red;
        }
         .input-group-addon {
            color: #FFF !important;
            text-shadow: -1px -1px 0 #333, 1px -1px 0 #333, -1px 1px 0 #333, 1px 1px 0 #333;
            background-color: #fff !important;
            border-right-color: #FFF !important;
            border: 1px solid #d9d9d9;
        }

        .form-group {
            margin-bottom: 10px !important;
        }

        .form-control {
            border: 1px solid #008080;
        }

        label {
            color: #165C68;
        }
        
    </style>
</head>
<body>
    <div class="container-fluid padding-0 login-bg">
        <div class="login-wrapper">
            <div class="col-xs-12 login-bg-clr border-rad">
            <form role="form" runat="server">
                <div id="divSuccessMessage" class="" runat="server" visible="false">
                    You sent a request to reset your password. Please use the link provided in your email to reset the password for your account.
                </div>
                <div class="text-center" style="padding:10px;">
                    <asp:Label ID="lblMessage" runat="server"></asp:Label>
                </div>
                <div class="form-group">
                    <label for="uLogin">Enter Email Id</label>
                    <div class="input-group">
                        <span class="input-group-addon"><i class="glyphicon glyphicon-envelope"></i></span>
                        <%--<input type="text" class="form-control" id="uLogin" placeholder="Login">--%>
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control"></asp:TextBox>
                        <%--<span class="warning">*</span>--%>
                    </div>
                    <asp:RequiredFieldValidator ErrorMessage="*Required" ForeColor="Red" ID="reqValUserName" runat="server"
                        Display="Dynamic"  ControlToValidate="txtEmail"></asp:RequiredFieldValidator>
                </div>
                <!-- /.form-group -->
                <div class="button-wrapper">
                    <center>
                        <div class="submit-btn">
                            <asp:Button ID="btnSubmit" CausesValidation="true" runat="server" Text="Submit" CssClass="btn btn-primary button-style " OnClick="btnSubmit_Click" />

                        </div>
                        <div class="submit-btn">
                            <asp:Button ID="btnBack" CausesValidation="false" runat="server" Text="Back" CssClass="btn btn-primary button-style" PostBackUrl="~/Login.aspx" />
                        </div>
                        
                    </center>
                </div>
                
                <%--<div class="form-group">
                    <div class="pull-left">
                        <asp:Button ID="btnSubmit" CausesValidation="true" runat="server" Text="Submit" CssClass="btn btn-primary button-style" OnClick="btnSubmit_Click" />
                    </div>
                    <div class="pull-right">
                        <asp:Button ID="btnBack" CausesValidation="true" runat="server" Text="Submit" CssClass="btn btn-primary button-style" PostBackUrl="~/Login.aspx" />
                    </div>
                </div>--%>
            </form>
            <!-- End of form-->
            </div><!-- End of form Wrapper-->

            <!-- End of login box-->
        </div>
        <!-- End of login Wrapper-->
    </div>

    <!-- jQuery (necessary for Bootstrap's JavaScript plugins) -->
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js"></script>
    <!-- Include all compiled plugins (below), or include individual files as needed -->
    <script src="../Scripts/bootstrap.min.js"></script>
</body>
</html>
