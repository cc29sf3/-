using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Data.OleDb;


namespace Utility.Dao
{
    /// <summary>
    /// 针对DBF数据库的操作
    /// 名称：DBFHelper
    /// </summary>
    public sealed class DbfHelper
    {
        private string connectionString;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="Dir">文件库目录</param>
        public DbfHelper(string Dir)
        { connectionString = "Driver={Microsoft Visual FoxPro Driver};SourceType=DBF;SourceDB=" + Dir + ";Exclusive=No;"; }

        /// <summary>
        /// 根据路径和查询语句，返回查询结果
        /// </summary>
        /// <param name="cmdText">查询语句</param>
        /// <param name="path">数据库文件的路径</param>
        /// <returns>DataTable</returns>
        public DataTable ExecuteTable(string cmdText)
        {
            DataTable dataTable = new DataTable();
            using (OdbcConnection connection = new OdbcConnection(connectionString))
            {
                OdbcCommand cmd = new OdbcCommand();
                PrepareCommand(cmd, connection, null, CommandType.Text, cmdText, null);
                OdbcDataAdapter adapter = new OdbcDataAdapter(cmd);
                try
                { adapter.Fill(dataTable); }
                catch (OutOfMemoryException e)
                { }
                catch (Exception e)
                { }
                cmd.Parameters.Clear();
            }
            return dataTable;
        }

        /// <summary>
        /// 利用SQL语句来对记录执行增删改操作
        /// </summary>
        /// <param name="cmdText">要执行的增删改的SQL语句</param>
        /// <returns>返回受影响的行数</returns>
        public int ExecuteNonQuery(string cmdText)
        {
            int num = -1;
            using (OdbcConnection connection = new OdbcConnection(connectionString))
            {
                OdbcCommand cmd = new OdbcCommand();
                PrepareCommand(cmd, connection, null, CommandType.Text, cmdText, null);
                num = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                cmd.Dispose();
            }
            return num;
        }

        /// <summary>
        /// 获取列名
        /// </summary>
        /// <param name="table">表</param>
        /// <returns>List[string]</returns>
        public List<string> GetColomnname(string table)
        {
            DataTable rsbz = ExecuteTable("select TOP 1 * from " + table);
            List<string> colomnNames = new List<string>();
            foreach (DataColumn Columns in rsbz.Columns)//获得列名
                colomnNames.Add(Columns.ColumnName);
            return colomnNames;
        }

        /// <summary>
        /// 给SqlCommand实例指定参数信息
        /// </summary>
        /// <param name="cmd">OdbcCommand实例</param>
        /// <param name="conn">OdbcConnection实例</param>
        /// <param name="trans">数据库事务实例</param>
        /// <param name="cmdType">SQL命令类型</param>
        /// <param name="cmdText">SQL命令</param>
        /// <param name="cmdParms">SQL命令参数</param>
        private void PrepareCommand(OdbcCommand cmd, OdbcConnection conn, OdbcTransaction trans, CommandType cmdType, string cmdText, OdbcParameter[] cmdParms)
        {
            try
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = cmdText;
                if (trans != null)
                    cmd.Transaction = trans;
                cmd.CommandType = cmdType;
                if (cmdParms != null)
                    foreach (OdbcParameter parameter in cmdParms)
                        cmd.Parameters.Add(parameter);
            }
            catch (Exception e)
            {  };
        }

        //public static void dbfExport(DataTable dt, string strExportPath, string strExportFile, string strStructFile, System.Windows.Forms.ProgressBar prgBar)
        //{
        //    //第一步：拷贝标准库
        //    //GetText.Copy(strStructFile, strExportPath + "\\" + strExportFile, true);

        //    //建立连接，读取拷贝过去的那个库，注意连接字符串，使用的是vfp9.0的driver，微软网站上有下载
        //    OleDbConnection conn1 = new OleDbConnection();
        //    conn1.ConnectionString = "Provider=VFPOLEDB.1;DATA Source=" + strExportPath + "\\" + strExportFile + ";";
        //    string strSQL = "SELECT * FROM " + strExportFile;
        //    OleDbDataAdapter adp = new OleDbDataAdapter(strSQL, conn1);
        //    DataTable dt1 = new DataTable();
        //    adp.Fill(dt1);

        //    //初始化进度条
        //    prgBar.Value = 0;
        //    prgBar.Maximum = dt.Rows.Count;

        //    //循环读取dt的数据添加到dt1，注意方法，还有一个就是null值的处理，具体要根据你的dbf库结构来设定。
        //    //我的dbf库主要就是两种字段，一种是字符型，一种是整型

        //    for (int i = 0; i < dt.Rows.Count; i++)
        //    {
        //        DataRow dr = dt1.NewRow();
        //        for (int j = 0; j < dt.Columns.Count; j++)
        //            //下面这段代码我做点解释，对于dr[j]的赋值，首先判断dt.Rows[i][dt1.Colums[j].ColumnName.ToString()]的值是否为null，
        //            //如果是null，则赋值为DBNull.Value；如果不是null，则插入原始值dt.Rows[i][dt1.Colums[j].ColumnName.ToString()]
        //            //直接赋值null是不可以的，null主要用于对象类的数据，DataTable数据库中的null应该用DBNull.Value来代替
        //            dr[j] = dt.Rows[i][dt1.Columns[j].ColumnName.ToString()] == null ? DBNull.Value : dt.Rows[i][dt1.Columns[j].ColumnName.ToString()];
        //        dt1.Rows.Add(dr);
        //        prgBar.Value++;
        //    }

        //    //初始化一个CommandBuilder，目的是使得adp的更新操作初始化
        //    System.Data.OleDb.OleDbCommandBuilder cmdBld = new System.Data.OleDb.OleDbCommandBuilder(adp);

        //    //更新dt1，系统自动将数据添加到dbf库中
        //    adp.Update(dt1);
        //}
    }
}