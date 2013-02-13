<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<% if (Request.IsAuthenticated)
   {  %>
<div id="logoff" class="logon">
    <%: Page.User.Identity.Name %> [ <%: Html.ActionLink("Log Off", "LogOff", "Account") %> ]
</div>
<%}
   else
   {  %><div id="logon" class="logon">
       <%: Html.ActionLink("Log On", "Logon", "Account") %></div>
<%} %>