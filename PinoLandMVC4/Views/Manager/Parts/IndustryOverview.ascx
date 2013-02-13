<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Fuqua.CompetativeAnalysis.MarketGame.Food_Industry>" %>


<fieldset>
    <legend>Food_Industry</legend>

    <div class="display-label">
        <%: Html.DisplayNameFor(model => model.EntryCostMean) %>
    </div>
    <div class="display-field">
        <%: Html.DisplayFor(model => model.EntryCostMean) %>
    </div>

    <div class="display-label">
        <%: Html.DisplayNameFor(model => model.EntryCostStdDev) %>
    </div>
    <div class="display-field">
        <%: Html.DisplayFor(model => model.EntryCostStdDev) %>
    </div>

    <div class="display-label">
        <%: Html.DisplayNameFor(model => model.CapacityCostMean) %>
    </div>
    <div class="display-field">
        <%: Html.DisplayFor(model => model.CapacityCostMean) %>
    </div>

    <div class="display-label">
        <%: Html.DisplayNameFor(model => model.CapacityCostStdDev) %>
    </div>
    <div class="display-field">
        <%: Html.DisplayFor(model => model.CapacityCostStdDev) %>
    </div>

    <div class="display-label">
        <%: Html.DisplayNameFor(model => model.MarginalCostMean) %>
    </div>
    <div class="display-field">
        <%: Html.DisplayFor(model => model.MarginalCostMean) %>
    </div>

    <div class="display-label">
        <%: Html.DisplayNameFor(model => model.MarginalCostStdDev) %>
    </div>
    <div class="display-field">
        <%: Html.DisplayFor(model => model.MarginalCostStdDev) %>
    </div>

    <div class="display-label">
        <%: Html.DisplayNameFor(model => model.CapacityDecayRate) %>
    </div>
    <div class="display-field">
        <%: Html.DisplayFor(model => model.CapacityDecayRate) %>
    </div>

    <div class="display-label">
        <%: Html.DisplayNameFor(model => model.CapacityResaleRate) %>
    </div>
    <div class="display-field">
        <%: Html.DisplayFor(model => model.CapacityResaleRate) %>
    </div>

    <div class="display-label">
        <%: Html.DisplayNameFor(model => model.CanHoldInventory) %>
    </div>
    <div class="display-field">
        <%: Html.DisplayFor(model => model.CanHoldInventory) %>
    </div>

    <div class="display-label">
        <%: Html.DisplayNameFor(model => model.Elasticity) %>
    </div>
    <div class="display-field">
        <%: Html.DisplayFor(model => model.Elasticity) %>
    </div>

    <div class="display-label">
        <%: Html.DisplayNameFor(model => model.MaintenanceCostMean) %>
    </div>
    <div class="display-field">
        <%: Html.DisplayFor(model => model.MaintenanceCostMean) %>
    </div>

    <div class="display-label">
        <%: Html.DisplayNameFor(model => model.MaintenanceCostStdDev) %>
    </div>
    <div class="display-field">
        <%: Html.DisplayFor(model => model.MaintenanceCostStdDev) %>
    </div>

    <div class="display-label">
        <%: Html.DisplayNameFor(model => model.InventoryCostMean) %>
    </div>
    <div class="display-field">
        <%: Html.DisplayFor(model => model.InventoryCostMean) %>
    </div>

    <div class="display-label">
        <%: Html.DisplayNameFor(model => model.InventoryCostStdDev) %>
    </div>
    <div class="display-field">
        <%: Html.DisplayFor(model => model.InventoryCostStdDev) %>
    </div>

    <div class="display-label">
        <%: Html.DisplayNameFor(model => model.Name) %>
    </div>
    <div class="display-field">
        <%: Html.DisplayFor(model => model.Name) %>
    </div>

    <div class="display-label">
        <%: Html.DisplayNameFor(model => model.Type) %>
    </div>
    <div class="display-field">
        <%: Html.DisplayFor(model => model.Type) %>
    </div>
</fieldset>