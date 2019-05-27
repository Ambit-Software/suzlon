<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TestUpload.aspx.cs" Inherits="SolarPMS.Admin.TestUpload" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
        <script src="../Scripts/Page/SurveyMaster.js"></script>
</head>
<body>
      
    <form id="form1" runat="server">
     
    <div>   
        <asp:ScriptManager ID="ScriptManager1" runat="server">  
        </asp:ScriptManager> 
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" EnableAJAX="true">  
        </telerik:RadAjaxManager> 
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Height="75px" 
        Width="75px">  
        <asp:Image ID="Image1" runat="server" AlternateText="Loading..." ImageUrl="~/RadControls/Ajax/Skins/Default/Loading.gif" /> 
        </telerik:RadAjaxLoadingPanel> 

       <telerik:RadAsyncUpload runat="server" ID="BtnImport1" HideFileInput="false" OnClientFileUploaded="callAjaxRequest"
                    OnFileUploaded="BtnImport1_FileUploaded" TargetFolder="~/Upload/Survey/" AllowedFileExtensions=".xlsx,.xls"
                    MaxFileInputsCount="1">
                    <Localization Select="Import" />
                </telerik:RadAsyncUpload>
    </div>
    </form>
</body>
</html>
