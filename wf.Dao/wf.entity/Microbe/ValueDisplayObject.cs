using System;

namespace dcl.entity
{
    /// <summary>
    /// 带有value和display并且有拼音五笔码的通用对象
    /// 如：项目中的静态状态，性别等
    /// </summary>
    [Serializable]
    public class ValueDisplayObject
    {
        public ValueDisplayObject(object Value, string Display)
        {
            this.Display = Display;
            this.Value = Value;
        }

        public ValueDisplayObject(object Value, string Display,string Group)
        {
            this.Display = Display;
            this.Value = Value;
            this.Group = Group;
        }


        public ValueDisplayObject()
        {
        }

        public object Value { get; set; }
        public string Display { get; set; }
        public string Group { get; set; }
        public object Tag { get; set; }
        public int Seq { get; set; }
        public string py { get; set; }
        public string wb { get; set; }
    }
}