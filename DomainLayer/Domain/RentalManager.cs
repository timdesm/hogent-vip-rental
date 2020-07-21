using System;
using System.Collections.Generic;
using System.Text;

namespace DomainLayer.Domain
{
    public class RentalManager
    {
        private IUnitOfWork uow;

        public RentalManager(IUnitOfWork uow)
        {
            this.uow = uow;
        }
    }
}
