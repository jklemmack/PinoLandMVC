<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<PinoLandMVC4.Controllers.TileController.PinolandBounds>" %>

<!DOCTYPE html>
<html>
<head>
    <title>Dynamic tile layers in C# with Entity Framework 5</title>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <script src="../../Scripts/jquery-1.9.1.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-ui-1.10.0.min.js" type="text/javascript"></script>
    <script src="../../Scripts/leaflet.js" type="text/javascript"></script>
    <script src="../../Scripts/leaflet.markercluster.js" type="text/javascript"></script>
    <link href="../../Content/leaflet.css" rel="stylesheet" type="text/css" />
    <link href="../../Content/MarkerCluster.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript">
        $(function () {

        });
    </script>
</head>
<link href="../../Content/MarkerCluster.Default.css" rel="stylesheet" type="text/css" />
<body>
    <img id="marker" src="../../Scripts/images/marker-icon.png" style="z-index: 999" />
    <div id="map" style="width: 600px; height: 600px">
    </div>
    <script type="text/javascript" language="javascript">        
        var map, markers;
        map = new L.Map('map',
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
        markers = new L.MarkerClusterGroup();

        var skip = 0;
        var take = 10000;

        var dataNomNom = function(data) {
                
            for (var i = 0; i < data.length; i++)
            {
			    var a = data[i];
			    var title = a.Id;
			    var marker = new L.Marker(new L.LatLng(a.Lat, a.Lng), { title: title });
			    marker.bindPopup(title);
			    markers.addLayer(marker);
		    }

		    map.addLayer(markers);
            if (data.length > 0)
            {
                skip += take;
                getData(skip, take);
            } 
            //else alert('done!');
        };
        
        var getData = function(s, t) {
            $.get("/Tile/Points/" + location.hash.substr(1) + '/' + s + '/' + t, dataNomNom);
        };

        $("#marker").draggable();
        $("map").droppable({
            drop: function (event, ui) {
                alert('dropping!');
            }
        });

        //getData(skip, take);

    </script>
</body>
</html>
