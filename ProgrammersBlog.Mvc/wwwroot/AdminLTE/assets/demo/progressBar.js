function updateProgressBars(totalApplications, totalShortlisted, totalOnHold, totalRejected) {
    const total = totalApplications + totalShortlisted + totalOnHold + totalRejected;

    const applicationsPercentage = (totalApplications / total) * 100;
    const shortlistedPercentage = (totalShortlisted / total) * 100;
    const onHoldPercentage = (totalOnHold / total) * 100;
    const rejectedPercentage = (totalRejected / total) * 100;

    const applicationsBar = document.querySelector('.bar-progress.birim');
    const shortlistedBar = document.querySelector('.bar-progress.fonksiyonellik');
    const onHoldBar = document.querySelector('.bar-progress.veritabani');
    const rejectedBar = document.querySelector('.bar-progress.entegrasyon');

    applicationsBar.style.width = applicationsPercentage + '%';
    shortlistedBar.style.width = shortlistedPercentage + '%';
    onHoldBar.style.width = onHoldPercentage + '%';
    rejectedBar.style.width = rejectedPercentage + '%';

    const applicationsAmount = document.querySelector('.progress-amount.birim');
    const shortlistedAmount = document.querySelector('.progress-amount.fonksiyonellik');
    const onHoldAmount = document.querySelector('.progress-amount.veritabani');
    const rejectedAmount = document.querySelector('.progress-amount.entegrasyon');

    applicationsAmount.textContent = applicationsPercentage.toFixed(2) + '%';
    shortlistedAmount.textContent = shortlistedPercentage.toFixed(2) + '%';
    onHoldAmount.textContent = onHoldPercentage.toFixed(2) + '%';
    rejectedAmount.textContent = rejectedPercentage.toFixed(2) + '%';

    const totalElement = document.querySelector('.total');
    totalElement.textContent = 'Toplam Test Sayısı: ' + total;
}


// Örnek kullanım:
updateProgressBars(25, 21, 47, 23);
