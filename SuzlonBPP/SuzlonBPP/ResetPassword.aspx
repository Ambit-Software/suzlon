<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ResetPassword.aspx.cs" Inherits="SuzlonBPP.ResetPassword" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Suzlon BPP - Reset Password</title>
    <link href="Content/css/bootstrap.min.css" rel="stylesheet" />
    <link href="Content/css/global.css" rel="stylesheet" />
    <style>
        .val-text {
            color: red;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="row margin-0">
            <div class="survey-wrapper">
                <div id="divErrorMessage" runat="server" visible="false">
                    <h2>Invalid Url</h2>
                </div>
                <div class="row margin-0" id="divContainer" runat="server" visible="false">
                    <div class="text-center">
                        <asp:Label ID="lblUpdateSuccess" runat="server" Font-Bold="true" ForeColor="Blue"></asp:Label>
                    </div>
                    <div id="divOldPassword" runat="server" class="col-xs-12">
                        <div>
                            <div class="col-xs-2">
                                <label class="col-xs-12 control-label lable-txt padding-0" for="name">Old Password</label>
                            </div>
                            <!-- End of col-lg-1-->

                            <div class="col-xs-10 padding-0">
                                <asp:TextBox ID="txtOldPassword" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
                                <asp:RequiredFieldValidator ErrorMessage="*Rrequired" ID="reqValUserName" runat="server"
                                    Display="Dynamic" CssClass="val-text" ControlToValidate="txtOldPassword"></asp:RequiredFieldValidator>
                            </div>
                            <!-- End of input-group-col-xs-8-->

                        </div>
                        <!-- End of form-->

                    </div>

                    <div class="col-xs-12">

                        <div class="col-xs-2">
                            <label class="col-xs-12 control-label lable-txt padding-0" for="name">New Password</label>
                        </div>
                        <!-- End of col-lg-1-->

                        <div class=" col-xs-10 padding-0">
                            <%--<input type="Password" class="search-query form-control" placeholder="Enter New Password" />--%>
                            <asp:TextBox ID="txtNewPassword" runat="server" CssClass="form-control" TextMode="Password" MaxLength="15"></asp:TextBox>
                            <asp:RequiredFieldValidator ErrorMessage="*Required" ID="RequiredFieldValidator1" runat="server"
                                Display="Dynamic" CssClass="val-text" ControlToValidate="txtNewPassword"></asp:RequiredFieldValidator>
                        </div>
                        <!-- End of input-group-col-xs-8-->

                        <!-- End of form-->

                    </div>

                    <div class="col-xs-12">

                        <div class="col-xs-2">
                            <label class="col-xs-12 control-label lable-txt padding-0" for="name">Confirm Password</label>
                        </div>
                        <!-- End of col-lg-1-->

                        <div class="col-xs-10 padding-0">
                            <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="form-control" TextMode="Password" MaxLength="15"></asp:TextBox>
                            <asp:RequiredFieldValidator ErrorMessage="*Required" ID="RequiredFieldValidator2" runat="server"
                                Display="Dynamic" CssClass="val-text" ControlToValidate="txtConfirmPassword"></asp:RequiredFieldValidator>
                            <asp:CompareValidator ID="cmpValPassword" runat="server" ErrorMessage="Password and confirm password should be same"
                                ControlToCompare="txtNewPassword" ForeColor="Red" ControlToValidate="txtConfirmPassword" CultureInvariantValues="False"></asp:CompareValidator>
                        </div>
                        <!-- End of input-group-col-xs-8-->

                        <!-- End of form-->
                    </div>
                    <div class="col-xs-12 text-center">
                        <div class="submit-btn text-center">
                            <asp:Button ID="btnSubmit" CausesValidation="true" runat="server" Text="Submit" CssClass="btn btn-primary button-style" OnClick="btnSubmit_Click" />
                        </div>
                    </div>
                </div>

            </div>
            <!-- End of Survey wrapper-->
        </div>
        <asp:HiddenField ID="hidUserName" runat="server" />
        <asp:HiddenField ID="hidOldPassword" runat="server" />
    </form>
    <!-- jQuery (necessary for Bootstrap's JavaScript plugins) -->
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js"></script>
    <!-- Include all compiled plugins (below), or include individual files as needed -->
    <script src="../Scripts/bootstrap.min.js"></script>
</body>
</html>
