using System;
using System.Collections.Generic;

using System.Text;
using System.Data;
using dcl.entity;

namespace wf.client.reagent.Interface
{
    public interface IReagentEditor
    {
        /// <summary>
        /// 试剂组ID
        /// </summary>
        string GroupID { get; set; }

        event ReagentAddedEventHandler ReagentAdded;
        event ReagentRemovedEventHandler ReagentRemoved;
        event ReagentEditBoxButtonClick ButtonClicked;
        event EventHandler Reseted;

        void RemoveReagent(string com_id);
        void AddReagent(string com_id);
        void AddReagent(string com_id, string yz_id, decimal? price, string bar_code);
        void AddReagent2(string com_id, string yz_id, decimal? price, string bar_code);
        void Reset();

        void RefreshEditBoxText();

        List<EntityReaApplyDetail> listReaApplyDetail { get; set; }
        string Text { get; }
    }

    public delegate void ReagentAddedEventHandler(object sender, string com_id, int com_seq);
    public delegate void ReagentRemovedEventHandler(object sender, string com_id);
    public delegate void ReagentEditBoxButtonClick(object sender, string button_type);
}
