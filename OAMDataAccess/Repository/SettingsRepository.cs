using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAMDataAccess.Repository
{
    public class SettingsRepository
    {
        OAMEntities db = new OAMEntities();

        public Settings GetById(string id) 
        {
            return db.Settings.Where(c => c.SettingId == id).FirstOrDefault();
        }
    }
}
