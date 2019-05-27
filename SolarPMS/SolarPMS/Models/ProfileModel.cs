using SolarPMS.Admin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace SolarPMS.Models
{
    public class ProfileModel
    {
        public int ProfileId { get; set; }
        public string ProfileName { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }
        public string MenuAuthorization { get; set; }
        public string MenuIds { get; set; }

        public int DEDocumentUploadAccess { get; set; }
        public string MenuNames { get; set; }
        public List<Menu> MenuDetail;

        #region "Public Methods"
        public List<ProfileModel> GetProfiles()
        {

            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                solarPMSEntities.Configuration.ProxyCreationEnabled = false;
                List<ProfileModel> profileDetailList = (from profile in solarPMSEntities.ProfileMasters
                                                        select new ProfileModel()
                                                        {
                                                            ProfileId = profile.ProfileId,
                                                            ProfileName = profile.ProfileName,
                                                            Status = profile.Status,
                                                            Description = profile.Description,
                                                            DEDocumentUploadAccess = profile.DEDocumentUploadAccess,
                                                            MenuDetail = (from menu in solarPMSEntities.Menus
                                                                          join access in solarPMSEntities.MenuAccesses
                                                                           on menu.MenuId equals access.MenuId
                                                                          where profile.ProfileId == access.ProfileId
                                                                          select menu).ToList()
                                                        }).OrderBy(profile => profile.ProfileName).ToList();
                profileDetailList.ForEach(profile =>
                {
                    profile.MenuIds = string.Join(",", profile.MenuDetail.Select(m => m.MenuId));
                    profile.MenuNames = string.Join(",", profile.MenuDetail.Select(m => m.MenuName));
                    profile.MenuDetail = null;
                });
                return profileDetailList;
            }
        }

        public List<Menu> GetAllMenuList()
        {
            List<Menu> menues = new List<Menu>();
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                solarPMSEntities.Configuration.ProxyCreationEnabled = false;
                menues = solarPMSEntities.Menus.ToList();
            }
            return menues;
        }

        public ProfileModel AddProfile(ProfileModel profileModel, int userId)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                ProfileMaster profileMaster = new ProfileMaster();
                profileMaster.ProfileName = profileModel.ProfileName;
                profileMaster.Description = profileModel.Description;
                profileMaster.Status = true;
                profileMaster.DEDocumentUploadAccess = profileModel.DEDocumentUploadAccess;
                profileMaster.CreatedBy = userId;
                profileMaster.CreatedOn = DateTime.Now;
                profileMaster.ModifiedBy = userId;
                profileMaster.ModifiedOn = DateTime.Now;
                solarPMSEntities.ProfileMasters.Add(profileMaster);
                solarPMSEntities.SaveChanges();
                profileModel.ProfileId = profileMaster.ProfileId;
                profileModel.Status = true;
                if (!string.IsNullOrEmpty(profileModel.MenuIds))
                {
                    AddMenuAccess(profileModel.MenuIds, profileMaster.ProfileId, userId);
                }
                return profileModel;
            }
        }

        public bool UpdateProfile(ProfileModel profileModel, int userId)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                ProfileMaster profileMaster = solarPMSEntities.ProfileMasters.FirstOrDefault(l => l.ProfileId == profileModel.ProfileId);

                if (profileMaster != null)
                {
                    profileMaster.ProfileName = profileModel.ProfileName;
                    profileMaster.Description = profileModel.Description;
                    profileMaster.Status = profileModel.Status;
                    profileMaster.DEDocumentUploadAccess = profileModel.DEDocumentUploadAccess;
                    profileMaster.ModifiedBy = userId;
                    profileMaster.ModifiedOn = DateTime.Now;
                    solarPMSEntities.Entry(profileMaster).State = EntityState.Modified;
                    solarPMSEntities.SaveChanges();
                    RemoveMenuAccess(profileMaster.ProfileId, userId);
                    if (!string.IsNullOrEmpty(profileModel.MenuIds))
                    {
                        AddMenuAccess(profileModel.MenuIds, profileMaster.ProfileId, userId);
                    }
                    return true;
                }
                else return false;

            }
        }
        public bool ProfileExist(string name,int profileId)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                return solarPMSEntities.ProfileMasters.FirstOrDefault(l => l.ProfileName.ToLower() == name.ToLower() && l.ProfileId != profileId) != null;
            }
        }

        private void AddMenuAccess(string menuIds, int profileId, int userId)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                menuIds.Split(',').ToList().ForEach(m =>
                    {
                        MenuAccess menuAccess = new MenuAccess();
                        menuAccess.MenuId = Convert.ToInt32(m);
                        menuAccess.ProfileId = profileId;
                        menuAccess.CreatedBy = userId;
                        menuAccess.CreatedOn = DateTime.Now;
                        menuAccess.ModifiedBy = userId;
                        menuAccess.ModifiedOn = DateTime.Now;
                        solarPMSEntities.MenuAccesses.Add(menuAccess);
                    });
                solarPMSEntities.SaveChanges();
            }
        }
        private void RemoveMenuAccess(int profileId, int userId)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                solarPMSEntities.MenuAccesses.RemoveRange(solarPMSEntities.MenuAccesses.Where(m => m.ProfileId == profileId).ToList());
                solarPMSEntities.SaveChanges();
            }
        }

        #endregion "Public Methods"
    }
}
