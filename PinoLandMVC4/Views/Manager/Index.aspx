<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<Fuqua.CompetativeAnalysis.MarketGame.Economy>>" %>

<%@ Import Namespace="Fuqua.CompetativeAnalysis.MarketGame" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Manager Page
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Index</h2>
    <table>
        <tr>
            <th>
                <%: Html.DisplayNameFor(model => model.Name) %>
            </th>
            <th>
                <%: Html.DisplayNameFor(model => model.DateCreated) %>
            </th>
            <th>
                <%: Html.DisplayNameFor(model => model.CurrentRoundId) %>
            </th>
            <th>
                <%: Html.DisplayNameFor(model => model.Reference) %>
            </th>
            <th>
            </th>
        </tr>
        <% foreach (var item in Model)
           { %>
        <tr>
            <td>
                <%: Html.DisplayFor(modelItem => item.Name) %>
            </td>
            <td>
                <%: Html.DisplayFor(modelItem => item.DateCreated) %>
            </td>
            <td>
                <%: Html.DisplayFor(modelItem => item.CurrentRoundId) %>
            </td>
            <td>
                <%: Html.DisplayFor(modelItem => item.Reference) %>
            </td>
            <td>
                <%: Html.ActionLink("Edit", "Details", new { id=item.EconomyId }) %>
                |
                <%: Html.ActionLink("Delete", "Delete", new { id=item.EconomyId }) %>
                |
                <%: Html.ActionLink("Start Game", "StartGame", new { id = item.EconomyId })%>
                |
                <%: Html.ActionLink("Process Round", "ProcessRound", new { id = item.EconomyId })%>
            </td>
        </tr>
        <% } %></table>
    <p>
        <%: Html.ActionLink("Create New Game", "Create") %></p>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>
