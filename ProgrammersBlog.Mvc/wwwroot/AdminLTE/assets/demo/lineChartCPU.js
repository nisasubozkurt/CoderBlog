async function fetchCpuUsage() {
    try {
        const response = await fetch('http://127.0.0.1:5000/cpu');
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
        const data = await response.json();
        return {
            totalCpuUsage: data.total_cpu_usage,
            perCpuUsage: data.per_cpu_usage,
            cpuFrequency: data.cpu_frequency,
            cpuCores: data.cpu_cores
        };
    } catch (error) {
        console.error('Error fetching CPU usage:', error);
    }
}

function updateChart() {
    const labels = ['1s', '2s', '3s', '4s', '5s', '6s', '7s', '8s', '9s', '10s']; // Saniyelik etiketler
    const cpuData = Array(labels.length).fill(0);

    const chart = document.getElementById('chart-cpu').getContext('2d');
    const gradient = chart.createLinearGradient(0, 0, 0, 450);
    gradient.addColorStop(0, 'rgba(255, 105, 97, 0.32)'); // Kýrmýzý ve tonlarý
    gradient.addColorStop(0.3, 'rgba(255, 105, 97, 0.1)');
    gradient.addColorStop(1, 'rgba(255, 105, 97, 0)');


    const data = {
        labels: labels,
        datasets: [{
            label: 'CPU Usage (%)',
            backgroundColor: gradient,
            pointBackgroundColor: '#8B0000',
            borderWidth: 1,
            borderColor: '#8B0000',
            data: cpuData
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
                    beginAtZero: true,
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
        const cpuUsage = await fetchCpuUsage();
        if (cpuUsage) {
            // Yeni CPU kullaným yüzdesini ekle ve ilk öðeyi kaldýr
            cpuData.shift();
            cpuData.push(parseFloat(cpuUsage.perCpuUsage.cpu_0));

            chartInstance.data.datasets[0].data = cpuData;
            chartInstance.update();
        }
    }, 1000); // 1 saniyede bir
}

function updateCPUUsage() {
    var output = document.getElementById("output-cpu");
    var pie = document.getElementById("stroke-active-cpu");

    var maxCPU = document.getElementById("cpu-max");
    var currentCPU = document.getElementById("cpu-current");
    var minCPU = document.getElementById("cpu-min");

    setInterval(async () => {
        const cpuUsage = await fetchCpuUsage();
        if (cpuUsage) {
            output.innerHTML = cpuUsage.perCpuUsage.cpu_0 + "%";

            maxCPU.innerHTML = cpuUsage.cpuFrequency.max + "MHZ  MANTIKSAL CORE SAYISI: " + cpuUsage.cpuCores.logical;
            currentCPU.innerHTML = cpuUsage.cpuFrequency.current + "MHZ  FiZiKSEL CORE SAYISI: " + cpuUsage.cpuCores.physical;
            minCPU.innerHTML = cpuUsage.cpuFrequency.min+"MHZ";
            pie.style.strokeDasharray = cpuUsage.totalCpuUsage + " 100";
        }
    }, 1000); // 1 saniyede bir
}


function updateCPUUsagePerCore() {
    var cpuInfoList = document.getElementById("cpu-usage-list");

    // Ýlk olarak baþlýðý ekleyelim
    var header = document.createElement("div");
    header.className = "chart-container-header";

    var headerTitle = document.createElement("h2");
    headerTitle.textContent = "CPU CORKULLANIMLARI";

    header.appendChild(headerTitle);
    cpuInfoList.appendChild(header);
    setInterval(async () => {
        const cpuUsage = await fetchCpuUsage();
        if (cpuUsage) {
            cpuInfoList.innerHTML = "";  // Mevcut içeriði temizle
            cpuInfoList.appendChild(header);

            for (let cpu in cpuUsage.perCpuUsage) {
                const usage = cpuUsage.perCpuUsage[cpu];

                const cpuInfo = document.createElement("div");
                cpuInfo.className = "cpu-info";

                const cpuName = document.createElement("span");
                cpuName.className = "cpu-name";
                cpuName.textContent = cpu+": ";

                const cpuPercent = document.createElement("span");
                cpuPercent.className = "cpu-percent";
                cpuPercent.textContent = `${usage}%`;

                cpuInfo.appendChild(cpuName);
                cpuInfo.appendChild(cpuPercent);

                cpuInfoList.appendChild(cpuInfo);
            }
        }
    }, 1000); // 1 saniyede bir
}

updateCPUUsage();       
updateChart();
updateCPUUsagePerCore();
