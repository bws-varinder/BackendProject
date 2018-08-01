using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlDapper
{
    public abstract class DapperRepository
    {
        protected IDatabasecontext databaseContext { get; private set; }

        protected DapperRepository(IDatabasecontext databaseContext)
        {
            if (databaseContext == null)
            {
                throw new ArgumentNullException("databaseContext");
            }
            this.databaseContext = databaseContext;
        }

        protected DapperRepository()
        {

        }

    }
}
