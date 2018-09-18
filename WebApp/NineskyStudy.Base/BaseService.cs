using NineskyStudy.InterfaceBase;
using NineskyStudy.InterfaceDataLibrary;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace NineskyStudy.Base
{
    public class BaseService<T> : InterfaceBaseService<T> where T : class
    {
        private InterfaceBaseRepository<T> _interfaceBaseRepository;
        public BaseService(InterfaceBaseRepository<T> interfaceBaseRepository)
        {
            _interfaceBaseRepository = interfaceBaseRepository;
        }
        public T Find(Expression<Func<T, bool>> predicate)
        {
            return _interfaceBaseRepository.Find(predicate);
        }

        public T Find(string[] includeParams, Expression<Func<T, bool>> predicate)
        {
            return _interfaceBaseRepository.Find(includeParams, predicate);
        }
    }
}
