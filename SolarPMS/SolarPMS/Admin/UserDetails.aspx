<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/SolarPMS.Master" AutoEventWireup="true" CodeBehind="UserDetails.aspx.cs" 
    EnableEventValidation="false" Inherits="SolarPMS.Admin.UserDetails" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid padding-0">
        <div class="row margin-0">
            <div class="block-wrapper">
                <form role="form">
                    <div class="col-lg-1 padding-0 margin-10">
                        <label class="control-label lable-txt" for="name">User Name</label>
                    </div>
                    <!-- End of col-lg-1-->
                    <div class=" col-xs-10 margin-10">
                        <select class="form-control">
                            <option>Search User Name</option>
                            <option>Shital Kumar</option>
                            <option>Amit Kumar</option>
                            <option>Giridhar</option>
                            <option>Atmaram</option>
                            <option>Prashant</option>
                        </select>
                    </div>
                    <!-- End of input-group-col-xs-8-->
                    <div class="col-lg-1 margin-10">

                        <a href="user-detail-brif.html">
                            <span class=" glyphicon glyphicon-plus-sign text-primary plus-glymphicon"></span>
                        </a>
                    </div>
                    <!-- End of col-lg-1-->
                </form>
                <!-- End of form-->

                <div class="block-table-wrapper">
                    <div class="container-fluid padding-0">
                        <div class="widget-content">

                            <table class="table table-striped table-bordered">
                                <thead>
                                    <tr>
                                        <th>User Name</th>
                                        <th>Employee ID</th>
                                        <th>Location</th>
                                        <th>Email ID</th>
                                        <th>Mobile No</th>
                                        <th>Authentication</th>
                                        <th>Status</th>

                                    </tr>
                                </thead>
                                <tbody>
                                    <tr>
                                        <td>Name 1</td>
                                        <td>001</td>
                                        <td>Pune</td>
                                        <td>abc@email.com</td>
                                        <td>1234567890</td>
                                        <td>AD</td>
                                        <td>Enabled</td>
                                    </tr>
                                    <tr>
                                        <td>Name 2</td>
                                        <td>002</td>
                                        <td>Pune</td>
                                        <td>def@email.com</td>
                                        <td>1234567891</td>
                                        <td>Database</td>
                                        <td>Disabled</td>
                                    </tr>
                                    <tr>
                                        <td>Name 3</td>
                                        <td>003</td>
                                        <td>Pune</td>
                                        <td>xyz@email.com</td>
                                        <td>1234567892</td>
                                        <td>AD</td>
                                        <td>Enabled</td>
                                    </tr>

                                    <tr>
                                        <td>Name 4</td>
                                        <td>004</td>
                                        <td>Pune</td>
                                        <td>def@email.com</td>
                                        <td>1234567891</td>
                                        <td>Database</td>
                                        <td>Disabled</td>
                                    </tr>
                                    <tr>
                                        <td>Name 5</td>
                                        <td>005</td>
                                        <td>Pune</td>
                                        <td>xyz@email.com</td>
                                        <td>1234567892</td>
                                        <td>AD</td>
                                        <td>Enabled</td>
                                    </tr>
                                    <tr>
                                        <td>Name 6</td>
                                        <td>006</td>
                                        <td>Pune</td>
                                        <td>abc@email.com</td>
                                        <td>1234567890</td>
                                        <td>AD</td>
                                        <td>Enabled</td>
                                    </tr>
                                    <tr>
                                        <td>Name 7</td>
                                        <td>007</td>
                                        <td>Pune</td>
                                        <td>def@email.com</td>
                                        <td>1234567891</td>
                                        <td>Database</td>
                                        <td>Disabled</td>
                                    </tr>
                                    <tr>
                                        <td>Name 8</td>
                                        <td>008</td>
                                        <td>Pune</td>
                                        <td>xyz@email.com</td>
                                        <td>1234567892</td>
                                        <td>AD</td>
                                        <td>Enabled</td>
                                    </tr>

                                    <tr>
                                        <td>Name 9</td>
                                        <td>009</td>
                                        <td>Pune</td>
                                        <td>xyz@email.com</td>
                                        <td>1234567892</td>
                                        <td>AD</td>
                                        <td>Enabled</td>
                                    </tr>

                                </tbody>
                               
                            </table>
                            
                                    
                            <div class="row margin-0">    
                                <div class="col-lg-4" style="padding-top:30px;">
                                    Page 1 of 5
                                </div>
                                <div class="col-lg-8">
                                    <div class="page-nation pull-right">
                                        <ul class="pagination pagination-large">
                                            <li class="disabled"><span>«</span></li>
                                            <li class="active"><span>1</span></li>
                                            <li><a href="#">2</a></li>
                                            <li><a href="#">3</a></li>
                                            <li><a href="#">4</a></li>
                                            <li><a href="#">6</a></li>
                                            <li><a href="#">7</a></li>
                                            <li><a href="#">8</a></li>
                                            <li><a href="#">9</a></li>
                                            <li class="disabled"><span>...</span></li>
                                            <li>
                                                <li><a rel="next" href="#">Next</a></li>

                                        </ul>
                                    </div>
                                </div>
                            </div>

                        </div>
                        <!-- /widget-content -->
                    </div>
                    <!-- End of table container-->
                </div>
                <!-- End of survey table wrapper-->

            </div>
            <!-- End of Survey wrapper-->
        </div>
        <!-- End of box row-->

    </div>
</asp:Content>
