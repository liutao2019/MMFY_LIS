using System;
using System.Collections.Generic;

using System.Text;

namespace dcl.client.oa
{
    public class OfficeMessage
    {
        //右上角文本
        public static string BASE_TITLE = "信息";
 
        //基本增删改提示
        public static string BASE_SELECT_NULL = "未选中任何数据";
        public static string BASE_DELETE_CONFIRM = "确定要删除该记录吗?";
        public static string BASE_SAVE_SUCCESS = "保存成功";
        public static string BASE_SAVE_ERROR = "保存失败";

        //字段不能为空
        public static string BASE_MESSAGETITLE_NULL = "消息标题不能为空";
        public static string BASE_MESSAGECONTENT_NULL = "消息正文不能为空";
        public static string BASE_MESSAGETO_NULL = "收件人不能为空";
        public static string BASE_ORDERTYPENAME_NULL = "单证类型名称不能为空";
        public static string BASE_ORDERITEMTYPE_NULL = "数据类型不能为空";
        public static string BASE_ORDERITEMNAME_NULL = "单证字段名称不能为空";

        //文件上传
        public static string BASE_UPLOAD_SUCCESS = "上传成功";
        public static string BASE_UPLOAD_ERROR = "上传失败,文件大小超过限制";

    }
}
