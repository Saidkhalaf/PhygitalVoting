$(document).ready(function() {
    
    const subthemeId = sessionStorage.getItem("subthemeId");
    
    console.log('Retrieved subthemeId:', subthemeId);
    
    $('form').on('submit', function(event) {
        event.preventDefault(); 

        const opinionText = $('#opinion').val().trim();
        
        if (opinionText === '') {
            $('#opinion-alert').show()
            return;
        } else {
            $('#opinion-alert').hide();
        }

        console.log($('#subthemeId').val());

        $.ajax({
            url: '/Question/SubmitOpinion',
            type: 'POST',
            data: {
                opinion: opinionText,
                subthemeId: subthemeId
            },
            success: function(response) {
                $('#success-message').show();
                console.log(response.opinion);
            }
        });
    });
});