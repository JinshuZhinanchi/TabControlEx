using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.IO;
using System.Data;

namespace TabControlExDemo
{
    #region sqlite 接口封装
    public class SQLiteHelper
    {
        private static string dbFileName = "";
        //创建数据库文件
        public static void CreateDBFile(string fileName)
        {

            string path = System.Windows.Forms.Application.StartupPath + @"\Data\";
            //path = "D:\\data\\";
           
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string databaseFileName = path + fileName;
            dbFileName = databaseFileName;
            if (!File.Exists(databaseFileName))
            {
                SQLiteConnection.CreateFile(databaseFileName);                
            }
        }

        //生成连接字符串
        private static string CreateConnectionString()
        {
            SQLiteConnectionStringBuilder connectionString = new SQLiteConnectionStringBuilder();
            connectionString.DataSource = dbFileName;

            string conStr = connectionString.ToString();
            return conStr;
        }

        /// <summary>
        /// 对插入到数据库中的空值进行处理
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static object ToDbValue(object value)
        {
            if (value == null)
            {
                return DBNull.Value;
            }
            else
            {
                return value;
            }
        }

        /// <summary>
        /// 对从数据库中读取的空值进行处理
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static object FromDbValue(object value)
        {
            if (value == DBNull.Value)
            {
                return null;
            }
            else
            {
                return value;
            }
        }
             

        /// <summary>
        /// 执行非查询的数据库操作
        /// </summary>
        /// <param name="sqlString">要执行的sql语句</param>
        /// <param name="parameters">参数列表</param>
        /// <returns>返回受影响的条数</returns>
        public static int ExecuteNonQuery(string sqlString, params SQLiteParameter[] parameters)
        {
            string connectionString = CreateConnectionString();
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                using (SQLiteCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sqlString;
                    foreach (SQLiteParameter parameter in parameters)
                    {
                        cmd.Parameters.Add(parameter);
                    }
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// 执行查询并返回查询结果第一行第一列
        /// </summary>
        /// <param name="sqlString">SQL语句</param>
        /// <param name="sqlparams">参数列表</param>
        /// <returns></returns>
        public static object ExecuteScalar(string sqlString, params SQLiteParameter[] parameters)
        {
            string connectionString = CreateConnectionString();
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                using (SQLiteCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sqlString;
                    foreach (SQLiteParameter parameter in parameters)
                    {
                        cmd.Parameters.Add(parameter);
                    }
                    return cmd.ExecuteScalar();
                }
            }
        }

        /// <summary>
        /// 查询多条数据
        /// </summary>
        /// <param name="sqlString">SQL语句</param>
        /// <param name="parameters">参数列表</param>
        /// <returns>返回查询的数据表</returns>
        public static DataTable GetDataTable(string sqlString, params SQLiteParameter[] parameters)
        {
            string connectionString = CreateConnectionString();
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                using (SQLiteCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sqlString;
                    foreach (SQLiteParameter parameter in parameters)
                    {
                        cmd.Parameters.Add(parameter);
                    }
                    DataSet ds = new DataSet();
                    SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
                    adapter.Fill(ds);
                    return ds.Tables[0];
                }
            }
        }
    }
    #endregion 接口封装结束

    #region 操作封装
    public class WarningTableData
    {
        public int WarningID { get; set; }
        public string MachineName { get; set; }
        public DateTime Time { get; set; }
        public string Name { get; set; }        
        public string Descript { get; set; }

        public WarningTableData()
        {
            MachineName = "001";
            Time = DateTime.Now;
            Name = "";
            Descript = "";
        }
    }

    public class WarningTableOP
    {
        public WarningTableData ToScriptType(DataRow row)
        {
            WarningTableData type = new WarningTableData();
            type.MachineName = (string)row["MachineName"];
            type.Time = (DateTime)row["DateTime"];
            type.Name = (string)row["Name"];            
            type.Descript = (string)row["Descript"];
            return type;
        }

        public void Insert(WarningTableData Type)
        {
            SQLiteHelper.ExecuteNonQuery(@"insert into WarningTable(MachineName,Time,Name,Descript)
                                            values(@MachineName,@Time,@Name,@Descript)",                                           
                                           new SQLiteParameter("MachineName", Type.MachineName),
                                           new SQLiteParameter("Time", Type.Time),
                                           new SQLiteParameter("Name", Type.Name),
                                           new SQLiteParameter("Descript", Type.Descript));

        }

        public void Update(WarningTableData Type)
        {
            SQLiteHelper.ExecuteNonQuery(@"update WarningTable set MachineName=@MachineName,
                                           Time=@Time  Descript=@Descript where Name=@Name",                                           
                                           new SQLiteParameter("MachineName", Type.MachineName),
                                           new SQLiteParameter("Time", Type.Time),
                                           new SQLiteParameter("Name", Type.Name),
                                           new SQLiteParameter("Descript", Type.Descript)
                                           );
        }             


        public DataTable GetbyName(string name)
        {
            DataTable table = SQLiteHelper.GetDataTable("select * from WarningTable where Name=@Name",
                                                        new SQLiteParameter("Name", name));
            return table;
        }

        public int GetCountByTime( DateTime time1 , DateTime time2 )
        {
            int nCount = 0;
            Object ob = SQLiteHelper.ExecuteScalar(@"select count(*) from WarningTable where Time>=@time1 AND Time <= @time2 ",
                                                        new SQLiteParameter("time1", time1),
                                                        new SQLiteParameter("time2", time2)
                                                        );

            nCount = Convert.ToInt32(ob);
            return nCount;
        }

        public DataTable GetDataByTime(DateTime time1,DateTime time2)
        {            
            DataTable table = SQLiteHelper.GetDataTable("select Time,Name from WarningTable where Time>=@time1 AND Time <= @time2",
                                                        new SQLiteParameter("time1", time1),
                                                        new SQLiteParameter("time2", time2)
                                                        );
            return table;
        }

        public DataTable GetDataByTime(DateTime time1, DateTime time2, int nStart, int nCount)
        {
            DataTable table = SQLiteHelper.GetDataTable("select Time,Name,Descript from WarningTable where Time>=@time1 AND Time <= @time2 limit @nCount offset @nStart",
                                                        new SQLiteParameter("time1", time1),
                                                        new SQLiteParameter("time2", time2),
                                                        new SQLiteParameter("nCount", nCount),
                                                        new SQLiteParameter("nStart", nStart)                                                        
                                                        );
            return table;
        }

        public void ClearAll()
        {
            SQLiteHelper.ExecuteNonQuery(@"delete from WarningTable" ,                                           
                                           new SQLiteParameter("Descript", "")
                                           );
        }

        public WarningTableData[] ListAll()
        {
            DataTable table = SQLiteHelper.GetDataTable("select * from WarningTable where MachineName='0'");
            WarningTableData[] type = new WarningTableData[table.Rows.Count];
            for (int i = 0; i < table.Rows.Count; i++)
            {
                type[i] = ToScriptType(table.Rows[i]);
            }
            return type;
        }

        public WarningTableData[] Search(string sql, List<SQLiteParameter> parameterList)
        {
            DataTable table = SQLiteHelper.GetDataTable(sql, parameterList.ToArray());
            WarningTableData[] type = new WarningTableData[table.Rows.Count];
            for (int i = 0; i < table.Rows.Count; i++)
            {
                type[i] = ToScriptType(table.Rows[i]);
            }
            return type;
        }
    }
    #endregion 操作封装结束

    #region 操作封装
    public class DeviceWarnings
    {           
        public string Name { get; set; }
        public string Address { get; set; }
        public string Descript { get; set; }         
        public Int64 Count { get; set; }
    }

    public class DeviceWarningsOP
    {
        public DeviceWarnings ToScriptType(DataRow row)
        {
            DeviceWarnings type = new DeviceWarnings();
                        
            type.Name = (string)row["Name"];
            type.Address = (string)row["Address"];
            type.Descript = (string)row["Descript"];
            type.Count = (Int64)row["Count"];
            return type;
        }


        public void Update(DeviceWarnings Type)
        {
            SQLiteHelper.ExecuteNonQuery(@"update DeviceWarnings set   Count =@Count where Name=@Name",                                                                                      
                                           new SQLiteParameter("Name", Type.Name),
                                           new SQLiteParameter("Count", Type.Count)
                                           );
        }

        public void ClearCount()
        {
            int nCount = 0;
            SQLiteHelper.ExecuteNonQuery(@"update DeviceWarnings set   Count =@Count",                                           
                                           new SQLiteParameter("Count", nCount)  );
        }

        public DeviceWarnings GetbyName(string name)
        {
            DataTable table = SQLiteHelper.GetDataTable("select * from DeviceWarnings where Name=@Name",
                                                        new SQLiteParameter("Name", name));
            if (table.Rows.Count <= 0)
            {
                return null;
            }
            else if (table.Rows.Count > 1)
            {
                throw new Exception("类型名称重复！");
            }
            else
            {
                return ToScriptType(table.Rows[0]);
            }
        }     

    }
    #endregion 操作封装结束
}
