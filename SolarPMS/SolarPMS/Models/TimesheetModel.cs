using SAP.Connector;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Configuration;
using System.Web;

namespace SolarPMS.Models
{
    public class TimesheetModel
    {
        Dictionary<string, string> mobileFunctionlist = new Dictionary<string, string>();
        public TimesheetModel()
        {
            mobileFunctionlist.Add("A", "AutoApproved");
            mobileFunctionlist.Add("B", "AutoApproved");
            mobileFunctionlist.Add("C", "QAApprovalPending");
            mobileFunctionlist.Add("D", "PMApprovalPending");
            mobileFunctionlist.Add("E", "PMApprovalPending");
            mobileFunctionlist.Add("F", "PMApprovalPending");
            mobileFunctionlist.Add("G", "QAApprovalPending");
        }

        public int GetWorkFlowStatus(Timesheet timesheet, SolarPMSEntities solarPMSEntities)
        {
            int workFlowStatusId = 0;

            if (timesheet.ActivityId != null && timesheet.ActivityId > 0)
            {
                int activityId = Convert.ToInt32(timesheet.ActivityId);

                var mobileFunction = (from activity in solarPMSEntities.Activities
                                      join sap in solarPMSEntities.SAPMasters
                                      on activity.SAPActivity equals sap.SAPActivity
                                      join wA in solarPMSEntities.WBSAreas
                                      on activity.WBSAreaId equals wA.WBSAreaId
                                      where activity.ActivityId == activityId
                                      && activity.SAPNetwork == sap.SAPNetwork
                                      && activity.SAPSite == sap.SAPSite
                                      && activity.SAPProject == sap.SAPProjectId
                                      && sap.WBSArea == wA.WBSArea1
                                      select new { sap.MobileFunction }).FirstOrDefault().MobileFunction;

                string approvalType = string.Empty;
                if (mobileFunctionlist.ContainsKey(Convert.ToString(mobileFunction)))
                {
                    approvalType = mobileFunctionlist[mobileFunction.ToString()];
                    if (approvalType != null)
                        approvalType = approvalType.Replace(" ", "");
                    var workFlow = (from workflow in solarPMSEntities.WorkflowStatus
                                    where workflow.WorkflowStatus.Replace(" ", "") == approvalType
                                    select new { workflow.WorkflowStatusId }).Single();
                    workFlowStatusId = Convert.ToInt32(workFlow.WorkflowStatusId);
                }
            }
            else if (timesheet.SubActivityId != null && timesheet.SubActivityId > 0)
            {
                int subActivityId = Convert.ToInt32(timesheet.SubActivityId);

                var mobileFunction = (from subActivity in solarPMSEntities.SubActivities
                                      join activity in solarPMSEntities.Activities
                                      on subActivity.ActivityId equals activity.ActivityId
                                      join sAPMaster in solarPMSEntities.SAPMasters
                                      on subActivity.SAPSubActivity equals sAPMaster.SAPSubActivity
                                      join wA in solarPMSEntities.WBSAreas
                                      on activity.WBSAreaId equals wA.WBSAreaId
                                      where subActivity.SubActivityId == subActivityId
                                      && activity.SAPSite == sAPMaster.SAPSite
                                      && activity.SAPProject == sAPMaster.SAPProjectId
                                      && activity.SAPNetwork == sAPMaster.SAPNetwork
                                      && activity.SAPActivity == sAPMaster.SAPActivity
                                      && sAPMaster.WBSArea == wA.WBSArea1
                                      //&& activity.SAPActivity == sAPMaster.ActivityElementof
                                      select new { sAPMaster.MobileFunction }).FirstOrDefault().MobileFunction;

                //var mobileFunction = (from subActivity in solarPMSEntities.SubActivities
                //                      join sap in solarPMSEntities.SAPMasters
                //                      on subActivity.SAPSubActivity equals sap.SAPActivity
                //                      join activity in solarPMSEntities.Activities
                //                      on subActivity.ActivityId equals activity.ActivityId
                //                      join wA in solarPMSEntities.WBSAreas
                //                      on activity.WBSAreaId equals wA.WBSAreaId
                //                      where subActivity.ActivityId == subActivityId
                //                      && activity.SAPNetwork == sap.SAPNetwork
                //                      && activity.SAPSite == sap.SAPSite
                //                      && activity.SAPProject == sap.SAPProjectId
                //                      && sap.WBSArea == wA.WBSArea1
                //                      select new { sap.MobileFunction }).Single();

                string approvalType = string.Empty;
                if (mobileFunctionlist.ContainsKey(mobileFunction))
                {
                    approvalType = mobileFunctionlist[mobileFunction.ToString()];
                    var workFlow = (from workflow in solarPMSEntities.WorkflowStatus
                                    where workflow.WorkflowStatus == approvalType
                                    select new { workflow.WorkflowStatusId }).Single();
                    workFlowStatusId = Convert.ToInt32(workFlow.WorkflowStatusId);

                }
            }
            return workFlowStatusId;
        }

        public int AddTimesheet(Timesheet timesheet, int userId)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                timesheet.CreatedBy = userId;
                timesheet.CreatedOn = DateTime.Now;
                timesheet.ModifiedBy = userId;
                timesheet.ModifiedOn = DateTime.Now;

                if (timesheet.StatusId == 2)
                {
                    timesheet.WorkflowStatusId = null;
                }
                else
                {
                    timesheet.WorkflowStatusId = GetWorkFlowStatus(timesheet, solarPMSEntities);
                    if (timesheet.WorkflowStatusId < 0 || timesheet.WorkflowStatusId == null)
                        return 0;
                }

                List<TimesheetAttachment> attachments = timesheet.TimesheetAttachments.ToList();
                List<TimesheetSurveyDetail> surveyDetail = timesheet.TimesheetSurveyDetails.ToList();
                List<TimesheetBlockDetail> blockDetail = timesheet.TimesheetBlockDetails.ToList();
                List<TimesheetActivity> timesheetActivityDetail = timesheet.TimesheetActivities.ToList();

                timesheet.TimesheetAttachments = null;
                timesheet.TimesheetSurveyDetails = null;
                timesheet.TimesheetBlockDetails = null;
                timesheet.TimesheetActivities = null;
                SetSAPCallSuccess(timesheet);
                solarPMSEntities.Timesheets.Add(timesheet);

                solarPMSEntities.SaveChanges();
                solarPMSEntities.Configuration.ProxyCreationEnabled = false;
                solarPMSEntities.Configuration.LazyLoadingEnabled = false;

                timesheet.TimesheetAttachments = AddAttachment(attachments, timesheet.TimeSheetId, userId);
                timesheet.TimesheetSurveyDetails = AddSurveyDetail(surveyDetail, timesheet.TimeSheetId, userId);
                timesheet.TimesheetBlockDetails = AddBlock(blockDetail, timesheet.TimeSheetId, userId);
                timesheet.TimesheetActivities = AddTimesheetActivity(timesheetActivityDetail, timesheet.TimeSheetId, userId);

                if (timesheet.StatusId == 1)// Submitted
                {
                    AddTimesheetApprovalEntry(userId, timesheet);
                }

                return timesheet.TimeSheetId;
            }
        }

        public int AddOfflineTimesheet(Timesheet timesheet, int userId, bool Isvalid, string Reason)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                OfflineTimesheet offlineTimesheet = new OfflineTimesheet()
                {
                    CreatedBy = userId,
                    CreatedOn = DateTime.Now,
                    ActivityId = timesheet.ActivityId,
                    ActualDate = timesheet.ActualDate.ToLocalTime(),
                    ActualQuantity = timesheet.ActualQuantity,
                    Comments = timesheet.Comments,
                    SAPActivity = timesheet.SAPActivity,
                    SAPNetwork = timesheet.SAPNetwork,
                    SAPProject = timesheet.SAPProject,
                    SAPSite = timesheet.SAPSite,
                    SAPSubActivity = timesheet.SAPSubActivity,
                    StatusId = timesheet.StatusId,
                    SubActivityId = timesheet.SubActivityId,
                    WBSAreaId = timesheet.WBSAreaId,
                    IsValid = Isvalid,
                    Reason = Reason
                };



                solarPMSEntities.OfflineTimesheets.Add(offlineTimesheet);
                solarPMSEntities.SaveChanges();
                solarPMSEntities.Configuration.ProxyCreationEnabled = false;
                solarPMSEntities.Configuration.LazyLoadingEnabled = false;
                return timesheet.TimeSheetId;
            }
        }
        public int UpdateTimesheet(Timesheet timesheet, int userId, bool IsFromMobile = true)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                var timeSheetDetail = solarPMSEntities.Timesheets.AsNoTracking().FirstOrDefault(t => t.TimeSheetId == timesheet.TimeSheetId);
                if (timeSheetDetail != null)
                {
                    List<TimesheetAttachment> timesheetAttachment = timesheet.TimesheetAttachments.ToList();
                    List<TimesheetSurveyDetail> timesheetSurveyDetail = timesheet.TimesheetSurveyDetails.ToList();
                    List<TimesheetBlockDetail> timesheetBlockDetail = timesheet.TimesheetBlockDetails.ToList();
                    List<TimesheetActivity> timesheetActivityDetail = timesheet.TimesheetActivities.ToList();

                    timesheet.TimesheetAttachments = null;
                    timesheet.TimesheetSurveyDetails = null;
                    timesheet.TimesheetBlockDetails = null;
                    timesheet.TimesheetActivities = null;

                    //timesheet.Status = timesheet.Status;
                    timesheet.CreatedBy = timeSheetDetail.CreatedBy;
                    timesheet.CreatedOn = timeSheetDetail.CreatedOn;
                    timesheet.ModifiedBy = userId;
                    timesheet.ModifiedOn = DateTime.Now;
                    timesheet.ActivityId = timeSheetDetail.ActivityId;
                    timesheet.SubActivityId = timeSheetDetail.SubActivityId;

                    if (timesheet.StatusId == 2)
                    {
                        timesheet.WorkflowStatusId = null;
                    }
                    else
                    {
                        timesheet.WorkflowStatusId = GetWorkFlowStatus(timesheet, solarPMSEntities);
                        if (timesheet.WorkflowStatusId < 0 || timesheet.WorkflowStatusId == null)
                            return 0;
                    }

                    SetSAPCallSuccess(timesheet);

                    solarPMSEntities.Entry(timesheet).State = EntityState.Modified;
                    solarPMSEntities.SaveChanges();
                    solarPMSEntities.Configuration.ProxyCreationEnabled = false;
                    solarPMSEntities.Configuration.LazyLoadingEnabled = false;

                    if (IsFromMobile)
                    {
                        DeleteSurveyDetail(timesheet.TimeSheetId);
                        DeleteTimesheetBlockDetails(timesheet.TimeSheetId);

                        timesheet.TimesheetAttachments = AddAttachment(timesheetAttachment, timesheet.TimeSheetId, userId);
                        timesheet.TimesheetSurveyDetails = AddSurveyDetail(timesheetSurveyDetail, timesheet.TimeSheetId, userId);
                        timesheet.TimesheetBlockDetails = AddBlock(timesheetBlockDetail, timesheet.TimeSheetId, userId);
                    }

                    DeleteTimesheetActivity(timesheet.TimeSheetId);
                    timesheet.TimesheetActivities = AddTimesheetActivity(timesheetActivityDetail, timesheet.TimeSheetId, userId);

                    //if (timesheet.StatusId == 1)// Submitted
                    //    AddTimesheetApprovalEntry(userId, timesheet);

                    if (timesheet.StatusId == 1)// Submitted
                    {
                        AddTimesheetApprovalEntry(userId, timesheet);
                    }
                }
            }
            return timesheet.TimeSheetId;
        }

        public bool ApproveRejectTimesheet(List<Timesheet> lstTimesheet, int userId)
        {
            bool result = false;
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                foreach (Timesheet timesheet in lstTimesheet)
                {
                    Timesheet updateTimesheet = solarPMSEntities.Timesheets.Where(t => t.TimeSheetId == timesheet.TimeSheetId).FirstOrDefault();
                    if (updateTimesheet != null)
                    {
                        result = false;
                        //This adjustment for C and G type mobile fuction when QA approves then add PMApprovalPending(4)
                        if (timesheet.WorkflowStatusId == 3)
                            updateTimesheet.WorkflowStatusId = 4;
                        else
                            updateTimesheet.WorkflowStatusId = timesheet.WorkflowStatusId;
                        //updateTimesheet.Comments = timesheet.Comments;
                        updateTimesheet.ModifiedBy = userId;
                        updateTimesheet.ModifiedOn = DateTime.Now;
                        SetSAPCallSuccess(updateTimesheet);
                        solarPMSEntities.Entry(updateTimesheet).State = EntityState.Modified;
                        solarPMSEntities.SaveChanges();
                        AddTimesheetApprovalEntry(userId, timesheet);
                        result = true;
                    }
                }
            }

            return result;
        }

        private static void AddTimesheetApprovalEntry(int userId, Timesheet timesheet)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                TimesheetApproval updateTimesheetStatus = new TimesheetApproval();
                updateTimesheetStatus.TimesheetId = timesheet.TimeSheetId;
                updateTimesheetStatus.Comment = timesheet.Comments;
                updateTimesheetStatus.WorkflowStatusId = Convert.ToInt32(timesheet.WorkflowStatusId);
                updateTimesheetStatus.CreatedBy = userId;
                updateTimesheetStatus.CreatedOn = DateTime.Now;
                solarPMSEntities.TimesheetApprovals.Add(updateTimesheetStatus);
                solarPMSEntities.SaveChanges();
            }
        }


        /// <summary>
        /// This method is used to add timesheet attchments.
        /// </summary>
        /// <param name="timesheetAttachment"></param>
        /// <param name="timesheetId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<TimesheetAttachment> AddAttachment(List<TimesheetAttachment> timesheetAttachment, int timesheetId, int userId)
        {
            if (timesheetAttachment != null && timesheetAttachment.Count > 0)
            {
                using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
                {
                    timesheetAttachment.ForEach(attachment =>
                    {
                        attachment.TimesheetId = timesheetId;
                        attachment.CreatedBy = userId;
                        attachment.CreatedOn = DateTime.Now;
                        attachment.ModifiedBy = userId;
                        attachment.ModifiedOn = DateTime.Now;
                        solarPMSEntities.TimesheetAttachments.Add(attachment);
                    });
                    solarPMSEntities.SaveChanges();
                }
            }
            return timesheetAttachment;
        }

        /// <summary>
        /// This method is used to add SurveyDetails
        /// </summary>
        /// <param name="timesheetSurveyDetail"></param>
        /// <param name="timesheetId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<TimesheetSurveyDetail> AddSurveyDetail(List<TimesheetSurveyDetail> timesheetSurveyDetail, int timesheetId, int userId)
        {
            if (timesheetSurveyDetail != null && timesheetSurveyDetail.Count > 0)
            {
                using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
                {
                    timesheetSurveyDetail.ForEach(surveyDetail =>
                    {
                        surveyDetail.TimesheetId = timesheetId;
                        surveyDetail.CreatedBy = userId;
                        surveyDetail.CreatedOn = DateTime.Now;
                        surveyDetail.ModifiedBy = userId;
                        surveyDetail.ModifiedOn = DateTime.Now;
                        solarPMSEntities.TimesheetSurveyDetails.Add(surveyDetail);
                    });
                    solarPMSEntities.SaveChanges();
                }
            }
            return timesheetSurveyDetail;
        }

        /// <summary>
        /// This is used to add Block Details
        /// </summary>
        /// <param name="timesheetBlockDetail"></param>
        /// <param name="timesheetId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<TimesheetBlockDetail> AddBlock(List<TimesheetBlockDetail> timesheetBlockDetail, int timesheetId, int userId)
        {
            if (timesheetBlockDetail != null && timesheetBlockDetail.Count > 0)
            {
                using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
                {
                    timesheetBlockDetail.ForEach(blockDetail =>
                    {
                        blockDetail.TimesheetId = timesheetId;
                        blockDetail.CreatedBy = userId;
                        blockDetail.CreatedOn = DateTime.Now;
                        blockDetail.ModifiedBy = userId;
                        blockDetail.ModifiedOn = DateTime.Now;
                        solarPMSEntities.TimesheetBlockDetails.Add(blockDetail);
                    });
                    solarPMSEntities.SaveChanges();
                }
            }
            return timesheetBlockDetail;
        }

        public List<TimesheetActivity> AddTimesheetActivity(List<TimesheetActivity> timesheetActivityDetail, int timesheetId, int userId)
        {
            if (timesheetActivityDetail != null && timesheetActivityDetail.Count > 0)
            {
                using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
                {
                    timesheetActivityDetail.ForEach(activityDetail =>
                    {
                        activityDetail.TimesheetId = timesheetId;
                        activityDetail.CreatedBy = userId;
                        activityDetail.CreatedOn = DateTime.Now;
                        activityDetail.ModifiedBy = userId;
                        activityDetail.ModifiedOn = DateTime.Now;
                        solarPMSEntities.TimesheetActivities.Add(activityDetail);
                    });
                    solarPMSEntities.SaveChanges();
                }
            }
            return timesheetActivityDetail;
        }

        public static bool UpdateTimesheetActualQuantity(int TimesheetId, decimal Quantity, int UserId)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                var timesheet = solarPMSEntities.Timesheets.FirstOrDefault(t => t.TimeSheetId == TimesheetId);
                if (timesheet != null)
                {
                    timesheet.ActualQuantity = Quantity;
                    timesheet.ModifiedBy = UserId;
                    timesheet.ModifiedOn = DateTime.Now;
                    solarPMSEntities.SaveChanges();
                    return true;
                }
            }
            return false;
        }
        public void DeleteSurveyDetail(int timesheetId)
        {
            if (timesheetId > 0)
            {
                using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
                {
                    var child = solarPMSEntities.TimesheetSurveyDetails.Where(t => t.TimesheetId == timesheetId);
                    if (child != null)
                    {
                        solarPMSEntities.TimesheetSurveyDetails.RemoveRange(child);
                        solarPMSEntities.SaveChanges();
                    }
                }
            }
        }

        public static bool DeleteTimesheetSurvey(int SurveyId)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                var timesheetSurveyDetails = solarPMSEntities.TimesheetSurveyDetails.Where(t => t.TimesheetSurveyDetailId == SurveyId);
                if (timesheetSurveyDetails != null)
                {
                    solarPMSEntities.TimesheetSurveyDetails.RemoveRange(timesheetSurveyDetails);
                    solarPMSEntities.SaveChanges();
                    return true;
                }
            }
            return false;
        }
        public void DeleteTimesheetBlockDetails(int timesheetId)
        {
            if (timesheetId > 0)
            {
                using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
                {
                    var child = solarPMSEntities.TimesheetBlockDetails.Where(t => t.TimesheetId == timesheetId);
                    if (child != null)
                    {
                        solarPMSEntities.TimesheetBlockDetails.RemoveRange(child);
                        solarPMSEntities.SaveChanges();
                    }
                }
            }
        }

        public static bool DeleteTimesheetBlock(int BlockId)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                var timesheetBlockDetails = solarPMSEntities.TimesheetBlockDetails.Where(t => t.TimesheetBlockDetailId == BlockId);
                if (timesheetBlockDetails != null)
                {
                    solarPMSEntities.TimesheetBlockDetails.RemoveRange(timesheetBlockDetails);
                    solarPMSEntities.SaveChanges();
                    return true;
                }
            }
            return false;
        }
        public void DeleteTimesheetActivity(int timesheetId)
        {
            if (timesheetId > 0)
            {
                using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
                {
                    var child = solarPMSEntities.TimesheetActivities.Where(t => t.TimesheetId == timesheetId);
                    if (child != null)
                    {
                        solarPMSEntities.TimesheetActivities.RemoveRange(child);
                        solarPMSEntities.SaveChanges();
                    }
                }
            }
        }

        public static bool DeleteTimesheetTableActivity(int TimesheetActivityId)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                var timesheetActivities = solarPMSEntities.TimesheetActivities.Where(t => t.TimesheetActivityId == TimesheetActivityId);
                if (timesheetActivities != null)
                {
                    solarPMSEntities.TimesheetActivities.RemoveRange(timesheetActivities);
                    solarPMSEntities.SaveChanges();
                    return true;
                }
            }
            return false;
        }

        public static bool DeleteAttachment(int AttachmentId)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                TimesheetAttachment timesheetAttachment = solarPMSEntities.TimesheetAttachments.Where(t => t.TimesheetAttachmentId == AttachmentId).FirstOrDefault();
                if (timesheetAttachment != null)
                {
                    solarPMSEntities.TimesheetAttachments.Remove(timesheetAttachment);
                    solarPMSEntities.SaveChanges();
                    return true;
                }
            }
            return false;
        }

        public static bool UpdateTimesheetBlock(TimesheetBlockDetail timesheetBlockDetail)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                TimesheetBlockDetail timesheetBlock = solarPMSEntities.TimesheetBlockDetails.FirstOrDefault(t => t.TimesheetBlockDetailId == timesheetBlockDetail.TimesheetBlockDetailId);
                if (timesheetBlock != null)
                {
                    timesheetBlock.ActualQuantity = timesheetBlockDetail.ActualQuantity;
                    timesheetBlock.BlockNo = timesheetBlockDetail.BlockNo;
                    timesheetBlock.TimesheetId = timesheetBlockDetail.TimesheetId;
                    timesheetBlock.ModifiedBy = timesheetBlockDetail.ModifiedBy;
                    timesheetBlock.ModifiedOn = DateTime.Now;
                    solarPMSEntities.SaveChanges();
                    return true;
                }
            }
            return false;
        }

        public static bool UpdateTimesheetSurvey(TimesheetSurveyDetail timesheetSurveyDetail)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                TimesheetSurveyDetail timesheetSurvey = solarPMSEntities.TimesheetSurveyDetails
                                                         .FirstOrDefault(t => t.TimesheetSurveyDetailId == timesheetSurveyDetail.TimesheetSurveyDetailId);
                if (timesheetSurvey != null)
                {
                    timesheetSurvey.SurveyNo = timesheetSurveyDetail.SurveyNo;
                    timesheetSurvey.VillageId = timesheetSurveyDetail.VillageId;
                    timesheetSurvey.ActualNoOfDivision = timesheetSurveyDetail.ActualNoOfDivision;
                    timesheetSurvey.TimesheetId = timesheetSurveyDetail.TimesheetId;
                    timesheetSurvey.ModifiedBy = timesheetSurveyDetail.ModifiedBy;
                    timesheetSurvey.ModifiedOn = DateTime.Now;
                    solarPMSEntities.SaveChanges();
                    return true;
                }
            }
            return false;
        }

        public static bool UpdateTimesheetACtivity(TimesheetActivity timesheetActivity)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                TimesheetActivity activity = solarPMSEntities.TimesheetActivities
                                                         .FirstOrDefault(t => t.TimesheetActivityId == timesheetActivity.TimesheetActivityId);
                if (activity != null)
                {
                    activity.FromRange = activity.FromRange;
                    activity.ToRange = activity.ToRange;
                    activity.ProposedQuantity = activity.ProposedQuantity;
                    activity.Flag = activity.Flag;
                    activity.RangeType = activity.RangeType;
                    activity.TimesheetId = activity.TimesheetId;
                    activity.ModifiedBy = activity.ModifiedBy;
                    activity.ModifiedOn = DateTime.Now;
                    solarPMSEntities.SaveChanges();
                    return true;
                }
            }
            return false;
        }

        public static bool CloseTimesheet(int TimesheetId)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {

                Timesheet timesheet = solarPMSEntities.Timesheets.FirstOrDefault(t => t.TimeSheetId == TimesheetId);
                if (timesheet != null)
                {
                    timesheet.StatusId = 3;
                    solarPMSEntities.SaveChanges();
                    return true;
                }
            }

            return false;
        }
        public dynamic GetTimesheetByTimesheetId(Timesheet timesheets)
        {

            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                solarPMSEntities.Configuration.ProxyCreationEnabled = false;
                var timesheetDetail = (from timesheet in solarPMSEntities.Timesheets
                                       where timesheet.TimeSheetId == timesheets.TimeSheetId
                                       select new
                                       {
                                           timesheet.TimeSheetId,
                                           timesheet.ActivityId,
                                           timesheet.SubActivityId,
                                           timesheet.SAPSite,
                                           timesheet.SAPProject,
                                           timesheet.WBSAreaId,
                                           timesheet.SAPNetwork,
                                           timesheet.SAPActivity,
                                           timesheet.SAPSubActivity,
                                           timesheet.ActualDate,
                                           timesheet.ActualQuantity,
                                           timesheet.Comments,
                                           timesheet.WorkflowStatusId,
                                           timesheet.StatusId,
                                           timesheet.CreatedBy,
                                           timesheet.CreatedOn,
                                           timesheet.ModifiedBy,
                                           timesheet.ModifiedOn,
                                           TimesheetSurveyDetails = (from surveyDetail in solarPMSEntities.TimesheetSurveyDetails
                                                                     join village in solarPMSEntities.VillageMasters
                                                                     on surveyDetail.VillageId equals village.VillageId
                                                                     where surveyDetail.TimesheetId == timesheets.TimeSheetId
                                                                     select new
                                                                     {
                                                                         surveyDetail.TimesheetSurveyDetailId,
                                                                         surveyDetail.TimesheetId,
                                                                         surveyDetail.VillageId,
                                                                         surveyDetail.SurveyNo,
                                                                         surveyDetail.ActualNoOfDivision,
                                                                         surveyDetail.ActualArea,
                                                                         village.VillageName,
                                                                         surveyDetail.CreatedBy,
                                                                         surveyDetail.CreatedOn,
                                                                         surveyDetail.ModifiedBy,
                                                                         surveyDetail.ModifiedOn,
                                                                     }).ToList(),

                                           TimesheetBlockDetails = solarPMSEntities.TimesheetBlockDetails.Where(bd => bd.TimesheetId == timesheets.TimeSheetId).ToList<TimesheetBlockDetail>(),
                                           TimesheetAttachments = solarPMSEntities.TimesheetAttachments.Where(ta => ta.TimesheetId == timesheets.TimeSheetId).ToList<TimesheetAttachment>(),
                                           TimesheetActivities = solarPMSEntities.TimesheetActivities.Where(ta => ta.TimesheetId == timesheets.TimeSheetId).ToList<TimesheetActivity>(),
                                           
                                       }).FirstOrDefault();
                return timesheetDetail;
            }

        }

        public static TimesheetActivityDetails GetTimesheetActivityDetails(int TimesheetId)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                return solarPMSEntities.usp_GetTimesheetActivityDetails(TimesheetId).FirstOrDefault();
            }
        }
        public List<dynamic> GetTimesheetsByActivityId(Timesheet timesheet)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                solarPMSEntities.Configuration.ProxyCreationEnabled = false;
                //return solarPMSEntities.Timesheets.Where(t => t.ActivityId == timesheet.ActivityId).ToList<Timesheet>();

                var timesheetDetails = (from Ts in solarPMSEntities.Timesheets
                                        join wBSArea in solarPMSEntities.WBSAreas
                                        on Ts.WBSAreaId equals wBSArea.WBSAreaId
                                        join activity in solarPMSEntities.Activities
                                        on Ts.ActivityId equals activity.ActivityId
                                        join sapMaster in solarPMSEntities.SAPMasters
                                        on activity.SAPActivity equals sapMaster.SAPActivity
                                        where activity.SAPNetwork == sapMaster.SAPNetwork
                                        && activity.SAPProject == sapMaster.SAPProjectId
                                        && activity.SAPSite == sapMaster.SAPSite
                                        && Ts.ActivityId == timesheet.ActivityId
                                        && (Ts.SubActivityId == null || Ts.SubActivityId ==0)
                                        && wBSArea.WBSArea1 == sapMaster.WBSArea
                                        orderby Ts.ModifiedOn descending
                                        select new
                                        {
                                            Ts.TimeSheetId,
                                            Ts.ActivityId,
                                            Ts.ActualQuantity,
                                            Ts.ActualDate,
                                            Ts.CreatedBy,
                                            Ts.CreatedOn,
                                            Ts.ModifiedBy,
                                            Ts.ModifiedOn,
                                            Ts.StatusId,
                                            sapMaster.SAPActivity,
                                            sapMaster.SAPProjectId,
                                            sapMaster.SAPSite,
                                            sapMaster.SAPNetwork,
                                            sapMaster.ActivityDescription,
                                            sapMaster.SAPSubActivity,
                                            sapMaster.SAPSubActivityDescription,
                                        }).ToList();

                return timesheetDetails.ToList<dynamic>();
            }
        }

        public decimal? GetTotalActualQuantityByActivityId(Timesheet timesheet)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                solarPMSEntities.Configuration.ProxyCreationEnabled = false;
                decimal? totalActualQuantity = solarPMSEntities.Timesheets.Where(t => t.ActivityId == timesheet.ActivityId && t.TimeSheetId != timesheet.TimeSheetId).Sum(t => t.ActualQuantity);
                return totalActualQuantity;
            }
        }

        public List<dynamic> GetTimesheetsBySubActivityId(Timesheet timesheet)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                solarPMSEntities.Configuration.ProxyCreationEnabled = false;
                //return solarPMSEntities.Timesheets.Where(t => t.SubActivityId == timesheet.SubActivityId).ToList<Timesheet>();

                var timesheetDetails = (from timesheets in solarPMSEntities.Timesheets
                                        join sapMaster in solarPMSEntities.SAPMasters
                                        on timesheet.SAPSubActivity equals timesheet.SAPSubActivity
                                        join wBSArea in solarPMSEntities.WBSAreas
                                        on timesheet.WBSAreaId equals wBSArea.WBSAreaId
                                        where timesheets.SAPSite == sapMaster.SAPSite
                                        && timesheets.SAPProject == sapMaster.SAPProjectId
                                        && timesheets.SAPNetwork == sapMaster.SAPNetwork
                                        && timesheets.SAPActivity == sapMaster.SAPActivity
                                        && timesheets.SAPSubActivity == sapMaster.SAPSubActivity
                                        && wBSArea.WBSArea1 == sapMaster.WBSArea
                                        && timesheets.SubActivityId == timesheet.SubActivityId
                                        orderby timesheets.ModifiedOn descending
                                        select new
                                        {
                                            timesheets.TimeSheetId,
                                            timesheets.ActualQuantity,
                                            timesheets.SubActivityId,
                                            timesheets.ActualDate,
                                            timesheets.CreatedBy,
                                            timesheets.CreatedOn,
                                            timesheets.ModifiedBy,
                                            timesheets.ModifiedOn,
                                            timesheets.StatusId,
                                            sapMaster.SAPActivity,
                                            sapMaster.ActivityDescription,
                                            sapMaster.SAPSubActivity,
                                            sapMaster.SAPSubActivityDescription,
                                            sapMaster.SAPProjectId,
                                            sapMaster.SAPSite,
                                            sapMaster.SAPNetwork,
                                        }).ToList();

                return timesheetDetails.ToList<dynamic>();
            }
        }

        public decimal? GetTotalActualQuantityBySubActivityId(Timesheet timesheet)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                solarPMSEntities.Configuration.ProxyCreationEnabled = false;
                decimal? totalActualQuantity = solarPMSEntities.Timesheets.Where(t => t.SubActivityId == timesheet.SubActivityId && t.TimeSheetId != timesheet.TimeSheetId).Sum(t => t.ActualQuantity);
                return totalActualQuantity;
            }
        }

        public List<WorkflowStatu> GetWorkFlow()
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                solarPMSEntities.Configuration.ProxyCreationEnabled = false;
                return solarPMSEntities.WorkflowStatus.ToList<WorkflowStatu>();
            }
        }

        public List<StatusMaster> GetStatus()
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                solarPMSEntities.Configuration.ProxyCreationEnabled = false;
                return solarPMSEntities.StatusMasters.ToList<StatusMaster>();
            }
        }

        public static List<SubmiitedTimesheetAttachment> GetTimesheetAttachment(int ActivityId, int SubActivityId, int UserId)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                solarPMSEntities.Configuration.ProxyCreationEnabled = false;
                return solarPMSEntities.usp_GetSubmiitedTimesheetAttachment(ActivityId, UserId, SubActivityId).ToList();
            }
        }

        public static List<TimesheetAttachment> GetTimesheetAttachmentByTimesheetId(int TimesheetId)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                solarPMSEntities.Configuration.ProxyCreationEnabled = false;
                return solarPMSEntities.TimesheetAttachments
                                .Join(solarPMSEntities.Timesheets, ta => ta.TimesheetId, t => t.TimeSheetId, (ta, t) => new { TimesheetAttachment = ta, Timesheet = t })
                                .Where(r => r.Timesheet.TimeSheetId == TimesheetId)
                                .Select(r => r.TimesheetAttachment).ToList();
            }
        }

        public static dynamic GetTimesheetDatailsWithUser(int ActivityId)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                return solarPMSEntities.Timesheets.Join(solarPMSEntities.Users, t => t.CreatedBy, u => u.UserId, (t, u) => new { t, u })
                        .Join(solarPMSEntities.StatusMasters, t => t.t.StatusId, st => st.StatusId, (t, st) => new { t, st })
                                        .Where(r => r.t.t.ActivityId == ActivityId)
                                        .Select(r => new { r.t.t.ActualDate, r.t.t.ActualQuantity, r.t.u.Name, r.st.Status, r.t.t.TimeSheetId, r.t.t.ActivityId, r.t.t.SubActivityId }).ToList();
            }
        }

        public static List<SubmittedTimesheetDetails> GetSubmittedTimesheetDatails(int ActivityId, int SubActivityId, int UserId)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                return solarPMSEntities.usp_GetSubmittedTimesheetDetails(UserId, ActivityId, SubActivityId).ToList();
            }
        }
        public static List<TimesheetSurveyDetailsView> GetTimesheetSurvayDetails(int TimesheetId, bool IsManager = false, short Status = 0)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                if (IsManager)
                    return solarPMSEntities.ManagerTimesheetSurveyDetails
                                                    .Join(solarPMSEntities.Timesheets, tsd => tsd.TimesheetId, t => t.TimeSheetId, (tsd, t) => new { TimesheetSurveyDetail = tsd, Timesheet = t })
                                                    .Join(solarPMSEntities.SurveyMasters, tsd => tsd.TimesheetSurveyDetail.SurveyNo, sm => sm.SurveyNo, (tsd, sm) => new { tsd = tsd.TimesheetSurveyDetail, sm })
                                                    .Join(solarPMSEntities.VillageMasters, sm => sm.sm.VillageId, v => v.VillageId, (sm, v) => new { sm.tsd, sm.sm, v.VillageName })
                                                    .Where(r => r.tsd.TimesheetId == TimesheetId && r.tsd.Status == Status)
                                                    .Select(r => new TimesheetSurveyDetailsView()
                                                    {
                                                        VillageName = r.VillageName,
                                                        SurveyNo = r.sm.SurveyNo,
                                                        PraposedDivision = r.sm.NoOfDivision,
                                                        PraposedArea = r.sm.Area,
                                                        ActualNoOfDivision = r.tsd.ActualNoOfDivision,
                                                        ActualArea = r.tsd.ActualArea,
                                                        TimesheetSurveyDetailId = r.tsd.TimesheetSurveyDetailId,
                                                        TimesheetId = r.tsd.TimesheetId,
                                                        ManagerType=r.tsd.ManagerType
                                                    }).Distinct().ToList();
                else
                    return solarPMSEntities.TimesheetSurveyDetails
                                                        .Join(solarPMSEntities.Timesheets, tsd => tsd.TimesheetId, t => t.TimeSheetId, (tsd, t) => new { TimesheetSurveyDetail = tsd, Timesheet = t })
                                                        .Join(solarPMSEntities.SurveyMasters, tsd => tsd.TimesheetSurveyDetail.SurveyNo, sm => sm.SurveyNo, (tsd, sm) => new { tsd = tsd.TimesheetSurveyDetail, sm })
                                                        .Join(solarPMSEntities.VillageMasters, sm => sm.sm.VillageId, v => v.VillageId, (sm, v) => new { sm.tsd, sm.sm, v.VillageName })
                                                        .Where(r => r.tsd.TimesheetId == TimesheetId)
                                                        .Select(r => new TimesheetSurveyDetailsView()
                                                        {
                                                            VillageName = r.VillageName,
                                                            SurveyNo = r.sm.SurveyNo,
                                                            PraposedDivision = r.sm.NoOfDivision,
                                                            PraposedArea = r.sm.Area,
                                                            ActualNoOfDivision = r.tsd.ActualNoOfDivision,
                                                            ActualArea = r.tsd.ActualArea,
                                                            TimesheetSurveyDetailId = r.tsd.TimesheetSurveyDetailId,
                                                            TimesheetId = r.tsd.TimesheetId
                                                        }).Distinct().ToList();
            }
        }
        public static List<ListItem> GetTimesheetStatus()
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                return solarPMSEntities.StatusMasters.Where(s => s.ShowInDropDown)
                             .Select(s => new ListItem() { Id = s.StatusId.ToString(), Name = s.Status }).ToList();
            }
        }

        public static List<SurveyMaster> GetAllSurveyNumberForVillage(int VillageId, string SAPSite, string SAPProjectId)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                solarPMSEntities.Configuration.ProxyCreationEnabled = false;
                return solarPMSEntities.SurveyMasters.Where(s => s.VillageId == VillageId && s.Site == SAPSite && s.ProjectId == SAPProjectId).ToList();
            }
        }

        public static SurveyMaster GetSurveyDetailsById(int SurveyId)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                return solarPMSEntities.SurveyMasters.FirstOrDefault(s => s.SurveyId == SurveyId);
            }
        }

        public static dynamic GetTimesheetComments(int TimesheetId)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                return solarPMSEntities.TimesheetApprovals
                            .Join(solarPMSEntities.Users, ta => ta.CreatedBy, u => u.UserId, (ta, u) => new { ta, u })
                            .Where(r => r.ta.TimesheetId == TimesheetId && !string.IsNullOrEmpty(r.ta.Comment))
                            .Select(r => new { r.ta.Comment, r.ta.TimesheetId, r.u.Name }).ToList();
            }
        }

        public static decimal GetSubmittedActualQuantityForActivity(string SubActivityId, string ActivityId, string SAPNetwork, int SAPAreaId, string SAPProjectId, string SAPSite)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {

                var result = solarPMSEntities.Timesheets.Where(s => s.SAPSubActivity == SubActivityId
                                                        && s.SAPActivity == ActivityId
                                                        && s.SAPNetwork == SAPNetwork
                                                        && s.WBSAreaId == SAPAreaId
                                                        && s.SAPSite == SAPSite
                                                        && s.SAPProject == SAPProjectId
                                                        && s.WorkflowStatusId != 2
                                                        && s.WorkflowStatusId != 5
                                                        && s.StatusId != 2);
                                                        
                if (result != null)
                {
                    decimal? actualQuantity = 0;
                    var actualQuantityTimesheet = result.Where(r => r.ApprovedQuantity == null && r.ActualQuantity != null).ToList();
                    if (actualQuantityTimesheet != null && actualQuantityTimesheet.Count > 0)
                        actualQuantity = actualQuantityTimesheet.Sum(r => r.ActualQuantity);

                    var approvedTimesheet = result.Where(r => r.ApprovedQuantity != null).ToList();
                    decimal? approvedQuantity = 0;
                    if (approvedTimesheet != null && approvedTimesheet.Count > 0)
                        approvedQuantity = approvedTimesheet.Sum(r => r.ApprovedQuantity);

                    if (approvedQuantity != null)
                        return actualQuantity.Value + approvedQuantity.Value;
                    else
                        return actualQuantity.Value;
                }
            }
            return 0;
        }

        public static bool CheckTimesheetExistsForDate(Timesheet timesheet, DateTime Date, int UserId)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                return solarPMSEntities.Timesheets
                    .Where(s => s.ActualDate == Date
                    && s.SAPSubActivity == timesheet.SAPSubActivity
                    && s.SAPActivity == timesheet.SAPActivity
                    && s.SAPNetwork == timesheet.SAPNetwork
                    && s.WBSAreaId == timesheet.WBSAreaId
                    && s.SAPProject == timesheet.SAPProject
                    && s.SAPSite == timesheet.SAPSite
                    && s.WorkflowStatusId != 2
                    && s.WorkflowStatusId != 5
                    && s.CreatedBy == UserId).FirstOrDefault() == null;
            }
        }

        public List<dynamic> GetTimesheetApprovalComment(int TimesheetId)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                var timesheetCommentDetails = (from timesheetApprovals in solarPMSEntities.TimesheetApprovals
                                               join workflowStatus in solarPMSEntities.WorkflowStatus
                                               on timesheetApprovals.WorkflowStatusId equals workflowStatus.WorkflowStatusId
                                               join user in solarPMSEntities.Users
                                               on timesheetApprovals.CreatedBy equals user.UserId
                                               where timesheetApprovals.TimesheetId == TimesheetId
                                               orderby timesheetApprovals.CreatedOn descending
                                               select new
                                               {
                                                   timesheetApprovals.TimesheetApprovalId,
                                                   timesheetApprovals.WorkflowStatusId,
                                                   timesheetApprovals.Comment,
                                                   timesheetApprovals.CreatedOn,
                                                   user.Name,
                                                   workflowStatus = workflowStatus.WorkflowStatus
                                               }).ToList();

                return timesheetCommentDetails.ToList<dynamic>();

            }
        }

        public List<dynamic> GetActivityHistory(int ActivityId, int SubActivityId)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                Activity activity = solarPMSEntities.Activities
                                        .FirstOrDefault(a => a.ActivityId == ActivityId);
                SubActivity subActivity = solarPMSEntities.SubActivities.FirstOrDefault(s => s.SubActivityId == SubActivityId);

                if (activity != null && subActivity != null)
                {
                    return solarPMSEntities.ActivityAssignmentHistories
                                            .Join(solarPMSEntities.Users, aa => aa.UserId, u => u.UserId, (aa, u) => new { aa, u })
                                           .Where(a => a.aa.SAPActivity == activity.SAPActivity
                                           && a.aa.SAPNetwork == activity.SAPNetwork
                                           && a.aa.WBSAreaId == activity.WBSAreaId
                                           && a.aa.SAPProject == activity.SAPProject
                                           && a.aa.SAPSite == activity.SAPSite
                                           && a.aa.SAPSubactivity == subActivity.SAPSubActivity
                                           )
                                           .Select(r => new { r.aa.IsAssigned, r.aa.Date, r.u.Name }).OrderByDescending(r => r.Date).ToList<dynamic>();
                }
                else if (activity != null)
                {

                    return solarPMSEntities.ActivityAssignmentHistories
                                            .Join(solarPMSEntities.Users, aa => aa.UserId, u => u.UserId, (aa, u) => new { aa, u })
                                           .Where(a => a.aa.SAPActivity == activity.SAPActivity
                                           && a.aa.SAPNetwork == activity.SAPNetwork
                                           && a.aa.WBSAreaId == activity.WBSAreaId
                                           && a.aa.SAPProject == activity.SAPProject
                                           && a.aa.SAPSite == activity.SAPSite
                                           )
                                           .Select(r => new { r.aa.IsAssigned, r.aa.Date, r.u.Name }).OrderByDescending(r => r.Date).ToList<dynamic>();
                }

                return null;
            }
        }

        /// <summary>
        /// Check last status of timesheet is rejected by QA.
        /// </summary>
        /// <param name="TimesheetId"></param>
        /// <returns></returns>
        public static bool IsRejectedTimesheet(int TimesheetId)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                var result = solarPMSEntities.TimesheetApprovals.Where(t => t.TimesheetId == TimesheetId && t.WorkflowStatusId == 2)
                            .OrderByDescending(t => t.TimesheetApprovalId).Take(3);
                if (result != null)
                    return true;

                return false;
            }
        }

        public int PartialApproveTimesheet(Timesheet timesheet)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                var timeSheetDetail = solarPMSEntities.Timesheets.FirstOrDefault(t => t.TimeSheetId == timesheet.TimeSheetId);
                if (timeSheetDetail != null)
                {
                    if (timesheet.WorkflowStatusId == Convert.ToInt32(Constants.WorkflowStatus.PartialApprovedByQM))
                        timeSheetDetail.QAApprovedQuantity = timesheet.ApprovedQuantity;
                    else if (timesheet.WorkflowStatusId == Convert.ToInt32(Constants.WorkflowStatus.PartialApprovedByPM))
                        timeSheetDetail.ApprovedQuantity = timesheet.ApprovedQuantity;

                    timeSheetDetail.WorkflowStatusId = timesheet.WorkflowStatusId == Convert.ToInt32(Constants.WorkflowStatus.PartialApprovedByQM)
                                                        ? Convert.ToInt32(Constants.WorkflowStatus.PMApprovalPending) : timesheet.WorkflowStatusId;
                    timeSheetDetail.ModifiedBy = timesheet.ModifiedBy;
                    timeSheetDetail.ModifiedOn = DateTime.Now;

                    if (timesheet.ManagerTimesheetActivities != null && timesheet.ManagerTimesheetActivities.Count > 0)
                    {
                        timesheet.ManagerTimesheetActivities.ToList().ForEach(activityDetail =>
                        {
                            activityDetail.TimesheetId = timesheet.TimeSheetId;
                            activityDetail.CreatedBy = timesheet.ModifiedBy;
                            activityDetail.CreatedOn = DateTime.Now;
                            activityDetail.ModifiedBy = timesheet.ModifiedBy;
                            activityDetail.ModifiedOn = DateTime.Now;
                            solarPMSEntities.ManagerTimesheetActivities.Add(activityDetail);
                        });
                    }
                    else if (timesheet.ManagerTimesheetBlockDetails != null && timesheet.ManagerTimesheetBlockDetails.Count > 0)
                    {
                        timesheet.ManagerTimesheetBlockDetails.ToList().ForEach(activityDetail =>
                        {
                            activityDetail.TimesheetId = timesheet.TimeSheetId;
                            activityDetail.CreatedBy = timesheet.ModifiedBy;
                            activityDetail.CreatedOn = DateTime.Now;
                            activityDetail.ModifiedBy = timesheet.ModifiedBy;
                            activityDetail.ModifiedOn = DateTime.Now;
                            solarPMSEntities.ManagerTimesheetBlockDetails.Add(activityDetail);
                        });
                    }
                    else if (timesheet.ManagerTimesheetSurveyDetails != null && timesheet.ManagerTimesheetSurveyDetails.Count > 0)
                    {
                        timesheet.ManagerTimesheetSurveyDetails.ToList().ForEach(activityDetail =>
                        {
                            activityDetail.TimesheetId = timesheet.TimeSheetId;
                            activityDetail.CreatedBy = timesheet.ModifiedBy;
                            activityDetail.CreatedOn = DateTime.Now;
                            activityDetail.ModifiedBy = timesheet.ModifiedBy;
                            activityDetail.ModifiedOn = DateTime.Now;
                            solarPMSEntities.ManagerTimesheetSurveyDetails.Add(activityDetail);
                        });
                    }

                    AddTimesheetApprovalEntry(timesheet.ModifiedBy, timesheet);
                    solarPMSEntities.SaveChanges();
                    // As actual approved quantity should be sent to SAP
                    // and original quantity submitted by user should not be modified
                    // here data filed is changed temporary.
                    timesheet.ActualQuantity = timesheet.ApprovedQuantity;
                    SetSAPCallSuccess(timesheet);
                    return timesheet.TimeSheetId;
                }
                return 0;
            }
        }

        public static bool IsPartailApproveByQM(int TimesheetId)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                TimesheetApproval timesheetApproval = solarPMSEntities.TimesheetApprovals
                                                                      .Where(t => t.TimesheetId == TimesheetId)
                                                                      .OrderByDescending(t => t.TimesheetApprovalId)
                                                                      .Take(1).FirstOrDefault();
                if (timesheetApproval != null)
                    if (timesheetApproval.WorkflowStatusId == Convert.ToInt32(Constants.WorkflowStatus.PartialApprovedByQM))
                        return true;
            }
            return false;
        }

        public static decimal GetTimesheetApprovedQuantity(int TimesheetId)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                decimal ApprovedQuantity = 0;
                Timesheet result = solarPMSEntities.Timesheets.FirstOrDefault(t => t.TimeSheetId == TimesheetId);
                if (result != null)
                    if (result.ApprovedQuantity != null)
                        return result.ApprovedQuantity.Value;
                return ApprovedQuantity;
            }
        }
        public static List<ManagerTimesheetActivity> GetManagerActivityDetails(int TimesheetId, string Flag)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                Int16 managerType = Convert.ToInt16(Constants.ManagerType.QM);
                Int16 status = Convert.ToInt16(Constants.ManagerTimesheetStatus.Approved);
                solarPMSEntities.Configuration.ProxyCreationEnabled = false;
                return solarPMSEntities.ManagerTimesheetActivities.Where(t => t.TimesheetId == TimesheetId
                                                                        && t.Flag.ToLower() == Flag.ToLower()
                                                                        && t.ManagerType == managerType
                                                                        && t.Status == status)
                                                                        .ToList();
            }
        }
        public static List<ManagerTimesheetBlockDetail> GetManagerBlockDetails(int TimesheetId, string Flag)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                Int16 managerType = Convert.ToInt16(Constants.ManagerType.QM);
                Int16 status = Convert.ToInt16(Constants.ManagerTimesheetStatus.Approved);
                solarPMSEntities.Configuration.ProxyCreationEnabled = false;
                return solarPMSEntities.ManagerTimesheetBlockDetails.Where(t => t.TimesheetId == TimesheetId
                                                                        && t.ManagerType == managerType
                                                                        && t.Status == status)
                                                                        .ToList();
            }
        }

        
        public static dynamic GetManagerSurveyDetails(int TimesheetId, string Flag)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                Int16 managerType = Convert.ToInt16(Constants.ManagerType.QM);
                Int16 status = Convert.ToInt16(Constants.ManagerTimesheetStatus.Approved);
                return solarPMSEntities.ManagerTimesheetSurveyDetails
                                       .Join(solarPMSEntities.VillageMasters, m => m.VillageId, v => v.VillageId, (m, v) => new { m, v })
                                       .Where(r=>r.m.Status == status 
                                              && r.m.ManagerType == managerType
                                              && r.m.TimesheetId == TimesheetId)
                                       .Select(r => new
                                       {
                                           r.m.TimesheetSurveyDetailId,
                                           r.m.ActualArea,
                                           r.m.ActualNoOfDivision,
                                           r.m.CreatedBy,
                                           r.m.CreatedOn,
                                           r.m.ManagerType,
                                           r.m.ModifiedBy,
                                           r.m.ModifiedOn,
                                           r.m.Status,
                                           r.m.SurveyNo,
                                           r.m.TimesheetId,
                                           r.m.VillageId,
                                           r.v.VillageName
                                       }).ToList();
            }
        }

        public static List<ManagerTimesheetBlockDetail> GetManagerBlockDetails(int TimesheetId, Int16 status)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                solarPMSEntities.Configuration.ProxyCreationEnabled = false;
                return solarPMSEntities.ManagerTimesheetBlockDetails.Where(t => t.TimesheetId == TimesheetId
                                                                        && t.Status == status)
                                                                        .ToList();
            }
        }

        public static List<usp_GetInvalidOfflineTimesheet_Result> GetRejectedOfflineTimesheet(int UserId, string Site, string Project,string Area ,string Network, bool IsFromMobile)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                return solarPMSEntities.usp_GetInvalidOfflineTimesheet(UserId, IsFromMobile, Network, Area, Project, Site).ToList();
            }
        }
        public static string ValidateOfflineTimesheet(Timesheet offlineTimesheet)
        {
            if (CheckTimesheetExistsForDate(offlineTimesheet, offlineTimesheet.ActualDate.ToLocalTime(), offlineTimesheet.CreatedBy))
            {
                if (offlineTimesheet.TimesheetActivities.Count > 0)
                {
                    int[] numberArray = { };
                    string flag = offlineTimesheet.TimesheetActivities.FirstOrDefault().Flag;
                    foreach (var activity in offlineTimesheet.TimesheetActivities)
                        numberArray = CommonFunctions.GetNumberRange(activity.FromRange.Value, activity.ToRange.Value);
                    if (numberArray.Count() > 0)
                    {
                        List<int> existingNumbers = TableActivityModel.CheckExistingNumbers(offlineTimesheet.SAPSubActivity, offlineTimesheet.SAPActivity, offlineTimesheet.SAPNetwork, offlineTimesheet.WBSAreaId.Value, offlineTimesheet.SAPProject, offlineTimesheet.SAPSite, 0, numberArray, flag);
                        if (existingNumbers.Count() > 0)
                            return "Numbers already exists in another timesheet.";
                        else
                            return string.Empty;
                    }
                    else
                        return "Invalid range.";
                }
                else
                    return string.Empty;
            }
            else
                return "Date already exists.";
        }

        #region "SAP"

        /// <summary>
        /// Set SapCallSuccess flag in timesheet table according to SAP call result.
        /// </summary>
        /// <param name="updateTimesheet"></param>
        private void SetSAPCallSuccess(Timesheet updateTimesheet)
        {
            if (updateTimesheet.StatusId == Convert.ToInt32(Constants.TimesheetStatus.Ongoing)
                || updateTimesheet.StatusId == Convert.ToInt32(Constants.TimesheetStatus.Closed))
            {
                string sapresult = PushSapToData(updateTimesheet);
                if (string.IsNullOrEmpty(sapresult))
                    updateTimesheet.SAPCallSuccess = null;
                else if (sapresult == Constants.SUCCESS)
                    updateTimesheet.SAPCallSuccess = true;
                else
                    updateTimesheet.SAPCallSuccess = false;
            }
        }
        private string PushSapToData(Timesheet timesheet)
        {
            if (ConfigurationManager.AppSettings["PushDataToSAP"] == "true")
            {
                string activityCompletionStatus = string.Empty;
                if (timesheet.StatusId == Convert.ToInt32(Constants.TimesheetStatus.Closed))
                    activityCompletionStatus = "X";
                string result = string.Empty;
                if (timesheet.WorkflowStatusId == Convert.ToInt32(Constants.WorkflowStatus.AutoApproved)
                    || timesheet.WorkflowStatusId == Convert.ToInt32(Constants.WorkflowStatus.PMApproved)
                    || timesheet.WorkflowStatusId == Convert.ToInt32(Constants.WorkflowStatus.PartialApprovedByPM))
                    return PushTimesheetDataToSap(timesheet.SAPNetwork, timesheet.SAPActivity, timesheet.ActualDate.ToString("yyyyMMdd"), timesheet.ActualQuantity.Value, timesheet.Comments, activityCompletionStatus);
            }
            return string.Empty;
        }

        private string PushTimesheetDataToSap(string Network, string Activity, string ActualDate, decimal ActualQuantity, string Comments, string CompletionStatus)
        {
            SAPHelper objSAP = new SAPHelper();
            using (SAPConnection connection = objSAP.GetSAPConnection())
            {
                Solar_SAP.MobileData obj = new Solar_SAP.MobileData();
                Solar_SAP.BAPIRETURNTable bapiRTable = new Solar_SAP.BAPIRETURNTable();
                obj.Connection = connection;
                obj.Connection.Open();
                obj.Ypsf_Solar_Mobile_Update_Data(Activity, ActualDate, Math.Round(ActualQuantity,1), ActualDate, Comments, CompletionStatus, Network, ref bapiRTable);
                System.Data.DataTable bapiResult = bapiRTable.ToADODataTable();
                if (bapiResult != null && bapiResult.Rows.Count > 0)
                {
                    string result = Convert.ToString(bapiResult.Rows[0]["Type"]);
                    if (result != "S")
                    {
                        return Convert.ToString(bapiResult.Rows[0]["Message"]);
                    }
                    else
                        return Constants.SUCCESS;
                }
                else
                    return Constants.SAP_ERROR;
            }
        }
        #endregion
    }

    public class TimesheetSurveyDetailsView : TimesheetSurveyDetail
    {
        public string VillageName { get; set; }
        public int PraposedDivision { get; set; }
        public decimal PraposedArea { get; set; }
        public short ManagerType { get; set; }
    }

    public class TimesheetComment
    {
        public string VillageName { get; set; }
        public int PraposedDivision { get; set; }
        public decimal PraposedArea { get; set; }
    }
}
