using BusinessLayer.Entity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DAL
{
    public class DAL_BRAND
    {
        #region Brand Entry
        public string INSERT_BRAND (string Emp_Code, Brand_Entry brEntry, out ResultMessage oblMsg)
        {          
             string errorMsg = "";          
            oblMsg = new ResultMessage();
            using (var connection = new SqlConnection(sqlConnection.GetConnectionString_SGX()))
            {
                connection.Open();
                SqlCommand command;
                SqlTransaction transactionScope = null;
                transactionScope = connection.BeginTransaction(IsolationLevel.ReadCommitted);
                try
                {
                    SqlParameter[] param = 
                    {
                        new SqlParameter("@ERRORSTR", SqlDbType.VarChar,200)
                       ,new SqlParameter("@BRAND_ID",SqlDbType.Int)
                       ,new SqlParameter("@BRAND_CODE" , SqlDbType.VarChar, 150)
                       ,new SqlParameter("@BRAND_NAME" , brEntry.Brand)                         
                       ,new SqlParameter("@Material_Id",brEntry.Material_Id )  
                       ,new SqlParameter("@Rate", brEntry.Rate)
                       ,new SqlParameter("@UOM_ID", brEntry.UOM_ID)   
                       ,new SqlParameter("@Effective_Dt", brEntry.EffDate)                  
                       ,new SqlParameter("@REMARKS", brEntry.Remark)
                       ,new SqlParameter("@ADD_BY", Emp_Code)
                       
                    };
                    param[0].Direction = ParameterDirection.Output;
                    param[1].Direction = ParameterDirection.Output;
                    param[2].Direction = ParameterDirection.Output;

                    new DataAccess().InsertWithTransaction("[AGG].[USP_INSERT_BRAND]", CommandType.StoredProcedure, out command, connection, transactionScope, param);
                    int BRAND_ID =    (int)command.Parameters["@BRAND_ID"].Value;
                    string BRAND_CODE =Convert.ToString(command.Parameters["@BRAND_CODE"].Value);
                    string error_1 = (string)command.Parameters["@ERRORSTR"].Value;
                   
                    if (BRAND_ID == -1) { errorMsg = error_1; }
                   
                    if (errorMsg == "")
                    {
                        oblMsg.ID = BRAND_ID;
                        oblMsg.CODE = brEntry.Brand;
                        oblMsg.MsgType = "Success";
                        transactionScope.Commit();
                    }
                    else
                    {
                        oblMsg.Msg = errorMsg;
                        oblMsg.MsgType = "Error";
                        transactionScope.Rollback();
                    }
                }
                catch (Exception ex)
                {
                    try
                    {
                        transactionScope.Rollback();
                    }
                    catch (Exception ex1)
                    {
                        errorMsg = "Error:Exception occured." + ex1.StackTrace.ToString();
                    }

                }
                finally
                {
                    connection.Close();

                }

            }
            return errorMsg;
        }
         #endregion


       #region Brand List 

        public List<Brand_List> Get_Brand_Dtls()
       {
           List<Brand_List> brEntry = new List<Brand_List>();
         

           DataSet ds = new DataAccess(sqlConnection.GetConnectionString_SGX()).GetDataSet("[AGG].[USP_SELECT_BRAND_DTLS]", CommandType.StoredProcedure);

               DataTable dt = ds.Tables[0];
           
               if (ds != null && ds.Tables[0].Rows.Count > 0)
               {
                   foreach (DataRow Rows in ds.Tables[0].Rows)
                   {
                       brEntry.Add(new Brand_List
                       {
                           BRAND_ID = Convert.ToInt32(Rows["BRAND_ID"] == DBNull.Value ? 0 : Rows["BRAND_ID"]),
                           Material_Id = Convert.ToInt32(Rows["Material_Id"] == DBNull.Value ? 0 : Rows["Material_Id"]),
                           Material_Name = Convert.ToString(Rows["Material_Name"] == DBNull.Value ? "" : Rows["Material_Name"]),
                           Brand = Convert.ToString(Rows["BRAND_NAME"] == DBNull.Value ? "" : Rows["BRAND_NAME"]),
                           Brand_Code = Convert.ToString(Rows["BRAND_CODE"] == DBNull.Value ? "" : Rows["BRAND_CODE"]),
                           Rate = Convert.ToInt32(Rows["Rate"] == DBNull.Value ? 0 : Rows["Rate"]),
                           UOM_ID = Convert.ToInt32(Rows["UOM_ID"] == DBNull.Value ? 0 : Rows["UOM_ID"]),
                           UOM = Convert.ToString(Rows["UOM"] == DBNull.Value ? "" : Rows["UOM"]),
                           Effective_Dt = Convert.ToString(Rows["Effective_Dt"] == DBNull.Value ? "" : Rows["Effective_Dt"]),
                           Remark = Convert.ToString(Rows["REMARKS"] == DBNull.Value ? "" : Rows["REMARKS"]),
                       });
                   }
               }

           return brEntry;
       }

       #endregion

        #region Brand_Update

        public string Update_Brand(string Emp_Code, Brand_Entry EdBrand, out ResultMessage oblMsg)
       {      
           string errorMsg = "";
           oblMsg = new ResultMessage();

           using (var connection = new SqlConnection(sqlConnection.GetConnectionString_SGX()))
           {
               connection.Open();
               SqlCommand command;
               SqlTransaction transactionScope = null;
               transactionScope = connection.BeginTransaction(IsolationLevel.ReadCommitted);
               try
               {
                   SqlParameter[] param2 =
                         {
                           new SqlParameter("@ERRORSTR", SqlDbType.VarChar,200)
                         ,new SqlParameter("@BRAND_ID", SqlDbType.Int)
                         ,new SqlParameter("@BRAND_CODE" ,SqlDbType.VarChar,200)
                         ,new SqlParameter("@Material_Id", EdBrand.Material_Id)                     
                         ,new SqlParameter("@BRAND_NAME", EdBrand.Brand)
                         ,new SqlParameter("@Rate",EdBrand.Rate)
                         ,new SqlParameter("@UOM_ID", EdBrand.UOM_ID)
                         , new SqlParameter("@Effective_Dt", EdBrand.EffDate)
                         , new SqlParameter("@REMARKS", EdBrand.Remark)
                          ,new SqlParameter("@ADD_BY", Emp_Code)    
                         
                      };
                   param2[0].Direction = ParameterDirection.Output;
                   param2[1].Direction = ParameterDirection.Output;
                   param2[2].Direction = ParameterDirection.Output;
                   new DataAccess().InsertWithTransaction("[AGG].[USP_INSERT_BRAND]", CommandType.StoredProcedure, out command, connection, transactionScope, param2);
                   string error_2 = (string)command.Parameters["@ERRORSTR"].Value;
                   int BRAND_ID = (int)command.Parameters["@BRAND_ID"].Value;
                   string BRAND_CODE = Convert.ToString(command.Parameters["@BRAND_CODE"].Value);



                   if (error_2 != "") { errorMsg = error_2; }


                   if (EdBrand.oldBrandId > 0 && errorMsg=="")
                    {                          
                        SqlParameter[] param =
                        {
                        new SqlParameter("@ERRORSTR", SqlDbType.VarChar,200)
                        ,new SqlParameter("@BRAND_ID",EdBrand.oldBrandId)   
                      
                         ,new SqlParameter("@Updateby",Emp_Code)
                        };
                        param[0].Direction = ParameterDirection.Output;
                        new DataAccess().InsertWithTransaction("[AGG].[USP_BRAND_UPDATE]", CommandType.StoredProcedure, out command, connection, transactionScope, param);
                        string error_1 = (string)command.Parameters["@ERRORSTR"].Value;
                        if (error_1 != "") { errorMsg = error_1;  }                             
                    }

                   if (errorMsg == "")
                   {
                       oblMsg.ID = BRAND_ID;
                       oblMsg.CODE = EdBrand.Brand;
                       oblMsg.MsgType = "Success";
                       transactionScope.Commit();
                   }
                   else
                   {
                       oblMsg.Msg = errorMsg;
                       oblMsg.MsgType = "Error";
                       transactionScope.Rollback();
                   }
               }
               
                 catch (Exception ex)
                   {
                   try
                   {
                       transactionScope.Rollback();
                   }
                   catch (Exception ex1)
                   {
                       errorMsg = "Error: Exception occured. " + ex1.StackTrace.ToString();
                   }
               }
               finally
               {
                   connection.Close();
               }
           }
           return errorMsg;
       }
         #endregion

        #region
        public string Delete_Brand(string Emp_Code, Brand_Entry EdBrand, out ResultMessage oblMsg)
       {

           string errorMsg = "";
           oblMsg = new ResultMessage();
           using (var connection = new SqlConnection(sqlConnection.GetConnectionString_SGX()))
           {
               connection.Open();
               SqlCommand command;
               SqlTransaction transactionScope = null;
               transactionScope = connection.BeginTransaction(IsolationLevel.ReadCommitted);
               try
               {
                   if (EdBrand.BRAND_ID > 0)
                   {
                       SqlParameter[] param =
                        {
                          new SqlParameter("@ERRORSTR", SqlDbType.VarChar,200)
                         ,new SqlParameter("@BRAND_ID",EdBrand.BRAND_ID)   
                        
                         ,new SqlParameter("@Updateby",Emp_Code)
                        };
                       param[0].Direction = ParameterDirection.Output;
                       new DataAccess().InsertWithTransaction("[AGG].[USP_BRAND_UPDATE]", CommandType.StoredProcedure, out command, connection, transactionScope, param);
                       string error_1 = (string)command.Parameters["@ERRORSTR"].Value;
                       if (error_1 != "") { errorMsg = error_1; }
                   }

                   if (errorMsg == "")
                   {
                       oblMsg.ID =Convert.ToInt32(EdBrand.BRAND_ID);
                       oblMsg.MsgType = "Success";
                       transactionScope.Commit();
                   }
                   else
                   {
                       oblMsg.Msg = errorMsg;
                       oblMsg.MsgType = "Error";
                       transactionScope.Rollback();
                   }
               }

               catch (Exception ex)
               {
                   try
                   {
                       transactionScope.Rollback();
                   }
                   catch (Exception ex1)
                   {
                       errorMsg = "Error: Exception occured. " + ex1.StackTrace.ToString();
                   }
               }
               finally
               {
                   connection.Close();
               }
           }
           return errorMsg;
       }
        #endregion

    }
}
