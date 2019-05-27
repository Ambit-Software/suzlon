using Cryptography;
using Newtonsoft.Json;
using SolarPMS.Filters;
using SolarPMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SolarPMS.Controllers
{
    [ExceptionHandlingFilter]
    [Authorize]
    [RoutePrefix("api/Task")]
    public class TaskController : BaseApiController
    {
        TaskModel taskModel = new TaskModel();

        [HttpGet]
        // GET: api/Location
        [Route("getallsite")]
        public IHttpActionResult GetAllSite()
        {
            return Ok(taskModel.GetAllSite());
        }

        [HttpGet]
        // GET: api/Location
        [Route("getallprojects")]
        public IHttpActionResult GetAllProjects(string sapsite)
        {
            return Ok(taskModel.GetAllProjects(sapsite));
        }

        [HttpGet]
        // GET: api/Location
        [Route("getallusers")]
        public List<ListItem> GetAllUsers()
        {
            return taskModel.GetAllUsers();
        }

        [HttpGet]
        // GET: api/Location
        [Route("getuserprofile")]
        public ProfileMaster GetUserProfileDetails(int UserId)
        {
            return taskModel.GetUserProfileDetails(UserId);
        }

        [HttpGet]
        // GET: api/Location
        [Route("get")]
        public IHttpActionResult GetTaskAllocationMasterData(string ProjectId, string SiteId, int UserId, int Flag, string AreaId = "", string NetworkId = "", string ActivityId = "")
        {
            return Ok(taskModel.GetTaskAllocationMasterData(ProjectId, SiteId, UserId, Flag, AreaId, NetworkId, ActivityId));
        }

        [HttpGet]
        // GET: api/Location
        [Route("save")]
        public void SaveTaskDetailsForUser(List<TaskAllocationData> lstSelectedTask, int userId, string siteId, string projectId, int loggedInUserId)
        {
            taskModel.SaveTaskDetails(lstSelectedTask, userId, siteId, projectId, loggedInUserId);
        }


        [HttpGet]
        // GET: api/Task
        [Route("getalocatedtask")]
        public IHttpActionResult GetAlocatedTask(string projectId, string sapSite)
        {
            return Ok(taskModel.GetAllocatedTaskActivity(projectId, sapSite));
        }

        [HttpGet]
        // GET: api/Task
        [Route("getnotalocatedtask")]
        public IHttpActionResult GetNotAlocatedTask(string projectId, string sapSite)
        {
            return Ok(taskModel.GetNotAlocatedTask(projectId, sapSite));
        }

        [Route("getmyrecords")]
        [HttpPost]
        public IHttpActionResult GetMyRecords(PostParam param)
        {
            var paramDetail = Crypto.Instance.Decrypt(param.Data);
            TaskModel.ToDoTask toDoTask = JsonConvert.DeserializeObject<TaskModel.ToDoTask>(paramDetail);
            return Ok(taskModel.GetMyRecords(toDoTask.userId, toDoTask.isFromMobile, toDoTask.network, toDoTask.area, toDoTask.project, toDoTask.site));
        }

        [Route("getpendingforapproval")]
        [HttpPost]
        public IHttpActionResult GetPendingForApproval(PostParam param)
        {
            var paramDetail = Crypto.Instance.Decrypt(param.Data);
            TaskModel.ToDoTask toDoTask = JsonConvert.DeserializeObject<TaskModel.ToDoTask>(paramDetail);
            return Ok(taskModel.GetPendingForApproval(toDoTask.userId, toDoTask.isFromMobile, toDoTask.network, toDoTask.area, toDoTask.project, toDoTask.site));
        }

        [Route("getapprovedlist")]
        [HttpPost]
        public IHttpActionResult GetToDoApprovedList(PostParam param)
        {
            var paramDetail = Crypto.Instance.Decrypt(param.Data);
            TaskModel.ToDoTask toDoTask = JsonConvert.DeserializeObject<TaskModel.ToDoTask>(paramDetail);
            return Ok(taskModel.GetToDoApprovedList(toDoTask.userId, toDoTask.network, toDoTask.area, toDoTask.project, toDoTask.site, toDoTask.isFromMobile));
        }

        [Route("getrejectedlist")]
        [HttpPost]
        public IHttpActionResult GetToDoRejectedList(PostParam param)
        {
            var paramDetail = Crypto.Instance.Decrypt(param.Data);
            TaskModel.ToDoTask toDoTask = JsonConvert.DeserializeObject<TaskModel.ToDoTask>(paramDetail);
            return Ok(taskModel.GetToDoRejectedList(toDoTask.userId, toDoTask.network, toDoTask.area, toDoTask.project, toDoTask.site, toDoTask.isFromMobile));
        }

        [Route("getallocatedsite")]
        [HttpGet]
        public IHttpActionResult GetAllocatedSite()
        {
            return Ok(taskModel.GetAllocatedSite(UserId));
        }

        [Route("getallocatedProject")]
        [HttpPost]
        public IHttpActionResult GetAllocatedProject(PostParam param)
        {
            var paramDetail = Crypto.Instance.Decrypt(param.Data);
            TaskModel.ToDoTask toDoTask = JsonConvert.DeserializeObject<TaskModel.ToDoTask>(paramDetail);
            return Ok(taskModel.GetAllocatedProject(UserId, toDoTask.site));
        }

        [Route("getallocatednetwok")]
        [HttpPost]

        public IHttpActionResult GetAllocatedNetwork(PostParam param)
        {

            var paramDetail = Crypto.Instance.Decrypt(param.Data);
            TaskModel.ToDoTask toDoTask = JsonConvert.DeserializeObject<TaskModel.ToDoTask>(paramDetail);
            return Ok(taskModel.GetAllocatedNetwork(UserId, toDoTask.site, toDoTask.project,toDoTask.areaId));
        }

        /// <summary>
        /// Get details according to task allocation with 'Admin'
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="Site"></param>
        /// <param name="Project"></param>
        /// <param name="Area"></param>
        /// <param name="FilterType"></param>
        /// <returns></returns>
        [Route("getallocateddetail")]
        [HttpGet]
        public IHttpActionResult GetUserTaskDropdownDataForReport(int UserId, string Site, string Project, string Area, string FilterType)
        {
            if (!string.IsNullOrEmpty(Area))
                Area = Crypto.Instance.Decrypt(Area);
            return Ok(CommonFunctions.GetUserTaskDropdownDataForReport(UserId, Site, Project, Area, FilterType, false));
        }
    }
}
