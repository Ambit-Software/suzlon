<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="SuzlonBPP.Login" %>

<!--[if lt IE 7 ]> <html class="ie6"> <![endif]-->
<!--[if IE 7 ]> <html class="ie7"> <![endif]-->
<!--[if IE 8 ]> <html class="ie8"> <![endif]-->
<!--[if IE 9 ]> <html class="ie9"> <![endif]-->
<!--[if (gt IE 9)|!(IE)]><!--> 
<!DOCTYPE html><!--<![endif]-->
<%--<html xmlns="http://www.w3.org/1999/xhtml">--%>
<head runat="server">
    <title>Suzlon BPP - Login</title>
    <link href="Content/css/bootstrap.min.css" rel="stylesheet" />
    <link href="Content/css/global.css" rel="stylesheet" />
    <style>
        .val-text {
            color: red;
        }

        .input-group-addon {
            color: #FFF;
            text-shadow: -1px -1px 0 #484848, 1px -1px 0 #484848, -1px 1px 0 #484848, 1px 1px 0 #333;
            background-color: #fff !important;
            border-right-color: #FFF !important;
            border: 1px solid #d9d9d9;
        }

        .ie9 .input-group-addon {
            color: #808080;
            text-shadow: none;
            background-color: #d9d9d9 !important;
            border-right-color: #d9d9d9 !important;
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
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<body style="background-color: #FFF;">
    <div class="container-fluid padding-0 login-bg">
        <div class="login-wrapper">
            <div class="col-xs-12 padding-0 border-rad ">
                <div class="col-xs-12">
            <div class="img-wrapper">
                <center><img src="Content/images/logo.png" class="img-responsive" /></center>
            </div>
                </div>
                <div class="col-xs-12 padding-10">
            <div class="login-box">
                <form role="form" runat="server">
                    <asp:ScriptManager ID="ScriptMgr" runat="server" EnablePageMethods="true" EnablePartialRendering="true" />
                    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
                        <AjaxSettings>
                            <telerik:AjaxSetting AjaxControlID="btnLogin">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="formPanel" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                        </AjaxSettings>
                    </telerik:RadAjaxManager>
                    <telerik:RadAjaxLoadingPanel ID="LoadingPanel1" runat="server" Skin="Default"></telerik:RadAjaxLoadingPanel>
                    <asp:Panel ID="formPanel" runat="server">
                        <asp:Label ID="lblErrorMessage" runat="server" Visible="false" Text="Username or Password or Authentication type is invalid." CssClass="val-text"></asp:Label>
                        <div style="margin: 10px 0px;">
                            <select required="required" class="form-control" runat="server" id="ddlbProfiles">
                                <option value="ActiveDirectory">Active Directory</option>
                                <option value="Database">Database</option>
                            </select>
                        </div>
                        <div class="form-group">
                            <div class="input-group">
                                <span class="input-group-addon"><i class="glyphicon glyphicon-user"></i></span>
                                <asp:TextBox ID="txtUserName" runat="server" CssClass="form-control" placeholder="Enter Username"></asp:TextBox>
                            </div>
                            <asp:RequiredFieldValidator ErrorMessage="*Required" ID="reqValUserName" runat="server"
                                Display="Dynamic" CssClass="val-text" ControlToValidate="txtUserName"></asp:RequiredFieldValidator>
                        </div>
                        <div class="form-group">
                            <div class="input-group">
                                <span class="input-group-addon"><i class="glyphicon glyphicon-lock"></i></span>
                                <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="Enter Password"></asp:TextBox>

                            </div>
                            <asp:RequiredFieldValidator ErrorMessage="*Required" ID="reqValPassword" runat="server" Display="Dynamic"
                                CssClass="val-text" ControlToValidate="txtPassword"></asp:RequiredFieldValidator>
                        </div>
                        <div class="col-xs-12 padding-0">
                            <div class="col-sm-6 col-xs-6 padding-0">
                                <asp:CheckBox ID="chkRemember" runat="server" Text="Remember Me" CssClass="pull-left" Checked="true" TextAlign="Right" />
                            </div>
                            <div class="col-sm-6 col-xs-6 padding-0">
                                <asp:Button ID="btnLogin" CausesValidation="true" runat="server" Text="Sign In" CssClass="btn btn-primary button-style pull-right" OnClick="btnLogin_Click" />
                            </div>
                        </div>
                        <div>
                            <div class="col-xs-12 padding-0 lnkForgotPassword">
                                <asp:HyperLink ID="lnkForgotPassword" runat="server" CssClass="pull-right" NavigateUrl="ForgotPassword.aspx"><i>Forgot Password ?</i></asp:HyperLink>
                            </div>
                        </div>
                    </asp:Panel>
                </form>
            </div>
                    </div>
            </div> 
        </div>
    </div>
    <!-- End of container-fluid-->

    <!-- jQuery (necessary for Bootstrap's JavaScript plugins) -->
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js"></script>
    <!-- Include all compiled plugins (below), or include individual files as needed -->
    <script src="Scripts/bootstrap.min.js"></script>
    <script type="text/javascript">
        $(document).bind('keypress', 'return', function (e) {
            if (e.which == 13) {
                e.preventDefault(); //stops default action: submitting form
                $(this).blur();
                $('#btnLogin').focus().click();//give your submit an ID
            }
        });
    </script>
</body>
</html>
