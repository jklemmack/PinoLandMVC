<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Fuqua.CompetativeAnalysis.MarketGame.Profile>" %>
Probability Distribution
<table border="1">
    <tr>
        <td>
            &nbsp;
        </td>
        <%foreach (var wealth in Model.Economy.Wealths.OrderBy(w => w.DisplayOrder))
          {%>
        <td>
            <%: wealth.Name %>
        </td>
        <%} %>
    </tr>
    <%foreach (var age in Model.Economy.Ages.OrderBy(a => a.DisplayOrder))
      { %>
    <tr>
        <td>
            <%: age.Name %>
        </td>
        <%foreach (var wealth in Model.Economy.Wealths.OrderBy(w => w.DisplayOrder))
          { %><td>
              <%: Model.Profile_Age_Wealth.Single(paw=>paw.Age == age && paw.Wealth == wealth).Probability %>
          </td>
        <%} %>
    </tr>
    <%} %>
</table>
