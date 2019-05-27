using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SuzlonBPP
{
    public partial class VendorPaymentApprover : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("TotalAmount", typeof(string)));
            dt.Columns.Add(new DataColumn("ProposedTotal", typeof(string)));
            dt.Columns.Add(new DataColumn("ApprovedTotal", typeof(string)));
            dt.Columns.Add(new DataColumn("TodayTotal", typeof(string)));
            dt.Columns.Add(new DataColumn("i", typeof(int)));

            for (int i = 1; i < 3; i++)
            {
               
                DataRow dr = dt.NewRow();
                dr["i"] = i;
                dr["TotalAmount"] = "10,000";
                dr["ProposedTotal"] = "10,00000";
                dr["ApprovedTotal"] = "10,00000";
                dr["TodayTotal"] = "10,000000";
                dt.Rows.Add(dr);
            }

            gvPendingApproval.DataSource = dt;
            gvPendingApproval.DataBind();
            gvApproved.DataSource = dt;
            gvApproved.DataBind();
            gvCorrection.DataSource = dt;
            gvCorrection.DataBind();

        }

     

        protected void gvPendingApproval_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridNestedViewItem)
            {
                GridNestedViewItem item = e.Item as GridNestedViewItem;
                RadGrid grid = (RadGrid)item.FindControl("gvPendingApproval1"); // Get the inner RadGrid 
                DataTable dt = new DataTable();
                dt.Columns.Add(new DataColumn("i", typeof(int)));
                dt.Columns.Add(new DataColumn("TotalAmount", typeof(string)));
                dt.Columns.Add(new DataColumn("ProposedTotal", typeof(string)));
                dt.Columns.Add(new DataColumn("ApprovedTotal", typeof(string)));
                dt.Columns.Add(new DataColumn("TodayTotal", typeof(string)));

                for (int i = 1; i < 4; i++)
                {
                    DataRow dr = dt.NewRow();
                    dr["i"] = i;
                    dr["TotalAmount"] = "10,000";
                    dr["ProposedTotal"] = "10,00000";
                    dr["ApprovedTotal"] = "10,00000";
                    dr["TodayTotal"] = "10,000000";
                    dt.Rows.Add(dr);
                }

                grid.DataSource = dt;
                grid.DataBind();
            }
        }

        protected void gvPendingApproval1_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridNestedViewItem)
            {
                GridNestedViewItem item = e.Item as GridNestedViewItem;
                RadGrid grid = (RadGrid)item.FindControl("gvPendingApproval2"); // Get the inner RadGrid 
                DataTable dt = new DataTable();
                dt.Columns.Add(new DataColumn("SubVertical", typeof(string)));
                dt.Columns.Add(new DataColumn("AssignedBy", typeof(string)));
                dt.Columns.Add(new DataColumn("CompanyCode", typeof(string)));
                dt.Columns.Add(new DataColumn("VendorCode", typeof(string)));
                dt.Columns.Add(new DataColumn("VendorName", typeof(string)));
                dt.Columns.Add(new DataColumn("NatureOfRequest", typeof(string)));
                dt.Columns.Add(new DataColumn("DRPNumber", typeof(string)));
                dt.Columns.Add(new DataColumn("Reference", typeof(string)));
                dt.Columns.Add(new DataColumn("FiscalYear", typeof(string)));
                dt.Columns.Add(new DataColumn("PostDate", typeof(string)));
                dt.Columns.Add(new DataColumn("Amount", typeof(string)));
                dt.Columns.Add(new DataColumn("AmountProposed", typeof(string)));
                dt.Columns.Add(new DataColumn("Currency", typeof(string)));
                dt.Columns.Add(new DataColumn("Netduedate", typeof(string)));
                dt.Columns.Add(new DataColumn("BusinessArea", typeof(string)));
                dt.Columns.Add(new DataColumn("ProfitCentre", typeof(string)));
                dt.Columns.Add(new DataColumn("Vertical", typeof(string)));
                dt.Columns.Add(new DataColumn("Status", typeof(string)));
                dt.Columns.Add(new DataColumn("ApprovalStatus", typeof(string)));
                dt.Columns.Add(new DataColumn("Remarks", typeof(string)));
                dt.Columns.Add(new DataColumn("Attachments", typeof(string)));
                dt.Columns.Add(new DataColumn("PurchasingDocument", typeof(string)));
                dt.Columns.Add(new DataColumn("POLineItem", typeof(string)));
                dt.Columns.Add(new DataColumn("SpecialGL", typeof(string)));
                dt.Columns.Add(new DataColumn(" WithholdingTaxCode", typeof(string)));
                dt.Columns.Add(new DataColumn("UnSettledOpenAdvance", typeof(string)));
                dt.Columns.Add(new DataColumn("AdvancePaymentJustification", typeof(string)));

                for (int i = 1; i < 4; i++)
                {
                    DataRow dr = dt.NewRow();

                    dr["SubVertical"] = "OMS";
                    dr["AssignedBy"] = "Amol";
                    dr["CompanyCode"] = "1111";
                    dr["VendorCode"] = "1234";
                    dr["VendorName"] = "Ambit";
                    dr["NatureOfRequest"] = "Select Nature";
                    dr["DRPNumber"] = "5555";
                    dr["Reference"] = "abc";
                    dr["FiscalYear"] = "20/04/2016";
                    dr["PostDate"] = "20/04/2016";
                    dr["Amount"] = "20,0000";
                    dr["AmountProposed"] = "";
                    dr["Currency"] = "INR";
                    dr["Netduedate"] = "20/04/2016";
                    dr["BusinessArea"] = "IT";
                    dr["ProfitCentre"] = "PO-001";
                    dr["Vertical"] = "SCM";
                    dr["Status"] = "";
                    dr["ApprovalStatus"] = "Approved";
                    dr["Remarks"] = "Testing";
                    dr["Attachments"] = "Attachment";
                    dr["PurchasingDocument"] = "Document 1";
                    dr["POLineItem"] = "abcd";
                    dr["SpecialGL"] = "abcd1234";
                    dr[" WithholdingTaxCode"] = "10,000";
                    dr["UnSettledOpenAdvance"] = "40,000";
                    dr["AdvancePaymentJustification"] = "";

                    dt.Rows.Add(dr);
                }

                grid.DataSource = dt;
                grid.DataBind();
            }
        }

        protected void gvApproved_DetailTableDataBind(object sender, GridDetailTableDataBindEventArgs e)
        {
            GridDataItem dataItem = (GridDataItem)e.DetailTableView.ParentItem;
        

            if (e.DetailTableView.Name == "gvApproved1")
            {
                DataTable dt = new DataTable();
                dt.Columns.Add(new DataColumn("i", typeof(int)));
                dt.Columns.Add(new DataColumn("TotalAmount", typeof(string)));
                dt.Columns.Add(new DataColumn("ProposedTotal", typeof(string)));
                dt.Columns.Add(new DataColumn("ApprovedTotal", typeof(string)));
                dt.Columns.Add(new DataColumn("TodayTotal", typeof(string)));

                for (int i = 1; i < 4; i++)
                {
                    DataRow dr = dt.NewRow();
                    dr["i"] = i;
                    dr["TotalAmount"] = "23,232";
                    dr["ProposedTotal"] = "10,656";
                    dr["ApprovedTotal"] = "10,075666";
                    dr["TodayTotal"] = "10,90766";
                    dt.Rows.Add(dr);
                }

                e.DetailTableView.DataSource = dt;
            }

            if (e.DetailTableView.Name == "gvApproved2")
            {
                DataTable dt = new DataTable();
                dt.Columns.Add(new DataColumn("SubVertical", typeof(string)));
                dt.Columns.Add(new DataColumn("AssignedBy", typeof(string)));
                dt.Columns.Add(new DataColumn("CompanyCode", typeof(string)));
                dt.Columns.Add(new DataColumn("VendorCode", typeof(string)));
                dt.Columns.Add(new DataColumn("VendorName", typeof(string)));
                dt.Columns.Add(new DataColumn("NatureOfRequest", typeof(string)));
                dt.Columns.Add(new DataColumn("DRPNumber", typeof(string)));
                dt.Columns.Add(new DataColumn("Reference", typeof(string)));
                dt.Columns.Add(new DataColumn("FiscalYear", typeof(string)));
                dt.Columns.Add(new DataColumn("PostDate", typeof(string)));
                dt.Columns.Add(new DataColumn("Amount", typeof(string)));
                dt.Columns.Add(new DataColumn("AmountProposed", typeof(string)));
                dt.Columns.Add(new DataColumn("Currency", typeof(string)));
                dt.Columns.Add(new DataColumn("Netduedate", typeof(string)));
                dt.Columns.Add(new DataColumn("BusinessArea", typeof(string)));
                dt.Columns.Add(new DataColumn("ProfitCentre", typeof(string)));
                dt.Columns.Add(new DataColumn("Vertical", typeof(string)));
                dt.Columns.Add(new DataColumn("Status", typeof(string)));
                dt.Columns.Add(new DataColumn("ApprovalStatus", typeof(string)));
                dt.Columns.Add(new DataColumn("Remarks", typeof(string)));
                dt.Columns.Add(new DataColumn("Attachments", typeof(string)));
                dt.Columns.Add(new DataColumn("PurchasingDocument", typeof(string)));
                dt.Columns.Add(new DataColumn("POLineItem", typeof(string)));
                dt.Columns.Add(new DataColumn("SpecialGL", typeof(string)));
                dt.Columns.Add(new DataColumn(" WithholdingTaxCode", typeof(string)));
                dt.Columns.Add(new DataColumn("UnSettledOpenAdvance", typeof(string)));
                dt.Columns.Add(new DataColumn("AdvancePaymentJustification", typeof(string)));

                for (int i = 1; i < 4; i++)
                {
                    DataRow dr = dt.NewRow();

                    dr["SubVertical"] = "OMS";
                    dr["AssignedBy"] = "Amol";
                    dr["CompanyCode"] = "1111";
                    dr["VendorCode"] = "1234";
                    dr["VendorName"] = "Ambit";
                    dr["NatureOfRequest"] = "Select Nature";
                    dr["DRPNumber"] = "5555";
                    dr["Reference"] = "abc";
                    dr["FiscalYear"] = "20/04/2016";
                    dr["PostDate"] = "20/04/2016";
                    dr["Amount"] = "20,0000";
                    dr["AmountProposed"] = "";
                    dr["Currency"] = "INR";
                    dr["Netduedate"] = "20/04/2016";
                    dr["BusinessArea"] = "IT";
                    dr["ProfitCentre"] = "PO-001";
                    dr["Vertical"] = "SCM";
                    dr["Status"] = "";
                    dr["ApprovalStatus"] = "Approved";
                    dr["Remarks"] = "Testing";
                    dr["Attachments"] = "Attachment";
                    dr["PurchasingDocument"] = "Document 1";
                    dr["POLineItem"] = "abcd";
                    dr["SpecialGL"] = "abcd1234";
                    dr[" WithholdingTaxCode"] = "10,000";
                    dr["UnSettledOpenAdvance"] = "40,000";
                    dr["AdvancePaymentJustification"] = "";

                    dt.Rows.Add(dr);
                }

              
                e.DetailTableView.DataSource = dt;
            }



        }

        protected void gvApproved_ItemCommand(object sender, GridCommandEventArgs e)
        {
           
        }
    }
}