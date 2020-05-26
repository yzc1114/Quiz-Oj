using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using quiz_oj.Entities.User;

namespace quiz_oj.Entities.OJ
{
    [Table("OjSuccessCount")]
    public class OjSuccessCount
    {
        [Key]
        [Column("userId")]
        public string UserId { get; set; }
        [Column("successOjCount")]
        public int Count { get; set; }
        
        public UserInfo UserInfo { get; set; }
    }
}