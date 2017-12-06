
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

    placeDetails: function(place) {
        console.log(place)
        // handle adding multiple types
        let types = ''
        for (var type of place.types) {
            types += `<span class="badge badge-primary type">${type}</span>` 
        }
        // handle adding the array of hours
        let hours = ''
        let isOpen
        // check weather place has opening_hours, if its 24/7 it wont
        if (place.opening_hours ) {
            // put all hours into single string
            for (var hour of place.opening_hours.weekday_text) {
                hours += `<li class="ic-day">${hour}</li>`
            }
            // handle weather place is open / not open
            if (place.opening_hours.open_now) {
                isOpen = `<div class="ic-open-status open">Is currently open.</div>`
            } else {
                isOpen = `<div class="ic-open-status closed">Is currently closed.</div>`
            }
        } else {
            // place is open 24/7
            hours += `<div>No hours specified...</div>`
            isOpen = ''
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
                ${isOpen}
            </div>
        </div>
        <hr>
        <div class="row ic-r-5">
            <div class="col-md-12">
                <button data-placeid=${place.place_id} class="btn btn-primary add-place-btn">Add Place</button>
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