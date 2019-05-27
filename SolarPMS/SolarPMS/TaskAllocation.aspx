<%@ Page Title="Task Allocation" Language="C#" MasterPageFile="~/MasterPages/SolarPMS.Master" AutoEventWireup="true" CodeBehind="TaskAllocation.aspx.cs"
    EnableEventValidation="true" Inherits="SolarPMS.TaskAllocation" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>      
        <asp:UpdatePanel runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional" ID="upnlMain">
            <ContentTemplate>
                <div class="col-xs-12 padding-0">
                    <div class="col-xs-12 heading-big">
                        <h5 class="margin-0 lineheight-30 breath-ctrl">Home / Administration / <span style="font-weight: normal !important">Allocate Task</span></h5>
                    </div>                    
                </div>
                <div class="row margin-0">
                    <div class="block-wrapper">
                        <div class="col-lg-12 padding-0">
                            <div class="col-sm-11 col-xs-12 padding-0">
                                <div class="col-sm-4 padding-0">
                                    <div class=" col-xs-10 ">
                                        <telerik:RadComboBox RenderMode="Lightweight" ID="ddlSapSite" runat="server" Height="200" Width="305" AutoPostBack="true"
                                             Skin="Office2010Silver" Filter="Contains" MarkFirstMatch="true" EnableLoadOnDemand="false" OnSelectedIndexChanged="ddlSapSite_SelectedIndexChanged"
                                            Label="Site: ">
                                        </telerik:RadComboBox>
                                        <asp:RequiredFieldValidator ControlToValidate="ddlSapSite" ErrorMessage="*Required" ForeColor="Red" runat="server" ValidationGroup="Search"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="col-sm-4 padding-0">
                                    <div class=" col-xs-9">
                                         <telerik:RadComboBox RenderMode="Lightweight" ID="ddlSapProjects" runat="server" Height="200" Width="305" EmptyMessage=""
                                             Skin="Office2010Silver" Filter="Contains" ValidationGroup="Search" MarkFirstMatch="true" EnableLoadOnDemand="false" Label="Project: ">
                                        </telerik:RadComboBox>
                                        <asp:RequiredFieldValidator ValidationGroup="Search" ControlToValidate="ddlSapProjects" ErrorMessage="*Required" ForeColor="Red" runat="server"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="col-sm-4 col-xs-12 padding-0">
                                    <div class="col-xs-10">
                                        <telerik:RadComboBox RenderMode="Lightweight" ID="ddlUser" runat="server" Height="200" Width="305" AutoPostBack="true" 
                                             Skin="Office2010Silver" ValidationGroup="Search" MarkFirstMatch="true" EnableLoadOnDemand="false" Label="User: " 
                                            Filter="Contains" OnSelectedIndexChanged="ddlUser_SelectedIndexChanged">
                                        </telerik:RadComboBox>
                                        <asp:RequiredFieldValidator ValidationGroup="Search" ControlToValidate="ddlUser" ErrorMessage="*Required" ForeColor="Red" runat="server"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                            </div>

                            <div class="col-sm-1">
                                <div class="input-group-btn">
                                    <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" CssClass="btn btn-cust btn-primary" ValidationGroup="Search" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <telerik:RadNotification RenderMode="Lightweight" ID="radNotificationMessage" runat="server" VisibleOnPageLoad="true" Position="Center"
                    Width="330" Height="80" Animation="Fade" EnableRoundedCorners="true" EnableShadow="true" Visible="false" AnimationDuration="500"
                    Title="Success" Text="Task allocated/removed successfully." TitleIcon="none" ContentIcon="none"
                    Style="z-index: 100000">
                </telerik:RadNotification>
                <div class="padding-lr-10">
                    <telerik:RadGrid RenderMode="Lightweight" ID="gridTaskAllocation" Visible="false" runat="server" ShowStatusBar="true" AutoGenerateColumns="False"
                        AllowSorting="True" OnItemDataBound="gridTaskAllocation_ItemDataBound"
                        OnDetailTableDataBind="gridTaskAllocation_DetailTableDataBind">
                        <PagerStyle Mode="NumericPages"></PagerStyle>
                        <ClientSettings>
                            <Selecting AllowRowSelect="false" UseClientSelectColumnOnly="false" />
                        </ClientSettings>
                        <MasterTableView CssClass="area" DataKeyNames="Id,IsSelected,UniqueId" AllowMultiColumnSorting="True" HierarchyLoadMode="Conditional">
                            <DetailTables>
                                <telerik:GridTableView CssClass="network" DataKeyNames="Id,IsSelected,UniqueId" Name="Network" Width="100%" HierarchyLoadMode="Conditional">
                                    <DetailTables>
                                        <telerik:GridTableView ClientIDMode="Static" CssClass="activity" DataKeyNames="Id,IsSelected,UniqueId" Name="Activity" Width="100%" HierarchyLoadMode="Conditional">
                                            <DetailTables>
                                                <telerik:GridTableView CssClass="subactivity" DataKeyNames="Id,IsSelected,UniqueId" Name="SubActivity" Width="100%" HierarchyLoadMode="Conditional">
                                                    <Columns>
                                                        <telerik:GridTemplateColumn DataField="IsSelected" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="100px">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkSelect" CssClass="chksubactivity" runat="server" onclick="checkUncheckHeader(this, 'subactivity')" />
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridBoundColumn SortExpression="Description" HeaderText="Sub Activity Name" HeaderButtonType="LinkButton"
                                                            DataField="Description" UniqueName="Name">
                                                        </telerik:GridBoundColumn>
                                                    </Columns>
                                                </telerik:GridTableView>
                                            </DetailTables>
                                            <Columns>
                                                <telerik:GridTemplateColumn DataField="IsSelected" ItemStyle-Width="50px">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkSelect" CssClass="chkactivity" runat="server" onclick="checkUncheckHeader(this, 'activity')" />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridBoundColumn SortExpression="Description" HeaderText="Activity Name" HeaderButtonType="LinkButton"
                                                    DataField="Description" UniqueName="ActivityName">
                                                </telerik:GridBoundColumn>
                                            </Columns>
                                        </telerik:GridTableView>
                                    </DetailTables>
                                    <Columns>
                                        <telerik:GridTemplateColumn DataField="IsSelected" ItemStyle-Width="50px">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkSelect" runat="server" CssClass="chkNetwork" onclick="checkUncheckHeader(this, 'network')" />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridBoundColumn SortExpression="Description" HeaderText="Network" HeaderButtonType="LinkButton"
                                            DataField="Description">
                                        </telerik:GridBoundColumn>
                                    </Columns>
                                </telerik:GridTableView>
                            </DetailTables>
                            <Columns>
                                <telerik:GridTemplateColumn DataField="IsSelected" HeaderStyle-Width="50px" ItemStyle-Width="50px">
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="chkAll" runat="server" onclick="checkAllRows(this);" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkSelect" runat="server" onclick="checkAllChildGridCheckbox(this);" AutoPostBack="true" CssClass="chkarea"/>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridBoundColumn SortExpression="Description" HeaderText="Area" HeaderButtonType="LinkButton"
                                    DataField="Description">
                                </telerik:GridBoundColumn>
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                </div>
                <div class="task-allocation-details padding-0" style="background-color: transparent !important;">
                    <div class="container-fluid mxhight">
                        <div class="row margin-0">
                            <div class="col-md-12">
                            </div>
                        </div>
                    </div>
                    <div id="divButtons" runat="server" class="col-lg-12 text-center" visible="false">
                        <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Save/Update" CssClass="btn btn-primary button" />
                        <asp:Button ID="btnCopyPopup" runat="server" Text="Copy" OnClick="btnCopyPopup_Click" CssClass="btn btn-primary button" />
                    </div>
                </div>
                <asp:HiddenField runat="server" Value="New" ID="hidEditMode" />
               
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnCopyTask" EventName="Click" />                
            </Triggers>
        </asp:UpdatePanel>

        <asp:Panel runat="server" ID="panel1" CssClass="modal fade" tabindex="-1" role="dialog" aria-labelledby="modalLabel" aria-hidden="true">
        <asp:UpdatePanel runat="server" ID="upnl2" UpdateMode="Conditional">
            <ContentTemplate>
                 <div class="" id="squarespaceModal">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header modal-header-resize">
                        <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">×</span><span class="sr-only">Close</span></button>
                        <h4 class="modal-title text-primary" id="lineModalLabel">Copy To</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group">
                            <div class="row margin-0">
                                <div class="col-lg-12 text-center">
                                    <asp:CustomValidator ID="CustomValidator1" runat="server" ClientValidationFunction="validateUser"
                                        ErrorMessage="Please select at least one user." ValidationGroup="Copy" ForeColor="Red" EnableClientScript="true" ValidateEmptyText="True"></asp:CustomValidator>
                                </div>
                            </div>
                            <div class="row margin-0">
                                <div class="col-lg-6">
                                    <label id="lblStateheadProfile" for="StateProjectHead" style="line-height: 32px;" runat="server">Select State Project Head</label>
                                </div>
                                <div class="col-lg-6">
                                    <telerik:RadComboBox RenderMode="Lightweight" ID="cmbProjectHead" runat="server" Height="200" Width="250" ValidationGroup="Copy"
                                        CausesValidation="true" Filter="Contains" CheckBoxes="true" EmptyMessage="Select" MarkFirstMatch="true" EnableLoadOnDemand="false" 
                                        Skin="Office2010Silver">
                                    </telerik:RadComboBox>
                                </div>
                            </div>
                            <div class="row margin-0" id="divManagerProfile">
                                <div class="col-lg-6">
                                    <label id="lblManagerProfile" for="cmbManager" style="line-height: 32px;"></label>
                                </div>
                                <div class="col-lg-6">
                                    <telerik:RadComboBox RenderMode="Lightweight" ID="cmbManager" runat="server" Height="200" Width="250" ValidationGroup="Copy"
                                        CheckBoxes="true" MarkFirstMatch="true" EnableLoadOnDemand="false" Filter="Contains" EmptyMessage="Select" Skin="Office2010Silver">
                                    </telerik:RadComboBox>
                                </div>
                            </div>
                            <div class="row margin-0" id="divUserProfile">
                                <div class="col-lg-6">
                                    <label id="lblUserProfile" for="cmbUser" style="line-height: 32px;"></label>
                                </div>
                                <div class="col-lg-6">
                                    <telerik:RadComboBox RenderMode="Lightweight" ID="cmbUser" runat="server" Height="200" Width="250" ValidationGroup="Copy"
                                        EnableLoadOnDemand="false" CheckBoxes="true" Filter="Contains" EmptyMessage="Select" MarkFirstMatch="true">
                                    </telerik:RadComboBox>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnCopyTask" runat="server" CausesValidation="true" Text="Copy"
                            CssClass="btn btn-primary button" ValidationGroup="Copy" OnClick="btnCopyTask_Click" />
                        <button type="button" class="btn btn-primary button" data-dismiss="modal">Cancel</button>

                    </div>
                </div>
            </div>
        </div>
            </ContentTemplate>
        </asp:UpdatePanel>
            </asp:Panel>
    </div>

    <!-- End of popup button-->

    <script type="text/javascript">

        function showPopup() {
            $('#squarespaceModal').modal('show');
        }

        function CloseModal() {
            $('#squarespaceModal').modal('hide');
            return true;
        }

        function validateUser(oSrc, args) {
            var comboUser = $find('<%= cmbUser.ClientID %>');
            var comboManager = $find('<%= cmbManager.ClientID %>');
            var comboHead = $find('<%= cmbProjectHead.ClientID %>');

            var userCount = 0;
            var managerCount = 0; 
            var headCount = 0;
            if (comboUser != null && comboUser.get_checkedItems().length > 0)
                userCount = comboUser.get_checkedItems().length;
            if (comboManager != null && comboManager.get_checkedItems().length > 0)
                managerCount = comboManager.get_checkedItems().length;
            if (comboHead != null && comboHead.get_checkedItems().length > 0)
                headCount = comboHead.get_checkedItems().length;
            console.log(userCount + " " + comboManager + " " + headCount)

            if (userCount == 0 && managerCount == 0 && headCount == 0) {
                args.IsValid = false;
                return false;
            }
            else {
                args.IsValid = true;
                return true;
            }
        }

        var gridToCheckAll = '<%= gridTaskAllocation.ClientID %>';
        function checkAllRows(sender) {
            var checked = sender.checked;
            var container = document.getElementById(gridToCheckAll);
            var checkboxes = container.getElementsByTagName('input');
            for (var i = 0, l = checkboxes.length; i < l; i++) {
                if (checkboxes[i] != sender && !checkboxes[i].disabled)
                    checkboxes[i].checked = checked;
            }
        }

        function checkAllChildGridCheckbox(sender) {
            var checked = $(sender)[0].checked
            $(sender).closest("tr").next().find("table input[type='checkbox']")
                .each(
               function () {
                   this.checked = checked;
               }
             );
        }

        function checkUncheckHeader(sender, type) {
            var parentContainer = $(sender).parents('table:first').attr('id');
            $("#" + parentContainer + " input[type=checkbox]")
            var parent1 = $("#" + parentContainer).closest("table").parents("tr:first").prev("tr");
            var parent2 = parent1.parents("table:first").parents("tr:first").prev();
            var parent3 = parent2.parents("table:first").parents("tr").prev();

            if (!$(sender).is(':checked')) {
                if (type == 'subactivity') {
                    var subactivity = $("#" + parentContainer).find("span.chksubactivity input[type='checkbox']:checked");
                    if (subactivity.length == 0) {
                        parent1.find("input[type=checkbox]")[0].checked = false;
                    }
                    else
                        parent1.find("input[type=checkbox]")[0].checked = true;

                    var activity = $("#" + parentContainer).parents('table.activity').find("span.chkactivity input[type='checkbox']:checked");
                    
                    if (activity.length == 0)
                        parent2.find("input[type=checkbox]")[0].checked = false;
                    else
                        parent2.find("input[type=checkbox]")[0].checked = true;

                    var network = $("#" + parentContainer).parents('table.network').find("span.chkNetwork input[type='checkbox']:checked")
                    
                    if (network.length == 0)
                        parent3.find("input[type=checkbox]")[0].checked = false;
                    else
                        parent3.find("input[type=checkbox]")[0].checked = true;
                }

                if (type == 'activity') {
                    var activity = $("#" + parentContainer).find("span.chkactivity input[type='checkbox']:checked");
                    
                    if (activity.length == 0)
                        parent1.find("input[type=checkbox]")[0].checked = false;
                    else
                        parent1.find("input[type=checkbox]")[0].checked = true;

                    var network = $("#" + parentContainer).parents('table.network').find("span.chkNetwork input[type='checkbox']:checked")

                    if (network.length == 0)
                        parent2.find("input[type=checkbox]")[0].checked = false;
                    else
                        parent2.find("input[type=checkbox]")[0].checked = true;
                }

                if (type == 'network') {
                    var network = $("#" + parentContainer).find("span.chkNetwork input[type='checkbox']:checked")
                    console.log($("#" + parentContainer).find("span.chkNetwork input[type='checkbox']:checked"));
                    if (network.length == 0)
                        parent1.find("input[type=checkbox]")[0].checked = false;
                    else
                        parent1.find("input[type=checkbox]")[0].checked = true;
                }
                
            }
            else {
                switch (type) {
                    case 'subactivity':
                        parent1.find("input[type=checkbox]")[0].checked = true;
                        parent2.find("input[type=checkbox]")[0].checked = true;
                        parent3.find("input[type=checkbox]")[0].checked = true;
                        break;

                    case 'activity':
                        parent1.find("input[type=checkbox]")[0].checked = true;
                        parent2.find("input[type=checkbox]")[0].checked = true;
                        break
                    case 'network':
                        parent1.find("input[type=checkbox]")[0].checked = true;
                        break;
                }
            }

            var checked = $(sender)[0].checked;
            $(sender).parents('tr').next().find('table.rgDetailTable').find("input[type='checkbox']").each(function () {
                this.checked = checked;
            });
        }
    </script>
</asp:Content>
