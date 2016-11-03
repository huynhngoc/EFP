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

        public string GetIntentNameById(int id)
        {
            string intentName = intentRepository.GetIntentNameById(id);
            return intentName;
        }

    }
}
