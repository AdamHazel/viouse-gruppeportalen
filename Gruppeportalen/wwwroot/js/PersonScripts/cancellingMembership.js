"use strict";

const cancelModal = document.getElementById('cancelModal');
let formSuccessful = false;

cancelModal.addEventListener('show.bs.modal', function (event) {
    const button = event.relatedTarget;
    document.getElementById('cancelModalLabel').textContent = button.getAttribute('data-group-name');
    document.getElementById('membershipId').value = button.getAttribute('data-membership-id');
    const info = document.getElementById('cancelInfo');
    const status = button.getAttribute('data-status');
    const okButton = document.getElementById("cancelButton");
    const avbrytButton = document.getElementById("avbrytButton");
    const resultInfo = document.getElementById("informationText");
    console.log(typeof status);
    console.log(status);
    
    resultInfo.innerText = "";
    okButton.disabled = false;
    okButton.textContent ="Ok";
    avbrytButton.textContent = "Avbryt";
    
    if(status === "False")
    {
        info.innerText = "Ved å fornye medlemskapet, vil medlemskapet ikke være kansellert ved neste fornyelsesdato.";
    }
    else
    {
        info.innerText = "Ved å melde personen fra denne gruppen, vil medlemsskapet ikke være fornyet. De vil ha et aktiv medlemskap frem til fornyelsesdatoen.";
    }
});

cancelModal.addEventListener('hide.bs.modal', function () {
    if (formSuccessful) {
        window.location.href = "/PrivateUser/Persons/Index";
    }
});

document.getElementById("cancelMembershipForm").addEventListener("submit", function (event) {
    event.preventDefault();
    
    const resultInfo = document.getElementById("informationText");
    const okButton = document.getElementById("cancelButton");
    const avbrytButton = document.getElementById("avbrytButton");
    
    const form = event.target;
    const dataFromForm = new FormData(form);

    for (let pair of dataFromForm.entries()) {
        console.log(pair[0] + ': ' + pair[1]); // Logs field names and values
    }
    
    fetch("/PrivateUser/Persons/CancelMembership", {
        method: "POST",
        body: dataFromForm,
    })
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                resultInfo.textContent = data.message;
                resultInfo.className = "";
                resultInfo.className = "text-success";
                formSuccessful = true;
            }
            else
            {
                resultInfo.textContent = data.message;
                resultInfo.className = "";
                resultInfo.className = "text-danger";
            }
            okButton.disabled = true;
            avbrytButton.textContent = "Lukk";
        })
});
