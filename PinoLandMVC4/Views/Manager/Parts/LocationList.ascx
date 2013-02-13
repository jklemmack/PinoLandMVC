<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<Fuqua.CompetativeAnalysis.MarketGame.Location>>" %>
<%
    //int minX = (int)Model.Min(l => l.CenterX);
    int maxX = (int)Model.Max(l => l.CenterX);
        //int minY = (int)Model.Min(l => l.CenterY);
        //int maxY = (int)Model.Max(l => l.CenterY);
%>
<table id="tabletwo">
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
