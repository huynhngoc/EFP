using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ApiAiSDK;
using DataService;
using DataService.Service;
using ShopManager.Api.Ai.Custom;

namespace ShopManager.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        ShopService shopService = new ShopService();
        IntentService intentService = new IntentService();
        AIDataServiceCustom apiService = new AIDataServiceCustom(
                new AIConfigurationCustom(ConfigurationManager.AppSettings["ApiAiDeveloper"], SupportedLanguage.English, "intents"));
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ShopUser()
        {
            ViewBag.Shops = shopService.GetAll();
            return View();
        }

        public JsonResult SetActive(string shopId, string userId, bool isActive)
        {
            return Json(shopService.SetActive(shopId, userId, isActive), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ApiAiSetting()
        {
            var intents = intentService.GetAllIntent();
            List<IntentWithTemplates> list = new List<IntentWithTemplates>();
            foreach (var intent in intents)
            {
                if (!string.IsNullOrEmpty(intent.ApiAiId))
                {
                    AiIntent aiIntent = apiService.RequestIntentGet(intent.ApiAiId);
                    list.Add(new IntentWithTemplates()
                    {
                        Intent = intent,
                        Templates = aiIntent.Templates
                    });
                }
                else
                {
                    list.Add(new IntentWithTemplates()
                    {
                        Intent = intent,
                        Templates = null
                    });
                }
            }
            ViewBag.Intents = list;
            return View();
        }

        public JsonResult RemoveTemplate(string apiAiId, string content)
        {
            try
            {
                var intent = apiService.RequestIntentGet(apiAiId);
                intent.RemoveTemplate(content);
                var result = apiService.RequestIntentPut(intent, apiAiId);
                if (!result.IsError)
                {
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult AddTemplate(string apiAiId, string content)
        {
            try
            {
                var intent = apiService.RequestIntentGet(apiAiId);
                intent.SetMoreTemplates(content);
                var result = apiService.RequestIntentPut(intent, apiAiId);
                if (!result.IsError)
                {
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult AddIntent(string name)
        {
            Intent intent = intentService.AddIntent(name);
            if (intent != null)
            {
                try
                {
                    var result = apiService.RequestIntentPost(new AiIntent(intent.Id.ToString()));
                    if (!result.IsError)
                    {
                        intent.ApiAiId = result.Id;
                        bool update = intentService.UpdateIntent(intent);
                        if (update)
                        {
                            return Json(new
                            {
                                success = true,
                                id = intent.Id,
                                apiAiId = intent.ApiAiId
                            },
                                JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            intentService.DeleteIntent(intent.Id);
                            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        intentService.DeleteIntent(intent.Id);
                        return Json(new { success = false }, JsonRequestBehavior.AllowGet);
                    }

                }
                catch (Exception)
                {
                    intentService.DeleteIntent(intent.Id);
                    return Json(new { success = false }, JsonRequestBehavior.AllowGet);
                }

            }
            else
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult DeleteIntent(int id, string apiAiId)
        {
            try
            {
                intentService.DeleteIntent(id);
                apiService.RequestIntentDelete(apiAiId);
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
            
        }
    }


    public class IntentWithTemplates
    {
        public Intent Intent { get; set; }
        public List<string> Templates { get; set; }
    }
}