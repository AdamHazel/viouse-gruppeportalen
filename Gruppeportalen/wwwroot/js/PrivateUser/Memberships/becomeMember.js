"use strict";

const beMemberModal = document.getElementById("becomeMemberModal");

beMemberModal.addEventListener("show.bs.modal", function (event) {
    const button = event.relatedTarget;
    const groupName = button.getAttribute("data-group-name");
    const groupId = button.getAttribute("data-group-id");
    const formContent = beMemberModal.querySelector("#form-content");

    const resultMessage = beMemberModal.querySelector("#result-message");
    if (resultMessage) {
        resultMessage.innerHTML = ""; 
    }
    
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
            formContent.innerHTML = data;
            
            const form = formContent.querySelector("form");
            if (form) {
                attachFormHandler(form, beMemberModal);
            }
            
        })
        .catch(error => {
            console.error("Error loading modal content: ", error);
            formContent.innerHTML = '<p class ="text-danger">Det skjedde en feil</p>';
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
                    throw new Error("Det oppsto en feil.");
                }
                return response.json();
            })
            .then (result => {
                const modalBody = modElement.querySelector(".modal-body");
                const resultMessage = modElement.querySelector("#result-message");
                if (result.success) {
                    
                    if (resultMessage) {
                        resultMessage.innerHTML = `<span class="text-success">Membership added successfully!</span>`;
                    }
                    else
                    {
                        console.log("#result-message NOT found");
                    }
                    console.log("Successfully added form!");                    
                }
                else {
                    const errorMessage = result.message.toString();
                    console.log(errorMessage);
                    if (resultMessage) {
                        resultMessage.innerHTML = `<span class="text-danger">Error: ${errorMessage} </span>`;
                    }
                    else
                    {
                        console.log("#result-message NOT found");
                    }
                }
            })
            .catch(error => {
                console.error("Error:", error);
                const resultMessage = modElement.querySelector("#result-message");
                if (resultMessage) {
                    resultMessage.innerHTML = `<span class="text-danger">${error}</span>`;
                }
            });
    });
}

 
