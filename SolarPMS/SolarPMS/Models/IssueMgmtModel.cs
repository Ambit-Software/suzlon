using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SolarPMS.Models
{
    public class IssueMgmtModel
    {
        /// <summary>
        /// This method is used to get issue details by issueId.
        /// </summary>
        /// <param name="issueId"></param>
        /// <returns></returns>
        public dynamic GetIssueDetail(int issueId)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                solarPMSEntities.Configuration.ProxyCreationEnabled = false;
                var issueDetail = (from issue in solarPMSEntities.IssueManagements
                                   where issue.IssueId == issueId
                                   join usr in solarPMSEntities.Users
                                   on issue.CreatedBy equals usr.UserId
                                   select new
                                   {
                                       issue.IssueId,
                                       issue.IssueDate,
                                       issue.CategoryId,
                                       issue.LocationId,
                                       issue.LocationType,
                                       issue.Description,
                                       issue.ExpectedClosureDate,
                                       issue.IssueStatus,
                                       issue.AssignedTo,
                                       issue.Resolution,
                                       issue.CreatedBy,
                                       issue.CreatedOn,
                                       issue.ModifiedBy,
                                       issue.ModifiedOn,
                                       usr.Name,

                                       IssueCreatedBy = (from issues in solarPMSEntities.IssueManagements
                                                         join user in solarPMSEntities.Users
                                                             on issues.CreatedBy equals user.UserId
                                                         where issues.IssueId == issueId
                                                         select new { user.Name }).FirstOrDefault(),

                                       IssueModifiedBy = (from issues in solarPMSEntities.IssueManagements
                                                          join user in solarPMSEntities.Users
                                                             on issues.ModifiedBy equals user.UserId
                                                          where issues.IssueId == issueId
                                                          select new { user.Name }).FirstOrDefault(),


                                       IssueComments = (from comment in solarPMSEntities.IssueComments
                                                        join user in solarPMSEntities.Users
                                                            on comment.CreatedBy equals user.UserId
                                                        where comment.IssueId == issueId
                                                        select new { comment.IssueId, comment.IssueCommentId, comment.Comment, comment.CreatedOn, comment.CreatedBy, user.Name }).ToList(),
                                       IssueAttachments = (from attch in solarPMSEntities.IssueAttachments
                                                           join user in solarPMSEntities.Users
                                                           on attch.CreatedBy equals user.UserId
                                                           where attch.IssueId == issueId
                                                           select new { attch.IssueId, attch.IssueAttachementId, attch.FilePath, attch.CreatedOn, attch.CreatedBy, user.Name }).ToList(),
                                   }).FirstOrDefault();
                return issueDetail;
            }
        }

        /// <summary>
        /// This method is used to get issue assigned to user.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<dynamic> GetIssueAssignedToMe(int userId, string searchText)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                solarPMSEntities.Configuration.ProxyCreationEnabled = false;
                //return   solarPMSEntities.IssueManagements.ToList();
                var issueDetail = (from issue in solarPMSEntities.IssueManagements
                                   join user in solarPMSEntities.Users
                                   on issue.AssignedTo equals user.UserId
                                   join category in solarPMSEntities.IssueCategories
                                   on issue.CategoryId equals category.IssueCategoryId
                                   join loc in solarPMSEntities.LocationMasters
                                   on issue.LocationId equals loc.LocationId into location
                                   from loc in location.DefaultIfEmpty()
                                   where issue.AssignedTo == userId
                                  // && ( issue.IssueStatus == "Open" || issue.IssueStatus == "Ongoing" || issue.IssueStatus == "Closed")
                                   && (string.IsNullOrEmpty(searchText) ? true : (
                                   issue.Description.Contains(searchText) || loc.LocationName.Contains(searchText)
                                   || category.CategoryName.Contains(searchText)
                                   ))
                                   orderby issue.ModifiedOn descending
                                   select new
                                   {
                                       issue.IssueId,
                                       issue.Description,
                                       issue.Site,
                                       issue.Area,
                                       issue.IssueDate,
                                       issue.ExpectedClosureDate,
                                       issue.ActualClosingDate,
                                       issue.IssueStatus,
                                       category.CategoryName,
                                       loc.LocationName,
                                       issue.Status,

                                       UserName = user.Name
                                   }).ToList();

                return issueDetail.ToList<dynamic>();
            }
        }

        /// <summary>
        /// This method is used to get issue raised by user.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="searchText"></param>
        /// <returns></returns>
        public List<dynamic> GetIssueRaisedByMe(int userId, string searchText)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                solarPMSEntities.Configuration.ProxyCreationEnabled = false;
                var issueDetail = (from issue in solarPMSEntities.IssueManagements
                                   join user in solarPMSEntities.Users
                                   on issue.AssignedTo equals user.UserId
                                   join category in solarPMSEntities.IssueCategories
                                   on issue.CategoryId equals category.IssueCategoryId
                                   join loc in solarPMSEntities.LocationMasters
                                    on issue.LocationId equals loc.LocationId into location
                                   from loc in location.DefaultIfEmpty()
                                   where issue.CreatedBy == userId
                                    && (string.IsNullOrEmpty(searchText) ? true : (
                                   issue.Description.Contains(searchText) || loc.LocationName.Contains(searchText)
                                   || category.CategoryName.Contains(searchText)
                                   ))
                                   orderby issue.ModifiedOn descending
                                   select new
                                   {
                                       issue.IssueId,
                                       issue.Description,
                                       issue.Site,
                                       issue.Area,
                                       issue.IssueDate,
                                       issue.ExpectedClosureDate,
                                       issue.ActualClosingDate,
                                       issue.IssueStatus,
                                       category.CategoryName,
                                       loc.LocationName,
                                       issue.Status,
                                       UserName = user.Name
                                   }).ToList();

                return issueDetail.ToList<dynamic>();
            }
        }

        public List<dynamic> GetAllIssues(string searchText)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                solarPMSEntities.Configuration.ProxyCreationEnabled = false;
                var issueDetail = (from issue in solarPMSEntities.IssueManagements
                                   join user in solarPMSEntities.Users
                                   on issue.AssignedTo equals user.UserId
                                   join category in solarPMSEntities.IssueCategories
                                   on issue.CategoryId equals category.IssueCategoryId
                                   join loc in solarPMSEntities.LocationMasters
                                    on issue.LocationId equals loc.LocationId into location
                                   from loc in location.DefaultIfEmpty()
                                   where (string.IsNullOrEmpty(searchText) ? true : (
                                   issue.Description.Contains(searchText) || loc.LocationName.Contains(searchText)
                                   || category.CategoryName.Contains(searchText)
                                   ))
                                   orderby issue.ModifiedOn descending
                                   select new
                                   {
                                       issue.IssueId,
                                       issue.Description,
                                       issue.Site,
                                       issue.Area,
                                       issue.IssueDate,
                                       issue.ExpectedClosureDate,
                                       issue.ActualClosingDate,
                                       issue.IssueStatus,
                                       category.CategoryName,
                                       loc.LocationName,
                                       issue.Status,
                                       UserName = user.Name
                                   }).ToList();

                return issueDetail.ToList<dynamic>();
            }
        }

        /// <summary>
        /// /// This method is used to add issue.
        /// </summary>
        /// <param name="issueManagement"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IssueManagement AddIssue(IssueManagement issueManagement, int userId)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                if (issueManagement.IssueStatus == "Resolved")
                    issueManagement.ResolutionDate = DateTime.Now;

                issueManagement.Status = true;
                issueManagement.CreatedBy = userId;
                issueManagement.CreatedOn = DateTime.Now;
                issueManagement.ModifiedBy = userId;
                issueManagement.ModifiedOn = DateTime.Now;
                List<IssueAttachment> attachments = issueManagement.IssueAttachments.ToList();
                List<IssueComment> comments = issueManagement.IssueComments.ToList();
                issueManagement.IssueAttachments = null;
                issueManagement.IssueComments = null;
                solarPMSEntities.IssueManagements.Add(issueManagement);
                solarPMSEntities.SaveChanges();
                solarPMSEntities.Configuration.ProxyCreationEnabled = false;
                solarPMSEntities.Configuration.LazyLoadingEnabled = false;
                issueManagement.IssueAttachments = AddAttachment(attachments, issueManagement.IssueId, userId);
                issueManagement.IssueComments = AddComment(comments, issueManagement.IssueId, userId);
                return issueManagement;
            }
        }

        public IssueManagement UpdateIssue(IssueManagement issueManagement, int userId)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                var issue = solarPMSEntities.IssueManagements.AsNoTracking().FirstOrDefault(i => i.IssueId == issueManagement.IssueId);
                if (issue != null)
                {
                    List<IssueAttachment> attachments = issueManagement.IssueAttachments.ToList();
                    List<IssueComment> comments = issueManagement.IssueComments.ToList();

                    if (issueManagement.ActivityId != 0)
                        issue.ActivityId = issueManagement.ActivityId;
                    if (issueManagement.SubActivityId != 0)
                        issue.SubActivityId = issueManagement.SubActivityId;

                    issue.Status = issueManagement.Status;
                    issue.IssueDate = issueManagement.IssueDate;
                    issue.ExpectedClosureDate = issueManagement.ExpectedClosureDate;

                    issue.CategoryId = issueManagement.CategoryId;
                    issue.LocationId = issueManagement.LocationId;
                    issue.AssignedTo = issueManagement.AssignedTo;

                    issue.Description = issueManagement.Description;
                    issue.Resolution = issueManagement.Resolution;

                    if (issue.IssueStatus != "Resolved" && issueManagement.IssueStatus == "Resolved")
                        issue.ResolutionDate = DateTime.Now; 

                    issue.IssueStatus = issueManagement.IssueStatus;
                    issue.LocationType = issueManagement.LocationType;
                    if (issueManagement.IssueStatus == "Closed")
                        issue.ActualClosingDate = DateTime.Now;

                    issue.ModifiedBy = userId;
                    issue.ModifiedOn = DateTime.Now;
                    solarPMSEntities.Entry(issue).State = EntityState.Modified;
                    solarPMSEntities.SaveChanges();
                    solarPMSEntities.Configuration.ProxyCreationEnabled = false;
                    solarPMSEntities.Configuration.LazyLoadingEnabled = false;
                    issueManagement.IssueAttachments = AddAttachment(attachments, issueManagement.IssueId, userId);
                    issueManagement.IssueComments = AddComment(comments, issueManagement.IssueId, userId);
                    issueManagement.CreatedBy = issue.CreatedBy;
                }
            }
            return issueManagement;
        }

        /// <summary>
        /// This method is used to add issue attchments.
        /// </summary>
        /// <param name="issueAttachments"></param>
        /// <param name="issueId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<IssueAttachment> AddAttachment(List<IssueAttachment> issueAttachments, int issueId, int userId)
        {
            if (issueAttachments != null && issueAttachments.Count > 0)
            {
                using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
                {
                    issueAttachments.ForEach(attachment =>
                    {
                        attachment.IssueId = issueId;
                        attachment.CreatedBy = userId;
                        attachment.CreatedOn = DateTime.Now;
                        attachment.ModifiedBy = userId;
                        attachment.ModifiedOn = DateTime.Now;
                        solarPMSEntities.IssueAttachments.Add(attachment);
                    });
                    solarPMSEntities.SaveChanges();
                }
            }
            return issueAttachments;
        }

        /// <summary>
        /// This method is used to add issue comments.
        /// </summary>
        /// <param name="issueComments"></param>
        /// <param name="issueId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<IssueComment> AddComment(List<IssueComment> issueComments, int issueId, int userId)
        {
            if (issueComments != null && issueComments.Count > 0)
            {
                using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
                {
                    issueComments.ForEach(comment =>
                    {
                        comment.IssueId = issueId;
                        comment.CreatedBy = userId;
                        comment.CreatedOn = DateTime.Now;
                        comment.ModifiedBy = userId;
                        comment.ModifiedOn = DateTime.Now;
                        solarPMSEntities.IssueComments.Add(comment);
                    });
                    solarPMSEntities.SaveChanges();
                }
            }
            return issueComments;
        }

        public List<dynamic> GetIssueByActivityId(int? activityId)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                solarPMSEntities.Configuration.ProxyCreationEnabled = false;
                //return   solarPMSEntities.IssueManagements.ToList();
                var issueDetail = (from issue in solarPMSEntities.IssueManagements
                                   join category in solarPMSEntities.IssueCategories
                                   on issue.CategoryId equals category.IssueCategoryId
                                   join loc in solarPMSEntities.LocationMasters
                                   on issue.LocationId equals loc.LocationId into location
                                   from loc in location.DefaultIfEmpty()
                                   join user in solarPMSEntities.Users
                                   on issue.AssignedTo equals user.UserId
                                   where issue.ActivityId == activityId
                                   orderby issue.ModifiedOn
                                   select new
                                   {
                                       issue.IssueId,
                                       issue.Description,
                                       issue.Site,
                                       issue.Area,
                                       issue.IssueDate,
                                       issue.ExpectedClosureDate,
                                       issue.ActualClosingDate,
                                       issue.IssueStatus,
                                       category.CategoryName,
                                       loc.LocationName,
                                       issue.Status,
                                       user.Name,
                                       issue.ActivityId,
                                       issue.SubActivityId
                                   }).ToList(); ;

                return issueDetail.ToList<dynamic>();
            }
        }

        public List<dynamic> GetIssueBySubActivityId(int? subActivityId)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                solarPMSEntities.Configuration.ProxyCreationEnabled = false;
                //return   solarPMSEntities.IssueManagements.ToList();
                var issueDetail = (from issue in solarPMSEntities.IssueManagements
                                   join category in solarPMSEntities.IssueCategories
                                   on issue.CategoryId equals category.IssueCategoryId
                                   join loc in solarPMSEntities.LocationMasters
                                   on issue.LocationId equals loc.LocationId into location
                                   from loc in location.DefaultIfEmpty()
                                   join user in solarPMSEntities.Users
                                   on issue.AssignedTo equals user.UserId
                                   where issue.SubActivityId == subActivityId
                                   orderby issue.ModifiedOn
                                   select new
                                   {
                                       issue.IssueId,
                                       issue.Description,
                                       issue.Site,
                                       issue.Area,
                                       issue.IssueDate,
                                       issue.ExpectedClosureDate,
                                       issue.ActualClosingDate,
                                       issue.IssueStatus,
                                       category.CategoryName,
                                       loc.LocationName,
                                       issue.Status,
                                       user.Name,
                                       issue.ActivityId,
                                       issue.SubActivityId
                                   }).ToList();

                return issueDetail.ToList<dynamic>();
            }
        }

        public List<dynamic> GetIssueAssignHistory(int issueId)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                solarPMSEntities.Configuration.ProxyCreationEnabled = false;

                var issueHistoryDetail = (from issue in solarPMSEntities.IssueManagementAudits
                                          join userdtl in solarPMSEntities.Users on issue.AssignedTo equals userdtl.UserId
                                          where issue.IssueId == issueId
                                          orderby issue.AssignedDate descending
                                          select new { userdtl.Name, issue.AssignedDate }
                                         ).ToList();

                return issueHistoryDetail.ToList<dynamic>();
            }
        }


        public static void GetIssueByAssignedActivity()
        {
        }
    }
}
