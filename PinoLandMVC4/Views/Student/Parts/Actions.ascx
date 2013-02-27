<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<PinoLandMVC4.Models.FoodTeamAction>>" %>

<div id="RestaurantPopup" style="display:none;">

</div>

<p>
    <a id="CreateNewAction">Create New</a>
</p>
<table>
    <tr>
        <th>Team</th>
        <th>Type</th>
        <th>Location</th>

        <th>Capacity</th>
        <th>Price</th>

        <th>Capacity Change</th>
        <th>Production</th>
        <th>Price Next</th>

        <th></th>
    </tr>
    <tr id="NewRestaurant" style="display: none">
        <td></td>
        <td></td>
        <td id="latlng"></td>
        <td>0</td>
        <td>$0.00</td>
        <td></td>
        <td>0</td>
        <td>$0.00</td>
        <td><a>Edit</a> | <a>Delete</a>
        </td>
    </tr>
    <% foreach (var item in Model)
       { %>
    <tr>
        <td><%: item.Team %></td>
        <td><%: item.Type %></td>
        <td><%: string.Format("{0:F3} : {1:F3}", item.Latitude, item.Longitude) %></td>

        <td><%: item.CapacityNow %></td>
        <td><%: item.Price %></td>

        <td><%: item.CapacityChange %></td>
        <td><%: item.ProductionNext%></td>
        <td><%: item.PriceNext %></td>

        <td>
            <%if (item.IsMine)
              {%>
            <a>Edit</a> | <a>Delete</a>
            <%} %>
        </td>
    </tr>
    <% } %>
</table>
