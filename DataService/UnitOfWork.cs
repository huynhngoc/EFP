using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService
{
    public interface IUnitOfWork
    {
        void Save();
        Task SaveAsync();
    }

    public class UnitOfWork : IUnitOfWork
    {
        private EFPEntities entities { get; set; }

        public UnitOfWork(EFPEntities entities)
        {
            this.entities = entities;
        }

        public void Save()
        {
            entities.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await this.entities.SaveChangesAsync();
        }
    }
}
