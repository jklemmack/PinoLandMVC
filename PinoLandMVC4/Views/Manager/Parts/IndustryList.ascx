<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<Fuqua.CompetativeAnalysis.MarketGame.Industry>>" %>
<div id="industryTabs">
    <ul>
        <% foreach (var industry in Model)
           { %>
        <li><a href="#industryTabs_tab<%=industry.Name.Replace(" ", "_")%>">
            <%=industry.Name%></a></li>
        <% } %>
    </ul>
    <% int index = 1;
       foreach (var industry in Model)
       { %>
    <div id="industryTabs_tab<%=industry.Name.Replace(" ", "_")%>" index="<%=index++ %>">
        <% Html.RenderPartial("Parts/Partial1", industry); %>
    </div>
    <% } %>
</div>
