using System;
using System.Collections.Generic;
using System.Text;

namespace dcl.entity
{
    [Serializable]
    public class EntityOperationResultList : List<EntityOperationResult>
    {
        public EntityOperationResult[] GetSuccessOperations()
        {
            return GetSuccessOperations(true);
        }

        public EntityOperationResult[] GetFailedOperations()
        {
            return GetSuccessOperations(false);
        }

        private EntityOperationResult[] GetSuccessOperations(bool success)
        {
            List<EntityOperationResult> list = new List<EntityOperationResult>();
            foreach (EntityOperationResult item in this)
            {
                if (item.Success == success)
                {
                    list.Add(item);
                }
            }
            return list.ToArray();
        }

        public int FailedCount
        {
            get
            {
                return GetSuccessCount(false);
            }
        }

        public int SuccessCount
        {
            get
            {
                return GetSuccessCount(true);
            }
        }

        private int GetSuccessCount(bool success)
        {
            int count = 0;
            foreach (EntityOperationResult result in this)
            {
                if (result.Success == success)
                {
                    count++;
                }
            }
            return count;
        }

        public override string ToString()
        {
            return string.Format("成功{0}；失败{1}", SuccessCount, FailedCount);
        }
    }
}
