using FinPos.Data.Entities;
using FinPos.Data.Interfaces;
using FinPos.DomainContracts.DataContracts;
using FinPos.WcfHost.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

namespace FinPos.WcfHost.Services
{
    [ServiceBehavior(Name = "TaxService", Namespace = "http://locahost:8080/TaxService", InstanceContextMode = InstanceContextMode.Single)]
    public class TaxService : ITaxService
    {
        #region Properties
        private readonly ITaxRepository _taxRepository;
        FaultData fault = new FaultData();
        #endregion

        #region Constructor
        public TaxService(ITaxRepository taxRepository)
        {
            this._taxRepository = taxRepository;
        }
        #endregion

        #region Getter Methods
        public IList<TaxModel> GetTax()
        {
            try
            {
                List<Tax> taxs = _taxRepository.GetTax();
                // var items = products.ToList();
                return taxs?.Select(x =>
                    new TaxModel(x.Id, x.TaxDetail, x.Rate, x.CreatedDate, x.ModifiedDate, x.ModifiedBy, x.CreatedBy)
                ).ToList();
            }
            catch (Exception ex)
            {
                fault.Result = false;
                fault.ErrorMessage = "Error in GetTax method";
                fault.ErrorDetails = ex.ToString();
                throw new FaultException<FaultData>(fault);
            }
        }
        #endregion

        #region CRUD Operations
        public void SaveUpdateTax(TaxModel model)
        {
            try
            {
                Tax tax = new Tax();
                if (model.TaxCode > 0)
                {
                    tax = _taxRepository.GetTax().FirstOrDefault(x => x.Id == model.TaxCode);
                    tax.CreatedDate = CommonFunctions.ParseDateToFinclaveString(DateTime.Now.ToShortDateString());
                }
                else
                {
                    tax.CreatedDate = model.CreatedDate;
                    tax.ModifiedBy = model.ModifiedBy;
                    tax.ModifiedDate = model.ModifiedDate;

                }
                tax.TaxDetail = model.TaxDetail;
                tax.Rate = model.Rate;
                _taxRepository.SaveUpdateTax(tax);
            }
            catch (Exception ex)
            {
                fault.Result = false;
                fault.ErrorMessage = "Error During Save Or Update Tax";
                fault.ErrorDetails = ex.ToString();
                throw new FaultException<FaultData>(fault);
            }
        }
        public bool DeleteTax(int id)
        {
            try
            {
                Tax tax = _taxRepository.GetTax().FirstOrDefault(x => x.Id == id);
                _taxRepository.DeleteTax(tax);
                return true;
            }
            catch (Exception ex)
            {
                fault.Result = false;
                fault.ErrorMessage = "Error in DeleteProducts method";
                fault.ErrorDetails = ex.ToString();
                throw new FaultException<FaultData>(fault);
            }
        }
        #endregion
    }
}
