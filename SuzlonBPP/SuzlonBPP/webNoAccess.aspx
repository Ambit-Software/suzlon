<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="webNoAccess.aspx.cs" Inherits="SuzlonBPP.webNoAccess" %>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <title>Page Access Denied</title>
    <link href="Content/css/bootstrap.min.css" rel="stylesheet">
    <script>
        function goBack() {
            window.history.back();
        }
    </script>
    <style type="text/css">
        body {
            background-color: #EAEDF4;
        }

        h3, h4 {
            margin: 0px;
        }

        .bg-white {
            background-color: #FFF;
            border-radius: 10px;
        }

        .access-txt {
            padding-left: 10px;
            line-height: 24px;
        }

        .wrapper-margin {
            margin-bottom: 20px;
        }

        .go-back-txt {
            padding-left: 10px;
            line-height: 24px;
        }

        .margin-top {
            margin-top: 10%;
        }

        .margin-bot-27 {
            margin-bottom: 27px;
        }

        .padding-15 {
            padding: 15px;
        }

        .glyphicon {
            font-size: 23px;
        }

        .heading {
            line-height: 30px;
        }

        .padding-0 {
            padding: 0px;
        }
    </style>
</head>
<body>
    <div class="col-xs-12 padding-0">
        <div class="col-sm-offset-4 col-sm-4 col-xs-12 bg-white margin-top padding-15">

            <div class="col-sm-12 col-xs-12 wrapper-margin padding-0">
                <span class="pull-left glyphicon glyphicon-lock text-danger"></span>
                <h3 class="text-danger access-txt pull-left">Access Denied</h3>
            </div>

            <div class="col-xs-12 wrapper-margin">
                You do not have access to the page you requested.
            </div>

            <div class="col-sm-12 col-xs-12 wrapper-margin padding-0">
                <span class="pull-left glyphicon glyphicon glyphicon-circle-arrow-left text-primary"></span>
                <h4 class="text-warning go-back-txt pull-left"><a onclick="goBack()" href="#">Go back</a></h4>
            </div>
        </div>
    </div>
    <script src="Scripts/jquery.min.js"></script>
    <script src="Scripts/bootstrap.min.js"></script>
</body>
</html>

