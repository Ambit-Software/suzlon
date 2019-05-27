<%@ Page Title="" Language="C#" MasterPageFile="~/SuzlonBPP.Master" AutoEventWireup="true" CodeBehind="VendorPaymentInitiator.aspx.cs" Inherits="SuzlonBPP.VendorPaymentInitiator" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <%-- <script src="Scripts/bootstrap.min.js"></script>--%>



    <script type="text/javascript">

        $(document).keypress(function (e) {
            if (e.keyCode === 13) {
               
                e.preventDefault();
                return false;
            }
        });
        function onRequestStart(sender, args) {
            if (args.get_eventTarget().indexOf("Export") >= 0)
                args.set_enableAjax(false);
        }
        function RowSelectedgrdInitiator_Advance(sender, args) {
            debugger;
            var grid = $find("<%=grdInitiator_Advance.ClientID%>").get_masterTableView();              
                debugger;
                var currentTempINR = 0;
                var currentTempProposed = 0;
                var CurrentTempDebit = 0;
                var CurrentTempCredit = 0;
                var CurrentProposedDebit = 0;
                var CurrentProposedCredit = 0;

                var selrec = $find("<%=grdInitiator_Advance.ClientID%>").get_masterTableView().get_dataItems();
                for(var i=0;i<selrec.length;i++)
                {
                    if (selrec[i]._selected) {                        
                        var row = grid.get_dataItems()[i];
                        var tempInr = parseFloat($(row.get_cell('Amount')).text().split(',').join(''))
                        var dcflag = $(row.get_cell('DCFLag')).text();

                        if (tempInr) {
                            // currentTempINR = currentTempINR + tempInr;
                            if(dcflag=='Credit')
                                CurrentTempCredit = CurrentTempCredit + tempInr
                            else
                                CurrentTempDebit = CurrentTempDebit + tempInr
                        }
                      
                        var tempProp = parseFloat((JSON.parse($(row.findElement('tbProposed_ClientState')).val()).valueAsString.split(',').join('')));
                      
                        if (tempProp) {
                            //currentTempProposed = currentTempProposed + tempProp;
                            if (dcflag == 'Credit')
                                CurrentProposedCredit = CurrentProposedCredit + tempInr
                            else
                                CurrentProposedDebit = CurrentProposedDebit + tempInr
                        }

                    }
                }
                currentTempINR = parseFloat(CurrentTempCredit - CurrentTempDebit).toFixed(2);
                currentTempProposed =parseFloat(CurrentProposedCredit - CurrentProposedDebit).toFixed(2);

                $("#ContentPlaceHolder1_Label7").html(currentTempINR);
                $("#ContentPlaceHolder1_Label9").html(currentTempProposed);
        }
        function RowSelected(sender, args) {         
            if (sender.ClientID.indexOf("gvRequest") != -1) {
               var grid = $find("<%=gvRequest.ClientID%>").get_masterTableView();
                var Index = args.get_itemIndexHierarchical();
                var row = grid.get_dataItems()[Index];
                
                debugger;
                var currentTempINR = 0;
                var currentTempProposed = 0;

                var CurrentTempDebit = 0;
                var CurrentTempCredit = 0;
                var CurrentProposedDebit = 0;
                var CurrentProposedCredit = 0;

                var selrec = $find("<%=gvRequest.ClientID%>").get_masterTableView().get_dataItems();
                for(var i=0;i<selrec.length;i++)
                {
                    if (selrec[i]._selected) {                        
                        var row = grid.get_dataItems()[i];
                        var tempInr = parseFloat($(row.get_cell('Amount')).text().split(',').join(''))
                        var dcflag = $(row.get_cell('DCFLag')).text();

                        if (tempInr) {
                            //   currentTempINR = currentTempINR + tempInr;
                            if (dcflag == 'Credit')
                                CurrentTempCredit = CurrentTempCredit + tempInr
                            else
                                CurrentTempDebit = CurrentTempDebit + tempInr

                        }
                        var tempProp = parseFloat($(row.get_cell('AmountProposed1')).text().split(',').join(''))
                        if (tempProp) {
                            //   currentTempProposed = currentTempProposed + tempProp;

                            if (dcflag == 'Credit')
                                CurrentProposedCredit = CurrentProposedCredit + tempInr
                            else
                                CurrentProposedDebit = CurrentProposedDebit + tempInr
                        }

                    }
                }
                currentTempINR = parseFloat(CurrentTempCredit - CurrentTempDebit).toFixed(2);
                currentTempProposed = parseFloat(CurrentProposedCredit - CurrentProposedDebit).toFixed(2);
                $("#ContentPlaceHolder1_lblINRAmt").html(currentTempINR);
                $("#ContentPlaceHolder1_lblProposedAmt").html(currentTempProposed);
                
            }
            else if (sender.ClientID.indexOf("RadGrid1") != -1) {
                var grid = $find("<%=RadGrid1.ClientID%>").get_masterTableView();
                    var Index = args.get_itemIndexHierarchical();
                    var row = grid.get_dataItems()[Index];
                    var cell = grid.getCellByColumnUniqueName(row, "UnsettledOpenAdvance");
                    //if (parseFloat(cell.innerHTML) > 0) {
                    //    //var validator = row.findElement("rfvJustificationforAdvPayment");
                    //    //validator.enabled = true;
                    //}
                    //var valtbRemarks = row.findElement("rfvRemarks");
                    //var rfvWithholdingTaxCode = row.findElement("rfvWithholdingTaxCode");
                    //valtbRemarks.enabled = true;
                //rfvWithholdingTaxCode.enabled = true;
                    debugger;
                 var currentTempINR = 0;
                var currentTempProposed = 0;
                var selrec = $find("<%=RadGrid1.ClientID%>").get_masterTableView().get_dataItems();
                for(var i=0;i<selrec.length;i++)
                {
                    if (selrec[i]._selected) {                        
                        var row = grid.get_dataItems()[i];
                        //var tempInr = parseFloat($(row.get_cell('Amount')).text().replace(',',''))
                        var tempInr = parseFloat($(row.get_cell('Amount')).text().split(',').join(''));
                        if (tempInr)
                            currentTempINR = currentTempINR + tempInr;

                        var tempProp = parseFloat($(row.get_cell('AmountProposed1')).text().split(',').join(''));
                        if (tempProp)
                            currentTempProposed = currentTempProposed + tempProp;

                    }
                }                
                $("#ContentPlaceHolder1_Label3").html(currentTempINR);
                $("#ContentPlaceHolder1_Label5").html(currentTempProposed);


            }
            else if (sender.ClientID.indexOf("grdInitiator") != -1) {

                var grid = $find("<%=grdInitiator.ClientID%>").get_masterTableView();              
                debugger;
                var currentTempINR = 0;
                var currentTempProposed = 0;

                var CurrentTempDebit = 0;
                var CurrentTempCredit = 0;
                var CurrentProposedDebit = 0;
                var CurrentProposedCredit = 0;


                var selrec = $find("<%=grdInitiator.ClientID%>").get_masterTableView().get_dataItems();
                for(var i=0;i<selrec.length;i++)
                {
                    if (selrec[i]._selected) {                        
                        var row = grid.get_dataItems()[i];
                        // var tempInr = parseFloat($(row.get_cell('Amount')).text().replace(',',''))
                        var tempInr = parseFloat($(row.get_cell('Amount')).text().split(',').join(''));
                        var dcflag = $(row.get_cell('DCFLag')).text();

                        if (tempInr) {
                            // currentTempINR = currentTempINR + tempInr;
                            if (dcflag == 'Credit')
                                CurrentTempCredit = CurrentTempCredit + tempInr
                            else
                                CurrentTempDebit = CurrentTempDebit + tempInr
                        }
                       
                        var tempProp = parseFloat((JSON.parse($(row.findElement('tbAmountProposed_ClientState')).val()).valueAsString.split(',').join('')));
                        if (tempProp) {
                            // currentTempProposed = currentTempProposed + tempProp;
                            if (dcflag == 'Credit')
                                CurrentProposedCredit = CurrentProposedCredit + tempInr
                            else
                                CurrentProposedDebit = CurrentProposedDebit + tempInr
                        }
                    }
                }
                currentTempINR = parseFloat(CurrentTempCredit - CurrentTempDebit).toFixed(2);
                currentTempProposed = parseFloat(CurrentProposedCredit - CurrentProposedDebit).toFixed(2);
                $("#ContentPlaceHolder1_Label7").html(currentTempINR);
                $("#ContentPlaceHolder1_Label9").html(currentTempProposed);
            }
            //fggdgfdg
            if (sender.ClientID.indexOf("gvAllRequest") != -1) {
               var grid = $find("<%=gvAllRequest.ClientID%>").get_masterTableView();
                var Index = args.get_itemIndexHierarchical();
                var row = grid.get_dataItems()[Index];
                
                debugger;
                var currentTempINR = 0;
                var currentTempProposed = 0;

                var CurrentTempDebit = 0;
                var CurrentTempCredit = 0;
                var CurrentProposedDebit = 0;
                var CurrentProposedCredit = 0;

                var selrec = $find("<%=gvAllRequest.ClientID%>").get_masterTableView().get_dataItems();
                for(var i=0;i<selrec.length;i++)
                {
                    if (selrec[i]._selected) {                        
                        var row = grid.get_dataItems()[i];
                        var tempInr = parseFloat($(row.get_cell('Amount')).text().split(',').join(''))
                        var dcflag = $(row.get_cell('DCFLag')).text();

                        if (tempInr) {
                            //   currentTempINR = currentTempINR + tempInr;
                            if (dcflag == 'Credit')
                                CurrentTempCredit = CurrentTempCredit + tempInr
                            else
                                CurrentTempDebit = CurrentTempDebit + tempInr

                        }
                        var tempProp = parseFloat($(row.get_cell('AmountProposed1')).text().split(',').join(''))
                        if (tempProp) {
                            //   currentTempProposed = currentTempProposed + tempProp;

                            if (dcflag == 'Credit')
                                CurrentProposedCredit = CurrentProposedCredit + tempInr
                            else
                                CurrentProposedDebit = CurrentProposedDebit + tempInr
                        }

                    }
                }
                currentTempINR = parseFloat(CurrentTempCredit - CurrentTempDebit).toFixed(2);
                currentTempProposed = parseFloat(CurrentProposedCredit - CurrentProposedDebit).toFixed(2);
                $("#ContentPlaceHolder1_lblINRAmt").html(currentTempINR);
                $("#ContentPlaceHolder1_lblProposedAmt").html(currentTempProposed);
                
            }

              else if (sender.ClientID.indexOf("RadGrid4") != -1) {
                var grid = $find("<%=RadGrid4.ClientID%>").get_masterTableView();
                    var Index = args.get_itemIndexHierarchical();
                    var row = grid.get_dataItems()[Index];
                    var cell = grid.getCellByColumnUniqueName(row, "UnsettledOpenAdvance");
                 var currentTempINR = 0;
                var currentTempProposed = 0;
                var selrec = $find("<%=RadGrid4.ClientID%>").get_masterTableView().get_dataItems();
                for(var i=0;i<selrec.length;i++)
                {
                    if (selrec[i]._selected) {                        
                        var row = grid.get_dataItems()[i];                       
                        var tempInr = parseFloat($(row.get_cell('Amount')).text().split(',').join(''));
                        if (tempInr)
                            currentTempINR = currentTempINR + tempInr;

                        var tempProp = parseFloat($(row.get_cell('AmountProposed1')).text().split(',').join(''));
                        if (tempProp)
                            currentTempProposed = currentTempProposed + tempProp;

                    }
                }                
                $("#ContentPlaceHolder1_Label3").html(currentTempINR);
                $("#ContentPlaceHolder1_Label5").html(currentTempProposed);


            }
        }


        function RowDeselected(sender, args) {
            if (sender.ClientID.indexOf("gvRequest") != -1) {
              var grid = $find("<%=gvRequest.ClientID%>").get_masterTableView();
                var Index = args.get_itemIndexHierarchical();
                var row = grid.get_dataItems()[Index];
                
                debugger;
                 var currentTempINR = 0;
                 var currentTempProposed = 0;
                 var CurrentTempDebit = 0;
                 var CurrentTempCredit = 0;
                 var CurrentProposedDebit = 0;
                 var CurrentProposedCredit = 0;

                var selrec = $find("<%=gvRequest.ClientID%>").get_masterTableView().get_dataItems();
                for(var i=0;i<selrec.length;i++)
                {
                    if (selrec[i]._selected) {                        
                        var row = grid.get_dataItems()[i];
                        var tempInr = parseFloat($(row.get_cell('Amount')).text().split(',').join(''))
                        var dcflag = $(row.get_cell('DCFLag')).text();

                        if (tempInr) {
                            //currentTempINR = currentTempINR + tempInr;
                            if (dcflag == 'Credit')
                                CurrentTempCredit = CurrentTempCredit + tempInr
                            else
                                CurrentTempDebit = CurrentTempDebit + tempInr
                        }

                        var tempProp = parseFloat($(row.get_cell('AmountProposed1')).text().split(',').join(''))
                        if (tempProp) {
                            //currentTempProposed = currentTempProposed + tempProp;
                            if (dcflag == 'Credit')
                                CurrentProposedCredit = CurrentProposedCredit + tempInr
                            else
                                CurrentProposedDebit = CurrentProposedDebit + tempInr
                        }
                    }
                }
                currentTempINR = parseFloat(CurrentTempCredit - CurrentTempDebit).toFixed(2);
                currentTempProposed = parseFloat(CurrentProposedCredit - CurrentProposedDebit).toFixed(2);
                $("#ContentPlaceHolder1_lblINRAmt").html(currentTempINR);
                $("#ContentPlaceHolder1_lblProposedAmt").html(currentTempProposed);

            }
            else if (sender.ClientID.indexOf("RadGrid1") != -1) {
                var grid = $find("<%=RadGrid1.ClientID%>").get_masterTableView();
                    var Index = args.get_itemIndexHierarchical();
                    var row = grid.get_dataItems()[Index];
                    //var validator = row.findElement("rfvJustificationforAdvPayment");
                    //var valtbRemarks = row.findElement("rfvRemarks");
                    //var rfvWithholdingTaxCode = row.findElement("rfvWithholdingTaxCode");
                    //validator.enabled = false;
                    //valtbRemarks.enabled = false;
                //rfvWithholdingTaxCode.enabled = false;

                debugger;
                 var currentTempINR = 0;
                 var currentTempProposed = 0;
                 var CurrentTempDebit = 0;
                 var CurrentTempCredit = 0;
                 var CurrentProposedDebit = 0;
                 var CurrentProposedCredit = 0;
                var selrec = $find("<%=RadGrid1.ClientID%>").get_masterTableView().get_dataItems();
                for(var i=0;i<selrec.length;i++)
                {
                    if (selrec[i]._selected) {                        
                        var row = grid.get_dataItems()[i];
                        //var tempInr = parseFloat($(row.get_cell('Amount')).text().replace(',',''))
                        var tempInr = parseFloat($(row.get_cell('Amount')).text().split(',').join(''));
                        var dcflag = $(row.get_cell('DCFLag')).text();

                        if (tempInr) {
                            //currentTempINR = currentTempINR + tempInr;
                            if (dcflag == 'Credit')
                                CurrentTempCredit = CurrentTempCredit + tempInr
                            else
                                CurrentTempDebit = CurrentTempDebit + tempInr
                        }

                        var tempProp = parseFloat($(row.get_cell('AmountProposed1')).text().split(',').join(''));
                        if (tempProp) {
                            // currentTempProposed = currentTempProposed + tempProp;
                            if (dcflag == 'Credit')
                                CurrentProposedCredit = CurrentProposedCredit + tempInr
                            else
                                CurrentProposedDebit = CurrentProposedDebit + tempInr
                        }
                    }
                }
                currentTempINR = parseFloat(CurrentTempCredit - CurrentTempDebit).toFixed(2);
                currentTempProposed = parseFloat(CurrentProposedCredit - CurrentProposedDebit).toFixed(2);
                $("#ContentPlaceHolder1_Label3").html(currentTempINR);
                $("#ContentPlaceHolder1_Label5").html(currentTempProposed);


            }

            if (sender.ClientID.indexOf("gvAllRequest") != -1) {
               var grid = $find("<%=gvAllRequest.ClientID%>").get_masterTableView();
                var Index = args.get_itemIndexHierarchical();
                var row = grid.get_dataItems()[Index];
                
                debugger;
                var currentTempINR = 0;
                var currentTempProposed = 0;

                var CurrentTempDebit = 0;
                var CurrentTempCredit = 0;
                var CurrentProposedDebit = 0;
                var CurrentProposedCredit = 0;

                var selrec = $find("<%=gvAllRequest.ClientID%>").get_masterTableView().get_dataItems();
                for(var i=0;i<selrec.length;i++)
                {
                    if (selrec[i]._selected) {                        
                        var row = grid.get_dataItems()[i];
                        var tempInr = parseFloat($(row.get_cell('Amount')).text().split(',').join(''))
                        var dcflag = $(row.get_cell('DCFLag')).text();

                        if (tempInr) {
                            //   currentTempINR = currentTempINR + tempInr;
                            if (dcflag == 'Credit')
                                CurrentTempCredit = CurrentTempCredit + tempInr
                            else
                                CurrentTempDebit = CurrentTempDebit + tempInr

                        }
                        var tempProp = parseFloat($(row.get_cell('AmountProposed1')).text().split(',').join(''))
                        if (tempProp) {
                            //   currentTempProposed = currentTempProposed + tempProp;

                            if (dcflag == 'Credit')
                                CurrentProposedCredit = CurrentProposedCredit + tempInr
                            else
                                CurrentProposedDebit = CurrentProposedDebit + tempInr
                        }

                    }
                }
                currentTempINR = parseFloat(CurrentTempCredit - CurrentTempDebit).toFixed(2);
                currentTempProposed = parseFloat(CurrentProposedCredit - CurrentProposedDebit).toFixed(2);
                $("#ContentPlaceHolder1_lblINRAmt").html(currentTempINR);
                $("#ContentPlaceHolder1_lblProposedAmt").html(currentTempProposed);
                
            }
            else if (sender.ClientID.indexOf("RadGrid4") != -1) {
                var grid = $find("<%=RadGrid4.ClientID%>").get_masterTableView();
                    var Index = args.get_itemIndexHierarchical();
                    var row = grid.get_dataItems()[Index];           
                 var currentTempINR = 0;
                 var currentTempProposed = 0;
                 var CurrentTempDebit = 0;
                 var CurrentTempCredit = 0;
                 var CurrentProposedDebit = 0;
                 var CurrentProposedCredit = 0;
                var selrec = $find("<%=RadGrid4.ClientID%>").get_masterTableView().get_dataItems();
                for(var i=0;i<selrec.length;i++)
                {
                    if (selrec[i]._selected) {                        
                        var row = grid.get_dataItems()[i];                     
                        var tempInr = parseFloat($(row.get_cell('Amount')).text().split(',').join(''));
                        var dcflag = $(row.get_cell('DCFLag')).text();

                        if (tempInr) {                          
                            if (dcflag == 'Credit')
                                CurrentTempCredit = CurrentTempCredit + tempInr
                            else
                                CurrentTempDebit = CurrentTempDebit + tempInr
                        }

                        var tempProp = parseFloat($(row.get_cell('AmountProposed1')).text().split(',').join(''));
                        if (tempProp) {                          
                            if (dcflag == 'Credit')
                                CurrentProposedCredit = CurrentProposedCredit + tempInr
                            else
                                CurrentProposedDebit = CurrentProposedDebit + tempInr
                        }
                    }
                }
                currentTempINR = parseFloat(CurrentTempCredit - CurrentTempDebit).toFixed(2);
                currentTempProposed = parseFloat(CurrentProposedCredit - CurrentProposedDebit).toFixed(2);
                $("#ContentPlaceHolder1_Label3").html(currentTempINR);
                $("#ContentPlaceHolder1_Label5").html(currentTempProposed);
            }



        }

        function RowSelectedNeedCorrection(sender, args) {
            debugger;
            if (sender.ClientID.indexOf("gvInitiatorNeedCorrection") != -1) {
                var grid = $find("<%=gvInitiatorNeedCorrection.ClientID%>").get_masterTableView();
                var Index = args.get_itemIndexHierarchical();
                var row = grid.get_dataItems()[Index];
                var validator = row.findElement("rfvAmtProposedNeedCorrection");
                var valtbRemarks = row.findElement("rfvRequestRemarksNeedCorrection");
                validator.enabled = true;
                valtbRemarks.enabled = true;


                var currentTempINR = 0;
                var currentTempProposed = 0;
                var CurrentTempDebit = 0;
                var CurrentTempCredit = 0;
                var CurrentProposedDebit = 0;
                var CurrentProposedCredit = 0;

                 var selrec = $find("<%=gvInitiatorNeedCorrection.ClientID%>").get_masterTableView().get_dataItems();

                for (var i = 0; i < selrec.length; i++) {
                    if (selrec[i]._selected) {
                        var row = grid.get_dataItems()[i];
                        var tempInr = parseFloat($(row.get_cell('AmountNeedCorrection')).text().split(',').join(''))
                        var dcflag = $(row.get_cell('DCFLag')).text();
                        if (tempInr) {                          
                            if (dcflag == 'Credit')
                                CurrentTempCredit = CurrentTempCredit + tempInr
                            else
                                CurrentTempDebit = CurrentTempDebit + tempInr
                        }
                       
                        var tempProp = parseFloat((JSON.parse($(row.findElement('tbAmountProposedNeedCorrection_ClientState')).val()).valueAsString.split(',').join('')));
                        if (tempProp) {
                            if (dcflag == 'Credit')
                                CurrentProposedCredit = CurrentProposedCredit + tempInr
                            else
                                CurrentProposedDebit = CurrentProposedDebit + tempInr
                        }

                    }
                }
                currentTempINR = parseFloat(CurrentTempCredit - CurrentTempDebit).toFixed(2);
                currentTempProposed = parseFloat(CurrentProposedCredit - CurrentProposedDebit).toFixed(2);
                $("#ContentPlaceHolder1_lblINRAmt").html(currentTempINR);
                $("#ContentPlaceHolder1_lblProposedAmt").html(currentTempProposed);
            }
            else if (sender.ClientID.indexOf("RadGrid3") != -1) {
                var grid = $find("<%=RadGrid3.ClientID%>").get_masterTableView();
                    var Index = args.get_itemIndexHierarchical();
                    var row = grid.get_dataItems()[Index];
                    var cell = grid.getCellByColumnUniqueName(row, "UnsettledOpenAdvance");
                    //if (parseFloat(cell.innerHTML) > 0) {
                    //    var validator = row.findElement("rfvJustificationforAdvPaymentAdvanceCorrection");
                    //    validator.enabled = true;
                    //}
                    //var valtbRemarks = row.findElement("rfvRemarksAdvanceCorrection");
                    //var rfvWithholdingTaxCode = row.findElement("rfvWithholdingTaxCodeAdvanceCorrection");
                    //valtbRemarks.enabled = true;
                //rfvWithholdingTaxCode.enabled = true;

                    var currentTempINR = 0;
                    var currentTempProposed = 0;
                    var CurrentTempDebit = 0;
                    var CurrentTempCredit = 0;
                    var CurrentProposedDebit = 0;
                    var CurrentProposedCredit = 0;

                    var selrec = $find("<%=RadGrid3.ClientID%>").get_masterTableView().get_dataItems();

                    for (var i = 0; i < selrec.length; i++) {
                    if (selrec[i]._selected) {
                        var row = grid.get_dataItems()[i];
                        var tempInr = parseFloat($(row.get_cell('AmountAdvanceCorrection')).text().split(',').join(''))
                        var dcflag = $(row.get_cell('DCFLag')).text();
                        if (tempInr) {                          
                            if (dcflag == 'Credit')
                                CurrentTempCredit = CurrentTempCredit + tempInr
                            else
                                CurrentTempDebit = CurrentTempDebit + tempInr
                        }
                       
                        var tempProp = parseFloat((JSON.parse($(row.findElement('tbProposedAdvanceCorrection_ClientState')).val()).valueAsString.split(',').join('')));
                        if (tempProp) {
                            if (dcflag == 'Credit')
                                CurrentProposedCredit = CurrentProposedCredit + tempInr
                            else
                                CurrentProposedDebit = CurrentProposedDebit + tempInr
                        }

                    }
                    }
                    currentTempINR = parseFloat(CurrentTempCredit - CurrentTempDebit).toFixed(2);
                    currentTempProposed = parseFloat(CurrentProposedCredit - CurrentProposedDebit).toFixed(2);
                    $("#ContentPlaceHolder1_Label3").html(currentTempINR);
                    $("#ContentPlaceHolder1_Label5").html(currentTempProposed);



                }
        }

        function RowDeselectedNeedCorrection(sender, args) {
            if (sender.ClientID.indexOf("gvInitiatorNeedCorrection") != -1) {
                var grid = $find("<%=gvInitiatorNeedCorrection.ClientID%>").get_masterTableView();
                var Index = args.get_itemIndexHierarchical();
                var row = grid.get_dataItems()[Index];
                var validator = row.findElement("rfvAmtProposedNeedCorrection");
                var valtbRemarks = row.findElement("rfvRequestRemarksNeedCorrection");
                validator.enabled = false;
                valtbRemarks.enabled = false;

                   var currentTempINR = 0;
                var currentTempProposed = 0;
                var CurrentTempDebit = 0;
                var CurrentTempCredit = 0;
                var CurrentProposedDebit = 0;
                var CurrentProposedCredit = 0;

                 var selrec = $find("<%=gvInitiatorNeedCorrection.ClientID%>").get_masterTableView().get_dataItems();

                for (var i = 0; i < selrec.length; i++) {
                    if (selrec[i]._selected) {
                        var row = grid.get_dataItems()[i];
                        var tempInr = parseFloat($(row.get_cell('AmountNeedCorrection')).text().split(',').join(''))
                        var dcflag = $(row.get_cell('DCFLag')).text();
                        if (tempInr) {                          
                            if (dcflag == 'Credit')
                                CurrentTempCredit = CurrentTempCredit + tempInr
                            else
                                CurrentTempDebit = CurrentTempDebit + tempInr

                        }
                        //  var tempProp = parseFloat($(row.get_cell('AmountProposedNeedCorrection')).text().split(',').join(''))
                        var tempProp = parseFloat((JSON.parse($(row.findElement('tbAmountProposedNeedCorrection_ClientState')).val()).valueAsString.split(',').join('')));
                        if (tempProp) {
                            if (dcflag == 'Credit')
                                CurrentProposedCredit = CurrentProposedCredit + tempInr
                            else
                                CurrentProposedDebit = CurrentProposedDebit + tempInr
                        }

                    }
                }
                currentTempINR = parseFloat(CurrentTempCredit - CurrentTempDebit).toFixed(2);
                currentTempProposed = parseFloat(CurrentProposedCredit - CurrentProposedDebit).toFixed(2);
                $("#ContentPlaceHolder1_lblINRAmt").html(currentTempINR);
                $("#ContentPlaceHolder1_lblProposedAmt").html(currentTempProposed);





            }
            else if (sender.ClientID.indexOf("RadGrid3") != -1) {
                var grid = $find("<%=RadGrid3.ClientID%>").get_masterTableView();
                      var Index = args.get_itemIndexHierarchical();
                      var row = grid.get_dataItems()[Index];
                      var validator = row.findElement("rfvJustificationforAdvPaymentAdvanceCorrection");
                      var valtbRemarks = row.findElement("rfvRemarksAdvanceCorrection");
                      var rfvWithholdingTaxCode = row.findElement("rfvWithholdingTaxCodeAdvanceCorrection");
                      validator.enabled = false;
                      valtbRemarks.enabled = false;
                      rfvWithholdingTaxCode.enabled = false;


                  var currentTempINR = 0;
                    var currentTempProposed = 0;
                    var CurrentTempDebit = 0;
                    var CurrentTempCredit = 0;
                    var CurrentProposedDebit = 0;
                    var CurrentProposedCredit = 0;

                    var selrec = $find("<%=RadGrid3.ClientID%>").get_masterTableView().get_dataItems();

                    for (var i = 0; i < selrec.length; i++) {
                    if (selrec[i]._selected) {
                        var row = grid.get_dataItems()[i];
                        var tempInr = parseFloat($(row.get_cell('AmountAdvanceCorrection')).text().split(',').join(''))
                        var dcflag = $(row.get_cell('DCFLag')).text();
                        if (tempInr) {                          
                            if (dcflag == 'Credit')
                                CurrentTempCredit = CurrentTempCredit + tempInr
                            else
                                CurrentTempDebit = CurrentTempDebit + tempInr
                        }
                       
                        var tempProp = parseFloat((JSON.parse($(row.findElement('tbProposedAdvanceCorrection_ClientState')).val()).valueAsString.split(',').join('')));
                        if (tempProp) {
                            if (dcflag == 'Credit')
                                CurrentProposedCredit = CurrentProposedCredit + tempInr
                            else
                                CurrentProposedDebit = CurrentProposedDebit + tempInr
                        }

                    }
                    }
                    currentTempINR = parseFloat(CurrentTempCredit - CurrentTempDebit).toFixed(2);
                    currentTempProposed = parseFloat(CurrentProposedCredit - CurrentProposedDebit).toFixed(2);
                    $("#ContentPlaceHolder1_Label3").html(currentTempINR);
                    $("#ContentPlaceHolder1_Label5").html(currentTempProposed);



                  }
          }



          function OnBlurRequestRemarks(sender, args) {

              if (sender.get_value() == '') {
                  alert("Please Enter remarks");
              }
          }

          function OnBlurAmountProposed(sender, args) {

              if (sender.get_value() == '') {
                  alert("Please Enter Proposed Amount");
              }
          }


          function OnBlurAdvanceRemarks(sender, args) {

              if (sender.get_value() == '') {
                  alert("Please Enter remarks");
              }
          }

          function OnBlurAdvancedAmountProposed(sender, args) {
              if (sender.get_value() == '') {
                  alert("Please Enter Proposed Amount");
              }
          }

          function OnBlurWithholdingTax(sender, args) {

              if (sender.get_value() == '') {
                  alert("Please Enter Withholding Tax Code");
              }
          }

          function OnBlurJustification(sender, args) {
              if (sender.get_value() == '') {
                  alert("Please Enter Justification for Adv Paymen");
              }
          }
          function validateCompCode()
          {
             
              var compCode = $find('<%= cmbCompany.ClientID %>');
              var value = compCode.get_value();
              if (value == null || value == "") {
                  var $radNotify = $find("<%= radMessage.ClientID%>");
                  $radNotify.set_title("Alert");
                  $radNotify.set_text("Please select Company");
                  $radNotify.show();
              }

              else {
                  
                  var txtVendor = $find('<%= txtVendorSearch.ClientID%>');
                  txtVendor.set_value("");
                  $("#squarespaceVendorSearch").modal('show');

              }
          }

        $(document).ready(function () {
            $("#ContentPlaceHolder1_btnOptions_0").change(function () {

                 var compCode = $find("<%= cmbCompany.ClientID%>");
                var vendrCode = $find("<%= cmbVendor.ClientID%>");      
                if (compCode.get_selectedItem() == null || compCode.get_selectedItem() == "" || compCode.get_selectedItem() == "Undefined")
                {
                  var $radNotify = $find("<%= radMessage.ClientID%>");                
                  $radNotify.set_title("Alert");
                  $radNotify.set_text("Please select Company");
                  $radNotify.show();
                  return;
                }
                if (compCode.get_selectedItem()._text != "All" && (vendrCode.get_selectedItem() == null || vendrCode.get_selectedItem() == "" || vendrCode.get_selectedItem() == "Undefined"))
                {
                  
                        var $radNotify = $find("<%= radMessage.ClientID%>");
                        $radNotify.set_title("Alert");
                        $radNotify.set_text("Please select Vendor");
                        $radNotify.show();
                        return;
                   
                }
                function linkButtonCallbackFn(arg) {
                    if (arg) {

                        callAjaxRequest('Against');
                    }
                    else {                       
                      
                        $("#ContentPlaceHolder1_btnOptions_0").attr("checked", false);
                        $("#ContentPlaceHolder1_btnOptions_1").prop("checked", "checked");
                    }
                }
                radconfirm("Are you sure you want to change the selection?", linkButtonCallbackFn, 370, 100, null, "Alert", "");
            
            });
            
            $("#ContentPlaceHolder1_btnOptions_1").change(function () {

                debugger;
                var compCode = $find("<%= cmbCompany.ClientID%>");
                var vendrCode = $find("<%= cmbVendor.ClientID%>");
      
                if (compCode.get_selectedItem() == null || compCode.get_selectedItem() == "" || compCode.get_selectedItem() == "Undefined")
                {
                    var $radNotify = $find("<%= radMessage.ClientID%>");                
                  $radNotify.set_title("Alert");
                  $radNotify.set_text("Please select Company");
                  $radNotify.show();
                  return;
                }

                if (compCode.get_selectedItem()._text != "All" && (vendrCode.get_selectedItem() == null || vendrCode.get_selectedItem() == "" || vendrCode.get_selectedItem() == "Undefined"))
                {
                    var $radNotify = $find("<%= radMessage.ClientID%>");                
                  $radNotify.set_title("Alert");
                  $radNotify.set_text("Please select Vendor");
                  $radNotify.show();
                  return;
                }

                function linkButtonCallbackFn(arg) {

                    if (arg) {
                        callAjaxRequest('Advance');                       
                    }
                    else {
                        $("#ContentPlaceHolder1_btnOptions_1").attr("checked", false);
                        $("#ContentPlaceHolder1_btnOptions_0").prop("checked", "checked");
                    }
                }
                radconfirm("Are you sure you want to change the selection?", linkButtonCallbackFn, 370, 100, null, "Alert", "");

            });
             function callAjaxRequest(parameter) {  
                 radAjaxManager1 = $find("<%= RadAjaxManager1.ClientID %>");  
                 radAjaxManager1.ajaxRequest(parameter);
                }

        });
        
        function openRequestUnderProcessPopup() {           
            setTimeout(function () {
                $("#squarespaceWorkFlowModal").modal("show");
            }, 500);
        }

    </script>


    <style>
        .RadGrid_Default .rgSelectedRow {
            background-image: none !important;
            color: #333 !important;
        }

        .RadTabStrip_Silk .rtsLevel1 {
            border-color: transparent !important;
        }

        .RadPicker {
            width: 140px !important;
        }

            .RadPicker .rcTimePopup {
                display: inline-block;
                content: "\e13d" !important;
                background-image: none !important;
            }

        .RadGrid_Default {
            color: #333;
            background-color: transparent !important;
        }

        .RadPicker_Default {
            background-image: none !important;
            background-image: url('../images/calendar-icon.png') !important;
            border-color: #cdcdcd !important;
            background-color: #e6e6e6 !important;
        }

        .rcTimePopup {
            background-image: none !important;
            background-image: none !important;
            background: none !important;
            background: none !important;
        }

            .rcTimePopup::before {
                content: "\e08d" !important;
            }

        .grid-wrapper {
            overflow: hidden;
            overflow-x: hidden !important;
        }

        .panel-body {
            padding: 0px !important;
        }

        .panel {
            -webkit-box-shadow: 0 0px 0px rgba(0,0,0,.05) !important;
            box-shadow: 0 0px 0px rgba(0,0,0,.05) !important;
        }

        .RadPicker {
            width: 81% !important;
        }

        .RadGrid .rgFilterBox {
            width: 50% !important;
        }

        .RadWindow .rwIcon {
            display: none !important;
        }

        .RadWindow .rwTitleWrapper .rwTitle {
            margin: 2px 0 0 -21px !important;
            font-weight: bold !important;
        }

        .table {
            width: 100% !important;
        }

        label {
            margin-right: 10px !important;
        }

        .agnstbillradio {
            display: table-cell;
            vertical-align: middle;
            padding-top: 5px !important;
        }

        .agnstbilllable {
            padding-top: 7px;
        }

        .button-wrapper li {
            padding: 0px 5px 10px 0px !important;
        }

        .button-wrapper ul {
            float: right !important;
        }
.RadGrid_Default {
    border: 1px solid #bbb !important;
    background-color: #fff !important;
    color: #333 !important;
}
.RadGrid_Frozen 
 {
      overflow-x: scroll !important;
 }
        .girdcolwidth {
            width:100px !important;
        }
  
[id$='Frozen'] {
  overflow:scroll !important;  
}

        .RadNotification .rnContentWrapper {
            position: relative;
            padding: .35714286em;
            border-top-width: 1px;
            border-top-style: solid;
            border-top-color: transparent;
            overflow: visible;
        }

        .RadNotification.rnNoContentIcon .rnContent {
            overflow-y: scroll;
            overflow-x: hidden;
            max-height: 400px;
            width: 100%;
        }

        .RadNotification .rnContentWrapper {
            overflow: hidden !important;
            max-height: 500px;
        }

        .RadNotification, .RadNotification * {
            max-height: 500px;
            overflow: hidden;
        }

    </style>
     <style type="text/css">
        .dialog-background {
            background: none repeat scroll 0 0 rgba(248, 246, 246, 0.00);
            height: 100%;
            left: 0;
            margin: 0;
            padding: 0;
            position: absolute;
            top: 0;
            width: 100%;
            z-index: 100;
        }

        .dialog-loading-wrapper {
            background-image: url(../Content/images/loading.gif);
            border: 0 none;
            height: 100px;
            left: 50%;
            margin-left: -50px;
            margin-top: -50px;
            position: fixed;
            top: 50%;
            width: 100px;
            z-index: 9999999;
            opacity: 1;
        }

        .dialog-loading-icon {
            background-image: url("content/images/loading.gif");
            background-repeat: no-repeat;
            /*background-color: #EFEFEF !important;*/
            /*border-radius: 13px;*/
            display: block;
            height: 100px;
            line-height: 100px;
            margin: 0;
            padding: 1px;
            text-align: center;
            width: 100px;
            opacity: 1;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="Scripts/bootstrap.min.js"></script>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript">
            function ShowComments(id) {
                var ajaxManager = $find("<%= RadAjaxManager1.ClientID %>");
                ajaxManager.ajaxRequest("Comment#" + id + "");

                $('#squarespaceCommentModal').modal();
            }

            function openRadWin(screen, mode, entityId, canAdd, canDelete, isMultiFileUpload, showDocumentType, entityName) {
                debugger;
                var manager = $find("<%= RadWindowManager.ClientID %>");
                // if (screen == 'VC') {
                manager.open("AddAttachments.aspx?mode=" + mode + "&entityId=" + entityId + "&canAdd= " + canAdd + "&canDelete= " + canDelete + "&isMultiUpload= " + isMultiFileUpload + "&showDtype= " + showDocumentType + "&entityName=" + entityName, "RadWindow");
                return false;
                // }
            }
            function ClosePopup()
            {
                $("#squarespaceVendorSearch").modal("hide");
            }

            function openModal() {
                $("#squarespaceInitiator").modal("show");
            }
            function ClosePopupInitiator() {
                $("#squarespaceInitiator").modal("hide");
            }


            function openRadWinInitiator(screen, BillType, CompanyCode, vendorCode) {
                debugger;
                var initiatorPopUp = $find("<%= RadWindowManagerPopup.ClientID %>");             
                initiatorPopUp.open("PaymentInitiator.aspx?billType=" + BillType + "&companyCode=" + CompanyCode + "&vendorCode= " + vendorCode, "RadWindow1");
                return false;
               
            }

       function ValidateRow(sender, args, index, curBalanceAmount) {
           debugger;
        var grid = $find("<%=grdInitiator_Advance.ClientID%>");
        var dataItems = grid.get_masterTableView().get_dataItems();
        var dataItem = dataItems[index];
        var CurrentVal = parseFloat(JSON.parse(dataItem.findElement("tbProposed_ClientState").value).valueAsString); //sender._value;//
                  
        var currentTotalAllocation = 0;
        for (var i = 0; i < dataItems.length; i++) {
            if (dataItems_selected) {
                temp = parseFloat(JSON.parse(dataItems[i].findElement("tbProposed_ClientState").value).valueAsString);
                if (temp) currentTotalAllocation = currentTotalAllocation + temp;
            }
        }
        $("#ContentPlaceHolder1_Label9").text(currentTotalAllocation);        
        return true;
    }
     function ValidateRowAgainst(sender, args, index, curBalanceAmount) {
           debugger;
        var grid = $find("<%=grdInitiator.ClientID%>");
        var dataItems = grid.get_masterTableView().get_dataItems();
        var dataItem = dataItems[index];
        var CurrentVal = parseFloat(JSON.parse(dataItem.findElement("tbAmountProposed_ClientState").value).valueAsString); //sender._value;//
                       
        var currentTotalAllocation = 0;
        for (var i = 0; i < dataItems.length; i++) {
            if (dataItems_selected) {
                temp = parseFloat(JSON.parse(dataItems[i].findElement("tbAmountProposed_ClientState").value).valueAsString);
                if (temp) currentTotalAllocation = currentTotalAllocation + temp;
            }
        }
        $("#ContentPlaceHolder1_Label9").text(currentTotalAllocation);        
        return true;
    }

            
        </script>
    </telerik:RadCodeBlock>

    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" EnablePageHeadUpdate="false" OnAjaxRequest="RadAjaxManager1_AjaxRequest">

       
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadAjaxManager1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdComment" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                     <telerik:AjaxUpdatedControl ControlID="pnlAdvanceBill" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="pnlAgainstBill" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="btnAgainstExport" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="btnAdvanceExport" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="grdWorkFlowInProcess" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                  <%--  <telerik:AjaxUpdatedControl ControlID="btnOptions" />--%>
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
        <ClientEvents OnRequestStart="onRequestStart" />
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="btnRequestUnderPorcess">
                  <UpdatedControls>
                 <telerik:AjaxUpdatedControl ControlID="grdWorkFlowInProcess" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                  </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lbSearch">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="pnlAdvanceBill" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="pnlAgainstBill" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                <telerik:AjaxUpdatedControl ControlID="radMessage" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnOptions">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="pnlAgainstBill" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="pnlAdvanceBill" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="btnOptions" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="grdWorkFlowInProcess" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="gvRequest">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="gvRequest" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="gvInitiatorNeedCorrection">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="gvInitiatorNeedCorrection" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="gvIntitiatorInProcess">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="gvIntitiatorInProcess" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadGrid1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadGrid2">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid2" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadGrid3">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid3" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnAgainstExport">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="gvRequest" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="gvInitiatorNeedCorrection" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="gvIntitiatorInProcess" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnAdvanceExport">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid2" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid3" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="cmbcompany">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="cmbVendor" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="txtCompanyCode" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="grdVendorSearch" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            
              <telerik:AjaxSetting AjaxControlID="LinkButton3">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="radMessage" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnRequestSave">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="gvRequest" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="gvIntitiatorInProcess" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="gvAllRequest" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="radMessage" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnRequestSubmit">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="gvRequest" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="gvIntitiatorInProcess" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                     <telerik:AjaxUpdatedControl ControlID="gvAllRequest" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="radMessage" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>

             <telerik:AjaxSetting AjaxControlID="SaveAllTab">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="gvRequest" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="gvIntitiatorInProcess" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="gvAllRequest" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="radMessage" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="SubmitAllTab">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="gvRequest" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="gvIntitiatorInProcess" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                     <telerik:AjaxUpdatedControl ControlID="gvAllRequest" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="radMessage" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="btnSave">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid2" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="radMessage" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnSubmit">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid2" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="radMessage" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lbSaveBillNeedCorrection">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="gvInitiatorNeedCorrection" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="radMessage" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lbSubmitBillNeedCorrection">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="gvInitiatorNeedCorrection" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="radMessage" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lbSAveAdvancedNeedCorrection">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid3" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="radMessage" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lbSubmitAdvancedNeedCorrection">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid3" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="radMessage" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>

             <telerik:AjaxSetting AjaxControlID="btnSearchVendor">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdVendorSearch" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>                   
                    <telerik:AjaxUpdatedControl ControlID="radMessage" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
               <telerik:AjaxSetting AjaxControlID="grdVendorSearch">
                <UpdatedControls>
                     <telerik:AjaxUpdatedControl ControlID="cmbVendor" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="radMessage" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>

             <telerik:AjaxSetting AjaxControlID="grdInitiator">
                <UpdatedControls>
                     <telerik:AjaxUpdatedControl ControlID="grdInitiator" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                   
                </UpdatedControls>
            </telerik:AjaxSetting>
                    <telerik:AjaxSetting AjaxControlID="grdInitiator_Advance">
                <UpdatedControls>
                     <telerik:AjaxUpdatedControl ControlID="grdInitiator_Advance" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>                   
                </UpdatedControls>
            </telerik:AjaxSetting>
             <telerik:AjaxSetting AjaxControlID="btnAddMore">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdInitiator" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl> 
                     <telerik:AjaxUpdatedControl ControlID="grdInitiator_Advance" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>                   
                     <telerik:AjaxUpdatedControl ControlID="Label7" ></telerik:AjaxUpdatedControl>                   
                     <telerik:AjaxUpdatedControl ControlID="Label9" ></telerik:AjaxUpdatedControl>    
                    <telerik:AjaxUpdatedControl ControlID="cmbNatureofRequestPopUp" ></telerik:AjaxUpdatedControl> 
                    <telerik:AjaxUpdatedControl ControlID="cmbSubVerticalPopUp" ></telerik:AjaxUpdatedControl> 
                     <telerik:AjaxUpdatedControl ControlID="btnAddMore"  LoadingPanelID="LoadingPanel1" ></telerik:AjaxUpdatedControl> 
                </UpdatedControls>
            </telerik:AjaxSetting>
             <telerik:AjaxSetting AjaxControlID="cmbNatureofRequestPopUp">
                <UpdatedControls>
                     <telerik:AjaxUpdatedControl ControlID="grdInitiator" LoadingPanelID="LoadingPanel1" ></telerik:AjaxUpdatedControl> 
                    <telerik:AjaxUpdatedControl ControlID="grdInitiator_Advance" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl> 
                     </UpdatedControls>
            </telerik:AjaxSetting>
              <telerik:AjaxSetting AjaxControlID="cmbSubVerticalPopUp">
                <UpdatedControls>
                     <telerik:AjaxUpdatedControl ControlID="grdInitiator" LoadingPanelID="LoadingPanel1" ></telerik:AjaxUpdatedControl> 
                    <telerik:AjaxUpdatedControl ControlID="grdInitiator_Advance" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl> 
                     </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="lnkSubmitInitiator">
                <UpdatedControls>
                     <telerik:AjaxUpdatedControl ControlID="grdInitiator" LoadingPanelID="LoadingPanel1" ></telerik:AjaxUpdatedControl> 
                    <telerik:AjaxUpdatedControl ControlID="grdInitiator_Advance" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl> 
                     <telerik:AjaxUpdatedControl ControlID="radMessage" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>                    
                    <telerik:AjaxUpdatedControl ControlID="gvRequest" LoadingPanelID="" ></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" LoadingPanelID="" ></telerik:AjaxUpdatedControl>
                 <%--   <telerik:AjaxUpdatedControl ControlID="lblINRAmt" LoadingPanelID="" ></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="lblProposedAmt" LoadingPanelID="" ></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="Label3" LoadingPanelID="" ></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="Label5" LoadingPanelID="" ></telerik:AjaxUpdatedControl>--%>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid4" LoadingPanelID="" ></telerik:AjaxUpdatedControl>                    
                  </UpdatedControls>
            </telerik:AjaxSetting>
            
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="LoadingPanel1" BackgroundPosition="Center" runat="server" Height="100%" Width="75px" Transparency="50">
         <div class="dialog-background">
            <div class="dialog-loading-wrapper">
                <span class="dialog-loading-icon"></span>
            </div>
        </div>
    </telerik:RadAjaxLoadingPanel>
    <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager" runat="server" EnableShadow="true">
        <Windows>
            <telerik:RadWindow ID="RadWindow" RenderMode="Lightweight" runat="server" CssClass="window123" VisibleStatusbar="false"
                Width="700%" Height="550" AutoSize="false" Behaviors="Close" ShowContentDuringLoad="false"
                Modal="true" ReloadOnShow="true" />
        </Windows>
    </telerik:RadWindowManager>
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManagerPopup" runat="server" EnableShadow="true">
        <Windows>
            <telerik:RadWindow ID="RadWindow1" RenderMode="Lightweight" runat="server"  VisibleStatusbar="false"
                Width="1300%" Height="400px" AutoSize="false" Behaviors="Close" ShowContentDuringLoad="false"
                Modal="true" ReloadOnShow="true" />
        </Windows>
    </telerik:RadWindowManager>

    <div class="row margin-0">
        <div class="col-xs-6 heading-big">
            <h5 class="margin-0 lineheight-42 breath-ctrl">Home / <span>Against Bill Initiator</span></h5>
        </div>

    </div>

    <!--End of heading wrapper-->

    <div class="col-md-12 margin-10">
        <div class="col-sm-8 padding-0">
            <div class="col-sm-5 padding-0">
                <div class="col-sm-3 col-xs-12 padding-0">
                    <asp:Label ID="lblCode" runat="server" CssClass="control-label lable-txt">Company</asp:Label>
                </div>
                <div class="col-sm-9 col-xs-12" style="padding-left:0px">
                    <div class="form-group">
                        <telerik:RadComboBox CausesValidation="false" RenderMode="Lightweight" ID="cmbCompany" runat="server" Height="200" Width="305" AutoPostBack="true"
                            EmptyMessage="Select Company Code"  Filter="Contains" MarkFirstMatch="true" EnableLoadOnDemand="false" OnSelectedIndexChanged="cmbCompany_SelectedIndexChanged" >
                        </telerik:RadComboBox>

                    </div>

                </div>
                <div class="col-sm-12" style="padding: 5px;">
                    <div class=" col-sm-12 text-info small">
                        Note:Items blocked for payment are not considered. 
                    </div>      
                </div>
            </div>
            <div class="col-sm-4 padding-0">
                <div class="col-sm-3 col-xs-12 padding-0">
                    <asp:Label ID="lblVendor" runat="server" CssClass="control-label lable-txt">Vendor</asp:Label>
                </div>
                <div class="col-sm-7 col-xs-12 padding-0">
                    <div class="form-group">

                        <telerik:RadComboBox CausesValidation="false" RenderMode="Lightweight" ID="cmbVendor" runat="server" Height="200" Width="305" AutoPostBack="true"
                         EmptyMessage="Select Vendor" Enabled="false"   Filter="Contains" MarkFirstMatch="true" EnableLoadOnDemand="false">
                        </telerik:RadComboBox>
                        
                    </div>                                              
                </div>
                <div class="col-sm-2 col-xs-12 padding-1">
                        <%--<button id="btnSerachPopUp" onclick="return validateCompCode();" type="button" data-toggle="modal" style="padding:2px,0px !important" class="btn btn-grey" data-target="#squarespaceVendorSearch">
                            <span class="glyphicon glyphicon-search"></span>
                        </button>  --%>                                               
                    <button id="btnSerachPopUp" onclick="validateCompCode()" type="button" style="padding:2px,0px !important" class="btn btn-grey">
                            <span class="glyphicon glyphicon-search"></span>
                       </button>  
                      </div>

            </div>
            <div class="col-sm-3  padding-0">

                <div class="col-sm-12 col-xs-12 padding-0">
                    <div class="col-xs-12 agnstbillradio" id="divbtnOptions">
                       <%-- <asp:RadioButtonList ID="btnOptions" runat="server" OnSelectedIndexChanged="btnOptions_SelectedIndexChanged" RepeatDirection="Horizontal" AutoPostBack="true">
                            <asp:ListItem Text="Against Bill" Value="Against"></asp:ListItem>
                            <asp:ListItem Text="Advance" Value="Advance"></asp:ListItem>
                        </asp:RadioButtonList>--%>
                         <asp:RadioButtonList ID="btnOptions" runat="server"  RepeatDirection="Horizontal" AutoPostBack="false" >
                            <asp:ListItem Text="Against Bill" Value="Against"></asp:ListItem>
                            <asp:ListItem Text="Advance" Value="Advance"></asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-sm-4 padding-0" >
            <div class="row margin-0" >
                <div class="button-wrapper margin-0" >
                    <ul>
                        <li>
                            <asp:Button CssClass="btn btn-grey" Text="Search" OnClick="lbSearch_Click" runat="server" ID="lbSearch"></asp:Button>
                        </li>

                        <li>
                            <asp:Button CssClass="btn btn-grey" Text="Get Data" OnClick="btnAddMore_Click" runat="server" ID="btnAddMore" ></asp:Button>
                        </li>

                        <li>
                           <%-- <button type="button" data-toggle="modal" class="btn btn-grey" data-target="#squarespaceWorkFlowModal">Request Under Process
                            </button>--%>
                            <asp:Button CssClass="btn btn-grey" Text="Request Under Process" OnClick="btnRequestUnderPorcess_Click" runat="server" ID="btnRequestUnderPorcess" />
                        </li>
                    </ul>
                </div>
            </div>

        </div>

        <!-- End of Company name-row-->

        <div class="row margin-10" runat="server" id="pnlAgainstBill">

            <div class="col-xs-12 padding-0" style="overflow: hidden;">
                <div class="panel with-nav-tabs panel-default border-0">
                    <div class="panel-heading padding-0 nocolor">
                        <div class="row margin-0">
                            <div class="col-md-12" style="padding-left: 0px; padding-right: 5px">

                                <div class="col-md-8" style="padding-left: 0px">
                                <telerik:RadTabStrip  RenderMode="Lightweight" runat="server" ID="RadTabStrip1" MultiPageID="RadMultiPage1" SelectedIndex="0" Skin="Silk">
                                    <Tabs>
                                        <telerik:RadTab Text="My Request" CssClass="nav-tabs"></telerik:RadTab>
                                        <telerik:RadTab Text="In Process"></telerik:RadTab>
                                        <telerik:RadTab Text="Need Correction"></telerik:RadTab>
                                        <telerik:RadTab Text="Saved Request"></telerik:RadTab>
                                    </Tabs>
                                </telerik:RadTabStrip>
                               </div>      
                               <div  class="col-md-4" >

                                   <div  class="col-md-12">
                                       <div class="col-md-10 text-info small">                                         
                                            <div style="margin:auto!important">
                                            <asp:Label runat="server" ID="lblINRAmtText" Text="INR Total:" CssClass="control-label lable-txt  pull-left"></asp:Label>
                                           </div>
                                           <div style="margin:auto!important">
                                            <asp:Label ID="lblINRAmt" runat="server" CssClass="control-label lable-txt pull-left" Text="0"></asp:Label>                                         
                                            </div>
                                           <div style="margin:auto!important;">
                                           <asp:Label runat="server" style="padding-left: 10px;" ID="lblProposedAmtText" CssClass="control-label lable-txt pull-left" Text="Proposed Total:"></asp:Label>                                   
                                            </div>
                                           <div style="margin:auto!important">
                                            <asp:Label ID="lblProposedAmt" runat="server" CssClass="control-label lable-txt pull-left" Text="0"></asp:Label>
                                            </div>
                                          
                                        </div>
                                       <div  class="col-md-2">
                                            <ul class="col-xs-2 pull-right nav" style="padding: 0px 0px 0px 5px;">
                                            <li>
                                            <asp:Button class="btn btn-grey" ID="btnAgainstExport" CommandName="Against" OnClick="btnAgainstExport_Click" runat="server" CssClass="pull-right btn btn-grey button button-style" Text="Export"></asp:Button>
                                            </li>                                  
                                            </ul>                              
                                        </div>

                                    </div> 
                                   
                                   </div>                          
                               <%--<div class="col-md-1">
                                <ul class="col-xs-2 pull-right nav" style="padding: 0px 0px 0px 5px;">                                  
                                   <li>
                                        <asp:Button class="btn btn-grey" ID="btnAgainstExport" CommandName="Against" OnClick="btnAgainstExport_Click" runat="server" CssClass="pull-right btn btn-grey button button-style" Text="Export"></asp:Button>
                                    </li>
                                </ul>
                             </div>--%>
                            </div>
                        </div>
                    </div>
                    <div class="panel-body grid-wrapper">
                        <div class="tab-content">
                            <telerik:RadMultiPage runat="server" ID="RadMultiPage1" SelectedIndex="0" CssClass="outerMultiPage">
                                <telerik:RadPageView runat="server" ID="RadPageView1">
                                    <telerik:RadGrid RenderMode="Lightweight" OnExcelMLExportRowCreated="gvRequest_ExcelMLExportRowCreated" ExportSettings-ExportOnlyData="true" ID="gvRequest" runat="server" AutoGenerateColumns="false" OnItemDataBound="gvRequest_ItemDataBound"
                                        AllowPaging="false" AllowSorting="true"  AllowFilteringByColumn="true" AllowMultiRowSelection="true" OnItemCommand="gvRequest_ItemCommand"  OnNeedDataSource="gvRequest_NeedDataSource" OnExcelMLExportStylesCreated="gvRequest_ExcelMLExportStylesCreated">
                                        <ExportSettings IgnorePaging="true" Excel-Format="ExcelML" ExportOnlyData="true" />
                                        <GroupingSettings CaseSensitive="false" />
                                        <ClientSettings>
                                            <Selecting AllowRowSelect="true"></Selecting>
                                            <ClientEvents OnRowSelected="RowSelected" OnRowDeselected="RowDeselected" />
                                            <Scrolling AllowScroll="True" UseStaticHeaders="True" ScrollHeight="275px" SaveScrollPosition="true" FrozenColumnsCount="3"></Scrolling>
                                        </ClientSettings>
                                        <MasterTableView CommandItemSettings-ShowRefreshButton="false" EditMode="InPlace" CommandItemDisplay="None"
                                            AllowFilteringByColumn="true" AllowSorting="true" NoMasterRecordsText="" DataKeyNames="SAPAgainstBillPaymentId,SaveMode" TableLayout="Fixed" Font-Size="8">
                                            <HeaderStyle Height="36px" />

                                            <CommandItemSettings ShowRefreshButton="false" />
                                            <NoRecordsTemplate>
                                                <table width="100%" border="0" cellpadding="20" cellspacing="20">
                                                    <tr>
                                                        <td align="center">No records to display.
                                                        </td>
                                                    </tr>
                                                </table>
                                            </NoRecordsTemplate>
                                            <Columns>
                                                <telerik:GridClientSelectColumn HeaderStyle-Width="70px" HeaderStyle-Height="36px" HeaderText="Select" UniqueName="RequestSelect">
                                                </telerik:GridClientSelectColumn>
                                                   <telerik:GridBoundColumn HeaderText="Vendor Code" HeaderStyle-Width="100px" HeaderStyle-Height="36px" UniqueName="VendorCode" DataField="VendorCode">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn HeaderText="Vendor Name" HeaderStyle-Width="170px" HeaderStyle-Height="36px" UniqueName="VendorName" DataField="VendorName">
                                                </telerik:GridBoundColumn>
                                                 <telerik:GridBoundColumn HeaderText="Document Number" HeaderStyle-Width="120px" HeaderStyle-Height="36px" UniqueName="DRPNumber" DataField="DocumentNumber">
                                                </telerik:GridBoundColumn>       
                                                <telerik:GridBoundColumn HeaderText="Reference" HeaderStyle-Width="150px" HeaderStyle-Height="36px" UniqueName="Reference" DataField="Reference">
                                                </telerik:GridBoundColumn>   
                                                 <telerik:GridBoundColumn  HeaderStyle-Width="80px" HeaderText="Debit/Credit" UniqueName="DCFLag" DataField="DCFLag" >
                                                  </telerik:GridBoundColumn>                                                                                   
                                                <%--<telerik:GridNumericColumn DataField="Amount" HeaderText="Test" DataFormatString="{0:###,##0.00}" />--%>
                                                  <telerik:GridBoundColumn   HeaderText="Amount in INR" UniqueName="Amount" HeaderStyle-Width="170px" HeaderStyle-Height="36px" DataField="Amount" DataFormatString="{0:###,##0.00}">
                                                </telerik:GridBoundColumn>                                                
                                                 <telerik:GridTemplateColumn  HeaderText="Amount Proposed" HeaderStyle-Width="170px" HeaderStyle-Height="36px" UniqueName="AmountProposed" Display="false">
                                                    <ItemTemplate>
                                                        <telerik:RadNumericTextBox ID="tbAmountProposed" Text='<%#String.Format("{0:###,##0.00}",Eval("AmountProposed"))%>' Width="125px" class="form-control" runat="server"></telerik:RadNumericTextBox>
                                                        <%--<asp:RequiredFieldValidator Enabled="false" ID="rfvAmtProposed" runat="server" ControlToValidate="tbAmountProposed" ForeColor="Red" ErrorMessage="* Required"></asp:RequiredFieldValidator>--%>
                                                        <asp:Label ID="lblAmountProposed" runat="server" Style="display: none" Text='<%#Eval("AmountProposed") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                 <telerik:GridBoundColumn  HeaderText="Amount Proposed" UniqueName="AmountProposed1" HeaderStyle-Width="170px" HeaderStyle-Height="36px" DataField="AmountProposed" DataFormatString="{0:###,##0.00}">
                                                </telerik:GridBoundColumn>  

                                               <telerik:GridBoundColumn HeaderText="Document Currency" HeaderStyle-Width="100px" HeaderStyle-Height="36px" UniqueName="Currency" DataField="Currency">
                                                </telerik:GridBoundColumn>
                                                  <telerik:GridDateTimeColumn HeaderStyle-Width="150px" HeaderStyle-Height="36px" HeaderText="Net due date" EnableTimeIndependentFiltering="true" FilterControlWidth="130px" UniqueName="RequestNetduedate"
                                                    DataType="System.DateTime" DataFormatString="{0:dd-MM-yyyy}" DataField="BaseLineDate" Visible="true">
                                                </telerik:GridDateTimeColumn>
                                                  <telerik:GridTemplateColumn HeaderText="Nature of Request" HeaderStyle-Width="170px" HeaderStyle-Height="36px" UniqueName="NatureOfRequest" ItemStyle-VerticalAlign="Middle" Display="false">
                                                    <ItemTemplate>
                                                        <telerik:RadDropDownList DefaultMessage="-Select-" RenderMode="Lightweight" HeaderStyle-Width="170px" HeaderStyle-Height="36px" ID="cmbNatureofRequest" runat="server">
                                                        </telerik:RadDropDownList>
                                                       <%--<asp:RequiredFieldValidator  ID="rfvNatureOfReq" runat="server" ControlToValidate="cmbNatureofRequest" ForeColor="Red" ErrorMessage="* Required"></asp:RequiredFieldValidator>--%>
                                                        <asp:Label ID="lblRequestNatureofRequest" runat="server" Style="display: none"></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                   <telerik:GridBoundColumn HeaderText="Nature of Request" HeaderStyle-Width="150px" HeaderStyle-Height="36px" UniqueName="NatureOfRequest1" DataField="">
                                                </telerik:GridBoundColumn>                           

                                                          <telerik:GridTemplateColumn HeaderStyle-Width="170px" HeaderStyle-Height="36px" HeaderText="Remarks" UniqueName="Remarks" Display="false">
                                                    <ItemTemplate>
                                                        <telerik:RadTextBox ID="tbRequestRemarks" TextMode="MultiLine" Text='<%#Eval("comment") %>' runat="server"></telerik:RadTextBox>
                                                        <%--<asp:RequiredFieldValidator ID="rfvRequestRemarks" Enabled="false" runat="server" ControlToValidate="tbRequestRemarks" ForeColor="Red" ErrorMessage="* Required"></asp:RequiredFieldValidator>--%>
                                                        <asp:Label ID="lblRequestRemarks" runat="server" Style="display: none"></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                  <telerik:GridBoundColumn HeaderText="Remarks" HeaderStyle-Width="150px" HeaderStyle-Height="36px" UniqueName="Remarks1" DataField="comment">
                                                </telerik:GridBoundColumn>

                                                <telerik:GridTemplateColumn HeaderStyle-Width="120px" HeaderStyle-Height="36px" HeaderText="Attachment" AllowFiltering="false" AllowSorting="false" UniqueName="Attachments">
                                                    <ItemTemplate>
                                                        <asp:Button ID="btnAttachment" runat="server" CommandName="Attachment" CommandArgument='<%# Eval("SAPAgainstBillPaymentId") %>' Text="Add/View" CssClass="btn btn-grey"></asp:Button>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                   <telerik:GridDateTimeColumn HeaderText="Posting Date" HeaderStyle-Width="150px" HeaderStyle-Height="36px" FilterControlWidth="130px" UniqueName="PostDate" EnableTimeIndependentFiltering="true"
                                                    DataType="System.DateTime" DataFormatString="{0:dd-MM-yyyy}" DataField="PostDate">
                                                </telerik:GridDateTimeColumn>
                                                 <telerik:GridBoundColumn HeaderText="Fiscal Year" HeaderStyle-Width="100px" HeaderStyle-Height="36px" UniqueName="FiscalYear" DataField="FiscalYear">
                                                </telerik:GridBoundColumn>
                                                   <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderStyle-Height="36px" HeaderText="Profit Centre" UniqueName="ProfitCentre" DataField="ProfitCentre">
                                                </telerik:GridBoundColumn>
                                                 <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderStyle-Height="36px" HeaderText="Business Area" UniqueName="BusinessArea" DataField="BusinessArea">
                                                </telerik:GridBoundColumn>
                                                 <telerik:GridBoundColumn HeaderText="Company Code" HeaderStyle-Width="100px" HeaderStyle-Height="36px" UniqueName="CompanyCode" DataField="CompanyCode">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridTemplateColumn HeaderStyle-Width="170px" HeaderStyle-Height="36px" HeaderText="Sub Vertical" UniqueName="SubVertical" Display="false">
                                                    <ItemTemplate>
                                                        <telerik:RadDropDownList AutoPostBack="true" OnSelectedIndexChanged="cmbSubVertical_SelectedIndexChanged" RenderMode="Lightweight" ID="cmbSubVertical" runat="server">
                                                        </telerik:RadDropDownList>
                                                        <asp:Label ID="lblRequestSubVertical" runat="server" Style="display: none"></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>   
                                                  <telerik:GridBoundColumn HeaderText="Sub Vertical" HeaderStyle-Width="150px" HeaderStyle-Height="36px" UniqueName="SubVertical1" DataField="">
                                                </telerik:GridBoundColumn>
                                                                                 
                                                      <telerik:GridTemplateColumn HeaderStyle-Width="150px" HeaderStyle-Height="36px" HeaderText="Vertical" UniqueName="Vertical" Display="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbVerticalRequest" runat="server"></asp:Label>
                                                        <asp:Label ID="lblVerticalIDRequest" Style="display: none" runat="server"></asp:Label>
                                                        <asp:Label ID="Label1" Style="display: none" runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>                                            
                                              
                                                    <telerik:GridDateTimeColumn HeaderStyle-Width="170px" HeaderStyle-Height="36px" HeaderText="BaseLineDate" EnableTimeIndependentFiltering="true" FilterControlWidth="130px" UniqueName="BaseLineDate"
                                                    DataType="System.DateTime" DataFormatString="{0:dd-MM-yyyy}" DataField="BaseLineDate" Display="false">
                                                </telerik:GridDateTimeColumn>
                                                  <telerik:GridBoundColumn  HeaderText="DueDays" UniqueName="DueDays" DataField="DueDays" Display="false">
                                                </telerik:GridBoundColumn>                                         
                                                <telerik:GridTemplateColumn HeaderStyle-Width="130px" HeaderStyle-Height="36px" HeaderText="Status" UniqueName="Status">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblstatus" Text="Draft" runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                               
                                                
                                            </Columns>
                                            <PagerStyle PageSizes="5,10,15" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                                                PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />
                                        </MasterTableView>
                                    </telerik:RadGrid>

                                    <div class="col-lg-12 padding-0 margin-10">

                                        <asp:LinkButton CssClass="btn btn-grey" OnClick="btnRequestSave_Click" runat="server" CommandArgument="Save" ID="btnRequestSave">Save</asp:LinkButton>
                                        <asp:LinkButton OnClientClick="return confirm('Are you sure you want proceed?');" CssClass="btn btn-grey" OnClick="btnRequestSave_Click" CommandArgument="Submit" runat="server" ID="btnRequestSubmit">Submit</asp:LinkButton>

                                    </div>
                                </telerik:RadPageView>

                                <telerik:RadPageView runat="server" ID="RadPageView2">
                                    <telerik:RadGrid RenderMode="Lightweight" ExportSettings-ExportOnlyData="true" ID="gvIntitiatorInProcess" runat="server" AutoGenerateColumns="false" OnExcelMLExportRowCreated="gvIntitiatorInProcess_ExcelMLExportRowCreated"
                                        OnItemDataBound="gvIntitiatorInProcess_ItemDataBound" OnItemCommand="gvIntitiatorInProcess_ItemCommand" OnExcelMLExportStylesCreated="gvIntitiatorInProcess_ExcelMLExportStylesCreated" AllowPaging="false" AllowSorting="true" AllowFilteringByColumn="true" AllowMultiRowSelection="true"  OnNeedDataSource="gvIntitiatorInProcess_NeedDataSource">
                                        <ExportSettings IgnorePaging="true" Excel-Format="ExcelML" ExportOnlyData="true" />
                                        <GroupingSettings CaseSensitive="false" />
                                        <ClientSettings>
                                            <%--  <Selecting AllowRowSelect="true"></Selecting>        --%>
                                            <Scrolling AllowScroll="True" ScrollHeight="275px" UseStaticHeaders="True" SaveScrollPosition="true" FrozenColumnsCount="3"></Scrolling>
                                        </ClientSettings>

                                        <MasterTableView CommandItemSettings-ShowRefreshButton="false" EditMode="InPlace" CommandItemDisplay="None"
                                            AllowFilteringByColumn="true" AllowSorting="true" NoMasterRecordsText="" DataKeyNames="SAPAgainstBillPaymentId"
                                            TableLayout="Fixed" Font-Size="8">
                                            <HeaderStyle Height="36px" />
                                            <CommandItemSettings ShowRefreshButton="false" />
                                            <NoRecordsTemplate>
                                                <table width="100%" border="0" cellpadding="20" cellspacing="20">
                                                    <tr>
                                                        <td align="center">No records to display.
                                                        </td>
                                                    </tr>
                                                </table>
                                            </NoRecordsTemplate>
                                            <Columns>
                                                <telerik:GridBoundColumn HeaderStyle-Width="170px" HeaderText="Status" UniqueName="Status" DataField="Status">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn HeaderStyle-Width="100px" HeaderText="Vendor Code" UniqueName="VendorCode" DataField="VendorCode">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="Vendor Name" UniqueName="VendorName" DataField="VendorName">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn HeaderStyle-Width="120px" HeaderText="Document Number" UniqueName="DRPNumber" DataField="DocumentNumber">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="Reference" UniqueName="Reference" DataField="Reference">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn HeaderStyle-Width="150px"  HeaderText="Amount in INR" UniqueName="Amount" DataField="Amount" DataFormatString="{0:###,##0.00}">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn HeaderStyle-Width="150px"  HeaderText="Amount Proposed" UniqueName="AmountProposed" DataField="AmountProposed" DataFormatString="{0:###,##0.00}">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn HeaderStyle-Width="150px"  HeaderText="Amount Approved" UniqueName="ApprovedAmount" DataField="ApprovedAmount" DataFormatString="{0:###,##0.00}">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn HeaderStyle-Width="120px" HeaderText="Document Currency" UniqueName="Currency" DataField="Currency">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridDateTimeColumn HeaderStyle-Width="150px" HeaderStyle-Height="36px" HeaderText="Net due date" EnableTimeIndependentFiltering="true" FilterControlWidth="130px" UniqueName="RequestNetduedate"
                                                DataType="System.DateTime" DataFormatString="{0:dd-MM-yyyy}" DataField="BaseLineDate" Visible="true">
                                                </telerik:GridDateTimeColumn>
                                                <telerik:GridDateTimeColumn HeaderStyle-Width="150px" HeaderStyle-Height="36px" HeaderText="BaseLineDate" EnableTimeIndependentFiltering="true" FilterControlWidth="130px" UniqueName="BaseLineDate"
                                                DataType="System.DateTime" DataFormatString="{0:dd-MM-yyyy}" DataField="BaseLineDate" Display="false">
                                                </telerik:GridDateTimeColumn>
                                                <telerik:GridBoundColumn HeaderStyle-Width="100px" HeaderStyle-Height="36px" HeaderText="DueDays" UniqueName="DueDays" DataField="DueDays" Display="false">
                                                </telerik:GridBoundColumn>  
                                                   <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="Nature of Request" UniqueName="NatureOfRequest" DataField="NatureOfRequest">
                                                </telerik:GridBoundColumn>
                                                 <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="Remarks" UniqueName="Remarks" DataField="Comment">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridTemplateColumn HeaderText="Attachment" AllowFiltering="false" AllowSorting="false" UniqueName="Attachments">
                                                <ItemTemplate>
                                                <asp:Button ID="btnAttachment" CssClass="btn btn-grey" runat="server" CommandName="Attachment" CommandArgument='<%# Eval("SAPAgainstBillPaymentId") %>' Text="Add/View"></asp:Button>
                                                </ItemTemplate>
                                                </telerik:GridTemplateColumn>                                                
                                                <telerik:GridDateTimeColumn HeaderStyle-Width="150px" HeaderText="Posting Date" FilterControlWidth="100px" UniqueName="PostDate" EnableTimeIndependentFiltering="true"
                                                DataType="System.DateTime" DataFormatString="{0:dd-MM-yyyy}" DataField="Posting Date">
                                                <HeaderStyle Width="180px" />
                                                </telerik:GridDateTimeColumn>  
                                                <telerik:GridBoundColumn HeaderStyle-Width="110px" HeaderText="Fiscal Year" UniqueName="FiscalYear" DataField="FiscalYear">
                                                </telerik:GridBoundColumn>
                                                  <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="Profit Centre" UniqueName="ProfitCentre" DataField="ProfitCentre">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="Business Area" UniqueName="BusinessArea" DataField="BusinessArea">
                                                </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn HeaderStyle-Width="100px" HeaderText="Company Code" UniqueName="CompanyCode" DataField="CompanyCode">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="SubVertical" UniqueName="SubVertical" DataField="SubVertical">
                                                </telerik:GridBoundColumn> 
                                                <%--<telerik:GridDateTimeColumn HeaderStyle-Width="170px" PickerType="DateTimePicker" HeaderText="Net due date" FilterControlWidth="80px" UniqueName="Netduedate"
                                                    DataType="System.DateTime" EnableTimeIndependentFiltering="true" DataFormatString="{0:dd-MMM-yyyy}" DataField="DocumentDate">
                                                    <HeaderStyle Width="200px" />
                                                </telerik:GridDateTimeColumn>--%>
                                                <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="Vertical" UniqueName="Vertical" DataField="Vertical" Visible="false">
                                                </telerik:GridBoundColumn>    
                                                <telerik:GridDateTimeColumn HeaderText="Submission Date" HeaderStyle-Width="150px" FilterControlWidth="80px" UniqueName="SubmitDate" EnableTimeIndependentFiltering="true"
                                                    DataType="System.DateTime" DataFormatString="{0:dd-MM-yyyy}" DataField="SubmitDate">
                                                </telerik:GridDateTimeColumn>
                                                <telerik:GridTemplateColumn HeaderStyle-Width="150px" UniqueName="TemplateComment" HeaderText="Comments" AllowFiltering="false" AllowSorting="false">
                                                    <ItemTemplate>
                                                        <asp:HyperLink ID="viewComment" runat="server" Text="View" CssClass="btn btn-grey gridHyperlinks"></asp:HyperLink>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>

                                            </Columns>
                                            <PagerStyle PageSizes="5,10,15" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                                                PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />
                                        </MasterTableView>
                                    </telerik:RadGrid>
                                </telerik:RadPageView>

                                <telerik:RadPageView runat="server" ID="RadPageView3">
                                    <telerik:RadGrid RenderMode="Lightweight" ExportSettings-ExportOnlyData="true" ID="gvInitiatorNeedCorrection" runat="server" AutoGenerateColumns="false" OnExcelMLExportStylesCreated="gvInitiatorNeedCorrection_ExcelMLExportStylesCreated"
                                        OnItemDataBound="gvInitiatorNeedCorrection_ItemDataBound" AllowPaging="false" AllowSorting="true" AllowFilteringByColumn="true" AllowMultiRowSelection="true" 
                                        OnNeedDataSource="gvInitiatorNeedCorrection_NeedDataSource" OnItemCommand="gvInitiatorNeedCorrection_ItemCommand" 
                                        OnExcelMLExportRowCreated="gvInitiatorNeedCorrection_ExcelMLExportRowCreated">
                                        <ExportSettings IgnorePaging="true" Excel-Format="ExcelML" ExportOnlyData="true" />
                                        <GroupingSettings CaseSensitive="false" />
                                        <ClientSettings>
                                            <Selecting AllowRowSelect="true"></Selecting>
                                            <ClientEvents OnRowSelected="RowSelectedNeedCorrection" OnRowDeselected="RowDeselectedNeedCorrection" />
                                            <Scrolling AllowScroll="True" ScrollHeight="275px" UseStaticHeaders="True" SaveScrollPosition="true" FrozenColumnsCount="3"></Scrolling>
                                        </ClientSettings>
                                        <MasterTableView CommandItemSettings-ShowRefreshButton="false" EditMode="InPlace" CommandItemDisplay="None"
                                            AllowFilteringByColumn="true" AllowSorting="true" NoMasterRecordsText="" DataKeyNames="SAPAgainstBillPaymentId"
                                            TableLayout="Fixed" Font-Size="8">
                                            <HeaderStyle Width="170px" />
                                            <CommandItemSettings ShowRefreshButton="false" />
                                            <HeaderStyle Height="36px" />
                                            <NoRecordsTemplate>
                                                <table width="100%" border="0" cellpadding="20" cellspacing="20">
                                                    <tr>
                                                        <td align="center">No records to display.
                                                        </td>
                                                    </tr>
                                                </table>
                                            </NoRecordsTemplate>
                                            <Columns>
                                                <telerik:GridClientSelectColumn HeaderText="Select" UniqueName="RequestNeedCorrectionSelect">
                                                    <HeaderStyle Width="70px" />
                                                </telerik:GridClientSelectColumn>
                                                <telerik:GridBoundColumn HeaderText="Vendor Code" UniqueName="VendorCodeNeedCorrection" DataField="VendorCode">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn HeaderText="Vendor Name" UniqueName="VendorNameNeedCorrection" DataField="VendorName">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn HeaderText="Document Number" UniqueName="DRPNumberNeedCorrection" DataField="DocumentNumber">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn HeaderText="Reference" HeaderStyle-Width="170px" UniqueName="ReferenceNeedCorrection" DataField="Reference">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn HeaderStyle-Width="80px" HeaderText="Debit/Credit" UniqueName="DCFLag" DataField="DCFLag" >
                                            </telerik:GridBoundColumn> 
                                                <telerik:GridBoundColumn HeaderText="Amount in INR"  UniqueName="AmountNeedCorrection" DataField="Amount" DataFormatString="{0:###,##0.00}">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridTemplateColumn HeaderText="Amount Proposed"  UniqueName="AmountProposedNeedCorrection">
                                                    <ItemTemplate>
                                                        <telerik:RadNumericTextBox ID="tbAmountProposedNeedCorrection" Text='<%#String.Format("{0:###,##0.00}",Eval("AmountProposed")) %>' Width="125px" class="form-control" runat="server"></telerik:RadNumericTextBox>
                                                        <asp:RequiredFieldValidator Enabled="false" ID="rfvAmtProposedNeedCorrection" runat="server" ControlToValidate="tbAmountProposedNeedCorrection" ForeColor="Red" ErrorMessage="* Required"></asp:RequiredFieldValidator>
                                                        <asp:Label ID="lblAmountProposedNeedCorrection" runat="server" Style="display: none" Text='<%#Eval("AmountProposed") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                    <telerik:GridBoundColumn HeaderStyle-Width="150px"  HeaderText="Amount Approved" UniqueName="ApprovedAmount" DataField="ApprovedAmount" DataFormatString="{0:###,##0.00}">
                                                </telerik:GridBoundColumn>
                                                   <telerik:GridBoundColumn HeaderText="Document Currency" HeaderStyle-Width="100px" UniqueName="CurrencyNeedCorrection" DataField="Currency">
                                                </telerik:GridBoundColumn>
                                                 <telerik:GridDateTimeColumn HeaderStyle-Width="150px" HeaderStyle-Height="36px" HeaderText="Net due date" EnableTimeIndependentFiltering="true" FilterControlWidth="130px" UniqueName="RequestNetduedate"
                                                DataType="System.DateTime" DataFormatString="{0:dd-MM-yyyy}" DataField="BaseLineDate" Visible="true">
                                                </telerik:GridDateTimeColumn>
                                                <telerik:GridDateTimeColumn HeaderStyle-Width="150px" HeaderStyle-Height="36px" HeaderText="BaseLineDate" EnableTimeIndependentFiltering="true" FilterControlWidth="130px" UniqueName="BaseLineDate"
                                                DataType="System.DateTime" DataFormatString="{0:dd-MM-yyyy}" DataField="BaseLineDate" Display="false">
                                                </telerik:GridDateTimeColumn>
                                                <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderStyle-Height="36px" HeaderText="DueDays" UniqueName="DueDays" DataField="DueDays" Display="false">
                                                </telerik:GridBoundColumn>  
                                                <telerik:GridTemplateColumn HeaderText="Nature of Request" UniqueName="NatureOfRequestNeedCorrection">
                                                <ItemTemplate>
                                                <telerik:RadDropDownList RenderMode="Lightweight" ID="cmbNatureofRequestNeedCorrection" runat="server">
                                                </telerik:RadDropDownList>
                                                <asp:Label ID="lblRequestNatureofRequestNeedCorrection" runat="server" Style="display: none"></asp:Label>
                                                </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                 <telerik:GridTemplateColumn HeaderText="Remarks" ItemStyle-VerticalAlign="Middle" UniqueName="RemarksNeedCorrection" DataField="Comment">
                                                    <ItemTemplate>
                                                        <telerik:RadTextBox ID="tbRequestRemarksNeedCorrection" Text='<%#Eval("Comment") %>' TextMode="MultiLine" runat="server"></telerik:RadTextBox>
                                                        <asp:RequiredFieldValidator ID="rfvRequestRemarksNeedCorrection" Enabled="false" runat="server" ControlToValidate="tbRequestRemarksNeedCorrection" ForeColor="Red" ErrorMessage="* Required"></asp:RequiredFieldValidator>
                                                        <asp:Label ID="lblRequestRemarksNeedCorrection" Text='<%#Eval("Comment") %>' runat="server" Style="display: none"></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                 <telerik:GridTemplateColumn HeaderText="Attachment" AllowFiltering="false" AllowSorting="false" UniqueName="Attachments">
                                                    <ItemTemplate>
                                                        <asp:Button ID="btnAttachment" runat="server" CommandName="Attachment" CommandArgument='<%# Eval("SAPAgainstBillPaymentId") %>' Text="Add/View" CssClass="btn btn-grey"></asp:Button>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                     <telerik:GridDateTimeColumn HeaderText="Posting Date" FilterControlWidth="150px" UniqueName="PostDateNeedCorrection"
                                                    EnableTimeIndependentFiltering="true" DataType="System.DateTime" DataFormatString="{0:dd-MM-yyyy}" DataField="PostDate">
                                                </telerik:GridDateTimeColumn>
                                                     <telerik:GridBoundColumn HeaderText="Fiscal Year" UniqueName="FiscalYearNeedCorrection" DataField="FiscalYear">
                                                </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn HeaderText="Profit Centre" UniqueName="ProfitCentreNeedCorrection" DataField="ProfitCentre">
                                                </telerik:GridBoundColumn>
                                                 <telerik:GridBoundColumn HeaderText="Business Area" UniqueName="BusinessAreaNeedCorrection" DataField="BusinessArea">
                                                </telerik:GridBoundColumn>
                                                     <telerik:GridBoundColumn HeaderText="Company Code" UniqueName="CompanyCodeNeedCorrection" DataField="CompanyCode">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridTemplateColumn HeaderText="Sub Vertical" UniqueName="SubVerticalNeedCorrection">
                                                    <ItemTemplate>
                                                        <telerik:RadDropDownList AutoPostBack="true" OnSelectedIndexChanged="cmbSubVerticalNeedCorrection_SelectedIndexChanged" RenderMode="Lightweight" ID="cmbSubVerticalNeedCorrection" runat="server">
                                                        </telerik:RadDropDownList>
                                                        <asp:Label ID="lblRequestSubVerticalNeedCorrection" runat="server" Style="display: none"></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                 <telerik:GridDateTimeColumn HeaderText="Submission Date" FilterControlWidth="150px" HeaderStyle-Width="170px" UniqueName="SubmitDate" EnableTimeIndependentFiltering="true"
                                                    DataType="System.DateTime" DataFormatString="{0:dd-MM-yyyy}" DataField="SubmitDate">
                                                </telerik:GridDateTimeColumn>
                                                <telerik:GridBoundColumn HeaderText="Assigned By" UniqueName="AssigndBy" DataField="AssignedBy">
                                                </telerik:GridBoundColumn>                                             
                                                <%--<telerik:GridDateTimeColumn PickerType="DateTimePicker" HeaderText="Net due date" FilterControlWidth="130px" UniqueName="NetduedateNeedCorrection"
                                                    DataType="System.DateTime" EnableTimeIndependentFiltering="true" DataFormatString="{0:dd-MMM-yyyy}" DataField="DocumentDate">
                                                </telerik:GridDateTimeColumn>--%>    
                                                <telerik:GridTemplateColumn HeaderText="Vertical" UniqueName="VerticalNeedCorrection" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbVerticalRequestNeedCorrection" runat="server"></asp:Label>
                                                        <asp:Label ID="lblVerticalIDRequestNeedCorrection" Style="display: none" runat="server"></asp:Label>
                                                        <asp:Label ID="Label1NeedCorrection" Style="display: none" runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <%--<telerik:GridTemplateColumn HeaderStyle-Width="170px" HeaderText="Status" UniqueName="StatusNeedCorrection" DataField="Status">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblstatusNeedCorrection" Text="Need Correction" runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>--%>
                                                <telerik:GridTemplateColumn UniqueName="TemplateComment" HeaderText="Comments" HeaderStyle-Width="100px" AllowFiltering="false" AllowSorting="false">
                                                    <ItemTemplate>

                                                        <asp:HyperLink ID="viewComment" runat="server" Text="View" CssClass="btn btn-grey gridHyperlinks"></asp:HyperLink>

                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                  
                                               
                                            </Columns>
                                            <PagerStyle PageSizes="5,10,15" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                                                PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />
                                        </MasterTableView>
                                    </telerik:RadGrid>
                                    <div class="col-lg-12 padding-0 margin-10">
                                        <asp:LinkButton CssClass="btn btn-grey" OnClick="lbSaveBillNeedCorrection_Click" CommandArgument="Save" runat="server" ID="lbSaveBillNeedCorrection">Save</asp:LinkButton>
                                        <asp:LinkButton CssClass="btn btn-grey" OnClick="lbSaveBillNeedCorrection_Click" OnClientClick="return confirm('Are you sure you want proceed?');" CommandArgument="Submit" runat="server" ID="lbSubmitBillNeedCorrection">Submit</asp:LinkButton>
                                    </div>
                                </telerik:RadPageView>

                                <telerik:RadPageView runat="server" ID="RadPageView7">
                                     <telerik:RadGrid RenderMode="Lightweight"  ExportSettings-ExportOnlyData="true" ID="gvAllRequest" runat="server" AutoGenerateColumns="false" OnItemDataBound="gvAllRequest_ItemDataBound"
                                        AllowPaging="false" AllowSorting="true"  AllowFilteringByColumn="true" AllowMultiRowSelection="true" OnItemCommand="gvAllRequest_ItemCommand"  OnNeedDataSource="gvAllRequest_NeedDataSource" >
                                        <ExportSettings IgnorePaging="true" Excel-Format="ExcelML" ExportOnlyData="true" />
                                        <GroupingSettings CaseSensitive="false" />
                                        <ClientSettings>
                                            <Selecting AllowRowSelect="true"></Selecting>
                                            <ClientEvents OnRowSelected="RowSelected" OnRowDeselected="RowDeselected" />
                                            <Scrolling AllowScroll="True" UseStaticHeaders="True" ScrollHeight="275px" SaveScrollPosition="true" FrozenColumnsCount="3"></Scrolling>
                                        </ClientSettings>
                                        <MasterTableView CommandItemSettings-ShowRefreshButton="false" EditMode="InPlace" CommandItemDisplay="None"
                                            AllowFilteringByColumn="true" AllowSorting="true" NoMasterRecordsText="" DataKeyNames="SAPAgainstBillPaymentId,SaveMode" TableLayout="Fixed" Font-Size="8">
                                            <HeaderStyle Height="36px" />

                                            <CommandItemSettings ShowRefreshButton="false" />
                                            <NoRecordsTemplate>
                                                <table width="100%" border="0" cellpadding="20" cellspacing="20">
                                                    <tr>
                                                        <td align="center">No records to display.
                                                        </td>
                                                    </tr>
                                                </table>
                                            </NoRecordsTemplate>
                                            <Columns>
                                                <telerik:GridClientSelectColumn HeaderStyle-Width="70px" HeaderStyle-Height="36px" HeaderText="Select" UniqueName="RequestSelect">
                                                </telerik:GridClientSelectColumn>
                                                   <telerik:GridBoundColumn HeaderText="Vendor Code" HeaderStyle-Width="100px" HeaderStyle-Height="36px" UniqueName="VendorCode" DataField="VendorCode">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn HeaderText="Vendor Name" HeaderStyle-Width="170px" HeaderStyle-Height="36px" UniqueName="VendorName" DataField="VendorName">
                                                </telerik:GridBoundColumn>
                                                 <telerik:GridBoundColumn HeaderText="Document Number" HeaderStyle-Width="120px" HeaderStyle-Height="36px" UniqueName="DRPNumber" DataField="DocumentNumber">
                                                </telerik:GridBoundColumn>       
                                                <telerik:GridBoundColumn HeaderText="Reference" HeaderStyle-Width="150px" HeaderStyle-Height="36px" UniqueName="Reference" DataField="Reference">
                                                </telerik:GridBoundColumn>   
                                                <telerik:GridBoundColumn  HeaderStyle-Width="80px" HeaderText="Debit/Credit" UniqueName="DCFLag" DataField="DCFLag" >
                                            </telerik:GridBoundColumn>                                                                                    
                                                <%--<telerik:GridNumericColumn DataField="Amount" HeaderText="Test" DataFormatString="{0:###,##0.00}" />--%>
                                                  <telerik:GridBoundColumn  HeaderText="Amount in INR" UniqueName="Amount" HeaderStyle-Width="170px" HeaderStyle-Height="36px" DataField="Amount" DataFormatString="{0:###,##0.00}">
                                                </telerik:GridBoundColumn>                                                
                                                 <telerik:GridTemplateColumn   HeaderText="Amount Proposed" HeaderStyle-Width="170px" HeaderStyle-Height="36px" UniqueName="AmountProposed" Display="false">
                                                    <ItemTemplate>
                                                        <telerik:RadNumericTextBox ID="tbAmountProposed" Text='<%#String.Format("{0:###,##0.00}",Eval("AmountProposed"))%>' Width="125px" class="form-control" runat="server"></telerik:RadNumericTextBox>
                                                        <%--<asp:RequiredFieldValidator Enabled="false" ID="rfvAmtProposed" runat="server" ControlToValidate="tbAmountProposed" ForeColor="Red" ErrorMessage="* Required"></asp:RequiredFieldValidator>--%>
                                                        <asp:Label ID="lblAmountProposed" runat="server" Style="display: none" Text='<%#Eval("AmountProposed") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                 <telerik:GridBoundColumn HeaderText="Amount Proposed"  UniqueName="AmountProposed1" HeaderStyle-Width="170px" HeaderStyle-Height="36px" DataField="AmountProposed" DataFormatString="{0:###,##0.00}">
                                                </telerik:GridBoundColumn>  

                                               <telerik:GridBoundColumn HeaderText="Document Currency" HeaderStyle-Width="100px" HeaderStyle-Height="36px" UniqueName="Currency" DataField="Currency">
                                                </telerik:GridBoundColumn>
                                                  <telerik:GridDateTimeColumn HeaderStyle-Width="150px" HeaderStyle-Height="36px" HeaderText="Net due date" EnableTimeIndependentFiltering="true" FilterControlWidth="130px" UniqueName="RequestNetduedate"
                                                    DataType="System.DateTime" DataFormatString="{0:dd-MM-yyyy}" DataField="BaseLineDate" Visible="true">
                                                </telerik:GridDateTimeColumn>
                                                  <telerik:GridTemplateColumn HeaderText="Nature of Request" HeaderStyle-Width="170px" HeaderStyle-Height="36px" UniqueName="NatureOfRequest" ItemStyle-VerticalAlign="Middle" Display="false">
                                                    <ItemTemplate>
                                                        <telerik:RadDropDownList DefaultMessage="-Select-" RenderMode="Lightweight" HeaderStyle-Width="170px" HeaderStyle-Height="36px" ID="cmbNatureofRequest" runat="server">
                                                        </telerik:RadDropDownList>
                                                      <%-- <asp:RequiredFieldValidator  ID="rfvNatureOfReq" runat="server" ControlToValidate="cmbNatureofRequest" ForeColor="Red" ErrorMessage="* Required"></asp:RequiredFieldValidator>--%>
                                                        <asp:Label ID="lblRequestNatureofRequest" runat="server" Style="display: none"></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                   <telerik:GridBoundColumn HeaderText="Nature of Request" HeaderStyle-Width="150px" HeaderStyle-Height="36px" UniqueName="NatureOfRequest1" DataField="">
                                                </telerik:GridBoundColumn>                           

                                                          <telerik:GridTemplateColumn HeaderStyle-Width="170px" HeaderStyle-Height="36px" HeaderText="Remarks" UniqueName="Remarks" Display="false">
                                                    <ItemTemplate>
                                                        <telerik:RadTextBox ID="tbRequestRemarks" TextMode="MultiLine" Text='<%#Eval("comment") %>' runat="server"></telerik:RadTextBox>
                                                       <%-- <asp:RequiredFieldValidator ID="rfvRequestRemarks" Enabled="false" runat="server" ControlToValidate="tbRequestRemarks" ForeColor="Red" ErrorMessage="* Required"></asp:RequiredFieldValidator>--%>
                                                        <asp:Label ID="lblRequestRemarks" runat="server" Style="display: none"></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                  <telerik:GridBoundColumn HeaderText="Remarks" HeaderStyle-Width="150px" HeaderStyle-Height="36px" UniqueName="Remarks1" DataField="comment">
                                                </telerik:GridBoundColumn>

                                                <telerik:GridTemplateColumn HeaderStyle-Width="120px" HeaderStyle-Height="36px" HeaderText="Attachment" AllowFiltering="false" AllowSorting="false" UniqueName="Attachments">
                                                    <ItemTemplate>
                                                        <asp:Button ID="btnAttachment" runat="server" CommandName="Attachment" CommandArgument='<%# Eval("SAPAgainstBillPaymentId") %>' Text="Add/View" CssClass="btn btn-grey"></asp:Button>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                   <telerik:GridDateTimeColumn HeaderText="Posting Date" HeaderStyle-Width="150px" HeaderStyle-Height="36px" FilterControlWidth="130px" UniqueName="PostDate" EnableTimeIndependentFiltering="true"
                                                    DataType="System.DateTime" DataFormatString="{0:dd-MM-yyyy}" DataField="PostDate">
                                                </telerik:GridDateTimeColumn>
                                                 <telerik:GridBoundColumn HeaderText="Fiscal Year" HeaderStyle-Width="100px" HeaderStyle-Height="36px" UniqueName="FiscalYear" DataField="FiscalYear">
                                                </telerik:GridBoundColumn>
                                                   <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderStyle-Height="36px" HeaderText="Profit Centre" UniqueName="ProfitCentre" DataField="ProfitCentre">
                                                </telerik:GridBoundColumn>
                                                 <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderStyle-Height="36px" HeaderText="Business Area" UniqueName="BusinessArea" DataField="BusinessArea">
                                                </telerik:GridBoundColumn>
                                                 <telerik:GridBoundColumn HeaderText="Company Code" HeaderStyle-Width="100px" HeaderStyle-Height="36px" UniqueName="CompanyCode" DataField="CompanyCode">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridTemplateColumn HeaderStyle-Width="170px" HeaderStyle-Height="36px" HeaderText="Sub Vertical" UniqueName="SubVertical" Display="false">
                                                    <ItemTemplate>
                                                        <telerik:RadDropDownList AutoPostBack="true" OnSelectedIndexChanged="cmbSubVertical_SelectedIndexChanged" RenderMode="Lightweight" ID="cmbSubVertical" runat="server">
                                                        </telerik:RadDropDownList>
                                                        <asp:Label ID="lblRequestSubVertical" runat="server" Style="display: none"></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>   
                                                  <telerik:GridBoundColumn HeaderText="Sub Vertical" HeaderStyle-Width="150px" HeaderStyle-Height="36px" UniqueName="SubVertical1" DataField="">
                                                </telerik:GridBoundColumn>
                                                                                 
                                                      <telerik:GridTemplateColumn HeaderStyle-Width="150px" HeaderStyle-Height="36px" HeaderText="Vertical" UniqueName="Vertical" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbVerticalRequest" runat="server"></asp:Label>
                                                        <asp:Label ID="lblVerticalIDRequest" Style="display: none" runat="server"></asp:Label>
                                                        <asp:Label ID="Label1" Style="display: none" runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>                                            
                                              
                                                    <telerik:GridDateTimeColumn HeaderStyle-Width="170px" HeaderStyle-Height="36px" HeaderText="BaseLineDate" EnableTimeIndependentFiltering="true" FilterControlWidth="130px" UniqueName="BaseLineDate"
                                                    DataType="System.DateTime" DataFormatString="{0:dd-MM-yyyy}" DataField="BaseLineDate" Display="false">
                                                </telerik:GridDateTimeColumn>
                                                  <telerik:GridBoundColumn  HeaderText="DueDays" UniqueName="DueDays" DataField="DueDays" Display="false">
                                                </telerik:GridBoundColumn>                                         
                                                <telerik:GridTemplateColumn HeaderStyle-Width="130px" HeaderStyle-Height="36px" HeaderText="Status" UniqueName="Status">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblstatus" Text="Draft" runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                               
                                                
                                            </Columns>
                                            <PagerStyle PageSizes="5,10,15" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                                                PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />
                                        </MasterTableView>
                                    </telerik:RadGrid>
                                 
                                     <div class="col-lg-12 padding-0 margin-10">

                                        <asp:LinkButton CssClass="btn btn-grey" OnClick="SaveAllTab_Click" runat="server" CommandArgument="Save" ID="SaveAllTab">Save</asp:LinkButton>
                                        <asp:LinkButton OnClientClick="return confirm('Are you sure you want proceed?');" CssClass="btn btn-grey" OnClick="SaveAllTab_Click" CommandArgument="Submit" runat="server" ID="SubmitAllTab">Submit</asp:LinkButton>

                                    </div>


                                </telerik:RadPageView>
                            </telerik:RadMultiPage>
                        </div>
                    </div>
                </div>



            </div>

        </div>

        <div class="row margin-10" runat="server" id="pnlAdvanceBill">

            <div class="col-xs-12 padding-0" style="overflow: hidden;">
                <div class="panel with-nav-tabs panel-default border-0">
                    <div class="panel-heading padding-0 nocolor">
                        <div class="row margin-0">
                            <div class="col-md-12" style="padding-left: 0px; padding-right: 5px">
                               
                               <div class="col-md-8" style="padding-left: 0px">
                                     <telerik:RadTabStrip RenderMode="Lightweight" runat="server" ID="RadTabStrip2" MultiPageID="RadMultiPage2" SelectedIndex="0" Skin="Silk">
                                    <Tabs>
                                        <telerik:RadTab Text="My Request"></telerik:RadTab>
                                        <telerik:RadTab Text="In Process"></telerik:RadTab>
                                        <telerik:RadTab Text="Need Correction"></telerik:RadTab>
                                         <telerik:RadTab Text="Saved Request"></telerik:RadTab>
                                    </Tabs>
                                </telerik:RadTabStrip>
                                </div>
                                <div class="col-md-4" style="padding-left: 0px">
                                     <div  class="col-md-12">
                                            <div class="col-md-10 text-info small"> 
                                            <asp:Label runat="server" ID="Label2" Text="INR Total:" CssClass="control-label lable-txt pull-left"></asp:Label>
                                            <asp:Label ID="Label3" runat="server" CssClass="control-label lable-txt pull-left" Text="0"></asp:Label>
                                            <asp:Label runat="server" style="padding-left: 10px;" ID="Label4" CssClass="control-label lable-txt pull-left" Text="Proposed Total:"></asp:Label>
                                            <asp:Label ID="Label5" runat="server" CssClass="control-label lable-txt pull-left" Text="0"></asp:Label>                                    
                                       </div>
                                          <div class="col-md-2">
                                           <ul class="col-xs-2 pull-right nav" style="padding: 0px 0px 0px 5px;">
                                             <li>
                                            <asp:Button class="btn btn-grey" ID="btnAdvanceExport" CommandName="Advance" OnClick="btnAdvanceExport_Click" runat="server" CssClass="pull-right btn btn-grey button button-style" Text="Export"></asp:Button>
                                         </li>
                                 </ul>
                                </div>  
                                </div>
                            </div>

                              </div>
                              <%--   <div class="col-md-1" style="padding-left: 0px">
                                <ul class="col-xs-2 pull-right nav" style="padding: 0px 0px 0px 5px;">
                                    <li>
                                        <asp:Button class="btn btn-grey" ID="btnAdvanceExport" CommandName="Advance" OnClick="btnAdvanceExport_Click" runat="server" CssClass="pull-right btn btn-grey button button-style" Text="Export"></asp:Button>
                                    </li>
                                 </ul>
                               </div>--%>
                         
                              

                            </div>
                        </div>
                    </div>
                    <div class="panel-body grid-wrapper">
                        <div class="tab-content">
                            <telerik:RadMultiPage runat="server" ID="RadMultiPage2" SelectedIndex="0" CssClass="outerMultiPage">
                                <telerik:RadPageView runat="server" ID="RadPageView4">
                                    <telerik:RadGrid RenderMode="Lightweight" ID="RadGrid1" runat="server" ExportSettings-ExportOnlyData="true" OnExcelMLExportRowCreated="RadGrid1_ExcelMLExportRowCreated" AutoGenerateColumns="false" OnNeedDataSource="RadGrid1_NeedDataSource"
                                        AllowPaging="false" AllowSorting="true" AllowFilteringByColumn="true" AllowMultiRowSelection="true" OnExcelMLExportStylesCreated="RadGrid1_ExcelMLExportStylesCreated"  OnItemDataBound="RadGrid1_ItemDataBound" OnItemCommand="RadGrid1_ItemCommand">
                                        <ExportSettings IgnorePaging="true" Excel-Format="ExcelML" ExportOnlyData="true" />
                                        <GroupingSettings CaseSensitive="false" />
                                        <ClientSettings>
                                            <Selecting AllowRowSelect="true"></Selecting>
                                            <ClientEvents OnRowSelected="RowSelected" OnRowDeselected="RowDeselected" />
                                            <Scrolling AllowScroll="True" UseStaticHeaders="True" ScrollHeight="275px" SaveScrollPosition="true" FrozenColumnsCount="3"></Scrolling>
                                        </ClientSettings>
                                        <MasterTableView CommandItemSettings-ShowRefreshButton="false" EditMode="InPlace" DataKeyNames="SAPAdvancePaymentId,SaveMode" CommandItemDisplay="None"
                                            AllowFilteringByColumn="true" AllowSorting="true" NoMasterRecordsText="" TableLayout="Fixed">
                                            <HeaderStyle Height="36px" />
                                            <CommandItemSettings ShowRefreshButton="false" ShowExportToExcelButton="false" />
                                            <NoRecordsTemplate>
                                                <table width="100%" border="0" cellpadding="20" cellspacing="20">
                                                    <tr>
                                                        <td align="center">No records to display.
                                                        </td>
                                                    </tr>
                                                </table>
                                            </NoRecordsTemplate>
                                            <Columns>
                                                <telerik:GridClientSelectColumn HeaderStyle-Width="70px" HeaderText="Select" UniqueName="Select">
                                                </telerik:GridClientSelectColumn>
                                                     <telerik:GridBoundColumn HeaderStyle-Width="100px" HeaderText="Vendor Code" UniqueName="VendorCode" DataField="VendorCode">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="Vendor Name" UniqueName="VendorName" DataField="VendorName">
                                                </telerik:GridBoundColumn>
                                                 <telerik:GridBoundColumn HeaderStyle-Width="120px" HeaderText="DPR Number" UniqueName="DRPNumber" DataField="DPRNumber">
                                                </telerik:GridBoundColumn>
                                                   <telerik:GridBoundColumn HeaderStyle-Width="150px"   HeaderText="Gross Amount" UniqueName="GrossAmount" DataField="GrossAmount" DataFormatString="{0:###,##0.00}">
                                                </telerik:GridBoundColumn>
                                                  <telerik:GridBoundColumn HeaderStyle-Width="150px"  HeaderText="Tax Rate" UniqueName="TaxRate" DataField="TaxRate" DataFormatString="{0:###,##0.00}">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn HeaderStyle-Width="150px"  HeaderText="Tax Amount" UniqueName="TaxAmount" DataField="TaxAmount" DataFormatString="{0:###,##0.00}">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn HeaderStyle-Width="150px"  HeaderText="Amount in INR" UniqueName="Amount" DataField="Amount" DataFormatString="{0:###,##0.00}">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridTemplateColumn HeaderStyle-Width="170px"  HeaderText="Amount Proposed" UniqueName="AmountProposed" Display="false">
                                                    <ItemTemplate>
                                                        <telerik:RadNumericTextBox ID="tbProposed" Text='<%#String.Format("{0:###,##0.00}",Eval("AmountProposed")) %>' Width="125px" class="form-control" runat="server"></telerik:RadNumericTextBox>
                                                        <asp:Label ID="lblAmountProposed" runat="server" Style="display: none" Text='<%# String.Format("{0:###,##0.00}",Eval("AmountProposed")) %>'></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridBoundColumn HeaderStyle-Width="150px"  HeaderText="Net Amount" UniqueName="AmountProposed1" DataField="AmountProposed" DataFormatString="{0:###,##0.00}">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn  HeaderStyle-Width="100px" HeaderText="Debit/Credit" UniqueName="DCFLag" DataField="DCFLag" >
                                            </telerik:GridBoundColumn>  
                                                      <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="Un-Settled Open Advance (INR)" UniqueName="UnsettledOpenAdvance" 
                                                    DataFormatString="{0:###,##0.00}" DataField="UnsettledOpenAdvance">
                                                </telerik:GridBoundColumn>
                                                 <telerik:GridTemplateColumn HeaderStyle-Width="150px" HeaderText="Withholding Tax Code" UniqueName="WithholdingTaxCode" Display="false">
                                                    <ItemTemplate>
                                                        <telerik:RadTextBox ID="tbWithholdingTaxCode" Text='<%#Eval("WithholdingTaxCode") %>' Width="125px" class="form-control" runat="server"></telerik:RadTextBox>
                                                       <%-- <asp:RequiredFieldValidator Enabled="false" ID="rfvWithholdingTaxCode" runat="server" ControlToValidate="tbWithholdingTaxCode" ForeColor="Red" ErrorMessage="* Required"></asp:RequiredFieldValidator>--%>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                    <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="Withholding Tax Code" UniqueName="WithholdingTaxCode1" DataField="WithholdingTaxCode">
                                                </telerik:GridBoundColumn>


                                                 <telerik:GridTemplateColumn HeaderStyle-Width="150px" HeaderText="Nature of Request" UniqueName="NatureOfRequest" Display="false">
                                                    <ItemTemplate>
                                                        <telerik:RadDropDownList DefaultMessage="-Select-" RenderMode="Lightweight" ID="cmbNatureofRequest" runat="server">
                                                        </telerik:RadDropDownList>
                                                         <%-- <asp:RequiredFieldValidator ID="rfvNatureOFReq1" runat="server" ControlToValidate="cmbNatureofRequest" ForeColor="Red" ErrorMessage="* Required"></asp:RequiredFieldValidator>--%>
                                                        <asp:Label ID="lblRequestNatureofRequest" runat="server" Style="display: none"></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                   <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="Nature of Request" UniqueName="NatureOfRequest1" DataField="">
                                                </telerik:GridBoundColumn>


                                                  <telerik:GridTemplateColumn HeaderStyle-Width="150px" HeaderText="Justification for Adv Payment" UniqueName="JustificationforAdvPayment" Display="false">
                                                    <ItemTemplate>
                                                        <telerik:RadTextBox ID="tbJustificationforAdvPayment" Text='<%#Eval("JustificationforAdvPayment") %>' Width="125px" class="form-control" runat="server"></telerik:RadTextBox>
                                                       <%-- <asp:RequiredFieldValidator Enabled="false" ID="rfvJustificationforAdvPayment" runat="server" ControlToValidate="tbJustificationforAdvPayment" ForeColor="Red" ErrorMessage="* Required"></asp:RequiredFieldValidator>--%>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                  <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="Justification for Adv Payment" UniqueName="JustificationforAdvPayment1" DataField="JustificationforAdvPayment">
                                                </telerik:GridBoundColumn>


                                                <telerik:GridTemplateColumn HeaderStyle-Width="150px" HeaderText="Remarks" UniqueName="Remarks" Display="false">
                                                    <ItemTemplate>
                                                        <telerik:RadTextBox ID="tbRemarks" TextMode="MultiLine" Text='<%#Eval("Comment") %>' runat="server"></telerik:RadTextBox>
                                                        <%--<asp:RequiredFieldValidator Enabled="false" ID="rfvRemarks" runat="server" ControlToValidate="tbRemarks" ForeColor="Red" ErrorMessage="* Required"></asp:RequiredFieldValidator>--%>
                                                        <asp:Label ID="lblRequestRemarks" runat="server" Style="display: none" Text='<%#Eval("Comment") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="Remarks" UniqueName="Remarks1" DataField="Comment">
                                                </telerik:GridBoundColumn>

                                                <telerik:GridTemplateColumn HeaderStyle-Width="120px" HeaderText="Attachment" AllowFiltering="false" AllowSorting="true" UniqueName="Attachments">
                                                    <ItemTemplate>
                                                        <asp:Button ID="btnAttachment" runat="server" CommandName="Attachment" CommandArgument='<%# Eval("SAPAdvancePaymentId") %>' Text="Add/View" CssClass="btn btn-grey"></asp:Button>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                 <telerik:GridDateTimeColumn HeaderStyle-Width="150px" HeaderText="Document Date" FilterControlWidth="130px" UniqueName="AdvanceDocumentDate" EnableTimeIndependentFiltering="true"
                                                    DataType="System.DateTime" DataFormatString="{0:dd-MM-yyyy}" DataField="Documentdate">
                                                </telerik:GridDateTimeColumn>
                                                 <telerik:GridDateTimeColumn HeaderStyle-Width="150px" HeaderText="Posting Date" FilterControlWidth="130px" UniqueName="PostDate" EnableTimeIndependentFiltering="true"
                                                    DataType="System.DateTime" DataFormatString="{0:dd-MM-yyyy}" DataField="PostingDate">
                                                </telerik:GridDateTimeColumn>
                                                            <telerik:GridDateTimeColumn HeaderStyle-Width="150px" HeaderText="Expected Clearing Date" FilterControlWidth="130px" UniqueName="ExpectedClearingDate" DataType="System.DateTime"
                                                    EnableTimeIndependentFiltering="true" DataFormatString="{0:dd-MM-yyyy}" DataField="ExpectedClearingDate">
                                                </telerik:GridDateTimeColumn>
                                                   <telerik:GridBoundColumn HeaderStyle-Width="100px" HeaderText="Fiscal Year" UniqueName="FiscalYear" DataField="FiscalYear">
                                                </telerik:GridBoundColumn>
                                                     <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="Purchasing Document" UniqueName="PurchasingDocument" DataField="PurchasingDocument">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn HeaderStyle-Width="120px" HeaderText="Special GL" UniqueName="SpecialGL" DataField="SpecialGL">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="Profit Centre" UniqueName="ProfitCentre" DataField="ProfitCentre">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="Business Area" UniqueName="BusinessArea" DataField="BusinessArea">
                                                </telerik:GridBoundColumn>
                                                 <telerik:GridBoundColumn HeaderStyle-Width="100px" HeaderText="Company Code" UniqueName="CompanyCode" DataField="CompanyCode">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridTemplateColumn HeaderStyle-Width="150px" HeaderText="Sub Vertical" UniqueName="SubVertical" Display="false">
                                                    <ItemTemplate>
                                                        <telerik:RadDropDownList RenderMode="Lightweight" AutoPostBack="true" OnSelectedIndexChanged="cmbAdvReqSubVertical_SelectedIndexChanged" ID="cmbAdvReqSubVertical" runat="server">
                                                        </telerik:RadDropDownList>
                                                        <asp:Label ID="lblRequestSubVertical" runat="server" Style="display: none"></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                  <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="Sub Vertical" UniqueName="SubVertical1" DataField="">
                                                </telerik:GridBoundColumn>

                                                <%--  <telerik:GridBoundColumn HeaderStyle-Width="170px" HeaderText="Reference" UniqueName="Reference" DataField="Reference">
                                            </telerik:GridBoundColumn>--%>                                                
                                                <%--  <telerik:GridBoundColumn HeaderStyle-Width="170px" HeaderText="Document Currency" UniqueName="Currency" DataField="Currency">
                                            </telerik:GridBoundColumn>--%>                                           
                                                <%--    <telerik:GridBoundColumn HeaderStyle-Width="170px" HeaderText="PO Line Item Text" UniqueName="POItemText" DataField="POItemText">
                                            </telerik:GridBoundColumn>--%>
                                                <telerik:GridTemplateColumn HeaderStyle-Width="150px" HeaderText="Vertical" UniqueName="Vertical">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbVerticalAdvReq" runat="server"></asp:Label>
                                                        <asp:Label ID="lbVerticalIdAdvReq" Style="display: none" runat="server"></asp:Label>
                                                        <asp:Label ID="Label1" Style="display: none" runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>                                     
                                                  <%--<telerik:GridBoundColumn HeaderStyle-Width="170px" HeaderText="SaveMode" UniqueName="SaveMode" DataField="SaveMode" Visible="false">
                                                </telerik:GridBoundColumn>--%>
                                                 
                                            </Columns>
                                            <PagerStyle PageSizes="5,10,15" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                                                PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />
                                        </MasterTableView>
                                    </telerik:RadGrid>
                                    <div class="col-lg-12 padding-0 margin-10">

                                        <asp:LinkButton CssClass="btn btn-grey" OnClick="btnSave_Click" CommandArgument="Save" runat="server" ID="btnSave">Save</asp:LinkButton>
                                        <asp:LinkButton CssClass="btn btn-grey" OnClientClick="return confirm('Are you sure you want proceed?');" OnClick="btnSave_Click" CommandArgument="Submit" runat="server" ID="LinkButton3">Submit</asp:LinkButton>


                                    </div>
                                </telerik:RadPageView>

                                <telerik:RadPageView runat="server" ID="RadPageView5">
                                    <telerik:RadGrid RenderMode="Lightweight" ID="RadGrid2" runat="server" ExportSettings-ExportOnlyData="true" AutoGenerateColumns="false" OnNeedDataSource="RadGrid2_NeedDataSource" OnExcelMLExportRowCreated="RadGrid2_ExcelMLExportRowCreated"
                                        OnItemDataBound="RadGrid2_ItemDataBound" AllowPaging="false" AllowSorting="true" AllowFilteringByColumn="true" OnExcelMLExportStylesCreated="RadGrid2_ExcelMLExportStylesCreated" AllowMultiRowSelection="true"  OnItemCommand="RadGrid2_ItemCommand">
                                        <ExportSettings IgnorePaging="true" Excel-Format="ExcelML" ExportOnlyData="true" />
                                        <GroupingSettings CaseSensitive="false" />
                                        <ClientSettings>
                                            <Scrolling AllowScroll="True" ScrollHeight="275px" UseStaticHeaders="True" SaveScrollPosition="true" FrozenColumnsCount="3"></Scrolling>
                                        </ClientSettings>
                                        <MasterTableView CommandItemSettings-ShowRefreshButton="false" EditMode="InPlace" DataKeyNames="SAPAdvancePaymentId" CommandItemDisplay="None"
                                            AllowFilteringByColumn="true" AllowSorting="true" NoMasterRecordsText=""
                                            TableLayout="Fixed" Font-Size="8">
                                            <CommandItemSettings ShowRefreshButton="false" ShowExportToExcelButton="false" />
                                            <HeaderStyle Height="36px" />
                                            <NoRecordsTemplate>
                                                <table width="100%" border="0" cellpadding="20" cellspacing="20">
                                                    <tr>
                                                        <td align="center">No records to display.
                                                        </td>
                                                    </tr>
                                                </table>
                                            </NoRecordsTemplate>
                                            <Columns>
                                                 <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="Approval Status" UniqueName="ApprovalStatus" DataField="Status">
                                                </telerik:GridBoundColumn>
                                                 <telerik:GridBoundColumn HeaderStyle-Width="100px" HeaderText="Vendor Code" UniqueName="VendorCode" DataField="VendorCode">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="Vendor Name" UniqueName="VendorName" DataField="VendorName">
                                                </telerik:GridBoundColumn>
                                                 <telerik:GridBoundColumn HeaderStyle-Width="110px" HeaderText="DPR Number" UniqueName="DRPNumber" DataField="DPRNumber">
                                                </telerik:GridBoundColumn>
                                                   <telerik:GridBoundColumn HeaderStyle-Width="150px"   HeaderText="Gross Amount" UniqueName="GrossAmount" DataField="GrossAmount" DataFormatString="{0:###,##0.00}">
                                                </telerik:GridBoundColumn>
                                                  <telerik:GridBoundColumn HeaderStyle-Width="150px"  HeaderText="Tax Rate" UniqueName="TaxRate" DataField="TaxRate" DataFormatString="{0:###,##0.00}">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn HeaderStyle-Width="150px"  HeaderText="Tax Amount" UniqueName="TaxAmount" DataField="TaxAmount" DataFormatString="{0:###,##0.00}">
                                                </telerik:GridBoundColumn>
                                                 <telerik:GridBoundColumn HeaderStyle-Width="150px"  HeaderText="Amount in INR" UniqueName="Amount" DataField="Amount" DataFormatString="{0:###,##0.00}">
                                                </telerik:GridBoundColumn>
                                                      <telerik:GridBoundColumn HeaderStyle-Width="150px"  HeaderText="Net Amount" UniqueName="AmountProposed" DataField="AmountProposed" DataFormatString="{0:###,##0.00}">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn HeaderStyle-Width="150px"  HeaderText="Amount Approved" UniqueName="AmountApproved" DataField="ApprovedAmount" DataFormatString="{0:###,##0.00}">
                                                </telerik:GridBoundColumn>
                                                 <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="Un-Settled Open Advance (INR)" UniqueName="UnsettledOpenAdvance" 
                                                    DataField="UnsettledOpenAdvance" DataFormatString="{0:###,##0.00}">
                                                </telerik:GridBoundColumn>
                                                   <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="Withholding Tax Code" UniqueName="WithholdingTaxCode" DataField="WithholdingTaxCode">
                                                </telerik:GridBoundColumn>
                                                   <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="Nature of Request" UniqueName="NatureOfRequest" DataField="NatureOfRequestValue">
                                                </telerik:GridBoundColumn>
                                                 <telerik:GridBoundColumn HeaderStyle-Width="170px" HeaderText="Justification for Adv Payment" UniqueName="JustificationforAdvPayment" DataField="JustificationforAdvPayment">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="Remarks" UniqueName="Remarks" DataField="Comment">
                                                </telerik:GridBoundColumn>
                                                    <telerik:GridTemplateColumn HeaderStyle-Width="120px" HeaderText="Attachment" AllowFiltering="false" AllowSorting="false" UniqueName="Attachments">
                                                    <ItemTemplate>
                                                        <asp:Button ID="btnAttachment" runat="server" CommandName="Attachment" CommandArgument='<%# Eval("SAPAdvancePaymentId") %>' Text="Add/View" CssClass="btn btn-grey"></asp:Button>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridDateTimeColumn HeaderStyle-Width="150px" HeaderText="Document Date" FilterControlWidth="130px" UniqueName="InprogressDocumentDate" EnableTimeIndependentFiltering="true"
                                                    DataType="System.DateTime" DataFormatString="{0:dd-MM-yyyy}" DataField="DocumentDate">
                                                </telerik:GridDateTimeColumn>
                                                  <telerik:GridDateTimeColumn HeaderStyle-Width="150px" HeaderText="Posting Date" FilterControlWidth="130px" UniqueName="PostDate" EnableTimeIndependentFiltering="true"
                                                    DataType="System.DateTime" DataFormatString="{0:dd-MM-yyyy}" DataField="PostingDate">
                                                </telerik:GridDateTimeColumn>
                                                  <telerik:GridDateTimeColumn HeaderStyle-Width="150px" HeaderText="Expected Clearing Date" FilterControlWidth="130px" UniqueName="ExpectedClearingDate" DataType="System.DateTime"
                                                    EnableTimeIndependentFiltering="true" DataFormatString="{0:dd-MM-yyyy}" DataField="ExpectedClearingDate">
                                                </telerik:GridDateTimeColumn>
                                                <telerik:GridBoundColumn HeaderStyle-Width="100px" HeaderText="Fiscal Year" UniqueName="FiscalYear" DataField="FiscalYear">
                                                </telerik:GridBoundColumn>
                                                 <telerik:GridBoundColumn HeaderStyle-Width="100px" HeaderText="Purchasing Document" UniqueName="PurchasingDocument" DataField="PurchasingDocument">
                                                </telerik:GridBoundColumn>                                                                                            
                                                <telerik:GridBoundColumn HeaderStyle-Width="110px" HeaderText="Special GL" UniqueName="SpecialGL" DataField="SpecialGL">
                                                </telerik:GridBoundColumn>  
                                                <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="Profit Centre" UniqueName="ProfitCentre" DataField="ProfitCentre">
                                                </telerik:GridBoundColumn>
                                               <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="Business Area" UniqueName="BusinessArea" DataField="BusinessArea">
                                                </telerik:GridBoundColumn>
                                                  <telerik:GridBoundColumn HeaderStyle-Width="100px" HeaderText="Company Code" UniqueName="CompanyCode" DataField="CompanyCode">
                                                </telerik:GridBoundColumn>                                                 
                                                <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="Sub Vertical" UniqueName="SubVertical" DataField="SubVertical">
                                                </telerik:GridBoundColumn>                                               
                                               <%-- <telerik:GridDateTimeColumn HeaderStyle-Width="210px" HeaderText="Document Date" FilterControlWidth="130px" UniqueName="TestAdvInprogressDocumentDate" EnableTimeIndependentFiltering="true"
                                                    DataType="System.DateTime" DataFormatString="{0:dd-MMM-yyyy}" DataField="Documentdate">
                                                </telerik:GridDateTimeColumn>--%>
                                                <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="Vertical" UniqueName="Vertical" DataField="vertical">
                                                </telerik:GridBoundColumn>
                                                  <telerik:GridDateTimeColumn HeaderText="Submission Date" HeaderStyle-Width="150px" FilterControlWidth="130px" UniqueName="SubmitDate" EnableTimeIndependentFiltering="true"
                                                    DataType="System.DateTime" DataFormatString="{0:dd-MM-yyyy}" DataField="SubmitDate">
                                                </telerik:GridDateTimeColumn>
                                                <telerik:GridTemplateColumn UniqueName="TemplateComment" HeaderText="Comments" HeaderStyle-Width="150px" AllowFiltering="false" AllowSorting="false">
                                                    <ItemTemplate>
                                                        <asp:HyperLink ID="viewComment" runat="server" Text="View" CssClass="btn btn-grey gridHyperlinks"></asp:HyperLink>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>

                                            
                                      
                                            </Columns>
                                            <PagerStyle PageSizes="5,10,15" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                                                PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />
                                        </MasterTableView>
                                    </telerik:RadGrid>
                                </telerik:RadPageView>

                                <telerik:RadPageView runat="server" ID="RadPageView6">
                                    <telerik:RadGrid RenderMode="Lightweight" ID="RadGrid3" runat="server" OnExcelMLExportStylesCreated="RadGrid3_ExcelMLExportStylesCreated" ExportSettings-ExportOnlyData="true" AutoGenerateColumns="false" OnNeedDataSource="RadGrid3_NeedDataSource" OnItemCommand="RadGrid3_ItemCommand"
                                        AllowPaging="false" AllowSorting="true" AllowFilteringByColumn="true" AllowMultiRowSelection="true"  OnItemDataBound="RadGrid3_ItemDataBound" OnExcelMLExportRowCreated="RadGrid3_ExcelMLExportRowCreated">
                                        <ExportSettings IgnorePaging="true" Excel-Format="ExcelML" ExportOnlyData="true" />
                                        <GroupingSettings CaseSensitive="false" />
                                        <ClientSettings>
                                            <Selecting AllowRowSelect="true"></Selecting>
                                            <ClientEvents OnRowSelected="RowSelectedNeedCorrection" OnRowDeselected="RowDeselectedNeedCorrection" />
                                            <Scrolling AllowScroll="True" UseStaticHeaders="True" ScrollHeight="275px" SaveScrollPosition="true" FrozenColumnsCount="3"></Scrolling>
                                        </ClientSettings>
                                        <MasterTableView CommandItemSettings-ShowRefreshButton="false" EditMode="InPlace" DataKeyNames="SAPAdvancePaymentId" CommandItemDisplay="None"
                                            AllowFilteringByColumn="true" AllowSorting="true" NoMasterRecordsText=""
                                            TableLayout="Fixed">
                                            <HeaderStyle Height="36px" />
                                            <CommandItemSettings ShowRefreshButton="false" ShowExportToExcelButton="false" />
                                            <NoRecordsTemplate>
                                                <table width="100%" border="0" cellpadding="20" cellspacing="20">
                                                    <tr>
                                                        <td align="center">No records to display.
                                                        </td>
                                                    </tr>
                                                </table>
                                            </NoRecordsTemplate>
                                            <Columns>

                                                <telerik:GridClientSelectColumn HeaderStyle-Width="70px" HeaderText="Select" UniqueName="SelectAdvanceCorrection">
                                                </telerik:GridClientSelectColumn>
                                                    <telerik:GridBoundColumn HeaderStyle-Width="100px" HeaderText="Vendor Code" UniqueName="VendorCodeAdvanceCorrection" DataField="VendorCode">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="Vendor Name" UniqueName="VendorNameAdvanceCorrection" DataField="VendorName">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn HeaderStyle-Width="110px" HeaderText="DPR Number" UniqueName="DRPNumbeAdvanceCorrectionr" DataField="DPRNumber">
                                                </telerik:GridBoundColumn>
                                                   <telerik:GridBoundColumn HeaderStyle-Width="150px"   HeaderText="Gross Amount" UniqueName="GrossAmount" DataField="GrossAmount" DataFormatString="{0:###,##0.00}">
                                                </telerik:GridBoundColumn>
                                                  <telerik:GridBoundColumn HeaderStyle-Width="150px"  HeaderText="Tax Rate" UniqueName="TaxRate" DataField="TaxRate" DataFormatString="{0:###,##0.00}">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn HeaderStyle-Width="150px"  HeaderText="Tax Amount" UniqueName="TaxAmount" DataField="TaxAmount" DataFormatString="{0:###,##0.00}">
                                                </telerik:GridBoundColumn>
                                                 <telerik:GridBoundColumn HeaderStyle-Width="150px"  HeaderText="Amount in INR" UniqueName="AmountAdvanceCorrection" DataField="Amount" DataFormatString="{0:###,##0.00}">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridTemplateColumn HeaderStyle-Width="150px"  HeaderText="Net Amount" UniqueName="AmountProposedAdvanceCorrection">
                                                    <ItemTemplate>
                                                        <telerik:RadNumericTextBox ID="tbProposedAdvanceCorrection" Text='<%#String.Format("{0:###,##0.00}",Eval("AmountProposed")) %>' Width="125px" class="form-control" runat="server" Enabled="false"></telerik:RadNumericTextBox>
                                                        <asp:Label ID="lblAmountProposedAdvanceCorrection" runat="server" Style="display: none" Text='<%# String.Format("{0:###,##0.00}",Eval("AmountProposed")) %>'></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                   <telerik:GridBoundColumn HeaderStyle-Width="150px"  HeaderText="Amount Approved" UniqueName="AmountApproved" DataField="ApprovedAmount" DataFormatString="{0:###,##0.00}">
                                                </telerik:GridBoundColumn>
                                                      <telerik:GridBoundColumn HeaderStyle-Width="80px"  HeaderText="Debit/Credit" UniqueName="DCFLag" DataField="DCFLag" >
                                            </telerik:GridBoundColumn>   
                                                       <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="Un-Settled Open Advance (INR)" UniqueName="UnsettledOpenAdvanceAdvanceCorrection"
                                                    DataFormatString="{0:###,##0.00}" DataField="UnsettledOpenAdvance">
                                                </telerik:GridBoundColumn>                                            
                                                     <telerik:GridTemplateColumn HeaderStyle-Width="150px" HeaderText="Withholding Tax Code" UniqueName="WithholdingTaxCodeAdvanceCorrection">
                                                    <ItemTemplate>
                                                        <telerik:RadTextBox ID="tbWithholdingTaxCodeAdvanceCorrection" Text='<%#Eval("WithholdingTaxCode") %>' Width="125px" class="form-control" runat="server"></telerik:RadTextBox>
                                                        <asp:RequiredFieldValidator Enabled="false" ID="rfvWithholdingTaxCodeAdvanceCorrection" runat="server" ControlToValidate="tbWithholdingTaxCodeAdvanceCorrection" ForeColor="Red" ErrorMessage="* Required"></asp:RequiredFieldValidator>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                        <telerik:GridTemplateColumn HeaderStyle-Width="150px" HeaderText="Nature of Request" UniqueName="NatureOfRequestAdvanceCorrection">
                                                    <ItemTemplate>
                                                        <telerik:RadDropDownList RenderMode="Lightweight" ID="cmbNatureofRequestAdvanceCorrection" runat="server">
                                                        </telerik:RadDropDownList>
                                                        <asp:Label ID="lblRequestNatureofRequestAdvanceCorrection" runat="server" Style="display: none"></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                         <telerik:GridTemplateColumn HeaderStyle-Width="150px" HeaderText="Justification for Adv Payment" UniqueName="JustificationforAdvPaymentAdvanceCorrection">
                                                    <ItemTemplate>
                                                        <telerik:RadTextBox ID="tbJustificationforAdvPaymentAdvanceCorrection" Text='<%#Eval("JustificationforAdvPayment") %>' Width="125px" class="form-control" runat="server"></telerik:RadTextBox>
                                                        <asp:RequiredFieldValidator Enabled="false" ID="rfvJustificationforAdvPaymentAdvanceCorrection" runat="server" ControlToValidate="tbJustificationforAdvPaymentAdvanceCorrection" ForeColor="Red" ErrorMessage="* Required"></asp:RequiredFieldValidator>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                 <telerik:GridTemplateColumn HeaderStyle-Width="150px" HeaderText="Remarks" UniqueName="RemarksAdvanceCorrection" DataField="Comment">
                                                    <ItemTemplate>
                                                        <telerik:RadTextBox ID="tbRemarksAdvanceCorrection" Text='<%#Eval("Comment") %>' TextMode="MultiLine" runat="server" Width="125px"></telerik:RadTextBox>
                                                        <asp:RequiredFieldValidator Enabled="false" ID="rfvRemarksAdvanceCorrection" runat="server" ControlToValidate="tbRemarksAdvanceCorrection" ForeColor="Red" ErrorMessage="* Required"></asp:RequiredFieldValidator>
                                                        <asp:Label ID="lblRequestRemarksAdvanceCorrection" runat="server" Style="display: none" Text='<%#Eval("Comment") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                      <telerik:GridTemplateColumn HeaderStyle-Width="120px" HeaderText="Attachment" AllowFiltering="false" AllowSorting="false" UniqueName="Attachments">
                                                    <ItemTemplate>
                                                        <asp:Button ID="btnAttachment" runat="server" CommandName="Attachment" CommandArgument='<%# Eval("SAPAdvancePaymentId") %>' Text="Add/View" CssClass="btn btn-grey"></asp:Button>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                  <telerik:GridDateTimeColumn HeaderStyle-Width="150px" HeaderText="Document Date" UniqueName="DocumentDateAdvanceCorrection" DataType="System.DateTime" EnableTimeIndependentFiltering="true"
                                                    FilterControlWidth="130px" DataFormatString="{0:dd-MM-yyyy}" DataField="DocumentDate">
                                                </telerik:GridDateTimeColumn>
                                                  <telerik:GridDateTimeColumn HeaderStyle-Width="150px" HeaderText="Posting Date" UniqueName="PostDateAdvanceCorrection" DataType="System.DateTime" EnableTimeIndependentFiltering="true"
                                                    FilterControlWidth="130px" DataFormatString="{0:dd-MM-yyyy}" DataField="PostingDate">
                                                </telerik:GridDateTimeColumn>
                                                 <telerik:GridDateTimeColumn HeaderStyle-Width="150px" HeaderText="Expected Clearing Date" FilterControlWidth="130px" UniqueName="ExpectedClearingDateAdvanceCorrection" DataType="System.DateTime"
                                                    DataFormatString="{0:dd-MM-yyyy}" DataField="ExpectedClearingDate">
                                                </telerik:GridDateTimeColumn>
                                                <telerik:GridBoundColumn HeaderStyle-Width="100px" HeaderText="Fiscal Year" UniqueName="FiscalYearAdvanceCorrection" DataField="FiscalYear">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn HeaderStyle-Width="120px" HeaderText="Purchasing Document" UniqueName="PurchasingDocumentAdvanceCorrection" DataField="PurchasingDocument">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="Special GL" UniqueName="SpecialGLAdvanceCorrection" DataField="SpecialGL">
                                                </telerik:GridBoundColumn>
                                                 <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="Profit Centre" UniqueName="ProfitCentreAdvanceCorrection" DataField="ProfitCentre">
                                                </telerik:GridBoundColumn>
                                                  <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="Business Area" UniqueName="BusinessAreaAdvanceCorrection" DataField="BusinessArea">
                                                </telerik:GridBoundColumn>
                                                  <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="Company Code" UniqueName="CompanyCodeAdvanceCorrection" DataField="CompanyCode">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridTemplateColumn HeaderStyle-Width="150px" HeaderText="Sub Vertical" UniqueName="SubVerticalAdvanceCorrection">
                                                    <ItemTemplate>
                                                        <telerik:RadDropDownList RenderMode="Lightweight" OnSelectedIndexChanged="cmbAdvReqSubVerticalAdvanceCorrection_SelectedIndexChanged" AutoPostBack="true" ID="cmbAdvReqSubVerticalAdvanceCorrection" runat="server">
                                                        </telerik:RadDropDownList>
                                                        <asp:Label ID="lblRequestSubVerticalAdvanceCorrection" runat="server" Style="display: none"></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>    
                                                <telerik:GridTemplateColumn HeaderStyle-Width="150px" HeaderText="Vertical" UniqueName="VerticalAdvanceCorrection">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbVerticalAdvReqAdvanceCorrection" runat="server"></asp:Label>
                                                        <asp:Label ID="lbVerticalIdAdvReqAdvanceCorrection" Style="display: none" runat="server"></asp:Label>
                                                        <asp:Label ID="Label1AdvanceCorrection" Style="display: none" runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>  
                                                    <telerik:GridDateTimeColumn HeaderText="Submission Date" HeaderStyle-Width="150px" FilterControlWidth="130px" UniqueName="SubmitDate" EnableTimeIndependentFiltering="true"
                                                    DataType="System.DateTime" DataFormatString="{0:dd-MM-yyyy}" DataField="SubmitDate">
                                                </telerik:GridDateTimeColumn>                                                                                             
                                                   <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="Assigned By" UniqueName="AssigndBy" DataField="AssignedBy">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridTemplateColumn UniqueName="TemplateComment" HeaderText="Comments" HeaderStyle-Width="170px" AllowFiltering="false" AllowSorting="false">
                                                    <ItemTemplate>
                                                        <asp:HyperLink ID="viewComment" runat="server" Text="View" CssClass="btn btn-grey gridHyperlinks"></asp:HyperLink>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                          
                                            </Columns>
                                            <PagerStyle PageSizes="5,10,15" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                                                PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />
                                        </MasterTableView>
                                    </telerik:RadGrid>

                                    <div class="col-lg-12 padding-0 margin-10">
                                        <asp:LinkButton CssClass="btn btn-grey" OnClick="lbSubmitAdvancedNeedCorrection_Click" CommandArgument="Save" runat="server" ID="lbSAveAdvancedNeedCorrection">Save</asp:LinkButton>
                                        <asp:LinkButton CssClass="btn btn-grey" OnClientClick="return confirm('Are you sure you want proceed?');" OnClick="lbSubmitAdvancedNeedCorrection_Click" CommandArgument="Submit" runat="server" ID="lbSubmitAdvancedNeedCorrection">Submit</asp:LinkButton>
                                    </div>
                                </telerik:RadPageView>

                                 <telerik:RadPageView runat="server" ID="RadPageView8">
                                    <telerik:RadGrid RenderMode="Lightweight" ID="RadGrid4" runat="server" ExportSettings-ExportOnlyData="true" OnExcelMLExportRowCreated="RadGrid4_ExcelMLExportRowCreated" AutoGenerateColumns="false" OnNeedDataSource="RadGrid4_NeedDataSource"
                                        AllowPaging="false" AllowSorting="true" AllowFilteringByColumn="true" AllowMultiRowSelection="true" OnExcelMLExportStylesCreated="RadGrid4_ExcelMLExportStylesCreated"  OnItemDataBound="RadGrid4_ItemDataBound" OnItemCommand="RadGrid4_ItemCommand">
                                        <ExportSettings IgnorePaging="true" Excel-Format="ExcelML" ExportOnlyData="true" />
                                        <GroupingSettings CaseSensitive="false" />
                                        <ClientSettings>
                                            <Selecting AllowRowSelect="true"></Selecting>
                                            <ClientEvents OnRowSelected="RowSelected" OnRowDeselected="RowDeselected" />
                                            <Scrolling AllowScroll="True" UseStaticHeaders="True" ScrollHeight="275px" SaveScrollPosition="true" FrozenColumnsCount="3"></Scrolling>
                                        </ClientSettings>
                                        <MasterTableView CommandItemSettings-ShowRefreshButton="false" EditMode="InPlace" DataKeyNames="SAPAdvancePaymentId,SaveMode" CommandItemDisplay="None"
                                            AllowFilteringByColumn="true" AllowSorting="true" NoMasterRecordsText="" TableLayout="Fixed">
                                            <HeaderStyle Height="36px" />
                                            <CommandItemSettings ShowRefreshButton="false" ShowExportToExcelButton="false" />
                                            <NoRecordsTemplate>
                                                <table width="100%" border="0" cellpadding="20" cellspacing="20">
                                                    <tr>
                                                        <td align="center">No records to display.
                                                        </td>
                                                    </tr>
                                                </table>
                                            </NoRecordsTemplate>
                                            <Columns>
                                                <telerik:GridClientSelectColumn HeaderStyle-Width="70px" HeaderText="Select" UniqueName="Select">
                                                </telerik:GridClientSelectColumn>
                                                     <telerik:GridBoundColumn HeaderStyle-Width="100px" HeaderText="Vendor Code" UniqueName="VendorCode" DataField="VendorCode">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="Vendor Name" UniqueName="VendorName" DataField="VendorName">
                                                </telerik:GridBoundColumn>
                                                 <telerik:GridBoundColumn HeaderStyle-Width="120px" HeaderText="DPR Number" UniqueName="DRPNumber" DataField="DPRNumber">
                                                </telerik:GridBoundColumn>
                                                   <telerik:GridBoundColumn HeaderStyle-Width="150px"   HeaderText="Gross Amount" UniqueName="GrossAmount" DataField="GrossAmount" DataFormatString="{0:###,##0.00}">
                                                </telerik:GridBoundColumn>
                                                  <telerik:GridBoundColumn HeaderStyle-Width="150px"  HeaderText="Tax Rate" UniqueName="TaxRate" DataField="TaxRate" DataFormatString="{0:###,##0.00}">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn HeaderStyle-Width="150px"  HeaderText="Tax Amount" UniqueName="TaxAmount" DataField="TaxAmount" DataFormatString="{0:###,##0.00}">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn HeaderStyle-Width="150px"  HeaderText="Amount in INR" UniqueName="Amount" DataField="Amount" DataFormatString="{0:###,##0.00}">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridTemplateColumn HeaderStyle-Width="170px"  HeaderText="Amount Proposed" UniqueName="AmountProposed" Display="false">
                                                    <ItemTemplate>
                                                        <telerik:RadNumericTextBox ID="tbProposed" Text='<%#String.Format("{0:###,##0.00}",Eval("AmountProposed")) %>' Width="125px" class="form-control" runat="server" Enabled="false"></telerik:RadNumericTextBox>
                                                        <asp:Label ID="lblAmountProposed" runat="server" Style="display: none" Text='<%# String.Format("{0:###,##0.00}",Eval("AmountProposed")) %>'></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridBoundColumn HeaderStyle-Width="150px"  HeaderText="Net Amount" UniqueName="AmountProposed1" DataField="AmountProposed" DataFormatString="{0:###,##0.00}">
                                                </telerik:GridBoundColumn>
                                                 <telerik:GridBoundColumn  HeaderStyle-Width="90px" HeaderText="Debit/Credit" UniqueName="DCFLag" DataField="DCFLag" >
                                            </telerik:GridBoundColumn> 
                                                      <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="Un-Settled Open Advance (INR)" UniqueName="UnsettledOpenAdvance" 
                                                    DataFormatString="{0:###,##0.00}" DataField="UnsettledOpenAdvance">
                                                </telerik:GridBoundColumn>
                                                 
                                                 <telerik:GridTemplateColumn HeaderStyle-Width="150px" HeaderText="Withholding Tax Code" UniqueName="WithholdingTaxCode" Display="false">
                                                    <ItemTemplate>
                                                        <telerik:RadTextBox ID="tbWithholdingTaxCode" Text='<%#Eval("WithholdingTaxCode") %>' Width="125px" class="form-control" runat="server"></telerik:RadTextBox>
                                                       <%-- <asp:RequiredFieldValidator Enabled="false" ID="rfvWithholdingTaxCode" runat="server" ControlToValidate="tbWithholdingTaxCode" ForeColor="Red" ErrorMessage="* Required"></asp:RequiredFieldValidator>--%>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                    <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="Withholding Tax Code" UniqueName="WithholdingTaxCode1" DataField="WithholdingTaxCode">
                                                </telerik:GridBoundColumn>


                                                 <telerik:GridTemplateColumn HeaderStyle-Width="150px" HeaderText="Nature of Request" UniqueName="NatureOfRequest" Display="false">
                                                    <ItemTemplate>
                                                        <telerik:RadDropDownList DefaultMessage="-Select-" RenderMode="Lightweight" ID="cmbNatureofRequest" runat="server">
                                                        </telerik:RadDropDownList>
                                                          <%--<asp:RequiredFieldValidator ID="rfvNatureOFReq1" runat="server" ControlToValidate="cmbNatureofRequest" ForeColor="Red" ErrorMessage="* Required"></asp:RequiredFieldValidator>--%>
                                                        <asp:Label ID="lblRequestNatureofRequest" runat="server" Style="display: none"></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                   <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="Nature of Request" UniqueName="NatureOfRequest1" DataField="">
                                                </telerik:GridBoundColumn>


                                                  <telerik:GridTemplateColumn HeaderStyle-Width="150px" HeaderText="Justification for Adv Payment" UniqueName="JustificationforAdvPayment" Display="false">
                                                    <ItemTemplate>
                                                        <telerik:RadTextBox ID="tbJustificationforAdvPayment" Text='<%#Eval("JustificationforAdvPayment") %>' Width="125px" class="form-control" runat="server"></telerik:RadTextBox>
                                                       <%-- <asp:RequiredFieldValidator Enabled="false" ID="rfvJustificationforAdvPayment" runat="server" ControlToValidate="tbJustificationforAdvPayment" ForeColor="Red" ErrorMessage="* Required"></asp:RequiredFieldValidator>--%>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                  <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="Justification for Adv Payment" UniqueName="JustificationforAdvPayment1" DataField="JustificationforAdvPayment">
                                                </telerik:GridBoundColumn>


                                                <telerik:GridTemplateColumn HeaderStyle-Width="150px" HeaderText="Remarks" UniqueName="Remarks" Display="false">
                                                    <ItemTemplate>
                                                        <telerik:RadTextBox ID="tbRemarks" TextMode="MultiLine" Text='<%#Eval("Comment") %>' runat="server"></telerik:RadTextBox>
                                                        <%--<asp:RequiredFieldValidator Enabled="false" ID="rfvRemarks" runat="server" ControlToValidate="tbRemarks" ForeColor="Red" ErrorMessage="* Required"></asp:RequiredFieldValidator>--%>
                                                        <asp:Label ID="lblRequestRemarks" runat="server" Style="display: none" Text='<%#Eval("Comment") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="Remarks" UniqueName="Remarks1" DataField="Comment">
                                                </telerik:GridBoundColumn>

                                                <telerik:GridTemplateColumn HeaderStyle-Width="120px" HeaderText="Attachment" AllowFiltering="false" AllowSorting="true" UniqueName="Attachments">
                                                    <ItemTemplate>
                                                        <asp:Button ID="btnAttachment" runat="server" CommandName="Attachment" CommandArgument='<%# Eval("SAPAdvancePaymentId") %>' Text="Add/View" CssClass="btn btn-grey"></asp:Button>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                 <telerik:GridDateTimeColumn HeaderStyle-Width="150px" HeaderText="Document Date" FilterControlWidth="130px" UniqueName="AdvanceDocumentDate" EnableTimeIndependentFiltering="true"
                                                    DataType="System.DateTime" DataFormatString="{0:dd-MM-yyyy}" DataField="Documentdate">
                                                </telerik:GridDateTimeColumn>
                                                 <telerik:GridDateTimeColumn HeaderStyle-Width="150px" HeaderText="Posting Date" FilterControlWidth="130px" UniqueName="PostDate" EnableTimeIndependentFiltering="true"
                                                    DataType="System.DateTime" DataFormatString="{0:dd-MM-yyyy}" DataField="PostingDate">
                                                </telerik:GridDateTimeColumn>
                                                            <telerik:GridDateTimeColumn HeaderStyle-Width="150px" HeaderText="Expected Clearing Date" FilterControlWidth="130px" UniqueName="ExpectedClearingDate" DataType="System.DateTime"
                                                    EnableTimeIndependentFiltering="true" DataFormatString="{0:dd-MM-yyyy}" DataField="ExpectedClearingDate">
                                                </telerik:GridDateTimeColumn>
                                                   <telerik:GridBoundColumn HeaderStyle-Width="100px" HeaderText="Fiscal Year" UniqueName="FiscalYear" DataField="FiscalYear">
                                                </telerik:GridBoundColumn>
                                                     <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="Purchasing Document" UniqueName="PurchasingDocument" DataField="PurchasingDocument">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn HeaderStyle-Width="120px" HeaderText="Special GL" UniqueName="SpecialGL" DataField="SpecialGL">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="Profit Centre" UniqueName="ProfitCentre" DataField="ProfitCentre">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="Business Area" UniqueName="BusinessArea" DataField="BusinessArea">
                                                </telerik:GridBoundColumn>
                                                 <telerik:GridBoundColumn HeaderStyle-Width="100px" HeaderText="Company Code" UniqueName="CompanyCode" DataField="CompanyCode">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridTemplateColumn HeaderStyle-Width="150px" HeaderText="Sub Vertical" UniqueName="SubVertical" Display="false">
                                                    <ItemTemplate>
                                                        <telerik:RadDropDownList RenderMode="Lightweight" AutoPostBack="true" OnSelectedIndexChanged="cmbAdvReqSubVertical_SelectedIndexChanged" ID="cmbAdvReqSubVertical" runat="server">
                                                        </telerik:RadDropDownList>
                                                        <asp:Label ID="lblRequestSubVertical" runat="server" Style="display: none"></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                  <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="Sub Vertical" UniqueName="SubVertical1" DataField="">
                                                </telerik:GridBoundColumn>

                                                <%--  <telerik:GridBoundColumn HeaderStyle-Width="170px" HeaderText="Reference" UniqueName="Reference" DataField="Reference">
                                            </telerik:GridBoundColumn>--%>                                                
                                                <%--  <telerik:GridBoundColumn HeaderStyle-Width="170px" HeaderText="Document Currency" UniqueName="Currency" DataField="Currency">
                                            </telerik:GridBoundColumn>--%>                                           
                                                <%--    <telerik:GridBoundColumn HeaderStyle-Width="170px" HeaderText="PO Line Item Text" UniqueName="POItemText" DataField="POItemText">
                                            </telerik:GridBoundColumn>--%>
                                                <telerik:GridTemplateColumn HeaderStyle-Width="150px" HeaderText="Vertical" UniqueName="Vertical">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbVerticalAdvReq" runat="server"></asp:Label>
                                                        <asp:Label ID="lbVerticalIdAdvReq" Style="display: none" runat="server"></asp:Label>
                                                        <asp:Label ID="Label1" Style="display: none" runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>                                     
                                                  <%--<telerik:GridBoundColumn HeaderStyle-Width="170px" HeaderText="SaveMode" UniqueName="SaveMode" DataField="SaveMode" Visible="false">
                                                </telerik:GridBoundColumn>--%>
                                                
                                            </Columns>
                                            <PagerStyle PageSizes="5,10,15" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                                                PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />
                                        </MasterTableView>
                                    </telerik:RadGrid>
                                    <div class="col-lg-12 padding-0 margin-10">

                                        <asp:LinkButton CssClass="btn btn-grey" OnClick="lnkSaveAllAdvTab_Click" CommandArgument="Save" runat="server" ID="lnkSaveAllAdvTab">Save</asp:LinkButton>
                                        <asp:LinkButton CssClass="btn btn-grey" OnClientClick="return confirm('Are you sure you want proceed?');" OnClick="lnkSaveAllAdvTab_Click" CommandArgument="Submit" runat="server" ID="lnkSubmitAllAdvTab">Submit</asp:LinkButton>


                                    </div>
                                </telerik:RadPageView>


                            </telerik:RadMultiPage>

                        </div>
                    </div>
                </div>

            </div>

        </div>

        <!-- Start of modal for addendum request-->
        <div class="modal fade" id="budget-utilization-modal" tabindex="-1" role="dialog" aria-labelledby="modalLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">×</span><span class="sr-only">Close</span></button>
                        <h4 class="modal-title subheading" id="lineModalLabel">Request Already Under Process</h4>
                    </div>
                    <div class="modal-body" style="overflow: hidden">

                        <!-- content goes here -->
                        <table class="table table-striped table-bordered">
                            <thead>
                                <tr>
                                    <th class="text-center">Document Number</th>
                                    <th class="text-center">Submitted On</th>
                                    <th class="text-center">Approval Status</th>

                                </tr>
                            </thead>
                            <tbody>
                                <tr class="text-center">
                                    <td class="valign">12345</td>
                                    <td class="valign">20/04/2016</td>
                                    <td class="valign">Controller Pending-Amol Ferange</td>

                                </tr>
                                <tr class="text-center">
                                    <td class="valign">23456</td>
                                    <td class="valign">22/04/2016</td>
                                    <td class="valign">Controller Pending-Amol Ferange</td>

                                </tr>
                                <tr class="text-center">
                                    <td class="valign">34567</td>
                                    <td class="valign">24/04/2016</td>
                                    <td class="valign">Controller Pending-Amol Ferange</td>

                                </tr>
                            </tbody>
                        </table>

                    </div>


                </div>
            </div>
        </div>
        <telerik:RadNotification RenderMode="Lightweight" Position="Center" runat="server" ID="radMessage" VisibleOnPageLoad="false"
            Width="450px" AutoCloseDelay="0" TitleIcon="none" ContentIcon="none">
        </telerik:RadNotification>
        <div class="modal fade" id="squarespaceCommentModal" tabindex="-1" role="dialog" aria-labelledby="modalLabel" aria-hidden="true">
            <div class="modal-dialog1">
                <div class="modal-content">
                    <div class="modal-header modal-header-resize">
                        <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">×</span><span class="sr-only">Close</span></button>
                        <h4 class="modal-title text-primary" id="lineModalCommentLabel">Comments</h4>
                    </div>
                    <div class="modal-body">
                        <telerik:RadGrid RenderMode="Lightweight" ID="grdComment" runat="server" OnNeedDataSource="grdComment_NeedDataSource"
                            AutoGenerateColumns="false" AllowPaging="true" AllowSorting="true" AllowFilteringByColumn="false" PageSize="10">
                            <GroupingSettings CaseSensitive="false" />
                            <MasterTableView CommandItemSettings-ShowRefreshButton="false" ClientDataKeyNames="" EditMode="InPlace" CommandItemDisplay="None" AllowFilteringByColumn="false">
                                <NoRecordsTemplate>
                                    <table width="100%" border="0" cellpadding="20" cellspacing="20">
                                        <tr>
                                            <td align="center" class="txt-white" class="txt-white">No records to display.
                                            </td>
                                        </tr>
                                    </table>
                                </NoRecordsTemplate>
                                <CommandItemSettings ShowRefreshButton="false" ShowExportToExcelButton="false" />
                                <Columns>
                                    <telerik:GridBoundColumn DataField="UserName" HeaderText="Name" UniqueName="Name" HeaderStyle-Width="30%">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="WorkFlowStatus" HeaderText="Status" UniqueName="WorkFlowStatus" HeaderStyle-Width="30%">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn  DataField="AppovedAmount" HeaderText="Approved Amount" UniqueName="AppovedAmount" HeaderStyle-Width="70%" DataFormatString="{0:###,##0.00}">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Comment" HeaderText="Comment" UniqueName="Comment" HeaderStyle-Width="70%">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridDateTimeColumn DataField="CreatedOn" HeaderText="CreatedOn" EnableTimeIndependentFiltering="true">
                                        <HeaderStyle Width="130px" />
                                    </telerik:GridDateTimeColumn>
                                </Columns>
                                <PagerStyle PageSizes="5,10,15" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                                    PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />
                            </MasterTableView>
                        </telerik:RadGrid>
                    </div>
                </div>
            </div>
        </div>
        <div class="modal fade" id="squarespaceWorkFlowModal" tabindex="-1" role="dialog" aria-labelledby="modalLabel" aria-hidden="true">
            <div class="modal-dialog1">
                <div class="modal-content">
                    <div class="modal-header modal-header-resize">
                        <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">×</span><span class="sr-only">Close</span></button>
                        <h4 class="modal-title text-primary" id="lineModalWorkFlowLabel">Request Already Under Process</h4>
                    </div>
                    <div class="modal-body">
                        <telerik:RadGrid RenderMode="Lightweight" ID="grdWorkFlowInProcess" runat="server" OnNeedDataSource="grdWorkFlowInProcess_NeedDataSource"
                            AutoGenerateColumns="false" AllowPaging="true" AllowSorting="true" AllowFilteringByColumn="true" PageSize="10">
                            <GroupingSettings CaseSensitive="false" />
                            <MasterTableView CommandItemSettings-ShowRefreshButton="false" ClientDataKeyNames="" EditMode="InPlace" CommandItemDisplay="None" AllowFilteringByColumn="true">
                                <NoRecordsTemplate>
                                    <table width="100%" border="0" cellpadding="20" cellspacing="20">
                                        <tr>
                                            <td align="center" class="txt-white" class="txt-white">No records to display.
                                            </td>
                                        </tr>
                                    </table>
                                </NoRecordsTemplate>
                                <CommandItemSettings ShowRefreshButton="false" ShowExportToExcelButton="false" />
                                <Columns>
                                    <telerik:GridBoundColumn DataField="DocumentNumber" HeaderText="Document Number" UniqueName="DocumentNo">
                                    </telerik:GridBoundColumn>
                                    <%--  <telerik:GridBoundColumn DataField="ModifiedOn" HeaderText="Submitted On" UniqueName="SubmittedOn">
                                </telerik:GridBoundColumn>--%>
                                    <telerik:GridDateTimeColumn HeaderText="Submitted On" UniqueName="ModifiedOn" DataType="System.DateTime" EnableTimeIndependentFiltering="true"
                                        FilterControlWidth="130px" DataFormatString="{0:dd-MM-yyyy}" DataField="ModifiedOn">
                                    </telerik:GridDateTimeColumn>
                                    <telerik:GridBoundColumn DataField="ApprovalStatus" HeaderText="Approval Status" UniqueName="ApprovalStatus">
                                    </telerik:GridBoundColumn>
                                </Columns>
                                <PagerStyle PageSizes="5,10,15" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                                    PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />
                            </MasterTableView>
                        </telerik:RadGrid>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade" id="squarespaceVendorSearch" tabindex="-1" role="dialog" aria-labelledby="modalLabel" aria-hidden="true">
             <div class="modal-dialog1" >
                  <div class="modal-content" style="overflow:hidden; height:auto">
                       <div class="modal-header modal-header-resize">
                        <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">×</span><span class="sr-only">Close</span></button>
                        <h4 class="modal-title text-primary" id="lineModalVndorSearchLabel">LookUp Vendor</h4>
                    </div>
                       <div class="modal-body" style="height:500px">
                           <div class="col-sm-12" style="margin-top:10px" >
                               <div class="col-sm-7" >
                               <div class="col-sm-2" style="text-align:left !important" >
                                  <label class="control-label lable-txt"  for="name">Vendor:   </label>
                              </div>
                             <div class="col-sm-8" >
                                 <telerik:RadTextBox ID="txtVendorSearch" Width="90%" runat="server" CssClass="search-query form-control"></telerik:RadTextBox>
                                 <%--<asp:RequiredFieldValidator  ID="rfvtxtVendorSearch" runat="server" ControlToValidate="txtVendorSearch" ForeColor="Red" ErrorMessage="* "></asp:RequiredFieldValidator>--%>
                                 </div>
                               <div class="col-sm-2">
                                   <asp:Button ID="btnSearchVendor"  OnClick="btnSearchVendor_Click"  CssClass="btn btn-grey" runat="server" Text="Search" />    
                             </div>

                               </div>
                                <div class="col-sm-5" >
                                    <div class="col-sm-3">
                                  <label class="control-label lable-txt" for="name">Company: </label>
                            </div >
                                <div class="col-sm-9">
                                 <telerik:RadTextBox ID="txtCompanyCode" Width="100%" Enabled="false" runat="server" CssClass="search-query form-control"></telerik:RadTextBox>                                 
                             </div>

                                </div>        
                           </div>
                         <div class="col-sm-12"  > 
                              <div style="overflow: hidden; overflow-x: auto; overflow-y: scroll; max-height: 400px !important">
                              <telerik:RadGrid RenderMode="Lightweight" ID="grdVendorSearch" runat="server" Width="100%"  OnNeedDataSource="grdVendorSearch_NeedDataSource"
                            AutoGenerateColumns="false" AllowPaging="false" AllowSorting="true" AllowFilteringByColumn="true" PageSize="10"  OnItemCommand="grdVendorSearch_ItemCommand">
                            <ClientSettings>

                                  <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" /> 
                            </ClientSettings>
                            <GroupingSettings CaseSensitive="false" />
                            <MasterTableView CommandItemSettings-ShowRefreshButton="false" ClientDataKeyNames="VendorCode" EditMode="InPlace" CommandItemDisplay="None" AllowFilteringByColumn="true" Font-Size="8">
                                
                                <NoRecordsTemplate>
                                    <table width="100%" border="0" cellpadding="20" cellspacing="20">
                                        <tr>
                                            <td align="center" class="txt-black" class="txt-white">No records to display.
                                            </td>
                                        </tr>
                                    </table>
                                </NoRecordsTemplate>
                                <CommandItemSettings ShowRefreshButton="false" ShowExportToExcelButton="false" />
                               
                                <Columns>
                              <telerik:GridTemplateColumn HeaderText="Vendor Code"  AllowFiltering="true"    DataField="VendorCode">
                                  <ItemTemplate>
                                      <asp:LinkButton ID="lnkVendor" runat="server"  CommandName="Select" Text='<%#Eval("VendorCode") %>'  CssClass="linkButtonColor"></asp:LinkButton>
                                  </ItemTemplate>
                              </telerik:GridTemplateColumn>                                    
                                    <telerik:GridBoundColumn DataField="VendorName" HeaderText="Vendor Name" UniqueName="VendorName"/>
                                    <telerik:GridBoundColumn DataField="City" HeaderText="City" UniqueName="City"/>
                                    <telerik:GridBoundColumn DataField="Region" HeaderText="Region" UniqueName="Region"/>                              
                                   
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
        </div>

          <div class="modal fade" id="squarespaceInitiator" tabindex="-1" role="dialog" aria-labelledby="modalLabel" aria-hidden="true">
             <div class="modal-dialog1" style="width:97% !important" >
                  <div class="modal-content" style="overflow:hidden; height:auto">
                       <div class="modal-header modal-header-resize">
                        <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">×</span><span class="sr-only">Close</span></button>
                        <h4 class="modal-title text-primary" id="lineModalInitiator">Payment Initiator</h4>
                    </div>
                   
                      <div class="modal-body" style=" margin:2px;height:90% !important" >
                             <div class="col-sm-12" style="margin-top:2px; height:auto"> 
                          <div class="col-sm-5 padding-0">    
                              <div class="col-sm-3 padding-0">                       
                                <asp:Label runat="server" ID="Label6" Text="Total INR Amount:" CssClass="control-label lable-txt"></asp:Label>&nbsp;                             
                              </div>
                               <div class="col-sm-2 padding-0" style="margin-left: -15px;">     
                                <asp:Label ID="Label7" runat="server" CssClass="control-label lable-txt" Text="0"></asp:Label>&nbsp;&nbsp;                                                        
                                </div>
                               <div class="col-sm-4 padding-0 text-right">     
                                <asp:Label runat="server" ID="Label8" CssClass="control-label lable-txt" Text="Total Proposed Amount:"></asp:Label>&nbsp;
                                </div>
                               <div class="col-sm-2" style="margin-left: -7px;">     
                                <asp:Label ID="Label9" runat="server" CssClass="control-label lable-txt" Text="0"></asp:Label>
                                </div>
                             
                         </div> 
                            <div class="col-sm-7 padding-0">  
                                 <div class="col-sm-2 padding-0">  
                                 <asp:Label ID="Label10" runat="server" CssClass="control-label lable-txt" Text="Nature Of Request"></asp:Label>
                                </div>
                                  <div class="col-sm-4 padding-0"> 
                                 <telerik:RadDropDownList DefaultMessage="-Select-" Width="125px"  DropDownHeight="150px" ID="cmbNatureofRequestPopUp" OnSelectedIndexChanged="cmbNatureofRequestPopUp_SelectedIndexChanged" runat="server" AutoPostBack="true">                                
                                 </telerik:RadDropDownList>                                 
                                 </div>    
                            <div class="col-sm-6 padding-0">
                                  <div class="col-sm-4 text-right">  
                                 <asp:Label ID="Label11" runat="server" CssClass="control-label lable-txt" Text="Sub Vertival"></asp:Label>
                                </div>
                                  <div class="col-sm-8 padding-0"> 
                                     <telerik:RadDropDownList DefaultMessage="-Select-" Width="125px"  DropDownHeight="150px" ID="cmbSubVerticalPopUp" OnSelectedIndexChanged="cmbSubVerticalPopUp_SelectedIndexChanged" runat="server" AutoPostBack="true">                                
                                     </telerik:RadDropDownList>                                 
                                 </div>    

                            </div>

                         </div>        
                       </div>
                             <div class="col-sm-12"> 

                           <telerik:RadGrid RenderMode="Lightweight"  Width="100%" ExportSettings-ExportOnlyData="true" ID="grdInitiator" OnItemDataBound="grdInitiator_ItemDataBound" runat="server" AutoGenerateColumns="false" 
                                         AllowPaging="false" AllowSorting="true" AllowFilteringByColumn="true" AllowMultiRowSelection="true" OnNeedDataSource="grdInitiator_NeedDataSource"  OnItemCommand="grdInitiator_ItemCommand"  >
                                        <ExportSettings IgnorePaging="true" Excel-Format="ExcelML" ExportOnlyData="true" />
                                        <GroupingSettings CaseSensitive="false" />
                                        <ClientSettings>
                                             <Resizing AllowColumnResize="true" EnableRealTimeResize="true" />
                                            <Selecting AllowRowSelect="true"></Selecting>
                                            <ClientEvents OnRowSelected="RowSelected" OnRowDeselected="RowSelected" />
                                            <Scrolling AllowScroll="True" UseStaticHeaders="True" ScrollHeight="350px" SaveScrollPosition="true"  FrozenColumnsCount="3"></Scrolling>
                                        </ClientSettings>
                                        <MasterTableView CommandItemSettings-ShowRefreshButton="false" EditMode="InPlace" CommandItemDisplay="None" 
                                            AllowFilteringByColumn="true" AllowSorting="true" NoMasterRecordsText="" DataKeyNames="" TableLayout="Auto" Font-Size="8">
                                             <CommandItemSettings ShowRefreshButton="false" />
                                            <NoRecordsTemplate>
                                                <table width="100%" border="0" cellpadding="20" cellspacing="20">
                                                    <tr>
                                                        <td align="center">No records to display.
                                                        </td>
                                                    </tr>
                                                </table>
                                            </NoRecordsTemplate>
                                         <Columns>
                                                 <telerik:GridClientSelectColumn   HeaderStyle-Width="70px" HeaderStyle-Height="36px" HeaderText="Select" UniqueName="RequestSelect">
                                                </telerik:GridClientSelectColumn>
                                                   <telerik:GridBoundColumn   HeaderText="Vendor Code" HeaderStyle-Width="100px" HeaderStyle-Height="36px" UniqueName="VendorCode" DataField="VendorCode">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn HeaderText="Vendor Name" HeaderStyle-Width="170px" HeaderStyle-Height="36px" UniqueName="VendorName" DataField="VendorName">
                                                </telerik:GridBoundColumn>
                                                 <telerik:GridBoundColumn HeaderText="Document Number" HeaderStyle-Width="100px" HeaderStyle-Height="36px" UniqueName="DPRNumber" DataField="DPRNumber">
                                                </telerik:GridBoundColumn>       
                                                <telerik:GridBoundColumn HeaderText="Reference" HeaderStyle-Width="150px" HeaderStyle-Height="36px" UniqueName="Reference" DataField="Reference">
                                                </telerik:GridBoundColumn>   
                                                  <telerik:GridBoundColumn  HeaderText="Amount in INR" UniqueName="Amount" HeaderStyle-Width="150px" HeaderStyle-Height="36px" DataField="Amount" DataFormatString="{0:###,##0.00}">
                                                </telerik:GridBoundColumn>                                                
                                                 <telerik:GridTemplateColumn  HeaderText="Amount Proposed" HeaderStyle-Width="150px" UniqueName="AmountProposed">
                                                    <ItemTemplate>
                                                        <telerik:RadNumericTextBox ID="tbAmountProposed" Text='<%#String.Format("{0:###,##0.00}",Eval("Amount"))%>' Width="125px" class="form-control" runat="server"
                                                            ClientEvents-OnValueChanged='<%# "function (s,a){ValidateRowAgainst(s,a,"+Container.ItemIndex+","+ Eval("Amount") +");}" %>'></telerik:RadNumericTextBox>
                                                   </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                              <telerik:GridBoundColumn  HeaderStyle-Width="80px" HeaderText="Debit/Credit" UniqueName="DCFLag" DataField="DCFLag" >
                                                </telerik:GridBoundColumn>  
                                               <telerik:GridBoundColumn HeaderText="Document Currency" HeaderStyle-Width="100px" HeaderStyle-Height="36px" UniqueName="Currency" DataField="Currency">
                                                </telerik:GridBoundColumn>
                                                  <telerik:GridDateTimeColumn HeaderStyle-Width="150px" HeaderStyle-Height="36px" HeaderText="Net due date" EnableTimeIndependentFiltering="true" FilterControlWidth="130px" UniqueName="RequestNetduedate"
                                                    DataType="System.DateTime" DataFormatString="{0:dd-MM-yyyy}" DataField="BaseLineDate" Visible="true">
                                                </telerik:GridDateTimeColumn>
                                                  <telerik:GridTemplateColumn HeaderText="Nature of Request" HeaderStyle-Width="150px" HeaderStyle-Height="36px" UniqueName="NatureOfRequest" ItemStyle-VerticalAlign="Middle">
                                                    <ItemTemplate>
                                                        <telerik:RadDropDownList DefaultMessage="-Select-" RenderMode="Lightweight" Width="125px" HeaderStyle-Width="170px" HeaderStyle-Height="36px" ID="cmbNatureofRequest" runat="server">
                                                        </telerik:RadDropDownList>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                          <telerik:GridTemplateColumn HeaderStyle-Width="130px" HeaderStyle-Height="36px" HeaderText="Remarks" UniqueName="Remarks">
                                                    <ItemTemplate>
                                                        <telerik:RadTextBox ID="tbRequestRemarks" TextMode="MultiLine" Text="" runat="server"></telerik:RadTextBox>
                                                      </ItemTemplate>
                                                </telerik:GridTemplateColumn>   
                                              <telerik:GridTemplateColumn HeaderStyle-Width="170px" HeaderStyle-Height="36px" HeaderText="Attachment" AllowFiltering="false" AllowSorting="false" UniqueName="Attachments">
                                                    <ItemTemplate>
                                                        <asp:Button ID="btnAttachment" runat="server" CommandName="Attachment"  CommandArgument='<%# Eval("DPRNumber") %>'  Text="Add/View" CssClass="btn btn-grey"></asp:Button>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>                                             
                                                                      
                                                   <telerik:GridDateTimeColumn HeaderText="Posting Date" HeaderStyle-Width="150px" HeaderStyle-Height="36px" FilterControlWidth="130px" UniqueName="PostDate" EnableTimeIndependentFiltering="true"
                                                    DataType="System.DateTime" DataFormatString="{0:dd-MM-yyyy}" DataField="PostDate">
                                                </telerik:GridDateTimeColumn>
                                                 <telerik:GridBoundColumn HeaderText="Fiscal Year" HeaderStyle-Width="100px" HeaderStyle-Height="36px" UniqueName="FiscalYear" DataField="FiscalYear">
                                                </telerik:GridBoundColumn>
                                                   <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderStyle-Height="36px" HeaderText="Profit Centre" UniqueName="ProfitCentre" DataField="ProfitCentre">
                                                </telerik:GridBoundColumn>
                                                 <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderStyle-Height="36px" HeaderText="Business Area" UniqueName="BusinessArea" DataField="BusinessArea">
                                                </telerik:GridBoundColumn>
                                                 <telerik:GridBoundColumn HeaderText="Company Code" HeaderStyle-Width="100px" HeaderStyle-Height="36px" UniqueName="CompanyCode" DataField="CompanyCode">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridTemplateColumn HeaderStyle-Width="150px" HeaderStyle-Height="36px" HeaderText="Sub Vertical" UniqueName="SubVertical">
                                                    <ItemTemplate>
                                                        <telerik:RadDropDownList DefaultMessage="-Select-"  RenderMode="Lightweight" ID="cmbSubVertical" runat="server">
                                                        </telerik:RadDropDownList>
                                                        <asp:Label ID="lblRequestSubVertical" runat="server" Style="display: none"></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>                                             
                                                      <telerik:GridTemplateColumn  HeaderText="Vertical" UniqueName="Vertical" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbVerticalRequest" runat="server"></asp:Label>
                                                        <asp:Label ID="lblVerticalIDRequest" Style="display: none" runat="server"></asp:Label>
                                                        <asp:Label ID="Label1" Style="display: none" runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>   
                                                <telerik:GridDateTimeColumn  HeaderText="BaseLineDate" EnableTimeIndependentFiltering="true" UniqueName="BaseLineDate"
                                                DataType="System.DateTime" DataFormatString="{0:dd-MM-yyyy}" DataField="BaseLineDate" Display="false">
                                                </telerik:GridDateTimeColumn>
                                                <telerik:GridBoundColumn  HeaderText="DueDays" UniqueName="DueDays" DataField="DueDays" Display="false">
                                                </telerik:GridBoundColumn>   
                                                     <telerik:GridDateTimeColumn HeaderText="DocumentDate" EnableTimeIndependentFiltering="true" UniqueName="DocumentDate"
                                                DataType="System.DateTime" DataFormatString="{0:dd-MM-yyyy}" DataField="DocumentDate" Display="false">
                                                </telerik:GridDateTimeColumn>
                                                <telerik:GridBoundColumn  HeaderText="PaymentMethod" UniqueName="PaymentMethod" DataField="PaymentMethod" Display="false">
                                                </telerik:GridBoundColumn>   
                                                
                                            </Columns>
                                     
                                            <PagerStyle PageSizes="5,10,15" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                                                PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />
                                        </MasterTableView>
                                    </telerik:RadGrid>

                           <telerik:RadGrid Width="100%" RenderMode="Lightweight" ID="grdInitiator_Advance" runat="server" ExportSettings-ExportOnlyData="true"  AutoGenerateColumns="false" 
                                        AllowPaging="false" AllowSorting="true" AllowFilteringByColumn="true" AllowMultiRowSelection="true" OnItemDataBound="grdInitiator_Advance_ItemDataBound" OnNeedDataSource="grdInitiator_Advance_NeedDataSource" OnItemCommand="grdInitiator_Advance_ItemCommand"  >
                                        <ExportSettings IgnorePaging="true" Excel-Format="ExcelML" ExportOnlyData="true" />
                                        <GroupingSettings CaseSensitive="false" />
                                        <ClientSettings>
                                            <Selecting AllowRowSelect="true"></Selecting>
                                            <ClientEvents OnRowSelected="RowSelectedgrdInitiator_Advance" OnRowDeselected="RowSelectedgrdInitiator_Advance" />
                                            <Scrolling AllowScroll="True" UseStaticHeaders="True" ScrollHeight="350px" SaveScrollPosition="true" FrozenColumnsCount="3"></Scrolling>
                                        </ClientSettings>
                                        <MasterTableView CommandItemSettings-ShowRefreshButton="false" EditMode="InPlace" DataKeyNames="" CommandItemDisplay="None"
                                            AllowFilteringByColumn="true" AllowSorting="true" NoMasterRecordsText="" TableLayout="Fixed" Font-Size="8">
                                            <HeaderStyle Height="36px" />
                                            <CommandItemSettings ShowRefreshButton="false" ShowExportToExcelButton="false" />
                                            <NoRecordsTemplate>
                                                <table width="100%" border="0" cellpadding="20" cellspacing="20">
                                                    <tr>
                                                        <td align="center">No records to display.
                                                        </td>
                                                    </tr>
                                                </table>
                                            </NoRecordsTemplate>
                                            <Columns>
                                                <telerik:GridClientSelectColumn HeaderStyle-Width="70px" HeaderText="Select" UniqueName="Select">
                                                </telerik:GridClientSelectColumn>
                                                     <telerik:GridBoundColumn HeaderStyle-Width="100px" HeaderText="Vendor Code" UniqueName="VendorCode" DataField="VendorCode">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn HeaderStyle-Width="170px" HeaderText="Vendor Name" UniqueName="VendorName" DataField="VendorName">
                                                </telerik:GridBoundColumn>
                                                 <telerik:GridBoundColumn HeaderStyle-Width="100px" HeaderText="DPR Number" UniqueName="DPRNumber" DataField="DPRNumber">
                                                </telerik:GridBoundColumn>
                                                   <telerik:GridBoundColumn HeaderStyle-Width="150px"   HeaderText="Gross Amount" UniqueName="GrossAmount" DataField="GrossAmount" DataFormatString="{0:###,##0.00}">
                                                </telerik:GridBoundColumn>


                                                <telerik:GridBoundColumn HeaderStyle-Width="150px"  HeaderText="Tax Rate" UniqueName="TaxRate" DataField="TaxRate" DataFormatString="{0:###,##0.00}">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn HeaderStyle-Width="150px"  HeaderText="Tax Amount" UniqueName="TaxAmount" DataField="TaxAmount" DataFormatString="{0:###,##0.00}">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn HeaderStyle-Width="150px"  HeaderText="Amount in INR" UniqueName="Amount" DataField="Amount" DataFormatString="{0:###,##0.00}">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridTemplateColumn HeaderStyle-Width="150px"  HeaderText="Net Amount" UniqueName="AmountProposed">
                                                    <ItemTemplate>
                                                        <telerik:RadNumericTextBox ID="tbProposed" Text='<%#String.Format("{0:###,##0.00}",Eval("Amount")) %>' Width="125px" class="form-control" runat="server"
                                                            ClientEvents-OnValueChanged='<%# "function (s,a){ValidateRow(s,a,"+Container.ItemIndex+","+ Eval("Amount") +");}" %>' Enabled="false"
                                                            ></telerik:RadNumericTextBox>
                                                        <%--<asp:Label ID="lblAmountProposed" runat="server" Style="display: none" Text='<%# String.Format("{0:###,##0.00}",Eval("Amount")) %>'></asp:Label>--%>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                 <telerik:GridBoundColumn  HeaderStyle-Width="80px" HeaderText="Debit/Credit" UniqueName="DCFLag" DataField="DCFLag" >
                                            </telerik:GridBoundColumn>  
                                                      <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="Un-Settled Open Advance (INR)" UniqueName="UnsettledOpenAdvance" 
                                                    DataFormatString="{0:###,##0.00}" DataField="UnsettledOpenAdvance">
                                                </telerik:GridBoundColumn>
                                                 <telerik:GridTemplateColumn HeaderStyle-Width="150px" HeaderText="Withholding Tax Code" UniqueName="WithholdingTaxCode">
                                                    <ItemTemplate>
                                                        <telerik:RadTextBox ID="tbWithholdingTaxCode" Text='<%#Eval("WithholdingTaxCode") %>' Width="125px" class="form-control" runat="server" MaxLength="20"></telerik:RadTextBox>
                                                       <%-- <asp:RequiredFieldValidator Enabled="false" ID="rfvWithholdingTaxCode" runat="server" ControlToValidate="tbWithholdingTaxCode" ForeColor="Red" ErrorMessage="* Required"></asp:RequiredFieldValidator>--%>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                 <telerik:GridTemplateColumn HeaderStyle-Width="150px" HeaderText="Nature of Request" UniqueName="NatureOfRequest">
                                                    <ItemTemplate>
                                                        <telerik:RadDropDownList DefaultMessage="-Select-" RenderMode="Lightweight" ID="cmbNatureofRequest" runat="server">
                                                        </telerik:RadDropDownList>
                                                         <%-- <asp:RequiredFieldValidator ID="rfvNatureOFReq1" runat="server" ControlToValidate="cmbNatureofRequest" ForeColor="Red" ErrorMessage="* Required"></asp:RequiredFieldValidator>
                                                        <asp:Label ID="lblRequestNatureofRequest" runat="server" Style="display: none"></asp:Label>--%>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                  <telerik:GridTemplateColumn HeaderStyle-Width="170px" HeaderText="Justification for Adv Payment" UniqueName="JustificationforAdvPayment" >
                                                    <ItemTemplate>
                                                        <telerik:RadTextBox ID="tbJustificationforAdvPayment" Text="" Width="150px" class="form-control" runat="server" MaxLength="40"></telerik:RadTextBox>
                                                        <%--<asp:RequiredFieldValidator Enabled="false" ID="rfvJustificationforAdvPayment" runat="server" ControlToValidate="tbJustificationforAdvPayment" ForeColor="Red" ErrorMessage="* Required"></asp:RequiredFieldValidator>--%>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderStyle-Width="150px" HeaderText="Remarks" UniqueName="Remarks">
                                                    <ItemTemplate>
                                                        <telerik:RadTextBox ID="tbRemarks" TextMode="MultiLine" Text="" Width="125px" runat="server" MaxLength="250"></telerik:RadTextBox>
                                                      <%--  <asp:RequiredFieldValidator Enabled="false" ID="rfvRemarks" runat="server" ControlToValidate="tbRemarks" ForeColor="Red" ErrorMessage="* Required"></asp:RequiredFieldValidator>--%>
                                                        <%--<asp:Label ID="lblRequestRemarks" runat="server" Style="display: none" Text='<%#Eval("Comment") %>'></asp:Label>--%>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                   <telerik:GridTemplateColumn HeaderStyle-Width="170px" HeaderStyle-Height="36px" HeaderText="Attachment" AllowFiltering="false" AllowSorting="false" UniqueName="Attachments">
                                                    <ItemTemplate>
                                                        <asp:Button ID="btnAttachment" runat="server" CommandName="Attachment"  CommandArgument='<%# Eval("DPRNumber") %>'  Text="Add/View" CssClass="btn btn-grey"></asp:Button>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                 <telerik:GridDateTimeColumn HeaderStyle-Width="150px" HeaderText="Document Date" FilterControlWidth="130px" UniqueName="AdvanceDocumentDate" EnableTimeIndependentFiltering="true"
                                                    DataType="System.DateTime" DataFormatString="{0:dd-MM-yyyy}" DataField="Documentdate">
                                                </telerik:GridDateTimeColumn>
                                                 <telerik:GridDateTimeColumn HeaderStyle-Width="150px" HeaderText="Posting Date" FilterControlWidth="130px" UniqueName="PostDate" EnableTimeIndependentFiltering="true"
                                                    DataType="System.DateTime" DataFormatString="{0:dd-MM-yyyy}" DataField="PostingDate">
                                                </telerik:GridDateTimeColumn>
                                                            <telerik:GridDateTimeColumn HeaderStyle-Width="150px" HeaderText="Expected Clearing Date" FilterControlWidth="130px" UniqueName="ExpectedClearingDate" DataType="System.DateTime"
                                                    EnableTimeIndependentFiltering="true" DataFormatString="{0:dd-MM-yyyy}" DataField="ExpectedClearingDate">
                                                </telerik:GridDateTimeColumn>
                                                   <telerik:GridBoundColumn HeaderStyle-Width="100px" HeaderText="Fiscal Year" UniqueName="FiscalYear" DataField="FiscalYear">
                                                </telerik:GridBoundColumn>
                                                     <telerik:GridBoundColumn HeaderStyle-Width="120px" HeaderText="Purchasing Document" UniqueName="PurchasingDocument" DataField="PurchasingDocument">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn HeaderStyle-Width="120px" HeaderText="Special GL" UniqueName="SpecialGL" DataField="SpecialGL">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="Profit Centre" UniqueName="ProfitCentre" DataField="ProfitCentre">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="Business Area" UniqueName="BusinessArea" DataField="BusinessArea">
                                                </telerik:GridBoundColumn>
                                                 <telerik:GridBoundColumn HeaderStyle-Width="100px" HeaderText="Company Code" UniqueName="CompanyCode" DataField="CompanyCode">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridTemplateColumn HeaderStyle-Width="150px" HeaderText="Sub Vertical" UniqueName="SubVertical">
                                                    <ItemTemplate>
                                                        <telerik:RadDropDownList DefaultMessage="-Select-" RenderMode="Lightweight" AutoPostBack="true" OnSelectedIndexChanged="cmbAdvReqSubVertical_SelectedIndexChanged" ID="cmbAdvReqSubVertical" runat="server">
                                                        </telerik:RadDropDownList>
                                                        <asp:Label ID="lblRequestSubVertical" runat="server" Style="display: none"></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>                                        
                                                <telerik:GridTemplateColumn HeaderStyle-Width="150px" HeaderText="Vertical" UniqueName="Vertical">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbVerticalAdvReq" runat="server"></asp:Label>
                                                        <asp:Label ID="lbVerticalIdAdvReq" Style="display: none" runat="server"></asp:Label>
                                                        <asp:Label ID="Label1" Style="display: none" runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>                                     
                                            <telerik:GridBoundColumn HeaderStyle-Width="170px" HeaderStyle-Height="36px" HeaderText="PaymentMethod" UniqueName="PaymentMethod" DataField="PaymentMethod" Display="false">
                                            </telerik:GridBoundColumn>  
                                            <telerik:GridBoundColumn HeaderStyle-Width="170px" HeaderStyle-Height="36px" HeaderText="POLineItemNo" UniqueName="POLineItemNo" DataField="POLineItemNo" Display="false">
                                            </telerik:GridBoundColumn>  
                                                <telerik:GridBoundColumn HeaderStyle-Width="170px" HeaderText="PO Line Item Text" UniqueName="POItemText" DataField="POItemText" Display="false">
                                            </telerik:GridBoundColumn>
                                                  <telerik:GridBoundColumn  HeaderStyle-Width="170px" HeaderText="UnsettleAmount" UniqueName="UnsetAmount" DataField="UnsetAmount" Display="false">
                                            </telerik:GridBoundColumn>
                                                  <telerik:GridBoundColumn HeaderStyle-Width="170px" HeaderText="HouseBank" UniqueName="HouseBank" DataField="HouseBank" Display="false">
                                            </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn HeaderStyle-Width="170px" HeaderText="Currency" UniqueName="Currency" DataField="Currency" Display="false">
                                            </telerik:GridBoundColumn>
                                                 <telerik:GridBoundColumn HeaderStyle-Width="170px" HeaderText="Reference" UniqueName="Reference" DataField="ReferenceDocumentNumber" Display="false">
                                            </telerik:GridBoundColumn>
                                                
                                            </Columns>
                                            <PagerStyle PageSizes="5,10,15" PagerTextFormat="{4}<strong>{5}</strong> records matching your search criteria"
                                                PageSizeLabelText="Records per page:" Mode="NextPrevAndNumeric" HorizontalAlign="Right" VerticalAlign="Middle" />
                                        </MasterTableView>
                                    </telerik:RadGrid>

                            </div>
                            <div class="col-lg-12 padding-0 margin-10"> 
                                <div class="col-lg-5 padding-0 margin-10"> </div>
                                <div class="col-lg-7 padding-0 margin-10"> 
                                <asp:LinkButton   CssClass="btn btn-grey" OnClick="lnkSubmitInitiator_Click" CommandArgument="Submit" runat="server" ID="lnkSubmitInitiator">Submit</asp:LinkButton>
                            </div>
                            </div>
                            
                      </div>
                          
                         
                      </div>
                 </div>
              </div>
        
        </div>
</asp:Content>
