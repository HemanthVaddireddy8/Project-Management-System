//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Dcm.EntityModels
{
    using System;
    using System.Collections.Generic;
    
    public partial class gnl_menu
    {
        public int menu_id { get; set; }
        public Nullable<int> parent_menu_id { get; set; }
        public string name { get; set; }
        public Nullable<int> menu_order { get; set; }
        public string primary_key { get; set; }
        public string table_name { get; set; }
        public string object_name { get; set; }
        public string hide_columns { get; set; }
        public string detail_page_url { get; set; }
        public Nullable<short> detail_update_button { get; set; }
        public Nullable<short> detail_delete_button { get; set; }
        public Nullable<short> list_detail_button { get; set; }
        public Nullable<short> list_new_record_button { get; set; }
        public Nullable<short> show_in_menu { get; set; }
        public string filter { get; set; }
        public string report_url { get; set; }
        public string only_detail_page { get; set; }
        public string icon_class { get; set; }
        public string show_columns { get; set; }
        public string order_by { get; set; }
    }
}
