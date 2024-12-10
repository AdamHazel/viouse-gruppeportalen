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
            
            const form = modalBody.querySelector("form");
            if (form) {
                attachFormHandler(form, beMemberModal);
            }
            
        })
        .catch(error => {
            console.error("Error loading modal content: ", error);
            modalBody.innerHTML = '<p class ="text-danger">Det skjedde en feil</p>';
        })
});

function attachFormHandler(form, modElement) {
    form.addEventListener("submit", function (e) {
        e.preventDefault();
        
        const actionUrl = form.getAttribute("action");
        const formData = new FormData(form);

        console.log("Form data:");
        for (const [key, value] of formData.entries()) {
            console.log(`${key}: ${value}`);
        }
        
        console.log(actionUrl.toString());
        
        fetch(actionUrl, {
            method: "POST",
            body: formData,
        })
            .then(response => {
                if (!response.ok) {
                    throw new Error("Failed to submit form!");
                }
                return response.json();
            })
            .then (result => {
                const modalBody = modElement.querySelector(".modal-body");
                if (result.success) {
                    console.log("Successfully added form!");
                    alert("Successfully added membership");
                    
                    const instance = bootstrap.Modal.getInstance(modElement);
                    if (instance) {
                        instance.hide();
                    }
                }
                else {
                    console.log(result.message.toString());
                    alert(result.message);
                }
            })
            .catch(error => {
                console.error("Error submitting form:", error);
                const modalBody = modElement.querySelector(".modal-body");
                modalBody.innerHTML = '<p class ="text-danger">Det skjedde en feil</p>';
            });
    });
}

 
