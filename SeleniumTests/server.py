from flask import Flask, jsonify
from flask_cors import CORS
import subprocess # alt parçacık çalıştırmak
import psutil # sistem bilgilerine erişmek.
app = Flask(__name__)
CORS(app)  # Tüm isteklere izin ver


@app.route('/cpu', methods=['GET'])
def get_cpu_usage():
    cpu_percent = psutil.cpu_percent(interval=1)
    cpu_percent_per_cpu = psutil.cpu_percent(interval=1, percpu=True)
    cpu_freq = psutil.cpu_freq()
    cpu_count_logical = psutil.cpu_count(logical=True)
    cpu_count_physical = psutil.cpu_count(logical=False)
    response = {
        'total_cpu_usage': round(cpu_percent, 2),
        'per_cpu_usage': {f'cpu_{i}': round(percent, 2) for i, percent in enumerate(cpu_percent_per_cpu)},
        'cpu_frequency': {
            'current': round(cpu_freq.current, 2),
            'max': round(cpu_freq.max, 2),
            'min': round(cpu_freq.min, 2)
        },
        'cpu_cores': {
            'logical': cpu_count_logical,
            'physical': cpu_count_physical
        }
    }
    return jsonify(response)


@app.route('/ram', methods=['GET'])
def get_ram_usage():
    memory_info = psutil.virtual_memory()
    response = {
        'total_ram':round(memory_info.total / (1024 ** 2), 2),
        'used_ram': round(memory_info.used / (1024 ** 2),2),
        'available_ram': round(memory_info.available / (1024 ** 2),2),
        'ram_usage_percent': memory_info.percent
    }
    return jsonify(response)


@app.route('/run-guvenlikTest', methods=['POST'])
def run_guvenlik():
    file_path = "brutforceAttackt.py"
    try:
        result = subprocess.run(['python', file_path], capture_output=True, text=True)
        return result.stdout
    except Exception as e:
        return str(e)
    

@app.route('/run-kullanilabilirlikTest', methods=['POST'])
def run_kullanilabilirlik():
    file_path = "YorumYapma.py"
    try:
        result = subprocess.run(['python', file_path], capture_output=True, text=True)
        return result.stdout
    except Exception as e:
        return str(e)
    

@app.route('/run-GUITest', methods=['POST'])
def run_GUI():
    file_path = "GUITest.py" 
    try:
        result = subprocess.run(['python', file_path], capture_output=True, text=True)
        return result.stdout
    except Exception as e:
        return str(e)
    
@app.route('/run-ButunlukTest', methods=['POST'])
def run_ButunlukTest():
    file_path = "OtomatikKategoriEkleme.py" 
    try:
        result = subprocess.run(['python', file_path], capture_output=True, text=True)
        return result.stdout
    except Exception as e:
        return str(e)

#flask çalıştırmak için
if __name__ == '__main__':
    app.run(debug=True)
