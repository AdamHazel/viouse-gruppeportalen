async function searchLocalGroups() {
    const searchQuery = document.getElementById('search').value;
    const countyDropdown = document.getElementById('countyDropdown').value;
    const container = document.getElementById('localGroupsContainer');

    try {
        const response = await fetch(`/CentralOrganisation/Localgroup/SearchLocalGroups?query=${encodeURIComponent(searchQuery)}&county=${encodeURIComponent(countyDropdown)}`);
        if (!response.ok) {
            console.error(response.statusText);
            return;
        }

        const html = await response.text();
        container.innerHTML = html;
    } catch (error) {
        console.error( error);
    }
}

const searchInput = document.getElementById('search');
const countyDropdown = document.getElementById('countyDropdown');

if (searchInput) {
    searchInput.addEventListener('input', searchLocalGroups);
}

if (countyDropdown) {
    countyDropdown.addEventListener('change', searchLocalGroups);
}
