using System.Collections.Generic;
using dcl.servececontract;
using dcl.entity;
using dcl.common;
using dcl.dao.interfaces;

namespace dcl.svr.oa
{
    class OrderTableBIZ : IOrderTable
    {
        /// <summary>
        /// 删除单证类型或者单证字段
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public EntityResponse DeleteOrder(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            if (request != null)
            {
                Dictionary<string, object> dict = (Dictionary<string, object>)request.GetRequestValue();
                EntityOaTable Table = new EntityOaTable();
                EntityOaTableField Field = new EntityOaTableField();
                if (dict.ContainsKey("Table"))
                {
                    object objReport = dict["Table"];
                    if (objReport != null)
                    {
                        Table = objReport as EntityOaTable;
                    }
                    IDaoOaOrderTable dao = DclDaoFactory.DaoHandler<IDaoOaOrderTable>();
                    if (dao == null)
                    {
                        response.Scusess = false;
                        response.ErroMsg = "未找到数据访问";
                    }
                    else
                    {
                        response.SetResult(dao.DeleteOrderTable(Table));
                    }
                }
                if (dict.ContainsKey("Field"))
                {
                    object objPar = dict["Field"];
                    if (objPar != null)
                    {
                        Field = objPar as EntityOaTableField;
                    }
                    IDaoOaOrderTableField daoField = DclDaoFactory.DaoHandler<IDaoOaOrderTableField>();
                    if (daoField == null)
                    {
                        response.Scusess = false;
                        response.ErroMsg = "未找到数据访问";
                    }
                    else
                    {
                        response.SetResult(daoField.DeleteOrderTableFiled(Field));
                    }
                }
            }
            else
            {
                response.Scusess = false;
                response.ErroMsg = "传入参数为Null";
            }
            return response;
        }

        /// <summary>
        /// 获得单证类型
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public EntityResponse GetOrderTable(EntityRequest request)
        {

            EntityResponse response = new EntityResponse();
            EntityOaTable type = request.GetRequestValue<EntityOaTable>();
            IDaoOaOrderTable dao = DclDaoFactory.DaoHandler<IDaoOaOrderTable>();
            if (dao == null)
            {
                response.Scusess = false;
                response.ErroMsg = "未找到数据访问";
            }
            else
            {
                response.Scusess = true;
                response.SetResult(dao.GetOrderTable(type));
            }
            return response;
        }
        /// <summary>
        /// 获得单证字段
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public EntityResponse GetOrderTableField(string id)
        {

            EntityResponse response = new EntityResponse();
            IDaoOaOrderTableField dao = DclDaoFactory.DaoHandler<IDaoOaOrderTableField>();
            if (dao == null)
            {
                response.Scusess = false;
                response.ErroMsg = "未找到数据访问";
            }
            else
            {
                response.Scusess = true;
                response.SetResult(dao.GetOrderTableFiled(id));
            }
            return response;
        }
        /// <summary>
        /// 新增单证类型
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public EntityResponse NewOrderTable(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            EntityOaTable type = request.GetRequestValue<EntityOaTable>();
            IDaoOaOrderTable dao = DclDaoFactory.DaoHandler<IDaoOaOrderTable>();
            if (dao == null)
            {
                response.Scusess = false;
                response.ErroMsg = "未找到数据访问";
            }
            else
            {
                response.Scusess = true;
                response.SetResult(dao.SaveOrderTable(type));
            }
            return response;
        }
        /// <summary>
        /// 新增单证字段
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public EntityResponse NewOrderTableField(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            EntityOaTableField type = request.GetRequestValue<EntityOaTableField>();
            IDaoOaOrderTableField dao = DclDaoFactory.DaoHandler<IDaoOaOrderTableField>();
            if (dao == null)
            {
                response.Scusess = false;
                response.ErroMsg = "未找到数据访问";
            }
            else
            {
                response.Scusess = true;
                response.SetResult(dao.SaveOrderTableFiled(type));
            }
            return response;
        }
        /// <summary>
        /// 更新单证类型
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public EntityResponse UpdateOrderTable(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            EntityOaTable type = request.GetRequestValue<EntityOaTable>();
            IDaoOaOrderTable dao = DclDaoFactory.DaoHandler<IDaoOaOrderTable>();
            if (dao == null)
            {
                response.Scusess = false;
                response.ErroMsg = "未找到数据访问";
            }
            else
            {
                response.Scusess = true;
                response.SetResult(dao.UpdateOrderTable(type));
            }
            return response;
        }
        /// <summary>
        /// 更新单证字段
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public EntityResponse UpdateOrderTableField(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            EntityOaTableField type = request.GetRequestValue<EntityOaTableField>();
            IDaoOaOrderTableField dao = DclDaoFactory.DaoHandler<IDaoOaOrderTableField>();
            if (dao == null)
            {
                response.Scusess = false;
                response.ErroMsg = "未找到数据访问";
            }
            else
            {
                response.Scusess = true;
                response.SetResult(dao.UpdateOrderTableFiled(type));
            }
            return response;
        }
        /// <summary>
        /// 上移下移单证字段顺序_写不下了放到这里来
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public EntityResponse UpdateOrderTableFieldIndex(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            List<EntityOaTableField> listField = request.GetRequestValue()as List<EntityOaTableField>;
            IDaoOaOrderTableField dao = DclDaoFactory.DaoHandler<IDaoOaOrderTableField>();
            if (dao == null)
            {
                response.Scusess = false;
                response.ErroMsg = "未找到数据访问";
            }
            else
            {
                int count = 0; ;
                foreach (EntityOaTableField field in listField)
                {
                    if (dao.UpdateOrderTableFiledIndex(field))
                    {
                        count++;
                    }
                }
                if (count == listField.Count)
                {
                    response.Scusess = true;
                }
                response.SetResult(listField);
            }
            return response;
        }

    }
}
