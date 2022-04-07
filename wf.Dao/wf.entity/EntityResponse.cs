using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 出参
    /// </summary>
    [Serializable]
    public class EntityResponse
    {
        public EntityResponse()
        {
            Scusess = true;
        }

        /// <summary>
        /// 成功状态
        /// </summary>
        public bool Scusess { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErroMsg { get; set; }

        /// <summary>
        /// 返回结果
        /// </summary>
        private byte[] result;

        /// <summary>
        /// 赋值
        /// </summary>
        /// <param name="obj"></param>
        public void SetResult(object obj)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream rems = new MemoryStream();
            formatter.Serialize(rems, obj);
            this.result = rems.GetBuffer();
        }

        /// <summary>
        /// 取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetResult<T>()
        {
            T entity = default(T);
            if (result != null)
            {
                object obj = GetResult();
                entity = (T)obj;
            }

            return entity;
        }

        public object GetResult()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream rems = new MemoryStream(result);
            return formatter.Deserialize(rems);
        }
    }
}
