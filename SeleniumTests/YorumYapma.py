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

link_element = WebDriverWait(driver, 30).until(
EC.element_to_be_clickable((By.XPATH, '//a[@class="btn btn-primary" and contains(@href, "/Article/Detail?articleId=5")]'))
)
link_element.click()
time.sleep(2)

ad_input = WebDriverWait(driver, 30).until(
EC.element_to_be_clickable((By.XPATH, '//*[@id="CreatedByName"]'))
)
ad_input.send_keys("NİSASU BOZKURT")
time.sleep(1)

yorum_input = WebDriverWait(driver, 30).until(
EC.element_to_be_clickable((By.XPATH, '//*[@id="Text"]'))
)
yorum_input.send_keys("Kalite Projesinden 100 Aldım!!!")
time.sleep(2)

send_button = WebDriverWait(driver, 30).until(
EC.element_to_be_clickable((By.XPATH, '//*[@id="btnSave"]'))
)
send_button.click()
time.sleep(2)
driver.quit()