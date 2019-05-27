<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="SolarPMS.Login" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Solar PMS - Login</title>
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
            border: 1px solid #D9D9D9;
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
            <div class="col-xs-12 login-bg-clr padding-0 border-rad ">
                <div class="col-xs-12">
                    <div class="img-wrapper">
                        <center><img src="Content/images/suzlon_logo.png" class="img-responsive" /></center>
                    </div>
                </div>
                <!-- image wrapper-->

                <div class="col-xs-12 padding-10">
                    <div class="login-box">
                        <form role="form" runat="server">
                            <asp:Label ID="lblErrorMessage" runat="server" Visible="false" Text="User name or Password or Authentication type is invalid." CssClass="alert alert-warning"></asp:Label>
                            <div class="form-group">
                                <div style="margin: 10px 0px;">
                                    <select required="required" class="form-control" runat="server" id="ddlbProfiles">
                                        <option value="ActiveDirectory">Active Directory</option>
                                        <option value="Database">Database</option>
                                    </select>
                                </div>
                            </div>
                            <div class="clearfix"></div>
                            <div class="form-group">
                                <div class="input-group">
                                    <span class="input-group-addon"><i class="glyphicon glyphicon-user"></i></span>
                                    <%--<input type="text" class="form-control" id="uLogin" placeholder="Login">--%>
                                    <asp:TextBox ID="txtUserName" runat="server" CssClass="form-control controls"></asp:TextBox>
                                    <%--<span class="warning">*</span>--%>
                                </div>
                                <asp:RequiredFieldValidator ErrorMessage="*Required" ID="reqValUserName" runat="server"
                                    CssClass="alert alert-warning" Display="Dynamic" role="alert" ControlToValidate="txtUserName"></asp:RequiredFieldValidator>
                            </div>
                            <!-- /.form-group -->

                            <div class="form-group">
                                <div class="input-group">
                                    <span class="input-group-addon"><i class="glyphicon glyphicon-lock"></i></span>
                                    <%--<input type="password" class="form-control" id="uPassword" placeholder="Password">--%>
                                    <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
                                    <%--<span class="warning">*</span>--%>
                                </div>
                                <asp:RequiredFieldValidator ErrorMessage="*Required" ID="reqValPassword" runat="server" Display="Dynamic"
                                    CssClass="alert alert-warning" ControlToValidate="txtPassword"></asp:RequiredFieldValidator>
                                <!-- /.input-group -->
                            </div>
                            <!-- /.form-group -->

                            <%--<a href="home_screen.html" class="btn btn-primary button-style">Sign In</a>--%>
                            <div class="col-xs-12 padding-0">
                                <div class="col-xs-7 padding-0" style="line-height: 36px;">
                                    <asp:CheckBox ID="chkRemember" runat="server" Text="Remember Me" Checked="true" TextAlign="Right" />
                                </div>
                                <!-- End of Forgot Password-->
                                <div class="col-xs-5 padding-0">
                                    <asp:Button ID="btnLogin" CausesValidation="true" runat="server" Text="Sign In" CssClass="btn btn-teal button-style" OnClick="btnLogin_Click" />
                                </div>
                                <!-- End of submit Button-->
                            </div>
                            <div class="form-group">
                            </div>
                            <div class="col-xs-12 padding-0">

                                <div class="col-xs-12 padding-0 lnkForgotPassword">
                                    <asp:HyperLink ID="lnkForgotPassword" CssClass="pull-right" runat="server" NavigateUrl="ForgotPassword.aspx" Style=""><i>Forgot Password</i></asp:HyperLink>
                                </div>
                            </div>
                        </form>
                        <!-- End of form-->
                    </div>
                </div>
                <!-- Login box wrapper-->
            </div>
        </div>
        <!-- End of login Wrapper-->

    </div>
    <!-- End of container-fluid-->


    <!-- jQuery (necessary for Bootstrap's JavaScript plugins) -->
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js"></script>
    <!-- Include all compiled plugins (below), or include individual files as needed -->
    <script src="Scripts/bootstrap.min.js"></script>
</body>
</html>
