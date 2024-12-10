"use strict";

// JavaScript for å sette riktig personId i delingsmodalen når Del-knappen trykkes
const shareModal = document.getElementById('shareModal');
shareModal.addEventListener('show.bs.modal', function (event) {
    const button = event.relatedTarget;
    document.getElementById('sharePersonId').value = button.getAttribute('data-person-id');
});

// Reset when share modal closes
shareModal.addEventListener("hidden.bs.modal", function (){

    const updateInfo = document.getElementById("update-info");
    const shareButton = document.getElementById("shareButton");
    const emailText = document.getElementById("desiredEmail");
    const cancelButton = document.getElementById("shareCancelButton");

    shareButton.disabled = false;
    updateInfo.textContent = "";
    updateInfo.className = "";
    emailText.value = "";
    cancelButton.textContent = "Avbryt";
});

// AJAX script for sharing
document.getElementById("sharePersonForm").addEventListener("submit", function (event) {
    event.preventDefault();

    const form = event.target;
    const dataFromForm = new FormData(form);
    for (let pair of dataFromForm.entries()) {
        console.log(pair[0] + ': ' + pair[1]); // Logs field names and values
    }
    const updateInfo = document.getElementById("update-info");
    updateInfo.textContent = "";

    const shareButton = document.getElementById("shareButton");
    const cancelButton = document.getElementById("shareCancelButton");


    fetch("/PrivateUser/Persons/SharePerson", {
        method: "POST",
        body: dataFromForm,
    })
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                updateInfo.textContent = "Personen ble delt!";
                updateInfo.className = "";
                updateInfo.className = "text-success";
                shareButton.disabled = true;
                cancelButton.textContent = "Lukk";
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
