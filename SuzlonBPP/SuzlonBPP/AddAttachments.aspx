<%@ Page Title="Attachment" Language="C#" MasterPageFile="~/Blank.Master" AutoEventWireup="true" CodeBehind="AddAttachments.aspx.cs" Async="true" Inherits="SuzlonBPP.AddAttachments" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script lang="javascript" type="text/javascript">

        function ClosePopUp() {
            GetRadWindow().Close();
        }

        function GetRadWindow() {
            var oWindow = null;
            if (window.radWindow)
                oWindow = window.RadWindow; //Will work in Moz in all cases, including clasic dialog      
            else if (window.frameElement.radWindow)
                oWindow = window.frameElement.radWindow; //IE (and Moz as well)      
            return oWindow;
        }

        function DeleteFiles() {
            debugger;
            var upload = $find("<%= RadAsyncUpload.ClientID %>");
                            var inputs = upload.getUploadedFiles().length;
                            for (i = 0; i <= inputs; i++) {
                                //upload.deleteFileInputAt(i);
                                upload.deleteFileInputAt(0);
                            }
                        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        
        .RadUpload .ruFakeInput {
            min-height: 29px !important;
            border-radius: 4px !important;
        }
         .RadUpload_Default .ruSelectWrap .ruButton {
    background-color: #00988c !important;
    background-image: none !important;
    min-height: 29px !important;
}
        .RadUpload .ruSelectWrap {
            display: inline-block;
            float: left;
        }
        .table-scroll{
            overflow-x:scroll !important;
        }

        .RadWindow .rwIcon {
            display: none !important;
        }
        
        .RadWindow .rwTitleWrapper .rwTitle {
            margin: 2px 0 0 -21px !important;
            font-weight: bold !important;
        }
    </style>

    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="buttonSubmit">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdAttachments" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="RadAsyncUpload1" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="radMessage" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="grdAttachments">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdAttachments" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="LoadingPanel1" runat="server" Skin="Default"></telerik:RadAjaxLoadingPanel>
    <div id="add-attachment" class="filter-panel collapse in" aria-expanded="true">
        <div class="panel panel-default">
            <div class="panel-body ">
                <table class="table table-striped table-bordered"  runat="server" id="tbAttachment">
                    <thead>
                        <tr>
                            <th class="text-center valign" runat="server" id="lblDType">Document Text</th>
                            <th class="text-center valign">Attachment</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr class="text-center" runat="server" id="DDDType">
                            <td class="valign">
                                <%--<telerik:RadDropDownList RenderMode="Lightweight" ID="cmbDocTypes" runat="server">
                                    <Items>
                                        <telerik:DropDownListItem Value="Cash" Text="Cash" runat="server" />
                                        <telerik:DropDownListItem Value="Cheque" Text="Cheque" runat="server" />
                                        <telerik:DropDownListItem Value="Draft" Text="Draft" runat="server" />
                                    </Items>
                                </telerik:RadDropDownList>--%>
                                <asp:TextBox runat="server" RenderMode="Lightweight" ID="txtDType"></asp:TextBox>
                            </td>

                            <td class="valign">
                                    <telerik:RadAsyncUpload RenderMode="Lightweight" ID="RadAsyncUpload" runat="server" Width="100%"
                                        OverwriteExistingFiles="true" >
                                        <Localization Select="Browse" />
                                    </telerik:RadAsyncUpload>
                                

                            </td>
                        </tr>

                    </tbody>
                </table>
                <div class="col-xs-12 padding-0">
                    <div class="button-wrapper pull-left">
                        <ul>
                            <li>
                                <asp:LinkButton ID="buttonSubmit" runat="server" OnClick="buttonSubmit_Click" CssClass="btn btn-grey">Save</asp:LinkButton>

                            </li>

                            <li>
                            <asp:LinkButton ID="buttomSaveClose" runat="server" OnClick="buttonSubmit_Click" OnClientClick="ClosePopUp()" CssClass="btn btn-grey">Save and Close</asp:LinkButton>
                            </li>
                            
                            <li>

                                <asp:LinkButton ID="btnCancel" runat="server" OnClientClick="ClosePopUp()" OnClick="btnCancel_Click" class="btn btn-grey">Close</asp:LinkButton>
                            </li>
                        </ul>
                        <ul>
                            <li>
                                <telerik:RadNotification RenderMode="Lightweight" Position="Center" runat="server" ID="radMessage" VisibleOnPageLoad="false"
                                    Width="350px" AutoCloseDelay="0" TitleIcon="none" ContentIcon="none">
                                </telerik:RadNotification>
                            </li>
                        </ul>
                    </div>
                </div>
                <div class="col-xs-12 padding-0">
                    <div class="table-scroll">
                        <telerik:RadGrid RenderMode="Lightweight" ID="grdAttachments" runat="server" OnNeedDataSource="grdAttachments_NeedDataSource"
                            OnItemDataBound="grdAttachments_ItemDataBound" AutoGenerateColumns="false" OnItemCommand="grdAttachments_ItemCommand"
                            AllowPaging="true" AllowSorting="true" AllowFilteringByColumn="true">
                            <GroupingSettings CaseSensitive="false" />
                            <MasterTableView CommandItemDisplay="None" DataKeyNames="FileName,FileuploadId,createdBy" EnableViewState="true" AllowPaging="true" AutoGenerateColumns="false"
                                EnableNoRecordsTemplate="true"  AllowFilteringByColumn="false">
                                <PagerStyle AlwaysVisible="true" />
                                <NoRecordsTemplate>
                                    <table width="100%" border="0" cellpadding="20" cellspacing="20">
                                        <tr>
                                            <td align="center">No records to display.
                                            </td>
                                        </tr>
                                    </table>
                                </NoRecordsTemplate>
                                <CommandItemSettings ShowRefreshButton="false" />
                                <Columns>
                                    
                                    <telerik:GridBoundColumn DataField="FileName" HeaderText="FileName" UniqueName="FileName" DataType="System.String" Display="false" />
                                     <telerik:GridBoundColumn DataField="createdBy" HeaderText="CreatedBy" UniqueName="createdBy" DataType="System.String" ReadOnly="true" Visible="false" />
                                    <telerik:GridBoundColumn DataField="DisplayName" HeaderText="File Name" UniqueName="DisplayName" DataType="System.String" ReadOnly="true" />
                                    <telerik:GridBoundColumn DataField="DocumentType" HeaderText="Document Text" UniqueName="DocumentType" DataType="System.String" ReadOnly="true" />
                                    <telerik:GridBoundColumn DataField="CreatedOn" HeaderText="Upload Date" UniqueName="DocumentType" DataType="System.String" ReadOnly="true" />
                                    <telerik:GridBoundColumn DataField="name" HeaderText="Upload By" UniqueName="UploadBy" DataType="System.String" ReadOnly="true" />
                                    <telerik:GridTemplateColumn UniqueName="TemplateAttachment" HeaderText="View" HeaderStyle-Width="5%">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="lkbtnView" runat="server" Text="View" CssClass="btn btn-grey gridHyperlinks"></asp:HyperLink>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="DeleteAttachment" HeaderText="Delete" HeaderStyle-Width="5%">
                                        <ItemTemplate>
                                            <asp:Button ID="btn" runat="server" CommandName="Delete" Text="Delete" CssClass="btn btn-grey gridHyperlinks"
                                                OnClientClick="javascript:if(!confirm('Are you sure you want to delete an Attachment?')){return false;}"></asp:Button>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>
                                <PagerStyle PageSizes="5" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                                    PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />
                            </MasterTableView>
                        </telerik:RadGrid>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
