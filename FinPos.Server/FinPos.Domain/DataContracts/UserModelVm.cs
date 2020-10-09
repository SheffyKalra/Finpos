using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPos.DomainContracts.DataContracts
{
   public class UserModelVm
    {
        public UserModelVm(int userId, string userName, string initials, string email, int companyId,string companyName, int? branchid,string branchName)
        {
            UserId = userId;
            UserName = userName;
            InitialName = initials;
            Email = email;
            CompanyId = companyId;
            CompanyName = companyName;
            BranchId = branchid;
            BranchName = branchName;
        }
        public static int UserId { get; set; }
        public static string UserName { get; set; }

        public static string InitialName { get; set; }

        public static string Email { get; set; }

        public static int CompanyId { get; set; }

        public static string CompanyName { get; set; }

        public static int? BranchId { get; set; }
        public static string BranchName { get; set; }
       
    }
}
