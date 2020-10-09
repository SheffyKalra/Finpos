using FinPos.Data.Entities;
using FinPos.Domain.DataContracts;
using FinPos.DomainContracts.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace FinPos.WcfHost
{
    [ServiceContract]
    public interface IFinPosService
    {




       



       
        

        
        

        

















       
        
        
        
        

        [OperationContract]
        IList<PaymentToSupplierModel> GetPaymentByInvoiceNo(int id);



















      

        [OperationContract]
        [FaultContract(typeof(FaultData))]
        void UpdateExistDefaultCompany(int? id);


        
        

        [OperationContract]
        [FaultContract(typeof(FaultData))]
        void UpdateExistDefaultBranch(int companyId, int branchId);

        

       







       


        
        
        
        

       

        
        
        
        

       

      
       

       

        

        
        //[OperationContract]
        //[FaultContract(typeof(FaultData))]
        //List<StockModel> GetStocksById(int purchaseId,int companyId,int? branchId);

       
       

        

        

       

        

      
        


    }
}