using System;
using System.Collections.Generic;
using System.Web;
using System.Data.SqlClient;
using System.Data;

namespace MedHealthSolutions.Classes
{
    public class clsDB
    {
        SqlConnection objConnect;
        public clsDB()
        {
            objConnect = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MedHealthSolutions"].ConnectionString.ToString());
            objConnect.Open();
        }

        public clsDB(bool IsSupportDB)
        {
            if (!IsSupportDB)
            {
                objConnect = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MedHealthSolutions"].ConnectionString.ToString());
            }
            else {
                objConnect = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["SupportDatabase"].ConnectionString.ToString());
            }
            objConnect.Open();
        }

        public clsDB(string conStringName)
        {
            objConnect = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings[conStringName].ConnectionString.ToString());
            objConnect.Open();
        }

        public void closeConnection()
        {
            if (objConnect.State==System.Data.ConnectionState.Open)
                objConnect.Close();
        }

        public DataSet getDS(string sSQL)
        {
            return getDS(sSQL, false);
        }

        public void getDS(string sSQL, bool CloseConnection,ref DataSet objDS)
        {
            if (objConnect.State != System.Data.ConnectionState.Open)
                objConnect.Open();

            SqlDataAdapter objDataAdapter = new SqlDataAdapter(sSQL, objConnect);
            objDS = new DataSet();
            objDataAdapter.SelectCommand.CommandTimeout = 0;
            objDataAdapter.Fill(objDS);
            //if (CloseConnection) closeConnection();
            closeConnection();
        }

        public DataSet getDS(string sSQL, bool CloseConnection)
        {
        //tp:
        //    int i = 0;
        //    try
        //    {

                if (objConnect.State != System.Data.ConnectionState.Open)
                    objConnect.Open();

                
                SqlDataAdapter objDataAdapter = new SqlDataAdapter(sSQL, objConnect);
                DataSet objDS = new DataSet();
                objDataAdapter.SelectCommand.CommandTimeout = 0;
                objDataAdapter.Fill(objDS);
                //if (CloseConnection) closeConnection();
                closeConnection();
                return objDS;
            //}
            //catch(Exception ex) {
            //    i++;
            //    if (i<=1 && ex.Message.Substring(0, 15) == "Timeout expired")
            //    {
            //        SqlCommand objCommand = new SqlCommand("DBCC DROPCLEANBUFFERS;DBCC FREEPROCCACHE", objConnect);
            //        objCommand.ExecuteNonQuery();
            //        goto tp;
            //    }
            //    else {
            //        throw (ex);
            //    }
                
            //}
        }

        public void executeSQL(string sSQL)
        {
            if (objConnect.State != System.Data.ConnectionState.Open)
                objConnect.Open();
            SqlCommand objCommand = new SqlCommand(sSQL,objConnect);
            objCommand.ExecuteNonQuery();
            closeConnection();
        }
    }
}


 