<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<Fuqua.CompetativeAnalysis.MarketGame.Company>>" %>
<div id="teamTabs">
    <ul>
        <% foreach (var team in Model)
           { %>
        <li><a href="#teamTabs_tab<%=team.Name.Replace(" ", "_")%>">
            <%=team.Name%></a></li>
        <% } %>
    </ul>
    <% int index = 1;
       foreach (var team in Model)
       { %>
    <div id="teamTabs_tab<%=team.Name.Replace(" ", "_")%>" index="<%=index++ %>">
        <% Html.RenderPartial("Parts/TeamOverview", team); %>
    </div>
    <% } %>
</div>
