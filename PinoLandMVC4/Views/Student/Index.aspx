<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Index
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Index</h2>
    <div id="tabsMain">
        <ul>
            <li><a href="#tabHome">Home</a></li>
            <li><a href="#tabMap">Map</a></li>
            <li><a href="#tabActions">Actions</a></li>
            <li><a href="#tabReports">Reports</a></li>
        </ul>
        <div id="tabHome" index="1">
            <% Html.RenderPartial("Parts/News", null); %>
        </div>
        <div id="tabMap" index="2">
            <% Html.RenderPartial("Parts/SmallMap", null); %>
        </div>
        <div id="tabActions" index="3">
        </div>
        <div id="tabReports" index="4">
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
    <style>
        h3.news
        {
            padding: 0.5em;
        }
        h3.news span.news
        {
            padding-left: 1.0em;
        }
        div.news
        {
            padding: 0.5em;
            padding-left: 2.0em;
            margin-bottom: 0.5em;
        }
    </style>
    <link href="../../Content/leaflet.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/leaflet.js" type="text/javascript"></script>
    <script language="javascript" type="text/ecmascript">
        $(function () {
            $("#tabsMain").tabs().addClass("ui-tabs-vertical ui-helper-clearfix");
            $("#tabsMain li").removeClass("ui-corner-top").addClass("ui-corner-left");


        });
    </script>
</asp:Content>
