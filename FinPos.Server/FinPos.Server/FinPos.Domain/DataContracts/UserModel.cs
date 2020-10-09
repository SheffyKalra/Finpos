using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FinPos.DomainContracts.DataContracts
{
    [DataContract]
    public class UserModel
    {
        public UserModel(int? id, int userCode, string createdDate, string firstname, string lastName, bool isAdmin, string email, string password, bool isActive, string modifiedDate, string modifiedBy, string createdBy, int roleId)
        {
            this.Id = id;
            this.UserCode = userCode;
            this.CreatedDate = createdDate;
            this.FirstName = firstname;
            this.LastName = lastName;
            this.IsAdmin = isAdmin;
            this.Email = email;
            this.Password = password;
            this.IsActive = isActive;
            this.ModifiedDate = ModifiedDate;
            this.ModifiedBy = modifiedBy;
            this.CreatedBy = createdBy;
            this.RoleId = roleId;
        }



        [DataMember]
        public int? Id { get; set; }

        [DataMember]
        public int UserCode { get; set; }

        [DataMember]
        public string CreatedDate { get; set; }

        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public string LastName { get; set; }

        [DataMember]
        public bool IsAdmin { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string Password { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

        [DataMember]
        public string ModifiedDate { get; set; }

        [DataMember]
        public string ModifiedBy { get; set; }

        [DataMember]
        public string CreatedBy { get; set; }

        [DataMember]
        public int RoleId { get; set; }
       
        public string Initials
        {
            get
            {
                Regex initials = new Regex(@"(\b[a-zA-Z])[a-zA-Z]* ?");
                return initials.Replace(this.FirstName + " " + this.LastName, "$1").ToUpper();
            }
        }
    }
}
