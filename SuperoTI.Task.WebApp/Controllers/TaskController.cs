using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SuperoTI.Task.DAO;
using SuperoTI.Task.WebApp.Services;
using SuperoTI.Task.DAO.Enums;
using System.Web.UI.WebControls;
using SuperoTI.Task.WebApp.Models;

namespace SuperoTI.Task.WebApp.Controllers
{
    public class TaskController : Controller
    {
        private APICallService APICallService = new APICallService();

        public async Task<ActionResult> Index()
        {
            return View(await APICallService.GetListAsync<TaskModel>("Task", null));
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,InsertDate,UpdateDate,Title,Description,Status")] TaskModel taskModel)
        {
            if (ModelState.IsValid)
            {
                taskModel.InsertDate = DateTime.Now;
                taskModel.Status = (int)TaskStatusEnum.Opened;
                string error = await APICallService.PostAsync<TaskModel>("Task", taskModel);
                if (!String.IsNullOrEmpty(error))
                {
                    ModelState.AddModelError("", error);
                    return View(taskModel);
                }
                return RedirectToAction("Index");
            }

            return View(taskModel);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaskModel taskModel = await APICallService.GetAsync<TaskModel>("Task", "id=" + id);
            if (taskModel == null)
            {
                return HttpNotFound();
            }
            ViewBag.Status = this.GetStatusDropDownList(taskModel.Status);
            return View(taskModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,InsertDate,UpdateDate,Title,Description,Status")] TaskModel taskModel)
        {
            if (ModelState.IsValid)
            {
                // Recarrega dropdownlist em casa de dar erro
                ViewBag.Status = this.GetStatusDropDownList(taskModel.Status);

                taskModel.UpdateDate = DateTime.Now;
                string error = await APICallService.PutAsync<TaskModel>("Task", "id=" + taskModel.ID, taskModel);
                if (!String.IsNullOrEmpty(error))
                {
                    ModelState.AddModelError("", error);
                    return View(taskModel);
                }
                return RedirectToAction("Index");
            }
            return View(taskModel);
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaskModel taskModel = await APICallService.GetAsync<TaskModel>("Task", "id=" + id);
            if (taskModel == null)
            {
                return HttpNotFound();
            }
            return View(taskModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            TaskModel taskModel = await APICallService.GetAsync<TaskModel>("Task", "id=" + id);
            taskModel.Status = (int)TaskStatusEnum.Deleted;
            taskModel.UpdateDate = DateTime.Now;
            string error = await APICallService.PutAsync<TaskModel>("Task", "id=" + taskModel.ID, taskModel);
            if (!String.IsNullOrEmpty(error))
            {
                ModelState.AddModelError("", error);
                return View(taskModel);
            }
            return RedirectToAction("Index");
        }

        public SelectList GetStatusDropDownList(int status)
        {
            List<DropDownListModel> ddlList = new List<DropDownListModel>();
            ddlList.Add(new DropDownListModel() { Code = "1", Description = "Aberto" });
            ddlList.Add(new DropDownListModel() { Code = "0", Description = "Fechado" });

            return new SelectList(ddlList, "Code", "Description", status);
        }
    }
}
