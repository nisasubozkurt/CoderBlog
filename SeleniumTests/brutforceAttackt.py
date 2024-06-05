from selenium import webdriver
from selenium.webdriver.common.by import By
from selenium.webdriver.support.ui import WebDriverWait
from selenium.webdriver.support import expected_conditions as EC
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


keys = ["selam","ben","güvenlik","açığı","arıyorum","admin123","123admin","programer","kalite","kullanıcı1234","456kullanıcı"]

# 'Email' ve 'Password' inputlarına veri girişi yapmak için XPath kullanarak buluyoruz
email_input = WebDriverWait(driver, 10).until(
    EC.presence_of_element_located((By.XPATH, '//input[@id="Email"]'))
)
email_input.send_keys("adminuser@gmail.com")

for password in keys:
    password_input = WebDriverWait(driver, 10).until(
        EC.presence_of_element_located((By.XPATH, '//input[@id="Password"]'))
    )
    password_input.send_keys(password)

    # 'Giriş Yap' butonuna tıklamak için XPath kullanarak buluyoruz ve tıklıyoruz
    login_button = WebDriverWait(driver, 10).until(
        EC.presence_of_element_located((By.XPATH, '//button[@type="submit"]'))
    )
    login_button.click()    

time.sleep(3)
driver.quit()
