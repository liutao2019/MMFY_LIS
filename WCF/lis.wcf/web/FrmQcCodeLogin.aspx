<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmQcCodeLogin.aspx.cs" Inherits="dcl.pub.wcf.web.FrmQcCodeLogin" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>检验二维码签收登录</title>
</head>
<body>
    <form id="form1" runat="server">
    <div><asp:Label ID="Label3" runat="server" Text="检验二维码签收登录" Font-Size="X-Large"></asp:Label></div>
    <div></div>
    <table>
    <tr>
       <td align="right"><asp:Label ID="Label2" runat="server" Text="用户"></asp:Label></td>
       <td><asp:TextBox ID="txtUserName" runat="server" AutoCompleteType="Disabled"></asp:TextBox></td>
    </tr>
    <tr>
       <td align="right"><asp:Label ID="Label4" runat="server" Text="密码"></asp:Label></td>
       <td><asp:TextBox ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox></td>
    </tr>
    <tr>
       <td align="right"></td>
       <td><asp:Button ID="btnLogin" runat="server" Text="登录" onclick="btnLogin_Click"  />
       &nbsp;&nbsp;&nbsp;<asp:Button ID="btnReset" runat="server" Text="重置" 
               onclick="btnReset_Click" />
       </td>
    </tr>
    </table>
    <div><asp:Label ID="labMessage" runat="server" ForeColor="#CC0000"></asp:Label></div>
    </form>
</body>
</html>
