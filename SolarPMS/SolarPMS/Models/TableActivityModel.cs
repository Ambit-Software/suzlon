using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SolarPMS.Models
{
    public class TableActivityModel
    {
        public static List<TableMasterData> GetTablemasterDataForDropdown(string FilterFlag, string FilterType, string Site = "", string Project = "",
            string Area = "", string Network = "", string ActivityId = "")
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                return solarPMSEntities.usp_GetTableMasterDataForDropdown(FilterFlag, FilterType, Site, Project, Area, Network, ActivityId).ToList();
            }
        }
        public TableActivity AddTableActivity(TableActivity tableActivity, int userId)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                tableActivity.CreatedBy = userId;
                tableActivity.CreatedOn = DateTime.Now;
                tableActivity.ModifiedBy = userId;
                tableActivity.ModifiedOn = DateTime.Now;
                solarPMSEntities.TableActivities.Add(tableActivity);
                solarPMSEntities.SaveChanges();
                solarPMSEntities.Configuration.ProxyCreationEnabled = false;
                solarPMSEntities.Configuration.LazyLoadingEnabled = false;
                return tableActivity;
            }
        }

        public bool TableActivityExists(TableActivity tableActivity)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                return solarPMSEntities.TableActivities.AsNoTracking().FirstOrDefault(t =>
                t.Site.ToLower() == tableActivity.Site.ToLower()
                && t.ProjectId == tableActivity.ProjectId
                && t.NetworkId == tableActivity.NetworkId
                && t.ActivityId == tableActivity.ActivityId
                && t.Flag == tableActivity.Flag
                && t.Number == tableActivity.Number
                && t.TableActivityId != tableActivity.TableActivityId) != null;
            }
        }

        public TableActivity GetBlock(TableActivity tableActivity)
        {

            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                TableActivity tActivity = solarPMSEntities.TableActivities.FirstOrDefault(p => p.Flag.ToLower() == tableActivity.Flag.ToLower()
                            && p.Number == tableActivity.Number);
                return tActivity;
            }

        }

        public List<TableActivity> GetTableActivityByRange(TableActivityRange tableActivityRange)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                var tableActvity = (from tableActivities in solarPMSEntities.TableActivities
                                    where tableActivities.Number >= tableActivityRange.FromRange
                                    && tableActivities.Number <= tableActivityRange.ToRange
                                    && tableActivities.Flag.ToLower() == tableActivityRange.Flag.ToLower()
                                    && tableActivities.Site == tableActivityRange.Site
                                    && tableActivities.ProjectId == tableActivityRange.ProjectId
                                    && tableActivities.AreaId == tableActivityRange.WBSAreaId
                                    && tableActivities.NetworkId == tableActivityRange.NetworkId
                                    && tableActivities.ActivityId == tableActivityRange.ActivityId
                                    && tableActivities.SubActivityId == (tableActivities.SubActivityId == null ? null : (tableActivities.SubActivityId == "" ? "" : tableActivityRange.SubActivityId))
                                    select tableActivities).ToList();

                TableActivity tActivity = tableActvity.Find(x => x.Number == tableActivityRange.FromRange);
                if (tActivity != null)
                {
                    tActivity = tableActvity.Find(x => x.Number == tableActivityRange.ToRange);
                }
                if (tActivity == null)
                {
                    return null;
                }
                else
                    return tableActvity;
            }
        }

        public static int[] FindMissingTableNo(int FromRange, int ToRange, TableActivityRange tableActivityRange)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                int[] numberArray = CommonFunctions.GetNumberRange(FromRange, ToRange);
                    var tableActvity = (from tableActivities in solarPMSEntities.TableActivities
                                        where numberArray.Contains(tableActivities.Number)
                                        && tableActivities.Flag.ToLower() == tableActivityRange.Flag.ToLower()
                                        && tableActivities.Site == tableActivityRange.Site
                                        && tableActivities.ProjectId == tableActivityRange.ProjectId
                                        && tableActivities.AreaId == tableActivityRange.WBSAreaId
                                        && tableActivities.NetworkId == tableActivityRange.NetworkId
                                        && tableActivities.ActivityId == tableActivityRange.ActivityId
                                        && tableActivities.SubActivityId == (tableActivities.SubActivityId == null ? null : (tableActivities.SubActivityId == "" ? "" : tableActivityRange.SubActivityId))
                                        select tableActivities.Number).ToArray();

                return numberArray.Except(tableActvity).ToArray();
            }
        }

        public static int[] FindExistingTableNo(int FromRange, int ToRange, TableActivityRange tableActivityRange)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                int[] numberArray = CommonFunctions.GetNumberRange(FromRange, ToRange);
                var tableActvity = (from tableActivities in solarPMSEntities.TableActivities
                                    where numberArray.Contains(tableActivities.Number)
                                    && tableActivities.Flag.ToLower() == tableActivityRange.Flag.ToLower()
                                    && tableActivities.Site == tableActivityRange.Site
                                    && tableActivities.ProjectId == tableActivityRange.ProjectId
                                    && tableActivities.AreaId == tableActivityRange.WBSAreaId
                                    && tableActivities.NetworkId == tableActivityRange.NetworkId
                                    && tableActivities.ActivityId == tableActivityRange.ActivityId
                                    && tableActivities.SubActivityId == (tableActivities.SubActivityId == null ? null : (tableActivities.SubActivityId == "" ? "" : tableActivityRange.SubActivityId))
                                    select tableActivities.Number).ToArray();

                return numberArray.Except(tableActvity).ToArray();
            }
        }
        public List<TableActivity> GetTableActivityByManualEntry(ManualEntryRange manualEntryRange)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                var tableActvity = (from tableActivities in solarPMSEntities.TableActivities
                                    where manualEntryRange.RangeArray.Contains(tableActivities.Number)
                                    && tableActivities.Flag.ToLower() == manualEntryRange.Flag.ToLower()
                                    && tableActivities.Site == manualEntryRange.Site
                                    && tableActivities.ProjectId == manualEntryRange.ProjectId
                                    && tableActivities.AreaId == manualEntryRange.WBSAreaId
                                    && tableActivities.NetworkId == manualEntryRange.NetworkId
                                    && tableActivities.ActivityId == manualEntryRange.ActivityId
                                    && tableActivities.SubActivityId == (tableActivities.SubActivityId == null ? null : (tableActivities.SubActivityId == "" ? "" : manualEntryRange.SubActivityId))
                                    select tableActivities).ToList();
                return tableActvity;
            }
        }

        public List<TableActivity> GetTableActivityByBlock(TableBlock tableBlock)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                var tableActvity = (from tableActivities in solarPMSEntities.TableActivities
                                    where tableActivities.Number == tableBlock.BlockNo
                                    && tableActivities.Flag.ToLower() == tableBlock.Flag.ToLower()
                                    && tableActivities.Site == tableBlock.Site
                                    && tableActivities.ProjectId == tableBlock.ProjectId
                                    && tableActivities.NetworkId == tableBlock.NetworkId
                                    && tableActivities.ActivityId == tableBlock.ActivityId
                                    select tableActivities).ToList();
                return tableActvity;
            }
        }
        public List<TimesheetBlockDetailsView> GetTimesheetBlockDetails(int TimesheetId, bool IsManager = false, short Status = 0)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                return solarPMSEntities.usp_GetTimesheetBlockDetails(TimesheetId, IsManager, Status).ToList();
            }
        }

        public List<TableActivity> GetBlocksForActivity(TableBlock tableBlock)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                var tableActvity = (from tableActivities in solarPMSEntities.TableActivities
                                    join wBSAreas in solarPMSEntities.WBSAreas on
                                    tableActivities.AreaId equals wBSAreas.WBSAreaId
                                    where tableActivities.Flag.ToLower() == tableBlock.Flag.ToLower()
                                    && tableActivities.Site == tableBlock.Site
                                    && tableActivities.ProjectId == tableBlock.ProjectId
                                    && tableActivities.NetworkId == tableBlock.NetworkId
                                    && tableActivities.ActivityId == tableBlock.ActivityId
                                    && wBSAreas.WBSArea1 == tableBlock.WBSArea
                                    select tableActivities).ToList();
                return tableActvity;
            }
        }

        public List<TableActivity> GetBlocksForSubActivity(TableBlock tableBlock)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                var tableActvity = (from tableActivities in solarPMSEntities.TableActivities
                                    join wBSAreas in solarPMSEntities.WBSAreas on
                                    tableActivities.AreaId equals wBSAreas.WBSAreaId
                                    where tableActivities.Flag.ToLower() == tableBlock.Flag.ToLower()
                                    && tableActivities.Site == tableBlock.Site
                                    && tableActivities.ProjectId == tableBlock.ProjectId
                                    && tableActivities.NetworkId == tableBlock.NetworkId
                                    && tableActivities.ActivityId == tableBlock.ActivityId
                                    && tableActivities.SubActivityId == tableBlock.SubActivityId
                                    && wBSAreas.WBSArea1 == tableBlock.WBSArea
                                    select tableActivities).ToList();
                return tableActvity;
            }
        }

        public static List<TableActivity> GetAllBlock(TableActivity tableActivity)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                List<TableActivity> lstActivity = solarPMSEntities.TableActivities
                                                  .Where(p => p.Flag.ToLower() == tableActivity.Flag.ToLower()
                                                         && p.Site == tableActivity.Site
                                                         && p.ProjectId == tableActivity.ProjectId
                                                         && p.AreaId == tableActivity.AreaId
                                                         && p.NetworkId == tableActivity.NetworkId
                                                         && p.ActivityId == tableActivity.ActivityId
                                                         && (p.SubActivityId == tableActivity.SubActivityId || p.SubActivityId == string.Empty)).ToList();
                return lstActivity;
            }
        }

        public static List<TimesheetActivity> GetTimesheetActivityDetails(int TimesheetId)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                return solarPMSEntities.TimesheetActivities.Where(t => t.TimesheetId == TimesheetId).ToList();
            }
        }

        /// <summary>
        /// Get praposed quanity for selected Block/Inverter/SCB/Table
        /// </summary>
        /// <param name="TableActivityId"></param>
        /// <returns></returns>
        public static int GetPraposedQuantity(int TableActivityId)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                return solarPMSEntities.TableActivities.Where(t => t.TableActivityId == TableActivityId).Select(t => t.Quantity).FirstOrDefault();
            }
        }

        public static int GetCompletedQuantity(string SubActivity, string Activity, string Network, int AreaId, string Project, string Site, int BlockNo, int TimesheetId)
        {
            int completedQuanity = 0;
            int qmApprovedQuanity = 0;
            int pmApprovedQuanity = 0;

            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                List<TimesheetBlockDetail> result = null;
                List<ManagerTimesheetBlockDetail> managerApprovedQuantity = null;
                int approvedByQM = Convert.ToInt32(Constants.WorkflowStatus.PartialApprovedByQM);
                int approvedByPM = Convert.ToInt32(Constants.WorkflowStatus.PartialApprovedByPM);
                int rejectedByQM = Convert.ToInt32(Constants.WorkflowStatus.QARejected);
                int rejectedByPM = Convert.ToInt32(Constants.WorkflowStatus.PMRejected);
                if (string.IsNullOrEmpty(SubActivity))
                {
                    result = solarPMSEntities.Timesheets
                                                .Join(solarPMSEntities.TimesheetBlockDetails, t => t.TimeSheetId, tbd => tbd.TimesheetId, (t, tbd) => new { t, tbd })
                                                .Where(r => r.t.SAPActivity == Activity
                                                       && r.t.SAPNetwork == Network
                                                       && r.t.WBSAreaId == AreaId
                                                       && r.t.SAPProject == Project
                                                       && r.t.SAPSite == Site
                                                       && r.t.TimeSheetId != TimesheetId
                                                       && r.t.WorkflowStatusId != rejectedByQM // qa rejected
                                                       && r.t.WorkflowStatusId != rejectedByPM // pm rejected
                                                       && r.t.WorkflowStatusId != approvedByQM // partial approved
                                                       && r.t.WorkflowStatusId != approvedByPM
                                                       && r.tbd.BlockNo == BlockNo).Select(r => r.tbd).ToList();

                    var allRecords = solarPMSEntities.Timesheets
                                                .Join(solarPMSEntities.ManagerTimesheetBlockDetails, t => t.TimeSheetId, tbd => tbd.TimesheetId, (t, tbd) => new { t, tbd })
                                                .Where(r => r.t.SAPActivity == Activity
                                                       && r.t.SAPNetwork == Network
                                                       && r.t.WBSAreaId == AreaId
                                                       && r.t.SAPProject == Project
                                                       && r.t.SAPSite == Site
                                                       && r.t.TimeSheetId != TimesheetId
                                                       && (r.t.WorkflowStatusId == approvedByQM // partial approved
                                                        || r.t.WorkflowStatusId == approvedByPM)
                                                        && r.tbd.Status == 1
                                                       && r.tbd.BlockNo == BlockNo).Select(r => new { r.tbd, r.t }).ToList();
                    // qm and pm approved quantity
                    var QAApprovedQuantity = allRecords.Where(t => t.t.WorkflowStatusId == 8 && t.tbd.ManagerType == 1).Select(t => t.tbd).ToList();
                    var PMApprovedQuantity = allRecords.Where(t => t.t.WorkflowStatusId == 9 && t.tbd.ManagerType == 2).Select(t => t.tbd).ToList();

                    GetMaangerApprovedArea(ref qmApprovedQuanity, ref pmApprovedQuanity, QAApprovedQuantity, PMApprovedQuantity);
                }
                else
                {
                    result = solarPMSEntities.Timesheets
                                               .Join(solarPMSEntities.TimesheetBlockDetails, t => t.TimeSheetId, tbd => tbd.TimesheetId, (t, tbd) => new { t, tbd })
                                               .Where(r => r.t.SAPActivity == Activity
                                                      && r.t.SAPSubActivity == SubActivity
                                                      && r.t.SAPNetwork == Network
                                                      && r.t.WBSAreaId == AreaId
                                                      && r.t.SAPProject == Project
                                                      && r.t.SAPSite == Site
                                                      && r.t.TimeSheetId != TimesheetId
                                                       && r.t.WorkflowStatusId != rejectedByQM // qa rejected
                                                       && r.t.WorkflowStatusId != rejectedByPM // pm rejected
                                                      && r.t.WorkflowStatusId != approvedByQM // partial approved
                                                      && r.t.WorkflowStatusId != approvedByPM
                                                      && r.tbd.BlockNo == BlockNo).Select(r => r.tbd).ToList();

                    var allRecords = solarPMSEntities.Timesheets
                                              .Join(solarPMSEntities.ManagerTimesheetBlockDetails, t => t.TimeSheetId, tbd => tbd.TimesheetId, (t, tbd) => new { t, tbd })
                                              .Where(r => r.t.SAPActivity == Activity
                                                     && r.t.SAPSubActivity == SubActivity
                                                     && r.t.SAPNetwork == Network
                                                     && r.t.WBSAreaId == AreaId
                                                     && r.t.SAPProject == Project
                                                     && r.t.SAPSite == Site
                                                     && r.t.TimeSheetId != TimesheetId
                                                      && (r.t.WorkflowStatusId == approvedByQM // partial approved
                                                      || r.t.WorkflowStatusId == approvedByPM)
                                                      && r.tbd.Status == 1
                                                     && r.tbd.BlockNo == BlockNo).Select(r => new { r.tbd, r.t }).ToList();

                    var QAApprovedQuantity = allRecords.Where(t => t.t.WorkflowStatusId == 8 && t.tbd.ManagerType == 1).Select(t => t.tbd).ToList();
                    var PMApprovedQuantity = allRecords.Where(t => t.t.WorkflowStatusId == 9 && t.tbd.ManagerType == 2).Select(t => t.tbd).ToList();

                    GetMaangerApprovedArea(ref qmApprovedQuanity, ref pmApprovedQuanity, QAApprovedQuantity, PMApprovedQuantity);
                }

                if (result != null)
                {
                    completedQuanity = result.Sum(r => r.ActualQuantity).Value;
                }

                if (managerApprovedQuantity != null && managerApprovedQuantity.Count > 0)
                    completedQuanity = completedQuanity + managerApprovedQuantity.Sum(r => r.ActualQuantity).Value;

                return completedQuanity + qmApprovedQuanity + pmApprovedQuanity;
            }
        }

        private static void GetMaangerApprovedArea(ref int qmApprovedQuanity, ref int pmApprovedQuanity, List<ManagerTimesheetBlockDetail> QAApprovedQuantity, List<ManagerTimesheetBlockDetail> PMApprovedQuantity)
        {
            if (QAApprovedQuantity != null && QAApprovedQuantity.Count > 0)
                qmApprovedQuanity = QAApprovedQuantity.Sum(r => r.ActualQuantity).Value;

            if (PMApprovedQuantity != null && PMApprovedQuantity.Count > 0)
                pmApprovedQuanity = PMApprovedQuantity.Sum(r => r.ActualQuantity).Value;
        }

        public static List<int> CheckExistingNumbers(string SubActivity, string Activity, string Network, int AreaId, string Project, string Site,int TimesheetId, int[] Numbers, string Flag)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                List<int?> existingNumbers = solarPMSEntities.usp_GetSavedNumbersForValidation(TimesheetId, SubActivity, Activity, Network, AreaId, Project, Site, Flag).ToList();
                List<int> returnResult = new List<int>();
                foreach (int number in Numbers)
                {
                    if (existingNumbers.Contains(number))
                        returnResult.Add(number);
                }

                return returnResult;
            }
        }

        public static List<ManagerTimesheetActivity> GetManagerUpdatedTimesheetActivity(int TimesheetId, Int16 status)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                return solarPMSEntities.ManagerTimesheetActivities.Where(t => t.TimesheetId == TimesheetId && t.Status == status).ToList();
            }
        }
    }
}