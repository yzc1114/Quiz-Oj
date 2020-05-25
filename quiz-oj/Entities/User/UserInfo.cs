using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace quiz_oj.Entities
{
    [Table("UserInfo")]
    public class UserInfo
    {
        [Column("name")]
        public string UserName { get; set; }
        [Column("id")]
        [Key]
        public int Id { get; set; }
        [Column("pwd")]
        public string Pwd { get; set; }

        public void ConvertToPublic()
        {
            this.Id = -1;
            this.Pwd = null;
        } 
    }
}