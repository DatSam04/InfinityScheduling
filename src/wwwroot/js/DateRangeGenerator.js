///This eventlistener handle the picking date range (start-end) using Litepicker
document.addEventListener("DOMContentLoaded", function () {
	const input = document.getElementById("DateRangeInput");
	const message = document.getElementById("dateRangeMessage");

	new Litepicker({
		element: input,
		singleMode: false,
		numberOfMonths: 1,
		numberOfColumns: 1,
		selectForward: true,
		minDate: new Date().toISOString().split('T')[0],
		autoApply: true,
		setup: (picker) => {
			picker.on('selected', (start, end) => {
				// Set formatted value so it can be posted back to server
				input.value = `${start.format('YYYY-MM-DD')} - ${end.format('YYYY-MM-DD')}`;
				const diff = (end.dateInstance - start.dateInstance) / (1000 * 60 * 60 * 24);
				if (diff > 7) {
					message.textContent = "These Services are not available right at this moment";
				} else {
					message.textContent = "Coming Soon";
				}
			});
		}
	});

	//Prevent user bypass validation by changing HTML
	const form = document.querySelector("form");
	form.addEventListener("submit", function (e) {
		if (!input.value.trim()) {
			e.preventDefault();
			message.textContent = "Please select a date range.";
		}
	});
});
