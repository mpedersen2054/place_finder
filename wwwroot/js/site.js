
$(function() {
    $('#lookup-form').on('submit', lookupFormSubmit)
    $('.add-review-form').on('submit', addReviewSubmit)
})

function lookupFormSubmit(e) {
    e.preventDefault()
    let $placeVal = $('.lf-place').val(),
        $serviceVal = $('.lf-service').val(),
        $keywordVal = $('.lf-keyword').val(),
        $inputErrors = $('.input-errors'),
        lookupObj = { Place: $placeVal, Service: $serviceVal, Keyword: $keywordVal },
        error = false

    // check for validity...
    if ($placeVal.length < 3 || !/^[a-zA-Z]+$/.test($placeVal)) error = true;
    if ($serviceVal.length < 3 || !/^[a-zA-Z]+$/.test($serviceVal)) error = true;
    if ($keywordVal.length < 3 || !/^[a-zA-Z]+$/.test($keywordVal)) error = true;

    if (error) {
        $inputErrors.html('* All inputs must be atleast 3 characters and only letters.')
        return 0
    } else {
        $inputErrors.html('')
    }

    $.post(
        '/map/lookup',
        lookupObj
    ).then(data => {
        // instantiate Map obj
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
        $reviewInp = $this.find('.review-inp'),
        review = $reviewInp.val().trim()

    $.post(
        '/users/add_review',
        { PlaceId: placeId, Review: review }
    ).then(data => {
        $reviewInp.val('')
        let review = TMPL.reviewTemplate(data)
        $this.siblings('.reviews').append(review)
    }).fail((xhr, status, message) => {
        console.log('there was an error!', xhr, status, message)
    })
}