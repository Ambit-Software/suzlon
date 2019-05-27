using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace SuzlonBPP.Models
{
    public class ProfileModel
    {
        public int ProfileId { get; set; }
        public string ProfileName { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }
        public string MenuAuthorization { get; set; }
        public string MenuIds { get; set; }
        public string MenuNames { get; set; }
        public List<Menu> MenuDetail;


        #region "Public Methods"

        /// <summary>
        /// This method used to get all profile details.
        /// </summary>
        /// <returns></returns>
        public List<ProfileModel> GetProfiles()
        {

            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                suzlonBPPEntities.Configuration.ProxyCreationEnabled = false;
                List<ProfileModel> profileDetailList = (from profile in suzlonBPPEntities.ProfileMasters
                                                        select new ProfileModel()
                                                        {
                                                            ProfileId = profile.ProfileId,
                                                            ProfileName = profile.ProfileName,
                                                            Status = profile.Status,
                                                            Description = profile.Description,
                                                            MenuDetail = (from menu in suzlonBPPEntities.Menus
                                                                          join access in suzlonBPPEntities.MenuAccesses
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

            //using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            //{
            //    suzlonBPPEntities.Configuration.ProxyCreationEnabled = false;
            //    List<ProfileModel> profileDetailList = (from profile in suzlonBPPEntities.ProfileMasters
            //                                            select new ProfileModel()
            //                                            {
            //                                                ProfileId = profile.ProfileId,
            //                                                ProfileName = profile.ProfileName,
            //                                                Status = profile.Status,
            //                                                Description = profile.Description
            //                                            }).OrderBy(profile => profile.ProfileName).ToList();

            //    return profileDetailList;
            //}





        }

        /// <summary>
        /// This method used to add profile details
        /// </summary>
        /// <param name="profileModel"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ProfileModel AddProfile(ProfileModel profileModel, int userId)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                ProfileMaster profileMaster = new ProfileMaster();
                profileMaster.ProfileName = profileModel.ProfileName;
                profileMaster.Description = profileModel.Description;
                profileMaster.Status = profileModel.Status;
                profileMaster.CreatedBy = userId;
                profileMaster.CreatedOn = DateTime.Now;
                profileMaster.ModifiedBy = userId;
                profileMaster.ModifiedOn = DateTime.Now;
                suzlonBPPEntities.ProfileMasters.Add(profileMaster);
                suzlonBPPEntities.SaveChanges();
                profileModel.ProfileId = profileMaster.ProfileId;
                if (!string.IsNullOrEmpty(profileModel.MenuIds))
                {
                    AddMenuAccess(profileModel.MenuIds, profileMaster.ProfileId, userId);
                }

                return profileModel;
            }
        }
        private void AddMenuAccess(string menuIds, int profileId, int userId)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
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
                    suzlonBPPEntities.MenuAccesses.Add(menuAccess);
                });
                suzlonBPPEntities.SaveChanges();
            }
        }
        /// <summary>
        /// This method used to update profile details.
        /// </summary>
        /// <param name="profileModel"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool UpdateProfile(ProfileModel profileModel, int userId)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                ProfileMaster profileMaster = suzlonBPPEntities.ProfileMasters.FirstOrDefault(l => l.ProfileId == profileModel.ProfileId);

                if (profileMaster != null)
                {
                    profileMaster.ProfileName = profileModel.ProfileName;
                    profileMaster.Description = profileModel.Description;
                    profileMaster.Status = profileModel.Status;
                    profileMaster.ModifiedBy = userId;
                    profileMaster.ModifiedOn = DateTime.Now;
                    suzlonBPPEntities.Entry(profileMaster).State = EntityState.Modified;
                    suzlonBPPEntities.SaveChanges();
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

        private void RemoveMenuAccess(int profileId, int userId)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                suzlonBPPEntities.MenuAccesses.RemoveRange(suzlonBPPEntities.MenuAccesses.Where(m => m.ProfileId == profileId).ToList());
                suzlonBPPEntities.SaveChanges();
            }
        }

        /// <summary>
        /// This method used to check for duplicate profile.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="profileId"></param>
        /// <returns></returns>
        //public bool ProfileExist(string name,int profileId)
        //{
        //    using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
        //    {
        //        return suzlonBPPEntities.ProfileMasters.FirstOrDefault(l => l.ProfileName.ToLower() == name.ToLower() && l.ProfileId != profileId) != null;
        //    }
        //}
        public bool ProfileExist(ProfileModel profileModel)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                //return suzlonBPPEntities.ProfileMasters.FirstOrDefault(l => l.ProfileName.ToLower() == name.ToLower() && l.ProfileId != profileId) != null;
                return suzlonBPPEntities.ProfileMasters.FirstOrDefault(l => l.ProfileName.ToLower() == profileModel.ProfileName.ToLower() && l.ProfileId != profileModel.ProfileId) != null;
            }
        }
        public List<Menu> GetAllMenuList()
        {
            List<Menu> menues = new List<Menu>();           

            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                suzlonBPPEntities.Configuration.ProxyCreationEnabled = false;
                menues = suzlonBPPEntities.Menus.ToList();
            }


                return menues;
        }

        #endregion "Public Methods"
    }
}
