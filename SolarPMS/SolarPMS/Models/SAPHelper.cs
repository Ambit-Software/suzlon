using SAP.Connector;
using System;
using System.Configuration;

namespace SolarPMS.Models
{
    public class SAPHelper
    {
        private SAPConnection connection = null;
        public SAPHelper()
        {
            //
            // TODO: Add constructor logic here
            //
        }


        public SAPConnection GetSAPConnection()
        {
            try
            {
                Destination destination = new Destination();
                destination.AppServerHost = ConfigurationManager.AppSettings["SAPAppServerHost"];
                destination.Client = Convert.ToInt16(ConfigurationManager.AppSettings["SAPClient"]);
                destination.Username = ConfigurationManager.AppSettings["SAPUsername"];
                destination.Password = ConfigurationManager.AppSettings["SAPPassword"];
                destination.SystemNumber = Convert.ToInt16(ConfigurationManager.AppSettings["SAPSystemNumber"]);
                connection = new SAPConnection(destination);
                return connection;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}