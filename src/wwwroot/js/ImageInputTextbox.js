//Render the existing input data or handle the add remove image url input textbox
document.addEventListener('DOMContentLoaded', function () {
    const maxImages = 3; //specify the max image url user can add to a specific location
    const imageInputsContainer = document.getElementById('image-inputs');
    const addImageBtn = document.getElementById('add-image-btn');

    if (!imageInputsContainer || !addImageBtn) return;

    /// Disable add button when # of input text box reach the max limit
    function updateAddButtonState() {
        const inputCount = imageInputsContainer.querySelectorAll('input[name^="Schedule.Image"]').length;
        addImageBtn.disabled = inputCount >= maxImages;
    }

    ///Handle add new input textbox
    addImageBtn.addEventListener('click', () => {
        const inputCount = imageInputsContainer.querySelectorAll('input[name="Schedule.Image"]').length;

        if (inputCount >= maxImages) return;

        const newInputGroup = document.createElement('div');
        newInputGroup.className = 'input-group mb-2';
        newInputGroup.innerHTML = `
            <input name="Schedule.Image" class="form-control" placeholder="Enter location image URL" />
            <button type="button" class="btn btn-outline-danger remove-image-btn">−</button>
        `;
        imageInputsContainer.appendChild(newInputGroup);
        updateAddButtonState();
    })

    ///Handle remove input textbox
    imageInputsContainer.addEventListener('click', (e) => {
        if (e.target.closest('.remove-image-btn')) {
            const group = e.target.closest('.input-group');
            if (group && imageInputsContainer.children.length > 1) {
                group.remove();
                updateAddButtonState();
            }
        }
    });

    updateAddButtonState();
});