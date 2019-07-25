$(() => {
    $(".form-control").on('keyup', function () {
        ensureFormValidity();
    });

    $("#like-question").on('click', () => {
        const questionId = $("#like-question").data('question-id')
        $.post('/home/likequestion', { questionId }, () => {
            updateLikes();
        });
    });


    function ensureFormValidity() {
        const isValid = isFormValid();
        $("#submit-button").prop('disabled', !isValid);
    }

    function isFormValid() {
        const answer = $("#answer").val();
        return answer;
    }

    const updateLikes = () => {
        const questionId = $("#likes").data('question-id');
        $.get('/home/getlikes', { questionId }, result => {
            $("#likes").text(result.likes);
        });
    };

    setInterval(updateLikes, 1000);
});