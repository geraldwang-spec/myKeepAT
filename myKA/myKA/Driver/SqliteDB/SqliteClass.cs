using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using System.Windows.Shapes;

namespace myKA.Driver.SqliteDB
{
    class SqliteClass
    {
        private static SqliteClass sqlite = null;
        private static readonly object oSyncLock = new object();

        public static SqliteClass Director { get { return SqliteClass.sqlite; } }

        public event EventHandler<SqliteHandler> sqlHandler = null;
        public static void initSqliteClass(EventHandler<SqliteHandler> handler)
        {
            lock (oSyncLock) //Thread safe here to ensure we only ever create/assign one.
            {
                if (sqlite == null)
                {
                    sqlite = new SqliteClass();
                    sqlite.sqlHandler += handler;
                }
            }
        }

        private string dbPath { get; set; }

        public void InitDatabase(string _path)
        {
            Task.Run(() =>
            {
                if (_path == null || string.IsNullOrEmpty(_path)) dbPath = Environment.CurrentDirectory + "\\KAs.db";
                else dbPath = _path;

                OnSqlResponseMessage(new SqliteHandler { dataTable = null, messageLog = "create DB", process = PROCESS.CREATE_DB, messageObject = dbPath });
            });
            
        }

        protected virtual void OnSqlResponseMessage(SqliteHandler e)
        {
            EventHandler<SqliteHandler> handler = sqlHandler;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        //public bool CreateTable(string table)
        //{
        //    bool b = false;
        //    try
        //    {
        //        WriteCommand(table);
        //        b = true;
        //    }
        //    catch (Exception exc) { ErrorMessage = exc.ToString(); }
        //    return b;
        //}

        //public int WriteCommand(string cmd)
        //{
        //    int numberOfRowsAffected = -1;
        //    try
        //    {
        //        using (var con = new SQLiteConnection("Data Source=" + DBPath))
        //        {
        //            con.Open();
        //            SQLiteCommand command = con.CreateCommand();
        //            command.CommandText = cmd;
        //            numberOfRowsAffected = command.ExecuteNonQuery();
        //        }
        //    }
        //    catch (Exception exc)
        //    {
        //        ErrorMessage = exc.Message;
        //    }
        //    return numberOfRowsAffected;
        //}

        //public int ExecuteWrite(string query, Dictionary<string, object> args)
        //{
        //    int numberOfRowsAffected;
        //    //setup the connection to the database
        //    using (var con = new SQLiteConnection("Data Source=" + DBPath))
        //    {
        //        con.Open();
        //        //open a new command
        //        using (var cmd = new SQLiteCommand(query, con))
        //        {
        //            //set the arguments given in the query
        //            foreach (var pair in args)
        //            {
        //                cmd.Parameters.AddWithValue(pair.Key, pair.Value);
        //            }
        //            //execute the query and get the number of row affected
        //            numberOfRowsAffected = cmd.ExecuteNonQuery();
        //        }
        //        return numberOfRowsAffected;
        //    }
        //}

        //public bool ExecuteRead(string query, out DataTable dataTable, out string errMesg)
        //{
        //    bool b = false;
        //    errMesg = "";
        //    dataTable = new DataTable();
        //    if (string.IsNullOrEmpty(query.Trim()))
        //    {
        //        errMesg = "query command is empty";
        //        return false;
        //    }
        //    try
        //    {
        //        using (var con = new SQLiteConnection("Data Source=" + DBPath))
        //        {
        //            con.Open();
        //            SQLiteDataAdapter da = new SQLiteDataAdapter(query, con);
        //            DataSet ds = new DataSet();
        //            ds.Clear();
        //            da.Fill(ds);
        //            dataTable = ds.Tables[0];
        //            if (con.State == ConnectionState.Open) con.Close();
        //            b = true;
        //        }
        //    }
        //    catch (Exception exc)
        //    {
        //        ErrorMessage = errMesg = exc.Message;
        //    }
        //    return b;
        //}

        //public bool ExecuteWrite(string cmd, List<WriteClass> writes)
        //{
        //    bool b;
        //    try
        //    {
        //        using (var con = new SQLiteConnection("Data Source=" + DBPath))
        //        {
        //            using (var command = con.CreateCommand())
        //            {
        //                con.Open();
        //                using (var tx = con.BeginTransaction())
        //                {
        //                    command.CommandText = cmd;
        //                    foreach (WriteClass t in writes)
        //                    {
        //                        Type type = t.ColumeValue.GetType();
        //                        switch (type.FullName)
        //                        {
        //                            case "System.String":
        //                                command.Parameters.AddWithValue(t.ColumnName, t.ColumeValue as string);
        //                                break;
        //                            case "System.Int32":
        //                                command.Parameters.AddWithValue(t.ColumnName, Convert.ToInt32(t.ColumeValue));
        //                                break;
        //                        }
        //                    }
        //                    command.ExecuteNonQuery();
        //                    tx.Commit();
        //                }
        //                con.Close();
        //            }

        //            //con.Open();

        //            //SQLiteCommand sqlCmd = new SQLiteCommand(con);
        //            //sqlCmd.CommandText = cmd;
        //            //foreach(WriteClass t in writes)
        //            //{
        //            //    Type type = t.ColumeValue.GetType();
        //            //    switch (type.FullName)
        //            //    {
        //            //        case "System.String":
        //            //            sqlCmd.Parameters.AddWithValue(t.ColumnName, t.ColumeValue as string);
        //            //            break;
        //            //        case "System.Int32":
        //            //            sqlCmd.Parameters.AddWithValue(t.ColumnName, Convert.ToInt32(t.ColumeValue));
        //            //            break;
        //            //    }
        //            //}

        //            //sqlCmd.ExecuteNonQuery();
        //            //if (con.State == ConnectionState.Open) con.Close();
        //            //b = true;
        //        }
        //        b = true;
        //    }
        //    catch (Exception exc)
        //    {
        //        ErrorMessage = exc.Message;
        //        b = false;
        //    }
        //    return b;
        //}

        //public bool ExecuteWriteList(List<WriteClassList> writeDatas)
        //{
        //    bool b;
        //    try
        //    {
        //        using (var con = new SQLiteConnection("Data Source=" + DBPath))
        //        {
        //            using (var command = con.CreateCommand())
        //            {
        //                con.Open();
        //                using (var tx = con.BeginTransaction())
        //                {
        //                    foreach (WriteClassList write in writeDatas)
        //                    {
        //                        command.CommandText = write.Command;
        //                        foreach (WriteClass t in write.Writes)
        //                        {
        //                            Type type = t.ColumeValue.GetType();
        //                            switch (type.FullName)
        //                            {
        //                                case "System.String":
        //                                    command.Parameters.AddWithValue(t.ColumnName, t.ColumeValue as string);
        //                                    break;
        //                                case "System.Int32":
        //                                    command.Parameters.AddWithValue(t.ColumnName, Convert.ToInt32(t.ColumeValue));
        //                                    break;
        //                            }
        //                        }
        //                        command.ExecuteNonQuery();
        //                    }
        //                    tx.Commit();
        //                }
        //                con.Close();
        //            }

        //        }
        //        b = true;
        //    }
        //    catch (Exception exc)
        //    {
        //        ErrorMessage = exc.Message;
        //        b = false;
        //    }
        //    return b;
        //}
    }
}
