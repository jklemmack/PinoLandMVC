<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<Fuqua.CompetativeAnalysis.MarketGame.Location>>" %>
<%
    //int minX = (int)Model.Min(l => l.CenterX);
    int maxX = (int)Model.Max(l => l.CenterX);
        //int minY = (int)Model.Min(l => l.CenterY);
        //int maxY = (int)Model.Max(l => l.CenterY);
%>
<script src="../../../Scripts/jquery.jeditable.min.js" type="text/javascript"></script>
<script src="../../../Scripts/jquery.columnhover.pack.js" type="text/javascript"></script>
<script language="javascript" type="text/javascript">
    $(function () {
    });
</script>
<table id="tabletwo" style="background: url('/Tile/FullMap/500/500') no-repeat; width: 500px;
    height: 500px;" border="1">
    <%foreach (var loc in Model.OrderBy(l => l.CenterX).OrderBy(l => l.CenterY))
      {
          if (loc.CenterX == 1)
              Response.Write("<tr>");
    %><td>
        <%: loc.TotalPopulation%><br />
        <%: loc.Profile.Name%>
    </td>
    <%
          if (loc.CenterX == maxX)
              Response.Write("</tr>");
      }
    %>
</table>
