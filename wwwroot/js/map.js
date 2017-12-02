const LEAFLET_KEY = "pk.eyJ1IjoicGF0bWVkZXJzZW4iLCJhIjoiOTRhZTRjOTdhZjA0NjI3YjIxZjBiNzEwNTk2OTY2OTEifQ.9hV_lLr_olzMPH0A7ON9SQ"

const Map = (function(coords, lookupObj, places) {
    this.lat = coords[0]
    this.lng = coords[1]
    this.lookupObj = lookupObj
    this.places = places.length > 0 ? places : []
    this.zoom = 17
    this.activeMap = null

    this.$mapContainer = $('.map-container')

    this.init = function() {
        console.log('initing!')
        console.log(coords, lookupObj)
        // resets the view for each new map init'ed
        this.$mapContainer.html('<div id="mapid"></div>')
        this.activeMap = L.map('mapid').setView([this.lat, this.lng], this.zoom)
        L.tileLayer('https://api.tiles.mapbox.com/v4/{id}/{z}/{x}/{y}.png?access_token={accessToken}', {
            attribution: 'Map data &copy; <a href="http://openstreetmap.org">OpenStreetMap</a> contributors, <a href="http://creativecommons.org/licenses/by-sa/2.0/">CC-BY-SA</a>, Imagery © <a href="http://mapbox.com">Mapbox</a>',
            maxZoom: 18,
            id: 'mapbox.streets',
            accessToken: LEAFLET_KEY
        }).addTo(this.activeMap);

        this.attachHandlers()
        this.renderPlaces(places)
    }

    this.attachHandlers = function() {
        console.log('attaching handlers')
        let self = this
        this.activeMap.on('moveend', function() {
            self.requestMorePlace(this.getCenter())
        })
    }

    this.requestMorePlace = function(center) {
        let self = this
        $.get(`/map/get_new_places/${center.lat}/${center.lng}/${lookupObj.Place}/${lookupObj.Service}/${lookupObj.Keyword}`)
        .done(data => {
            let toRenderArr = []
            $.each(data.results, (i, place) => {
                // check exists this.places and see if indv place is already in there
                const exists = self.places.some(p => p.place_id == place.place_id)
                // if the place doesnt exist, add it to existing
                // places & arr of places to be rendered
                if (!exists) {
                    toRenderArr.push(place)
                    self.places.push(place)
                }
            })
            // render only the new places
            self.renderPlaces(toRenderArr)
        })
        .fail((xhr, status, err) => {
            console.log('there was an error!', err)
        })
    }

    this.renderPlaces = function(placesArr) {
        console.log(`rendering ${placesArr.length} places!`)
        let self = this
        $.each(placesArr, (i, place) => {
            let lat = place.geometry.location.lat,
                lng = place.geometry.location.lng,
                popupElem = TMPL.markerPopup(place)

            let marker = L.marker([lat, lng]).addTo(self.activeMap)
            marker.bindPopup(popupElem)
        })
    }
})