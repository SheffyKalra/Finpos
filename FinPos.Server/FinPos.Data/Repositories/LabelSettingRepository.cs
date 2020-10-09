using FinPos.DAL.Interfaces;
using FinPos.Data.Entities;
using FinPos.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPos.Data.Repositories
{
    public class LabelSettingRepository : ILabelSettingRepository
    {
        private readonly IEntityProvider<LabelSetting> _labelSettingProvider;
        private readonly IEntityProvider<MasterLabelSetting> _masterLabelSettingProvider;
        private readonly IEntityProvider<ProductLabelSetting> _productLabelSettingProvider;

        public LabelSettingRepository(IEntityProvider<LabelSetting> labelSettingProvider, IEntityProvider<MasterLabelSetting> masterLabelSettingProvider, IEntityProvider<ProductLabelSetting> productLabelSettingProvider)
        {
            this._labelSettingProvider = labelSettingProvider;
            this._masterLabelSettingProvider = masterLabelSettingProvider;
            this._productLabelSettingProvider = productLabelSettingProvider;
        }

        public List<LabelSetting> GetLabel()
        {
            return _labelSettingProvider.Get().ToList();
        }
        public LabelSetting GetLabelData(int ItemId)
        {
            return _labelSettingProvider.Get().FirstOrDefault(x => x.ItemId == ItemId);
        }
        public MasterLabelSetting GetMasaterLabelSetting()
        {
            return _masterLabelSettingProvider.Get().FirstOrDefault();
        }

        public List<ProductLabelSetting> GetProductLabelSettings(int masterLabelId)
        {
            return _productLabelSettingProvider.Get().Where(x=>x.MasterLabelSettingCode==masterLabelId).ToList();
        }
        public List<ProductLabelSetting> GetAllProductLabelSettings()
        {
            return _productLabelSettingProvider.Get().ToList();
        }

        public void UpdateLabel(LabelSetting labelData)
        {
            if (labelData.Id > 0 && labelData.Id != null)
            {
                _labelSettingProvider.Update(labelData);
            }
            else
            {
                _labelSettingProvider.Insert(labelData);
            }
        }
        public int  UpdateMasterLabel(MasterLabelSetting labelData)
        {
            //if (labelData.Id > 0 && labelData.Id != null)
            //{
            //    _labelSettingProvider.Update(labelData);
            //}
            //else
            //{
            int masterLabelId = _masterLabelSettingProvider.Insert(labelData);
            return masterLabelId;
            //  }
        }
        public void DeleteMasterLabelSetting(MasterLabelSetting masterLabel)
        {
             _masterLabelSettingProvider.Delete(masterLabel);
            //  }
        }
        public void DeleteProductLabelSetting(List<ProductLabelSetting> productLabel)
        {
            productLabel.ForEach(x =>
            {
                _productLabelSettingProvider.Delete(x);

            });
            //  }
        }
        public void SaveProductLabel(List<ProductLabelSetting> productLabelData)
        {
            //productLabelData.ForEach(x =>
            //{
            //    _productLabelSettingProvider.Insert(x);
            //});
             _productLabelSettingProvider.InsertAll(productLabelData);
        }
    }
}
