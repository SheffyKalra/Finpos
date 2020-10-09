using FinPos.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPos.Data.Entities
{
    public class User : BaseEntity
    {
        public User()
        {

        }
        public User(int? id, int userCode, string createdDate, string firstname, string lastName, bool isAdmin, string email, string password, bool isActive, string modifiedDate, string modifiedBy, string createdBy, int roleId)
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



        // public int? Id { get; set; }

        public int UserCode { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public bool IsAdmin { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public bool IsActive { get; set; } = true;
        public string ModifiedDate { get; set; }

        public string ModifiedBy { get; set; }

        public string CreatedBy { get; set; }

        public int RoleId { get; set; }
    }
}

