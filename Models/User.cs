using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace new_pages.Models
{
    [Table("a_users_system")]
    public class User
    {
        [Key]
        public string hdrid { get; set; }
        public string? role_id { get; set; }
        public string? role_name { get; set; }
        public string? nik { get; set; }
        public string? username { get; set; }
        public string? office_email { get; set; }
        public string? kode_department { get; set; }
        public string? nama_department { get; set; }
    }
}