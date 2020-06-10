using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Dcm.Models
{
    public class Kurum
    {
        [Required]
        public string RecordId { get; set; }
        public string MenuId { get; set; }

        [Required(ErrorMessage = "Company Code is required.")]
        public string kodu { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        public string ticari_unvan { get; set; }
        public string vergino { get; set; }
        public string vergi_dairesi { get; set; }
        public int kurulus_yili { get; set; }
        public int calisan_sayisi { get; set; }
        public string eposta { get; set; }
        [Required(ErrorMessage = "Phone is required.")]
        public string telefon1 { get; set; }
        public string telefon2 { get; set; }
        public string mobile_phone { get; set; }
        public string faks1 { get; set; }
        public string faks2 { get; set; }
        public string adres { get; set; }

        public string note { get; set; }
        public string web_sitesi { get; set; }

        public bool is_active { get; set; }

        public string FromDeleteButton { get; set; }
    }
}