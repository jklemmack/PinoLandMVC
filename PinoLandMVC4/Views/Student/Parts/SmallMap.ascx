<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<PinoLandMVC4.Models.PinolandBounds>" %>
<div id="map" style="width: 600px; height: 600px">
</div>
<script language="javascript" type="text/javascript">
    var map;
    map = new L.Map('map',
        {
            center: new L.LatLng(0, 0),
            maxBounds: new L.LatLngBounds([<%: Model.top %>, <%: Model.left %>], [<%: Model.bottom %>, <%: Model.right %>])
        });

    //var osmUrl = 'http://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png';
    //var osmAttrib = 'Map data © openstreetmap contributors';
    //var osm = new L.TileLayer(osmUrl, { attribution: osmAttrib });


    var dynamicTileUrl = '/Tile/<%: ViewBag.EconomyId%>/{z}/{x}/{y}';
    dynamicTile = new L.TileLayer(dynamicTileUrl);

    map.setView(new L.LatLng(0.0, 0.0), 2).addLayer(dynamicTile);

    L.control.scale().addTo(map);
</script>

