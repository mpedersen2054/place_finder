
const TMPL = {
    markerPopup: function(place) {
        return `
        <div class="popup">
            <b class="p-title">${place.name}</b>
            <div class="p-desc">desc goes here</div>
            <a href="#" class="p-viewmore">More...</a>
        </div>
        `
    }
}