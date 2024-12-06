using Model;
using Model.DataModel;
using Repositories.IRepo;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repositories
{
  
    public class CapteurRepository : GenericRepository<Capteur>, ICapteurRepository
    {
        public CapteurRepository(ContextProject dbContext) : base(dbContext)
        {

        }
    }
}
