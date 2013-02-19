<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<IEnumerabl<Fuqua.CompetativeAnalysis.MarketGame.Location>>" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <meta name="viewport" content="width=device-width" />
    <title>Locations</title>
</head>
   <script language="javascript" type="text/javascript">
       $(function () {
       });
    </script>
 <body>
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
</body>
</html>
