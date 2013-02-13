<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<Fuqua.CompetativeAnalysis.MarketGame.Company>>" %>

<p>
    <%: Html.ActionLink("Create New", "Create") %>
</p>
<table>
    <tr>
        <th>
            <%: Html.DisplayNameFor(model => model.Name) %>
        </th>
        <th></th>
    </tr>

<% foreach (var item in Model) { %>
    <tr>
        <td>
            <%: Html.DisplayFor(modelItem => item.Name) %>
        </td>

    </tr>
<% } %>

</table>
