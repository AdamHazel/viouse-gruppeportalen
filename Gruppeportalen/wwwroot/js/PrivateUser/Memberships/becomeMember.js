"use strict";

const beMemberModal = document.getElementById("becomeMemberModal");

beMemberModal.addEventListener("show.bs.modal", function (event) {
    const button = event.relatedTarget;
    const groupName = button.getAttribute("data-group-name");
    const groupId = button.getAttribute("data-group-id");
    const modalBody = beMemberModal.querySelector(".modal-body");
    
    const url = button.getAttribute("data-url");
    console.log(url.toString());
    
    document.getElementById("becomeMemberTitle").textContent = groupName;
    
    fetch(url)
        .then(response => {
            if (!response.ok) {
                throw new Error(`Failed to load from server: ${response.statusText}.`);
            }
            return response.text();
        })
        .then(data => {
            modalBody.innerHTML = data;
        })
        .catch(error => {
            console.error("Error loading modal content: ", error);
            modalBody.innerHTML = '<p class ="text-danger">Det skjedde en feil</p>';
        })
});

 
