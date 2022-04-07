using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 通用入参
    /// </summary>
    [Serializable]
    public class EntityRequest
    {
        public EntityRequest()
        { }

        private byte[] requestValue;

        /// <summary>
        /// 设置参数
        /// </summary>
        /// <param name="obj"></param>
        public void SetRequestValue(object obj)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream rems = new MemoryStream();
            formatter.Serialize(rems, obj);
            requestValue = rems.GetBuffer();
        }

        /// <summary>
        /// 获取参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetRequestValue<T>()
        {
            T entity = default(T);
            if (requestValue != null)
            {
                object obj = GetRequestValue();
                entity = (T)obj;
            }

            return entity;
        }

        public object GetRequestValue()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream rems = new MemoryStream(requestValue);
            return formatter.Deserialize(rems);
        }
    }
}
