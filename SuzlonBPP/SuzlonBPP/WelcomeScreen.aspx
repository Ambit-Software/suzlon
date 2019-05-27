<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="SuzlonBPP.Master" CodeBehind="WelcomeScreen.aspx.cs" Inherits="SuzlonBPP.WelcomeScreen" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="Scripts/bootstrap.min.js"></script>
    <style>
        .navbar {
            margin-bottom: 0px !important;
        }
    </style>
    <div class="container-fluid padding-0 welcome-bg">
        <div class="welcome-wrapper">
            <div class="col-xs-12 welcome-bg-clr padding-0 border-rad ">
                <div class="col-xs-12">
                    <div class="img-wrapper">
                        <img src="Content/images/suzlon-logo-welcome.jpg" class="img-responsive"/>
                    </div>
                </div>
                <!-- image wrapper-->

                <div class="col-xs-12" style="padding: 10px 0px 0px;">
                    <div class="welcome-box">
                        <h3 style="margin-top: -75px; color: #fff; font-weight:bold">
                            <center> Suzlon Cash & Bank Application Welcomes You !</center>
                        </h3>
                    </div>
                </div>
                <!-- Login box wrapper-->
            </div>
        </div>
        <!-- End of login Wrapper-->

    </div>
</asp:Content>
