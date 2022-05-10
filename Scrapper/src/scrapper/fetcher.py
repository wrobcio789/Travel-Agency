from datetime import datetime

from selenium import webdriver
from selenium.webdriver.common.by import By

from scrapper.base_selenium import BaseSelenium


def scrap_tours(stop_at_page=-1):
    url = "https://www.tui.pl/last-minute"
    chrome_path = r"webdriver/chromedriver.exe"
    show_more_xpath = "//button[contains(@data-testid,'results-list-load-more-button')]"
    accept_cookies_xpath = "//button[contains(@class,'cookies-bar__button--accept')]"
    loading_xpath = "//div[contains(@class,'results-container__loading-wrapper')]"

    customdriver = BaseSelenium(webdriver.Chrome(chrome_path))

    customdriver.open(url)

    customdriver.click_elements_by_send_key((By.XPATH, accept_cookies_xpath), "\n")

    button = (By.XPATH, show_more_xpath)
    loading_loc = (By.XPATH, loading_xpath)

    ctr = 0
    while stop_at_page == -1 or ctr < stop_at_page:
        customdriver.wait_for_element_invisible(loading_loc)
        try:
            customdriver.click_elements_by_send_key(button, "\n")
            ctr += 1
            print(f"{datetime.now()} See more {ctr}")
        except Exception as e:
            print(e)
            with open('../data/page.html', 'w', encoding="utf- 8") as f:
                f.write(customdriver.driver.page_source)


if __name__ == '__main__':
    scrap_tours()
