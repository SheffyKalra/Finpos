using FinPos.Client.ServiceEndPoints.Interface;
using FinPos.DomainContracts.DataContracts;
using FinPos.WcfHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace FinPos.Client.Controllers
{
    public class TaxController
    {
        IServiceEndpoints objTaxService = new ServiceEndPoints.ServiceEndPoints();
        public ResponseVm GetTax()
        {
            try
            {
                IList<TaxModel> tax = objTaxService.TaxServiceInstance().GetTax();
                return new ResponseVm(null, new List<dynamic>(tax).ToList());
            }
            catch (FaultException<FaultData> e)
            {
                return new ResponseVm(e, null);
            }finally
            {
                objTaxService.TaxServiceInstanceClosed();
            }
        }
        public ResponseVm SaveUpdateTax(TaxModel model)
        {
            try
            {
                objTaxService.TaxServiceInstance().SaveUpdateTax(model);
                return new ResponseVm(null, null);
            }
            catch (FaultException<FaultData> e)
            {
                return new ResponseVm(e, null);
            }
            finally
            {
                objTaxService.TaxServiceInstanceClosed();
            }
        }
        public bool DeleteTax(int id)
        {
            try
            {
                return objTaxService.TaxServiceInstance().DeleteTax(id);
            }
            catch (FaultException<FaultData> ex)
            {
                throw ex;
            }
            finally
            {
                objTaxService.TaxServiceInstanceClosed();
            }
        }
    }
}
