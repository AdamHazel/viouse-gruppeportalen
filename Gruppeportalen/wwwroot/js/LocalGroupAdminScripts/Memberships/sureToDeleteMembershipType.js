document.addEventListener("DOMContentLoaded", function () {
    const deleteButtons = document.querySelectorAll("[data-bs-target='#sureToDeleteModal']");

    deleteButtons.forEach(button => {
        button.addEventListener("click", function () {
            const membershipId = this.getAttribute("data-membership-id");
            document.getElementById("deleteMembershipId").value = membershipId; // Sett ID-en i skjemaet
        });
    });
});
