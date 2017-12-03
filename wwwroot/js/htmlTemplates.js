
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
        return `
        <div class="row ic-r-1">
            <div class="col-md-12">
                <h5 class="ic-name">${place.name}</h5>
            </div>
        </div>
        <div class="row ic-r-2">
            <div class="col-md-12 ic-addr">
                <div>42/48 The Promenade, Sydney NSW 2000, Australia</div>
            </div>
            <div class="col-md-12 ic-phone">
                <div>Phone : 224 563 7611</div>
            </div>
        </div>
        <hr>
        <div class="row ic-r-3">
            <div class="col-md-12">
                <div class="badges">
                    <span class="badge badge-primary">restaurant</span>
                    <span class="badge badge-primary">food</span>
                    <span class="badge badge-primary">point_of_interest</span>
                    <span class="badge badge-primary">establishment</span>
                </div>
            </div>
        </div>
        <hr>
        <div class="row ic-r-4">
            <div class="col-md-12">
                <b>Hours</b>
                <ul class="ic-hours">
                    <li class="ic-day">Monday: 11:30 AM – 10:00 PM</li>
                    <li class="ic-day">Tuesday: 11:30 AM – 10:00 PM</li>
                    <li class="ic-day">Wednesday: 11:30 AM – 10:00 PM</li>
                    <li class="ic-day">Thursday: 11:30 AM – 10:00 PM</li>
                    <li class="ic-day">Friday: 11:30 AM – 12:00 AM</li>
                    <li class="ic-day">Saturday: 12:00 AM – 11:00 PM</li>
                    <li class="ic-day">Sunday: 9:00 AM – 10:00 PM</li>
                </ul>
                <div class="ic-open-status open">Is currently open</div>
            </div>
        </div>
        <hr>
        <div class="row ic-r-5">
            <div class="col-md-12">
                <button class="btn btn-primary" outline>Add Place</button>
            </div>
        </div>
        `
    }
}