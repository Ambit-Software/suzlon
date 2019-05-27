<%@ Page Title="Project Management Application Welcome Screen" Language="C#" MasterPageFile="~/MasterPages/SolarPMS.Master" 
    AutoEventWireup="true" CodeBehind="WelcomeScreen.aspx.cs" Inherits="SolarPMS.WelcomeScreen" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
                        <center><img src="Content/images/suzlon-logo-welcome.png" class="img-responsive" /></center>
                    </div>
                </div>
                <!-- image wrapper-->

                <div class="col-xs-12" style="padding:10px 0px 0px;">
                    <div class="welcome-box">
                        <h3>
                           <center> Solar Project Management<br /> Application Welcomes You !</center>
                        </h3>
                    </div>
                </div>
                <!-- Login box wrapper-->
            </div>
        </div>
        <!-- End of login Wrapper-->

    </div>
</asp:Content>
