from selenium import webdriver
from selenium.webdriver.common.by import By
from selenium.webdriver.support.ui import WebDriverWait
from selenium.webdriver.support import expected_conditions as EC
import time
from datetime import datetime

def makaleAdd(driver, title, content, imagePath):
    # 'Kategori' linkine tıklamak için XPath kullanarak buluyoruz
    link_element = WebDriverWait(driver, 10).until(
        EC.presence_of_element_located((By.XPATH, '//a[@href="/Admin/Article"]'))
    )
    link_element.click()
    time.sleep(1)

    # 'Ekle' düğmesine tıklamak için XPath kullanarak buluyoruz
    add_button = WebDriverWait(driver, 10).until(
        EC.presence_of_element_located((By.XPATH, '//*[@id="layoutSidenav_content"]/main/div/div/div[1]/a'))
    )
    add_button.click()
    time.sleep(1)


    # Kategori Adı, Açıklama ve Not Alanlarını Doldurma
    article_title_input = WebDriverWait(driver, 10).until(
        EC.visibility_of_element_located((By.XPATH, '//*[@id="articleTitle"]'))
    )
    article_title_input.send_keys(title)

    # Thumbnail file upload
    thumbnail_upload = driver.find_element(By.ID, "thumbnailUpload")
    thumbnail_upload.send_keys(imagePath)  # Yüklemek istediğiniz dosyanın yolunu girin

    is_active_checkbox = driver.find_element(By.ID, "isActive")
    if not is_active_checkbox.is_selected():
        is_active_checkbox.click()

    seo_author = driver.find_element(By.ID, "seoWriter")
    seo_author.clear()
    seo_author.send_keys("Nisasu Bozkurt")  # Buraya istediğiniz yazar adını girin

    seo_tags = driver.find_element(By.ID, "seoTags")
    seo_tags.clear()
    seo_tags.send_keys("tag1, tag2, tag3")  # Buraya istediğiniz etiketleri girin

    seo_description = driver.find_element(By.ID, "seoDescription")
    seo_description.clear()
    seo_description.send_keys("This is a sample SEO description.")  # Buraya istediğiniz açıklamayı girin

    submit_button = driver.find_element(By.CSS_SELECTOR, "button[type='submit']")
    submit_button.click()