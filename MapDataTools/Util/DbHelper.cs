using System;
using System.Collections.Generic;
using System.Text;

namespace MapDataTools.Util
{
    using System.Data;

    using Npgsql;

    public class DbHelper : IDisposable
    {
        private string connection;

        public DbHelper(string connection)
        {
            this.connection = connection;
            dbcon = new NpgsqlConnection(connection);
        }

        public IDbCommand CreateParametersCommand(Action<NpgsqlCommand> action)
        {
            using (var con = new NpgsqlConnection(connection))
            {
                con.Open();
                var command = con.CreateCommand() as NpgsqlCommand;
                if (action != null)
                {
                    action(command);
                }
                return command;
            }
        }



        public IDataReader ExecuteReader(string sql)
        {
            var con = new NpgsqlConnection(connection);
            con.Open();
            var command = con.CreateCommand();
            command.CommandText = sql;
            command.CommandType = CommandType.Text;
            return command.ExecuteReader(CommandBehavior.CloseConnection);
        }

        public object ExecuteScalar(string sql)
        {
            using (var con = new NpgsqlConnection(connection))
            {
                con.Open();
                var command = con.CreateCommand();
                command.CommandText = sql;
                command.CommandType = CommandType.Text;
                var obj = command.ExecuteScalar();
                command.Dispose();
                return obj;
            }
        }

        public int ExecuteNonQuery(string sql,int timeOut = 60000)
        {
            using (var con = new NpgsqlConnection(connection))
            {
                con.Open();
                var command = con.CreateCommand();
                command.CommandText = sql;
                command.CommandType = CommandType.Text;
                command.CommandTimeout = timeOut;
                var obj = command.ExecuteNonQuery();
                command.Dispose();
                return obj;
            }
        }


        public void Dispose()
        {
            
        }

        private NpgsqlConnection dbcon = null;
        public void AddColumn(string columnName, String tableName)
        {
            //using (var con = new NpgsqlConnection(connection))
            {
                if (dbcon.State != ConnectionState.Open)
                {
                    dbcon.Open();
                }
                
                var command = dbcon.CreateCommand();
                string sqlString =
                    " SELECT column_name FROM information_schema.columns WHERE table_schema='public' AND table_name='"
                    + tableName + "'";
                command.CommandText = sqlString;
                command.CommandType = CommandType.Text;
                var read = command.ExecuteReader();
                var fieldList = new List<string>();
                while (read.Read())
                {
                    string field = read.GetString(0).ToLower();
                    fieldList.Add(field);
                }
                if (!fieldList.Contains(columnName.ToLower()))
                {
                    string sqlString1 = "alter table " + tableName + " add " + columnName + " character varying(254)";
                    this.ExecuteNonQuery(sqlString1);
                }
                read.Close();
                dbcon.Close();
            }
        }

        public List<string> GetTableColumns(string tableName)
        {
            if (dbcon.State != ConnectionState.Open)
            {
                dbcon.Open();
            }

            var command = dbcon.CreateCommand();
            string sqlString =
                " SELECT column_name FROM information_schema.columns WHERE table_schema='public' AND table_name='"
                + tableName + "'";
            command.CommandText = sqlString;
            command.CommandType = CommandType.Text;
            var read = command.ExecuteReader();
            var fieldList = new List<string>();
            while (read.Read())
            {
                string field = read.GetString(0).ToLower();
                fieldList.Add(field);
            }
            
            read.Close();
            dbcon.Close();
            return fieldList;
        }
        public void InsertRoadNet(string table,  List<object> parms)
        {
            string sql = string.Format(@"INSERT INTO {0}(
             name, width, type, geom, length, car, walk, speed, highspeed, 
                np_level)
             VALUES (", table);
            StringBuilder sbBuilder = new StringBuilder();
            foreach (object o in parms)
            {
                sbBuilder.AppendFormat("{0},", o);
            }
            this.ExecuteNonQuery(sql + sbBuilder.ToString().TrimEnd(',') + ")");
        }
    }
}
