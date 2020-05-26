using System.ComponentModel.DataAnnotations.Schema;

namespace quiz_oj.Entities.OJ
{
    [Table("OjSuccess")]
    public class OjSuccess
    {
        [Column("userId")]
        public string UserId { get; set; }
        [Column("ojId")]
        public string OjQuestionId { get; set; }
    }
}