<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Fuqua.CompetativeAnalysis.MarketGame.Food_Industry>" %>


<% using (Html.BeginForm()) { %>
    <%: Html.ValidationSummary(true) %>

    <fieldset>
        <legend>Food_Industry</legend>

        <div class="editor-label">
            <%: Html.LabelFor(model => model.EntryCostMean) %>
        </div>
        <div class="editor-field">
            <%: Html.EditorFor(model => model.EntryCostMean) %>
            <%: Html.ValidationMessageFor(model => model.EntryCostMean) %>
        </div>

        <div class="editor-label">
            <%: Html.LabelFor(model => model.EntryCostStdDev) %>
        </div>
        <div class="editor-field">
            <%: Html.EditorFor(model => model.EntryCostStdDev) %>
            <%: Html.ValidationMessageFor(model => model.EntryCostStdDev) %>
        </div>

        <div class="editor-label">
            <%: Html.LabelFor(model => model.CapacityCostMean) %>
        </div>
        <div class="editor-field">
            <%: Html.EditorFor(model => model.CapacityCostMean) %>
            <%: Html.ValidationMessageFor(model => model.CapacityCostMean) %>
        </div>

        <div class="editor-label">
            <%: Html.LabelFor(model => model.CapacityCostStdDev) %>
        </div>
        <div class="editor-field">
            <%: Html.EditorFor(model => model.CapacityCostStdDev) %>
            <%: Html.ValidationMessageFor(model => model.CapacityCostStdDev) %>
        </div>

        <div class="editor-label">
            <%: Html.LabelFor(model => model.MarginalCostMean) %>
        </div>
        <div class="editor-field">
            <%: Html.EditorFor(model => model.MarginalCostMean) %>
            <%: Html.ValidationMessageFor(model => model.MarginalCostMean) %>
        </div>

        <div class="editor-label">
            <%: Html.LabelFor(model => model.MarginalCostStdDev) %>
        </div>
        <div class="editor-field">
            <%: Html.EditorFor(model => model.MarginalCostStdDev) %>
            <%: Html.ValidationMessageFor(model => model.MarginalCostStdDev) %>
        </div>

        <div class="editor-label">
            <%: Html.LabelFor(model => model.CapacityDecayRate) %>
        </div>
        <div class="editor-field">
            <%: Html.EditorFor(model => model.CapacityDecayRate) %>
            <%: Html.ValidationMessageFor(model => model.CapacityDecayRate) %>
        </div>

        <div class="editor-label">
            <%: Html.LabelFor(model => model.CapacityResaleRate) %>
        </div>
        <div class="editor-field">
            <%: Html.EditorFor(model => model.CapacityResaleRate) %>
            <%: Html.ValidationMessageFor(model => model.CapacityResaleRate) %>
        </div>

        <div class="editor-label">
            <%: Html.LabelFor(model => model.CanHoldInventory) %>
        </div>
        <div class="editor-field">
            <%: Html.EditorFor(model => model.CanHoldInventory) %>
            <%: Html.ValidationMessageFor(model => model.CanHoldInventory) %>
        </div>

        <div class="editor-label">
            <%: Html.LabelFor(model => model.Elasticity) %>
        </div>
        <div class="editor-field">
            <%: Html.EditorFor(model => model.Elasticity) %>
            <%: Html.ValidationMessageFor(model => model.Elasticity) %>
        </div>

        <div class="editor-label">
            <%: Html.LabelFor(model => model.MaintenanceCostMean) %>
        </div>
        <div class="editor-field">
            <%: Html.EditorFor(model => model.MaintenanceCostMean) %>
            <%: Html.ValidationMessageFor(model => model.MaintenanceCostMean) %>
        </div>

        <div class="editor-label">
            <%: Html.LabelFor(model => model.MaintenanceCostStdDev) %>
        </div>
        <div class="editor-field">
            <%: Html.EditorFor(model => model.MaintenanceCostStdDev) %>
            <%: Html.ValidationMessageFor(model => model.MaintenanceCostStdDev) %>
        </div>

        <div class="editor-label">
            <%: Html.LabelFor(model => model.InventoryCostMean) %>
        </div>
        <div class="editor-field">
            <%: Html.EditorFor(model => model.InventoryCostMean) %>
            <%: Html.ValidationMessageFor(model => model.InventoryCostMean) %>
        </div>

        <div class="editor-label">
            <%: Html.LabelFor(model => model.InventoryCostStdDev) %>
        </div>
        <div class="editor-field">
            <%: Html.EditorFor(model => model.InventoryCostStdDev) %>
            <%: Html.ValidationMessageFor(model => model.InventoryCostStdDev) %>
        </div>

        <%: Html.HiddenFor(model => model.IndustryId) %>

        <%: Html.HiddenFor(model => model.EconomyId) %>

        <div class="editor-label">
            <%: Html.LabelFor(model => model.Name) %>
        </div>
        <div class="editor-field">
            <%: Html.EditorFor(model => model.Name) %>
            <%: Html.ValidationMessageFor(model => model.Name) %>
        </div>

        <div class="editor-label">
            <%: Html.LabelFor(model => model.Type) %>
        </div>
        <div class="editor-field">
            <%: Html.EditorFor(model => model.Type) %>
            <%: Html.ValidationMessageFor(model => model.Type) %>
        </div>

        <p>
            <input type="submit" value="Save" />
        </p>
    </fieldset>
<% } %>

<div>
    <%: Html.ActionLink("Back to List", "Index") %>
</div>
