//This eventlistener handle the adding trip type and save it to hidden area
document.addEventListener('DOMContentLoaded', function () {
    const input = document.getElementById('triptype-input');
    const addBtn = document.getElementById('add-triptype-btn');
    const tagContainer = document.getElementById('triptype-tags');

    //Add trip type to hidden list and display to user
    addBtn.addEventListener('click', function () {
        const value = input.value.trim();
        if (!value) return;

        // Prevent duplicates
        const existing = Array.from(tagContainer.querySelectorAll('input[name="Schedule.Triptype"]'))
            .some(input => input.value.toLowerCase() === value.toLowerCase());
        if (existing) {
            input.value = '';
            return;
        }

        const tag = document.createElement('div');
        tag.className = 'tag-name';
        tag.dataset.value = value;
        tag.innerHTML = `
            ${value}
            <button type="button" class="remove-tag ms-2" aria-label="Remove">×</button>
            <input type="hidden" name="Schedule.Triptype" value="${value}" />
        `;
        tagContainer.appendChild(tag);
        input.value = '';
    });

    //remove the associate trip type
    tagContainer.addEventListener('click', function (e) {
        if (e.target.classList.contains('remove-tag')) {
            const tag = e.target.closest('.tag-name');
            if (tag) tag.remove();
        }
    });
});