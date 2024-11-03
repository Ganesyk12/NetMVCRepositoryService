
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace new_pages.Models
{
    [Table("user_role")]
    public class Role 
    {
        [Key]
        public string role_id { get; set; }
        
        public string? role_name { get; set; }

        public string? description { get; set; }
    }

    [Table("department")]
    public class Dept
    {
        [Key]
        public string deptId { get; set; }
        public string? dept_name { get; set; }
    }
}