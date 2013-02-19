<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<Fuqua.CompetativeAnalysis.MarketGame.Company>>" %>
<div id="teamTabs">
    <ul>
        <% foreach (var team in Model)
           { %>
        <li><a href="#teamTabs_tab<%=team.Name.Replace(" ", "_")%>">
            <%=team.Name%></a></li>
        <% } %>
        <li><a href="#teamTabs_tabAddNew">+ Add New</a></li>
    </ul>
    <% int index = 1;
       foreach (var team in Model)
       { %>
    <div id="teamTabs_tab<%=team.Name.Replace(" ", "_")%>" index="<%=index++ %>">
        <% Html.RenderPartial("Parts/TeamOverview", team); %>
    </div>
    <% } %>
    <div id="teamTabs_tabAddNew" index="0">
        Upload a team file:
        <form action="/Manager/UploadTeam" method="post" enctype="multipart/form-data">
        <input type="hidden" id="id" name="id" value="<%: ViewBag.EconomyId %>" />
        <label for="file">
            Filename:</label>
        <input type="file" name="file" id="file" />
        <input type="submit" />
        </form>
        <a href="../../../Content/Team File.xlsx">Download sample team file</a>
    </div>
</div>
