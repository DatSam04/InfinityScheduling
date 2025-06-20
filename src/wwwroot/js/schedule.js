document.addEventListener("DOMContentLoaded", () => {

    // Add confirmation popup when user clicks delete icon
    const deleteLinks = document.querySelectorAll("a[title='Delete this destination']"); //Confirmation for Deleting 

    deleteLinks.forEach(link => { //Looping through location 
        link.addEventListener("click", function (e) {
            const confirmed = confirm("Are you sure you want to delete this destination?");  
            if (!confirmed) e.preventDefault();
        });
    });

    

});