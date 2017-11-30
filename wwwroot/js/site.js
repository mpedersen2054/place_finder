
const LEAFLET_KEY = "pk.eyJ1IjoicGF0bWVkZXJzZW4iLCJhIjoiOTRhZTRjOTdhZjA0NjI3YjIxZjBiNzEwNTk2OTY2OTEifQ.9hV_lLr_olzMPH0A7ON9SQ"

$(function() {
    var mymap = L.map('mapid').setView([51.505, -0.09], 13)
    L.tileLayer('https://api.tiles.mapbox.com/v4/{id}/{z}/{x}/{y}.png?access_token={accessToken}', {
        attribution: 'Map data &copy; <a href="http://openstreetmap.org">OpenStreetMap</a> contributors, <a href="http://creativecommons.org/licenses/by-sa/2.0/">CC-BY-SA</a>, Imagery © <a href="http://mapbox.com">Mapbox</a>',
        maxZoom: 18,
        id: 'mapbox.streets',
        accessToken: LEAFLET_KEY
    }).addTo(mymap);
})
