using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace quiz_oj.Entities.OJ
{
    [Table("OJ")]
    public class OjQuestion
    {
        [Key]
        [Column("id")]
        public string Id { get; set; }
        [Column("title")]
        public string Title { get; set; }
        [Column("content")]
        public string Content { get; set; }
        
        [Column("difficulty")]
        public int Difficulty { get; set; }
        [Column("createTime")]
        public DateTime CreateTime { get; set; }
        [Column("code")]
        public string Code { get; set; }
        [Column("orderId")]
        public int? OrderId { get; set; }
        
        [NotMapped]
        public bool Passed { get; set; }
    }
}