function exportMemberships(element) {
    const groupId = element.getAttribute('data-group-id');
    console.log("Group ID hentet:", groupId);

    fetch(`/PrivateUser/MyLocalGroups/CheckIfMembershipListIsEmpty?groupId=${groupId}`)
        .then(response => {
            console.log("Responsstatus:", response.status);
            if (!response.ok) {
                throw new Error(`HTTP-feil: ${response.status}`);
            }
            return response.json();
        })
        .then(data => {
            console.log("Responsdata:", data);
            if (data.isEmpty) {
                console.log("Listen er tom. Venter på brukerens bekreftelse...");
                if (confirm("Listen er tom. Ønsker du likevel å generere listen?")) {
                    console.log("Brukeren bekreftet. Genererer tom CSV.");
                    window.location.href = `/PrivateUser/MyLocalGroups/ExportActiveMembershipsToCsv?groupId=${groupId}`;
                } else {
                    console.log("Brukeren avbrøt generering av tom CSV.");
                }
            } else {
                console.log("Listen er ikke tom. Genererer CSV...");
                window.location.href = `/PrivateUser/MyLocalGroups/ExportActiveMembershipsToCsv?groupId=${groupId}`;
            }
        })
        .catch(error => {
            console.error("Feil under sjekken av listen:", error);
            alert("Det oppstod en feil. Vennligst prøv igjen senere.");
        });
}
