async function fetchMemoryUsage() {
    try {
        const response = await fetch('http://127.0.0.1:5000/ram');
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
        const data = await response.json();
        return {
            totalMemory: data.total_ram,
            freeMemory: data.available_ram,
            usedMemory: data.used_ram,
            ramPercent: data.ram_usage_percent
        };
    } catch (error) {
        console.error('Error fetching memory usage:', error);
    }
}

function updateChart() {
    const labels = Array.from({ length: 10 }, (_, i) => (i + 1).toString()); // Ay yerine sayýlar
    const memoryData = Array(labels.length).fill(0);

    const chart = document.getElementById('chart-ram').getContext('2d');
    const gradient = chart.createLinearGradient(0, 0, 0, 450);
    gradient.addColorStop(0, 'rgba(0, 199, 214, 0.32)');
    gradient.addColorStop(0.3, 'rgba(0, 199, 214, 0.1)');
    gradient.addColorStop(1, 'rgba(0, 199, 214, 0)');

    const data = {
        labels: labels,
        datasets: [{
            label: 'RAM Usage (MB)',
            backgroundColor: gradient,
            pointBackgroundColor: '#00c7d6',
            borderWidth: 1,
            borderColor: '#0e1a2f',
            data: memoryData
        }]
    };

    const options = {
        responsive: true,
        maintainAspectRatio: true,
        animation: {
            easing: 'easeInOutQuad',
            duration: 520
        },
        scales: {
            yAxes: [{
                ticks: {
                    fontColor: '#ffffff',
                },
                gridLines: {
                    color: 'rgba(200, 200, 200, 0.4)',
                    lineWidth: 1
                }
            }],
            xAxes: [{
                ticks: {
                    fontColor: '#5e6a81'
                }
            }]
        },
        elements: {
            line: {
                tension: 0.4
            }
        },
        legend: {
            display: false
        },
        point: {
            backgroundColor: '#00c7d6'
        },
        tooltips: {
            titleFontFamily: 'Poppins',
            backgroundColor: 'rgba(0,0,0,0.4)',
            titleFontColor: 'white',
            caretSize: 5,
            cornerRadius: 2,
            xPadding: 10,
            yPadding: 10
        }
    };

    let chartInstance = new Chart(chart, {
        type: 'line',
        data: data,
        options: options
    });

    // Grafik güncellemeleri için belirli aralýklarla fonksiyon çaðýr
    setInterval(async () => {
        const memoryUsage = await fetchMemoryUsage();
        if (memoryUsage) {
            memoryData.shift(); // Ýlk öðeyi kaldýr
            memoryData.push(parseFloat(memoryUsage.freeMemory)); // Yeni RAM deðerini ekle
            chartInstance.data.datasets[0].data = memoryData;
            chartInstance.update();
        }
    }, 1000); // 1 saniyede bir
}

function updateRAMUsage() {
    var output = document.getElementById("output-ram");
    var pie = document.getElementById("stroke-active-ram");
    var ramTotal = document.getElementById("ram-total");
    var ramUsege = document.getElementById("ram-used");

    setInterval(async () => {
        const memoryUsage = await fetchMemoryUsage();
        if (memoryUsage) {
            ramTotal.innerHTML = "TOPLAM RAM MiKTARI: " + parseFloat(memoryUsage.totalMemory).toFixed(2) + "MB";
            ramUsege.innerHTML = "KULLANILAN RAM MiKTARI: " + parseFloat(memoryUsage.usedMemory).toFixed(2) + "MB";
            output.innerHTML = memoryUsage.ramPercent + "%";
            pie.style.strokeDasharray = memoryUsage.ramPercent + " 100";
        }
    }, 1000);

}

updateRAMUsage();

updateChart();