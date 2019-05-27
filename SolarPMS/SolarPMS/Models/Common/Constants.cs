using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SolarPMS.Models.Common
{
    public class Constants
    {

        public const string CONST_NEW_MODE = "New";
        public const string CONST_EDIT_MODE = "Edit";

        #region "Grid Constants"

        public const int CONST_GRID_PAGE_SIZE = 15;
        public const string CONST_GRID_RECORD_DETAILS = "records matching your search criteria";
        public const string CONST_GRID_RECORD_PER_PAGE = "Records per page:";

        public const string CONST_ERROR_MAX_150_CHARACTERS = "Maximum 150 characters allowed.";
        public const string CONST_ERROR_MAX_135_CHARACTERS = "Maximum 135 characters allowed.";




        #endregion

        #region "Error Messages"

        public const string CONST_RECORD_ALREADY_EXIST = "Record already exists";
        
        #endregion
    }
}