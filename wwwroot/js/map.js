const LEAFLET_KEY = "pk.eyJ1IjoicGF0bWVkZXJzZW4iLCJhIjoiOTRhZTRjOTdhZjA0NjI3YjIxZjBiNzEwNTk2OTY2OTEifQ.9hV_lLr_olzMPH0A7ON9SQ"

const Map = (function(coords, lookupObj, places) {
    this.lat = coords[0]
    this.lng = coords[1]
    this.lookupObj = lookupObj
    this.places = places.length > 0 ? places : []
    this.zoom = 17
    this.activeMap = null

    this.$mapContainer = $('.map-container')
    this.$infoCol = $('.info-col')

    this.init = function() {
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
        let self = this
        this.activeMap.on('moveend', function() {
            self.requestMorePlace(this.getCenter())
        })

        this.$mapContainer.on('click', '.p-viewmore', this.showPlaceDetails)
        this.$infoCol.on('click', '.add-place-btn', this.addPlace)
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
        let self = this
        $.each(placesArr, (i, place) => {
            let lat = place.geometry.location.lat,
                lng = place.geometry.location.lng,
                popupElem = TMPL.markerPopup(place)

            let marker = L.marker([lat, lng]).addTo(self.activeMap)
            marker.bindPopup(popupElem)
        })
    }

    this.showPlaceDetails = function(e) {
        e.preventDefault()
        let $this = $(this),
            $infoCol = $('.info-col'),
            $popup = $this.parent('.popup'),
            placeId = $popup.data('placeid')

        // show spinner while waiting for response
        $infoCol.html('<div class="spinner"><i class="fa fa-circle-o-notch fa-spin fa-3x fa-fw"></i></div>')

        // request the specific places details using the place_id
        $.get(`/place/${placeId}`)
            .then(data => {
                // check if the place is already added for the user
                // const placeExists = false
                const placeExists = data.userPlaceIds.some(p => p == data.place.place_id)
                let template = TMPL.placeDetails(data.place, placeExists)
                $infoCol.html(template)
            })
            .fail((xhr, status, err) => {
                console.log('there was an err getting place details!', err)
            })
    }

    this.addPlace = function(e) {
        e.preventDefault()
        let $this = $(this),
            placeId = $this.data('placeid')

        $.post(`/place/${placeId}/add`, { PlaceId: placeId })
            .done(data => {
                // place the 'Add place' btn with 'Already added' btn
                $('.ic-r-5 .btn-col').html(`<button class="btn btn-primary" disabled>Already added</button>`)
            })
            .fail((xhr, status, err) => {
                console.log('there was an err adding new place!', err)
            })
    }
})