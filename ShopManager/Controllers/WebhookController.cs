using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace ShopManager.Controllers
{
    public class WebhookController : Controller
    {
        // GET: Webhook
        public ActionResult Index()
        {
            return View();
        }

        public string Subscribe()
        {
            string param1 = this.Request.QueryString["hub.challenge"];
            string param2 = this.Request.QueryString["hub.mode"];
            string param3 = this.Request.QueryString["hub.verify_token"];

            return param1;
        }

        [HttpPost]
        [ActionName("Subscribe")]
        public void SubscribeData()
        {
            String jsonData = Regex.Unescape(new StreamReader(this.Request.InputStream, this.Request.ContentEncoding).ReadToEnd());
            System.Diagnostics.Debug.WriteLine(jsonData);
            
            dynamic fbJson = System.Web.Helpers.Json.Decode(jsonData);

            System.Diagnostics.Debug.WriteLine("header");
            System.Diagnostics.Debug.WriteLine(this.Request.Headers["X-Hub-Signature"]);

            var reader = new System.IO.StreamReader(this.HttpContext.Request.InputStream);
            System.Diagnostics.Debug.WriteLine(reader.ReadToEnd());
            return;
        }

        private void ProcessItem(dynamic fbObj)
        {
            dynamic obj = fbObj["object"];
            dynamic entries = fbObj.entry;
            foreach (dynamic entry in entries)
            {
                string shopId = entry.id;
                string time = entry.time;
                dynamic changes = entry.changes;
                foreach(dynamic change in changes)
                {
                    string field = change.field;
                    dynamic value = change.value;
                }
            }


        }
    }
}