﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Atomo.Utils;
using Microsoft.ApplicationBlocks.Data;
using Microsoft.Practices.EnterpriseLibrary.Caching;

namespace Atomo.Data
{
    public class ReflectionBase 
    {
        public ReflectionBase()
        {
        }

        public string Conn { get; set; }

        #region MultipleRowInsert

        public void MultipleRowsInsert<GenericType>(List<GenericType> ListValues, string SQLTable)
        {
            InsertToDatabase(
                             BuildTable<GenericType>(ListValues)
                            ,SQLTable);

        }

        private void InsertToDatabase(DataTable Data, string SQLTable)
        {
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(Conn))
            {
                bulkCopy.DestinationTableName = SQLTable;

                //BulkCopyAddMapping(bulkCopy, SQLTable);
                bulkCopy.BulkCopyTimeout = 240;

                try
                {
                    // Write from the source to the destination.
                    bulkCopy.WriteToServer(Data);
                }
                catch (Exception)
                {
                }
            }
        }

        private DataTable BuildTable<GenericType>(List<GenericType> ListValues)
        {
            Type t = typeof(GenericType);
            DataTable dt = new DataTable();
            DataRow dr;
            foreach (GenericType _item in ListValues)
            {
                dr = dt.NewRow();

                foreach (PropertyInfo property in t.GetProperties())
                {
                    if (!dt.Columns.Contains(property.Name))
                        dt.Columns.Add(property.Name);

                    dr[property.Name] = property.GetValue(_item, null);
                }

                dt.Rows.Add(dr);
            }

            return dt;
        }

        #endregion

        #region InsertRegion

        public void EncapsulatedInsert(object ParametersObject, string ProcedureName)
        {
            SingleInsertToDataBase(ParametersObject, ProcedureName);
        }

        public GenericType EncapsulatedInsert<GenericType>(object ParametersObject, string ProcedureName)
        {
            return (GenericType)SingleInsertToDataBase(ParametersObject, ProcedureName);
        }

        private object SingleInsertToDataBase(object ParametersObject, string ProcedureName)
        {
            try
            {
                SqlParameter[] parameters;
                if (ParametersObject != null)
                {
                    parameters = BindParameters(ParametersObject, ProcedureName);
                    return SqlHelper.ExecuteScalar(Conn, CommandType.StoredProcedure, ProcedureName, parameters);
                }
                //else
                    //SqlHelper.ExecuteNonQuery(Conn, CommandType.StoredProcedure, ProcedureName);

            }
            catch (Exception)
            {
                throw;
            }

            return null;
        }


        #endregion

        #region GenericReadofData

        #region public

        /// <summary>
        /// Used to extract a single object from a specific row of the DataTable.
        /// </summary>
        /// <typeparam name="GenericType">Type of object to extract</typeparam>
        /// <param name="ParametersObject">Object with suplied execution params for the procedure</param>
        /// <param name="ProcedureName">Name of the procedure to be executed</param>
        /// <param name="index">Index of the row in the DataTable to extract the object from</param>
        /// <returns>Populated typed business object.</returns>
        /// 



        //You inform Expiration Police
        public GenericType EncapsulatedRead<GenericType>(object ParametersObject, string ProcedureName, int RowIndex, params  ICacheItemExpiration[] expirations)
        {

            CacheBase cb = new CacheBase();
            object cachedObj = cb[ParametersObject];

            if (cachedObj != null)
                return (GenericType)cachedObj;
            else
            {
                GenericType ret = (GenericType)System.Activator.CreateInstance(typeof(GenericType));
                ExtractObject<GenericType>(ReturnDataTable(ParametersObject, ProcedureName), ret, RowIndex);

                return cb.Add<GenericType>(ParametersObject
                                                , ret
                                                , expirations);
            }

        }

        //With SqlDependency
        public GenericType EncapsulatedRead<GenericType>(object ParametersObject, string ProcedureName, int RowIndex, bool AllowCache)
        {
            if (AllowCache)
            {
                CacheBase cb = new CacheBase();
                object cachedObj = cb[ParametersObject];

                if (cachedObj != null)
                    return (GenericType)cachedObj;
                else
                {
                    GenericType ret = (GenericType)System.Activator.CreateInstance(typeof(GenericType));
                    ExtractObject<GenericType>(ReturnDataTable(ParametersObject, ProcedureName), ret, RowIndex);
                    SqlDependencyExpiration dependency = GetSQLDependency(ParametersObject,ProcedureName); 
                    return cb.Add<GenericType>(ParametersObject, ret, dependency);
                }
            }
            else
            {
                GenericType ret = (GenericType)System.Activator.CreateInstance(typeof(GenericType));
                ExtractObject<GenericType>(ReturnDataTable(ParametersObject, ProcedureName), ret, RowIndex);
                return ret;
            }
            
        }

        //You inform Expiration Police
        public List<GenericType> EncapsulatedRead<GenericType>(object ParametersObject, string ProcedureName, params  ICacheItemExpiration[] expirations)
        {
            CacheBase cb = new CacheBase();
            object cachedObj = cb[ParametersObject];

            if (cachedObj != null)
                return (List<GenericType>)cachedObj;
            else
                return cb.Add<List<GenericType>>(ParametersObject
                                                , EncapsulatedRead<GenericType>(ParametersObject, ProcedureName)
                                                , expirations);
        }

        //With SqlDependency
        public List<GenericType> EncapsulatedRead<GenericType>(object ParametersObject, string ProcedureName, bool AllowCache)
        {
            if (AllowCache)
            {
                CacheBase cb = new CacheBase();
                object cachedObj = cb[ParametersObject];

                if (cachedObj != null)
                    return (List<GenericType>)cachedObj;
                else
                    return cb.Add<List<GenericType>>(ParametersObject
                                                    , EncapsulatedRead<GenericType>(ParametersObject, ProcedureName)
                                                    , GetSQLDependency(ParametersObject, ProcedureName));
            }

            return EncapsulatedRead<GenericType>(ParametersObject, ProcedureName);
        }

        public List<GenericType> EncapsulatedRead<GenericType>(object ParametersObject, string ProcedureName)
        {
            return
                ExtractObjects<GenericType>(ReturnDataTable(ParametersObject, ProcedureName), typeof(GenericType));
        }
        
        #endregion

        #region private


        private GenericType MultObjectRead<GenericType>(object ParametersObject, string ProcedureName, params object[] InnerObjects)
        {
            GenericType ret = (GenericType)System.Activator.CreateInstance(typeof(GenericType));

            DataSet data = ReturnDataSet(ParametersObject, ProcedureName); ;

            ret = ExtractObject<GenericType>(data.Tables[0], ret, 0);

            for(int i = 0; i < InnerObjects.Length; i++)
	        {
                object tempObj = ExtractObjects<object>(data.Tables[i], InnerObjects[i].GetType());

                ret = (GenericType)EvaluateCustomInnerObject(ret, InnerObjects[i], data.Tables[i]);
	        }

            return ret;
        }


        private DataTable ReturnDataTable(object ParametersObject, string ProcedureName)
        {
            return ReturnDataSet(ParametersObject, ProcedureName).Tables[0];
        }

        private DataSet ReturnDataSet(object ParametersObject, string ProcedureName)
        {
            DataSet ds;

            try
            {
                SqlParameter[] parameters;
                if (ParametersObject != null)
                {
                    parameters = BindParameters(ParametersObject, ProcedureName);
                    ds = SqlHelper.ExecuteDataset(Conn, CommandType.StoredProcedure, ProcedureName, parameters);
                }
                else
                    ds = SqlHelper.ExecuteDataset(Conn, CommandType.StoredProcedure, ProcedureName);
                
            }
            catch (Exception)
            {
                throw;
            }

            return ds;
        }

        private SqlParameter[] BindParameters(object inputObject, string procedureName)
        {
            //Get the type of the inputObject (used later in the function)
            Type inputType = inputObject.GetType();

            //Get the parameters for the procedure using the Data Access Block
            SqlParameter[] parameters = SqlHelperParameterCache.GetSpParameterSet(Conn, procedureName);
            
            //Loop through the parameters and find all parameters of the procedure 
            // that match properties of our inputObject.
            foreach (SqlParameter parameter in parameters)
            {
                //Get the property. The SqlParameter.ParameterName will include the "@"
                // sign so we must remove it. Notice that the parameters of the stored 
                // procedure must exactly match the property values. It is possible to 
                // map properties of one name to parameters of another name, and that 
                // will likely be covered in a future article.
                PropertyInfo property = inputType.GetProperty(parameter.ParameterName.Replace("@", ""));

                
                if (property == null)
                    property = inputType.GetProperty(parameter.ParameterName.Replace("@", "").ToUpper());
                
                if (property == null)
                    property = inputType.GetProperty(parameter.ParameterName.Replace("@", "").ToLower());
                //If the property doesn't exist, we will have a null property reference, 
                // so we have to check for that here before setting the parameter's value 
                // from the property value.
                if (property != null)
                    parameter.Value = property.GetValue(inputObject, null);
            }

            return parameters;
        }

        /// <summary>
        /// Used to extract a single object from a specific row of the DataTable.
        /// </summary>
        /// <typeparam name="GenericType">Type of object to extract</typeparam>
        /// <param name="data">DataTable to extract object from</param>
        /// <param name="destination">Type of object to extract (i.e. typeof(Customer))</param>
        /// <param name="index">Index of the row in the DataTable to extract the object from</param>
        /// <returns>Populated business object.</returns>
        private GenericType ExtractObject<GenericType>(DataTable data, GenericType destination, int index)
        {
            //Get the row we need to extract data from.
            DataRow row = data.Rows[index];

            //Call GetValidProperties to get the properties of the business object
            // which match fields in the DataTable. We are left with a PropertyInfo
            // array after this call. The result of this call can actually be cached
            // into a Dictionary object for better performance on subsequent calls.
            List<PropertyInfo> validProperties = GetValidProperties(destination, data);

            //Loop through the PropertyInfo array and use the individual PropertyInfo
            // objects to populate each property of our business object.
            foreach (PropertyInfo property in validProperties)
            {
                //Make sure that the property has a "set" accessor.
                if (row[property.Name] != System.DBNull.Value && property.CanWrite)
                {
                    //Set the value of the property to the value of the field in the DataRow
                    property.SetValue(destination, ConvertionFunctions.Parse(row[property.Name.ToLower()],property.PropertyType.FullName), null);
                }
            }

            //Return the fully populated business object.
            return destination;
        }

        /// <summary>
        /// Used to extract a list of objects out of a DataTable
        /// </summary>
        /// <typeparam name="GenericType">Type of object to extract</typeparam>
        /// <param name="data">DataTable to extract objects from</param>
        /// <param name="destinationType">Type of object to extract</param>
        /// <returns>List of objects populated from a DataTable</returns>
        private List<GenericType> ExtractObjects<GenericType>(DataTable data, Type destinationType)
        {
            //First we make a generic list that will contain our result objects.
            List<GenericType> objects = new List<GenericType>();

            //We then loop through each row of the DataTable and extract a single
            // business object for each row.
            for (int i = 0; data != null && i < data.Rows.Count; i++)
            {
                //Notice in this line that I am using System.Activator.CreateInstance().
                // This is an extremely useful function when you need to dynamically
                // instantiate objects.
                objects.Add(this.ExtractObject<GenericType>(data, (GenericType)System.Activator.CreateInstance(destinationType), i));
            }

            //Return the object list
            return objects;
        }

        /// <summary>
        /// Determines which properties match between the rows in the DataTable and
        /// the business object.
        /// </summary>
        /// <param name="destination">Type of object being extracted from the DataTable</param>
        /// <param name="data">DataTable to extract data from</param>
        /// <returns>An array of PropertyInfo objects</returns>
        private List<PropertyInfo> GetValidProperties(object destination, DataTable data)
        {
            //Please note that in this example the name of the field in 
            // the DataTable must match the name of the property in our
            // business object. While this is a limitation, it can be
            // overcome. One method to overcome this limitation is to
            // use custom attributes to 'map' properties of the class to
            // specific fields of the DataTable. If you need this type
            // of functionality, please contact me at zs_box@hotmail.com.

            //Get the Type of the object we're creating
            Type destinationType = destination.GetType();

            //Create a generic list to hold PropertyInfo objects
            List<PropertyInfo> validProperties = new List<PropertyInfo>();

            //Create a generic list to hold our column names
            List<string> validColumns = new List<string>();

            //Add a column to the validColumns list for each column in the DataTable
            foreach (DataColumn column in data.Columns)
                validColumns.Add(column.ColumnName.ToLower());

            //Go through the properties of the object we're going to put
            // data into and when we find a property that has a corresponding
            // field in the DataTable, add the PropertyInfo object to the
            // validProperties list.
            foreach (PropertyInfo property in destinationType.GetProperties())
                if (validColumns.Contains(property.Name.ToLower()))
                    validProperties.Add(property);

            //Return the valid properties.
            return validProperties;
        }

        private object EvaluateCustomInnerObject(object destination, object InnerObject, DataTable data)
        {
            Type destinationType = destination.GetType();

            object inner = System.Activator.CreateInstance(InnerObject.GetType());

            inner = ExtractObjects<object>(data, inner.GetType());
            
            destinationType.GetProperty(InnerObject.GetType().Name).SetValue(destination,inner,null);

            return destination;
        }

        private SqlDependencyExpiration GetSQLDependency(object ParametersObject, string ProcedureName)
        {
            using (SqlConnection conn = new SqlConnection(Conn))
            {
                SqlCommand command = new SqlCommand(ProcedureName, conn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddRange(BindParameters(ParametersObject, ProcedureName));
                conn.Open();
                SqlDependencyExpiration dependency = new SqlDependencyExpiration(command);
                command.ExecuteNonQuery();
                return dependency;
            }
        }
        
        #endregion

        #endregion
    }
}