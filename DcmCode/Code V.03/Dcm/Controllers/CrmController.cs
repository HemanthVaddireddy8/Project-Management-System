﻿using Dcm.Models;
using Dcm.Source;
using Gunluk.EntityModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dcm.EntityModels;
using BaseDB;
using Dcm.Filters;
using System.Web.Script.Serialization;
using BaseClasses;

namespace Dcm.Controllers
{
    public class CrmController : Controller
    {

        #region Kurum İşlemleri
        [HttpGet]
        [AllowAnonymous]
        [SessionCheckAttribute]
        public ActionResult Kurumlar(string RecordId,string MenuId )
        {
            CrmRepository crmDB = RepositoryManager.GetRepository<CrmRepository>();
            Kurum model = new Kurum();

            #region Ortak Set Edilecek Değerler 
            MenuId = GlobalHelper.Decrypt(MenuId);
            model.RecordId = RecordId;
            model.MenuId = MenuId;
            SessionContext.Current.ActiveUser.MenuId = MenuId;
            #endregion

            Guid recordId = Guid.Empty;


            if (GlobalHelper.IsGuid(model.RecordId))
            {
                try
                {
                    recordId = Guid.Parse(model.RecordId);
                    model = crmDB.BindKurum(model, recordId);
                    ViewBag.ResultMessage = Resources.GlobalResource.transaction_success;
                }
                catch (Exception exp)
                {
                    ViewBag.Success = false;
                    ModelState.AddModelError("Error", exp.Message);
                }
            }

            return View(model);
        }

        [HttpPost]
        [SessionCheckAttribute]
        public ActionResult Kurumlar(Kurum model)
        {
            Guid recordId = Guid.Empty;
            CrmRepository crmDB = RepositoryManager.GetRepository<CrmRepository>();

            #region Ortak Set Edilecek Değerler
            SessionContext.Current.ActiveUser.MenuId = model.MenuId;
            ViewBag.Success = true;
            #endregion

            ModelState.Remove("is_active");

            if (model.FromDeleteButton == "1")
            {
                if (GlobalHelper.IsGuid(model.RecordId))
                {
                    crmDB.DeleteKurum(model, Guid.Parse(model.RecordId));
                    return RedirectToAction("ListPage", "General", new { MenuId =Dcm.Source.GlobalHelper.Encrypt( model.MenuId )});
                }
            }
            else
            {
                if (ModelState.IsValid)
                {
                    if (GlobalHelper.IsGuid(model.RecordId))
                    {
                        recordId = Guid.Parse(model.RecordId);
                        try
                        {
                            model = crmDB.UpdateKurum(model, recordId);
                            ViewBag.ResultMessage = Resources.GlobalResource.transaction_success;
                        }
                        catch (Exception exp)
                        {
                            ViewBag.Success = false;
                            ModelState.AddModelError("Error", exp.Message);
                        }
                    }
                    else
                    {
                        try
                        {
                            crm_kurumlar kurum = new crm_kurumlar();
                            crmDB.AddKurum(kurum, model);
                            model.RecordId = kurum.kurum_id.ToString();
                            ViewBag.ResultMessage = Resources.GlobalResource.transaction_success;
                        }
                        catch (Exception exp)
                        {
                            ViewBag.Success = false;
                            ModelState.AddModelError("Error", exp.Message);
                        }
                    }
                }
                else
                {
                    ViewBag.Success = false;
                }
            }

            return View(model);
        }
        #endregion

        #region Birey İşlemleri
        [HttpGet]
        [AllowAnonymous]
        [SessionCheckAttribute]
        public ActionResult Bireyler(string RecordId, string MenuId)
        {
            CrmRepository crmDB = RepositoryManager.GetRepository<CrmRepository>();
            Birey model = new Birey();

            #region Ortak Set Edilecek Değerler
            MenuId = GlobalHelper.Decrypt(MenuId);
            model.RecordId = RecordId;
            model.MenuId = MenuId;
            SessionContext.Current.ActiveUser.MenuId = MenuId;
            #endregion

            Guid recordId = Guid.Empty;

            model.activeKurumlar = crmDB.GetKurumList();
            

            if (GlobalHelper.IsGuid(model.RecordId))
            {
                try
                {
                    recordId = Guid.Parse(model.RecordId);
                    model = crmDB.BindBirey(model, recordId);
                    if (model.calistigi_kurum_id != null && GlobalHelper.IsGuidOrEmpty(model.calistigi_kurum_id))
                        model.calistigi_kurum_id = model.calistigi_kurum_id.ToString();
                    ViewBag.ResultMessage = Resources.GlobalResource.transaction_success;
                }
                catch (Exception exp)
                {
                    ViewBag.Success = false;
                    ModelState.AddModelError("Error", exp.Message);
                }
            }

            return View(model);
        }

        [HttpPost]
        [SessionCheckAttribute]
        public ActionResult Bireyler(Birey model)
        {
            Guid recordId = Guid.Empty;
            CrmRepository crmDB = RepositoryManager.GetRepository<CrmRepository>();

            #region Ortak Set Edilecek Değerler
            SessionContext.Current.ActiveUser.MenuId = model.MenuId;
            ViewBag.Success = true;
            #endregion

            model.activeKurumlar = crmDB.GetKurumList();
            
            ModelState.Remove("is_active");

            if (model.FromDeleteButton == "1")
            {
                if (GlobalHelper.IsGuid(model.RecordId))
                {
                    crmDB.DeleteBirey(model, Guid.Parse(model.RecordId));
                    return RedirectToAction("ListPage", "General", new { MenuId = Dcm.Source.GlobalHelper.Encrypt(model.MenuId) });
                }
            }
            else
            {
                if (ModelState.IsValid)
                {
                    if (GlobalHelper.IsGuid(model.RecordId))
                    {
                        recordId = Guid.Parse(model.RecordId);
                        try
                        {
                            model = crmDB.UpdateBirey(model, recordId);

                            if (model.calistigi_kurum_id != null && GlobalHelper.IsGuidOrEmpty(model.calistigi_kurum_id))
                                model.calistigi_kurum_id = model.calistigi_kurum_id.ToString();
                            
                            ViewBag.ResultMessage = Resources.GlobalResource.transaction_success;
                        }
                        catch (Exception exp)
                        {
                            ViewBag.Success = false;
                            ModelState.AddModelError("Error", exp.Message);
                        }
                    }
                    else
                    {
                        try
                        {
                            crm_bireyler birey = new crm_bireyler();
                            crmDB.AddBirey(birey, model);
                            model.RecordId = birey.birey_id.ToString();
                            if (birey.calistigi_kurum_id != null)
                                model.calistigi_kurum_id = birey.calistigi_kurum_id.ToString();

                            ViewBag.ResultMessage = Resources.GlobalResource.transaction_success;
                        }
                        catch (Exception exp)
                        {
                            ViewBag.Success = false;
                            ModelState.AddModelError("Error", exp.Message);
                        }
                    }
                }
                else
                {
                    ViewBag.Success = false;
                }
            }

            return View(model);
        }
        #endregion

        #region Proje İşlemleri
        [HttpGet]
        [AllowAnonymous]
        [SessionCheckAttribute]
        public ActionResult Projects(string RecordId, string MenuId)
        {
            CrmRepository crmDB = RepositoryManager.GetRepository<CrmRepository>();
            GenelRepository gnlDB = RepositoryManager.GetRepository<GenelRepository>();

            Project model = new Project();

            #region Ortak Set Edilecek Değerler
            MenuId = GlobalHelper.Decrypt(MenuId);
            model.RecordId = RecordId;
            model.MenuId = MenuId;
            SessionContext.Current.ActiveUser.MenuId = MenuId;
            #endregion

            Guid recordId = Guid.Empty;

            model.activeKurumlar = crmDB.GetKurumList();
            model.activeBireyler = crmDB.GetBireyList();
            model.activeBireylerKurumlar = crmDB.GetBireyKurumList();
            model.activeUsers = gnlDB.GetActiveUsers();
            model.projectTypes = crmDB.GetProjectTypeList();

            model.kurum_id = Guid.Empty.ToString();
            model.birey_id = Guid.Empty.ToString();
            model.project_type_id = "1";

            if (GlobalHelper.IsGuid(model.RecordId))
            {
                try
                {
                    recordId = Guid.Parse(model.RecordId);
                    model = crmDB.BindProject(model, recordId);
                    
                    if (model.birey_id != null && GlobalHelper.IsGuidOrEmpty(model.birey_id))
                        model.birey_id = model.birey_id.ToString();

                    if (model.kurum_id != null && GlobalHelper.IsGuidOrEmpty(model.kurum_id))
                        model.kurum_id = model.kurum_id.ToString();


                    if (model.statik != null && GlobalHelper.IsGuidOrEmpty(model.statik))
                        model.statik = model.statik.ToString();

                    if (model.mekanik != null && GlobalHelper.IsGuidOrEmpty(model.mekanik))
                        model.mekanik = model.mekanik.ToString();

                    if (model.elektrik != null && GlobalHelper.IsGuidOrEmpty(model.elektrik))
                        model.elektrik = model.elektrik.ToString();

                    if (model.harita != null && GlobalHelper.IsGuidOrEmpty(model.harita))
                        model.harita = model.harita.ToString();

                    if (model.yapi_denetim != null && GlobalHelper.IsGuidOrEmpty(model.yapi_denetim))
                        model.yapi_denetim = model.yapi_denetim.ToString();

                    if (model.isveren_mobil_phone != null)
                        model.isveren_mobil_phone = model.isveren_mobil_phone.ToString();

                    if (model.proje_metrekare != null && !string.IsNullOrEmpty(model.proje_metrekare.ToString()))
                    {
                        model.proje_metrekare = model.proje_metrekare;
                        model.proje_metrekare = model.proje_metrekare.Replace(',', '.');
                    }

                    ViewBag.ResultMessage = Resources.GlobalResource.transaction_success;
                }
                catch (Exception exp)
                {
                    ViewBag.Success = false;
                    ModelState.AddModelError("Error", exp.Message);
                }
            }

            return View(model);
        }

        [HttpPost]
        [SessionCheckAttribute]
        public ActionResult Projects(Project model)
        {
            Guid recordId = Guid.Empty;
            CrmRepository crmDB = RepositoryManager.GetRepository<CrmRepository>();
            GenelRepository gnlDB = RepositoryManager.GetRepository<GenelRepository>();

            #region Ortak Set Edilecek Değerler
            SessionContext.Current.ActiveUser.MenuId = model.MenuId;
            ViewBag.Success = true;
            #endregion

            model.activeKurumlar = crmDB.GetKurumList();
            model.activeBireyler = crmDB.GetBireyList();
            model.activeUsers = gnlDB.GetActiveUsers();
            model.projectTypes = crmDB.GetProjectTypeList();
            model.activeBireylerKurumlar = crmDB.GetBireyKurumList();
            model.project_type_id = "1";

            ModelState.Remove("is_active");

            if (model.FromDeleteButton == "1")
            {
                if (GlobalHelper.IsGuid(model.RecordId))
                {
                    crmDB.DeleteProject(model, Guid.Parse(model.RecordId));
                    return RedirectToAction("ListPage", "General", new { MenuId = Dcm.Source.GlobalHelper.Encrypt(model.MenuId) });
                }
            }
            else
            {
                if (ModelState.IsValid)
                {
                    if (GlobalHelper.IsGuid(model.RecordId))
                    {
                        recordId = Guid.Parse(model.RecordId);
                        try
                        {
                            model = crmDB.UpdateProject(model, recordId);

                            if (model.birey_id != null && GlobalHelper.IsGuidOrEmpty(model.birey_id))
                                model.birey_id = model.birey_id.ToString();

                            if (model.kurum_id != null && GlobalHelper.IsGuidOrEmpty(model.kurum_id))
                                model.kurum_id = model.kurum_id.ToString();


                            if (model.statik != null && GlobalHelper.IsGuidOrEmpty(model.statik))
                                model.statik = model.statik.ToString();

                            if (model.mekanik != null && GlobalHelper.IsGuidOrEmpty(model.mekanik))
                                model.mekanik = model.mekanik.ToString();

                            if (model.elektrik != null && GlobalHelper.IsGuidOrEmpty(model.elektrik))
                                model.elektrik = model.elektrik.ToString();

                            if (model.harita != null && GlobalHelper.IsGuidOrEmpty(model.harita))
                                model.harita = model.harita.ToString();

                            if (model.yapi_denetim != null && GlobalHelper.IsGuidOrEmpty(model.yapi_denetim))
                                model.yapi_denetim = model.yapi_denetim.ToString();

                            if (model.isveren_mobil_phone != null)
                                model.isveren_mobil_phone = model.isveren_mobil_phone.ToString();

                            if (model.proje_metrekare != null && !string.IsNullOrEmpty(model.proje_metrekare.ToString()))
                                model.proje_metrekare = model.proje_metrekare.ToString();



                            ViewBag.ResultMessage = Resources.GlobalResource.transaction_success;
                        }
                        catch (Exception exp)
                        {
                            ViewBag.Success = false;
                            ModelState.AddModelError("Error", exp.Message);
                        }
                    }
                    else
                    {
                        try
                        {
                            crm_projects project = new crm_projects();
                            crmDB.AddProject(project, model);
                            model.RecordId = project.project_id.ToString();

                            if (project.birey_id != null)
                                model.birey_id = project.birey_id.ToString();

                            if (project.kurum_id != null)
                                model.kurum_id = project.kurum_id.ToString();

                            if (project.statik != null)
                                model.statik = project.statik.ToString();

                            if (project.mekanik != null )
                                model.mekanik = project.mekanik.ToString();

                            if (project.elektrik != null )
                                model.elektrik = project.elektrik.ToString();

                            if (project.harita != null)
                                model.harita = project.harita.ToString();

                            if (project.yapı_denetimi != null )
                                model.yapi_denetim = project.yapı_denetimi.ToString();

                            if (project.isveren_mobile_phone != null)
                                model.isveren_mobil_phone = project.isveren_mobile_phone.ToString();

                            if (project.metre_kare != null && !string.IsNullOrEmpty(model.proje_metrekare.ToString()))
                            {
                                model.proje_metrekare = project.metre_kare.ToString();
                                model.proje_metrekare = model.proje_metrekare.Replace(',', '.');
                            }
                            
                            ViewBag.ResultMessage = Resources.GlobalResource.transaction_success;
                        }
                        catch (Exception exp)
                        {
                            ViewBag.Success = false;
                            ModelState.AddModelError("Error", exp.Message);
                        }
                    }
                }
                else
                {
                    ViewBag.Success = false;
                }
            }

            return View(model);
        }
        #endregion
     

    }
}