$(function () {
    // Function to load modal content.
    function loadModal(url) {
        $.get(url).done(function (data) {
            $('#modalOpen .modal-content').html(data);
            $('#modalOpen').modal('show');
        });
    }

    //// Add Student
    //$('#btnAddStudent').click(function () {
    //    loadModal('@Url.Action("Create", "Students")');
    //});

    //// Add Address
    //$('#btnAddAddress').click(function () {
    //    loadModal('@Url.Action("Create", "Addresses", new { studentId = studentId })');
    //});

    // modal
    $('.btn-general').click(function () {
        loadModal($(this).data('url'));
    });

    // Handle form submissions in modal via AJAX.
    $('#modalOpen').on('submit', 'form', function (event) {
        event.preventDefault();
        var form = $(this);
        var formData = new FormData(this);
        $.ajax({
            url: form.attr('action'),
            type: form.attr('method'),
            data: formData,
            contentType: false,
            processData: false,
            success: function (response) {
                if (response.isValid) {
                    location.reload();
                }
                else {
                    $('#modalOpen .modal-content').html(response);
                }
            }
        });
    });
});