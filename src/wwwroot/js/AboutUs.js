// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.
const faders = document.querySelectorAll('.fade-in'); // Select all elements with the class 'fade-in'
const teamMembers = document.querySelectorAll('.team-member'); // Select all elements with the class 'team-member'
const modal = document.getElementById('contactModal'); // Select the modal element
const modalName = document.getElementById('modalName'); // Select the modal name element
const modalRole = document.getElementById('modalRole'); // Select the modal role element
const modalMessage = document.getElementById('modalMessage'); // Select the modal message element
const modalEmail = document.getElementById('modalEmail'); // Select the modal email element

// Add event listener to the close button
const appearOptions = {
    threshold: 0.2
};

// Create an IntersectionObserver to handle the fade-in effect
const appearOnScroll = new IntersectionObserver((entries, observer) => { // Callback function for the observer
    entries.forEach(entry => { // Loop through each entry
        if (entry.isIntersecting) { // Check if the entry is in the viewport
            entry.target.classList.add('visible'); // Add the 'visible' class to the entry
            observer.unobserve(entry.target); // Stop observing the entry
        }
    });
}, appearOptions); // Create an IntersectionObserver instance

faders.forEach(fader => appearOnScroll.observe(fader)); // Observe each fader element

teamMembers.forEach(member => { // Loop through each team member
    member.addEventListener('click', () => { // Add click event listener to each member
        modalName.textContent = member.dataset.name; // Set the modal name
        modalRole.textContent = member.dataset.role; // Set the modal role
        modalMessage.textContent = member.dataset.message; // Set the modal message
        modalEmail.textContent = member.dataset.email; // Set the modal email
        modalEmail.href = `mailto:${member.dataset.email}`; // Set the modal email link
        modal.style.display = 'flex'; // Show the modal
    });
});

// Add event listener to the close button
function closeModal() {
    modal.style.display = 'none';
}

// Add event listener to the close button
window.onclick = function (event) {
    if (event.target === modal) {
        closeModal();
    }
};
