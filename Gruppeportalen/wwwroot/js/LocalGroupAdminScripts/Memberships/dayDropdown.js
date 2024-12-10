"use strict";


function populateDayDropdown(){
    const month = parseInt(document.getElementById("monthDropdown").value, 10)

    const dayResetDropdown = document.getElementById("dayResetDropdown");

    let daysInMonth;
    switch (month) {
        case 2:
            daysInMonth = 28;
            break;
        case 4: case 6: case 9: case 11:
            daysInMonth = 30;
            break;
        default:
            daysInMonth = 31;
            break;
    }

    dayResetDropdown.innerHTML = "";

    for (let i = 1; i <= daysInMonth; i++) {
        const option = document.createElement("option");
        option.value = i.toString();
        option.text = i.toString();
        dayResetDropdown.add(option);
    }
}

document.addEventListener("DOMContentLoaded", populateDayDropdown);
document.getElementById("monthDropdown").addEventListener("change", populateDayDropdown );