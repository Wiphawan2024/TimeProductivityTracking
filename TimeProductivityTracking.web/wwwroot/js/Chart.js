document.addEventListener("DOMContentLoaded", function () {
    var ctx = document.getElementById('productivityChart').getContext('2d');

    var chart = new Chart(ctx, {
        type: 'line',
        data: {
            labels: JSON.parse(document.getElementById("chartMonths").value), // Dynamic Months
            datasets: [
                {
                    label: 'Planned Days',
                    data: JSON.parse(document.getElementById("chartPlanned").value), // Dynamic Planned Days
                    borderColor: 'blue',
                    backgroundColor: 'rgba(0, 0, 255, 0.2)',
                    fill: true
                },
                {
                    label: 'Achieved Days',
                    data: JSON.parse(document.getElementById("chartAchieved").value), // Dynamic Achieved Days
                    borderColor: 'green',
                    backgroundColor: 'rgba(0, 255, 0, 0.2)',
                    fill: true
                }
            ]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            scales: {
                x: { title: { display: true, text: 'Month' } },
                y: { title: { display: true, text: 'Days' }, beginAtZero: true }
            }
        }
    });
});
