from selenium import webdriver
from selenium.webdriver.common.by import By
from selenium.webdriver.support.ui import WebDriverWait
from selenium.webdriver.support import expected_conditions as EC
import time

def categoryAdd(driver, name, description, note):
    # 'Kategori' linkine tıklamak için XPath kullanarak buluyoruz
    link_element = WebDriverWait(driver, 10).until(
        EC.presence_of_element_located((By.XPATH, '//a[@href="/Admin/Category"]'))
    )
    link_element.click()
    time.sleep(1)

    # 'Ekle' düğmesine tıklamak için XPath kullanarak buluyoruz
    add_button = WebDriverWait(driver, 10).until(
        EC.presence_of_element_located((By.XPATH, '//button[@id="btnAdd"]'))
    )
    add_button.click()
    time.sleep(1)


    # Kategori Adı, Açıklama ve Not Alanlarını Doldurma
    category_name_input = WebDriverWait(driver, 10).until(
        EC.visibility_of_element_located((By.XPATH, '//*[@id="Name"]'))
    )
    category_name_input.send_keys(name)

    category_description_input = WebDriverWait(driver, 10).until(
        EC.visibility_of_element_located((By.XPATH, '//*[@id="Description"]'))
    )
    category_description_input.send_keys(description)

    category_note_input = WebDriverWait(driver, 10).until(
        EC.visibility_of_element_located((By.XPATH, '//*[@id="Note"]'))
    )
    category_note_input.send_keys(note)

    # 'Aktif Mi?' onay kutusunu işaretlemek için XPath kullanarak buluyoruz
    is_active_checkbox = WebDriverWait(driver, 10).until(
        EC.visibility_of_element_located((By.XPATH, '//*[@id="IsActive"]'))
    )
    is_active_checkbox.click()
    time.sleep(1)

    # Formu göndermek için XPath kullanarak buluyoruz
    submit_button = WebDriverWait(driver, 10).until(
        EC.element_to_be_clickable((By.XPATH, '//*[@id="btnSave"]'))
    )
    submit_button.click()