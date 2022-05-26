import time

from selenium.webdriver.common.by import By
from selenium.webdriver.common.keys import Keys


def login(driver, username, password):
    username_xpath = "//input[contains(@name,'login')]"
    password_xpath = "//input[contains(@name,'password')]"
    login_button_xpath = "//button[text()='Login']"

    driver.click_elements_by_send_key((By.XPATH, username_xpath), username)
    driver.click_elements_by_send_key((By.XPATH, password_xpath), password)
    time.sleep(1)
    driver.click_elements_by_send_key((By.XPATH, login_button_xpath), "\n")


def choose_offer(driver):
    time.sleep(1)
    first_offer_xpath = "//div[contains(@class,'trip-tile flex-column')]"
    driver.click_with_element((By.XPATH, first_offer_xpath))


def choose_transport(driver):
    time.sleep(1)
    transport_to_offer_xpath = "//b[contains(text(),'Transport to')]/following::input[@type='search'][1]"
    driver.click_elements_by_send_key((By.XPATH, transport_to_offer_xpath), Keys.DOWN + Keys.DOWN + "\n")


def choose_hotel(driver):
    time.sleep(1)
    hotel_search_xpath = "//b[text()='Hotel']/following::input[@type='search']"
    driver.click_elements_by_send_key((By.XPATH, hotel_search_xpath), Keys.DOWN)
    driver.click_elements_by_send_key((By.XPATH, hotel_search_xpath), "\n")


def check_offer_availability(driver):
    offer_available_button_xpath = "//button[@class='availability-button']"
    offer_available_text_xpath = "//div[@class='ok-label']"
    offer_unavailable_text_xpath = "//div[@class='error-label']"
    driver.click_elements_by_send_key((By.XPATH, offer_available_button_xpath), "\n")
    time.sleep(1)
    element = driver.locate_element((By.XPATH, offer_available_text_xpath))
    success = element is not None
    if element is None:
        time.sleep(1)
        element = driver.locate_element((By.XPATH, offer_unavailable_text_xpath))
    if element is None:
        time.sleep(1)
        element = driver.locate_element((By.XPATH, offer_available_text_xpath))
        success = element is not None
    if element is None:
        print("Error: could not find offer available result (both available and unavailable)")
    return success


def buy_offer(driver):
    time.sleep(1)
    buy_button_xpath = "//button[text()='Buy trip in the configuration']"
    driver.click_elements_by_send_key((By.XPATH, buy_button_xpath), "\n")


def pay(driver, correct_payment=True):
    time.sleep(5)
    cc_number_input_xpath = "//input[@placeholder='Number']"
    cc_cv_input_xpath = "//input[@placeholder='CV']"
    cc_expiration_date_input_xpath = "//input[@placeholder='Exp date']"
    pay_button_xpath = "//button[text()='Pay for order']"
    driver.click_elements_by_send_key((By.XPATH, cc_number_input_xpath), "1234123412341234")
    cv = "123" if correct_payment else "420"
    driver.click_elements_by_send_key((By.XPATH, cc_cv_input_xpath), cv)
    driver.click_elements_by_send_key((By.XPATH, cc_expiration_date_input_xpath), "10/2022")
    driver.click_elements_by_send_key((By.XPATH, pay_button_xpath), "\n")


def is_order_bought(driver):
    time.sleep(5)
    is_bought_xpath = "//span[text()='Offer bought successfuly']"
    element = driver.locate_element((By.XPATH, is_bought_xpath))
    return element is not None
