using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts;

namespace Logic
{
    public class UnitOfWork
    {
        public IUow Uow { get; set; }
    }
}
