using DataService.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Service
{
    public class IntentService
    {
        IntentRepository intentRepository = new IntentRepository();

        public List<Intent> GetAllIntent()
        {
            List<Intent> listIntent = intentRepository.GetAllIntent().ToList();
            return listIntent;
        }

        public string GetIntentNameById(int? id)
        {
            string intentName = intentRepository.GetIntentNameById(id);
            return intentName;
        }

        public Intent AddIntent(string name)
        {
            return intentRepository.CreateNew(new Intent()
            {
                IntentName = name
            });
        }

        public bool UpdateIntent(Intent intent)
        {
            return intentRepository.Update(intent);
        }

        public bool DeleteIntent(int id)
        {
            return intentRepository.Delete(id);
        }

        public Intent GetIntent(int id)
        {
            return intentRepository.FindByKey(id);
        }
    }
}
