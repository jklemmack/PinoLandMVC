<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Fuqua.CompetativeAnalysis.MarketGame.Economy>" %>
<fieldset>
    <legend>Economy</legend>
    <div class="display-label">
        <%: Html.DisplayNameFor(model => model.Name) %>
    </div>
    <div class="display-field">
        <%: Html.DisplayFor(model => model.Name) %>
    </div>
    <div class="display-label">
        <%: Html.DisplayNameFor(model => model.DateCreated) %>
    </div>
    <div class="display-field">
        <%: Html.DisplayFor(model => model.DateCreated) %>
    </div>
    <div class="display-label">
        <%: Html.DisplayNameFor(model => model.CurrentRoundId) %>
    </div>
    <div class="display-field">
        <%: Html.DisplayFor(model => (model.CurrentRound != null) ? model.CurrentRound.Identifier : "" %>
    </div>
    <div class="display-label">
        <%: Html.DisplayNameFor(model => model.Reference) %>
    </div>
    <div class="display-field">
        <%: Html.DisplayFor(model => model.Reference) %>
    </div>
</fieldset>
<p>
    <%: Html.ActionLink("Edit", "Edit", new { id=Model.EconomyId }) %>
    |
    <%: Html.ActionLink("Back to List", "Index") %>
</p>
