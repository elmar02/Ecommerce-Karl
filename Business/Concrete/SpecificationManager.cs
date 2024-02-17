using Business.Abstract;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResults;
using Core.Utilities.Results.Concrete.SuccessResults;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class SpecificationManager : ISpecificationService
    {
        private readonly ISpecificationDAL _specificationDAL;

        public SpecificationManager(ISpecificationDAL specificationDAL)
        {
            _specificationDAL = specificationDAL;
        }

        public IResult AddSpecifications(Dictionary<string, string> specifications, int productId)
        {
            try
            {
                var newSpec = specifications.Select(x => new Specification()
                {
                    Key = x.Key,
                    Value = x.Value,
                    ProductId = productId
                });
                _specificationDAL.AddRange(newSpec);
                return new SuccessResult();
            }
            catch (Exception ex)
            {
                return new ErrorResult(ex.Message);
            }
        }
    }
}
