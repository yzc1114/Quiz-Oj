using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Castle.Core.Internal;
using Microsoft.EntityFrameworkCore.Internal;

namespace quiz_oj.Dao.impls
{
    public class DaoUtils
    {
        private QOJDBContext dbContext;
        
        public DaoUtils(QOJDBContext qojdbContext)
        {
            dbContext = qojdbContext;
        }

        public string GUID()
        {
            return Guid.NewGuid().ToString("N");
        }
    }
}