<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmHistoryAnaly.aspx.cs"
    Inherits="dcl.pub.wcf.web.FrmHistoryAnaly" %>

<%@ Register Assembly="DevExpress.Web.v15.2" Namespace="DevExpress.Web"
    TagPrefix="dx" %>


<%@ Register Assembly="DevExpress.XtraCharts.v15.2.Web, Version=15.2.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraCharts.Web" TagPrefix="dxchartsui" %>

<%@ Register Assembly="DevExpress.XtraCharts.v15.2, Version=15.2.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraCharts" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link href="css/style.css" rel="stylesheet" type="text/css" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div style="height: 450px; width: 615px;">
                <table style="height: 450px; width: 615px">
                    <tr>
                        <td style="height: 225px; width: 500px">
                            <dxchartsui:WebChartControl ID="WebChartControl1" runat="server" Style="height: 225px;
                                width: 615px" >
                                <SeriesSerializable>
                                    <cc1:Series  Name="系列1" 
                                        >
                                        <ViewSerializable>
<cc1:LineSeriesView HiddenSerializableString="to be serialized">
                                        </cc1:LineSeriesView>
</ViewSerializable>
                                        <LabelSerializable>
<cc1:PointSeriesLabel HiddenSerializableString="to be serialized" OverlappingOptionsTypeName="PointOverlappingOptions">
                                            <FillStyle >
                                                <OptionsSerializable>
<cc1:SolidFillOptions HiddenSerializableString="to be serialized" />
</OptionsSerializable>
                                            </FillStyle>
                                        </cc1:PointSeriesLabel>
</LabelSerializable>
                                        <PointOptionsSerializable>
<cc1:PointOptions HiddenSerializableString="to be serialized">
                                        </cc1:PointOptions>
</PointOptionsSerializable>
                                        <LegendPointOptionsSerializable>
<cc1:PointOptions HiddenSerializableString="to be serialized">
                                        </cc1:PointOptions>
</LegendPointOptionsSerializable>
                                    </cc1:Series>
                                    <cc1:Series  Name="系列2" 
                                        >
                                        <ViewSerializable>
<cc1:LineSeriesView HiddenSerializableString="to be serialized">
                                        </cc1:LineSeriesView>
</ViewSerializable>
                                        <LabelSerializable>
<cc1:PointSeriesLabel HiddenSerializableString="to be serialized" OverlappingOptionsTypeName="PointOverlappingOptions">
                                            <FillStyle >
                                                <OptionsSerializable>
<cc1:SolidFillOptions HiddenSerializableString="to be serialized" />
</OptionsSerializable>
                                            </FillStyle>
                                        </cc1:PointSeriesLabel>
</LabelSerializable>
                                        <PointOptionsSerializable>
<cc1:PointOptions HiddenSerializableString="to be serialized">
                                        </cc1:PointOptions>
</PointOptionsSerializable>
                                        <LegendPointOptionsSerializable>
<cc1:PointOptions HiddenSerializableString="to be serialized">
                                        </cc1:PointOptions>
</LegendPointOptionsSerializable>
                                    </cc1:Series>
                                </SeriesSerializable>
                                <SeriesTemplate  
                                    >
                                    <ViewSerializable>
<cc1:LineSeriesView HiddenSerializableString="to be serialized">
                                    </cc1:LineSeriesView>
</ViewSerializable>
                                    <LabelSerializable>
<cc1:PointSeriesLabel HiddenSerializableString="to be serialized" OverlappingOptionsTypeName="PointOverlappingOptions">
                                        <FillStyle >
                                            <OptionsSerializable>
<cc1:SolidFillOptions HiddenSerializableString="to be serialized" />
</OptionsSerializable>
                                        </FillStyle>
                                    </cc1:PointSeriesLabel>
</LabelSerializable>
                                    <PointOptionsSerializable>
<cc1:PointOptions HiddenSerializableString="to be serialized">
                                    </cc1:PointOptions>
</PointOptionsSerializable>
                                    <LegendPointOptionsSerializable>
<cc1:PointOptions HiddenSerializableString="to be serialized">
                                    </cc1:PointOptions>
</LegendPointOptionsSerializable>
                                </SeriesTemplate>
                                <DiagramSerializable>
<cc1:XYDiagram>
                                    <axisx visibleinpanesserializable="-1">
<range sidemarginsenabled="True"></range>
</axisx>
                                    <axisy visibleinpanesserializable="-1">
<range sidemarginsenabled="True"></range>
</axisy>
                                </cc1:XYDiagram>
</DiagramSerializable>
                                <FillStyle >
                                    <OptionsSerializable>
<cc1:SolidFillOptions HiddenSerializableString="to be serialized"></cc1:SolidFillOptions>
</OptionsSerializable>
                                </FillStyle>
                            </dxchartsui:WebChartControl>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Panel ID="PlResult" runat="server" HorizontalAlign="Left" ScrollBars="Auto"
                                Width="615px" Height="225px">
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
    </form>
</body>
</html>
