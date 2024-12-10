"use strict";

function MonthNumberToWords(number) {
    const months = [
        null, // Placeholder for index 0 (months start from 1)
        "januar", "februar", "mars", "april", "mai", "juni",
        "juli", "august", "september", "october", "november", "desember"
    ];
    return months[number] || "Invalid month";
}

document.addEventListener("DOMContentLoaded", function (event) {
    let items = document.querySelectorAll(".month");
    
    for (let item of items) {
        let number = parseInt(item.textContent);
        item.textContent = MonthNumberToWords(number);
    }
});