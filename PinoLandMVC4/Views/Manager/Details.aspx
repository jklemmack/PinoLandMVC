<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<PinoLandMVC4.Helpers.PageModel<Fuqua.CompetativeAnalysis.MarketGame.Economy>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Details
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Details of
        <%: Model.Reference %></h2>
    <p>
        <%: Html.ActionLink("Back to List", "Index") %>
        <%: Html.ActionLink("Start Game", "StartGame", new { id = Model.Data.EconomyId })%>
    </p>
    <div id="mainTabs">
        <ul>
            <li><a href="#tabOverview">Overview</a></li>
            <li><a href="#tabCompanies">Companies</a></li>
            <%--<li><a href="#tabAgeWealth">Age & Wealth</a></li>--%>
            <li><a href="#tabProfiles">Profiles</a></li>
            <li><a href="#tabLocations">Locations</a></li>
            <li><a href="#tabIndustries">Industries</a></li>
        </ul>
        <div id="tabOverview" index="1">
            <% %>
        </div>
        <div id="tabCompanies" index="2">
            <% Html.RenderPartial("Parts/TeamList", Model.Data.Companies); %>
        </div>
        <%--        <div id="tabAgeWealth" index="3">
            <% Html.RenderPartial("Parts/AgeWealthList", Model); %></div>
        --%>
        <div id="tabProfiles" index="4">
            <% Html.RenderPartial("Parts/ProfileList", Model.Data.Profiles); %></div>
        <div id="tabLocations" index="5">
            <% Html.RenderPartial("Parts/LocationList", Model.Data.Locations); %></div>
        <div id="tabIndustries" index="6">
            <% Html.RenderPartial("Parts/IndustryList", Model.Data.Industries); %>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
    <script src="../../Scripts/Site/Industry.js" type="text/javascript"></script>
    <script src="../../Scripts/Site/Economy.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery.columnhover.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">

        $(function () {

            $("#mainTabs").tabs();

            // Vertical tabs
            $("#industryTabs").tabs().addClass("ui-tabs-vertical ui-helper-clearfix");
            $("#industryTabs li").removeClass("ui-corner-top").addClass("ui-corner-left");

            $("#teamTabs").tabs().addClass("ui-tabs-vertical ui-helper-clearfix");
            $("#teamTabs li").removeClass("ui-corner-top").addClass("ui-corner-left");

            $("#agewealthTabs").tabs().addClass("ui-tabs-vertical ui-helper-clearfix");
            $("#agewealthTabs li").removeClass("ui-corner-top").addClass("ui-corner-left");

            $("#profileTabs").tabs().addClass("ui-tabs-vertical ui-helper-clearfix");
            $("#profileTabs li").removeClass("ui-corner-top").addClass("ui-corner-left");


            // Location / map grid
            $("#LocationTable").columnHover({ eachCell: true, hoverClass: 'columnhover' });  //hovering
            $('#tabletwo').columnHover({ eachCell: true, hoverClass: 'betterhover' });


            $(".ui-tabs").on("tabsactivate", function (event, ui) {

                var p = [];
                p.push(ui.newPanel.attr("index"));
                var q = ui.newPanel.parents(".ui-tabs-panel").filter("[index]").map(function () {
                    return this.getAttribute("index");
                }).get();
                if (q.length > 0) p.push(q);
                p.reverse();
                location.hash = p.join("&");
            });


            PageLoad();
        });

        $(document).ready(function () {
            $('#tabletwo').columnHover({ eachCell: true, hoverClass: 'betterhover' });
        });

    </script>

    <style type="text/css">
        td.betterhover, #tabletwo tbody tr:hover
        {
            background: LightCyan;
        }
        
        .ui-tabs-vertical
        {
            width: 55em;
        }
        .ui-tabs-vertical .ui-tabs-nav
        {
            padding: .2em .1em .2em .2em;
            float: left;
            width: 12em;
        }
        .ui-tabs-vertical .ui-tabs-nav li
        {
            clear: left;
            width: 100%;
            border-bottom-width: 1px !important;
            border-right-width: 0 !important;
            margin: 0 -1px .2em 0;
        }
        .ui-tabs-vertical .ui-tabs-nav li a
        {
            display: block;
        }
        .ui-tabs-vertical .ui-tabs-nav li.ui-tabs-active
        {
            padding-bottom: 0;
            padding-right: .1em;
            border-right-width: 1px;
            border-right-width: 1px;
        }
        .ui-tabs-vertical .ui-tabs-panel
        {
            padding: 1em;
            float: right;
            width: 40em;
        }
        
        td.betterhover
        {
            background: LightCyan;
        }
    </style>
</asp:Content>
