using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myKA.Driver.SqliteDB
{
    public enum PROCESS
    {
        CREATE_DB,
        UPDATE_DATA,
        WRITE_DATA,
        READ_DATA,
    }

    public class SqliteHandler
    {
        public PROCESS process { get; set; }
        public DataTable dataTable { get; set; }
        public string messageLog { get; set; }
        public object messageObject { get; set; }
    }
}
