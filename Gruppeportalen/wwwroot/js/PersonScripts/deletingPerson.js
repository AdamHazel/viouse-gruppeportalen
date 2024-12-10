"use strict";

// JavaScript for å sette riktig personId i delingsmodalen når fjern-knappen trykkes
const deleteModal = document.getElementById('deleteModal');
deleteModal.addEventListener('show.bs.modal', function (event) {
    const button = event.relatedTarget;
    // document.querySelector('#deleteModal h3').innerText = button.getAttribute('data-person-id');
    document.getElementById('deletePersonId').value = button.getAttribute('data-person-id');
});