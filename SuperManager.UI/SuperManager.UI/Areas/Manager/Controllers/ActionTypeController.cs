﻿using Helper.Core.Library;
using SuperManager.DAL;
using SuperManager.ENUM;
using SuperManager.FILTER;
using SuperManager.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SuperManager.UI.Areas.Manager.Controllers
{
    public class ActionTypeController : BaseManagerListController
    {
        [RoleMenuFilter]
        public ActionResult List(string searchKey = "", int pageIndex = 1)
        {
            searchKey = StringHelper.FilterSpecChar(searchKey);
            List<DBActionTypeModel> modelList = DALFactory.ActionType.Page(searchKey, pageIndex, ConfigHelper.ManagerPageSize, ref this.totalCount, ref this.pageCount);

            this.InitViewData(searchKey, pageIndex, Url.Action("List", new { PageIndex = -999, SearchKey = searchKey }));

            return View(modelList);
        }

        [RoleActionFilter]
        public ActionResult Add()
        {
            return this.Edit();
        }

        [RoleActionFilter]
        public ActionResult Edit(int identityID = 0)
        {
            return View("Edit", identityID > 0 ? DALFactory.ActionType.Select(identityID) : null);
        }

        [RoleActionFilter]
        public ActionResult DeleteOperater(int identityID)
        {
            return this.Delete(() =>
            {
                return DALFactory.ActionType.Delete(identityID);
            }, Url.Action("List"));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RoleActionFilter]
        public ActionResult AddOperater(DBActionTypeModel model)
        {
            return this.AddOrEditOperater(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RoleActionFilter]
        public ActionResult EditOperater(DBActionTypeModel model)
        {
            return this.AddOrEditOperater(model);
        }

        [HttpPost]
        [RoleActionFilter]
        public ActionResult MoreOperater()
        {
            return this.More((string identityIDList) =>
            {
                return DALFactory.ActionType.DeleteMore(identityIDList);
            }, null, Url.Action("List"));
        }

        [NonAction]
        private ActionResult AddOrEditOperater(DBActionTypeModel model)
        {
            return this.OperaterConfirm(() =>
            {
                return FilterFactory.ActionType.Operater(model);
            }, () =>
            {
                return DALFactory.ActionType.Exists(model.TypeCode, model.IdentityID);
            }, () =>
            {
                return DALFactory.ActionType.Operater(model);
            }, Url.Action("List"), model.IdentityID == 0 ? Url.Action("Add") : "", Url.Action("Edit", new { identityID = model.IdentityID }), "动作编号已存在！");
        }
    }
}