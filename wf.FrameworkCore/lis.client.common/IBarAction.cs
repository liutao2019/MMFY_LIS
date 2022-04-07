using System;
using System.Collections.Generic;

using System.Text;

namespace dcl.client.common
{
  public interface IBarAction
  {
     void Add();
     void Save();
     void Delete();
     void DoRefresh();
     Dictionary<string, Boolean> GetActiveCtrls();
  }

  /// <summary>
  /// 完整的操作条
  /// </summary>
  public interface IBarActionExt : IBarAction
  {
      /// <summary>
      /// 撤销
      /// </summary>
      void Cancel();

      /// <summary>
      /// 开始编辑
      /// </summary>
      void Edit();

      /// <summary>
      /// 关闭
      /// </summary>
      void Close();

      /// <summary>
      ///下一个
      /// </summary>
      void MoveNext();

      /// <summary>
      /// 上一个
      /// </summary>
      void MovePrev();
  }
}
