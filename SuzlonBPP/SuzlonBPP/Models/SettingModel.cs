using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SuzlonBPP.Models
{
    public class SettingModel
    {
        public ApplicationConfiguration GetApplSettings()
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                suzlonBPPEntities.Configuration.ProxyCreationEnabled = false;
                return suzlonBPPEntities.ApplicationConfigurations.FirstOrDefault();
            }
        }

        public bool SaveApplSettings(ApplicationConfiguration appSettings, int userId)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                ApplicationConfiguration setting = suzlonBPPEntities.ApplicationConfigurations.FirstOrDefault();
                if (setting != null)
                {
                    setting.BudgetLimit = appSettings.BudgetLimit;
                    setting.Addendum = appSettings.Addendum;
                    setting.PaymentMethod = appSettings.PaymentMethod;
                    setting.DailyPaymentLimit = appSettings.DailyPaymentLimit;
                    setting.ModifiedBy = userId;
                    setting.ModifiedOn = DateTime.Now;
                    suzlonBPPEntities.SaveChanges();
                }
                else
                {
                    appSettings.CreatedBy = userId;
                    appSettings.CreatedOn = DateTime.Now;
                    appSettings.ModifiedBy = userId;
                    appSettings.ModifiedOn = DateTime.Now;
                    suzlonBPPEntities.ApplicationConfigurations.Add(appSettings);
                    suzlonBPPEntities.SaveChanges();
                }
                return true;
            }
        }
    }
}