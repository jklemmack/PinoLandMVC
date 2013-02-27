<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<PinoLandMVC4.Models.FoodTeamAction>" %>

<script src="<%: Url.Content("~/Scripts/jquery-1.7.1.min.js") %>"></script>
<script src="<%: Url.Content("~/Scripts/jquery.validate.min.js") %>"></script>
<script src="<%: Url.Content("~/Scripts/jquery.validate.unobtrusive.min.js") %>"></script>

<% using (Html.BeginForm()) { %>
    <%: Html.ValidationSummary(true) %>

    <fieldset>
        <legend>FoodTeamAction</legend>

        <p>
            <input type="submit" value="Save" />
        </p>
    </fieldset>
<% } %>

<div>
    <%: Html.ActionLink("Back to List", "Index") %>
</div>
