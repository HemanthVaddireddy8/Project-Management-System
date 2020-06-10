using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.Common;

using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core.Metadata.Edm;



namespace BaseDB
{
    public class BaseRepository<T> : IRepository 
            where T : DbContext, new()
    {
        protected T db = new T();
        
        public void Kaydet()
        {
            UpdateRecordTrackingMark();
            db.SaveChanges();
        }
        protected void UpdateRecordTrackingMark()
        {
            //Guid user_uid = BaseDB.SessionContext.Current.ActiveUser.UserUid;
            Guid user_uid = (BaseDB.SessionContext.Current == null || BaseDB.SessionContext.Current.ActiveUser == null) ? Guid.Empty : BaseDB.SessionContext.Current.ActiveUser.UserUid;
            var manager = ((IObjectContextAdapter)db).ObjectContext.ObjectStateManager;
            var entries = from e in manager.GetObjectStateEntries(
                System.Data.Entity.EntityState.Added | System.Data.Entity.EntityState.Modified)
                          where e.Entity != null
                          select e;

            foreach (var entry in entries)
            {
                var fieldMetaData = entry.CurrentValues.DataRecordInfo.FieldMetadata;
                System.Data.Entity.Core.Common.FieldMetadata updatedAtField = fieldMetaData
                    .Where(f => f.FieldType.Name == "updated_at").FirstOrDefault();
                System.Data.Entity.Core.Common.FieldMetadata updatedByField = fieldMetaData
                    .Where(f => f.FieldType.Name == "updated_by").FirstOrDefault();


                System.Data.Entity.Core.Common.FieldMetadata insertedAtField = fieldMetaData
                    .Where(f => f.FieldType.Name == "inserted_at").FirstOrDefault();
                System.Data.Entity.Core.Common.FieldMetadata insertedByField = fieldMetaData
                    .Where(f => f.FieldType.Name == "inserted_by").FirstOrDefault();


                if (entry.State == System.Data.Entity.EntityState.Added)
                {
                    if (insertedAtField.FieldType != null)
                        if (insertedAtField.FieldType.TypeUsage.EdmType.Name == PrimitiveTypeKind.DateTime.ToString())
                            entry.CurrentValues.SetDateTime(insertedAtField.Ordinal, DateTime.UtcNow);

                    if (insertedByField.FieldType != null)
                        if (insertedByField.FieldType.TypeUsage.EdmType.Name == PrimitiveTypeKind.Guid.ToString())
                            entry.CurrentValues.SetGuid(insertedByField.Ordinal, user_uid);

                }
                if (entry.State == System.Data.Entity.EntityState.Modified)
                {
                    if (updatedAtField.FieldType != null)
                        if (updatedAtField.FieldType.TypeUsage.EdmType.Name == PrimitiveTypeKind.DateTime.ToString())
                            entry.CurrentValues.SetDateTime(updatedAtField.Ordinal, DateTime.UtcNow);

                    if (updatedByField.FieldType != null)

                        if (updatedByField.FieldType.TypeUsage.EdmType.Name == PrimitiveTypeKind.Guid.ToString())
                            entry.CurrentValues.SetGuid(updatedByField.Ordinal, user_uid);
                }
            }
        }
        public void Dispose()
        {
            db.Dispose();
        }
    }
}
