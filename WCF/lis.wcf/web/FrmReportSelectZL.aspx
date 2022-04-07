<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmReportSelectZL.aspx.cs"
    EnableEventValidation="false" Inherits="dcl.pub.wcf.web.FrmReportSelectZL" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc3" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="css/style.css" rel="stylesheet" type="text/css" />
    <title>新检验报告查询系统</title>
    <script language="javascript" src="LodopFuncs.js"></script>
    <object id="LODOP" classid="clsid:2105C259-1E0C-4534-8141-A753534CB4CA" width=0 height=0> 
	    <embed id="LODOP_EM" type="application/x-print-lodop" width=0 height=0 pluginspage="install_lodop.exe"></embed>
    </object>
    <style type="text/css">
        .autocomplete_completionListElement
        {
            margin: 0px !important;
            background-color: inherit;
            color: windowtext;
            border: buttonshadow;
            border-width: 1px;
            border-style: solid;
            cursor: 'default';
            overflow: auto;
            height: auto;
            max-height: 200px;
            text-align: left;
            list-style-type: none;
        }
        .autocomplete_listItem
        {
            border-style: solid;
            border: #EAEFF9;
            border-width: 1px;
            background-color: window;
            color: windowtext;
        }
        .autocomplete_highlightedListItem
        {
            background-color: #ffff99;
            color: black;
            padding: 1px;
        }
        .gridheader
        {
            text-align: left;
        }
    </style>

    <script type="text/javascript">
    //屏蔽所有javascript错误显示
//    function killerrors() {
//        return true;
//    }
//    window.onerror = killerrors;
  
//    //ID类型
//    function SetNoType( source, eventArgs ) { 
//        if(eventArgs.get_text() ==null)
//         {
//            document.getElementById(('txtNoType')).value="";
//            document.getElementById(('valNoType')).value="";
//        }
//        else
//        {
//            document.getElementById(('txtNoType')).value=eventArgs.get_text().substring(15);
//            document.getElementById(('valNoType')).value=eventArgs.get_value();
//        }
//    }
    
    //科室
     function SetDepart( source, eventArgs ) { 
     if(eventArgs.get_text() ==null)
         {
            document.getElementById(('txtDepart')).value="";
            document.getElementById(('valDepart')).value="";
        }
        else
        {
            document.getElementById(('txtDepart')).value=eventArgs.get_text().substring(15);
            document.getElementById(('valDepart')).value=eventArgs.get_value();
        }
    }
    
    function DeptClick(source , eventArgs){

     document.getElementById(('txtDepart')).value="";
     document.getElementById(('txtDepart')).focus();
 
    }


    var LODOP; //声明为全局变量
    function Preview(path, filename, pageSize, filetype) {
        CreatePrintPage(path, filename, pageSize, filetype);
        //LODOP.SET_PRINT_PAGESIZE(2,0,0,"A5");
        LODOP.SET_PREVIEW_WINDOW(1, 0, 0, 0, 0, "");
        LODOP.SET_SHOW_MODE("LANDSCAPE_DEFROTATED", 1); //横向时的正向显示
        LODOP.SET_PRINT_MODE("AUTO_CLOSE_PREWINDOW", 1); //打印后自动关闭预览窗口
        LODOP.SET_PRINT_MODE("PRINT_PAGE_PERCENT", "Full-Height");
        //LODOP.SET_PRINT_STYLEA(0,"Stretch",2);//按原图比例(不变形)缩放模式
        LODOP.PREVIEW();
    };

    function PreviewManay(pritescript) {
        CreateManyPrintPage(pritescript);
        //LODOP.SET_PRINT_PAGESIZE(2,0,0,"A5");
        LODOP.SET_PREVIEW_WINDOW(1, 0, 0, 0, 0, "");
        LODOP.SET_SHOW_MODE("LANDSCAPE_DEFROTATED", 1); //横向时的正向显示
        LODOP.SET_PRINT_MODE("AUTO_CLOSE_PREWINDOW", 1); //打印后自动关闭预览窗口
        LODOP.SET_PRINT_MODE("PRINT_PAGE_PERCENT", "Full-Height");
        //LODOP.SET_PRINT_STYLEA(0,"Stretch",2);//按原图比例(不变形)缩放模式
        LODOP.PREVIEW();
    };

    function Setup() {
        CreatePrintPage();
        LODOP.PRINT_SETUP();
    };
    function Design() {
        CreatePrintPage();
        LODOP.PRINT_DESIGN();
    };
    function CreatePrintPage(path, filename, pageSize, filetype) {
        LODOP = getLodop(document.getElementById('LODOP'), document.getElementById('LODOP_EM'));
        for (i = 1; i <= pageSize; i++) {
            LODOP.NewPage();
            LODOP.ADD_PRINT_IMAGE(0, 0, "100%", "100%", "<img border='0' src='reportImg/" + filename + "_" + i + "." + filetype + "' />");
        }
    };

    function CreateManyPrintPage(path) {
        var arr = new Array();
        //alert(path);
        arr = path.split(',');
        var temp = "";
        LODOP = getLodop(document.getElementById('LODOP'), document.getElementById('LODOP_EM'));
        for (i = 0; i < arr.length; i++) {
            temp = arr[i].replace('|', '/').replace('|', '/').replace('|', '/');
            LODOP.NewPage();
            LODOP.ADD_PRINT_IMAGE(0, 0, "100%", "100%", "<img border='0' src='" + temp + "' />");
            LODOP.PRINT();
        }
    };

    function CreateManyPrintPageWithClose(path) {
        var arr = new Array();
        //alert(path);
        arr = path.split(',');
        var temp = "";
        LODOP = getLodop(document.getElementById('LODOP'), document.getElementById('LODOP_EM'));
        for (i = 0; i < arr.length; i++) {
            temp = arr[i].replace('|', '/').replace('|', '/').replace('|', '/');
            LODOP.NewPage();
            LODOP.ADD_PRINT_IMAGE(0, 0, "100%", "100%", "<img border='0' src='" + temp + "' />");
            LODOP.PRINT();
        }
        setTimeout("window.opener=null;window.open('','_self');window.close();", 60000);
    };

    function ClosePageIn60(sec) {
        setTimeout("window.opener=null;window.open('','_self');window.close();", 60000);
    };

    function AlterMessage(sec) {
        alert(sec);
    };
    function HideLabel() {
        document.getElementById('a1').style.display = "none"; 
    };
    function CheckIsInstall() {
        try {
            var LODOP = getLodop(document.getElementById('LODOP'), document.getElementById('LODOP_EM'));
            if ((LODOP != null) && (typeof (LODOP.VERSION) != "undefined")) alert("本机已成功安装过Lodop控件!\n  版本号:" + LODOP.VERSION);
        } catch (err) {
            alert("Error:本机未安装!");
        }
    }
    
//    //样本
//    function SetSample( source, eventArgs ) { 
//      if(eventArgs.get_text() ==null)
//         {
//            document.getElementById(('txtSample')).value="";
//            document.getElementById(('valSample')).value="";
//        }
//        else
//        {
//            document.getElementById(('txtSample')).value=eventArgs.get_text().substring(15);
//            document.getElementById(('valSample')).value=eventArgs.get_value();
//        }
//    }
    
//    //组合
//    function SetCombine( source, eventArgs ) { 
//     if(eventArgs.get_text() ==null)
//         {
//            document.getElementById(('txtCombine')).value="";
//            document.getElementById(('valCombine')).value="";
//        }
//        else
//        {
//            document.getElementById(('txtCombine')).value=eventArgs.get_text().substring(15);
//            document.getElementById(('valCombine')).value=eventArgs.get_value();
//        }
//    }
    
//    //病人来源
//    function SetOrigin( source, eventArgs ) { 
//     if(eventArgs.get_text() ==null)
//         {
//            document.getElementById(('txtOrigin')).value="";
//            document.getElementById(('valOrigin')).value="";
//        }
//        else
//        {
//            document.getElementById(('txtOrigin')).value=eventArgs.get_text().substring(15);
//            document.getElementById(('valOrigin')).value=eventArgs.get_value();
//        }
//    }
//    
//    //报告类型
//    function SetReportType( source, eventArgs ) { 
//    if(eventArgs.get_text() ==null)
//         {
//            document.getElementById(('txtReportType')).value="";
//            document.getElementById(('valReportType')).value="";
//        }
//        else
//        {
//            document.getElementById(('txtReportType')).value=eventArgs.get_text().substring(15);
//            document.getElementById(('valReportType')).value=eventArgs.get_value();
//        }
//    }

    </script>

</head>
<body style="text-align: center; margin: 0 0 0 0;">
<div class="logo" style="background-image: url('images/zs_pic2.jpg');"><div align="left"><img src="images/zs_pic3.png" /></div></div>

    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table style="width: 100%;">
                <tr>
                    <td style="width: 100%" valign="top" align="left" colspan="2">
                        <table style="width: 100%" id="tbSearchbar" runat="server">
                            <tr>
                                <td colspan="8">
                                    <asp:Label ID="Label1" runat="server" Style="text-align: left" Font-Bold="true" Text="查询条件"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Label ID="Lbl1" runat="server" Style="text-align: left" Font-Bold="True" Text="科室输入空格可列出所有科室信息，也可输入拼音码快速检索数科室.如：妇产科 输入'fck' 即可查询出来"
                                        ForeColor="Red"></asp:Label>
                                        &nbsp;&nbsp;&nbsp;&nbsp;<asp:Button ID="btModifyPwd" runat="server" 
                                        Text="修改密码" CssClass="button" ForeColor="Green" 
                                        onclick="btModifyPwd_Click"/>
                                </td>
                            </tr>
                            <tr>
                                <td class="td3-ttl" style="width: 50px">
                                    <asp:Label ID="Label2" runat="server" Text="日期范围" Width="50px"></asp:Label>
                                </td>
                                <td class="td3-tti">
                                    <asp:TextBox ID="txtTimeFrom" runat="server" Width="71px" AutoCompleteType="Disabled"></asp:TextBox>
                                    <cc3:CalendarExtender ID="txtTimeFrom_CalendarExtender" runat="server" TargetControlID="txtTimeFrom"
                                        Format="yyyy-MM-dd" CssClass="MyCalendar">
                                    </cc3:CalendarExtender>
                                    至<asp:TextBox ID="txtTimeTo" runat="server" Width="71px" AutoCompleteType="Disabled"></asp:TextBox>
                                    <cc3:CalendarExtender ID="txtTimeTo_CalendarExtender" runat="server" TargetControlID="txtTimeTo"
                                        Format="yyyy-MM-dd" CssClass="MyCalendar">
                                    </cc3:CalendarExtender>
                                </td>
                                <td class="td3-ttl" style="width: 30px">
                                    <asp:Label ID="Label6" runat="server" Text="科室" Width="30px"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtDepart" runat="server" Width="140" AutoCompleteType="Disabled"></asp:TextBox>
                                    <cc3:AutoCompleteExtender ID="txtDepart_AutoCompleteExtender" runat="server" FirstRowSelected="true"
                                        BehaviorID="ace1_bID" CompletionInterval="100" CompletionSetCount="12" EnableCaching="true"
                                        MinimumPrefixLength="0" OnClientItemSelected="SetDepart" ServiceMethod="GetDepart"
                                        ServicePath="AutoCompleteDict.asmx" TargetControlID="txtDepart" CompletionListCssClass="autocomplete_completionListElement"
                                        CompletionListItemCssClass="autocomplete_listItem" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem">
                                    </cc3:AutoCompleteExtender>
                                    <label id="a1" onclick="DeptClick()" class="button" style="font-size: 16px; " visible="True">
                                        ▼</label>
                                </td>
                                <td class="td3-ttl" style="width: 50px">
                                    <asp:Label ID="Label4" runat="server" Text="病人ID" Width="50px"></asp:Label>
                                </td>
                                <td class="td3-tti">
                                    <asp:TextBox ID="txtNo" runat="server" Width="120" AutoCompleteType="Disabled"></asp:TextBox>
                                    <asp:TextBox ID="txtNo2" runat="server" Width="1" AutoCompleteType="Disabled" Visible="False"></asp:TextBox>
                                    <asp:TextBox ID="txtPrintFlag" runat="server" Width="1" AutoCompleteType="Disabled" Visible="False"></asp:TextBox>
                                    <asp:TextBox ID="txtAutoClose" runat="server" Width="1" AutoCompleteType="Disabled" Visible="False"></asp:TextBox>
                                </td>
                                <td class="td3-ttl" style="width: 30px">
                                    <asp:Label ID="Label5" runat="server" Text="姓名" Width="30px"></asp:Label>
                                </td>
                                <td class="td3-tti">
                                    <asp:TextBox ID="txtName" runat="server" Width="120" AutoCompleteType="Disabled"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="td3-ttl" style="width:14px">
                                    &nbsp;
                                </td>
                                <td class="td3-tti">
                                    <asp:Label ID="Lbl3" runat="server" Style="text-align: left" Font-Bold="true" Text="只可查询出已报告和已打印的数据"></asp:Label>
                                </td>
                                <td class="td3-ttl" style="width: 14px">
                                    &nbsp;
                                </td>
                                <td class="td3-tti">
                                    <asp:Label ID="erroMes" runat="server" Text="" Font-Bold="True" ForeColor="Red"></asp:Label>
                                </td>
                                <td>
                                    <asp:Button ID="btnSearch" runat="server" Text="查询" CssClass="button" OnClick="btnSearch_Click" />
                                </td>
                                <td>
                                    <asp:Button ID="btnReset" runat="server" Text="重置" CssClass="button" OnClick="btnReset_Click" />
                                </td>
                                <td>
                                     <asp:Button ID="Button1" runat="server" Text="打印" CssClass="button" OnClick="btnPrint_Click" />
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="width: 30%" valign="top" id="tdLeft" runat="server">
                        <asp:Label ID="Label12" runat="server" Style="text-align: left" Font-Bold="true"
                            Text="病人资料"></asp:Label>
                        &nbsp(<%=gvPatients.PageIndex + 1%>/<%=gvPatients.PageCount.ToString()%>页)
                        <asp:Panel ID="Panel2" runat="server" HorizontalAlign="Left" ScrollBars="Auto" Width="100%"
                            Height="480px">
                            <asp:GridView ID="gvPatients" runat="server" AllowPaging="True" AllowSorting="True"
                                AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None"
                                OnPageIndexChanging="gvPatients_PageIndexChanging" OnRowCreated="gvPatients_RowCreated"
                                OnRowDataBound="gvPatients_RowDataBound" OnSelectedIndexChanged="gvPatients_SelectedIndexChanged"
                                PagerSettings-FirstPageText="首页" PagerSettings-LastPageText="末页" PagerSettings-Mode="NumericFirstLast"
                                PagerSettings-NextPageText="下一页" PagerSettings-PreviousPageText="上一页" PageSize="15"
                                RowStyle-Wrap="False" SelectedRowStyle-Wrap="False" Width="30%">
                                <PagerSettings FirstPageText="首页" LastPageText="末页" Mode="NextPreviousFirstLast"
                                    NextPageText="下一页" PageButtonCount="5" Position="Top" PreviousPageText="上一页" />
                                <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                                <RowStyle BackColor="#E3EAEB" />
                                <Columns>
                                    <asp:BoundField DataField="pat_id" HeaderText="pat_id" ReadOnly="True">
                                        <HeaderStyle Wrap="False" />
                                        <ItemStyle Wrap="False" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="pat_in_no" HeaderText="pat_in_no" />
                                    <asp:BoundField DataField="pat_sdate" HeaderText="pat_sdate" />
                                    <asp:BoundField DataField="pat_dep_name" HeaderText="pat_dep_name" />
                                    <asp:BoundField DataField="pat_bed_no" HeaderText="pat_bed_no" />
                                    <asp:BoundField DataField="pat_diag" HeaderText="pat_diag" />
                                    <asp:BoundField DataField="pat_host_order" HeaderText="pat_host_order" />
                                    <asp:BoundField DataField="sam_name" HeaderText="sam_name" />
                                    <asp:BoundField DataField="pat_sam_rem" HeaderText="pat_sam_rem" />
                                    <asp:BoundField DataField="pat_rem" HeaderText="pat_rem" />
                                    <asp:BoundField DataField="pat_bar_code" HeaderText="pat_bar_code" />
                                    <asp:BoundField DataField="pat_c_name" HeaderText="pat_c_name" />
                                    <asp:BoundField DataField="itr_mid" HeaderText="itr_mid" />
                                    <asp:BoundField DataField="pat_sample_date" HeaderText="pat_sample_date" />
                                    <asp:BoundField DataField="pat_prt_date" HeaderText="pat_prt_date" />
                                    <asp:BoundField DataField="doc_name" HeaderText="doc_name" />
                                    <asp:BoundField DataField="lrName" HeaderText="lrName" />
                                    <asp:BoundField DataField="shName" HeaderText="shName" />
                                    <asp:BoundField DataField="bgName" HeaderText="bgName" />
                                    <asp:BoundField DataField="pat_sample_receive_date" HeaderText="pat_sample_receive_date" />
                                    <asp:BoundField DataField="pat_jy_date" HeaderText="pat_jy_date" />
                                    <asp:BoundField DataField="pat_chk_date" HeaderText="pat_chk_date" />
                                    <asp:BoundField DataField="pat_report_date" HeaderText="pat_report_date" />
                                    <%-- <asp:CommandField ShowSelectButton="True" SelectText="选择">
                                            <FooterStyle Wrap="False" Width="30" />
                                            <HeaderStyle Wrap="False" Width="30" />
                                            <ItemStyle Wrap="False" Width="30" />
                                        </asp:CommandField>--%>
                                        <asp:BoundField DataField="ori_name" HeaderText="来源" ReadOnly="True">
                                        <HeaderStyle Wrap="False" />
                                        <ItemStyle Wrap="False" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="pat_name" HeaderText="姓名" ReadOnly="True">
                                        <HeaderStyle Wrap="False" />
                                        <ItemStyle Wrap="False" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="pat_c_name" HeaderText="组合">
                                        <FooterStyle Wrap="False" />
                                        <HeaderStyle Wrap="False" />
                                        <ItemStyle Wrap="False" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="pat_bed_no" HeaderText="床号">
                                        <HeaderStyle Wrap="False" />
                                        <ItemStyle Wrap="False" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="pat_date" DataFormatString="{0:d}" HeaderText="日期">
                                        <FooterStyle Wrap="False" />
                                        <HeaderStyle Wrap="False" />
                                        <ItemStyle Wrap="False" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="pat_sex" HeaderText="性别">
                                        <FooterStyle Wrap="False" />
                                        <HeaderStyle Wrap="False" />
                                        <ItemStyle Wrap="False" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="pat_age" HeaderText="年龄">
                                        <FooterStyle Wrap="False" />
                                        <HeaderStyle Wrap="False" />
                                        <ItemStyle Wrap="False" />
                                    </asp:BoundField>
                                     <asp:BoundField DataField="itr_rep_id" HeaderText="报表"  />
				     <asp:BoundField DataField="pat_ctype" HeaderText="pat_ctype" />
                                </Columns>
                                <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                                <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" HorizontalAlign="Left" />
                                <EditRowStyle BackColor="#7C6F57" />
                                <AlternatingRowStyle BackColor="White" />
                            </asp:GridView>
                        </asp:Panel>
                    </td>
                    <td style="width: 70%" valign="top" align="left">
                        <asp:Label ID="lblCustomMessage" runat="server" Style="text-align: left;" ForeColor="Red" Font-Bold="true"
                            Text="123"></asp:Label>
                        <br id="brCustomMessage" runat="server" />
                        <asp:Label ID="Label13" runat="server" Style="text-align: left" Font-Bold="true"
                            Text="检验报告"></asp:Label>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="lblZH" runat="server" Font-Bold="True"></asp:Label>
                        <asp:Panel ID="Panel3" runat="server" Width="100%" Height="70px" ScrollBars="Auto"
                            HorizontalAlign="Left">
                            <table width="100%">
                                <tr>
                                    <td>
                                        姓名:
                                        <asp:Label ID="lblName" runat="server" Font-Bold="True"></asp:Label>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        性别:
                                        <asp:Label ID="lblSex" runat="server" Font-Bold="True"></asp:Label>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        年龄:
                                        <asp:Label ID="lblAge" runat="server" Font-Bold="True"></asp:Label>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        诊疗号:
                                        <asp:Label ID="lblZY" runat="server" Font-Bold="True"></asp:Label>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        送检时间:
                                        <asp:Label ID="lblSJDate" runat="server" Font-Bold="True"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        送检科室:
                                        <asp:Label ID="lblSJKS" runat="server" Font-Bold="True"></asp:Label>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        床号:
                                        <asp:Label ID="lblCH" runat="server" Font-Bold="True"></asp:Label>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        诊断:
                                        <asp:Label ID="lblZD" runat="server" Font-Bold="True"></asp:Label>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        样本序号:<asp:Label ID="lblYBNumber" runat="server" Font-Bold="True"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        标本类别:<asp:Label ID="lblBBType" runat="server" Font-Bold="True"></asp:Label>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        标本备注：<asp:Label ID="lblBBBZ" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        标本状态:<asp:Label ID="lblBBZT" runat="server" Font-Bold="True"></asp:Label>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        样本条码号:<asp:Label ID="lblYBTM" runat="server" Font-Bold="True"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="Panel4" runat="server" HorizontalAlign="Left" ScrollBars="Auto" Width="100%"
                            Height="400">
                            <!--<br>-->
                            <!--<iframe id="showReport" scrolling="no" src="" width="710px" height="460" frameborder="0"></iframe>-->
                            <asp:GridView ID="gvResulto" runat="server" Width="100%" valign="top" CellPadding="4"
                                ForeColor="#333333" GridLines="None" AutoGenerateColumns="False" 
                                DataKeyNames="res_key" onrowdatabound="gvResulto_RowDataBound">
                                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" HorizontalAlign="Left" />
                                <Columns>
                                    <asp:BoundField DataField="itm_name" HeaderText="项目" SortExpression="itm_name" />
                                    <asp:BoundField DataField="res_itm_ecd" HeaderText="" SortExpression="res_itm_ecd" />
                                    <asp:BoundField DataField="res_chr" HeaderText="结果" SortExpression="res_chr" />
                                    <%--   <asp:BoundField DataField="res_chr2" HeaderText="浓度" SortExpression="res_chr2" />
                                    <asp:BoundField DataField="res_chr3" HeaderText="级别" SortExpression="res_chr3" />--%>
                                    <asp:BoundField DataField="res_prompt" HeaderText="   " SortExpression="res_prompt" />
                                    <asp:BoundField DataField="res_unit" HeaderText="单位" SortExpression="res_unit" />
                                    <asp:BoundField DataField="res_ref" HeaderText="参考值" SortExpression="res_ref" />
                                    <asp:BoundField DataField="res_meams" HeaderText="实验方法" SortExpression="res_meams" />
                                </Columns>
                                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" HorizontalAlign="Left" />
                                <EditRowStyle BackColor="#999999" />
                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                            </asp:GridView>
                            <asp:GridView ID="gvBac" runat="server" CellPadding="4" Width="100%" ForeColor="#333333"
                                GridLines="None" Visible="False" AutoGenerateColumns="False">
                                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" HorizontalAlign="Left" />
                                <Columns>
                                    <asp:BoundField DataField="bac_cname" HeaderText="检测菌株" SortExpression="bac_cname" />
                                    <asp:BoundField DataField="bar_bcnt" HeaderText="菌落计数" SortExpression="bar_bcnt" />
                                </Columns>
                                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" HorizontalAlign="Left" />
                                <EditRowStyle BackColor="#999999" />
                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                            </asp:GridView>
                            <asp:GridView ID="gvAnti" runat="server" CellPadding="4" Width="100%" ForeColor="#333333"
                                GridLines="None" Visible="False" AutoGenerateColumns="False">
                                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" HorizontalAlign="Left" />
                                <Columns>
                                    <asp:BoundField DataField="anti_code" HeaderText="简称" SortExpression="anti_code" />
                                    <asp:BoundField DataField="anti_cname" HeaderText="抗生素" SortExpression="anti_cname" />
                                    <asp:BoundField DataField="anr_mic" HeaderText="药敏性" SortExpression="anr_mic" />
                                    <asp:BoundField DataField="anr_smic1" HeaderText="MIC" SortExpression="anr_smic1" />
                                    <asp:BoundField DataField="ss_hstd" HeaderText="敏感" SortExpression="ss_hstd" />
                                    <asp:BoundField DataField="ss_mstd" HeaderText="中介" SortExpression="ss_mstd" />
                                    <asp:BoundField DataField="ss_lstd" HeaderText="耐药" SortExpression="ss_lstd" />
                                </Columns>
                                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" HorizontalAlign="Left" />
                                <EditRowStyle BackColor="#999999" />
                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                            </asp:GridView>
                            <asp:GridView ID="gvSmear" runat="server" CellPadding="4" Width="100%" ForeColor="#333333"
                                GridLines="None" Visible="False" AutoGenerateColumns="False">
                                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" HorizontalAlign="Left" />
                                <Columns>
                                    <asp:BoundField DataField="bsr_cname" HeaderText="描述报告" SortExpression="bsr_cname" />
                                    <asp:BoundField DataField="bsr_i_flag" HeaderText="阳性标志" SortExpression="bsr_i_flag" />
                                </Columns>
                                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" HorizontalAlign="Left" />
                                <EditRowStyle BackColor="#999999" />
                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                            </asp:GridView>
                        </asp:Panel>
                        <asp:Panel ID="pl4" runat="server" Width="100%" Height="80px" ScrollBars="Auto"
                            HorizontalAlign="Left">
                            <table width="100%">
                                <tr>
                                    <td>
                                        检测仪器:
                                        <asp:Label ID="lblMid" runat="server" Font-Bold="True"></asp:Label>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        <%-- 性别:
                                <asp:Label ID="Label7" runat="server" Font-Bold="True"></asp:Label>--%>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        采样时间:
                                        <asp:Label ID="lblCYdate" runat="server" Font-Bold="True"></asp:Label>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        打印时间:
                                        <asp:Label ID="lblDydate" runat="server" Font-Bold="True"></asp:Label>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        申请医生:
                                        <asp:Label ID="lblSQdoc" runat="server" Font-Bold="True"></asp:Label>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        录入者:
                                        <asp:Label ID="lblLR" runat="server" Font-Bold="True"></asp:Label>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        审核者:
                                        <asp:Label ID="lblSH" runat="server" Font-Bold="True"></asp:Label>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        报告者:
                                        <asp:Label ID="lblBG" runat="server" Font-Bold="True"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        申请时间:<asp:Label ID="lblSQdate" runat="server" Font-Bold="True"></asp:Label>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        检验时间：<asp:Label ID="lblJYdate" runat="server" Font-Bold="True"></asp:Label>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        审核时间:<asp:Label ID="lblSHdate" runat="server" Font-Bold="True"></asp:Label>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        报告时间:<asp:Label ID="lblBGdate" runat="server" Font-Bold="True"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:Panel ID="Panel1" runat="server" HorizontalAlign="Left">
        <table width="100%">
            <tr>
                <td>
                    <asp:HiddenField ID="valNoType" runat="server" />
                </td>
                <td>
                    <asp:HiddenField ID="valDepart" runat="server" />
                </td>
                <td>
                    <asp:HiddenField ID="valSample" runat="server" />
                </td>
                <td>
                    <asp:HiddenField ID="valCombine" runat="server" />
                </td>
                <td>
                    <asp:HiddenField ID="valOrigin" runat="server" />
                </td>
                <td>
                    <asp:HiddenField ID="valReportType" runat="server" />
                </td>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
        </table>
    </asp:Panel>
    </form>
</body>

</html>
