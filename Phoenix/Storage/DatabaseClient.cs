using MySql.Data.MySqlClient;
using System;
using System.Data;
using Phoenix.Storage;
namespace Phoenix.Storage
{
    internal class DatabaseClient : IDisposable
    {
        private MySqlCommand Command;
        private MySqlConnection Connection;
        private DatabaseManager Manager;

        public DatabaseClient(DatabaseManager _Manager)
        {
            this.Manager = _Manager;
            this.Connection = new MySqlConnection(_Manager.ConnectionString);
            this.Command = this.Connection.CreateCommand();
            this.Connection.Open();
        }

        public void AddParamWithValue(string sParam, object val)
        {
            this.Command.Parameters.AddWithValue(sParam, val);
        }

        public void Dispose()
        {
            this.Connection.Close();
            this.Command.Dispose();
            this.Connection.Dispose();
        }

        public void ExecuteQuery(string sQuery)
        {
            this.Command.CommandText = sQuery;
            this.Command.ExecuteScalar();
            this.Command.CommandText = null;
        }

        public DataRow ReadDataRow(string Query)
        {
            DataTable table = this.ReadDataTable(Query);
            if ((table != null) && (table.Rows.Count > 0))
            {
                return table.Rows[0];
            }
            return null;
        }

        public DataSet ReadDataSet(string Query)
        {
            DataSet dataSet = new DataSet();
            this.Command.CommandText = Query;
            using (MySqlDataAdapter adapter = new MySqlDataAdapter(this.Command))
            {
                adapter.Fill(dataSet);
            }
            this.Command.CommandText = null;
            return dataSet;
        }

        public DataTable ReadDataTable(string Query)
        {
            DataTable dataTable = new DataTable();
            this.Command.CommandText = Query;
            using (MySqlDataAdapter adapter = new MySqlDataAdapter(this.Command))
            {
                adapter.Fill(dataTable);
            }
            this.Command.CommandText = null;
            return dataTable;
        }

        public int ReadInt32(string Query)
        {
            this.Command.CommandText = Query;
            int num = int.Parse(this.Command.ExecuteScalar().ToString());
            this.Command.CommandText = null;
            return num;
        }

        public string ReadString(string Query)
        {
            this.Command.CommandText = Query;
            string str = this.Command.ExecuteScalar().ToString();
            this.Command.CommandText = null;
            return str;
        }

        public uint ReadUInt32(string sQuery)
        {
            this.Command.CommandText = sQuery;
            uint num = (uint)this.Command.ExecuteScalar();
            this.Command.CommandText = null;
            return num;
        }
    }
}
