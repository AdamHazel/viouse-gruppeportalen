document.addEventListener("DOMContentLoaded", function () {
    const editButtons = document.querySelectorAll("[data-bs-target='#editMembershipModal']");

    editButtons.forEach(button => {
        button.addEventListener("click", function () {
            const membershipId = this.getAttribute("data-membership-id");
            const membershipName = this.getAttribute("data-membership-name");
            const membershipPrice = this.getAttribute("data-membership-price");
            const membershipMonth = this.getAttribute("data-membership-month");
            const membershipDay = this.getAttribute("data-membership-day");
            const localGroupId = this.getAttribute("data-localgroup-id"); 

            document.getElementById("editMembershipId").value = membershipId;
            document.getElementById("editMembershipName").value = membershipName;
            document.getElementById("editMembershipPrice").value = membershipPrice;
            document.getElementById("editMembershipMonth").value = membershipMonth;
            document.getElementById("editMembershipLocalGroupId").value = localGroupId; 

           
            const dayDropdown = document.getElementById("editMembershipDay");
            dayDropdown.innerHTML = ""; 
            for (let i = 1; i <= 31; i++) {
                const option = document.createElement("option");
                option.value = i;
                option.textContent = i;
                if (i == membershipDay) option.selected = true;
                dayDropdown.appendChild(option);
            }
        });
    });
});
