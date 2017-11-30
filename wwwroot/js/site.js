
const LEAFLET_KEY = "pk.eyJ1IjoicGF0bWVkZXJzZW4iLCJhIjoiOTRhZTRjOTdhZjA0NjI3YjIxZjBiNzEwNTk2OTY2OTEifQ.9hV_lLr_olzMPH0A7ON9SQ"

$(function() {
    var mymap = L.map('mapid').setView([27.950575, -82.4571776], 12)
    L.tileLayer('https://api.tiles.mapbox.com/v4/{id}/{z}/{x}/{y}.png?access_token={accessToken}', {
        attribution: 'Map data &copy; <a href="http://openstreetmap.org">OpenStreetMap</a> contributors, <a href="http://creativecommons.org/licenses/by-sa/2.0/">CC-BY-SA</a>, Imagery © <a href="http://mapbox.com">Mapbox</a>',
        maxZoom: 18,
        id: 'mapbox.streets',
        accessToken: LEAFLET_KEY
    }).addTo(mymap);

    $('#lookup-form').on('submit', (e) => {
        e.preventDefault()
        let $placeVal = $('.lf-place').val(),
            $serviceVal = $('.lf-service').val(),
            $keywordVal = $('.lf-keyword').val()

        console.log('Looking up...')
        // do some validations here...
        $.post(
            '/map/lookup',
            { Place: $placeVal, Service: $serviceVal, Keyword: $keywordVal }
        ).then((data) => {
            console.log('posted to /map/lookup!', data)
        }).fail((xhr, status, message) => {
            console.log('there was an error!', status, message)
        })
    })
})
