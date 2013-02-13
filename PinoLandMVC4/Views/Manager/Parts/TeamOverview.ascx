<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Fuqua.CompetativeAnalysis.MarketGame.Company>" %>
<fieldset>
    <legend>Company</legend>
    <div class="display-label">
        <%: Html.DisplayNameFor(model => model.Name) %>
    </div>
    <div class="display-field">
        <%: Html.DisplayFor(model => model.Name) %>
    </div>
</fieldset>
<%foreach (var user in Model.Users)
  {%>
<table>
    <tr>
        <th>
            UserName
        </th>
        <th>
            First Name
        </th>
        <th>
            Last Name
        </th>
        <th>
            Email
        </th>
    </tr>
    <% foreach (var item in Model.Users)
       { %>
    <tr>
        <td>
            <%: Html.DisplayFor(modelItem => item.UserName) %>
        </td>
        <td>
            <%: Html.DisplayFor(modelItem => item.FirstName) %>
        </td>
        <td>
            <%: Html.DisplayFor(modelItem => item.LastName) %>
        </td>
        <td>
            <%: Html.DisplayFor(modelItem => item.Email) %>
        </td>
    </tr>
    <% } %>
</table>
<%} %>