
$(function() {
    $('#lookup-form').on('submit', lookupFormSubmit)
})

function lookupFormSubmit(e) {
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
        
        var map = new Map(data.Coords)
        map.init()
        map.renderPlaces(data.results)

    }).fail((xhr, status, message) => {
        console.log('there was an error!', status, message)
    })
}
