
$(function() {
    $('#lookup-form').on('submit', lookupFormSubmit)
})

function lookupFormSubmit(e) {
    e.preventDefault()
    let $placeVal = $('.lf-place').val(),
        $serviceVal = $('.lf-service').val(),
        $keywordVal = $('.lf-keyword').val(),
        lookupObj = { Place: $placeVal, Service: $serviceVal, Keyword: $keywordVal }

    // do some validations here...

    $.post(
        '/map/lookup',
        lookupObj
    ).then((data) => {
        var map = new Map(data.Coords, lookupObj, data.results)
        map.init()
    }).fail((xhr, status, message) => {
        console.log('there was an error!', status, message)
    })
}
