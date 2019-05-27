using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SolarPMS.Models
{
    public class SurveyModel
    {
        /// <summary>
        /// This method used to get all survey details
        /// </summary>
        /// <returns></returns>
        public List<dynamic> GetAllSurveyInfo()
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                solarPMSEntities.Configuration.ProxyCreationEnabled = false;
                var SurveyDetails = (from survey in solarPMSEntities.SurveyMasters
                                     join village in solarPMSEntities.VillageMasters
                                     on survey.VillageId equals village.VillageId
                                     let project = solarPMSEntities.SAPMasters.Where(s => s.SAPProjectId == survey.ProjectId).FirstOrDefault()
                                     select new
                                     {
                                         survey.SurveyId,
                                         survey.SurveyNo,
                                         survey.Site,
                                         survey.NoOfDivision,
                                         //survey.PropsedTotal,
                                         survey.Area,
                                         survey.VillageId,
                                         village.VillageName,
                                         project.SAPProjectId,
                                         project.ProjectDescription,
                                         survey.Status
                                     });

                return SurveyDetails.ToList<dynamic>();
            }
        }

        /// <summary>
        /// This method used to add survey details
        /// </summary>
        /// <param name="surveyMaster"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public SurveyMaster AddSurvey(SurveyMaster surveyMaster, int userId)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                // surveyMaster.Status = true;
                surveyMaster.CreatedBy = userId;
                surveyMaster.CreatedOn = DateTime.Now;
                surveyMaster.ModifiedBy = userId;
                surveyMaster.ModifiedOn = DateTime.Now;
                solarPMSEntities.SurveyMasters.Add(surveyMaster);
                solarPMSEntities.SaveChanges();
                solarPMSEntities.Configuration.ProxyCreationEnabled = false;
                solarPMSEntities.Configuration.LazyLoadingEnabled = false;
                return surveyMaster;
            }
        }

        /// <summary>
        /// This method used to update survey details.
        /// </summary>
        /// <param name="surveyMaster"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool UpdateSurveyDetail(SurveyMaster surveyMaster, int userId)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                SurveyMaster Survey = solarPMSEntities.SurveyMasters.AsNoTracking().FirstOrDefault(l => l.SurveyId == surveyMaster.SurveyId);
                if (Survey != null)
                {
                    surveyMaster.CreatedBy = Survey.CreatedBy;
                    surveyMaster.CreatedOn = Survey.CreatedOn;
                    //surveyMaster.Status = Survey.Status;
                    surveyMaster.ModifiedBy = userId;
                    surveyMaster.ModifiedOn = DateTime.Now;
                    solarPMSEntities.Entry(surveyMaster).State = EntityState.Modified;
                    solarPMSEntities.SaveChanges();
                    return true;
                }
                else
                    return false;
            }
        }

        /// <summary>
        /// This method used to check for duplicate survey.
        /// </summary>
        /// <param name="suveryNo"></param>
        /// <param name="villageId"></param>
        /// <param name="suveryId"></param>
        /// <returns></returns>
        public bool SurveyExists(string suveryNo, int villageId, int suveryId, string site, string projid)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                return solarPMSEntities.SurveyMasters.FirstOrDefault(s => s.SurveyNo.ToLower() == suveryNo.ToLower() && s.VillageId == villageId && s.ProjectId == projid.Trim() && s.SurveyId != suveryId && s.Site == site.ToLower()) != null;
            }
        }

        //public List<SurveyMaster> GetVillage(SurveyMaster surveyMaster)
        //{
        //    using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
        //    {
        //        return solarPMSEntities.SurveyMasters.Where(s => s.Site.ToLower() == surveyMaster.Site.ToLower() && s.ProjectId == surveyMaster.ProjectId).ToList<SurveyMaster>();
        //    }
        //}

        public List<dynamic> GetVillage(SurveyMaster surveyMaster)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                solarPMSEntities.Configuration.ProxyCreationEnabled = false;
                var SurveyDetails = (from survey in solarPMSEntities.SurveyMasters
                                     join village in solarPMSEntities.VillageMasters
                                     on survey.VillageId equals village.VillageId
                                     where survey.Site == surveyMaster.Site.Trim()
                                     && survey.ProjectId == surveyMaster.ProjectId.Trim()
                                     orderby village.VillageName ascending
                                     select new
                                     {
                                         survey.SurveyId,
                                         survey.SurveyNo,
                                         survey.Site,
                                         survey.NoOfDivision,
                                         survey.Area,
                                         survey.VillageId,
                                         village.VillageName,
                                         survey.Status
                                     });

                return SurveyDetails.ToList<dynamic>();
            }
        }

        public static decimal[] GetCompletedQuantity(string SubActivity, string Activity, string Network, int AreaId, string Project, string Site,int VillageId, string SurveyNo, int TimesheetId)
        {
            decimal[] completedArea = { 0, 0 };
            int approvedByQM = Convert.ToInt32(Constants.WorkflowStatus.PartialApprovedByQM);
            int approvedByPM = Convert.ToInt32(Constants.WorkflowStatus.PartialApprovedByPM);
            int rejectedByQM = Convert.ToInt32(Constants.WorkflowStatus.QARejected);
            int rejectedByPM = Convert.ToInt32(Constants.WorkflowStatus.PMRejected);

            decimal[] qmApprovedArea = { 0, 0 };
            decimal[] pmApprovedArea = { 0, 0 };

            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                List<TimesheetSurveyDetail> result = null;
                dynamic managerApprovedQuantity = null;
                if (string.IsNullOrEmpty(SubActivity))
                {
                    result = solarPMSEntities.Timesheets
                                                .Join(solarPMSEntities.TimesheetSurveyDetails, t => t.TimeSheetId, tsd => tsd.TimesheetId, (t, tsd) => new { t, tsd })
                                                .Where(r => r.t.SAPActivity == Activity
                                                       && r.t.SAPNetwork == Network
                                                       && r.t.WBSAreaId == AreaId
                                                       && r.t.SAPProject == Project
                                                       && r.t.SAPSite == Site
                                                       && r.t.TimeSheetId != TimesheetId
                                                       && r.tsd.VillageId == VillageId
                                                       && r.tsd.SurveyNo == SurveyNo
                                                       && r.t.WorkflowStatusId != approvedByQM
                                                       && r.t.WorkflowStatusId != approvedByPM
                                                       && r.t.WorkflowStatusId != rejectedByQM // qa rejected
                                                       && r.t.WorkflowStatusId != rejectedByPM // pm rejected
                                                       ).Select(r => r.tsd).ToList();

                    var allRecords = solarPMSEntities.Timesheets
                                                .Join(solarPMSEntities.ManagerTimesheetSurveyDetails, t => t.TimeSheetId, tsd => tsd.TimesheetId, (t, tsd) => new { t, tsd })
                                                .Where(r => r.t.SAPActivity == Activity
                                                       && r.t.SAPNetwork == Network
                                                       && r.t.WBSAreaId == AreaId
                                                       && r.t.SAPProject == Project
                                                       && r.t.SAPSite == Site
                                                       && r.t.TimeSheetId != TimesheetId
                                                       && r.tsd.VillageId == VillageId
                                                       && r.tsd.SurveyNo == SurveyNo
                                                       && (r.t.WorkflowStatusId == approvedByQM
                                                       || r.t.WorkflowStatusId == approvedByPM)
                                                       && r.tsd.Status == 1//Approved records
                                                       ).Select(r => new { r.tsd, r.t }).ToList();

                    var QAApprovedQuantity = allRecords.Where(t => t.t.WorkflowStatusId == 8 && t.tsd.ManagerType == 1).Select(t => t.tsd).ToList();
                    var PMApprovedQuantity = allRecords.Where(t => t.t.WorkflowStatusId == 9 && t.tsd.ManagerType == 2).Select(t => t.tsd).ToList();
                    GetManagerApprovedArea(qmApprovedArea, pmApprovedArea, QAApprovedQuantity, PMApprovedQuantity);
                }
                else
                {
                    result = solarPMSEntities.Timesheets
                                                .Join(solarPMSEntities.TimesheetSurveyDetails, t => t.TimeSheetId, tsd => tsd.TimesheetId, (t, tsd) => new { t, tsd })
                                                .Where(r => r.t.SAPActivity == Activity
                                                        && r.t.SAPActivity == SubActivity
                                                       && r.t.SAPNetwork == Network
                                                       && r.t.WBSAreaId == AreaId
                                                       && r.t.SAPProject == Project
                                                       && r.t.SAPSite == Site
                                                       && r.t.TimeSheetId != TimesheetId
                                                       && r.t.WorkflowStatusId != approvedByQM
                                                       && r.t.WorkflowStatusId != approvedByPM
                                                       && r.t.WorkflowStatusId != rejectedByQM // qa rejected
                                                       && r.t.WorkflowStatusId != rejectedByPM // pm rejected
                                                       && r.tsd.VillageId == VillageId
                                                       && r.tsd.SurveyNo == SurveyNo
                                                       ).Select(r => r.tsd).ToList();

                    var allRecords = solarPMSEntities.Timesheets
                                                .Join(solarPMSEntities.ManagerTimesheetSurveyDetails, t => t.TimeSheetId, tsd => tsd.TimesheetId, (t, tsd) => new { t, tsd })
                                                .Where(r => r.t.SAPActivity == Activity
                                                        && r.t.SAPActivity == SubActivity
                                                       && r.t.SAPNetwork == Network
                                                       && r.t.WBSAreaId == AreaId
                                                       && r.t.SAPProject == Project
                                                       && r.t.SAPSite == Site
                                                       && r.t.TimeSheetId != TimesheetId
                                                       && (r.t.WorkflowStatusId == approvedByQM
                                                       || r.t.WorkflowStatusId == approvedByPM)
                                                       && r.tsd.Status == 1//Approved records
                                                       && r.tsd.VillageId == VillageId
                                                       && r.tsd.SurveyNo == SurveyNo
                                                       ).Select(r => new { r.tsd, r.t }).ToList();

                    var QAApprovedQuantity = allRecords.Where(t => t.t.WorkflowStatusId == 8 && t.tsd.ManagerType == 1).Select(t => t.tsd).ToList();
                    var PMApprovedQuantity = allRecords.Where(t => t.t.WorkflowStatusId == 9 && t.tsd.ManagerType == 2).Select(t => t.tsd).ToList();

                    GetManagerApprovedArea(qmApprovedArea, pmApprovedArea, QAApprovedQuantity, PMApprovedQuantity);
                }

                if (result != null && result.Count > 0)
                {
                    completedArea[0] = result.Sum(r => r.ActualArea).Value;
                    completedArea[1] = result.Sum(r => r.ActualNoOfDivision).Value;
                }

                completedArea[0] += qmApprovedArea[0] + pmApprovedArea[0];
                completedArea[1] += qmApprovedArea[1] + pmApprovedArea[1];

                return completedArea;
            }
        }

        private static void GetManagerApprovedArea(decimal[] qmApprovedArea, decimal[] pmApprovedArea, List<ManagerTimesheetSurveyDetail> QAApprovedQuantity, List<ManagerTimesheetSurveyDetail> PMApprovedQuantity)
        {
            if (QAApprovedQuantity != null && QAApprovedQuantity.Count > 0)
            {
                qmApprovedArea[0] = QAApprovedQuantity.Sum(r => r.ActualArea);
                qmApprovedArea[1] = QAApprovedQuantity.Sum(r => r.ActualNoOfDivision);
            }

            if (PMApprovedQuantity != null && PMApprovedQuantity.Count > 0)
            {
                pmApprovedArea[0] = PMApprovedQuantity.Sum(r => r.ActualArea);
                pmApprovedArea[1] = PMApprovedQuantity.Sum(r => r.ActualNoOfDivision);
            }
        }
    }
}