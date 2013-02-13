<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Fuqua.CompetativeAnalysis.MarketGame.Economy>" %>
<div id="agewealthTabs">
    <ul>
        <li><a href="#agewealthTabs_Age">Ages</a></li>
        <li><a href="#agewealthTabs_Wealth">Wealths</a></li>
    </ul>
    <div id="agewealthTabs_Age" index="1">
        <ul>
            <%foreach (var age in Model.Ages.OrderBy(a => a.DisplayOrder))
              {  %>
            <li>
                <%=age.Name %></li>
            <%} %>
        </ul>
    </div>
    <div id="agewealthTabs_Wealth" index="2">
        <ul>
            <%foreach (var wealth in Model.Wealths.OrderBy(a => a.DisplayOrder))
              {  %>
            <li>
                <%=wealth.Name %></li>
            <%} %>
        </ul>
    </div>
</div>
