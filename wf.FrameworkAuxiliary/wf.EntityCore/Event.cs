using System;
using System.Collections.Generic;
using System.Text;

namespace Lib.EntityCore
{
    public delegate void EntityPropertyChangedEventHandler(object sender, EntityPropertyChangedEventArgs args);

    public delegate void EntityPropertyChangingEventHandler(object sender, EntityPropertyChangingEventArgs args);


    public class EntityPropertyChangedEventArgs : EventArgs
    {
        public object OldValue { get; set; }
        public object NewValue { get; set; }
        public string PropertyName { get; set; }
    }

    public class EntityPropertyChangingEventArgs : EntityPropertyChangedEventArgs
    {
        public bool Cancel { get; set; }

        public EntityPropertyChangingEventArgs()
        {
            this.Cancel = false;
        }
    }
}
