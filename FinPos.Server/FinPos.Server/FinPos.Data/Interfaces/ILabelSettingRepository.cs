using FinPos.Data.Entities;
using FinPos.DomainContracts.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPos.Data.Interfaces
{
    public interface ILabelSettingRepository
    {
        LabelSetting GetLabelData(int ItemId);
        void UpdateLabel(LabelSetting labelData);
        int UpdateMasterLabel(MasterLabelSetting masterLabelData);
        void DeleteMasterLabelSetting(MasterLabelSetting masterLabel);
        void DeleteProductLabelSetting(List<ProductLabelSetting> productLabel);
        MasterLabelSetting GetMasaterLabelSetting();
        void SaveProductLabel(List<ProductLabelSetting> masterLabelData);
        List<LabelSetting> GetLabel();
        List<ProductLabelSetting> GetProductLabelSettings(int masterLabelId);
        List<ProductLabelSetting> GetAllProductLabelSettings();
    }
}
