using Dcm.EntityModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Dcm.Models
{
    public class Birey
    {
        [Required]
        public string RecordId { get; set; }
        public string MenuId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string ad { get; set; }

        [Required(ErrorMessage = "Surname is required")]
        public string soyad { get; set; }
        public string calistigi_kurum_id { get; set; }
        public string eposta_adresi { get; set; }
        public string telefonu_1 { get; set; }
        public string telefonu_2 { get; set; }

        [Required(ErrorMessage = "Mobile phone is required.")]
        public string mobile_phone { get; set; }
        public string faksi_1 { get; set; }
        public string faksi_2 { get; set; }
        public string adres { get; set; }

        public string note { get; set; }
        public string web_sitesi { get; set; }

        public bool is_active { get; set; }

        public string FromDeleteButton { get; set; }

        public List<crm_kurumlar> activeKurumlar { get; set; }


    }
}