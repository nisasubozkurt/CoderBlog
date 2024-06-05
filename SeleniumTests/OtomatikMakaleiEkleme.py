from selenium import webdriver
from selenium.webdriver.common.by import By
from selenium.webdriver.support.ui import WebDriverWait
from selenium.webdriver.support import expected_conditions as EC
from login import login
from MakaleEkleme import makaleAdd
import time
options = webdriver.ChromeOptions()

# Tarayıcı penceresinin otomatik olarak kapanmasını engelle
options.add_argument("--disable-gpu")
options.add_argument("--disable-dev-shm-usage")
options.add_experimental_option("detach", True)

driver = webdriver.Chrome(options=options)

driver.get("https://localhost:44328/")

# 'Yönetim Paneli' linkine tıklamak için XPath kullanarak buluyoruz
link_element = WebDriverWait(driver, 10).until(
    EC.presence_of_element_located((By.XPATH, '//a[text()="Yönetim Paneli"]'))
)
link_element.click()

# 'Email' ve 'Password' inputlarına veri girişi yapmak için XPath kullanarak buluyoruz
login(driver, "adminuser@gmail.com", "adminuser")

makaleAdd(driver, "Test makale", "Kategori content", "C:\\Users\\LaraA\\OneDrive\\Masaüstü\\bangkok2.jpeg")


time.sleep(3)
# Tarayıcı penceresini kapatmak için:
driver.quit()
