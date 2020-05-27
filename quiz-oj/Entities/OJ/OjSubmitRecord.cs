using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace quiz_oj.Entities.OJ
{
    [Table("OjSubmitRecord")]
    public class OjSubmitRecord
    {
        [Column("userId")]
        public string UserId { get; set; }
        [Column("ojId")]
        public string OjId { get; set; }
        [Column("code")]
        public string Code { get; set; }
        [Column("createTime")]
        public DateTime CreateTime { get; set; }
        [Column("info")]
        public string Info { get; set; }
        
        [NotMapped]
        public string OjTitle { get; set; }
    }
}