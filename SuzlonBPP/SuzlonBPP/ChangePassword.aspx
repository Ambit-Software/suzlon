<%@ Page Title="Change Password" Language="C#" MasterPageFile="SuzlonBPP.Master" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="SuzlonBPP.ChangePassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="Scripts/bootstrap.min.js"></script>
    <style>
        .submit-btn{
            width:auto !important;
        }
    </style>
    <div class="row margin-0">
        <div class="chng-passwrd-wrapper">
            <div class="row margin-0" id="divContainer" runat="server">
                <div id="divErrorMessage" runat="server" visible="false" class="text-center">
                    <asp:Label ID="lblMessage" runat="server" Font-Bold="true"></asp:Label>
                </div>
                <div id="divOldPassword" runat="server" class="col-xs-12">
                    <div>
                        <div class="col-xs-4">
                            <label class="col-xs-12 control-label lable-txt padding-0" for="name">Old Password</label>
                        </div>
                        <!-- End of col-lg-1-->

                        <div class="col-xs-8 padding-0">
                            <asp:TextBox ID="txtOldPassword" runat="server" CssClass="form-control" TextMode="Password" MaxLength="15"></asp:TextBox>
                            <asp:RequiredFieldValidator ErrorMessage="*Required" ID="reqValUserName" runat="server" ForeColor="Red"
                                Display="Dynamic" CssClass="val-text" ControlToValidate="txtOldPassword"></asp:RequiredFieldValidator>
                        </div>
                        <!-- End of input-group-col-xs-8-->

                    </div>
                    <!-- End of form-->

                </div>

                <div class="col-xs-12">

                    <div class="col-xs-4">
                        <label class="col-xs-12 control-label lable-txt padding-0" for="name">New Password</label>
                    </div>
                    <!-- End of col-lg-1-->

                    <div class=" col-xs-8 padding-0">
                        <%--<input type="Password" class="search-query form-control" placeholder="Enter New Password" />--%>
                        <asp:TextBox ID="txtNewPassword" runat="server" CssClass="form-control" TextMode="Password" MaxLength="15"></asp:TextBox>
                        <asp:RequiredFieldValidator ErrorMessage="*Required" ID="RequiredFieldValidator1" runat="server" ForeColor="Red"
                            Display="Dynamic" CssClass="val-text" ControlToValidate="txtNewPassword"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="regPassword" ControlToValidate="txtNewPassword" runat="server"
                            ForeColor="Red" ErrorMessage="Password criteria doesn't match." Display="Dynamic"
                            ValidationExpression="^(?=.*[!@#$%^&*]).{6,15}$"></asp:RegularExpressionValidator>
                    </div>
                    <!-- End of input-group-col-xs-8-->

                    <!-- End of form-->

                </div>

                <div class="col-xs-12">

                    <div class="col-xs-4">
                        <label class="col-xs-12 control-label lable-txt padding-0" for="name">Confirm Password</label>
                    </div>
                    <!-- End of col-lg-1-->

                    <div class=" col-xs-8 padding-0">
                        <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="form-control" TextMode="Password" MaxLength="15"></asp:TextBox>
                        <asp:RequiredFieldValidator ErrorMessage="*Required" ID="RequiredFieldValidator2" runat="server" ForeColor="Red"
                            Display="Dynamic" CssClass="val-text" ControlToValidate="txtConfirmPassword"></asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="cmpValPassword" runat="server" ErrorMessage="Password and confirm password should be same"
                            ControlToCompare="txtNewPassword" ForeColor="Red" ControlToValidate="txtConfirmPassword" CultureInvariantValues="False"></asp:CompareValidator>
                    </div>
                    <!-- End of input-group-col-xs-8-->

                    <!-- End of form-->
                </div>
                <div class="text-info small">
                    <div class="col-xs-12">Note: Password requires 1 special character (!@#$%^&*) and the length should be between 6-15 characters. 
                    </div>
                </div>
                <div class="margin-10">
                    <center>
                <div class="submit-btn">
                    <%--<a href="#" class="btn btn-primary button-style button">Save</a>--%>
                    <asp:Button ID="btnSubmit" CausesValidation="true" runat="server" Text="Submit" CssClass="btn btn-primary button-style" OnClick="btnSubmit_Click" />
                </div>
                <div class="submit-btn">
                        <asp:Button ID="btnCancel" CausesValidation="false" runat="server" Text="Clear" CssClass="btn btn-primary button-style" />
                </div>
                </center>
                </div>
            </div>

        </div>
    <!-- End of Survey wrapper-->
    </div>
    <asp:HiddenField ID="hidUserName" runat="server" />
    <asp:HiddenField ID="hidOldPassword" runat="server" />
</asp:Content>
