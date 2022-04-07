using System;
using System.Collections.Generic;

using System.Text;

namespace dcl.client.users
{
    class PowerMessage
    {
        //右上角文本
        public static string BASE_TITLE = "信息";
 
        //基本增删改提示
        public static string BASE_SELECT_NULL = "未选中任何数据";
        public static string BASE_DELETE_CONFIRM = "确定要删除该记录吗?";
        public static string BASE_SAVE_SUCCESS = "保存成功";
        public static string BASE_SAVE_ERROR = "保存失败";

        //字段不能为空
        public static string BASE_FUNCNAME_NULL = "功能名称不能为空";
        public static string BASE_ROLENAME_NULL = "角色名称不能为空";
        public static string BASE_USERNAME_NULL = "用户名称不能为空";
        public static string BASE_LOGINID_NULL = "登录账号不能为空";
        public static string BASE_PASSWORD_NULL = "用户密码不能为空";
        public static string BASE_ROLENAME_SAME = "角色名称不能重复";
        public static string BASE_LOGINID_SAME = "登录账号不能重复";
        public static string BASE_INCODE_NULL = "输入码不能为空";

        //操作提示
        public static string SELECT_ROLE_NULL = "未选中任何角色";
        public static string PASSWORD_OLD_ERROR = "原密码错误";
        public static string PASSWORD_NEW_NULL = "新密码不能为空";
        public static string PASSWORD_NEW_ERROR = "两次输入的新密码不匹配";
        public static string SELECT_TABLE_NULL = "未选中任何数据表";
        public static string SELECT_OPTION_NULL = "未选中任何整理操作";
        public static string SELECT_ITR_NULL = "未选中任何仪器";
        public static string DATA_CLEAN_CONFIRM = "确定要进行数据整理吗?";
        public static string DATA_DELETE_CONFIRM = "确定要进行数据删除吗?";
        public static string DATA_SAVE_CONFIRM = "确定要进行数据封档吗?";
    }
}
