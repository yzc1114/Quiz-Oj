using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using quiz_oj.Entities.OJ;

namespace quiz_oj.Entities.User
{
    [Table("UserInfo")]
    public class UserInfo
    {
        [Column("name")]
        public string UserName { get; set; }
        [Column("id")]
        [Key]
        public string Id { get; set; }
        [Column("pwd")]
        public string Pwd { get; set; }

        public void ConvertToPublic()
        {
            this.Id = null;
            this.Pwd = null;
        } 
        
        public OjSuccessCount OjSuccessCount { get; set; }
    }
}