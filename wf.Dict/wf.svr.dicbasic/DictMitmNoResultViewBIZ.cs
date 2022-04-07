using dcl.servececontract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.entity;
using dcl.common;
using dcl.dao.interfaces;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.IO.Compression;
using System.Data;

namespace dcl.svr.dicbasic
{
    public class DictMitmNoResultViewBIZ : IDictMitmNoResultView
    {

        public List<EntityDicObrResultOriginal> GetInstructmentResult2(DateTime date, string itr_id, int result_type, string filter)
        {
            IDaoMitmNoResultView dao = DclDaoFactory.DaoHandler<IDaoMitmNoResultView>();
            List<EntityDicObrResultOriginal> list = new List<EntityDicObrResultOriginal>();
            if (dao != null)
            {
                list = dao.GetInstructmentResult2(date, itr_id, result_type, filter);
            }
            return list;
        }

        public EntityResponse SearchItem(EntityRequest request)
        {
            EntityResponse result = new EntityResponse();
            try
            {
                result.Scusess = true;
                result.SetResult(new ItemBIZ().GetItemByItmId(""));
            }
            catch
            {
                result.Scusess = false;
            }

            return result;
        }

        public EntityResponse SaveOrUpdateMitmNo(List<EntityDicMachineCode> ds)
        {
            EntityResponse result = new EntityResponse();

            IDaoMitmNoResultView dao = DclDaoFactory.DaoHandler<IDaoMitmNoResultView>();

            if (dao != null)
            {
                result = dao.SaveOrUpdateMitmNo(ds);
            }
            return result;
        }


        /// <summary>
        /// 压缩字节流
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] DeflateData(byte[] data)
        {
            if (data == null) return null;

            byte[] buffer = null;
            using (MemoryStream stream = new MemoryStream())
            {
                using (DeflateStream inflateStream = new DeflateStream(stream, CompressionMode.Compress, true))
                {
                    inflateStream.Write(data, 0, data.Length);
                }

                stream.Seek(0, SeekOrigin.Begin);

                int length = Convert.ToInt32(stream.Length);
                buffer = new byte[length];
                stream.Read(buffer, 0, length);
            }

            return buffer;
        }
        /// <summary>
        /// 获取压缩后所有字节流
        /// </summary>
        /// <returns></returns>
        public byte[] GetAllBuffer(DateTime date, string itr_id, int result_type, string filter)
        {
            List<EntityDicObrResultOriginal> list = GetInstructmentResult2(date, itr_id, result_type, filter);
            IFormatter formatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream();
            formatter.Serialize(stream, list);
            byte[] origionByte = stream.ToArray();
            byte[] compressByte = DeflateData(origionByte);
            return compressByte;
        }

    }
}
