document.addEventListener("DOMContentLoaded", function () {
    const searchInput = document.getElementById("searchMember");
    const activeCheckbox = document.getElementById("activeCheckbox");
    const inactiveCheckbox = document.getElementById("inactiveCheckbox");
    const membershipTypeSelect = document.getElementById("membershipType");
    const tableRows = document.querySelectorAll("#membersTable tbody tr");

    function filterTable() {
        const query = searchInput.value.toLowerCase();
        const showActive = activeCheckbox.checked;
        const showInactive = inactiveCheckbox.checked;
        const selectedMembershipType = membershipTypeSelect.value;

        tableRows.forEach(row => {
            const nameCell = row.children[1].textContent.toLowerCase(); 
            const activeStatus = row.getAttribute("data-active-status") === "true"; 
            const matchesQuery = nameCell.includes(query);

            const matchesActive =
                (showActive && activeStatus) ||
                (showInactive && !activeStatus) ||
                (!showActive && !showInactive);

            if (matchesQuery && matchesActive) {
                row.style.display = ""; 
            } else {
                row.style.display = "none"; 
            }
        });
    }

    searchInput.addEventListener("input", filterTable);
    activeCheckbox.addEventListener("change", filterTable);
    inactiveCheckbox.addEventListener("change", filterTable);
    membershipTypeSelect.addEventListener("change", filterTable);
});
