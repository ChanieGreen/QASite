$(() => {
    $(".form-control").on('keyup', function () {
        ensureFormValidity();
    });

    function ensureFormValidity() {
        const isValid = isFormValid();
        $("#submit-button").prop('disabled', !isValid);
    }

    function isFormValid() {
        const title = $("#title").val();
        const text = $("#text").val();
        const tags = $("#tags").val();
        return title && text && tags !== "-1";
    }
});