using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace FinPos.Domain.DataContracts
{
    [DataContract]
    public class CompanyModel : INotifyPropertyChanged
    {

       
        //public CompanyModel(string name, int? code, string phoneNo, string description, string logo)
        public CompanyModel(string name, string phoneNo, string description)
        {
            this.Name = name;
            this.Description = description;
            //this.Logo = logo;
            this.PhoneNo = phoneNo;
        }
        //public CompanyModel(int? id, int? code, string name, string description, string phoneNo, string logo, bool isDefault, bool isActive, DateTime? createdDate, DateTime? updatedDate, string modifiedBy, string createdBy)
        //{
        public CompanyModel(int? id, string name, string description, string phoneNo, string logo, bool isDefault, bool isActive, string createdDate, string updatedDate, string modifiedBy, string createdBy)
        {
            this.Id = id;
            this.Name = name;
            this.Description = description;
            this.Logo = logo;
            this.PhoneNo = phoneNo;
            this.IsDefault = isDefault;
            this.IsActive = isActive;
            this.CreatedDate = createdDate;
            this.UpdatedDate = updatedDate;
            this.ModifiedBy = modifiedBy;
            this.CreatedBy = createdBy;
        }
        public CompanyModel(int?id,string name)
        {
            this.Id = id;
            this.Name = name;
        }

        [DataMember]
        public int? Id { get; set; }

        [DataMember]
        public string Name { get; set; }


        [DataMember]
        public string PhoneNo { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string Logo { get; set; }

        [DataMember]
        public bool IsDefault { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

        [DataMember]
        public string CreatedDate { get; set; }

        [DataMember]
        public string UpdatedDate { get; set; }

        [DataMember]
        public string ModifiedBy { get; set; }

        [DataMember]
        public string CreatedBy { get; set; }

        [DataMember]
        public string ColorProperty { get; set; }
        public string colorProperty
        {
            get { return ColorProperty; }
            set
            {
                ColorProperty = value;
                NotifyPropertyChanged("ColorProperty");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
