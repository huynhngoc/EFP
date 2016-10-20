using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataService.Repository;
using DataService.Utils;

namespace DataService.Service
{
    public class EntityService
    {
        EntityRepository repository = new EntityRepository();
        public bool InitEntity(string shopId)
        {
            try
            {
                AddEntity(EntityGreeting.Name, EntityGreeting.Value, EntityGreeting.IsDynamic, 
                    EntityGreeting.Description, shopId);
                AddEntity(EntityAddress.Name, EntityAddress.Value, EntityAddress.IsDynamic,
                    EntityAddress.Description, shopId);
                AddEntity(EntityPhone.Name, EntityPhone.Value, EntityPhone.IsDynamic,
                    EntityPhone.Description, shopId);
                AddEntity(EntityName.Name, EntityName.Value, EntityName.IsDynamic,
                    EntityName.Description, shopId);
                AddEntity(EntityIntroduction.Name, EntityIntroduction.Value, EntityIntroduction.IsDynamic,
                    EntityIntroduction.Description, shopId);
                AddEntity(EntityBankAccount.Name, EntityBankAccount.Value, EntityBankAccount.IsDynamic,
                    EntityBankAccount.Description, shopId);
                AddEntity(EntityProductPrice.Name, EntityProductPrice.Value, EntityProductPrice.IsDynamic,
                    EntityProductPrice.Description, shopId);
                AddEntity(EntityProductInStock.Name, EntityProductInStock.Value, EntityProductInStock.IsDynamic,
                    EntityProductInStock.Description, shopId);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool AddEntity(string name, string value, bool isDynamic, string description, string shopId)
        {
            Entity e = new Entity()
            {
                EntityName = name,
                Value = value,
                IsDynamic = isDynamic,
                Description = description,
                ShopId = shopId
            };
            return repository.Create(e);
            
        }
    }
}
