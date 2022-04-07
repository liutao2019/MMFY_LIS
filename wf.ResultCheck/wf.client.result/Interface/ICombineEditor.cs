using System;
using System.Collections.Generic;

using System.Text;
using System.Data;
using dcl.entity;

namespace dcl.client.result.Interface
{
    public interface ICombineEditor
    {
        /// <summary>
        /// 专业组ID
        /// </summary>
        string PTypeID { get; set; }

        /// <summary>
        /// 物理组ID
        /// </summary>
        string CTypeID { get; set; }

        /// <summary>
        /// 仪器ID
        /// </summary>
        string ItrID { get; set; }

        event CombineAddedEventHandler CombineAdded;
        event CombineRemovedEventHandler CombineRemoved;
        event CombineEditBoxButtonClick ButtonClicked;
        event EventHandler Reseted;

        void RemoveCombine(string com_id);
        void AddCombine(string com_id);
        void AddCombine(string com_id, string yz_id, decimal? price, string bar_code);

        void Reset();

        void RefreshEditBoxText();

        List<EntityPidReportDetail> listRepDetail { get; set; }
        string Text { get; }
    }

    public delegate void CombineAddedEventHandler(object sender, string com_id, int com_seq);
    public delegate void CombineRemovedEventHandler(object sender, string com_id);
    public delegate void CombineEditBoxButtonClick(object sender, string button_type);
}
