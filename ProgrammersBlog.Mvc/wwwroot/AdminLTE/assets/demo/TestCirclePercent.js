const tests = [
    {
        testName: "birim",
        testCount: 25,
        testValue: 25
    },
    {
        testName: "entegrasyon",
        testCount: 21,
        testValue: 21
    },
    {
        testName: "veritabani",
        testCount: 23,
        testValue: 23
    },
    {
        testName: "fonksiyonellik",
        testCount: 47,
        testValue: 47
    }
];

function updateRAMUsage() {
    tests.forEach(obj => {
        const percentage = (obj.testValue / obj.testCount) * 100;
        const outputElement = document.getElementById(`output-${obj.testName}`);
        const pieElement = document.getElementById(`stroke-active-${obj.testName}`);
        const testCount = document.getElementById(`count-${obj.testName}`);

        if (outputElement) {
            outputElement.innerHTML = `${percentage.toFixed(1)}%`;
        }

        if (pieElement) {
            pieElement.style.strokeDasharray = `${percentage} 100`;
        }

        if (testCount) {
            testCount.innerHTML = `${obj.testCount} Test`
        }
    });
}

updateRAMUsage();