using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BaseDB;
using System.Data.SqlClient;
using BaseClasses;

namespace BaseClasses
{
    public class BaseListDataAccess : BaseDB.BaseDataAccess
    {
        public BaseDB.BaseAdapter baseClassAdapter = null;
        public string primaryKey = "";
        public string objectName = "";
        public string objectType = "";
        public string tableName = "";
        private string menuFilter = "";

        public BaseListDataAccess(string tbl_name)
        {
            this.tableName = tbl_name;
            CreateAdapter();
            base.Dispose();
        }
        private void CreateAdapter()
        {
            baseClassAdapter = new BaseAdapter();
            
            baseClassAdapter.TableMappings(tableName, tableName);
        }
        
        public DataSet SelectPartial(string from, string fields, string filter, int start, int limit, string orderBy, out int totalRecords)
        {
           
            BaseCommand cmn = new BaseCommand(MsConn);
            cmn.CommandType = System.Data.CommandType.StoredProcedure;
            cmn.CommandText = "GetPartialData";
            cmn.Command.Parameters.AddWithValue("tableName", from);
            cmn.Command.Parameters.AddWithValue("fields", fields);
            cmn.Command.Parameters.AddWithValue("filter", filter);
            cmn.Command.Parameters.AddWithValue("start", start);
            cmn.Command.Parameters.AddWithValue("limit", limit);
            cmn.Command.Parameters.AddWithValue("orderby", orderBy);
            cmn.Command.Parameters.Add("totalRecords", SqlDbType.Int);
            cmn.Command.Parameters["totalRecords"].Direction = ParameterDirection.Output;
            cmn.Command.CommandType = CommandType.StoredProcedure;

            if (baseClassAdapter != null)
                baseClassAdapter.Dispose();
            baseClassAdapter = new BaseAdapter();

            baseClassAdapter.SelectCommand = cmn.Command;

            baseClassAdapter.TableMappings(tableName, tableName);

            DataSet Data = new DataSet();

            baseClassAdapter.Fill(Data, tableName);
            totalRecords = (int)cmn.Command.Parameters["totalRecords"].Value;
            return Data;

        }

    }
}
