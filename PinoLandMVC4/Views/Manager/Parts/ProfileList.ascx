<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<Fuqua.CompetativeAnalysis.MarketGame.Profile>>" %>
<div id="profileTabs">
    <ul>
        <% foreach (var profile in Model)
           { %>
        <li><a href="#profileTabs_tab<%=profile.Name.Replace(" ", "_")%>">
            <%=profile.Name%></a></li>
        <% } %>
    </ul>
    <% int index = 1;
        foreach (var profile in Model)
       { %>
    <div id="profileTabs_tab<%=profile.Name.Replace(" ", "_")%>" index="<%=index++ %>">
        <% Html.RenderPartial("Parts/ProfileOverview", profile); %>
    </div>
    <% } %>
</div>
