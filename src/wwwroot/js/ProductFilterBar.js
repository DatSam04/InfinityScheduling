// Run the code after the full DOM has loaded
document.addEventListener("DOMContentLoaded", function () {

    // Get reference to the input box used for filtering
    const input = document.getElementById("productFilterBar");

    // Get all product cards that will be filtered
    const productCards = document.querySelectorAll(".product-card-container");

    // Add event listener for when the user types in the search box
    input.addEventListener("keyup", function () {
        // Get the search text, convert it to lowercase for case-insensitive comparison
        const filter = input.value.toLowerCase();

        // Flag to check if any card is visible after filtering
        let anyVisible = false;

        // Loop through all product cards
        productCards.forEach(card => {
            // Extract the title and description text from each card
            const title = card.querySelector(".card-title")?.textContent.toLowerCase() || "";
            const desc = card.querySelector(".card-text")?.textContent.toLowerCase() || "";

            // Check if either title or description contains the search text
            const match = title.includes(filter) || desc.includes(filter);

            // Show or hide the card based on match result
            card.style.display = match ? "block" : "none";

            // If there's at least one match, update the flag
            if (match) {
                anyVisible = true;
            }
        });

        // Handle the 'no results found' message
        const noResults = document.getElementById("noResultsMessage");

        // If no cards matched, show the message; otherwise, hide it
        if (noResults) {
            noResults.style.display = anyVisible ? "none" : "block";
        }
    });
});

