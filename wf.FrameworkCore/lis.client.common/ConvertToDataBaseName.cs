using dcl.client.frame;
using dcl.entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace dcl.client.common
{
    public class ConvertToDataBaseName
    {
        public static string ConvertToDBName(string inputName)
        {

            int index = inputName.LastIndexOf(".");
            if (index == -1) return inputName;
            string windowName = inputName.Substring(index);
            inputName = inputName.Substring(0, index);

            string outputName = inputName;//转换后的前缀
             
            outputName += windowName;

            return outputName;
        }
        public static bool isNeedChecked(string funcCode)
        {
            string lisCode = ConvertToDBName(funcCode); 
             List<EntitySysFunction> listMenu = new List<EntitySysFunction>();
            if (UserInfo.isAdmin == true)
            {
                listMenu = UserInfo.entityUserInfo.AllFunc;
            }
            else
            {
                listMenu = UserInfo.entityUserInfo.Func;
            }
            try
            {
                List<EntitySysFunction> list = listMenu.Where(w=>w.FuncCode==lisCode).ToList();
                if (list != null && list.Count > 0)
                {
                    string result = list[0].FuncValiuser;
                    return result == "1" ? true : false;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            
        }
    }
}
