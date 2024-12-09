"use strict";

// JavaScript for å sette riktig personId i overføringsmodalen når Overfør-knappen trykkes
const transferModal = document.getElementById('transferModal');
let transferSuccessful = false;

transferModal.addEventListener('show.bs.modal', function (event) {
    const button = event.relatedTarget;
    document.getElementById('transferPersonId').value = button.getAttribute('data-person-id');
});

// Reset when transfer modal closes
transferModal.addEventListener("hidden.bs.modal", function (){

    const updateInfo = document.getElementById("update-info-transfer");
    const transferButton = document.getElementById("transferButton");
    const emailText = document.getElementById("transferEmail");
    const cancelButton = document.getElementById("transferCancelButton");

    transferButton.disabled = false;
    updateInfo.textContent = "";
    updateInfo.className = "";
    emailText.value = "";
    cancelButton.textContent = "Avbryt";
    
    if (transferSuccessful)
    {
        window.location.href = "/PrivateUser/Persons/Index";
    }
});

// AJAX script for transferring
document.getElementById("transferPersonForm").addEventListener("submit", function (event) {
    event.preventDefault();

    const form = event.target;
    const dataFromForm = new FormData(form);
    for (let pair of dataFromForm.entries()) {
        console.log(pair[0] + ': ' + pair[1]); // Logs field names and values
    }
    const updateInfo = document.getElementById("update-info-transfer");
    updateInfo.textContent = "";

    const transferButton = document.getElementById("transferButton");
    const cancelButton = document.getElementById("transferCancelButton");


    fetch("/PrivateUser/Persons/TransferPerson", {
        method: "POST",
        body: dataFromForm,
    })
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                updateInfo.textContent = "Personen ble overført!";
                updateInfo.className = "";
                updateInfo.className = "text-success";
                transferButton.disabled = true;
                cancelButton.textContent = "Lukk";
                transferSuccessful = true;
            } else {
                updateInfo.textContent = data.errorMessage;
                updateInfo.className = "text-danger";
            }
        })
        .catch(error => {
            console.error('Error:', error);
            updateInfo.textContent = "En feil oppstod under delingen.";
            updateInfo.className = "text-danger";
        });
});