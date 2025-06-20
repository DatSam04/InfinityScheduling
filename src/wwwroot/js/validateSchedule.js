const updateBtn = document.getElementById("updateButton");
function validateForm() {
    const nameInput = document.querySelector('[name="Schedule.Name"]');
    const nameError = document.getElementById("nameError");

    // Regex allows letters, spaces, dots, commas, and hyphens
    const regex = /^[a-zA-Z\s.,-]+$/;

    // Check if name and location are non-empty and valid
    const isNameValid = nameInput.value.trim() !== "" && regex.test(nameInput.value);

    // Show or hide error messages based on input validity
    nameError.style.display = isNameValid ? "none" : "block";
    
    // Enable the update button only if both fields are valid
    updateBtn.disabled = isNameValid;
}

// Wait until the DOM is fully loaded, then trigger initial validation
window.addEventListener('DOMContentLoaded', () => {
    validateForm();
});