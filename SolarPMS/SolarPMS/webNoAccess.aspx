<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/SolarPMS.Master" AutoEventWireup="true" CodeBehind="webNoAccess.aspx.cs" Inherits="SolarPMS.webNoAccess" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="col-xs-12 padding-0">
            <div class="col-sm-offset-4 col-sm-4 col-xs-12 bg-white margin-top padding-15">

                <div class="col-sm-12 col-xs-12 wrapper-margin padding-0">
                    <span class="pull-left glyphicon glyphicon-lock text-danger"></span>
                    <h3 class="text-danger access-txt pull-left">Access Denied</h3>
                </div>

                <div class="col-xs-12 wrapper-margin">
                    You do not have access to the page you requested.
                </div>

                <%--<div class="col-sm-12 col-xs-12 wrapper-margin padding-0">
                    <span class="pull-left glyphicon glyphicon glyphicon-circle-arrow-left text-primary"></span>
                    <h4 class="text-warning go-back-txt pull-left">
                        <asp:LinkButton runat="server" ID="lnkGoBack" Text="Go back"></asp:LinkButton>
                    </h4>
                </div>--%>
            </div>
        </div>
     <script>
        function goBack() {
            window.history.back();
        }
    </script>
</asp:Content>
