using AppBackend.DataAccessLayer.Abstract;
using AppBackend.DataAccessLayer.Concrete;
using AppBackend.DataAccessLayer.Repositories;
using AppBackend.EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBackend.DataAccessLayer.EntityFramework
{
    public class EfAnswersDal : GenericRepository<Answers>, IAnswersDal
    {
        public EfAnswersDal(Context context) : base(context)
        {
        }


    }
}
