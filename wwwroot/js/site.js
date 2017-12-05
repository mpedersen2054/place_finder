
$(function() {
    $('#lookup-form').on('submit', lookupFormSubmit)
    $('.add-review-form').on('submit', addReviewSubmit)
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
    ).then(data => {
        // console.log('data from /map/lookup!!', data)
        console.log(data.results)
        var map = new Map(data.Coords, lookupObj, data.results)
        map.init()
    }).fail((xhr, status, message) => {
        console.log('there was an error!', xhr, status, message)
    })
}

function addReviewSubmit(e) {
    e.preventDefault()
    let $this = $(this),
        placeId = $this.data('placeid'),
        review = $this.find('.review-inp').val().trim()

    console.log('submitting review', placeId, review)
    $.post(
        '/users/add_review',
        { PlaceId: placeId, Review: review }
    ).then(data => {
        console.log('submitted!', data)
        if (data.success) {
            $('.reviews').append()
        }
    }).fail((xhr, status, message) => {
        console.log('there was an error!', xhr, status, message)
    })
}