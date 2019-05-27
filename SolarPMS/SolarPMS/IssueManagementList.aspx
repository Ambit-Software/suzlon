<%@ Page Title="Project Management Application Issue Management Summary" Language="C#" MasterPageFile="~/MasterPages/SolarPMS.Master" AutoEventWireup="true" CodeBehind="IssueManagementList.aspx.cs" Inherits="SolarPMS.IssueManagementList" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        .RadButton.rbButton {
        min-height: 34px;
        }
        .badge-important {
    background-color: #d15b47 !important;
}
        .badge-warning {
    background-color: #f89406 !important;
}
        .badge-purple {
    background-color: #9585bf !important;
}
    </style>
   
<%--    <script src="Scripts/jquery.min.js"></script>
    <script src="Scripts/bootstrap.min.js"></script>
   <script type="text/javascript">
      	var $j = jQuery.noConflict();
    </script>--%>


     <telerik:RadAjaxManager ID="RadAjaxManager1" EnablePageHeadUpdate="false" runat="server" OnAjaxRequest="RadAjaxManager1_AjaxRequest">      
        <AjaxSettings>
            
                <telerik:AjaxSetting AjaxControlID="btnSearch">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdAssignToMe" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>  
                    <telerik:AjaxUpdatedControl ControlID="grdRaisedByMe" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>                    
                    <telerik:AjaxUpdatedControl ControlID="grdAllIssue" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl> 
                   
                     <telerik:AjaxUpdatedControl ControlID="lblAllissueCount" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>  
                    <telerik:AjaxUpdatedControl ControlID="lblAssignIssueCount" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>                    
                    <telerik:AjaxUpdatedControl ControlID="lblRaisedByMeCount" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl> 
                    
                                                         
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="grdAssignToMe">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdAssignToMe" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>                   
                        
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="grdRaisedByMe">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdRaisedByMe" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>                    
                   
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="grdAllIssue">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdAllIssue" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>   
                </UpdatedControls>
            </telerik:AjaxSetting>
            
            <telerik:AjaxSetting AjaxControlID="RadAjaxManager1">  
                <UpdatedControls>  
                    <telerik:AjaxUpdatedControl ControlID="grdIssueHistory" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>                    
                </UpdatedControls>  
            </telerik:AjaxSetting>  


            <telerik:AjaxSetting AjaxControlID="drpSearchCategorys">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdAssignToMe" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>    
                    <telerik:AjaxUpdatedControl ControlID="grdRaisedByMe" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>                    
                     <telerik:AjaxUpdatedControl ControlID="grdAllIssue" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>                   
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="grdIssueHistory">
            <UpdatedControls>
            <telerik:AjaxUpdatedControl ControlID="grdIssueHistory" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>                   
                        
            </UpdatedControls>
            </telerik:AjaxSetting>

        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="LoadingPanel1" runat="server" Skin="Default">
    </telerik:RadAjaxLoadingPanel>

       <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript">
            $(document).ready(function () {           
               
                var a = document.getElementById("<%=ProfileName.ClientID%>").value;             
                if (a == 'ShowAllIssue') {
                    $("#lnkAll").show();                    
                }
                else {
                    $("#lnkAll").hide();
                }
            });
         function ShowAssignHistory(id, rowIndex) {
             //alert("Test");
             debugger;
             var ajaxManager = $find("<%= RadAjaxManager1.ClientID %>");

           
             ajaxManager.ajaxRequest("AssignHistory#" + id + "");

             $('#squarespaceModal2').modal();           

         }

        </script>
    </telerik:RadCodeBlock>

   <asp:HiddenField ID="ProfileName" runat="Server" />

    <div class="col-xs-12 padding-0">
        <div class="col-xs-6 heading-big">
                    <h5 class="margin-0 lineheight-30 breath-ctrl">Home / <span style="font-weight: normal !important">Issue Management</span></h5>
                </div>
    </div>
    <div class="clearfix"></div>

    <div class="row margin-0 padding-tb-10">
      <%--<div class="block-wrapper">
           
            <div class="col-sm-10">
                <div class="col-sm-1 padding-0">
                    <label class="control-label lable-txt" for="name">Search</label>
                </div>--%>
                <%-- <div class="col-sm-11 padding-0">
                   
                     <telerik:RadComboBox RenderMode="Lightweight" ID="drpSearchCategorys" runat="server" Height="200" Width="100%" 
                          OnSelectedIndexChanged="drpSearchCategorys_SelectedIndexChanged" AutoPostBack="true"
                                    Skin="Office2010Silver" MarkFirstMatch="true" EnableLoadOnDemand="false"></telerik:RadComboBox>
                </div>

            </div>--%>
            <%--<div class="col-sm-1 padding-0 ">
                <center>
                <asp:Button ID="btnSearch" OnClick="btnSearch_Click" runat="server" Text="Search" CssClass="btn btn-primary"/>
                </center>
            </div>
            
        </div>--%>
            
        <div class="block-table-wrapper">
                <div class="container-fluid padding-0">
                    <div class="widget-content">
                        <div class="row margin-0">
                            <div class="col-md-12 padding-0">
                                <div class="panel with-nav-tabs panel-default border-0">
                                    <div class="panel-heading padding-0 nocolor">
                                        <ul class="nav nav-tabs border-0 col-sm-11">
                                            <li class="active"><a href="#tab1default" data-toggle="tab">Assigned to Me &nbsp;<span class="badge badge-important">
                                            <asp:Label ID="lblAssignIssueCount" runat="server" Text=""></asp:Label>
                                              </span></a></li>
                                            <li><a href="#tab2default" data-toggle="tab">Raised by Me &nbsp;<span class="badge badge-warning">
                                            <asp:Label ID="lblRaisedByMeCount" runat="server" Text=""></asp:Label>
                                           </span></a></li>
                                             <li><a href="#tab3default" data-toggle="tab" id="lnkAll">All Issue &nbsp;<span class="badge badge-purple">
                                                <asp:Label ID="lblAllissueCount" runat="server" Text=""></asp:Label>
                                            </span></a></li>
                                          
                                        </ul>
                                        <ul class="nav nav-tabs border-0 col-sm-1" style="padding-top:5px; padding-right:0px;">
                                            <li class="pull-right">
                                                <telerik:RadButton RenderMode="Lightweight" ID="btnAddNewIssue"  runat="server" Text="Add" OnClick="btnAddNewIssue_Click">
                                                    <Icon PrimaryIconCssClass="rbAdd"></Icon>
                                                </telerik:RadButton>
                                            </li>
                                        </ul>
                                    </div>
                                    <div class="panel-body padding-0">
                                        <div class="tab-content">
                                            <div class="tab-pane fade in active" id="tab1default">
                                                <div class="row margin-0 padding-tb-10"  style="overflow:hidden;overflow-x:scroll;">
                                                <telerik:RadGrid RenderMode="Lightweight" ID="grdAssignToMe" runat="server" ClientIDMode="AutoID"
                                                    AutoGenerateColumns="false" AllowPaging="true" AllowSorting="true" AllowFilteringByColumn="false" PageSize="<%# Convert.ToInt32(SolarPMS.Models.Constants.CONST_GRID_PAGE_SIZE) %>" OnNeedDataSource="grdAssignToMe_NeedDataSource"  OnItemCreated="grdAssignToMe_ItemCreated" OnEditCommand="grdAssignToMe_EditCommand"  OnItemDataBound="grdAssignToMe_ItemDataBound">
                                                    <GroupingSettings CaseSensitive="false" />
                                                    <MasterTableView CommandItemSettings-ShowRefreshButton="false" ClientDataKeyNames="IssueId" EditMode="InPlace" CommandItemDisplay="None" AllowFilteringByColumn="true" ClientIDMode="AutoID">
                                                        <CommandItemSettings ShowRefreshButton="false" ShowExportToExcelButton="false" />
                                                        <Columns>                                                

                                                         <telerik:GridDateTimeColumn DataField="IssueDate" MaxLength="30" HeaderText="Date" 
                                                         DataType="System.DateTime" DataFormatString="{0:dd-MMM-yyyy}">                                                             
                                                         </telerik:GridDateTimeColumn>
                                                          
                                                            <telerik:GridBoundColumn DataField="IssueId" HeaderText="Issue Id" UniqueName="IssueId">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="Description" HeaderText="Description" UniqueName="Description">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="CategoryName" HeaderText="Category" UniqueName="Category">
                                                            </telerik:GridBoundColumn>
                                                          
                                                               <telerik:GridDateTimeColumn DataField="ExpectedClosureDate" MaxLength="30" HeaderText="Expected Closure Date" 
                                                                DataType="System.DateTime" DataFormatString="{0:dd-MMM-yyyy}"> </telerik:GridDateTimeColumn>


                                                            <telerik:GridBoundColumn DataField="UserName" HeaderText="Assigned To" UniqueName="AssignedTo">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="IssueStatus" HeaderText="Status" UniqueName="Status">
                                                            </telerik:GridBoundColumn>
                                                   
                                                              <telerik:GridTemplateColumn UniqueName="TemplateEditColumn" HeaderText="Assigned History">
                                                            <ItemTemplate>
                                                                <asp:HyperLink ID="ViewHistory"  runat="server"  Text="View"></asp:HyperLink>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>                                                
                                                          
                                                   
                                                        <telerik:GridButtonColumn CommandName="Edit" Text="Edit" UniqueName="Edit" HeaderText="Edit"
                                                         ButtonType="ImageButton" ImageUrl="">
                                                     </telerik:GridButtonColumn>

                                                        </Columns>
                                                        <PagerStyle PageSizes="5,10,15" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                                                            PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />


                                                    </MasterTableView>
                                                </telerik:RadGrid>
                                                 </div>                                               
                                            </div>


                                            <div class="tab-pane fade" id="tab2default">
                                                  <div class="row margin-0 padding-10"  style="overflow:hidden;overflow-x:scroll;">
                                                <telerik:RadGrid RenderMode="Lightweight" ID="grdRaisedByMe" runat="server"
                                                    AutoGenerateColumns="false" AllowPaging="true" AllowSorting="true" AllowFilteringByColumn="false" PageSize="<%# Convert.ToInt32(SolarPMS.Models.Constants.CONST_GRID_PAGE_SIZE) %>" OnNeedDataSource="grdRaisedByMe_NeedDataSource" OnItemCreated="grdRaisedByMe_ItemCreated"  OnEditCommand="grdRaisedByMe_EditCommand" OnItemDataBound="grdRaisedByMe_ItemDataBound">
                                                    <GroupingSettings CaseSensitive="false" />
                                                    <MasterTableView CommandItemSettings-ShowRefreshButton="false" ClientDataKeyNames="IssueId" EditMode="InPlace" CommandItemDisplay="None" AllowFilteringByColumn="true">
                                                        <CommandItemSettings ShowRefreshButton="false" ShowExportToExcelButton="false" />
                                                        <Columns>
                                                  
                                                        <telerik:GridDateTimeColumn DataField="IssueDate" MaxLength="30" HeaderText="Date" 
                                                       DataType="System.DateTime" DataFormatString="{0:dd-MMM-yyyy}"> </telerik:GridDateTimeColumn>


                                                            <telerik:GridBoundColumn DataField="IssueId" HeaderText="Issue Id" UniqueName="IssueId">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="Description" HeaderText="Description" UniqueName="Description">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="CategoryName" HeaderText="Category" UniqueName="Category">
                                                            </telerik:GridBoundColumn>
                                                       

                                                            <telerik:GridDateTimeColumn DataField="ExpectedClosureDate" MaxLength="30" HeaderText="Expected Closure Date" 
                                                            DataType="System.DateTime" DataFormatString="{0:dd-MMM-yyyy}"> </telerik:GridDateTimeColumn>

                                                            <telerik:GridBoundColumn DataField="UserName" HeaderText="Assigned To" UniqueName="AssignedTo">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="IssueStatus" HeaderText="Status" UniqueName="Status">
                                                            </telerik:GridBoundColumn>                                      

                                                            
                                                              <telerik:GridTemplateColumn UniqueName="TemplateEditColumn" HeaderText="Assigned History">
                                                            <ItemTemplate>
                                                                <asp:HyperLink ID="ViewHistory" runat="server" Text="View"></asp:HyperLink>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                 
                                                    
                                                        <telerik:GridButtonColumn CommandName="Edit" Text="Edit" UniqueName="Edit" HeaderText="Edit"
                                                         ButtonType="ImageButton" ImageUrl="">
                                                     </telerik:GridButtonColumn>
                                                        </Columns>
                                                        <PagerStyle PageSizes="5,10,15" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                                                            PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />


                                                    </MasterTableView>
                                                </telerik:RadGrid>
                                                   </div>

                                            </div>
                                                   <div class="tab-pane fade" id="tab3default" >
                                                  <div class="row margin-0 padding-10"  style="overflow:hidden;overflow-x:scroll;">
                                                <telerik:RadGrid RenderMode="Lightweight" ID="grdAllIssue" runat="server" OnEditCommand="grdAllIssue_EditCommand" 
                                                    AutoGenerateColumns="false" AllowPaging="true" AllowSorting="true" AllowFilteringByColumn="false" PageSize="<%# Convert.ToInt32(SolarPMS.Models.Constants.CONST_GRID_PAGE_SIZE) %>" OnItemCreated="grdAllIssue_ItemCreated" OnNeedDataSource="grdAllIssue_NeedDataSource" OnItemDataBound="grdAllIssue_ItemDataBound" >
                                                    <GroupingSettings CaseSensitive="false" />
                                                    <MasterTableView CommandItemSettings-ShowRefreshButton="false" ClientDataKeyNames="IssueId" EditMode="InPlace" CommandItemDisplay="None" AllowFilteringByColumn="true">
                                                        <CommandItemSettings ShowRefreshButton="false" ShowExportToExcelButton="false" />
                                                        <Columns>
                                                   
                                                         <telerik:GridDateTimeColumn DataField="IssueDate" MaxLength="30" HeaderText="Date" 
                                                       DataType="System.DateTime" DataFormatString="{0:dd-MMM-yyyy}"> </telerik:GridDateTimeColumn>


                                                            <telerik:GridBoundColumn DataField="IssueId" HeaderText="Issue Id" UniqueName="IssueId">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="Description" HeaderText="Description" UniqueName="Description">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="CategoryName" HeaderText="Category" UniqueName="Category">
                                                            </telerik:GridBoundColumn>
                                                         
                                                            <telerik:GridDateTimeColumn DataField="ExpectedClosureDate" MaxLength="30" HeaderText="Expected Closure Date" 
                                                            DataType="System.DateTime" DataFormatString="{0:dd-MMM-yyyy}"> </telerik:GridDateTimeColumn>


                                                            <telerik:GridBoundColumn DataField="UserName" HeaderText="Assigned To" UniqueName="AssignedTo">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="IssueStatus" HeaderText="Status" UniqueName="Status">
                                                            </telerik:GridBoundColumn>
                                                            
                                                              <telerik:GridTemplateColumn UniqueName="TemplateEditColumn" HeaderText="Assigned History">
                                                            <ItemTemplate>
                                                                <asp:HyperLink ID="ViewHistory" runat="server" Text="View"></asp:HyperLink>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                 
                                                    
                                                        <telerik:GridButtonColumn CommandName="Edit" Text="Edit" UniqueName="Edit" HeaderText="Edit"
                                                         ButtonType="ImageButton" ImageUrl="">
                                                     </telerik:GridButtonColumn>
                                                        </Columns>
                                                        <PagerStyle PageSizes="5,10,15" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                                                            PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />


                                                    </MasterTableView>
                                                </telerik:RadGrid>
                                                   </div>

                                            </div>


                                        </div>
                                    </div>
                                </div>
                     
                                <div class="modal fade" id="squarespaceModal2" tabindex="-1" role="dialog" aria-labelledby="modalLabel" aria-hidden="true">
                                    <div class="modal-dialog1">
                                        <div class="modal-content">
                                            <div class="modal-header modal-header-resize">
                                                <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">×</span><span class="sr-only">Close</span></button>
                                                <h4 class="modal-title text-primary" id="lineModalLabel">Assigned History</h4>
                                            </div>
                                            <div class="modal-body">
                                          <telerik:RadGrid RenderMode="Lightweight" ID="grdIssueHistory" runat="server" OnNeedDataSource="grdIssueHistory_NeedDataSource"
                                    AutoGenerateColumns="false" AllowPaging="true" AllowSorting="true" AllowFilteringByColumn="false" PageSize="<%# Convert.ToInt32(SolarPMS.Models.Constants.CONST_GRID_PAGE_SIZE) %>"  >
                                    <GroupingSettings CaseSensitive="false" />
                                    <MasterTableView CommandItemSettings-ShowRefreshButton="false" ClientDataKeyNames="" EditMode="InPlace" CommandItemDisplay="None" AllowFilteringByColumn="false">
                                        <CommandItemSettings ShowRefreshButton="false" ShowExportToExcelButton="false" />
                                        <Columns>
                                            <telerik:GridBoundColumn DataField="name" HeaderText="User Name" UniqueName="UserName">
                                            </telerik:GridBoundColumn>
                                          <%--  <telerik:GridBoundColumn DataField="assignedDate" HeaderText="Date" UniqueName="Date" >
                                            </telerik:GridBoundColumn> --%>
                                            <telerik:GridDateTimeColumn DataField="assignedDate" HeaderText="Date" UniqueName="Date" DataFormatString="{0:MM/dd/yyyy}"></telerik:GridDateTimeColumn>                                           
                                        </Columns>
                                        <PagerStyle PageSizes="5,10,15" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                                            PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />


                                    </MasterTableView>
                                </telerik:RadGrid>

                                            </div>
                                            <!--End of Scroll-->

                                        </div>

                                    </div>
                                </div>
                               


                            </div>
                            
                        </div>
                       
                    </div>
                   

                </div>
              
            </div>
          
       
    </div>

    

</asp:Content>
