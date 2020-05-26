using System;
using System.ComponentModel.DataAnnotations.Schema;
using quiz_oj.Entities.User;

namespace quiz_oj.Entities.Review
{
    [Table("OjReview")]
    public class OjReview
    {
        [Column("ojId")]
        public string OjId { get; set; }
        [Column("userId")]
        public string UserId { get; set; }
        [Column("review")]
        public string Review { get; set; }

        [Column("createTime")]
        public DateTime CreateTime { get; set; }
        
        public UserInfo UserInfo { get; set; }
    }
}