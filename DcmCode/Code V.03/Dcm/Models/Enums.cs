using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dcm.Models
{
    public class Enums
    {
        public enum TaskStatus
        {
            Bekliyor = 1,
            Gonderildi = 2,
            Kabul_Edildi = 3,
            Calismaya_Baslandi = 4,
            Reddedildi = 5,
            Tamamlandi = 6
        }

        public enum NotificationType
        {
            Normal = 1,
            Sms = 2,
            Mail = 3,
            Expire=99
        }

        public enum NotificationModuleType
        {
            Task = 1
        }
    }
}