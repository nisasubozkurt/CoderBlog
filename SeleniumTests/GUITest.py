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
time.sleep(2    )
# Bağlantıyı bulmak ve tıklamak için XPath kullanma
# İlk elementi bulup tıklamak için XPath kullanma
link_element = WebDriverWait(driver, 30).until(
    EC.element_to_be_clickable((By.XPATH, '//a[@class="btn btn-primary" and contains(@href, "/Article/Detail?articleId=5")]'))
)
link_element.click()
time.sleep(2)

# Hakkında sayfasının elementi bulup tıklamak için XPath kullanma
about_page = WebDriverWait(driver, 40).until(
    EC.element_to_be_clickable((By.XPATH, '//a[@class="nav-link" and contains(@href, "/Home/About")]'))
)
about_page.click()
time.sleep(2)

# İletişim sayfasının elementi bulup tıklamak için XPath kullanma
contact_page = WebDriverWait(driver, 30).until(
    EC.element_to_be_clickable((By.XPATH, '//a[@class="nav-link" and contains(@href, "/Home/Contact")]'))
)
contact_page.click()
time.sleep(2)

# Tarayıcı penceresini kapatmak için:
driver.quit()
