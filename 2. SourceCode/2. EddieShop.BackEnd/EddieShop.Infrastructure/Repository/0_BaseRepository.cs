using Dapper;
using EddieShop.Core.Entities;
using EddieShop.Core.Entities.Common;
using EddieShop.Core.Interfaces.Base;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Text;
using static EddieShop.Core.Attributes.EddieShopAttributes;

namespace EddieShop.Infrastructure.Repository
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity>
    {
        #region DECLARE
        protected IConfiguration _configuration;
        protected IDbConnection _dbConnection;
        protected string _connectionString = string.Empty;
        private string _className;

        #endregion

        #region CONSTRUCTOR
        public BaseRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = configuration.GetConnectionString("EddieShopConnection");
            _className = typeof(TEntity).Name;
        }
        #endregion

        #region METHOD
        #region GetAll
        /// <summary>
        /// Lấy toàn bộ dữ liệu
        /// </summary>
        /// <param name="sessionID"></param>
        /// <returns></returns>  
        /// CreatedBy: NTDUNG(17/8/2021)
        /// ModifiedBy: NTDUNG(17/8/2021)
        public virtual IEnumerable<TEntity> GetAllEntities(Guid? sessionID)
        {
            using (_dbConnection = new MySqlConnection(_connectionString))
            {
                var sqlCommand = $"SELECT * from {_className} ORDER BY CreatedDate DESC";
                var entities = _dbConnection.Query<TEntity>(sqlCommand);
                return entities;

            }

        }
        #endregion

        #region GetByID
        /// <summary>
        /// Lấy thông tin theo Id
        /// </summary>
        /// <param name="entityId">Id đối tượng</param>
        /// <param name="sessionID"></param>
        /// <returns></returns>
        /// CreatedBy: NTDUNG(17/8/2021)
        /// ModifiedBy: NTDUNG(17/8/2021) 
        public virtual TEntity GetEntityById(Guid entityId, Guid? sessionID, bool? mode)
        {
            using (_dbConnection = new MySqlConnection(_connectionString))
            {
                DynamicParameters dynamicParameters = new DynamicParameters();
                dynamicParameters.Add($"@{_className}ID", entityId);
                if (mode == null || (bool)!mode)
                {
                    var sqlCommand = $"SELECT * from {_className} WHERE {_className}ID = @{_className}ID";
                    return _dbConnection.QueryFirstOrDefault<TEntity>(sqlCommand, param: dynamicParameters);
                } else
                {
                    var procedure = $"Proc_Get{_className}ById";
                    return _dbConnection.QueryFirstOrDefault<TEntity>(procedure, param: dynamicParameters, commandType: CommandType.StoredProcedure);
                }
            }
        }
        #endregion

        #region GetByProperties
        /// <summary>
        /// Lấy thông tin theo property
        /// </summary>
        /// <param name="sessionID"></param>
        /// <returns></returns>
        /// CreatedBy: NTDUNG(23/11/2021)
        public virtual TEntity GetEntityByProperties(object columnsGet, Guid? sessionID)
        {
            using (_dbConnection = new MySqlConnection(_connectionString))
            { 
                var queryLine = new List<string>();
                DynamicParameters dynamicParameters = new DynamicParameters();
                var properties = columnsGet.GetType().GetProperties();
                foreach (var property in properties)
                {
                    var propName = property.Name;
                    var propValue = property.GetValue(columnsGet); 

                    queryLine.Add($"{propName} = @{propName}");
                    dynamicParameters.Add($"@{propName}", propValue);
                }

                var sqlCommand = $"SELECT * FROM {_className} WHERE {String.Join(" AND ", queryLine.ToArray())} ";
                var entity = _dbConnection.QueryFirstOrDefault<TEntity>(sqlCommand, param: dynamicParameters);
                return entity;
            }
        }
        #endregion

        #region GetByValueColumns
        /// <summary>
        /// Lấy thông tin theo các cột có giá trị
        /// </summary>
        /// <param name="sessionID"></param>
        /// <returns></returns>
        /// CreatedBy: NTDUNG(23/11/2021)
        public virtual List<TEntity> GetByValueColumns(TEntity columnsGet, Guid? sessionID)
        {
            using (_dbConnection = new MySqlConnection(_connectionString))
            {
                var queryLine = new List<string>();
                DynamicParameters dynamicParameters = new DynamicParameters();
                var properties = columnsGet.GetType().GetProperties();
                foreach (var property in properties)
                {
                    var propName = property.Name;
                    var propValue = property.GetValue(columnsGet, null);
                    if (propValue != null)
                    {
                        queryLine.Add($"{propName} = @{propName}");
                        dynamicParameters.Add($"@{propName}", propValue);
                    }
                }

                var sqlCommand = $"SELECT * FROM {_className} WHERE {String.Join(" AND ", queryLine.ToArray())} ";
                var entity = _dbConnection.Query<TEntity>(sqlCommand, param: dynamicParameters);
                return entity.ToList();
            }
        }
        #endregion

        #region GetByIds
        /// <summary>
        /// Lấy theo các id
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="sessionID"></param>
        /// <returns></returns>
        /// CreatedBy: NTDUNG (24/11/2021)
        public virtual List<TEntity> GetByIds(List<Guid> ids, Guid? sessionID)
        {
            using (_dbConnection = new MySqlConnection(_connectionString))
            {
                DynamicParameters dynamicParameters = new DynamicParameters();
                var listIds = ids.Select(id => $"'{id}'").ToList();
                var sqlCommand = $"SELECT * FROM {_className} WHERE {_className}ID IN ({String.Join(", ", listIds)})";
                var entity = _dbConnection.Query<TEntity>(sqlCommand, param: dynamicParameters);
                return entity.ToList();
            }
        }
        #endregion

        #region Insert
        /// <summary>
        /// Thêm mới
        /// </summary>
        /// <param name="entity">Thông tin được thêm</param>
        /// <returns>số bản ghi được thêm</returns>
        /// CreatedBy: NTDUNG(17/8/2021)
        /// ModifiedBy: NTDUNG(17/8/2021)
        public virtual int Insert(TEntity entity)
        {
            MySqlConnection mySqlConnection = null;
            IDbTransaction transaction = null;
            var rowEffects = -1;

            try
            {
                mySqlConnection = new MySqlConnection(_connectionString);
                mySqlConnection.Open();
                transaction = mySqlConnection.BeginTransaction();

                var columnsName = new List<string>();
                var columnsParam = new List<string>();
                DynamicParameters dynamicParameters = new DynamicParameters();
                var properties = entity.GetType().GetProperties();
                foreach (var property in properties)
                {
                    if (property.IsDefined(typeof(EddieNotMap), false)) continue;
                    var propName = property.Name;
                    var propValue = property.GetValue(entity);

                    // Bổ sung ngày tạo mới
                    if (propName == "CreatedDate")
                    {
                        propValue = DateTime.Now;
                    }

                    columnsName.Add(propName);
                    columnsParam.Add($"@{propName}");
                    dynamicParameters.Add($"@{propName}", propValue);

                }
                var sqlQuery = $"INSERT INTO {_className} ({String.Join(", ", columnsName.ToArray())}) " +
                            $"VALUES({String.Join(", ", columnsParam.ToArray())})";
                rowEffects = mySqlConnection.Execute(sqlQuery, param: dynamicParameters, transaction: transaction);

                transaction.Commit();
            }
            catch(Exception)
            {
                if (transaction != null)
                {
                    transaction.Rollback();
                }
                throw;
            }
            finally
            {
                if (mySqlConnection != null) mySqlConnection.Close();
            }
            return rowEffects;
        }
        #endregion

        #region Update
        /// <summary>
        /// Sửa thông tin
        /// </summary>
        /// <param name="entity">Thông tin cần sửa</param>
        /// <param name="entityId">Id </param>
        /// <param name="sessionID"></param>
        /// <returns>số bản ghi được sửa</returns>
        /// CreatedBy: NTDUNG(17/8/2021)
        /// ModifiedBy: NTDUNG(17/8/2021)
        public virtual int Update(TEntity entity, Guid entityId, Guid? sessionID)
        {
            MySqlConnection mySqlConnection = null;
            IDbTransaction transaction = null;
            var rowEffects = -1;

            try
            {
                mySqlConnection = new MySqlConnection(_connectionString);
                mySqlConnection.Open();
                transaction = mySqlConnection.BeginTransaction();

                var queryLine = new List<string>();
                DynamicParameters dynamicParameters = new DynamicParameters();
                var properties = entity.GetType().GetProperties();
                foreach (var property in properties)
                {
                    if (property.IsDefined(typeof(EddieNotMap), false)) continue;
                    var propName = property.Name;
                    var propValue = property.GetValue(entity);
                    // Gán Id cũ
                    if (propName.Equals($"{_className}ID") && property.PropertyType == typeof(Guid))
                    {
                        propValue = entityId;
                    }
                    // Bổ sung ngày chỉnh sửa
                    if (propName == "ModifiedDate")
                    {
                        propValue = DateTime.Now;
                    }

                    queryLine.Add($"{propName} = @{propName}");
                    dynamicParameters.Add($"@{propName}", propValue);
                }

                dynamicParameters.Add("@oldEntityId", entityId);
                var sqlQuery = $"UPDATE {_className} SET {String.Join(", ", queryLine.ToArray())} " +
                                $"WHERE {_className}Id = @oldEntityId";
                rowEffects = mySqlConnection.Execute(sqlQuery, param: dynamicParameters, transaction: transaction);

                transaction.Commit();
            }
            catch (Exception)
            {
                if (transaction != null)
                {
                    transaction.Rollback();
                }
                throw;
            }
            finally
            {
                if (mySqlConnection != null) mySqlConnection.Close();
            }
            return rowEffects;
        }
        #endregion

        #region DeleteOne
        /// <summary>
        /// Xóa theo Id
        /// </summary>
        /// <param name="entityId">Id </param>
        /// <param name="sessionID"></param>
        /// <returns>Số bản ghi được xóa</returns>
        /// CreatedBy: NTDUNG(27/8/2021)
        /// ModifiedBy: NTDUNG(27/8/2021)
        public virtual int Delete(Guid entityId, Guid? sessionID)
        {
            using (_dbConnection = new MySqlConnection(_connectionString))
            {
                var sqlCommand = $"DELETE FROM {_className} WHERE {_className}ID = @{_className}ID";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add($"@{_className}ID", entityId);
                var rowEffects = _dbConnection.Execute(sqlCommand, param: parameters);
                return rowEffects;
            }

        }
        #endregion

        #region DeleteMany

        /// <summary>
        /// Xóa nhiều 
        /// </summary>
        /// <param name="entityIds"> mảng chứa các Id</param>
        /// <param name="sessionID"></param>
        /// <returns></returns>
        /// CreatedBy: NTDUNG(19/8/2021)
        /// ModifiedBy: NTDUNG(19/8/2021)
        public virtual int DeleteMultiple(List<Guid> entityIds, Guid? sessionID)
        {
            MySqlConnection mySqlConnection = null;
            IDbTransaction transaction = null;
            var rowEffects = -1;
            var parameters = new DynamicParameters();
            var paramName = new List<string>();
            
            try
            {
                mySqlConnection = new MySqlConnection(_connectionString);
                mySqlConnection.Open();
                transaction = mySqlConnection.BeginTransaction();

                for (int i = 0; i < entityIds.Count; i++)
                {
                    var id = entityIds[i];
                    // Đặt tên cho param
                    paramName.Add($"@mID{i}");
                    // Đặt giá trị cho param bằng id 
                    parameters.Add($"@mID{i}", id);
                }
                // Join mảng để tạo ra câu truy vấn xoá nhiều
               
                var sqlCommand = $"DELETE FROM {_className} WHERE {_className}ID IN ({String.Join(", ", paramName.ToArray())})";
                rowEffects = mySqlConnection.Execute(sqlCommand, param: parameters, transaction: transaction);

                transaction.Commit();
            }
            catch(Exception)
            {
                if (transaction != null)
                {
                    transaction.Rollback();
                }
                throw;
            }
            finally
            {
                if (mySqlConnection != null) mySqlConnection.Close();
            }
            return rowEffects;
            
        }
        #endregion

        #region CheckDuplicate
        /// <summary>
        /// Kiểm tra trùng lặp
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="fieldName"></param>
        /// <param name="mode"></param>
        /// <param name="sessionID"></param>
        /// <returns></returns>
        public virtual bool CheckDuplicate(TEntity entity, string fieldName, string mode, Guid? sessionID)
        {
            using (_dbConnection = new MySqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                var sqlQuery = "";

                parameters.Add("@fieldValue", entity.GetType().GetProperty(fieldName).GetValue(entity, null));
                parameters.Add("@fieldName", fieldName);
                parameters.Add($"@{_className}ID", (entity.GetType().GetProperty($"{_className}ID").GetValue(entity, null)));

                if (mode == "ADD")
                {
                    sqlQuery = $"SELECT * FROM {_className} WHERE {fieldName} = @fieldValue";
                }
                else if (mode == "UPDATE")
                {
                    sqlQuery = $"SELECT * FROM {_className} WHERE {fieldName} = @fieldValue AND {_className}ID != @{_className}ID";
                }

                // Lấy dữ liệu và phản hồi cho client
                var queryField = _dbConnection.Query<TEntity>(sqlQuery, param: parameters, commandType: CommandType.Text);

                return queryField.ToList().Count() >= 1;
            }
        }
        #endregion

        #region GetNewCode
        /// <summary>
        /// Lấy mã mới
        /// </summary>
        /// <param name="sessionID"></param>
        /// <returns></returns>
        /// CreatedBy: NTDUNG(07/10/2021)
        public virtual string GetNewCode(Guid? sessionID)
        {

            using (_dbConnection = new MySqlConnection(_connectionString))
            {
                var proceduce = $"Proc_GetNew{_className}Code";
                var newCode = _dbConnection.Query<string>(proceduce, commandType: CommandType.StoredProcedure);
                return newCode.ToList()[0];
            }
        }
        #endregion

        #region GetFilterPaging
        /// <summary>
        /// Lọc và phân trang
        /// </summary>
        /// <param name="filterString"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="sessionID"></param>
        /// <returns></returns>
        /// CreatedBy: NTDUNG(27/10/2021)
        public virtual Object GetFilterPaging(string filterString, int pageNumber, int pageSize, FilterData filterData, Guid? sessionID, string storeCustom)
        {
            var totalFields = filterData.TotalFields;
            var rangeDates = filterData.RangeDates;
            var sorts = filterData.Sorts;
            var conditions = filterData.Conditions;

            using(_dbConnection = new MySqlConnection(_connectionString))
            {
                DynamicParameters dynamicParameters = new DynamicParameters();
                if (filterString == null)
                {
                    filterString = "";
                }
                dynamicParameters.Add("@filterString", filterString);
                dynamicParameters.Add("@pageStart", (pageNumber - 1) * pageSize);
                dynamicParameters.Add("@pageSize", pageSize);
                dynamicParameters.Add("@sessionID", sessionID);

                var proceduceFilterPaging = $"Proc_Get{_className}FilterPaging";
                var proceduceFilter = $"Proc_Get{_className}Filter";
                if (storeCustom != "" && storeCustom != null)
                {
                    proceduceFilter = storeCustom;
                    proceduceFilterPaging = storeCustom;
                }
                var entitiesFilterPaging = _dbConnection.Query<TEntity>(proceduceFilterPaging, param: dynamicParameters, commandType: CommandType.StoredProcedure);
                var entitiesFilter = _dbConnection.Query<TEntity>(proceduceFilter, param: dynamicParameters, commandType: CommandType.StoredProcedure);
                
                // Lọc theo điều kiện
                if (conditions.Count() > 0)
                {
                    var tempEntities = new List<TEntity>();
                    foreach (var record in entitiesFilterPaging)
                    {
                        foreach (var (condition, i) in conditions.Select((condition, i) => (condition, i)))
                        {
                            var valid = true;
                            switch (condition.Type)
                            {
                                case "Guid":
                                    if (condition.Method == "Equal")
                                    {
                                        if (record.GetType().GetProperty(condition.FieldName).GetValue(record).ToString() == condition.Value)
                                        {
                                            if (i == conditions.Count() - 1)
                                                tempEntities.Add(record);
                                        } else
                                        {
                                            valid = false;
                                        }
                                    } 
                                    break;
                                default:
                                    tempEntities.Add(record);
                                    break;
                            }

                            if (!valid) break;
                        } 
                    }
                    entitiesFilterPaging = tempEntities;
                }

                // Sắp xếp
                entitiesFilterPaging = SortData(entitiesFilterPaging.ToList(), sorts);

                // Truy vấn trong khoảng thời gian
                if (rangeDates.Count() > 0) { 
                    entitiesFilterPaging = FilterInRange(entitiesFilterPaging.ToList(), rangeDates);
                    entitiesFilter = FilterInRange(entitiesFilter.ToList(), rangeDates);
                } 

                var totalRecord = entitiesFilter.ToList().Count();

                // Tổng số trang bằng phần nguyên của tổng bán ghi chia cho kích thước, nếu số dư là khác 0 thì cộng thêm 1
                var totalPage = (int)(totalRecord / pageSize) + ((totalRecord % pageSize != 0) ? 1 : 0);

                // Tính toán tổng cho các trường cần tính tổng
                dynamic totalDatas = new ExpandoObject();
                if (entitiesFilter.ToList().Count() > 0)
                {
                    foreach (var field in totalFields)
                    {
                        decimal totalInPage = 0;
                        decimal totalAll = 0;
                        var properties = entitiesFilter.ToList()[0].GetType().GetProperties();
                        var checkField = false;
                        // Kiểm tra có trường này không
                        foreach (var property in properties)
                        {
                            var propName = property.Name;
                            if (propName == field)
                            {
                                checkField = true;
                                break;
                            }
                        }
                        if (checkField)
                        {
                            foreach (var entities in entitiesFilterPaging)
                            {
                                var value = entities.GetType().GetProperty(field).GetValue(entities);
                                if (value != null)
                                    totalInPage += Convert.ToDecimal(value);
                            };
                            foreach (var entities in entitiesFilter)
                            {
                                var value = entities.GetType().GetProperty(field).GetValue(entities);
                                if (value != null)
                                    totalAll += Convert.ToDecimal(value);
                            };
                            var totalData = new
                            {
                                InPage = totalInPage,
                                All = totalAll
                            };
                            AddProperty(totalDatas, field, totalData, sessionID);
                        }
                    }
                }
                
                // Trả về
                var filterResult = new
                {
                    TotalPage = totalPage,
                    TotalRecord = totalRecord,
                    TotalDatas = totalDatas,
                    Records = pageSize == -1 ? entitiesFilter : entitiesFilterPaging
                };

                return filterResult;
            }    
        }
        #region SortData
        /// <summary>
        /// 
        /// </summary>
        /// <param name="listEntity"></param>
        /// <param name="sorts"></param>
        /// <returns></returns>
        /// CreatedBy: NTDUNG (23/12/2021)
        public List<TEntity> SortData(List<TEntity> listEntity, List<Sort> sorts)
        {
            var result = listEntity;

            switch (sorts.Count())
            {
                case 0:
                    result = (List<TEntity>)result.OrderByDescending(t => t.GetType().GetProperty("CreatedDate").GetValue(t)).ToList();
                    break;
                case 1:
                    result = Sort1(result, sorts);
                    break;
                case 2:
                    result = Sort2(result, sorts);
                    break;
                case 3:
                    result = Sort3(result, sorts);
                    break;
                case 4:
                    result = Sort4(result, sorts);
                    break;
                default:
                    result = new List<TEntity>();
                    result[0] = listEntity[0];
                    result[1] = listEntity[1];
                    result[2] = listEntity[2];
                    result[3] = listEntity[3];
                    result = Sort4(result, sorts);
                    break;
            }
            
            return result;
        }
        /// <summary>
        /// Sắp xếp 1 trường
        /// </summary>
        /// <param name="listEntity"></param>
        /// <param name="sorts"></param>
        /// <returns></returns>
        /// CreatedBy: NTDUNG (23/12/2021)
        public List<TEntity> Sort1(List<TEntity> listEntity, List<Sort> sorts)
        {
            var result = listEntity;
            if (sorts.ElementAt(0).Desc)
                result = (List<TEntity>)result.OrderByDescending(t => t.GetType().GetProperty(sorts.ElementAt(0).Field).GetValue(t)).ToList();
            else 
                result = (List<TEntity>)result.OrderBy(t => t.GetType().GetProperty(sorts.ElementAt(0).Field).GetValue(t)).ToList();
            return result;
        }
        /// <summary>
        /// Sắp xếp 2 trường
        /// </summary>
        /// <param name="listEntity"></param>
        /// <param name="sorts"></param>
        /// <returns></returns>
        /// CreatedBy: NTDUNG (23/12/2021)
        public List<TEntity> Sort2(List<TEntity> listEntity, List<Sort> sorts)
        {
            var result = listEntity;
            var field0 = sorts.ElementAt(0);
            var field1 = sorts.ElementAt(1);
                
            if (field0.Desc && field1.Desc)
                result = (List<TEntity>)result.OrderByDescending(t => t.GetType().GetProperty(field0.Field).GetValue(t))
                    .ThenByDescending(t => t.GetType().GetProperty(field1.Field).GetValue(t)).ToList();
            else if (field0.Desc && !field1.Desc) 
                result = (List<TEntity>)result.OrderByDescending(t => t.GetType().GetProperty(field0.Field).GetValue(t))
                    .ThenBy(t => t.GetType().GetProperty(field1.Field).GetValue(t)).ToList();
            else if (!field0.Desc && field1.Desc)
                result = (List<TEntity>)result.OrderBy(t => t.GetType().GetProperty(field0.Field).GetValue(t))
                    .ThenByDescending(t => t.GetType().GetProperty(field1.Field).GetValue(t)).ToList();
            else
                result = (List<TEntity>)result.OrderBy(t => t.GetType().GetProperty(field0.Field).GetValue(t))
                    .ThenBy(t => t.GetType().GetProperty(field1.Field).GetValue(t)).ToList();

            return result;
        }
        /// <summary>
        /// Sắp xếp 3 trường
        /// </summary>
        /// <param name="listEntity"></param>
        /// <param name="sorts"></param>
        /// <returns></returns>
        /// CreatedBy: NTDUNG (23/12/2021)
        public List<TEntity> Sort3(List<TEntity> listEntity, List<Sort> sorts)
        {
            var result = listEntity;
            var field0 = sorts.ElementAt(0);
            var field1 = sorts.ElementAt(1);
            var field2 = sorts.ElementAt(2);

            if (field0.Desc && field1.Desc)
            {
                if (field2.Desc)
                    result = (List<TEntity>)result.OrderByDescending(t => t.GetType().GetProperty(field0.Field).GetValue(t))
                        .ThenByDescending(t => t.GetType().GetProperty(field1.Field).GetValue(t))
                        .ThenByDescending(t => t.GetType().GetProperty(field2.Field).GetValue(t)).ToList();
                else
                    result = (List<TEntity>)result.OrderByDescending(t => t.GetType().GetProperty(field0.Field).GetValue(t))
                        .ThenByDescending(t => t.GetType().GetProperty(field1.Field).GetValue(t))
                        .ThenBy(t => t.GetType().GetProperty(field2.Field).GetValue(t)).ToList();
            }
            else if (field0.Desc && !field1.Desc)
            {
                if (field2.Desc)
                    result = (List<TEntity>)result.OrderByDescending(t => t.GetType().GetProperty(field0.Field).GetValue(t))
                        .ThenBy(t => t.GetType().GetProperty(field1.Field).GetValue(t))
                        .ThenByDescending(t => t.GetType().GetProperty(field2.Field).GetValue(t)).ToList();
                else
                    result = (List<TEntity>)result.OrderByDescending(t => t.GetType().GetProperty(field0.Field).GetValue(t))
                        .ThenBy(t => t.GetType().GetProperty(field1.Field).GetValue(t))
                        .ThenBy(t => t.GetType().GetProperty(field2.Field).GetValue(t)).ToList();
            }
            else if (!field0.Desc && field1.Desc)
            {
                if (field2.Desc)
                    result = (List<TEntity>)result.OrderBy(t => t.GetType().GetProperty(field0.Field).GetValue(t))
                        .ThenByDescending(t => t.GetType().GetProperty(field1.Field).GetValue(t))
                        .ThenByDescending(t => t.GetType().GetProperty(field2.Field).GetValue(t)).ToList();
                else
                    result = (List<TEntity>)result.OrderBy(t => t.GetType().GetProperty(field0.Field).GetValue(t))
                        .ThenByDescending(t => t.GetType().GetProperty(field1.Field).GetValue(t))
                        .ThenBy(t => t.GetType().GetProperty(field2.Field).GetValue(t)).ToList();
            }
            else
            {
                if (field2.Desc)
                    result = (List<TEntity>)result.OrderBy(t => t.GetType().GetProperty(field0.Field).GetValue(t))
                        .ThenBy(t => t.GetType().GetProperty(field1.Field).GetValue(t))
                        .ThenByDescending(t => t.GetType().GetProperty(field2.Field).GetValue(t)).ToList();
                else
                    result = (List<TEntity>)result.OrderBy(t => t.GetType().GetProperty(field0.Field).GetValue(t))
                        .ThenBy(t => t.GetType().GetProperty(field1.Field).GetValue(t))
                        .ThenBy(t => t.GetType().GetProperty(field2.Field).GetValue(t)).ToList();
            }

            return result;

        }
        /// <summary>
        /// Sắp xếp 4 trường
        /// </summary>
        /// <param name="listEntity"></param>
        /// <param name="sorts"></param>
        /// <returns></returns>
        /// CreatedBy: NTDUNG (23/12/2021)
        public List<TEntity> Sort4(List<TEntity> listEntity, List<Sort> sorts)
        {
            var result = listEntity;
            var field0 = sorts.ElementAt(0);
            var field1 = sorts.ElementAt(1);
            var field2 = sorts.ElementAt(2);
            var field3 = sorts.ElementAt(3);

            if (field0.Desc && field1.Desc)
            {
                if (field2.Desc)
                {
                    if (field3.Desc) 
                        result = (List<TEntity>)result.OrderByDescending(t => t.GetType().GetProperty(field0.Field).GetValue(t))
                            .ThenByDescending(t => t.GetType().GetProperty(field1.Field).GetValue(t))
                            .ThenByDescending(t => t.GetType().GetProperty(field2.Field).GetValue(t))
                            .ThenByDescending(t => t.GetType().GetProperty(field3.Field).GetValue(t)).ToList();
                    else
                        result = (List<TEntity>)result.OrderByDescending(t => t.GetType().GetProperty(field0.Field).GetValue(t))
                            .ThenByDescending(t => t.GetType().GetProperty(field1.Field).GetValue(t))
                            .ThenByDescending(t => t.GetType().GetProperty(field2.Field).GetValue(t))
                            .ThenBy(t => t.GetType().GetProperty(field3.Field).GetValue(t)).ToList();
                } else
                {
                    if (field3.Desc)
                        result = (List<TEntity>)result.OrderByDescending(t => t.GetType().GetProperty(field0.Field).GetValue(t))
                            .ThenByDescending(t => t.GetType().GetProperty(field1.Field).GetValue(t))
                            .ThenBy(t => t.GetType().GetProperty(field2.Field).GetValue(t))
                            .ThenByDescending(t => t.GetType().GetProperty(field3.Field).GetValue(t)).ToList();
                    else
                        result = (List<TEntity>)result.OrderByDescending(t => t.GetType().GetProperty(field0.Field).GetValue(t))
                            .ThenByDescending(t => t.GetType().GetProperty(field1.Field).GetValue(t))
                            .ThenBy(t => t.GetType().GetProperty(field2.Field).GetValue(t))
                            .ThenBy(t => t.GetType().GetProperty(field3.Field).GetValue(t)).ToList();
                }                 
            }
            else if (field0.Desc && !field1.Desc)
            {
                if (field2.Desc)
                {
                    if (field3.Desc)
                        result = (List<TEntity>)result.OrderByDescending(t => t.GetType().GetProperty(field0.Field).GetValue(t))
                            .ThenBy(t => t.GetType().GetProperty(field1.Field).GetValue(t))
                            .ThenByDescending(t => t.GetType().GetProperty(field2.Field).GetValue(t))
                            .ThenByDescending(t => t.GetType().GetProperty(field3.Field).GetValue(t)).ToList();
                    else
                        result = (List<TEntity>)result.OrderByDescending(t => t.GetType().GetProperty(field0.Field).GetValue(t))
                            .ThenBy(t => t.GetType().GetProperty(field1.Field).GetValue(t))
                            .ThenByDescending(t => t.GetType().GetProperty(field2.Field).GetValue(t))
                            .ThenBy(t => t.GetType().GetProperty(field3.Field).GetValue(t)).ToList();
                }
                else
                {
                    if (field3.Desc)
                        result = (List<TEntity>)result.OrderByDescending(t => t.GetType().GetProperty(field0.Field).GetValue(t))
                            .ThenBy(t => t.GetType().GetProperty(field1.Field).GetValue(t))
                            .ThenBy(t => t.GetType().GetProperty(field2.Field).GetValue(t))
                            .ThenByDescending(t => t.GetType().GetProperty(field3.Field).GetValue(t)).ToList();
                    else
                        result = (List<TEntity>)result.OrderByDescending(t => t.GetType().GetProperty(field0.Field).GetValue(t))
                            .ThenBy(t => t.GetType().GetProperty(field1.Field).GetValue(t))
                            .ThenBy(t => t.GetType().GetProperty(field2.Field).GetValue(t))
                            .ThenBy(t => t.GetType().GetProperty(field3.Field).GetValue(t)).ToList();
                }
            }
            else if (!field0.Desc && field1.Desc)
            {
                if (field2.Desc)
                {
                    if (field3.Desc)
                        result = (List<TEntity>)result.OrderBy(t => t.GetType().GetProperty(field0.Field).GetValue(t))
                            .ThenByDescending(t => t.GetType().GetProperty(field1.Field).GetValue(t))
                            .ThenByDescending(t => t.GetType().GetProperty(field2.Field).GetValue(t))
                            .ThenByDescending(t => t.GetType().GetProperty(field3.Field).GetValue(t)).ToList();
                    else
                        result = (List<TEntity>)result.OrderBy(t => t.GetType().GetProperty(field0.Field).GetValue(t))
                            .ThenByDescending(t => t.GetType().GetProperty(field1.Field).GetValue(t))
                            .ThenByDescending(t => t.GetType().GetProperty(field2.Field).GetValue(t))
                            .ThenBy(t => t.GetType().GetProperty(field3.Field).GetValue(t)).ToList();
                }
                else
                {
                    if (field3.Desc)
                        result = (List<TEntity>)result.OrderBy(t => t.GetType().GetProperty(field0.Field).GetValue(t))
                            .ThenByDescending(t => t.GetType().GetProperty(field1.Field).GetValue(t))
                            .ThenBy(t => t.GetType().GetProperty(field2.Field).GetValue(t))
                            .ThenByDescending(t => t.GetType().GetProperty(field3.Field).GetValue(t)).ToList();
                    else
                        result = (List<TEntity>)result.OrderBy(t => t.GetType().GetProperty(field0.Field).GetValue(t))
                            .ThenByDescending(t => t.GetType().GetProperty(field1.Field).GetValue(t))
                            .ThenBy(t => t.GetType().GetProperty(field2.Field).GetValue(t))
                            .ThenBy(t => t.GetType().GetProperty(field3.Field).GetValue(t)).ToList();
                }
            }
            else
            {
                if (field2.Desc)
                {
                    if (field3.Desc)
                        result = (List<TEntity>)result.OrderBy(t => t.GetType().GetProperty(field0.Field).GetValue(t))
                            .ThenBy(t => t.GetType().GetProperty(field1.Field).GetValue(t))
                            .ThenByDescending(t => t.GetType().GetProperty(field2.Field).GetValue(t))
                            .ThenByDescending(t => t.GetType().GetProperty(field3.Field).GetValue(t)).ToList();
                    else
                        result = (List<TEntity>)result.OrderBy(t => t.GetType().GetProperty(field0.Field).GetValue(t))
                            .ThenBy(t => t.GetType().GetProperty(field1.Field).GetValue(t))
                            .ThenByDescending(t => t.GetType().GetProperty(field2.Field).GetValue(t))
                            .ThenBy(t => t.GetType().GetProperty(field3.Field).GetValue(t)).ToList();
                }
                else
                {
                    if (field3.Desc)
                        result = (List<TEntity>)result.OrderBy(t => t.GetType().GetProperty(field0.Field).GetValue(t))
                            .ThenBy(t => t.GetType().GetProperty(field1.Field).GetValue(t))
                            .ThenBy(t => t.GetType().GetProperty(field2.Field).GetValue(t))
                            .ThenByDescending(t => t.GetType().GetProperty(field3.Field).GetValue(t)).ToList();
                    else
                        result = (List<TEntity>)result.OrderBy(t => t.GetType().GetProperty(field0.Field).GetValue(t))
                            .ThenBy(t => t.GetType().GetProperty(field1.Field).GetValue(t))
                            .ThenBy(t => t.GetType().GetProperty(field2.Field).GetValue(t))
                            .ThenBy(t => t.GetType().GetProperty(field3.Field).GetValue(t)).ToList();
                }
            }

            return result;
        }
       #endregion

        #region FilterInRange
        /// <summary>
        /// Lọc 
        /// </summary>
        /// <param name="listEntity"></param>
        /// <param name="rangeDates"></param>
        /// <returns></returns>
        /// CreatedBy: NTDUNG (10/12/2021)
        public List<TEntity> FilterInRange(List<TEntity> listEntity, List<RangeDate> rangeDates) {
            var result = new List<TEntity>();
            foreach(var rangeDate in rangeDates)
            {
                // Nếu null fromDate thì là nhỏ hơn ngày toDate
                if (rangeDate.FromDate == null)
                {
                    result = listEntity.Where(entity =>
                    {
                        var time = entity.GetType().GetProperty(rangeDate.FieldName).GetValue(entity, null);
                        if (time.GetType() == typeof(DateTime))
                        {
                            return (DateTime)time >= rangeDate.FromDate;
                        }
                        return true;
                    }).ToList();
                }
                // Nếu null toDate thì là lớn hơn ngày fromDate
                else if (rangeDate.ToDate == null)
                {
                    result = listEntity.Where(entity =>
                    {
                        var time = entity.GetType().GetProperty(rangeDate.FieldName).GetValue(entity, null);
                        if (time.GetType() == typeof(DateTime))
                        {
                            return (DateTime)time <= rangeDate.ToDate;
                        }
                        return true;
                    }).ToList();
                }
                // Còn lại là trong khoảng fromDate và toDate
                else
                {
                    result = listEntity.Where(entity =>
                    {
                        var time = entity.GetType().GetProperty(rangeDate.FieldName).GetValue(entity, null);
                        if (time != null && time.GetType() == typeof(DateTime))
                        {
                            return (DateTime)time >= rangeDate.FromDate && (DateTime)time <= rangeDate.ToDate;
                        }
                        return true;
                    }).ToList();
                }
            }
            return result;
        }
        #endregion

        #region AddProperty
        public static void AddProperty(ExpandoObject expando, string propertyName, object propertyValue, Guid? sessionID)
        {
            // ExpandoObject supports IDictionary so we can extend it like this
            var expandoDict = expando as IDictionary<string, object>;
            if (expandoDict.ContainsKey(propertyName))
                expandoDict[propertyName] = propertyValue;
            else
                expandoDict.Add(propertyName, propertyValue);
        }
        #endregion
        #endregion

        #region UpdateColumns
        /// <summary>
        /// Cập nhật theo các cột
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="entityId"></param>
        /// <param name="columns"></param>
        /// <param name="sessionID"></param>
        /// <returns></returns>
        /// CreatedBy: NTDUNG (14/11/2021) 
        public virtual int UpdateColumns(TEntity entity, Guid entityId, List<string> columns, Guid? sessionID)
        {
            MySqlConnection mySqlConnection = null;
            IDbTransaction transaction = null;
            var rowEffects = -1;

            try
            {
                mySqlConnection = new MySqlConnection(_connectionString);
                mySqlConnection.Open();
                transaction = mySqlConnection.BeginTransaction();

                var queryLine = new List<string>();
                DynamicParameters dynamicParameters = new DynamicParameters();
                var properties = entity.GetType().GetProperties();
                foreach (var property in properties)
                {
                    var propName = property.Name;
                    // Kiểm tra có thuộc colums gửi lên không
                    var checkColumn = columns.Contains(propName);
                    if (!checkColumn) continue;

                    var propValue = property.GetValue(entity);
                    // Gán Id cũ
                    if (propName.Equals($"{_className}ID") && property.PropertyType == typeof(Guid))
                    {
                        propValue = entityId;
                    }
                    // Bổ sung ngày chỉnh sửa
                    if (propName == "ModifiedDate")
                    {
                        propValue = DateTime.Now;
                    }

                    queryLine.Add($"{propName} = @{propName}");
                    dynamicParameters.Add($"@{propName}", propValue);
                }

                dynamicParameters.Add("@oldEntityId", entityId);
                var sqlQuery = $"UPDATE {_className} SET {String.Join(", ", queryLine.ToArray())} " +
                                $"WHERE {_className}Id = @oldEntityId";
                rowEffects = mySqlConnection.Execute(sqlQuery, param: dynamicParameters, transaction: transaction);

                transaction.Commit();
            }
            catch (Exception)
            {
                if (transaction != null)
                {
                    transaction.Rollback();
                }
                throw;
            }
            finally
            {
                if (mySqlConnection != null) mySqlConnection.Close();
            }
            return rowEffects;
        }
        #endregion

        #region UpdateMultiple
        /// <summary>
        /// Cập nhật nhiều
        /// </summary>
        /// <param name="listEntity"></param>
        /// <param name="sessionID"></param>
        /// <returns></returns>
        /// CreatedBy: NTDUNG (23/12/2021)
        public virtual int UpdateMultiple(List<TEntity> listEntity, Guid? sessionID) {
            MySqlConnection mySqlConnection = null;
            IDbTransaction transaction = null;
            var rowEffects = -1;
            var sqlCommand = "";

            try
            {
                mySqlConnection = new MySqlConnection(_connectionString);
                mySqlConnection.Open();
                transaction = mySqlConnection.BeginTransaction();

                var queryLine = new List<string>();
                DynamicParameters dynamicParameters = new DynamicParameters();
                foreach(var (entity, index) in listEntity.Select((entity, index) => (entity, index)))
                {
                    var properties = entity.GetType().GetProperties();
                    Guid? entityId = null;
                    foreach (var property in properties)
                    {
                        if (property.IsDefined(typeof(EddieNotMap), false)) continue;
                        var propName = property.Name;
                        var propValue = property.GetValue(entity);

                        // Bổ sung ngày chỉnh sửa
                        if (propName == "ModifiedDate")
                        {
                            propValue = DateTime.Now;
                        }
                        if (propName.Equals($"{_className}ID") && property.PropertyType == typeof(Guid))
                        {
                            entityId = (Guid?)propValue;
                        }
                        else
                        { 
                            queryLine.Add($"{propName} = @{propName}{index}");
                            dynamicParameters.Add($"@{propName}{index}", propValue);
                        }
                    }

                    dynamicParameters.Add($"@oldEntityId{index}", entityId);
                    sqlCommand += $"UPDATE {_className} SET {String.Join(", ", queryLine.ToArray())} " +
                                    $"WHERE {_className}ID = @oldEntityId{index};";

                }
                rowEffects = mySqlConnection.Execute(sqlCommand, param: dynamicParameters, transaction: transaction);
                
                transaction.Commit();
            }
            catch (Exception)
            {
                if (transaction != null)
                {
                    transaction.Rollback();
                }
                throw;
            }
            finally
            {
                if (mySqlConnection != null) mySqlConnection.Close();
            }
            return rowEffects;
        }
        #endregion
        #endregion
    }
}
