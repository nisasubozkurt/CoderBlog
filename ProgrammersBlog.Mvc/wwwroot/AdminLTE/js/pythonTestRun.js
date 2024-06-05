document.getElementById('guvenlikTest').addEventListener('click', function () {
    fetch('http://127.0.0.1:5000/run-guvenlikTest', {
        method: 'POST',
        body: JSON.stringify({}), // Boş bir nesne gönderilebilir
        headers: {
            'Content-Type': 'application/json'
        }
    })
        .then(response => response.text())
        .then(data => console.log(data))
        .catch(error => console.error('Error:', error));
});
document.getElementById('kullanilabilirlikTest').addEventListener('click', function () {
    fetch('http://127.0.0.1:5000/run-kullanilabilirlikTest', {
        method: 'POST',
        body: JSON.stringify({}), // Boş bir nesne gönderilebilir
        headers: {
            'Content-Type': 'application/json'
        }
    })
        .then(response => response.text())
        .then(data => console.log(data))
        .catch(error => console.error('Error:', error));
});
document.getElementById('GUITest').addEventListener('click', function () {
    fetch('http://127.0.0.1:5000/run-GUITest', {
        method: 'POST',
        body: JSON.stringify({}), // Boş bir nesne gönderilebilir
        headers: {
            'Content-Type': 'application/json'
        }
    })
        .then(response => response.text())
        .then(data => console.log(data))
        .catch(error => console.error('Error:', error));
});

document.getElementById('butunlesiktest').addEventListener('click', function () {
    fetch('http://127.0.0.1:5000/run-ButunlukTest', {
        method: 'POST',
        body: JSON.stringify({}), // Boş bir nesne gönderilebilir
        headers: {
            'Content-Type': 'application/json'
        }
    })
        .then(response => response.text())
        .then(data => console.log(data))
        .catch(error => console.error('Error:', error));
        console.log(".")
});