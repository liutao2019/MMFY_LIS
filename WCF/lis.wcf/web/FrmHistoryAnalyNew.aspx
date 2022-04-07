<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmHistoryAnalyNew.aspx.cs"
    Inherits="dcl.pub.wcf.web.FrmHistoryAnalyNew" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link href="css/style.css" rel="stylesheet" type="text/css" />
    <title></title>
</head>

<script src="js/jquery-1.7.2.min.js" type="text/javascript"></script>

<script src="js/highcharts.js" type="text/javascript"></script>

<script src="js/reportscript.js" type="text/javascript"></script>

<script type="text/javascript">

    var LoadHigChar = function () {
        document.getElementById('PlResult').scrollTop = document.getElementById('ScrollPosY').value;
        //获取json
        var json = eval($("#txttest").val());
        //标题
        var title = json.title;
        //子标题
        var subtitle = json.subtitle;
        //日期(Y轴线)
        var categories = json.categories;
        //数据(X轴线)
        var data = json.data;
        //单位
        var yunit = json.yunit;

        drawChart("item-chart", title, subtitle, categories, data, yunit);
    };
    
Highcharts.theme = {
	colors: ["#514F78", "#42A07B", "#9B5E4A", "#72727F", "#1F949A", "#82914E", "#86777F", "#42A07B"],
	chart: {
		className: 'skies',
		borderWidth: 0,
		plotShadow: true,
		plotBackgroundImage: 'images/skies.jpg',
		plotBackgroundColor: {
			linearGradient: [0, 0, 250, 500],
			stops: [
				[0, 'rgba(255, 255, 255, 1)'],
				[1, 'rgba(255, 255, 255, 0)']
			]
		},
		plotBorderWidth: 1
	},
	title: {
		style: {
			color: '#3E576F',
			font: '16px Lucida Grande, Lucida Sans Unicode, Verdana, Arial, Helvetica, sans-serif'
		}
	},
	subtitle: {
		style: {
			color: '#6D869F',
			font: '12px Lucida Grande, Lucida Sans Unicode, Verdana, Arial, Helvetica, sans-serif'
		}
	},
	xAxis: {
		gridLineWidth: 0,
		lineColor: '#C0D0E0',
		tickColor: '#C0D0E0',
		labels: {
			style: {
				color: '#666',
				fontWeight: 'bold'
			}
		},
		title: {
			style: {
				color: '#666',
				font: '12px Lucida Grande, Lucida Sans Unicode, Verdana, Arial, Helvetica, sans-serif'
			}
		}
	},
	yAxis: {
		alternateGridColor: 'rgba(255, 255, 255, .5)',
		lineColor: '#C0D0E0',
		tickColor: '#C0D0E0',
		tickWidth: 1,
		labels: {
			style: {
				color: '#666',
				fontWeight: 'bold'
			}
		},
		title: {
			style: {
				color: '#666',
				font: '12px Lucida Grande, Lucida Sans Unicode, Verdana, Arial, Helvetica, sans-serif'
			}
		}
	},
	legend: {
		itemStyle: {
			font: '9pt Trebuchet MS, Verdana, sans-serif',
			color: '#3E576F'
		},
		itemHoverStyle: {
			color: 'black'
		},
		itemHiddenStyle: {
			color: 'silver'
		}
	},
	labels: {
		style: {
			color: '#3E576F'
		}
	}
};

// Apply the theme
var highchartsOptions = Highcharts.setOptions(Highcharts.theme);
window.onload = LoadHigChar;
</script>

<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div style="height: 450px; width: 615px;">
                <table style="height: 450px; width: 615px">
                    <tr>
                        <td style="height: 225px; width: 610px">
                            <div id="item-chart" style="height: 225px; margin: 0 auto">
                            </div>
                            <asp:TextBox ID="txttest" runat="server" Style="display: none;">
                            </asp:TextBox>
                            <input type="hidden" name="_ScrollPosY" runat="server" id="ScrollPosY" value="0" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Panel ID="PlResult" runat="server" HorizontalAlign="Left" ScrollBars="Auto"
                                Width="610px" Height="225px">
                                <asp:GridView ID="gridHistory" runat="server" AllowPaging="false" AllowSorting="false"
                                    AutoGenerateColumns="true" CellPadding="4" OnRowCreated="gridHistory_RowCreated"
                                    PagerSettings-Mode="NumericFirstLast" PageSize="15" RowStyle-Wrap="False" SelectedRowStyle-Wrap="False"
                                    OnRowDataBound="gridHistory_RowDataBound" BackColor="White" BorderColor="#3366CC"
                                    BorderStyle="None" BorderWidth="1px" OnSelectedIndexChanged="gridHistory_SelectedIndexChanged">
                                    <PagerSettings Mode="NumericFirstLast" />
                                    <RowStyle Wrap="False" />
                                    <PagerStyle BackColor="#99CCCC" ForeColor="#003399" HorizontalAlign="Left" />
                                    <SelectedRowStyle BackColor="#009999" Font-Bold="True" ForeColor="#CCFF99" />
                                    <HeaderStyle BackColor="#003399" Font-Bold="True" ForeColor="#CCCCFF" HorizontalAlign="Left" />
                                    <Columns>
                                        <asp:CommandField ShowSelectButton="True" SelectText=" ">
                                            <ItemStyle Width="10px" />
                                        </asp:CommandField>
                                    </Columns>
                                </asp:GridView>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
