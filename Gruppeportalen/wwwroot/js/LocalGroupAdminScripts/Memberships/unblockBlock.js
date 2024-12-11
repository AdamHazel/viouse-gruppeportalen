//Block members
document.querySelector(".block-member-btn").addEventListener("click", function () {
    const selectedIds = Array.from(document.querySelectorAll(".select-member:checked"))
        .map(checkbox => checkbox.value);

    if (selectedIds.length === 0) {
        alert("Please select at least one member to block.");
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
            alert("An error occurred while blocking members.");
        });
});


// Unblock members
document.querySelector(".unblock-member-btn").addEventListener("click", function () {
    const selectedIds = Array.from(document.querySelectorAll(".select-member:checked"))
        .map(checkbox => checkbox.value);

    if (selectedIds.length === 0) {
        alert("Please select at least one member to unblock.");
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
            alert("An error occurred while unblocking members.");
        });
});