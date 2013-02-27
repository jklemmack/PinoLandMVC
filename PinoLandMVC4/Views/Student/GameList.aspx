<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<PinoLandMVC4.Models.StudentGame>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    GameList
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        GameList</h2>
    <table>
        <% foreach (var game in Model)
           {%><tr>
               <td>
                   <%: game.GameReference %>
               </td>
               <td>
                   <%:game.TeamName %>
               </td>
               <td>
                   <%: Html.ActionLink("Select game", "Details", new { economy = game.EconomyId, team = game.TeamId }); %>
               </td>
           </tr>
        <%} %>
    </table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>
