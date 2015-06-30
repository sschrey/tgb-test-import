using ShippingService.Business.EF.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace ShippingService.Business.EF.Facade
{
    public abstract class BaseFacade : IDisposable
    {
        private DbContext db;
        private TransactionScope scope;

        public BaseFacade(DbContext db)
        {
            this.db = db;
            // ROLA - This is a hack to ensure that Entity Framework SQL Provider is copied across to the output folder.
            // As it is installed in the GAC, Copy Local does not work. It is required for probing.
            // Fixed "Provider not loaded" error
            var ensureDLLIsCopied = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
        }

        public DbContext GetContext()
        {
            return db;
        }

        public virtual IQueryable<T> GetAll<T>() where T : class
        {
            return db.Set<T>().AsQueryable();
        }

        public virtual List<T> GetByQuery<T>(string sql, params object[] paramaters) where T : class
        {
            return db.Set<T>().SqlQuery(sql, paramaters).ToList();
        }

        public T GetById<T>(string id) where T : EntityWithTypedId<string>
        {
            return db.Set<T>().FirstOrDefault(t => t.Id.Equals(id));
        }

        public T GetById<T>(int id) where T : EntityWithTypedId<int>
        {
            return db.Set<T>().FirstOrDefault(t => t.Id.Equals(id));
        }

        public void Delete<T>(T e) where T : class
        {
            db.Set<T>().Remove(e);
        }

        public void Delete<T>(List<T> lst) where T : class
        {
            foreach (var item in lst)
            {
                db.Set<T>().Remove(item);


            }
        }

        public void BeginTransaction()
        {
            BeginTransaction(System.Transactions.IsolationLevel.ReadCommitted);
        }

        public void BeginTransaction(System.Transactions.IsolationLevel isolationLevel)
        {
            var transactionOptions = new TransactionOptions();

            transactionOptions.IsolationLevel = isolationLevel;
            transactionOptions.Timeout = TransactionManager.MaximumTimeout;

            scope = new TransactionScope(TransactionScopeOption.Required, transactionOptions);

        }

        public void CommitTransaction()
        {
            if (scope != null)
            {
                scope.Complete();
                //a complete without dispose will rollback!
                scope.Dispose();
                scope = null;
            }
        }
        public void RollbackTransaction()
        {
            if (scope != null)
                scope.Dispose();
        }
        public void Dispose()
        {
            if (scope != null)
            {
                scope.Dispose();
            }
        }


        public virtual Validation Save()
        {
            Validation val = new Validation();
            try
            {
                db.SaveChanges();
            }
            catch (DbEntityValidationException exc)
            {
                foreach (var error in exc.EntityValidationErrors)
                {
                    foreach (var valError in error.ValidationErrors)
                    {
                        val.AddBrokenRule(error.Entry.Entity.GetType() + ", " + valError.PropertyName + ", " + valError.ErrorMessage);
                    }
                }

            }
            catch (Exception exc)
            {
                string brokenRule = exc.Message;
                while (exc.InnerException != null)
                {
                    brokenRule += "\n" + exc.InnerException.Message;
                    exc = exc.InnerException;
                }

                val.AddBrokenRule(brokenRule);
            }
            return val;
        }

        public bool Contains<T>(T e) where T : Entity
        {
            return db.Set<T>().FirstOrDefault(ent => ent.Id == e.Id) != null;
        }

        public virtual void Add<T>(T e) where T : class
        {
            db.Set<T>().Add(e);
        }

        public Validation Validate<T>(T e) where T : class
        {
            Validation val = new Validation();

            var dbEntityValidationResult = db.Entry(e).GetValidationResult();

            foreach (var valError in dbEntityValidationResult.ValidationErrors)
            {
                val.AddBrokenRule(valError.PropertyName + ", " + valError.ErrorMessage);
            }

            return val;
        }

        public void Reload<T>(T e) where T : class
        {
            db.Entry(e).Reload();
        }

        public void Undo<T>(T e) where T : class
        {
            var entry = db.Entry(e);

            UndoEntity(entry);
        }

        public void UndoEntity(DbEntityEntry entry)
        {
            if (entry.State == System.Data.Entity.EntityState.Added)
            {
                entry.State = System.Data.Entity.EntityState.Detached;
            }
            if (entry.State == System.Data.Entity.EntityState.Modified)
            {
                entry.State = System.Data.Entity.EntityState.Unchanged;
            }
        }

        public void UndoAll()
        {
            var entries = db.ChangeTracker.Entries().Where(e => e.State == System.Data.Entity.EntityState.Added || e.State == System.Data.Entity.EntityState.Modified);

            foreach (var entry in entries)
            {
                UndoEntity(entry);
            }
        }

        public bool IsNew<T>(T e) where T : class
        {
            var entry = db.Entry(e);
            return entry.State == System.Data.Entity.EntityState.Detached || entry.State == System.Data.Entity.EntityState.Added;
        }

    }
}
