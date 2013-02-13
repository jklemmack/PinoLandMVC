<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<PinoLandMVC4.Controllers.TileController.PinolandBounds>" %>

<!DOCTYPE html>
<html>
<head>
    <title>Dynamic tile layers in C# with Entity Framework 5</title>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <link href="../../Content/leaflet.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/leaflet.js" type="text/javascript"></script>
</head>
<body>
    <div id="map" style="width: 600px; height: 600px">
    </div>
    <script>

        var map = new L.Map('map',
            {
                center: new L.LatLng(0, 0),
                maxBounds: new L.LatLngBounds([<%: Model.top %>, <%: Model.left %>], [<%: Model.bottom %>, <%: Model.right %>])
            });

        var osmUrl = 'http://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png';
        var osmAttrib = 'Map data © openstreetmap contributors';
        var osm = new L.TileLayer(osmUrl, { attribution: osmAttrib });


        var dynamicTileUrl = '/Tile/{z}/{x}/{y}';
        dynamicTile = new L.TileLayer(dynamicTileUrl);

        map.setView(new L.LatLng(0.0, 0.0), 2).addLayer(dynamicTile);

        L.control.scale().addTo(map);
    </script>
</body>
</html>
