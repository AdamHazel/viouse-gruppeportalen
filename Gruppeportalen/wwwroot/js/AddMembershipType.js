$(document).ready(function () {
    $('#addMembershipModal').on('show.bs.modal', function (event) {
        var button = $(event.relatedTarget); 
        var localGroupId = button.data('localgroup-id'); 
        
        $('#LocalGroupId').val(localGroupId);
    });

   
    $(document).off('click', '#saveMembershipButton').on('click', '#saveMembershipButton', function (e) {
        e.preventDefault();

        var saveButton = $(this);
        saveButton.prop('disabled', true);

        var form = $($(this).data('form')); 
        var url = form.data('url');
        var formData = form.serialize();

        $.ajax({
            url: url,
            type: 'POST',
            data: formData,
            beforeSend: function () {
                saveButton.html('<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span> Lagrer...');
            },
            success: function (response) {
                saveButton.html('Lagre'); 
                saveButton.prop('disabled', false); 

                if (response.success) {
                    alert('Medlemskap ble lagt til!');
                    $('#addMembershipModal').modal('hide');
                    form[0].reset();
                    location.reload();
                    
                } else {
                    alert(response.message || 'Noe gikk galt.');
                }
            },
            error: function (xhr, status, error) {
                saveButton.html('Lagre'); 
                saveButton.prop('disabled', false); 

                var errorMessage = xhr.responseJSON && xhr.responseJSON.message
                    ? xhr.responseJSON.message
                    : 'Noe gikk galt. Vennligst prøv igjen.';

                console.error(errorMessage);
                alert(errorMessage);
            }
        });
    });
    
    $('#addMembershipModal').on('hidden.bs.modal', function () {
        $(this).find('form')[0].reset();
    });
});
