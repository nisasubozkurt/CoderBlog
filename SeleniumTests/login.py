from selenium import webdriver
from selenium.webdriver.common.by import By
from selenium.webdriver.support.ui import WebDriverWait
from selenium.webdriver.support import expected_conditions as EC


def login(driver, email, password):
    
    # 'Email' ve 'Password' inputlarına veri girişi yapmak için XPath kullanarak buluyoruz
    email_input = WebDriverWait(driver, 10).until(
        EC.presence_of_element_located((By.XPATH, '//input[@id="Email"]'))
    )
    email_input.send_keys(email)

    password_input = WebDriverWait(driver, 10).until(
        EC.presence_of_element_located((By.XPATH, '//input[@id="Password"]'))
    )
    password_input.send_keys(password)

    # 'Giriş Yap' butonuna tıklamak için XPath kullanarak buluyoruz ve tıklıyoruz
    login_button = WebDriverWait(driver, 10).until(
        EC.presence_of_element_located((By.XPATH, '//button[@type="submit"]'))
    )
    login_button.click()    
