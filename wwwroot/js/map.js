const LEAFLET_KEY = "pk.eyJ1IjoicGF0bWVkZXJzZW4iLCJhIjoiOTRhZTRjOTdhZjA0NjI3YjIxZjBiNzEwNTk2OTY2OTEifQ.9hV_lLr_olzMPH0A7ON9SQ"

const Map = (function(coords) {
    this.lat = coords[0]
    this.lng = coords[1]
    this.zoom = 16
    this.activeMap = null

    this.$mapContainer = $('.map-container')

    this.init = function() {
        console.log('initing!')
        // resets the view for each new map init'ed
        this.$mapContainer.html('<div id="mapid"></div>')
        this.activeMap = L.map('mapid').setView([this.lat, this.lng], this.zoom)
        L.tileLayer('https://api.tiles.mapbox.com/v4/{id}/{z}/{x}/{y}.png?access_token={accessToken}', {
            attribution: 'Map data &copy; <a href="http://openstreetmap.org">OpenStreetMap</a> contributors, <a href="http://creativecommons.org/licenses/by-sa/2.0/">CC-BY-SA</a>, Imagery © <a href="http://mapbox.com">Mapbox</a>',
            maxZoom: 18,
            id: 'mapbox.streets',
            accessToken: LEAFLET_KEY
        }).addTo(this.activeMap);
    }

    this.renderPlaces = function(placesArr) {
        console.log('rendering places!')
        let self = this
        $.each(placesArr, (i, place) => {
            let lat = place.geometry.location.lat,
                lng = place.geometry.location.lng

            console.log('place lat/lng', lat, lng)
            let marker = L.marker([lat, lng]).addTo(self.activeMap)
        })
    }
})