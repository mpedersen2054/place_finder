
const TMPL = {
    markerPopup: function(place) {
        return `
        <div class="popup" data-placeid="${place.place_id}">
            <b class="p-title">${place.name}</b>
            <div class="p-desc">desc goes here</div>
            <a href="#" class="p-viewmore">More...</a>
        </div>
        `
    },

    placeDetails: function(place, placeExists) {
        console.log(place)
        // handle adding multiple types
        let types = ''
        for (var type of place.types) {
            types += `<span class="badge badge-primary type">${type}</span>` 
        }
        // handle adding the array of hours
        let hours = ''
        // check weather place has opening_hours, if its 24/7 it wont
        if (place.opening_hours) {
            // put all hours into single string
            for (var hour of place.opening_hours.weekday_text) {
                hours += `<li class="ic-day">${hour}</li>`
            }
        } else {
            // place is open 24/7
            hours += `<div>No hours specified...</div>`
        }

        // show 'Add Place' btn if place isnt already added
        // otherwise show a disabled button
        let btn = ''
        if (placeExists) {
            btn += `<button data-placeid=${place.place_id} class="btn btn-primary" disabled>Already added</button>`
        } else {
            btn += `<button data-placeid=${place.place_id} class="btn btn-primary add-place-btn">Add Place</button>`
        }
        
        return `
        <div class="row ic-r-1">
            <div class="col-md-12">
                <h5 class="ic-name">${place.name}</h5>
            </div>
        </div>
        <div class="row ic-r-2">
            <div class="col-md-12 ic-addr">
                <div>${place.formatted_address}</div>
            </div>
            <div class="col-md-12 ic-phone">
                <div>Phone : ${place.formatted_phone_number}</div>
            </div>
            <div class="col-md-12 ic-rating">
            <div>Rating : ${place.rating}</div>
        </div>
        </div>
        <hr>
        <div class="row ic-r-3">
            <div class="col-md-12">
                <div class="badges types">
                    ${types}
                </div>
            </div>
        </div>
        <hr>
        <div class="row ic-r-4">
            <div class="col-md-12">
                <b>Hours</b>
                <ul class="ic-hours">
                    ${hours}
                </ul>
            </div>
        </div>
        <hr>
        <div class="row ic-r-5">
            <div class="col-md-12 btn-col">
                ${btn}
            </div>
        </div>
        `
    },

    reviewTemplate: function(review) {
        return `
        <li class="review">
            <span class="r-name">${review.name}</span>
            <span class="r-date">${review.created_at}</span>
            <div class="r-content">${review.text}</div>
        </li>
        `
    }
}