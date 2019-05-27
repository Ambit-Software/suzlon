using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
namespace SolarPMS.Models
{
    public class EscalationMatrixModel
    {    
        public EscalationMatrix GetEscaltionMatrix()
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                solarPMSEntities.Configuration.ProxyCreationEnabled = false;  
                return solarPMSEntities.EscalationMatrices.FirstOrDefault();               
            }
        }


        public EscalationMatrix AddEscaltion(EscalationMatrix EscalationMatrix, int userId)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                EscalationMatrix.CreatedBy = userId;
                EscalationMatrix.CreatedDate = DateTime.Now;
                EscalationMatrix.ModifiedBy = userId;
                EscalationMatrix.ModifiedDate = DateTime.Now;
                solarPMSEntities.EscalationMatrices.Add(EscalationMatrix);
                solarPMSEntities.SaveChanges();
                return EscalationMatrix;               
            }
        }
        public bool UpdateEscalation(EscalationMatrix EscalationMatrix, int userId)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                EscalationMatrix escalation = solarPMSEntities.EscalationMatrices.FirstOrDefault(e => e.EscalationMatrixId ==EscalationMatrix.EscalationMatrixId);              
                if (escalation != null)
                {                 
                    escalation.ActivityTakingLongerThanPlanned = EscalationMatrix.ActivityTakingLongerThanPlanned;
                    escalation.IssueNotResolved = EscalationMatrix.IssueNotResolved;
                    escalation.IssueNotClosed = EscalationMatrix.IssueNotClosed;
                    escalation.QualityIssueNotResolved = EscalationMatrix.QualityIssueNotResolved;
                    escalation.QualityRejectionNotClosed = EscalationMatrix.QualityRejectionNotClosed;
                    escalation.CreatedBy = userId;
                    escalation.CreatedDate = DateTime.Now;
                    escalation.ModifiedBy = userId;
                    escalation.ModifiedDate = DateTime.Now;
                    solarPMSEntities.Entry(escalation).State = EntityState.Modified;
                    solarPMSEntities.SaveChanges();
                    return true;
                }
                else
                    return false;
            }
        }

    }
}