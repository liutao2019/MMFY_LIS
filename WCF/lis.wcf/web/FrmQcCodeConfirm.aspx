<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmQcCodeConfirm.aspx.cs" Inherits="dcl.pub.wcf.web.FrmQcCodeConfirm" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>检验二维码签收</title>
</head>
<body>
    <form id="form1" runat="server">
    <div><asp:Label ID="Label3" runat="server" Text="检验二维码签收" Font-Size="X-Large"></asp:Label></div>
    <div></div>
    <table>
    <tr>
       <td><asp:Label ID="Label1" runat="server" Text="扫描二维码"></asp:Label></td>
       <td><asp:TextBox ID="txtQcCode" runat="server" 
               ontextchanged="txtQcCode_TextChanged" AutoPostBack="True"></asp:TextBox></td>
    </tr>
    <tr>
       <td><asp:Label ID="Label2" runat="server" Text="确认类型"></asp:Label></td>
       <td><asp:DropDownList ID="DDLType" runat="server">
           <asp:ListItem Value="0">签收</asp:ListItem>
           <asp:ListItem Value="1">核酸提取</asp:ListItem>
           <asp:ListItem Value="2">核酸定量扩增</asp:ListItem>
           </asp:DropDownList></td>
    </tr> 
    </table>
    <div><asp:Label ID="lblMsg" runat="server" ForeColor="#CC0000"></asp:Label></div>
    </form>
</body>
</html>
