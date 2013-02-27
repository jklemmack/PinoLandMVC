<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<PinoLandMVC4.Models.StudentModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Index
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Index</h2>
    <div id="tabsMain">
        <ul>
            <li><a href="#tabHome">Home</a></li>
            <li><a href="#tabMap">Map</a></li>
            <li><a href="#tabActions">Actions</a></li>
            <li><a href="#tabReports">Reports</a></li>
        </ul>
        <div id="tabHome" index="1">
            <% Html.RenderPartial("Parts/News", Model.News); %>
        </div>
        <div id="tabMap" index="2">
            <div style="display: block; float: left;">
                <% Html.RenderPartial("Parts/SmallMap", Model.MapBounds); %>
            </div>
            <div style="display: block; float: right;">
                <% Html.RenderPartial("Parts/Actions", Model.FoodActions); %>
            </div>
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
            $("#tabsMain").tabs().addClass("ui-helper-clearfix");
            //$("#tabsMain li").removeClass("ui-corner-top").addClass("ui-corner-left");

            $("#CreateNewAction").click(function () {
                // add a new row to the actions grid (at top ideally)

                // add a default marker to the map
                $("#NewRestaurant").css("display", "");

                var marker = L.marker(map.getCenter(),
                        {
                            draggable: true,
                            riseOnHover: true
                        }
                    );
                marker.on('dragend', MarkerDrag, this);
                marker.on('drag', MarkerDrag, this);
                marker.addTo(map);

                SetLatLng(map.getCenter().lat, map.getCenter().lng);


                //Hide the "new action"
                $("#CreateNewAction").css("visibility", "hidden");
            });

        });

        function MarkerDrag(a) {
            var lat = a.target.getLatLng().lat;
            var lng = a.target.getLatLng().lng;
            SetLatLng(lat, lng);
        }

        function SetLatLng(lat, lng) {
            $("#latlng").html(lat.toFixed(3) + " : " + lng.toFixed(3));
        }

    </script>
</asp:Content>
