using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SolarPMS.Models
{
    public class IssueCategoryModel
    {
        #region "Public Methods"
        /// <summary>
        ///// This method used to get all IssueCategories details.
        /// </summary>
        /// <returns></returns>
        public List<IssueCategory> GetIssueCategories()
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                solarPMSEntities.Configuration.ProxyCreationEnabled = false;
                return solarPMSEntities.IssueCategories.ToList();
            }
        }
        /// <summary>
        /// This method used to add IssueCategory details
        /// </summary>
        /// <param name="issueCategory"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IssueCategory AddIssueCategory(IssueCategory issueCategory, int userId)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
               // issueCategory.Status = true;
                issueCategory.CreatedBy = userId;
                issueCategory.CreatedOn = DateTime.Now;
                issueCategory.ModifiedBy = userId;
                issueCategory.ModifiedOn = DateTime.Now;
                solarPMSEntities.IssueCategories.Add(issueCategory);
                solarPMSEntities.SaveChanges();
                return issueCategory;
            }
        }

        /// <summary>
        /// This method used to update IssueCategory details.
        /// </summary>
        /// <param name="issueCategory"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool UpdateIssueCategory(IssueCategory issueCategory, int userId)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                IssueCategory issueCategoryMaster = solarPMSEntities.IssueCategories.FirstOrDefault(l => l.IssueCategoryId == issueCategory.IssueCategoryId);
                if (issueCategoryMaster != null)
                {
                    issueCategoryMaster.CategoryName = issueCategory.CategoryName;
                    issueCategoryMaster.Description = issueCategory.Description;
                    issueCategoryMaster.Status = issueCategory.Status;
                    issueCategoryMaster.ModifiedBy = userId;
                    issueCategoryMaster.ModifiedOn = DateTime.Now;
                    solarPMSEntities.Entry(issueCategoryMaster).State = EntityState.Modified;
                    solarPMSEntities.SaveChanges();
                    return true;
                }
                else
                    return false;
            }
        }

        /// <summary>
        ///// This method used to check for duplicate IssueCategory.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool IssueCategoryExists(string name, int id)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                return solarPMSEntities.IssueCategories.FirstOrDefault(l => l.CategoryName.ToLower() == name.ToLower() && l.IssueCategoryId != id) != null;
            }
        }
        #endregion "Public Methods"
    }
}