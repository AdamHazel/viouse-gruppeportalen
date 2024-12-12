// Block members
document.querySelector(".block-member-btn").addEventListener("click", function () {
    const selectedIds = Array.from(document.querySelectorAll(".select-member:checked"))
        .map(checkbox => checkbox.value);

    if (selectedIds.length === 0) {
        alert("Ingen personer valgt.");
        return;
    }


    const confirmBlock = confirm("Er du sikker på at du ønsker å blokke de valgte medlemmene?");
    if (!confirmBlock) {
        return; 
    }

    fetch("/PrivateUser/MyLocalGroups/BlockMember", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(selectedIds)
    })
        .then(response => {
            if (!response.ok) {
                throw new Error(`Failed to block members: ${response.statusText}`);
            }
            return response.json();
        })
        .then(data => {
            location.reload();
        })
        .catch(error => {
            console.error("Error:", error);
            alert("En feil oppstod da man prøvde å blokke medlem.");
        });
});

// Unblock members
document.querySelector(".unblock-member-btn").addEventListener("click", function () {
    const selectedIds = Array.from(document.querySelectorAll(".select-member:checked"))
        .map(checkbox => checkbox.value);

    if (selectedIds.length === 0) {
        alert("Ingen personer valgt.");
        return;
    }

    fetch("/PrivateUser/MyLocalGroups/UnblockMember", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(selectedIds)
    })
        .then(response => {
            if (!response.ok) {
                throw new Error(`Failed to unblock members: ${response.statusText}`);
            }
            return response.json();
        })
        .then(data => {
            location.reload();
        })
        .catch(error => {
            console.error("Error:", error);
            alert("En feil oppstod under blokkering av medlem.");
        });
});
